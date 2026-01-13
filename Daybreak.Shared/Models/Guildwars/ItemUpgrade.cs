using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.Guildwars;

public sealed class ItemUpgrade
{
    public static readonly ItemUpgrade Unknown = new(-1, "Unknown", ItemUpgradeType.Unknown);

    // Axe
    public static readonly ItemUpgrade Icy_Axe = new(129, "Icy", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Ebon_Axe = new(130, "Ebon", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Shocking_Axe = new(131, "Shocking", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Fiery_Axe = new(132, "Fiery", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Barbed_Axe = new(146, "Barbed", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Crippling_Axe = new(148, "Crippling", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Cruel_Axe = new(150, "Cruel", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Furious_Axe = new(153, "Furious", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Poisonous_Axe = new(158, "Poisonous", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Heavy_Axe = new(161, "Heavy", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Zealous_Axe = new(163, "Zealous", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Vampiric_Axe = new(167, "Vampiric", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Sundering_Axe = new(171, "Sundering", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade OfDefense_Axe = new(197, "of Defense", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfWarding_Axe = new(199, "of Warding", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfShelter_Axe = new(205, "of Shelter", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSlaying_Axe = new(212, "of ____slaying", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfFortitude_Axe = new(217, "of Fortitude", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfEnchanting_Axe = new(222, "of Enchanting", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfAxeMastery = new(232, "of Axe Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfTheProfession_Axe = new(550, "of the Profession", ItemUpgradeType.Suffix);

    // Bow
    public static readonly ItemUpgrade Icy_Bow = new(133, "Icy", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Ebon_Bow = new(134, "Ebon", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Shocking_Bow = new(135, "Shocking", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Fiery_Bow = new(136, "Fiery", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Poisonous_Bow = new(159, "Poisonous", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Zealous_Bow = new(165, "Zealous", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Vampiric_Bow = new(169, "Vampiric", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Sundering_Bow = new(173, "Sundering", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade OfDefense_Bow = new(198, "of Defense", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfWarding_Bow = new(200, "of Warding", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfShelter_Bow = new(206, "of Shelter", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSlaying_Bow = new(213, "of _____slaying", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfFortitude_Bow = new(218, "of Fortitude", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfEnchanting_Bow = new(223, "of Enchanting", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMarksmanship = new(233, "of Marksmanship", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade Barbed_Bow = new(327, "Barbed", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Crippling_Bow = new(328, "Crippling", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Silencing_Bow = new(329, "Silencing", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade OfTheProfession_Bow = new(551, "of the Profession", ItemUpgradeType.Suffix);

    // Daggers
    public static readonly ItemUpgrade Icy_Daggers = new(302, "Icy", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Ebon_Daggers = new(303, "Ebon", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Fiery_Daggers = new(304, "Fiery", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Shocking_Daggers = new(305, "Shocking", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Zealous_Daggers = new(306, "Zealous", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Vampiric_Daggers = new(307, "Vampiric", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Sundering_Daggers = new(308, "Sundering", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Barbed_Daggers = new(309, "Barbed", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Crippling_Daggers = new(310, "Crippling", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Cruel_Daggers = new(311, "Cruel", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Poisonous_Daggers = new(312, "Poisonous", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Silencing_Daggers = new(313, "Silencing", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Furious_Daggers = new(314, "Furious", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade OfDefense_Daggers = new(321, "of Defense", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfWarding_Daggers = new(322, "of Warding", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfShelter_Daggers = new(323, "of Shelter", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfEnchanting_Daggers = new(324, "of Enchanting", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfFortitude_Daggers = new(325, "of Fortitude", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfDaggerMastery = new(326, "of Dagger Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfTheProfession_Daggers = new(552, "of the Profession", ItemUpgradeType.Suffix);

    // Focus
    public static readonly ItemUpgrade OfAptitude_Focus = new(535, "of Aptitude", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfFortitude_Focus = new(536, "of Fortitude", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfDevotion_Focus = new(537, "of Devotion", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfValor_Focus = new(538, "of Valor", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfEndurance_Focus = new(539, "of Endurance", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSwiftness_Focus = new(540, "of Swiftness", ItemUpgradeType.Suffix);

    // Hammer
    public static readonly ItemUpgrade Icy_Hammer = new(137, "Icy", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Ebon_Hammer = new(138, "Ebon", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Shocking_Hammer = new(139, "Shocking", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Fiery_Hammer = new(140, "Fiery", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Cruel_Hammer = new(151, "Cruel", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Furious_Hammer = new(154, "Furious", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Heavy_Hammer = new(162, "Heavy", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Zealous_Hammer = new(164, "Zealous", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Vampiric_Hammer = new(168, "Vampiric", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Sundering_Hammer = new(172, "Sundering", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade OfWarding_Hammer = new(201, "of Warding", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfDefense_Hammer = new(204, "of Defense", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfShelter_Hammer = new(207, "of Shelter", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSlaying_Hammer = new(214, "of _____slaying", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfFortitude_Hammer = new(219, "of Fortitude", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfEnchanting_Hammer = new(224, "of Enchanting", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfHammerMastery = new(234, "of Hammer Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfTheProfession_Hammer = new(553, "of the Profession", ItemUpgradeType.Suffix);

    // Inscription
    public static readonly ItemUpgrade IHaveThePower = new(348, "\"I have the power!\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade LetTheMemoryLiveAgain = new(350, "\"Let the Memory Live Again!\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade TooMuchInformation = new(355, "\"Too Much Information\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade GuidedByFate = new(356, "\"Guided by Fate\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade StrengthAndHonor = new(357, "\"Strength and Honor\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade VengeanceIsMine = new(358, "\"Vengeance is Mine\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade DontFearTheReaper = new(359, "\"Don't Fear the Reaper\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade DanceWithDeath = new(360, "\"Dance with Death\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade BrawnOverBrains = new(361, "\"Brawn over Brains\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade ToThePain = new(362, "\"To The Pain!\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade IgnoranceIsBliss = new(438, "\"Ignorance is Bliss\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade LifeIsPain = new(439, "\"Life is Pain\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade ManForAllSeasons = new(440, "\"Man for All Seasons\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade SurvivalOfTheFittest = new(441, "\"Survival of the Fittest\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade MightMakesRight = new(442, "\"Might makes Right!\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade KnowingIsHalfTheBattle = new(443, "\"Knowing is Half the Battle.\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade FaithIsMy = new(444, "\"Faith is My \"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade DownButNotOut = new(445, "\"Down But Not Out\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade HailToTheKing = new(446, "\"Hail to the King\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade BeJustAndFearNot = new(447, "\"Be Just and Fear Not\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade LiveForToday = new(448, "\"Live for Today\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade SerenityNow = new(449, "\"Serenity Now\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade ForgetMeNot = new(450, "\"Forget Me Not\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade NotTheFace = new(451, "\"Not the face!\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade LeafOnTheWind = new(452, "\"Leaf on the Wind\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade LikeARollingStone = new(453, "\"Like a Rolling Stone\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade RidersOnTheStorm = new(454, "\"Riders on the Storm\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade SleepNowInTheFire = new(455, "\"Sleep Now in the Fire\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade ThroughThickAndThin = new(456, "\"Through Thick and Thin\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade TheRiddleOfSteel = new(457, "\"The Riddle of Steel\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade FearCutsDeeper = new(458, "\"Fear Cuts Deeper\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade ICanSeeClearlyNow = new(459, "\"I Can See Clearly Now\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade SwiftAsTheWind = new(460, "\"Swift as the Wind\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade StrengthOfBody = new(461, "\"Strength of Body\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade CastOutTheUnclean = new(462, "\"Cast Out the Unclean\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade PureOfHeart = new(463, "\"Pure of Heart\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade SoundnessOfMind = new(464, "\"Soundness of Mind\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade OnlyTheStrongSurvive = new(465, "\"Only the Strong Survive\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade LuckOfTheDraw = new(466, "\"Luck of the Draw\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade ShelteredByFaith = new(467, "\"Sheltered by Faith\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade NothingToFear = new(468, "\"Nothing to Fear\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade RunForYourLife = new(469, "\"Run For Your Life!\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade MasterOfMyDomain = new(470, "\"Master of My Domain\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade AptitudeNotAttitude = new(471, "\"Aptitude not Attitude\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade SeizeTheDay = new(472, "\"Seize the Day\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade HaveFaith = new(473, "\"Have Faith\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade HaleAndHearty = new(474, "\"Hale and Hearty\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade DontCallItAComeback = new(475, "\"Don't call it a comeback!\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade IAmSorrow = new(476, "\"I am Sorrow.\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade DontThinkTwice = new(477, "\"Don't Think Twice\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade ShowMeTheMoney = new(542, "\"Show me the money\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade MeasureForMeasure = new(543, "\"Measure for Measure\"", ItemUpgradeType.Inscription);

    // Scythe
    public static readonly ItemUpgrade Icy_Scythe = new(363, "Icy", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Ebon_Scythe = new(364, "Ebon", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Zealous_Scythe = new(367, "Zealous", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Vampiric_Scythe = new(369, "Vampiric", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Sundering_Scythe = new(371, "Sundering", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Barbed_Scythe = new(372, "Barbed", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Crippling_Scythe = new(373, "Crippling", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Cruel_Scythe = new(374, "Cruel", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Poisonous_Scythe = new(375, "Poisonous", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Heavy_Scythe = new(376, "Heavy", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Furious_Scythe = new(377, "Furious", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade OfDefense_Scythe = new(392, "of Defense", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfWarding_Scythe = new(393, "of Warding", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfShelter_Scythe = new(394, "of Shelter", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfEnchanting_Scythe = new(395, "of Enchanting", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfFortitude_Scythe = new(396, "of Fortitude", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfScytheMastery = new(397, "of Scythe Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade Fiery_Scythe = new(523, "Fiery", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Shocking_Scythe = new(524, "Shocking", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade OfTheProfession_Scythe = new(556, "of the Profession", ItemUpgradeType.Suffix);

    // Shield
    public static readonly ItemUpgrade OfValor_Shield = new(337, "of Valor", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfEndurance_Shield = new(338, "of Endurance", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfFortitude_Shield = new(353, "of Fortitude", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfDevotion_Shield = new(354, "of Devotion", ItemUpgradeType.Suffix);

    // Spear
    public static readonly ItemUpgrade Fiery_Spear = new(365, "Fiery", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Shocking_Spear = new(366, "Shocking", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Zealous_Spear = new(368, "Zealous", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Vampiric_Spear = new(370, "Vampiric", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Sundering_Spear = new(378, "Sundering", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Barbed_Spear = new(379, "Barbed", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Crippling_Spear = new(380, "Crippling", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Cruel_Spear = new(381, "Cruel", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Poisonous_Spear = new(382, "Poisonous", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Silencing_Spear = new(383, "Silencing", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Furious_Spear = new(384, "Furious", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Heavy_Spear = new(385, "Heavy", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade OfDefense_Spear = new(398, "of Defense", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfWarding_Spear = new(399, "of Warding", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfShelter_Spear = new(400, "of Shelter", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfEnchanting_Spear = new(401, "of Enchanting", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfFortitude_Spear = new(402, "of Fortitude", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSpearMastery = new(403, "of Spear Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade Icy_Spear = new(525, "Icy", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Ebon_Spear = new(526, "Ebon", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade OfTheProfession_Spear = new(557, "of the Profession", ItemUpgradeType.Suffix);

    // Staff
    public static readonly ItemUpgrade Defensive_Staff = new(145, "Defensive", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Insightful_Staff = new(156, "Insightful", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Hale_Staff = new(157, "Hale", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade OfAttribute_Staff = new(195, "of <Attribute>", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfWarding_Staff = new(202, "of Warding", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfShelter_Staff = new(208, "of Shelter", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfDefense_Staff = new(210, "of Defense", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSlaying_Staff = new(215, "of _____slaying", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfFortitude_Staff = new(220, "of Fortitude", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfEnchanting_Staff = new(225, "of Enchanting", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMastery_Staff = new(339, "of Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfDevotion_Staff = new(340, "of Devotion", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfValor_Staff = new(341, "of Valor", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfEndurance_Staff = new(342, "of Endurance", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade Swift_Staff = new(527, "Swift", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Adept_Staff = new(528, "Adept", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade OfTheProfession_Staff = new(555, "of the Profession", ItemUpgradeType.Suffix);

    // Sword
    public static readonly ItemUpgrade Icy_Sword = new(141, "Icy", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Ebon_Sword = new(142, "Ebon", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Shocking_Sword = new(143, "Shocking", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Fiery_Sword = new(144, "Fiery", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Barbed_Sword = new(147, "Barbed", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Crippling_Sword = new(149, "Crippling", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Cruel_Sword = new(152, "Cruel", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Furious_Sword = new(155, "Furious", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Poisonous_Sword = new(160, "Poisonous", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Zealous_Sword = new(166, "Zealous", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Vampiric_Sword = new(170, "Vampiric", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Sundering_Sword = new(174, "Sundering", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade OfWarding_Sword = new(203, "of Warding", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfShelter_Sword = new(209, "of Shelter", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfDefense_Sword = new(211, "of Defense", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSlaying_Sword = new(216, "of _____slaying", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfFortitude_Sword = new(221, "of Fortitude", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfEnchanting_Sword = new(226, "of Enchanting", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSwordsmanship = new(235, "of Swordsmanship", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfTheProfession_Sword = new(558, "of the Profession", ItemUpgradeType.Suffix);

    // Wand
    public static readonly ItemUpgrade OfMemory_Wand = new(351, "of Memory", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfQuickening_Wand = new(352, "of Quickening", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfTheProfession_Wand = new(554, "of the Profession", ItemUpgradeType.Suffix);

    // Insignia
    public static readonly ItemUpgrade Survivor = new(486, "Survivor", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Radiant = new(485, "Radiant", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Stalwart = new(487, "Stalwart", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Brawlers = new(488, "Brawler's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Blessed = new(489, "Blessed", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Heralds = new(490, "Herald's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Sentrys = new(491, "Sentry's", ItemUpgradeType.Prefix);

    // Warrior
    public static readonly ItemUpgrade Knights = new(505, "Knight's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Lieutenants = new(520, "Lieutenant's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Stonefist = new(521, "Stonefist", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Dreadnought = new(506, "Dreadnought", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Sentinels = new(507, "Sentinel's", ItemUpgradeType.Prefix);

    // Ranger
    public static readonly ItemUpgrade Frostbound = new(508, "Frostbound", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Pyrebound = new(510, "Pyrebound", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Stormbound = new(511, "Stormbound", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Scouts = new(513, "Scout's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Earthbound = new(509, "Earthbound", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Beastmasters = new(512, "Beastmaster's", ItemUpgradeType.Prefix);

    // Monk
    public static readonly ItemUpgrade Wanderers = new(502, "Wanderer's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Disciples = new(503, "Disciple's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Anchorites = new(504, "Anchorite's", ItemUpgradeType.Prefix);

    // Necromancer
    public static readonly ItemUpgrade Bloodstained = new(522, "Bloodstained", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Tormentors = new(492, "Tormentor's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Bonelace = new(494, "Bonelace", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade MinionMasters = new(495, "Minion Master's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Blighters = new(496, "Blighter's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Undertakers = new(493, "Undertaker's", ItemUpgradeType.Prefix);

    // Mesmer
    public static readonly ItemUpgrade Virtuosos = new(484, "Virtuoso's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Artificers = new(482, "Artificer's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Prodigys = new(483, "Prodigy's", ItemUpgradeType.Prefix);

    // Elementalist
    public static readonly ItemUpgrade Hydromancer = new(498, "Hydromancer", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Geomancer = new(499, "Geomancer", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Pyromancer = new(500, "Pyromancer", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Aeromancer = new(501, "Aeromancer", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Prismatic = new(497, "Prismatic", ItemUpgradeType.Prefix);

    // Assassin
    public static readonly ItemUpgrade Vanguards = new(478, "Vanguard's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Infiltrators = new(479, "Infiltrator's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Saboteurs = new(480, "Saboteur's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Nightstalkers = new(481, "Nightstalker's", ItemUpgradeType.Prefix);

    // Ritualist
    public static readonly ItemUpgrade Shamans = new(516, "Shaman's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade GhostForge = new(517, "Ghost Forge", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Mystics = new(518, "Mystic's", ItemUpgradeType.Prefix);

    // Dervish
    public static readonly ItemUpgrade Windwalker = new(514, "Windwalker", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Forsaken = new(515, "Forsaken", ItemUpgradeType.Prefix);

    // Paragon
    public static readonly ItemUpgrade Centurions = new(519, "Centurion's", ItemUpgradeType.Prefix);

    // Runes
    public static readonly ItemUpgrade OfAttunement = new(529, "of Attunement", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfRecovery = new(531, "of Recovery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfRestoration = new(532, "of Restoration", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfClarity = new(533, "of Clarity", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfPurity = new(534, "of Purity", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorVigor = new(255, "of Minor Vigor", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorVigor2 = new(194, "of Minor Vigor", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorVigor = new(257, "of Superior Vigor", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorVigor = new(256, "of Major Vigor", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfVitae = new(530, "of Vitae", ItemUpgradeType.Suffix);

    // Warrior
    public static readonly ItemUpgrade OfMinorAbsorption = new(252, "of Minor Absorption", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorTactics = new(5377, "of Minor Tactics", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorStrength = new(4353, "of Minor Strength", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorAxeMastery = new(4609, "of Minor Axe Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorHammerMastery = new(4865, "of Minor Hammer Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorSwordsmanship = new(5121, "of Minor Swordsmanship", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorAbsorption = new(253, "of Major Absorption", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorTactics = new(5378, "of Major Tactics", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorStrength = new(4354, "of Major Strength", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorAxeMastery = new(4610, "of Major Axe Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorHammerMastery = new(4866, "of Major Hammer Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorSwordsmanship = new(5122, "of Major Swordsmanship", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorAbsorption = new(254, "of Superior Absorption", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorTactics = new(5379, "of Superior Tactics", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorStrength = new(4355, "of Superior Strength", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorAxeMastery = new(4611, "of Superior Axe Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorHammerMastery = new(4867, "of Superior Hammer Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorSwordsmanship = new(5123, "of Superior Swordsmanship", ItemUpgradeType.Suffix);

    // Ranger
    public static readonly ItemUpgrade OfMinorWildernessSurvival = new(6145, "of Minor Wilderness Survival", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorExpertise = new(5889, "of Minor Expertise", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorBeastMastery = new(5633, "of Minor Beast Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorMarksmanship = new(6401, "of Minor Marksmanship", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorWildernessSurvival = new(6146, "of Major Wilderness Survival", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorExpertise = new(5890, "of Major Expertise", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorBeastMastery = new(5634, "of Major Beast Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorMarksmanship = new(6402, "of Major Marksmanship", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorWildernessSurvival = new(6147, "of Superior Wilderness Survival", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorExpertise = new(5891, "of Superior Expertise", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorBeastMastery = new(5635, "of Superior Beast Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorMarksmanship = new(6403, "of Superior Marksmanship", ItemUpgradeType.Suffix);

    // Monk
    public static readonly ItemUpgrade OfMinorHealingPrayers = new(3329, "of Minor Healing Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorSmitingPrayers = new(3585, "of Minor Smiting Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorProtectionPrayers = new(3841, "of Minor Protection Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorDivineFavor = new(4097, "of Minor Divine Favor", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorHealingPrayers = new(3330, "of Major Healing Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorSmitingPrayers = new(3586, "of Major Smiting Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorProtectionPrayers = new(3842, "of Major Protection Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorDivineFavor = new(4098, "of Major Divine Favor", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorHealingPrayers = new(3331, "of Superior Healing Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorSmitingPrayers = new(3587, "of Superior Smiting Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorProtectionPrayers = new(3843, "of Superior Protection Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorDivineFavor = new(4099, "of Superior Divine Favor", ItemUpgradeType.Suffix);

    // Necromancer
    public static readonly ItemUpgrade OfMinorBloodMagic = new(1025, "of Minor Blood Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorDeathMagic = new(1281, "of Minor Death Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorCurses = new(1793, "of Minor Curses", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorSoulReaping = new(1537, "of Minor Soul Reaping", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorBloodMagic = new(1026, "of Major Blood Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorDeathMagic = new(1282, "of Major Death Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorCurses = new(1794, "of Major Curses", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorSoulReaping = new(1538, "of Major Soul Reaping", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorBloodMagic = new(1027, "of Superior Blood Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorDeathMagic = new(1283, "of Superior Death Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorCurses = new(1795, "of Superior Curses", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorSoulReaping = new(1539, "of Superior Soul Reaping", ItemUpgradeType.Suffix);

    // Mesmer
    public static readonly ItemUpgrade OfMinorFastCasting = new(1, "of Minor Fast Casting", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorDominationMagic = new(513, "of Minor Domination Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorIllusionMagic = new(257, "of Minor Illusion Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorInspirationMagic = new(769, "of Minor Inspiration Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorFastCasting = new(2, "of Major Fast Casting", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorDominationMagic = new(514, "of Major Domination Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorIllusionMagic = new(258, "of Major Illusion Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorInspirationMagic = new(770, "of Major Inspiration Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorFastCasting = new(3, "of Superior Fast Casting", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorDominationMagic = new(515, "of Superior Domination Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorIllusionMagic = new(259, "of Superior Illusion Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorInspirationMagic = new(771, "of Superior Inspiration Magic", ItemUpgradeType.Suffix);

    // Elementalist
    public static readonly ItemUpgrade OfMinorEnergyStorage = new(3073, "of Minor Energy Storage", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorFireMagic = new(2561, "of Minor Fire Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorAirMagic = new(2049, "of Minor Air Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorEarthMagic = new(2305, "of Minor Earth Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorWaterMagic = new(2817, "of Minor Water Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorEnergyStorage = new(3074, "of Major Energy Storage", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorFireMagic = new(2562, "of Major Fire Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorAirMagic = new(2050, "of Major Air Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorEarthMagic = new(2306, "of Major Earth Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorWaterMagic = new(2818, "of Major Water Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorEnergyStorage = new(3075, "of Superior Energy Storage", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorFireMagic = new(2563, "of Superior Fire Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorAirMagic = new(2051, "of Superior Air Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorEarthMagic = new(2307, "of Superior Earth Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorWaterMagic = new(2819, "of Superior Water Magic", ItemUpgradeType.Suffix);

    // Assassin
    public static readonly ItemUpgrade OfMinorCriticalStrikes = new(8961, "of Minor Critical Strikes", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorDaggerMastery = new(7425, "of Minor Dagger Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorDeadlyArts = new(7681, "of Minor Deadly Arts", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorShadowArts = new(7937, "of Minor Shadow Arts", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorCriticalStrikes = new(8962, "of Major Critical Strikes", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorDaggerMastery = new(7426, "of Major Dagger Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorDeadlyArts = new(7682, "of Major Deadly Arts", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorShadowArts = new(7938, "of Major Shadow Arts", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorCriticalStrikes = new(8963, "of Superior Critical Strikes", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorDaggerMastery = new(7427, "of Superior Dagger Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorDeadlyArts = new(7683, "of Superior Deadly Arts", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorShadowArts = new(7939, "of Superior Shadow Arts", ItemUpgradeType.Suffix);

    // Ritualist
    public static readonly ItemUpgrade OfMinorChannelingMagic = new(8705, "of Minor Channeling Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorRestorationMagic = new(8449, "of Minor Restoration Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorCommuning = new(8193, "of Minor Communing", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorSpawningPower = new(9217, "of Minor Spawning Power", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorChannelingMagic = new(8706, "of Major Channeling Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorRestorationMagic = new(8450, "of Major Restoration Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorCommuning = new(8194, "of Major Communing", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorSpawningPower = new(9218, "of Major Spawning Power", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorChannelingMagic = new(8707, "of Superior Channeling Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorRestorationMagic = new(8451, "of Superior Restoration Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorCommuning = new(8195, "of Superior Communing", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorSpawningPower = new(9219, "of Superior Spawning Power", ItemUpgradeType.Suffix);

    // Dervish
    public static readonly ItemUpgrade OfMinorMysticism = new(11265, "of Minor Mysticism", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorEarthPrayers = new(11009, "of Minor Earth Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorScytheMastery = new(10497, "of Minor Scythe Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorWindPrayers = new(10753, "of Minor Wind Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorMysticism = new(11266, "of Major Mysticism", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorEarthPrayers = new(11010, "of Major Earth Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorScytheMastery = new(10498, "of Major Scythe Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorWindPrayers = new(10754, "of Major Wind Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorMysticism = new(11267, "of Superior Mysticism", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorEarthPrayers = new(11011, "of Superior Earth Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorScytheMastery = new(10499, "of Superior Scythe Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorWindPrayers = new(10755, "of Superior Wind Prayers", ItemUpgradeType.Suffix);

    // Paragon
    public static readonly ItemUpgrade OfMinorLeadership = new(10241, "of Minor Leadership", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorMotivation = new(9985, "of Minor Motivation", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorCommand = new(9729, "of Minor Command", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorSpearMastery = new(9473, "of Minor Spear Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorLeadership = new(10242, "of Major Leadership", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorMotivation = new(9986, "of Major Motivation", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorCommand = new(9730, "of Major Command", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorSpearMastery = new(9474, "of Major Spear Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorLeadership = new(10243, "of Superior Leadership", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorMotivation = new(9987, "of Superior Motivation", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorCommand = new(9731, "of Superior Command", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorSpearMastery = new(9475, "of Superior Spear Mastery", ItemUpgradeType.Suffix);

    public static readonly ImmutableArray<ItemUpgrade> ItemUpgrades = [
        Unknown,
        Icy_Axe,
        Ebon_Axe,
        Shocking_Axe,
        Fiery_Axe,
        Barbed_Axe,
        Crippling_Axe,
        Cruel_Axe,
        Furious_Axe,
        Poisonous_Axe,
        Heavy_Axe,
        Zealous_Axe,
        Vampiric_Axe,
        Sundering_Axe,
        OfDefense_Axe,
        OfWarding_Axe,
        OfShelter_Axe,
        OfSlaying_Axe,
        OfFortitude_Axe,
        OfEnchanting_Axe,
        OfAxeMastery,
        Icy_Bow,
        Ebon_Bow,
        Shocking_Bow,
        Fiery_Bow,
        Poisonous_Bow,
        Zealous_Bow,
        Vampiric_Bow,
        Sundering_Bow,
        OfDefense_Bow,
        OfWarding_Bow,
        OfShelter_Bow,
        OfSlaying_Bow,
        OfFortitude_Bow,
        OfEnchanting_Bow,
        OfMarksmanship,
        Barbed_Bow,
        Crippling_Bow,
        Silencing_Bow,
        Icy_Daggers,
        Ebon_Daggers,
        Fiery_Daggers,
        Shocking_Daggers,
        Zealous_Daggers,
        Vampiric_Daggers,
        Sundering_Daggers,
        Barbed_Daggers,
        Crippling_Daggers,
        Cruel_Daggers,
        Poisonous_Daggers,
        Silencing_Daggers,
        Furious_Daggers,
        OfDefense_Daggers,
        OfWarding_Daggers,
        OfShelter_Daggers,
        OfEnchanting_Daggers,
        OfFortitude_Daggers,
        OfDaggerMastery,
        OfAptitude_Focus,
        OfFortitude_Focus,
        OfDevotion_Focus,
        OfValor_Focus,
        OfEndurance_Focus,
        OfSwiftness_Focus,
        Icy_Hammer,
        Ebon_Hammer,
        Shocking_Hammer,
        Fiery_Hammer,
        Cruel_Hammer,
        Furious_Hammer,
        Heavy_Hammer,
        Zealous_Hammer,
        Vampiric_Hammer,
        Sundering_Hammer,
        OfWarding_Hammer,
        OfDefense_Hammer,
        OfShelter_Hammer,
        OfSlaying_Hammer,
        OfFortitude_Hammer,
        OfEnchanting_Hammer,
        OfHammerMastery,
        IHaveThePower,
        LetTheMemoryLiveAgain,
        TooMuchInformation,
        GuidedByFate,
        StrengthAndHonor,
        VengeanceIsMine,
        DontFearTheReaper,
        DanceWithDeath,
        BrawnOverBrains,
        ToThePain,
        IgnoranceIsBliss,
        LifeIsPain,
        ManForAllSeasons,
        SurvivalOfTheFittest,
        MightMakesRight,
        KnowingIsHalfTheBattle,
        FaithIsMy,
        DownButNotOut,
        HailToTheKing,
        BeJustAndFearNot,
        LiveForToday,
        SerenityNow,
        ForgetMeNot,
        NotTheFace,
        LeafOnTheWind,
        LikeARollingStone,
        RidersOnTheStorm,
        SleepNowInTheFire,
        ThroughThickAndThin,
        TheRiddleOfSteel,
        FearCutsDeeper,
        ICanSeeClearlyNow,
        SwiftAsTheWind,
        StrengthOfBody,
        CastOutTheUnclean,
        PureOfHeart,
        SoundnessOfMind,
        OnlyTheStrongSurvive,
        LuckOfTheDraw,
        ShelteredByFaith,
        NothingToFear,
        RunForYourLife,
        MasterOfMyDomain,
        AptitudeNotAttitude,
        SeizeTheDay,
        HaveFaith,
        HaleAndHearty,
        DontCallItAComeback,
        IAmSorrow,
        DontThinkTwice,
        ShowMeTheMoney,
        MeasureForMeasure,
        Icy_Scythe,
        Ebon_Scythe,
        Zealous_Scythe,
        Vampiric_Scythe,
        Sundering_Scythe,
        Barbed_Scythe,
        Crippling_Scythe,
        Cruel_Scythe,
        Poisonous_Scythe,
        Heavy_Scythe,
        Furious_Scythe,
        OfDefense_Scythe,
        OfWarding_Scythe,
        OfShelter_Scythe,
        OfEnchanting_Scythe,
        OfFortitude_Scythe,
        OfScytheMastery,
        Fiery_Scythe,
        Shocking_Scythe,
        OfValor_Shield,
        OfEndurance_Shield,
        OfFortitude_Shield,
        OfDevotion_Shield,
        Fiery_Spear,
        Shocking_Spear,
        Zealous_Spear,
        Vampiric_Spear,
        Sundering_Spear,
        Barbed_Spear,
        Crippling_Spear,
        Cruel_Spear,
        Poisonous_Spear,
        Silencing_Spear,
        Furious_Spear,
        Heavy_Spear,
        OfDefense_Spear,
        OfWarding_Spear,
        OfShelter_Spear,
        OfEnchanting_Spear,
        OfFortitude_Spear,
        OfSpearMastery,
        Icy_Spear,
        Ebon_Spear,
        Defensive_Staff,
        Insightful_Staff,
        Hale_Staff,
        OfAttribute_Staff,
        OfWarding_Staff,
        OfShelter_Staff,
        OfDefense_Staff,
        OfSlaying_Staff,
        OfFortitude_Staff,
        OfEnchanting_Staff,
        OfMastery_Staff,
        OfDevotion_Staff,
        OfValor_Staff,
        OfEndurance_Staff,
        Swift_Staff,
        Adept_Staff,
        Icy_Sword,
        Ebon_Sword,
        Shocking_Sword,
        Fiery_Sword,
        Barbed_Sword,
        Crippling_Sword,
        Cruel_Sword,
        Furious_Sword,
        Poisonous_Sword,
        Zealous_Sword,
        Vampiric_Sword,
        Sundering_Sword,
        OfWarding_Sword,
        OfShelter_Sword,
        OfDefense_Sword,
        OfSlaying_Sword,
        OfFortitude_Sword,
        OfEnchanting_Sword,
        OfSwordsmanship,
        OfMemory_Wand,
        OfQuickening_Wand,
        OfTheProfession_Axe,
        OfTheProfession_Bow,
        OfTheProfession_Daggers,
        OfTheProfession_Hammer,
        OfTheProfession_Wand,
        OfTheProfession_Staff,
        OfTheProfession_Scythe,
        OfTheProfession_Spear,
        OfTheProfession_Sword,
        Survivor,
        Radiant,
        Stalwart,
        Brawlers,
        Blessed,
        Heralds,
        Sentrys,
        Knights,
        Lieutenants,
        Stonefist,
        Dreadnought,
        Sentinels,
        Frostbound,
        Pyrebound,
        Stormbound,
        Scouts,
        Earthbound,
        Beastmasters,
        Wanderers,
        Disciples,
        Anchorites,
        Bloodstained,
        Tormentors,
        Bonelace,
        MinionMasters,
        Blighters,
        Undertakers,
        Virtuosos,
        Artificers,
        Prodigys,
        Hydromancer,
        Geomancer,
        Pyromancer,
        Aeromancer,
        Prismatic,
        Vanguards,
        Infiltrators,
        Saboteurs,
        Nightstalkers,
        Shamans,
        GhostForge,
        Mystics,
        Windwalker,
        Forsaken,
        Centurions,
        OfAttunement,
        OfRecovery,
        OfRestoration,
        OfClarity,
        OfPurity,
        OfMinorVigor,
        OfMinorVigor2,
        OfSuperiorVigor,
        OfMajorVigor,
        OfVitae,
        OfMinorAbsorption,
        OfMinorTactics,
        OfMinorStrength,
        OfMinorAxeMastery,
        OfMinorHammerMastery,
        OfMinorSwordsmanship,
        OfMajorAbsorption,
        OfMajorTactics,
        OfMajorStrength,
        OfMajorAxeMastery,
        OfMajorHammerMastery,
        OfMajorSwordsmanship,
        OfSuperiorAbsorption,
        OfSuperiorTactics,
        OfSuperiorStrength,
        OfSuperiorAxeMastery,
        OfSuperiorHammerMastery,
        OfSuperiorSwordsmanship,
        OfMinorWildernessSurvival,
        OfMinorExpertise,
        OfMinorBeastMastery,
        OfMinorMarksmanship,
        OfMajorWildernessSurvival,
        OfMajorExpertise,
        OfMajorBeastMastery,
        OfMajorMarksmanship,
        OfSuperiorWildernessSurvival,
        OfSuperiorExpertise,
        OfSuperiorBeastMastery,
        OfSuperiorMarksmanship,
        OfMinorHealingPrayers,
        OfMinorSmitingPrayers,
        OfMinorProtectionPrayers,
        OfMinorDivineFavor,
        OfMajorHealingPrayers,
        OfMajorSmitingPrayers,
        OfMajorProtectionPrayers,
        OfMajorDivineFavor,
        OfSuperiorHealingPrayers,
        OfSuperiorSmitingPrayers,
        OfSuperiorProtectionPrayers,
        OfSuperiorDivineFavor,
        OfMinorBloodMagic,
        OfMinorDeathMagic,
        OfMinorCurses,
        OfMinorSoulReaping,
        OfMajorBloodMagic,
        OfMajorDeathMagic,
        OfMajorCurses,
        OfMajorSoulReaping,
        OfSuperiorBloodMagic,
        OfSuperiorDeathMagic,
        OfSuperiorCurses,
        OfSuperiorSoulReaping,
        OfMinorFastCasting,
        OfMinorDominationMagic,
        OfMinorIllusionMagic,
        OfMinorInspirationMagic,
        OfMajorFastCasting,
        OfMajorDominationMagic,
        OfMajorIllusionMagic,
        OfMajorInspirationMagic,
        OfSuperiorFastCasting,
        OfSuperiorDominationMagic,
        OfSuperiorIllusionMagic,
        OfSuperiorInspirationMagic,
        OfMinorEnergyStorage,
        OfMinorFireMagic,
        OfMinorAirMagic,
        OfMinorEarthMagic,
        OfMinorWaterMagic,
        OfMajorEnergyStorage,
        OfMajorFireMagic,
        OfMajorAirMagic,
        OfMajorEarthMagic,
        OfMajorWaterMagic,
        OfSuperiorEnergyStorage,
        OfSuperiorFireMagic,
        OfSuperiorAirMagic,
        OfSuperiorEarthMagic,
        OfSuperiorWaterMagic,
        OfMinorCriticalStrikes,
        OfMinorDaggerMastery,
        OfMinorDeadlyArts,
        OfMinorShadowArts,
        OfMajorCriticalStrikes,
        OfMajorDaggerMastery,
        OfMajorDeadlyArts,
        OfMajorShadowArts,
        OfSuperiorCriticalStrikes,
        OfSuperiorDaggerMastery,
        OfSuperiorDeadlyArts,
        OfSuperiorShadowArts,
        OfMinorChannelingMagic,
        OfMinorRestorationMagic,
        OfMinorCommuning,
        OfMinorSpawningPower,
        OfMajorChannelingMagic,
        OfMajorRestorationMagic,
        OfMajorCommuning,
        OfMajorSpawningPower,
        OfSuperiorChannelingMagic,
        OfSuperiorRestorationMagic,
        OfSuperiorCommuning,
        OfSuperiorSpawningPower,
        OfMinorMysticism,
        OfMinorEarthPrayers,
        OfMinorScytheMastery,
        OfMinorWindPrayers,
        OfMajorMysticism,
        OfMajorEarthPrayers,
        OfMajorScytheMastery,
        OfMajorWindPrayers,
        OfSuperiorMysticism,
        OfSuperiorEarthPrayers,
        OfSuperiorScytheMastery,
        OfSuperiorWindPrayers,
        OfMinorLeadership,
        OfMinorMotivation,
        OfMinorCommand,
        OfMinorSpearMastery,
        OfMajorLeadership,
        OfMajorMotivation,
        OfMajorCommand,
        OfMajorSpearMastery,
        OfSuperiorLeadership,
        OfSuperiorMotivation,
        OfSuperiorCommand,
        OfSuperiorSpearMastery,
        ];

    public static ItemUpgrade Parse(int id)
    {
        if (!TryParse(id, out var upgrade))
        {
            throw new InvalidOperationException($"Could not find {nameof(ItemUpgrade)} by id {id}");
        }

        return upgrade;
    }

    public static bool TryParse(int id, [NotNullWhen(true)] out ItemUpgrade? upgrade)
    {
        upgrade = ItemUpgrades.FirstOrDefault(u => u.Id == id);
        return upgrade is not null;
    }

    [JsonPropertyName("id")]
    public int Id { get; }

    [JsonPropertyName("name")]
    public string Name { get; }

    [JsonPropertyName("type")]
    public ItemUpgradeType Type { get; }

    private ItemUpgrade(int id, string name, ItemUpgradeType type)
    {
        this.Id = id;
        this.Name = name;
        this.Type = type;
    }
}
