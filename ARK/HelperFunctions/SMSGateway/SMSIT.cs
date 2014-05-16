using System.Net;

namespace ARK.HelperFunctions.SMSGateway
{
    public class SmsIt : ISmsGateway
    {
        #region Constants

        private const string UrlBase = "http://www.smsit.dk/api/sendSms.php";

        #endregion

        #region Constructors and Destructors

        public SmsIt(string apiKey)
            : this(apiKey, "UTF-8")
        {
        }

        public SmsIt(string apiKey, string charSet)
        {
            this.ApiKey = apiKey;
            this.CharSet = charSet;
        }

        #endregion

        #region Public Properties

        public string ApiKey { get; private set; }

        public string CharSet { get; private set; }

        #endregion

        #region Public Methods and Operators

        public bool SendSms(string sender, string reciever, string message)
        {
            string url = string.Format(
                "{0}?apiKey={1}&senderId={2}&mobile=45{3}&message={4}&charset={5}", 
                UrlBase, 
                this.ApiKey, 
                sender, 
                reciever, 
                message, 
                this.CharSet);

            var client = new WebClient();
            return client.DownloadString(url) == "0";
        }

        #endregion
    }
}