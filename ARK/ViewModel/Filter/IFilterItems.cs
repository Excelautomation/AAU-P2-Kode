using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.ViewModel.Filter
{
    interface IFilterItems<T>
    {
        bool FilterItem(T item);
    }
}
