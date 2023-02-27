using Daybreak.Configuration;
using Daybreak.Services.Configuration;
using Daybreak.Services.Options;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Daybreak.Tests.Services;

[TestClass]
public class ApplicationConfigurationOptionsManagerTests
{
    private ApplicationConfigurationOptionsManager applicationConfigurationOptionsManager;

    [TestInitialize]
    public void TestInitialize()
    {
        var configurationManagerMock = new Mock<IConfigurationManager>();
        configurationManagerMock
            .Setup(u => u.GetConfiguration())
            .Returns(new ApplicationConfiguration());

        this.applicationConfigurationOptionsManager = new ApplicationConfigurationOptionsManager(configurationManagerMock.Object);
    }

    [TestMethod]
    public void GetApplicationConfiguration_ReturnsObject()
    {
        var config = this.applicationConfigurationOptionsManager.GetOptions<ApplicationConfiguration>();

        config.Should().NotBeNull();
    }

    [TestMethod]
    public void GetOtherOptions_ThrowsInvalidOperationException()
    {
        var action = new Action(() =>
        {
            this.applicationConfigurationOptionsManager.GetOptions<object>();
        });

        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void UpdateOptions_OnApplicationConfiguration_Succeeds()
    {
        this.applicationConfigurationOptionsManager.UpdateOptions(new ApplicationConfiguration());
    }

    [TestMethod]
    public void UpdateOptions_OnOthers_ThrowsInvalidOperationException()
    {
        var action = new Action(() =>
        {
            this.applicationConfigurationOptionsManager.UpdateOptions(new object());
        });

        action.Should().Throw<InvalidOperationException>();
    }
}
