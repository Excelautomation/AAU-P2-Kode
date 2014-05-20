using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using ARK.Model;
using ARK.Model.DB;
using ARK.View.Protokolsystem.Additional;
using ARK.View.Protokolsystem.Confirmations;
using ARK.View.Protokolsystem.Pages;
using ARK.ViewModel.Protokolsystem.Additional;
using ARK.ViewModel.Protokolsystem.Confirmations;
using ARK.ViewModel.Protokolsystem.Data;

namespace ARK.ViewModel.Protokolsystem.Pages
{
    public class CreateLongTripViewModel : ProtokolsystemContentViewModelBase
    {
        // Fields
        private readonly ObservableCollection<MemberViewModel> _selectedMembers =
            new ObservableCollection<MemberViewModel>(); // Members in boat

        private List<Boat> _boats;

        private string _campSites;

        private string _distancesPerDay;

        private FrameworkElement _infoPage;

        private List<LongTripForm> _longTripForms;

        private List<Member> _members = new List<Member>();

        private List<MemberViewModel> _membersFiltered;

        private DateTime? _plannedEndDate;

        private DateTime? _plannedStartDate;

        private Boat _selectedBoat;

        private string _tourDescription;

        // Constructor
        public CreateLongTripViewModel()
        {
            DbArkContext db = DbArkContext.GetDbContext();

            // Load of data
            ParentAttached += (sender, e) =>
                {
                    _members = db.Member.OrderBy(x => x.FirstName).ToList();
                    MembersFiltered = _members.Select(member => new MemberViewModel(member)).ToList();

                    // get long trip forms
                    LongTripForms = db.LongTripForm.OrderBy(x => x.FormCreated).Where(x => true).ToList();
                    Boats = db.Boat.Where(x => x.Active).ToList();

                    // Set info
                    UpdateInfo();
                };
        }

        public ICommand AddBlank
        {
            get
            {
                return
                    GetCommand(
                        () => SelectedMembers.Add(new MemberViewModel(new Member { Id = -1, FirstName = "Blank" })));
            }
        }

        public ICommand AddGuest
        {
            get
            {
                return
                    GetCommand(
                        () => SelectedMembers.Add(new MemberViewModel(new Member { Id = -2, FirstName = "Gæst" })));
            }
        }

        // Properties
        public List<Boat> Boats
        {
            get
            {
                return _boats;
            }

            set
            {
                _boats = value;
                Notify();
            }
        }

        public string CampSites
        {
            get
            {
                return _campSites;
            }

            set
            {
                _campSites = value;
                NotifyCustom("AllDataFilled");
            }
        }

        public string DistancesPerDay
        {
            get
            {
                return _distancesPerDay;
            }

            set
            {
                _distancesPerDay = value;
                NotifyCustom("AllDataFilled");
            }
        }

        public List<LongTripForm> LongTripForms
        {
            get
            {
                return _longTripForms;
            }

            set
            {
                _longTripForms = value;
                Notify();
            }
        }

        public List<MemberViewModel> MembersFiltered
        {
            get
            {
                return _membersFiltered;
            }

            set
            {
                _membersFiltered = value;

                Notify();
            }
        }

        public DateTime? PlannedEndDate
        {
            get
            {
                return _plannedEndDate;
            }

            set
            {
                _plannedEndDate = value;
                NotifyCustom("AllDataFilled");
            }
        }

        public DateTime? PlannedStartDate
        {
            get
            {
                return _plannedStartDate;
            }

            set
            {
                _plannedStartDate = value;
                NotifyCustom("AllDataFilled");
            }
        }

        public Boat SelectedBoat
        {
            get
            {
                return _selectedBoat;
            }

            set
            {
                _selectedBoat = value;
                Notify();
                UpdateInfo();
            }
        }

        public ObservableCollection<MemberViewModel> SelectedMembers
        {
            get
            {
                return _selectedMembers;
            }
        }

        public ICommand ShowConfirmationDialog
        {
            get
            {
                return GetCommand(
                    () =>
                        {
                            LongTripForm longTripForm = null;
                            longTripForm = new LongTripForm
                                               {
                                                   FormCreated = DateTime.Now, 
                                                   PlannedStartDate = PlannedStartDate ?? DateTime.MinValue, 
                                                   PlannedEndDate = PlannedEndDate ?? DateTime.MinValue, 
                                                   Boat = SelectedBoat, 
                                                   TourDescription = TourDescription, 
                                                   DistancesPerDay = DistancesPerDay, 
                                                   CampSites = CampSites, 
                                                   Members =
                                                       SelectedMembers.Select(mvm => mvm.Member).ToList(), 
                                                   Status = LongTripForm.BoatStatus.Awaiting, 
                                                   ResponsibleMember =
                                                       Info.ResponsibleMember != null
                                                           ? Info.ResponsibleMember.Member
                                                           : null
                                               };
                            var ConfirmView = new CreateLongTripConfirm();
                            var ConfirmViewModel = (CreateLongTripConfirmViewModel)ConfirmView.DataContext;

                            ConfirmViewModel.LongTrip = longTripForm;

                            ProtocolSystem.ShowDialog(ConfirmView);
                        });
            }
        }

        public string TourDescription
        {
            get
            {
                return _tourDescription;
            }

            set
            {
                _tourDescription = value;
                NotifyCustom("AllDataFilled");
            }
        }

        public ICommand ViewLongTripForm
        {
            get
            {
                return
                    GetCommand(() => ProtocolSystem.NavigateToPage(() => new ViewLongTripForm(), "LANGTURSBLANKETTER"));
            }
        }

        private CreateLongTripFormAdditionalInfoViewModel Info
        {
            get
            {
                return InfoPage.DataContext as CreateLongTripFormAdditionalInfoViewModel;
            }
        }

        private FrameworkElement InfoPage
        {
            get
            {
                return _infoPage ?? (_infoPage = new CreateLongTripFormAdditionalInfo());
            }
        }

        private void UpdateInfo()
        {
            Info.SelectedBoat = SelectedBoat;
            Info.SelectedMembers = SelectedMembers;

            ProtocolSystem.ChangeInfo(InfoPage, Info);
        }
    }
}