using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARK.Model;
using ARK.ViewModel.Base;
using System.Windows.Input;

namespace ARK.ViewModel.Protokolsystem
{
    public class BeginTripAdditionalInfoViewModel : ContentViewModelBase, IInfoContentViewModel<BeginTripAdditionalInfoViewModel>
    {
        private ObservableCollection<Boat> _selectedBoat;
        private ObservableCollection<Member> _selectedMembers;

        public BeginTripAdditionalInfoViewModel Info
        {
            get { return this; }
            set
            {
                this.SelectedBoat = value.SelectedBoat;
                this.SelectedMembers = value.SelectedMembers;
                Notify(); 
            }
        }

        public ObservableCollection<Boat> SelectedBoat
        {
            get { return _selectedBoat; }
            set { _selectedBoat = value; Notify(); }
        }

        public ObservableCollection<Member> SelectedMembers
        {
            get { return _selectedMembers; }
            set { _selectedMembers = value; Notify(); }
        }

        public ICommand MemberClicked
        {
            get
            {
                return GetCommand<Member>(x =>
                    {

                    });
            }
        }
    }
}
