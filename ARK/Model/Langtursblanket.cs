using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.Administrationssystem.Model
{
    class Person
    {
        public string Navn { get; set; }
        public string Telefon { get; set; }
    }

    class Langtursblanket
    {
        public int Id { get; set; }
        public DateTime Afgangstid { get; set; }
        public DateTime Hjemkomsttid { get; set; }
        public string Text { get; set; }
        public virtual List<Person> Deltager { get; set; }
        public bool Godkendt { get; set; }
    }
}
