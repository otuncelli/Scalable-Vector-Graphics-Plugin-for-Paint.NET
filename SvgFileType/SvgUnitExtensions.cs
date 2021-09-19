// Copyright 2021 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by a LGPL license that can be
// found in the COPYING file.

using Svg;
using System;

namespace SvgFileTypePlugin
{
    internal static class SvgUnitExtensions
    {
        public static int ToPixels(this SvgUnit unit, SvgElement owner)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            using (var renderer = SvgRenderer.FromNull())
            {
                return (int)Math.Round(unit.ToDeviceValue(renderer, UnitRenderingType.Other, owner));
            }
        }
    }
}