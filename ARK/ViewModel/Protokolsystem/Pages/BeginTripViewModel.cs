using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ARK.HelperFunctions;
using ARK.Model;
using ARK.Model.DB;
using ARK.Model.Extensions;
using ARK.View.Protokolsystem.Additional;
using ARK.View.Protokolsystem.Filters;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces;
using ARK.ViewModel.Base.Interfaces.Filter;
using ARK.ViewModel.Protokolsystem.Additional;
using ARK.ViewModel.Protokolsystem.Data;
using ARK.ViewModel.Protokolsystem.Confirmations;
using ARK.View.Protokolsystem.Confirmations;

namespace ARK.ViewModel.Protokolsystem.Pages
{
    public class BeginTripViewModel : ProtokolsystemContentViewModelBase, IFilterContentViewModel
    {
        private readonly DbArkContext db = DbArkContext.GetDbContext(); // Database
        private ObservableCollection<MemberViewModel> _selectedMembers; // Members in boat
        private IEnumerable<Boat> _boats; // All boats
        private IEnumerable<Boat> _boatsFiltered; // Boats to display
        private IEnumerable<MemberViewModel> _membersFiltered; // Members to display

        private bool _enableMembers; // Used to determine whether the members-listview should be enabled
        private FrameworkElement _infoPage; // Informationpage
        private Boat _selectedBoat; // Holds the selected boat


        private DateTime _latestData;

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

