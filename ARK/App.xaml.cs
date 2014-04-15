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
                        //Afsender SMS´er
                        var FindSMS = from s in db.SMS
                                       where s.Dispatched == false
                                            select s;

                        foreach(var sms in FindSMS)
                        {
                            SMS SMS = new SMS() { 
                                Reciever = sms.Reciever, 
                                Message = "Hej" +sms.Name+ "bekræft venligst med OK, at du har det godt hilsen Aalborg Roklub"
                            };
                            SMSIT.SendSMS(SMS);
                            sms.Dispatched = true;
                        }



                        //Modtager SMS´er
                        var smsser = from s in db.GetSMS
                                     where s.Handled
                                     select s;
                        var getsms = from s in db.SMS
                                     select s;

                        foreach(var sms in smsser){
                            if (sms.Text == "ok" || sms.Text == "oK" || sms.Text == "Ok" || sms.Text == "OK")
                            {
                                //Mangler dato
                                getsms.Where(e => e.Reciever == sms.From).ToList().ForEach(e => e.approved = true);
                                
                                
                            }
                            //sms.Handled = true; 
                        }


                        //Save til databasen
                        db.SaveChanges();
                    }
                    
                    Debug.WriteLine("Søren er noob");
                    Thread.Sleep(1000);
                }
            }));
            thr.Start();
        }
    }
}
