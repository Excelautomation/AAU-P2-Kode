﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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

namespace ARK.ViewModel.Protokolsystem
{
    public class BeginTripViewModel : ProtokolsystemContentViewModelBase, IFilterContentViewModel
    {
        private readonly DbArkContext _db = DbArkContext.GetDbContext(); // Database
        private readonly ObservableCollection<MemberViewModel> _selectedMembers; // Members in boat
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
            filterController.FilterChanged += (o, eventArgs) => UpdateFilter(eventArgs);

            // Set up variables to load of data
            Task<List<Boat>> boatsAsync = null;
            Task<List<Member>> membersAsync = null;

            ParentAttached += (sender, args) =>
            {
                if (boatsAsync == null || membersAsync == null || (DateTime.Now - _latestData).TotalHours > 2)
                {
                    // Load data. Check the boats activitylevel on a 8-day-basis
                    DateTime limit = DateTime.Now.AddDays(-8);

                    // Async start load of data
                    boatsAsync = _db.Boat
                        .Where(b => b.Active)
                        .OrderByDescending(b => b.Trips.Count(t => t.TripStartTime > limit)).ToListAsync();
                    membersAsync = _db.Member.OrderBy(x => x.FirstName).ToListAsync();

                    // Set date
                    _latestData = DateTime.Now;

                    // Read data
                    _boats = boatsAsync.Result;
                    MembersFiltered = new ObservableCollection<MemberViewModel>(membersAsync.Result.Select(member => new MemberViewModel(member)));
                }

                ResetData();
            };
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
                if (_selectedBoat == value || // Nothing changed - silently discart - STACKOVERFLOW IF NOT DISCARTED
                    value == null) 
                    return;
                
                _selectedBoat = value;

                Keyboard.KeyboardClear();
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
        }

        public ICommand StartTripNow
        {
            get
            {
                return GetCommand<object>(x =>
                {
                    var trip = new Trip
                    {
                        Id = _db.Trip.OrderByDescending(t => t.Id).First(y => true).Id + 1,
                        TripStartTime = DateTime.Now,
                        Members = new List<Member>(),
                        BoatId = SelectedBoat.Id
                    };

                    // Add selected members to trip
                    foreach (var m in SelectedMembers.Select(member => member.Member))
                    {
                        trip.Members.Add(m);
                    }

                    trip.LongTrip = LongTrip;
                    trip.Direction = Direction;

                    _db.Trip.Add(trip);
                    _db.SaveChanges();

                    var mainViewModel = Parent as ProtocolSystemMainViewModel;
                    mainViewModel.UpdateNumBoatsOut();

                    ResetData();
                });
            }
        }

        private IInfoContainerViewModel GetInfoContainerViewModel
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

        private FrameworkElement InfoPage
        {
            get { return _infoPage ?? (_infoPage = new BeginTripAdditionalInfo()); }
        }

        private BeginTripAdditionalInfoViewModel Info
        {
            get { return InfoPage.DataContext as BeginTripAdditionalInfoViewModel; }
        }

        #endregion

        private void UpdateInfo()
        {
            Info.SelectedBoat = new ObservableCollection<Boat> {SelectedBoat};
            Info.SelectedMembers = SelectedMembers;

            GetInfoContainerViewModel.ChangeInfo(InfoPage, Info);
        }

        #region Filter

        public FrameworkElement Filter
        {
            get { return new BeginTripFilters(); }
        }

        private void ResetFilter()
        {
            Boats = new ObservableCollection<Boat>(_boats);
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