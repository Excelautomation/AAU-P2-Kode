using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ARK.Extensions;
using ARK.Model;
using ARK.Model.DB;
using ARK.Protokolsystem.Pages;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Filter;
using ARK.ViewModel.Base.Interfaces;

namespace ARK.ViewModel.Protokolsystem
{
    public class BeginTripViewModel : KeyboardContentViewModelBase
    {
        private readonly IEnumerable<Boat> _boats; // All boats
        private readonly ObservableCollection<MemberViewModel> _selectedMembers; // members in boat
        private readonly DbArkContext db;
        private IEnumerable<Boat> _boatsFiltered; // boats to display

        private bool _enableMembers;
        private FrameworkElement _infoPage;
        private IEnumerable<Member> _members; // All members
        private IEnumerable<MemberViewModel> _membersFiltered; // members to display
        private Boat _selectedBoat;

        // Constructor
        public BeginTripViewModel()
        {
            TimeCounter.StartTimer();

            // Opret forbindelse til DB
            db = DbArkContext.GetDbContext();

            // Load data
            _boats = new List<Boat>(db.Boat).Where(x => x.Usable).OrderBy(x => x.NumberofSeats).ToList();
            _members = new List<Member>(db.Member).Select(x =>
            {
                x.FirstName = x.FirstName.Trim();
                return x;
            }).OrderBy(x => x.FirstName).ToList();

            // Instaliser lister og sæt members
            _selectedMembers = new ObservableCollection<MemberViewModel>();
            Members = new ObservableCollection<MemberViewModel>(_members.Select(member => new MemberViewModel(member)));

            // Nulstil filter
            ResetFilter();

            // Setup filter
            var filterController = new FilterContent(this);
            filterController.EnableFilter(true, false, null);
            filterController.FilterChanged += (o, eventArgs) => UpdateFilter(eventArgs);

            // Sæt keyboard op
            ParentAttached += (sender, args) =>
            {
                // Bind på keyboard toggle changed
                Keyboard.PropertyChanged += (senderKeyboard, keyboardArgs) =>
                {
                    // Tjek om toggled er ændret
                    if (keyboardArgs.PropertyName == "KeyboardToggled")
                        NotifyCustom("KeyboardToggleText");
                };

                // Notify at parent er ændret
                NotifyCustom("Keyboard");
                NotifyCustom("KeyboardToggleText");

                UpdateInfo();
            };

            TimeCounter.StopTime();
        }

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
            get { return GetCommand<object>(x => { Trip trip = new Trip(); }); }
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
            get { return _infoPage ?? (_infoPage = new BeginTripsAdditionalInfo()); }
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

        private void UpdateInfo()
        {
            Info.SelectedBoat = new ObservableCollection<Boat> {SelectedBoat};
            Info.SelectedMembers = SelectedMembers;

            GetInfoContainerViewModel.ChangeInfo(InfoPage, Info);
        }

        #region Filter

        private void ResetFilter()
        {
            Boats = new ObservableCollection<Boat>(_boats);
            foreach (MemberViewModel member in Members)
                member.Visible = true;
        }

        private void UpdateFilter(FilterEventArgs args)
        {
            // Nulstil filter
            ResetFilter();

            // Tjek om en af filtertyperne er aktive
            if (!args.Filters.Any() && string.IsNullOrEmpty(args.SearchText))
                return;

            // Tjek filter
            if (args.Filters.Any())
            {

            }

            // Tjek søgning
            if (!string.IsNullOrEmpty(args.SearchText))
            {
                Boats = from boat in Boats
                    where boat.FilterBoat(args.SearchText)
                    select boat;

                foreach (
                    var member in Members.Where(member => !member.Member.FilterMembers(args.SearchText)))
                    member.Visible = false;
            }
        }

        #endregion

        #region sort

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