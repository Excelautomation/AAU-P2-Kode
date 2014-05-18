using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

using ARK.HelperFunctions;
using ARK.Model;
using ARK.Model.DB;
using ARK.Model.Extensions;
using ARK.View.Protokolsystem.Additional;
using ARK.View.Protokolsystem.Confirmations;
using ARK.View.Protokolsystem.Filters;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces;
using ARK.ViewModel.Base.Interfaces.Filter;
using ARK.ViewModel.Protokolsystem.Additional;
using ARK.ViewModel.Protokolsystem.Confirmations;
using ARK.ViewModel.Protokolsystem.Data;

namespace ARK.ViewModel.Protokolsystem.Pages
{
    public class BeginTripViewModel : ProtokolsystemContentViewModelBase, IFilterContentViewModel
    {
        #region Fields

        private readonly DbArkContext db = DbArkContext.GetDbContext(); // Database

        private IEnumerable<Boat> _boats; // All boats

        private IEnumerable<Boat> _boatsFiltered; // Boats to display

        private bool _enableMembers; // Used to determine whether the members-listview should be enabled

        private FrameworkElement _infoPage; // Informationpage

        private DateTime _latestData;

        private IEnumerable<MemberViewModel> _membersFiltered; // Members to display

        private Boat _selectedBoat; // Holds the selected boat

        private ObservableCollection<MemberViewModel> _selectedMembers; // Members in boat

        #endregion

        #region Constructors and Destructors

        public BeginTripViewModel()
        {
            // Instaliser lister
            this._boats = new List<Boat>();
            this._selectedMembers = new ObservableCollection<MemberViewModel>();
            this._membersFiltered = new ObservableCollection<MemberViewModel>();

            // Setup filter
            var filterController = new FilterContent(this);
            filterController.EnableFilter(false, true);
            filterController.FilterChanged += (sender, e) => this.UpdateFilter(e);

            // Load members
            this.LoadMembers();

            // Set up variables to load of data
            this.ParentAttached += (sender, e) =>
                {
                    // Load members and boats
                    this.LoadMembers();
                    this.LoadBoats();

                    this.ResetData();
                };
        }

        #endregion

        #region Public Properties

        public ICommand AddBlank
        {
            get
            {
                return new RelayCommand(
                    x =>
                        {
                            if (this.SelectedMembers.Count < this.SelectedBoat.NumberofSeats)
                            {
                                this.SelectedMembers.Add(
                                    new MemberViewModel(new Member() { Id = -1, FirstName = "Blank" }));
                            }
                        });
            }
        }

        public ICommand AddGuest
        {
            get
            {
                return new RelayCommand(
                    x =>
                        {
                            if (this.SelectedMembers.Count < this.SelectedBoat.NumberofSeats)
                            {
                                this.SelectedMembers.Add(
                                    new MemberViewModel(new Member() { Id = -2, FirstName = "Gæst" }));
                            }
                        });
            }
        }

        public IEnumerable<Boat> Boats
        {
            get
            {
                return this._boatsFiltered;
            }

            set
            {
                this._boatsFiltered = value;
                this.Notify();
            }
        }

        public string Direction { get; set; }

        public ICommand DirectionSelected
        {
            get
            {
                return new RelayCommand(
                    x =>
                        {
                            var temp = x as string;
                            this.Direction = Regex.Replace(temp, @"\n", " ");
                        });
            }
        }

        public bool EnableMembers
        {
            get
            {
                return this._enableMembers;
            }

            set
            {
                this._enableMembers = value;
                this.Notify();
            }
        }

        public FrameworkElement Filter
        {
            get
            {
                return new BeginTripFilters();
            }
        }

        public bool LongTrip { get; set; }

        public int MembersCount
        {
            get
            {
                return this.MembersFiltered.Count(member => member.Visible);
            }
        }

        public IEnumerable<MemberViewModel> MembersFiltered
        {
            get
            {
                return this._membersFiltered;
            }

            set
            {
                this._membersFiltered = value;
                this.Notify();
                this.NotifyCustom("MembersCount");
            }
        }

        public Boat SelectedBoat
        {
            get
            {
                return this._selectedBoat;
            }

            set
            {
                if (this._selectedBoat == value
                    || // Nothing changed - silently discard - STACKOVERFLOW IF NOT DISCARDED (Keyboard chaning LV)
                    value == null)
                {
                    return;
                }

                this._selectedBoat = value;

                this.ProtocolSystem.KeyboardClear();
                this.SelectedMembers.Clear();
                this.UpdateInfo();

                if (value.Usable && !value.BoatOut)
                {
                    this.EnableMembers = true;
                }
                else
                {
                    this.EnableMembers = false;
                }
            }
        }

        public ObservableCollection<MemberViewModel> SelectedMembers
        {
            get
            {
                return this._selectedMembers;
            }

            set
            {
                this._selectedMembers = value;
                this.Notify();
            }
        }

