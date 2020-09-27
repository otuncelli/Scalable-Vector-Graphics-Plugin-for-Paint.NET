using Svg;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SvgFileTypePlugin
{
    // Used to determine boundaries of a group.
    internal class PaintGroupBoundaries : SvgVisualElement
    {
        public PaintGroupBoundaries(SvgGroup relatedGroup)
        {
            RelatedGroup = relatedGroup;
        }

        public SvgGroup RelatedGroup { get; }

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
