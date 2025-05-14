using System.Xml.Serialization;

namespace Fanex.Settlement.MessageBridge.RefundEvents;

public class ResourceItem
{
    private string determined = "0";

    [XmlAttribute("Id")]
    public string ResourceId { get; set; }

    [XmlAttribute("Home")]
    public string HomeValue { get; set; }

    [XmlAttribute("Away")]
    public string AwayValue { get; set; }

    [XmlAttribute("OtherInfo")]
    public string OtherInfo { get; set; }

    [XmlAttribute("IsDetermined")]
    public string Determined
    {
        get => IsSettleDetermined ? "1" : determined;
        set => determined = value;
    }

    [XmlIgnore]
    public bool IsSettleDetermined { get; set; }

    [XmlIgnore]
    public string AwayId { get; set; }

    [XmlIgnore]
    public string HomeId { get; set; }

    [XmlIgnore]
    public string ResourceName { get; set; }

    [XmlIgnore]
    public int GroupBetType { get; set; }
}