using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.Model
{
    public class Person : Search.Searchable<Person>
    {
        public string Navn { get; set; }
        public string Telefon { get; set; }

        public override Person getTarget()
        {
            return this;
        }
    }
}
