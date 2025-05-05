using Fanex.Data.Repository;

namespace Fanex.SiteDbConnectionManager.Services.Criteria;

public class CheckingPendingCommandCriteria : NonQueryCommand
{
    public int MatchId { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override string GetSettingKey() => "SiteDB_PendingCommand_Checking";

    public override bool IsValid() => MatchId > 0;
}