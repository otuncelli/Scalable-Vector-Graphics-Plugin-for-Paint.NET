// Copyright 2021 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by a LGPL license that can be
// found in the COPYING file.

namespace SvgFileTypePlugin.Import
{
    internal sealed class SvgImportConfig
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool KeepAspectRatio { get; set; }
        public int Ppi { get; set; }
        public LayersMode LayersMode { get; set; }
        public bool GroupBoundariesAsLayers { get; set; }
        public bool RespectElementOpacity { get; set; }
        public bool HiddenElements { get; set; }
    }
}
