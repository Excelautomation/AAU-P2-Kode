using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ARK.ViewModel
{
    public class PageInformation : INotifyPropertyChanged
    {
        private UserControl _page;
        private string _pageName;
        public ObservableCollection<Control> Filter { get { return ((IFilter)Page.DataContext).Filters; } }

        public UserControl Page { get { return _page; } set { _page = value; Notify("Page"); Notify("Filter"); Notify("ShowFilter"); Notify("ShowSearch"); } }

        public string PageName { get { return _pageName; } set { _pageName = value; Notify("PageName"); } }
        public Visibility ShowFilter
        {
            get
            {
                return Filter.Count == 0 ? Visibility.Hidden : Visibility.Visible;
            }
        }

        public Visibility ShowSearch
        {
            get
            {
                return Visibility.Visible;
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

        #endregion Property
    }
}