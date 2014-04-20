using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARK.Model.XML;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ARK.Model
{
    public class Trip
    {
        public int Id { get; set; }
        public int Distance { get; set; }
        public DateTime Date { get; set; }
        public bool LongTrip { get; set; }

        //Foreign key
        public int BoatId { get; set; }

        //Navigation properties
        public virtual Boat Boat { get; set; }
        public virtual ICollection<Member> Members { get; set; }
    }
}
