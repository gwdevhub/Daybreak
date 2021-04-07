using Daybreak.Exceptions;
using Daybreak.Models;
using Daybreak.Utils;
using System;
using System.Collections.Generic;
using System.Extensions;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daybreak.Services.Logging
{
    public class FlatLoggingDatabase : ILoggingDatabase
    {
        private const string Path = "logs.db";
        private const int bufferSize = 10;
        private const char separator = '§';
        private readonly string filePath = Path;
        private readonly List<string> buffer = new List<string>();

        public FlatLoggingDatabase()
        {
        }

        public Task<IEnumerable<Log>> GetLogs()
        {
            return Task.Run(() =>
            {
                lock (this.buffer)
                {
                    if (this.buffer.Count > 0)
                    {
                        this.WriteBufferToFile();
                    }
                }

                return this.GetSerializedLogs().Select(s => s.Deserialize<Log>());
            });
        }

        public Task<IEnumerable<Log>> GetLogsByDate(DateTime startTime, DateTime endTime)
        {
            return Task.Run(() =>
            {
                lock (this.buffer)
                {
                    if (this.buffer.Count > 0)
                    {
                        this.WriteBufferToFile();
                    }
                }

                return this.GetSerializedLogs()
                    .Select(s => s.Deserialize<Log>())
                    .Where(l => l.Timestamp > startTime && l.Timestamp < endTime);
            });
        }

        public Task<bool> InsertLog(Log log)
        {
            return Task.Run(() =>
            {
                lock (this.buffer)
                {
                    this.buffer.Add(log.Serialize());

                    if (this.buffer.Count > bufferSize)
                    {
                        this.WriteBufferToFile();
                    }
                }

                return true;
            });
        }

        public Task<bool> ClearDatabase()
        {
            return Task.Run(() =>
            {
                lock (this.buffer)
                {
                    this.buffer.Clear();
                }
                lock (this.filePath)
                {
                    File.WriteAllText(this.filePath, string.Empty);
                }

                return true;
            });
        }

        public void OnClosing()
        {
            this.WriteBufferToFile();
        }

        public void OnStartup()
        {
            if (!File.Exists(filePath))
            {
                try
                {
                    using var fs = File.Create(filePath);
                }
                catch (Exception ex)
                {
                    throw new FatalException("Could not initialize logging. See inner exception for details.", ex);
                }
            }
        }

        private void WriteBufferToFile()
        {
            lock (this.filePath)
            {
                File.AppendAllLines(this.filePath, this.buffer.Select(s => s + separator), Encoding.UTF8);
            }
            this.buffer.Clear();
        }

        private IEnumerable<string> GetSerializedLogs()
        {
            lock (this.filePath)
            {
                var stringBuilder = new StringBuilder();
                using var fileStream = File.OpenRead(this.filePath);
                using var streamReader = new StreamReader(fileStream, Encoding.UTF8);
                string serializedLog = null;
                while ((serializedLog = streamReader.ReadUntil(separator.ToString())) != null
                    && serializedLog != string.Empty
                    && serializedLog != "\r"
                    && serializedLog != "\n"
                    && serializedLog != "\r\n")
                {
                    yield return serializedLog;
                }
            }
        }
    }
}
