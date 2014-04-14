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
        private List<LongTripForm> _LongTripForms = new List<LongTripForm>();
        private UserControl _page;

        public UserControl Page { get { return _page; } set { _page = value; Notify("Page"); } } 
        public ICommand SelectDamageFormCommand { get { return GetCommand<DamageForm>(e => {
            Page.DataContext = e;
        }); } }

        public FormsViewModel()
        {
            using (var db = new DbArkContext())
            {
                DamageForms = new List<DamageForm>(db.DamageForm);
                LongTripForms = new List<LongTripForm>(db.LongTripForm);
            }


            DamageForms.Add(new DamageForm { DamagedBoat = new Boat() { Name = "Hajen" }, Date = new System.DateTime(2014, 4, 14), ReportedBy = "John Doge Larsen"});
            DamageForms.Add(new DamageForm { DamagedBoat = new Boat() { Name = "Den Flyvende Hollænder" }, Date = new System.DateTime(2014, 4, 14), ReportedBy = "Davy Jones" });
            DamageForms.Add(new DamageForm { DamagedBoat = new Boat() { Name = "A Motherfucking Boat" }, Date = new System.DateTime(2014, 4, 14), ReportedBy = "Samuel L. Jackson" });

            //LongTripForms.Add(new LongTripForm { Departure = System.DateTime.Now, Arrival = System.DateTime.Now.AddYears(10)});

            Page = new FormsDamage { DataContext = DamageForms[0] };
        }
        
        

        public List<DamageForm> DamageForms
        {
            get { return _DamageForms; }
            set { _DamageForms = value; Notify("DamageForms"); }
        }

        public List<LongTripForm> LongTripForms
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