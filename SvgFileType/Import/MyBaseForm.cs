// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;
using PaintDotNet;
using SvgFileTypePlugin.Extensions;

namespace SvgFileTypePlugin.Import;

internal partial class MyBaseForm : Form
{
    #region Properties

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool UseAppThemeColors { get; private set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual bool EnableImmersiveDarkMode => false;

    #endregion

    #region Constructor

    protected MyBaseForm(Form? owner)
    {
        Owner = owner;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowIcon = false;
    }

    protected MyBaseForm() : this(null)
    {
    }

    #endregion

    #region Public Instance Methods

    public void SetUseAppThemeColors(bool enable = true)
    {
        if (enable == UseAppThemeColors) 
        { 
            return;
        }
        using (PdnBaseForm dummy = new PdnBaseForm())
        {
            dummy.UseAppThemeColors = enable;
            BackColor = dummy.BackColor;
            ForeColor = dummy.ForeColor;
        }
        UpdateControlColorsRecursive();
        UseAppThemeColors = enable;
    }

    #endregion

    #region Private Instance Methods

    private void UpdateControlColorsRecursive()
    {
        bool isDark = ForeColor.GetBrightness() > BackColor.GetBrightness();

        if (EnableImmersiveDarkMode)
        {
            // Native.SetPreferredAppMode(true);
            Native.UseImmersiveDarkModeColors(this, isDark);
            Native.UseImmersiveDarkMode(this, isDark);
        }

        foreach (Control control in this.Descendants())
        {
            control.ForeColor = ForeColor;
            control.BackColor = BackColor;
            switch (control)

            {
                case LinkLabel linkLabel when isDark:
                    linkLabel.LinkColor = Color.Orange;
                    linkLabel.VisitedLinkColor = Color.Orange;
                    break;
                case LinkLabel linkLabel when !isDark:
                    linkLabel.LinkColor = Color.Empty;
                    linkLabel.VisitedLinkColor = Color.Empty;
                    break;
                case Label label:
                    label.FlatStyle = FlatStyle.Standard;
                    SubscribeOnPaint(label, Label_Paint);
                    break;
                case CheckBox _:
                case RadioButton _:
                    ((ButtonBase)control).FlatStyle = FlatStyle.Standard;
                    SubscribeOnPaint(control, CheckRadio_Paint);
                    break;
                case Button button:
                    button.FlatStyle = FlatStyle.System;
                    break;
                case GroupBox groupBox:
                    groupBox.FlatStyle = FlatStyle.Standard;
                    SubscribeOnPaint(groupBox, GroupBox_Paint);
                    break;
                case ComboBox _ when !isDark:
                case TextBox _ when !isDark:
                    control.BackColor = SystemColors.Window;
                    break;
            }

            if (EnableImmersiveDarkMode)
            {
                if (control.IsHandleCreated)
                {
                    Native.ControlSetDarkMode(control, isDark);
                }
                control.HandleCreated -= Control_HandleCreated;
                control.HandleCreated -= Control_HandleCreated;
                control.HandleCreated += Control_HandleCreated;
            }
        }
    }

    #endregion

    #region Private Static Methods

    private static void DrawBackground(Graphics g, Color backColor, Rectangle rect)
    {
        using SolidBrush brush = new SolidBrush(backColor);
        g.FillRectangle(brush, rect);
    }

    #region Control Event Handlers

    private static void Control_HandleCreated(object? sender, EventArgs e)
    {
        if (sender is not Control ctrl)
        {
            return;
        }

        bool isDark = ctrl.ForeColor.GetBrightness() > ctrl.BackColor.GetBrightness();
        Native.ControlSetDarkMode(ctrl, isDark);
    }

    private static void SubscribeOnPaint(Control control, PaintEventHandler handler)
    {
        control.Paint -= handler;
        control.Paint -= handler;
        control.Paint += handler;
    }

    private static void GroupBox_Paint(object? sender, PaintEventArgs e)
    {
        if (sender is not GroupBox ctrl || ctrl.Enabled)
        {
            return;
        }

        Color foreColor = SystemColors.GrayText;
        bool rtl = ctrl.RightToLeft == RightToLeft.Yes;
        TextFormatFlags tff = ConvertAlignment(ContentAlignment.MiddleLeft, rtl) | TextFormatFlags.WordBreak;
        DrawBackground(e.Graphics, ctrl.BackColor, e.ClipRectangle);
        GroupBoxRenderer.DrawGroupBox(e.Graphics, e.ClipRectangle, ctrl.Text, ctrl.Font, foreColor, tff, System.Windows.Forms.VisualStyles.GroupBoxState.Disabled);
    }

    private static void Label_Paint(object? sender, PaintEventArgs e)
    {
        if (sender is not Label ctrl || ctrl.Enabled)
        {
            return;
        }

        Color foreColor = SystemColors.GrayText;
        bool rtl = ctrl.RightToLeft == RightToLeft.Yes;
        TextFormatFlags tff = ConvertAlignment(ctrl.TextAlign, rtl) | TextFormatFlags.WordBreak;
        DrawBackground(e.Graphics, ctrl.BackColor, e.ClipRectangle);
        TextRenderer.DrawText(e.Graphics, ctrl.Text, ctrl.Font, e.ClipRectangle, foreColor, tff);
    }

    private static void CheckRadio_Paint(object? sender, PaintEventArgs e)
    {
        if (sender is not ButtonBase ctrl || ctrl.Enabled)
        {
            return;
        }

        Color foreColor = SystemColors.GrayText;
        Size glyphSize = CheckBoxRenderer.GetGlyphSize(e.Graphics, System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);
        bool rtl = ctrl.RightToLeft == RightToLeft.Yes;
        TextFormatFlags tff = ConvertAlignment(ctrl.TextAlign, rtl) 
            | TextFormatFlags.LeftAndRightPadding 
            | TextFormatFlags.Internal;
        Rectangle rect = e.ClipRectangle;
        Size size = new Size(rect.Width - 1 - glyphSize.Width, rect.Height);
        Point point = rtl ? new Point(rect.X - 1, rect.Y) : new Point(rect.X + glyphSize.Width + 1, rect.Y + 1);
        rect = new Rectangle(point, size);
        DrawBackground(e.Graphics, ctrl.BackColor, rect);
        TextRenderer.DrawText(e.Graphics, ctrl.Text, ctrl.Font, rect, foreColor, tff);
    }

    #endregion

    #region ContentAlignment<->TextFormatting Conversions

    public static TextFormatFlags ConvertAlignment(ContentAlignment alignment, bool rtl)
    {
        TextFormatFlags tff = TextFormatFlags.Default;
        // Vertical alignment
        tff |= alignment switch
        {
            ContentAlignment.TopLeft or ContentAlignment.TopRight or ContentAlignment.TopCenter => TextFormatFlags.Top,
            ContentAlignment.BottomLeft or ContentAlignment.BottomRight or ContentAlignment.BottomCenter => TextFormatFlags.Bottom,
            ContentAlignment.MiddleLeft or ContentAlignment.MiddleRight or ContentAlignment.MiddleCenter => TextFormatFlags.VerticalCenter,
            _ => TextFormatFlags.Default
        };
        // Horizontal alignment
        tff |= alignment switch
        {
            ContentAlignment.TopLeft or ContentAlignment.BottomLeft or ContentAlignment.MiddleLeft when rtl => TextFormatFlags.Right,
            ContentAlignment.TopLeft or ContentAlignment.BottomLeft or ContentAlignment.MiddleLeft => TextFormatFlags.Left,
            ContentAlignment.TopRight or ContentAlignment.BottomRight or ContentAlignment.MiddleRight when rtl => TextFormatFlags.Left,
            ContentAlignment.TopRight or ContentAlignment.BottomRight or ContentAlignment.MiddleRight => TextFormatFlags.Right,
            _ => TextFormatFlags.HorizontalCenter
        };
        return tff;
    }

    #endregion

    #endregion

    #region Native

    [SuppressUnmanagedCodeSecurity]
    private static partial class Native
    {
        #region Scaling Factor

        [LibraryImport("gdi32.dll", SetLastError = true)]
        private static partial int GetDeviceCaps(nint hdc, int nIndex);
        
        public static float GetScalingFactor()
        {
            const int VERTRES = 10;
            const int DESKTOPVERTRES = 117;
            using Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();
            int logicalScreenHeight = GetDeviceCaps(desktop, VERTRES);
            int physicalScreenHeight = GetDeviceCaps(desktop, DESKTOPVERTRES);
            return physicalScreenHeight / (float)logicalScreenHeight; // 1.25 = 125%
        }

        #endregion

        #region Experimental Dark Mode

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private const int PREFFERED_APP_MODE__ALLOW_DARK = 1;
        private const int PREFFERED_APP_MODE__DEFAULT = 0;
        private const int S_OK = 0;

        public static bool IsDarkModeSupported { get; } = IsWindows10OrGreater(17763);

        public static bool ControlSetDarkMode(IWin32Window window, bool enable)
        {
            if (!IsDarkModeSupported) 
            { 
                return false; 
            }
            int error = SetWindowTheme(window.Handle, enable ? "darkmode_explorer" : "explorer", null);
            return Check(error);
        }

        public static bool UseImmersiveDarkModeColors(IWin32Window window, bool enable)
        {
            if (!IsDarkModeSupported) 
            { 
                return false;
            }
            WindowCompositionAttributeData data = new()
            {
                Attribute = WindowCompositionAttribute.WCA_USEDARKMODECOLORS,
                Data = enable ? 1 : 0,
                SizeOfData = Marshal.SizeOf<int>()
            };
            return SetWindowCompositionAttribute(window.Handle, data);
        }

        public static bool SetPreferredAppMode(bool dark)
        {
            if (!IsDarkModeSupported) 
            {
                return false;
            }
            int error = SetPreferredAppMode(dark ? 1 : 0);
            return Check(error);
        }

        public static bool UseImmersiveDarkMode(IWin32Window window, bool enabled)
        {
            if (!IsDarkModeSupported) 
            { 
                return false; 
            }
            int attr = IsWindows10OrGreater(18985)
                ? DWMWA_USE_IMMERSIVE_DARK_MODE
                : DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;
            int attrValue = enabled ? 1 : 0;
            int error = DwmSetWindowAttribute(window.Handle, attr, ref attrValue, sizeof(int));
            return Check(error);
        }

        private static bool Check(int error)
        {
            return error == S_OK ? true : throw new Win32Exception(error);
        }

        private static bool IsWindows10OrGreater(int build = -1)
        {
            Version version = Environment.OSVersion.Version;
            return version.Major >= 10 && version.Build >= build;
        }

        [LibraryImport("uxtheme.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        private static partial int SetWindowTheme(nint hWnd, string? pszSubAppName, string? pszSubIdList);

        [LibraryImport("uxtheme.dll", EntryPoint = "#133", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool AllowDarkModeForWindow(nint hWnd, [MarshalAs(UnmanagedType.Bool)] bool allow);

        [LibraryImport("uxtheme.dll", EntryPoint = "#135", SetLastError = true)]
        private static partial int SetPreferredAppMode(int i);

        [LibraryImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool SetWindowCompositionAttribute(nint hwnd, WindowCompositionAttributeData data);
        
        [LibraryImport("dwmapi.dll", SetLastError = true)]
        private static partial int DwmSetWindowAttribute(nint hwnd, int attr, ref int attrValue, int attrSize);

        private enum WindowCompositionAttribute
        {
            WCA_ACCENT_POLICY = 19,
            WCA_USEDARKMODECOLORS = 26
        }

        [StructLayout(LayoutKind.Sequential)]
        private ref struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public int Data;
            public int SizeOfData;
        }

        #endregion
    }

    #endregion
}
