using System;
using System.ComponentModel.DataAnnotations;

namespace ARK.Model
{
    public class SMS
    {
        [Key]
        public string Reciever { get; set; }

        public string Name { get; set; }
        public bool Dispatched { get; set; }
        public string Message { get; set; }
        public bool approved { get; set; }
        public DateTime Time { get; set; }
        public bool Handled { get; set; }
    }
}