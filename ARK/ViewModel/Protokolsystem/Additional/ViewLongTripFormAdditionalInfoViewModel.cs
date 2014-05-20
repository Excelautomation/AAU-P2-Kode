using ARK.Model;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Protokolsystem.Additional
{
    public class ViewLongTripFormAdditionalInfoViewModel : ContentViewModelBase
    {
        // Fields
        private LongTripForm _selectedLongTripForm;

        public LongTripForm SelectedLongTripForm
        {
            get
            {
                return _selectedLongTripForm;
            }

            set
            {
                _selectedLongTripForm = value;
                Notify();
            }
        }
    }
}