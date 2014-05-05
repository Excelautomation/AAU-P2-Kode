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


            if (Admins.Count != 0)
            {
                CurrentAdminInt = 0;
            }
            if (DamageTypes.Count != 0)
            {
                SelectedListItemDamageTypes = 0;
                CurrentDamageType = DamageTypes[0];
            }
            if (StandardTrips.Count != 0)
            {
                SelectedListItemStandardTrips = 0;
                CurrentStandardTrip = StandardTrips[0];
            }
        }


        #endregion

        #region Skadetyper
        private DamageType ReferenceToCurrentDamageType;
        private ObservableCollection<DamageType> _damageTypes;
        private DamageType _currentDamageType;
        private Feedback _feedbackDamageType;
        private int _selectedListItemDamageTypes;

        public int SelectedListItemDamageTypes
        {
            get { return _selectedListItemDamageTypes; }
            set { _selectedListItemDamageTypes = value; Notify(); }
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
        private StandardTrip ReferenceToCurrentStandardTrip;
        private ObservableCollection<StandardTrip> _standardTrips;
        private StandardTrip _currentStandardTrip;
        private Feedback _feedbackStandardTrip;
        private int _selectedListItemStandardTrips;

        public int SelectedListItemStandardTrips
        {
            get { return _selectedListItemStandardTrips; }
            set { _selectedListItemStandardTrips = value; Notify(); }
        }


        public Feedback FeedbackStandardTrip
        {
            get { return _feedbackStandardTrip; }
            set { _feedbackStandardTrip = value; Notify(); }
        }


        public ObservableCollection<StandardTrip> StandardTrips
        {
            get { return _standardTrips; }
            set { _standardTrips = value; Notify(); }
        }


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
                    if (e == null) return;

                    CurrentStandardTrip = new StandardTrip()
                    {
                        Distance = e.Distance,
                        Direction = e.Direction,
                        Title = e.Title
                    };
                    ReferenceToCurrentStandardTrip = e;

                    FeedbackStandardTrip = Feedback.Default;
                });
            }
        }

        public ICommand SaveChangesStandardTrips
        {
            get
            {
                return GetCommand<StandardTrip>(e =>
                {
                    ReferenceToCurrentStandardTrip.Distance = CurrentStandardTrip.Distance;
                    ReferenceToCurrentStandardTrip.Title = CurrentStandardTrip.Title;
                    ReferenceToCurrentStandardTrip.Direction = CurrentStandardTrip.Direction;
                    _dbcontext.SaveChanges();

                    // Loader igen fra HELE databasen, og sætter ind i listview.
                    // Bør optimseres til kun at loade den ændrede query.
                    StandardTrips = new ObservableCollection<StandardTrip>(_dbcontext.StandardTrip.ToList());
                    FeedbackStandardTrip = Feedback.Save;
                });
            }
        }

        public ICommand CancelChangesStandardTrips
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    CurrentStandardTrip = new StandardTrip()
                    {
                        Distance = ReferenceToCurrentStandardTrip.Distance,
                        Direction = ReferenceToCurrentStandardTrip.Direction,
                        Title = ReferenceToCurrentStandardTrip.Title
                    };
                    FeedbackStandardTrip = Feedback.Cancel;
                });
            }
        }

        public ICommand CreateStandardTrip
        {
            get
            {
                return GetCommand<StandardTrip>(e =>
                {
                    StandardTrip StandardTripTemplate = new StandardTrip()
                    {
                        Title = "Ny standardtur",
                        Direction = "Vest",
                        Distance = 5
                    };
                    _dbcontext.StandardTrip.Add(StandardTripTemplate);
                    _dbcontext.SaveChanges();
                    StandardTrips.Add(StandardTripTemplate);
                    FeedbackStandardTrip = Feedback.Create;
                    SelectedListItemStandardTrips = StandardTrips.Count - 1;
                });
            }
        }

        public ICommand DeleteStandardTrip
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    _dbcontext.StandardTrip.Remove(ReferenceToCurrentStandardTrip);
                    _dbcontext.SaveChanges();
                    StandardTrips.Remove(ReferenceToCurrentStandardTrip);
                    FeedbackDamageType = Feedback.Delete;
                    SelectedListItemStandardTrips = StandardTrips.Count - 1;
                });
            }
        }
        #endregion

        #region Administratorer
        private Admin _referenceToCurrentAdmin;
        private ObservableCollection<Admin> _admins;
        private Admin _currentAdmin;
        private ObservableCollection<Member> _members;
        public MembersListWindow MembersListWindow;
        private Feedback _feedbackAdmin;

        public Feedback FeedbackAdmin
        {
            get { return _feedbackAdmin; }
            set { _feedbackAdmin = value; Notify(); }
        }

        private int _CurrentAdminInt;

        public Admin ReferenceToCurrentAdmin
        {
            get { return _referenceToCurrentAdmin; }
            set { _referenceToCurrentAdmin = value; Notify(); }
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

        public ICommand SelectedChangeAdmin
        {
            get
            {
                return GetCommand<Admin>(e =>
                {
                    if (e == null) return;

                    CurrentAdmin = new Admin()
                    {
                        Username = e.Username,
                        Password = e.Password,
                        ContactTrip = e.ContactTrip,
                        ContactDark = e.ContactDark,
                        Member = e.Member
                    };
                    ReferenceToCurrentAdmin = e;

                    FeedbackAdmin = Feedback.Default;
                });
            }
        }

        public ICommand SaveChangesAdmins
        {
            get
            {
                return GetCommand<Admin>(e =>
                {
                    ReferenceToCurrentAdmin.ContactTrip = CurrentAdmin.ContactTrip;
                    ReferenceToCurrentAdmin.ContactDark = CurrentAdmin.ContactDark;
                    _dbcontext.SaveChanges();

                    Admins = new ObservableCollection<Admin>(_dbcontext.Admin.ToList());
                    FeedbackAdmin = Feedback.Save;
                });
            }
        }

        public ICommand CancelChangesAdmins
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    CurrentAdmin = new Admin()
                    {
                        Username = ReferenceToCurrentAdmin.Username,
                        Password = ReferenceToCurrentAdmin.Password,
                        Member = ReferenceToCurrentAdmin.Member,
                        ContactDark = ReferenceToCurrentAdmin.ContactDark,
                        ContactTrip = ReferenceToCurrentAdmin.ContactTrip
                    };
                    FeedbackAdmin = Feedback.Cancel;
                });
            }
        }

        public ICommand CreateAdmin
        {
            get
            {
                return GetCommand<Admin>(e =>
                {
                    MembersListWindow = new View.Administrationssystem.Pages.MembersListWindow();
                    MembersListWindow.DataContext = this;
                    MembersListWindow.ShowDialog();

                    Admin AdminTemplate = new Admin()
                    {
                        Username = CurrentAdmin.Username,
                        Password = CurrentAdmin.Password,
                        ContactTrip = false,
                        ContactDark = false,
                        Member = CurrentAdmin.Member
                    };
                    _dbcontext.Admin.Add(AdminTemplate);
                    _dbcontext.SaveChanges();
                    Admins.Add(AdminTemplate);
                    FeedbackAdmin = Feedback.Create;
                    CurrentAdminInt = Admins.Count - 1;
                });
            }
        }

        public ICommand DeleteAdmin
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    _dbcontext.Admin.Remove(ReferenceToCurrentAdmin);
                    _dbcontext.SaveChanges();
                    Admins.Remove(ReferenceToCurrentAdmin);
                    FeedbackAdmin = Feedback.Delete;
                    CurrentAdminInt = Admins.Count - 1;
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
                });
            }
        }

        #endregion
    }
}
