using System.Collections.Generic;
using System.Windows.Media;

namespace Daybreak.Shared.Models;
public static class ColorPalette
{
    public readonly static Color Red = Color.FromArgb(255, 213, 0, 0);
    public readonly static Color Pink = Color.FromArgb(255, 197, 17, 98);
    public readonly static Color Purple = Color.FromArgb(255, 170, 0, 255);
    public readonly static Color DeepPurple = Color.FromArgb(255, 98, 0, 234);
    public readonly static Color Indigo = Color.FromArgb(255, 48, 79, 254);
    public readonly static Color Blue = Color.FromArgb(255, 41, 98, 255);
    public readonly static Color LightBlue = Color.FromArgb(255, 0, 145, 234);
    public readonly static Color Cyan = Color.FromArgb(255, 0, 184, 212);
    public readonly static Color Teal = Color.FromArgb(255, 0, 191, 165);
    public readonly static Color Yellow = Color.FromArgb(255, 255, 214, 0);
    public readonly static Color Amber = Color.FromArgb(255, 255, 171, 0);
    public readonly static Color Orange = Color.FromArgb(255, 255, 109, 0);
    public readonly static Color DeepOrange = Color.FromArgb(255, 221, 44, 0);
    public readonly static Color Green = Color.FromArgb(255, 56, 142, 60);
    public readonly static Color Lime = Color.FromArgb(255, 174, 234, 0);
    public readonly static Color Magenta = Color.FromArgb(255, 213, 0, 249);
    public readonly static Color Gold = Color.FromArgb(255, 255, 241, 118);
    public readonly static Color BlueGrey = Color.FromArgb(255, 96, 125, 139);
    public readonly static Color Grey = Color.FromArgb(255, 158, 158, 158);

    public readonly static List<Color> Colors =
    [
        Pink,
        Amber,
        Purple,
        DeepPurple,
        Yellow,
        Indigo,
        Orange,
        Blue,
        DeepOrange,
        LightBlue,
        Red,
        Cyan,
        Teal,
        Green,
        Lime,
        Magenta,
        Gold,
        BlueGrey,
        Grey
    ];
}
