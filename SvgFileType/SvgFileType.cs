using PaintDotNet;
using Svg;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;

namespace SvgFileTypePlugin
{
    public class SvgFileType : FileType
    {
        public SvgFileType()
            : base(
                "Scalable Vector Graphics",
                new FileTypeOptions {LoadExtensions = new[] {".svg", ".svgz"}})
        {
        }

        // Don't change this text! It's used by a PSD import plugin to keep Photoshop's folder structure.
        public const string LayerGroupBegin = "Layer Group: {0}";
        public const string LayerGroupEnd = "End Layer Group: {0}";

        private const string GroupAttribute = "import_group_name";
        private static readonly string VisibilityAttribute = "import_visibility";

        private static readonly string[] AllowedTitles = new string[] {"label", "title", "inskape:label"};

        private static Form GetMainForm()
        {
            try
            {
                var form = Control.FromHandle(Process.GetCurrentProcess().MainWindowHandle) as Form;
                return form ?? Application.OpenForms["MainForm"];
            }
            catch
            {
                return null;
            }
        }

        protected override Document OnLoad(Stream input)
        {
            return Get(input);
        }

        public static Document Get(Stream input)
        {
            SvgDocument doc;
            using (var wrapper = new SvgStreamWrapper(input))
                doc = SvgDocument.Open<SvgDocument>(wrapper.SvgStream);

            var vpw = 0;
            var vph = 0;
            var ppi = doc.Ppi;
            if (!doc.Width.IsNone && !doc.Width.IsEmpty)
            {
                vpw = ConvertToPixels(doc.Width.Type, doc.Width.Value, doc.Ppi);
            }

            if (!doc.Height.IsNone && !doc.Height.IsEmpty)
            {
                vph = ConvertToPixels(doc.Height.Type, doc.Height.Value, doc.Ppi);
            }

            var vbx = (int) doc.ViewBox.MinX;
            var vby = (int) doc.ViewBox.MinY;
            var vbw = (int) doc.ViewBox.Width;
            var vbh = (int) doc.ViewBox.Height;

            // Store opacity as layer options.
            bool setOpacityForLayer;
            bool importHiddenLayers;
            bool importGroupBoundariesAsLayers;
            var dr = DialogResult.Cancel;
            Document results = null;
            using (var dialog = new UiDialog())
            {
                var tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;

                dialog.FormClosing += (o, e) => { tokenSource.Cancel(); };

                dialog.OkClick += (o, e) =>
                {
                    var canvasw = dialog.CanvasW;
                    var canvash = dialog.CanvasH;
                    var resolution = dialog.Dpi;
                    var layersMode = dialog.LayerMode;
                    var keepAspectRatio = dialog.KeepAspectRatio;
                    setOpacityForLayer = dialog.ImportOpacity;
                    importHiddenLayers = dialog.ImportHiddenLayers;
                    importGroupBoundariesAsLayers = dialog.ImportGroupBoundariesAsLayers;

                    doc.Ppi = resolution;
                    doc.Width = new SvgUnit(SvgUnitType.Pixel, canvasw);
                    doc.Height = new SvgUnit(SvgUnitType.Pixel, canvash);
                    doc.AspectRatio = keepAspectRatio
                        ? new SvgAspectRatio(SvgPreserveAspectRatio.xMinYMin)
                        : new SvgAspectRatio(SvgPreserveAspectRatio.none);

                    var progressCallback = new Action<int>(p => dialog.ReportProgress(p));
                    // Run in another thread and unblock the UI.
                    // Cannot run .AsParallel().AsOrdered() to render each element in async thread while gdi+ svg renderer failing with errors...
                    Task.Run(() =>
                        {
                            if (layersMode == LayersMode.Flat)
                            {
                                // Render one flat image and quit.
                                var bmp = RenderImage(doc, canvasw, canvash);
                                results = Document.FromImage(bmp);
                            }
                            else
                            {
                                List<SvgVisualElement> allElements = PrepareFlatElements(doc.Children)
                                    .Where(p => p is SvgVisualElement).Cast<SvgVisualElement>().ToList();

                                var outputDocument = new Document(canvasw, canvash);
                                if (layersMode == LayersMode.All)
                                {
                                    // Dont render groups and boundaries if defined
                                    allElements = allElements.Where(p => !(p is SvgGroup)).ToList();

                                    // Filter out group boundaries if not set.
                                    if (!importGroupBoundariesAsLayers)
                                    {
                                        allElements = allElements.Where(p => !(p is PaintGroupBoundaries)).ToList();
                                    }

                                    // Thread safe
                                    dialog.SetMaxProgress(allElements.Count + 10);
                                    dialog.ReportProgress(10);

                                    RenderElements(allElements, outputDocument, setOpacityForLayer, importHiddenLayers,
                                        progressCallback, token);
                                }
                                else if (layersMode == LayersMode.Groups)
                                {
                                    // Get only parent groups and single elements
                                    var groupsAndElementsWithoutGroup = new List<SvgVisualElement>();

                                    foreach (var element in allElements)
                                    {
                                        if (element is PaintGroupBoundaries)
                                            continue;

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
                                                    var title = GetLayerTitle(groupToCheck);

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
                                            groupsAndElementsWithoutGroup.Add(element);
                                        }
                                    }

                                    // Thread safe
                                    dialog.SetMaxProgress(groupsAndElementsWithoutGroup.Count + 10);
                                    dialog.ReportProgress(10);

                                    RenderElements(groupsAndElementsWithoutGroup, outputDocument, setOpacityForLayer,
                                        importHiddenLayers, progressCallback, token);
                                }

                                // Fallback. Nothing is added. Render one default layer.
                                if (outputDocument.Layers.Count == 0)
                                {
                                    var bmp = RenderImage(doc, canvasw, canvash);
                                    outputDocument = Document.FromImage(bmp);
                                }

                                results = outputDocument;
                            }

                        }, token)
                        .ContinueWith(p =>
                        {
                            if (p.Exception != null && !p.IsCanceled)
                            {
                                if (p.Exception.InnerExceptions != null &&
                                    p.Exception.InnerExceptions.Any(exception => exception is OutOfMemoryException))
                                {
                                    MessageBox.Show("Not enough memory to complete this operation.");
                                }
                                else
                                {
                                    var innerExpection = p.Exception?.InnerException?.Message;
                                    MessageBox.Show(p.Exception.Message + ". Message:" + innerExpection);
                                }

                                dialog.DialogResult = DialogResult.Cancel;
                            }
                            else
                            {
                                if (dialog.DialogResult == DialogResult.None)
                                    dialog.DialogResult = DialogResult.OK;
                            }
                        }, CancellationToken.None);
                };

                Form mainForm = GetMainForm();
                if (mainForm != null)
                {
                    mainForm.Invoke((MethodInvoker) (() =>
                    {
                        dialog.SetSvgInfo(vpw, vph, vbx, vby, vbw, vbh, ppi);
                        dr = dialog.ShowDialog(mainForm);
                    }));
                }
                else
                {
                    dialog.SetSvgInfo(vpw, vph, vbx, vby, vbw, vbh, ppi);
                    dr = dialog.ShowDialog();
                }

                if (dr != DialogResult.OK)
                    throw new OperationCanceledException("Cancelled by user");

                return results;
            }
        }

