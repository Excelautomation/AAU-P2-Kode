using System;
using System.Linq;
using System.Windows.Input;
using ARK.Model;
using ARK.Model.DB;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;
using ARK.View.Protokolsystem.Pages;
using ARK.ViewModel.Protokolsystem.Pages;

namespace ARK.ViewModel.Protokolsystem.Confirmations
{
    public class ChangeDistanceConfirmViewModel : ConfirmationViewModelBase
    {
        // Fields
        private double _localDistance;
        private DistanceStatisticsViewModel _distanceStatisticsVM;
        private EndTripViewModel _EndTripVM;

        public EndTripViewModel EndTripVM
        {
            get { return _EndTripVM; }
            set { _EndTripVM = value; Notify(); }
        }

        public ChangeDistanceConfirmViewModel()
        {
            //LocalDistance = DistanceStatisticsVM.SelectedTrip.Trip.Distance;
        }
        
        public DistanceStatisticsViewModel DistanceStatisticsVM
        {
            get { return _distanceStatisticsVM; }
            set { _distanceStatisticsVM = value; Notify(); }
        }
        
        public double LocalDistance
        {
            get { return _localDistance; }
            set { _localDistance = value; Notify(); }
        }

        public ICommand SaveChanges
        {
            get
            {
                return GetCommand(e => 
                {
                    if (DistanceStatisticsVM != null) 
                    { 
                        DistanceStatisticsVM.SelectedTrip.Trip.Distance = LocalDistance;
                        DbArkContext.GetDbContext().SaveChanges();
                        DistanceStatisticsVM.NotifyTripList();
                        var a = DistanceStatisticsVM.SelectedMember;
                        DistanceStatisticsVM.SelectedMember = null;
                        DistanceStatisticsVM.SelectedMember = a;
                    }
                    else if (EndTripVM != null)
                    {
                        EndTripVM.SelectedTrip.Distance = LocalDistance;
                        DbArkContext.GetDbContext().SaveChanges();
                        // Lister bliver nok heller ikke opdateret her
                    }

                    Hide();
                    ProtocolSystem.StatisticsDistance.Execute(null);
                });
            }
        }

        public ICommand Cancel
        {
            get
            {
                return GetCommand(e =>
                {
                    Hide();
                });
            }
        }


    }
}