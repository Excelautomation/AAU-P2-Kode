using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.ViewModel.Base.Interfaces.Filter
{
    public interface IFilterViewModel
    {
        IFilterContainerViewModel FilterContainer { get; set; }
    }
}
