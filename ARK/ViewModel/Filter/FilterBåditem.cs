using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ARK.ViewModel.Filter
{
    class FilterBåditem : IFilter<ARK.Administrationssystem.BådItem>
    {
        Dictionary<CheckBox, Func<ARK.Administrationssystem.BådItem, bool>> checkboxes;

        public FilterBåditem(Dictionary<CheckBox, Func<ARK.Administrationssystem.BådItem, bool>> checkboxes)
        {
            this.checkboxes = checkboxes;
        }

        public bool Active()
        {
            return (from c in checkboxes
                    where c.Key.IsChecked == true
                    select c).Count() > 0;
                
        }

        public bool FilterItem(Administrationssystem.BådItem item)
        {
            if (!Active())
                return true;

            //Filter
            foreach(var cb in checkboxes) {
                if (cb.Value(item))
                    return true;
            }

            return false;
        }

        public IEnumerable<Administrationssystem.BådItem> FilterItems(Administrationssystem.BådItem[] items)
        {
            return from b in items
                   where FilterItem(b)
                   select b;
        }
    }
}
