#pragma warning disable CS0436 // Type conflicts with imported type
using System.Extensions;

[assembly: GenerateFixedArray<uint>(Size = 4)]
[assembly: GenerateFixedArray<char>(Size = 0x14)]
[assembly: GenerateFixedArray<char>(Size = 0x40)]
[assembly: GenerateFixedArray<byte>(Size = 0x18)]
[assembly: GenerateFixedArray<char>(Size = 256)]
[assembly: GenerateFixedArray<char>(Size = 32)]
[assembly: GenerateFixedArray<char>(Size = 5)]
[assembly: GenerateFixedArray<char>(Size = 8)]
[assembly: GenerateFixedArray<byte>(Size = 56)]
[assembly: GenerateFixedArray<uint>(Size = 17)]
[assembly: GenerateFixedArray<uint>(Size = 12)]
[assembly: GenerateFixedArray<uint>(Size = 8)]
#pragma warning restore CS0436 // Type conflicts with imported type

namespace Daybreak.API.Interop;

public static class FixedArrays
{
}
