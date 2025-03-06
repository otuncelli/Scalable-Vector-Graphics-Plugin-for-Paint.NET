// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

namespace SvgFileTypePlugin.Import;

internal sealed class SvgImportConfig
{
    public required int RasterWidth { get; init; }

    public required int RasterHeight { get; init; }

    public bool PreserveAspectRatio { get; set; } = true;

    public int Ppi { get; set; } = 96;

    public LayersMode LayersMode { get; set; } = LayersMode.Flat;

    public bool GroupBoundariesAsLayers { get; set; }

    public bool RespectElementOpacity { get; set; }

    public bool ImportHiddenElements { get; set; }
}
