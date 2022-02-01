// Copyright 2023 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

namespace SvgFileTypePlugin.Import;

internal static class SvgConverterFactory
{
    public static ISvgConverter Get()
    {
#if RESVG
        return ResvgConverter.Instance;
#elif SKIA
        return SkiaSvg2Document.Instance;
#else
        return Direct2DSvgConverter.Instance; // DefaultSvgConverter.Instance;
#endif
    }
}
