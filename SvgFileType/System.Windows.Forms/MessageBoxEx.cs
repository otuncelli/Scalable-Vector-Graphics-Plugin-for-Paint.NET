// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;

namespace System.Windows.Forms;

/// <summary>
/// Parent centered MessageBox dialog in C#
/// </summary>
internal static class MessageBoxEx
{
    private static IWin32Window? _owner;
    private static readonly HookProc _hookProc = MessageBoxHookProc;
    private static nint _hHook = nint.Zero;

    private const int WH_CALLWNDPROCRET = 12;
    // private const int WH_CALLWNDPROC = 4;
    private const SetWindowPosFlags DefaultWindowPosFlags =
        SetWindowPosFlags.AsynchronousWindowPosition |
        SetWindowPosFlags.IgnoreResize |
        SetWindowPosFlags.DoNotActivate |
        SetWindowPosFlags.DoNotChangeOwnerZOrder |
        SetWindowPosFlags.IgnoreZOrder;

    #region Show
    public static DialogResult Show(string text)
    {
        Initialize();
        return MessageBox.Show(text);
    }

    public static DialogResult Show(string text, string caption)
    {
        Initialize();
        return MessageBox.Show(text, caption);
    }

    public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
    {
        Initialize();
        return MessageBox.Show(text, caption, buttons);
    }

    public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
    {
        Initialize();
        return MessageBox.Show(text, caption, buttons, icon);
    }

    public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
    {
        Initialize();
        return MessageBox.Show(text, caption, buttons, icon, defaultButton);
    }

    public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
    {
        Initialize();
        return MessageBox.Show(text, caption, buttons, icon, defaultButton, options);
    }

