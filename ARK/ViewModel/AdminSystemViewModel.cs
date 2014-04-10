using ARK.Administrationssystem.Pages;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace ARK.ViewModel
{
    internal class AdminSystemViewModel : INotifyPropertyChanged
    {
        private PageInformation _page;
        private Oversigt _pageoversigt;
        private Blanketter _pageforms;
        private Baede _pagebaede;
        private Indstillinger _pagesettings;

        public PageInformation Page
        {
            get { return _page; }
            set { _page = value; Notify("Page"); }
        }

        #region Commands

        public ICommand MenuOverview { get { return GenerateCommand("Overview", PageOverview); } }

        public ICommand MenuForms { get { return GenerateCommand("Forms", PageForms); } }

        public ICommand MenuBoats { get { return GenerateCommand("Boats", PageBoats); } }

        public ICommand MenuConfigurations { get { return GenerateCommand("Configurations", PageConfigurations); } }

        #endregion Commands

        #region private

        // TODO: Implementer noget cache på objekterne
        private Oversigt PageOverview
        {
            get { return _pageoversigt ?? (_pageoversigt = new Oversigt() { DataContext = new OverviewViewModel() }); }
        }

        private Blanketter PageForms
        {
            get { return _pageforms ?? (_pageforms = new Blanketter { DataContext = new FormsViewModel() }); }
        }

        private Baede PageBoats
        {
            get { return _pagebaede ?? (_pagebaede = new Baede { DataContext = new BoatViewModel() }); }
        }

        private Indstillinger PageConfigurations
        {
            get { return _pagesettings ?? (_pagesettings = new Indstillinger() { DataContext = new SettingsViewModel() }); }
        }

        #endregion private

        public AdminSystemViewModel()
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
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion Property

        #region Command

        private ICommand GenerateCommand(string pageName, UserControl page)
        {
            return new DelegateCommand<object>((e) =>
            {
                Page.PageName = pageName;
                Page.Page = page;
            }, (e) => true);
        }

        #endregion Command
    }
}