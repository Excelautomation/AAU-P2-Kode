using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using ARK.Model;
using ARK.Model.DB;
using ARK.View.Protokolsystem.Confirmations;
using ARK.View.Protokolsystem.Pages;
using ARK.ViewModel.Protokolsystem.Confirmations;

namespace ARK.ViewModel.Protokolsystem.Pages
{
    internal class CreateDamageFormViewModel : ProtokolsystemContentViewModelBase
    {
        // Fields
        #region Fields

        private List<DamageForm> _activeDamageForms;

        private List<Boat> _boats;

        private List<DamageType> _damageTypes;

        private string _description;

        private bool _isFunctional;

        private List<Member> _members;

        private Boat _selectedBoat;

        private DamageForm _selectedDamageForm;

        private DamageType _selectedDamageType;

        private Member _selectedMember;

        #endregion

        // constructor
        #region Constructors and Destructors

        public CreateDamageFormViewModel()
        {
            DbArkContext db = DbArkContext.GetDbContext();

            this.ParentAttached += (sender, e) =>
                {
                    this.Members = db.Member.OrderBy(x => x.FirstName).ToList();
                    this.Boats = db.Boat.OrderBy(x => x.Name).ToList();
                    this.DamageTypes = db.DamageType.ToList();
                    this.ActiveDamageForms = db.DamageForm.Where(d => d.Closed == false).ToList();
                };
        }

        #endregion

        #region Public Properties

        public List<DamageForm> ActiveDamageForms
        {
            get
            {
                return this._activeDamageForms;
            }

            set
            {
                this._activeDamageForms = value;
                this.Notify();
            }
        }

        public bool AllFieldsFilled
        {
            get
            {
                return this.SelectedBoat != null && this.SelectedMember != null && this.SelectedDamageType != null;
            }
        }

        public ICommand BoatSelectionChanged
        {
            get
            {
                return this.GetCommand(b => { this.SelectedBoat = (Boat)b; });
            }
        }

        // Properties
        // injury reporter
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

        public ICommand CreateDamageForm
        {
            get
            {
                return
                    this.GetCommand(
                        () => this.ProtocolSystem.NavigateToPage(() => new CreateDamageForm(), "OPRET NY SKADE"));
            }
        }

        public ICommand DamageTypeSelected
        {
            get
            {
                return this.GetCommand(d => { this.SelectedDamageType = (DamageType)d; });
            }
        }

        public List<DamageType> DamageTypes
        {
            get
            {
                return this._damageTypes;
            }

            set
            {
                this._damageTypes = value;
                this.Notify();
            }
        }

        public string Description
        {
            get
            {
                return this._description;
            }

            set
            {
                this._description = value;
                this.NotifyCustom("AllFieldsFilled");
            }
        }

        public bool IsFunctional
        {
            get
            {
                return this._isFunctional;
            }

            set
            {
                this._isFunctional = value;
                this.NotifyCustom("AllFieldsFilled");
            }
        }

        public ICommand MemberSelectionChanged
        {
            get
            {
                return this.GetCommand(e => { this.SelectedMember = (Member)e; });
            }
        }

        public List<Member> Members
        {
            get
            {
                return this._members;
            }

            set
            {
                this._members = value;
                this.Notify();
            }
        }

        // injured boat
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
            }
        }

        public DamageForm SelectedDamageForm
        {
            get
            {
                return this._selectedDamageForm;
            }

            set
            {
                this._selectedDamageForm = value;
                this.Notify();
                this.NotifyCustom("AllFieldsFilled");
            }
        }

        public DamageType SelectedDamageType
        {
            get
            {
                return this._selectedDamageType;
            }

            set
            {
                this._selectedDamageType = value;
                this.Notify();
                this.NotifyCustom("AllFieldsFilled");
            }
        }

        public Member SelectedMember
        {
            get
            {
                return this._selectedMember;
            }

            set
            {
                this._selectedMember = value;
                this.Notify();
            }
        }

        public ICommand SubmitForm
        {
            get
            {
                return this.GetCommand(
                    () =>
                        {
                            // Fjern evt tjek her i VM og lav button inactive ind til betingelser er ok.
                            if (this.SelectedBoat != null && this.SelectedMember != null
                                && this.SelectedDamageType != null)
                            {
                                var damageForm = new DamageForm();
                                damageForm.RegisteringMember = this.SelectedMember; // Member
                                damageForm.Boat = this.SelectedBoat; // Boat

                                // set damagetype
                                damageForm.Type = this.SelectedDamageType.Type;

                                // set additional description
                                damageForm.Description = this.Description;
                                damageForm.Functional = this.IsFunctional;

                                DbArkContext.GetDbContext().DamageForm.Add(damageForm);
                                DbArkContext.GetDbContext().SaveChanges();

                                this.ProtocolSystem.StatisticsDistance.Execute(null);
                            }
                        });
            }
        }

        public ICommand ViewDamageForm
        {
            get
            {
                return
                    this.GetCommand(
                        () => this.ProtocolSystem.NavigateToPage(() => new ViewDamageForm(), "SKADEBLANKETTER"));
            }
        }

        #endregion
    }
}