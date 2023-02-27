using Daybreak.Models.Builds;
using Daybreak.Models.Guildwars;
using System;
using System.Collections.Generic;
using System.Extensions;
using System.Threading.Tasks;

namespace Daybreak.Services.BuildTemplates;

public interface IBuildTemplateManager
{
    bool IsTemplate(string template);
    BuildEntry CreateBuild();
    BuildEntry CreateBuild(string name);
    void ClearBuilds();
    void SaveBuild(BuildEntry buildEntry);
    void RemoveBuild(BuildEntry buildEntry);
    IAsyncEnumerable<BuildEntry> GetBuilds();
    Task<Result<BuildEntry, Exception>> GetBuild(string name);
    Build DecodeTemplate(string template);
    bool TryDecodeTemplate(string template, out Build build);
    string EncodeTemplate(Build build);
}
