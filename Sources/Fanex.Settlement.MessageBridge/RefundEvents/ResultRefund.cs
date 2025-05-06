using System.Text.Json;

namespace Fanex.Settlement.MessageBridge.RefundEvents;

public static class EventActionType
{
    public const string Refund = "Refund";
    public const string CfmRefund = "CfmRefund";
    public const string CfmRefundSDT = "CfmRefundSDT";
    public const string Cal = "Cal";
    public const string CfmRefundAdminSite = "CfmRefundAdminSite";
}

public record ResultsRefunded(
    int MatchId,
    short ShortType,
    short ChildMatchType,
    List<ResourceItem> Resources,
    string Action = EventActionType.Refund)
{
    public override string ToString()
        => JsonSerializer.Serialize(this);
}

public record ResultsRefundConfirmed(
    int MatchId,
    short ShortType,
    short ChildMatchType,
    List<ResourceItem> Resources,
    string Action = EventActionType.CfmRefund)
{
    public override string ToString()
        => JsonSerializer.Serialize(this);
}

public record ResultsRefundConfirmedSdt(
    int MatchId,
    short ShortType,
    short ChildMatchType,
    List<ResourceItem> Resources,
    string Action = EventActionType.CfmRefundSDT)
{
    public override string ToString()
        => JsonSerializer.Serialize(this);
}

public record MatchCalculated(
    int MatchId,
    int ParentId,
    short ShortType,
    short ChildMatchType,
    int? HalfTimeHomeScore,
    int? HalfTimeAwayScore,
    int? FinalHomeScore,
    int? FinalAwayScore,
    string Action = EventActionType.Cal)
{
    public override string ToString()
        => JsonSerializer.Serialize(this);
}

public record MatchRefunded(
    int MatchId,
    int ParentId,
    short ShortType,
    short ChildMatchType,
    string Action = EventActionType.Refund)
{
    public override string ToString()
        => JsonSerializer.Serialize(this);
}

public record ResultsCalculated(
    int MatchId,
    short ShortType,
    short ChildMatchType,
    List<ResourceItem> Resources,
    string Action = EventActionType.Cal)
{
    public override string ToString()
        => JsonSerializer.Serialize(this);
}

public record NotTakenResultsConfirmed(
    int MatchId,
    short ShortType,
    short ChildMatchType,
    List<ResourceItem> Resources,
    string Action = EventActionType.CfmRefundAdminSite)
{
    public override string ToString()
        => JsonSerializer.Serialize(this);
}