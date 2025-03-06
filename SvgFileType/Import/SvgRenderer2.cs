// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime;
using System.Threading;
using PaintDotNet;
using Svg;
using SvgFileTypePlugin.Extensions;
using SR = SvgFileTypePlugin.Localization.StringResources;

namespace SvgFileTypePlugin.Import;

internal abstract class SvgRenderer2(string name)
{
    private static readonly FrozenSet<string> TitleAttributes = ["label", "title", "inkscape:label"];
    private int step, total;

    public string Name { get; } = name;

    public event ProgressChangedEventHandler? ProgressChanged;

    #region Public

    public Document Rasterize(string svgdata, SvgImportConfig config, CancellationToken ctoken = default)
    {
        ArgumentNullException.ThrowIfNull(config);

        Logger.WriteLine($"Using '{Name}' renderer with '{config.LayersMode}' layers mode.");
        return config.LayersMode == LayersMode.Flat
            ? GetFlatDocument(svgdata, config, ctoken)
            : GetLayeredDocument(svgdata, config, ctoken);
    }

    #endregion

    #region Public Static

    public static SvgRenderer2 Create(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        return name.ToLowerInvariant() switch
        {
            "resvg" => new ResvgSvgRenderer(),
            "gdip" or "gdiplus" or "gdi+" => new GdipSvgRenderer(),
            "direct2d" or "d2d" => new Direct2DSvgRenderer(),
            _ => throw new ArgumentOutOfRangeException(nameof(name), $"Unknown SVG renderer: {name}")
        };
    }

    #endregion

    #region Protected

    protected abstract Document GetFlatDocument(string svgdata, SvgImportConfig config, CancellationToken ctoken = default);

    protected abstract Document GetLayeredDocument(string svgdata, SvgImportConfig config, CancellationToken ctoken = default);

    protected virtual void RenderSvgUseElement(SvgUse useElement, Action<SvgElement> renderAction)
    {
        ArgumentNullException.ThrowIfNull(useElement);
        ArgumentNullException.ThrowIfNull(renderAction);

        if (useElement.GetCopyOfReferencedElement() is not SvgElement referencedElement)
        {
            return;
        }
        referencedElement.Visibility = "visible";
        useElement.Visibility = "hidden";
        SvgElementCollection children = useElement.Parent.Children;
        children.AddAndForceUniqueID(referencedElement);
        try
        {
            renderAction(referencedElement);
        }
        finally
        {
            children.Remove(referencedElement);
        }
    }

