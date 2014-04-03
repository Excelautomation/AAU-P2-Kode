using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ARK.ViewModel
{
    class AdministrationssystemViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<CheckBox> CurrentFilter { 
            get { 
                return _filterControls; 
            } 
            set { 
                _filterControls = value;
                Notify("CurrentFilter"); 
            } 
        }
        public UserControl CurrentPage { 
            get { 
                return _currentPage; 
            } 
            private set { 
                _currentPage = value;
                Notify("CurrentPage"); 
            } 
        }

        public ICommand MenuOversigt { get { return GenerateCommand(PageOversigt, FiltersOversigt); } }
        public ICommand MenuBlanketter { get { return GenerateCommand(PageBlanketter, FiltersBlanketter); } }
        public ICommand MenuIndstilinger { get { return GenerateCommand(PageIndstillinger, FiltersIndstillinger); } }

        #region private
        private ObservableCollection<CheckBox> _filterControls;
        private UserControl _currentPage;

        // TODO: Implementer noget cache på objekterne
        private Administrationssystem.Oversigt PageOversigt { get {
            return new Administrationssystem.Oversigt();
        }}

        private Administrationssystem.Pages.Blanketter PageBlanketter { get {
            return new Administrationssystem.Pages.Blanketter();
        }}

        private Administrationssystem.Pages.Indstillinger PageIndstillinger { get {
                return new Administrationssystem.Pages.Indstillinger();
        }}

        private ObservableCollection<CheckBox> FiltersOversigt { get {
            return new ObservableCollection<CheckBox>() 
            {
                new CheckBox() { Name="oversigt_cb1", Content="Både på vandet"},
                new CheckBox() { Name="oversigt_cb2", Content="Skadesblanketter"},
                new CheckBox() { Name="oversigt_cb3", Content="Langtursansøgninger"},
                new CheckBox() { Name="oversigt_cb4", Content="Bådtype1" },
                new CheckBox() { Name="oversigt_cb5", Content="Bådtype2"},
                new CheckBox() { Name="oversigt_cb6", Content="Bådtype3"},
                new CheckBox() { Name="oversigt_cb7", Content="Bådtype4"}
            };
        }}

        private ObservableCollection<CheckBox> FiltersBlanketter { get {
            return new ObservableCollection<CheckBox>() {
                new CheckBox() { Name="blank_cb1", Content="Skadesblanketter"},
                new CheckBox() { Name="blank_cb2", Content="Langtursblanketter"},
                new CheckBox() { Name="blank_cb3", Content="Ulæste"},
                new CheckBox() { Name="blank_cb4", Content="Læste"}
            };
        }}

        private ObservableCollection<CheckBox> FiltersIndstillinger { get {
            return new ObservableCollection<CheckBox>();
        }}
        #endregion

        public AdministrationssystemViewModel()
        {
            DateTime starttime = DateTime.Now;

            // Start oversigten
            CurrentPage = PageOversigt;
            CurrentFilter = FiltersOversigt;

            Debug.WriteLine("AdministrationssystemViewModel starttime " + ((TimeSpan)(DateTime.Now - starttime)).TotalMilliseconds);
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
        private ICommand GenerateCommand(UserControl page, ObservableCollection<CheckBox> filters)
        {
            return new DelegateCommand<object>((e) =>
            {
                CurrentPage = page;
                CurrentFilter = filters;
            }, (e) => { return true; });
        }

        public class DelegateCommand<T> : ICommand
        {
            public Action<T> executeMethod { get; set; }
            public Func<T, bool> canExecuteMethod { get; set; }

            public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
            {
                this.executeMethod = executeMethod;
                this.canExecuteMethod = canExecuteMethod;
            }

            public bool CanExecute(object parameter)
            {
                return canExecuteMethod((T)parameter);
            }

            public void Execute(object parameter)
            {
                executeMethod((T)parameter);
            }

            public event EventHandler CanExecuteChanged;
        }
        #endregion
    }
}
