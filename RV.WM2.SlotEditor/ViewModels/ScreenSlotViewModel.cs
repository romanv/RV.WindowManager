namespace RV.WM2.SlotEditor.ViewModels
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    using Microsoft.TeamFoundation.MVVM;

    using RV.WM2.Infrastructure.Models;
    using RV.WM2.Infrastructure.MVVM;
    using RV.WM2.SlotEditor.Utils;

    public class ScreenSlotViewModel : BrowsableObject
    {
        private bool _isSelected;
        private bool _hasChanges;

        private double _opacity;
        private int _zIndex;

        private SolidColorBrush _borderBrush = Brushes.DarkGreen;
        private Thickness _borderThickness = new Thickness(1);

        private ICommand _cmdMouseEnter;
        private ICommand _cmdMouseLeave;

        public ScreenSlotViewModel(ScreenSlot slot)
        {
            Slot = slot;
            Fill = new SolidColorBrush(
                    Color.FromRgb(
                        (byte)RandomGenerator.Instance.Next(255),
                        (byte)RandomGenerator.Instance.Next(255),
                        (byte)RandomGenerator.Instance.Next(255))
                    );
            Opacity = 0.5;
        }

        public string Name
        {
            get
            {
                return Slot.Name;
            }

            set
            {
                if (Slot.Name == value)
                {
                    return;
                }

                Slot.Name = value;
                HasChanges = true;
                RaisePropertyChanged();
            }
        }

        public double Left
        {
            get
            {
                return Slot.Left;
            }

            set
            {
                if (Slot.Left == value)
                {
                    return;
                }

                Slot.Left = value;
                HasChanges = true;
                RaisePropertyChanged();
            }
        }

        public double Top
        {
            get
            {
                return Slot.Top;
            }

            set
            {
                if (Slot.Top == value)
                {
                    return;
                }

                Slot.Top = value;
                HasChanges = true;
                RaisePropertyChanged();
            }
        }

        public double Height
        {
            get
            {
                return Slot.Height;
            }

            set
            {
                if (Slot.Height == value)
                {
                    return;
                }

                Slot.Height = value;
                HasChanges = true;
                RaisePropertyChanged();
            }
        }

        public double Width
        {
            get
            {
                return Slot.Width;
            }

            set
            {
                if (Slot.Width == value)
                {
                    return;
                }

                Slot.Width = value;
                HasChanges = true;
                RaisePropertyChanged();
            }
        }

        public SolidColorBrush Fill { get; }

        public SolidColorBrush BorderBrush
        {
            get
            {
                return _borderBrush;
            }

            set
            {
                if (_borderBrush.Equals(value))
                {
                    return;
                }

                _borderBrush = value;
                RaisePropertyChanged();
            }
        }

        public Thickness BorderThickness
        {
            get
            {
                return _borderThickness;
            }

            set
            {
                if (_borderThickness == value)
                {
                    return;
                }

                _borderThickness = value;
                RaisePropertyChanged();
            }
        }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                if (_isSelected == value)
                {
                    return;
                }

                _isSelected = value;

                if (value)
                {
                    SetOnTop();
                }
                else
                {
                    RemoveFromTop();
                }

                RaisePropertyChanged();
            }
        }

        public double Opacity
        {
            get
            {
                return _opacity;
            }

            set
            {
                if (_opacity == value)
                {
                    return;
                }

                _opacity = value;
                RaisePropertyChanged();
            }
        }

        public int ZIndex
        {
            get
            {
                return _zIndex;
            }

            set
            {
                if (_zIndex == value)
                {
                    return;
                }

                _zIndex = value;
                RaisePropertyChanged();
            }
        }

        public ICommand CmdMouseLeave => _cmdMouseLeave ?? (_cmdMouseLeave = new RelayCommand(OnCmdMouseLeave));

        public ICommand CmdMouseEnter => _cmdMouseEnter ?? (_cmdMouseEnter = new RelayCommand(OnCmdMouseEnter));

        // used to track changes in properties, which ment to be saved in config
        public bool HasChanges
        {
            get
            {
                return _hasChanges;
            }

            set
            {
                if (_hasChanges == value)
                {
                    return;
                }

                _hasChanges = value;
                RaisePropertyChanged();
            }
        }

        public ScreenSlot Slot { get; }

        private void OnCmdMouseLeave()
        {
            BorderBrush = Brushes.DarkGreen;
            BorderThickness = new Thickness(1);
        }

        private void OnCmdMouseEnter()
        {
            BorderBrush = Brushes.Red;
            BorderThickness = new Thickness(3);
        }

        private void SetOnTop()
        {
            Opacity = 0.9;
            ZIndex = 999;
        }

        private void RemoveFromTop()
        {
            Opacity = 0.5;
            ZIndex = 0;
        }
    }
}
