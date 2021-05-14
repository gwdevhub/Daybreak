using Daybreak.Models.Builds;
using Daybreak.Services.BuildTemplates;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace Daybreak.Tests.Services
{
    [TestClass]
    public class BuildTemplateManagerTests
    {
        private const string EncodedTemplate = "OwBk0texXNu0Dj/z+TDzBj+TN4AE";
        private IBuildTemplateManager buildTemplateManager;

        [TestInitialize]
        public void Initialize()
        {
            buildTemplateManager = new BuildTemplateManager(new Mock<ILogger<BuildTemplateManager>>().Object);
        }

        [TestMethod]
        public void TestDecode()
        {
            var build = this.buildTemplateManager.DecodeTemplate(EncodedTemplate);
            build.Primary.Should().Be(Profession.Assassin);
            build.Secondary.Should().Be(Profession.None);
            build.Attributes.Count.Should().Be(4);
            build.Attributes[0].Attribute.Should().Be(Attribute.DaggerMastery);
            build.Attributes[0].Points.Should().Be(11);
            build.Attributes[1].Attribute.Should().Be(Attribute.DeadlyArts);
            build.Attributes[1].Points.Should().Be(1);
            build.Attributes[2].Attribute.Should().Be(Attribute.ShadowArts);
            build.Attributes[2].Points.Should().Be(5);
            build.Attributes[3].Attribute.Should().Be(Attribute.CriticalStrikes);
            build.Attributes[3].Points.Should().Be(11);
            build.Skills.Count.Should().Be(8);
            build.Skills[0].Should().Be(Skill.UnsuspectingStrike);
            build.Skills[1].Should().Be(Skill.WildStrike);
            build.Skills[2].Should().Be(Skill.CriticalStrike);
            build.Skills[3].Should().Be(Skill.MoebiusStrike);
            build.Skills[4].Should().Be(Skill.DeathBlossom);
            build.Skills[5].Should().Be(Skill.CriticalEye);
            build.Skills[6].Should().Be(Skill.CriticalAgility);
            build.Skills[7].Should().Be(Skill.CriticalDefenses);
        }

        [TestMethod]
        public void TestEncode()
        {
            var build = new Build()
            {
                Primary = Profession.Assassin,
                Secondary = Profession.None,
                Attributes = new List<AttributeEntry>
                {
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
                },
                Skills = new List<Skill>
                {
                    Skill.UnsuspectingStrike,
                    Skill.WildStrike,
                    Skill.CriticalStrike,
                    Skill.MoebiusStrike,
                    Skill.DeathBlossom,
                    Skill.CriticalEye,
                    Skill.CriticalAgility,
                    Skill.CriticalDefenses
                }
            };

            var encoded = this.buildTemplateManager.EncodeTemplate(build);
            encoded.Should().Be(EncodedTemplate);
        }
    }
}
