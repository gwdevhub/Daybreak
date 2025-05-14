using Daybreak.Shared.Models.Builds;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Daybreak.Shared.Models.Guildwars;

public abstract class BuildEntryBase : INotifyPropertyChanged, IBuildEntry
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public Dictionary<string, string>? Metadata { get; set; }
    public string? PreviousName { get; set; }
    public int? ToolboxBuildId
    {
        get
        {
            if (this.Metadata?.TryGetValue(nameof(this.ToolboxBuildId), out var toolBoxBuildString) is true &&
                int.TryParse(toolBoxBuildString, out var toolboxBuild))
            {
                return toolboxBuild;
            }

            return default;
        }
        set
        {
            this.Metadata ??= [];
            this.Metadata[nameof(this.ToolboxBuildId)] = value.HasValue ? value.Value.ToString() : string.Empty;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.ToolboxBuildId)));
        }
    }
    public bool IsToolboxBuild
    {
        get
        {
            if (this.Metadata?.TryGetValue(nameof(this.IsToolboxBuild), out var toolBoxBuildString) is true &&
                bool.TryParse(toolBoxBuildString, out var toolboxBuild))
            {
                return toolboxBuild;
            }

            return false;
        }
        set
        {
            this.Metadata ??= [];
            this.Metadata[nameof(this.IsToolboxBuild)] = value.ToString();
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IsToolboxBuild)));
        }
    }
    public DateTimeOffset CreationTime
    {
        get
        {
            if (this.Metadata?.TryGetValue(nameof(this.CreationTime), out var creationTimeString) is true &&
                int.TryParse(creationTimeString, out var creationTimeUnix))
            {
                return DateTimeOffset.FromUnixTimeSeconds(creationTimeUnix);
            }

            return DateTimeOffset.MinValue;
        }
        set
        {
            this.Metadata ??= [];
            this.Metadata[nameof(this.CreationTime)] = value.ToUniversalTime().ToUnixTimeSeconds().ToString();
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CreationTime)));
        }
    }
    public string? Name
    {
        get;
        set
        {
            field = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Name)));
        }
    }
    public string? SourceUrl
    {
        get
        {
            if (this.Metadata?.TryGetValue(nameof(this.SourceUrl), out var sourceUrl) is true)
            {
                return sourceUrl;
            }

            return default;
        }
        set
        {
            this.Metadata ??= [];
            this.Metadata[nameof(this.SourceUrl)] = value ?? string.Empty;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.SourceUrl)));
        }
    }
    public List<PartyCompositionMetadataEntry>? PartyComposition
    {
        get
        {
            if (this.Metadata?.TryGetValue(nameof(this.PartyComposition), out var serializedPartyComposition) is true &&
                !string.IsNullOrWhiteSpace(serializedPartyComposition))
            {
                return JsonConvert.DeserializeObject<List<PartyCompositionMetadataEntry>>(serializedPartyComposition);
            }

            return default;
        }
        set
        {
            this.Metadata ??= [];
            this.Metadata[nameof(PartyCompositionMetadataEntry)] = value is null 
                ? string.Empty
                : JsonConvert.SerializeObject(value, Formatting.None);
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.PartyComposition)));
        }
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
