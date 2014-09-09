using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text.RegularExpressions;
using System.Windows.Input;

using ARK.HelperFunctions;
using ARK.Model;
using ARK.Model.DB;
using ARK.View.Administrationssystem.Pages;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Administrationssystem
{
    public class SettingsViewModel : ContentViewModelBase
    {
        public enum Feedback
        {
            Default, 

            Save, 

            Cancel, 

            Delete, 

            Create, 

            Error
        }

        private MembersListWindow MembersListWindow;

        private DamageType ReferenceToCurrentDamageType;

        private StandardTrip ReferenceToCurrentStandardTrip;

        private ObservableCollection<Admin> _admins;

        private Admin _currentAdmin;

        private DamageType _currentDamageType;

        private Season _currentSeason;

        private StandardTrip _currentStandardTrip;

        private ObservableCollection<DamageType> _damageTypes;

        private DbArkContext _db;

        private bool _displaySeasonErrorLabel = false;

        private string _errorAdmin;

        private string _errorDamageType;

        private string _errorStandardTrip;

        private Feedback _feedbackAdmin;

        private Feedback _feedbackDamageType;

        private Feedback _feedbackStandardTrip;

        private ObservableCollection<Member> _members;

        private Admin _newAdmin;

        private Admin _referenceToCurrentAdmin;

        private int _selectedAdmin;

        private int _selectedDamageType;

        private int _selectedStandardTrip;

        private ObservableCollection<StandardTrip> _standardTrips;

        private string _smsDelay;

        private string _smsWait;

        // Constructor
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

            // Initialize the selected items in the respective lists.
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

            // Read SMS settings
            Setting temp;
            SmsDelay = (temp = _db.Settings.FirstOrDefault(x => x.Name == "SmsDelay")) != null ? temp.Value : "0";
            SmsWait = (temp = _db.Settings.FirstOrDefault(x => x.Name == "SmsWait")) != null ? temp.Value : "0";
        }

        public ObservableCollection<Admin> Admins
        {
            get
            {
                return _admins;
            }

            set
            {
                _admins = value;
                Notify();
            }
        }

        public ICommand CancelChangesAdmins
        {
            get
            {
                return GetCommand(
                    () =>
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

        public ICommand CancelChangesDamageTypes
        {
            get
            {
                return GetCommand(
                    () =>
                        {
                            // Revert changes made by user
                            // Copy from database to the selected damagetype
                            CurrentDamageType = new DamageType() { Type = ReferenceToCurrentDamageType.Type };

                            // Give feedback
                            FeedbackDamageType = Feedback.Cancel;
                        });
            }
        }

        public ICommand CancelChangesStandardTrips
        {
            get
            {
                return GetCommand(
                    () =>
                        {
                            // Copies original info from database to the selected standardtrip
                            CurrentStandardTrip = new StandardTrip()
                                                      {
                                                          Distance =
                                                              ReferenceToCurrentStandardTrip.Distance, 
                                                          Direction =
                                                              ReferenceToCurrentStandardTrip.Direction, 
                                                          Title = ReferenceToCurrentStandardTrip.Title
                                                      };

                            // Give feedback
                            FeedbackStandardTrip = Feedback.Cancel;
                        });
            }
        }

        public ICommand CreateAdmin
        {
            get
            {
                return GetCommand(
                    () =>
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

                            // Convert username and password to lower-case
                            NewAdmin.Username = NewAdmin.Username.ToLower();
                            NewAdmin.Password = PasswordHashing.HashPassword(NewAdmin.Password.ToLower());

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

        public ICommand CreateDamageType
        {
            get
            {
                return GetCommand(
                    () =>
                        {
                            // Preconfigured damagetype to create
                            DamageType NewDamageType = new DamageType() { Type = "Ny skadetype" };

                            // Add new damagetype
                            _db.DamageType.Add(NewDamageType);
                            _db.SaveChanges();
                            DamageTypes.Add(NewDamageType);

                            // Refresh from database, select new damagetype and give feedback
                            DamageTypes = new ObservableCollection<DamageType>(_db.DamageType.OrderBy(m => m.Type));
                            SelectedDamageType =
                                DamageTypes.IndexOf(DamageTypes.First(m => m.Type == NewDamageType.Type));
                            FeedbackDamageType = Feedback.Create;
                        });
            }
        }

        public ICommand CreateStandardTrip
        {
            get
            {
                return GetCommand(
                    () =>
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
                            StandardTrips =
                                new ObservableCollection<StandardTrip>(_db.StandardTrip.OrderBy(m => m.Title));
                            SelectedStandardTrip =
                                StandardTrips.IndexOf(StandardTrips.First(m => m.Title == NewStandardTrip.Title));
                            FeedbackStandardTrip = Feedback.Create;
                        });
            }
        }

        public Admin CurrentAdmin
        {
            get
            {
                return _currentAdmin;
            }

            set
            {
                _currentAdmin = value;
                Notify();
            }
        }

        public DamageType CurrentDamageType
        {
            get
            {
                return _currentDamageType;
            }

            set
            {
                _currentDamageType = value;
                Notify();
            }
        }

        public Season CurrentSeason
        {
            get
            {
                return _currentSeason;
            }

            set
            {
                _currentSeason = value;
                Notify();
            }
        }

        public StandardTrip CurrentStandardTrip
        {
            get
            {
                return _currentStandardTrip;
            }

            set
            {
                _currentStandardTrip = value;
                Notify();
            }
        }

        public ObservableCollection<DamageType> DamageTypes
        {
            get
            {
                return _damageTypes;
            }

            set
            {
                _damageTypes = value;
                Notify();
            }
        }

        public ICommand DeleteAdmin
        {
            get
            {
                return GetCommand(
                    () =>
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

        public ICommand DeleteDamageType
        {
            get
            {
                return GetCommand(
                    () =>
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

        public ICommand DeleteStandardTrip
        {
            get
            {
                return GetCommand(
                    () =>
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
                                StandardTrips =
                                    new ObservableCollection<StandardTrip>(_db.StandardTrip.OrderBy(m => m.Title));
                                ErrorStandardTrip = "Standardturen blev ikke fundet i databasen!";
                                FeedbackStandardTrip = Feedback.Error;
                                return;
                            }

                            // Refresh database, select last standardtrip and give feedback
                            StandardTrips =
                                new ObservableCollection<StandardTrip>(_db.StandardTrip.OrderBy(m => m.Title));
                            SelectedStandardTrip = StandardTrips.Count - 1;
                            FeedbackDamageType = Feedback.Delete;
                        });
            }
        }

        public bool DisplaySeasonErrorLabel
        {
            get
            {
                return _displaySeasonErrorLabel;
            }

            set
            {
                _displaySeasonErrorLabel = value;
                Notify();
            }
        }

        public string ErrorAdmin
        {
            get
            {
                return _errorAdmin;
            }

            set
            {
                _errorAdmin = value;
                Notify();
            }
        }

        public string ErrorDamageType
        {
            get
            {
                return _errorDamageType;
            }

            set
            {
                _errorDamageType = value;
                Notify();
            }
        }

        public string ErrorStandardTrip
        {
            get
            {
                return _errorStandardTrip;
            }

            set
            {
                _errorStandardTrip = value;
                Notify();
            }
        }

        public Feedback FeedbackAdmin
        {
            get
            {
                return _feedbackAdmin;
            }

            set
            {
                _feedbackAdmin = value;
                Notify();
            }
        }

        public Feedback FeedbackDamageType
        {
            get
            {
                return _feedbackDamageType;
            }

            set
            {
                _feedbackDamageType = value;
                Notify();
            }
        }

        public Feedback FeedbackStandardTrip
        {
            get
            {
                return _feedbackStandardTrip;
            }

            set
            {
                _feedbackStandardTrip = value;
                Notify();
            }
        }

        public ObservableCollection<Member> Members
        {
            get
            {
                return _members;
            }

            set
            {
                _members = value;
                Notify();
            }
        }

        public Admin NewAdmin
        {
            get
            {
                return _newAdmin;
            }

            set
            {
                _newAdmin = value;
                Notify();
            }
        }

        public ICommand NewSeason
        {
            get
            {
                return GetCommand(
                    () =>
                        {
                            // if current season started less then 183 days ago promt the user!
                            if (CurrentSeason != null
                                && DateTime.Compare(CurrentSeason.SeasonStart.AddDays(183), DateTime.Now) > 0)
                            {
                                // promp the user that the current season is less than a half year old!
                                // throw new NotImplementedException();
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

        public Admin ReferenceToCurrentAdmin
        {
            get
            {
                return _referenceToCurrentAdmin;
            }

            set
            {
                _referenceToCurrentAdmin = value;
                Notify();
            }
        }

        public ICommand SaveChangesAdmins
        {
            get
            {
                return GetCommand(
                    () =>
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
                            Admins = new ObservableCollection<Admin>(_db.Admin.OrderBy(m => m.Member.FirstName));
                            SelectedAdmin = tempindex;
                            FeedbackAdmin = Feedback.Save;
                        });
            }
        }

        public ICommand SaveChangesDamageTypes
        {
            get
            {
                return GetCommand(
                    () =>
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

        public ICommand SaveChangesStandardTrips
        {
            get
            {
                return GetCommand(
                    () =>
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
                                StandardTrips =
                                    new ObservableCollection<StandardTrip>(_db.StandardTrip.OrderBy(m => m.Title));
                                ErrorStandardTrip = "Standardturen blev ikke fundet i databasen!";
                                FeedbackStandardTrip = Feedback.Error;
                                return;
                            }

                            // Reload database and give feedback.
                            StandardTrips =
                                new ObservableCollection<StandardTrip>(_db.StandardTrip.OrderBy(m => m.Title));
                            FeedbackStandardTrip = Feedback.Save;
                        });
            }
        }

        public int SelectedAdmin
        {
            get
            {
                return _selectedAdmin;
            }

            set
            {
                _selectedAdmin = value;
                Notify();
            }
        }

        public ICommand SelectedChangeAdmin
        {
            get
            {
                return GetCommand(
                    e =>
                        {
                            // If the list is empty return safely (Should not be possibly for admins)
                            if (e == null)
                            {
                                return;
                            }

                            var admin = (Admin)e;

                            // Sets CurrentAdmin to contain a copy of the selected item
                            // This object contains the direct changes made by the user
                            CurrentAdmin = new Admin()
                                               {
                                                   Username = admin.Username, 
                                                   Password = admin.Password, 
                                                   ContactTrip = admin.ContactTrip, 
                                                   ContactDark = admin.ContactDark, 
                                                   Member = admin.Member
                                               };

                            // Sets reference to the selected admin in database
                            ReferenceToCurrentAdmin = admin;

                            // Give feedback
                            FeedbackAdmin = Feedback.Default;
                        });
            }
        }

        public ICommand SelectedChangeDamageType
        {
            get
            {
                return GetCommand(
                    e =>
                        {
                            // If list is empty return safely
                            if (e == null)
                            {
                                return;
                            }

                            var damageType = (DamageType)e;

                            // Create a copy of the selected damagetype from database
                            CurrentDamageType = new DamageType() { Type = damageType.Type };

                            // Remember reference to the selected damagetype in the list synced with database
                            ReferenceToCurrentDamageType = damageType;

                            // Give feedback
                            FeedbackDamageType = Feedback.Default;
                        });
            }
        }

        public ICommand SelectedChangeStandardTrip
        {
            get
            {
                return GetCommand(
                    e =>
                        {
                            // If the list is empty return safely
                            if (e == null)
                            {
                                return;
                            }

                            var standardTrip = (StandardTrip)e;

                            // Create a copy from the selected item
                            CurrentStandardTrip = new StandardTrip()
                                                      {
                                                          Distance = standardTrip.Distance, 
                                                          Direction = standardTrip.Direction, 
                                                          Title = standardTrip.Title
                                                      };

                            // Save reference to the original position of the selected item
                            ReferenceToCurrentStandardTrip = standardTrip;

                            // Give feedback
                            FeedbackStandardTrip = Feedback.Default;
                        });
            }
        }

        public int SelectedDamageType
        {
            get
            {
                return _selectedDamageType;
            }

            set
            {
                _selectedDamageType = value;
                Notify();
            }
        }

        public int SelectedStandardTrip
        {
            get
            {
                return _selectedStandardTrip;
            }

            set
            {
                _selectedStandardTrip = value;
                Notify();
            }
        }

        public string SmsDelay
        {
            get
            {
                return _smsDelay;
            }
            set
            {
                _smsDelay = value;
                Notify();
            }
        }

        public ICommand SmsDelayEnter
        {
            get
            {
                return new RelayCommand(
                    x =>
                        {
                            var setting = _db.Settings.FirstOrDefault(e => e.Name == "SmsDelay");
                            if (setting != null)
                            {
                                setting.Value = SmsDelay;
                            }
                            else
                            {
                                _db.Settings.Add(new Setting { Name = "SmsDelay", Value = SmsDelay });
                            }
                            _db.SaveChangesAsync();
                        },
                    x => !Regex.IsMatch(x as string, @"[^0-9]"));
            }
        }

        public string SmsWait
        {
            get
            {
                return _smsWait;
            }
            set
            {
                _smsWait = value;
                Notify();
            }
        }

        public ICommand SmsWaitEnter
        {
            get
            {
                return new RelayCommand(
                    x =>
                        {
                            var setting = _db.Settings.FirstOrDefault(e => e.Name == "SmsWait");
                            if (setting != null)
                            {
                                setting.Value = SmsWait;
                            }
                            else
                            {
                                _db.Settings.Add(new Setting { Name = "SmsWait", Value = SmsWait });
                            }
                            _db.SaveChangesAsync();
                        },
                    x => !Regex.IsMatch(x as string, @"[^0-9]"));
            }
        }

        public ICommand ShowMembersContinue
        {
            get
            {
                return GetCommand(
                    e =>
                        {
                            // Assign a member as foreign key to the desired administrator
                            NewAdmin.Member = (Member)e;
                            MembersListWindow.Close();
                        });
            }
        }

        public ObservableCollection<StandardTrip> StandardTrips
        {
            get
            {
                return _standardTrips;
            }

            set
            {
                _standardTrips = value;
                Notify();
            }
        }

        public DateTime Sunset
        {
            get
            {
                return SunsetClass.Sunset;
            }
        }

        public DateTime Today
        {
            get
            {
                return DateTime.Now;
            }
        }
    }
}