using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ARK.Model.XML;

namespace ARK.HelperFunctions
{
    public static class SunsetClass
    {
        private static Task _task;
        private static DateTime _sunset;

        public static DateTime Sunset
        {
            get { return _sunset != default(DateTime) ? _sunset : (_sunset = XmlParser.GetSunsetFromXml()); }
            private set { _sunset = value; }
        }

        internal static void StartSunsetTask(CancellationTokenSource wtoken)
        {
            if (_task == null)
            {
                _task = Task.Factory.StartNew(async () =>
                {
                    while (true)
                    {
                        Sunset = XmlParser.GetSunsetFromXml();
                        var delay = DateTime.Today.AddDays(1).AddHours(2) - DateTime.Now;
                        await Task.Delay(delay, wtoken.Token);
                    }
                }, wtoken.Token);
            }
            else
            {
                throw new InvalidOperationException("The task has already been started");
            }
        }
    }
}
