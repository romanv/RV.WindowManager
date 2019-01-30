namespace RV.WM2.WindowManager.Core
{
    using System;
    using System.Windows;
    using System.Windows.Interop;

    public class HotkeyHelper
    {
        private const int HotkeyId = 9000;

        private readonly Action _onHotkeyPressed;

        private HwndSource _source;

        public HotkeyHelper(Action onHotkeyPressed)
        {
            _onHotkeyPressed = onHotkeyPressed;
        }

        public void ActivateHotkeys(Window mainWindow)
        {
            var helper = new WindowInteropHelper(mainWindow);
            _source = HwndSource.FromHwnd(helper.Handle);
            _source?.AddHook(HwndHook);
            RegisterHotKey(mainWindow);
        }

        public void DisableHotkeys(Window mainWindow)
        {
            var helper = new WindowInteropHelper(mainWindow);
            NativeMethods.UnregisterHotKey(helper.Handle, HotkeyId);
        }

        private void RegisterHotKey(Window mainWindow)
        {
            var helper = new WindowInteropHelper(mainWindow);
            const uint VkC = 0x43;
            const uint ModWin = 0x0008;

            if (!NativeMethods.RegisterHotKey(helper.Handle, HotkeyId, ModWin, VkC))
            {
                var error = NativeMethods.GetLastError();
                MessageBox.Show("Cannot register global hotkey, error:\n" + error);
            }
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WmHotkey = 0x0312;

            switch (msg)
            {
                case WmHotkey:
                    switch (wParam.ToInt32())
                    {
                        case HotkeyId:
                            _onHotkeyPressed();
                            handled = true;
                            break;
                    }

                    break;
            }

            return IntPtr.Zero;
        }
    }
}
