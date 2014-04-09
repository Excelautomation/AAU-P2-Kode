using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ARK.Model.XML
{
    [XmlRoot("dataroot")]
    public class FlereBåde
    {
        [System.Xml.Serialization.XmlElementAttribute("BådeSpecifik")]
        public ARK.Model.Baad[] BådeSpecifik { get; set; }
    }
    public class XMLBåde
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class dataroot
        {

            private datarootBådeSpecifik[] bådeSpecifikField;

            private System.DateTime generatedField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("BådeSpecifik")]
            public datarootBådeSpecifik[] BådeSpecifik
            {
                get
                {
                    return this.bådeSpecifikField;
                }
                set
                {
                    this.bådeSpecifikField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public System.DateTime generated
            {
                get
                {
                    return this.generatedField;
                }
                set
                {
                    this.generatedField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class datarootBådeSpecifik
        {

            private byte idField;

            private string navnField;

            private byte antalPladserField;

            private byte aktivField;

            private byte bådTypeField;

            private byte roforbudField;

            private byte specifikBådTypeField;

            private byte langtursBådField;

            /// <remarks/>
            public byte ID
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            public string Navn
            {
                get
                {
                    return this.navnField;
                }
                set
                {
                    this.navnField = value;
                }
            }

            /// <remarks/>
            public byte AntalPladser
            {
                get
                {
                    return this.antalPladserField;
                }
                set
                {
                    this.antalPladserField = value;
                }
            }

            /// <remarks/>
            public byte Aktiv
            {
                get
                {
                    return this.aktivField;
                }
                set
                {
                    this.aktivField = value;
                }
            }

            /// <remarks/>
            public byte BådType
            {
                get
                {
                    return this.bådTypeField;
                }
                set
                {
                    this.bådTypeField = value;
                }
            }

            /// <remarks/>
            public byte Roforbud
            {
                get
                {
                    return this.roforbudField;
                }
                set
                {
                    this.roforbudField = value;
                }
            }

            /// <remarks/>
            public byte SpecifikBådType
            {
                get
                {
                    return this.specifikBådTypeField;
                }
                set
                {
                    this.specifikBådTypeField = value;
                }
            }

            /// <remarks/>
            public byte LangtursBåd
            {
                get
                {
                    return this.langtursBådField;
                }
                set
                {
                    this.langtursBådField = value;
                }
            }
        }
    }
}

