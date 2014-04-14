using System.Collections.ObjectModel;
using System.Windows.Controls;
using ARK.ViewModel.Base;

namespace ARK.ViewModel
{
    public class FormsViewModel : Base.ViewModel, IFilter
    {
        private ObservableCollection<Control> _filters;

        public ObservableCollection<Control> Filters
        {
            get
            {
                return _filters ?? (_filters = new ObservableCollection<Control>
                {
                    new CheckBox {Content = "Langtur"},
                    new CheckBox {Content = "Skader"},
                    new Separator {Height = 20},
                    new CheckBox {Content = "Afviste"},
                    new CheckBox {Content = "Godkendte"}
                });
            }
        }
    }
}