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

namespace ARK.ViewModel
{
    public class FormsViewModel : INotifyPropertyChanged, IFilter
    {
        private ObservableCollection<Control> _filters;

        private List<DamageForm> _DamageForms = new List<DamageForm>();


        public FormsViewModel()
        {
            using (var db = new DbArkContext())
            {
                DamageForms = new List<DamageForm>(db.DamageForm);
            }


            DamageForms.Add(new DamageForm { DamagedBoat = new Boat() { Name = "Hajen" }, Date = new System.DateTime(2014, 4, 14), ReportedBy = "John Doge Larsen"});
            DamageForms.Add(new DamageForm { DamagedBoat = new Boat() { Name = "Den Flyvende Hollænder" }, Date = new System.DateTime(2014, 4, 14), ReportedBy = "Davy Jones" });
            DamageForms.Add(new DamageForm { DamagedBoat = new Boat() { Name = "A Motherfucking Boat" }, Date = new System.DateTime(2014, 4, 14), ReportedBy = "Samuel L. Jackson" });
            
        }
        
        

        public List<DamageForm> DamageForms
        {
            get { return _DamageForms; }
            set { _DamageForms = value; Notify("DamageForms"); }
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
        #region Property

        public event PropertyChangedEventHandler PropertyChanged;

        protected void Notify(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion Property
    }
}