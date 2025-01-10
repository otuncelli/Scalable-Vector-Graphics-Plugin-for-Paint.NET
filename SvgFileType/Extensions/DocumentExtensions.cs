// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using PaintDotNet;

namespace SvgFileTypePlugin.Extensions;

internal static class DocumentExtensions
{
    public static void SetDpi(this Document doc, double x, double y)
    {
        ArgumentNullException.ThrowIfNull(doc);

        doc.DpuUnit = MeasurementUnit.Inch;
        doc.DpuX = x;
        doc.DpuY = y;
    }

    public static void SetDpi(this Document doc, double dpi)
    {
        SetDpi(doc, dpi, dpi);
    }
}
