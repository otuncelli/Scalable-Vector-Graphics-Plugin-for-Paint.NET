// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using PaintDotNet;

namespace SvgFileTypePlugin.Extensions;

internal static class DocumentExtensions
{
    public static void SetDpi(this Document doc, double x, double y, MeasurementUnit dpuUnit = MeasurementUnit.Inch)
    {
        ArgumentNullException.ThrowIfNull(doc);

        doc.DpuUnit = dpuUnit;
        doc.DpuX = x;
        doc.DpuY = y;
    }

    public static void SetDpi(this Document doc, double dpi, MeasurementUnit dpuUnit = MeasurementUnit.Inch)
    {
        SetDpi(doc, dpi, dpi, dpuUnit);
    }
}
