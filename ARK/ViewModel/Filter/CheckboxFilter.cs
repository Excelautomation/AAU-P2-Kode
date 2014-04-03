using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ARK.ViewModel.Filter
{
    class CheckboxFilter<T> : FilterItems<T>
    {
        protected CheckBox checkbox;

        CheckboxFilter(Predicate<T> predicate, CheckBox checkbox) 
            : base(predicate) 
        {
            this.checkbox = checkbox;
        }

        public override bool Active()
        {
            return (bool) checkbox.IsChecked;
        }
    }
}
