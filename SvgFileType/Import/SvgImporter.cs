// Copyright 2021 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by a LGPL license that can be
// found in the COPYING file.

using PaintDotNet;
using Svg;
using Svg.Transforms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SvgFileTypePlugin.Import
{
    internal sealed class SvgImporter
    {
        // Don't change this text! It's used by a PSD import plugin to keep Photoshop's folder structure.
        // More info: https://forums.getpaint.net/topic/113742-photoshop-psd-file-plugin-with-layers-support/
        public const string LayerGroupBegin = "Layer Group: {0}";
        public const string LayerGroupEnd = "End Layer Group: {0}";
        private const string GroupAttribute = "import_group_name";
        private const string VisibilityAttribute = "import_visibility";
        private static readonly IReadOnlyCollection<string> AllowedTitles = new string[] { "label", "title", "inkscape:label" };
        private const int LayerCountWarningThreshold = 50;

        private SvgDocument svg;
        private CancellationTokenSource cts;
        private int rasterWidth;
        private int rasterHeight;
        private Document pdnDocument;
        private SvgImportDialog dialog;
        private Exception reason;
        private Bitmap scratchBitmap;
        private Graphics scratchGraphics;

        public Document Import(Stream input)
        {
            svg = SvgDocumentOpener.Open(input);
            using (cts = new CancellationTokenSource())
            using (dialog = new SvgImportDialog(svg))
            {
                dialog.FormClosing += Dialog_FormClosing;
                dialog.OkClick += Dialog_OkClick;
                DialogResult dialogResult = dialog.ShowDialog();
                if (dialogResult != DialogResult.OK)
                {
                    pdnDocument?.Dispose();
                    pdnDocument = null;
                    svg = null;
                    throw reason ?? new OperationCanceledException("Canceled by user");
                }
                return pdnDocument;
            }
        }

        private DialogResult WarnAboutMemory(int layerCount)
        {
            var dialogResult = Utils.ThreadSafeShowDialog(dialog, form =>
              {
                  string[] lines =
                  {
                    $"There are {layerCount} layers about to be imported from the SVG file.",
                    String.Empty,
                    "Importing many layers requires a lot of memory, especially if you're using a large canvas. " +
                    "Insufficient memory may cause operating system instability. " +
                    "Please make sure you've enough memory available before continue.",
                    String.Empty,
                    "Do you want to continue?"
                  };
                  return MessageBoxEx.Show(form, String.Join(Environment.NewLine, lines), "Warning",
                      MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                      MessageBoxDefaultButton.Button2);
              });
            return dialogResult;
        }

        #region DoImport

        private void DoImport(SvgImportConfig config, CancellationToken cancellationToken = default)
        {
            // set the requested size & resolution for SvgDocument
            rasterWidth = config.Width;
            rasterHeight = config.Height;
            SizeF size = svg.GetDimensions();
            if (svg.ViewBox.IsEmpty())
            {
                svg.ViewBox = new SvgViewBox(0, 0, size.Width, size.Height);
            }
            svg.Ppi = config.Ppi;
            svg.RasterizeDimensions(ref size, rasterWidth, rasterHeight);
            svg.Width = size.Width;
            svg.Height = size.Height;
            svg.AspectRatio = config.KeepAspectRatio
                ? new SvgAspectRatio(SvgPreserveAspectRatio.xMinYMin)
                : new SvgAspectRatio(SvgPreserveAspectRatio.none);

            LayersMode layersMode = config.LayersMode;
            bool importGroupBoundariesAsLayers = config.GroupBoundariesAsLayers;
            bool setOpacityForLayer = config.RespectElementOpacity;
            bool importHiddenLayers = config.HiddenElements;

            using (scratchBitmap = new Bitmap(rasterWidth, rasterHeight))
            using (scratchGraphics = Graphics.FromImage(scratchBitmap))
            {
                // Render one flat image and quit.
                if (layersMode == LayersMode.Flat)
                {
                    RenderSvgDocument();
                    pdnDocument = Document.FromImage(scratchBitmap);
                    pdnDocument.DpuX = pdnDocument.DpuY = config.Ppi;
                    pdnDocument.DpuUnit = MeasurementUnit.Inch;
                    return;
                }

                IReadOnlyCollection<SvgVisualElement> allElements = PrepareFlatElements(svg.Children)
                    .OfType<SvgVisualElement>()
                    .ToList();

                if (layersMode == LayersMode.All)
                {
                    // Dont render groups and boundaries if defined
                    allElements = allElements.Where(p => !(p is SvgGroup)).ToList();

                    // Filter out group boundaries if not set.
                    if (!importGroupBoundariesAsLayers)
                    {
                        allElements = allElements.Where(p => !(p is PaintGroupBoundaries)).ToList();
                    }

                    if (GetLayerCountToRender(allElements, importHiddenLayers) > LayerCountWarningThreshold)
                    {
                        if (WarnAboutMemory(allElements.Count) != DialogResult.Yes)
                        {
                            dialog.DialogResult = DialogResult.Cancel;
                            return;
                        }
                    }

                    // Thread safe
                    dialog.SetMaxProgress(allElements.Count);
                    dialog.ReportProgress(0);
                    pdnDocument = RenderElements(allElements, setOpacityForLayer, importHiddenLayers, dialog.ReportProgress, cancellationToken);
                }
                else if (layersMode == LayersMode.Groups)
                {
                    // Get only parent groups and single elements
                    var groupsAndElementsWithoutGroup = new HashSet<SvgVisualElement>();

                    foreach (SvgVisualElement element in allElements)
                    {
                        if (element is PaintGroupBoundaries)
                        {
                            continue;
                        }

                        if (element.ContainsAttribute(GroupAttribute))
                        {
                            // Get only root level
                            SvgGroup lastGroup = null;
                            if (element is SvgGroup group)
                            {
                                lastGroup = group;
                            }

                            SvgElement toCheck = element;
                            while (toCheck != null)
                            {
                                toCheck = toCheck.Parent;
                                if (toCheck is SvgGroup groupToCheck)
                                {
                                    // TODO: render more groups. In most cases svg has only few root groups.
                                    string title = GetLayerTitle(groupToCheck);
                                    if (!String.IsNullOrEmpty(title))
                                    {
                                        lastGroup = groupToCheck;
                                    }
                                }
                            }

                            if (!groupsAndElementsWithoutGroup.Contains(lastGroup))
                            {
                                groupsAndElementsWithoutGroup.Add(lastGroup);
                            }
                        }
                        else
                        {
                            if (!groupsAndElementsWithoutGroup.Contains(element))
                            {
                                groupsAndElementsWithoutGroup.Add(element);
                            }
                        }
                    }

                    if (groupsAndElementsWithoutGroup.Count > LayerCountWarningThreshold &&
                        WarnAboutMemory(groupsAndElementsWithoutGroup.Count) != DialogResult.Yes)
                    {
                        dialog.DialogResult = DialogResult.Cancel;
                        return;
                    }

                    // Thread safe
                    dialog.SetMaxProgress(groupsAndElementsWithoutGroup.Count);
                    dialog.ReportProgress(0);

                    pdnDocument = RenderElements(groupsAndElementsWithoutGroup, setOpacityForLayer, importHiddenLayers,
                        dialog.ReportProgress, cancellationToken);
                }

                // Fallback. Nothing is added. Render one default layer.
                if (pdnDocument == null || pdnDocument.Layers.Count == 0)
                {
                    pdnDocument?.Dispose();
                    RenderSvgDocument();
                    pdnDocument = Document.FromImage(scratchBitmap);
                }

                pdnDocument.DpuX = pdnDocument.DpuY = config.Ppi;
                pdnDocument.DpuUnit = MeasurementUnit.Inch;
            }
        }

        #endregion

        #region Render Elements

        private static int GetLayerCountToRender(IReadOnlyCollection<SvgVisualElement> elements, bool importHiddenLayers)
        {
            return importHiddenLayers ? elements.Count : elements.Count(IsVisibleOriginally);
        }

        private Document RenderElements(IReadOnlyCollection<SvgVisualElement> elements, bool setOpacityForLayer, bool importHiddenLayers, Action<int> progress, CancellationToken cancellationToken)
        {
            // I had problems to render each element directly while parent transformation can affect child. 
            // But we can do a trick and render full document each time with only required nodes set as visible.

            int layerCount = 0;
            // Render all visual elements that are passed here.
            foreach (SvgVisualElement element in elements)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (element is PaintGroupBoundaries boundaryNode)
                {
                    // Store related group opacity and visibility.
                    if (!importHiddenLayers)
                    {
                        // Skip group boundaries for hidden layers.
                        if (!IsVisibleOriginally(boundaryNode.RelatedGroup))
                        {
                            progress?.Invoke(++layerCount);
                            continue;
                        }
                    }

                    pdnDocument = pdnDocument ?? new Document(rasterWidth, rasterHeight);
                    // Render empty group boundary and continue
                    var pdnLayer = new BitmapLayer(rasterWidth, rasterHeight)
                    {
                        Name = boundaryNode.ID,
                        Opacity = (byte)(boundaryNode.RelatedGroup.Opacity * 255f),
                        Visible = boundaryNode.RelatedGroup.Visible
                    };
                    pdnDocument.Layers.Add(pdnLayer);
                    progress?.Invoke(++layerCount);
                    continue;
                }

                // Turn off visibility of all elements
                foreach (SvgVisualElement elemntToChange in elements)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    elemntToChange.Visibility = "hidden";
                }

                var itemShouldBeIgnored = false;

                // Turn on visibility from node to parent
                var toCheck = (SvgElement)element;
                do
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    if (toCheck is SvgVisualElement visual)
                    {
                        visual.Visibility = "visible";
                        // Check, maybe parent group was initially hidden
                        if (!importHiddenLayers)
                        {
                            // Skip hidden layers.
                            if (!IsVisibleOriginally(visual))
                            {
                                itemShouldBeIgnored = true;
                                break;
                            }
                        }
                    }

                    toCheck = toCheck.Parent;
                } while (toCheck != null);

                if (itemShouldBeIgnored)
                {
                    progress?.Invoke(++layerCount);
                    continue;
                }

                RenderElement(element, setOpacityForLayer, importHiddenLayers);
                progress?.Invoke(++layerCount);
            }

            return pdnDocument;
        }

        private void RenderElement(SvgElement element, bool setOpacityForLayer, bool importHiddenLayers)
        {
            var visible = true;
            if (element is SvgVisualElement visualElement)
            {
                visible = IsVisibleOriginally(visualElement);
                if (importHiddenLayers)
                {
                    // Set visible to render image and then item can be hidden.
                    visualElement.Visibility = "visible";
                }
                else if (!visible)
                {
                    // Hidden layers are ignored.
                    return;
                }
            }
            else
            {
                visualElement = null;
            }

            float opacity = element.Opacity;

            // Store opacity as layer options.
            if (setOpacityForLayer)
            {
                // Set full opacity when enabled to render 100%. 
                // Anyway opacity will be set as paint layer options.
                element.Opacity = 1;
            }

            BitmapLayer pdnLayer;
            pdnDocument = pdnDocument ?? new Document(rasterWidth, rasterHeight);

            using (var failPoint = new MemoryFailPoint(Utils.CalcMemoryNeeded(rasterWidth, rasterHeight)))
            {
                if (element is SvgUse useElement)
                {
                    RenderSvgUseElement(useElement);
                }
                else
                {
                    RenderSvgDocument();
                }
                pdnLayer = new BitmapLayer(Surface.CopyFromBitmap(scratchBitmap), takeOwnership: true);
            }

            pdnLayer.Name = GetLayerTitle(element);
            if (setOpacityForLayer)
            {
                pdnLayer.Opacity = (byte)(opacity * 255);
            }

            if (importHiddenLayers && visualElement != null)
            {
                pdnLayer.Visible = visible;
            }

            pdnDocument.Layers.Add(pdnLayer);
        }

        private void RenderSvgUseElement(SvgUse useElement)
        {
            if (useElement == null) throw new ArgumentNullException(nameof(useElement));
            SvgUse owner = useElement;
            SvgElement refElem = null;
            List<SvgTransform> transforms = new List<SvgTransform>();
            while (owner != null)
            {
                string id = owner.ReferencedElement?.ToString().Substring(1);
                if (id == null) break;
                refElem = svg.GetElementById(id);
                owner = refElem as SvgUse;
                if (owner?.Transforms != null)
                {
                    transforms.AddRange(owner.Transforms);
                }
            }

            if (refElem == null)
            {
                return;
            }

            SvgElement refCopy = refElem.DeepCopy();
            refCopy.Visibility = "visible";
            if (useElement.Transforms != null)
            {
                transforms.AddRange(useElement.Transforms);
            }
            useElement.Visibility = "hidden";
            refCopy.Transforms = refCopy.Transforms ?? new SvgTransformCollection();
            refCopy.Transforms.AddRange(transforms);
            foreach(KeyValuePair<string, object> attr in useElement.GetAttributes())
            {
                var refAttrs = refCopy.GetAttributes();
                if (refAttrs.ContainsKey(attr.Key))
                {
                    switch (attr.Key)
                    {
                        case "x":
                        case "y":
                        case "width":
                        case "height":
                        case "href":
                        case "xlink:href":
                            refAttrs[attr.Key] = attr.Value;
                            break;
                    }
                }
                else
                {
                    refAttrs[attr.Key] = attr.Value;
                }
            }
            useElement.Parent.Children.AddAndForceUniqueID(refCopy);
            RenderSvgDocument();
            useElement.Parent.Children.Remove(refCopy);
        }

        private static bool IsVisibleOriginally(SvgElement element)
        {
            return element is SvgVisualElement visual && visual.CustomAttributes.TryGetValue(VisibilityAttribute, out string arg)
                ? bool.Parse(arg)
                : true;
        }

        private void RenderSvgDocument()
        {
            scratchGraphics.Clear(default);
            svg.Draw(scratchGraphics);
        }

        /// <summary>
        /// Get a title for a specified svg element.
        /// </summary>
        private static string GetLayerTitle(SvgElement element, int maxLength = 32)
        {
            string elementName = element.GetName();
            string layerName = null;

            if (element.ID != null)
            {
                layerName = element.ID;
            }

            if (String.IsNullOrEmpty(layerName) && element.CustomAttributes != null)
            {
                // get custom title attributes.
                foreach (string titleAttribute in AllowedTitles)
                {
                    if (element.CustomAttributes.TryGetValue(titleAttribute, out string title))
                    {
                        if (!String.IsNullOrEmpty(title))
                        {
                            layerName = title;
                            break;
                        }
                    }
                }
            }

            if (String.IsNullOrEmpty(layerName) && element.Children != null)
            {
                // Get child title tag
                SvgTitle title = element.Children.OfType<SvgTitle>().FirstOrDefault();
                if (!String.IsNullOrEmpty(title?.Content))
                {
                    layerName = title.Content;
                }
            }

            if (String.IsNullOrEmpty(layerName))
            {
                // Generate more meanfull name for a svg use node. Add reference element name in a case if it's local document.
                if (element is SvgUse useElement
                    && useElement.ReferencedElement != null
                    && !String.IsNullOrEmpty(useElement.ReferencedElement.OriginalString))
                {
                    string str = useElement.ReferencedElement.OriginalString.Trim();
                    if (str.StartsWith("#"))
                    {
                        layerName = str.Substring(1);
                    }
                }
                // Generate more meanfull name for a svg text.
                else if (element is SvgTextBase text && !String.IsNullOrEmpty(text.Text))
                {
                    layerName = text.Text;
                    if (text.Text.Length > maxLength)
                    {
                        layerName = text.Text.Substring(0, maxLength) + "...";
                    }
                }
                else if (element is SvgPath path && path.PathData.Count > 0)
                {
                    layerName = path.PathData.ToString();
                    if (layerName.Length > maxLength)
                    {
                        layerName = layerName.Substring(0, maxLength) + "...";
                    }
                }
            }

            return layerName == null ? elementName : String.Join(": ", elementName, layerName);
        }

        private static IEnumerable<SvgElement> PrepareFlatElements(SvgElementCollection collection, string groupName = null)
        {
            if (collection == null)
            {
                yield break;
            }

            // Prepare a collection of elements that about to be rendered. 
            foreach (SvgElement toRender in collection)
            {
                // Don't prepare for a separate parsing def lists.
                if (toRender is SvgDefinitionList)
                {
                    continue;
                }

                if (toRender is SvgVisualElement visual)
                {
                    // Fix problem that SVG visual element lib style "display:none" is not recognized as visible state.
                    if (visual.Visible && visual.Display?.Trim().Equals("none", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        visual.Visibility = "hidden";
                        visual.Display = null;
                    }

                    // Store visibility
                    toRender.CustomAttributes.Add(VisibilityAttribute, visual.Visible.ToString());

                    // Save current group to indicate that elements inside a group.
                    if (!String.IsNullOrEmpty(groupName) && !toRender.ContainsAttribute(GroupAttribute))
                    {
                        // Store group info
                        toRender.CustomAttributes.Add(GroupAttribute, groupName);
                    }
                }

                var group = toRender as SvgGroup;
                if (group != null)
                {
                    groupName = GetLayerTitle(group);

                    // Return fake node to indicate group end. (order is reversed)
                    yield return new PaintGroupBoundaries(group, false)
                    {
                        ID = String.Format(LayerGroupEnd, groupName)
                    };
                }

                if (PrepareFlatElements(toRender.Children, groupName) is IEnumerable<SvgElement> returned)
                {
                    foreach (SvgElement output in returned)
                    {
                        yield return output;
                    }
                }

                if (group != null)
                {
                    // Return fake node to indicate group start.
                    yield return new PaintGroupBoundaries(group, true)
                    {
                        ID = String.Format(LayerGroupBegin, groupName),
                    };
                }

                // Skip text with empty content. But keep all children nodes.
                if (toRender is SvgTextBase textNode && String.IsNullOrEmpty(textNode.Text))
                {
                    continue;
                }

                yield return toRender;
            }
        }

        #endregion

        #region Events

        private void Dialog_OkClick(object sender, EventArgs e)
        {
            SvgImportDialog dialog = (SvgImportDialog)sender;
            SvgImportConfig config = dialog.GetConfig();
            Task task = Task.Run(() => DoImport(config, cts.Token));
            task.ContinueWith(t =>
            {
                if (t.IsFaulted && !t.IsCanceled)
                {
                    reason = t.Exception;
                    if (t.Exception.Flatten().InnerExceptions.Any(exception => exception is OutOfMemoryException || exception is InsufficientMemoryException))
                    {
                        Utils.ThreadSafeShowDialog(dialog, form => MessageBoxEx.Show(form,
                              "Not enough memory to complete this operation.",
                              "Out of Memory", MessageBoxButtons.OK, MessageBoxIcon.Error));
                    }
                    else
                    {
                        Utils.ThreadSafeShowDialog(dialog, form => MessageBoxEx.Show(form,
                            t.Exception.ToString().Substring(0, 1000),
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error));
                    }
                    dialog.Close();
                    return;
                }

                if (t.IsCanceled)
                {
                    dialog.Close();
                    return;
                }

                if (t.IsCompleted)
                {
                    if (dialog.DialogResult == DialogResult.None)
                    {
                        dialog.DialogResult = DialogResult.OK;
                    }
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Dialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            cts.Cancel();
        }

        #endregion
    }
}