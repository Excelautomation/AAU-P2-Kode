using ARK.Model;
using ARK.Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Controllers
{
    public class TripController : ApiController
    {
        // GET: api/Trip
        public IEnumerable<Trip> GetAll()
        {
            using (var db = new DbArkContext())
            {
                return db.Trip.Where(trip => true);
            }
        }

        // GET: api/Trip/5
        public Trip Get(int id)
        {
            using (var db = new DbArkContext())
            {
                return db.Trip.Single(trip => trip.Id == id);
            }
        }

        // POST: api/Trip
        [HttpPost]
        public bool Edit([FromBody]string value)
        {
            return false;
        }

        // PUT: api/Trip/5
        [HttpPut]
        public bool CreateNewTrip(Trip trip, [FromBody]string value)
        {
            return false;
        }

        // PUT: api/Trip/5
        [HttpPut]
        public bool EndTrip(Trip trip, [FromBody]string value)
        {
            // verify that trip contain valid data.

            // end trip
            return false;
        }
    }
}
