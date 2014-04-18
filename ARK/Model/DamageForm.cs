using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.Model
{
    public class DamageForm
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public int RegisteringMemberId { get; set; }
        public virtual Member RegisteringMember { get; set; }

        public int BoatId { get; set; }
        public virtual Boat Boat { get; set; }

        public virtual DamageDescription DamageDescription { get; set; }
    }
}
