using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARK.Model;
using ARK.Model.DB;
using ARK.Model.XML;

namespace ARK.HelperFunctions.SMSGateway
{
    public class SmsWarnings
    {
        public ISmsGateway Gateway { get; set; }

        public void Test()
        {
            using (var db = new DbArkContext())
            {
                var warnings = GetTripWarningSms(db).ToList();
            }
        }
        private IEnumerable<TripWarningSms> GetTripWarningSms(DbArkContext db)
        {
            // Find all trips which dosn't have a tripwarningsms yet - select tripWarningSms and add to db
            IEnumerable<TripWarningSms> missingTrips = (from trip in db.Trip
                where trip.TripEndedTime == null &&
                        !trip.LongTrip
                select trip).ToList()
            .Where(trip => !db.TripWarningSms.Any(warning => warning.Trip.Id == trip.Id))
            .Select(trip => new TripWarningSms {Trip = trip})
            .ToList();

            // Remove trips home
            IEnumerable<TripWarningSms> endedTrips = db.TripWarningSms
                .Where(warning => warning.Trip.TripEndedTime != null)
                .ToList();

            // Update database
            if (missingTrips.Any())
                db.TripWarningSms.AddRange(missingTrips);

            if (endedTrips.Any())
                db.TripWarningSms.RemoveRange(endedTrips);

            // Save
            if (missingTrips.Any() || endedTrips.Any())
                db.SaveChanges();

            return db.TripWarningSms
                .Where(warning => warning.RecievedSms == null)
                .Include(warning => warning.Trip);
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
