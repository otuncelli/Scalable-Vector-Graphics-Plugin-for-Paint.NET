// Copyright 2021 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by a LGPL license that can be
// found in the COPYING file.

using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SvgFileTypePlugin.Import
{
    internal sealed class MyCheckBox : CheckBox
    {
        public MyCheckBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(BackColor))
            {
                e.Graphics.FillRectangle(b, ClientRectangle);
            }

            //Draw the checkbox
            CheckBoxState state;
            switch(CheckState)
            {
                case CheckState.Checked when Enabled:
                    state = CheckBoxState.CheckedNormal;
                    break;
                case CheckState.Checked when !Enabled:
                    state = CheckBoxState.CheckedDisabled;
                    break;
                case CheckState.Unchecked when Enabled:
                    state = CheckBoxState.UncheckedNormal;
                    break;
                case CheckState.Unchecked when !Enabled:
                    state = CheckBoxState.UncheckedDisabled;
                    break;
                case CheckState.Indeterminate when Enabled:
                    state = CheckBoxState.MixedNormal;
                    break;
                default:
                case CheckState.Indeterminate when !Enabled:
                    state = CheckBoxState.MixedDisabled;
                    break;
            }

            Size glyphSize = CheckBoxRenderer.GetGlyphSize(e.Graphics, state);
            Point glyphLocation = new Point(0, (Bounds.Height - glyphSize.Height) / 2);
            CheckBoxRenderer.DrawCheckBox(e.Graphics, glyphLocation, state);
            int left = glyphSize.Width + 2;
            Rectangle textArea = new Rectangle(left, 1, Bounds.Width - left, Bounds.Height);
            ButtonHelper.DrawText(e.Graphics, textArea, Font, Text, Enabled, ForeColor);
        }
    }
}
