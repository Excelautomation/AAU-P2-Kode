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
        #region Fields

        private int _boatNumberOfSeats; // Default is 0

        private Boat _selectedBoat;

        private ObservableCollection<MemberViewModel> _selectedMembers;

        #endregion

        #region Public Properties

        public int BoatNumberOfSeats
        {
            get
            {
                return this._boatNumberOfSeats;
            }

            private set
            {
                this._boatNumberOfSeats = value;
                this.Notify();
            }
        }

        // Methods
        public ICommand RemoveMember
        {
            get
            {
                return this.GetCommand(
                    member =>
                        {
                            if (member == null)
                            {
                                return;
                            }

                            var memberVm = (MemberViewModel)member;

                            if (memberVm.Member.Id < 0)
                            {
                                var temp = this.SelectedMembers.ToList();
                                temp.Remove(memberVm);

                                // Clear because of sync
                                this.SelectedMembers.Clear();
                                foreach (var m in temp)
                                {
                                    this.SelectedMembers.Add(m);
                                }
                            }
                            else
                            {
                                this.SelectedMembers.Remove(memberVm);
                            }
                        });
            }
        }

        public Boat SelectedBoat
        {
            get
            {
                return this._selectedBoat;
            }

            set
            {
                this._selectedBoat = value;

                // Update number of seats
                this.BoatNumberOfSeats = value != null ? value.NumberofSeats : 0;

                this.Notify();
            }
        }

        public ObservableCollection<MemberViewModel> SelectedMembers
        {
            get
            {
                return this._selectedMembers;
            }

            set
            {
                this._selectedMembers = value;
                this.Notify();
            }
        }

        #endregion
    }
}