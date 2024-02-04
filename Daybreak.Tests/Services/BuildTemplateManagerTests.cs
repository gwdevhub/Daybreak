using Daybreak.Models.Builds;
using Daybreak.Models.Guildwars;
using Daybreak.Services.BuildTemplates;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Daybreak.Tests.Services;

[TestClass]
public class BuildTemplateManagerTests
{
    private const string EncodedSingleTemplate = "OwBk0texXNu0Dj/z+TDzBj+TN4AE";
    private const string EncodedTeamTemplate = "OQJUAMxOEMLnw0nDXQGY6aw2B OQNCA8wDP9B8DyzmOUi1veC OggjYxXTIPWbp5krZfxEAAAoD OwUTM4HDn5gc9TJSh6xddmETA";
    private IBuildTemplateManager buildTemplateManager;

    [TestInitialize]
    public void Initialize()
    {
        this.buildTemplateManager = new BuildTemplateManager(Substitute.For<ILogger<BuildTemplateManager>>());
    }

    [TestMethod]
    public void TestSingleDecode()
    {
        var build = this.buildTemplateManager.DecodeTemplate(EncodedSingleTemplate);
        var singleBuildEntry = build.As<SingleBuildEntry>();
        singleBuildEntry.Should().NotBeNull();
        singleBuildEntry.Primary.Should().Be(Profession.Assassin);
        singleBuildEntry.Secondary.Should().Be(Profession.None);
        singleBuildEntry.Attributes.Count.Should().Be(4);
        singleBuildEntry.Attributes[1].Attribute.Should().Be(Attribute.DaggerMastery);
        singleBuildEntry.Attributes[1].Points.Should().Be(11);
        singleBuildEntry.Attributes[2].Attribute.Should().Be(Attribute.DeadlyArts);
        singleBuildEntry.Attributes[2].Points.Should().Be(1);
        singleBuildEntry.Attributes[3].Attribute.Should().Be(Attribute.ShadowArts);
        singleBuildEntry.Attributes[3].Points.Should().Be(5);
        singleBuildEntry.Attributes[0].Attribute.Should().Be(Attribute.CriticalStrikes);
        singleBuildEntry.Attributes[0].Points.Should().Be(11);
        singleBuildEntry.Skills.Count.Should().Be(8);
        singleBuildEntry.Skills[0].Should().Be(Skill.UnsuspectingStrike);
        singleBuildEntry.Skills[1].Should().Be(Skill.WildStrike);
        singleBuildEntry.Skills[2].Should().Be(Skill.CriticalStrike);
        singleBuildEntry.Skills[3].Should().Be(Skill.MoebiusStrike);
        singleBuildEntry.Skills[4].Should().Be(Skill.DeathBlossom);
        singleBuildEntry.Skills[5].Should().Be(Skill.CriticalEye);
        singleBuildEntry.Skills[6].Should().Be(Skill.CriticalAgility);
        singleBuildEntry.Skills[7].Should().Be(Skill.CriticalDefenses);
    }

