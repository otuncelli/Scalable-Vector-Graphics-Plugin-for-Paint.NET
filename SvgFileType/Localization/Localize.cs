// Copyright 2023 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Text.RegularExpressions;
using SR = SvgFileTypePlugin.Localization.StringResources;

namespace SvgFileTypePlugin.Localization;

internal static class Localize
{
    public static string GetDisplayName<TEnum>(TEnum @enum) where TEnum : struct, Enum
    {
        string name = Enum.GetName(@enum);
        string key = String.Concat(typeof(TEnum).Name, name);
        return SR.ResourceManager.GetString(key) ?? SplitCamelCase(name);
    }

    private static string SplitCamelCase(string str) => Regex.Replace(str, "(\\B[A-Z])", " $1");
}
