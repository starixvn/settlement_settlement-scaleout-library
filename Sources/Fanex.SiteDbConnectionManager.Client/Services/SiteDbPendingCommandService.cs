using Fanex.SiteDbConnectionManager.Client.Models;

namespace Fanex.SiteDbConnectionManager.Client.Services;

public interface SiteDbPendingCommandService
{
    Task<ResultMessage> SavePendingCommand(SiteDbPendingCommand command);

    Task<bool> UpdatePendingCommandStatus(int pId, int statusId);

    Task<ResultMessage> CheckingPendingCommand(int matchId, int siteDbGroupId);
}