using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Interfaces;

namespace ARK.ViewModel.Protokolsystem.Additional
{
    public class ViewLongTripFormAdditionalInfoViewModel : ContentViewModelBase
    {
        // Fields
        #region Fields

        private LongTripForm _selectedLongTripForm;

        #endregion

        #region Public Properties

        public LongTripForm SelectedLongTripForm
        {
            get
            {
                return this._selectedLongTripForm;
            }

            set
            {
                this._selectedLongTripForm = value;
                this.Notify();
            }
        }

        #endregion
    }
}