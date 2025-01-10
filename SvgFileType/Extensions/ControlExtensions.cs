// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SvgFileTypePlugin.Extensions;

internal static class ControlExtensions
{
    public static IEnumerable<Control> Descendants(this Control root)
    {
        ArgumentNullException.ThrowIfNull(root);

        foreach (Control control in root.Controls)
        {
            yield return control;
            if (control.HasChildren)
                foreach (Control child in Descendants(control))
                    yield return child;
        }
    }

    public static object? RunOnUIThread<T>(this Control control, T action, params object[] args) where T : Delegate
    {
        ArgumentNullException.ThrowIfNull(control);

        return control.InvokeRequired ? control.Invoke(action, args) : action.DynamicInvoke(args);
    }

    public static object? RunOnUIThread<T>(this Control control, T action) where T : Delegate
    {
        ArgumentNullException.ThrowIfNull(control);

        return control.InvokeRequired ? control.Invoke(action) : action.DynamicInvoke();
    }
}
