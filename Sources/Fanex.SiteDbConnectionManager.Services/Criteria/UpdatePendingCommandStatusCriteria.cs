using Fanex.Data.Repository;

namespace Fanex.SiteDbConnectionManager.Services.Criteria;

public class UpdatePendingCommandStatusCriteria : NonQueryCommand
{
    public int PId { get; set; }

    public int StatusId { get; set; }

    public override string GetSettingKey() => "SiteDB_PendingCommand_Upd";

    public override bool IsValid() => PId > 0;
}