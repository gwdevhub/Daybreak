using Daybreak.Models;
using Daybreak.Models.Guildwars;
using Microsoft.Extensions.Logging;
using System;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.Text;
using System.Threading;

namespace Daybreak.Services.Scanner;

public sealed class GuildwarsMemoryReader : IGuildwarsMemoryReader, IDisposable
{
    private readonly IMemoryScanner memoryScanner;
    private readonly ILogger<GuildwarsMemoryReader> logger;

    private volatile CancellationTokenSource? cancellationTokenSource;

    public bool Running => this.cancellationTokenSource is not null && this.cancellationTokenSource.IsCancellationRequested is false;

    public GameData? GameData { get; private set; }

    public Process? TargetProcess => this.memoryScanner.Process;

    public GuildwarsMemoryReader(
        IMemoryScanner memoryScanner,
        ILogger<GuildwarsMemoryReader> logger)
    {
        this.memoryScanner = memoryScanner.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }
    
    public void Initialize(Process process)
    {
        var scoppedLogger = this.logger.CreateScopedLogger(nameof(this.Initialize), default);
        if (process is null)
        {
            scoppedLogger.LogWarning($"Process is null. {nameof(GuildwarsMemoryReader)} will not start");
            return;
        }

        scoppedLogger.LogInformation($"Initializing {nameof(GuildwarsMemoryReader)}");
        if (this.memoryScanner.Process?.MainModule?.FileName != process?.MainModule?.FileName)
        {
            if (this.memoryScanner.Scanning)
            {
                scoppedLogger.LogInformation("Scanner is already scanning a different process. Restart scanner and target the new process");
                this.memoryScanner.EndScanner();
            }
            
            scoppedLogger.LogInformation("Initializing scanner");
            this.memoryScanner.BeginScanner(process!);
        }

        if (!this.memoryScanner.Scanning)
        {
            scoppedLogger.LogInformation("Initializing scanner");
            this.memoryScanner.BeginScanner(process!);
        }

        this.cancellationTokenSource?.Cancel();
        this.PeriodicallyReadGuildwarsMemory();
    }
    
    public void Stop()
    {
        var scoppedLogger = this.logger.CreateScopedLogger(nameof(this.Stop), default);
        scoppedLogger.LogInformation($"Stopping {nameof(GuildwarsMemoryReader)}");
        this.cancellationTokenSource?.Cancel();
    }

    private void PeriodicallyReadGuildwarsMemory()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        this.cancellationTokenSource = cancellationTokenSource;
        TaskExtensions.RunPeriodicAsync(() => this.ReadGameMemory(cancellationTokenSource.Token), TimeSpan.Zero, TimeSpan.FromSeconds(1), cancellationTokenSource.Token);
    }

    private void ReadGameMemory(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }
        
        /*
         * The following offsets were reverse-engineered using pointer scanning. All of the following ones seem to currently work.
         * If any breaks, try the other ones from the list below.
         * startAddress + 0061C118 -> +0x0 -> +0x18
         * startAddress + 00629244 -> +0xC -> +0xC
         * startAddress + 0062971C -> +0xC -> +0xC
         * startAddress + 00AA9D58 -> +0xC -> +0xC
         * startAddress + 00AA9D58 -> +0x0 -> +0xC -> +0xC
         */
        var gameContext = this.memoryScanner.ReadPtrChain<GameContext>(this.memoryScanner.ModuleStartAddress, finalPointerOffset: 0x0, 0x00629244, 0xC, 0xC);

        // WorldContext struct is offset by 0x528 due to the memory layout of the structure.
        var worldContext = this.memoryScanner.Read<WorldContext>(gameContext.WorldContext + WorldContext.BaseOffset);
        // CharContext struct is offset by 0x074 due to the memory layout of the structure.
        var charContext = this.memoryScanner.Read<CharContext>(gameContext.CharContext + CharContext.BaseOffset);

        var email = ParseAndCleanWCharArray(charContext.PlayerEmailBytes);
        var name = ParseAndCleanWCharArray(charContext.PlayerNameBytes);
        _ = Quest.TryParse((int)worldContext.QuestId, out var quest);

        this.GameData = new GameData
        {
            CharacterName = name,
            Email = email,
            Quest = quest,
            HardModeUnlocked = worldContext.HardModeUnlocked == 1,
            Level = worldContext.Level,
            Morale = worldContext.Morale,
            Experience = worldContext.Experience,
            CurrentBalthazarPoints = worldContext.CurrentBalthazar,
            CurrentImperialPoints = worldContext.CurrentImperial,
            CurrentKurzickPoints = worldContext.CurrentKurzick,
            CurrentLuxonPoints = worldContext.CurrentLuxon,
            TotalBalthazarPoints = worldContext.TotalBalthazar,
            TotalImperialPoints = worldContext.TotalImperial,
            TotalKurzickPoints = worldContext.TotalKurzick,
            TotalLuxonPoints = worldContext.TotalLuxon,
            MaxBalthazarPoints = worldContext.MaxBalthazar,
            MaxImperialPoints = worldContext.MaxImperial,
            MaxKurzickPoints = worldContext.MaxKurzick,
            MaxLuxonPoints = worldContext.MaxLuxon,
            CurrentSkillPoints = worldContext.CurrentSkillPoints,
            TotalSkillPoints = worldContext.TotalSkillPoints,
            FoesKilled = worldContext.FoesKilled,
            FoesToKill = worldContext.FoesToKill
        };
    }

    private static string ParseAndCleanWCharArray(byte[] bytes)
    {
        var str = Encoding.Unicode.GetString(bytes);
        var indexOfNull = str.IndexOf('\0');
        if (indexOfNull > 0)
        {
            str = str[..indexOfNull];
        }

        return str;
    }

    public void Dispose()
    {
        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource?.Dispose();
        this.cancellationTokenSource = null;
    }
}
