using ARK.Model;
using ARK.Model.Search;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;

namespace ARK.ViewModel
{
    public class OverviewViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Skadesblanket> _skadesblanketter = new ObservableCollection<Skadesblanket>();

        public ObservableCollection<Skadesblanket> Skadesblanketter { get { return _skadesblanketter; } set { _skadesblanketter = value; Notify("Skadesblanketter"); } }

        public ObservableCollection<IFilter<OverviewViewModel, CheckBox>> SkadesblanketterFilters
        {
            get
            {
                return new ObservableCollection<IFilter<OverviewViewModel, CheckBox>>
                {
                    new CheckboxFilter<OverviewViewModel>(new CheckBox { Content = "Langtur" }, (viewmodel) => true),
                    new CheckboxFilter<OverviewViewModel>(new CheckBox { Content = "Skader" }, (viewmodel) => true),
                    new CheckboxFilter<OverviewViewModel>(new CheckBox { Content = "Både ude"}, (viewmodel) => true)
                };
            }
        }

        public OverviewViewModel()
        {
            Skadesblanketter.Add(new Skadesblanket { ReportedBy = "Martin er noob" });
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

        #endregion Property
    }
}
