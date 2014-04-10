using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ARK.Administrationssystem.Pages;
using ARK.Model;

namespace ARK.ViewModel
{
    class AdminSystemViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Skadesblanket> _skadesblanketter = new ObservableCollection<Skadesblanket>();

        public ObservableCollection<Skadesblanket> Skadesblanketter { get { return _skadesblanketter; } set { _skadesblanketter = value; Notify("Skadesblanketter"); } }

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
        // TODO: Implementer noget cache på objekterne
        private Oversigt PageOverview
        {
            get
            {
                return new Oversigt
                {
                    DataContext = this
                };
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

        public AdminSystemViewModel()
        {
            TimeCounter.StartTimer();

            // Start oversigten
            Page = new PageInformation();
            MenuOverview.Execute(null);

            Skadesblanketter.Add(new Skadesblanket { ReportedBy = "Martin er noob" });
            Skadesblanketter.Add(new Skadesblanket { ReportedBy = "Martin er mere noob" });

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
