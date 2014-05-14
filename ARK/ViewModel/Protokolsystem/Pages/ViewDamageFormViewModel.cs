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
            set { _selectedDamageForm = value; Notify(); UpdateInfo(); }
        }


        public ViewDamageFormViewModel()
        {

            ParentAttached += (sender, e) =>
            {
                DamageForms = db.DamageForm.Where(x => x.Closed == false).ToList();

                // Select first instance
                if (DamageForms.Any())
                    SelectedDamageForm = DamageForms.First();

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
                    SelectedDamageForm.Closed = true;
                    db.SaveChanges();
                    DamageForms = db.DamageForm.Where(x => x.Closed == false).ToList();

                    if (DamageForms.Any())
                        SelectedDamageForm = DamageForms.First();
                });
            }
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
