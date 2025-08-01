using Daybreak.Services.ApplicationArguments;
using Daybreak.Tests.Services.Models;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Slim;

namespace Daybreak.Tests.Services;

[TestClass]
public class ApplicationArgumentServiceTests
{
    private readonly ServiceManager serviceManager = new();
    private readonly ApplicationArgumentService applicationArgumentService;

    public ApplicationArgumentServiceTests()
    {
        this.applicationArgumentService = new ApplicationArgumentService(this.serviceManager, Substitute.For<ILogger<ApplicationArgumentService>>());
    }

    [TestMethod]
    public void HandleArguments_ExpectedArguments_HandlesCorrectly()
    {
        this.serviceManager.RegisterSingleton<TestArgumentHandler>();
        this.serviceManager.RegisterSingleton<TestArgumentHandler2>();

        this.applicationArgumentService.HandleArguments("-r so me arg -c uments he re and there".Split(" "));
        
        this.serviceManager.GetRequiredService<TestArgumentHandler>().Called.Should().BeTrue();
        this.serviceManager.GetRequiredService<TestArgumentHandler2>().Called.Should().BeTrue();
    }

    [TestMethod]
    public void HandleArguments_NotEnoughArguments_DoesNotCallHandlers()
    {
        this.serviceManager.RegisterScoped<TestArgumentHandler>();
        this.serviceManager.RegisterScoped<TestArgumentHandler2>();

        this.applicationArgumentService.HandleArguments("-r so me arg -c uments he re".Split(" "));

        this.serviceManager.GetRequiredService<TestArgumentHandler>().Called.Should().BeTrue();
        this.serviceManager.GetRequiredService<TestArgumentHandler2>().Called.Should().BeFalse();
    }

    [TestMethod]
    public void RegisterHandler_RegistersHandler()
    {
        this.applicationArgumentService.RegisterArgumentHandler<TestArgumentHandler>();

        var action = this.serviceManager.GetRequiredService<TestArgumentHandler>;

        action.Should().NotThrow();
    }
}
