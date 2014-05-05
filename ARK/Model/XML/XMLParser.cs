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
using ARK.Model.DB;

namespace ARK.Model.XML
{
    public static class XMLParser
    {
        public static void UpdateDataFromFtp()
        {
            var context = DbArkContext.GetDbContext();

            //Boats
            var xmlString = DownloadLatestFromFtp(@"BådeSpecifik.xml");
            var xmlObject = ParseXML<XMLBoats.dataroot>(xmlString);

            foreach (var boatXml in xmlObject.BådeSpecifik)
            {
                Boat boat;
                if ((boat = context.Boat.Find(boatXml.ID)) != null)
                {
                    
                }
                else
                {
                    
                }
            }

            //Members
        }

        private static void BoatXmlToModel(Boat boat, XMLBoats.datarootBådeSpecifik boatXml)
        {
            boat.Id = boatXml.ID;
            boat.Name = boatXml.Navn;
            boat.NumberofSeats = boatXml.AntalPladser;
            boat.Active = boatXml.Aktiv == 1;
            boat.SpecificBoatType = (Boat.BoatType)boatXml.SpecifikBådType;
            boat.LongTripBoat = boatXml.LangtursBåd == 1;
        }

        public static void LoadBoatsFromXml()
        {
            var context = DbArkContext.GetDbContext();

            var xmlString = DownloadLatestFromFtp(@"BådeSpecifik.xml");
            var xmlObject = ParseXML<XMLBoats.dataroot>(xmlString);

            foreach (XMLBoats.datarootBådeSpecifik boatXml in xmlObject.BådeSpecifik)
            {
                var boat = new Boat
                {
                    DamageForms = new List<DamageForm>(),
                    LongDistanceForms = new LinkedList<LongDistanceForm>()
                };
                BoatXmlToModel(boat, boatXml);

                context.Boat.Add(boat);
            }

            context.SaveChanges();
        }


