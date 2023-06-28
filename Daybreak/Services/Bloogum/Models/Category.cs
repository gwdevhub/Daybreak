using Daybreak.Models.Guildwars;

namespace Daybreak.Services.Bloogum.Models;

public sealed class Category
{
    public Map Map { get; }
    public string CategoryName { get; }
    public int ImageCount { get; }

    public Category(Map map, string categoryName, int imageCount)
    {
        this.Map = map;
        this.CategoryName = categoryName;
        this.ImageCount = imageCount;
    }
}
