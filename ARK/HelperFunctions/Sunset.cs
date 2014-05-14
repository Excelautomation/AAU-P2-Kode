using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARK.Model.XML;

namespace ARK.HelperFunctions
{
    public static class Sunset
    {
        private static DateTime _sunset;

        public static DateTime GetSunset
        {
            get { return _sunset != DateTime.MinValue ? _sunset : (_sunset = XmlParser.GetSunsetFromXml()); }
        }
    }
}
