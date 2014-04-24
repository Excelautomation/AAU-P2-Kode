using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARK.ViewModel.Base;

namespace ARK.ViewModel
{
    public abstract class FilterContentViewModelBase : ContentViewModelBase
    {
        public IFilterContainerViewModel FilterContainer
        {
            get { return Parent as IFilterContainerViewModel; }
        }
    }
}
