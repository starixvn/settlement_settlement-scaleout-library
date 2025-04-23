using Fanex.SiteDbConnectionManager.Models;

namespace Fanex.SiteDbConnectionManager.Services;

public interface SiteDbPendingCommandServices
{
    Task<int> SavePendingCommand(SiteDbPendingCommand command);

    Task<bool> UpdatePendingCommandStatus(int pId, int statusId);
}