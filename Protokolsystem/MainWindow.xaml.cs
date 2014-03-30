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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Protokolsystem
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

        private void BeginTrip_Click(object sender, RoutedEventArgs e)
        {
            BeginTripBoats boatCollection = new BeginTripBoats();

            Content.Children.Clear();
            Content.Children.Add(boatCollection);
        }

        private void EndTrip_Click(object sender, RoutedEventArgs e)
        {
            EndTrip boatEndtrip = new EndTrip();

            Content.Children.Clear();
            Content.Children.Add(boatEndtrip);
        }

        private void boatsOverview_Click(object sender, RoutedEventArgs e)
        {
            BoatsOut boatOut = new BoatsOut();

            Content.Children.Clear();
            Content.Children.Add(boatOut);
        }

        private void DistanceStatistics_Click(object sender, RoutedEventArgs e)
        {
            DistanceStatistics distanceMembers = new DistanceStatistics();

            Content.Children.Clear();
            Content.Children.Add(distanceMembers);
        }

        private void MembersInformation_Click(object sender, RoutedEventArgs e)
        {
            MembersInformation membersInfo = new MembersInformation();

            Content.Children.Clear();
            Content.Children.Add(membersInfo);
        }

        private void CreateInjury_Click(object sender, RoutedEventArgs e)
        {
            CreateInjury injury = new CreateInjury();

            Content.Children.Clear();
            Content.Children.Add(injury);
        }

        private void CreateLongDistance_Click(object sender, RoutedEventArgs e)
        {
            CreateLongDistance longDistance = new CreateLongDistance();

            Content.Children.Clear();
            Content.Children.Add(longDistance);
        }

        private void AdminPanel_Click(object sender, RoutedEventArgs e)
        {
            AdminLogin admin = new AdminLogin();

            admin.Show();
        }
    }
}
