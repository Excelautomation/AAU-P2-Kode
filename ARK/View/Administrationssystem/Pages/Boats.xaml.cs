using System.Windows.Controls;

namespace ARK.View.Administrationssystem.Pages
{
    /// <summary>
    /// Interaction logic for Baede.xaml
    /// </summary>
    public partial class Baede : UserControl
    {
        public Baede()
        {
            InitializeComponent();
        }

        private void OnManipulationBoundaryFeedback(object sender, System.Windows.Input.ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
    }
}