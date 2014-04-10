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
        [Key]
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string ReportedBy { get; set; }
        public int ReportedByNumber { get; set; }
        public Boat DamagedBoat { get; set; }
        public DamageDescription Damage { get; set; }
    }
}
