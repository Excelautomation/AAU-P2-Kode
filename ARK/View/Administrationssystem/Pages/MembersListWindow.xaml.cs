using System.Windows;

namespace ARK.View.Administrationssystem.Pages
{
    /// <summary>
    /// Interaction logic for MembersListWindow.xaml
    /// </summary>
    public partial class MembersListWindow : Window
    {
        public MembersListWindow()
        {
            InitializeComponent();
        }

        private void OnManipulationBoundaryFeedback(object sender, System.Windows.Input.ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
    }
}