        private void LoadBoats()
        {
            // Load data. Check the boats activitylevel on a 8-day-basis
            DateTime limit = DateTime.Now.AddDays(-8);

            _boats = db.Boat
                .Where(boat => boat.Active)
                .OrderBy(boat => boat.Trips.Any(trip => trip.TripEndedTime == null))
                .ThenByDescending(boat => boat.Trips.Count(trip => trip.TripStartTime > limit))
                .Include(boat => boat.Trips)
                .ToList();
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

        #region Properties

        public bool LongTrip { get; set; }

        public string Direction { get; set; }

        public IEnumerable<Boat> Boats
        {
            get { return _boatsFiltered; }
            set
            {
                _boatsFiltered = value;
                Notify();
            }
        }

        public Boat SelectedBoat
        {
            get { return _selectedBoat; }
            set
            {
                if (_selectedBoat == value || // Nothing changed - silently discard - STACKOVERFLOW IF NOT DISCARDED (Keyboard chaning LV)
                    value == null) 
                    return;
                
                _selectedBoat = value;

                ProtocolSystem.KeyboardClear();
                SelectedMembers.Clear();
                UpdateInfo();

                EnableMembers = true;
            }
        }

        public IEnumerable<MemberViewModel> MembersFiltered
        {
            get { return _membersFiltered; }
            set
            {
                _membersFiltered = value;
                Notify();
                NotifyCustom("MembersCount");
            }
        }

        public int MembersCount
        {
            get { return MembersFiltered.Count(member => member.Visible); }
        }

        public ObservableCollection<MemberViewModel> SelectedMembers
        {
            get { return _selectedMembers; }
            set
            {
                _selectedMembers = value;
                Notify();
            }
        }

        public ICommand AddBlank
        {
            get
            {
                return GetCommand(() =>
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
                return GetCommand(() =>
                {
                    if (SelectedMembers.Count < SelectedBoat.NumberofSeats)
                    {
                        SelectedMembers.Add(new MemberViewModel(new Member() { Id = -2, FirstName = "Gæst" }));
                    }
                });
            }
                    }

        public ICommand DirectionSelected
        {
            get
            {
                return new RelayCommand(x =>
                {
                    var temp = x as string;
                    this.Direction = temp.Trim(new[] { '\n' });
                });
            }
        }

        public bool EnableMembers
        {
            get { return _enableMembers; }
            set
            {
                _enableMembers = value;
                Notify();
            }
        }

        private FrameworkElement InfoPage
        {
            get { return _infoPage ?? (_infoPage = new BeginTripAdditionalInfo()); }
        }

        private BeginTripAdditionalInfoViewModel Info
        {
            get { return InfoPage.DataContext as BeginTripAdditionalInfoViewModel; }
        }

        #endregion

        public ICommand ShowConfirmDialog
        {
            get 
            { 
                return new RelayCommand(
                    x => ConfirmTripData(),
                    x => this.SelectedBoat != null && this.SelectedMembers.Count == this.SelectedBoat.NumberofSeats && this.Direction != null);
            }
        }

        public void ConfirmTripData()
        {
            {
                Trip trip = new Trip
                {
                    Id = db.Trip.OrderByDescending(t => t.Id).First().Id + 1,
                    TripStartTime = DateTime.Now,
                    Members = new List<Member>(),
                    Boat = SelectedBoat
                };

                // Add selected members to trip
                foreach (var m in SelectedMembers.Select(member => member.Member))
                {
                    if (m.Id == -1)
                    {
                        //-1 is a blank spot => Do nothing
                    }
                    else if (m.Id == -2)
                    {
                        //-2 is a guest => Increment the crew count, but don't add the member to the member list
                        trip.CrewCount++;
                    }
                    else
                    {
                        //Add the member reference and increment the crew count
                        trip.Members.Add(m);
                        trip.CrewCount++;
                    }
                }

                trip.LongTrip = LongTrip;
                trip.Direction = Direction;

                var dlg = new BeginTripBoatsConfirm();
                var ConfirmTripViewModel = (BeginTripBoatsConfirmViewModel)dlg.DataContext;
                ConfirmTripViewModel.Trip = trip;
                ProtocolSystem.ShowDialog(dlg);

                // hold øje med om denne bliver kørt
                //mainViewModel.UpdateNumBoatsOut();
            }
        }

        //public ICommand ShowConfirmDialog
        //{
        //    get 
        //    { 
        //        return GetCommand<object>(e =>
        //        {    
        //            var trip = new Trip
        //            {
        //                Id = _db.Trip.OrderByDescending(t => t.Id).First().Id + 1,
        //                TripStartTime = DateTime.Now,
        //                Members = new List<Member>(),
        //                Boat = SelectedBoat
        //            };
                
        //            foreach (var m in SelectedMembers.Select(member => member.Member))
        //            {
        //                if (m.Id == -1)
        //                {
        //                    //-1 is a blank spot => Do nothing
        //                }
        //                else if (m.Id == -2)
        //                {
        //                    //-2 is a guest => Increment the crew count, but don't add the member to the member list
        //                    trip.CrewCount++;
        //                }
        //                else
        //                {
        //                    //Add the member reference and increment the crew count
        //                    trip.Members.Add(m);
        //                    trip.CrewCount++;
        //                }
        //            }

        //            trip.LongTrip = LongTrip;
        //            trip.Direction = Direction;
                
        //            // åbner dialog vinduet
        //            ShowDialog(new BeginTripBoatsConfirm()); 
        //        });
        //    }
        //}

        private void UpdateInfo()
        {
            Info.SelectedBoat = SelectedBoat;
            Info.SelectedMembers = SelectedMembers;

            ProtocolSystem.ChangeInfo(InfoPage, Info);
        }

        #region Filter

        public FrameworkElement Filter
        {
            get { return new BeginTripFilters(); }
        }

        private void ResetFilter()
        {
            Boats = new ObservableCollection<Boat>(_boats.ToList());
            foreach (MemberViewModel member in MembersFiltered)
                member.Visible = true;
        }

        private void UpdateFilter(FilterChangedEventArgs args)
        {
            // Reset filters
            ResetFilter();

            if ((args.FilterEventArgs == null || !args.FilterEventArgs.Filters.Any()) &&
                (args.SearchEventArgs == null || string.IsNullOrEmpty(args.SearchEventArgs.SearchText)))
                return;

            // Filter only boats
            if (args.FilterEventArgs != null && args.FilterEventArgs.Filters.Any())
            {
                Boats = FilterContent.FilterItems(Boats, args.FilterEventArgs);
            }

            // Check search
            if (args.SearchEventArgs != null && !string.IsNullOrEmpty(args.SearchEventArgs.SearchText))
            {
                Boats = from boat in Boats
                    where boat.Filter(args.SearchEventArgs.SearchText)
                    select boat;

                foreach (
                    MemberViewModel member in
                        MembersFiltered.Where(member => !member.Member.Filter(args.SearchEventArgs.SearchText)))
                    member.Visible = false;
            }
        }

        #endregion

        #region Sorting

        private void SortBoats(Func<Boat, string> predicate)
        {
            Boats = Boats.OrderBy(predicate);
        }

        private void SortMembers(Func<MemberViewModel, string> predicate)
        {
            MembersFiltered = MembersFiltered.OrderBy(predicate);
        }

        #endregion
    }
}