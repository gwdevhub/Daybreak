using System.Threading.Tasks;

namespace Daybreak.Updater
{
    internal class Program
    {
        internal static ReleaseClient releaseClient = new();
        public async static Task<int> Main(string[] args)
        {
            if (args.Length == 0)
            {
                return -1;
            }

            switch (args[0])
            {
                case "-check":
                    return await releaseClient.CheckRelease(args[1]) ? 1 : 0;
                case "-update":
                    await releaseClient.Update();
                    return 0;
            }

            return -1;
        }
    }
}
