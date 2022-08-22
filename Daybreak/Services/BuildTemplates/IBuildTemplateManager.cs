using Daybreak.Models.Builds;
using System.Collections.Generic;

namespace Daybreak.Services.BuildTemplates
{
    public interface IBuildTemplateManager
    {
        bool IsTemplate(string template);
        BuildEntry CreateBuild();
        BuildEntry CreateBuild(string name);
        void ClearBuilds();
        void SaveBuild(BuildEntry buildEntry);
        void RemoveBuild(BuildEntry buildEntry);
        IAsyncEnumerable<BuildEntry> GetBuilds();
        Build DecodeTemplate(string template);
        bool TryDecodeTemplate(string template, out Build build);
        string EncodeTemplate(Build build);
    }
}
