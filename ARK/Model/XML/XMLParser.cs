using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ARK.Model.XML
{
    public static class XMLParser
    {
        public static T ParseXML<T>(string xml) where T : class
        {
            var reader = XmlReader.Create(new StringReader(xml), new XmlReaderSettings() { ConformanceLevel = ConformanceLevel.Auto });
            return new XmlSerializer(typeof(T)).Deserialize(reader) as T;
        }

        public static string DlToMemFromFtp(Uri uri, NetworkCredential credentials)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
            string retString;

            request.Credentials = credentials;

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        retString = reader.ReadToEnd();
                    }
                }
            }

            return retString;
        }
    }
}