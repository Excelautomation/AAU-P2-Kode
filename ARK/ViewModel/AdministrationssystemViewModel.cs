using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ARK.Administrationssystem.Pages;

namespace ARK.ViewModel
{
    class AdministrationssystemViewModel : INotifyPropertyChanged
    {
        private PageInformation _page;
        public PageInformation Page
        {
            get { return _page; }
            set { _page = value; Notify("Page"); }
        }

        public ICommand MenuOverview { get { return GenerateCommand("Overview", PageOverview, FiltersOverview); } }
        public ICommand MenuForms { get { return GenerateCommand("Forms", PageForms, FiltersForms); } }
        public ICommand MenuBoats { get { return GenerateCommand("Boats", PageBoats, FiltersBoats); } }
        public ICommand MenuConfigurations { get { return GenerateCommand("Configurations", PageConfigurations, FiltersConfigurations); } }

        #region private
        private ObservableCollection<Control> _filterControls;
        private UserControl _currentPage;

        // TODO: Implementer noget cache på objekterne
        private Oversigt PageOverview
        {
            get
            {
                return new Oversigt();
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
                    new CheckBox() { Content = "Langtur" },
                    new CheckBox() { Content = "Skader" },
                    new CheckBox() { Content = "Både ude" }
                };
            }
        }

        private ObservableCollection<Control> FiltersForms
        {
            get
            {
                return new ObservableCollection<Control>
                {
                    new CheckBox() { Content = "Langtur" },
                    new CheckBox() { Content = "Skader" },
                    new Separator() { Height = 20 },
                    new CheckBox() { Content = "Afviste" },
                    new CheckBox() { Content = "Godkendte" }
                };
            }
        }

        private ObservableCollection<Control> FiltersBoats
        {
            get
            {
                return new ObservableCollection<Control>
                {
                    new CheckBox() { Content = "Både ude" },
                    new CheckBox() { Content = "Både hjemme" },
                    new Separator() { Height = 20 },
                    new CheckBox() { Content = "Både under reparation" },
                    new CheckBox() { Content = "Beskadigede både" },
                    new CheckBox() { Content = "Inaktive både" },
                    new CheckBox() { Content = "Funktionelle både" }
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
            Page = new PageInformation();
            MenuOverview.Execute(null);

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
        private ICommand GenerateCommand(string pageName, UserControl page, ObservableCollection<Control> filters)
        {
            return new DelegateCommand<object>((e) =>
            {
                Page.PageName = pageName;
                Page.Page = page;
                Page.Filter = filters;
            }, (e) => true);
        }
        #endregion
    }
}
