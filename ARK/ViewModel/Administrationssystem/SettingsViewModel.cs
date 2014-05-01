using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;
using System.Windows;
using ARK.View.Administrationssystem.Pages;

namespace ARK.ViewModel.Administrationssystem
{
    public class SettingsViewModel : ContentViewModelBase
    {
        #region Generelt
        public enum Feedback
        {
            Default, Save, Cancel, Delete, Create
        }
        private DbArkContext _dbcontext;

        public SettingsViewModel()
        {
            _dbcontext = DbArkContext.GetDbContext();

            lock (_dbcontext)
            {
                DamageTypes = new ObservableCollection<DamageType>(_dbcontext.DamageType);
                StandardTrips = new ObservableCollection<StandardTrip>(_dbcontext.StandardTrip);
                Admins = new ObservableCollection<Admin>(_dbcontext.Admin);
                Members = new ObservableCollection<Member>(_dbcontext.Member);
            }

            // Templates til oprettelse af entries
            StandardTripTemplate.Description = "Ny standardtur";
            StandardTripTemplate.Distance = 0;
            StandardTripTemplate.Direction = "En beskrivelse.";
            NewAdmin.Username = "Ny administrator";
            NewAdmin.Contact = false;
            NewAdmin.Password = "kode1234";
            NewAdmin.Member = null;

            if (Admins.Count != 0)
            {
                CurrentAdminInt = 0;
            }
            if (DamageTypes.Count != 0)
            {
                SelectedListItemDamageTypes = 0;
                CurrentDamageType = DamageTypes[0];
            }
            



        }


        #endregion

        #region Skadetyper
        private DamageType ReferenceToCurrentDamageType;
        private ObservableCollection<DamageType> _damageTypes;
        private DamageType _currentDamageType;
        private Feedback _feedbackDamageType;
        private int _SelectedListItemDamageTypes;

        public int SelectedListItemDamageTypes
        {
            get { return _SelectedListItemDamageTypes; }
            set { _SelectedListItemDamageTypes = value; Notify(); }    
        }

        public Feedback FeedbackDamageType
        {
            get { return _feedbackDamageType; }
            set { _feedbackDamageType = value; Notify(); }
        }

        public ObservableCollection<DamageType> DamageTypes
        {
            get { return _damageTypes; }
            set { _damageTypes = value; Notify(); }
        }

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
                    if (e == null) return;

                    CurrentDamageType = new DamageType()
                    {
                        Description = e.Description,
                        IsFunctional = e.IsFunctional,
                        Title = e.Title
                    };
                    ReferenceToCurrentDamageType = e;

                    FeedbackDamageType = Feedback.Default;
                });
            }
        }

        public ICommand SaveChangesSkadeTyper
        {
            get
            {
                return GetCommand<DamageType>(e =>
                {
                    ReferenceToCurrentDamageType.Description = CurrentDamageType.Description;
                    ReferenceToCurrentDamageType.Title = CurrentDamageType.Title;
                    ReferenceToCurrentDamageType.IsFunctional = CurrentDamageType.IsFunctional;
                    _dbcontext.SaveChanges();

                    // Loader igen fra HELE databasen, og sætter ind i listview.
                    // Bør optimseres til kun at loade den ændrede query.
                    DamageTypes = new ObservableCollection<DamageType>(_dbcontext.DamageType.ToList());
                    FeedbackDamageType = Feedback.Save;
                });
            }
        }

        public ICommand CancelChangesSkadeTyper
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    CurrentDamageType = new DamageType()
                    {
                        Description = ReferenceToCurrentDamageType.Description,
                        IsFunctional = ReferenceToCurrentDamageType.IsFunctional,
                        Title = ReferenceToCurrentDamageType.Title
                    };
                    FeedbackDamageType = Feedback.Cancel;
                });
            }
        }

        public ICommand CreateSkadeType
        {
            get
            {
                return GetCommand<DamageType>(e =>
                {
                    DamageType DamageTypeTemplate = new DamageType()
                    {
                        Title = "Ny skadetype",
                        IsFunctional = true,
                        Description = "En beskrivelse."
                    };
                    _dbcontext.DamageType.Add(DamageTypeTemplate);
                    _dbcontext.SaveChanges();
                    DamageTypes.Add(DamageTypeTemplate);
                    FeedbackDamageType = Feedback.Create;
                    SelectedListItemDamageTypes = DamageTypes.Count - 1;
                });
            }
        }

        public ICommand DeleteSkadeType
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    _dbcontext.DamageType.Remove(ReferenceToCurrentDamageType);
                    _dbcontext.SaveChanges();
                    DamageTypes.Remove(ReferenceToCurrentDamageType);
                    FeedbackDamageType = Feedback.Delete;
                    SelectedListItemDamageTypes = DamageTypes.Count - 1;
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

        private Admin NewAdmin = new Admin();
        private bool _NewAdminBool = false;
        private ObservableCollection<Admin> _admins;
        private Admin _currentAdmin;
        private ObservableCollection<Member> _members;
        public MembersListWindow MembersListWindow;
        private int _CurrentAdminInt;

        public bool NewAdminBool
        {
            get { return _NewAdminBool; }
            set 
            { 
                _NewAdminBool = value;
                Notify();
                
            }
        }

                public int CurrentAdminInt
        { 
            get { return _CurrentAdminInt; }
            set { _CurrentAdminInt = value; Notify(); }
        
        }


        public ObservableCollection<Admin> Admins
        {
            get { return _admins; }
            set { _admins = value; Notify(); }
        }

        public ObservableCollection<Member> Members
        {
            get { return _members; }
            set { _members = value; Notify(); }
        }

        public Admin CurrentAdmin
        {
            get { return _currentAdmin; }
            set { _currentAdmin = value; Notify(); }
        }

        #region ShowMembers related
        public ICommand ShowMembers
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    MembersListWindow = new View.Administrationssystem.Pages.MembersListWindow();
                    MembersListWindow.DataContext = this;
                    MembersListWindow.ShowDialog();
                });
            }
        }

        public ICommand ShowMembersContinue
        {
            get
            {
                return GetCommand<Member>(e =>
                {
                    CurrentAdmin.Member = e;
                    MembersListWindow.Close();
                    NewAdminBool = false;
                    NotifyCustom("CurrentAdmin");
                });
            }
        }

        #endregion

        public ICommand SelectedChangeAdmin
        {

            get
            {
                return GetCommand<Admin>(e => 
                { 
                    CurrentAdmin = e;
                    if (e != null)
                    {
                        if (e.Member != null)
                            NewAdminBool = false;
                        else
                            NewAdminBool = true;
                        Notify("NewAdminBool");
                    }
                });
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
                    CurrentAdmin = null;
                    Admins.Add(NewAdmin);
                    NewAdminBool = true;
                    CurrentAdminInt = Admins.Count - 1;
                    NotifyCustom("Admins");
                    CurrentAdmin = Admins[Admins.Count - 1];
                });
            }
        }

        public ICommand DeleteAdmin
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    Admins.Remove(CurrentAdmin);
                    CurrentAdmin = Admins[Admins.Count - 1];
                    CurrentAdminInt = Admins.Count - 1;
                });
            }
        }
        #endregion
    }
}
