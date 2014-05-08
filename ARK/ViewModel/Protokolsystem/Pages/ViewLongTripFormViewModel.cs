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
        private List<LongTripForm> _lingTripForms;

        // Constructor
        public ViewLongTripFormViewModel()
        {
            var db = DbArkContext.GetDbContext();

            ParentAttached += (sender, e) =>
                _lingTripForms = db.LongTripForm.ToList();
        }

        // Props
        public List<LongTripForm> LongTripForms
        {
            get { return _lingTripForms; }
            set { _lingTripForms = value; Notify(); }
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
