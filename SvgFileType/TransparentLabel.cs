// Copyright 2021 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by a LGPL license that can be
// found in the COPYING file.

using System.Windows.Forms;

namespace SvgFileTypePlugin
{
    internal class TransparentLabel : Label
    {
        public TransparentLabel()
        {
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams parms = base.CreateParams;
                parms.ExStyle |= 0x20; // WS_EX_TRANSPARENT
                return parms;
            }
        }
    }
}
