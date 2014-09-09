using System.Data.Entity;
using System.Windows.Input;

using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Administrationssystem
{
    public class FormsDamageViewModel : ContentViewModelBase
    {
        private DamageForm _damageForm;

        private bool _recentChange;

        public ICommand CloseDamageForm
        {
            get
            {
                return GetCommand(
                    () =>
                        {
                            DamageForm.Closed = true;

                            Save();

                            var vm = (FormsViewModel)Parent;
                            vm.UpdateFilter();
                        });
            }
        }

        public ICommand CloseForm
        {
            get
            {
                return GetCommand(
                    () =>
                        {
                            DamageForm.Closed = true;
                            Save();

                            var vm = (FormsViewModel)Parent;
                            vm.UpdateFilter();
                        });
            }
        }

        public DamageForm DamageForm
        {
            get
            {
                return _damageForm;
            }

            set
            {
                _damageForm = value;
                Notify();
            }
        }

        public ICommand OpenDamageForm
        {
            get
            {
                return GetCommand(
                    () =>
                        {
                            DamageForm.Closed = false;
                            Save();

                            var vm = (FormsViewModel)Parent;
                            vm.UpdateFilter();
                        });
            }
        }

        public bool RecentChange
        {
            get
            {
                return _recentChange;
            }

            set
            {
                _recentChange = value;
                Notify();
            }
        }

        public ICommand SaveChanges
        {
            get
            {
                return GetCommand(Save);
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