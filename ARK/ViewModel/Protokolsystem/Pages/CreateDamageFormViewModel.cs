using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using ARK.View.Protokolsystem.Pages;
using System;

namespace ARK.ViewModel.Protokolsystem.Pages
{
    internal class CreateDamageFormViewModel : ProtokolsystemContentViewModelBase
    {
        // Fields
        private List<DamageForm> _activeDamageForms;
        private List<Boat> _boats;
        private List<DamageType> _damageTypes;
        private List<Member> _members;
        private Boat _selectedBoat;
        private DamageForm _selectedDamageForm;
        private DamageType _selectedDamageType;
        private Member _selectedMember;

        // constructor
        public CreateDamageFormViewModel()
        {
            DbArkContext db = DbArkContext.GetDbContext();

            ParentAttached += (sender, e) =>
            {
                Members = db.Member.ToList();
                Boats = db.Boat.ToList();
                DamageTypes = db.DamageType.ToList();
                ActiveDamageForms = db.DamageForm.Where(d => d.Closed == false).ToList();
            };
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
            set
            {
                _members = value;
                Notify();
            }
        }

        // injured boat
        public Boat SelectedBoat
        {
            get { return _selectedBoat; }
            set
            {
                _selectedBoat = value;
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

        public List<DamageType> DamageTypes
        {
            get { return _damageTypes; }
            set
            {
                _damageTypes = value;
                Notify();
            }
        }

        public DamageType SelectedDamageType
        {
            get { return _selectedDamageType; }
            set
            {
                _selectedDamageType = value;
                Notify();
            }
        }

        public string Description { get; set; }

        public bool IsFunctional { get; set; }

        public List<DamageForm> ActiveDamageForms
        {
            get { return _activeDamageForms; }
            set
            {
                _activeDamageForms = value;
                Notify();
            }
        }

        public DamageForm SelectedDamageForm
        {
            get { return _selectedDamageForm; }
            set
            {
                _selectedDamageForm = value;
                Notify();
            }
        }

        public ICommand CreateDamageForm
        {
            get
            {
                return
                    GetCommand<object>(
                        a => ProtocolSystem.NavigateToPage(() => new CreateDamageForm(), "OPRET NY SKADE"));
            }
        }

        public ICommand ViewDamageForm
        {
            get
            {
                return
                    GetCommand<object>(
                        a => ProtocolSystem.NavigateToPage(() => new ViewDamageForm(), "AKTIVE SKADES BLANKETTER"));
            }
        }

        public ICommand MemberSelectionChanged
        {
            get { return GetCommand<Member>(e => { SelectedMember = e; }); }
        }

        public ICommand BoatSelectionChanged
        {
            get { return GetCommand<Boat>(b => { SelectedBoat = b; }); }
        }

        public ICommand DamageTypeSelected
        {
            get { return GetCommand<DamageType>(d => { SelectedDamageType = d; }); }
        }

        public ICommand AddDamageForm
        {
            get
            {
                return GetCommand<object>(d =>
                {
                    // Fjern evt tjek her i VM og lav button inactive ind til betingelser er ok.
                    if (SelectedBoat != null && SelectedMember != null && SelectedDamageType != null)
                    {
                        var damageForm = new DamageForm();
                        damageForm.RegisteringMember = SelectedMember; // Member
                        damageForm.Boat = SelectedBoat; // Boat
                        // set damagetype
                        damageForm.Type = SelectedDamageType.Type;

                        // set additional description
                        damageForm.Description = Description;
                        damageForm.Functional = IsFunctional;

                        DbArkContext db = DbArkContext.GetDbContext();

                        db.DamageForm.Add(damageForm);
                        db.SaveChanges();

                        throw new NotImplementedException("Retur til DistanceStatistics");
                    }
                });
            }
        }
    }
}