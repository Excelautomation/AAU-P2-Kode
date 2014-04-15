using ARK.Model.DB;
using ARK.Model.XML;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ARK.Model.XML
{
    public static class XMLParser
    {
        public static void LoadBoatsFromXml()
        {
            LoadFromXml<XMLBoats.dataroot, XMLBoats.datarootBådeSpecifik, Boat>(x => x.Boat, "/upload/monday/BådeSpecifik.xml");
        }

        public static void LoadMembersFromXml()
        {
            LoadFromXml<XMLMembers.dataroot, XMLMembers.datarootAktiveMedlemmer, Member>(x => x.Member, "/upload/monday/AktiveMedlemmer.xml");
        }

        public static void LoadTripsFromXml()
        {
            LoadFromXml<XMLTrips.dataroot, XMLTrips.datarootTur, Trip>(x => x.Trip, "/upload/monday/Tur.xml");
        }

        public static DateTime GetSunsetFromXml()
        {
            DateTime now = DateTime.Today;
            string basePath = "/sun/57.0488195/9.921746999999982";
            string basePathEnd = "/99/0";
            string day = now.Day.ToString();
            string month = now.Month.ToString();
            StringBuilder sb = new StringBuilder();
            sb.Append(basePath);
            sb.Append("/");
            sb.Append(day);
            sb.Append("/");
            sb.Append(month);
            sb.Append(basePathEnd);

            UriBuilder ub = new UriBuilder("http", "www.earthtools.org", 80, sb.ToString());
            NetworkCredential creds = new NetworkCredential("", "");

            string xml = DlToMem(ub.Uri, creds);

            XMLSunset.sun sunXml = ParseXML<XMLSunset.sun>(xml);
            DateTime sunset = DateTime.Today;
            sunset = sunset.Add(sunXml.evening.twilight.nautical.TimeOfDay);
            
            return sunset;

        }

        public static void LoadFromXml<TXml, TSubXml, TClass>(Func<DbArkContext, DbSet<TClass>> prop, string path)
            where TXml : class
            where TSubXml : class
            where TClass : class
        {
            using (DbArkContext dbContext = new DbArkContext())
            {
                FtpInfo ftpInfo = dbContext.FtpInfo.OrderByDescending(x => x.Id).First(x => true);
                IEnumerable<TClass> objects = XMLParser.GetObjectsFromXml<TXml, TSubXml, TClass>(ftpInfo, path);
                var coll = prop.Invoke(dbContext);
                coll.RemoveRange(coll);
                coll.AddRange(objects);
                dbContext.SaveChanges();
            }
        }

        public static IEnumerable<TResult> GetObjectsFromXml<T, TSub, TResult>(FtpInfo ftpInfo, string ftpPath)
            where T : class
            where TSub : class
            where TResult : class
        {
            UriBuilder ub = new UriBuilder("ftp", ftpInfo.HostName, ftpInfo.Port, ftpPath);
            NetworkCredential ftpCreds = new NetworkCredential(ftpInfo.Username, ftpInfo.Password);

            string xmlString = XMLParser.DlToMem(ub.Uri, ftpCreds);
            var xmlObject = XMLParser.ParseXML<T>(xmlString);

            List<PropertyInfo> tProps = new List<PropertyInfo>(typeof(T).GetProperties());
            PropertyInfo tProp = tProps.First(x => !x.Name.Contains("generated"));

            ConstructorInfo con = typeof(TResult).GetConstructor(new Type[] { typeof(TSub) });

            IEnumerable<TResult> result = ((IEnumerable<TSub>)tProp.GetValue(xmlObject)).Select(x => 
                (TResult)con.Invoke(new object[] {x})).ToList();

            return result;
        }

        public static T ParseXML<T>(string xml) where T : class
        {
            var reader = XmlReader.Create(new StringReader(xml), new XmlReaderSettings() { ConformanceLevel = ConformanceLevel.Auto });
            return new XmlSerializer(typeof(T)).Deserialize(reader) as T;
        }

        public static string DlToMem(Uri uri, NetworkCredential credentials)
        {
            WebRequest request = WebRequest.Create(uri);
            string retString;

            request.Credentials = credentials;
            ////request.UsePassive = true;
            ////request.UseBinary = true;
            ////request.KeepAlive = false;
            request.Timeout = 10000;

            using (WebResponse response = request.GetResponse())
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