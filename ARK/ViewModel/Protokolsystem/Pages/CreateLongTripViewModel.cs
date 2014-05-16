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
        #region Fields

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

        #endregion

        // Constructor
        #region Constructors and Destructors

        public CreateLongTripViewModel()
        {
            DbArkContext db = DbArkContext.GetDbContext();

            // Load of data
            this.ParentAttached += (sender, e) =>
                {
                    this._members = db.Member.OrderBy(x => x.FirstName).ToList();
                    this.MembersFiltered = this._members.Select(member => new MemberViewModel(member)).ToList();

                    // get long trip forms
                    this.LongTripForms = db.LongTripForm.OrderBy(x => x.FormCreated).Where(x => true).ToList();
                    this.Boats = db.Boat.Where(x => x.Active).ToList();

                    // Set info
                    this.UpdateInfo();
                };
        }

        #endregion

        #region Public Properties

        public ICommand AddBlank
        {
            get
            {
                return
                    this.GetCommand(
                        () => this.SelectedMembers.Add(new MemberViewModel(new Member { Id = -1, FirstName = "Blank" })));
            }
        }

        public ICommand AddGuest
        {
            get
            {
                return
                    this.GetCommand(
                        () => this.SelectedMembers.Add(new MemberViewModel(new Member { Id = -2, FirstName = "Gæst" })));
            }
        }

        // Properties
        public List<Boat> Boats
        {
            get
            {
                return this._boats;
            }

            set
            {
                this._boats = value;
                this.Notify();
            }
        }

        public string CampSites
        {
            get
            {
                return this._campSites;
            }

            set
            {
                this._campSites = value;
                this.NotifyCustom("AllDataFilled");
            }
        }

        public string DistancesPerDay
        {
            get
            {
                return this._distancesPerDay;
            }

            set
            {
                this._distancesPerDay = value;
                this.NotifyCustom("AllDataFilled");
            }
        }

        public List<LongTripForm> LongTripForms
        {
            get
            {
                return this._longTripForms;
            }

            set
            {
                this._longTripForms = value;
                this.Notify();
            }
        }

        public List<MemberViewModel> MembersFiltered
        {
            get
            {
                return this._membersFiltered;
            }

            set
            {
                this._membersFiltered = value;

                this.Notify();
            }
        }

        public DateTime? PlannedEndDate
        {
            get
            {
                return this._plannedEndDate;
            }

            set
            {
                this._plannedEndDate = value;
                this.NotifyCustom("AllDataFilled");
            }
        }

        public DateTime? PlannedStartDate
        {
            get
            {
                return this._plannedStartDate;
            }

            set
            {
                this._plannedStartDate = value;
                this.NotifyCustom("AllDataFilled");
            }
        }

        public Boat SelectedBoat
        {
            get
            {
                return this._selectedBoat;
            }

            set
            {
                this._selectedBoat = value;
                this.Notify();
                this.UpdateInfo();
            }
        }

        public ObservableCollection<MemberViewModel> SelectedMembers
        {
            get
            {
                return this._selectedMembers;
            }
        }

        public ICommand ShowConfirmationDialog
        {
            get
            {
                return this.GetCommand(
                    () =>
                        {
                            LongTripForm longTripForm = null;
                            longTripForm = new LongTripForm
                                               {
                                                   FormCreated = DateTime.Now, 
                                                   PlannedStartDate =
                                                       this.PlannedStartDate ?? DateTime.MinValue, 
                                                   PlannedEndDate = this.PlannedEndDate ?? DateTime.MinValue, 
                                                   Boat = this.SelectedBoat, 
                                                   TourDescription = this.TourDescription, 
                                                   DistancesPerDay = this.DistancesPerDay, 
                                                   CampSites = this.CampSites, 
                                                   Members =
                                                       this.SelectedMembers.Select(mvm => mvm.Member)
                                                       .ToList(), 
                                                   Status = LongTripForm.BoatStatus.Awaiting, 
                                                   ResponsibleMember =
                                                       this.Info.ResponsibleMember != null
                                                           ? this.Info.ResponsibleMember.Member
                                                           : null
                                               };
                            var ConfirmView = new CreateLongTripConfirm();
                            var ConfirmViewModel = (CreateLongTripConfirmViewModel)ConfirmView.DataContext;

                            ConfirmViewModel.LongTrip = longTripForm;

                            this.ProtocolSystem.ShowDialog(ConfirmView);
                        });
            }
        }

        public string TourDescription
        {
            get
            {
                return this._tourDescription;
            }

            set
            {
                this._tourDescription = value;
                this.NotifyCustom("AllDataFilled");
            }
        }

        public ICommand ViewLongTripForm
        {
            get
            {
                return
                    this.GetCommand(
                        () => this.ProtocolSystem.NavigateToPage(() => new ViewLongTripForm(), "LANGTURSBLANKETTER"));
            }
        }

        #endregion

        #region Properties

        private CreateLongTripFormAdditionalInfoViewModel Info
        {
            get
            {
                return this.InfoPage.DataContext as CreateLongTripFormAdditionalInfoViewModel;
            }
        }

        private FrameworkElement InfoPage
        {
            get
            {
                return this._infoPage ?? (this._infoPage = new CreateLongTripFormAdditionalInfo());
            }
        }

        #endregion

        #region Methods

        private void UpdateInfo()
        {
            this.Info.SelectedBoat = this.SelectedBoat;
            this.Info.SelectedMembers = this.SelectedMembers;

            this.ProtocolSystem.ChangeInfo(this.InfoPage, this.Info);
        }

        #endregion
    }
}