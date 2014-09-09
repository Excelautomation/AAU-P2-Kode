using System.Windows.Controls;

namespace ARK.View.Administrationssystem.Pages
{
    /// <summary>
    /// Interaction logic for Trips.xaml
    /// </summary>
    public partial class Trips : UserControl
    {
        public Trips()
        {
            InitializeComponent();
        }

        private void OnManipulationBoundaryFeedback(object sender, System.Windows.Input.ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
    }
}