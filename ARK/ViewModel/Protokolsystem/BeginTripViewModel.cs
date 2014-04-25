using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using System.Collections.ObjectModel;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using ARK.Protokolsystem.Pages;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Protokolsystem
{
    public class BeginTripViewModel : KeyboardContentViewModelBase
    {
        private Boat _selectedBoat;
        private List<Boat> _boats = new List<Boat>();
        private List<Member> _members = new List<Member>();
        private bool _enableMembers;
        private FrameworkElement _infoPage;

        public BeginTripViewModel()
        {
            TimeCounter.StartTimer();

            // Load data
            using (DbArkContext db = new DbArkContext())
            {
                Boats = new List<Boat>(db.Boat).Where(x => x.Usable).OrderBy(x => x.NumberofSeats).ToList();
                Members = new List<Member>(db.Member).Select(x =>
                    {
                        x.FirstName = x.FirstName.Trim();
                        return x;
                    }).OrderBy(x => x.FirstName).ToList();
            }

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
            };

            TimeCounter.StopTime("BeginTripViewModel constructor");
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

        public List<Boat> Boats
        {
            get { return _boats; }
            set
            {
                _boats = value;
                Notify();
            }
        }

        public List<Member> Members
        {
            get { return _members; }
            set
            {
                _members = value;
                Notify();
            }
        }

        public ICommand BoatSelected
        {
            get
            {
                return GetCommand<Boat>(e =>
                {
                    EnableMembers = true;
                    Boat = e;
                    UpdateInfo();
                });
            }
        }

        public ICommand MemberSelected
        {
            get
            {
                return GetCommand<IList>(e =>
                {
                    UpdateInfo();
                });
            }
        }

        public Boat Boat
        {
            get { return _selectedBoat; }
            set
            {
                _selectedBoat = value;
                Notify();
            }
        }

        public ObservableCollection<Member> SelectedMembers { get; set; }

        public FrameworkElement InfoPage
        {
            get { return _infoPage ?? (_infoPage = new BeginTripsAdditionalInfo()); }
        }

        public BeginTripAdditionalInfoViewModel Info
        {
            get { return InfoPage.DataContext as BeginTripAdditionalInfoViewModel; }
        }

        public void UpdateInfo()
        {
            Info.SelectedBoat = new ObservableCollection<Boat> {Boat};
            Info.SelectedMembers = new ObservableCollection<Member>(Members);

            GetInfoContainerViewModel.ChangeInfo(InfoPage, Info);
        }

        public string KeyboardToggleText
        {
            get { return Keyboard.KeyboardToggled ? "SKJUL\nTASTATUR" : "VIS\nTASTATUR"; }
        }
    }
}
