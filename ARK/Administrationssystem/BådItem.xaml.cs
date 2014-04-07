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
    /// <summary>
    /// Interaction logic for BådItem.xaml
    /// </summary>
    public partial class BådItem : UserControl, INotifyPropertyChanged
    {
        // TODO: Båd skal nok være af typen båd
        // Skal laves til dette http://msdn.microsoft.com/en-us/library/ms742521(v=vs.110).aspx
        public string Båd 
        { 
            get 
            { 
                return _båd; 
            } 
            set 
            { 
                _båd = value; 
                Notify("Båd"); 
            } 
        }
        public string Besked { get { return _besked; } set { _besked = value; Notify("Besked"); } }

        private string _båd;
        private string _besked;

        public BådItem()
        {
            InitializeComponent();

            txtBåd.DataContext = this;
            txtBesked.DataContext = this;
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
