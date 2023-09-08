using Fanex.Settlement.ScaleOut.Services;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace Fanex.Settlement.ScaleOut.Tests.Services;

public class GeneralServiceTests
{
    private readonly IDynamicRepository repository;

    private readonly GeneralService service;

    public GeneralServiceTests()
    {
        repository = Substitute.For<IDynamicRepository>();
        var inMemorySettings = new Dictionary<string, string> {
            {"SiteDbServer:0", "01"},
            {"SiteDbServer:1", "02"}
        };
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
        service = new GeneralServiceImp(configuration, repository);
    }

    [Fact]
    public void ExecuteAsync_Always_CallCorrectDb()
    {
        service.ExecuteAsync(new ExecuteCriteria());

        repository.Received(2).ExecuteAsync(Arg.Any<ExecuteCriteria>());
    }

    [Fact]
    public async Task ExecuteAsync_ExistError_StopAndReturnErrorAsync()
    {
        await repository.ExecuteAsync(Arg.Do<ExecuteCriteria>(item =>
        {
            {
                item.ErrCode = 1;
                item.ErrMsg = "Error";
            }
        }));

        var result = await service.ExecuteAsync(new ExecuteCriteria());

        await repository.Received(1).ExecuteAsync(Arg.Any<ExecuteCriteria>());
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