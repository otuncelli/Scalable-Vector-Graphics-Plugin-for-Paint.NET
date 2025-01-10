// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using PaintDotNet;

namespace SvgFileTypePlugin;

internal static class Services
{
    private static IServiceProvider? Provider;

    public static void Init(IServiceProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);

        Provider = provider;
    }

    public static TService Get<TService>() where TService : class
    {
        if (Provider is null)
            throw new InvalidOperationException("No service provider has been configured. Call Init(IServiceProvider) first.");

        TService? service = Provider.GetService<TService>();
        return service ?? throw new InvalidOperationException($"Cannot find the service: `{typeof(TService)}`");
    }
}
