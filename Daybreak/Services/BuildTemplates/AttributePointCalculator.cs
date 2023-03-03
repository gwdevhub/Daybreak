﻿using Daybreak.Models.Guildwars;
using System;
using System.Collections.Generic;

namespace Daybreak.Services.BuildTemplates;

/// <summary>
/// Attribute point calculator.
/// Based on https://wiki.guildwars.com/wiki/Attribute_point.
/// </summary>
public sealed class AttributePointCalculator : IAttributePointCalculator
{
    private const int MaxRank = 12;
    private const int MinRank = 0;

    private static readonly List<int> PointsRequiredToIncreaseRankMapping = new()
    {
        1, 2, 3, 4, 5, 6, 7, 9, 11, 13, 16, 20, int.MaxValue
    };

    public int MaximumAttributePoints => 200;

    public int GetPointsRequiredToIncreaseRank(int currentRank)
    {
        if (currentRank < MinRank ||
            currentRank > MaxRank)
        {
            throw new ArgumentException($"Current rank must be between {MinRank} and {MaxRank}");
        }

        return PointsRequiredToIncreaseRankMapping[currentRank];
    }

    public int GetRemainingFreePoints(Build build)
    {
        return this.MaximumAttributePoints - this.GetUsedPoints(build);
    }
    
    public int GetUsedPoints(Build build)
    {
        var totalPoints = 0;
        foreach(var attribute in build.Attributes)
        {
            for(var i = 0; i < attribute.Points; i++)
            {
                totalPoints += this.GetPointsRequiredToIncreaseRank(i);
            }
        }

        return totalPoints;
    }
}
