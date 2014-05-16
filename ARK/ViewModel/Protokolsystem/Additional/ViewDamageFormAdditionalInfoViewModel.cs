using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Interfaces;

namespace ARK.ViewModel.Protokolsystem.Additional
{
    public class ViewDamageFormAdditionalInfoViewModel : ContentViewModelBase
    {
        // Fields
        #region Fields

        private DamageForm _selectedDamageForm;

        #endregion

        // Properties
        #region Public Properties

        public DamageForm SelectedDamageForm
        {
            get
            {
                return this._selectedDamageForm;
            }

            set
            {
                this._selectedDamageForm = value;
                this.Notify();
            }
        }

        #endregion
    }
}