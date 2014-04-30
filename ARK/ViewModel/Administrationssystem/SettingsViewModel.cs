using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Administrationssystem
{
    public class SettingsViewModel : ContentViewModelBase
    {
        #region Generelt
        private DbArkContext _dbcontext;

        public SettingsViewModel()
        {
            _dbcontext = DbArkContext.GetDbContext();

            // Templates til oprettelse af entries
            DamageTypeTemplate.Title = "Ny skadetype";
            DamageTypeTemplate.IsFunctional = true;
            DamageTypeTemplate.Description = "En beskrivelse.";
            StandardTripTemplate.Description = "Ny standardtur";
            StandardTripTemplate.Distance = 0;
            StandardTripTemplate.Direction = "En beskrivelse.";
            AdminTemplate.Username = "Ny administrator";
            AdminTemplate.Contact = false;
            AdminTemplate.Password = "kode1234";

            // Virker vidst ikke -> Ingen forskel, hvis LoadData står alane.
            // Funktionen skulle gerne refreshe indhold af entries ved menu-skift.
            ParentAttached += (sender, e) =>
            {
                LoadData();
            };
        }

        private void LoadData()
        {
            DamageTypes = new ObservableCollection<DamageType>(_dbcontext.DamageType);
            StandardTrips = new ObservableCollection<StandardTrip>(_dbcontext.StandardTrip);
            Admins = new ObservableCollection<Admin>(_dbcontext.Admin);
        }


        public ICommand ChangeTab
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    LoadData();
                    Notify();
                });
            }
        }
        #endregion

        #region Skadetyper
        private DamageType DamageTypeTemplate = new DamageType();
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
                return GetCommand<DamageType>(e =>
                {
                    CurrentDamageType = e;
                    Notify();
                });
            }
        }

        public ICommand SaveChangesSkadeTyper
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    _dbcontext.SaveChanges();
                    System.Windows.MessageBox.Show("Gem knap");
                });
            }
        }

        public ICommand CancelChangesSkadeTyper
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    System.Windows.MessageBox.Show("Annulér knap");
                });
            }
        }

        public ICommand CreateSkadeType
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    _dbcontext.DamageType.Add(DamageTypeTemplate);
                    _dbcontext.SaveChanges();
                    DamageTypes.Add(DamageTypeTemplate);
                    System.Windows.MessageBox.Show("Opret knap");
                });
            }
        }

        public ICommand DeleteSkadeType
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    _dbcontext.DamageType.Remove(CurrentDamageType);
                    _dbcontext.SaveChanges();
                    DamageTypes.Remove(CurrentDamageType);
                    System.Windows.MessageBox.Show("Slet knap");
                });
            }
        }
        #endregion

        #region Standardture
        private StandardTrip StandardTripTemplate = new StandardTrip();
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
                return GetCommand<StandardTrip>(e =>
                {
                    CurrentStandardTrip = e;
                });
            }
        }

        public ICommand SaveChangesStandardTrips
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    _dbcontext.SaveChanges();
                    System.Windows.MessageBox.Show("Gem knap");
                });
            }
        }

        public ICommand CancelChangesStandardTrips
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    System.Windows.MessageBox.Show("Annulér knap");
                });
            }
        }

        public ICommand CreateStandardTrip
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    _dbcontext.StandardTrip.Add(StandardTripTemplate);
                    _dbcontext.SaveChanges();
                    StandardTrips.Add(StandardTripTemplate);
                    System.Windows.MessageBox.Show("Opret knap");
                });
            }
        }

        public ICommand DeleteStandarTrip
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    _dbcontext.StandardTrip.Remove(CurrentStandardTrip);
                    _dbcontext.SaveChanges();
                    StandardTrips.Remove(CurrentStandardTrip);
                    System.Windows.MessageBox.Show("Slet knap");
                });
            }
        }
        #endregion

        #region Administratorer
        private Admin AdminTemplate = new Admin();
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

        public ICommand SelectedChangeAdmin
        {
            get
            {
                return GetCommand<Admin>(e => { CurrentAdmin = e; });
            }
        }

        public ICommand SaveChangesAdmins
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    _dbcontext.SaveChanges();
                    System.Windows.MessageBox.Show("Gem knap");
                });
            }
        }

        public ICommand CancelChangesAdmins
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    System.Windows.MessageBox.Show("Annulér knap");
                });
            }
        }

        public ICommand CreateAdmin
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    // Virker ikke! Der lader til at være problemer ved at binde foreign (Medlemmer)key til admin.
                    Admin NewAdmin = new Admin();
                    NewAdmin.Member = _dbcontext.Member.Find(855);
                    _dbcontext.Admin.Add(NewAdmin);
                    _dbcontext.SaveChanges();
                    Admins.Add(NewAdmin);
                    System.Windows.MessageBox.Show("Opret knap");
                });
            }
        }

        public ICommand DeleteAdmin
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    _dbcontext.Admin.Remove(CurrentAdmin);
                    _dbcontext.SaveChanges();
                    Admins.Remove(CurrentAdmin);
                    System.Windows.MessageBox.Show("Slet knap");
                });
            }
        }
        #endregion
    }
}
