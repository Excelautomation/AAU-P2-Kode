using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using ARK.Extensions;
using ARK.Model;
using ARK.Model.DB;
using ARK.Model.Search;

namespace ARK.ViewModel.Administrationssystem
{
    public class SettingsViewModel : ContentViewModelBase
    {


        #region Skadetyper
        private ObservableCollection<DamageType> _damageTypes;
        private DamageType _CurrentDamageType;

        public ObservableCollection<DamageType> DamageTypes
        {
            get { return _damageTypes; }
            set { _damageTypes = value; Notify(); }
        }

        public DamageType CurrentDamageType
        {
            get { return _CurrentDamageType; }
            set { _CurrentDamageType = value; Notify(); }
        }

        public ICommand SelectedChange
        {
            get
            {
                return GetCommand<DamageType>(e => { CurrentDamageType = e; });
            }
        }


        #endregion

        #region Constructor
        public SettingsViewModel()
        {
            using (var dbcontext = new DbArkContext())
            {
                DamageTypes = new ObservableCollection<DamageType>(dbcontext.DamageType);
            }
        }
        #endregion
    }
}
