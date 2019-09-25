namespace RV.WM2.WindowManager.Core
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    internal class NativeMethods
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        public static string GetWindowTitleUnderCursor()
        {
            var p = default(Point);
            bool retVal = GetCursorPos(ref p);

            var title = string.Empty;

            if (retVal)
            {
                var hwnd = WindowFromPoint(p);
                var ancestorHwnd = GetAncestor(hwnd, 2);
                title = GetWindowTitle(ancestorHwnd);
            }

            return title;
        }

        public static IntPtr GetWindowHandleUnderCursor()
        {
            var p = default(Point);
            bool retVal = GetCursorPos(ref p);

            var hwnd = IntPtr.Zero;

            if (retVal)
            {
                hwnd = WindowFromPoint(p);
                var ancestorHwnd = GetAncestor(hwnd, 2);
                return ancestorHwnd;
            }

            return hwnd;
        }

        [DllImport("User32.dll")]
        public static extern bool RegisterHotKey([In] IntPtr hWnd, [In] int id, [In] uint fsModifiers, [In] uint vk);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        [DllImport("User32.dll")]
        public static extern bool UnregisterHotKey([In] IntPtr hWnd, [In] int id);

        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(ref Point p);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int width, int height, bool repaint);

        [DllImport("dwmapi.dll")]
        public static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out RECT pvAttribute, int cbAttribute);

        [DllImport("user32.dll")]
        public static extern int GetWindowRect(IntPtr hwnd, out RECT rect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetAncestor(IntPtr hWnd, uint gaFlags);

        private static string GetWindowTitle(IntPtr hwnd)
        {
            var windowTitle = string.Empty;
            StringBuilder builder;

            try
            {
                var size = GetWindowTextLength(hwnd);
                builder = new StringBuilder(size + 1);
                GetWindowText(hwnd, builder, builder.Capacity);

                if (!string.IsNullOrEmpty(builder.ToString()) && !string.IsNullOrWhiteSpace(builder.ToString()))
                {
                    windowTitle = builder.ToString();
                }
            }
            catch (Exception ex)
            {
                windowTitle = ex.Message;
            }
            finally
            {
                builder = null;
            }

            return windowTitle;
        }

        [DllImport("User32.dll")]
        private static extern IntPtr WindowFromPoint(Point p);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int X;
            public int Y;

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
    }
}
