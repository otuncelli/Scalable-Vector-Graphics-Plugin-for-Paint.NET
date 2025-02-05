// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System.Diagnostics;

namespace SvgFileTypePlugin;

internal static class Logger
{
    private const string Category = $"[{nameof(SvgFileTypePlugin)}]";

    public static void WriteLine(string message)
    {
        Trace.WriteLine(message, Category);
    }

    public static void WriteLineIf(bool condition, string message)
    {
        Trace.WriteLineIf(condition, message, Category);
    }
}
