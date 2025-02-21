namespace Fanex.Settlement.MessageBridge.ResultAmendment;


public record class ResultAmended(
    int MatchId,
    string BroadcastSessionId,
    int SportType,
    string Action,
    List<ResourceItem> SelectedResources,
    List<ResultChanged> ResultsChanged);
