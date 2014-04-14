using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using ARK.ViewModel.Base;

namespace ARK.ViewModel
{
    public class SettingsViewModel : Base.ViewModel, IFilter
    {
        public ObservableCollection<Control> Filters
        {
            get { return new ObservableCollection<Control>(); }
        }
    }
}
