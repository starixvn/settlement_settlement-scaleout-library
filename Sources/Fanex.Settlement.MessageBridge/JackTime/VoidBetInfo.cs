using System.Text.Json.Serialization;

namespace Fanex.Settlement.MessageBridge.JackTime;

public class VoidBetInfo
{
    public string TransId { get; set; }

    public string CustId { get; set; }

    public string Action { get; set; }

    [JsonIgnore]
    public string ExtraInfo { get; set; }
}