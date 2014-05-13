using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using ARK.View.Protokolsystem.Additional;
using ARK.View.Protokolsystem.Pages;
using ARK.ViewModel.Protokolsystem.Additional;

namespace ARK.ViewModel.Protokolsystem.Pages
{
    class ViewLongTripFormViewModel : ProtokolsystemContentViewModelBase
    {
        // Fields
        private List<LongTripForm> _longTripForms;
        private  FrameworkElement _infoPage;
        private LongTripForm _selectedLongTripForm;

        // Constructor
        public ViewLongTripFormViewModel()
        {
            var db = DbArkContext.GetDbContext();

            ParentAttached += (sender, e) =>
            {
                LongTripForms = db.LongTripForm.ToList();
                SelectedLongTripForm = LongTripForms[0];

                UpdateInfo();
            };
        }

        // Props
        public List<LongTripForm> LongTripForms
        {
            get { return _longTripForms; }
            set { _longTripForms = value; Notify(); }
        }

        public LongTripForm SelectedLongTripForm
        {
            get { return _selectedLongTripForm; }
            set { _selectedLongTripForm = value; Notify(); }
        }

        public ICommand CreateLongTripForm
        {
            get
            {
                return GetCommand<object>(a => ProtocolSystem.NavigateToPage(() => new CreateLongTripForm(), "OPRET NY LANGTUR"));
            }
        }

        public ICommand ViewLongTripForm
        {
            get
            {
                return GetCommand<object>(a => ProtocolSystem.NavigateToPage(() => new ViewLongTripForm(), "AKTIVE LANGTURS BLANKETTER"));
            }
        }

        private FrameworkElement InfoPage
        {
            get { return _infoPage ?? (_infoPage = new ViewLongTripFormAdditionalInfo()); }
        }

        private ViewLongTripFormAdditionalInfoViewModel Info
        {
            get { return InfoPage.DataContext as ViewLongTripFormAdditionalInfoViewModel; }
        }

        private void UpdateInfo()
        {
            Info.SelectedLongTripForm = SelectedLongTripForm;

            ProtocolSystem.ChangeInfo(InfoPage, Info);
        }
    }
}
