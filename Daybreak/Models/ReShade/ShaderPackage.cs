using System.Collections.Generic;

namespace Daybreak.Models.ReShade;

public sealed class ShaderPackage
{
    public bool Enabled { get; set; }
    public bool Required { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? InstallPath { get; set; }
    public string? TextureInstallPath { get; set; }
    public string? DownloadUrl { get; set; }
    public string? RepositoryUrl { get; set; }
    public List<string>? EffectFiles { get; set; }
}
