using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using ARK.Model;
using ARK.Model.DB;
using ARK.View.Administrationssystem.Pages;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Administrationssystem
{
    public class SettingsViewModel : ContentViewModelBase
    {
        #region Fields

        public MembersListWindow MembersListWindow;

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

        #endregion

        #region Constructors and Destructors

        public SettingsViewModel()
        {
            // Initialize database and get data in a single thread(lock)
            this._db = DbArkContext.GetDbContext();
            lock (this._db)
            {
                this.DamageTypes = new ObservableCollection<DamageType>(this._db.DamageType.OrderBy(e => e.Type));
                this.StandardTrips = new ObservableCollection<StandardTrip>(this._db.StandardTrip.OrderBy(e => e.Title));
                this.Admins = new ObservableCollection<Admin>(this._db.Admin.OrderBy(e => e.Member.FirstName));
                this.Members = new ObservableCollection<Member>(this._db.Member.OrderBy(e => e.FirstName));
            }

            // Initialize the selected items in the respective lists.
            if (this.Admins.Any())
            {
                this.SelectedAdmin = 0;
                this.CurrentAdmin = this.Admins[0];
                this.ReferenceToCurrentAdmin = this.CurrentAdmin;
            }

            if (this.DamageTypes.Any())
            {
                this.SelectedDamageType = 0;
                this.CurrentDamageType = this.DamageTypes[0];
                this.ReferenceToCurrentDamageType = this.CurrentDamageType;
            }

            if (this.StandardTrips.Any())
            {
                this.SelectedStandardTrip = 0;
                this.CurrentStandardTrip = this.StandardTrips[0];
                this.ReferenceToCurrentStandardTrip = this.CurrentStandardTrip;
            }

            // Check seasons
            if (!this._db.Season.Any(x => true))
            {
                this.CurrentSeason = new Season();
                this._db.Season.Add(this.CurrentSeason);
            }
            else
            {
                this.CurrentSeason = this._db.Season.AsEnumerable().Last(x => true);
            }
        }

        #endregion

        #region Enums

        public enum Feedback
        {
            Default, 

            Save, 

            Cancel, 

            Delete, 

            Create, 

            Error
        }

        #endregion

        #region Public Properties

        public ObservableCollection<Admin> Admins
        {
            get
            {
                return this._admins;
            }

            set
            {
                this._admins = value;
                this.Notify();
            }
        }

        public ICommand CancelChangesAdmins
        {
            get
            {
                return this.GetCommand(
                    () =>
                        {
                            // Reloads all the original values for the selected admin
                            this.CurrentAdmin = new Admin()
                                                    {
                                                        Username = this.ReferenceToCurrentAdmin.Username, 
                                                        Password = this.ReferenceToCurrentAdmin.Password, 
                                                        Member = this.ReferenceToCurrentAdmin.Member, 
                                                        ContactDark = this.ReferenceToCurrentAdmin.ContactDark, 
                                                        ContactTrip = this.ReferenceToCurrentAdmin.ContactTrip
                                                    };

                            // Give feedback through view
                            this.FeedbackAdmin = Feedback.Cancel;
                        });
            }
        }

        public ICommand CancelChangesDamageTypes
        {
            get
            {
                return this.GetCommand(
                    () =>
                        {
                            // Revert changes made by user
                            // Copy from database to the selected damagetype
                            this.CurrentDamageType = new DamageType() { Type = this.ReferenceToCurrentDamageType.Type };

                            // Give feedback
                            this.FeedbackDamageType = Feedback.Cancel;
                        });
            }
        }

        public ICommand CancelChangesStandardTrips
        {
            get
            {
                return this.GetCommand(
                    () =>
                        {
                            // Copies original info from database to the selected standardtrip
                            this.CurrentStandardTrip = new StandardTrip()
                                                           {
                                                               Distance =
                                                                   this.ReferenceToCurrentStandardTrip
                                                                   .Distance, 
                                                               Direction =
                                                                   this.ReferenceToCurrentStandardTrip
                                                                   .Direction, 
                                                               Title =
                                                                   this.ReferenceToCurrentStandardTrip
                                                                   .Title
                                                           };

                            // Give feedback
                            this.FeedbackStandardTrip = Feedback.Cancel;
                        });
            }
        }

        public ICommand CreateAdmin
        {
            get
            {
                return this.GetCommand(
                    () =>
                        {
                            // Create a new object to store the info of the desired admin
                            this.NewAdmin = new Admin();

                            // Initialize and open a new window for the user
                            // Get the required info for creating an admin through the new window
                            this.MembersListWindow = new View.Administrationssystem.Pages.MembersListWindow();
                            this.MembersListWindow.DataContext = this;
                            this.MembersListWindow.ShowDialog();
                            this.NewAdmin.ContactDark = false;
                            this.NewAdmin.ContactTrip = false;

                            // Refresh from database
                            this.Admins = new ObservableCollection<Admin>(this._db.Admin.ToList());

                            // Check there already exists an admin with the same username og member
                            // Also check if user made a password and username
                            if (this.NewAdmin.Username.Length == 0 || this.NewAdmin.Password.Length == 0)
                            {
                                System.Windows.MessageBox.Show("Brugernavn og kodeord er påkrævet!");
                                return;
                            }
                            else if (this.Admins.Any(m => m.Member == this.NewAdmin.Member))
                            {
                                System.Windows.MessageBox.Show("Det valgte medlem er allerede administrator!");
                                return;
                            }
                            else if (this.Admins.Any(m => m.Username == this.NewAdmin.Username))
                            {
                                System.Windows.MessageBox.Show("Det ønskede brugernavn eksisterer allerede!");
                                return;
                            }

                            // Convert username and password to lower-case
                            this.NewAdmin.Username = this.NewAdmin.Username.ToLower();
                            this.NewAdmin.Password = this.NewAdmin.Password.ToLower();

                            // Add the new admin to database and list
                            this._db.Admin.Add(this.NewAdmin);
                            this.Admins.Add(this.NewAdmin);

                            // Try to save changes to database
                            this._db.SaveChanges();

                            // Change focus in view to the last admin in the list and give feedback
                            this.SelectedAdmin = this.Admins.Count - 1;
                            this.FeedbackAdmin = Feedback.Create;
                        });
            }
        }

        public ICommand CreateDamageType
        {
            get
            {
                return this.GetCommand(
                    () =>
                        {
                            // Preconfigured damagetype to create
                            DamageType NewDamageType = new DamageType() { Type = "Ny skadetype" };

                            // Add new damagetype
                            this._db.DamageType.Add(NewDamageType);
                            this._db.SaveChanges();
                            this.DamageTypes.Add(NewDamageType);

                            // Refresh from database, select new damagetype and give feedback
                            this.DamageTypes =
                                new ObservableCollection<DamageType>(this._db.DamageType.OrderBy(m => m.Type));
                            this.SelectedDamageType =
                                this.DamageTypes.IndexOf(this.DamageTypes.First(m => m.Type == NewDamageType.Type));
                            this.FeedbackDamageType = Feedback.Create;
                        });
            }
        }

        public ICommand CreateStandardTrip
        {
            get
            {
                return this.GetCommand(
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
                            this._db.StandardTrip.Add(NewStandardTrip);
                            this._db.SaveChanges();
                            this.StandardTrips.Add(NewStandardTrip);

                            // (hopefully) selects the new standardtrip and give feedback
                            this.StandardTrips =
                                new ObservableCollection<StandardTrip>(this._db.StandardTrip.OrderBy(m => m.Title));
                            this.SelectedStandardTrip =
                                this.StandardTrips.IndexOf(
                                    this.StandardTrips.First(m => m.Title == NewStandardTrip.Title));
                            this.FeedbackStandardTrip = Feedback.Create;
                        });
            }
        }

        public Admin CurrentAdmin
        {
            get
            {
                return this._currentAdmin;
            }

            set
            {
                this._currentAdmin = value;
                this.Notify();
            }
        }

        public DamageType CurrentDamageType
        {
            get
            {
                return this._currentDamageType;
            }

            set
            {
                this._currentDamageType = value;
                this.Notify();
            }
        }

        public Season CurrentSeason
        {
            get
            {
                return this._currentSeason;
            }

            set
            {
                this._currentSeason = value;
                this.Notify();
            }
        }

        public StandardTrip CurrentStandardTrip
        {
            get
            {
                return this._currentStandardTrip;
            }

            set
            {
                this._currentStandardTrip = value;
                this.Notify();
            }
        }

        public ObservableCollection<DamageType> DamageTypes
        {
            get
            {
                return this._damageTypes;
            }

            set
            {
                this._damageTypes = value;
                this.Notify();
            }
        }

        public ICommand DeleteAdmin
        {
            get
            {
                return this.GetCommand(
                    () =>
                        {
                            // Keep a temp. reference to the selected admin
                            // Refresh from database and select the desired admin again
                            string tmpId = this.ReferenceToCurrentAdmin.Username;
                            this.Admins = new ObservableCollection<Admin>(this._db.Admin.ToList());
                            this.ReferenceToCurrentAdmin = this.Admins.First(m => m.Username == tmpId);

                            // Check if there's only one admin
                            if (this.Admins.Count == 1)
                            {
                                this.ErrorAdmin = "Sidste administrator kan ikke slettes!";
                                this.FeedbackAdmin = Feedback.Error;
                                return;
                            }

                            // Remove admin
                            this._db.Admin.Remove(this.ReferenceToCurrentAdmin);
                            this.Admins.Remove(this.ReferenceToCurrentAdmin);

                            // Try to save changes.
                            try
                            {
                                this._db.SaveChanges();
                            }
                            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
                            {
                                // The admin has likely already been deleted
                                this.Admins =
                                    new ObservableCollection<Admin>(this._db.Admin.OrderBy(m => m.Member.FirstName));
                                this.ErrorAdmin = "Administratoren blev ikke fundet i databasen!";
                                this.FeedbackAdmin = Feedback.Error;
                                return;
                            }

                            // Change focus in view to the last admin in the list and give feedback
                            this.SelectedAdmin = this.Admins.Count - 1;
                            this.FeedbackAdmin = Feedback.Delete;
                        });
            }
        }

        public ICommand DeleteDamageType
        {
            get
            {
                return this.GetCommand(
                    () =>
                        {
                            // Remove selected damagetype from list
                            this._db.DamageType.Remove(this.ReferenceToCurrentDamageType);

                            // Try to save Changes to database
                            try
                            {
                                this._db.SaveChanges();
                            }
                            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
                            {
                                // The selected damagetype has likely already been deleted
                                this.DamageTypes =
                                    new ObservableCollection<DamageType>(this._db.DamageType.OrderBy(m => m.Type));
                                this.ErrorDamageType = "Skadetypen blev ikke fundet i databasen!";
                                this.FeedbackDamageType = Feedback.Error;
                                return;
                            }

                            // Refresh database, select last item and give feedback
                            this.DamageTypes =
                                new ObservableCollection<DamageType>(this._db.DamageType.OrderBy(m => m.Type));
                            this.SelectedDamageType = this.DamageTypes.Count - 1;
                            this.FeedbackDamageType = Feedback.Delete;
                        });
            }
        }

        public ICommand DeleteStandardTrip
        {
            get
            {
                return this.GetCommand(
                    () =>
                        {
                            // Remove selected trip
                            this._db.StandardTrip.Remove(this.ReferenceToCurrentStandardTrip);

                            // Try to save changes
                            try
                            {
                                this._db.SaveChanges();
                            }
                            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
                            {
                                // The selected standardtrip has likely already been deleted from database
                                this.StandardTrips =
                                    new ObservableCollection<StandardTrip>(this._db.StandardTrip.OrderBy(m => m.Title));
                                this.ErrorStandardTrip = "Standardturen blev ikke fundet i databasen!";
                                this.FeedbackStandardTrip = Feedback.Error;
                                return;
                            }

                            // Refresh database, select last standardtrip and give feedback
                            this.StandardTrips =
                                new ObservableCollection<StandardTrip>(this._db.StandardTrip.OrderBy(m => m.Title));
                            this.SelectedStandardTrip = this.StandardTrips.Count - 1;
                            this.FeedbackDamageType = Feedback.Delete;
                        });
            }
        }

        public bool DisplaySeasonErrorLabel
        {
            get
            {
                return this._displaySeasonErrorLabel;
            }

            set
            {
                this._displaySeasonErrorLabel = value;
                this.Notify();
            }
        }

        public string ErrorAdmin
        {
            get
            {
                return this._errorAdmin;
            }

            set
            {
                this._errorAdmin = value;
                this.Notify();
            }
        }

        public string ErrorDamageType
        {
            get
            {
                return this._errorDamageType;
            }

            set
            {
                this._errorDamageType = value;
                this.Notify();
            }
        }

        public string ErrorStandardTrip
        {
            get
            {
                return this._errorStandardTrip;
            }

            set
            {
                this._errorStandardTrip = value;
                this.Notify();
            }
        }

        public Feedback FeedbackAdmin
        {
            get
            {
                return this._feedbackAdmin;
            }

            set
            {
                this._feedbackAdmin = value;
                this.Notify();
            }
        }

        public Feedback FeedbackDamageType
        {
            get
            {
                return this._feedbackDamageType;
            }

            set
            {
                this._feedbackDamageType = value;
                this.Notify();
            }
        }

        public Feedback FeedbackStandardTrip
        {
            get
            {
                return this._feedbackStandardTrip;
            }

            set
            {
                this._feedbackStandardTrip = value;
                this.Notify();
            }
        }

        public ObservableCollection<Member> Members
        {
            get
            {
                return this._members;
            }

            set
            {
                this._members = value;
                this.Notify();
            }
        }

        public Admin NewAdmin
        {
            get
            {
                return this._newAdmin;
            }

            set
            {
                this._newAdmin = value;
                this.Notify();
            }
        }

        public ICommand NewSeason
        {
            get
            {
                return this.GetCommand(
                    () =>
                        {
                            // if current season started less then 183 days ago promt the user!
                            if (this.CurrentSeason != null
                                && DateTime.Compare(this.CurrentSeason.SeasonStart.AddDays(183), DateTime.Now) > 0)
                            {
                                // promp the user that the current season is less than a half year old!
                                // throw new NotImplementedException();
                                this.DisplaySeasonErrorLabel = true;
                            }
                            else
                            {
                                this.CurrentSeason.SeasonEnd = DateTime.Now;
                                Season tmpSeason = new Season();
                                this._db.Season.Add(tmpSeason);
                                this.CurrentSeason = tmpSeason;
                                this._db.SaveChanges();
                                this.DisplaySeasonErrorLabel = false;
                            }
                        });
            }
        }

        public Admin ReferenceToCurrentAdmin
        {
            get
            {
                return this._referenceToCurrentAdmin;
            }

            set
            {
                this._referenceToCurrentAdmin = value;
                this.Notify();
            }
        }

        public ICommand SaveChangesAdmins
        {
            get
            {
                return this.GetCommand(
                    () =>
                        {
                            // Save a temp. reference to the selected admin
                            int tempindex = this.SelectedAdmin;

                            // Retrieve changes from selected admin
                            this.ReferenceToCurrentAdmin.ContactTrip = this.CurrentAdmin.ContactTrip;
                            this.ReferenceToCurrentAdmin.ContactDark = this.CurrentAdmin.ContactDark;

                            // Try to save changes to database
                            try
                            {
                                this._db.SaveChanges();
                            }
                            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
                            {
                                // The selected admin has likely been deleted
                                this.Admins =
                                    new ObservableCollection<Admin>(this._db.Admin.OrderBy(m => m.Member.FirstName));
                                this.ErrorAdmin = "Administratoren blev ikke fundet i databasen!";
                                this.FeedbackAdmin = Feedback.Error;
                                return;
                            }

                            // Refresh from database, reselect the previously selected admin and give feedback
                            this.Admins =
                                new ObservableCollection<Admin>(this._db.Admin.OrderBy(m => m.Member.FirstName));
                            this.SelectedAdmin = tempindex;
                            this.FeedbackAdmin = Feedback.Save;
                        });
            }
        }

        public ICommand SaveChangesDamageTypes
        {
            get
            {
                return this.GetCommand(
                    () =>
                        {
                            // Save the selected damagetype to the list synced with database
                            this.ReferenceToCurrentDamageType.Type = this.CurrentDamageType.Type;

                            // Try to save changes to databse
                            try
                            {
                                this._db.SaveChanges();
                            }
                            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
                            {
                                // The sleceted damagetype has likely been deleted in database
                                this.DamageTypes =
                                    new ObservableCollection<DamageType>(this._db.DamageType.OrderBy(m => m.Type));
                                this.ErrorDamageType = "Skadetypen blev ikke fundet i databasen!";
                                this.FeedbackDamageType = Feedback.Error;
                                return;
                            }

                            // Refresh database and give feedback
                            this.DamageTypes =
                                new ObservableCollection<DamageType>(this._db.DamageType.OrderBy(m => m.Type));
                            this.FeedbackDamageType = Feedback.Save;
                        });
            }
        }

        public ICommand SaveChangesStandardTrips
        {
            get
            {
                return this.GetCommand(
                    () =>
                        {
                            int tmptrip = this.CurrentStandardTrip.Id;

                            // Copies data from userinput to the list synched with the database
                            this.ReferenceToCurrentStandardTrip.Distance = this.CurrentStandardTrip.Distance;
                            this.ReferenceToCurrentStandardTrip.Title = this.CurrentStandardTrip.Title;
                            this.ReferenceToCurrentStandardTrip.Direction = this.CurrentStandardTrip.Direction;

                            // Try to save changes
                            try
                            {
                                this._db.SaveChanges();
                            }
                            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
                            {
                                // The standardtrip has likely been deleted in the database
                                this.StandardTrips =
                                    new ObservableCollection<StandardTrip>(this._db.StandardTrip.OrderBy(m => m.Title));
                                this.ErrorStandardTrip = "Standardturen blev ikke fundet i databasen!";
                                this.FeedbackStandardTrip = Feedback.Error;
                                return;
                            }

                            // Reload database and give feedback.
                            this.StandardTrips =
                                new ObservableCollection<StandardTrip>(this._db.StandardTrip.OrderBy(m => m.Title));
                            this.FeedbackStandardTrip = Feedback.Save;
                        });
            }
        }

        public int SelectedAdmin
        {
            get
            {
                return this._selectedAdmin;
            }

            set
            {
                this._selectedAdmin = value;
                this.Notify();
            }
        }

        public ICommand SelectedChangeAdmin
        {
            get
            {
                return this.GetCommand(
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
                            this.CurrentAdmin = new Admin()
                                                    {
                                                        Username = admin.Username, 
                                                        Password = admin.Password, 
                                                        ContactTrip = admin.ContactTrip, 
                                                        ContactDark = admin.ContactDark, 
                                                        Member = admin.Member
                                                    };

                            // Sets reference to the selected admin in database
                            this.ReferenceToCurrentAdmin = admin;

                            // Give feedback
                            this.FeedbackAdmin = Feedback.Default;
                        });
            }
        }

        public ICommand SelectedChangeDamageType
        {
            get
            {
                return this.GetCommand(
                    e =>
                        {
                            // If list is empty return safely
                            if (e == null)
                            {
                                return;
                            }

                            var damageType = (DamageType)e;

                            // Create a copy of the selected damagetype from database
                            this.CurrentDamageType = new DamageType() { Type = damageType.Type };

                            // Remember reference to the selected damagetype in the list synced with database
                            this.ReferenceToCurrentDamageType = damageType;

                            // Give feedback
                            this.FeedbackDamageType = Feedback.Default;
                        });
            }
        }

        public ICommand SelectedChangeStandardTrip
        {
            get
            {
                return this.GetCommand(
                    e =>
                        {
                            // If the list is empty return safely
                            if (e == null)
                            {
                                return;
                            }

                            var standardTrip = (StandardTrip)e;

                            // Create a copy from the selected item
                            this.CurrentStandardTrip = new StandardTrip()
                                                           {
                                                               Distance = standardTrip.Distance, 
                                                               Direction = standardTrip.Direction, 
                                                               Title = standardTrip.Title
                                                           };

                            // Save reference to the original position of the selected item
                            this.ReferenceToCurrentStandardTrip = standardTrip;

                            // Give feedback
                            this.FeedbackStandardTrip = Feedback.Default;
                        });
            }
        }

        public int SelectedDamageType
        {
            get
            {
                return this._selectedDamageType;
            }

            set
            {
                this._selectedDamageType = value;
                this.Notify();
            }
        }

        public int SelectedStandardTrip
        {
            get
            {
                return this._selectedStandardTrip;
            }

            set
            {
                this._selectedStandardTrip = value;
                this.Notify();
            }
        }

        public ICommand ShowMembersContinue
        {
            get
            {
                return this.GetCommand(
                    e =>
                        {
                            // Assign a member as foreign key to the desired administrator
                            this.NewAdmin.Member = (Member)e;
                            this.MembersListWindow.Close();
                        });
            }
        }

        public ObservableCollection<StandardTrip> StandardTrips
        {
            get
            {
                return this._standardTrips;
            }

            set
            {
                this._standardTrips = value;
                this.Notify();
            }
        }

        public DateTime Today
        {
            get
            {
                return DateTime.Now;
            }
        }

        #endregion
    }
}