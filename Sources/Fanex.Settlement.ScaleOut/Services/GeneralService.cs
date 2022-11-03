namespace Fanex.Settlement.ScaleOut.Services;

public interface GeneralService
{
    Task<ResultMessage> ExecuteAsync(ExecuteCriteria criteria);

    Task<ResultMessage> ExecuteSingle(ExecuteSingleCriteria criteria);
}

public class GeneralServiceImp : GeneralService
{
    private readonly IConfiguration configuration;

    private readonly IDynamicRepository repository;

    public GeneralServiceImp(IConfiguration configuration, IDynamicRepository repository)
    {
        this.configuration = configuration;
        this.repository = repository;
    }

    public async Task<ResultMessage> ExecuteAsync(ExecuteCriteria criteria)
    {
        var servers = configuration.GetSection("SiteDbServer").Get<List<string>>();
        foreach (var server in servers)
        {
            criteria.DbServer = server;
            await repository.ExecuteAsync(criteria);
            if (criteria.ErrCode > 0)
            {
                return new ResultMessage
                {
                    ErrorCode = criteria.ErrCode,
                    ErrorMessage = criteria.ErrMsg
                };
            }
        }

        return new ResultMessage();
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