    [TestMethod]
    public void TestTeamDecode()
    {
        var build = this.buildTemplateManager.DecodeTemplate(EncodedTeamTemplate);
        var teamBuildEntry = build.As<TeamBuildEntry>();
        teamBuildEntry.Should().NotBeNull();
        teamBuildEntry.Builds.Should().HaveCount(4);

        var firstBuild = teamBuildEntry.Builds[0];
        firstBuild.Primary.Should().Be(Profession.Mesmer);
        firstBuild.Secondary.Should().Be(Profession.Ranger);
        firstBuild.Attributes.Should().BeEquivalentTo(
        [
            new AttributeEntry
            {
                Attribute = Attribute.FastCasting,
                Points = 6
            },
            new AttributeEntry
            {
                Attribute = Attribute.DominationMagic,
                Points = 11
            },
            new AttributeEntry
            {
                Attribute = Attribute.IllusionMagic,
                Points = 0
            },
            new AttributeEntry
            {
                Attribute = Attribute.InspirationMagic,
                Points = 2
            },
            new AttributeEntry
            {
                Attribute = Attribute.BeastMastery,
                Points = 0
            },
            new AttributeEntry
            {
                Attribute = Attribute.Marksmanship,
                Points = 0
            },
            new AttributeEntry
            {
                Attribute = Attribute.WildernessSurvival,
                Points = 12
            },
        ]);
        firstBuild.Skills.Should().BeEquivalentTo(
        [
            Skill.EnergySurge,
            Skill.Mistrust,
            Skill.CryofFrustration,
            Skill.PowerSpike,
            Skill.PowerDrain,
            Skill.UnnaturalSignet,
            Skill.Empathy,
            Skill.QuickeningZephyr
        ]);

        var secondBuild = teamBuildEntry.Builds[1];
        secondBuild.Primary.Should().Be(Profession.Mesmer);
        secondBuild.Secondary.Should().Be(Profession.Monk);
        secondBuild.Attributes.Should().BeEquivalentTo(
        [
            new AttributeEntry
            {
                Attribute = Attribute.FastCasting,
                Points = 12
            },
            new AttributeEntry
            {
                Attribute = Attribute.DominationMagic,
                Points = 0
            },
            new AttributeEntry
            {
                Attribute = Attribute.IllusionMagic,
                Points = 0
            },
            new AttributeEntry
            {
                Attribute = Attribute.InspirationMagic,
                Points = 12
            },
            new AttributeEntry
            {
                Attribute = Attribute.HealingPrayers,
                Points = 0
            },
            new AttributeEntry
            {
                Attribute = Attribute.SmitingPrayers,
                Points = 0
            },
            new AttributeEntry
            {
                Attribute = Attribute.ProtectionPrayers,
                Points = 0
            },
        ]);
        secondBuild.Skills.Should().BeEquivalentTo(
        [
            Skill.SymbolicCelerity,
            Skill.MantraofInscriptions,
            Skill.KeystoneSignet,
            Skill.SignetofClumsiness,
            Skill.UnnaturalSignet,
            Skill.BaneSignet,
            Skill.CastigationSignet,
            Skill.SignetofRage
        ]);

        var thirdBuild = teamBuildEntry.Builds[2];
        thirdBuild.Primary.Should().Be(Profession.Ranger);
        thirdBuild.Secondary.Should().Be(Profession.Ritualist);
        thirdBuild.Attributes.Should().BeEquivalentTo(
        [
            new AttributeEntry
            {
                Attribute = Attribute.Expertise,
                Points = 3
            },
            new AttributeEntry
            {
                Attribute = Attribute.BeastMastery,
                Points = 12
            },
            new AttributeEntry
            {
                Attribute = Attribute.Marksmanship,
                Points = 0
            },
            new AttributeEntry
            {
                Attribute = Attribute.WildernessSurvival,
                Points = 0
            },
            new AttributeEntry
            {
                Attribute = Attribute.ChannelingMagic,
                Points = 0
            },
            new AttributeEntry
            {
                Attribute = Attribute.Communing,
                Points = 0
            },
            new AttributeEntry
            {
                Attribute = Attribute.RestorationMagic,
                Points = 12
            },
        ]);
        thirdBuild.Skills.Should().BeEquivalentTo(
        [
            Skill.XinraesWeapon,
            Skill.MendBodyandSoul,
            Skill.SpiritLight,
            Skill.GhostmirrorLight,
            Skill.FleshofMyFlesh,
            Skill.ResurrectionSignet,
            Skill.NoSkill,
            Skill.EdgeofExtinction
        ]);

        var fourthBuild = teamBuildEntry.Builds[3];
        fourthBuild.Primary.Should().Be(Profession.Monk);
        fourthBuild.Secondary.Should().Be(Profession.Mesmer);
        fourthBuild.Attributes.Should().BeEquivalentTo(
        [
            new AttributeEntry
            {
                Attribute = Attribute.DivineFavor,
                Points = 3
            },
            new AttributeEntry
            {
                Attribute = Attribute.HealingPrayers,
                Points = 0
            },
            new AttributeEntry
            {
                Attribute = Attribute.SmitingPrayers,
                Points = 0
            },
            new AttributeEntry
            {
                Attribute = Attribute.ProtectionPrayers,
                Points = 12
            },
            new AttributeEntry
            {
                Attribute = Attribute.DominationMagic,
                Points = 0
            },
            new AttributeEntry
            {
                Attribute = Attribute.IllusionMagic,
                Points = 0
            },
            new AttributeEntry
            {
                Attribute = Attribute.InspirationMagic,
                Points = 12
            },
        ]);
        fourthBuild.Skills.Should().BeEquivalentTo(
        [
            Skill.ProtectiveBond,
            Skill.PurifyingVeil,
            Skill.BlessedSignet,
            Skill.MantraofRecall,
            Skill.ProtectiveSpirit,
            Skill.ShieldofAbsorption,
            Skill.ReversalofFortune,
            Skill.Resurrect
        ]);
    }

