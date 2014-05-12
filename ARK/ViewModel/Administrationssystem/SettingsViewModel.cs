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
            Default, Save, Cancel, Delete, Create, Error
        }
        private DbArkContext _db;

        // General page
        private Season _currentSeason;

        public SettingsViewModel()
        {
            // Initialize database and get data in a single thread(lock)
            _db = DbArkContext.GetDbContext();
            lock (_db)
            {
                DamageTypes = new ObservableCollection<DamageType>(_db.DamageType.OrderBy(e => e.Type));
                StandardTrips = new ObservableCollection<StandardTrip>(_db.StandardTrip.OrderBy(e => e.Title));
                Admins = new ObservableCollection<Admin>(_db.Admin.OrderBy(e => e.Member.FirstName));
                Members = new ObservableCollection<Member>(_db.Member.OrderBy(e => e.FirstName));
            }

            // Initialise the selected items in the respective lists.
            if (Admins.Any())
            {
                SelectedAdmin = 0;
                CurrentAdmin = Admins[0];
                ReferenceToCurrentAdmin = CurrentAdmin;
            }
            if (DamageTypes.Any())
            {
                SelectedDamageType = 0;
                CurrentDamageType = DamageTypes[0];
                ReferenceToCurrentDamageType = CurrentDamageType;
            }
            if (StandardTrips.Any())
            {
                SelectedStandardTrip = 0;
                CurrentStandardTrip = StandardTrips[0];
                ReferenceToCurrentStandardTrip = CurrentStandardTrip;
            }

            // Check seasons
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

        #region General
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

        #region DamageTypes
        private DamageType ReferenceToCurrentDamageType;
        private ObservableCollection<DamageType> _damageTypes;
        private DamageType _currentDamageType;
        private Feedback _feedbackDamageType;
        private int _selectedDamageType;
        private string _errorDamageType;

        public string ErrorDamageType
        {
            get { return _errorDamageType; }
            set { _errorDamageType = value; Notify(); }
        }

        public int SelectedDamageType
        {
            get { return _selectedDamageType; }
            set { _selectedDamageType = value; Notify(); }
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
                    // If list is empty return safely
                    if (e == null) return;

                    // Create a copy of the selected damagetype from database
                    CurrentDamageType = new DamageType()
                    {
                        Type = e.Type
                    };
                    // Remember reference to the selected damagetype in the list synced with database
                    ReferenceToCurrentDamageType = e;

                    // Give feedback
                    FeedbackDamageType = Feedback.Default;
                });
            }
        }

        public ICommand SaveChangesDamageTypes
        {
            get
            {
                return GetCommand<DamageType>(e =>
                {
                    // Save the selected damagetype to the list synced with database
                    ReferenceToCurrentDamageType.Type = CurrentDamageType.Type;

                    // Try to save changes to databse
                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
                    {
                        // The sleceted damagetype has likely been deleted in database
                        DamageTypes = new ObservableCollection<DamageType>(_db.DamageType.OrderBy(m => m.Type));
                        ErrorDamageType = "Skadetypen blev ikke fundet i databasen!";
                        FeedbackDamageType = Feedback.Error;
                        return;
                    }

                    // Refresh database and give feedback
                    DamageTypes = new ObservableCollection<DamageType>(_db.DamageType.OrderBy(m => m.Type));
                    FeedbackDamageType = Feedback.Save;
                });
            }
        }

        public ICommand CancelChangesDamageTypes
        {
            get
            {
                return GetCommand<DamageType>(e =>
                {
                    // Revert changes made by user
                    // Copy from database to the selected damagetype
                    CurrentDamageType = new DamageType()
                    {
                        Type = ReferenceToCurrentDamageType.Type
                    };

                    // Give feedback
                    FeedbackDamageType = Feedback.Cancel;
                });
            }
        }

        public ICommand CreateDamageType
        {
            get
            {
                return GetCommand<DamageType>(e =>
                {
                    // Preconfigured damagetype to create
                    DamageType NewDamageType = new DamageType()
                    {
                        Type = "Ny skadetype"
                    };

                    // Add new damagetype
                    _db.DamageType.Add(NewDamageType);
                    _db.SaveChanges();
                    DamageTypes.Add(NewDamageType);

                    // Refresh from database, select new damagetype and give feedback
                    DamageTypes = new ObservableCollection<DamageType>(_db.DamageType.OrderBy(m => m.Type));
                    SelectedDamageType = DamageTypes.IndexOf(DamageTypes.First(m => m.Type == NewDamageType.Type));
                    FeedbackDamageType = Feedback.Create;
                });
            }
        }

        public ICommand DeleteDamageType
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    // Remove selected damagetype from list
                    _db.DamageType.Remove(ReferenceToCurrentDamageType);

                    // Try to save Changes to database
                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
                    {
                        // The selected damagetype has likely already been deleted
                        DamageTypes = new ObservableCollection<DamageType>(_db.DamageType.OrderBy(m => m.Type));
                        ErrorDamageType = "Skadetypen blev ikke fundet i databasen!";
                        FeedbackDamageType = Feedback.Error;
                        return;
                    }
                    // Refresh database, select last item and give feedback
                    DamageTypes = new ObservableCollection<DamageType>(_db.DamageType.OrderBy(m => m.Type));
                    SelectedDamageType = DamageTypes.Count - 1;
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
        private int _selectedStandardTrip;
        private string _errorStandardTrip;

        public string ErrorStandardTrip
        {
            get { return _errorStandardTrip; }
            set { _errorStandardTrip = value; Notify(); }
        }

        public int SelectedStandardTrip
        {
            get { return _selectedStandardTrip; }
            set { _selectedStandardTrip = value; Notify(); }
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
                    // If the list is empty return safely
                    if (e == null) return;

                    // Create a copy from the selected item
                    CurrentStandardTrip = new StandardTrip()
                    {
                        Distance = e.Distance,
                        Direction = e.Direction,
                        Title = e.Title
                    };
                    // Save reference to the original position of the selected item
                    ReferenceToCurrentStandardTrip = e;

                    // Give feedback
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
                    int tmptrip = CurrentStandardTrip.Id;

                    // Copies data from userinput to the list synched with the database
                    ReferenceToCurrentStandardTrip.Distance = CurrentStandardTrip.Distance;
                    ReferenceToCurrentStandardTrip.Title = CurrentStandardTrip.Title;
                    ReferenceToCurrentStandardTrip.Direction = CurrentStandardTrip.Direction;

                    // Try to save changes
                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
                    {
                        // The standardtrip has likely been deleted in the database
                        StandardTrips = new ObservableCollection<StandardTrip>(_db.StandardTrip.OrderBy(m => m.Title));
                        ErrorStandardTrip = "Standardturen blev ikke fundet i databasen!";
                        FeedbackStandardTrip = Feedback.Error;
                        return;
                    }

                    // Reload database and give feedback.
                    StandardTrips = new ObservableCollection<StandardTrip>(_db.StandardTrip.OrderBy(m => m.Title));
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
                    // Copies original info from database to the selected standardtrip
                    CurrentStandardTrip = new StandardTrip()
                    {
                        Distance = ReferenceToCurrentStandardTrip.Distance,
                        Direction = ReferenceToCurrentStandardTrip.Direction,
                        Title = ReferenceToCurrentStandardTrip.Title
                    };
                    // Give feedback
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
                    // Create an object with premade info
                    StandardTrip NewStandardTrip = new StandardTrip()
                    {
                        Title = "Ny standardtur",
                        Direction = "Vest",
                        Distance = 5
                    };

                    // Add the new object to list and database
                    _db.StandardTrip.Add(NewStandardTrip);
                    _db.SaveChanges();
                    StandardTrips.Add(NewStandardTrip);

                    // (hopefully) selects the new standardtrip and give feedback
                    StandardTrips = new ObservableCollection<StandardTrip>(_db.StandardTrip.OrderBy(m => m.Title));
                    SelectedStandardTrip = StandardTrips.IndexOf(StandardTrips.First(m => m.Title == NewStandardTrip.Title));
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
                    // Remove selected trip
                    _db.StandardTrip.Remove(ReferenceToCurrentStandardTrip);

                    // Try to save changes
                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
                    {
                        // The selected standardtrip has likely already been deleted from database
                        StandardTrips = new ObservableCollection<StandardTrip>(_db.StandardTrip.OrderBy(m => m.Title));
                        ErrorStandardTrip = "Standardturen blev ikke fundet i databasen!";
                        FeedbackStandardTrip = Feedback.Error;
                        return;
                    }
                    // Refresh database, select last standardtrip and give feedback
                    StandardTrips = new ObservableCollection<StandardTrip>(_db.StandardTrip.OrderBy(m => m.Title));
                    SelectedStandardTrip = StandardTrips.Count - 1;
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
        private int _selectedAdmin;
        private string _errorAdmin;
        private Admin _newAdmin;

        public string ErrorAdmin
        {
            get { return _errorAdmin; }
            set { _errorAdmin = value; Notify(); }
        }

        public Admin NewAdmin
        {
            get { return _newAdmin; }
            set { _newAdmin = value; Notify(); }
        }

        public int SelectedAdmin
        {
            get { return _selectedAdmin; }
            set { _selectedAdmin = value; Notify(); }
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
                    // If the list is empty return safely (Should not be possibly for admins)
                    if (e == null) return;

                    // Sets CurrentAdmin to contain a copy of the selected item
                    // This object contains the direct changes made by the user
                    CurrentAdmin = new Admin()
                    {
                        Username = e.Username,
                        Password = e.Password,
                        ContactTrip = e.ContactTrip,
                        ContactDark = e.ContactDark,
                        Member = e.Member
                    };
                    // Sets reference to the selected admin in database
                    ReferenceToCurrentAdmin = e;

                    // Give feedback
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
                    // Save a temp. reference to the selected admin
                    int tempindex = SelectedAdmin;
                    
                    // Retrieve changes from selected admin
                    ReferenceToCurrentAdmin.ContactTrip = CurrentAdmin.ContactTrip;
                    ReferenceToCurrentAdmin.ContactDark = CurrentAdmin.ContactDark;

                    // Try to save changes to database
                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
                    {
                        // The selected admin has likely been deleted
                        Admins = new ObservableCollection<Admin>(_db.Admin.OrderBy(m => m.Member.FirstName));
                        ErrorAdmin = "Administratoren blev ikke fundet i databasen!";
                        FeedbackAdmin = Feedback.Error;
                        return;
                    }

                    // Refresh from database, reselect the previously selected admin and give feedback
                    Admins = new ObservableCollection<Admin>(_db.Admin.ToList());
                    SelectedAdmin = tempindex;
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
                    // Reloads all the original values for the selected admin
                    CurrentAdmin = new Admin()
                    {
                        Username = ReferenceToCurrentAdmin.Username,
                        Password = ReferenceToCurrentAdmin.Password,
                        Member = ReferenceToCurrentAdmin.Member,
                        ContactDark = ReferenceToCurrentAdmin.ContactDark,
                        ContactTrip = ReferenceToCurrentAdmin.ContactTrip
                    };
                    // Give feedback through view
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
                    // Create a new object to store the info of the desired admin
                    NewAdmin = new Admin();
                    
                    // Initialize and open a new window for the user
                    // Get the required info for creating an admin through the new window
                    MembersListWindow = new View.Administrationssystem.Pages.MembersListWindow();
                    MembersListWindow.DataContext = this;
                    MembersListWindow.ShowDialog();
                    NewAdmin.ContactDark = false;
                    NewAdmin.ContactTrip = false;

                    // Refresh from database
                    Admins = new ObservableCollection<Admin>(_db.Admin.ToList());

                    // Check there already exists an admin with the same username og member
                    // Also check if user made a password and username
                    if (NewAdmin.Username.Length == 0 || NewAdmin.Password.Length == 0)
                    {
                        System.Windows.MessageBox.Show("Brugernavn og kodeord er påkrævet!");
                        return;
                    }
                    else if (Admins.Any(m => m.Member == NewAdmin.Member))
                    {
                        System.Windows.MessageBox.Show("Det valgte medlem er allerede administrator!");
                        return;
                    }
                    else if (Admins.Any(m => m.Username == NewAdmin.Username))
                    {
                        System.Windows.MessageBox.Show("Det ønskede brugernavn eksisterer allerede!");
                        return;
                    }

                    // Add the new admin to database and list
                    _db.Admin.Add(NewAdmin);
                    Admins.Add(NewAdmin);

                    // Try to save changes to database
                    _db.SaveChanges();

                    // Change focus in view to the last admin in the list and give feedback
                    SelectedAdmin = Admins.Count - 1;
                    FeedbackAdmin = Feedback.Create;
                });
            }
        }

        public ICommand DeleteAdmin
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    // Keep a temp. reference to the selected admin
                    // Refresh from database and select the desired admin again
                    string tmpId = ReferenceToCurrentAdmin.Username;
                    Admins = new ObservableCollection<Admin>(_db.Admin.ToList());
                    ReferenceToCurrentAdmin = Admins.First(m => m.Username == tmpId);

                    // Check if there's only one admin
                    if (Admins.Count == 1)
                    {
                        ErrorAdmin = "Sidste administrator kan ikke slettes!";
                        FeedbackAdmin = Feedback.Error;
                        return;
                    }

                    // Remove admin
                    _db.Admin.Remove(ReferenceToCurrentAdmin);
                    Admins.Remove(ReferenceToCurrentAdmin);

                    // Try to save changes.
                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
                    {
                        // The admin has likely already been deleted
                        Admins = new ObservableCollection<Admin>(_db.Admin.OrderBy(m => m.Member.FirstName));
                        ErrorAdmin = "Administratoren blev ikke fundet i databasen!";
                        FeedbackAdmin = Feedback.Error;
                        return;
                    }
                    
                    // Change focus in view to the last admin in the list and give feedback
                    SelectedAdmin = Admins.Count - 1;
                    FeedbackAdmin = Feedback.Delete;
                });
            }
        }

        public ICommand ShowMembersContinue
        {
            get
            {
                return GetCommand<Member>(e =>
                {
                    // Assign a member as foreign key to the desired administrator
                    NewAdmin.Member = e;
                    MembersListWindow.Close();
                });
            }
        }

        #endregion
    }
}
