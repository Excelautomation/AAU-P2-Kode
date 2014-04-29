using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using ARK.Extensions;
using ARK.Model;
using ARK.Model.DB;
using System.Collections.ObjectModel;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using ARK.Protokolsystem.Pages;
using ARK.Interfaces;
using ARK.ViewModel.Interfaces;
using ARK.ViewModel.Filter;

namespace ARK.ViewModel.Protokolsystem
{
    public class BeginTripViewModel : KeyboardContentViewModelBase, IDisposable
    {
        private IEnumerable<Member> _members;   // All members
        private IEnumerable<Boat> _boats;       // All boats
        private Boat _selectedBoat;

        private IEnumerable<Member> _membersFiltered;   // members to display
        private IEnumerable<Boat> _boatsFiltered;       // boats to display
        private ObservableCollection<Member> _selectedMembers;  // members in boat

        private bool _enableMembers;
        private FrameworkElement _infoPage;

        private DbArkContext db = new DbArkContext();

        public BeginTripViewModel()
        {
            TimeCounter.StartTimer();

            // Load data
                _boatsFiltered = new List<Boat>(db.Boat).Where(x => x.Usable).OrderBy(x => x.NumberofSeats).ToList();
                _membersFiltered = new List<Member>(db.Member).Select(x =>
                    {
                        x.FirstName = x.FirstName.Trim();
                        return x;
                    }).OrderBy(x => x.FirstName).ToList();

            _selectedMembers = new ObservableCollection<Member>();

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

        // properties
        public IEnumerable<Boat> Boats
        {
            get { return _boats; }
            set
            {
                _boats = value;
                Notify();
            }
        }

        public Boat SelectedBoat
        {
            get { return _selectedBoat; }
            set { _selectedBoat = value; Notify(); }
        }

        public IEnumerable<Member> Members
        {
            get { return _members; }
            set
            {
                _members = value;
                Notify();
            }
        }

        public ObservableCollection<Member> SelectedMembers
        {
            get
            {
                return _selectedMembers;
            }
        }

        public ICommand StartTripNow
        {
            get
            {
                return GetCommand<object>(x =>
                    {
                        Trip trip = new Trip()
                            {
                                
                            };
                    });
            }
        }

        public Boat Boat
        {
            get { return _selectedBoat; }
            set
            {
                _selectedBoat = value;
            }
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
                    this.SelectedMembers.Clear();
                    UpdateInfo();
                });
            }
        }

        public void UpdateInfo()
        {
            Info.SelectedBoat = new ObservableCollection<Boat> {SelectedBoat};
            Info.SelectedMembers = SelectedMembers;

            GetInfoContainerViewModel.ChangeInfo(InfoPage, Info);
        }

        #region Filter
        private void ResetFilter()
        {
            _boatsFiltered = new ObservableCollection<Boat>(Boats);
            _membersFiltered = new ObservableCollection<Member>(Members);
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
                _boatsFiltered = from boat in Boats
                    where boat.FilterBoat(args.SearchText)
                    select boat;
                _membersFiltered = from member in Members
                    where member.FilterMembers(args.SearchText)
                    select member;
            }
        }

        #endregion

        #region sort
        void SortBoats(Func<Boat, string> predicate)
        {
            Boats = Boats.OrderBy(predicate);
        }
        void SortMembers(Func<Member, string> predicate)
        {
            Members = Members.OrderBy(predicate);
        }
        #endregion

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
