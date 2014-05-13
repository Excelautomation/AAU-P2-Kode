using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ARK.Model.DB;
using ARK.Model.Extensions;
using ARK.View.Protokolsystem.Additional;
using ARK.View.Protokolsystem.Filters;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces.Filter;
using ARK.ViewModel.Protokolsystem.Additional;
using ARK.ViewModel.Protokolsystem.Data;
using ARK.Model;

namespace ARK.ViewModel.Protokolsystem.Pages
{
    internal class DistanceStatisticsViewModel : ProtokolsystemContentViewModelBase, IFilterContentViewModel
    {
        // Fields
        private readonly DbArkContext _db;
        private DateTime _latestData;
        private IEnumerable<MemberDistanceViewModel> _memberKmCollection;
        private IEnumerable<MemberDistanceViewModel> _memberKmCollectionFiltered;
        private MemberDistanceViewModel _selectedMember;
        private FrameworkElement _additionalInfoPage;
        private TripViewModel _selectedTrip;


        // Constructor
        public DistanceStatisticsViewModel()
        {
            _db = DbArkContext.GetDbContext();

            ParentAttached += (sender, e) =>
            {
                // Load data and order list
                LoadMembers();
                MemberKmCollectionFiltered = _memberKmCollection.ToList();

                // Set selected member
                SelectedMember = MemberKmCollectionFiltered.First();
            };

            // Setup filter
            var filterController = new FilterContent(this);
            filterController.EnableFilter(false, false);
            filterController.FilterChanged += (o, eventArgs) => UpdateFilter(eventArgs);
        }

        public MemberDistanceViewModel SelectedMember
        {
            get { return _selectedMember; }
            set
            {
                _selectedMember = value;
                Notify();

                UpdateInfo();
            }
        }

        public TripViewModel SelectedTrip
        {
            get { return _selectedTrip; }
            set { 
                _selectedTrip = value;
                Notify();

                UpdateInfo();
            }
        }

        public IEnumerable<MemberDistanceViewModel> MemberKmCollectionFiltered
        {
            get { return _memberKmCollectionFiltered; }
            private set
            {
                _memberKmCollectionFiltered = value;
                Notify();
            }
        }

        public ICommand MemberSelectionChanged
        {
            get { return GetCommand<MemberDistanceViewModel>(e => { SelectedMember = e; }); }
        }

        #region Filter

        public FrameworkElement Filter
        {
            get { return new DistanceStatisticsFilters(); }
        }

        private void ResetFilter()
        {
            MemberKmCollectionFiltered = _memberKmCollection.ToList();
        }

        private void OrderFilter()
        {
            MemberKmCollectionFiltered =
                MemberKmCollectionFiltered.OrderByDescending(member => member.Distance).ToList();
        }

        private void UpdateFilter(FilterChangedEventArgs args)
        {
            ResetFilter();

            if ((args.FilterEventArgs == null || !args.FilterEventArgs.Filters.Any()) &&
                (args.SearchEventArgs == null || string.IsNullOrEmpty(args.SearchEventArgs.SearchText)))
            {
                // Order filter
                OrderFilter();

                return;
            }

            // Search
            if (args.SearchEventArgs != null && !string.IsNullOrEmpty(args.SearchEventArgs.SearchText))
            {
                MemberKmCollectionFiltered =
                    MemberKmCollectionFiltered.Where(member => member.Member.Filter(args.SearchEventArgs.SearchText)).ToList();
            }

            // Filter
            if (args.FilterEventArgs != null && args.FilterEventArgs.Filters.Any())
            {
                foreach (MemberDistanceViewModel elm in MemberKmCollectionFiltered)
                    elm.UpdateFilter(args);
            }

            // Order filter
            OrderFilter();
        }

        #endregion

        private void LoadMembers()
        {
            if (_memberKmCollection == null || (DateTime.Now - _latestData).TotalHours > 1)
            {
                _latestData = DateTime.Now;

                _memberKmCollection = _db.Member
                    .OrderBy(x => x.FirstName)
                    .Include(m => m.Trips)
                    .AsEnumerable()
                    .Select((member, i) => new MemberDistanceViewModel(member))
                    .OrderByDescending(member => member.Distance)
                    .ToList();
            }
            else
            {
                foreach (var member in _memberKmCollection)
                {
                    member.ResetFilter();
                    member.UpdateDistance();
                }
            }
        }

        public FrameworkElement InfoPage
        {
            get { return _additionalInfoPage ?? (_additionalInfoPage = new DistanceStatisticsAdditionalInfo()); }
        }

        public DistanceStatisticsAdditionalInfoViewModel Info
        {
            get { return InfoPage.DataContext as DistanceStatisticsAdditionalInfoViewModel; }
        }

        private void UpdateInfo()
        {
            Info.SelectedMember = SelectedMember;
            Info.SelectedTrip = SelectedTrip;

            ProtocolSystem.ChangeInfo(InfoPage, Info);
        }
    }
}