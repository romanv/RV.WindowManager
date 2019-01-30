namespace RV.WM2.SlotEditor.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using Microsoft.TeamFoundation.MVVM;

    using RV.WM2.Infrastructure.Core;
    using RV.WM2.Infrastructure.Models;
    using RV.WM2.Infrastructure.MVVM;

    public class MainViewModel : BrowsableObject
    {
        private ScreenSlotViewModel _selectedSlotViewModel;
        private SlotsConfigurationViewModel _slotsConfigurationViewModel;

        private ICommand _cmdAddNewSlot;
        private ICommand _cmdRemoveSelectedSlot;
        private ICommand _cmdCancelChanges;
        private ICommand _cmdSaveChanges;
        private ICommand _cmdSnapSelectedSlot;
        private ICommand _cmdStretchSelectedSlotV;
        private ICommand _cmdStretchSelectedSlotH;

        private bool _hasChanges;

        public MainViewModel()
        {
            ScreenModelWidth = SystemParameters.WorkArea.Width / 2;
            ScreenModelHeight = SystemParameters.WorkArea.Height / 2;

            ReloadSlotsConfiguration();
        }

        public double ScreenModelWidth { get; }

        public double ScreenModelHeight { get; }

        public SlotsConfigurationViewModel SlotsConfigurationViewModel
        {
            get
            {
                return _slotsConfigurationViewModel;
            }

            set
            {
                if (_slotsConfigurationViewModel == value)
                {
                    return;
                }

                _slotsConfigurationViewModel = value;
                RaisePropertyChanged();
            }
        }

        public ICommand CmdRemoveSelectedSlot
            => _cmdRemoveSelectedSlot ?? (_cmdRemoveSelectedSlot = new RelayCommand(OnCmdRemoveSelectedSlot));

        public ICommand CmdAddNewSlot => _cmdAddNewSlot ?? (_cmdAddNewSlot = new RelayCommand(OnCmdAddNewSlot));

        public ICommand CmdCancelChanges
            => _cmdCancelChanges ?? (_cmdCancelChanges = new RelayCommand(OnCmdCancelChanges, CanExecuteSaveCancelCmds));

        public ICommand CmdSaveChanges
            => _cmdSaveChanges ?? (_cmdSaveChanges = new RelayCommand(OnCmdSaveChanges, CanExecuteSaveCancelCmds));

        public ICommand CmdSnapSelectedSlot
            => _cmdSnapSelectedSlot ?? (_cmdSnapSelectedSlot = new RelayCommand(OnCmdSnapSelectedSlot));

        public ICommand CmdStretchSelectedSlotH
            => _cmdStretchSelectedSlotH ?? (_cmdStretchSelectedSlotH = new RelayCommand(OnCmdStretchSelectedSlotH));

        public ICommand CmdStretchSelectedSlotV
            => _cmdStretchSelectedSlotV ?? (_cmdStretchSelectedSlotV = new RelayCommand(OnCmdStretchSelectedSlotV));

        public ScreenSlotViewModel SelectedSlotViewModel
        {
            get
            {
                return _selectedSlotViewModel;
            }

            set
            {
                if (_selectedSlotViewModel == value)
                {
                    return;
                }

                if (_selectedSlotViewModel != null)
                {
                    // do not track INPC of the vm that we are unselecting
                    _selectedSlotViewModel.PropertyChanged -= SetChanges;
                }

                _selectedSlotViewModel = value;

                if (value != null)
                {
                    _selectedSlotViewModel.PropertyChanged += SetChanges;
                }

                RaisePropertyChanged();
            }
        }

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

        private void SetChanges(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != nameof(ScreenSlotViewModel.HasChanges))
            {
                return;
            }

            HasChanges = true;
        }

        private bool CanExecuteSaveCancelCmds(object obj)
        {
            return HasChanges;
        }

        private void OnCmdRemoveSelectedSlot()
        {
            if (SelectedSlotViewModel != null)
            {
                SlotsConfigurationViewModel.RemoveSlot(SelectedSlotViewModel);
                HasChanges = true;
            }
        }

        private void OnCmdAddNewSlot()
        {
            var newSlot = new ScreenSlot
                              {
                                  Name = "New Slot",
                                  Height = 512,
                                  Width = 512,
                                  Left = (SystemParameters.WorkArea.Width - 512) / 2,
                                  Top = (SystemParameters.WorkArea.Height - 512) / 2,
                              };

            SlotsConfigurationViewModel.AddSlot(newSlot);

            // select if is the only slot
            if (SlotsConfigurationViewModel.Slots.Count == 1)
            {
                SelectedSlotViewModel = SlotsConfigurationViewModel.Slots.First();
            }

            HasChanges = true;
        }

        private void OnCmdSaveChanges(object o)
        {
            SlotsConfigurationViewModel.SaveChanges();
            HasChanges = false;
        }

        private void OnCmdCancelChanges(object o)
        {
            ReloadSlotsConfiguration();
        }

        private void OnCmdSnapSelectedSlot(object param)
        {
            var prms = ((string)param).Split(',');
            int x = Convert.ToByte(prms[0]);
            int y = Convert.ToByte(prms[1]);

            switch (x)
            {
                case 0:
                    SelectedSlotViewModel.Left = 0;
                    break;
                case 1:
                    SelectedSlotViewModel.Left = (SystemParameters.WorkArea.Width - SelectedSlotViewModel.Width) / 2;
                    break;
                case 2:
                    SelectedSlotViewModel.Left = SystemParameters.WorkArea.Width - SelectedSlotViewModel.Width;
                    break;
            }

            switch (y)
            {
                case 0:
                    SelectedSlotViewModel.Top = 0;
                    break;
                case 1:
                    SelectedSlotViewModel.Top = (SystemParameters.WorkArea.Height - SelectedSlotViewModel.Height) / 2;
                    break;
                case 2:
                    SelectedSlotViewModel.Top = SystemParameters.WorkArea.Height - SelectedSlotViewModel.Height;
                    break;
            }
        }

        private void OnCmdStretchSelectedSlotH()
        {
            SelectedSlotViewModel.Width = SystemParameters.WorkArea.Width;
            SelectedSlotViewModel.Left = 0;
        }

        private void OnCmdStretchSelectedSlotV()
        {
            SelectedSlotViewModel.Height = SystemParameters.WorkArea.Height;
            SelectedSlotViewModel.Top = 0;
        }

        private void ReloadSlotsConfiguration()
        {
            SlotsConfigurationViewModel = new SlotsConfigurationViewModel(ConfigManager.GetSlotConfig());
            SelectedSlotViewModel = SlotsConfigurationViewModel.Slots.FirstOrDefault();
            HasChanges = false;
        }
    }
}
