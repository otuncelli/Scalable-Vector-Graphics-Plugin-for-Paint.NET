﻿// Copyright 2023 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using PaintDotNet;
using PaintDotNet.AppModel;
using SvgFileTypePlugin.Extensions;

namespace SvgFileTypePlugin;

internal static partial class UIHelper
{
    public static Form GetMainForm()
    {
        IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;
        return (Form)Control.FromHandle(handle) ?? Application.OpenForms["MainForm"] ?? Application.OpenForms[0];
    }

    public static bool IsSaveConfigDialogVisible()
    {
        Form main = GetMainForm();
        return Application.OpenForms
            .OfType<PdnBaseForm>()
            .Where(form => form != main)
            .Reverse()
            .SelectMany(form => form.Descendants())
            .AsParallel()
            .OfType<SaveConfigWidget>()
            .Any();
    }

    public static TResult RunOnUIThread<TResult>(Func<TResult> d)
    {
        IUISynchronizationContext ctx = Services.Get<IUISynchronizationContext>();
        if (ctx.IsOnUIThread)
        {
            return d();
        }
        TResult result = default;
        Exception error = null;
        ctx.Send(new SendOrPostCallback(_ =>
        {
            try
            {
                result = d();
            }
            catch (Exception ex)
            {
                error = ex;
            }
        }), null);

        if (error != null)
        {
            throw error;
        }
        return result;
    }

    public static void RunOnUIThread(Action d)
    {
        IUISynchronizationContext ctx = Services.Get<IUISynchronizationContext>();
        if (ctx.IsOnUIThread)
        {
            d();
            return;
        }
        Exception error = null;
        ctx.Send(new SendOrPostCallback(_ =>
        {
            try
            {
                d();
            }
            catch (Exception ex)
            {
                error = ex;
            }
        }), null);

        if (error != null)
        {
            throw error;
        }
    }
}
