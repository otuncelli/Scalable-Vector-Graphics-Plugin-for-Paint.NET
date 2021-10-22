// Copyright 2021 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by a LGPL license that can be
// found in the COPYING file.

using PaintDotNet;
using System;
using System.Reflection;

namespace SvgFileTypePlugin
{
    public sealed class MyPluginSupportInfo : IPluginSupportInfo, IPluginSupportInfoProvider
    {
        internal static readonly MyPluginSupportInfo Instance = new MyPluginSupportInfo();
        internal const string Url = "https://github.com/otuncelli/Scalable-Vector-Graphics-Plugin-for-Paint.NET";
        internal const string ForumUrl = "https://forums.getpaint.net/topic/117086-scalable-vector-graphics-filetype-alternative-plugin-svg-svgz/";

        #region IPluginSupportInfo

        public string Author => "Osman Tunçelli";
        public string Copyright => GetType().Assembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
        public string DisplayName => GetType().Assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;
        public Version Version => GetType().Assembly.GetName().Version;
        public Uri WebsiteUri => String.IsNullOrEmpty(Url) ? null : new Uri(Url);

        #endregion

        #region IPluginSupportInfoProvider

        public IPluginSupportInfo GetPluginSupportInfo() => new MyPluginSupportInfo();

        #endregion
    }
}