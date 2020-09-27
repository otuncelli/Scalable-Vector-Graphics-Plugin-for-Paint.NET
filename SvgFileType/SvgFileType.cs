using PaintDotNet;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SvgFileTypePlugin
{
    public class SvgFileType : FileType
    {
        public SvgFileType() : base(FileTypeName,
            new FileTypeOptions
            {
                LoadExtensions = SupportedExtensions,
                SupportsCancellation = true,
                SupportsLayers = true
            })
        {
        }

        private const string FileTypeName = "Scalable Vector Graphics";
        private const string WindowTitle = "SVG Import Plug-in v1.0.1";
        private static readonly string[] SupportedExtensions = {".svg", ".svgz"};

        // Don't change this text! It's used by a PSD import plugin to keep Photoshop's folder structure.
        public const string LayerGroupBegin = "Layer Group: {0}";
        public const string LayerGroupEnd = "End Layer Group: {0}";

        private const string GroupAttribute = "import_group_name";
        private const string VisibilityAttribute = "import_visibility";
        private static readonly string[] AllowedTitles = {"label", "title", "inskape:label"};

        private const int LayerCountWarningThreshold = 50;

        private SvgDocument svg;
        private CancellationTokenSource cts;
        private int width, height;
        private Document pdnDocument;
        private SvgImportDialog dialog;

        protected override Document OnLoad(Stream input)
        {
            pdnDocument = null;
            svg = SvgDocumentOpener.FromStream(input);
            GetInitialValues(out int viewportW, out int viewportH, out int ppi,
                out int viewBoxX, out int viewBoxY, out int viewBoxW, out int viewBoxH);

            using (cts = new CancellationTokenSource())
            using (dialog = new SvgImportDialog())
            {
                dialog.Title = WindowTitle;
                dialog.ViewportW = viewportW;
                dialog.ViewportH = viewportH;
                dialog.SourceDpi = ppi;
                dialog.ViewBoxX = viewBoxX;
                dialog.ViewBoxY = viewBoxY;
                dialog.ViewBoxW = viewBoxW;
                dialog.ViewBoxH = viewBoxH;
                dialog.InitSizeHint();
                dialog.FormClosing += Dialog_FormClosing;
                dialog.FormClosed += Dialog_FormClosed;
                dialog.OkClick += Dialog_OkClick;
                Form mainForm = Utils.GetMainForm();
                Func<DialogResult> callback = () => dialog.ShowDialog(mainForm);
                DialogResult dialogResult = mainForm?.InvokeRequired == true
                    ? (DialogResult)mainForm.Invoke(callback)
                    : callback.Invoke();

                svg = null;
                if (dialogResult != DialogResult.OK)
                {
                    pdnDocument?.Dispose();
                    throw new OperationCanceledException("Cancelled by user");
                }
                return pdnDocument;
            }
        }

        private void GetInitialValues(out int viewportW, out int viewportH, out int ppi,
            out int viewBoxX, out int viewBoxY, out int viewBoxW, out int viewBoxH)
        {
            ppi = svg.Ppi;
            viewportW = !svg.Width.IsNone && !svg.Width.IsEmpty
                ? Utils.ConvertToPixels(svg.Width.Type, svg.Width.Value, svg.Ppi)
                : 0;
            viewportH = !svg.Height.IsNone && !svg.Height.IsEmpty
                ? Utils.ConvertToPixels(svg.Height.Type, svg.Height.Value, svg.Ppi)
                : 0;
            viewBoxX = (int) svg.ViewBox.MinX;
            viewBoxY = (int) svg.ViewBox.MinY;
            viewBoxW = (int) svg.ViewBox.Width;
            viewBoxH = (int) svg.ViewBox.Height;
        }

        #region Events

        private async void Dialog_OkClick(object sender, EventArgs e)
        {
            pdnDocument = await Task.Run(DoImport, cts.Token)
                .ContinueWith(AfterImport)
                .ConfigureAwait(false);
        }

        private void Dialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            cts.Cancel();
        }

        private void Dialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            cts = null;
            svg = null;
            GC.Collect();
        }

        #endregion

        private DialogResult ShowMemoryWarningDialog(int layerCount)
        {
            var dialogResult = (DialogResult) dialog.Invoke((Func<DialogResult>) (() =>
            {
                string msg = $"This process will import {layerCount} layers.\r\n" +
                             "Rendering many svg elements on a separate layer requires a lot of memory, especially if you're using a large canvas.\r\n" +
                             "Please make sure you've enough available memory before continue.\r\n" +
                             "\r\n" +
                             "Do you really want to continue?";
                return MessageBoxEx.Show(dialog, msg, "Warning",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);
            }));

            return dialogResult;
        }

        private Document DoImport()
        {
            // set the requested size & resolution for SvgDocument
            width = dialog.CanvasW;
            height = dialog.CanvasH;
            svg.Ppi = dialog.TargetDpi;
            svg.Width = new SvgUnit(SvgUnitType.Pixel, width);
            svg.Height = new SvgUnit(SvgUnitType.Pixel, height);
            svg.AspectRatio = dialog.KeepAspectRatio
                ? new SvgAspectRatio(SvgPreserveAspectRatio.xMinYMin)
                : new SvgAspectRatio(SvgPreserveAspectRatio.none);

            LayersMode layersMode = dialog.LayersMode;
            int canvasW = dialog.CanvasW;
            int canvasH = dialog.CanvasH;
            bool importGroupBoundariesAsLayers = dialog.ImportGroupBoundariesAsLayers;
            bool setOpacityForLayer = dialog.ImportOpacity;
            bool importHiddenLayers = dialog.ImportHiddenLayers;
            CancellationToken ct = cts.Token;

            // Render one flat image and quit.
            if (layersMode == LayersMode.Flat)
            {
                using (Bitmap bmp = RenderFlatImage(svg, canvasW, canvasH))
                {
                    return Document.FromImage(bmp);
                }
            }

            IReadOnlyCollection<SvgVisualElement> allElements = PrepareFlatElements(svg.Children)
                .Where(p => p is SvgVisualElement)
                .Cast<SvgVisualElement>().ToList();

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
                    if (ShowMemoryWarningDialog(allElements.Count) != DialogResult.Yes)
                    {
                        dialog.DialogResult = DialogResult.Cancel;
                        return null;
                    }
                }

                // Thread safe
                dialog.SetMaxProgress(allElements.Count);
                dialog.ReportProgress(0);
                pdnDocument = RenderElements(allElements, setOpacityForLayer, importHiddenLayers, dialog.ReportProgress,
                    ct);
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
                                if (!string.IsNullOrEmpty(title))
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

                if (ShowMemoryWarningDialog(groupsAndElementsWithoutGroup.Count) != DialogResult.Yes)
                {
                    dialog.DialogResult = DialogResult.Cancel;
                    return null;
                }

                // Thread safe
                dialog.SetMaxProgress(groupsAndElementsWithoutGroup.Count);
                dialog.ReportProgress(0);

                pdnDocument = RenderElements(groupsAndElementsWithoutGroup, setOpacityForLayer, importHiddenLayers,
                    dialog.ReportProgress, ct);
            }

            // Fallback. Nothing is added. Render one default layer.
            if (pdnDocument == null || pdnDocument.Layers.Count == 0)
            {
                pdnDocument?.Dispose();
                using (Bitmap bmp = RenderFlatImage(svg, canvasW, canvasH))
                {
                    return Document.FromImage(bmp);
                }
            }

            return pdnDocument;
        }

        private Document AfterImport(Task<Document> p)
        {
            if (p.Exception != null && !p.IsCanceled)
            {
                if (p.Exception.InnerExceptions.Any(exception => exception is OutOfMemoryException))
                {
                    dialog.Invoke((Action) (() =>
                    {
                        MessageBoxEx.Show(dialog, "Not enough memory to complete this operation.",
                            "Out of Memory", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
                else
                {
                    var innerExpection = p.Exception?.InnerException?.Message;
                    dialog.Invoke((Action) (() =>
                    {
                        MessageBoxEx.Show(dialog, p.Exception.Message + "\r\nMessage: " + innerExpection,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }

                dialog.DialogResult = DialogResult.Cancel;
                return null;
            }

            if (dialog.DialogResult == DialogResult.None)
            {
                dialog.DialogResult = DialogResult.OK;
            }

            return p.Result;
        }

        #region Render Elements

        private static int GetLayerCountToRender(IReadOnlyCollection<SvgVisualElement> elements,
            bool importHiddenLayers)
        {
            return importHiddenLayers ? elements.Count : elements.Count(IsVisibleOriginally);
        }

        private Document RenderElements(IReadOnlyCollection<SvgVisualElement> elements, bool setOpacityForLayer,
            bool importHiddenLayers, Action<int> progress, CancellationToken token)
        {
            // I had problems to render each element directly while parent transformation can affect child. 
            // But we can do a trick and render full document each time with only required nodes set as visible.

            int layer = 0;
            // Render all visual elements that are passed here.
            foreach (SvgVisualElement element in elements)
            {
                token.ThrowIfCancellationRequested();
                if (element is PaintGroupBoundaries boundaryNode)
                {
                    // Store related group opacity and visibility.
                    if (!importHiddenLayers)
                    {
                        // Skip group boundaries for hidden layers.
                        if (!IsVisibleOriginally(boundaryNode.RelatedGroup))
                        {
                            progress?.Invoke(++layer);
                            continue;
                        }
                    }

                    pdnDocument = pdnDocument ?? new Document(width, height);
                    // Render empty group boundary and continue
                    var pdnLayer = new BitmapLayer(pdnDocument.Width, pdnDocument.Height)
                    {
                        Name = boundaryNode.ID,
                        Opacity = (byte) (boundaryNode.RelatedGroup.Opacity * 255),
                        Visible = boundaryNode.RelatedGroup.Visible
                    };
                    pdnDocument.Layers.Add(pdnLayer);
                    progress?.Invoke(++layer);
                    continue;
                }

                // Turn off visibility of all elements
                foreach (SvgVisualElement elemntToChange in elements)
                {
                    token.ThrowIfCancellationRequested();
                    elemntToChange.Visibility = "hidden";
                }

                var itemShouldBeIgnored = false;

                // Turn on visibility from node to parent
                var toCheck = (SvgElement) element;
                do
                {
                    token.ThrowIfCancellationRequested();
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
                    progress?.Invoke(++layer);
                    continue;
                }

                RenderElement(element, setOpacityForLayer, importHiddenLayers);
                progress?.Invoke(++layer);
            }

            return pdnDocument;
        }

        private void RenderElement(SvgElement element, bool setOpacityForLayer,
            bool importHiddenLayers)
        {
            var opacity = element.Opacity;
            var visible = true;
            var visualElement = element as SvgVisualElement;
            if (visualElement != null)
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

            // Store opacity as layer options.
            if (setOpacityForLayer)
            {
                // Set full opacity when enabled to render 100%. 
                // Anyway opacity will be set as paint layer options.
                if (element.Opacity > 0.01)
                {
                    element.Opacity = 1;
                }
            }

            BitmapLayer pdnLayer;
            pdnDocument = pdnDocument ?? new Document(width, height);
            using (Bitmap bmp = RenderFlatImage(element.OwnerDocument, pdnDocument.Width, pdnDocument.Height))
            using (Surface surface = Surface.CopyFromBitmap(bmp))
            {
                pdnLayer = new BitmapLayer(surface);
            }

            pdnLayer.Name = GetLayerTitle(element); //leg_left_top
            if (setOpacityForLayer)
            {
                pdnLayer.Opacity = (byte) (opacity * 255);
            }

            if (importHiddenLayers && visualElement != null)
            {
                pdnLayer.Visible = visible;
            }

            pdnDocument.Layers.Add(pdnLayer);
        }

        private static bool IsVisibleOriginally(SvgElement element)
        {
            if (element is SvgVisualElement visual)
            {
                if (visual.CustomAttributes.TryGetValue(VisibilityAttribute, out string argument))
                {
                    return bool.Parse(argument);
                }
            }

            return true;
        }

        private static Bitmap RenderFlatImage(SvgDocument doc, int canvasw, int canvash)
        {
            var bmp = new Bitmap(canvasw, canvash);
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                doc.Draw(graph);
            }

            return bmp;
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

            if (string.IsNullOrEmpty(layerName) && element.CustomAttributes != null)
            {
                // get custom title attributes.
                foreach (var titleAttribute in AllowedTitles)
                {
                    if (element.CustomAttributes.TryGetValue(titleAttribute, out string title))
                    {
                        if (!string.IsNullOrEmpty(title))
                        {
                            layerName = title;
                            break;
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(layerName) && element.Children != null)
            {
                // Get child title tag
                SvgTitle title = element.Children.OfType<SvgTitle>().FirstOrDefault();
                if (title != null && !string.IsNullOrEmpty(title.Content))
                {
                    layerName = title.Content;
                }
            }

            if (string.IsNullOrEmpty(layerName))
            {
                // Generate more meanfull name for a svg use node. Add reference element name in a case if it's local document.
                if (element is SvgUse useElement
                    && useElement.ReferencedElement != null
                    && !string.IsNullOrEmpty(useElement.ReferencedElement.OriginalString))
                {
                    string str = useElement.ReferencedElement.OriginalString.Trim();
                    if (str.StartsWith("#"))
                    {
                        layerName = str.Substring(1);
                    }
                }
                // Generate more meanfull name for a svg text.
                else if (element is SvgTextBase text && !string.IsNullOrEmpty(text.Text))
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

        private static IEnumerable<SvgElement> PrepareFlatElements(SvgElementCollection collection,
            string groupName = null)
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
                    if (visual.Visible &&
                        (visual.Display?.Trim().Equals("none", StringComparison.OrdinalIgnoreCase) == true ||
                         visual.Display?.Trim().Equals("hidden", StringComparison.OrdinalIgnoreCase) == true))
                    {
                        visual.Visibility = "hidden";
                        visual.Display = "none";
                    }

                    // Store opacity
                    toRender.CustomAttributes.Add(VisibilityAttribute, visual.Visible.ToString());

                    // Save current group to indicate that elements inside a group.
                    if (!string.IsNullOrEmpty(groupName) && !toRender.ContainsAttribute(GroupAttribute))
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
                    yield return new PaintGroupBoundaries(group)
                    {
                        ID = string.Format(LayerGroupEnd, groupName)
                    };
                }

                IEnumerable<SvgElement> returned = PrepareFlatElements(toRender.Children, groupName);
                if (returned != null)
                {
                    foreach (SvgElement output in returned)
                    {
                        yield return output;
                    }
                }

                if (group != null)
                {
                    // Return fake node to indicate group start.
                    yield return new PaintGroupBoundaries(group)
                    {
                        ID = string.Format(LayerGroupBegin, groupName),
                        IsStart = true
                    };
                }

                // Skip text with empty content. But keep all children nodes.
                if (toRender is SvgTextBase textNode && string.IsNullOrEmpty(textNode.Text))
                {
                    continue;
                }

                yield return toRender;
            }
        }

        #endregion
    }
}