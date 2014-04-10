using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
            var admin = new ARK.Administrationssystem.AdminLogin();
            admin.Show();

            this.Closing += (sender2, e2) => admin.Close();
        }

        private void TestFTP_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new DbArkContext())
            {
                FtpInfo ftp = new FtpInfo() { Id = 1, Hostname = "ftp.test.dk", Name = "Test FTP", Password = "1234password", Port = 21 };
                db.FTPInfo.Add(ftp);
            }
        }
    }
}