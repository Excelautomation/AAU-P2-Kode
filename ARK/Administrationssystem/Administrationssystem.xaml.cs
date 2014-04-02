using ARK.Administrationssystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ARK.Administrationssystem
{
    /// <summary>
    /// Interaction logic for Administrationssystem.xaml
    /// </summary>
    public partial class Administrationssystem : Window
    {
        ObservableCollection<FilterControl> filterControls = new ObservableCollection<FilterControl>();

        public Administrationssystem()
        {
            InitializeComponent();

            //for (int i = 0; i < 4; i++)
            //    Filters.Children.Add(new FilterControl("FilterNavn" + i));


            CheckBox[] oversigt_checkbox_arr = new CheckBox[] {
                new CheckBox() { Name="oversigt_cb1", Content="Både på vandet"},
                new CheckBox() { Name="oversigt_cb2", Content="Skadesblanketter"},
                new CheckBox() { Name="oversigt_cb3", Content="Langtursansøgninger"},
                new CheckBox() { Name="oversigt_cb4", Content="Bådtype1" },
                new CheckBox() { Name="oversigt_cb5", Content="Bådtype2"},
                new CheckBox() { Name="oversigt_cb6", Content="Bådtype3"},
                new CheckBox() { Name="oversigt_cb7", Content="Bådtype4"}
            };

            // Jeg ville godt have en lille streg ind mellem langtursblanketter og bådtype1, men det er ikke en prioritet.
            foreach (CheckBox ch in oversigt_checkbox_arr)      
            {
                Stackpanelfilter.Children.Add(ch);
            }
            
        }

        // Skift vinduer i maincontent
        private void btnMenuHome_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.Clear();
            MainContent.Children.Add(new Oversigt());

            // duplikeret fra Administrationssystem(). Grimt grimt.
            CheckBox[] oversigt_checkbox_arr = new CheckBox[] {
                new CheckBox() { Name="oversigt_cb1", Content="Både på vandet"},
                new CheckBox() { Name="oversigt_cb2", Content="Skadesblanketter"},
                new CheckBox() { Name="oversigt_cb3", Content="Langtursansøgninger"},
                new CheckBox() { Name="oversigt_cb4", Content="Bådtype1" },
                new CheckBox() { Name="oversigt_cb5", Content="Bådtype2"},
                new CheckBox() { Name="oversigt_cb6", Content="Bådtype3"},
                new CheckBox() { Name="oversigt_cb7", Content="Bådtype4"}
            };

            Stackpanelfilter.Children.Clear();
            foreach (CheckBox cb in oversigt_checkbox_arr)
            {
                Stackpanelfilter.Children.Add(cb);
            }
        }
        private void btnMenuBlanketter_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.Clear();
            MainContent.Children.Add(new Pages.Blanketter());

            CheckBox[] Blanketter_checkbox_arr = new CheckBox[] {
                new CheckBox() { Name="blank_cb1", Content="Skadesblanketter"},
                new CheckBox() { Name="blank_cb2", Content="Langtursblanketter"},
                new CheckBox() { Name="blank_cb3", Content="Ulæste"},
                new CheckBox() { Name="blank_cb4", Content="Læste"},
            };

            Stackpanelfilter.Children.Clear();
            foreach (CheckBox cb in Blanketter_checkbox_arr)
            {
                Stackpanelfilter.Children.Add(cb);
            }

        }
        private void btnMenuIndstillinger_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.Clear();
            MainContent.Children.Add(new Pages.Indstillinger());
        }
    }
}
