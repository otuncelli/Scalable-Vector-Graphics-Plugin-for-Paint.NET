// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;

namespace SvgFileTypePlugin.Extensions;

internal static class StringExtensions
{
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static string Truncate(this string s, int maxLength, string suffix = "...")
    {
        ArgumentNullException.ThrowIfNull(s);
        ArgumentOutOfRangeException.ThrowIfNegative(maxLength);

        return s.Length > maxLength ? $"{s[..maxLength]}{suffix}" : s;
    }
}
