using Fanex.Data.Repository;
using Fanex.SiteDbConnectionManager.Models;
using Fanex.SiteDbConnectionManager.Services.Criteria;

namespace Fanex.SiteDbConnectionManager.Services;

public class SiteDbPendingCommandServicesImp(IDynamicRepository repository) : SiteDbPendingCommandServices
{
    public async Task<int> SavePendingCommand(SiteDbPendingCommand command)
    {
        var criteria = new SavePendingCommandCriteria
        {
            MatchId = command.MatchId,
            SportType = command.SportType,
            SpName = command.SpName,
            Parameter = command.Parameter,
            Action = command.Action,
            IP = command.IP,
            AdminId = command.AdminId,
            IsAPI = command.IsAPI,
            SiteDBGroupId = command.SiteDBGroupId,
            StatusId = command.StatusId
        };

        await repository.ExecuteAsync(criteria);
        return criteria.ErrCode;
    }

    public async Task<bool> UpdatePendingCommandStatus(int pId, int statusId)
    {
        var criteria = new UpdatePendingCommandStatusCriteria
        {
            PId = pId,
            StatusId = statusId
        };

        await repository.ExecuteAsync(criteria);
        return true;
    }
}