    public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, bool displayHelpButton)
    {
        Initialize();
        return MessageBox.Show(text, caption, buttons, icon, defaultButton, options, displayHelpButton);
    }

    public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath)
    {
        Initialize();
        return MessageBox.Show(text, caption, buttons, icon, defaultButton, options, helpFilePath);
    }

    public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator)
    {
        Initialize();
        return MessageBox.Show(text, caption, buttons, icon, defaultButton, options, helpFilePath, navigator);
    }

    public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param)
    {
        Initialize();
        return MessageBox.Show(text, caption, buttons, icon, defaultButton, options, helpFilePath, navigator, param);
    }

    public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword)
    {
        Initialize();
        return MessageBox.Show(text, caption, buttons, icon, defaultButton, options, helpFilePath, keyword);
    }

    #endregion

    #region Show with Owner
    public static DialogResult Show(IWin32Window owner, string text)
    {
        Initialize(owner);
        return MessageBox.Show(owner, text);
    }

    public static DialogResult Show(IWin32Window owner, string text, string caption)
    {
        Initialize(owner);
        return MessageBox.Show(owner, text, caption);
    }

    public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
    {
        Initialize(owner);
        return MessageBox.Show(owner, text, caption, buttons);
    }

    public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
    {
        Initialize(owner);
        return MessageBox.Show(owner, text, caption, buttons, icon);
    }

    public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
    {
        Initialize(owner);
        return MessageBox.Show(owner, text, caption, buttons, icon, defaultButton);
    }

    public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
    {
        Initialize(owner);
        return MessageBox.Show(owner, text, caption, buttons, icon, defaultButton, options);
    }

    public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath)
    {
        Initialize(owner);
        return MessageBox.Show(owner, text, caption, buttons, icon, defaultButton, options, helpFilePath);
    }

    public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator)
    {
        Initialize(owner);
        return MessageBox.Show(owner, text, caption, buttons, icon, defaultButton, options, helpFilePath, navigator);
    }

    public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param)
    {
        Initialize(owner);
        return MessageBox.Show(owner, text, caption, buttons, icon, defaultButton, options, helpFilePath, navigator, param);
    }

    public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword)
    {
        Initialize(owner);
        return MessageBox.Show(owner, text, caption, buttons, icon, defaultButton, options, helpFilePath, keyword);
    }

    #endregion

    #region Private Methods

    private static void Initialize(IWin32Window? owner = null)
    {
        _owner = owner;
        if (_hHook != nint.Zero)
        {
            throw new NotSupportedException("Multiple calls are not supported");
        }
        if (_owner == null)
        {
            return;
        }

        nint ownerHandle = _owner.Handle;
        uint threadId = NativeMethods.GetWindowThreadProcessId(ownerHandle, out uint _);
        _hHook = NativeMethods.SetWindowsHookEx(WH_CALLWNDPROCRET, _hookProc, nint.Zero, threadId);
    }

    private static nint MessageBoxHookProc(int nCode, nint wParam, CWPRETSTRUCT lParam)
    {
        if (nCode < 0)
        {
            return NativeMethods.CallNextHookEx(_hHook, nCode, wParam, lParam);
        }

        nint hook = _hHook;

        if (lParam.message == (uint)CbtHookAction.HCBT_ACTIVATE)
        {
            try
            {
                CenterWindow(lParam.hwnd);
            }
            finally
            {
                int ret = NativeMethods.UnhookWindowsHookEx(_hHook);
                Debug.WriteLineIf(ret == 0, $"Win32 Error: {Marshal.GetLastWin32Error()} when unhooking.");
                _hHook = nint.Zero;
            }
        }

        return NativeMethods.CallNextHookEx(hook, nCode, wParam, lParam);
    }

    private static void CenterWindow(nint hChildWnd)
    {
        Rectangle recChild = Rectangle.Empty;
        bool success = NativeMethods.GetWindowRect(hChildWnd, ref recChild);

        int width = recChild.Width - recChild.X;
        int height = recChild.Height - recChild.Y;

        Rectangle recParent = Rectangle.Empty;

        Debug.Assert(_owner is not null);
        success = NativeMethods.GetWindowRect(_owner.Handle, ref recParent);

        if (!success || (recParent.X == -32000 && recParent.Y == -32000))
        {
            // https://blogs.msdn.microsoft.com/oldnewthing/20041028-00/?p=37453
            int ret = NativeMethods.UnhookWindowsHookEx(_hHook);
            Debug.WriteLineIf(ret == 0, $"Win32 Error: {Marshal.GetLastWin32Error()} when unhooking.");
            return;
        }

        Point ptCenter = new Point
        {
            X = recParent.X + (recParent.Width - recParent.X) / 2,
            Y = recParent.Y + (recParent.Height - recParent.Y) / 2
        };

        Point ptStart = new Point
        {
            X = ptCenter.X - width / 2,
            Y = ptCenter.Y - height / 2
        };

        //ptStart.X = ptStart.X < 0 ? 0 : ptStart.X;
        //ptStart.Y = ptStart.Y < 0 ? 0 : ptStart.Y;

        //int result = NativeMethods.MoveWindow(hChildWnd, ptStart.X, ptStart.Y, width, height, false);
        Task.Factory.StartNew(() => NativeMethods.SetWindowPos(hChildWnd, nint.Zero, ptStart.X, ptStart.Y, width, height, DefaultWindowPosFlags));
    }

    #endregion

    #region WinAPI

    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Winapi, SetLastError = true)]
    private delegate nint HookProc(int nCode, nint wParam, CWPRETSTRUCT lParam);

    [SuppressUnmanagedCodeSecurity]
    private static class NativeMethods
    {
        private const string User32 = "user32";

        [DllImport(User32, SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(nint hWnd, out uint lpdwProcessId);

        [DllImport(User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(nint hWnd, ref Rectangle lpRect);

        [DllImport(User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(nint hWnd, nint hWndInsertAfter, int x, int y, int cx, int cy, SetWindowPosFlags uFlags);

        [DllImport(User32, SetLastError = true)]
        public static extern nint SetWindowsHookEx(int idHook, HookProc lpfn, nint hInstance, uint threadId);

        [DllImport(User32, SetLastError = true)]
        public static extern int UnhookWindowsHookEx(nint idHook);

        [DllImport(User32, SetLastError = true)]
        public static extern nint CallNextHookEx(nint idHook, int nCode, nint wParam, CWPRETSTRUCT lParam);
    }

    [StructLayout(LayoutKind.Sequential)]
    private ref struct CWPRETSTRUCT
    {
        public nint lResult;
        public nint lParam;
        public nint wParam;
        public uint message;
        public nint hwnd;
    };

    private enum CbtHookAction : uint
    {
        HCBT_MOVESIZE = 0,
        HCBT_MINMAX = 1,
        HCBT_QS = 2,
        HCBT_CREATEWND = 3,
        HCBT_DESTROYWND = 4,
        HCBT_ACTIVATE = 5,
        HCBT_CLICKSKIPPED = 6,
        HCBT_KEYSKIPPED = 7,
        HCBT_SYSCOMMAND = 8,
        HCBT_SETFOCUS = 9
    }

    [Flags]
#pragma warning disable IDE0079 // Remove unnecessary suppression
    [SuppressMessage("Design", "CA1069:Enums values should not be duplicated", Justification = "https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowpos")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
    private enum SetWindowPosFlags : uint
    {
        /// <summary>If the calling thread and the thread that owns the window are attached to different input queues, 
        /// the system posts the request to the thread that owns the window. This prevents the calling thread from 
        /// blocking its execution while other threads process the request.</summary>
        /// <remarks>SWP_ASYNCWINDOWPOS</remarks>
        AsynchronousWindowPosition = 0x4000,

        /// <summary>Prevents generation of the WM_SYNCPAINT message.</summary>
        /// <remarks>SWP_DEFERERASE</remarks>
        DeferErase = 0x2000,

        /// <summary>Draws a frame (defined in the window's class description) around the window.</summary>
        /// <remarks>SWP_DRAWFRAME</remarks>
        DrawFrame = 0x0020,

        /// <summary>Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to 
        /// the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE 
        /// is sent only when the window's size is being changed.</summary>
        /// <remarks>SWP_FRAMECHANGED</remarks>
        FrameChanged = 0x0020,

        /// <summary>Hides the window.</summary>
        /// <remarks>SWP_HIDEWINDOW</remarks>
        HideWindow = 0x0080,

        /// <summary>Does not activate the window. If this flag is not set, the window is activated and moved to the 
        /// top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter 
        /// parameter).</summary>
        /// <remarks>SWP_NOACTIVATE</remarks>
        DoNotActivate = 0x0010,

        /// <summary>Discards the entire contents of the client area. If this flag is not specified, the valid 
        /// contents of the client area are saved and copied back into the client area after the window is sized or 
        /// repositioned.</summary>
        /// <remarks>SWP_NOCOPYBITS</remarks>
        DoNotCopyBits = 0x0100,

        /// <summary>Retains the current position (ignores X and Y parameters).</summary>
        /// <remarks>SWP_NOMOVE</remarks>
        IgnoreMove = 0x0002,

        /// <summary>Does not change the owner window's position in the Z order.</summary>
        /// <remarks>SWP_NOOWNERZORDER</remarks>
        DoNotChangeOwnerZOrder = 0x0200,

        /// <summary>Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to 
        /// the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent 
        /// window uncovered as a result of the window being moved. When this flag is set, the application must 
        /// explicitly invalidate or redraw any parts of the window and parent window that need redrawing.</summary>
        /// <remarks>SWP_NOREDRAW</remarks>
        DoNotRedraw = 0x0008,

        /// <summary>Same as the SWP_NOOWNERZORDER flag.</summary>
        /// <remarks>SWP_NOREPOSITION</remarks>
        DoNotReposition = 0x0200,

        /// <summary>Prevents the window from receiving the WM_WINDOWPOSCHANGING message.</summary>
        /// <remarks>SWP_NOSENDCHANGING</remarks>
        DoNotSendChangingEvent = 0x0400,

        /// <summary>Retains the current size (ignores the cx and cy parameters).</summary>
        /// <remarks>SWP_NOSIZE</remarks>
        IgnoreResize = 0x0001,

        /// <summary>Retains the current Z order (ignores the hWndInsertAfter parameter).</summary>
        /// <remarks>SWP_NOZORDER</remarks>
        IgnoreZOrder = 0x0004,

        /// <summary>Displays the window.</summary>
        /// <remarks>SWP_SHOWWINDOW</remarks>
        ShowWindow = 0x0040,
    }

    #endregion
}
