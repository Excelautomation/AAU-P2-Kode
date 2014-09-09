using System.Windows.Controls;

namespace ARK.View.Administrationssystem.Pages
{
    /// <summary>
    /// Interaction logic for Indstillinger.xaml
    /// </summary>
    public partial class Indstillinger : UserControl
    {
        public Indstillinger()
        {
            InitializeComponent();
        }

        private void OnManipulationBoundaryFeedback(object sender, System.Windows.Input.ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
    }
}