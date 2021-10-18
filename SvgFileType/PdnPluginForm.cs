// Copyright 2021 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by a LGPL license that can be
// found in the COPYING file.

using PaintDotNet.VisualStyling;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;

namespace SvgFileTypePlugin
{
    internal class PdnPluginForm : Form
    {
        public PdnPluginForm()
        {
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = MinimizeBox = false;
            ShowIcon = false;
        }

        public bool UseNativeDarkMode { get; set; }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            InitTheme();
        }

        public void InitTheme()
        {
            bool isDark = false;
            void Apply(Control.ControlCollection controls)
            {
                foreach (Control control in controls)
                {
                    if (control is TransparentLabel)
                    {
                        continue;
                    }

                    if (control is Label label)
                    {
                        label.FlatStyle = FlatStyle.System;
                        if (control is LinkLabel link)
                        {
                            if (ForeColor != DefaultForeColor)
                            {
                                link.LinkColor = ForeColor;
                                link.VisitedLinkColor = ForeColor;
                            }
                            else
                            {
                                link.LinkColor = Color.Empty;
                                link.VisitedLinkColor = Color.Empty;
                            }
                        }
                    }
                    else if (control is ButtonBase buttonBase)
                    {
                        if (control is CheckBox || control is RadioButton)
                        {
                            buttonBase.FlatStyle = FlatStyle.Standard;
                        }
                        else
                        {
                            buttonBase.FlatStyle = FlatStyle.System;
                        }
                    }

                    if (isDark && UseNativeDarkMode)
                    {
                        Native.SetWindowTheme(control.Handle, "DarkMode_Explorer", null);
                    }

                    control.ForeColor = ForeColor;
                    control.BackColor = BackColor;
                    if (control.HasChildren)
                    {
                        Apply(control.Controls);
                    }
                }
            }

            if (ThemeConfig.EffectiveTheme == PdnTheme.Aero)
            {
                if (AeroColors.IsDark)
                {
                    BackColor = AeroColors.CanvasBackFillColor;
                    if (UseNativeDarkMode)
                    {
                        Native.UseImmersiveDarkModeColors(Handle, true);
                    }
                    isDark = true;
                }
                else
                {
                    BackColor = AeroColors.FormBackColor;
                }
                ForeColor = AeroColors.FormTextColor;
            }
            else if (SystemInformation.HighContrast)
            {
                BackColor = DefaultBackColor;
                ForeColor = DefaultForeColor;
            }
            else
            {
                BackColor = ClassicColors.FormBackColor;
                ForeColor = ClassicColors.FormForeColor;
            }
            Apply(Controls);
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

        #region Dark Mode
        [SuppressUnmanagedCodeSecurity]
        internal static class Native
        {
            const string uxtheme = "uxtheme";
            const string user32 = "user32";

            [DllImport(uxtheme, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
            public static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

            [DllImport(uxtheme, SetLastError = true, EntryPoint = "#133")]
            public static extern bool AllowDarkModeForWindow(IntPtr hWnd, bool allow);

            [DllImport(uxtheme, SetLastError = true, EntryPoint = "#135")]
            static extern int SetPreferredAppMode(int i);

            [DllImport(user32, SetLastError = true)]
            static extern bool SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

            [DllImport(user32, SetLastError = true, CharSet = CharSet.Auto)]
            public static extern bool DestroyIcon(IntPtr handle);

            [DllImport("dwmapi", SetLastError = true)]
            static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

            [DllImport("gdi32", SetLastError = true)]
            static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

            const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
            const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
            const int PREFFERED_APP_MODE__ALLOW_DARK = 1;
            const int PREFFERED_APP_MODE__DEFAULT = 0;

            public enum DeviceCap
            {
                VERTRES = 10,
                DESKTOPVERTRES = 117
            }

            [StructLayout(LayoutKind.Sequential)]
            struct WindowCompositionAttributeData
            {
                public WindowCompositionAttribute Attribute;
                public IntPtr Data;
                public int SizeOfData;
            }

            public static void ControlSetDarkMode(IntPtr handle, bool v)
            {
                SetWindowTheme(handle, v ? "DarkMode_Explorer" : "Explorer", null);
            }

            enum WindowCompositionAttribute
            {
                WCA_ACCENT_POLICY = 19,
                WCA_USEDARKMODECOLORS = 26
            }

            internal static bool UseImmersiveDarkModeColors(IntPtr handle, bool dark)
            {
                int size = Marshal.SizeOf(typeof(int));
                IntPtr pBool = Marshal.AllocHGlobal(size);
                try
                {
                    Marshal.WriteInt32(pBool, 0, dark ? 1 : 0);
                    var data = new WindowCompositionAttributeData()
                    {
                        Attribute = WindowCompositionAttribute.WCA_USEDARKMODECOLORS,
                        Data = pBool,
                        SizeOfData = size
                    };
                    return SetWindowCompositionAttribute(handle, ref data);
                }
                finally
                {
                    if (pBool != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pBool);
                    }
                }
            }

            public static void SetPrefferDarkMode(bool dark)
            {
                SetPreferredAppMode(dark ? PREFFERED_APP_MODE__ALLOW_DARK : PREFFERED_APP_MODE__DEFAULT);
            }

            public static bool UseImmersiveDarkMode(IntPtr handle, bool enabled)
            {
                if (IsWindows10OrGreater(17763))
                {

                    var attribute = DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;
                    if (IsWindows10OrGreater(18985))
                    {
                        attribute = DWMWA_USE_IMMERSIVE_DARK_MODE;
                    }

                    int useImmersiveDarkMode = enabled ? 1 : 0;
                    return DwmSetWindowAttribute(handle, attribute, ref useImmersiveDarkMode, sizeof(int)) == 0;
                }

                return false;
            }

            public static bool IsWindows10OrGreater(int build = -1)
            {
                return Environment.OSVersion.Version.Major >= 10 && Environment.OSVersion.Version.Build >= build;
            }

            public static float GetScalingFactor()
            {
                float ScreenScalingFactor;

                using (var g = Graphics.FromHwnd(IntPtr.Zero))
                {
                    var desktop = g.GetHdc();
                    var LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
                    var PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);

                    ScreenScalingFactor = PhysicalScreenHeight / (float)LogicalScreenHeight;
                }

                return ScreenScalingFactor; // 1.25 = 125%
            }
        }
        #endregion
    }
}