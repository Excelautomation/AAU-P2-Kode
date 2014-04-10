using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace ARK.ViewModel
{
    public class BoatViewModel : IFilter
    {
        private ObservableCollection<Control> _filters;

        public ObservableCollection<Control> Filters
        {
            get
            {
                return _filters ?? (_filters = new ObservableCollection<Control>
                {
                    new CheckBox {Content = "Både ude"},
                    new CheckBox {Content = "Både hjemme"},
                    new Separator {Height = 20},
                    new CheckBox {Content = "Både under reparation"},
                    new CheckBox {Content = "Beskadigede både"},
                    new CheckBox {Content = "Inaktive både"},
                    new CheckBox {Content = "Funktionelle både"}
                });
            }
        }
    }
}