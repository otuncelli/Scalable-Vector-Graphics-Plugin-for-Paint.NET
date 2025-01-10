// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Reflection;
using PaintDotNet;

namespace SvgFileTypePlugin;

public sealed class MyPluginSupportInfo : IPluginSupportInfo, IPluginSupportInfoProvider
{
    #region Properties

    internal static MyPluginSupportInfo Instance { get; } = new MyPluginSupportInfo();

    #region IPluginSupportInfo

    public string Author { get; } = "Osman Tunçelli";

    public string Copyright { get; } = GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;

    public string DisplayName { get; } = GetCustomAttribute<AssemblyProductAttribute>().Product;

    public Version Version { get; } = GetAssembly().GetName().Version;

    public Uri WebsiteUri { get; } = new Uri("https://github.com/otuncelli/Scalable-Vector-Graphics-Plugin-for-Paint.NET");

    #endregion

    internal Uri ForumUri { get; } = new Uri("https://forums.getpaint.net/index.php?showtopic=117086");

    #endregion

    #region IPluginSupportInfoProvider

    public IPluginSupportInfo GetPluginSupportInfo()
    {
        return new MyPluginSupportInfo();
    }

    #endregion

    #region Static Methods

    private static T GetCustomAttribute<T>() where T : Attribute
    {
        return GetAssembly().GetCustomAttribute<T>();
    }

    private static Assembly GetAssembly()
    {
        return typeof(MyPluginSupportInfo).Assembly;
    }

    #endregion
}
