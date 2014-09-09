using System.Windows.Controls;

namespace ARK.View.Administrationssystem.Pages
{
    /// <summary>
    /// Interaction logic for FormsDamage.xaml
    /// </summary>
    public partial class FormsDamage : UserControl
    {
        public FormsDamage()
        {
            InitializeComponent();
        }

        private void OnManipulationBoundaryFeedback(object sender, System.Windows.Input.ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
    }
}