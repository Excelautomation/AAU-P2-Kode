using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace ARK.ViewModel.Base
{
    public interface IFilter
    {
        ObservableCollection<Control> Filters { get; }
    }
}