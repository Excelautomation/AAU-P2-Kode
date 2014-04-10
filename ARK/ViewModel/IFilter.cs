using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace ARK.ViewModel
{
    public interface IFilter
    {
        ObservableCollection<Control> Filters { get; }
    }
}