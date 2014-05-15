using System;
using System.Collections.Generic;
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

        public BeginTripBoatsConfirmViewModel()
        {
        }

        public Trip Trip
        {
            get { return _trip; }
            set
            {
                _trip = value; Notify();
            }
        }


        public ICommand Save
        {
            get
            {
                return GetCommand(() =>
                {
                    var members = Trip.Members;
                    Trip.Members = new List<Member>();

                    // Add selected members to trip
                    foreach (var m in members)
                    {
                        if (m.Id == -1)
                        {
                            //-1 is a blank spot => Do nothing
                        }
                        else if (m.Id == -2)
                        {
                            //-2 is a guest => Increment the crew count, but don't add the member to the member list
                            Trip.CrewCount++;
                        }
                        else
                        {
                            //Add the member reference and increment the crew count
                            Trip.Members.Add(m);
                            Trip.CrewCount++;
                        }
                    }

                    DbArkContext.GetDbContext().Trip.Add(Trip);
                    DbArkContext.GetDbContext().SaveChanges();

                    ProtocolSystem.UpdateNumBoatsOut();

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
            get { return HelperFunctions.SunsetClass.Sunset; }
        }
    }
}