        private static int ConvertToPixels(SvgUnitType type, float value, float ppi)
        {
            const double defaultRatioFor96 = 3.78;
            var convertationRatio = ppi / 96 * defaultRatioFor96;

            if (type == SvgUnitType.Millimeter)
            {
                return (int) Math.Ceiling(value * convertationRatio);
            }

            if (type == SvgUnitType.Centimeter)
            {
                return (int) Math.Ceiling(value * 10 * convertationRatio);
            }

            if (type == SvgUnitType.Inch)
            {
                return (int) Math.Ceiling(value * 25.4 * convertationRatio);
            }

            if (type == SvgUnitType.Em || type == SvgUnitType.Pica)
            {
                // Default 1 em for 16 pixels.
                return (int) Math.Ceiling(value * 16);
            }

            if (type != SvgUnitType.Percentage)
            {
                return (int) Math.Ceiling(value);
            }

            return 0;
        }

        private static void RenderElements(IReadOnlyCollection<SvgVisualElement> elements, Document outputDocument,
            bool setOpacityForLayer, bool importHiddenLayers, Action<int> progress, CancellationToken token)
        {
            // I had problems to render each element directly while parent transformation can affect child. 
            // But we can do a trick and render full document each time with only required nodes set as visible.

            // Render all visual elements that are passed here.
            int layer = 0;
            foreach (var element in elements)
            {
                token.ThrowIfCancellationRequested();

                if (element is PaintGroupBoundaries boundaryNode)
                {
                    // Render empty group boundary and continue
                    var pdnLayer = new BitmapLayer(outputDocument.Width, outputDocument.Height)
                    {
                        Name = boundaryNode.ID
                    };

                    // Store related group opacity and visibility.
                    if (boundaryNode.RelatedGroup != null)
                    {
                        if (!importHiddenLayers)
                        {
                            // Skip group boundaries for hidden layers.
                            if (!GetOriginalVisibilityState(boundaryNode.RelatedGroup))
                            {
                                layer++;
                                progress?.Invoke(layer);
                                continue;
                            }
                        }

                        pdnLayer.Opacity = (byte) (boundaryNode.RelatedGroup.Opacity * 255);
                        pdnLayer.Visible = boundaryNode.RelatedGroup.Visible;
                    }

                    outputDocument.Layers.Add(pdnLayer);
                    layer++;
                    progress?.Invoke(layer);
                    continue;
                }

                // Turn off visibility of all elements
                foreach (var elemntToChange in elements)
                {
                    elemntToChange.Visible = false;
                }

                var itemShouldBeIgnored = false;

                // Turn on visibility from node to parent
                var toCheck = (SvgElement) element;
                while (toCheck != null)
                {
                    if (toCheck is SvgVisualElement visual)
                    {
                        visual.Visible = true;
                        // Check, maybe parent group was initially hidden
                        if (!importHiddenLayers)
                        {
                            // Skip hidden layers.
                            if (!GetOriginalVisibilityState(visual))
                            {
                                itemShouldBeIgnored = true;
                                break;
                            }
                        }
                    }

                    toCheck = toCheck.Parent;
                }

                if (itemShouldBeIgnored)
                {
                    layer++;
                    progress?.Invoke(layer);
                    continue;
                }

                RenderElement(element, outputDocument, setOpacityForLayer, importHiddenLayers);

                layer++;
                progress?.Invoke(layer);
            }
        }

