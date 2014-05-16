using System.Data.Entity;
using System.Windows.Input;

using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;

namespace ARK.ViewModel.Administrationssystem
{
    public class FormsDamageViewModel : ContentViewModelBase
    {
        #region Fields

        private DamageForm _damageForm;

        private bool _recentChange;

        #endregion

        #region Public Properties

        public ICommand CloseDamageForm
        {
            get
            {
                return this.GetCommand(
                    () =>
                        {
                            this.DamageForm.Closed = true;

                            this.Save();
                        });
            }
        }

        public ICommand CloseForm
        {
            get
            {
                return this.GetCommand(
                    () =>
                        {
                            this.DamageForm.Closed = true;
                            this.Save();

                            var vm = (FormsViewModel)this.Parent;
                            var tempdmf2 = vm.DamageForms;
                            vm.DamageForms = null;
                            vm.DamageForms = tempdmf2;
                        });
            }
        }

        public DamageForm DamageForm
        {
            get
            {
                return this._damageForm;
            }

            set
            {
                this._damageForm = value;
                this.Notify();
            }
        }

        public ICommand OpenDamageForm
        {
            get
            {
                return this.GetCommand(
                    () =>
                        {
                            this.DamageForm.Closed = false;
                            this.Save();

                            var vm = (FormsViewModel)this.Parent;
                            var tempdmf2 = vm.DamageForms;
                            vm.DamageForms = null;
                            vm.DamageForms = tempdmf2;
                        });
            }
        }

        public bool RecentChange
        {
            get
            {
                return this._recentChange;
            }

            set
            {
                this._recentChange = value;
                this.Notify();
            }
        }

        public ICommand SaveChanges
        {
            get
            {
                return this.GetCommand(this.Save);
            }
        }

        #endregion

        #region Methods

        private void Save()
        {
            using (var db = new DbArkContext())
            {
                db.Entry(this.DamageForm).State = EntityState.Modified;
                db.SaveChanges();
            }

            this.RecentChange = true;
        }

        #endregion
    }
}