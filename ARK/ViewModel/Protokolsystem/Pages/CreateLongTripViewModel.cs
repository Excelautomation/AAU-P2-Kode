using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARK.Model;
using System.Collections.ObjectModel;
using ARK.Model.DB;
using System.Data.Entity;
using System.Windows.Input;

namespace ARK.ViewModel.Protokolsystem
{
    public class CreateLongTripViewModel : ProtokolsystemContentViewModelBase
    {
        // Fields
        private List<Member> _members = new List<Member>();
        private readonly ObservableCollection<MemberViewModel> _selectedMembers = new ObservableCollection<MemberViewModel>(); // Members in boat
        private List<MemberViewModel> _membersFiltered;
        private List<LongTripForm> _longTripForms;
        private Boat _selectedBoat;

        // Constructor
        public CreateLongTripViewModel()
        {
            var db = DbArkContext.GetDbContext();

            // Set up variables to load of data
            Task<List<Member>> _membersAsync = null;

            // Async start load of data
            _membersAsync = db.Member.OrderBy(x => x.FirstName).ToListAsync();
            _membersFiltered = _membersAsync.Result.Select(member => new MemberViewModel(member)).ToList();
            
            // get long trip forms
            _longTripForms = db.LongTripForm.OrderBy(x => x.FormCreated).Where(x => true).ToList();
            Boats = db.Boat.Where(x => true).ToList();
        }

        // Properties
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public List<Boat> Boats { get; set; }
        public Boat SelectedBoat
        {
            get { return _selectedBoat; }
            set { _selectedBoat = value; Notify(); }
        }
        public string TourDescription { get; set; }
        public string DistancesPerDay { get; set; }
        public string CampSites { get; set; }

        public List<MemberViewModel> MembersFiltered
        {
            get { return _membersFiltered; }
            set { _membersFiltered = value; }
        }

        public ObservableCollection<MemberViewModel> SelectedMembers
        {
            get { return _selectedMembers; }
        }

        public List<LongTripForm> LongTripForms
        {
            get { return _longTripForms; }
            set { _longTripForms = value; }
        }

        public ICommand AddLongTrip
        {
            get
            {
                return GetCommand<object>(d =>
                {
                    var db = new DbArkContext();
                    var longTripForm = new LongTripForm();
                    longTripForm.FormCreated = DateTime.Now;
                    longTripForm.PlannedStartDate = PlannedStartDate ?? DateTime.MinValue;
                    longTripForm.PlannedEndDate = PlannedEndDate ?? DateTime.MinValue;
                    longTripForm.Boat = SelectedBoat;
                    longTripForm.TourDescription = TourDescription;
                    longTripForm.DistancesPerDay = DistancesPerDay;
                    longTripForm.CampSites = CampSites;
                    longTripForm.Members = SelectedMembers.Select(mvm => mvm.Member).ToList();
                    longTripForm.Status = LongTripForm.BoatStatus.Awaiting;

                    db.LongTripForm.Add(longTripForm);
                });
            }
        }

        public ICommand AddBlanc
        {
            get
            {
                return GetCommand<object>(d =>
                {
                    // Add a empty seat to boat
                });
            }
        }
        
        public ICommand AddGuest
        {
            get
            {
                return GetCommand<object>(d =>
                {
                    // Add a guest to boat
                });
            }
        }
    }
}
