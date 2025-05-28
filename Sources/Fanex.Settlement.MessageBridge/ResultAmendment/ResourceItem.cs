using System.Xml.Serialization;

namespace Fanex.Settlement.MessageBridge.ResultAmendment;


public class ResourceItem
{
    [XmlAttribute("Id")]
    public string ResourceId { get; set; }

    [XmlAttribute("Home")]
    public string HomeValue { get; set; }

    [XmlAttribute("Away")]
    public string AwayValue { get; set; }

    [XmlAttribute("OtherInfo")]
    public string OtherInfo { get; set; }

    [XmlIgnore]
    public string AwayId { get; set; }

    [XmlIgnore]
    public string Action { get; set; }

    [XmlIgnore]
    public string HomeId { get; set; }

    [XmlIgnore]
    public int GroupBetType { get; set; }
}