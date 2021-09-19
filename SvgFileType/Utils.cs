// Copyright 2021 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by a LGPL license that can be
// found in the COPYING file.

using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace SvgFileTypePlugin
{
    internal static class Utils
    {
        private static readonly Lazy<Form> MainFormLazy = new Lazy<Form>(() =>
        {
            IntPtr windowHandle = Process.GetCurrentProcess().MainWindowHandle;
            Form form = Control.FromHandle(windowHandle) as Form ?? Application.OpenForms["MainForm"];
            if (form == null)
            {
                Debug.WriteLine("Can't get the main form.");
            }
            return form;
        });

        public static Form GetMainForm()
        {
            return MainFormLazy.Value;
        }

        public static DialogResult ThreadSafeShowDialog(Form owner, Func<DialogResult> func)
        {
            return owner.InvokeRequired ? (DialogResult)owner.Invoke(func) : func();
        }

        public static int CalcMemoryNeeded(int width, int height)
        {
            return Math.Max(height * CalcStride(width) / (1024 * 1024), 1);
        }

        public static int CalcStride(int width, int bytesPerPixel = 4)
        {
            return 4 * ((width * bytesPerPixel * 8 + 31) / 32);
        }
    }
}