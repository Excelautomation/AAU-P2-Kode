using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using ARK.Model.Extensions;
using ARK.Protokolsystem.Pages;
using ARK.View.Protokolsystem.Filters;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces;
using ARK.ViewModel.Base.Interfaces.Filter;
using ARK.ViewModel.Base.Interfaces.Info;
using ARK.ViewModel.Protokolsystem.Additional;
using ARK.View.Protokolsystem.Additional;

namespace ARK.ViewModel.Protokolsystem
{
    public class BeginTripViewModel : ProtokolsystemContentViewModelBase, IFilterContentViewModel
    {
        private readonly IEnumerable<Boat> _boats; // All boats
        private readonly ObservableCollection<MemberViewModel> _selectedMembers; // Members in boat
        private readonly DbArkContext db; // Database
        private IEnumerable<Boat> _boatsFiltered; // Boats to display

        private bool _enableMembers; // Used to determine whether the members-listview should be enabled
        private FrameworkElement _infoPage; // Informationpage
        private IEnumerable<Member> _members; // All members
        private IEnumerable<MemberViewModel> _membersFiltered; // Members to display
        private Boat _selectedBoat; // Holds the selected boat
        private FrameworkElement _filter;

        #region Constructors
        public BeginTripViewModel()
        {
            TimeCounter.StartTimer();

            // Establish connection to DB
            db = DbArkContext.GetDbContext();

            // Load data. Check the boats activitylevel on a 8-day-basis
            _boats = db.Boat.AsEnumerable().Where(x => x.Active).OrderByDescending(x => 
                db.Trip.OrderByDescending(t => t.Id).Take(50).Count(y => y.BoatId == x.Id)).ToList();
            //var temp = _db.Boat
            //    .Where(x => x.Active)
            //    .Include(x => x.Trips);

            //var temp2 = temp
            //    .Select(b => b.Trips
            //        .Where(t => t.TripStartTime > limit));

            //var temp3 = temp2
            //    .Select(x => x.Aggregate(0, (a, b) => a + 1));

            //_boats = temp3.AsEnumerable()
            //    .Select((a, b) => temp.ElementAt(b));
            _members = db.Member.OrderBy(x => x.FirstName).ToList();

            // Initialize lists and set members
            _selectedMembers = new ObservableCollection<MemberViewModel>();
            Members = new ObservableCollection<MemberViewModel>(_members.Select(member => new MemberViewModel(member)));

            // Reset filter
            ResetFilter();

            // Setup filter
            var filterController = new FilterContent(this);
            filterController.EnableFilter(true, false);
            filterController.FilterChanged += (o, eventArgs) => UpdateFilter(eventArgs);

            // Configurate the keyboard
            ParentAttached += (sender, args) =>
            {
                // Bind on keyboard toggle changed
                Keyboard.PropertyChanged += (senderKeyboard, keyboardArgs) =>
                {
                    // Check whether or not the toggle has changed
                    if (keyboardArgs.PropertyName == "KeyboardToggled")
                        NotifyCustom("KeyboardToggleText");
                };

                // Notify that parent has changed
                NotifyCustom("Keyboard");
                NotifyCustom("KeyboardToggleText");

                UpdateInfo();
            };

            TimeCounter.StopTime();
        }
        #endregion

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
                _selectedBoat = value;
                Notify();
            }
        }

        public IEnumerable<MemberViewModel> Members
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
            get { return Members.Count(member => member.Visible); }
        }

        public ObservableCollection<MemberViewModel> SelectedMembers
        {
            get { return _selectedMembers; }
        }

        public ICommand StartTripNow
        {
            get { return GetCommand<object>(x => 
            { 
                Trip trip = new Trip();
                trip.Id = db.Trip.OrderByDescending(t => t.Id).First(y => true).Id + 1;
                trip.TripStartTime = DateTime.Now;
                trip.Members = new List<Member>();
                trip.BoatId = SelectedBoat.Id;

                // Add selected members to trip
                foreach (var m in SelectedMembers.Select(member => member.Member))
                {
                    trip.Members.Add(m);
                }

                trip.LongTrip = LongTrip;
                trip.Direction = Direction;

                db.Trip.Add(trip);
                db.SaveChanges();

                SelectedBoat = null;
            }); }
        }

        public IInfoContainerViewModel GetInfoContainerViewModel
        {
            get { return Parent as IInfoContainerViewModel; }
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

        public FrameworkElement InfoPage
        {
            get { return _infoPage ?? (_infoPage = new BeginTripAdditionalInfo()); }
        }

        public BeginTripAdditionalInfoViewModel Info
        {
            get { return InfoPage.DataContext as BeginTripAdditionalInfoViewModel; }
        }

        public ICommand BoatSelected
        {
            get
            {
                return GetCommand<Boat>(e =>
                {
                    if (e == null)
                        return;

                    EnableMembers = true;
                    SelectedBoat = e;
                    Keyboard.KeyboardClear();
                    SelectedMembers.Clear();
                    UpdateInfo();
                });
            }
        }

        #endregion

        private void UpdateInfo()
        {
            Info.SelectedBoat = new ObservableCollection<Boat> { SelectedBoat };
            Info.SelectedMembers = SelectedMembers;

            GetInfoContainerViewModel.ChangeInfo(InfoPage, Info);
        }

        #region Filter

        public FrameworkElement Filter
        {
            get { return _filter ?? (_filter = new BeginTripFilters()); }
        }

        private void ResetFilter()
        {
            Boats = new ObservableCollection<Boat>(_boats);
            foreach (MemberViewModel member in Members)
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
                        where boat.FilterBoat(args.SearchEventArgs.SearchText)
                    select boat;

                foreach (
                    var member in Members.Where(member => !member.Member.FilterMembers(args.SearchEventArgs.SearchText)))
                    member.Visible = false;
            }
        }
        #endregion

        #region Sorting
        void SortBoats(Func<Boat, string> predicate)
        {
            Boats = Boats.OrderBy(predicate);
        }
        void SortMembers(Func<MemberViewModel, string> predicate)
        {
            Members = Members.OrderBy(predicate);
        }
        #endregion

        
    }
}