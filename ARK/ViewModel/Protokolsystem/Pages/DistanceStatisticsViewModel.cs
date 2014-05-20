using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;

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
    public class DistanceStatisticsViewModel : ProtokolsystemContentViewModelBase, IFilterContentViewModel
    {
        // Fields
        private readonly DbArkContext _db;

        private FrameworkElement _additionalInfoPage;

        private bool _distanceSelector;

        private DateTime _latestData;

        private IEnumerable<MemberDistanceViewModel> _memberKmCollection;

        private IEnumerable<MemberDistanceViewModel> _memberKmCollectionFiltered;

        private MemberDistanceViewModel _selectedMember;

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

        public ICommand ChangeDistance
        {
            get
            {
                return new RelayCommand(
                    x =>
                        {
                            DistanceSelector = true;

                            var confirmView = new ChangeDistanceConfirm();
                            var confirmViewModel = (ChangeDistanceConfirmViewModel)confirmView.DataContext;

                            confirmViewModel.SelectedTrip = SelectedTrip.Trip;
                            ProtocolSystem.ShowDialog(confirmView);

                            confirmViewModel.WindowHide += confirmViewModel_WindowHide;
                        }, 
                    x => SelectedTrip != null && SelectedTrip.Editable);
            }
        }

        public DistanceStatisticsAdditionalInfoViewModel Info
        {
            get
            {
                return InfoPage.DataContext as DistanceStatisticsAdditionalInfoViewModel;
            }
        }

        public FrameworkElement InfoPage
        {
            get
            {
                return _additionalInfoPage ?? (_additionalInfoPage = new DistanceStatisticsAdditionalInfo());
            }
        }

        public IEnumerable<MemberDistanceViewModel> MemberKmCollectionFiltered
        {
            get
            {
                return _memberKmCollectionFiltered;
            }

            private set
            {
                _memberKmCollectionFiltered = value;
                Notify();
            }
        }

        public ICommand MemberSelectionChanged
        {
            get
            {
                return GetCommand(e => { SelectedMember = (MemberDistanceViewModel)e; });
            }
        }

        public MemberDistanceViewModel SelectedMember
        {
            get
            {
                return _selectedMember;
            }

            set
            {
                _selectedMember = value;
                Notify();

                UpdateInfo();
            }
        }

        public TripViewModel SelectedTrip
        {
            get
            {
                return _selectedTrip;
            }

            set
            {
                _selectedTrip = value;
                Notify();

                UpdateInfo();
            }
        }

        private bool DistanceSelector
        {
            get
            {
                return _distanceSelector;
            }

            set
            {
                _distanceSelector = value;
                base.ProtocolSystem.EnableSearch = true;
            }
        }

        public FrameworkElement Filter
        {
            get
            {
                return new DistanceStatisticsFilters();
            }
        }

        public void NotifyTripList()
        {
            NotifyCustom("SelectedMember");
            NotifyCustom("SelectedTrip");
        }

        private void LoadMembers()
        {
            if (_memberKmCollection == null || (DateTime.Now - _latestData).TotalHours > 1)
            {
                _latestData = DateTime.Now;

                _memberKmCollection =
                    _db.Member.OrderBy(x => x.FirstName)
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

        private void OrderFilter()
        {
            MemberKmCollectionFiltered =
                MemberKmCollectionFiltered.OrderByDescending(member => member.Distance).ToList();
        }

        private void ResetFilter()
        {
            MemberKmCollectionFiltered = _memberKmCollection.ToList();
        }

        private void UpdateFilter(FilterChangedEventArgs args)
        {
            // Ignore filter if distance is changing
            if (DistanceSelector)
            {
                return;
            }

            ResetFilter();

            if ((args.FilterEventArgs == null || !args.FilterEventArgs.Filters.Any())
                && (args.SearchEventArgs == null || string.IsNullOrEmpty(args.SearchEventArgs.SearchText)))
            {
                // Order filter
                OrderFilter();
                UpdateRank();

                return;
            }

            // Filter
            if (args.FilterEventArgs != null && args.FilterEventArgs.Filters.Any())
            {
                foreach (MemberDistanceViewModel elm in MemberKmCollectionFiltered)
                {
                    elm.UpdateFilter(args);
                }
            }

            // Order filter
            OrderFilter();
            UpdateRank();

            // Search
            if (args.SearchEventArgs != null && !string.IsNullOrEmpty(args.SearchEventArgs.SearchText))
            {
                MemberKmCollectionFiltered =
                    MemberKmCollectionFiltered.Where(member => member.Member.Filter(args.SearchEventArgs.SearchText))
                        .ToList();
            }
        }

        private void UpdateInfo()
        {
            Info.SelectedMember = SelectedMember;
            Info.SelectedTrip = SelectedTrip;

            ProtocolSystem.ChangeInfo(InfoPage, Info);
        }

        private void UpdateRank()
        {
            MemberKmCollectionFiltered.Aggregate(
                1, 
                (acc, val) =>
                    {
                        val.Position = acc;
                        return acc + 1;
                    });
        }

        private void confirmViewModel_WindowHide(object sender, EventArgs e)
        {
            ((ChangeDistanceConfirmViewModel)sender).WindowHide -= confirmViewModel_WindowHide;

            DistanceSelector = false;
        }
    }
}