using Daybreak.Models;
using System.Diagnostics;

namespace Daybreak.Services.Scanner;

public interface IGuildwarsMemoryReader
{
    bool Running { get; }
    GameData? GameData { get; }
    Process? TargetProcess { get; }
    void Initialize(Process process);
    void Stop();
}
