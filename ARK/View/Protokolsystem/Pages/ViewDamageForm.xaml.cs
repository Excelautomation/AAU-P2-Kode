using System.Windows.Controls;

namespace ARK.View.Protokolsystem.Pages
{
    /// <summary>
    /// Interaction logic for ActiveTrips.xaml
    /// </summary>
    public partial class ViewDamageForm : UserControl
    {
        public ViewDamageForm()
        {
            InitializeComponent();
        }

        private void OnManipulationBoundaryFeedback(object sender, System.Windows.Input.ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
    }
}