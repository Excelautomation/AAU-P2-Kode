using System;
using System.Windows.Controls;

using ARK.ViewModel.Base;

namespace ARK.View.Protokolsystem.Confirmations
{
    /// <summary>
    /// Interaction logic for BeginTripBoatsConfirm.xaml
    /// </summary>
    public partial class AdminLoginConfirm : UserControl
    {
        public AdminLoginConfirm()
        {
            InitializeComponent();

            ((ContentViewModelBase)DataContext).ParentAttached += AdminLoginConfirm_ParentAttached;
        }

        private void AdminLoginConfirm_ParentAttached(object sender, EventArgs e)
        {
            var vm = (ContentViewModelBase)DataContext;

            vm.ParentAttached -= AdminLoginConfirm_ParentAttached;
            vm.GotFocus.Execute(UsernameBox);
        }
    }
}