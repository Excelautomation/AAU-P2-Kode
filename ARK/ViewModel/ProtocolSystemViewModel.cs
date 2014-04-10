using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ARK.Protokolsystem.Pages;

namespace ARK.ViewModel
{
    class ProtocolSystemViewModel : INotifyPropertyChanged
    {
        public string HeadlineText
        {
            get { return this._headlineText; }
            set { this._headlineText = value; Notify("HeadlineText"); }
        }

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

        #region Commands
        public ICommand StartTrip
        {
            get
            {
                return GenerateCommand("Start rotur", PageBeginTripBoats);
            }
        }

        public ICommand EndTrip
        {
            get
            {
                return GenerateCommand("Afslut rotur", PageEndTrip);
            }
        }

        public ICommand BoatsOut
        {
            get
            {
                return GenerateCommand("Både på vandet", PageBoatsOut);
            }
        }

        public ICommand StatisticsDistance
        {
            get
            {
                return GenerateCommand("Kilometerstatistik", PageDistanceStatistics);
            }
        }

        public ICommand MemberInformation
        {
            get
            {
                return GenerateCommand("Medlemsinformation", PageMembersInformation);
            }
        }

        public ICommand CreateDamage
        {
            get
            {
                return GenerateCommand("Opret blanket ► Skade", PageCreateInjury);
            }
        }

        public ICommand CreateLongDistance
        {
            get
            {
                return GenerateCommand("Opret blanket ► Langtur", PageCreateLongDistance);
            }
        }
        #endregion

        #region Pages
        // TODO: Implementer noget cache på objekterne
        public BeginTripBoats PageBeginTripBoats { get { return new BeginTripBoats(); } }
        public EndTrip PageEndTrip { get { return new EndTrip(); } }
        public BoatsOut PageBoatsOut { get { return new BoatsOut(); } }
        public DistanceStatistics PageDistanceStatistics { get { return new DistanceStatistics(); } }
        public MembersInformation PageMembersInformation { get { return new MembersInformation(); } }
        public CreateInjury PageCreateInjury { get { return new CreateInjury(); } }
        public CreateLongDistance PageCreateLongDistance { get { return new CreateLongDistance(); } }

        #endregion

        #region Private
        private string _headlineText;
        private UserControl _currentPage;
        #endregion

        public ProtocolSystemViewModel()
        {
            StartTrip.Execute(null);
            HeadlineText = "Aalborg Roklubs Protokolsystem";
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
        private ICommand GenerateCommand(string HeadLineText, UserControl page)
        {
            return new DelegateCommand<object>((e) =>
            {
                this.HeadlineText = HeadLineText;
                CurrentPage = page;
            }, (e) => { return true; });
        }
        #endregion
    }
}
