namespace Fanex.Settlement.MessageBridge.ResultAmendment;
public record class ResultChanged
{
    public string ResourceId { get; set; } = string.Empty;

    public string HomeValue { get; set; } = string.Empty;

    public string AwayValue { get; set; } = string.Empty;

    public string Action { get; set; } = string.Empty;

    public string OtherInfo { get; set; } = string.Empty;

    public DateTimeOffset LogDate { get; set; }

    public short ReSettleType { get; set; }
}