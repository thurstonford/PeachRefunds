using System.Xml.Serialization;

namespace PeachRefunds.Models
{
    internal class CDVResults
    {
        [XmlAttribute("Result")]
        public CDVResult? Result { get; set; }
    }
}
