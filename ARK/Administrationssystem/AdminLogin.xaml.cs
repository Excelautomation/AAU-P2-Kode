using System.Windows;

namespace ARK.Administrationssystem
{
    /// <summary>
    /// Interaction logic for AdminLogin.xaml
    /// </summary>
    public partial class AdminLogin : Window
    {
        public AdminLogin()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var window = new AdminSystem();
            window.Show();
            this.Close();
        }
    }
}
