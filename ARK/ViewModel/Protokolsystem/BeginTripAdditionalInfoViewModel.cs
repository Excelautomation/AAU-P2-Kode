using System.Collections.ObjectModel;
using ARK.Model;
using ARK.ViewModel.Interfaces;

namespace ARK.ViewModel.Protokolsystem
{
    public class BeginTripAdditionalInfoViewModel : ContentViewModelBase,
        IInfoContentViewModel<BeginTripAdditionalInfoViewModel>
    {
        private ObservableCollection<Boat> _selectedBoat;
        private ObservableCollection<MemberViewModel> _selectedMembers;

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

        public void RemoveMember(MemberViewModel member)
        {
            SelectedMembers.Remove(member);
        }
    }
}