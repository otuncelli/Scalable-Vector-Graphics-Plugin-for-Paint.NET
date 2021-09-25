// Copyright 2021 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by a LGPL license that can be
// found in the COPYING file.

using PaintDotNet;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SvgFileTypePlugin
{
    public sealed class MyPluginSupportInfo : IPluginSupportInfo, IPluginSupportInfoProvider
    {
        private static readonly Assembly CurrentAssembly = typeof(MyPluginSupportInfo).Assembly;
        internal static readonly MyPluginSupportInfo Instance = new MyPluginSupportInfo();
        public const string VersionString = "1.0.4.0";
        public const string Url = "https://github.com/otuncelli/Scalable-Vector-Graphics-Plugin-for-Paint.NET";

        private static string GetAssemblyAttributeValue<T>(Expression<Func<T, string>> expr) where T : Attribute
        {
            var attr = CurrentAssembly.GetCustomAttribute(typeof(T));
            var prop = ((MemberExpression)expr.Body).Member as PropertyInfo;
            return prop?.GetGetMethod().Invoke(attr, null) as string;
        }

        #region IPluginSupportInfo
        public string Author => "Osman Tunçelli";
        public string Copyright => GetAssemblyAttributeValue<AssemblyCopyrightAttribute>(a => a.Copyright);
        public string DisplayName => GetAssemblyAttributeValue<AssemblyProductAttribute>(a => a.Product);
        public Version Version => CurrentAssembly.GetName().Version;
        public Uri WebsiteUri => new Uri(Url);
        #endregion

        #region IPluginSupportInfoProvider
        public IPluginSupportInfo GetPluginSupportInfo() => Instance;
        #endregion
    }
}