    public virtual Document GetNoPathDocument()
    {
        Graphics g;
        Size size;
        using Font font = new Font("Arial", 24, FontStyle.Bold);
        using (g = Graphics.FromHwnd(nint.Zero))
        {
            size = g.MeasureString(SR.NoPath, font).ToSize();
        }
        using Bitmap bmp = new Bitmap(size.Width, size.Height, PixelFormat.Format24bppRgb);
        g = Graphics.FromImage(bmp);
        using var _ = g;
        g.Clear(Color.LightGray);
        Rectangle layoutRectangle = new Rectangle(Point.Empty, bmp.Size);
        using StringFormat format = new StringFormat()
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };
        g.DrawString(SR.NoPath, font, Brushes.Black, layoutRectangle, format);
        return Document.FromImage(bmp);
    }

    protected void IncrementProgress()
    {
        if (ProgressChanged is null)
        {
            return;
        }

        int value = Interlocked.Increment(ref step);
        value = Math.Clamp(value, 0, total);
        ProgressChangedEventArgs args = new ProgressChangedEventArgs(step, total);
        ProgressChanged.Invoke(this, args);
    }

    protected void ResetProgress(int total)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(total);

        this.total = total;
        int value = Interlocked.Exchange(ref step, 0);
        if (ProgressChanged is not null)
        {
            ProgressChangedEventArgs args = new ProgressChangedEventArgs(step, total);
            ProgressChanged?.Invoke(this, args);
        }
    }

    #endregion

    #region Protected Static

    protected static List<SvgVisualElement> GetSvgVisualElements(SvgDocument svg, SvgImportConfig config, CancellationToken ctoken = default)
    {
        ArgumentNullException.ThrowIfNull(svg);
        ArgumentNullException.ThrowIfNull(config);

        IEnumerable<SvgVisualElement> elements = GetPreparedElements(svg.Children, ctoken);

        if (config.LayersMode == LayersMode.Groups)
        {
            elements = FilterByGroups(elements, ctoken);
        }
        else
        {
            elements = FilterByNonGroups(elements);
        }

        if (config.LayersMode == LayersMode.Flat || !config.GroupBoundariesAsLayers)
        {
            elements = elements.NotOfType<GroupBoundary>();
        }

        if (config.LayersMode != LayersMode.Flat)
        {
            // Skip group boundaries for hidden layers.
            elements = elements.Where(e => config.ImportHiddenElements || (e is GroupBoundary groupBoundary
                ? groupBoundary.IsOriginallyVisible
                : e.IsOriginallyVisible()));
        }

        return elements.ToList();
    }

    protected static string GetLayerTitle(SvgElement element, bool prependElementName = true)
    {
        ArgumentNullException.ThrowIfNull(element);

        string elementName = element.GetName();
        string? layerName = null;

        if (element.ID is not null)
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
                    // found a title candidate
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
            if (element is SvgUse useElement and { ReferencedElement.OriginalString.Length: > 1 })
            {
                string str = useElement.ReferencedElement.OriginalString.Trim();
                if (str.StartsWith('#'))
                {
                    layerName = str[1..];
                }
                prependElementName = true;
            }
            // Generate more meanfull name for a svg text.
            else if (element is SvgTextBase text and { Text.Length: > 0})
            {
                layerName = text.Text.Truncate(maxLength: 32);
            }
            // Generate more meanfull name for a svg path.
            else if (element is SvgPath path and { PathData.Count: > 0 })
            {
                layerName = path.PathData.ToString().Truncate(maxLength: 32);
            }
        }

        return layerName is null
            ? elementName
            : prependElementName
            ? $"{elementName}: {layerName}"
            : layerName;
    }

    /// <summary>
    /// Generates a Paint.NET document with given layers and configuration.
    /// </summary>
    /// <param name="layers"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    protected static Document GetDocument(List<BitmapLayer> layers, SvgImportConfig config)
    {
        ArgumentNullException.ThrowIfNull(layers);
        ArgumentNullException.ThrowIfNull(config);
        if (layers.Count == 0)
        {
            throw new ArgumentException("There aren't any layers in the collection.", nameof(layers));
        }

        Debug.Assert(layers[0].Width == config.RasterWidth);
        Debug.Assert(layers[0].Height == config.RasterHeight);

        Document document = new Document(config.RasterWidth, config.RasterHeight);
        try
        {
            layers.ForEach(document.Layers.Add);
            document.SetDpi(config.Ppi);
            return document;
        }
        catch (Exception)
        {
            document.Dispose();
            layers.ForEach(layer => layer.Dispose());
            throw;
        }
    }

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    protected static byte ToByteOpacity(float opacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(opacity);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(opacity, 1f);

        int value = (int)MathF.Round(opacity * 255f);
        value = Math.Clamp(value, byte.MinValue, byte.MaxValue);
        return (byte)value;
    }

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="InsufficientMemoryException"></exception>
    protected static MemoryFailPoint GetMemoryFailPoint(int width, int height, int count = 1)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count);

        int sizeInMegabytes = checked((int)(height * 4L * width * count / (1024 * 1024)));
        sizeInMegabytes = Math.Max(sizeInMegabytes, 1);
        return new MemoryFailPoint(sizeInMegabytes);
    }

    #endregion

    #region Private Static

    private static IEnumerable<SvgVisualElement> GetPreparedElements(IEnumerable<SvgElement> elements, CancellationToken ctoken = default)
    {
        return GetPreparedElements(elements, groupName: null, ctoken);
    }

    private static IEnumerable<SvgVisualElement> GetPreparedElements(IEnumerable<SvgElement> elements, string? groupName, CancellationToken ctoken = default)
    {
        // Prepare a collection of elements that about to be rendered. 
        // Don't prepare for a separate parsing def lists.
        foreach (SvgVisualElement visual in elements.OfType<SvgVisualElement>())
        {
            ctoken.ThrowIfCancellationRequested();

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
                visual.SetGroupName(groupName); // Store group info
            }

            SvgGroup? group = visual as SvgGroup;
            if (group is not null)
            {
                groupName = GetLayerTitle(group, prependElementName: false);

                // Return fake node to indicate group end. (order is reversed)
                yield return new GroupBoundary(group, groupName, isStart: false);
            }

            if (GetPreparedElements(visual.Children, groupName, ctoken) is IEnumerable<SvgVisualElement> preparedElements)
            {
                foreach (SvgVisualElement element in preparedElements)
                {
                    yield return element;
                }
            }

            if (group is not null)
            {
                Debug.Assert(groupName is not null);

                // Return fake node to indicate group start.
                yield return new GroupBoundary(group, groupName, isStart: true);
            }

            // Skip text with empty content. But keep all children nodes.
            if (visual is SvgTextBase textNode and { Text.Length: > 0 })
            {
                continue;
            }

            yield return visual;
        }
    }

    private static IEnumerable<SvgVisualElement> FilterByNonGroups(IEnumerable<SvgVisualElement> elements)
    {
        return elements.NotOfType<SvgGroup>();
    }

    private static IEnumerable<SvgVisualElement> FilterByGroups(IEnumerable<SvgVisualElement> elements, CancellationToken ctoken = default)
    {
        HashSet<SvgVisualElement> groups = [];
        foreach (SvgVisualElement element in elements.NotOfType<GroupBoundary>())
        {
            ctoken.ThrowIfCancellationRequested();

            SvgVisualElement? visual = null;
            if (element.GetGroupName() is not null)
            {
                // Get only root level
                for (SvgElement parent = element; parent is not null; parent = parent.Parent)
                {
                    // TODO: render more groups. In most cases svg has only few root groups.
                    if (parent is SvgGroup group)
                    {
                        visual = group;
                    }
                }
            }

            visual ??= element;
            if (groups.Add(visual))
            {
                yield return visual;
            }
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

        public bool IsOriginallyVisible => group.IsOriginallyVisible();

        public string Name { get; } = string.Format(isStart ? SR.LayerGroup : SR.EndLayerGroup, name);

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

    #endregion
}
