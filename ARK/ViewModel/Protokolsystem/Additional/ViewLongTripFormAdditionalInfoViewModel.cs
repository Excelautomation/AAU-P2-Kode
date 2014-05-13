using System.Linq;
using System.Windows.Input;
using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Interfaces;
using System.Collections.ObjectModel;

namespace ARK.ViewModel.Protokolsystem.Additional
{
    public class ViewLongTripFormAdditionalInfoViewModel : ContentViewModelBase
    {
        // Fields
        private LongTripForm _selectedLongTripForm;

        public LongTripForm SelectedLongTripForm
        {
            get { return _selectedLongTripForm; }
            set { _selectedLongTripForm = value; }
        }
        

    }
}