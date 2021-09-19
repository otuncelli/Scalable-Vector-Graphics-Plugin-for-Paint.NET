// Copyright 2021 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by a LGPL license that can be
// found in the COPYING file.

using PaintDotNet;
using System;
using System.IO;

namespace SvgFileTypePlugin
{
    [PluginSupportInfo(typeof(MyPluginSupportInfo))]
    public sealed class SvgFileType : FileType, IFileTypeFactory
    {
        private static readonly FileTypeOptions options = new FileTypeOptions
        {
            LoadExtensions = new[] { ".svg", ".svgz" },
            SupportsCancellation = true,
            SupportsLayers = true
        };

        public SvgFileType() : base("Scalable Vector Graphics", options)
        {
        }

        protected override Document OnLoad(Stream input) 
            => new SvgImporter().Import(input);

        protected override void OnSave(Document input, Stream output, SaveConfigToken token, Surface scratchSurface, ProgressEventHandler callback) 
            => throw new NotSupportedException();

        #region IFileTypeFactory
        public FileType[] GetFileTypeInstances() => new[] { new SvgFileType() };
        #endregion
    }
}