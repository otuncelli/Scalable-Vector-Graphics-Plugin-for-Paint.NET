// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Runtime.CompilerServices;
using PaintDotNet;

namespace SvgFileTypePlugin.Extensions;

internal static class SurfaceExtensions
{
    /// <exception cref="ArgumentNullException"></exception>
    public static Document CreateSingleLayerDocument(this Surface surface, bool takeOwnership = false)
    {
        ArgumentNullException.ThrowIfNull(surface);

        Document document = new Document(surface.Width, surface.Height);
        try
        {
            BitmapLayer layer = new BitmapLayer(surface, takeOwnership);
            document.Layers.Add(layer);
        }
        catch
        {
            document.Dispose();
            throw;
        }
        return document;
    }

    public static bool IsEmpty(this Surface surface)
    {
        ArgumentNullException.ThrowIfNull(surface);

        for (int y = 0; y < surface.Height; y++)
        {
            ref ColorBgra pix = ref surface.GetRowReferenceUnchecked(y);
            for (int x = surface.Width; x > 0; x--)
            {
                if (pix.A > 0)
                    return false;
                pix = ref Unsafe.Add(ref pix, 1);
            }
        }
        return true;
    }

    public static void BlendOnto<T>(this Surface surface, ColorBgra backgroundColor) where T : UserBlendOp, new()
    {
        ArgumentNullException.ThrowIfNull(surface);

        using Surface tmp = surface.Clone();
        int w = tmp.Width;
        int h = tmp.Height;
        int stride = tmp.Stride;
        surface.Fill(backgroundColor);
        new T().Apply(surface, tmp);
    }
}
