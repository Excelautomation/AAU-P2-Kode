using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Administrationssystem
{
    /// <summary>
    /// Interaction logic for FilterControl.xaml
    /// </summary>
    public partial class FilterControl : UserControl, INotifyPropertyChanged
    {
        private bool _isChecked;
        private string _filterName;

        public string FilterName { get { return _filterName; } set { _filterName = value; Notify("FilterName"); } }
        public bool IsChecked { get { return _isChecked; } set { _isChecked = value; Notify("IsChecked"); } }

        public FilterControl(string filterName)
        {
            InitializeComponent();

            this.FilterName = filterName;
            this.IsChecked = false;

            knap1.DataContext = this;
        }

        private void knap1_Click(object sender, RoutedEventArgs e)
        {
            this.IsChecked = !this.IsChecked;
        }

        #region Property
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
