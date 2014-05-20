using System.Collections.Generic;
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
        private List<DamageForm> _damageForms;

        private FrameworkElement _infoPage;

        private DamageForm _selectedDamageForm;

        private DbArkContext db = DbArkContext.GetDbContext();

        public ViewDamageFormViewModel()
        {
            ParentAttached += (sender, e) =>
                {
                    DamageForms = db.DamageForm.Where(x => x.Closed == false).ToList();

                    // Vis info
                    UpdateInfo();
                };
        }

        public ICommand CreateDamageForm
        {
            get
            {
                return GetCommand(() => ProtocolSystem.NavigateToPage(() => new CreateDamageForm(), "OPRET NY SKADE"));
            }
        }

        public List<DamageForm> DamageForms
        {
            get
            {
                return _damageForms;
            }

            set
            {
                _damageForms = value;
                Notify();
            }
        }

        public ICommand FixDamageForm
        {
            get
            {
                return GetCommand(
                    () =>
                        {
                            var confirmView = new DamageFormConfirm();
                            var confirmViewModel = (DamageFormConfirmViewModel)confirmView.DataContext;

                            confirmViewModel.DamageForm = SelectedDamageForm;

                            ProtocolSystem.ShowDialog(confirmView);

                            ProtocolSystem.EnableSearch = true;
                        });
            }
        }

        public DamageForm SelectedDamageForm
        {
            get
            {
                return _selectedDamageForm;
            }

            set
            {
                _selectedDamageForm = value;
                Notify();
                UpdateInfo();
            }
        }

        public ICommand ViewDamageForm
        {
            get
            {
                return GetCommand(() => ProtocolSystem.NavigateToPage(() => new ViewDamageForm(), "SKADEBLANKETTER"));
            }
        }

        private ViewDamageFormAdditionalInfoViewModel Info
        {
            get
            {
                return InfoPage.DataContext as ViewDamageFormAdditionalInfoViewModel;
            }
        }

        private FrameworkElement InfoPage
        {
            get
            {
                return _infoPage ?? (_infoPage = new ViewDamageFormAdditionalInfo());
            }
        }

        public void ResetList()
        {
            DamageForms = db.DamageForm.Where(x => x.Closed == false).ToList();

            if (DamageForms.Any())
            {
                SelectedDamageForm = DamageForms.First();
            }
        }

        private void UpdateInfo()
        {
            Info.SelectedDamageForm = SelectedDamageForm;

            ProtocolSystem.ChangeInfo(InfoPage, Info);
        }
    }
}