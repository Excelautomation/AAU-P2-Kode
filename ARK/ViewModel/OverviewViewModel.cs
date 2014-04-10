using System.Collections.Generic;
using ARK.Model;
using ARK.Model.Search;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;

namespace ARK.ViewModel
{
    public class OverviewViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<DamageForm> _skadesblanketter = new ObservableCollection<DamageForm>();
        private List<DamageForm> _skadesblanketterSource = new List<DamageForm>();
        public ObservableCollection<DamageForm> Skadesblanketter { get { return _skadesblanketter; } set { _skadesblanketter = value; Notify("Skadesblanketter"); } }

        public ObservableCollection<IFilter<OverviewViewModel, CheckBox>> SkadesblanketterFilters
        {
            get
            {
                return new ObservableCollection<IFilter<OverviewViewModel, CheckBox>>
                {
                    new CheckboxFilter<OverviewViewModel>(new CheckBox { Content = "Langtur" }, UpdateFilter),
                    new CheckboxFilter<OverviewViewModel>(new CheckBox { Content = "Skader" }, UpdateFilter),
                    new CheckboxFilter<OverviewViewModel>(new CheckBox { Content = "Både ude" }, UpdateFilter)
                };
            }
        }

        public OverviewViewModel()
        {
            Skadesblanketter.Add(new DamageForm { ReportedBy = "Martin er noob" });
            Skadesblanketter.Add(new DamageForm { ReportedBy = "Martin er mere noob" });
        }

        private void UpdateFilter()
        {            
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
