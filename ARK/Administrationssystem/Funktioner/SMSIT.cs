using System.Net;
using ARK.Model;

namespace ARK.Administrationssystem.Funktioner
{
    public class SMSIT
    {
        private static string urlBase;

        public static bool SendSMS(SMS smsInformation)
        {
            urlBase = "http://www.smsit.dk/api/sendSms.php";
            WebClient client = new WebClient();
            string apikey = "0000000000000000";
            string senderid = "22821674";
            string charset = "UTF-8";
            string url = string.Format("{0}?apiKey={1}&senderId={2}&mobile={3}&message={4}&charset={5}", urlBase, apikey, senderid, smsInformation.Reciever, smsInformation.Message, charset);
            return client.DownloadString(url) == "0";
        }
    }
}