using System.Net;
using ARK.Model;

namespace ARK.View.Administrationssystem.Functions
{
    public class SMSIT
    {
        public static bool SendSMS(SMS smsInformation)
        {
            const string urlBase = "http://www.smsit.dk/api/sendSms.php";

            var client = new WebClient();
            const string apikey = "0000000000000000";
            const string senderid = "22821674";
            const string charset = "UTF-8";
            string url = string.Format("{0}?apiKey={1}&senderId={2}&mobile={3}&message={4}&charset={5}", urlBase, apikey, senderid, smsInformation.Reciever, smsInformation.Message, charset);
            return client.DownloadString(url) == "0";
        }
    }
}