using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SvgFileTypePlugin
{
    /// <summary>
    /// Parent centered MessageBox dialog in C#
    /// </summary>
    internal static class MessageBoxEx
    {
        private static IWin32Window _owner;
        private static readonly HookProc _hookProc = MessageBoxHookProc;
        private static IntPtr _hHook = IntPtr.Zero;

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

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
            MessageBoxDefaultButton defButton)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons, icon, defButton);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
            MessageBoxDefaultButton defButton, MessageBoxOptions options)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons, icon, defButton, options);
        }

        public static DialogResult Show(IWin32Window owner, string text)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons,
            MessageBoxIcon icon)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons, icon);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons,
            MessageBoxIcon icon, MessageBoxDefaultButton defButton)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons, icon, defButton);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons,
            MessageBoxIcon icon, MessageBoxDefaultButton defButton, MessageBoxOptions options)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons, icon,
                defButton, options);
        }

        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        public const int WH_CALLWNDPROCRET = 12;
        public const int WH_CALLWNDPROC = 4;

        public enum CbtHookAction
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

        private static class NativeMethods
        {
            [DllImport("user32", SetLastError = true)]
            public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

            [DllImport("user32", SetLastError = true)]
            public static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle lpRect);

            [DllImport("user32", SetLastError = true)]
            public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy,
                SetWindowPosFlags uFlags);

            [DllImport("user32", SetLastError = true)]
            public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, uint threadId);

            [DllImport("user32", SetLastError = true)]
            public static extern int UnhookWindowsHookEx(IntPtr idHook);

            [DllImport("user32", SetLastError = true)]
            public static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CWPRETSTRUCT
        {
            public IntPtr lResult;
            public IntPtr lParam;
            public IntPtr wParam;
            public uint message;
            public IntPtr hwnd;
        };

        private static void Initialize()
        {
            if (_hHook != IntPtr.Zero)
            {
                throw new NotSupportedException("multiple calls are not supported");
            }

            if (_owner == null)
                return;

            IntPtr ptr = _owner.Handle;
            _hHook = NativeMethods.SetWindowsHookEx(WH_CALLWNDPROCRET, _hookProc, IntPtr.Zero,
                NativeMethods.GetWindowThreadProcessId(ptr, out uint _));
        }

        private static IntPtr MessageBoxHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return NativeMethods.CallNextHookEx(_hHook, nCode, wParam, lParam);
            }

            CWPRETSTRUCT msg = (CWPRETSTRUCT)Marshal.PtrToStructure(lParam, typeof(CWPRETSTRUCT));
            IntPtr hook = _hHook;

            if (msg.message == (int)CbtHookAction.HCBT_ACTIVATE)
            {
                try
                {
                    CenterWindow(msg.hwnd);
                }
                finally
                {
                    NativeMethods.UnhookWindowsHookEx(_hHook);
                    _hHook = IntPtr.Zero;
                }
            }

            return NativeMethods.CallNextHookEx(hook, nCode, wParam, lParam);
        }

        private static void CenterWindow(IntPtr hChildWnd)
        {
            Rectangle recChild = new Rectangle(0, 0, 0, 0);
            bool success = NativeMethods.GetWindowRect(hChildWnd, ref recChild);

            int width = recChild.Width - recChild.X;
            int height = recChild.Height - recChild.Y;

            Rectangle recParent = new Rectangle(0, 0, 0, 0);
            success = NativeMethods.GetWindowRect(_owner.Handle, ref recParent);

            if (!success ||
                (recParent.X == -32000 && recParent.Y == -32000))
            {
                // https://blogs.msdn.microsoft.com/oldnewthing/20041028-00/?p=37453
                NativeMethods.UnhookWindowsHookEx(_hHook);
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
            Task.Factory.StartNew(() =>
            {
                NativeMethods.SetWindowPos(hChildWnd, IntPtr.Zero, ptStart.X, ptStart.Y, width, height,
                    SetWindowPosFlags.AsynchronousWindowPosition |
                    SetWindowPosFlags.IgnoreResize |
                    SetWindowPosFlags.DoNotActivate |
                    SetWindowPosFlags.DoNotChangeOwnerZOrder |
                    SetWindowPosFlags.IgnoreZOrder);
            });
        }

        [Flags]
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
    }
}