using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Api;
using Daybreak.Shared.Models.Builds;
using System.Diagnostics.CodeAnalysis;

namespace Daybreak.Shared.Services.BuildTemplates;

public interface IBuildTemplateManager
{
    SingleBuildEntry ConvertToSingleBuildEntry(TeamBuildEntry teamBuildEntry);
    TeamBuildEntry ConvertToTeamBuildEntry(SingleBuildEntry singleBuildEntry);
    bool IsTemplate(string template);
    bool CanTemplateApply(BuildTemplateValidationRequest request);
    SingleBuildEntry CreateSingleBuild();
    SingleBuildEntry CreateSingleBuild(string name);
    SingleBuildEntry CreateSingleBuild(BuildEntry buildEntry);
    SingleBuildEntry CreateSingleBuild(string name, BuildEntry buildEntry);
    TeamBuildEntry CreateTeamBuild();
    TeamBuildEntry CreateTeamBuild(string name);
    TeamBuildEntry CreateTeamBuild(PartyLoadout partyLoadout);
    TeamBuildEntry CreateTeamBuild(PartyLoadout partyLoadout, string name);
    PartyLoadout ConvertToPartyLoadout(TeamBuildEntry teamBuildEntry);
    BuildEntry ConvertToBuildEntry(SingleBuildEntry singleBuildEntry);
    bool CanApply(MainPlayerBuildContext mainPlayerBuildContext, TeamBuildEntry teamBuildEntry);
    bool CanApply(MainPlayerBuildContext mainPlayerBuildContext, SingleBuildEntry singleBuildEntry);
    void ClearBuilds();
    void SaveBuild(IBuildEntry buildEntry);
    void RemoveBuild(IBuildEntry buildEntry);
    IAsyncEnumerable<IBuildEntry> GetBuilds();
    Task<IBuildEntry?> GetBuild(string name);
    IBuildEntry DecodeTemplate(string template);
    bool TryDecodeTemplate(string template, [NotNullWhen(true)] out IBuildEntry? build);
    string EncodeTemplate(IBuildEntry buildEntry);
}
