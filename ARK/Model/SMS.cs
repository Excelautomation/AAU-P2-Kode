using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.Model
{
    public class SMS
    {
        [Key]
        public string Reciever { get; set; }
        public string Name { get; set; }
        public bool Dispatched { get; set; }
        public string Message { get; set; }
    }
}
