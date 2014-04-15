using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.Model
{
    public class GetSMS
    {
        [Key]
        public int Id { get; set; }
        public string From { get; set; }
        public string Text { get; set; }
        public DateTime RecievedDate { get; set; }
        public bool Handled { get; set; }
    }
}
