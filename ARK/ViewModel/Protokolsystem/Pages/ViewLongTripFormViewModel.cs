using ARK.Protokolsystem.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;

namespace ARK.ViewModel.Protokolsystem
{
    class ViewLongTripFormViewModel : ProtokolsystemContentViewModelBase
    {
        // Fields
        private List<LongTripForm> _lingTripForms;

        // Constructor
        public ViewLongTripFormViewModel()
        {
            var db = DbArkContext.GetDbContext();
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
