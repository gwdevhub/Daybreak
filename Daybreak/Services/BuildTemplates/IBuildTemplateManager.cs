using Daybreak.Models.Builds;
using Daybreak.Models.Guildwars;
using System;
using System.Collections.Generic;
using System.Extensions;
using System.Threading.Tasks;

namespace Daybreak.Services.BuildTemplates;

public interface IBuildTemplateManager
{
    SingleBuildEntry ConvertToSingleBuildEntry(TeamBuildEntry teamBuildEntry);
    TeamBuildEntry ConvertToTeamBuildEntry(SingleBuildEntry singleBuildEntry);
    bool IsTemplate(string template);
    SingleBuildEntry CreateSingleBuild();
    SingleBuildEntry CreateSingleBuild(string name);
    TeamBuildEntry CreateTeamBuild();
    TeamBuildEntry CreateTeamBuild(string name);
    TeamBuildEntry CreateTeamBuild(TeamBuildData teamBuildData);
    TeamBuildEntry CreateTeamBuild(TeamBuildData teamBuildData, string name);
    void ClearBuilds();
    void SaveBuild(IBuildEntry buildEntry);
    void RemoveBuild(IBuildEntry buildEntry);
    IAsyncEnumerable<IBuildEntry> GetBuilds();
    Task<Result<IBuildEntry, Exception>> GetBuild(string name);
    IBuildEntry DecodeTemplate(string template);
    bool TryDecodeTemplate(string template, out IBuildEntry build);
    string EncodeTemplate(IBuildEntry buildEntry);
}
