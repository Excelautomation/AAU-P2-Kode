using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using ARK.HelperFunctions.SMSGateway;
using ARK.Model;
using ARK.Model.DB;
using ARK.Model.XML;
using ARK.View;
using ARK.View.Administrationssystem;
using ARK.View.Protokolsystem;
using ARK.ViewModel.Protokolsystem;

namespace ARK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //var window = new AdminLogin();
            var window = new AdminSystem();
            ShowWindow(window);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var window = new ProtocolSystem();
            ShowWindow(window);
        }

        private void ShowWindow(Window window)
        {
            window.Show();
            Hide();

            window.Closing += (sender, e) => Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //SmsWarnings warn = new SmsWarnings();
            //warn.Test();

            //XmlParser.UpdateDataFromFtp();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var window = new AdminLogin();
            ShowWindow(window);
        }
    }
}
