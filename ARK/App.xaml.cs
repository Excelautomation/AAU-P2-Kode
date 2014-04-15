using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ARK.Model;
using ARK.Model.DB;
using ARK.Administrationssystem.Funktioner;

namespace ARK
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var thr = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    using (DbArkContext db = new DbArkContext())
                    {
                        //SUNSET
                        var sunset = false;
                        
                        var SunsetTime = DateTime.Now;

                        if (SunsetTime > DateTime.Now)
                        {
                            sunset = true;
                        }

                        //Afsender SMS´er
                        var FindSMS = from s in db.SMS
                                       where s.Dispatched == false
                                            select s;

                        foreach(var sms in FindSMS)
                        {
                            if (sunset)
                            {
                                SMS SMS = new SMS()
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

                        foreach(var sms in smsser){
                            if (sms.Text.ToLower() == "ok")
                            {
                                getsms.Where(e => e.Reciever == sms.From.Replace("+", "")).ToList().ForEach(e => e.approved = true);

                                SMS SMS = new SMS()
                                {
                                    Reciever = sms.From.Replace("+", ""),
                                    Message = "Bekræftelsen er modtaget, venlig hilsen Aalborg Roklub"
                                };
                                SMSIT.SendSMS(SMS);
                                sms.Handled = true;
                            }
                            else
                            {
                                SMS SMS = new SMS()
                                {
                                    Reciever = sms.From.Replace("+", ""),
                                    Message = "Beskeden blev ikke forstået, bekræft venligst igen, venlig hilsen Aalborg Roklub"
                                };
                                SMSIT.SendSMS(SMS);
                                sms.Handled = true;
                            }
                        }

                        //Ingen response i 20 min --> Send besked til bestyrelsen
                        foreach (var noresponse in Noresponse)
                        {
                            if (DateTime.Now.AddMinutes(-20) > noresponse.Time && sunset)
                            {
                                SMS SMS = new SMS()
                                {
                                    Reciever = "4522907111",
                                    Message = "Følgende person er ikke kommet hjem " + noresponse.Name + " hans telefon nummer er " + noresponse.Reciever + ""
                                };
                                SMSIT.SendSMS(SMS);
                                noresponse.Handled = true;
                            }
                        }

                        //Save til databasen
                        db.SaveChanges();
                    }
                    Thread.Sleep(5000);
                }
            }));
            thr.Start();

            Application.Current.Exit += (sender, e) =>
            {
                thr.Abort();
            };
        }
    }
}
