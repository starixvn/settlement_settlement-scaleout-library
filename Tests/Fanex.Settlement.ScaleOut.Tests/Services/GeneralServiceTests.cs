using Fanex.Settlement.ScaleOut.Services;
using Fanex.SiteDbConnectionManager.Client.Models;
using Fanex.SiteDbConnectionManager.Client.Services;
using NSubstitute;

namespace Fanex.Settlement.ScaleOut.Tests.Services;

public class GeneralServiceTests
{
    private readonly IDynamicRepository repository;
    private readonly SiteDbClientService siteDbClientService;
    private readonly GeneralServiceImp service;

    public GeneralServiceTests()
    {
        repository = Substitute.For<IDynamicRepository>();
        siteDbClientService = Substitute.For<SiteDbClientService>();
        service = new GeneralServiceImp(repository, siteDbClientService);
    }

    [Fact]
    public async Task ExecuteAsync_Always_CallCorrectDb()
    {
        var sites = new List<SiteDb> {
            new SiteDb { SiteName = "01", Status = true },
            new SiteDb { SiteName = "02", Status = true }
        };
        siteDbClientService.GetActiveSiteDbAsync().Returns(sites);

        await service.ExecuteAsync(new ExecuteCriteria());

        await repository.Received(2).ExecuteAsync(Arg.Any<ExecuteCriteria>());
    }

    [Fact]
    public async Task ExecuteAsync_ExistError_StopAndReturnErrorAsync()
    {
        var sites = new List<SiteDb> {
            new SiteDb { SiteName = "01", Status = true },
            new SiteDb { SiteName = "02", Status = true }
        };
        siteDbClientService.GetActiveSiteDbAsync().Returns(sites);

        await repository.ExecuteAsync(Arg.Do<ExecuteCriteria>(item =>
        {
            item.ErrCode = 1;
            item.ErrMsg = "Error";
        }));

        var result = await service.ExecuteAsync(new ExecuteCriteria());

        await repository.Received(2).ExecuteAsync(Arg.Any<ExecuteCriteria>());
        Assert.Equal(1, result.ErrorCode);
        Assert.Equal("Error", result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteSingle_Always_Execute()
    {
        await repository.ExecuteAsync(Arg.Do<ExecuteSingleCriteria>(item => item.ErrCode = 1));

        var result = await service.ExecuteSingle(new ExecuteSingleCriteria());

        Assert.Equal(1, result.ErrorCode);
    }
}