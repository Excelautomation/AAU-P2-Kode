using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

using ARK.Model;
using ARK.Model.DB;
using ARK.Model.Extensions;
using ARK.View.Protokolsystem.Additional;
using ARK.View.Protokolsystem.Confirmations;
using ARK.View.Protokolsystem.Filters;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces.Filter;
using ARK.ViewModel.Protokolsystem.Additional;
using ARK.ViewModel.Protokolsystem.Confirmations;
using ARK.ViewModel.Protokolsystem.Data;

namespace ARK.ViewModel.Protokolsystem.Pages
{
    public class BeginTripViewModel : ProtokolsystemContentViewModelBase, IFilterContentViewModel
    {
        private readonly DbArkContext db = DbArkContext.GetDbContext(); // Database

        private IEnumerable<Boat> _boats; // All boats

        private IEnumerable<Boat> _boatsFiltered; // Boats to display

        private bool _enableMembers; // Used to determine whether the members-listview should be enabled

        private FrameworkElement _infoPage; // Informationpage

        private DateTime _latestData;

        private IEnumerable<MemberViewModel> _membersFiltered; // Members to display

        private Boat _selectedBoat; // Holds the selected boat

        private ObservableCollection<MemberViewModel> _selectedMembers; // Members in boat

        public BeginTripViewModel()
        {
            // Instaliser lister
            _boats = new List<Boat>();
            _selectedMembers = new ObservableCollection<MemberViewModel>();
            _membersFiltered = new ObservableCollection<MemberViewModel>();

            // Setup filter
            var filterController = new FilterContent(this);
            filterController.EnableFilter(false, true);
            filterController.FilterChanged += (sender, e) => UpdateFilter(e);

            // Load members
            LoadMembers();

            // Set up variables to load of data
            ParentAttached += (sender, e) =>
                {
                    // Load members and boats
                    LoadMembers();
                    LoadBoats();

                    ResetData();
                };
        }

        public ICommand AddBlank
        {
            get
            {
                return new RelayCommand(
                    x =>
                        {
                            if (SelectedMembers.Count < SelectedBoat.NumberofSeats)
                            {
                                SelectedMembers.Add(new MemberViewModel(new Member() { Id = -1, FirstName = "Blank" }));
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
                            if (SelectedMembers.Count < SelectedBoat.NumberofSeats)
                            {
                                SelectedMembers.Add(new MemberViewModel(new Member() { Id = -2, FirstName = "Gæst" }));
                            }
                        });
            }
        }

        public IEnumerable<Boat> Boats
        {
            get
            {
                return _boatsFiltered;
            }

            set
            {
                _boatsFiltered = value;
                Notify();
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
                            Direction = Regex.Replace(temp, @"\n", " ");
                        });
            }
        }

        public bool EnableMembers
        {
            get
            {
                return _enableMembers;
            }

            set
            {
                _enableMembers = value;
                Notify();
            }
        }

        public bool LongTrip { get; set; }

        public int MembersCount
        {
            get
            {
                return MembersFiltered.Count(member => member.Visible);
            }
        }

        public IEnumerable<MemberViewModel> MembersFiltered
        {
            get
            {
                return _membersFiltered;
            }

            set
            {
                _membersFiltered = value;
                Notify();
                NotifyCustom("MembersCount");
            }
        }

        public Boat SelectedBoat
        {
            get
            {
                return _selectedBoat;
            }

            set
            {
                if (_selectedBoat == value
                    || // Nothing changed - silently discard - STACKOVERFLOW IF NOT DISCARDED (Keyboard chaning LV)
                    value == null)
                {
                    return;
                }

                _selectedBoat = value;

                ProtocolSystem.KeyboardClear();
                SelectedMembers.Clear();
                UpdateInfo();

                if (value.Usable && !value.BoatOut)
                {
                    EnableMembers = true;
                }
                else
                {
                    EnableMembers = false;
                }
            }
        }

        public ObservableCollection<MemberViewModel> SelectedMembers
        {
            get
            {
                return _selectedMembers;
            }

            set
            {
                _selectedMembers = value;
                Notify();
            }
        }

        public ICommand ShowConfirmDialog
        {
            get
            {
                return new RelayCommand(
                    x => ConfirmTripData(), 
                    x =>
                    (SelectedBoat != null && SelectedMembers.Count == SelectedBoat.NumberofSeats && Direction != null) ||
                    (SelectedBoat != null && SelectedMembers.Count == SelectedBoat.NumberofSeats && SelectedBoat.SpecificBoatType == Boat.BoatType.Ergometer) // Ergometer dont need direction!!
                    );
            }
        }

