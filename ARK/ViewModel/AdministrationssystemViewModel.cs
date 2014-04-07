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

        public ICommand MenuOversigt { get { return GenerateCommand(PageOversigt, FiltersOversigt); } }
        public ICommand MenuBlanketter { get { return GenerateCommand(PageBlanketter, FiltersBlanketter); } }
        public ICommand MenuIndstilinger { get { return GenerateCommand(PageIndstillinger, FiltersIndstillinger); } }

        #region private
        private ObservableCollection<Control> _filterControls;
        private UserControl _currentPage;

        // TODO: Implementer noget cache på objekterne
        private Administrationssystem.Oversigt PageOversigt
        {
            get
            {
                return new Administrationssystem.Oversigt();
            }
        }

        private Administrationssystem.Pages.Blanketter PageBlanketter
        {
            get
            {
                return new Administrationssystem.Pages.Blanketter();
            }
        }

        private Administrationssystem.Pages.Indstillinger PageIndstillinger
        {
            get
            {
                return new Administrationssystem.Pages.Indstillinger();
            }
        }

        private ObservableCollection<Control> FiltersOversigt
        {
            get
            {
                return new ObservableCollection<Control>() 
                {
                    new CheckBox() { Content = "Både på vandet"},
                    new CheckBox() { Content = "Skadesblanketter"},
                    new CheckBox() { Content = "Langtursansøgninger"},
                    new Separator() { Height = 20 },
                    new CheckBox() { Content = "Bådtype1" },
                    new CheckBox() { Content = "Bådtype2" },
                    new CheckBox() { Content = "Bådtype3" },
                    new CheckBox() { Content = "Bådtype4" }
                };
            }
        }

        private ObservableCollection<Control> FiltersBlanketter
        {
            get
            {
                return new ObservableCollection<Control>() {
                    new CheckBox() { Content = "Skadesblanketter"},
                    new CheckBox() { Content = "Langtursblanketter"},
                    new CheckBox() { Content = "Ulæste"},
                    new CheckBox() { Content = "Læste"}
                };
            }
        }

        private ObservableCollection<Control> FiltersIndstillinger
        {
            get
            {
                return new ObservableCollection<Control>();
            }
        }
        #endregion

        public AdministrationssystemViewModel()
        {
            TimeCounter.startTimer();

            // Start oversigten
            CurrentPage = PageOversigt;
            CurrentFilter = FiltersOversigt;

            TimeCounter.stopTime("AdministrationssystemViewModel load");
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
