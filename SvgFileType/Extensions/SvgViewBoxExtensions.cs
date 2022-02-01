// Copyright 2023 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System.Drawing;
using Svg;

namespace SvgFileTypePlugin.Extensions;

internal static class SvgViewBoxExtensions
{
    public static Rectangle ToRectangle(this SvgViewBox vb) => Rectangle.Round(vb);

    public static bool IsEmpty(this SvgViewBox vb) => vb.Width < float.Epsilon || vb.Height < float.Epsilon;
}
