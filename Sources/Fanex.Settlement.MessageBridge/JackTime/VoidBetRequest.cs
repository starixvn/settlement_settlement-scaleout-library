namespace Fanex.Settlement.MessageBridge.JackTime;
public class VoidBetRequest
{
    public int MatchId { get; set; }

    public string MatchCode { get; set; }

    public List<VoidBetInfo> VoidBetInfos { get; set; }

    public string Action { get; set; }

    public int SportType { get; set; }

    public int HomeId { get; set; }

    public int AwayId { get; set; }
}
