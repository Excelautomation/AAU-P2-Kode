using System;

namespace ARK.Model
{
    public class GetSMS
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string Text { get; set; }
        public DateTime RecievedDate { get; set; }
        public bool Handled { get; set; }
    }
}