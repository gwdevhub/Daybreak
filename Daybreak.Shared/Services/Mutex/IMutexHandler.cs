using System.Diagnostics;

namespace Daybreak.Shared.Services.Mutex;

public interface IMutexHandler
{
    void CloseMutex(Process process, string mutexName);
}
