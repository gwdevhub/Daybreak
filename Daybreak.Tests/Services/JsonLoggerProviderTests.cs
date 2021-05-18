using Daybreak.Services.Logs;
using FluentAssertions;
using LiteDB;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Daybreak.Tests.Services
{
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
            this.logsManager = new JsonLogsManager(this.liteDatabase);
            this.loggerProvider = new JsonLoggerProvider(this.logsManager);
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
}
