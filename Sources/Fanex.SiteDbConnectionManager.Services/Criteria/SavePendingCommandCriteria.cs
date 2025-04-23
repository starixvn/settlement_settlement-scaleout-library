using Fanex.Data.Repository;

namespace Fanex.SiteDbConnectionManager.Services.Criteria;

public class SavePendingCommandCriteria : NonQueryCommand
{
    public int MatchId { get; set; }

    public string SportType { get; set; }

    public string SpName { get; set; }

    public string Parameter { get; set; }

    public string Action { get; set; }

    public string IP { get; set; }

    public string AdminId { get; set; }

    public bool IsAPI { get; set; }

    public string SiteDBGroupId { get; set; }

    public int StatusId { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override string GetSettingKey() => "SiteDB_PendingCommand_Ins";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(SpName) && !string.IsNullOrWhiteSpace(SiteDBGroupId);
}