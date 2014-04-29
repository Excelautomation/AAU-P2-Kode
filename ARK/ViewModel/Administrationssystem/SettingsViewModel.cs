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
    public class SettingsViewModel : ContentViewModelBase, IDisposable
    {
        private DbArkContext dbcontext;

        #region Constructor
        public SettingsViewModel()
        {
            dbcontext = new DbArkContext();

            DamageTypes = new ObservableCollection<DamageType>(dbcontext.DamageType);
            StandardTrips = new ObservableCollection<StandardTrip>(dbcontext.StandardTrip);
            Admins = new ObservableCollection<Admin>(dbcontext.Admin);
        }
        #endregion

        public void Dispose()
        {
            dbcontext.Dispose();
        }

        #region Skadetyper
        private ObservableCollection<DamageType> _damageTypes;
        public ObservableCollection<DamageType> DamageTypes
        {
            get { return _damageTypes; }
            set { _damageTypes = value; Notify(); }
        }

        private DamageType _currentDamageType;
        public DamageType CurrentDamageType
        {
            get { return _currentDamageType; }
            set { _currentDamageType = value; Notify(); }
        }

        public ICommand SelectedChangeDamageType
        {
            get
            {
                return GetCommand<DamageType>(e => { CurrentDamageType = e; });
            }
        }
        #endregion

        #region Standardture
        private ObservableCollection<StandardTrip> _standardTrips;
        public ObservableCollection<StandardTrip> StandardTrips
        {
            get { return _standardTrips; }
            set { _standardTrips = value; Notify(); }
        }

        private StandardTrip _currentStandardTrip;
        public StandardTrip CurrentStandardTrip
        {
            get { return _currentStandardTrip; }
            set { _currentStandardTrip = value; Notify(); }
        }

        public ICommand SelectedChangeStandardTrip
        {
            get
            {
                return GetCommand<StandardTrip>(e => { CurrentStandardTrip = e; });
            }
        }
        #endregion

        #region Administratorer
        private ObservableCollection<Admin> _admins;
        public ObservableCollection<Admin> Admins
        {
            get { return _admins; }
            set { _admins = value; Notify(); }
        }

        private Admin _currentAdmin;
        public Admin CurrentAdmin
        {
            get { return _currentAdmin; }
            set { _currentAdmin = value; Notify(); }
        }

        private Member _currentAdminMember;
        public Member CurrentAdminMember
        {
            get
            {
                return _currentAdmin.Member;
            }
        }


        public ICommand SelectedChangeAdmin
        {
            get
            {
                return GetCommand<Admin>(e => { CurrentAdmin = e; });
            }
        }
        #endregion
    }
}
