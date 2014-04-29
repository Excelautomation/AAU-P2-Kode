using System.Windows.Controls;
using ARK.ViewModel.Protokolsystem;

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

            if (selectedMembers.Count > vm.Boat.NumberofSeats)
            {
                selectedMembers.Remove(e.AddedItems[0]);
            }
        }
    }
}
