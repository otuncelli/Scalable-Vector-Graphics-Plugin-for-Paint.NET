// Copyright 2023 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using PaintDotNet;

namespace SvgFileTypePlugin;

public sealed class SvgFileTypeFactory : IFileTypeFactory2
{
    public FileType[] GetFileTypeInstances(IFileTypeHost host)
    {
        Services.Init(host!.Services);
        return new FileType[] { new SvgFileType() };
    }
}
