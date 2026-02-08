namespace Daybreak.Shared.Utils;

/// <summary>
/// Platform-agnostic resize direction enum.
/// On Windows, maps to Win32 HT* hit-test values.
/// On Linux, used as logical direction identifiers.
/// </summary>
public enum ResizeDirection
{
    Left = 10,
    Right = 11,
    Top = 12,
    TopLeft = 13,
    TopRight = 14,
    Bottom = 15,
    BottomLeft = 16,
    BottomRight = 17
}
