using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.Model
{
    public partial class XMLParseHelpers
    {
        public class XMLMedlemmer
        {
            /// <remarks/>
            [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
            [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
            public partial class dataroot
            {

                private datarootAktiveMedlemmer[] aktiveMedlemmerField;

                private System.DateTime generatedField;

                /// <remarks/>
                [System.Xml.Serialization.XmlElementAttribute("AktiveMedlemmer")]
                public datarootAktiveMedlemmer[] AktiveMedlemmer
                {
                    get
                    {
                        return this.aktiveMedlemmerField;
                    }
                    set
                    {
                        this.aktiveMedlemmerField = value;
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
            public partial class datarootAktiveMedlemmer
            {

                private object[] itemsField;

                private ItemsChoiceType[] itemsElementNameField;

                /// <remarks/>
                [System.Xml.Serialization.XmlElementAttribute("Adresse1", typeof(string))]
                [System.Xml.Serialization.XmlElementAttribute("Adresse2", typeof(string))]
                [System.Xml.Serialization.XmlElementAttribute("By", typeof(string))]
                [System.Xml.Serialization.XmlElementAttribute("EMail", typeof(string))]
                [System.Xml.Serialization.XmlElementAttribute("EMail2", typeof(string))]
                [System.Xml.Serialization.XmlElementAttribute("Efternavn", typeof(string))]
                [System.Xml.Serialization.XmlElementAttribute("Fornavn", typeof(string))]
                [System.Xml.Serialization.XmlElementAttribute("Frigivet", typeof(byte))]
                [System.Xml.Serialization.XmlElementAttribute("FrigivetDato", typeof(System.DateTime))]
                [System.Xml.Serialization.XmlElementAttribute("Fødselsdato", typeof(System.DateTime))]
                [System.Xml.Serialization.XmlElementAttribute("ID", typeof(ushort))]
                [System.Xml.Serialization.XmlElementAttribute("Kajakret", typeof(byte))]
                [System.Xml.Serialization.XmlElementAttribute("KajakretDato", typeof(System.DateTime))]
                [System.Xml.Serialization.XmlElementAttribute("Korttursstyrmand", typeof(byte))]
                [System.Xml.Serialization.XmlElementAttribute("KorttursstyrmandDato", typeof(System.DateTime))]
                [System.Xml.Serialization.XmlElementAttribute("Langtursstyrmand", typeof(byte))]
                [System.Xml.Serialization.XmlElementAttribute("LangtursstyrmandDato", typeof(System.DateTime))]
                [System.Xml.Serialization.XmlElementAttribute("MedlemsNr", typeof(byte))]
                [System.Xml.Serialization.XmlElementAttribute("Outriggerret", typeof(byte))]
                [System.Xml.Serialization.XmlElementAttribute("OutriggerretDato", typeof(System.DateTime))]
                [System.Xml.Serialization.XmlElementAttribute("PostNr", typeof(ushort))]
                [System.Xml.Serialization.XmlElementAttribute("Scullerret", typeof(byte))]
                [System.Xml.Serialization.XmlElementAttribute("ScullerretDato", typeof(System.DateTime))]
                [System.Xml.Serialization.XmlElementAttribute("Svømmeprøve", typeof(byte))]
                [System.Xml.Serialization.XmlElementAttribute("SvømmeprøveDato", typeof(System.DateTime))]
                [System.Xml.Serialization.XmlElementAttribute("Telefon", typeof(string))]
                [System.Xml.Serialization.XmlElementAttribute("TelefonArbejde", typeof(string))]
                [System.Xml.Serialization.XmlElementAttribute("TelefonMobil", typeof(string))]
                [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
                public object[] Items
                {
                    get
                    {
                        return this.itemsField;
                    }
                    set
                    {
                        this.itemsField = value;
                    }
                }

                /// <remarks/>
                [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
                [System.Xml.Serialization.XmlIgnoreAttribute()]
                public ItemsChoiceType[] ItemsElementName
                {
                    get
                    {
                        return this.itemsElementNameField;
                    }
                    set
                    {
                        this.itemsElementNameField = value;
                    }
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTypeAttribute(IncludeInSchema = false)]
            public enum ItemsChoiceType
            {

                /// <remarks/>
                Adresse1,

                /// <remarks/>
                Adresse2,

                /// <remarks/>
                By,

                /// <remarks/>
                EMail,

                /// <remarks/>
                EMail2,

                /// <remarks/>
                Efternavn,

                /// <remarks/>
                Fornavn,

                /// <remarks/>
                Frigivet,

                /// <remarks/>
                FrigivetDato,

                /// <remarks/>
                Fødselsdato,

                /// <remarks/>
                ID,

                /// <remarks/>
                Kajakret,

                /// <remarks/>
                KajakretDato,

                /// <remarks/>
                Korttursstyrmand,

                /// <remarks/>
                KorttursstyrmandDato,

                /// <remarks/>
                Langtursstyrmand,

                /// <remarks/>
                LangtursstyrmandDato,

                /// <remarks/>
                MedlemsNr,

                /// <remarks/>
                Outriggerret,

                /// <remarks/>
                OutriggerretDato,

                /// <remarks/>
                PostNr,

                /// <remarks/>
                Scullerret,

                /// <remarks/>
                ScullerretDato,

                /// <remarks/>
                Svømmeprøve,

                /// <remarks/>
                SvømmeprøveDato,

                /// <remarks/>
                Telefon,

                /// <remarks/>
                TelefonArbejde,

                /// <remarks/>
                TelefonMobil,
            }
        }
    }
}
