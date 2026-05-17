using Daybreak.Services.BuildTemplates;
using Daybreak.Shared.Models.Builds;
using FluentAssertions;

namespace Daybreak.Tests.Services.BuildTemplates;

[TestClass]
public sealed class AttributePointCalculatorTests
{
    // From PointsRequiredToIncreaseRankMapping: 1+2+3+4+5+6+7+9+11+13+16+20.
    private const int PointsForRankTwelve = 97;

    private readonly AttributePointCalculator calculator = new();

    [TestMethod]
    public void MaximumAttributePoints_IsTwoHundred()
    {
        this.calculator.MaximumAttributePoints.Should().Be(200);
    }

    [TestMethod]
    [DataRow(0, 1)]
    [DataRow(1, 2)]
    [DataRow(7, 9)]
    [DataRow(11, 20)]
    public void GetPointsRequiredToIncreaseRank_KnownRank_ReturnsExpectedCost(int rank, int expected)
    {
        this.calculator.GetPointsRequiredToIncreaseRank(rank).Should().Be(expected);
    }

    [TestMethod]
    public void GetPointsRequiredToIncreaseRank_AtMaxRank_ReturnsIntMaxValueSentinel()
    {
        this.calculator.GetPointsRequiredToIncreaseRank(12).Should().Be(int.MaxValue);
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(13)]
    public void GetPointsRequiredToIncreaseRank_OutOfRange_Throws(int rank)
    {
        var action = () => this.calculator.GetPointsRequiredToIncreaseRank(rank);
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void GetUsedPoints_EmptyBuild_ReturnsZero()
    {
        var build = new SingleBuildEntry { Attributes = [] };
        this.calculator.GetUsedPoints(build).Should().Be(0);
    }

    [TestMethod]
    public void GetUsedPoints_SingleAttributeAtRankTwelve_ReturnsCumulativeCost()
    {
        var build = new SingleBuildEntry { Attributes = [new AttributeEntry { Points = 12 }] };
        this.calculator.GetUsedPoints(build).Should().Be(PointsForRankTwelve);
    }

    [TestMethod]
    public void GetUsedPoints_MultipleAttributes_SumsIndependently()
    {
        // Rank 3 costs 1+2+3 = 6, rank 5 costs 1+2+3+4+5 = 15. Total = 21.
        var build = new SingleBuildEntry
        {
            Attributes =
            [
                new AttributeEntry { Points = 3 },
                new AttributeEntry { Points = 5 },
            ]
        };

        this.calculator.GetUsedPoints(build).Should().Be(21);
    }

    [TestMethod]
    public void GetRemainingFreePoints_IsMaximumMinusUsed()
    {
        var build = new SingleBuildEntry { Attributes = [new AttributeEntry { Points = 12 }] };
        this.calculator.GetRemainingFreePoints(build).Should().Be(200 - PointsForRankTwelve);
    }
}
