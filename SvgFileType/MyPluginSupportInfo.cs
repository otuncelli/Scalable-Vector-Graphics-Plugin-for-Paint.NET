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
        public const string VersionString = "1.0.4.0";
        public const string Url = "https://github.com/otuncelli/Scalable-Vector-Graphics-Plugin-for-Paint.NET";

        #region IPluginSupportInfo
        public string Author => "Osman Tunçelli";
        public string Copyright => ((AssemblyCopyrightAttribute)(typeof(PluginSupportInfo).Assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0])).Copyright;
        public string DisplayName => ((AssemblyProductAttribute)GetType().Assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), inherit: false)[0]).Product;
        public Version Version => typeof(PluginSupportInfo).Assembly.GetName().Version;
        public Uri WebsiteUri => new Uri(Url);
        #endregion

        #region IPluginSupportInfoProvider
        public IPluginSupportInfo GetPluginSupportInfo()
        {
            return new MyPluginSupportInfo();
        }
        #endregion
    }
}