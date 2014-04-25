using System.Windows.Controls;

namespace ARK.Administrationssystem.Pages
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

        #region SMS-Modul
        private void TimeOfDarkCheckBox_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (TimeOfDarkCheckBox.IsChecked.Value)
                TimeOfDarkTextBox.IsEnabled = false;
            else
                TimeOfDarkTextBox.IsEnabled = true;
        }
        #endregion

        #region Skadetyper
        #endregion

        #region Standardture
        #endregion

        #region Administratorer
        #endregion
    }
}
