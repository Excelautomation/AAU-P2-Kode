using ARK.Model.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace ARK.Model.XML
{
    public static class XMLParser
    {
        public static void LoadBoatsFromXml()
        {
            using (DbArkContext context = new DbArkContext())
            {
                string ftpPath = @"/upload/monday/BådeSpecifik.xml";
                FtpInfo ftpInfo = context.FtpInfo.OrderByDescending(x => x.Id).First(x => true);
                UriBuilder ub = new UriBuilder("ftp", ftpInfo.HostName, ftpInfo.Port, ftpPath);
                NetworkCredential ftpCreds = new NetworkCredential(ftpInfo.Username, ftpInfo.Password);

                string xmlString = XMLParser.DlToMem(ub.Uri, ftpCreds);
                var xmlObject = XMLParser.ParseXML<XMLBoats.dataroot>(xmlString);

                List<Boat> boats = new List<Boat>();

                foreach (var boatXml in xmlObject.BådeSpecifik)
                {
                    Boat boat = new Boat()
                    {
                        Id = boatXml.ID,
                        Name = boatXml.Navn,
                        NumberofSeats = boatXml.AntalPladser,
                        Active = boatXml.Aktiv == 1,
                        SpecificBoatType = (Boat.BoatType)boatXml.SpecifikBådType,
                        Usable = boatXml.Roforbud == 1,
                        LongTripBoat = boatXml.LangtursBåd == 1,
                        DamageForms = new List<DamageForm>(),
                        LongDistanceForms = new LinkedList<LongDistanceForm>()
                    };

                    boats.Add(boat);
                }

                context.Boat.AddRange(boats);
                context.SaveChanges();
            }
        }

        public static void LoadMembersFromXml()
        {
            using (DbArkContext context = new DbArkContext())
            {
                string ftpPath = @"/upload/monday/AktiveMedlemmer.xml";
                FtpInfo ftpInfo = context.FtpInfo.OrderByDescending(x => x.Id).First(x => true);
                UriBuilder ub = new UriBuilder("ftp", ftpInfo.HostName, ftpInfo.Port, ftpPath);
                NetworkCredential ftpCreds = new NetworkCredential(ftpInfo.Username, ftpInfo.Password);

                string xmlString = XMLParser.DlToMem(ub.Uri, ftpCreds);
                var xmlObject = XMLParser.ParseXML<XMLMembers.dataroot>(xmlString);

                List<Member> members = new List<Member>();

                foreach (var memberXml in xmlObject.activeMembers)
                {
                    Member member = new Member()
                    {
                        MemberNumber = Convert.ToInt32(memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.MedlemsNr)),
                        Id = Convert.ToInt32(memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.ID)),
                        FirstName = (string)memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.Fornavn),
                        LastName = (string)memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.Efternavn),
                        Address1 = (string)memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.Adresse1),
                        Address2 = (string)memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.Adresse2),
                        ZipCode = Convert.ToInt32(memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.PostNr)),
                        City = (string)memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.By),
                        Phone = new Func<string, string>(x => x != null ? Regex.Replace(x, @"[^0-9]", "") : "")
                            .Invoke((string)memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.Telefon)),
                        PhoneWork = new Func<string, string>(x => x != null ? Regex.Replace(x, @"[^0-9]", "") : "")
                            .Invoke((string)memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.TelefonArbejde)),
                        Cellphone = new Func<string, string>(x => x != null ? Regex.Replace(x, @"[^0-9]", "") : "")
                            .Invoke((string)memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.TelefonMobil)),
                        Email1 = (string)memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.EMail),
                        Email2 = (string)memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.EMail2),
                        Birthday = Convert.ToDateTime(memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.Fødselsdato)),
                        Released = Convert.ToInt32(memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.Frigivet)) == 1,
                        SwimmingTest = Convert.ToInt32(memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.Svømmeprøve)) == 1,
                        ShortTripCox = Convert.ToInt32(memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.Korttursstyrmand)) == 1,
                        LongTripCox = Convert.ToInt32(memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.Langtursstyrmand)) == 1,
                        MayUseSculler = Convert.ToInt32(memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.Scullerret)) == 1,
                        MayUseOutrigger = Convert.ToInt32(memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.Outriggerret)) == 1,
                        MayUseKayak = Convert.ToInt32(memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.Kajakret)) == 1,
                        Trips = new List<Trip>(),
                        LongDistanceForms = new List<LongDistanceForm>(),
                        DamageForms = new List<DamageForm>()
                    };

                    members.Add(member);
                }

                context.Member.AddRange(members);
                context.SaveChanges();
            }
        }

        public static void LoadTripsFromXml()
        {
            string ftpPath = @"/upload/monday/Tur.xml";
            using (DbArkContext context = new DbArkContext())
            {
                FtpInfo ftpInfo = context.FtpInfo.OrderByDescending(x => x.Id).First(x => true);
                UriBuilder ub = new UriBuilder("ftp", ftpInfo.HostName, ftpInfo.Port, ftpPath);
                NetworkCredential ftpCreds = new NetworkCredential(ftpInfo.Username, ftpInfo.Password);

                string xmlString = XMLParser.DlToMem(ub.Uri, ftpCreds);
                var xmlObject = XMLParser.ParseXML<XMLTrips.dataroot>(xmlString);

                List<Trip> trips = new List<Trip>();

                IEnumerable<PropertyInfo> props = new List<PropertyInfo>(typeof(XMLTrips.datarootTur).GetProperties());
                IEnumerable<PropertyInfo> filteredProps = props.Where(x => Regex.IsMatch(x.Name, @"Nr\dSpecified"));

                foreach (var tripXml in xmlObject.Tur)
                {
                    Trip trip = new Trip();

                    trip.Id = tripXml.ID;
                    trip.Distance = tripXml.Kilometer;
                    trip.Date = tripXml.Dato;
                    trip.LongTrip = tripXml.Langtur == 1;
                    trip.BoatId = tripXml.BådID;
                    trip.Members = new List<Member>();

                    trip.Members.Add(context.Member.Find((int)tripXml.Nr1));
                    foreach (PropertyInfo prop in filteredProps)
                    {
                        if ((bool)prop.GetValue(tripXml))
                        {
                            PropertyInfo elementProp = props.First(x => Regex.IsMatch(prop.Name, x.Name) && prop.Name != x.Name);
                            trip.Members.Add(context.Member.Find(Convert.ToInt32(elementProp.GetValue(tripXml))));
                        }
                    }

                    trips.Add(trip);
                }

                context.Trip.AddRange(trips);
                context.SaveChanges();
            }
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
            request.Timeout = 20000;

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