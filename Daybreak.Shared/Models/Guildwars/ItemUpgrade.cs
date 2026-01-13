using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.Guildwars;

public sealed class ItemUpgrade
{
    public static readonly ItemUpgrade Unknown                              = new(-1, "Unknown", ItemUpgradeType.Unknown);
    public static readonly ItemUpgrade Icy_Axe                              = new(0x0081, "Icy", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Ebon_Axe                             = new(0x0082, "Ebon", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Shocking_Axe                         = new(0x0083, "Shocking", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Fiery_Axe                            = new(0x0084, "Fiery", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Barbed_Axe                           = new(0x0092, "Barbed", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Crippling_Axe                        = new(0x0094, "Crippling", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Cruel_Axe                            = new(0x0096, "Cruel", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Furious_Axe                          = new(0x0099, "Furious", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Poisonous_Axe                        = new(0x009E, "Poisonous", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Heavy_Axe                            = new(0x00A1, "Heavy", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Zealous_Axe                          = new(0x00A3, "Zealous", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Vampiric_Axe                         = new(0x00A7, "Vampiric", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Sundering_Axe                        = new(0x00AB, "Sundering", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade OfDefense_Axe                        = new(0x00C5, "of Defense", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfWarding_Axe                        = new(0x00C7, "of Warding", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfShelter_Axe                        = new(0x00CD, "of Shelter", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSlaying_Axe                        = new(0x00D4, "of ____slaying", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfFortitude_Axe                      = new(0x00D9, "of Fortitude", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfEnchanting_Axe                     = new(0x00DE, "of Enchanting", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfAxeMastery                         = new(0x00E8, "of Axe Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfTheProfession_Axe                  = new(0x0226, "of the Profession", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade Icy_Bow                              = new(0x0085, "Icy", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Ebon_Bow                             = new(0x0086, "Ebon", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Shocking_Bow                         = new(0x0087, "Shocking", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Fiery_Bow                            = new(0x0088, "Fiery", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Poisonous_Bow                        = new(0x009F, "Poisonous", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Zealous_Bow                          = new(0x00A5, "Zealous", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Vampiric_Bow                         = new(0x00A9, "Vampiric", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Sundering_Bow                        = new(0x00AD, "Sundering", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade OfDefense_Bow                        = new(0x00C6, "of Defense", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfWarding_Bow                        = new(0x00C8, "of Warding", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfShelter_Bow                        = new(0x00CE, "of Shelter", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSlaying_Bow                        = new(0x00D5, "of _____slaying", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfFortitude_Bow                      = new(0x00DA, "of Fortitude", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfEnchanting_Bow                     = new(0x00DF, "of Enchanting", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMarksmanship                       = new(0x00E9, "of Marksmanship", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade Barbed_Bow                           = new(0x0147, "Barbed", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Crippling_Bow                        = new(0x0148, "Crippling", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Silencing_Bow                        = new(0x0149, "Silencing", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade OfTheProfession_Bow                  = new(0x0227, "of the Profession", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade Icy_Daggers                          = new(0x012E, "Icy", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Ebon_Daggers                         = new(0x012F, "Ebon", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Fiery_Daggers                        = new(0x0130, "Fiery", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Shocking_Daggers                     = new(0x0131, "Shocking", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Zealous_Daggers                      = new(0x0132, "Zealous", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Vampiric_Daggers                     = new(0x0133, "Vampiric", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Sundering_Daggers                    = new(0x0134, "Sundering", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Barbed_Daggers                       = new(0x0135, "Barbed", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Crippling_Daggers                    = new(0x0136, "Crippling", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Cruel_Daggers                        = new(0x0137, "Cruel", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Poisonous_Daggers                    = new(0x0138, "Poisonous", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Silencing_Daggers                    = new(0x0139, "Silencing", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Furious_Daggers                      = new(0x013A, "Furious", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade OfDefense_Daggers                    = new(0x0141, "of Defense", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfWarding_Daggers                    = new(0x0142, "of Warding", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfShelter_Daggers                    = new(0x0143, "of Shelter", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfEnchanting_Daggers                 = new(0x0144, "of Enchanting", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfFortitude_Daggers                  = new(0x0145, "of Fortitude", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfDaggerMastery                      = new(0x0146, "of Dagger Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfTheProfession_Daggers              = new(0x0228, "of the Profession", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfAptitude_Focus                     = new(0x0217, "of Aptitude", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfFortitude_Focus                    = new(0x0218, "of Fortitude", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfDevotion_Focus                     = new(0x0219, "of Devotion", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfValor_Focus                        = new(0x021A, "of Valor", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfEndurance_Focus                    = new(0x021B, "of Endurance", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSwiftness_Focus                    = new(0x021C, "of Swiftness", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade Icy_Hammer                           = new(0x0089, "Icy", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Ebon_Hammer                          = new(0x008A, "Ebon", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Shocking_Hammer                      = new(0x008B, "Shocking", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Fiery_Hammer                         = new(0x008C, "Fiery", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Cruel_Hammer                         = new(0x0097, "Cruel", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Furious_Hammer                       = new(0x009A, "Furious", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Heavy_Hammer                         = new(0x00A2, "Heavy", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Zealous_Hammer                       = new(0x00A4, "Zealous", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Vampiric_Hammer                      = new(0x00A8, "Vampiric", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Sundering_Hammer                     = new(0x00AC, "Sundering", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade OfWarding_Hammer                     = new(0x00C9, "of Warding", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfDefense_Hammer                     = new(0x00CC, "of Defense", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfShelter_Hammer                     = new(0x00CF, "of Shelter", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSlaying_Hammer                     = new(0x00D6, "of _____slaying", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfFortitude_Hammer                   = new(0x00DB, "of Fortitude", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfEnchanting_Hammer                  = new(0x00E0, "of Enchanting", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfHammerMastery                      = new(0x00EA, "of Hammer Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfTheProfession_Hammer               = new(0x0229, "of the Profession", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade IHaveThePower                        = new(0x015C, "\"I have the power!\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade LetTheMemoryLiveAgain                = new(0x015E, "\"Let the Memory Live Again!\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade TooMuchInformation                   = new(0x0163, "\"Too Much Information\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade GuidedByFate                         = new(0x0164, "\"Guided by Fate\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade StrengthAndHonor                     = new(0x0165, "\"Strength and Honor\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade VengeanceIsMine                      = new(0x0166, "\"Vengeance is Mine\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade DontFearTheReaper                    = new(0x0167, "\"Don't Fear the Reaper\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade DanceWithDeath                       = new(0x0168, "\"Dance with Death\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade BrawnOverBrains                      = new(0x0169, "\"Brawn over Brains\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade ToThePain                            = new(0x016A, "\"To The Pain!\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade IgnoranceIsBliss                     = new(0x01B6, "\"Ignorance is Bliss\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade LifeIsPain                           = new(0x01B7, "\"Life is Pain\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade ManForAllSeasons                     = new(0x01B8, "\"Man for All Seasons\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade SurvivalOfTheFittest                 = new(0x01B9, "\"Survival of the Fittest\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade MightMakesRight                      = new(0x01BA, "\"Might makes Right!\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade KnowingIsHalfTheBattle               = new(0x01BB, "\"Knowing is Half the Battle.\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade FaithIsMy                            = new(0x01BC, "\"Faith is My \"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade DownButNotOut                        = new(0x01BD, "\"Down But Not Out\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade HailToTheKing                        = new(0x01BE, "\"Hail to the King\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade BeJustAndFearNot                     = new(0x01BF, "\"Be Just and Fear Not\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade LiveForToday                         = new(0x01C0, "\"Live for Today\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade SerenityNow                          = new(0x01C1, "\"Serenity Now\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade ForgetMeNot                          = new(0x01C2, "\"Forget Me Not\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade NotTheFace                           = new(0x01C3, "\"Not the face!\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade LeafOnTheWind                        = new(0x01C4, "\"Leaf on the Wind\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade LikeARollingStone                    = new(0x01C5, "\"Like a Rolling Stone\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade RidersOnTheStorm                     = new(0x01C6, "\"Riders on the Storm\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade SleepNowInTheFire                    = new(0x01C7, "\"Sleep Now in the Fire\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade ThroughThickAndThin                  = new(0x01C8, "\"Through Thick and Thin\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade TheRiddleOfSteel                     = new(0x01C9, "\"The Riddle of Steel\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade FearCutsDeeper                       = new(0x01CA, "\"Fear Cuts Deeper\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade ICanSeeClearlyNow                    = new(0x01CB, "\"I Can See Clearly Now\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade SwiftAsTheWind                       = new(0x01CC, "\"Swift as the Wind\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade StrengthOfBody                       = new(0x01CD, "\"Strength of Body\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade CastOutTheUnclean                    = new(0x01CE, "\"Cast Out the Unclean\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade PureOfHeart                          = new(0x01CF, "\"Pure of Heart\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade SoundnessOfMind                      = new(0x01D0, "\"Soundness of Mind\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade OnlyTheStrongSurvive                 = new(0x01D1, "\"Only the Strong Survive\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade LuckOfTheDraw                        = new(0x01D2, "\"Luck of the Draw\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade ShelteredByFaith                     = new(0x01D3, "\"Sheltered by Faith\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade NothingToFear                        = new(0x01D4, "\"Nothing to Fear\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade RunForYourLife                       = new(0x01D5, "\"Run For Your Life!\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade MasterOfMyDomain                     = new(0x01D6, "\"Master of My Domain\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade AptitudeNotAttitude                  = new(0x01D7, "\"Aptitude not Attitude\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade SeizeTheDay                          = new(0x01D8, "\"Seize the Day\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade HaveFaith                            = new(0x01D9, "\"Have Faith\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade HaleAndHearty                        = new(0x01DA, "\"Hale and Hearty\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade DontCallItAComeback                  = new(0x01DB, "\"Don't call it a comeback!\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade IAmSorrow                            = new(0x01DC, "\"I am Sorrow.\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade DontThinkTwice                       = new(0x01DD, "\"Don't Think Twice\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade ShowMeTheMoney                       = new(0x021E, "\"Show me the money\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade MeasureForMeasure                    = new(0x021F, "\"Measure for Measure\"", ItemUpgradeType.Inscription);
    public static readonly ItemUpgrade Icy_Scythe                           = new(0x016B, "Icy", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Ebon_Scythe                          = new(0x016C, "Ebon", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Zealous_Scythe                       = new(0x016F, "Zealous", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Vampiric_Scythe                      = new(0x0171, "Vampiric", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Sundering_Scythe                     = new(0x0173, "Sundering", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Barbed_Scythe                        = new(0x0174, "Barbed", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Crippling_Scythe                     = new(0x0175, "Crippling", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Cruel_Scythe                         = new(0x0176, "Cruel", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Poisonous_Scythe                     = new(0x0177, "Poisonous", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Heavy_Scythe                         = new(0x0178, "Heavy", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Furious_Scythe                       = new(0x0179, "Furious", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade OfDefense_Scythe                     = new(0x0188, "of Defense", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfWarding_Scythe                     = new(0x0189, "of Warding", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfShelter_Scythe                     = new(0x018A, "of Shelter", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfEnchanting_Scythe                  = new(0x018B, "of Enchanting", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfFortitude_Scythe                   = new(0x018C, "of Fortitude", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfScytheMastery                      = new(0x018D, "of Scythe Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade Fiery_Scythe                         = new(0x020B, "Fiery", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Shocking_Scythe                      = new(0x020C, "Shocking", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade OfTheProfession_Scythe               = new(0x022C, "of the Profession", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfValor_Shield                       = new(0x0151, "of Valor", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfEndurance_Shield                   = new(0x0152, "of Endurance", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfFortitude_Shield                   = new(0x0161, "of Fortitude", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfDevotion_Shield                    = new(0x0162, "of Devotion", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade Fiery_Spear                          = new(0x016D, "Fiery", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Shocking_Spear                       = new(0x016E, "Shocking", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Zealous_Spear                        = new(0x0170, "Zealous", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Vampiric_Spear                       = new(0x0172, "Vampiric", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Sundering_Spear                      = new(0x017A, "Sundering", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Barbed_Spear                         = new(0x017B, "Barbed", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Crippling_Spear                      = new(0x017C, "Crippling", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Cruel_Spear                          = new(0x017D, "Cruel", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Poisonous_Spear                      = new(0x017E, "Poisonous", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Silencing_Spear                      = new(0x017F, "Silencing", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Furious_Spear                        = new(0x0180, "Furious", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Heavy_Spear                          = new(0x0181, "Heavy", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade OfDefense_Spear                      = new(0x018E, "of Defense", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfWarding_Spear                      = new(0x018F, "of Warding", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfShelter_Spear                      = new(0x0190, "of Shelter", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfEnchanting_Spear                   = new(0x0191, "of Enchanting", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfFortitude_Spear                    = new(0x0192, "of Fortitude", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSpearMastery                       = new(0x0193, "of Spear Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade Icy_Spear                            = new(0x020D, "Icy", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Ebon_Spear                           = new(0x020E, "Ebon", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade OfTheProfession_Spear                = new(0x022D, "of the Profession", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade Defensive_Staff                      = new(0x0091, "Defensive", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Insightful_Staff                     = new(0x009C, "Insightful", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Hale_Staff                           = new(0x009D, "Hale", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade OfAttribute_Staff                    = new(0x00C3, "of <Attribute>", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfWarding_Staff                      = new(0x00CA, "of Warding", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfShelter_Staff                      = new(0x00D0, "of Shelter", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfDefense_Staff                      = new(0x00D2, "of Defense", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSlaying_Staff                      = new(0x00D7, "of _____slaying", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfFortitude_Staff                    = new(0x00DC, "of Fortitude", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfEnchanting_Staff                   = new(0x00E1, "of Enchanting", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMastery_Staff                      = new(0x0153, "of Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfDevotion_Staff                     = new(0x0154, "of Devotion", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfValor_Staff                        = new(0x0155, "of Valor", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfEndurance_Staff                    = new(0x0156, "of Endurance", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade Swift_Staff                          = new(0x020F, "Swift", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Adept_Staff                          = new(0x0210, "Adept", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade OfTheProfession_Staff                = new(0x022B, "of the Profession", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade Icy_Sword                            = new(0x008D, "Icy", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Ebon_Sword                           = new(0x008E, "Ebon", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Shocking_Sword                       = new(0x008F, "Shocking", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Fiery_Sword                          = new(0x0090, "Fiery", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Barbed_Sword                         = new(0x0093, "Barbed", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Crippling_Sword                      = new(0x0095, "Crippling", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Cruel_Sword                          = new(0x0098, "Cruel", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Furious_Sword                        = new(0x009B, "Furious", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Poisonous_Sword                      = new(0x00A0, "Poisonous", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Zealous_Sword                        = new(0x00A6, "Zealous", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Vampiric_Sword                       = new(0x00AA, "Vampiric", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Sundering_Sword                      = new(0x00AE, "Sundering", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade OfWarding_Sword                      = new(0x00CB, "of Warding", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfShelter_Sword                      = new(0x00D1, "of Shelter", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfDefense_Sword                      = new(0x00D3, "of Defense", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSlaying_Sword                      = new(0x00D8, "of _____slaying", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfFortitude_Sword                    = new(0x00DD, "of Fortitude", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfEnchanting_Sword                   = new(0x00E2, "of Enchanting", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSwordsmanship                      = new(0x00EB, "of Swordsmanship", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfTheProfession_Sword                = new(0x022E, "of the Profession", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMemory_Wand                        = new(0x015F, "of Memory", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfQuickening_Wand                    = new(0x0160, "of Quickening", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfTheProfession_Wand                 = new(0x022A, "of the Profession", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade Survivor                             = new(0x01E6, "Survivor", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Radiant                              = new(0x01E5, "Radiant", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Stalwart                             = new(0x01E7, "Stalwart", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Brawlers                             = new(0x01E8, "Brawler's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Blessed                              = new(0x01E9, "Blessed", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Heralds                              = new(0x01EA, "Herald's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Sentrys                              = new(0x01EB, "Sentry's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Knights                              = new(0x01F9, "Knight's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Lieutenants                          = new(0x0208, "Lieutenant's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Stonefist                            = new(0x0209, "Stonefist", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Dreadnought                          = new(0x01FA, "Dreadnought", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Sentinels                            = new(0x01FB, "Sentinel's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Frostbound                           = new(0x01FC, "Frostbound", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Pyrebound                            = new(0x01FE, "Pyrebound", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Stormbound                           = new(0x01FF, "Stormbound", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Scouts                               = new(0x0201, "Scout's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Earthbound                           = new(0x01FD, "Earthbound", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Beastmasters                         = new(0x0200, "Beastmaster's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Wanderers                            = new(0x01F6, "Wanderer's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Disciples                            = new(0x01F7, "Disciple's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Anchorites                           = new(0x01F8, "Anchorite's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Bloodstained                         = new(0x020A, "Bloodstained", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Tormentors                           = new(0x01EC, "Tormentor's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Bonelace                             = new(0x01EE, "Bonelace", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade MinionMasters                        = new(0x01EF, "Minion Master's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Blighters                            = new(0x01F0, "Blighter's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Undertakers                          = new(0x01ED, "Undertaker's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Virtuosos                            = new(0x01E4, "Virtuoso's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Artificers                           = new(0x01E2, "Artificer's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Prodigys                             = new(0x01E3, "Prodigy's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Hydromancer                          = new(0x01F2, "Hydromancer", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Geomancer                            = new(0x01F3, "Geomancer", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Pyromancer                           = new(0x01F4, "Pyromancer", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Aeromancer                           = new(0x01F5, "Aeromancer", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Prismatic                            = new(0x01F1, "Prismatic", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Vanguards                            = new(0x01DE, "Vanguard's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Infiltrators                         = new(0x01DF, "Infiltrator's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Saboteurs                            = new(0x01E0, "Saboteur's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Nightstalkers                        = new(0x01E1, "Nightstalker's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Shamans                              = new(0x0204, "Shaman's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade GhostForge                           = new(0x0205, "Ghost Forge", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Mystics                              = new(0x0206, "Mystic's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Windwalker                           = new(0x0202, "Windwalker", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Forsaken                             = new(0x0203, "Forsaken", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade Centurions                           = new(0x0207, "Centurion's", ItemUpgradeType.Prefix);
    public static readonly ItemUpgrade OfAttunement                         = new(0x0211, "of Attunement", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfRecovery                           = new(0x0213, "of Recovery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfRestoration                        = new(0x0214, "of Restoration", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfClarity                            = new(0x0215, "of Clarity", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfPurity                             = new(0x0216, "of Purity", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorVigor                         = new(0x00FF, "of Minor Vigor", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorVigor2                        = new(0x00C2, "of Minor Vigor", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorVigor                      = new(0x0101, "of Superior Vigor", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorVigor                         = new(0x0100, "of Major Vigor", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfVitae                              = new(0x0212, "of Vitae", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorAbsorption                    = new(0x00FC, "of Minor Absorption", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorTactics                       = new(0x1501, "of Minor Tactics", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorStrength                      = new(0x1101, "of Minor Strength", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorAxeMastery                    = new(0x1201, "of Minor Axe Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorHammerMastery                 = new(0x1301, "of Minor Hammer Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorSwordsmanship                 = new(0x1401, "of Minor Swordsmanship", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorAbsorption                    = new(0x00FD, "of Major Absorption", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorTactics                       = new(0x1502, "of Major Tactics", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorStrength                      = new(0x1102, "of Major Strength", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorAxeMastery                    = new(0x1202, "of Major Axe Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorHammerMastery                 = new(0x1302, "of Major Hammer Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorSwordsmanship                 = new(0x1402, "of Major Swordsmanship", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorAbsorption                 = new(0x00FE, "of Superior Absorption", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorTactics                    = new(0x1503, "of Superior Tactics", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorStrength                   = new(0x1103, "of Superior Strength", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorAxeMastery                 = new(0x1203, "of Superior Axe Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorHammerMastery              = new(0x1303, "of Superior Hammer Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorSwordsmanship              = new(0x1403, "of Superior Swordsmanship", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorWildernessSurvival            = new(0x1801, "of Minor Wilderness Survival", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorExpertise                     = new(0x1701, "of Minor Expertise", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorBeastMastery                  = new(0x1601, "of Minor Beast Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorMarksmanship                  = new(0x1901, "of Minor Marksmanship", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorWildernessSurvival            = new(0x1802, "of Major Wilderness Survival", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorExpertise                     = new(0x1702, "of Major Expertise", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorBeastMastery                  = new(0x1602, "of Major Beast Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorMarksmanship                  = new(0x1902, "of Major Marksmanship", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorWildernessSurvival         = new(0x1803, "of Superior Wilderness Survival", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorExpertise                  = new(0x1703, "of Superior Expertise", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorBeastMastery               = new(0x1603, "of Superior Beast Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorMarksmanship               = new(0x1903, "of Superior Marksmanship", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorHealingPrayers                = new(0x0D01, "of Minor Healing Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorSmitingPrayers                = new(0x0E01, "of Minor Smiting Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorProtectionPrayers             = new(0x0F01, "of Minor Protection Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorDivineFavor                   = new(0x1001, "of Minor Divine Favor", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorHealingPrayers                = new(0x0D02, "of Major Healing Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorSmitingPrayers                = new(0x0E02, "of Major Smiting Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorProtectionPrayers             = new(0x0F02, "of Major Protection Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorDivineFavor                   = new(0x1002, "of Major Divine Favor", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorHealingPrayers             = new(0x0D03, "of Superior Healing Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorSmitingPrayers             = new(0x0E03, "of Superior Smiting Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorProtectionPrayers          = new(0x0F03, "of Superior Protection Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorDivineFavor                = new(0x1003, "of Superior Divine Favor", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorBloodMagic                    = new(0x0401, "of Minor Blood Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorDeathMagic                    = new(0x0501, "of Minor Death Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorCurses                        = new(0x0701, "of Minor Curses", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorSoulReaping                   = new(0x0601, "of Minor Soul Reaping", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorBloodMagic                    = new(0x0402, "of Major Blood Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorDeathMagic                    = new(0x0502, "of Major Death Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorCurses                        = new(0x0702, "of Major Curses", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorSoulReaping                   = new(0x0602, "of Major Soul Reaping", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorBloodMagic                 = new(0x0403, "of Superior Blood Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorDeathMagic                 = new(0x0503, "of Superior Death Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorCurses                     = new(0x0703, "of Superior Curses", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorSoulReaping                = new(0x0603, "of Superior Soul Reaping", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorFastCasting                   = new(0x0001, "of Minor Fast Casting", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorDominationMagic               = new(0x0201, "of Minor Domination Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorIllusionMagic                 = new(0x0101, "of Minor Illusion Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorInspirationMagic              = new(0x0301, "of Minor Inspiration Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorFastCasting                   = new(0x0002, "of Major Fast Casting", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorDominationMagic               = new(0x0202, "of Major Domination Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorIllusionMagic                 = new(0x0102, "of Major Illusion Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorInspirationMagic              = new(0x0302, "of Major Inspiration Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorFastCasting                = new(0x0003, "of Superior Fast Casting", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorDominationMagic            = new(0x0203, "of Superior Domination Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorIllusionMagic              = new(0x0103, "of Superior Illusion Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorInspirationMagic           = new(0x0303, "of Superior Inspiration Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorEnergyStorage                 = new(0x0C01, "of Minor Energy Storage", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorFireMagic                     = new(0x0A01, "of Minor Fire Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorAirMagic                      = new(0x0801, "of Minor Air Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorEarthMagic                    = new(0x0901, "of Minor Earth Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorWaterMagic                    = new(0x0B01, "of Minor Water Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorEnergyStorage                 = new(0x0C02, "of Major Energy Storage", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorFireMagic                     = new(0x0A02, "of Major Fire Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorAirMagic                      = new(0x0802, "of Major Air Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorEarthMagic                    = new(0x0902, "of Major Earth Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorWaterMagic                    = new(0x0B02, "of Major Water Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorEnergyStorage              = new(0x0C03, "of Superior Energy Storage", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorFireMagic                  = new(0x0A03, "of Superior Fire Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorAirMagic                   = new(0x0803, "of Superior Air Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorEarthMagic                 = new(0x0903, "of Superior Earth Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorWaterMagic                 = new(0x0B03, "of Superior Water Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorCriticalStrikes               = new(0x2301, "of Minor Critical Strikes", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorDaggerMastery                 = new(0x1D01, "of Minor Dagger Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorDeadlyArts                    = new(0x1E01, "of Minor Deadly Arts", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorShadowArts                    = new(0x1F01, "of Minor Shadow Arts", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorCriticalStrikes               = new(0x2302, "of Major Critical Strikes", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorDaggerMastery                 = new(0x1D02, "of Major Dagger Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorDeadlyArts                    = new(0x1E02, "of Major Deadly Arts", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorShadowArts                    = new(0x1F02, "of Major Shadow Arts", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorCriticalStrikes            = new(0x2303, "of Superior Critical Strikes", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorDaggerMastery              = new(0x1D03, "of Superior Dagger Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorDeadlyArts                 = new(0x1E03, "of Superior Deadly Arts", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorShadowArts                 = new(0x1F03, "of Superior Shadow Arts", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorChannelingMagic               = new(0x2201, "of Minor Channeling Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorRestorationMagic              = new(0x2101, "of Minor Restoration Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorCommuning                     = new(0x2001, "of Minor Communing", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorSpawningPower                 = new(0x2401, "of Minor Spawning Power", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorChannelingMagic               = new(0x2202, "of Major Channeling Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorRestorationMagic              = new(0x2102, "of Major Restoration Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorCommuning                     = new(0x2002, "of Major Communing", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorSpawningPower                 = new(0x2402, "of Major Spawning Power", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorChannelingMagic            = new(0x2203, "of Superior Channeling Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorRestorationMagic           = new(0x2103, "of Superior Restoration Magic", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorCommuning                  = new(0x2003, "of Superior Communing", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorSpawningPower              = new(0x2403, "of Superior Spawning Power", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorMysticism                     = new(0x2C01, "of Minor Mysticism", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorEarthPrayers                  = new(0x2B01, "of Minor Earth Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorScytheMastery                 = new(0x2901, "of Minor Scythe Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorWindPrayers                   = new(0x2A01, "of Minor Wind Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorMysticism                     = new(0x2C02, "of Major Mysticism", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorEarthPrayers                  = new(0x2B02, "of Major Earth Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorScytheMastery                 = new(0x2902, "of Major Scythe Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorWindPrayers                   = new(0x2A02, "of Major Wind Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorMysticism                  = new(0x2C03, "of Superior Mysticism", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorEarthPrayers               = new(0x2B03, "of Superior Earth Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorScytheMastery              = new(0x2903, "of Superior Scythe Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorWindPrayers                = new(0x2A03, "of Superior Wind Prayers", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorLeadership                    = new(0x2801, "of Minor Leadership", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorMotivation                    = new(0x2701, "of Minor Motivation", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorCommand                       = new(0x2601, "of Minor Command", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMinorSpearMastery                  = new(0x2501, "of Minor Spear Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorLeadership                    = new(0x2802, "of Major Leadership", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorMotivation                    = new(0x2702, "of Major Motivation", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorCommand                       = new(0x2602, "of Major Command", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfMajorSpearMastery                  = new(0x2502, "of Major Spear Mastery", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorLeadership                 = new(0x2803, "of Superior Leadership", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorMotivation                 = new(0x2703, "of Superior Motivation", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorCommand                    = new(0x2603, "of Superior Command", ItemUpgradeType.Suffix);
    public static readonly ItemUpgrade OfSuperiorSpearMastery               = new(0x2503, "of Superior Spear Mastery", ItemUpgradeType.Suffix);

    public static readonly ItemUpgrade UpgradeMinorRune_Warrior             = new(0x00B3, "Upgrade warrior minor rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToMinorRune_Warrior           = new(0x0167, "Applies to warrior minor rune", ItemUpgradeType.AppliesToRune);
    public static readonly ItemUpgrade UpgradeMajorRune_Warrior             = new(0x00B9, "Upgrade warrior major rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToMajorRune_Warrior           = new(0x0173, "Applies to warrior major rune", ItemUpgradeType.AppliesToRune);
    public static readonly ItemUpgrade UpgradeSuperiorRune_Warrior          = new(0x00BF, "Upgrade warrior superior rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToSuperiorRune_Warrior        = new(0x017F, "Applies to warrior superior rune", ItemUpgradeType.AppliesToRune);

    public static readonly ItemUpgrade UpgradeMinorRune_Ranger              = new(0x00B4, "Upgrade ranger minor rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToMinorRune_Ranger            = new(0x0169, "Applies to ranger minor rune", ItemUpgradeType.AppliesToRune);
    public static readonly ItemUpgrade UpgradeMajorRune_Ranger              = new(0x00BA, "Upgrade ranger major rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToMajorRune_Ranger            = new(0x0175, "Applies to ranger major rune", ItemUpgradeType.AppliesToRune);
    public static readonly ItemUpgrade UpgradeSuperiorRune_Ranger           = new(0x00C0, "Upgrade ranger superior rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToSuperiorRune_Ranger         = new(0x0181, "Applies to ranger superior rune", ItemUpgradeType.AppliesToRune);

    public static readonly ItemUpgrade UpgradeMinorRune_Monk                = new(0x00B2, "Upgrade monk major rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToMinorRune_Monk              = new(0x0165, "Applies to monk minor rune", ItemUpgradeType.AppliesToRune);
    public static readonly ItemUpgrade UpgradeMajorRune_Monk                = new(0x00B8, "Upgrade monk major rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToMajorRune_Monk              = new(0x0171, "Applies to monk major rune", ItemUpgradeType.AppliesToRune);
    public static readonly ItemUpgrade UpgradeSuperiorRune_Monk             = new(0x00BE, "Upgrade monk superior rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToSuperiorRune_Monk           = new(0x017D, "Applies to monk superior rune", ItemUpgradeType.AppliesToRune);

    public static readonly ItemUpgrade UpgradeMinorRune_Necromancer         = new(0x00B0, "Upgrade necromancer minor rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToMinorRune_Necromancer       = new(0x0161, "Applies to necromancer minor rune", ItemUpgradeType.AppliesToRune);
    public static readonly ItemUpgrade UpgradeMajorRune_Necromancer         = new(0x00B6, "Upgrade necromancer major rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToMajorRune_Necromancer       = new(0x016D, "Applies to necromancer major rune", ItemUpgradeType.AppliesToRune);
    public static readonly ItemUpgrade UpgradeSuperiorRune_Necromancer      = new(0x00BC, "Upgrade necromancer superior rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToSuperiorRune_Necromancer    = new(0x0179, "Applies to necromancer superior rune", ItemUpgradeType.AppliesToRune);

    public static readonly ItemUpgrade UpgradeMinorRune_Mesmer              = new(0x00AF, "Upgrade mesmer minor rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToMinorRune_Mesmer            = new(0x015F, "Applies to mesmer minor rune", ItemUpgradeType.AppliesToRune);
    public static readonly ItemUpgrade UpgradeMajorRune_Mesmer              = new(0x00B5, "Upgrade mesmer major rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToMajorRune_Mesmer            = new(0x016B, "Applies to mesmer major rune", ItemUpgradeType.AppliesToRune);
    public static readonly ItemUpgrade UpgradeSuperiorRune_Mesmer           = new(0x00BB, "Upgrade mesmer superior rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToSuperiorRune_Mesmer         = new(0x0177, "Applies to mesmer superior rune", ItemUpgradeType.AppliesToRune);

    public static readonly ItemUpgrade UpgradeMinorRune_Elementalist        = new(0x00B1, "Upgrade elementalist minor rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToMinorRune_Elementalist      = new(0x0163, "Applies to elementalist minor rune", ItemUpgradeType.AppliesToRune);
    public static readonly ItemUpgrade UpgradeMajorRune_Elementalist        = new(0x00B7, "Upgrade elementalist major rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToMajorRune_Elementalist      = new(0x016F, "Applies to elementalist major rune", ItemUpgradeType.AppliesToRune);
    public static readonly ItemUpgrade UpgradeSuperiorRune_Elementalist     = new(0x00BD, "Upgrade elementalist superior rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToSuperiorRune_Elementalist   = new(0x017B, "Applies to elementalist superior rune", ItemUpgradeType.AppliesToRune);

    public static readonly ItemUpgrade UpgradeMinorRune_Assassin            = new(0x013B, "Upgrade assassin minor rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToMinorRune_Assassin          = new(0x0277, "Applies to assassin minor rune", ItemUpgradeType.AppliesToRune);
    public static readonly ItemUpgrade UpgradeMajorRune_Assassin            = new(0x014C, "Upgrade assassin major rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToMajorRune_Assassin          = new(0x0279, "Applies to assassin major rune", ItemUpgradeType.AppliesToRune);
    public static readonly ItemUpgrade UpgradeSuperiorRune_Assassin         = new(0x013D, "Upgrade assassin superior rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToSuperiorRune_Assassin       = new(0x027B, "Applies to assassin superior rune", ItemUpgradeType.AppliesToRune);

    public static readonly ItemUpgrade UpgradeMinorRune_Ritualist           = new(0x013E, "Upgrade ritualist minor rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToMinorRune_Ritualist         = new(0x027D, "Applies to ritualist minor rune", ItemUpgradeType.AppliesToRune);
    public static readonly ItemUpgrade UpgradeMajorRune_Ritualist           = new(0x013F, "Upgrade ritualist major rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToMajorRune_Ritualist         = new(0x027F, "Applies to ritualist major rune", ItemUpgradeType.AppliesToRune);
    public static readonly ItemUpgrade UpgradeSuperiorRune_Ritualist        = new(0x0140, "Upgrade ritualist superior rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToSuperiorRune_Ritualist      = new(0x0281, "Applies to ritualist superior rune", ItemUpgradeType.AppliesToRune);

    public static readonly ItemUpgrade UpgradeMinorRune_Dervish             = new(0x0182, "Upgrade dervish minor rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToMinorRune_Dervish           = new(0x0305, "Applies to dervish minor rune", ItemUpgradeType.AppliesToRune);
    public static readonly ItemUpgrade UpgradeMajorRune_Dervish             = new(0x0183, "Upgrade dervish major rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToMajorRune_Dervish           = new(0x0307, "Applies to dervish major rune", ItemUpgradeType.AppliesToRune);
    public static readonly ItemUpgrade UpgradeSuperiorRune_Dervish          = new(0x0184, "Upgrade dervish superior rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToSuperiorRune_Dervish        = new(0x0309, "Applies to dervish superior rune", ItemUpgradeType.AppliesToRune);

    public static readonly ItemUpgrade UpgradeMinorRune_Paragon             = new(0x0185, "Upgrade paragon minor rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToMinorRune_Paragon           = new(0x030B, "Applies to paragon minor rune", ItemUpgradeType.AppliesToRune);
    public static readonly ItemUpgrade UpgradeMajorRune_Paragon             = new(0x0186, "Upgrade paragon major rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToMajorRune_Paragon           = new(0x030D, "Applies to paragon major rune", ItemUpgradeType.AppliesToRune);
    public static readonly ItemUpgrade UpgradeSuperiorRune_Paragon          = new(0x0187, "Upgrade paragon superior rune", ItemUpgradeType.UpgradeRune);
    public static readonly ItemUpgrade AppliesToSuperiorRune_Paragon        = new(0x030F, "Applies to paragon superior rune", ItemUpgradeType.AppliesToRune);

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
        UpgradeMinorRune_Warrior,
        AppliesToMinorRune_Warrior,
        UpgradeMajorRune_Warrior,
        AppliesToMajorRune_Warrior,
        UpgradeSuperiorRune_Warrior,
        AppliesToSuperiorRune_Warrior,
        UpgradeMinorRune_Ranger,
        AppliesToMinorRune_Ranger,
        UpgradeMajorRune_Ranger,
        AppliesToMajorRune_Ranger,
        UpgradeSuperiorRune_Ranger,
        AppliesToSuperiorRune_Ranger,
        UpgradeMinorRune_Monk,
        AppliesToMinorRune_Monk,
        UpgradeMajorRune_Monk,
        AppliesToMajorRune_Monk,
        UpgradeSuperiorRune_Monk,
        AppliesToSuperiorRune_Monk,
        UpgradeMinorRune_Mesmer,
        AppliesToMinorRune_Mesmer,
        UpgradeMajorRune_Mesmer,
        AppliesToMajorRune_Mesmer,
        UpgradeSuperiorRune_Mesmer,
        AppliesToSuperiorRune_Mesmer,
        UpgradeMinorRune_Necromancer,
        AppliesToMinorRune_Necromancer,
        UpgradeMajorRune_Necromancer,
        AppliesToMajorRune_Necromancer,
        UpgradeSuperiorRune_Necromancer,
        AppliesToSuperiorRune_Necromancer,
        UpgradeMinorRune_Elementalist,
        AppliesToMinorRune_Elementalist,
        UpgradeMajorRune_Elementalist,
        AppliesToMajorRune_Elementalist,
        UpgradeSuperiorRune_Elementalist,
        AppliesToSuperiorRune_Elementalist,
        UpgradeMinorRune_Assassin,
        AppliesToMinorRune_Assassin,
        UpgradeMajorRune_Assassin,
        AppliesToMajorRune_Assassin,
        UpgradeSuperiorRune_Assassin,
        AppliesToSuperiorRune_Assassin,
        UpgradeMinorRune_Ritualist,
        AppliesToMinorRune_Ritualist,
        UpgradeMajorRune_Ritualist,
        AppliesToMajorRune_Ritualist,
        UpgradeSuperiorRune_Ritualist,
        AppliesToSuperiorRune_Ritualist,
        UpgradeMinorRune_Dervish,
        AppliesToMinorRune_Dervish,
        UpgradeMajorRune_Dervish,
        AppliesToMajorRune_Dervish,
        UpgradeSuperiorRune_Dervish,
        AppliesToSuperiorRune_Dervish,
        UpgradeMinorRune_Paragon,
        AppliesToMinorRune_Paragon,
        UpgradeMajorRune_Paragon,
        AppliesToMajorRune_Paragon,
        UpgradeSuperiorRune_Paragon,
        AppliesToSuperiorRune_Paragon,
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
