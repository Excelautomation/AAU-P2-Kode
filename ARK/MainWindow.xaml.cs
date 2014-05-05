using System;
using System.Windows;
using System.Windows.Controls;
using ARK.Administrationssystem;
using ARK.Model.XML;
using ARK.View;
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
            //var window = new Windows8Fuck();
            //ShowWindow(window);

            //XmlParser.LoadBoatsFromXml();
            //XmlParser.LoadMembersFromXml();
            //XmlParser.LoadTripsFromXml();
        }
    }
}
