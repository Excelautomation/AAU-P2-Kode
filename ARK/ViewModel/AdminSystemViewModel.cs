using System.Windows.Controls;
using System.Windows.Input;
using ARK.Administrationssystem.Pages;
using ARK.ViewModel.Base;

namespace ARK.ViewModel
{
    internal class AdminSystemViewModel : Base.ViewModel
    {
        private PageInformation _page;
        private Baede _pagebaede;
        private Blanketter _pageforms;
        private Oversigt _pageoversigt;
        private Indstillinger _pagesettings;

        public AdminSystemViewModel()
        {
            TimeCounter.StartTimer();

            // Start oversigten
            Page = new PageInformation();
            MenuOverview.Execute(null);

            TimeCounter.StopTime("AdministrationssystemViewModel load");
        }

        public PageInformation Page
        {
            get { return _page; }
            set { _page = value; Notify(); }
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
            get { return _pageoversigt ?? (_pageoversigt = new Oversigt()); }
        }

        private Blanketter PageForms
        {
            get { return _pageforms ?? (_pageforms = new Blanketter()); }
        }

        private Baede PageBoats
        {
            get { return _pagebaede ?? (_pagebaede = new Baede()); }
        }

        private Indstillinger PageConfigurations
        {
            get { return _pagesettings ?? (_pagesettings = new Indstillinger()); }
        }

        #endregion private

        private ICommand GenerateCommand(string pageName, UserControl page)
        {
            return GetCommand<object>((e) =>
            {
                Page.PageName = pageName;
                Page.Page = page;
            });
        }
    }
}