        private static void RenderElement(SvgElement element, Document outputDocument, bool setOpacityForLayer,
            bool importHiddenLayers)
        {
            var opacity = element.Opacity;
            var visualElement = (element as SvgVisualElement);
            var visible = true;
            if (visualElement != null)
            {
                visible = GetOriginalVisibilityState(visualElement);
                if (importHiddenLayers)
                {
                    // Set visible to render image and then item can be hidden.
                    visualElement.Visible = true;

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

            using (var bmp = RenderImage(element.OwnerDocument, outputDocument.Width, outputDocument.Height))
            {
                var pdnLayer = new BitmapLayer(Surface.CopyFromBitmap(bmp));
                var layerTitle = GetLayerTitle(element);
                pdnLayer.Name = layerTitle; //leg_left_top
                if (setOpacityForLayer)
                {
                    pdnLayer.Opacity = (byte) (opacity * 255);
                }

                if (importHiddenLayers && visualElement != null)
                {
                    pdnLayer.Visible = visible;
                }

                outputDocument.Layers.Add(pdnLayer);
            }
        }

        private static bool GetOriginalVisibilityState(SvgElement toCheck, bool forceVisible = false)
        {
            if (toCheck is SvgVisualElement visual)
            {
                if (visual.CustomAttributes.TryGetValue(VisibilityAttribute, out string argument))
                {
                    return bool.Parse(argument);
                }
            }

            return true;
        }

        private static Bitmap RenderImage(SvgDocument doc, int canvasw, int canvash)
        {
            var bmp = new Bitmap(canvasw, canvash);
            using (var graph = Graphics.FromImage(bmp))
            {
                doc.Draw(graph);
            }

            return bmp;
        }

        /// <summary>
        /// Get a title for a specified svg element.
        /// </summary>
        private static string GetLayerTitle(SvgElement element)
        {
            var layerName = element.ID;
            if (element.CustomAttributes != null)
            {
                // get custom title attributes.
                foreach (var titleAttribute in AllowedTitles)
                {
                    if (element.CustomAttributes.TryGetValue(titleAttribute, out string title))
                    {
                        if (!string.IsNullOrEmpty(title))
                        {
                            layerName = title;
                            return layerName;
                        }
                    }
                }

                // Get child title tag
                if (element.Children != null)
                {
                    var title = element.Children.FirstOrDefault(p => p is SvgTitle);
                    if (title != null && !string.IsNullOrEmpty(title.Content))
                    {
                        layerName = title.Content;
                        return layerName;
                    }
                }
            }

            if (string.IsNullOrEmpty(layerName))
            {
                var prop = typeof(SvgElement).GetProperty("ElementName",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                if (prop != null)
                {
                    var getter = prop.GetGetMethod(nonPublic: true);
                    layerName = getter.Invoke(element, null) as string;
                }

                if (string.IsNullOrEmpty(layerName))
                {
                    layerName = element.GetType().Name;
                }

                // Generate more meanfull name for a svg use node. Add reference element name in a case if it's local document.
                if (element is SvgUse useElement
                    && useElement.ReferencedElement != null
                    && !string.IsNullOrEmpty(useElement.ReferencedElement.OriginalString))
                {
                    var str = useElement.ReferencedElement.OriginalString.Trim();
                    if (str.StartsWith("#"))
                    {
                        layerName += str.Replace('#', ' ');
                    }
                }

                // Generate more meanfull name for a svg text.
                if (element is SvgTextBase text && !string.IsNullOrEmpty(text.Text))
                {
                    var textToUse = text.Text;
                    if (text.Text.Length > 35)
                        textToUse = text.Text.Substring(0, 35);
                    layerName += " " + textToUse;
                }
            }

            return layerName;
        }

        private static IEnumerable<SvgElement> PrepareFlatElements(SvgElementCollection collection,
            string groupName = null)
        {
            // Prepare a collection of elements that about to be rendered. 
            if (collection != null)
            {
                foreach (var toRender in collection)
                {
                    // Don't prepare for a separate parsing def lists.
                    if (toRender is SvgDefinitionList)
                    {
                        continue;
                    }

                    if (toRender is SvgVisualElement visual)
                    {
                        // Fix problem that SVG visual element lib style "display:none" is not recognized as visible state.
                        if (visual.Visible && (visual.Display == "none" || visual.Display == "hidden"))
                        {
                            visual.Visible = false;
                            visual.Display = null;
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
                        yield return new PaintGroupBoundaries()
                        {
                            RelatedGroup = group,
                            ID = string.Format(LayerGroupEnd, groupName)
                        };
                    }

                    var returned = PrepareFlatElements(toRender.Children, groupName);
                    if (returned != null)
                    {
                        foreach (var output in returned)
                        {
                            yield return output;
                        }
                    }

                    if (group != null)
                    {
                        // Return fake node to indicate group start.
                        yield return new PaintGroupBoundaries()
                        {
                            ID = string.Format(LayerGroupBegin, groupName),
                            IsStart = true,
                            RelatedGroup = group
                        };
                    }

                    // Skip text with empty content. But keep all children nodes.
                    if (toRender is SvgTextBase textNode && string.IsNullOrEmpty(textNode.Text))
                        continue;

                    yield return toRender;
                }
            }
        }

        private sealed class SvgStreamWrapper : IDisposable
        {
            public Stream SvgStream { get; }

            public SvgStreamWrapper(Stream input)
            {
                if (input.Length < 3)
                    throw new InvalidDataException();
                var headerBytes = new byte[3];
                input.Read(headerBytes, 0, 3);
                input.Position = 0;
                if (headerBytes[0] == 0x1f && headerBytes[1] == 0x8b && headerBytes[2] == 0x8)
                    SvgStream = new GZipStream(input, CompressionMode.Decompress, true);
                else
                    SvgStream = input;
            }

            #region IDisposable

            private bool _disposed;

            public void Dispose()
            {
                if (_disposed)
                    return;
                if (SvgStream is GZipStream)
                    SvgStream.Dispose();
                _disposed = true;
            }

            #endregion
        }
    }
}