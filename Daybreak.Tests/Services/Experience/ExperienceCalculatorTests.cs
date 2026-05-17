using Daybreak.Services.Experience;
using FluentAssertions;

namespace Daybreak.Tests.Services.Experience;

[TestClass]
public sealed class ExperienceCalculatorTests
{
    private const uint Level20TotalXp = 182600;
    private const uint PostCapPerLevel = 15000;

    private readonly ExperienceCalculator calculator = new();

    [TestMethod]
    [DataRow(0u, 0u)]
    [DataRow(1u, 1u)]
    [DataRow(1999u, 1999u)]
    [DataRow(2000u, 2000u)]
    [DataRow(2001u, 1u)]
    [DataRow(4600u, 2600u)]
    [DataRow(4601u, 1u)]
    [DataRow(154000u, 13400u)]
    public void GetExperienceForCurrentLevel_BelowCap_ReturnsXpSincePreviousThreshold(uint totalXp, uint expected)
    {
        this.calculator.GetExperienceForCurrentLevel(totalXp).Should().Be(expected);
    }

    [TestMethod]
    public void GetExperienceForCurrentLevel_AtCap_UsesModuloOfFixedRequirement()
    {
        this.calculator.GetExperienceForCurrentLevel(Level20TotalXp).Should().Be(0);
        this.calculator.GetExperienceForCurrentLevel(Level20TotalXp + 1).Should().Be(1);
        this.calculator.GetExperienceForCurrentLevel(Level20TotalXp + PostCapPerLevel).Should().Be(0);
        this.calculator.GetExperienceForCurrentLevel(Level20TotalXp + PostCapPerLevel + 123).Should().Be(123);
    }

    [TestMethod]
    [DataRow(0u, 2000u)]
    [DataRow(1u, 1999u)]
    [DataRow(1999u, 1u)]
    [DataRow(2000u, 2600u)]
    [DataRow(2001u, 2599u)]
    [DataRow(168000u, 14600u)]
    public void GetRemainingExperienceForNextLevel_BelowCap_ReturnsDistanceToNextThreshold(uint totalXp, uint expected)
    {
        this.calculator.GetRemainingExperienceForNextLevel(totalXp).Should().Be(expected);
    }

    [TestMethod]
    public void GetRemainingExperienceForNextLevel_AtCap_UsesFixedRequirementMinusModulo()
    {
        this.calculator.GetRemainingExperienceForNextLevel(Level20TotalXp + 1).Should().Be(PostCapPerLevel - 1);
        this.calculator.GetRemainingExperienceForNextLevel(Level20TotalXp + PostCapPerLevel - 1).Should().Be(1);
        this.calculator.GetRemainingExperienceForNextLevel(Level20TotalXp + PostCapPerLevel).Should().Be(PostCapPerLevel);
    }

    [TestMethod]
    public void GetTotalExperienceForNextLevel_IsCurrentPlusRemaining()
    {
        const uint totalXp = 12345u;
        var remaining = this.calculator.GetRemainingExperienceForNextLevel(totalXp);
        this.calculator.GetTotalExperienceForNextLevel(totalXp).Should().Be(totalXp + remaining);
    }

    [TestMethod]
    [DataRow(0u, 2000u)]
    [DataRow(1u, 2000u)]
    [DataRow(2000u, 2600u)]
    [DataRow(4600u, 3200u)]
    public void GetNextExperienceThreshold_BelowCap_ReturnsSizeOfCurrentLevel(uint totalXp, uint expected)
    {
        this.calculator.GetNextExperienceThreshold(totalXp).Should().Be(expected);
    }

    [TestMethod]
    public void GetNextExperienceThreshold_AtCap_ReturnsFixedRequirement()
    {
        this.calculator.GetNextExperienceThreshold(Level20TotalXp).Should().Be(PostCapPerLevel);
        this.calculator.GetNextExperienceThreshold(Level20TotalXp + 100_000).Should().Be(PostCapPerLevel);
    }
}
