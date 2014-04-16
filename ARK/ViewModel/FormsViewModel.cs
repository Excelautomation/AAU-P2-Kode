using ARK.Model;
using ARK.Model.DB;
using ARK.Model.Search;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ARK.ViewModel.Base;
using System.Windows.Input;
using ARK.Administrationssystem.Pages;

namespace ARK.ViewModel
{
    public class FormsViewModel : Base.ViewModel, IFilter
    {
        private ObservableCollection<Control> _filters;

        private List<DamageForm> _DamageForms = new List<DamageForm>();
        private List<LongDistanceForm> _LongTripForms = new List<LongDistanceForm>();
        private UserControl _page;

        public UserControl Page { get { return _page; } set { _page = value; Notify("Page"); } }

        public ICommand SelectDamageFormCommand
        {
            get
            {
                return GetCommand<DamageForm>(e =>
                {
                    Page.DataContext = e;
                });
            }
        }

        public ICommand SelectLongDistanceFormCommand
        {
            get
            {
                return GetCommand<LongDistanceForm>(e =>
                {
                    Page.DataContext = e;
                });
            }
        }

        public FormsViewModel()
        {
            using (var db = new DbArkContext())
            {
                //DamageForms = new List<DamageForm>(db.DamageForm);
                LongDistanceForms = new List<LongDistanceForm>(db.LongTripForm);
            }

            DamageForms.Add(new DamageForm { Boat = new Boat() {
                Id = 100,
                Name = "Hajen",
                Usable = false,
                Active = false},
                Date = new System.DateTime(2014, 4, 14),
                //ReportedBy = "John Doge Larsen",
                DamageDescription = new DamageDescription() { Description = "Den er FUBAR", Type = "Hul i skroget", Id = 1, NeededMaterials = "Træ, spyt og sæd"},
                //ReportedByNumber = 42
            });
            DamageForms.Add(new DamageForm { Boat = new Boat() {
                Id = 200,
                Name = "Den Flyvende Hollænder",
                Usable = false,
                Active = false},
                Date = new System.DateTime(2014, 4, 14),
                //ReportedBy = "Davy Jones",
                DamageDescription = new DamageDescription() { Description = "Den er FUBAR", Type = "Hul i skroget", Id = 1, NeededMaterials = "Træ, spyt og sæd" },
                //ReportedByNumber = 43
            });
            
            
            
            
            DamageForms.Add(new DamageForm { Boat = new Boat() { Name = "A Motherfucking Boat" }, Date = new System.DateTime(2014, 4, 14), /*ReportedBy = "Samuel L. Jackson"*/ });

            LongDistanceForms.Add(new LongDistanceForm { Departure = new System.DateTime(2014,4,14), Arrival = new System.DateTime(2024,4,14)});

            Page = new FormsLongTrip { DataContext = LongDistanceForms[0] };

        }
        
        

        public List<DamageForm> DamageForms
        {
            get { return _DamageForms; }
            set { _DamageForms = value; Notify("DamageForms"); }
        }

        public List<LongDistanceForm> LongDistanceForms
        {
            get { return _LongTripForms; }
            set { _LongTripForms = value; Notify("LongTripForms"); }
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