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

namespace ARK.ViewModel.Protokolsystem.Confirmations
{
    public class BeginTripBoatsConfirmViewModel : ConfirmationViewModelBase
    {
        // Fields
        private Trip _trip;

        public Trip Trip
        {
            get { return _trip; }
            set
            {
                _trip = value;
                Notify();
            }
        }

        public ICommand Save
        {
            get
            {
                return GetCommand(() =>
                {
                    DbArkContext.GetDbContext().Trip.Add(Trip);
                    DbArkContext.GetDbContext().SaveChanges();

                    Hide();
                    ProtocolSystem.StatisticsDistance.Execute(null);
                });
            }
        }

        public ICommand CancelTrip
        {
            get
            {
                return GetCommand(() => 
                {
                    Hide();
                    ProtocolSystem.StatisticsDistance.Execute(null);
                });
            }
        }

        public ICommand ChangeTrip
        {
            get
            {
                return GetCommand(Hide);
            }
        }

        public DateTime Sunset
        {
            get { return ARK.HelperFunctions.SunsetClass.Sunset; }
        }
    }
}