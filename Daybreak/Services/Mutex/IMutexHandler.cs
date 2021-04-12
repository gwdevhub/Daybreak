using System.Diagnostics;

namespace Daybreak.Services.Mutex
{
    public interface IMutexHandler
    {
        void CloseMutex(Process process, string mutexName);
    }
}