        public ICommand ShowConfirmDialog
        {
            get
            {
                return new RelayCommand(
                    x => this.ConfirmTripData(), 
                    x =>
                    this.SelectedBoat != null && this.SelectedMembers.Count == this.SelectedBoat.NumberofSeats
                    && this.Direction != null);
            }
        }

        #endregion

        #region Properties

        private BeginTripAdditionalInfoViewModel Info
        {
            get
            {
                return this.InfoPage.DataContext as BeginTripAdditionalInfoViewModel;
            }
        }

        private FrameworkElement InfoPage
        {
            get
            {
                return this._infoPage ?? (this._infoPage = new BeginTripAdditionalInfo());
            }
        }

        #endregion

        #region Public Methods and Operators

        public void ConfirmTripData()
        {
            {
                Trip trip = new Trip
                                {
                                    Id = this.db.Trip.OrderByDescending(t => t.Id).First().Id + 1, 
                                    TripStartTime = DateTime.Now, 
                                    Members = new List<Member>(this.SelectedMembers.Select(member => member.Member)), 
                                    Boat = this.SelectedBoat, 
                                    LongTrip = this.LongTrip, 
                                    Direction = this.Direction
                                };

                var dlg = new BeginTripBoatsConfirm();
                var confirmTripViewModel = (BeginTripBoatsConfirmViewModel)dlg.DataContext;
                confirmTripViewModel.Trip = trip;
                this.ProtocolSystem.ShowDialog(dlg);
            }
        }

        #endregion

        #region Methods

        private void LoadBoats()
        {
            // Load data. Check the boats activitylevel on a 8-day-basis
            DateTime limit = DateTime.Now.AddDays(-8);

            this._boats = (from boat in db.Boat
                where boat.Active
                orderby boat.Trips.Any(trip => trip.TripEndedTime == null),
                    boat.Trips.Count(trip => trip.TripStartTime > limit) descending
                select boat) 
                .Include(boat => boat.Trips)
                .ToList();
                          

            /*this._boats =
                this.db.Boat.Where(boat => boat.Active)
                    .OrderBy(boat => boat.Trips.Any(trip => trip.TripEndedTime == null))
                    .ThenByDescending(boat => boat.Trips.Count(trip => trip.TripStartTime > limit))
                    .Include(boat => boat.Trips)
                    .ToList();*/
        }

        private void LoadMembers()
        {
            if (this.MembersFiltered == null || (DateTime.Now - this._latestData).TotalHours > 1)
            {
                this._latestData = DateTime.Now;

                var members = this.db.Member.Where(member => member.Active).OrderBy(x => x.FirstName).ToList();
                this.MembersFiltered =
                    new ObservableCollection<MemberViewModel>(members.Select(member => new MemberViewModel(member)));
            }
        }

        private void ResetData()
        {
            // Reset filter
            this.ResetFilter();

            // Reset lists
            this.SelectedMembers.Clear();
            this.EnableMembers = false;

            this._selectedBoat = null;
            this.NotifyCustom("SelectedBoat");

            this.UpdateInfo();
        }

        private void ResetFilter()
        {
            this.Boats = new ObservableCollection<Boat>(this._boats.ToList());
            foreach (MemberViewModel member in this.MembersFiltered)
            {
                member.Visible = true;
            }
        }

        private void SortBoats(Func<Boat, string> predicate)
        {
            this.Boats = this.Boats.OrderBy(predicate);
        }

        private void SortMembers(Func<MemberViewModel, string> predicate)
        {
            this.MembersFiltered = this.MembersFiltered.OrderBy(predicate);
        }

        private void UpdateFilter(FilterChangedEventArgs args)
        {
            // Reset filters
            this.ResetFilter();

            if ((args.FilterEventArgs == null || !args.FilterEventArgs.Filters.Any())
                && (args.SearchEventArgs == null || string.IsNullOrEmpty(args.SearchEventArgs.SearchText)))
            {
                return;
            }

            // Filter only boats
            if (args.FilterEventArgs != null && args.FilterEventArgs.Filters.Any())
            {
                this.Boats = FilterContent.FilterItems(this.Boats, args.FilterEventArgs);
            }

            // Check search
            if (args.SearchEventArgs != null && !string.IsNullOrEmpty(args.SearchEventArgs.SearchText))
            {
                this.Boats = from boat in this.Boats where boat.Filter(args.SearchEventArgs.SearchText) select boat;

                foreach (MemberViewModel member in
                    this.MembersFiltered.Where(member => !member.Member.Filter(args.SearchEventArgs.SearchText)))
                {
                    member.Visible = false;
                }
            }
        }

        private void UpdateInfo()
        {
            this.Info.SelectedBoat = this.SelectedBoat;
            this.Info.SelectedMembers = this.SelectedMembers;

            this.ProtocolSystem.ChangeInfo(this.InfoPage, this.Info);
        }

        #endregion
    }
}