using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.Model.XML
{
    public class XMLSunset
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class sun
        {
            private decimal versionField;

            private sunLocation locationField;

            private sunDate dateField;

            private sunMorning morningField;

            private sunEvening eveningField;

            /// <remarks/>
            public decimal version
            {
                get
                {
                    return this.versionField;
                }
                set
                {
                    this.versionField = value;
                }
            }

            /// <remarks/>
            public sunLocation location
            {
                get
                {
                    return this.locationField;
                }
                set
                {
                    this.locationField = value;
                }
            }

            /// <remarks/>
            public sunDate date
            {
                get
                {
                    return this.dateField;
                }
                set
                {
                    this.dateField = value;
                }
            }

            /// <remarks/>
            public sunMorning morning
            {
                get
                {
                    return this.morningField;
                }
                set
                {
                    this.morningField = value;
                }
            }

            /// <remarks/>
            public sunEvening evening
            {
                get
                {
                    return this.eveningField;
                }
                set
                {
                    this.eveningField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class sunLocation
        {
            private decimal latitudeField;

            private decimal longitudeField;

            /// <remarks/>
            public decimal latitude
            {
                get
                {
                    return this.latitudeField;
                }
                set
                {
                    this.latitudeField = value;
                }
            }

            /// <remarks/>
            public decimal longitude
            {
                get
                {
                    return this.longitudeField;
                }
                set
                {
                    this.longitudeField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class sunDate
        {
            private byte dayField;

            private byte monthField;

            private byte timezoneField;

            private byte dstField;

            /// <remarks/>
            public byte day
            {
                get
                {
                    return this.dayField;
                }
                set
                {
                    this.dayField = value;
                }
            }

            /// <remarks/>
            public byte month
            {
                get
                {
                    return this.monthField;
                }
                set
                {
                    this.monthField = value;
                }
            }

            /// <remarks/>
            public byte timezone
            {
                get
                {
                    return this.timezoneField;
                }
                set
                {
                    this.timezoneField = value;
                }
            }

            /// <remarks/>
            public byte dst
            {
                get
                {
                    return this.dstField;
                }
                set
                {
                    this.dstField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class sunMorning
        {
            private System.DateTime sunriseField;

            private sunMorningTwilight twilightField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "time")]
            public System.DateTime sunrise
            {
                get
                {
                    return this.sunriseField;
                }
                set
                {
                    this.sunriseField = value;
                }
            }

            /// <remarks/>
            public sunMorningTwilight twilight
            {
                get
                {
                    return this.twilightField;
                }
                set
                {
                    this.twilightField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class sunMorningTwilight
        {
            private System.DateTime civilField;

            private System.DateTime nauticalField;

            private System.DateTime astronomicalField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "time")]
            public System.DateTime civil
            {
                get
                {
                    return this.civilField;
                }
                set
                {
                    this.civilField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "time")]
            public System.DateTime nautical
            {
                get
                {
                    return this.nauticalField;
                }
                set
                {
                    this.nauticalField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "time")]
            public System.DateTime astronomical
            {
                get
                {
                    return this.astronomicalField;
                }
                set
                {
                    this.astronomicalField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class sunEvening
        {
            private System.DateTime sunsetField;

            private sunEveningTwilight twilightField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "time")]
            public System.DateTime sunset
            {
                get
                {
                    return this.sunsetField;
                }
                set
                {
                    this.sunsetField = value;
                }
            }

            /// <remarks/>
            public sunEveningTwilight twilight
            {
                get
                {
                    return this.twilightField;
                }
                set
                {
                    this.twilightField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class sunEveningTwilight
        {
            private System.DateTime civilField;

            private System.DateTime nauticalField;

            private System.DateTime astronomicalField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "time")]
            public System.DateTime civil
            {
                get
                {
                    return this.civilField;
                }
                set
                {
                    this.civilField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "time")]
            public System.DateTime nautical
            {
                get
                {
                    return this.nauticalField;
                }
                set
                {
                    this.nauticalField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "time")]
            public System.DateTime astronomical
            {
                get
                {
                    return this.astronomicalField;
                }
                set
                {
                    this.astronomicalField = value;
                }
            }
        }
    }
}
