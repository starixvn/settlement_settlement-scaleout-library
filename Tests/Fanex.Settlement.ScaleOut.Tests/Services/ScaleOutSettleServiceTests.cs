using AutoMapper.Configuration;
using Fanex.Settlement.ScaleOut.Services;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace Fanex.Settlement.ScaleOut.Tests.Services
{
    public class ScaleOutSettleServiceTests
    {
        private readonly IDynamicRepository repository;

        private readonly IMapper mapper;

        private readonly GeneralService generalService;

        private readonly ScaleOutSettleService service;

        private readonly IHttpContextAccessor accessor;

        public ScaleOutSettleServiceTests()
        {
            accessor = Substitute.For<IHttpContextAccessor>();
            var context = new DefaultHttpContext
            {
                Connection =
                {
                    RemoteIpAddress = new System.Net.IPAddress(1)
                }
            };
            accessor.HttpContext = context;
            repository = Substitute.For<IDynamicRepository>();
            generalService = Substitute.For<GeneralService>();
            var mapperConfig = new MapperConfigurationExpression();
            mapperConfig.AddProfile<ScaleOutMappingProfile>();
            var config = new MapperConfiguration(mapperConfig);
            mapper = new Mapper(config);
            service = new ScaleOutSettleServiceImp(repository, mapper, accessor, generalService);
        }

        [Theory]
        [InlineData(1, 1, ActionName.CalculateFullTime, 42)]
        [InlineData(1, 2, ActionName.RefundHalfTime, 42)]
        [InlineData(1, 2, ActionName.Undo, 42)]
        [InlineData(1, 2, ActionName.CalculateFullTime, 1)]
        [InlineData(null, 2, ActionName.CalculateFullTime, 42)]
        [InlineData(2, null, ActionName.CalculateFullTime, 42)]
        public void UpdateAutoRefundChildMatch_NotPassValidation_DoNotExecute(int? home, int? away, string action, int league)
        {
            var request = new RefundChildMatchRequest
            {
                HomeValue = home,
                AwayValue = away,
                ActionName = action,
                LeagueGroupId = league
            };

            service.UpdateAutoRefundChildMatch(request);

            repository.DidNotReceive().ExecuteAsync(Arg.Any<UpdateAutoRefundChildMatchCriteria>());
        }

        [Fact]
        public void UpdateAutoRefundChildMatch_PassValidation_DoExecute()
        {
            var request = new RefundChildMatchRequest
            {
                HomeValue = 1,
                AwayValue = 2,
                ActionName = ActionName.CalculateFullTime,
                LeagueGroupId = 42
            };

            service.UpdateAutoRefundChildMatch(request);

            repository.Received().ExecuteAsync(Arg.Any<UpdateAutoRefundChildMatchCriteria>());
        }

        [Fact]
        public void UpdateMatchInfo_Always_Execute()
        {
            service.UpdateMatchInfo(new UpdateMatchInfoRequest());

            repository.Received().ExecuteAsync(Arg.Any<UpdateMatchInfoCriteria>());
        }

        [Theory]
        [InlineData(ActionName.RefundHalfTime)]
        [InlineData(ActionName.CalculateHalfTime)]
        public void UpdateUnderSettlementFlag_ActionIsCalcHTOrRefundHT_DoNotExecute(string action)
        {
            service.UpdateUnderSettlementFlag(1, 1, action);

            repository.DidNotReceive().ExecuteAsync(Arg.Any<UpdateSettlementStatusCriteria>());
        }

        [Theory]
        [InlineData(ActionName.Undo, false)]
        [InlineData(ActionName.CalculateFullTime, true)]
        public void UpdateUnderSettlementFlag_PassValidation_Execute(string action, bool isEnable)
        {
            service.UpdateUnderSettlementFlag(1, 1, action);

            repository.Received().ExecuteAsync(Arg.Is<UpdateSettlementStatusCriteria>(item => item.IsEnabled == isEnable));
        }

        [Fact]
        public async Task UpdateUnderSettlementFlag_PassValidationAndIpEmpty_ExecuteAsync()
        {
            accessor.HttpContext = null;
            var tempService = new ScaleOutSettleServiceImp(repository, mapper, accessor, generalService);
            await tempService.UpdateUnderSettlementFlag(1, 1, ActionName.CalculateFullTime);

            await repository.Received().ExecuteAsync(Arg.Is<UpdateSettlementStatusCriteria>(item => item.Ip == string.Empty));
        }

        [Fact]
        public async Task ValidateEventAndTicket_ValidateEventFailed_ReturnErrorAndStop()
        {
            var error = new ResultMessage
            {
                ErrorCode = 1,
                ErrorMessage = "Error"
            };
            generalService.ExecuteSingle(Arg.Any<ValidateEventCriteria>()).Returns(error);

            var result = await service.ValidateEventAndTicket(new EventTicketRequest());

            await generalService.DidNotReceive().ExecuteAsync(Arg.Any<ValidateTicketCriteria>());
            Assert.Equal(1, result.ErrorCode);
            Assert.Equal("Error", result.ErrorMessage);
        }

        [Fact]
        public async Task ValidateEventAndTicket_ValidateEventPassed_ExecuteValidateTicketAndReturnValue()
        {
            generalService.ExecuteSingle(Arg.Any<ValidateEventCriteria>()).Returns(new ResultMessage());
            generalService.ExecuteAsync(Arg.Any<ValidateTicketCriteria>()).Returns(new ResultMessage { ErrorCode = 1 });

            var result = await service.ValidateEventAndTicket(new EventTicketRequest());

            await generalService.Received().ExecuteAsync(Arg.Any<ValidateTicketCriteria>());
            await generalService.Received().ExecuteSingle(Arg.Any<ValidateEventCriteria>());
            Assert.Equal(1, result.ErrorCode);
        }

        [Fact]
        public async Task ValidateEventAndTicketFastMarket_ValidateEventFailed_ReturnErrorAndStop()
        {
            var error = new ResultMessage
            {
                ErrorCode = 1,
                ErrorMessage = "Error"
            };
            generalService.ExecuteSingle(Arg.Any<ValidationEventFastMarketCriteria>()).Returns(error);

            var result = await service.ValidateEventAndTicketFastMarket(new EventTicketFastMarketRequest());

            await generalService.DidNotReceive().ExecuteAsync(Arg.Any<ValidationTicketFastMarketCriteria>());
            Assert.Equal(1, result.ErrorCode);
            Assert.Equal("Error", result.ErrorMessage);
        }

        [Fact]
        public async Task ValidateEventAndTicketFastMarket_ValidateEventPassed_ExecuteValidateTicketAndReturnValue()
        {
            generalService.ExecuteSingle(Arg.Any<ValidationEventFastMarketCriteria>()).Returns(new ResultMessage());
            generalService.ExecuteAsync(Arg.Any<ValidationTicketFastMarketCriteria>()).Returns(new ResultMessage { ErrorCode = 1 });

            var result = await service.ValidateEventAndTicketFastMarket(new EventTicketFastMarketRequest());

            await generalService.Received().ExecuteAsync(Arg.Any<ValidationTicketFastMarketCriteria>());
            await generalService.Received().ExecuteSingle(Arg.Any<ValidationEventFastMarketCriteria>());
            Assert.Equal(1, result.ErrorCode);
        }

        [Fact]
        public async Task GetSiteGroupID_Always_ReturnData()
        {
            repository.GetAsync<SiteGroup>(Arg.Any<GetSiteGroupCriteria>())
                .Returns(new SiteGroup { SiteDbGroupId = 1 });

            var result = await service.GetSiteGroupId("01");

            Assert.Equal(1, result);
        }
    }
}