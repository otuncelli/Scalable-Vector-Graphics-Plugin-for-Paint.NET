// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using PaintDotNet;

namespace SvgFileTypePlugin.Extensions;

internal static class SurfaceExtensions
{
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

    public static unsafe bool IsEmpty(this Surface surface)
    {
        ArgumentNullException.ThrowIfNull(surface);

        const int unrollFactor = 4;
        int pixcnt = surface.Width * surface.Height;
        sbyte* ptr = (sbyte*)surface.Scan0.VoidStar;

        if (Avx2.IsSupported && pixcnt >= Vector256<uint>.Count)
        {
            Vector256<sbyte> valphamask = Vector256.Create(
                3, 7, 11, 15, 19, 23, 27, 31,
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff).AsSByte();
            Vector256<sbyte> zero = Vector256<sbyte>.Zero;
            while (pixcnt >= Vector256<uint>.Count * unrollFactor)
            {
                Vector256<sbyte> v0 = Avx.LoadVector256(ptr);
                Vector256<sbyte> v1 = Avx.LoadVector256(ptr + Vector256<sbyte>.Count);
                Vector256<sbyte> v2 = Avx.LoadVector256(ptr + 2 * Vector256<sbyte>.Count);
                Vector256<sbyte> v3 = Avx.LoadVector256(ptr + 3 * Vector256<sbyte>.Count);

                v0 = Avx2.Shuffle(v0, valphamask);
                v1 = Avx2.Shuffle(v1, valphamask);
                v2 = Avx2.Shuffle(v2, valphamask);
                v3 = Avx2.Shuffle(v3, valphamask);

                v0 = Avx2.CompareGreaterThan(v0, zero);
                v1 = Avx2.CompareGreaterThan(v1, zero);
                v2 = Avx2.CompareGreaterThan(v2, zero);
                v3 = Avx2.CompareGreaterThan(v3, zero);

                int mask = Avx2.MoveMask(v0) | Avx2.MoveMask(v1) | Avx2.MoveMask(v2) | Avx2.MoveMask(v3);
                if (mask != 0)
                {
                    return false;
                }

                ptr += Vector256<sbyte>.Count * unrollFactor;
                pixcnt -= Vector256<uint>.Count * unrollFactor;
            }

            while (pixcnt >= Vector256<sbyte>.Count)
            {
                Vector256<sbyte> v = Avx.LoadVector256(ptr);
                v = Avx2.Shuffle(v, valphamask);
                v = Avx2.CompareGreaterThan(v, zero);
                if (Avx2.MoveMask(v) != 0)
                {
                    return false;
                }
                ptr += Vector256<sbyte>.Count;
                pixcnt -= Vector256<uint>.Count;
            }
        }

        if (Ssse3.IsSupported && pixcnt >= Vector128<uint>.Count)
        {
            Vector128<sbyte> valphamask = Vector128.Create(
                3, 7, 11, 15,
                0xff, 0xff, 0xff, 0xff,
                0xff, 0xff, 0xff, 0xff,
                0xff, 0xff, 0xff, 0xff).AsSByte();
            Vector128<sbyte> zero = Vector128<sbyte>.Zero;
            while (pixcnt >= Vector128<uint>.Count * unrollFactor)
            {
                Vector128<sbyte> v0 = Sse2.LoadVector128(ptr);
                Vector128<sbyte> v1 = Sse2.LoadVector128(ptr + Vector128<sbyte>.Count);
                Vector128<sbyte> v2 = Sse2.LoadVector128(ptr + 2 * Vector128<sbyte>.Count);
                Vector128<sbyte> v3 = Sse2.LoadVector128(ptr + 3 * Vector128<sbyte>.Count);

                v0 = Ssse3.Shuffle(v0, valphamask);
                v1 = Ssse3.Shuffle(v1, valphamask);
                v2 = Ssse3.Shuffle(v2, valphamask);
                v3 = Ssse3.Shuffle(v3, valphamask);

                v0 = Sse2.CompareGreaterThan(v0, zero);
                v1 = Sse2.CompareGreaterThan(v1, zero);
                v2 = Sse2.CompareGreaterThan(v2, zero);
                v3 = Sse2.CompareGreaterThan(v3, zero);

                int mask = Sse2.MoveMask(v0) | Sse2.MoveMask(v1) | Sse2.MoveMask(v2) | Sse2.MoveMask(v3);
                if (mask != 0)
                {
                    return false;
                }

                ptr += Vector128<sbyte>.Count * unrollFactor;
                pixcnt -= Vector128<uint>.Count * unrollFactor;
            }

            while (pixcnt >= Vector128<sbyte>.Count)
            {
                Vector128<sbyte> v = Sse2.LoadVector128(ptr);
                v = Ssse3.Shuffle(v, valphamask);
                v = Sse2.CompareGreaterThan(v, zero);
                if (Sse2.MoveMask(v) != 0)
                {
                    return false;
                }
                ptr += Vector128<sbyte>.Count;
                pixcnt -= Vector128<uint>.Count;
            }
        }

        while (pixcnt > 0)
        {
            if (ptr[3] > 0)
            {
                return false;
            }
            ptr += sizeof(uint);
            pixcnt--;
        }

        return true;
    }

    public static void BlendOnto<T>(this Surface surface, ColorBgra backgroundColor) where T : UserBlendOp, new()
    {
        ArgumentNullException.ThrowIfNull(surface);

        using Surface tmp = surface.Clone();
        surface.Fill(backgroundColor);
        T blendOp = new T();
        blendOp.Apply(surface, tmp);
    }
}
