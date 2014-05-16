using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using ARK.Model;
using ARK.Model.DB;
using ARK.View.Protokolsystem.Pages;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Interfaces;

namespace ARK.ViewModel.Protokolsystem.Confirmations
{
    public class BeginTripBoatsConfirmViewModel : ConfirmationViewModelBase
    {
        // Fields
        #region Fields

        private Trip _trip;

        #endregion

        #region Constructors and Destructors

        public BeginTripBoatsConfirmViewModel()
        {
        }

        #endregion

        #region Public Properties

        public ICommand CancelTrip
        {
            get
            {
                return this.GetCommand(
                    () =>
                        {
                            this.Hide();
                            this.ProtocolSystem.StatisticsDistance.Execute(null);
                        });
            }
        }

        public ICommand ChangeTrip
        {
            get
            {
                return this.GetCommand(this.Hide);
            }
        }

        public ICommand Save
        {
            get
            {
                return this.GetCommand(
                    () =>
                        {
                            var members = this.Trip.Members;
                            this.Trip.Members = new List<Member>();

                            // Add selected members to trip
                            foreach (var m in members)
                            {
                                if (m.Id == -1)
                                {
                                    // -1 is a blank spot => Do nothing
                                }
                                else if (m.Id == -2)
                                {
                                    // -2 is a guest => Increment the crew count, but don't add the member to the member list
                                    this.Trip.CrewCount++;
                                }
                                else
                                {
                                    // Add the member reference and increment the crew count
                                    this.Trip.Members.Add(m);
                                    this.Trip.CrewCount++;
                                }
                            }

                            DbArkContext.GetDbContext().Trip.Add(this.Trip);
                            DbArkContext.GetDbContext().SaveChanges();

                            this.ProtocolSystem.UpdateNumBoatsOut();

                            this.Hide();
                            this.ProtocolSystem.StatisticsDistance.Execute(null);
                        });
            }
        }

        public DateTime Sunset
        {
            get
            {
                return HelperFunctions.SunsetClass.Sunset;
            }
        }

        public Trip Trip
        {
            get
            {
                return this._trip;
            }

            set
            {
                this._trip = value;
                this.Notify();
            }
        }

        #endregion
    }
}