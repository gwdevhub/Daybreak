using System;
using System.Windows.Media;

namespace Daybreak.Services.Images;

public interface IImageCache
{
    ImageSource? GetImage(string? uri);
}
