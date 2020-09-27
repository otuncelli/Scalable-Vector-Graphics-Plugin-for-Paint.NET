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

        public static int ConvertToPixels(SvgUnitType type, float value, float ppi)
        {
            const double defaultRatioFor96 = 3.78;
            var convertationRatio = ppi / 96 * defaultRatioFor96;

            if (type == SvgUnitType.Millimeter)
            {
                return (int)Math.Ceiling(value * convertationRatio);
            }

            if (type == SvgUnitType.Centimeter)
            {
                return (int)Math.Ceiling(value * 10 * convertationRatio);
            }

            if (type == SvgUnitType.Inch)
            {
                return (int)Math.Ceiling(value * 25.4 * convertationRatio);
            }

            if (type == SvgUnitType.Em || type == SvgUnitType.Pica)
            {
                // Default 1 em for 16 pixels.
                return (int)Math.Ceiling(value * 16);
            }

            if (type != SvgUnitType.Percentage)
            {
                return (int)Math.Ceiling(value);
            }

            return 0;
        }
    }
}