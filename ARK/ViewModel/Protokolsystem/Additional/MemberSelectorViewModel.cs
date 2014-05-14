using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Protokolsystem.Data;

namespace ARK.ViewModel.Protokolsystem.Additional
{
    public class MemberSelectorViewModel : ContentViewModelBase
    {
        // Fields
        private Boat _selectedBoat;
        private ObservableCollection<MemberViewModel> _selectedMembers;
        private int _boatNumberOfSeats; // Default is 0

        // Properties
        public Boat SelectedBoat
        {
            get { return _selectedBoat; }
            set
            {
                _selectedBoat = value;

                // Update number of seats
                BoatNumberOfSeats = value != null ? value.NumberofSeats : 0;

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
                return GetCommand(member =>
                {
                    if (member == null) return;

                    var memberVm = (MemberViewModel) member;

                    if (memberVm.Member.Id < 0)
                    {
                        var temp = SelectedMembers.ToList();
                        temp.Remove(memberVm);

                        // Clear because of sync
                        SelectedMembers.Clear();
                        foreach (var m in temp)
                            SelectedMembers.Add(m);
                    }
                    else
                        SelectedMembers.Remove(memberVm);
                });
            }
        }

    }
}
