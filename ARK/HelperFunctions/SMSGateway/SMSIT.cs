using System.Net;

namespace ARK.HelperFunctions.SMSGateway
{
    public class SmsIt : ISmsGateway
    {
        private const string UrlBase = "http://www.smsit.dk/api/sendSms.php";

        public SmsIt(string apiKey)
            : this(apiKey, "UTF-8")
        {
        }

        public SmsIt(string apiKey, string charSet)
        {
            ApiKey = apiKey;
            CharSet = charSet;
        }

        public string ApiKey { get; private set; }

        public string CharSet { get; private set; }

        public bool SendSms(string sender, string reciever, string message)
        {
            string url = string.Format(
                "{0}?apiKey={1}&senderId={2}&mobile=45{3}&message={4}&charset={5}", 
                UrlBase, 
                ApiKey, 
                sender, 
                reciever, 
                message, 
                CharSet);

            var client = new WebClient();
            return client.DownloadString(url) == "0";
        }
    }
}