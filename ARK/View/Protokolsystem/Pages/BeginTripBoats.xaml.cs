using System.Collections.ObjectModel;
using System.Windows.Controls;
using ARK.ViewModel.Protokolsystem;
using System.Linq;

namespace ARK.Protokolsystem.Pages
{
    /// <summary>
    /// Interaction logic for BeginTripBoats.xaml
    /// </summary>
    public partial class BeginTripBoats : UserControl
    {
        public BeginTripBoats()
        {
            InitializeComponent();
        }

        private void AllMembersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = (BeginTripViewModel)this.DataContext;
            var selectedMembers = this.AllMembersList.SelectedItems;

            if (vm.SelectedMembers.Count > vm.SelectedBoat.NumberofSeats)
            {
                vm.SelectedMembers.Remove((MemberViewModel) e.AddedItems[0]);
            }
        }
    }
}
