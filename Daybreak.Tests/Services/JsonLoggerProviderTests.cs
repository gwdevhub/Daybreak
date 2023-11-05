using Daybreak.Configuration.Options;
using Daybreak.Services.Logging;
using FluentAssertions;
using LiteDB;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Logging;

namespace Daybreak.Tests.Services;

[TestClass]
public class JsonLoggerProviderTests
{
    private ILogsManager logsManager;
    private ILoggerProvider loggerProvider;
    private ILiteDatabase liteDatabase;

    [TestInitialize]
    public void InitializeProvider()
    {
        File.Delete("Daybreak.db");
        this.liteDatabase = new LiteDatabase("Daybreak.db");
        var options = Substitute.For<ILiveOptions<LauncherOptions>>();
        options.Value.Returns(new LauncherOptions { PersistentLogging = true });
        this.logsManager = new JsonLogsManager(this.liteDatabase.GetCollection<Daybreak.Models.Log>(), options);
        this.loggerProvider = new CVLoggerProvider(this.logsManager);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        this.liteDatabase.Dispose();
    }

    [TestMethod]
    public void CreateLoggerReturnsLogger()
    {
        var logger = this.loggerProvider.CreateLogger("SomeCategory");
        logger.Should().NotBeNull();
    }
    [TestMethod]
    public void LoggerLogsAndReaderReadsFiltered()
    {
        var logger = this.loggerProvider.CreateLogger("SomeCategory");
        logger.LogTrace("Logging some trace");
        logger.LogInformation("Logging some stuff");
        logger.LogError("Logging some error");

        this.logsManager.GetLogs(l => l.LogLevel < LogLevel.Information).Should().HaveCount(1);
        var log = this.logsManager.GetLogs(l => l.LogLevel < LogLevel.Information).First();
        log.LogLevel.Should().Be(LogLevel.Error);
    }
    [TestMethod]
    public void LoggerLogsAndReaderReads()
    {
        var logger = this.loggerProvider.CreateLogger("SomeCategory");
        logger.LogInformation("Logging some stuff");
        logger.LogError("Logging some error");

        this.logsManager.GetLogs().Should().HaveCount(2);
    }
    [TestMethod]
    public void DeletingLogsShouldDeleteLogs()
    {
        var logger = this.loggerProvider.CreateLogger("SomeCategory");
        logger.LogInformation("Logging some stuff");
        logger.LogError("Logging some error");

        this.logsManager.GetLogs().Should().HaveCount(2);

        this.logsManager.DeleteLogs();
        this.logsManager.GetLogs().Should().HaveCount(0);
    }
}
