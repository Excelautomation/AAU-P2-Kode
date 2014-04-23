using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using ARK.Administrationssystem.Pages;
using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;

namespace ARK.ViewModel
{
    public class FormsViewModel : Base.ViewModel, IFilter
    {
        private List<DamageForm> _damageForms = new List<DamageForm>();
        private ObservableCollection<Control> _filters;
        private List<LongDistanceForm> _longTripForms = new List<LongDistanceForm>();
        private UserControl _page;

        public FormsViewModel()
        {
            using (DbArkContext db = new DbArkContext())
            {
                //DamageForms = new List<DamageForm>(db.DamageForm);
                //LongDistanceForms = new List<LongDistanceForm>(db.LongTripForm);
            }

            DamageForms.Add(new DamageForm { Boat = new Boat
            {
                Id = 100,
                Name = "Hajen",
                Usable = false,
                Active = false},
                Date = new DateTime(2014, 4, 14),
                RegisteringMember = new Member {FirstName = "Kurt", LastName = "Nørremark"},
                DamageDescription = new DamageDescription { Description = "SVAB!", Type = "Hul i skroget", Id = 1, NeededMaterials = "Træ, spyt og sæd"}
            });
            DamageForms.Add(new DamageForm { Boat = new Boat
            {
                Id = 200,
                Name = "Den Flyvende Hollænder",
                Usable = false,
                Active = false},
                Date = new DateTime(2014, 4, 14),
                RegisteringMember = new Member {FirstName = "Davy", LastName = "Jones"},
                DamageDescription = new DamageDescription { Description = "Den er fooked OP", Type = "Hul i skroget", Id = 1, NeededMaterials = "Træ, spyt og sæd" }
            });
            DamageForms.Add(new DamageForm
            {
                Boat = new Boat
                {
                    Id = 200,
                    Name = "A Motherfucking Boat",
                    Usable = false,
                    Active = false
                },
                Date = new DateTime(2014, 4, 14),
                RegisteringMember = new Member {FirstName = "Samuel", LastName = "L. Jackson"},
                DamageDescription = new DamageDescription { Description = "Den er fooked OP", Type = "Hul i skroget", Id = 1, NeededMaterials = "Træ, spyt og sæd" }
            });
            
            LongDistanceForms.Add(new LongDistanceForm { Departure = new DateTime(2014,4,14), Arrival = new DateTime(2024,4,14)});

            Page = new FormsDamage { DataContext = new FormsDamageViewModel { DamageForm = DamageForms[0] } };
        }

        public UserControl Page { get { return _page; } set { _page = value; Notify(); } }

        public ICommand SelectDamageFormCommand
        {
            get
            {
                return GetCommand<DamageForm>(e => { Page.DataContext = new FormsDamageViewModel { DamageForm = e }; });
            }
        }

        public ICommand SelectLongDistanceFormCommand
        {
            get
            {
                return GetCommand<LongDistanceForm>(e => {Page.DataContext = e;});
            }
        }

        public List<DamageForm> DamageForms
        {
            get { return _damageForms; }
            set { _damageForms = value; Notify(); }
        }

        public List<LongDistanceForm> LongDistanceForms
        {
            get { return _longTripForms; }
            set { _longTripForms = value; Notify(); }
        } 

        public ObservableCollection<Control> Filters
        {
            get
            {
                return _filters ?? (_filters = new ObservableCollection<Control>
                {
                    new CheckBox {Content = "Langtur"},
                    new CheckBox {Content = "Skader"},
                    new Separator {Height = 20},
                    new CheckBox {Content = "Afviste"},
                    new CheckBox {Content = "Godkendte"}
                });
            }
        }
    }
}