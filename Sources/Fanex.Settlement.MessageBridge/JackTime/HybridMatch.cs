namespace Fanex.Settlement.MessageBridge.JackTime;

public class HybridMatch
{
    public int MatchId { get; set; }

    public string Action { get; set; }

    public DateTimeOffset LogDate { get; set; }

    public int UserId { get; set; }
}