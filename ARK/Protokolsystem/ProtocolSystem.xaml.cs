using System.Windows;
using ARK.Administrationssystem;
using ARK.Administrationssystem.Funktioner;
using ARK.Model;

namespace ARK.Protokolsystem
{
    /// <summary>
    /// Interaction logic for Protokolsystem.xaml
    /// </summary>
    public partial class Protokolsystem : Window
    {
        public Protokolsystem()
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
            SMS sms = new SMS() { Reciever = "4522345676", Message = "Trorlrl", Name = "Nigga" };
            SMSIT.SendSMS(sms);
        }
    }
}