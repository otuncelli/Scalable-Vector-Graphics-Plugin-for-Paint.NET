// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

namespace SvgFileTypePlugin.Import;

internal sealed class SvgImportConfig
{
    public int Width { get; set; }

    public int Height { get; set; }

    public bool PreserveAspectRatio { get; set; }

    public int Ppi { get; set; }

    public LayersMode LayersMode { get; set; }

    public bool GroupBoundariesAsLayers { get; set; }

    public bool RespectElementOpacity { get; set; }

    public bool ImportHiddenElements { get; set; }
}
