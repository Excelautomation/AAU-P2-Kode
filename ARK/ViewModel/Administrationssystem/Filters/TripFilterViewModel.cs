using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces.Filter;

namespace ARK.ViewModel.Administrationssystem.Filters
{
    public class TripFilterViewModel : ViewModelBase, IFilterViewModel
    {
        public event EventHandler<FilterEventArgs> FilterChanged;
    }
}
