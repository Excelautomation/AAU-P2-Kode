using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ARK.ViewModel
{
    class AdministrationssystemViewModel : INotifyPropertyChanged
    {
        public UserControl CurrentPage
        {
            get
            {
                return this._currentPage;
            }
            private set
            {
                this._currentPage = value;
                Notify("CurrentPage");
            }
        }
        public ObservableCollection<Control> CurrentFilter
        {
            get
            {
                return this._filterControls;
            }
            set
            {
                this._filterControls = value;
                Notify("CurrentFilter");
                Notify("ShowFilter");
            }
        }

        public Visibility ShowSearch
        {
            get
            {
                return Visibility.Visible;
            }
        }
        public Visibility ShowFilter
        {
            get
            {
                return CurrentFilter.Count == 0 ? Visibility.Hidden : Visibility.Visible;
            }
        }

        public bool OverviewSelected { get { return true; } set { } }
        public bool FormsSelected { get { return true; } set { } }
        public bool BoatsSelected { get { return true; } set { } }
        public bool ConfigurationsSelected { get { return true; } set { } }

        public ICommand MenuOverview { get { return GenerateCommand(PageOverview, FiltersOverview); } }
        public ICommand MenuForms { get { return GenerateCommand(PageForms, FiltersForms); } }
        public ICommand MenuBoats { get { return GenerateCommand(PageBoats, FiltersBoats); } }
        public ICommand MenuConfigurations { get { return GenerateCommand(PageConfigurations, FiltersConfigurations); } }

        #region private
        private ObservableCollection<Control> _filterControls;
        private UserControl _currentPage;

        // TODO: Implementer noget cache på objekterne
        private Administrationssystem.Oversigt PageOverview
        {
            get
            {
                return new Administrationssystem.Oversigt();
            }
        }

        private Administrationssystem.Pages.Blanketter PageForms
        {
            get
            {
                return new Administrationssystem.Pages.Blanketter();
            }
        }

        private Administrationssystem.Pages.Baede PageBoats
        {
            get
            {
                return new Administrationssystem.Pages.Baede();
            }
        }

        private Administrationssystem.Pages.Indstillinger PageConfigurations
        {
            get
            {
                return new Administrationssystem.Pages.Indstillinger();
            }
        }

        private ObservableCollection<Control> FiltersOverview
        {
            get
            {
                return new ObservableCollection<Control>() 
                {
                    new CheckBox() { Content = "Langtur"},
                    new CheckBox() { Content = "Skader"},
                    new CheckBox() { Content = "Både ude"}
                };
            }
        }

        private ObservableCollection<Control> FiltersForms
        {
            get
            {
                return new ObservableCollection<Control>() 
                {
                    new CheckBox() { Content = "Langtur"},
                    new CheckBox() { Content = "Skader"},
                    new Separator() { Height = 20 },
                    new CheckBox() { Content = "Afviste"},
                    new CheckBox() { Content = "Godkendte"}
                };
            }
        }

        private ObservableCollection<Control> FiltersBoats
        {
            get
            {
                return new ObservableCollection<Control>()
                {
                    new CheckBox() { Content = "Både ude"},
                    new CheckBox() { Content = "Både hjemme"},
                    new Separator() { Height = 20 },
                    new CheckBox() { Content = "Både under reparation"},
                    new CheckBox() { Content = "Beskadigede både"},
                    new CheckBox() { Content = "Inaktive både"},
                    new CheckBox() { Content = "Funktionelle både"}
                };
            }
        }

        private ObservableCollection<Control> FiltersConfigurations
        {
            get
            {
                return new ObservableCollection<Control>();
            }
        }
        #endregion

        public AdministrationssystemViewModel()
        {
            TimeCounter.StartTimer();

            // Start oversigten
            CurrentPage = PageOverview;
            CurrentFilter = FiltersOverview;
            OverviewSelected = true;

            TimeCounter.StopTime("AdministrationssystemViewModel load");
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
        #region Command
        private ICommand GenerateCommand(UserControl page, ObservableCollection<Control> filters)
        {
            return new DelegateCommand<object>((e) =>
            {
                CurrentPage = page;
                CurrentFilter = filters;
            }, (e) => { return true; });
        }
        #endregion
    }
}
