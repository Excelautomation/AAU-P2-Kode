using System.Windows.Controls;

namespace ARK.View.Administrationssystem.Pages
{
    /// <summary>
    /// Interaction logic for FormsLongTrip.xaml
    /// </summary>
    public partial class FormsLongTrip : UserControl
    {
        public FormsLongTrip()
        {
            InitializeComponent();
        }

        private void OnManipulationBoundaryFeedback(object sender, System.Windows.Input.ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
    }
}