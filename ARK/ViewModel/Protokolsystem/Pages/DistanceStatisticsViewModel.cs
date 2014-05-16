using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
using ARK.ViewModel.Base.Interfaces.Filter;
using ARK.ViewModel.Protokolsystem.Additional;
using ARK.ViewModel.Protokolsystem.Confirmations;
using ARK.ViewModel.Protokolsystem.Data;

namespace ARK.ViewModel.Protokolsystem.Pages
{
    public class DistanceStatisticsViewModel : ProtokolsystemContentViewModelBase, IFilterContentViewModel
    {
        // Fields
        #region Fields

        private readonly DbArkContext _db;

        private FrameworkElement _additionalInfoPage;

        private bool _distanceSelector;

        private DateTime _latestData;

        private IEnumerable<MemberDistanceViewModel> _memberKmCollection;

        private IEnumerable<MemberDistanceViewModel> _memberKmCollectionFiltered;

        private MemberDistanceViewModel _selectedMember;

        private TripViewModel _selectedTrip;

        #endregion

        // Constructor
        #region Constructors and Destructors

        public DistanceStatisticsViewModel()
        {
            this._db = DbArkContext.GetDbContext();

            this.ParentAttached += (sender, e) =>
                {
                    // Load data and order list
                    this.LoadMembers();
                    this.MemberKmCollectionFiltered = this._memberKmCollection.ToList();

                    // Set selected member
                    this.SelectedMember = this.MemberKmCollectionFiltered.First();
                };

            // Setup filter
            var filterController = new FilterContent(this);
            filterController.EnableFilter(false, false);
            filterController.FilterChanged += (o, eventArgs) => this.UpdateFilter(eventArgs);
        }

        #endregion

        #region Public Properties

        public ICommand ChangeDistance
        {
            get
            {
                return new RelayCommand(
                    x =>
                        {
                            this.DistanceSelector = true;

                            var confirmView = new ChangeDistanceConfirm();
                            var confirmViewModel = (ChangeDistanceConfirmViewModel)confirmView.DataContext;

                            confirmViewModel.SelectedTrip = this.SelectedTrip.Trip;
                            this.ProtocolSystem.ShowDialog(confirmView);

                            confirmViewModel.WindowHide += this.confirmViewModel_WindowHide;
                        }, 
                    x => this.SelectedTrip != null && this.SelectedTrip.Editable);
            }
        }

        public FrameworkElement Filter
        {
            get
            {
                return new DistanceStatisticsFilters();
            }
        }

        public DistanceStatisticsAdditionalInfoViewModel Info
        {
            get
            {
                return this.InfoPage.DataContext as DistanceStatisticsAdditionalInfoViewModel;
            }
        }

        public FrameworkElement InfoPage
        {
            get
            {
                return this._additionalInfoPage ?? (this._additionalInfoPage = new DistanceStatisticsAdditionalInfo());
            }
        }

        public IEnumerable<MemberDistanceViewModel> MemberKmCollectionFiltered
        {
            get
            {
                return this._memberKmCollectionFiltered;
            }

            private set
            {
                this._memberKmCollectionFiltered = value;
                this.Notify();
            }
        }

        public ICommand MemberSelectionChanged
        {
            get
            {
                return this.GetCommand(e => { this.SelectedMember = (MemberDistanceViewModel)e; });
            }
        }

        public MemberDistanceViewModel SelectedMember
        {
            get
            {
                return this._selectedMember;
            }

            set
            {
                this._selectedMember = value;
                this.Notify();

                this.UpdateInfo();
            }
        }

        public TripViewModel SelectedTrip
        {
            get
            {
                return this._selectedTrip;
            }

            set
            {
                this._selectedTrip = value;
                this.Notify();

                this.UpdateInfo();
            }
        }

        #endregion

        #region Properties

        private bool DistanceSelector
        {
            get
            {
                return this._distanceSelector;
            }

            set
            {
                this._distanceSelector = value;
                base.ProtocolSystem.EnableSearch = true;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void NotifyTripList()
        {
            this.NotifyCustom("SelectedMember");
            this.NotifyCustom("SelectedTrip");
        }

        #endregion

        #region Methods

        private void LoadMembers()
        {
            if (this._memberKmCollection == null || (DateTime.Now - this._latestData).TotalHours > 1)
            {
                this._latestData = DateTime.Now;

                this._memberKmCollection =
                    this._db.Member.OrderBy(x => x.FirstName)
                        .Include(m => m.Trips)
                        .AsEnumerable()
                        .Select((member, i) => new MemberDistanceViewModel(member))
                        .OrderByDescending(member => member.Distance)
                        .ToList();
            }
            else
            {
                foreach (var member in this._memberKmCollection)
                {
                    member.ResetFilter();
                    member.UpdateDistance();
                }
            }
        }

        private void OrderFilter()
        {
            this.MemberKmCollectionFiltered =
                this.MemberKmCollectionFiltered.OrderByDescending(member => member.Distance).ToList();
        }

        private void ResetFilter()
        {
            this.MemberKmCollectionFiltered = this._memberKmCollection.ToList();
        }

        private void UpdateFilter(FilterChangedEventArgs args)
        {
            // Ignore filter if distance is changing
            if (this.DistanceSelector)
            {
                return;
            }

            this.ResetFilter();

            if ((args.FilterEventArgs == null || !args.FilterEventArgs.Filters.Any())
                && (args.SearchEventArgs == null || string.IsNullOrEmpty(args.SearchEventArgs.SearchText)))
            {
                // Order filter
                this.OrderFilter();
                this.UpdateRank();

                return;
            }

            // Filter
            if (args.FilterEventArgs != null && args.FilterEventArgs.Filters.Any())
            {
                foreach (MemberDistanceViewModel elm in this.MemberKmCollectionFiltered)
                {
                    elm.UpdateFilter(args);
                }
            }

            // Order filter
            this.OrderFilter();
            this.UpdateRank();

            // Search
            if (args.SearchEventArgs != null && !string.IsNullOrEmpty(args.SearchEventArgs.SearchText))
            {
                this.MemberKmCollectionFiltered =
                    this.MemberKmCollectionFiltered.Where(
                        member => member.Member.Filter(args.SearchEventArgs.SearchText)).ToList();
            }
        }

        private void UpdateInfo()
        {
            this.Info.SelectedMember = this.SelectedMember;
            this.Info.SelectedTrip = this.SelectedTrip;

            this.ProtocolSystem.ChangeInfo(this.InfoPage, this.Info);
        }

        private void UpdateRank()
        {
            this.MemberKmCollectionFiltered.Aggregate(
                1, 
                (acc, val) =>
                    {
                        val.Position = acc;
                        return acc + 1;
                    });
        }

        private void confirmViewModel_WindowHide(object sender, EventArgs e)
        {
            ((ChangeDistanceConfirmViewModel)sender).WindowHide -= this.confirmViewModel_WindowHide;

            this.DistanceSelector = false;
        }

        #endregion
    }
}