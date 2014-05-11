using System.Windows.Input;
using ARK.View.Protokolsystem.Pages;
using ARK.Model;
using System.Collections.Generic;
using ARK.Model.DB;
using System.Linq;

namespace ARK.ViewModel.Protokolsystem.Pages
{
    class ViewDamageFormViewModel : ProtokolsystemContentViewModelBase
    {
        private List<DamageForm> _damageForms;

        public ViewDamageFormViewModel()
        {
            var db = DbArkContext.GetDbContext();
            DamageForms = db.DamageForm.Where(x => x.Closed == false).ToList();
        }

        public List<DamageForm> DamageForms
        {
            get { return _damageForms; }
            set { _damageForms = value; Notify(); }
        }
        

        public ICommand CreateDamageForm
        {
            get
            {
                return GetCommand<object>(a => ProtocolSystem.NavigateToPage(() => new CreateDamageForm(), "OPRET NY SKADE"));
            }
        }

        public ICommand ViewDamageForm
        {
            get
            {
                return GetCommand<object>(a => ProtocolSystem.NavigateToPage(() => new ViewDamageForm(), "AKTIVE SKADES BLANKETTER"));
            }
        }
    }
}
