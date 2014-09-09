using System.Windows;

namespace ARK.View.Administrationssystem.Pages
{
    /// <summary>
    /// Interaction logic for BoatListWindow.xaml
    /// </summary>
    public partial class BoatListWindow : Window
    {
        public BoatListWindow()
        {
            InitializeComponent();
        }

        private void OnManipulationBoundaryFeedback(object sender, System.Windows.Input.ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
    }
}