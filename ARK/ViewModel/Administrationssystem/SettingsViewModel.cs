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
        public enum Feedback
        {
            Default, Save, Cancel, Delete, Create
        }
        private DbArkContext _db;

        // General page
        private Season _currentSeason;

        public SettingsViewModel()
        {
            _db = DbArkContext.GetDbContext();

            lock (_db)
            {
                DamageTypes = new ObservableCollection<DamageType>(_db.DamageType);
                StandardTrips = new ObservableCollection<StandardTrip>(_db.StandardTrip);
                Admins = new ObservableCollection<Admin>(_db.Admin);
                Members = new ObservableCollection<Member>(_db.Member);
            }

            if (Admins.Any())
            {
                SelectedListItemAdmins = 0;
                CurrentAdmin = Admins[0];
                ReferenceToCurrentAdmin = CurrentAdmin;
            }
            if (Admins.Any())
            {
                SelectedListItemDamageTypes = 0;
                CurrentDamageType = DamageTypes[0];
                ReferenceToCurrentDamageType = CurrentDamageType;
            }
            if (Admins.Any())
            {
                SelectedListItemStandardTrips = 0;
                CurrentStandardTrip = StandardTrips[0];
                ReferenceToCurrentStandardTrip = CurrentStandardTrip;
            }

            if (!_db.Season.Any(x => true))
            {
                CurrentSeason = new Season();
                _db.Season.Add(CurrentSeason);
            }
            else
            {
                CurrentSeason = _db.Season.AsEnumerable().Last(x => true);
            }
        }

        #region Generelt
        private bool _displaySeasonErrorLabel = false;

        public bool DisplaySeasonErrorLabel
        {
            get { return _displaySeasonErrorLabel; }
            set { _displaySeasonErrorLabel = value; Notify(); }
        }

        public Season CurrentSeason
        {
            get { return _currentSeason; }
            set { _currentSeason = value; Notify(); }
        }

        public DateTime Today { get { return DateTime.Now; } }

        public ICommand NewSeason
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    // if current season started less then 183 days ago promt the user!
                    if (CurrentSeason != null && DateTime.Compare(CurrentSeason.SeasonStart.AddDays(183), DateTime.Now) > 0 )
                    {
                        // promp the user that the current season is less than a half year old!
                        //throw new NotImplementedException();
                        DisplaySeasonErrorLabel = true;
                    }
                    else
                    {
                        CurrentSeason.SeasonEnd = DateTime.Now;
                        Season tmpSeason = new Season();
                        _db.Season.Add(tmpSeason);
                        CurrentSeason = tmpSeason;
                        _db.SaveChanges();
                        DisplaySeasonErrorLabel = false;
                    }
                });
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
                        Type = e.Type
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
                    ReferenceToCurrentDamageType.Type = CurrentDamageType.Type;
                    _db.SaveChanges();

                    DamageTypes = new ObservableCollection<DamageType>(_db.DamageType.ToList());
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
                        Type = ReferenceToCurrentDamageType.Type
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
                        Type = "Ny skadetype"
                    };
                    _db.DamageType.Add(DamageTypeTemplate);
                    _db.SaveChanges();
                    DamageTypes.Add(DamageTypeTemplate);
                    SelectedListItemDamageTypes = DamageTypes.Count - 1;
                    FeedbackDamageType = Feedback.Create;
                });
            }
        }

        public ICommand DeleteSkadeType
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    _db.DamageType.Remove(ReferenceToCurrentDamageType);
                    _db.SaveChanges();
                    DamageTypes.Remove(ReferenceToCurrentDamageType);
                    SelectedListItemDamageTypes = DamageTypes.Count - 1;
                    FeedbackDamageType = Feedback.Delete;
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
                    _db.SaveChanges();

                    // Loader igen fra HELE databasen, og sætter ind i listview.
                    // Bør optimseres til kun at loade den ændrede query.
                    StandardTrips = new ObservableCollection<StandardTrip>(_db.StandardTrip.ToList());
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
                    _db.StandardTrip.Add(StandardTripTemplate);
                    _db.SaveChanges();
                    StandardTrips.Add(StandardTripTemplate);
                    SelectedListItemStandardTrips = StandardTrips.Count - 1;
                    FeedbackStandardTrip = Feedback.Create;
                });
            }
        }

        public ICommand DeleteStandardTrip
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    _db.StandardTrip.Remove(ReferenceToCurrentStandardTrip);
                    _db.SaveChanges();
                    StandardTrips.Remove(ReferenceToCurrentStandardTrip);
                    SelectedListItemStandardTrips = StandardTrips.Count - 1;
                    FeedbackDamageType = Feedback.Delete;
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
        private int _selectedListItemAdmins;
        private Admin _NewAdmin = new Admin();

        public Admin NewAdmin
        {
            get { return _NewAdmin; }
            set
            {
                _NewAdmin = value; Notify();
            }
        }

        public int SelectedListItemAdmins
        {
            get { return _selectedListItemAdmins; }
            set { _selectedListItemAdmins = value; Notify(); }
        }

        public Feedback FeedbackAdmin
        {
            get { return _feedbackAdmin; }
            set { _feedbackAdmin = value; Notify(); }
        }

        public Admin ReferenceToCurrentAdmin
        {
            get { return _referenceToCurrentAdmin; }
            set { _referenceToCurrentAdmin = value; Notify(); }
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
                    int tempindex = SelectedListItemAdmins;
                    
                    ReferenceToCurrentAdmin.ContactTrip = CurrentAdmin.ContactTrip;
                    ReferenceToCurrentAdmin.ContactDark = CurrentAdmin.ContactDark;
                    _db.SaveChanges();

                    Admins = new ObservableCollection<Admin>(_db.Admin.ToList());
                    SelectedListItemAdmins = tempindex;
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
                    NewAdmin = new Admin();
                    
                    MembersListWindow = new View.Administrationssystem.Pages.MembersListWindow();
                    MembersListWindow.DataContext = this;
                    MembersListWindow.ShowDialog();

                    if (NewAdmin.Member == null)
                        return;

                    Admin AdminTemplate = new Admin()
                    {
                        Username = NewAdmin.Username,
                        Password = NewAdmin.Password,
                        ContactTrip = false,
                        ContactDark = false,
                        Member = NewAdmin.Member
                    };
                    _db.Admin.Add(AdminTemplate);
                    Admins.Add(AdminTemplate);
                    _db.SaveChanges();
                    FeedbackAdmin = Feedback.Create;
                    SelectedListItemAdmins = Admins.Count - 1;
                });
            }
        }

        public ICommand DeleteAdmin
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    if (Admins.Count == 1)
                    {
                        System.Windows.MessageBox.Show("Sidste administrator kan ikke slettes!");
                        return;
                    }
                    _db.Admin.Remove(ReferenceToCurrentAdmin);
                    _db.SaveChanges();
                    Admins.Remove(ReferenceToCurrentAdmin);
                    FeedbackAdmin = Feedback.Delete;
                    SelectedListItemAdmins = Admins.Count - 1;
                });
            }
        }

        public ICommand ShowMembersContinue
        {
            get
            {
                return GetCommand<Member>(e =>
                {
                    NewAdmin.Member = e;
                    MembersListWindow.Close();
                });
            }
        }

        #endregion
    }
}
