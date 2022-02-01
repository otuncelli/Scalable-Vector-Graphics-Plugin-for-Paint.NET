// Copyright 2023 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System.Runtime.CompilerServices;
using PaintDotNet;

namespace SvgFileTypePlugin.Extensions
{
    internal static class SurfaceExtensions
    {
        public static Document CreateDocument(this Surface surface, bool takeOwnership = false)
        {
            Document document = new Document(surface.Width, surface.Height);
            document.Layers.Add(new BitmapLayer(surface, takeOwnership));
            return document;
        }

        public static void ConvertFromRgba(this Surface surface)
        {
            for (int y = 0; y < surface.Height; y++)
            {
                ref ColorBgra pix = ref surface.GetRowReferenceUnchecked(y);
                for (int x = surface.Width; x > 0; x--)
                {
                    (pix.R, pix.B) = (pix.B, pix.R);
                    pix = ref Unsafe.Add(ref pix, 1);
                }
            }
        }

        public static bool IsEmpty(this Surface surface)
        {
            for (int y = 0; y < surface.Height; y++)
            {
                ref ColorBgra pix = ref surface.GetRowReferenceUnchecked(y);
                for (int x = surface.Width; x > 0; x--)
                {
                    if (pix.A > 0) { return false; }
                    pix = ref Unsafe.Add(ref pix, 1);
                }
            }
            return true;
        }
    }
}
