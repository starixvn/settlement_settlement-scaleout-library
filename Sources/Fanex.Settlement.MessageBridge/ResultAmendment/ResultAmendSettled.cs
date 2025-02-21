namespace Fanex.Settlement.MessageBridge.ResultAmendment;

public record ResultAmendSettled(int MatchId, string SessionIds, bool IsOrg);