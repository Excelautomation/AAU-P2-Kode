using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.Model
{
    public class GetSMS
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string Text { get; set; }
        public DateTime RecievedDate { get; set; }
    }
}
