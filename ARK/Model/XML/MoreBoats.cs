using System.Xml.Serialization;

namespace ARK.Model.XML
{
    [XmlRoot("dataroot")]
    public class MoreBoats
    {
        [XmlElement("BådeSpecifik")]
        public ARK.Model.Boat[] BoatsSpecific { get; set; }
    }
}