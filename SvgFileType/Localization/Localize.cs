// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Text.RegularExpressions;
using SR = SvgFileTypePlugin.Localization.StringResources;

namespace SvgFileTypePlugin.Localization;

internal static partial class Localize
{
    public static string GetDisplayName<TEnum>(TEnum @enum) where TEnum : struct, Enum
    {
        string name = Enum.GetName(@enum) ?? throw new ArgumentException("Unknown enum constant.", nameof(@enum));
        string key = string.Concat(typeof(TEnum).Name, name);
        return SR.ResourceManager.GetString(key) ?? SplitCamelCase(name);
    }

    private static string SplitCamelCase(string str)
    {
        return CamelCaseRegex().Replace(str, " $1");
    }

    [GeneratedRegex("(\\B[A-Z])")]
    private static partial Regex CamelCaseRegex();
}
