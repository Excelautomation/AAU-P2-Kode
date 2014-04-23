using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ARK.Administrationssystem
{
    /// <summary>
    /// Interaction logic for FilterControl.xaml
    /// </summary>
    public partial class FilterControl : UserControl, INotifyPropertyChanged
    {
        private string _filterName;
        private bool _isChecked;

        public FilterControl(string filterName)
        {
            InitializeComponent();

            FilterName = filterName;
            IsChecked = false;

            knap1.DataContext = this;
        }

        public string FilterName
        {
            get 
            { 
                return _filterName; 
            }
            set 
            { 
                _filterName = value; 
                Notify("FilterName"); 
            }
        }

        public bool IsChecked
        {
            get 
            { 
                return _isChecked; 
            }
            set 
            { 
                _isChecked = value; 
                Notify("IsChecked"); 
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void Notify(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void knap1_Click(object sender, RoutedEventArgs e)
        {
            IsChecked = !IsChecked;
        }
    }
}
