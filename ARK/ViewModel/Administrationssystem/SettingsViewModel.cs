using System.Collections.ObjectModel;
using System.Windows.Controls;
using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Administrationssystem
{
    public class SettingsViewModel : ContentViewModelBase
    {
        private ObservableCollection<DamageType> _damageTypes;

        public SettingsViewModel()
        {
            using (var dbcontext = new DbArkContext())
            {
                DamageTypes = new ObservableCollection<DamageType>(dbcontext.DamageType);
            }
        }

        public ObservableCollection<DamageType> DamageTypes
        {
            get { return _damageTypes; }
            set { _damageTypes = value; Notify(); }
        }
    }
}
