using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Interfaces;
using ARK.ViewModel.Protokolsystem.Data;

namespace ARK.ViewModel.Protokolsystem.Additional
{
    public class DistanceStatisticsAdditionalInfoViewModel : ContentViewModelBase
    {
        #region Fields

        private double _kmErgometer;

        private double _kmGig;

        private double _kmInrigger;

        private double _kmKajak;

        private double _kmOutrigger;

        private Boat _mostUsedBoat;

        private MemberDistanceViewModel _selectedMember;

        private TripViewModel _selectedTrip;

        #endregion

        #region Public Properties

        public double KmErgometer
        {
            get
            {
                return this._kmErgometer;
            }

            private set
            {
                this._kmErgometer = value;
                this.Notify();
            }
        }

        public double KmGig
        {
            get
            {
                return this._kmGig;
            }

            private set
            {
                this._kmGig = value;
                this.Notify();
            }
        }

        public double KmInrigger
        {
            get
            {
                return this._kmInrigger;
            }

            private set
            {
                this._kmInrigger = value;
                this.Notify();
            }
        }

        public double KmKajak
        {
            get
            {
                return this._kmKajak;
            }

            private set
            {
                this._kmKajak = value;
                this.Notify();
            }
        }

        public double KmOutrigger
        {
            get
            {
                return this._kmOutrigger;
            }

            private set
            {
                this._kmOutrigger = value;
                this.Notify();
            }
        }

        public Boat MostUsedBoat
        {
            get
            {
                return this._mostUsedBoat;
            }

            private set
            {
                this._mostUsedBoat = value;
                this.Notify();
            }
        }

        public MemberDistanceViewModel SelectedMember
        {
            get
            {
                return this._selectedMember;
            }

            set
            {
                this._selectedMember = value;
                this.Notify();

                if (value == null)
                {
                    this.MostUsedBoat = null;
                    this.KmOutrigger = 0;
                    this.KmInrigger = 0;
                    this.KmKajak = 0;
                    this.KmErgometer = 0;
                    this.KmGig = 0;
                    return;
                }

                var boats = this._selectedMember.FilteredTrips.Select(trip => trip.Trip.Boat).ToList();

                this.MostUsedBoat =
                    boats.Distinct().OrderByDescending(boat => boats.Count(b => b.Id == boat.Id)).FirstOrDefault();

                this.KmOutrigger =
                    this._selectedMember.FilteredTrips.Where(
                        trip => trip.Trip.Boat.SpecificBoatType == Boat.BoatType.Outrigger)
                        .Sum(trip => trip.Trip.Distance);

                this.KmInrigger =
                    this._selectedMember.FilteredTrips.Where(
                        trip => trip.Trip.Boat.SpecificBoatType == Boat.BoatType.Inrigger)
                        .Sum(trip => trip.Trip.Distance);

                this.KmKajak =
                    this._selectedMember.FilteredTrips.Where(
                        trip => trip.Trip.Boat.SpecificBoatType == Boat.BoatType.Kajak).Sum(trip => trip.Trip.Distance);

                this.KmErgometer =
                    this._selectedMember.FilteredTrips.Where(
                        trip => trip.Trip.Boat.SpecificBoatType == Boat.BoatType.Ergometer)
                        .Sum(trip => trip.Trip.Distance);

                this.KmGig =
                    this._selectedMember.FilteredTrips.Where(
                        trip => trip.Trip.Boat.SpecificBoatType == Boat.BoatType.Gig).Sum(trip => trip.Trip.Distance);
            }
        }

        public TripViewModel SelectedTrip
        {
            get
            {
                return this._selectedTrip;
            }

            set
            {
                this._selectedTrip = value;
                this.Notify();
            }
        }

        #endregion
    }
}