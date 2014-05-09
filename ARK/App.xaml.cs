using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using ARK.Model;
using ARK.Model.DB;
using ARK.Model.XML;
using System.Globalization;
using ARK.View.Administrationssystem.Functions;

namespace ARK
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // Thread that checks if a new season needs to be started
            var checkForNewSeasonThread = new Thread(() =>
            {
                DateTime tomorrowAtTree = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 3, 00, 00);

                tomorrowAtTree = tomorrowAtTree.AddDays(1);

                TimeSpan TimeToTreeOCloc = tomorrowAtTree - DateTime.Now;

                Thread.Sleep((int)TimeToTreeOCloc.TotalMilliseconds);// sleep until 3.00 hours

                CheckCurrentSeasonEnd(); 

            });
            checkForNewSeasonThread.Start();
            //CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("da-DK");
            //CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("da-DK");
            var thr = new Thread(() =>
            {
                var db = DbArkContext.GetDbContext();
                while (true)
                {
                    //SUNSET
                    bool sunset = false;

                    DateTime SunsetTime = XmlParser.GetSunsetFromXml();

                    if (SunsetTime > DateTime.Now)
                    {
                        sunset = true;
                    }

                    //Afsender SMS´er
                    var FindSMS = from s in db.SMS
                        where s.Dispatched == false
                        select s;

                    foreach (SMS sms in FindSMS)
                    {
                        if (sunset)
                        {
                            SMS SMS = new SMS
                            {
                                Reciever = sms.Reciever,
                                Message = "Hej" + sms.Name + "bekræft venligst med OK, at du har det godt hilsen Aalborg Roklub"
                            };
                            SMSIT.SendSMS(SMS);
                            sms.Dispatched = true;
                        }
                    }

                    //Modtager SMS´er
                    var smsser = (from s in db.GetSMS
                        where !s.Handled && s.RecievedDate.Day == DateTime.Now.Day && s.RecievedDate.Month == DateTime.Now.Month && s.RecievedDate.Year == DateTime.Now.Year
                        select s).ToList();
                    var getsms = from s in db.SMS
                        select s;

                    var Noresponse = from s in db.SMS
                        where !s.approved && !s.Handled
                        select s;

                    foreach (GetSMS sms in smsser)
                    {
                        if (sms.Text.ToLower() == "ok")
                        {
                            getsms.Where(e => e.Reciever == sms.From.Replace("+", string.Empty)).ToList().ForEach(e => e.approved = true);

                            SMS SMS = new SMS
                            {
                                Reciever = sms.From.Replace("+", string.Empty),
                                Message = "Bekræftelsen er modtaget, venlig hilsen Aalborg Roklub"
                            };
                            SMSIT.SendSMS(SMS);
                            sms.Handled = true;
                        }
                        else
                        {
                            SMS SMS = new SMS
                            {
                                Reciever = sms.From.Replace("+", string.Empty),
                                Message = "Beskeden blev ikke forstået, bekræft venligst igen, venlig hilsen Aalborg Roklub"
                            };
                            SMSIT.SendSMS(SMS);
                            sms.Handled = true;
                        }
                    }

                    //Ingen response i 20 min --> Send besked til bestyrelsen
                    foreach (SMS noresponse in Noresponse)
                    {
                        if (DateTime.Now.AddMinutes(-20) > noresponse.Time && sunset)
                        {
                            SMS SMS = new SMS
                            {
                                Reciever = "4522907111",
                                Message = "Følgende person er ikke kommet hjem " + noresponse.Name + " hans telefon nummer er " + noresponse.Reciever + string.Empty
                            };
                            SMSIT.SendSMS(SMS);
                            noresponse.Handled = true;
                        }
                    }

                    //Save til databasen
                    db.SaveChanges();
                    Thread.Sleep(5000);
                }
            });

            WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
            if (windowsIdentity != null && windowsIdentity.Name == "SAHB-WIN7\\sahb1")
            {
                thr.Start();
            }

            Current.Exit += (sender, e) =>
            {
                if (thr.ThreadState == ThreadState.Running)
                    thr.Abort();
            };
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
#if AdministrationsSystem
            this.StartupUri = new Uri("/View/Administrationssystem/AdminSystem.xaml", UriKind.Relative);
#else
#if DEBUG
            this.StartupUri = new Uri("/MainWindow.xaml", UriKind.Relative);
#else
            this.StartupUri = new Uri("/View/Protokolsystem/ProtocolSystem.xaml", UriKind.Relative);
#endif
#endif
        }

        void CheckCurrentSeasonEnd()
        {
            using (var db = new DbArkContext())
            {
                Season currentSeason;

                // Get current season
                if (!db.Season.Any(x => true))
                {
                    currentSeason = new Season();
                    db.Season.Add(currentSeason);
                }
                else
                    currentSeason = db.Season.AsEnumerable().Last(x => true);


                // if current seasonEnd is before today add new season.
                if (DateTime.Compare(currentSeason.SeasonEnd, DateTime.Now) <= 0)
                {
                    currentSeason = new Season();
                    db.Season.Add(currentSeason);
                    db.SaveChanges();
                }
            }
        }
    }
}
