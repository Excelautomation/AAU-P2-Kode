using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Net;


namespace ARK.Model.XML
{
    public static class XMLParser
    {
        public static T ParseXML<T>(string xml) where T : class
        {
            var reader = XmlReader.Create(new StringReader(xml), new XmlReaderSettings() { ConformanceLevel = ConformanceLevel.Auto });
            return new XmlSerializer(typeof(T)).Deserialize(reader) as T;
        }

        public static string DlToMemFromFTP(string serverAddress, string path, NetworkCredential credentials)
        {
            string filePath = String.Concat(serverAddress, path);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(filePath);
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

        public static string GetPathFromDay(string filename)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("/upload/");
            sb.Append(DateTime.Today.DayOfWeek.ToString());
            sb.Append(filename);
            return sb.ToString();
        }
    }
}

