using System.Collections.Generic;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;

namespace ARK.ViewModel.Administrationssystem.Filters
{
    public class TripFilterViewModel : FilterViewModelBase
    {
        public override IEnumerable<Filter> GetFilter()
        {
            return new List<Filter>();
        }
    }
}