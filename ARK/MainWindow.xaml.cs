using System.Windows;

using ARK.Model.XML;
using ARK.View.Administrationssystem;
using ARK.View.Protokolsystem;

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
            // var window = new AdminLogin();
            var window = new AdminSystem();
            ShowWindow(window);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var window = new ProtocolSystem();
            ShowWindow(window);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            XmlParser.UpdateMembersFromFtp();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var window = new AdminLogin();
            ShowWindow(window);
        }

        private void ShowWindow(Window window)
        {
            window.Show();
            Hide();

            window.Closing += (sender, e) => Close();
        }
    }
}