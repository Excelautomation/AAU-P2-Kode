namespace ARK.Model
{
    public class TripWarningSms
    {
        public int Id { get; set; }

        public Trip Trip { get; set; }
        public bool SentSms { get; set; }
        public bool RecievedSms { get; set; }
    }
}