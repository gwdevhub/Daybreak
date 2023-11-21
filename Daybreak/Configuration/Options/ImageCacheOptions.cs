using Daybreak.Attributes;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Memory Cache")]
internal sealed class ImageCacheOptions
{
    [OptionName(Name = "Image Cache Limit", Description = "The maximum number of MBs that will be used to cache images in memory")]
    [OptionRange<double>(MinValue = 0d, MaxValue = 1000d)]
    public double MemoryImageCacheLimit { get; set; } = 100;
}
