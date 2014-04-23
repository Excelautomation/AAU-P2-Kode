using System.Collections.ObjectModel;
using System.Windows.Controls;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Administrationssystem
{
    public class SettingsViewModel : Base.ViewModel, IFilter
    {
        public ObservableCollection<Control> Filters
        {
            get { return new ObservableCollection<Control>(); }
        }
    }
}
