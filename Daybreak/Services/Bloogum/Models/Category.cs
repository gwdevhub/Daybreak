namespace Daybreak.Services.Bloogum.Models;

public sealed class Category
{
    public string CategoryName { get; }
    public int ImageCount { get; }

    public Category(string categoryName, int imageCount)
    {
        this.CategoryName = categoryName;
        this.ImageCount = imageCount;
    }
}
