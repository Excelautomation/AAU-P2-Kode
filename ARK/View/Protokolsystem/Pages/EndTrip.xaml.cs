using System.Windows.Controls;

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
            lstStandardTrip.SelectionChanged -= lstStandardTrip_SelectionChanged;

            var selectedStdTrips = lstStandardTrip.SelectedItems;
            if (selectedStdTrips.Count > 1)
            {
                selectedStdTrips.Clear();
                selectedStdTrips.Add(e.AddedItems[0]);
            }

            lstStandardTrip.SelectionChanged += lstStandardTrip_SelectionChanged;
        }

        private void OnManipulationBoundaryFeedback(object sender, System.Windows.Input.ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
    }
}