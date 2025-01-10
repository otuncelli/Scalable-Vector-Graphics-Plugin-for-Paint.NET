// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Runtime;

namespace SvgFileTypePlugin.Import;

internal static class Utils
{
    public static IDisposable UseMemoryFailPoint(int width, int height, int count)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(width, 0);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(height, 0);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(count, 0);

        return UseMemoryFailPoint(height * 4L * width * count);
    }

    public static IDisposable UseMemoryFailPoint(int width, int height)
    {
        return UseMemoryFailPoint(width, height, 1);
    }

    public static void EnsureMemoryAvailable(int width, int height, int count)
    {
        using (UseMemoryFailPoint(width, height, count))
        {
        }
    }

    public static IDisposable UseMemoryFailPoint(long bytes)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(bytes, 0);

        int mb = checked((int)Math.Max(bytes / (1024 * 1024), 1));
        return new MemoryFailPoint(mb);
    }
}
