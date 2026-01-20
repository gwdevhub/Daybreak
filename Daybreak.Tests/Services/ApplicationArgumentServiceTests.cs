using Daybreak.Services.ApplicationArguments;
using Daybreak.Shared.Services.ApplicationArguments.ArgumentHandling;
using Daybreak.Tests.Services.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Extensions;

namespace Daybreak.Tests.Services;

[TestClass]
public class ApplicationArgumentServiceTests
{
    private readonly List<IArgumentHandler> argumentHandlers = [new TestArgumentHandler(), new TestArgumentHandler2()];
    private readonly ApplicationArgumentService applicationArgumentService;

    public ApplicationArgumentServiceTests()
    {
        this.applicationArgumentService = new ApplicationArgumentService(this.argumentHandlers, Substitute.For<ILogger<ApplicationArgumentService>>());
    }

    [TestMethod]
    public void HandleArguments_ExpectedArguments_HandlesCorrectly()
    {
        this.applicationArgumentService.HandleArguments("-r so me arg -c uments he re and there".Split(" "));
        
        this.argumentHandlers[0].Cast<TestArgumentHandler>().Called.Should().BeTrue();
        this.argumentHandlers[1].Cast<TestArgumentHandler2>().Called.Should().BeTrue();
    }

    [TestMethod]
    public void HandleArguments_NotEnoughArguments_DoesNotCallHandlers()
    {
        this.applicationArgumentService.HandleArguments("-r so me arg -c uments he re".Split(" "));

        this.argumentHandlers[0].Cast<TestArgumentHandler>().Called.Should().BeTrue();
        this.argumentHandlers[1].Cast<TestArgumentHandler2>().Called.Should().BeFalse();
    }
}
