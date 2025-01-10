// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading;
using PaintDotNet;
using Svg;
using SvgFileTypePlugin.Extensions;
using SR = SvgFileTypePlugin.Localization.StringResources;

namespace SvgFileTypePlugin.Import;

internal class DefaultSvgConverter : ISvgConverter
{
    public static DefaultSvgConverter Instance { get; } = new DefaultSvgConverter();

    public virtual string Name => "GDI+";

    #region Prepare

    public IEnumerable<SvgVisualElement> Prepare(SvgDocument svg, SvgImportConfig config, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(svg);
        ArgumentNullException.ThrowIfNull(config);

        IEnumerable<SvgVisualElement> elements = PrepareRecursive(svg.Children, groupName: null, cancellationToken);
        if (config.LayersMode == LayersMode.Groups)
        {
            elements = FilterGroups(elements, cancellationToken);
        }
        else
        {
            elements = elements.NotOfType<SvgGroup>();
            if (config.LayersMode == LayersMode.Flat || !config.GroupBoundariesAsLayers)
            {
                elements = elements.NotOfType<GroupBoundary>();
            }
        }

        if (config.LayersMode == LayersMode.Flat)
        {
            return elements;
        }

        // Skip group boundaries for hidden layers.
        elements = elements.Where(e => config.ImportHiddenElements || (e is GroupBoundary groupBoundary ? groupBoundary.GetOriginalVisibility() : e.IsOriginallyVisible()));
        return elements;
    }

    private static IEnumerable<SvgVisualElement> FilterGroups(IEnumerable<SvgVisualElement> elements, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(elements);

        HashSet<SvgVisualElement> groupElements = [];
        foreach (SvgVisualElement element in elements.NotOfType<GroupBoundary>())
        {
            cancellationToken.ThrowIfCancellationRequested();

            SvgVisualElement? visual = null;

            if (element.GetGroupName() != null)
            {
                // Get only root level
                for (SvgElement parent = element; parent != null; parent = parent.Parent)
                {
                    // TODO: render more groups. In most cases svg has only few root groups.
                    if (parent is SvgGroup group && !string.IsNullOrEmpty(GetLayerTitle(group)))
                    {
                        visual = group;
                    }
                }
            }

            visual ??= element;
            if (groupElements.Contains(visual))
            {
                continue;
            }
            groupElements.Add(visual);
            yield return visual;
        }
    }

