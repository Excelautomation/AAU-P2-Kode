using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ARK.ViewModel
{
    public class PageInformation : INotifyPropertyChanged
    {
        private string _pageName;
        private UserControl _page;
        private ObservableCollection<Control> _filter;

        public string PageName { get { return _pageName; } set { _pageName = value; Notify("PageName"); } }
        public UserControl Page { get { return _page; } set { _page = value; Notify("Page"); } }
        public ObservableCollection<Control> Filter { get { return _filter; } set { _filter = value; Notify("Filter"); Notify("ShowFilter"); Notify("ShowSearch"); } }

        public Visibility ShowSearch
        {
            get
            {
                return Visibility.Visible;
            }
        }

        public Visibility ShowFilter
        {
            get
            {
                return Filter.Count == 0 ? Visibility.Hidden : Visibility.Visible;
            }
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