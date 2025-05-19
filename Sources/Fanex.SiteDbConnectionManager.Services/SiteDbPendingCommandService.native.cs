using Fanex.Data.Repository;
using Fanex.SiteDbConnectionManager.Client.Models;
using Fanex.SiteDbConnectionManager.Client.Services;
using Fanex.SiteDbConnectionManager.Services.Criteria;

namespace Fanex.SiteDbConnectionManager.Services;

public class SiteDbPendingCommandServiceImp(IDynamicRepository repository) : SiteDbPendingCommandService
{
    public async Task<ResultMessage> SavePendingCommand(SiteDbPendingCommand command)
    {
        var criteria = new SavePendingCommandCriteria
        {
            MatchId = command.MatchId,
            ProductType = command.ProductType,
            SpName = command.SpName,
            Parameter = command.Parameter,
            Action = command.Action,
            IP = command.IP,
            AdminId = command.AdminId,
            SystemName = command.SystemName,
            SiteDBGroupId = command.SiteDBGroupId,
            StatusId = command.StatusId
        };

        await repository.ExecuteAsync(criteria);

        return new ResultMessage();
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

    public async Task<ResultMessage> CheckingPendingCommand(int matchId, int siteDbGroupId)
    {
        var criteria = new CheckingPendingCommandCriteria
        {
            MatchId = matchId,
            SiteDbGroupId = siteDbGroupId
        };
        await repository.ExecuteAsync(criteria);

        return new ResultMessage
        {
            ErrorCode = criteria.ErrCode,
            ErrorMessage = criteria.ErrMsg
        };
    }
}