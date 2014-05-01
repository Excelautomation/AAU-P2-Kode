using System.Windows;
using ARK.Administrationssystem;
using ARK.Administrationssystem.Funktioner;
using ARK.Model;

namespace ARK.View.Protokolsystem
{
    /// <summary>
    /// Interaction logic for Protokolsystem.xaml
    /// </summary>
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

        private void TestFTP_Click(object sender, RoutedEventArgs e)
        {
            var sms = new SMS { Reciever = "4522345676", Message = "Trorlrl", Name = "Nigga" };
            SMSIT.SendSMS(sms);
        }
    }
}