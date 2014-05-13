using System.Linq;
using System.Windows.Input;
using ARK.Model;
using ARK.ViewModel.Base;
using ARK.ViewModel.Base.Interfaces;
using System.Collections.ObjectModel;
using ARK.ViewModel.Protokolsystem.Data;

namespace ARK.ViewModel.Protokolsystem.Additional
{
    public class DistanceStatisticsAdditionalInfoViewModel : ContentViewModelBase
    {
        private MemberDistanceViewModel _selectedMember;
        private TripViewModel _selectedTrip;
        private Boat _mostUsedBoat;
        private double _kmOutrigger;
        private double _kmInrigger;
        private double _kmKajak;
        private double _kmErgometer;
        private double _kmGig;
        // Fields
        public MemberDistanceViewModel SelectedMember
        {
            get { return _selectedMember; }
            set
            {
                _selectedMember = value;
                Notify();

                if (value == null)
                {
                    MostUsedBoat = null;
                    KmOutrigger = 0;
                    KmInrigger = 0;
                    KmKajak = 0;
                    KmErgometer = 0;
                    KmGig = 0;
                    return;
                }

                var boats = _selectedMember.FilteredTrips
                    .Select(trip => trip.Trip.Boat)
                    .ToList();

                MostUsedBoat = boats
                    .Distinct()
                    .OrderByDescending(boat => boats.Count(b => b.Id == boat.Id))
                    .FirstOrDefault();


                KmOutrigger = _selectedMember.FilteredTrips
                    .Where(trip => trip.Trip.Boat.SpecificBoatType == Boat.BoatType.Outrigger)
                    .Sum(trip => trip.Trip.Distance);

                KmInrigger = _selectedMember.FilteredTrips
                    .Where(trip => trip.Trip.Boat.SpecificBoatType == Boat.BoatType.Inrigger)
                    .Sum(trip => trip.Trip.Distance);

                KmKajak = _selectedMember.FilteredTrips
                    .Where(trip => trip.Trip.Boat.SpecificBoatType == Boat.BoatType.Kajak)
                    .Sum(trip => trip.Trip.Distance);

                KmErgometer = _selectedMember.FilteredTrips
                    .Where(trip => trip.Trip.Boat.SpecificBoatType == Boat.BoatType.Ergometer)
                    .Sum(trip => trip.Trip.Distance);

                KmGig = _selectedMember.FilteredTrips
                    .Where(trip => trip.Trip.Boat.SpecificBoatType == Boat.BoatType.Gig)
                    .Sum(trip => trip.Trip.Distance);
            }
        }

        public TripViewModel SelectedTrip
        {
            get { return _selectedTrip; }
            set
            {
                _selectedTrip = value;
                Notify();
            }
        }

        public Boat MostUsedBoat
        {
            get { return _mostUsedBoat; }
            private set
            {
                _mostUsedBoat = value;
                Notify();
            }
        }

        public double KmOutrigger
        {
            get { return _kmOutrigger; }
            private set
            {
                _kmOutrigger = value;
                Notify();
            }
        }

        public double KmInrigger
        {
            get { return _kmInrigger; }
            private set
            {
                _kmInrigger = value;
                Notify();
            }
        }

        public double KmKajak
        {
            get { return _kmKajak; }
            private set
            {
                _kmKajak = value;
                Notify();
            }
        }

        public double KmErgometer
        {
            get { return _kmErgometer; }
            private set
            {
                _kmErgometer = value;
                Notify();
            }
        }

        public double KmGig
        {
            get { return _kmGig; }
            private set
            {
                _kmGig = value;
                Notify();
            }
        }
    }
}