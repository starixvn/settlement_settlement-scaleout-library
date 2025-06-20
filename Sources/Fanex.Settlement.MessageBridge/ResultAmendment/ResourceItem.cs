using System.Xml.Serialization;

namespace Fanex.Settlement.MessageBridge.ResultAmendment;

public class ResourceItem
{
    [XmlAttribute("Id")]
    public string ResourceId { get; set; } = string.Empty;

    [XmlAttribute("Home")]
    public string HomeValue { get; set; } = string.Empty;

    [XmlAttribute("Away")]
    public string AwayValue { get; set; } = string.Empty;

    [XmlAttribute("OtherInfo")]
    public string OtherInfo { get; set; } = string.Empty;

    [XmlIgnore]
    public string AwayId { get; set; } = string.Empty;

    [XmlIgnore]
    public string Action { get; set; } = string.Empty;

    [XmlIgnore]
    public string HomeId { get; set; } = string.Empty;

    [XmlIgnore]
    public int GroupBetType { get; set; }
}