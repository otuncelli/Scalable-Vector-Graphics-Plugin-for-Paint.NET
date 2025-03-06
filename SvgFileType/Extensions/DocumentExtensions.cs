// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using PaintDotNet;

namespace SvgFileTypePlugin.Extensions;

internal static class DocumentExtensions
{
    public static void SetDpi(this Document doc, double dpuX, double dpuY, MeasurementUnit dpuUnit = MeasurementUnit.Inch)
    {
        ArgumentNullException.ThrowIfNull(doc);

        doc.DpuUnit = dpuUnit;
        doc.DpuX = dpuX;
        doc.DpuY = dpuY;
    }

    public static void SetDpi(this Document doc, double dpu, MeasurementUnit dpuUnit = MeasurementUnit.Inch)
    {
        SetDpi(doc, dpu, dpu, dpuUnit);
    }
}
