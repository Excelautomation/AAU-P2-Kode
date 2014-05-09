using System.Windows.Controls;
using ARK.ViewModel.Protokolsystem.Data;
using ARK.ViewModel.Protokolsystem.Pages;

namespace ARK.View.Protokolsystem.Pages
{
    /// <summary>
    /// Interaction logic for EndTrip.xaml
    /// </summary>
    public partial class EndTrip : UserControl
    {
        public EndTrip()
        {
            InitializeComponent();
        }

        private void lstStandardTrip_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.lstStandardTrip.SelectionChanged -= this.lstStandardTrip_SelectionChanged;

            var selectedStdTrips = this.lstStandardTrip.SelectedItems;
            if (selectedStdTrips.Count > 1)
            {
                selectedStdTrips.Clear();
                selectedStdTrips.Add(e.AddedItems[0]);
            }

            this.lstStandardTrip.SelectionChanged += this.lstStandardTrip_SelectionChanged;
        }
    }
}
