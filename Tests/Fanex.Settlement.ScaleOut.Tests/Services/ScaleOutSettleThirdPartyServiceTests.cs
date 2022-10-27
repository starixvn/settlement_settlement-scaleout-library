using AutoMapper.Configuration;
using Castle.Core.Smtp;
using Fanex.Settlement.ScaleOut.Criteria.BetBuilder;
using Fanex.Settlement.ScaleOut.Criteria.HappyFive;
using Fanex.Settlement.ScaleOut.Services;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Fanex.Settlement.ScaleOut.Tests.Services
{
    public class ScaleOutSettleThirdPartyServiceTests
    {
        private readonly IDynamicRepository repository;

        private readonly IConfiguration configuration;

        private readonly IMapper mapper;

        private readonly GeneralService generalService;

        private readonly ScaleOutSettleThirdPartyService service;

        public ScaleOutSettleThirdPartyServiceTests()
        {
            repository = Substitute.For<IDynamicRepository>();
            generalService = Substitute.For<GeneralService>();
            var inMemory = new Dictionary<string, string>
            {
                { "SiteDbServer:0", "01" },
                { "SiteDbServer:1", "02" }
            };

            configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemory).Build();
            var mapperConfig = new MapperConfigurationExpression();
            mapperConfig.AddProfile<ScaleOutMappingProfile>();
            var config = new MapperConfiguration(mapperConfig);
            mapper = new Mapper(config);
            service = new ScaleOutSettleThirdPartyServiceImp(repository, configuration, mapper, generalService);
        }

        [Fact]
        public async Task ValidateBetBuilder_Always_CallAndReturnValueAsync()
        {
            await repository.ExecuteAsync(Arg.Do<ValidateBetBuilderCriteria>(item =>
            {
                item.InvalidMarketId = "1";
                item.ErrCode = 1;
                item.ErrMsg = "Error";
            }));

            var result = await service.ValidateBetBuilder(new BetBuilderRequest());

            Assert.Equal(1, result.Item1.ErrorCode);
            Assert.Equal("Error", result.Item1.ErrorMessage);
            Assert.Equal("1", result.Item2);
        }

        [Fact]
        public void UpdateMatchInfo3rd_Always_Execute()
        {
            service.UpdateMatchInfo3rd(new UpdateMatchInfo3rdRequest());

            repository.Received().ExecuteAsync(Arg.Any<UpdateMatchInfo3rdCriteria>());
        }

        [Fact]
        public void GetDeploymentInfo_Always_Execute()
        {
            service.GetDeploymentInfo(new GetDeploymentInfoRequest());

            generalService.Received().ExecuteAsync(Arg.Any<GetDeploymentInfoCriteria>());
        }

        [Fact]
        public async Task ValidateMoveBetDay_ExecutePassed_ExecuteEnoughTimesAsync()
        {
            var result = await service.ValidateMoveBetDay("1", "text");

            await repository.Received(2).ExecuteAsync(Arg.Any<ValidateMoveBetDayCriteria>());
            Assert.Equal(0, result.ErrorCode);
        }

        [Fact]
        public async Task ValidateMoveBetDay_ExecuteFailed_StopAndReturnError()
        {
            await repository.ExecuteAsync(Arg.Do<ValidateMoveBetDayCriteria>(item => item.ErrCode = 1));

            var result = await service.ValidateMoveBetDay("1", "text");

            await repository.Received(1).ExecuteAsync(Arg.Any<ValidateMoveBetDayCriteria>());
            Assert.Equal(1, result.ErrorCode);
            Assert.Equal("Cannot proceed due to this match is in during move-bet progress. Please contact admin for support.", result.ErrorMessage);
        }
    }
}