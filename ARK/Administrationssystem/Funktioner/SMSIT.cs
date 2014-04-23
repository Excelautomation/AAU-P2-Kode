using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ARK.Administrationssystem.Funktioner
{
    public class SMSIT
    {
        private static string urlBase;

        public static bool SendSMS(Model.SMS smsInformation)
        {
            urlBase = "http://www.smsit.dk/api/sendSms.php";
            WebClient client = new WebClient();
            var apikey = "0000000000000000";
            var senderid = "22821674";
            var charset = "UTF-8";
            var url = string.Format("{0}?apiKey={1}&senderId={2}&mobile={3}&message={4}&charset={5}", urlBase, apikey, senderid, smsInformation.Reciever, smsInformation.Message, charset);
            return client.DownloadString(url) == "0";
        }
    }
}