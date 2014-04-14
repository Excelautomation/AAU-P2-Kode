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

        public Trip(XMLTrips.datarootTur tripXML)
        {
            ID = tripXML.ID;
            Distance = tripXML.Kilometer;
            Date = tripXML.Dato;
            LongTrip = tripXML.Langtur == 1 ? true : false;
            BoatID = tripXML.BådID;

            Type type = tripXML.GetType();
            IEnumerable<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
            IEnumerable<PropertyInfo> filteredProps = props.Where(x => Regex.IsMatch(x.Name, @"nr\d") && x.PropertyType == typeof(bool));

            using (DB.DbArkContext dbContext = new DB.DbArkContext())
            {
                foreach (PropertyInfo prop in filteredProps)
                {
                    if ((bool)prop.GetValue(tripXML))
                    {
                        PropertyInfo elementProp = props.First(x => Regex.IsMatch(prop.Name, x.Name) && prop.Name != x.Name);
                        Member memberRef = dbContext.Member.Find((int)elementProp.GetValue(tripXML));
                        MembersOnTrip.Add(memberRef);
                    }
                }
            }
            
        }

        [Key]
        public int ID { get; set; }
        public int Distance { get; set; }
        public DateTime Date { get; set; }
        public bool LongTrip { get; set; }
        public int BoatID { get; set; }
        public List<Member> MembersOnTrip { get; set; }
    }
}
