// Copyright 2021 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by a LGPL license that can be
// found in the COPYING file.

using System;
using System.Windows.Forms;

namespace SvgFileTypePlugin.Import
{
    internal class PdnPluginForm : Form
    {
        public PdnPluginForm()
        {
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = MinimizeBox = false;
            ShowIcon = false;
        }

        public DialogResult ShowDialog(Form owner)
        {
            Func<DialogResult> action = () => base.ShowDialog(owner);
            return owner?.InvokeRequired == true ? (DialogResult)owner.Invoke(action) : action();
        }

        public new DialogResult ShowDialog()
        {
            return ShowDialog(Utils.GetMainForm());
        }

        protected void ModifyControl(Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke((MethodInvoker)(() => ModifyControl(control, action)));
                return;
            }
            action();
        }
    }
}