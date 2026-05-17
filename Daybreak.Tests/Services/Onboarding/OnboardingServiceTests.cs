using Daybreak.Services.Onboarding;
using Daybreak.Shared.Models;
using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Models.Onboarding;
using Daybreak.Shared.Services.Credentials;
using Daybreak.Shared.Services.ExecutableManagement;
using Daybreak.Shared.Services.LaunchConfigurations;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Daybreak.Tests.Services.Onboarding;

[TestClass]
public sealed class OnboardingServiceTests
{
    private readonly ICredentialManager credentialManager = Substitute.For<ICredentialManager>();
    private readonly IGuildWarsExecutableManager executableManager = Substitute.For<IGuildWarsExecutableManager>();
    private readonly ILaunchConfigurationService launchConfigurationService = Substitute.For<ILaunchConfigurationService>();
    private readonly OnboardingService service;

    public OnboardingServiceTests()
    {
        this.service = new OnboardingService(
            this.credentialManager,
            this.executableManager,
            this.launchConfigurationService,
            Substitute.For<ILogger<OnboardingService>>());
    }

    [TestMethod]
    public void CheckOnboardingStage_NoCredentials_ReturnsNeedsCredentials()
    {
        this.credentialManager.GetCredentialList().Returns([]);

        this.service.CheckOnboardingStage().Should().Be(LauncherOnboardingStage.NeedsCredentials);
    }

    [TestMethod]
    public void CheckOnboardingStage_NoExecutable_ReturnsNeedsExecutable()
    {
        this.credentialManager.GetCredentialList().Returns([new LoginCredentials { Identifier = "x" }]);
        this.executableManager.GetExecutableList().Returns([]);

        this.service.CheckOnboardingStage().Should().Be(LauncherOnboardingStage.NeedsExecutable);
    }

    [TestMethod]
    public void CheckOnboardingStage_NoLaunchConfiguration_ReturnsNeedsConfiguration()
    {
        this.credentialManager.GetCredentialList().Returns([new LoginCredentials { Identifier = "x" }]);
        this.executableManager.GetExecutableList().Returns(["/bin/gw.exe"]);
        this.launchConfigurationService.GetLaunchConfigurations().Returns([]);

        this.service.CheckOnboardingStage().Should().Be(LauncherOnboardingStage.NeedsConfiguration);
    }

    [TestMethod]
    public void CheckOnboardingStage_AllConfigured_ReturnsComplete()
    {
        this.credentialManager.GetCredentialList().Returns([new LoginCredentials { Identifier = "x" }]);
        this.executableManager.GetExecutableList().Returns(["/bin/gw.exe"]);
        this.launchConfigurationService.GetLaunchConfigurations().Returns([new LaunchConfigurationWithCredentials { Identifier = "1" }]);

        this.service.CheckOnboardingStage().Should().Be(LauncherOnboardingStage.Complete);
    }
}
