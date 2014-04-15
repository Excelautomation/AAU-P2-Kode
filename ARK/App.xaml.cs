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
                        var sms = from s in db.GetSMS
                                  where s.Handled
                                      select s;


                    }
                    
                    Debug.WriteLine("Søren er noob");
                    Thread.Sleep(1000);
                }
            }));
            thr.Start();
        }
    }
}
