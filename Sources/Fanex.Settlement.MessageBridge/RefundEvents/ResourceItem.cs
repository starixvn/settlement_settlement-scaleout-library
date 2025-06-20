using System.Xml.Serialization;

namespace Fanex.Settlement.MessageBridge.RefundEvents;

public class ResourceItem
{
    private string determined = "0";

    [XmlAttribute("Id")]
    public string ResourceId { get; set; } = string.Empty;

    [XmlAttribute("Home")]
    public string HomeValue { get; set; } = string.Empty;

    [XmlAttribute("Away")]
    public string AwayValue { get; set; } = string.Empty;

    [XmlAttribute("OtherInfo")]
    public string OtherInfo { get; set; } = string.Empty;

    [XmlAttribute("IsDetermined")]
    public string Determined
    {
        get => IsSettleDetermined ? "1" : determined;
        set => determined = value;
    }

    [XmlIgnore]
    public bool IsSettleDetermined { get; set; }

    [XmlIgnore]
    public string AwayId { get; set; } = string.Empty;

    [XmlIgnore]
    public string HomeId { get; set; } = string.Empty;

    [XmlIgnore]
    public string ResourceName { get; set; } = string.Empty;

    [XmlIgnore]
    public int GroupBetType { get; set; }
}