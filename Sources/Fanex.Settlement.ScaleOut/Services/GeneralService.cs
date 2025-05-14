using Fanex.SiteDbConnectionManager.Client.Services;

namespace Fanex.Settlement.ScaleOut.Services;

public interface GeneralService
{
    Task<ResultMessage> ExecuteAsync(ExecuteCriteria criteria);

    Task<ResultMessage> ExecuteSingle(ExecuteSingleCriteria criteria);
}

public class GeneralServiceImp(
    IDynamicRepository repository,
    SiteDbClientService siteDbClientService)
    : GeneralService
{
    public async Task<ResultMessage> ExecuteAsync(ExecuteCriteria criteria)
    {
        var activeSites = await siteDbClientService.GetActiveSiteDbAsync();
        var tasks = activeSites.Select(async site =>
        {
            criteria.DbServer = site.SiteName;
            await repository.ExecuteAsync(criteria);
            return new ResultMessage
            {
                ErrorCode = criteria.ErrCode,
                ErrorMessage = criteria.ErrMsg
            };
        });

        var results = await Task.WhenAll(tasks);
        var failedResult = results.ToList().Find(r => r.ErrorCode > 0);

        return failedResult ?? new ResultMessage();
    }

    public async Task<ResultMessage> ExecuteSingle(ExecuteSingleCriteria criteria)
    {
        await repository.ExecuteAsync(criteria);

        return new ResultMessage
        {
            ErrorCode = criteria.ErrCode,
            ErrorMessage = criteria.ErrMsg
        };
    }
}