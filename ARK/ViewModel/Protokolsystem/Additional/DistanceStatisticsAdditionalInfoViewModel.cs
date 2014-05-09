using System.Linq;
using System.Windows.Input;
using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Interfaces;
using System.Collections.ObjectModel;

namespace ARK.ViewModel.Protokolsystem.Additional
{
    public class DistanceStatisticsAdditionalInfoViewModel : ContentViewModelBase
    {
        // Fields
        public Member SelectedMember { get; set; }
        public Trip SelectedTrip { get; set; }

    }
}