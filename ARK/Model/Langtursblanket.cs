using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.Model
{
    public class Langtursblanket : Search.Searchable<Langtursblanket>
    {
        public int Id { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ReturnTime { get; set; }
        public string Text { get; set; }
        public virtual List<Person> Participant { get; set; }
        public bool Approved { get; set; }

        public override Langtursblanket getTarget()
        {
            return this;
        }
    }
}
