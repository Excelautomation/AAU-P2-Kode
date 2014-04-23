using System.Windows.Controls;
using System.Windows.Input;
using ARK.Protokolsystem.Pages;

namespace ARK.ViewModel
{
    public class ProtocolSystemViewModel : Base.ViewModel
    {
        private UserControl _currentInfo;
        private UserControl _currentPage;
        private string _headlineText;

        public ProtocolSystemViewModel()
        {
            StartTrip.Execute(null);
            HeadlineText = "Aalborg Roklubs Protokolsystem";
        }

        public string HeadlineText
        {
            get { return _headlineText; }
            set { _headlineText = value; Notify(); }
        }

        public UserControl CurrentPage
        {
            get
            {
                return _currentPage;
            }

            private set
            {
                _currentPage = value;
                Notify();
            }
        }

        public UserControl CurrentInfo
        {
            get
            {
                return _currentInfo;
            }

            private set
            {
                _currentInfo = value;
                Notify();
            }
        }

        #region Commands
        public ICommand StartTrip
        {
            get
            {
                return GenerateCommand("Start rotur", PageBeginTripBoats, PageAdditionalInfo);
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
        public BeginTripBoats PageBeginTripBoats
        {
            get { return new BeginTripBoats(); }
        }
        public EndTrip PageEndTrip
        { 
            get { return new EndTrip(); } 
        }
        public BoatsOut PageBoatsOut 
        { 
            get { return new BoatsOut(); } 
        }
        public DistanceStatistics PageDistanceStatistics 
        { 
            get { return new DistanceStatistics(); } 
        }
        public MembersInformation PageMembersInformation 
        { 
            get { return new MembersInformation(); } 
        }
        public CreateInjury PageCreateInjury 
        { 
            get { return new CreateInjury(); } 
        }
        public CreateLongDistance PageCreateLongDistance 
        { 
            get { return new CreateLongDistance(); } 
        }
        public AdditionalInfo PageAdditionalInfo 
        { 
            get { return new AdditionalInfo(); } 
        }

        #endregion

        private ICommand GenerateCommand(string headLineText, UserControl page)
        {
            return GenerateCommand(HeadlineText, page, null);
        }

        private ICommand GenerateCommand(string headLineText, UserControl page, UserControl additionalInfo)
        {
            return GetCommand<object>((e) =>
            {
                HeadlineText = headLineText;
                CurrentPage = page;
                CurrentInfo = additionalInfo;
                CurrentInfo.DataContext = CurrentPage.DataContext;
            });
        }
    }
}
