using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using ARK.View.Protokolsystem.Pages;

namespace ARK.ViewModel.Protokolsystem.Pages
{
    class ViewLongTripFormViewModel : ProtokolsystemContentViewModelBase
    {
        // Fields
        private List<LongTripForm> _longTripForms;

        // Constructor
        public ViewLongTripFormViewModel()
        {
            var db = DbArkContext.GetDbContext();

            LongTripForms = db.LongTripForm.ToList();
        }

        // Props
        public List<LongTripForm> LongTripForms
        {
            get { return _longTripForms; }
            set { _longTripForms = value; Notify(); }
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
    }
}
