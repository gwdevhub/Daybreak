﻿using Daybreak.Models.Builds;
using Daybreak.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Web.WebView2.Wpf;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Core.Extensions;
using System.Extensions;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Daybreak.Services.IconRetrieve
{
    public sealed class IconBrowser : IIconBrowser
    {
        // Sometimes due to browser issues, retrieved base64 is just a blank jpeg. This is the base64 of the image.
        private const string FaultyBase64 = "/9j/4AAQSkZJRgABAQAAAQABAAD/4gIoSUNDX1BST0ZJTEUAAQEAAAIYAAAAAAQwAABtbnRyUkdCIFhZWiAAAAAAAAAAAAAAAABhY3NwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQAA9tYAAQAAAADTLQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAlkZXNjAAAA8AAAAHRyWFlaAAABZAAAABRnWFlaAAABeAAAABRiWFlaAAABjAAAABRyVFJDAAABoAAAAChnVFJDAAABoAAAAChiVFJDAAABoAAAACh3dHB0AAAByAAAABRjcHJ0AAAB3AAAADxtbHVjAAAAAAAAAAEAAAAMZW5VUwAAAFgAAAAcAHMAUgBHAEIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAFhZWiAAAAAAAABvogAAOPUAAAOQWFlaIAAAAAAAAGKZAAC3hQAAGNpYWVogAAAAAAAAJKAAAA+EAAC2z3BhcmEAAAAAAAQAAAACZmYAAPKnAAANWQAAE9AAAApbAAAAAAAAAABYWVogAAAAAAAA9tYAAQAAAADTLW1sdWMAAAAAAAAAAQAAAAxlblVTAAAAIAAAABwARwBvAG8AZwBsAGUAIABJAG4AYwAuACAAMgAwADEANv/bAEMAAwICAgICAwICAgMDAwMEBgQEBAQECAYGBQYJCAoKCQgJCQoMDwwKCw4LCQkNEQ0ODxAQERAKDBITEhATDxAQEP/bAEMBAwMDBAMECAQECBALCQsQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEP/AABEIAEAAQAMBIgACEQEDEQH/xAAVAAEBAAAAAAAAAAAAAAAAAAAACf/EABQQAQAAAAAAAAAAAAAAAAAAAAD/xAAUAQEAAAAAAAAAAAAAAAAAAAAA/8QAFBEBAAAAAAAAAAAAAAAAAAAAAP/aAAwDAQACEQMRAD8AlUAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//2Q==";
        private const string LargeFaultyBase64 = "/9j/4AAQSkZJRgABAQAAAQABAAD/4gIoSUNDX1BST0ZJTEUAAQEAAAIYAAAAAAQwAABtbnRyUkdCIFhZWiAAAAAAAAAAAAAAAABhY3NwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQAA9tYAAQAAAADTLQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAlkZXNjAAAA8AAAAHRyWFlaAAABZAAAABRnWFlaAAABeAAAABRiWFlaAAABjAAAABRyVFJDAAABoAAAAChnVFJDAAABoAAAAChiVFJDAAABoAAAACh3dHB0AAAByAAAABRjcHJ0AAAB3AAAADxtbHVjAAAAAAAAAAEAAAAMZW5VUwAAAFgAAAAcAHMAUgBHAEIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAFhZWiAAAAAAAABvogAAOPUAAAOQWFlaIAAAAAAAAGKZAAC3hQAAGNpYWVogAAAAAAAAJKAAAA+EAAC2z3BhcmEAAAAAAAQAAAACZmYAAPKnAAANWQAAE9AAAApbAAAAAAAAAABYWVogAAAAAAAA9tYAAQAAAADTLW1sdWMAAAAAAAAAAQAAAAxlblVTAAAAIAAAABwARwBvAG8AZwBsAGUAIABJAG4AYwAuACAAMgAwADEANv/bAEMAAwICAgICAwICAgMDAwMEBgQEBAQECAYGBQYJCAoKCQgJCQoMDwwKCw4LCQkNEQ0ODxAQERAKDBITEhATDxAQEP/bAEMBAwMDBAMECAQECBALCQsQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEP/AABEIAIAAQAMBIgACEQEDEQH/xAAVAAEBAAAAAAAAAAAAAAAAAAAACf/EABQQAQAAAAAAAAAAAAAAAAAAAAD/xAAUAQEAAAAAAAAAAAAAAAAAAAAA/8QAFBEBAAAAAAAAAAAAAAAAAAAAAP/aAAwDAQACEQMRAD8AlUAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//2Q==";
        private const string BaseUrl = "https://wiki.guildwars.com";
        private const string QueryUrl = $"wiki/File:{NamePlaceholder}.jpg";
        private const string NamePlaceholder = "[SKILLNAME]";
        private const string IconsDirectoryName = "Icons";
        private const string IconsLocation = $"{IconsDirectoryName}/{NamePlaceholder}.jpg";

        private readonly ConcurrentQueue<IconRequest> iconRequests = new();
        private readonly ILogger<IconBrowser> logger;
        private WebView2 webView2;
        private CancellationToken cancellationToken;

        public IconBrowser(
            ILogger<IconBrowser> logger)
        {
            this.logger = logger.ThrowIfNull();
        }

        public void InitializeWebView(WebView2 webView2, CancellationToken cancellationToken)
        {
            this.webView2 = webView2.ThrowIfNull();
            this.cancellationToken = cancellationToken;
            Task.Run(this.PeriodicallyServeRequests, cancellationToken);
        }

        public void QueueIconRequest(IconRequest iconRequest)
        {
            this.iconRequests.Enqueue(iconRequest);
        }

        private async Task PeriodicallyServeRequests()
        {
            while(true)
            {
                if (this.cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                if (this.iconRequests.TryDequeue(out var request) is false)
                {
                    await Task.Delay(1000);
                    continue;
                }

                var logger = this.logger.CreateScopedLogger(nameof(this.PeriodicallyServeRequests), request.Skill?.Name);
                logger.LogInformation($"Retrieving icon");
                while(this.webView2 is null)
                {
                    if (this.cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    logger.LogInformation($"Browser is not yet initialized. Waiting");
                    await Task.Delay(1000);
                }
                
                var curedSkillName = request.Skill.AlternativeName.IsNullOrWhiteSpace() ?
                    request.Skill.Name.Replace(" ", "_") :
                    request.Skill.AlternativeName.Replace(" ", "_");
                var skillIconUrl = $"{BaseUrl}/{QueryUrl.Replace(NamePlaceholder, curedSkillName)}";
                logger.LogInformation($"Looking for icon at {skillIconUrl}");

                Application.Current.Dispatcher.Invoke(() =>
                {
                    this.webView2.Source = new Uri(skillIconUrl);
                });

                for(var i = 0; i < 5; i++)
                {
                    if (this.cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    logger.LogInformation("Executing extraction script");
                    var responseTask = await Application.Current.Dispatcher.InvokeAsync(async () =>
                    {
                        return await this.webView2.ExecuteScriptAsync(Scripts.GetHrefFromSkillPage);
                    });
                    var response = await responseTask;
                    logger.LogInformation("Parsing response");
                    var iconPayload = JsonConvert.DeserializeObject<IconPayload>(response);
                    if (iconPayload is null)
                    {
                        logger.LogInformation("Bad response");
                        await Task.Delay(1000);
                        continue;
                    }

                    if (iconPayload.SkillUrl != skillIconUrl.Replace("\"", "%22"))
                    {
                        logger.LogInformation("Retrieved icon doesn't match");
                        await Task.Delay(1000);
                        continue;
                    }

                    var potentialBase64 = iconPayload.SkillImage.Split(',').Skip(1).FirstOrDefault();
                    if (potentialBase64 == FaultyBase64 ||
                        potentialBase64 == LargeFaultyBase64)
                    {
                        logger.LogInformation("Faulty base64 retrieved");
                        await Task.Delay(1000);
                        continue;
                    }

                    byte[] bytes;
                    try
                    {
                        bytes = Convert.FromBase64String(potentialBase64);
                    }
                    catch
                    {
                        logger.LogError("Failed to parse base64");
                        await Task.Delay(1000);
                        continue;
                    }

                    await SaveIconLocally(request.Skill, bytes);
                    request.IconBase64 = potentialBase64;
                    request.Finished = true;
                    break;
                }

                logger.LogError($"Failed to retrieve icon");
                request.Finished = true;
            }
        }

        private static async Task<string> SaveIconLocally(Skill skill, byte[] data)
        {
            var curedSkillName = skill.Name
                .Replace(" ", "_")
                .Replace("\"", "");
            await File.WriteAllBytesAsync(IconsLocation.Replace(NamePlaceholder, curedSkillName), data);
            return curedSkillName;
        }
    }
}