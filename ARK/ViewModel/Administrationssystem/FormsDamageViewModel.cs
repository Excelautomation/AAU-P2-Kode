using System.Data.Entity;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Administrationssystem
{
    public class FormsDamageViewModel : ContentViewModelBase
    {
        private bool _recentChange;
        private DamageForm _damageForm;

        public bool RecentChange
        {
            get { return _recentChange; }
            set
            {
                _recentChange = value;
                Notify();
            }
        }

        public DamageForm DamageForm
        {
            get { return _damageForm; }
            set
            {
                _damageForm = value;
                Notify();
            }
        }

        public ICommand SaveChanges
        {
            get { return GetCommand<object>(e => Save()); }
        }

        public ICommand CloseDamageForm
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    DamageForm.Closed = true;

                    Save();
                });
            }
        }

        public ICommand OpenDamageForm
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    DamageForm.Closed = false;
                    Save();

                    var vm = (FormsViewModel)Parent;
                    var tempdmf2 = vm.DamageForms;
                    vm.DamageForms = null;
                    vm.DamageForms = tempdmf2;
                });
            }
        }

        public ICommand CloseForm
        {
            get
            {
                return GetCommand<object>(e =>
                {
                    DamageForm.Closed = true;
                    Save();

                    var vm = (FormsViewModel)Parent;
                    var tempdmf2 = vm.DamageForms;
                    vm.DamageForms = null;
                    vm.DamageForms = tempdmf2;
                });
            }
        }

        private void Save()
        {
            using (var db = new DbArkContext())
            {
                db.Entry(DamageForm).State = EntityState.Modified;
                db.SaveChanges();
            }

            RecentChange = true;
        }
    }
}