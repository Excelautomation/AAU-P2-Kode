using System.Windows;
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
			this.InitializeComponent();

            ((ContentViewModelBase)this.DataContext).ParentAttached += AdminLoginConfirm_ParentAttached;
		}

        void AdminLoginConfirm_ParentAttached(object sender, System.EventArgs e)
        {
            var vm = ((ContentViewModelBase)this.DataContext);

            vm.ParentAttached -= AdminLoginConfirm_ParentAttached;
            vm.GotFocus.Execute(UsernameBox);
        }
	}
}