    private static IEnumerable<SvgVisualElement> PrepareRecursive(SvgElementCollection? elements, string? groupName = null, CancellationToken cancellationToken = default)
    {
        if (elements == null || elements.Count == 0)
        {
            yield break;
        }

        // Prepare a collection of elements that about to be rendered. 
        // Don't prepare for a separate parsing def lists.
        foreach (SvgVisualElement visual in elements.OfType<SvgVisualElement>())
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Fix problem that SVG visual element lib style "display:none" is not recognized as visible state.
            if (visual.Visible && !visual.IsDisplayable())
            {
                visual.Visibility = "hidden";
                visual.Display = string.Empty; // null throws exception
            }

            // Store visibility
            visual.StoreOriginalVisibility();

            // Save current group to indicate that elements inside a group.
            if (!string.IsNullOrEmpty(groupName))
            {
                // Store group info
                visual.SetGroupName(groupName);
            }

            SvgGroup? group = visual as SvgGroup;
            GroupBoundary boundary;
            if (group != null)
            {
                groupName = GetLayerTitle(group, prependElementName: false);

                // Return fake node to indicate group end. (order is reversed)
                string name = string.Format(SR.EndLayerGroup, groupName);
                boundary = new GroupBoundary(group, name, false);
                yield return boundary;
            }

            if (PrepareRecursive(visual.Children, groupName, cancellationToken) is IEnumerable<SvgVisualElement> prepared)
            {
                foreach (SvgVisualElement element in prepared)
                {
                    yield return element;
                }
            }

            if (group != null)
            {
                // Return fake node to indicate group start.
                string name = string.Format(SR.LayerGroup, groupName);
                boundary = new GroupBoundary(group, name, true);
                yield return boundary;
            }

            // Skip text with empty content. But keep all children nodes.
            if (visual is SvgTextBase textNode && string.IsNullOrEmpty(textNode.Text))
            {
                continue;
            }

            yield return visual;
        }
    }

    #endregion

    #region Render

    public virtual Document GetFlatDocument(IReadOnlyCollection<SvgVisualElement> elements, SvgImportConfig config, Action<int>? progress = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(elements);
        ArgumentNullException.ThrowIfNull(config);

        Logger.WriteLine($"Using {Name}...");
        cancellationToken.ThrowIfCancellationRequested();
        int layersProcessed = 0;
        using Bitmap bmp = new Bitmap(config.Width, config.Height);
        using (Graphics g = Graphics.FromImage(bmp))
        {
            // Render all visual elements that are passed here.
            foreach (SvgVisualElement element in elements)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (!element.PreRender(elements, config.ImportHiddenElements, cancellationToken))
                {
                    IncrementProgress(progress, ref layersProcessed);
                    continue;
                }
                RenderSvgDocument(element, g);
                IncrementProgress(progress, ref layersProcessed);
            }
        }
        Document document = Document.FromImage(bmp);
        document.SetDpi(config.Ppi);
        return document;
    }

    public virtual Document GetFlatDocument(Stream stream, SvgImportConfig config, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Document GetFlatDocument(SvgDocument svg, SvgImportConfig config, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(svg);
        ArgumentNullException.ThrowIfNull(config);

        IEnumerable<SvgVisualElement> elements = Prepare(svg, config, cancellationToken);
        List<SvgVisualElement> prepared = elements.ToList();
        return GetFlatDocument(prepared, config, null, cancellationToken);
    }

    public Document GetFlatDocument(SvgDocument svg, Stream stream, SvgImportConfig config)
    {
        ArgumentNullException.ThrowIfNull(svg);
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentNullException.ThrowIfNull(config);

        using MyCancellationTokenSource cts = new MyCancellationTokenSource(stream);
        return GetFlatDocument(svg, config, cts.Token);
    }

    public virtual Document GetLayeredDocument(IReadOnlyCollection<SvgVisualElement> elements, SvgImportConfig config, Action<int>? progress = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(elements);
        ArgumentNullException.ThrowIfNull(config);

        Logger.WriteLine("Using GDI+...");
        cancellationToken.ThrowIfCancellationRequested();
        IEnumerable<BitmapLayer> GetLayers()
        {
            int layersProcessed = 0;
            using Bitmap bmp = new Bitmap(config.Width, config.Height);
            using Graphics g = Graphics.FromImage(bmp);
            // Render all visual elements that are passed here.
            foreach (SvgVisualElement element in elements)
            {
                cancellationToken.ThrowIfCancellationRequested();

                BitmapLayer? layer = null;

                if (element is GroupBoundary boundaryNode)
                {
                    // Render empty group boundary and continue
                    layer = new BitmapLayer(config.Width, config.Height);
                    try
                    {
                        layer.Name = boundaryNode.Name;
                        // Store related group opacity and visibility.
                        layer.Opacity = ToByteOpacity(boundaryNode.Opacity);
                        layer.Visible = boundaryNode.Visible;
                    }
                    catch (Exception)
                    {
                        layer.Dispose();
                        throw;
                    }
                    IncrementProgress(progress, ref layersProcessed);
                    yield return layer;
                    continue;
                }

                if (!element.PreRender(elements, config.ImportHiddenElements, cancellationToken))
                {
                    IncrementProgress(progress, ref layersProcessed);
                    continue;
                }

                g.Clear(default);
                RenderSvgDocument(element, g);

                Surface surface = Surface.CopyFromBitmap(bmp, detectDishonestAlpha: false);
                layer = new BitmapLayer(surface, takeOwnership: true);
                try
                {
                    layer.Name = GetLayerTitle(element);
                    if (config.RespectElementOpacity)
                    {
                        layer.Opacity = ToByteOpacity(element.Opacity);
                    }

                    if (config.ImportHiddenElements && !element.IsOriginallyVisible())
                    {
                        layer.Visible = false;
                    }
                }
                catch (Exception)
                {
                    surface.Dispose();
                    layer?.Dispose();
                    throw;
                }

                IncrementProgress(progress, ref layersProcessed);
                yield return layer;
            }
        }
        return GetDocument(GetLayers(), config);
    }

    #region Private Render

    protected virtual void RenderSvgDocument(SvgElement element, object context)
    {
        ArgumentNullException.ThrowIfNull(element);
        ArgumentNullException.ThrowIfNull(context);

        Graphics g = (Graphics)context;
        if (element is SvgUse use)
        {
            RenderSvgUseElement(use, e => RenderSvgDocument(e, context));
        }
        else
        {
            SvgDocument clone = element.OwnerDocument.Cleanup();
            clone.Draw(g);
        }
    }

    protected virtual void RenderSvgUseElement(SvgUse use, Action<SvgElement> renderCallback)
    {
        ArgumentNullException.ThrowIfNull(use);
        ArgumentNullException.ThrowIfNull(renderCallback);

        SvgElement? refCopy = use.CopyReferencedRootElement();
        if (refCopy is null)
            return;
        refCopy.Visibility = "visible";
        use.Visibility = "hidden";
        SvgElementCollection children = use.Parent.Children;
        children.AddAndForceUniqueID(refCopy);
        try
        {
            renderCallback(refCopy);
        }
        finally
        {
            children.Remove(refCopy);
        }
    }

    #endregion

    #endregion

    #region GetNoPathDocument

    public virtual Document GetNoPathDocument()
    {
        string text = SR.NoPath;
        using Font font = new Font("Arial", 24, FontStyle.Bold);
        Graphics g;
        Size size;
        using (g = Graphics.FromHwnd(IntPtr.Zero))
        {
            size = Size.Round(g.MeasureString(text, font));
        }

        StringFormat sf = new StringFormat()
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        using (sf)
        using (Bitmap bmp = new Bitmap(size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb))
        using (g = Graphics.FromImage(bmp))
        {
            g.Clear(Color.LightGray);
            g.DrawString(text, font, Brushes.Black, new Rectangle(default, bmp.Size), sf);
            return Document.FromImage(bmp);
        }
    }

    #endregion

    #region Helpers

    protected static byte ToByteOpacity(float opacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(opacity);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(opacity, 1f);

        int value = (int)MathF.Round(opacity * 255f);
        value = Math.Clamp(value, byte.MinValue, byte.MaxValue);
        return (byte)value;
    }

    protected static void IncrementProgress(Action<int>? progressCallback, ref int layersProcessed)
    {
        progressCallback?.Invoke(++layersProcessed);
    }

    private static readonly IReadOnlyCollection<string> TitleAttributes = ["label", "title", "inkscape:label"];

    protected static string GetLayerTitle(SvgElement element, bool prependElementName = true)
    {
        ArgumentNullException.ThrowIfNull(element);

        string elementName = element.GetName();
        string? layerName = null;

        if (element.ID != null)
        {
            layerName = element.ID;
        }

        if (string.IsNullOrEmpty(layerName) && element.CustomAttributes != null)
        {
            // get custom title attributes.
            foreach (string attr in TitleAttributes)
            {
                if (element.CustomAttributes.TryGetValue(attr, out string? title) && !string.IsNullOrEmpty(title))
                {
                    layerName = title;
                    break;
                }
            }
        }

        if (string.IsNullOrEmpty(layerName) && element.Children != null)
        {
            // Get child title tag
            SvgTitle? title = element.Children.OfType<SvgTitle>().FirstOrDefault();
            if (!string.IsNullOrEmpty(title?.Content))
            {
                layerName = title.Content;
            }
        }

        if (string.IsNullOrEmpty(layerName))
        {
            // Generate more meanfull name for a svg use node. Add reference element name in a case if it's local document.
            if (element is SvgUse use
                && use.ReferencedElement != null
                && !string.IsNullOrEmpty(use.ReferencedElement.OriginalString))
            {
                string str = use.ReferencedElement.OriginalString.Trim();
                if (str.StartsWith('#'))
                {
                    layerName = str[1..];
                }
                prependElementName = true;
            }
            // Generate more meanfull name for a svg text.
            else if (element is SvgTextBase text && !string.IsNullOrEmpty(text.Text))
            {
                layerName = Truncate(text.Text);
            }
            else if (element is SvgPath path && path.PathData.Count > 0)
            {
                layerName = Truncate(path.PathData.ToString());
            }
        }
        return layerName == null ? elementName : prependElementName ? string.Join(": ", elementName, layerName) : layerName;

        static string Truncate(string s, int maxLength = 32)
        {
            return s.Length > maxLength ? s[..maxLength] + "..." : s;
        }
    }

    protected static Document GetDocument(IEnumerable<BitmapLayer> layers, SvgImportConfig config)
    {
        ArgumentNullException.ThrowIfNull(layers);
        ArgumentNullException.ThrowIfNull(config);

        Document document = new Document(config.Width, config.Height);
        try
        {
            foreach (BitmapLayer layer in layers)
            {
                document.Layers.Add(layer);
            }
            document.SetDpi(config.Ppi);
            return document;
        }
        catch (Exception)
        {
            document.Dispose();
            throw;
        }
    }

    #endregion

    #region GroupBoundary

    /* a private type to determine boundaries of a group. */
    protected sealed class GroupBoundary(SvgGroup group, string name, bool isStart) : SvgVisualElement
    {
        private readonly SvgGroup group = group;

        public override bool Visible => group.Visible;

        public override float Opacity => group.Opacity;

        public bool IsStart { get; } = isStart;

        public string Name { get; } = name;

        public override SvgDocument OwnerDocument => throw new NotImplementedException();

        public override RectangleF Bounds => throw new NotImplementedException();

        public bool GetOriginalVisibility()
        {
            return group.IsOriginallyVisible();
        }

        public override SvgElement DeepCopy()
        {
            throw new NotImplementedException();
        }

        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}
