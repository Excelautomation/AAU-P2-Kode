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

        public ViewDamageFormViewModel()
        {
            var db = DbArkContext.GetDbContext();

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
            //Info. = new ObservableCollection<Boat> { SelectedBoat };
            //Info.SelectedMembers = SelectedMembers;

            ProtocolSystem.ChangeInfo(InfoPage, Info);
        }
    }
}
