// Copyright 2023 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using PaintDotNet;

namespace SvgFileTypePlugin.Extensions;

internal static class DocumentExtensions
{
    public static void SetDpi(this Document doc, double dpi)
    {
        doc.DpuUnit = MeasurementUnit.Inch;
        doc.DpuX = dpi;
        doc.DpuY = dpi;
    }
}
