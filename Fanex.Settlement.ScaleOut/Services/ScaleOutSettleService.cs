using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fanex.Data.Repository;
using Fanex.Settlement.ScaleOut.Constants;
using Fanex.Settlement.ScaleOut.Criteria;
using Fanex.Settlement.ScaleOut.Criteria.FastMarket;
using Fanex.Settlement.ScaleOut.Criteria.Normal;
using Fanex.Settlement.ScaleOut.Criteria.Update;
using Fanex.Settlement.ScaleOut.Models;
using Fanex.Settlement.ScaleOut.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fanex.Settlement.ScaleOut.Services
{
    public interface ScaleOutSettleService
    {
        Task UpdateAutoRefundChildMatch(RefundChildMatchRequest request);

        Task UpdateMatchInfo(UpdateMatchInfoRequest request);

        Task UpdateUnderSettlementFlag(int matchId, int adminId, string actionName);

        Task<ResultMessage> ValidateEventAndTicket(EventTicketRequest request);

        Task<ResultMessage> ValidateEventAndTicketFastMarket(EventTicketFastMarketRequest request);

        Task<int> GetSiteGroupId(string dbServer);
    }

    public class ScaleOutSettleServiceImp : ScaleOutSettleService
    {
        private readonly IDynamicRepository repository;

        private readonly IConfiguration configuration;

        private readonly IMapper mapper;

        private readonly IHttpContextAccessor accessor;

        public ScaleOutSettleServiceImp(
            IDynamicRepository repository,
            IConfiguration configuration,
            IMapper mapper,
            IHttpContextAccessor accessor)
        {
            this.repository = repository;
            this.configuration = configuration;
            this.mapper = mapper;
            this.accessor = accessor;
        }

        public async Task UpdateAutoRefundChildMatch(RefundChildMatchRequest request)
        {
            var leagueGroupIds = new int[] { 42, 74, 108 };
            var isCalcAll = string.Equals(request.ActionName, ActionName.CalculateFullTime) ||
                            string.Equals(request.ActionName, ActionName.CalculateAll);

            if (DifferentFinalScore(request.HomeValue, request.AwayValue)
                && leagueGroupIds.Contains(request.LeagueGroupId)
                && isCalcAll)
            {
                var criteria = new UpdateAutoRefundChildMatchCriteria
                {
                    ParentId = request.ParentId,
                    ChildMatchIds = request.ChildMatchIds
                };

                await repository.ExecuteAsync(criteria);
            }
        }

        private static bool DifferentFinalScore(int? homeScore, int? awayScore)
        {
            var score = homeScore != awayScore;
            return homeScore != null && awayScore != null && score;
        }

        public async Task UpdateMatchInfo(UpdateMatchInfoRequest request)
        {
            var criteria = mapper.Map<UpdateMatchInfoRequest, UpdateMatchInfoCriteria>(request);
            await repository.ExecuteAsync(criteria);
        }

        public async Task UpdateUnderSettlementFlag(int matchId, int adminId, string actionName)
        {
            if (!string.Equals(actionName, ActionName.CalculateHalfTime) && !string.Equals(actionName, ActionName.RefundHalfTime))
            {
                const int settlementSystem = 4;
                var httpContext = accessor.HttpContext;
                var criteria = new UpdateSettlementStatusCriteria
                {
                    MatchId = matchId,
                    AdminId = adminId,
                    Ip = httpContext == null ? string.Empty : httpContext.Connection.RemoteIpAddress.ToString(),
                    Type = settlementSystem,
                    IsEnabled = actionName != ActionName.Undo
                };

                await repository.ExecuteAsync(criteria);
            }
        }

        public async Task<ResultMessage> ValidateEventAndTicket(EventTicketRequest request)
        {
            var eventCriteria = mapper.Map<EventTicketRequest, ValidateEventCriteria>(request);
            var validateEvent = await ValidateEvent(eventCriteria);

            if (validateEvent.ErrorCode > 0)
            {
                return validateEvent;
            }

            var ticketCriteria = mapper.Map<EventTicketRequest, ValidateTicketCriteria>(request);
            return await ValidateTickets(ticketCriteria);
        }

        private async Task<ResultMessage> ValidateTickets(ValidateTicketCriteria criteria)
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

        private async Task<ResultMessage> ValidateEvent(ValidateEventCriteria criteria)
        {
            await repository.ExecuteAsync(criteria);

            return new ResultMessage
            {
                ErrorCode = criteria.ErrCode,
                ErrorMessage = criteria.ErrMsg
            };
        }

        public async Task<ResultMessage> ValidateEventAndTicketFastMarket(EventTicketFastMarketRequest request)
        {
            var eventCriteria = mapper.Map<EventTicketFastMarketRequest, ValidationEventFastMarketCriteria>(request);
            var validateEvent = await ValidateEventFastMarket(eventCriteria);

            if (validateEvent.ErrorCode > 0)
            {
                return validateEvent;
            }

            var ticketCriteria = mapper.Map<EventTicketFastMarketRequest, ValidationTicketFastMarketCriteria>(request);
            return await ValidateTicketsFastMarket(ticketCriteria);
        }

        private async Task<ResultMessage> ValidateTicketsFastMarket(ValidationTicketFastMarketCriteria criteria)
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

        private async Task<ResultMessage> ValidateEventFastMarket(ValidationEventFastMarketCriteria criteria)
        {
            await repository.ExecuteAsync(criteria);

            return new ResultMessage
            {
                ErrorCode = criteria.ErrCode,
                ErrorMessage = criteria.ErrMsg
            };
        }

        public async Task<int> GetSiteGroupId(string dbServer)
        {
            var criteria = new GetSiteGroupCriteria { DbServer = dbServer };
            var site = await repository.GetAsync<SiteGroup>(criteria);
            return site.SiteDbGroupId;
        }
    }
}