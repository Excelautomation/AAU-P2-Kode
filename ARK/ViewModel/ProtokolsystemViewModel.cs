using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ARK.ViewModel
{
    class ProtokolsystemViewModel : INotifyPropertyChanged
    {
        public string HeadlineText
        {
            get { return _headlineText; }
            set { _headlineText = value; Notify("HeadlineText"); }
        }

        public UserControl CurrentPage
        {
            get
            {
                return _currentPage;
            }
            private set
            {
                _currentPage = value;
                Notify("CurrentPage");
            }
        }

        public ICommand StartRotur { 
            get {
                return new DelegateCommand<object>((e) =>
                {
                    HeadlineText = "Søren er endnu mere awesome";
                }, (e) => { return true; });
            }
        }

        #region Private
        private string _headlineText;
        private UserControl _currentPage;
        #endregion

        public ProtokolsystemViewModel()
        {
            HeadlineText = "Søren er awesome";
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
