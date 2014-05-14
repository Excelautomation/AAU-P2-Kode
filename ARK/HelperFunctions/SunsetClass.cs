using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARK.Model.XML;

namespace ARK.HelperFunctions
{
    public static class SunsetClass
    {
        private static DateTime _sunset;

        public static DateTime Sunset
        {
            get
            {
                var val = _sunset != DateTime.MinValue ? _sunset : (_sunset = XmlParser.GetSunsetFromXml());
                Console.WriteLine();
                return val;
            }
        }
    }
}
