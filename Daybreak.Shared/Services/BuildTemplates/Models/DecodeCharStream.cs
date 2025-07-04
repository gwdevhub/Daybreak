﻿namespace Daybreak.Shared.Services.BuildTemplates.Models;

internal sealed class DecodeCharStream(string[] encodedValues)
{
    private readonly string innerCharArray = string.Join("", encodedValues);

    public int Length => this.innerCharArray.Length;

    public int Position { get; set; }

    public int Read(int count)
    {
        var value = FromEncodedBinary(this.innerCharArray, this.Position, count);
        this.Position += count;
        return value;
    }

    private static int FromEncodedBinary(string encoded, int startIndex, int count)
    {
        var sum = 0d;
        for (int i = startIndex; i < startIndex + count; i++)
        {
            sum += encoded[i] == '1' ? Math.Pow(2, i - startIndex) : 0;
        }

        return (int)sum;
    }
}
