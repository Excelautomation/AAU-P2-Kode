using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Net;


namespace ARK.Model
{
    public partial class XMLParseHelpers
    {
        public static class XMLParser
        {
            public static T ParseXML<T>(string xml) where T : class
            {
                var reader = XmlReader.Create(new StringReader(xml), new XmlReaderSettings() { ConformanceLevel = ConformanceLevel.Auto });
                return new XmlSerializer(typeof(T)).Deserialize(reader) as T;
            }

            public static byte[] DlToMemFromFTP(string serverAddress, string path, NetworkCredential credentials)
            {
                string filePath = Path.Combine(serverAddress, path);
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(filePath);
                byte[] retArray = new byte[2];

                request.Credentials = credentials;

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (MemoryStream memStream = new MemoryStream())
                        {
                            using (StreamWriter writer = new StreamWriter(memStream))
                            {
                                
                            }
                        }
                    }
                }
                return retArray;
            }
        }
    }
}
