using System.Windows;
using System.Windows.Controls;

using ARK.ViewModel.Base;

namespace ARK.View.Protokolsystem.Confirmations
{
    using System;

    /// <summary>
    /// Interaction logic for BeginTripBoatsConfirm.xaml
    /// </summary>
    public partial class AdminLoginConfirm : UserControl
    {
        #region Constructors and Destructors

        public AdminLoginConfirm()
        {
            this.InitializeComponent();

            ((ContentViewModelBase)this.DataContext).ParentAttached += this.AdminLoginConfirm_ParentAttached;
        }

        #endregion

        #region Methods

        private void AdminLoginConfirm_ParentAttached(object sender, EventArgs e)
        {
            var vm = (ContentViewModelBase)this.DataContext;

            vm.ParentAttached -= this.AdminLoginConfirm_ParentAttached;
            vm.GotFocus.Execute(this.UsernameBox);
        }

        #endregion
    }
}