    [TestMethod]
    public void TestSingleEncode()
    {
        var build = new SingleBuildEntry()
        {
            Primary = Profession.Assassin,
            Secondary = Profession.None,
            Attributes =
            [
                new AttributeEntry
                {
                    Attribute = Attribute.DaggerMastery,
                    Points = 11
                },
                new AttributeEntry
                {
                    Attribute = Attribute.DeadlyArts,
                    Points = 1
                },
                new AttributeEntry
                {
                    Attribute = Attribute.ShadowArts,
                    Points = 5
                },
                new AttributeEntry
                {
                    Attribute = Attribute.CriticalStrikes,
                    Points = 11
                }
            ],
            Skills =
            [
                Skill.UnsuspectingStrike,
                Skill.WildStrike,
                Skill.CriticalStrike,
                Skill.MoebiusStrike,
                Skill.DeathBlossom,
                Skill.CriticalEye,
                Skill.CriticalAgility,
                Skill.CriticalDefenses
            ]
        };

        var encoded = this.buildTemplateManager.EncodeTemplate(build);
        encoded.Should().Be(EncodedSingleTemplate);
    }

    [TestMethod]
    public void TestTeamEncode()
    {
        var build = new TeamBuildEntry
        {
            Builds = [
                new SingleBuildEntry
                {
                    Primary = Profession.Mesmer,
                    Secondary = Profession.Ranger,
                    Attributes = [
                        new AttributeEntry
                        {
                            Attribute = Attribute.FastCasting,
                            Points = 6
                        },
                        new AttributeEntry
                        {
                            Attribute = Attribute.DominationMagic,
                            Points = 11
                        },
                        new AttributeEntry
                        {
                            Attribute = Attribute.IllusionMagic,
                            Points = 0
                        },
                        new AttributeEntry
                        {
                            Attribute = Attribute.InspirationMagic,
                            Points = 2
                        },
                        new AttributeEntry
                        {
                            Attribute = Attribute.BeastMastery,
                            Points = 0
                        },
                        new AttributeEntry
                        {
                            Attribute = Attribute.Marksmanship,
                            Points = 0
                        },
                        new AttributeEntry
                        {
                            Attribute = Attribute.WildernessSurvival,
                            Points = 12
                        },
                    ],
                    Skills = [
                        Skill.EnergySurge,
                        Skill.Mistrust,
                        Skill.CryofFrustration,
                        Skill.PowerSpike,
                        Skill.PowerDrain,
                        Skill.UnnaturalSignet,
                        Skill.Empathy,
                        Skill.QuickeningZephyr
                    ]
                },
                new SingleBuildEntry
                {
                    Primary = Profession.Mesmer,
                    Secondary = Profession.Monk,
                    Attributes = [
                        new AttributeEntry
                        {
                            Attribute = Attribute.FastCasting,
                            Points = 12
                        },
                        new AttributeEntry
                        {
                            Attribute = Attribute.DominationMagic,
                            Points = 0
                        },
                        new AttributeEntry
                        {
                            Attribute = Attribute.IllusionMagic,
                            Points = 0
                        },
                        new AttributeEntry
                        {
                            Attribute = Attribute.InspirationMagic,
                            Points = 12
                        },
                        new AttributeEntry
                        {
                            Attribute = Attribute.HealingPrayers,
                            Points = 0
                        },
                        new AttributeEntry
                        {
                            Attribute = Attribute.SmitingPrayers,
                            Points = 0
                        },
                        new AttributeEntry
                        {
                            Attribute = Attribute.ProtectionPrayers,
                            Points = 0
                        },
                    ],
                    Skills = [
                        Skill.SymbolicCelerity,
                        Skill.MantraofInscriptions,
                        Skill.KeystoneSignet,
                        Skill.SignetofClumsiness,
                        Skill.UnnaturalSignet,
                        Skill.BaneSignet,
                        Skill.CastigationSignet,
                        Skill.SignetofRage
                    ]
                },
                new SingleBuildEntry
                {
                    Primary = Profession.Ranger,
                    Secondary = Profession.Ritualist,
                    Attributes = [
                        new AttributeEntry
                        {
                            Attribute = Attribute.Expertise,
                            Points = 3
                        },
                        new AttributeEntry
                        {
                            Attribute = Attribute.BeastMastery,
                            Points = 12
                        },
                        new AttributeEntry
                        {
                            Attribute = Attribute.Marksmanship,
                            Points = 0
                        },
                        new AttributeEntry
                        {
                            Attribute = Attribute.WildernessSurvival,
                            Points = 0
                        },
                        new AttributeEntry
                        {
                            Attribute = Attribute.ChannelingMagic,
                            Points = 0
                        },
                        new AttributeEntry
                        {
                            Attribute = Attribute.Communing,
                            Points = 0
                        },
                        new AttributeEntry
                        {
                            Attribute = Attribute.RestorationMagic,
                            Points = 12
                        },
                    ],
                    Skills = [
                        Skill.XinraesWeapon,
                        Skill.MendBodyandSoul,
                        Skill.SpiritLight,
                        Skill.GhostmirrorLight,
                        Skill.FleshofMyFlesh,
                        Skill.ResurrectionSignet,
                        Skill.NoSkill,
                        Skill.EdgeofExtinction
                    ]
                },
                new SingleBuildEntry
                {
                    Primary = Profession.Monk,
                    Secondary = Profession.Mesmer,
                    Attributes = [
                        new AttributeEntry
                        {
                            Attribute = Attribute.DivineFavor,
                            Points = 3
                        },
                        new AttributeEntry
                        {
                            Attribute = Attribute.HealingPrayers,
                            Points = 0
                        },
                        new AttributeEntry
                        {
                            Attribute = Attribute.SmitingPrayers,
                            Points = 0
                        },
                        new AttributeEntry
                        {
                            Attribute = Attribute.ProtectionPrayers,
                            Points = 12
                        },
                        new AttributeEntry
                        {
                            Attribute = Attribute.DominationMagic,
                            Points = 0
                        },
                        new AttributeEntry
                        {
                            Attribute = Attribute.IllusionMagic,
                            Points = 0
                        },
                        new AttributeEntry
                        {
                            Attribute = Attribute.InspirationMagic,
                            Points = 12
                        },
                    ],
                    Skills = [
                        Skill.ProtectiveBond,
                        Skill.PurifyingVeil,
                        Skill.BlessedSignet,
                        Skill.MantraofRecall,
                        Skill.ProtectiveSpirit,
                        Skill.ShieldofAbsorption,
                        Skill.ReversalofFortune,
                        Skill.Resurrect
                    ]
                }
            ]
        };

        var encoded = this.buildTemplateManager.EncodeTemplate(build);
        encoded.Should().Be(EncodedTeamTemplate);
    }
}
