// Copyright 2021 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by a LGPL license that can be
// found in the COPYING file.

using Svg;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SvgFileTypePlugin.Import
{
    /// <summary>
    /// Used to determine boundaries of a group.
    /// </summary>
    internal sealed class PaintGroupBoundaries : SvgVisualElement
    {
        public PaintGroupBoundaries(SvgGroup relatedGroup, bool isStart)
        {
            RelatedGroup = relatedGroup;
            IsStart = isStart;
        }

        public SvgGroup RelatedGroup { get; }

        public bool IsStart { get; }

        public override RectangleF Bounds => throw new NotImplementedException();
        public override SvgElement DeepCopy() => throw new NotImplementedException();
        public override GraphicsPath Path(ISvgRenderer renderer) => throw new NotImplementedException();
    }
}