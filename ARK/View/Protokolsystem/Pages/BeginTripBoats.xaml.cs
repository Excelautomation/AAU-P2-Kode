using System.Windows.Controls;

using ARK.ViewModel.Protokolsystem.Data;
using ARK.ViewModel.Protokolsystem.Pages;

namespace ARK.View.Protokolsystem.Pages
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
            var vm = (BeginTripViewModel)DataContext;
            var selectedMembers = AllMembersList.SelectedItems;

            if (vm.SelectedMembers.Count > vm.SelectedBoat.NumberofSeats)
            {
                vm.SelectedMembers.Remove((MemberViewModel)e.AddedItems[0]);
            }
        }

        private void OnManipulationBoundaryFeedback(object sender, System.Windows.Input.ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
    }
}