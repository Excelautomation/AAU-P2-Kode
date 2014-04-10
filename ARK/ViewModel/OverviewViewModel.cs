using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ARK.Model;
using ARK.Model.Search;

namespace ARK.ViewModel
{
    public class OverviewViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Skadesblanket> _skadesblanketter = new ObservableCollection<Skadesblanket>();
        public ObservableCollection<Skadesblanket> Skadesblanketter { get { return _skadesblanketter; } set { _skadesblanketter = value; Notify("Skadesblanketter"); } }

        public ObservableCollection<IFilter<Skadesblanket, CheckBox>> SkadesblanketterFilters
        {
            get
            {
                
            }
        }

        public OverviewViewModel()
        {
            Skadesblanketter.Add(new Skadesblanket() { ReportedBy = "Martin er noob" });
            Skadesblanketter.Add(new Skadesblanket { ReportedBy = "Martin er mere noob" });
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
        #endregion
    }
}
