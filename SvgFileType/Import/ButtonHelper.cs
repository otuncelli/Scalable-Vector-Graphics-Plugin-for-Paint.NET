// Copyright 2021 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by a LGPL license that can be
// found in the COPYING file.

using System.Drawing;

namespace SvgFileTypePlugin.Import
{
    internal static class ButtonHelper
    {
        public static void DrawText(Graphics g, Rectangle rectangle, Font font, string text, bool enabled, Color color)
        {
            //Create string format
            using (var sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Near;

                if (enabled)
                {
                    using (var solidBrush = new SolidBrush(color))
                    {
                        g.DrawString(text, font, solidBrush, rectangle, sf);
                    }
                }
                else
                {
                    g.DrawString(text, font, SystemBrushes.GrayText, rectangle, sf);
                }
            }
        }
    }
}
