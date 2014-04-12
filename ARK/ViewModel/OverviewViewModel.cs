using ARK.Model;
using ARK.Model.Search;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ARK.ViewModel
{
    internal class OverviewViewModel : INotifyPropertyChanged, IFilter
    {
        private readonly List<CheckboxFilter> _checkboxFilters = new List<CheckboxFilter>();

        private ObservableCollection<Control> _filters;

        private Visibility _showBoatsOut;

        private Visibility _showLangtur;

        private Visibility _showSkader;

        private ObservableCollection<DamageForm> _skadesblanketter = new ObservableCollection<DamageForm>();

        public OverviewViewModel()
        {
            // Load data
            Skadesblanketter.Add(new DamageForm { ReportedBy = "Martin er noob" });
            Skadesblanketter.Add(new DamageForm { ReportedBy = "Martin er mere noob" });

            // Add Checkbox filter
            var checkboxfilters = from c in Filters
                                  where c is CheckBox
                                  select new CheckboxFilter((CheckBox)c, UpdateFilter);
            _checkboxFilters.AddRange(checkboxfilters);
        }

        public ObservableCollection<Control> Filters
        {
            get
            {
                return _filters ?? (
                    _filters = new ObservableCollection<Control>()
                    {
                        new CheckBox {Content = "Skader"},
                        new CheckBox {Content = "Langtur"},
                        new CheckBox {Content = "Både ude"}
                    });
            }
        }
        public Visibility ShowBoatsOut
        {
            get { return _showBoatsOut; }
            set { _showBoatsOut = value; Notify("ShowBoatsOut"); }
        }

        public Visibility ShowLangtur
        {
            get { return _showLangtur; }
            set { _showLangtur = value; Notify("ShowLangtur"); }
        }

        public Visibility ShowSkader
        {
            get { return _showSkader; }
            set { _showSkader = value; Notify("ShowSkader"); }
        }

        public ObservableCollection<DamageForm> Skadesblanketter { get { return _skadesblanketter; } set { _skadesblanketter = value; Notify("Skadesblanketter"); } }
        private void ResetFilter()
        {
            ShowBoatsOut = Visibility.Visible;
            ShowLangtur = Visibility.Visible;
            ShowSkader = Visibility.Visible;
        }

        private void UpdateFilter()
        {
            // Nulstil filter
            ResetFilter();

            // Indlæs de valgte checkbox filtre
            var selectedCheckboxFilters = from c in _checkboxFilters
                                          where c.Active
                                          select c;

            if (!selectedCheckboxFilters.Any())
                return;

            ShowBoatsOut = selectedCheckboxFilters.Any(c => (string) c.Control.Content == "Både ude") ? Visibility.Visible : Visibility.Collapsed;
            ShowLangtur = selectedCheckboxFilters.Any(c => (string) c.Control.Content == "Langtur") ? Visibility.Visible : Visibility.Collapsed;
            ShowSkader = selectedCheckboxFilters.Any(c => (string) c.Control.Content == "Skader") ? Visibility.Visible : Visibility.Collapsed;
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