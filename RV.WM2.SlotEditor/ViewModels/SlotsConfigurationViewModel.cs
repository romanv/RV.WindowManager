namespace RV.WM2.SlotEditor.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using RV.WM2.Infrastructure.Core;
    using RV.WM2.Infrastructure.Models;
    using RV.WM2.Infrastructure.MVVM;

    public class SlotsConfigurationViewModel : BrowsableObject
    {
        private readonly SlotConfig _slotConfig;

        private ObservableCollection<ScreenSlotViewModel> _slots;

        public SlotsConfigurationViewModel(SlotConfig slotConfig)
        {
            _slotConfig = slotConfig;
            Slots = BuildSlotsViewModelCollection(slotConfig.Slots);
        }

        public ObservableCollection<ScreenSlotViewModel> Slots
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

        public void SaveChanges()
        {
            ConfigManager.SaveSlotConfig(_slotConfig);
        }

        public void AddSlot(ScreenSlot slot)
        {
            _slotConfig.Slots.Add(slot);
            Slots.Add(new ScreenSlotViewModel(slot));
        }

        public void RemoveSlot(ScreenSlotViewModel slot)
        {
            _slotConfig.Slots.Remove(slot.Slot);
            Slots.Remove(slot);
        }

        private ObservableCollection<ScreenSlotViewModel> BuildSlotsViewModelCollection(IEnumerable<ScreenSlot> newSlots)
        {
            return new ObservableCollection<ScreenSlotViewModel>(newSlots.Select(slot => new ScreenSlotViewModel(slot)).ToList());
        }
    }
}
