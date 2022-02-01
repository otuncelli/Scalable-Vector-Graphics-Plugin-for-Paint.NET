// Copyright 2023 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Threading;
using PaintDotNet;

namespace SvgFileTypePlugin;

internal static class Services
{
    private static IServiceProvider Provider;

    public static void Init(IServiceProvider provider)
    {
        _ = Interlocked.Exchange(ref Provider, provider);
    }

    public static TService Get<TService>() where TService : class
    {
        IServiceProvider provider = Interlocked.CompareExchange(ref Provider, null, null);
        return provider.GetService<TService>();
    }
}
