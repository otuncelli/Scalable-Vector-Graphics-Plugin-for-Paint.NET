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
using System.Drawing.Drawing2D;

namespace SvgFileTypePlugin
{
    public class SvgFileType : FileType
    {
        public SvgFileType()
            : base(
                "Scalable Vector Graphics",
                FileTypeFlags.SupportsLoading,
                new[] { ".svg", ".svgz" })
        {
        }

        // Dont change this text! It's used by a PSD import plugin to keep photoshop folder strure.
        public const string LayerGroupBegin = "Layer Group: {0}";
        public const string LayerGroupEnd = "End Layer Group: {0}";

        private static string groupAttribute = "import_group_name";
        private static string visibilityAttribute = "import_visibility";

        private static string[] allowedTitles = new string[] { "label", "title", "inskape:label" };

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

            bool keepAspectRatio;
            int resolution;
            int canvasw;
            int canvash;
            var vpw = 0;
            var vph = 0;
            var ppi = doc.Ppi;

            var layersMode = LayersMode.All;
            if (!doc.Width.IsNone && !doc.Width.IsEmpty)
            {
                vpw = ConvertToPixels(doc.Width.Type, doc.Width.Value, doc.Ppi);
            }

            if (!doc.Height.IsNone && !doc.Height.IsEmpty)
            {
                vph = ConvertToPixels(doc.Height.Type, doc.Height.Value, doc.Ppi);
            }

            var vbx = (int)doc.ViewBox.MinX;
            var vby = (int)doc.ViewBox.MinY;
            var vbw = (int)doc.ViewBox.Width;
            var vbh = (int)doc.ViewBox.Height;
            
