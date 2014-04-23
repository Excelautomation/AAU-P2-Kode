using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ARK.ViewModel.Base
{
    public class PageInformation : ViewModel
    {
        private UserControl _page;
        private string _pageName;
        public ObservableCollection<Control> Filter 
        { 
            get { return ((IFilter)Page.DataContext).Filters; } 
        }

        public UserControl Page { 
            get 
            { 
                return _page; 
            } 
            set 
            { 
                _page = value; 
                Notify(); 
                NotifyProp("Filter"); 
                NotifyProp("ShowFilter"); 
                NotifyProp("ShowSearch"); 
            } 
        }

        public string PageName 
        { 
            get 
            { 
                return _pageName; 
            } 
            set 
            { 
                _pageName = value; 
                Notify(); 
            } 
        }
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
    }
}