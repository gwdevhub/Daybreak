using Daybreak.Utils;
using System.IO;

namespace Daybreak.Services.Startup.Actions;
internal sealed class DeleteOldDatabase : StartupActionBase
{
    public override void ExecuteOnStartup()
    {
        var oldDbPath = PathUtils.GetAbsolutePathFromRoot("daybreak.db");
        if (File.Exists(oldDbPath))
        {
            File.Delete(oldDbPath);
        }
    }
}
