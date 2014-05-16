using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using ARK.Model;
using ARK.Model.DB;
using ARK.View.Protokolsystem.Additional;
using ARK.View.Protokolsystem.Confirmations;
using ARK.View.Protokolsystem.Pages;
using ARK.ViewModel.Protokolsystem.Additional;
using ARK.ViewModel.Protokolsystem.Confirmations;

namespace ARK.ViewModel.Protokolsystem.Pages
{
    internal class ViewDamageFormViewModel : ProtokolsystemContentViewModelBase
    {
        #region Fields

        private List<DamageForm> _damageForms;

        private FrameworkElement _infoPage;

        private DamageForm _selectedDamageForm;

        private DbArkContext db = DbArkContext.GetDbContext();

        #endregion

        #region Constructors and Destructors

        public ViewDamageFormViewModel()
        {
            this.ParentAttached += (sender, e) =>
                {
                    this.DamageForms = this.db.DamageForm.Where(x => x.Closed == false).ToList();

                    // Vis info
                    this.UpdateInfo();
                };
        }

        #endregion

        #region Public Properties

        public ICommand CreateDamageForm
        {
            get
            {
                return
                    this.GetCommand(
                        () => this.ProtocolSystem.NavigateToPage(() => new CreateDamageForm(), "OPRET NY SKADE"));
            }
        }

        public List<DamageForm> DamageForms
        {
            get
            {
                return this._damageForms;
            }

            set
            {
                this._damageForms = value;
                this.Notify();
            }
        }

        public ICommand FixDamageForm
        {
            get
            {
                return this.GetCommand(
                    () =>
                        {
                            var confirmView = new DamageFormConfirm();
                            var confirmViewModel = (DamageFormConfirmViewModel)confirmView.DataContext;

                            confirmViewModel.DamageForm = this.SelectedDamageForm;

                            this.ProtocolSystem.ShowDialog(confirmView);

                            this.ProtocolSystem.EnableSearch = true;
                        });
            }
        }

        public DamageForm SelectedDamageForm
        {
            get
            {
                return this._selectedDamageForm;
            }

            set
            {
                this._selectedDamageForm = value;
                this.Notify();
                this.UpdateInfo();
            }
        }

        public ICommand ViewDamageForm
        {
            get
            {
                return
                    this.GetCommand(
                        () => this.ProtocolSystem.NavigateToPage(() => new ViewDamageForm(), "SKADEBLANKETTER"));
            }
        }

        #endregion

        #region Properties

        private ViewDamageFormAdditionalInfoViewModel Info
        {
            get
            {
                return this.InfoPage.DataContext as ViewDamageFormAdditionalInfoViewModel;
            }
        }

        private FrameworkElement InfoPage
        {
            get
            {
                return this._infoPage ?? (this._infoPage = new ViewDamageFormAdditionalInfo());
            }
        }

        #endregion

        #region Public Methods and Operators

        public void ResetList()
        {
            this.DamageForms = this.db.DamageForm.Where(x => x.Closed == false).ToList();

            if (this.DamageForms.Any())
            {
                this.SelectedDamageForm = this.DamageForms.First();
            }
        }

        #endregion

        #region Methods

        private void UpdateInfo()
        {
            this.Info.SelectedDamageForm = this.SelectedDamageForm;

            this.ProtocolSystem.ChangeInfo(this.InfoPage, this.Info);
        }

        #endregion
    }
}