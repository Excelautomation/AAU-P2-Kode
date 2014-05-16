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
        #region Constructors and Destructors

        public BeginTripBoats()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        private void AllMembersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = (BeginTripViewModel)this.DataContext;
            var selectedMembers = this.AllMembersList.SelectedItems;

            if (vm.SelectedMembers.Count > vm.SelectedBoat.NumberofSeats)
            {
                vm.SelectedMembers.Remove((MemberViewModel)e.AddedItems[0]);
            }
        }

        #endregion
    }
}