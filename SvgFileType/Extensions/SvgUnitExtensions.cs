// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using Svg;

namespace SvgFileTypePlugin.Extensions;

internal static class SvgUnitExtensions
{
    public static int ToDeviceValue(this SvgUnit unit, SvgElement owner, UnitRenderingType unitRenderingType = UnitRenderingType.Other)
    {
        ArgumentNullException.ThrowIfNull(owner);

        using ISvgRenderer renderer = SvgRenderer.FromNull();
        float value = unit.ToDeviceValue(renderer, unitRenderingType, owner);
        return checked((int)Math.Round(value));
    }
}
