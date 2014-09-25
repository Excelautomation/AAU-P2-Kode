using System.Net;

namespace ARK.HelperFunctions.SMSGateway
{
    public class SmsIt : ISmsGateway
    {
        private const string UrlBase = "http://sms.stadel.dk/send.php?user=mclc&pass=y433zipx";

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
                "{0}&message={1}&mobile=45{2}",
                UrlBase,  
                message,
                reciever);

            var client = new WebClient();
            return client.DownloadString(url) == "0";
        }
    }
}