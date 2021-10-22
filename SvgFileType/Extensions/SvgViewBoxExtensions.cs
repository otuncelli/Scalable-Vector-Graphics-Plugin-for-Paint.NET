// Copyright 2021 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by a LGPL license that can be
// found in the COPYING file.

using Svg;
using System.Drawing;

namespace SvgFileTypePlugin
{
    internal static class SvgViewBoxExtensions
    {
        public static Rectangle ToRectangle(this SvgViewBox vb) => Rectangle.Round(vb);

        public static bool IsEmpty(this SvgViewBox vb) => ToRectangle(vb).IsEmpty;
    }
}