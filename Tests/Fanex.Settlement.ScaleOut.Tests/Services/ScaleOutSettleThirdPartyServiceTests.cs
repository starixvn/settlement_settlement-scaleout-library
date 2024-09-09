using Fanex.Settlement.ScaleOut.Services;
using NSubstitute;

namespace Fanex.Settlement.ScaleOut.Tests.Services
{
    public class ScaleOutSettleThirdPartyServiceTests
    {
        private readonly IDynamicRepository repository;

        private readonly GeneralService generalService;

        private readonly ScaleOutSettleThirdPartyServiceImp service;

        public ScaleOutSettleThirdPartyServiceTests()
        {
            repository = Substitute.For<IDynamicRepository>();
            generalService = Substitute.For<GeneralService>();

            var mapperConfig = new MapperConfigurationExpression();
            mapperConfig.AddProfile<ScaleOutMappingProfile>();
            var config = new MapperConfiguration(mapperConfig);
            var mapper = new Mapper(config);
            service = new ScaleOutSettleThirdPartyServiceImp(repository, mapper, generalService);
        }

        [Fact]
        public void UpdateMatchInfo3rd_Always_Execute()
        {
            service.UpdateMatchInfo3rd(new UpdateMatchInfo3RdRequest());

            repository.Received().ExecuteAsync(Arg.Any<UpdateMatchInfo3RdCriteria>());
        }

        [Fact]
        public void GetDeploymentInfo_Always_Execute()
        {
            service.GetDeploymentInfo(new GetDeploymentInfoRequest());

            generalService.Received().ExecuteAsync(Arg.Any<GetDeploymentInfoCriteria>());
        }

        [Fact]
        public async Task ValidateMoveBetDay_ExecutePassed_ReturnNonError()
        {
            generalService.ExecuteAsync(Arg.Any<ValidateMoveBetDayCriteria>()).Returns(new ResultMessage());
            var result = await service.ValidateMoveBetDay("1", "text");

            await generalService.Received().ExecuteAsync(Arg.Any<ValidateMoveBetDayCriteria>());
            Assert.Equal(0, result.ErrorCode);
        }

        [Fact]
        public async Task ValidateMoveBetDay_ExecuteFailed_ReturnError()
        {
            generalService.ExecuteAsync(Arg.Any<ValidateMoveBetDayCriteria>()).Returns(new ResultMessage { ErrorCode = 1 });

            var result = await service.ValidateMoveBetDay("1", "text");

            await generalService.Received().ExecuteAsync(Arg.Any<ValidateMoveBetDayCriteria>());
            Assert.Equal(1, result.ErrorCode);
            Assert.Equal("Cannot proceed due to this match is in during move-bet progress. Please contact admin for support.", result.ErrorMessage);
        }
    }
}