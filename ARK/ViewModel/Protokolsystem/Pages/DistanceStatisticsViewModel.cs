using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using ARK.Model.Extensions;
using ARK.View.Protokolsystem.Filters;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces.Filter;
using ARK.ViewModel.Protokolsystem.Data;

namespace ARK.ViewModel.Protokolsystem
{
    internal class DistanceStatisticsViewModel : ProtokolsystemContentViewModelBase, IFilterContentViewModel
    {
        // Fields
        private IEnumerable<MemberDistanceViewModel> _memberKmCollection;
        private IEnumerable<MemberDistanceViewModel> _memberKmCollectionFiltered;
        private MemberDistanceViewModel _selectedMember;

        // Constructor
        public DistanceStatisticsViewModel()
        {
            DbArkContext db = DbArkContext.GetDbContext();

            ParentAttached += (sender, e) =>
            {
                // Load data
                IEnumerable<Member> members = db.Member
                    .OrderBy(x => x.FirstName)
                    .Include(m => m.Trips)
                    .AsEnumerable();

                _memberKmCollection = members.Select((member, i) => new MemberDistanceViewModel(member));
                MemberKmCollectionFiltered = _memberKmCollection;

                // Order list
                OrderFilter();

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
                    MemberKmCollectionFiltered.Where(member => member.Member.Filter(args.SearchEventArgs.SearchText));
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
    }
}