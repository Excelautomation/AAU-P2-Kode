using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.Interfaces
{
    interface ICollection<T, TFilter>
    {
        T SelectedItem { get; set; }

        List<T> Items { get; set; }
        
        void Sort(Func<T, TFilter> predicate);
    }
}
