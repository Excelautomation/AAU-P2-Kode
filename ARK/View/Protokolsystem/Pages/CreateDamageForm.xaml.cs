using System.Windows.Controls;

namespace ARK.View.Protokolsystem.Pages
{
    /// <summary>
    /// Interaction logic for CreateDamageForm.xaml
    /// </summary>
    public partial class CreateDamageForm : UserControl
    {
        public CreateDamageForm()
        {
            InitializeComponent();
        }

        private void OnManipulationBoundaryFeedback(object sender, System.Windows.Input.ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
    }
}