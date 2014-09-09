using System.Windows.Controls;

namespace ARK.View.Protokolsystem.Pages
{
    /// <summary>
    /// Interaction logic for BeginTripBoats.xaml
    /// </summary>
    public partial class CreateLongTripForm : UserControl
    {
        public CreateLongTripForm()
        {
            InitializeComponent();
        }

        private void OnManipulationBoundaryFeedback(object sender, System.Windows.Input.ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
    }
}