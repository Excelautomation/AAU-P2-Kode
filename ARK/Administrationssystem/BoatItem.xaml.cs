using System.ComponentModel;
using System.Windows.Controls;

namespace ARK.Administrationssystem
{
    /// <summary>
    /// Interaction logic for BådItem.xaml
    /// </summary>
    public partial class BådItem : UserControl, INotifyPropertyChanged
    {
        private string _besked;
        private string _båd;

        public BådItem()
        {
            InitializeComponent();

            txtBåd.DataContext = this;
            txtBesked.DataContext = this;
        }

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

        #region Property
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
