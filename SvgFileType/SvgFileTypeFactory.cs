// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using PaintDotNet;

namespace SvgFileTypePlugin;

public sealed class SvgFileTypeFactory : IFileTypeFactory2
{
    public FileType[] GetFileTypeInstances(IFileTypeHost host)
    {
        ArgumentNullException.ThrowIfNull(host);

        Services.Init(host.Services);
        return [new SvgFileType()];
    }
}
