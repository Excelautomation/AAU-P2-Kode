using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARK.Model;
using System.Windows.Input;
using ARK.Model.DB;

namespace ARK.ViewModel.Protokolsystem
{
    class CreateInjuryViewModel : ProtokolsystemContentViewModelBase
    {
        // Fields
        private Member _selectedMember;
        private List<Member> _members;
        private Boat _selectedBoat;
        private List<Boat> _boats;
        private List<DamageType> _damageTypes;
        private DamageType _selectedDamageType;
        private List<DamageForm> _activeDamageForms;
        private DamageForm _selectedDamageForm;

        // constructor
        public CreateInjuryViewModel()
        {
            var db = DbArkContext.GetDbContext();
            _members = db.Member.Where(m => true).ToList();
            _boats = db.Boat.Where(b => true).ToList();
            _damageTypes = db.DamageType.Where(d => true).ToList();
            _activeDamageForms = db.DamageForm.Where(d => d.Closed == false).ToList();
        }

        // Properties
        // injury reporter
        public Member SelectedMember
        {
            get { return _selectedMember; }
            set 
            {
                _selectedMember = value; 
                Notify(); 
            }
        }

        public List<Member> Members
        {
            get { return _members; }
            set { _members = value; Notify(); }
        }

        // injured boat
        public Boat SelectedBoat
        {
            get { return _selectedBoat; }
            set { _selectedBoat = value; Notify(); }
        }

        public List<Boat> Boats
        {
            get { return _boats; }
            set { _boats = value; Notify(); }
        }

        public List<DamageType> DamageTypes
        {
            get { return _damageTypes; }
            set { _damageTypes = value; }
        }

        public DamageType SelectedDamageType
        {
            get { return _selectedDamageType; }
            set { _selectedDamageType = value; }
        }

        public string Description { get; set; }

        public bool IsFunctional { get; set; }

        public List<DamageForm> ActiveDamageForms
        {
            get { return _activeDamageForms; }
            set { _activeDamageForms = value; }
        }

        public DamageForm SelectedDamageForm
        {
            get { return _selectedDamageForm; }
            set { _selectedDamageForm = value; Notify(); }
        }

        public ICommand MemberSelectionChanged
        {
            get
            {
                return GetCommand<Member>(e =>
                {
                    SelectedMember = e;
                });
            }
        }

        public ICommand BoatSelectionChanged
        {
            get
            {
                return GetCommand<Boat>(b =>
                {
                    SelectedBoat = b;
                });
            }
        }

        public ICommand DamageTypeSelected
        {
            get
            {
                return GetCommand<DamageType>(d =>
                {
                    SelectedDamageType = d;
                });
            }
        }

        public ICommand AddDamageType
        {
            get
            {
                return GetCommand<object>(d =>
                {
                    // Fjern evt tjek her i VM og lav button inactive ind til betingelser er ok.
                    if (SelectedBoat != null && SelectedMember != null && SelectedDamageType != null)
                    {
                        var damageForm = new DamageForm();
                        damageForm.RegisteringMember = SelectedMember;  // Member
                        damageForm.Boat = SelectedBoat;                 // Boat
                        // set damagetype
                        damageForm.Type = SelectedDamageType.Type;

                        // set additional description
                        damageForm.Description = Description;
                        damageForm.Functional = IsFunctional;

                        var db = DbArkContext.GetDbContext();

                        db.DamageForm.Add(damageForm);
                    }
                });
            }
        }
    }
}
