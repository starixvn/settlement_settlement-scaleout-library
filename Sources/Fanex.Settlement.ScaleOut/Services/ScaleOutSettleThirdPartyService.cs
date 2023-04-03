using System;

namespace Fanex.Settlement.ScaleOut.Services;

public interface ScaleOutSettleThirdPartyService
{
    Task<(ResultMessage, string)> ValidateBetBuilder(BetBuilderRequest request);

    Task<ResultMessage> GetDeploymentInfo(GetDeploymentInfoRequest request);

    Task UpdateMatchInfo3rd(UpdateMatchInfo3rdRequest request);

    Task<ResultMessage> ValidateMoveBetDay(string matchIds, string eventTable, DateTime? eventDate = null);
}

public class ScaleOutSettleThirdPartyServiceImp : ScaleOutSettleThirdPartyService
{
    private readonly IDynamicRepository repository;

    private readonly IConfiguration configuration;

    private readonly IMapper mapper;

    private readonly GeneralService generalService;

    public ScaleOutSettleThirdPartyServiceImp(
        IDynamicRepository repository,
        IConfiguration configuration,
        IMapper mapper,
        GeneralService generalService)
    {
        this.repository = repository;
        this.configuration = configuration;
        this.mapper = mapper;
        this.generalService = generalService;
    }

    public async Task<(ResultMessage, string)> ValidateBetBuilder(BetBuilderRequest request)
    {
        var eventCriteria = mapper.Map<BetBuilderRequest, ValidateBetBuilderCriteria>(request);
        await repository.ExecuteAsync(eventCriteria);

        var result = new ResultMessage
        {
            ErrorCode = eventCriteria.ErrCode,
            ErrorMessage = eventCriteria.ErrMsg
        };

        return (result, eventCriteria.InvalidMarketId);
    }

    public async Task UpdateMatchInfo3rd(UpdateMatchInfo3rdRequest request)
    {
        var criteria = mapper.Map<UpdateMatchInfo3rdRequest, UpdateMatchInfo3rdCriteria>(request);
        await repository.ExecuteAsync(criteria);
    }

    public async Task<ResultMessage> GetDeploymentInfo(GetDeploymentInfoRequest request)
    {
        var criteria = new GetDeploymentInfoCriteria { SpName = request.SPName };

        return await generalService.ExecuteAsync(criteria);
    }

    public async Task<ResultMessage> ValidateMoveBetDay(string matchIds, string eventTable, DateTime? eventDate = null)
    {
        var criteria = new ValidateMoveBetDayCriteria { MatchIds = matchIds, EventTable = eventTable, EventDate = eventDate };
        var resultValidate = await generalService.ExecuteAsync(criteria);
        if (resultValidate.ErrorCode > 0)
        {
            resultValidate.ErrorMessage =
                "Cannot proceed due to this match is in during move-bet progress. Please contact admin for support.";
        }

        return resultValidate;
    }
}