            // Store opacity as layer options.
            var setOpacityForLayer = true;
            var importHiddenLayers = true;
            var importGroupBoundariesAsLayers = false;
            DialogResult dr = DialogResult.Cancel;
            using (var dialog = new UiDialog())
            {
                Form mainForm = GetMainForm();
                if (mainForm != null)
                {
                    mainForm.Invoke((MethodInvoker)(() =>
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
                canvasw = dialog.CanvasW;
                canvash = dialog.CanvasH;
                resolution = dialog.Dpi;
                layersMode = dialog.LayerMode;
                keepAspectRatio = dialog.KeepAspectRatio;
                setOpacityForLayer = dialog.ImportOpacity;
                importHiddenLayers = dialog.ImportHiddenLayers;
                importGroupBoundariesAsLayers = dialog.ImportGroupBoundariesAsLayers;
            }

            doc.Ppi = resolution;
            doc.Width = new SvgUnit(SvgUnitType.Pixel, canvasw);
            doc.Height = new SvgUnit(SvgUnitType.Pixel, canvash);
            doc.AspectRatio = keepAspectRatio
                ? new SvgAspectRatio(SvgPreserveAspectRatio.xMinYMin)
                : new SvgAspectRatio(SvgPreserveAspectRatio.none);


            if (layersMode== LayersMode.Flat)
            {
                // Render one flat image and quit.
                var bmp = RenderImage(doc, canvasw, canvash);
                return Document.FromImage(bmp);
            }
            else
            {
                var allElements = PrepareFlatElements(doc.Children).Where(p => p is SvgVisualElement).Cast<SvgVisualElement>().ToList();

                Document outputDocument = new Document(canvasw, canvash);
                if (layersMode == LayersMode.All)
                {
                    // Dont render groups and boundaries if defined
                    allElements = allElements.Where(p => !(p is SvgGroup)).ToList();

                    // Filter out group boundaries if not set.
                    if (!importGroupBoundariesAsLayers)
                    {
                        allElements = allElements.Where(p => !(p is PaintGroupBoundaries)).ToList();
                    }

                    RenderElements(allElements, outputDocument, setOpacityForLayer, importHiddenLayers);
                }
                else if (layersMode == LayersMode.Groups)
                {
                    // Get only parent groups and single elements
                    var groupsAndElementsWithoutGroup = new List<SvgVisualElement>();
                  
                    foreach(var element in allElements)
                    {
                        if (element is PaintGroupBoundaries)
                            continue;

                        if (element.ContainsAttribute(groupAttribute))
                        {
                            // Get only root level
                            SvgGroup lastGroup = null;
                            if (element is SvgGroup)
                            {
                                lastGroup = (SvgGroup)element;
                            }

                            SvgElement toCheck = element;
                            while (toCheck != null)
                            {
                                toCheck = toCheck.Parent;
                                if (toCheck is SvgGroup)
                                {
                                    // TODO: render more groups. In most cases svg has only few root groups.
                                    var groupToCheck= (SvgGroup)toCheck;
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

                    RenderElements(groupsAndElementsWithoutGroup, outputDocument, setOpacityForLayer, importHiddenLayers);
                }

                // Fallback. Nothing is added. Render one default layer.
                if (outputDocument.Layers.Count == 0)
                {
                    var bmp = RenderImage(doc, canvasw, canvash);
                    return Document.FromImage(bmp);
                }

                return outputDocument;
            }
        }

        private static int ConvertToPixels(SvgUnitType type, float value, float ppi)
        {
            var defaultRatioFor96 = 3.78;
            var convertationRatio = ppi / 96 * defaultRatioFor96;

            if (type == SvgUnitType.Millimeter)
            {
                return (int)Math.Ceiling(value * convertationRatio);
            }
            else if (type == SvgUnitType.Centimeter)
            {
                return (int)Math.Ceiling(value * 10 * convertationRatio);
            }
            else if (type == SvgUnitType.Inch)
            {
                return (int)Math.Ceiling(value * 25.4 * convertationRatio);
            }
            else if (type == SvgUnitType.Em || type == SvgUnitType.Pica)
            {
                // Default 1 em for 16 pixels.
                return (int)Math.Ceiling(value * 16);
            }
            else if (type != SvgUnitType.Percentage)
            {
                return (int)Math.Ceiling(value);
            }
            else
            {
                return 0;
            }
        }

        private static void RenderElements( List<SvgVisualElement> elements, Document outputDocument, bool setOpacityForLayer, bool importHiddenLayers)
        {
            // I had problems to render each element directly while parent transformation can affect child. 
            // But we can do a trick and render full document each time with only required nodes set as visible.

            // Render all visual elements that are passed here.
            foreach (var element in elements)
            {
                if (element is PaintGroupBoundaries)
                {
                    // Render empty boundary and continue
                    var pdnLayer = new BitmapLayer(outputDocument.Width, outputDocument.Height);
                    pdnLayer.Name = ((PaintGroupBoundaries)element).ID;
                    outputDocument.Layers.Add(pdnLayer);
                    continue;
                }

                // Turn off visibility of all elements
                foreach (var elemntToChange in elements)
                {
                    elemntToChange.Visible = false;
                }

                bool itemShouldBeIgnored = false;
                // Turn on visibility from node to parent
                var toCheck = (SvgElement)element;
                while (toCheck != null)
                {
                    var visual = toCheck as SvgVisualElement;
                    if (visual != null)
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

                if(itemShouldBeIgnored)
                {
                    continue;
                }

                RenderElement(element, outputDocument, setOpacityForLayer, importHiddenLayers);
            }
        }

        private static void RenderElement(SvgElement element, Document outputDocument, bool setOpacityForLayer, bool importHiddenLayers)
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
                // Set full opacity when enabled to render 100%. We will use this opacity as layer property.
                if (element.Opacity > 0.01)
                {
                    element.Opacity = 1;
                }
            }
            
            using (var bmp = RenderImage(element.OwnerDocument, outputDocument.Width, outputDocument.Height))
            {
                var pdnLayer = new BitmapLayer(Surface.CopyFromBitmap(bmp));
                var layerTitle = GetLayerTitle(element);
                pdnLayer.Name = layerTitle;//leg_left_top
                if (setOpacityForLayer)
                {
                    pdnLayer.Opacity = (byte)(opacity * 255);
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
            var visual = toCheck as SvgVisualElement;
            if (visual != null)
            {
                var argument = string.Empty;
                if (visual.CustomAttributes.TryGetValue(visibilityAttribute, out argument))
                {
                    return bool.Parse(argument);
                }
            }

            return true;
        }

        private static Bitmap RenderImage(SvgDocument doc, int canvasw, int canvash)
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
        private static string GetLayerTitle(SvgElement element)
        {
            var layerName = element.ID;
            if (element.CustomAttributes != null)
            {
                // get custom title attributes.
                foreach (var titleAttribute in allowedTitles)
                {
                    string title;
                    if (element.CustomAttributes.TryGetValue(titleAttribute, out title))
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
                var prop = typeof(SvgElement).GetProperty("ElementName", BindingFlags.NonPublic | BindingFlags.Instance);
                if (prop != null)
                {
                    var getter = prop.GetGetMethod(nonPublic: true);
                    layerName = getter.Invoke(element, null) as string;
                }

                if(string.IsNullOrEmpty(layerName))
                {
                    layerName = element.GetType().Name;
                }
            }

            return layerName;
        }

        private static IEnumerable<SvgElement> PrepareFlatElements(SvgElementCollection collection, string groupName = null)
        {
            // Prepare a collection of elements that about to be rendered. 
            if (collection != null)
            {
                foreach (var toRender in collection)
                {
                    // Dont prepare for a separate parsing def lists.
                    if (toRender is SvgDefinitionList)
                    {
                        continue;
                    }

                    // Dont prepare def lists for a separate rendering.
                    if (toRender is SvgDefinitionList)
                    {
                        continue;
                    }

                    var isGroup = toRender is SvgGroup;
                    if (isGroup)
                    {
                        groupName = GetLayerTitle((SvgGroup)toRender);

                        // Return fake node to indicate group end. 
                        yield return new PaintGroupBoundaries() { ID = string.Format(LayerGroupEnd, groupName) };
                    }

                    var visual = toRender as SvgVisualElement;

                    if (visual != null)
                    {
                        // Fix problem that SVG visual element lib style "display:none" is not recognized as visible state.
                        if (visual.Visible && (visual.Display == "none" || visual.Display == "hidden"))
                        {
                            visual.Visible = false;
                            visual.Display = null;
                        }

                        // Store opacity
                        toRender.CustomAttributes.Add(visibilityAttribute, visual.Visible.ToString());

                        // Save current group to indicate that elements inside a group.
                        if (!string.IsNullOrEmpty(groupName) && !toRender.ContainsAttribute(groupAttribute))
                        {
                            // Store group info
                            toRender.CustomAttributes.Add(groupAttribute, groupName);
                        }
                    }

                    var returned = PrepareFlatElements(toRender.Children, groupName);
                    if (returned != null)
                    {
                        foreach (var output in returned)
                        {
                            yield return output;
                        }
                    }

                    if (isGroup)
                    {
                        groupName = GetLayerTitle((SvgGroup)toRender);

                        // Return fake node to indicate group start.
                        yield return new PaintGroupBoundaries() { ID = string.Format(LayerGroupBegin, groupName), IsStart = true };
                    }

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

    // Used to determine boundaries of a group.
    public class PaintGroupBoundaries : SvgVisualElement
    {
        public bool IsStart { get; set; }
        public override RectangleF Bounds => throw new NotImplementedException();

        public override SvgElement DeepCopy()
        {
            throw new NotImplementedException();
        }

        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            throw new NotImplementedException();
        }
    }
}