using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.Model.Search
{
    public interface IFilter<TType, TControl>
    {
        TControl Control { get; set; }

        bool Active { get; set; }
        Func<TType, bool> Filter { get; set; }

        event EventHandler ActiveChanged;
    }
}
