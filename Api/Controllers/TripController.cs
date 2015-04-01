using ARK.Model;
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
        public IEnumerable<Trip> Get()
        {
            
            return null;
        }

        // GET: api/Trip/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Trip
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Trip/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Trip/5
        public void Delete(int id)
        {
        }
    }
}
