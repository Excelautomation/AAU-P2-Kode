using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Interfaces;
using ARK.ViewModel.Base.Interfaces.Info;
using System.Collections.ObjectModel;

namespace ARK.ViewModel.Protokolsystem.Additional
{
    public class BeginTripAdditionalInfoViewModel : ContentViewModelBase,
        IInfoContentViewModel<BeginTripAdditionalInfoViewModel>
    {
        // Fields
        private ObservableCollection<Boat> _selectedBoat;
        private ObservableCollection<MemberViewModel> _selectedMembers;

        public BeginTripAdditionalInfoViewModel Info
        {
            get { return this; }
            set
            {
                SelectedBoat = value.SelectedBoat;
                SelectedMembers = value.SelectedMembers;
                Notify();
            }
        }

        // Properties
        public ObservableCollection<Boat> SelectedBoat
        {
            get { return _selectedBoat; }
            set
            {
                _selectedBoat = value;
                Notify();
            }
        }

        public ObservableCollection<MemberViewModel> SelectedMembers
        {
            get { return _selectedMembers; }
            set
            {
                _selectedMembers = value;
                Notify();
            }
        }

        // Methods
        public void RemoveMember(MemberViewModel member)
        {
            SelectedMembers.Remove(member);
        }
    }
}