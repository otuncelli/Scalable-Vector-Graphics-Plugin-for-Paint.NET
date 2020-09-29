using Svg;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace SvgFileTypePlugin
{
    internal static class Utils
    {
        public static Form GetMainForm()
        {
            try
            {
                var form = Control.FromHandle(Process.GetCurrentProcess().MainWindowHandle) as Form;
                return form ?? Application.OpenForms["MainForm"];
            }
            catch
            {
                return null;
            }
        }

        public static int ConvertToPixels(SvgUnitType unit, float value, float ppi)
        {
            const float defaultRatioFor96 = 3.78f;
            var convertationRatio = ppi / 96 * defaultRatioFor96;
            float pixels;
            switch (unit)
            {
                case SvgUnitType.Millimeter:
                    pixels = value * convertationRatio;
                    break;
                case SvgUnitType.Centimeter:
                    pixels = value * convertationRatio * 10;
                    break;
                case SvgUnitType.Inch:
                    pixels = value * convertationRatio * 25.4f;
                    break;
                case SvgUnitType.Em:
                case SvgUnitType.Pica:
                    pixels = value * 16;
                    break;
                default:
                    pixels = 0;
                    break;
            }

            return (int) Math.Ceiling(pixels);
        }
    }
}