﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.Model
{
    public class LongTripForm
    {
        [Key]
        public int ID { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public string Text { get; set; }
        public virtual List<Person> Persons { get; set; }
        public bool Approved { get; set; }
    }
}
