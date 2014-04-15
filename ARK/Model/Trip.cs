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
        public Trip()
        {
        }

        public Trip(XMLTrips.datarootTur tripXml)
        {
            using (DB.DbArkContext context = new DB.DbArkContext())
            {
                TripId = tripXml.ID;
                Distance = tripXml.Kilometer;
                Date = tripXml.Dato;
                LongTrip = tripXml.Langtur == 1 ? true : false;
                BoatId = tripXml.BådID;
                Members = new List<Member>();

                IEnumerable<PropertyInfo> props = new List<PropertyInfo>(tripXml.GetType().GetProperties());
                IEnumerable<PropertyInfo> filteredProps = props.Where(x => Regex.IsMatch(x.Name, @"Nr\dSpecified"));

                foreach (PropertyInfo prop in filteredProps)
                {
                    if ((bool)prop.GetValue(tripXml))
                    {
                        PropertyInfo elementProp = props.First(x => Regex.IsMatch(prop.Name, x.Name) && prop.Name != x.Name);
                        Member memberRef = context.Member.Find(Convert.ToInt32(elementProp.GetValue(tripXml)));
                        Members.Add(memberRef);
                    }
                }

                Boat = context.Boat.Find(BoatId);

                context.SaveChanges();
            }
        }

        [Key]
        public int TripId { get; set; }
        public int Distance { get; set; }
        public DateTime Date { get; set; }
        public bool LongTrip { get; set; }
        public int BoatId { get; set; }
        public virtual Boat Boat { get; set; }
        public virtual ICollection<Member> Members { get; set; }
    }
}
