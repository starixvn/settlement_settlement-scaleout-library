using Fanex.Data.Repository;

namespace Fanex.SiteDbConnectionManager.Services.Criteria;

public class SavePendingCommandCriteria : NonQueryCommand
{
    public int MatchId { get; set; }

    public int SportType { get; set; }

    public string SpName { get; set; }

    public string Parameter { get; set; }

    public string Action { get; set; }

    public string IP { get; set; }

    public string SystemName { get; set; }

    public int AdminId { get; set; }

    public int SiteDBGroupId { get; set; }

    public int StatusId { get; set; }

    public override string GetSettingKey() => "SiteDB_PendingCommand_Ins";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(SpName) && MatchId > 0;
}