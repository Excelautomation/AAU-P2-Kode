using System.ComponentModel;
using System.Windows.Controls;

namespace ARK.Administrationssystem
{
    public partial class Header : UserControl, INotifyPropertyChanged
    {
        private string _title;

        public Header()
        {
            InitializeComponent();

            // Sæt datacontext
            HeaderTitleText.DataContext = this;
        }

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
