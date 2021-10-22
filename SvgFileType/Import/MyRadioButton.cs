// Copyright 2021 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by a LGPL license that can be
// found in the COPYING file.

using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SvgFileTypePlugin.Import
{
    internal sealed class MyRadioButton : RadioButton
    {
        public MyRadioButton()
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

            RadioButtonState state;
            
            if (Checked)
            {
                state = Enabled ? RadioButtonState.CheckedNormal : RadioButtonState.CheckedDisabled;
            }
            else
            {
                state = Enabled ? RadioButtonState.UncheckedNormal : RadioButtonState.UncheckedDisabled;
            }

            Size glyphSize = RadioButtonRenderer.GetGlyphSize(e.Graphics, state);
            Point glyphLocation = new Point(0, (Bounds.Height - glyphSize.Height) / 2);
            RadioButtonRenderer.DrawRadioButton(e.Graphics, glyphLocation, state);
            int left = glyphSize.Width + 2;
            Rectangle textArea = new Rectangle(left, 1, Bounds.Width - left, Bounds.Height);
            ButtonHelper.DrawText(e.Graphics, textArea, Font, Text, Enabled, ForeColor);
        }
    }
}
