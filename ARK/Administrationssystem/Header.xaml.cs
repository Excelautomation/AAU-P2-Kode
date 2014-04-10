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

namespace ARK.Administrationssystem
{
    public partial class Header : UserControl, INotifyPropertyChanged
    {
        public string Title 
        { 
            get 
            { 
                return _title; 
            } 
            set 
            { 
                _title = value; 
                Notify("Title"); 
            } 
        }

        private string _title;

        public Header()
        {
            InitializeComponent();

            // Sæt datacontext
            HeaderTitleText.DataContext = this;
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
