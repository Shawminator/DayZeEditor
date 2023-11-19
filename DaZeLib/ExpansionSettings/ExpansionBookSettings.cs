using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class ExpansionBookSettings
    {
        const int CurrentVersion = 4;

        public int m_Version { get; set; } //currently 3
        public int EnableStatusTab { get; set; }
        public int EnablePartyTab { get; set; }
        public int EnableServerInfoTab { get; set; }
        public int EnableServerRulesTab { get; set; }
        public int EnableTerritoryTab { get; set; }
        public int EnableBookMenu { get; set; }
        public int CreateBookmarks { get; set; }
        public BindingList<ExpansionBookRuleCategory> RuleCategories { get; set; }
        public int DisplayServerSettingsInServerInfoTab { get; set; }
        public int ShowHaBStats { get; set; }
        public int ShowPlayerFaction { get; set; }
        public BindingList<ExpansionBookSettingCategory> SettingCategories { get; set; }
        public BindingList<ExpansionBookLink> Links { get; set; }
        public BindingList<ExpansionBookDescriptionCategory> Descriptions { get; set; }
        public BindingList<ExpansionBookCraftingCategory> CraftingCategories { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public ExpansionBookSettings()
        {
            m_Version = CurrentVersion;
            EnableBookMenu = 1;
            CreateBookmarks = 0;
            EnableStatusTab = 1;
            ShowHaBStats = 1;
            EnablePartyTab = 1;
            EnableServerInfoTab = 1;
            EnableServerRulesTab = 1;
            EnableTerritoryTab = 1;
            DefaultRules();
            DisplayServerSettingsInServerInfoTab = 1;
            DefaultSettings();
            DefaultLinks();
            DefaultDescriptions();
            DefaultCraftingCategories();
        }
        public bool checkver()
        {
            if (m_Version != CurrentVersion)
            {
                m_Version = CurrentVersion;
                isDirty = true;
                return true;
            }
            return false;
        }
        public void RenameRules()
        {
            for(int i = 0; i < RuleCategories.Count; i++)
            {
                RuleCategories[i].renamerules(i+1);
            }
        }
		void DefaultRules()
		{
			RuleCategories = new BindingList<ExpansionBookRuleCategory>()
			{ 
				new ExpansionBookRuleCategory()
				{
					CategoryName = "General",
					Rules = new BindingList<ExpansionBookRule>()
					{
						new ExpansionBookRule()
						{
							RuleParagraph = "1.1.", RuleText = "Insults, discrimination, extremist and racist statements or texts are taboo."
						},
						new ExpansionBookRule()
						{
							RuleParagraph = "1.2.", RuleText = "We reserve the right to exclude people from the server who share extremist or racist ideas or who clearly disturb the server harmony."
						},
						new ExpansionBookRule()
						{
							RuleParagraph = "1.3.", RuleText = "Decisions of the team members, both the supporter and the admin are to be accepted without discussion."
						},
						new ExpansionBookRule()
						{
							RuleParagraph = "1.4.", RuleText = "Provocations and toxic behavior will not be tolerated and punished! Be friendly to fellow players and your team, both in chat and in voice!"
						},new ExpansionBookRule()
						{
							RuleParagraph = "1.5.", RuleText = "The use of external programs, scripts and cheats is not tolerated and is punished with a permanent exclusion."
						}
					}
				}
			};
		}
		void DefaultSettings()
		{
            SettingCategories = new BindingList<ExpansionBookSettingCategory>()
            {
                new ExpansionBookSettingCategory(){
                    CategoryName = "Base-Building Settings",
                    Settings = new BindingList<ExpansionBookSetting>()
                    {
                        new ExpansionBookSetting()
                        {
                            SettingTitle = "Expansion.Settings.BaseBuilding.CanCraftVanillaBasebuilding",
                            SettingText = "",
                            SettingValue = ""
                        },
                        new ExpansionBookSetting()
                        {
                            SettingTitle = "Expansion.Settings.BaseBuilding.CanCraftExpansionBasebuilding",
                            SettingText = "",
                            SettingValue = ""
                        }
                    }
                },
                new ExpansionBookSettingCategory(){
                    CategoryName = "Raid Settings",
                    Settings = new BindingList<ExpansionBookSetting>()
                    {
                        new ExpansionBookSetting()
                        {
                            SettingTitle = "Expansion.Settings.Raid.CanRaidSafes",
                            SettingText = "",
                            SettingValue = ""
                        },
                        new ExpansionBookSetting()
                        {
                            SettingTitle = "Expansion.Settings.Raid.SafeExplosionDamageMultiplier",
                            SettingText = "",
                            SettingValue = ""
                        },
                        new ExpansionBookSetting()
                        {
                            SettingTitle = "Expansion.Settings.Raid.SafeProjectileDamageMultiplier",
                            SettingText = "",
                            SettingValue = ""
                        },
                        new ExpansionBookSetting()
                        {
                            SettingTitle = "Expansion.Settings.Raid.ExplosionTime",
                            SettingText = "",
                            SettingValue = ""
                        },
                        new ExpansionBookSetting()
                        {
                            SettingTitle = "Expansion.Settings.Raid.ExplosionDamageMultiplier",
                            SettingText = "",
                            SettingValue = ""
                        },
                        new ExpansionBookSetting()
                        {
                            SettingTitle = "Expansion.Settings.Raid.ProjectileDamageMultiplier",
                            SettingText = "",
                            SettingValue = ""
                        }
                    }
                },
                new ExpansionBookSettingCategory(){
                    CategoryName = "Territory Settings",
                    Settings = new BindingList<ExpansionBookSetting>()
                    {
                        new ExpansionBookSetting()
                        {
                            SettingTitle = "Expansion.Settings.Territory.TerritorySize",
                            SettingText = "",
                            SettingValue = ""
                        },
                        new ExpansionBookSetting()
                        {
                            SettingTitle = "Expansion.Settings.Territory.UseWholeMapForInviteList",
                            SettingText = "",
                            SettingValue = ""
                        }
                    }
                },
                new ExpansionBookSettingCategory(){
                    CategoryName = "Map Settings",
                    Settings = new BindingList<ExpansionBookSetting>()
                    {
                        new ExpansionBookSetting()
                        {
                            SettingTitle = "Expansion.Settings.Map.NeedGPSItemForKeyBinding",
                            SettingText = "",
                            SettingValue = ""
                        },
                        new ExpansionBookSetting()
                        {
                            SettingTitle = "Expansion.Settings.Map.NeedGPSItemForKeyBinding",
                            SettingText = "",
                            SettingValue = ""
                        }
                    }
                },
                new ExpansionBookSettingCategory(){
                    CategoryName = "Party Settings",
                    Settings = new BindingList<ExpansionBookSetting>()
                    {
                        new ExpansionBookSetting()
                        {
                            SettingTitle = "Expansion.Settings.Party.MaxMembersInParty",
                            SettingText = "",
                            SettingValue = ""
                        },
                        new ExpansionBookSetting()
                        {
                            SettingTitle = "Expansion.Settings.Party.UseWholeMapForInviteList",
                            SettingText = "",
                            SettingValue = ""
                        }
                    }
                }
            };
		}
		void DefaultLinks()
		{
            Links = new BindingList<ExpansionBookLink>()
            {
                new ExpansionBookLink()
                {
                    Name = "Homepage",
                    URL = "https://www.google.com/",
                    IconName = "Homepage",
                    IconColor = -14473430
                },
                new ExpansionBookLink()
                {
                    Name = "Feedback",
                    URL = "https://www.google.com/",
                    IconName = "Forums",
                    IconColor = -14473430
                },
                new ExpansionBookLink()
                {
                    Name = "Discord",
                    URL = "https://www.google.com/",
                    IconName = "Discord",
                    IconColor = -9270822
                },
                new ExpansionBookLink()
                {
                    Name = "Patreon",
                    URL = "https://www.patreon.com/dayzexpansion",
                    IconName = "Patreon",
                    IconColor = -432044
                },
                new ExpansionBookLink()
                {
                    Name = "Steam",
                    URL = "https://steamcommunity.com/sharedfiles/filedetails/?id=2116151222",
                    IconName = "Steam",
                    IconColor = -14006434
                },
                new ExpansionBookLink()
                {
                    Name = "Reddit",
                    URL = "https://www.reddit.com/r/ExpansionProject/",
                    IconName = "Reddit",
                    IconColor = -12386303
                },
                new ExpansionBookLink()
                {
                    Name = "GitHub",
                    URL = "https://github.com/salutesh/DayZ-Expansion-Scripts/wiki",
                    IconName = "GitHub",
                    IconColor = -16777216
                },
                new ExpansionBookLink()
                {
                    Name = "YouTube",
                    URL = "https://www.youtube.com/channel/UCZNgSvIEWfru963tQZOAVJg",
                    IconName = "YouTube",
                    IconColor = -65536
                },
                new ExpansionBookLink()
                {
                    Name = "Twitter",
                    URL = "https://twitter.com/DayZExpansion",
                    IconName = "Twitter",
                    IconColor = -14835214
                }
            };
		}
		void DefaultDescriptions()
		{
            Descriptions = new BindingList<ExpansionBookDescriptionCategory>()
            {
                 new ExpansionBookDescriptionCategory()
                 {
                     CategoryName = "General Info",
                     Descriptions = new BindingList<ExpansionBookDescription>()
                     {
                         new ExpansionBookDescription()
                         {
                             DescriptionText = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet."
                         },
                         new ExpansionBookDescription()
                         {
                             DescriptionText = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet."
                         },
                         new ExpansionBookDescription()
                         {
                             DescriptionText = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet."
                         }
                     }
                 },
                 new ExpansionBookDescriptionCategory()
                 {
                     CategoryName = "Mod Info",
                     Descriptions = new BindingList<ExpansionBookDescription>()
                     {
                         new ExpansionBookDescription()
                         {
                             DescriptionText = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet."
                         },
                         new ExpansionBookDescription()
                         {
                             DescriptionText = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet."
                         },
                         new ExpansionBookDescription()
                         {
                             DescriptionText = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet."
                         }
                     }
                 }
            };
		}
		void DefaultCraftingCategories()
		{
            CraftingCategories = new BindingList<ExpansionBookCraftingCategory>()
            {
                new ExpansionBookCraftingCategory()
                {
                    CategoryName = "Accessories",
                    Results = new BindingList<string>() {
                        "Armband_",
                        "Armband_White",
                        "EyePatch_Improvised"
                    }
                },
                new ExpansionBookCraftingCategory()
                {
                    CategoryName = "Backpacks",
                    Results = new BindingList<string>() {
                        "CourierBag",
                        "FurCourierBag",
                        "FurImprovisedBag",
                        "ImprovisedBag",
                        "LeatherSack_Brown"
                    }
                },
                new ExpansionBookCraftingCategory()
                {
                    CategoryName = "Base-Building",
                    Results = new BindingList<string>() {
                    "ExpansionBarbedWireKit",
                    "FenceKit",
                    "ExpansionFloorKit",
                    "ExpansionHelipadKit",
                    "ExpansionHescoKit",
                    "ExpansionRampKit",
                    "ShelterKit",
                    "ExpansionStairKit",
                    "TerritoryFlagKit",
                    "ExpansionWallKit",
                    "WatchtowerKit"
                    }
                },
                new ExpansionBookCraftingCategory()
                {
                    CategoryName = "Camouflage",
                    Results = new BindingList<string>() {
                    "Camonet",
                    "GhillieAtt_Tan",
                    "GhillieAtt_Mossy",
                    "GhillieAtt_Woodland",
                    "GhillieBushrag_Tan",
                    "GhillieBushrag_Mossy",
                    "GhillieBushrag_Woodland",
                    "GhillieHood_Tan",
                    "GhillieHood_Mossy",
                    "GhillieHood_Woodland",
                    "GhillieSuit_Tan",
                    "GhillieSuit_Mossy",
                    "GhillieSuit_Woodland",
                    "GhillieTop_Tan",
                    "GhillieTop_Mossy",
                    "GhillieTop_Woodland"
                    }
                },
                new ExpansionBookCraftingCategory()
                {
                    CategoryName = "Cooking",
                    Results = new BindingList<string>() {
                    "Fireplace",
                    "Firewood",
                    "HandDrillKit"
                    }
                },
                new ExpansionBookCraftingCategory()
                {
                    CategoryName = "Fishing",
                    Results = new BindingList<string>() {
                    "Bait",
                    "BoneBait",
                    "BoneHook",
                    "ImprovisedFishingRod"
                    }
                },
                 new ExpansionBookCraftingCategory()
                 {
                     CategoryName = "Food",
                     Results = new BindingList<string>() {
                        "CarpFilletMeat",
                        "MackerelFilletMeat",
                        "ExpansionMilkBottle",
                        "Potato",
                        "SlicedPumpkin"
                     }
                 },
                new ExpansionBookCraftingCategory(){
                    CategoryName = "Horticulture",
                    Results = new BindingList<string>() {
                        "PepperSeeds",
                        "PumpkinSeeds",
                        "TomatoSeeds",
                        "ZucchiniSeeds"
                    }
                },
                new ExpansionBookCraftingCategory()
                {
                    CategoryName = "Lights",
                    Results = new BindingList<string>() {
                        "LongTorch",
                        "Torch"
                    }
                },
                new ExpansionBookCraftingCategory()
                {
                    CategoryName = "Medical Supplies",
                    Results = new BindingList<string>() {
                        "BloodBagIV",
                        "SalineBagIV",
                        "Splint"
                    }
                },
                new ExpansionBookCraftingCategory()
                {
                    CategoryName = "Melee Weapons",
                    Results = new BindingList<string>() {
                        "NailedBaseballBat",
                        "StoneKnife"
                    }
                },
                new ExpansionBookCraftingCategory()
                {
                    CategoryName = "Storage",
                    Results = new BindingList<string>() {
                        "WoodenCrate"
                    }
                },
                new ExpansionBookCraftingCategory()
                {
                CategoryName = "Supplies",
                    Results = new BindingList<string>() {
                        "BoarPelt",
                        "BurlapSack",
                        "BurlapStrip",
                        "LongWoodenStick",
                        "ExpansionLumber1",
                        "ExpansionLumber1_5",
                        "ExpansionLumber3",
                        "Nails",
                        "Netting",
                        "Rag",
                        "Rope",
                        "SharpWoodenStick",
                        "SmallStone",
                        "TannedLeather",
                        "WoodenPlank",
                        "WoodenStick"
                    }
                },
                new ExpansionBookCraftingCategory()
                {
                     CategoryName = "Weapon Modifications",
                     Results = new BindingList<string>() {
                        "SawedoffIzh18Shotgun"
                     }
                },
                new ExpansionBookCraftingCategory()
                {
                     CategoryName = "Weapon Attachments",
                     Results = new BindingList<string>() {
                            "ImprovisedSuppressor"
                     }
                }
            };
		}
	}
    public class ExpansionBookRuleCategory
    {
        public string CategoryName { get; set; }
        public BindingList<ExpansionBookRule> Rules { get; set; }

        public ExpansionBookRuleCategory()
        {
            Rules = new BindingList<ExpansionBookRule>();
        }

        public override string ToString()
        {
            return CategoryName;
        }
        public void renamerules(int i)
        {
            for (int j = 0; j < Rules.Count; j++)
            {
                Rules[j].RuleParagraph = i.ToString() + "." + (j + 1).ToString();
            }
        }
    }
    public class ExpansionBookRule
    {
        public string RuleParagraph { get; set; }
        public string RuleText { get; set; }

        public ExpansionBookRule()
        {
            RuleParagraph = "";
            RuleText = "";
        }
        public override string ToString()
        {
            return RuleParagraph;
        }
    }
    public class ExpansionBookSettingCategory
    {
        public string CategoryName { get; set; }
        public BindingList<ExpansionBookSetting> Settings { get; set; }

        public ExpansionBookSettingCategory()
        {
            Settings = new BindingList<ExpansionBookSetting>();
        }

        public override string ToString()
        {
            return CategoryName;
        }
    }
    public class ExpansionBookSetting
    {
        public string SettingTitle { get; set; }
        public string SettingText { get; set; }
        public string SettingValue { get; set; }
    }
    public class ExpansionBookLink
    {
        public string Name { get; set; }
        public string URL { get; set; }
        public string IconName { get; set; }
        public int IconColor { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
    public class ExpansionBookDescriptionCategory
    {
        public string CategoryName { get; set; }
        public BindingList<ExpansionBookDescription> Descriptions { get; set; }

        public ExpansionBookDescriptionCategory()
        {
            Descriptions = new BindingList<ExpansionBookDescription>();
        }
       
        public override string ToString()
        {
            return CategoryName;
        }
    }
    public class ExpansionBookDescription
    {
        public string DescriptionText { get; set; }

        [JsonIgnore]
        public string DTName { get; set; }

        public override string ToString()
        {
            return DTName;
        }
    }
    public class ExpansionBookCraftingCategory
    {
        public string CategoryName { get; set; }
        public BindingList<string> Results { get; set; }

        public ExpansionBookCraftingCategory()
        {
            Results = new BindingList<string>();
        }

        public override string ToString()
        {
            return CategoryName;
        }
    }
}
