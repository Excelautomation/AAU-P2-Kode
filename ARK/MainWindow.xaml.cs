using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
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
            var context = DbArkContext.GetDbContext();

            //var info = context.Boat.First();
            //var info2 = ARK.HelperFunctions.HelperFunctions.CloneObject<Boat>(info);
            //info2.Trips.First().Distance = 33;

            var temp = context.Boat.ToList();
            var temp2 = ARK.HelperFunctions.HelperFunctions.CloneCollection<Boat>(temp);
            temp.First().Name = "changed";

            Console.WriteLine();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var window = new AdminLogin();
            ShowWindow(window);
        }
    }
}
