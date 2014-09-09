using System.Windows.Controls;

namespace ARK.View.Protokolsystem.Pages
{
    /// <summary>
    /// Interaction logic for DistanceStatistics.xaml
    /// </summary>
    public partial class DistanceStatistics : UserControl
    {
        public DistanceStatistics()
        {
            InitializeComponent();
        }

        private void OnManipulationBoundaryFeedback(object sender, System.Windows.Input.ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
    }
}