using System.Windows.Controls;
using System.Windows;
using ARK.ViewModel.Protokolsystem;
using ARK.Model;

namespace ARK.Protokolsystem.Pages
{
    /// <summary>
    /// Interaction logic for BeginTripsAdditionalInfo.xaml
    /// </summary>
    public partial class BeginTripsAdditionalInfo : UserControl
    {
        public BeginTripsAdditionalInfo()
        {
            InitializeComponent();
        }

        private void StackPanel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var vm = (BeginTripAdditionalInfoViewModel)this.DataContext;
            var member = (Member)((FrameworkElement)sender).DataContext;
            vm.RemoveMember(member);
        }
    }
}
