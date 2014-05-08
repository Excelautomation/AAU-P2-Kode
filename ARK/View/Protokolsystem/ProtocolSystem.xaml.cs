using System.Windows;
using ARK.Model;
using ARK.View.Administrationssystem;

namespace ARK.View.Protokolsystem
{
    public partial class ProtocolSystem : Window
    {
        public ProtocolSystem()
        {
            InitializeComponent();
        }

        private void AdminPanel_Click(object sender, RoutedEventArgs e)
        {
            AdminLogin admin = new AdminLogin();
            admin.Show();

            Closing += (sender2, e2) => admin.Close();
        }
    }
}