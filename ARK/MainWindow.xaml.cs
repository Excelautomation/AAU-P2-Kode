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
        #region Constructors and Destructors

        public MainWindow()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // var window = new AdminLogin();
            var window = new AdminSystem();
            this.ShowWindow(window);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var window = new ProtocolSystem();
            this.ShowWindow(window);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // SmsWarnings warn = new SmsWarnings();
            // warn.Test();

            // XmlParser.UpdateDataFromFtp();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var window = new AdminLogin();
            this.ShowWindow(window);
        }

        private void ShowWindow(Window window)
        {
            window.Show();
            this.Hide();

            window.Closing += (sender, e) => this.Close();
        }

        #endregion
    }
}