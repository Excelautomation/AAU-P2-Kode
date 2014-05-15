using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ARK.View.Protokolsystem.Additional;
using ARK.View.Protokolsystem.Pages;
using ARK.Model;
using System.Collections.Generic;
using ARK.Model.DB;
using System.Linq;
using ARK.ViewModel.Protokolsystem.Additional;
using ARK.View.Protokolsystem.Confirmations;
using ARK.ViewModel.Protokolsystem.Confirmations;

namespace ARK.ViewModel.Protokolsystem.Pages
{
    class ViewDamageFormViewModel : ProtokolsystemContentViewModelBase
    {
        private List<DamageForm> _damageForms;
        private FrameworkElement _infoPage;
        DbArkContext db = DbArkContext.GetDbContext();

        private DamageForm _selectedDamageForm;

        public DamageForm SelectedDamageForm
        {
            get { return _selectedDamageForm; }
            set 
            { 
                _selectedDamageForm = value; Notify(); UpdateInfo(); 
            }
        }

        public ViewDamageFormViewModel()
        {
            ParentAttached += (sender, e) =>
            {
                DamageForms = db.DamageForm.Where(x => x.Closed == false).ToList();

                // Vis info
                UpdateInfo();
            };
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
                return GetCommand(() => ProtocolSystem.NavigateToPage(() => new CreateDamageForm(), "OPRET NY SKADE"));
            }
        }

        public ICommand ViewDamageForm
        {
            get
            {
                return GetCommand(() => ProtocolSystem.NavigateToPage(() => new ViewDamageForm(), "SKADEBLANKETTER"));
            }
        }

        public ICommand FixDamageForm
        {
            get
            {
                return GetCommand(() => 
                {
                    var confirmView = new DamageFormConfirm();
                    var confirmViewModel = (DamageFormConfirmViewModel)confirmView.DataContext;

                    confirmViewModel.DamageForm = SelectedDamageForm;

                    ProtocolSystem.ShowDialog(confirmView);

                    ProtocolSystem.EnableSearch = true;
                });
            }
        }

        public void ResetList()
        {
            DamageForms = db.DamageForm.Where(x => x.Closed == false).ToList();

            if (DamageForms.Any())
                SelectedDamageForm = DamageForms.First();
        }

        private FrameworkElement InfoPage
        {
            get { return _infoPage ?? (_infoPage = new ViewDamageFormAdditionalInfo()); }
        }

        private ViewDamageFormAdditionalInfoViewModel Info
        {
            get { return InfoPage.DataContext as ViewDamageFormAdditionalInfoViewModel; }
        }

        private void UpdateInfo()
        {
            Info.SelectedDamageForm = SelectedDamageForm;


            ProtocolSystem.ChangeInfo(InfoPage, Info);
        }
    }
}
