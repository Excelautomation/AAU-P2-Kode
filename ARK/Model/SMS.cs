using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.Model
{
    public class SMS
    {
        public string Reciever { get; set; }
        public string Name { get; set; }
        public DateTime DispatchTime { get; set; }
        public bool Dispatched { get; set; }
        public string Message { get; set; }
    }
}
