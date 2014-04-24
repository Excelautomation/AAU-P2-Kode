using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ARK.ViewModel.Base;

namespace ARK.ViewModel
{
    public class PageInformation : ViewModelBase
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
                NotifyCustom("Filter"); 
                NotifyCustom("ShowFilter"); 
                NotifyCustom("ShowSearch"); 
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