using Daybreak.Models.Builds;
using System.Collections.Generic;

namespace Daybreak.Services.BuildTemplates
{
    public interface IBuildTemplateManager
    {
        BuildEntry CreateBuild();
        BuildEntry CreateBuild(string name);
        void SaveBuild(BuildEntry buildEntry);
        void RemoveBuild(BuildEntry buildEntry);
        IEnumerable<BuildEntry> GetBuilds();
        Build DecodeTemplate(string template);
        string EncodeTemplate(Build build);
    }
}
