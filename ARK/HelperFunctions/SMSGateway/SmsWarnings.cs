using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARK.Model;
using ARK.Model.DB;
using ARK.Model.XML;

namespace ARK.HelperFunctions.SMSGateway
{
    class SmsWarnings
    {
        public ISmsGateway Gateway { get; set; }


        private TripWarningSms[] GetTripWarningSms()
        {
            IEnumerable<TripWarningSms> warningSMS;
            IEnumerable<Trip> trips = GetCurrentTrips();

            using (var db = new DbArkContext())
            {
                warningSMS = db.TripWarningSms
                    .Where(warning => trips.Any(trip => trip.Id == warning.Trip.Id)).AsEnumerable();
            }

            throw new NotImplementedException();
        }

        private IEnumerable<Trip> GetCurrentTrips()
        {
            IEnumerable<Trip> output;

            using (var db = new DbArkContext())
            {
                output = db.Trip
                    .Where(trip => 
                        trip.TripEndedTime == null 
                            && !trip.LongTrip)
                    .ToList();
            }

            return output;
        }

        private bool IsAfterSunset()
        {
            return XmlParser.GetSunsetFromXml() > DateTime.Now;
        }

        
    }
}