        private static void MemberXmlToModel(Member member, XMLMembers.datarootAktiveMedlemmer memberXml)
        {
            member.MemberNumber = Convert.ToInt32(memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.MedlemsNr));
            member.Id = Convert.ToInt32(memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.ID));
            member.FirstName = (string)memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.Fornavn);
            member.LastName = (string)memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.Efternavn);
            member.Address1 = (string)memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.Adresse1);
            member.Address2 = (string)memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.Adresse2);
            member.ZipCode = Convert.ToInt32(memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.PostNr));
            member.City = (string)memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.By);
            member.Phone = new Func<string, string>(x => x != null ? Regex.Replace(x, @"[^0-9]", string.Empty) : string.Empty)
                .Invoke((string)memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.Telefon));
            member.PhoneWork = new Func<string, string>(x => x != null ? Regex.Replace(x, @"[^0-9]", string.Empty) : string.Empty)
                .Invoke((string)memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.TelefonArbejde));
            member.Cellphone = new Func<string, string>(x => x != null ? Regex.Replace(x, @"[^0-9]", string.Empty) : string.Empty)
                .Invoke((string)memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.TelefonMobil));
            member.Email1 = (string)memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.EMail);
            member.Email2 = (string)memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.EMail2);
            member.Birthday = Convert.ToDateTime(memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.Fødselsdato));
            member.Released = Convert.ToInt32(memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.Frigivet)) == 1;
            member.SwimmingTest = Convert.ToInt32(memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.Svømmeprøve)) == 1;
            member.ShortTripCox = Convert.ToInt32(memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.Korttursstyrmand)) == 1;
            member.LongTripCox = Convert.ToInt32(memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.Langtursstyrmand)) == 1;
            member.MayUseSculler = Convert.ToInt32(memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.Scullerret)) == 1;
            member.MayUseOutrigger = Convert.ToInt32(memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.Outriggerret)) == 1;
            member.MayUseKayak = Convert.ToInt32(memberXml.GetObjFromName(XMLMembers.ItemsChoiceType.Kajakret)) == 1;
        }

        public static void LoadMembersFromXml()
        {
            var context = DbArkContext.GetDbContext();

            var xmlString = DownloadLatestFromFtp(@"AktiveMedlemmer.xml");
            var xmlObject = ParseXML<XMLMembers.dataroot>(xmlString);

            foreach (XMLMembers.datarootAktiveMedlemmer memberXml in xmlObject.activeMembers)
            {
                var member = new Member
                {
                    Trips = new List<Trip>(),
                    LongDistanceForms = new List<LongDistanceForm>(),
                    DamageForms = new List<DamageForm>()
                };
                MemberXmlToModel(member, memberXml);

                context.Member.Add(member);
            }

            context.SaveChanges();
        }

        public static void LoadTripsFromXml()
        {
            var context = DbArkContext.GetDbContext();

            var xmlString = DownloadLatestFromFtp(@"Tur.xml");
            var xmlObject = ParseXML<XMLTrips.dataroot>(xmlString);

            IEnumerable<PropertyInfo> props = new List<PropertyInfo>(typeof(XMLTrips.datarootTur).GetProperties());
            var filteredProps = props.Where(x => Regex.IsMatch(x.Name, @"Nr\dSpecified"));

            foreach (XMLTrips.datarootTur tripXml in xmlObject.Tur)
            {
                var trip = new Trip();

                trip.Id = tripXml.ID;
                trip.Distance = tripXml.Kilometer;
                trip.TripStartTime = tripXml.Dato;
                trip.TripEndedTime = tripXml.Dato;
                trip.LongTrip = tripXml.Langtur == 1;
                trip.BoatId = tripXml.BådID;
                trip.Members = new List<Member> { context.Member.Find((int)tripXml.Nr1) };

                foreach (PropertyInfo prop in filteredProps)
                {
                    if ((bool)prop.GetValue(tripXml))
                    {
                        PropertyInfo elementProp = props.First(x => Regex.IsMatch(prop.Name, x.Name) && prop.Name != x.Name);
                        trip.Members.Add(context.Member.Find(Convert.ToInt32(elementProp.GetValue(tripXml))));
                    }
                }

                context.Trip.Add(trip);
            }

            context.SaveChanges();
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
            NetworkCredential creds = new NetworkCredential(string.Empty, string.Empty);

            string xml = DlToMem(ub.Uri, creds);

            XMLSunset.sun sunXml = ParseXML<XMLSunset.sun>(xml);
            DateTime sunset = DateTime.Today;
            sunset = sunset.Add(sunXml.evening.twilight.nautical.TimeOfDay);

            return sunset;
        }

        private static T ParseXML<T>(string xml) where T : class
        {
            var reader = XmlReader.Create(new StringReader(xml), new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Auto });
            return new XmlSerializer(typeof(T)).Deserialize(reader) as T;
        }

        private static string DownloadLatestFromFtp(string filename)
        {
            var context = DbArkContext.GetDbContext();
            var ftpInfo = context.FtpInfo.OrderByDescending(x => x.Id).First(x => true);
            var ftpCreds = new NetworkCredential(ftpInfo.Username, ftpInfo.Password);

            var weekDays = new string[]
            {
                "monday",
                "tuesday",
                "wednesday",
                "thursday",
                "friday",
                "saturday",
                "sunday"
            };

            var latestFolder = string.Empty;
            var latestDateTime = new DateTime();
            foreach (var weekDay in weekDays)
            {
                var ub = new UriBuilder("ftp", ftpInfo.HostName, ftpInfo.Port,
                    @"/upload" + "/" + weekDay + "/" + filename);

                var request = WebRequest.Create(ub.Uri) as FtpWebRequest;

                request.Credentials = ftpCreds;
                request.KeepAlive = true;
                request.UsePassive = true;

                request.Method = WebRequestMethods.Ftp.GetDateTimestamp;

                using (var response = request.GetResponse() as FtpWebResponse)
                {
                    if (response.LastModified > latestDateTime)
                    {
                        latestDateTime = response.LastModified;
                        latestFolder = weekDay;
                    }
                }
            }

            var uriBuilder = new UriBuilder("ftp", ftpInfo.HostName, ftpInfo.Port,
                @"/upload" + @"/" + latestFolder + @"/" + filename);

            return DlToMem(uriBuilder.Uri, ftpCreds);
        }

        private static string DlToMem(Uri uri, NetworkCredential credentials)
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