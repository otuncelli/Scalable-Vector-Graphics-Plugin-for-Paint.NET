// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

namespace SvgFileTypePlugin.Import;

internal static class SvgConverterFactory
{
    public static ISvgConverter Get()
    {
#if RESVG
        return ResvgConverter.Instance;
#elif DIRECT2D
        return D2DSvgConverter.Instance;
#else
        return DefaultSvgConverter.Instance;
#endif
    }
}
