using System.Linq;
using System.Windows.Input;
using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Interfaces;
using System.Collections.ObjectModel;
using ARK.ViewModel.Protokolsystem.Data;

namespace ARK.ViewModel.Protokolsystem.Additional
{
    public class BeginTripAdditionalInfoViewModel : ContentViewModelBase
    {
        // Fields
        private ObservableCollection<Boat> _selectedBoat;
        private ObservableCollection<MemberViewModel> _selectedMembers;
        private int _boatNumberOfSeats; // Default is 0

        // Properties
        public ObservableCollection<Boat> SelectedBoat
        {
            get { return _selectedBoat; }
            set
            {
                _selectedBoat = value;

                // Update number of seats
                if (value.Count > 0 && value[0] != null) 
                {
                    BoatNumberOfSeats = value[0].NumberofSeats;
                }
                else
                {
                    BoatNumberOfSeats = 0;
                }
                Notify();
            }
        }

        public int BoatNumberOfSeats
        {
            get { return _boatNumberOfSeats; }
            private set 
            { 
                _boatNumberOfSeats = value;
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
        public ICommand RemoveMember
        {
            get
            {
                return GetCommand<MemberViewModel>(member =>
                {
                    if (member == null) return;

                    if (member.Member.Id < 0)
                    {
                        var temp = SelectedMembers.ToList();
                        temp.Remove(member);

                        SelectedMembers.Clear();
                        foreach (var m in temp)
                            SelectedMembers.Add(m);
                    }
                    else
                        SelectedMembers.Remove(member);
                });
            }
        }
    }
}