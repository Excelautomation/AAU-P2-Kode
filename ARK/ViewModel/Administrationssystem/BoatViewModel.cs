using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using ARK.ViewModel.Base;
using ARK.Model;
using ARK.Model.DB;

namespace ARK.ViewModel.Administrationssystem
{
    public class BoatViewModel : ViewModelBase, IFilter
    {
        private ObservableCollection<Control> _filters;
        private Boat _Boat;
        private List<Boat> _Boats = new List<Boat>();


        public BoatViewModel()
        {
            // Load data
            using (DbArkContext db = new DbArkContext())
            {
                Boats = new List<Boat>(db.Boat).Where(x => x.Active).OrderBy(x => x.NumberofSeats).ToList();
            }

            if (Boats.Count != 0)
            {
                Boat = Boats[0];
            }
        }





        public List<Boat> Boats
        {
            get { return _Boats; }
            set
            {
                _Boats = value;
                Notify();
            }
        }

        public Boat Boat
        {
            get { return _Boat; }
            set
            {
                _Boat = value;
                Notify();
                //NotifyProp("BoatContent");
            }
        }

        public ICommand SelectedChange
        {
            get
            {
                return GetCommand<Boat>(e =>
                {
                    Boat = e;
                });
            }
        }

        public ObservableCollection<Control> Filters
        {
            get
            {
                return _filters ?? (_filters = new ObservableCollection<Control>
                {
                    new CheckBox { Content = "Både ude" },
                    new CheckBox { Content = "Både hjemme" },
                    new Separator { Height = 20 },
                    new CheckBox { Content = "Både under reparation" },
                    new CheckBox { Content = "Beskadigede både" },
                    new CheckBox { Content = "Inaktive både" },
                    new CheckBox { Content = "Funktionelle både" }
                });
            }
        }
    }
}