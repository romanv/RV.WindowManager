namespace RV.WM2.WindowManager.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Microsoft.TeamFoundation.MVVM;

    using RV.WM2.Infrastructure.Core;
    using RV.WM2.Infrastructure.Models;
    using RV.WM2.Infrastructure.MVVM;
    using RV.WM2.WindowManager.Core;

    using Point = System.Windows.Point;

    public class MainViewModel : BrowsableObject
    {
        private ObservableCollection<ScreenSlot> _slots;

        private ICommand _cmdSelectSlot;
        private ICommand _cmdRegisterHotkeys;
        private ICommand _cmdReloadConfiguration;

        private string _activeWindowTitle = "No active windows found";

        private HotkeyHelper _hotkeyHelper;

        private IntPtr _activeWindowHandle = IntPtr.Zero;
        private Visibility _windowVisibility = Visibility.Visible;
        private double _windowLeft;
        private double _windowTop;
        private double _windowWidth;
        private double _titleTextWidth;
        private bool _activated;

        public MainViewModel()
        {
            ReloadSlotsConfiguration();
        }

        public string ActiveWindowTitle
        {
            get
            {
                return _activeWindowTitle;
            }

            set
            {
                if (_activeWindowTitle == value)
                {
                    return;
                }

                _activeWindowTitle = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ScreenSlot> Slots
        {
            get
            {
                return _slots;
            }

            set
            {
                if (_slots == value)
                {
                    return;
                }

                _slots = value;
                RaisePropertyChanged();
            }
        }

        public Visibility WindowVisibility
        {
            get
            {
                return _windowVisibility;
            }

            set
            {
                if (_windowVisibility == value)
                {
                    return;
                }

                _windowVisibility = value;
                RaisePropertyChanged();
            }
        }

        public double WindowTop
        {
            get
            {
                return _windowTop;
            }

            set
            {
                if (_windowTop == value)
                {
                    return;
                }

                _windowTop = value;
                RaisePropertyChanged();
            }
        }

        public double WindowLeft
        {
            get
            {
                return _windowLeft;
            }

            set
            {
                if (_windowLeft == value)
                {
                    return;
                }

                _windowLeft = value;
                RaisePropertyChanged();
            }
        }

        public double WindowWidth
        {
            get
            {
                return _windowWidth;
            }

            set
            {
                if (_windowWidth == value || double.IsNaN(value))
                {
                    return;
                }

                _windowWidth = value;

                if (TitleTextWidth == 0)
                {
                    TitleTextWidth = value - 38;
                }

                RaisePropertyChanged();
            }
        }

        public double WindowHeight { get; set; }

        public double TitleTextWidth
        {
            get
            {
                return _titleTextWidth;
            }

            set
            {
                if (_titleTextWidth == value)
                {
                    return;
                }

                _titleTextWidth = value;
                RaisePropertyChanged();
            }
        }

        public bool Activated
        {
            get
            {
                return _activated;
            }

            set
            {
                if (_activated == value)
                {
                    return;
                }

                _activated = value;
                RaisePropertyChanged();
            }
        }

        public ICommand CmdSelectSlot => _cmdSelectSlot ?? (_cmdSelectSlot = new RelayCommand(OnCmdSelectSlot));

        public ICommand CmdCloseApplication => new RelayCommand(() =>
            {
                WindowVisibility = Visibility.Visible;
                _hotkeyHelper.DisableHotkeys(Application.Current.MainWindow);
                Application.Current.Shutdown();
            });

        public ICommand CmdRegisterHotkeys
            => _cmdRegisterHotkeys ?? (_cmdRegisterHotkeys = new RelayCommand(OnCmdRegisterHotkeys));

        public ICommand CmdMouseLeftWindow => new RelayCommand(() => WindowVisibility = Visibility.Hidden);

        public ICommand CmdReloadConfiguration
            => _cmdReloadConfiguration ?? (_cmdReloadConfiguration = new RelayCommand(ReloadSlotsConfiguration));

        private void OnCmdRegisterHotkeys()
        {
            _hotkeyHelper = new HotkeyHelper(OnHotkeyPressed);
            _hotkeyHelper.ActivateHotkeys(Application.Current.MainWindow);
            WindowVisibility = Visibility.Hidden;
        }

        private void OnCmdSelectSlot(object param)
        {
            var slot = (ScreenSlot)param;

            if (_activeWindowHandle == IntPtr.Zero)
            {
                return;
            }

            var moveResult = NativeMethods.MoveWindow(
                _activeWindowHandle,
                (int)slot.Left,
                (int)slot.Top,
                (int)slot.Width,
                (int)slot.Height,
                true);

            if (moveResult)
            {
                WindowVisibility = Visibility.Hidden;
            }
            else
            {
                throw new Win32Exception(
                    $"Error while trying to move window\nHandle: {_activeWindowHandle}\nTitle: {ActiveWindowTitle}");
            }
        }

        private void OnHotkeyPressed()
        {
            ActiveWindowTitle = NativeMethods.GetWindowTitleUnderCursor();
            _activeWindowHandle = NativeMethods.GetWindowHandleUnderCursor();
            WindowVisibility = Visibility.Visible;
            Activated = true;

            var p = default(NativeMethods.Point);

            if (NativeMethods.GetCursorPos(ref p))
            {
                var windowLocation = GetAdjustedWindowCoordinates(p);
                WindowLeft = windowLocation.X;
                WindowTop = windowLocation.Y;
            }
        }

        private Point GetAdjustedWindowCoordinates(NativeMethods.Point mousePos)
        {
            double x = mousePos.X - 20;
            double y = mousePos.Y - 40;

            var scrWidth = SystemParameters.WorkArea.Width;
            var scrHeight = SystemParameters.WorkArea.Height;

            var wndWidth = WindowWidth;
            var wndHeight = WindowHeight;

            // cursor too far to the right
            if (mousePos.X + wndWidth > scrWidth)
            {
                x = scrWidth - wndWidth;
            }

            // cursor too far down
            if (mousePos.Y + wndHeight > scrHeight)
            {
                y = scrHeight - wndHeight;
            }

            return new Point((int)x, (int)y);
        }

        private void ReloadSlotsConfiguration()
        {
            Slots = new ObservableCollection<ScreenSlot>(ConfigManager.GetSlotConfig().Slots);
        }
    }
}