        private BeginTripAdditionalInfoViewModel Info
        {
            get
            {
                return InfoPage.DataContext as BeginTripAdditionalInfoViewModel;
            }
        }

        private FrameworkElement InfoPage
        {
            get
            {
                return _infoPage ?? (_infoPage = new BeginTripAdditionalInfo());
            }
        }

        public FrameworkElement Filter
        {
            get
            {
                return new BeginTripFilters();
            }
        }

        public void ConfirmTripData()
        {
            {
                Trip trip = new Trip
                                {
                                    Id = db.Trip.OrderByDescending(t => t.Id).First().Id + 1, 
                                    TripStartTime = DateTime.Now, 
                                    Members = new List<Member>(SelectedMembers.Select(member => member.Member)), 
                                    Boat = SelectedBoat, 
                                    LongTrip = LongTrip, 
                                    Direction = Direction
                                };

                var dlg = new BeginTripBoatsConfirm();
                var confirmTripViewModel = (BeginTripBoatsConfirmViewModel)dlg.DataContext;
                confirmTripViewModel.Trip = trip;
                ProtocolSystem.ShowDialog(dlg);
            }
        }

        private void LoadBoats()
        {
            // Load data. Check the boats activitylevel on a 8-day-basis
            DateTime limit = DateTime.Now.AddDays(-8);

            _boats = (from boat in db.Boat
                      where boat.Active
                      orderby boat.Trips.Any(trip => trip.TripEndedTime == null), 
                          boat.Trips.Count(trip => trip.TripStartTime > limit) descending
                      select boat).Include(boat => boat.Trips).ToList();

            /*this._boats =
                this.db.Boat.Where(boat => boat.Active)
                    .OrderBy(boat => boat.Trips.Any(trip => trip.TripEndedTime == null))
                    .ThenByDescending(boat => boat.Trips.Count(trip => trip.TripStartTime > limit))
                    .Include(boat => boat.Trips)
                    .ToList();*/
        }

        private void LoadMembers()
        {
            if (MembersFiltered == null || (DateTime.Now - _latestData).TotalHours > 1)
            {
                _latestData = DateTime.Now;

                var members = db.Member.Where(member => member.Active).OrderBy(x => x.FirstName).ToList();
                MembersFiltered =
                    new ObservableCollection<MemberViewModel>(members.Select(member => new MemberViewModel(member)));
            }
        }

        private void ResetData()
        {
            // Reset filter
            ResetFilter();

            // Reset lists
            SelectedMembers.Clear();
            EnableMembers = false;

            _selectedBoat = null;
            NotifyCustom("SelectedBoat");

            UpdateInfo();
        }

        private void ResetFilter()
        {
            Boats = new ObservableCollection<Boat>(_boats.ToList());
            foreach (MemberViewModel member in MembersFiltered)
            {
                member.Visible = true;
            }
        }

        private void SortBoats(Func<Boat, string> predicate)
        {
            Boats = Boats.OrderBy(predicate);
        }

        private void SortMembers(Func<MemberViewModel, string> predicate)
        {
            MembersFiltered = MembersFiltered.OrderBy(predicate);
        }

        private void UpdateFilter(FilterChangedEventArgs args)
        {
            // Reset filters
            ResetFilter();

            if ((args.FilterEventArgs == null || !args.FilterEventArgs.Filters.Any())
                && (args.SearchEventArgs == null || string.IsNullOrEmpty(args.SearchEventArgs.SearchText)))
            {
                return;
            }

            // Filter only boats
            if (args.FilterEventArgs != null && args.FilterEventArgs.Filters.Any())
            {
                Boats = FilterContent.FilterItems(Boats, args.FilterEventArgs);
            }

            // Check search
            if (args.SearchEventArgs != null && !string.IsNullOrEmpty(args.SearchEventArgs.SearchText))
            {
                Boats = from boat in Boats where boat.Filter(args.SearchEventArgs.SearchText) select boat;

                foreach (MemberViewModel member in
                    MembersFiltered.Where(member => !member.Member.Filter(args.SearchEventArgs.SearchText)))
                {
                    member.Visible = false;
                }
            }
        }

        private void UpdateInfo()
        {
            Info.SelectedBoat = SelectedBoat;
            Info.SelectedMembers = SelectedMembers;

            ProtocolSystem.ChangeInfo(InfoPage, Info);
        }
    }
}