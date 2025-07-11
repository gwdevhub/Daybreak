﻿namespace Daybreak.Shared.Services.ExecutableManagement;
public interface IGuildWarsExecutableManager
{
    void AddExecutable(string executablePath);
    void RemoveExecutable(string executablePath);
    IEnumerable<string> GetExecutableList();
    bool IsValidExecutable(string executablePath);
}
