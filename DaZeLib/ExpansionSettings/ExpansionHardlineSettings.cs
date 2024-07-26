using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace DayZeLib
{
    public enum ExpansionHardlineItemRarity
    {
        NONE,
        Poor,
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Mythic,
        Exotic,
        Quest,
        Collectable,
        Ingredient
    };
    public class ExpansionHardlineSettings
    {
        const int CurrentVersion = 11;

        public int m_Version { get; set; }

        public int PoorItemRequirement { get; set; }
        public int CommonItemRequirement { get; set; }
        public int UncommonItemRequirement { get; set; }
        public int RareItemRequirement { get; set; }
        public int EpicItemRequirement { get; set; }
        public int LegendaryItemRequirement { get; set; }
        public int MythicItemRequirement { get; set; }
        public int ExoticItemRequirement { get; set; }
        public int ShowHardlineHUD { get; set; }
        public int UseReputation { get; set; }
        public int UseFactionReputation { get; set; }
        public int EnableFactionPersistence { get; set; }
        public int EnableItemRarity { get; set; }
        public int UseItemRarityOnInventoryIcons { get; set; }
        public int UseItemRarityForMarketPurchase { get; set; }
        public int UseItemRarityForMarketSell { get; set; }
        public int MaxReputation { get; set; }
        public int ReputationLossOnDeath { get; set; }
        public int DefaultItemRarity { get; set; }
        public int ItemRarityParentSearch { get; set; }
        public Dictionary<string, int> EntityReputation { get; set; }
        public Dictionary<string, int> ItemRarity { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty;
        [JsonIgnore]
        public BindingList<string> PoorItems { get; set; }
        [JsonIgnore]
        public BindingList<string> CommonItems { get; set; }
        [JsonIgnore]
        public BindingList<string> UncommonItems { get; set; }
        [JsonIgnore]
        public BindingList<string> RareItems { get; set; }
        [JsonIgnore]
        public BindingList<string> EpicItems { get; set; }
        [JsonIgnore]
        public BindingList<string> LegendaryItems { get; set; }
        [JsonIgnore]
        public BindingList<string> MythicItems { get; set; }
        [JsonIgnore]
        public BindingList<string> ExoticItems { get; set; }
        [JsonIgnore]
        public BindingList<EntityReputationlevels> entityreps { get; set; }

        public ExpansionHardlineSettings()
        {
            m_Version = CurrentVersion;
            PoorItemRequirement = 0;
            CommonItemRequirement = 0;
            UncommonItemRequirement = 1000;
            RareItemRequirement = 2000;
            EpicItemRequirement = 3000;
            LegendaryItemRequirement = 4000;
            MythicItemRequirement = 5000;
            ExoticItemRequirement = 10000;

            ShowHardlineHUD = 1;
            UseReputation = 1;
            UseFactionReputation = 0;
            EnableFactionPersistence = 0;
            EnableItemRarity = 1;
            UseItemRarityOnInventoryIcons = 1;
            UseItemRarityForMarketPurchase = 1;
            UseItemRarityForMarketSell = 1;

            MaxReputation = 0;

            ReputationLossOnDeath = 100;

            DefaultItemRarity = (int)ExpansionHardlineItemRarity.Common;

            ItemRarityParentSearch = 0;

            DefaultEntityReputation();

            SetDefaultItemRarity();

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
        public void ConvertDictionarytoLevels()
        {
            List<string> InitialPoorItems = new List<string>();
            List<string> InitialCommonItems = new List<string>();
            List<string> InitialUncommonItems = new List<string>();
            List<string> InitialRareItems = new List<string>();
            List<string> InitialEpicItems = new List<string>();
            List<string> InitialLegendaryItems = new List<string>();
            List<string> InitialMythicItems = new List<string>();
            List<string> InitialExoticItems = new List<string>();

            if (ItemRarity == null)
                ItemRarity = new Dictionary<string, int>();

            foreach (KeyValuePair<string, int> item in ItemRarity)
            {
                string useitem = item.Key;
                if (useitem != useitem.ToLower())
                {
                    useitem = useitem.ToLower();
                    isDirty = true;
                }

                switch (item.Value)
                {
                    case 1:
                        InitialPoorItems.Add(useitem);
                        break;
                    case 2:
                        InitialCommonItems.Add(useitem);
                        break;
                    case 3:
                        InitialUncommonItems.Add(useitem);
                        break;
                    case 4:
                        InitialRareItems.Add(useitem);
                        break;
                    case 5:
                        InitialEpicItems.Add(useitem);
                        break;
                    case 6:
                        InitialLegendaryItems.Add(useitem);
                        break;
                    case 7:
                        InitialMythicItems.Add(useitem);
                        break;
                    case 8:
                        InitialExoticItems.Add(useitem);
                        break;

                }
            }

            InitialPoorItems.Sort();
            InitialCommonItems.Sort();
            InitialUncommonItems.Sort();
            InitialRareItems.Sort();
            InitialEpicItems.Sort();
            InitialLegendaryItems.Sort();
            InitialMythicItems.Sort();
            InitialExoticItems.Sort();

            PoorItems = new BindingList<string>(InitialPoorItems);
            CommonItems = new BindingList<string>(InitialCommonItems);
            UncommonItems = new BindingList<string>(InitialUncommonItems);
            RareItems = new BindingList<string>(InitialRareItems);
            EpicItems = new BindingList<string>(InitialEpicItems);
            LegendaryItems = new BindingList<string>(InitialLegendaryItems);
            MythicItems = new BindingList<string>(InitialMythicItems);
            ExoticItems = new BindingList<string>(InitialExoticItems);



        }
        public void convertliststoDict()
        {
            ItemRarity = new Dictionary<string, int>();
            foreach (string item in PoorItems)
            {
                ItemRarity.Add(item, 1);
            }
            foreach (string item in CommonItems)
            {
                ItemRarity.Add(item, 2);
            }
            foreach (string item in UncommonItems)
            {
                ItemRarity.Add(item, 3);
            }
            foreach (string item in RareItems)
            {
                ItemRarity.Add(item, 4);
            }
            foreach (string item in EpicItems)
            {
                ItemRarity.Add(item, 5);
            }
            foreach (string item in LegendaryItems)
            {
                ItemRarity.Add(item, 6);
            }
            foreach (string item in MythicItems)
            {
                ItemRarity.Add(item, 7);
            }
            foreach (string item in ExoticItems)
            {
                ItemRarity.Add(item, 8);
            }
        }
        public BindingList<string> getlist(string type)
        {
            switch (type)
            {
                case "Poor":
                    return PoorItems;
                case "Common":
                    return CommonItems;
                case "Uncommon":
                    return UncommonItems;
                case "Rare":
                    return RareItems;
                case "Epic":
                    return EpicItems;
                case "Legendary":
                    return LegendaryItems;
                case "Mythic":
                    return MythicItems;
                case "Exotic":
                    return ExoticItems;
            }
            return new BindingList<string>();
        }
        public string GetListfromitem(string item)
        {
            if (PoorItems.Contains(item))
                return "Poor";
            if (CommonItems.Contains(item))
                return "Common";
            if (UncommonItems.Contains(item))
                return "Uncommon";
            if (RareItems.Contains(item))
                return "Rare";
            if (EpicItems.Contains(item))
                return "Epic";
            if (LegendaryItems.Contains(item))
                return "Legendary";
            if (MythicItems.Contains(item))
                return "Mythic";
            if (ExoticItems.Contains(item))
                return "Exotic";
            return "none";

        }
        public int getRequirment(string type)
        {
            switch (type)
            {
                case "Poor":
                    return PoorItemRequirement;
                case "Common":
                    return CommonItemRequirement;
                case "Uncommon":
                    return UncommonItemRequirement;
                case "Rare":
                    return RareItemRequirement;
                case "Epic":
                    return EpicItemRequirement;
                case "Legendary":
                    return LegendaryItemRequirement;
                case "Mythic":
                    return MythicItemRequirement;
                case "Exotic":
                    return ExoticItemRequirement;
            }
            return 0;
        }
        public void SetRequirment(string type, int value)
        {
            switch (type)
            {
                case "Poor":
                    PoorItemRequirement = value;
                    break;
                case "Common":
                    CommonItemRequirement = value;
                    break;
                case "Uncommon":
                    UncommonItemRequirement = value;
                    break;
                case "Rare":
                    RareItemRequirement = value;
                    break;
                case "Epic":
                    EpicItemRequirement = value;
                    break;
                case "Legendary":
                    LegendaryItemRequirement = value;
                    break;
                case "Mythic":
                    MythicItemRequirement = value;
                    break;
                case "Exotic":
                    ExoticItemRequirement = value;
                    break;
            }
        }
        public void setIntValue(string v, int value)
        {
            GetType().GetProperty(v).SetValue(this, value, null);
        }
        public void COnvertReputationDictionarytolist()
        {
            entityreps = new BindingList<EntityReputationlevels>();
            if (EntityReputation == null)
                EntityReputation = new Dictionary<string, int>();
            foreach (KeyValuePair<string, int> item in EntityReputation)
            {
                string classaname = item.Key;
                int level = item.Value;
                entityreps.Add(new EntityReputationlevels(classaname, level));
            }
        }
        public void convertreplisttodict()
        {
            EntityReputation = new Dictionary<string, int>();
            foreach (EntityReputationlevels item in entityreps)
            {
                EntityReputation.Add(item.Classname, item.Level);
            }
        }
        public void DefaultEntityReputation()
        {
            EntityReputation = new Dictionary<string, int>();
            //! Player
            EntityReputation.Add("PlayerBase", 100);

            //! Specific animals
            EntityReputation.Add("Animal_GallusGallusDomesticus", 1);
            EntityReputation.Add("Animal_UrsusArctos", 50);
            EntityReputation.Add("Animal_UrsusMaritimus", 50);

            //! All other animals
            EntityReputation.Add("AnimalBase", 10);

            //! Military Infected
            EntityReputation.Add("ZmbM_SoldierNormal_Base", 20);

            //! NBC Infected
            EntityReputation.Add("ZmbM_NBC_Yellow", 20);
            EntityReputation.Add("ZmbM_NBC_Grey", 20);

            //! All other Infected
            EntityReputation.Add("ZombieBase", 10);

            EntityReputation.Add("eAIBase", 100);
        }
        public void SetDefaultItemRarity()
        {
            ItemRarity = new Dictionary<string, int>();
            //! Ammo
            AddItem("Ammo_12gaPellets", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Ammo_12gaRubberSlug", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Ammo_12gaSlug", ExpansionHardlineItemRarity.Uncommon);

            AddItem("Ammo_22", ExpansionHardlineItemRarity.Uncommon);

            AddItem("Ammo_308Win", ExpansionHardlineItemRarity.Rare);
            AddItem("Ammo_308WinTracer", ExpansionHardlineItemRarity.Rare);

            AddItem("Ammo_357", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Ammo_380", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Ammo_45ACP", ExpansionHardlineItemRarity.Uncommon);

            AddItem("Ammo_545x39", ExpansionHardlineItemRarity.Rare);
            AddItem("Ammo_545x39Tracer", ExpansionHardlineItemRarity.Rare);

            AddItem("Ammo_556x45", ExpansionHardlineItemRarity.Rare);
            AddItem("Ammo_556x45Tracer", ExpansionHardlineItemRarity.Rare);

            AddItem("Ammo_762x39", ExpansionHardlineItemRarity.Rare);
            AddItem("Ammo_762x39Tracer", ExpansionHardlineItemRarity.Rare);

            AddItem("Ammo_762x54", ExpansionHardlineItemRarity.Rare);
            AddItem("Ammo_762x54Tracer", ExpansionHardlineItemRarity.Rare);

            AddItem("Ammo_9x19", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Ammo_9x39", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Ammo_9x39AP", ExpansionHardlineItemRarity.Uncommon);

            AddItem("Ammo_Flare", ExpansionHardlineItemRarity.Uncommon);

            AddItem("Ammo_HuntingBolt", ExpansionHardlineItemRarity.Uncommon);

            //! Ammo boxes
            AddItem("AmmoBox_00buck_10rnd", ExpansionHardlineItemRarity.Rare);
            AddItem("AmmoBox_12gaSlug_10Rnd", ExpansionHardlineItemRarity.Rare);
            AddItem("AmmoBox_12gaRubberSlug_10Rnd", ExpansionHardlineItemRarity.Rare);

            AddItem("AmmoBox_22_50Rnd", ExpansionHardlineItemRarity.Rare);

            AddItem("AmmoBox_308Win_20Rnd", ExpansionHardlineItemRarity.Rare);
            AddItem("AmmoBox_308WinTracer_20Rnd", ExpansionHardlineItemRarity.Rare);

            AddItem("AmmoBox_357_20Rnd", ExpansionHardlineItemRarity.Rare);
            AddItem("AmmoBox_380_35rnd", ExpansionHardlineItemRarity.Rare);
            AddItem("AmmoBox_45ACP_25rnd", ExpansionHardlineItemRarity.Rare);

            AddItem("AmmoBox_545x39_20Rnd", ExpansionHardlineItemRarity.Rare);
            AddItem("AmmoBox_545x39Tracer_20Rnd", ExpansionHardlineItemRarity.Rare);

            AddItem("AmmoBox_556x45_20Rnd", ExpansionHardlineItemRarity.Rare);
            AddItem("AmmoBox_556x45Tracer_20Rnd", ExpansionHardlineItemRarity.Rare);

            AddItem("AmmoBox_762x39_20Rnd", ExpansionHardlineItemRarity.Rare);
            AddItem("AmmoBox_762x39Tracer_20Rnd", ExpansionHardlineItemRarity.Rare);

            AddItem("AmmoBox_762x54_20Rnd", ExpansionHardlineItemRarity.Rare);
            AddItem("AmmoBox_762x54Tracer_20Rnd", ExpansionHardlineItemRarity.Rare);

            AddItem("AmmoBox_9x19_25rnd", ExpansionHardlineItemRarity.Rare);
            AddItem("AmmoBox_9x39_20rnd", ExpansionHardlineItemRarity.Rare);
            AddItem("AmmoBox_9x39AP_20rnd", ExpansionHardlineItemRarity.Rare);
            //! Armbands
            AddItem("Armband_White", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_Yellow", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_Orange", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_Red", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_Green", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_Pink", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_Black", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_APA", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_Altis", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_BabyDeer", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_Bear", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_Bohemia", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_BrainZ", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_CDF", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_CHEL", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_CMC", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_Cannibals", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_Chedaki", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_Chernarus", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_DayZ", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_HunterZ", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_Livonia", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_LivoniaArmy", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_LivoniaPolice", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_NAPA", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_NSahrani", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_Pirates", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_RSTA", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_Refuge", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_Rooster", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_SSahrani", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_Snake", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_TEC", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_UEC", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_Wolf", ExpansionHardlineItemRarity.Common);
            AddItem("Armband_Zenit", ExpansionHardlineItemRarity.Common);

            //! Assault Rifles
            AddItem("FAL", ExpansionHardlineItemRarity.Epic);
            AddItem("AKM", ExpansionHardlineItemRarity.Epic);
            AddItem("AK101", ExpansionHardlineItemRarity.Epic);
            AddItem("AK74", ExpansionHardlineItemRarity.Epic);
            AddItem("M4A1", ExpansionHardlineItemRarity.Epic);
            AddItem("M16A2", ExpansionHardlineItemRarity.Epic);
            AddItem("FAMAS", ExpansionHardlineItemRarity.Epic);

            AddItem("Aug", ExpansionHardlineItemRarity.Epic);
            AddItem("AugShort", ExpansionHardlineItemRarity.Epic);
            //! Backpacks
            AddItem("ChildBag_Red", ExpansionHardlineItemRarity.Common);
            AddItem("ChildBag_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("ChildBag_Green", ExpansionHardlineItemRarity.Common);

            AddItem("DryBag_Orange", ExpansionHardlineItemRarity.Uncommon);
            AddItem("DryBag_Yellow", ExpansionHardlineItemRarity.Uncommon);
            AddItem("DryBag_Blue", ExpansionHardlineItemRarity.Uncommon);
            AddItem("DryBag_Black", ExpansionHardlineItemRarity.Uncommon);
            AddItem("DryBag_Red", ExpansionHardlineItemRarity.Uncommon);
            AddItem("DryBag_Green", ExpansionHardlineItemRarity.Uncommon);

            AddItem("TaloonBag_Blue", ExpansionHardlineItemRarity.Uncommon);
            AddItem("TaloonBag_Orange", ExpansionHardlineItemRarity.Uncommon);
            AddItem("TaloonBag_Violet", ExpansionHardlineItemRarity.Uncommon);
            AddItem("TaloonBag_Green", ExpansionHardlineItemRarity.Uncommon);

            AddItem("SmershBag", ExpansionHardlineItemRarity.Rare);

            AddItem("AssaultBag_Black", ExpansionHardlineItemRarity.Rare);
            AddItem("AssaultBag_Green", ExpansionHardlineItemRarity.Rare);
            AddItem("AssaultBag_Ttsko", ExpansionHardlineItemRarity.Rare);

            AddItem("HuntingBag", ExpansionHardlineItemRarity.Rare);
            AddItem("TortillaBag", ExpansionHardlineItemRarity.Rare);
            AddItem("CoyoteBag_Brown", ExpansionHardlineItemRarity.Rare);

            AddItem("MountainBag_Red", ExpansionHardlineItemRarity.Epic);
            AddItem("MountainBag_Blue", ExpansionHardlineItemRarity.Epic);
            AddItem("MountainBag_Orange", ExpansionHardlineItemRarity.Epic);
            AddItem("MountainBag_Green", ExpansionHardlineItemRarity.Epic);

            AddItem("AliceBag_Green", ExpansionHardlineItemRarity.Epic);
            AddItem("AliceBag_Black", ExpansionHardlineItemRarity.Epic);
            AddItem("AliceBag_Camo", ExpansionHardlineItemRarity.Epic);

            //! Bandanas
            AddItem("Bandana_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("Bandana_Pink", ExpansionHardlineItemRarity.Common);
            AddItem("Bandana_Yellow", ExpansionHardlineItemRarity.Common);
            AddItem("Bandana_RedPattern", ExpansionHardlineItemRarity.Common);
            AddItem("Bandana_BlackPattern", ExpansionHardlineItemRarity.Common);
            AddItem("Bandana_PolkaPattern", ExpansionHardlineItemRarity.Common);
            AddItem("Bandana_Greenpattern", ExpansionHardlineItemRarity.Common);
            AddItem("Bandana_CamoPattern", ExpansionHardlineItemRarity.Common);

            //! Batteries
            AddItem("Battery9V", ExpansionHardlineItemRarity.Uncommon);
            AddItem("CarBattery", ExpansionHardlineItemRarity.Rare);
            AddItem("TruckBattery", ExpansionHardlineItemRarity.Rare);

            //! Bayonets
            AddItem("Mosin_Bayonet", ExpansionHardlineItemRarity.Uncommon);
            AddItem("SKS_Bayonet", ExpansionHardlineItemRarity.Uncommon);
            AddItem("M9A1_Bayonet", ExpansionHardlineItemRarity.Uncommon);
            AddItem("AK_Bayonet", ExpansionHardlineItemRarity.Uncommon);

            //! Belts
            AddItem("CivilianBelt", ExpansionHardlineItemRarity.Uncommon);
            AddItem("MilitaryBelt", ExpansionHardlineItemRarity.Uncommon);
            AddItem("HipPack_Black", ExpansionHardlineItemRarity.Uncommon);
            AddItem("HipPack_Green", ExpansionHardlineItemRarity.Uncommon);
            AddItem("HipPack_Medical", ExpansionHardlineItemRarity.Uncommon);
            AddItem("HipPack_Party", ExpansionHardlineItemRarity.Uncommon);

            //! Blouses and Suits
            AddItem("Blouse_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("Blouse_Violet", ExpansionHardlineItemRarity.Common);
            AddItem("Blouse_White", ExpansionHardlineItemRarity.Common);
            AddItem("Blouse_Green", ExpansionHardlineItemRarity.Common);

            AddItem("ManSuit_Beige", ExpansionHardlineItemRarity.Common);
            AddItem("ManSuit_Black", ExpansionHardlineItemRarity.Common);
            AddItem("ManSuit_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("ManSuit_Brown", ExpansionHardlineItemRarity.Common);
            AddItem("ManSuit_DarkGrey", ExpansionHardlineItemRarity.Common);
            AddItem("ManSuit_Khaki", ExpansionHardlineItemRarity.Common);
            AddItem("ManSuit_LightGrey", ExpansionHardlineItemRarity.Common);
            AddItem("ManSuit_White", ExpansionHardlineItemRarity.Common);

            AddItem("WomanSuit_Beige", ExpansionHardlineItemRarity.Common);
            AddItem("WomanSuit_Black", ExpansionHardlineItemRarity.Common);
            AddItem("WomanSuit_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("WomanSuit_Brown", ExpansionHardlineItemRarity.Common);
            AddItem("WomanSuit_DarkGrey", ExpansionHardlineItemRarity.Common);
            AddItem("WomanSuit_Khaki", ExpansionHardlineItemRarity.Common);
            AddItem("WomanSuit_LightGrey", ExpansionHardlineItemRarity.Common);
            AddItem("WomanSuit_White", ExpansionHardlineItemRarity.Common);

            //! Boots and Shoes
            AddItem("AthleticShoes_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("AthleticShoes_Brown", ExpansionHardlineItemRarity.Common);
            AddItem("AthleticShoes_Grey", ExpansionHardlineItemRarity.Common);
            AddItem("AthleticShoes_Black", ExpansionHardlineItemRarity.Common);
            AddItem("AthleticShoes_Green", ExpansionHardlineItemRarity.Common);

            AddItem("JoggingShoes_Black", ExpansionHardlineItemRarity.Common);
            AddItem("JoggingShoes_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("JoggingShoes_Red", ExpansionHardlineItemRarity.Common);
            AddItem("JoggingShoes_Violet", ExpansionHardlineItemRarity.Common);
            AddItem("JoggingShoes_White", ExpansionHardlineItemRarity.Common);

            AddItem("Sneakers_Green", ExpansionHardlineItemRarity.Common);
            AddItem("Sneakers_Red", ExpansionHardlineItemRarity.Common);
            AddItem("Sneakers_White", ExpansionHardlineItemRarity.Common);
            AddItem("Sneakers_Black", ExpansionHardlineItemRarity.Common);
            AddItem("Sneakers_Gray", ExpansionHardlineItemRarity.Common);

            AddItem("Ballerinas_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("Ballerinas_Red", ExpansionHardlineItemRarity.Common);
            AddItem("Ballerinas_White", ExpansionHardlineItemRarity.Common);
            AddItem("Ballerinas_Yellow", ExpansionHardlineItemRarity.Common);

            AddItem("DressShoes_White", ExpansionHardlineItemRarity.Common);
            AddItem("DressShoes_Beige", ExpansionHardlineItemRarity.Common);
            AddItem("DressShoes_Black", ExpansionHardlineItemRarity.Common);
            AddItem("DressShoes_Brown", ExpansionHardlineItemRarity.Common);
            AddItem("DressShoes_Sunburst", ExpansionHardlineItemRarity.Common);

            AddItem("HikingBootsLow_Blue", ExpansionHardlineItemRarity.Uncommon);
            AddItem("HikingBootsLow_Grey", ExpansionHardlineItemRarity.Uncommon);
            AddItem("HikingBootsLow_Beige", ExpansionHardlineItemRarity.Uncommon);
            AddItem("HikingBootsLow_Black", ExpansionHardlineItemRarity.Uncommon);

            AddItem("WorkingBoots_Yellow", ExpansionHardlineItemRarity.Uncommon);
            AddItem("WorkingBoots_Grey", ExpansionHardlineItemRarity.Uncommon);
            AddItem("WorkingBoots_Brown", ExpansionHardlineItemRarity.Uncommon);
            AddItem("WorkingBoots_Beige", ExpansionHardlineItemRarity.Uncommon);
            AddItem("WorkingBoots_Green", ExpansionHardlineItemRarity.Uncommon);

            AddItem("HikingBoots_Brown", ExpansionHardlineItemRarity.Rare);
            AddItem("HikingBoots_Black", ExpansionHardlineItemRarity.Rare);

            AddItem("CombatBoots_Beige", ExpansionHardlineItemRarity.Rare);
            AddItem("CombatBoots_Black", ExpansionHardlineItemRarity.Rare);
            AddItem("CombatBoots_Brown", ExpansionHardlineItemRarity.Rare);
            AddItem("CombatBoots_Green", ExpansionHardlineItemRarity.Rare);
            AddItem("CombatBoots_Grey", ExpansionHardlineItemRarity.Rare);

            AddItem("JungleBoots_Beige", ExpansionHardlineItemRarity.Rare);
            AddItem("JungleBoots_Black", ExpansionHardlineItemRarity.Rare);
            AddItem("JungleBoots_Brown", ExpansionHardlineItemRarity.Rare);
            AddItem("JungleBoots_Green", ExpansionHardlineItemRarity.Rare);
            AddItem("JungleBoots_Olive", ExpansionHardlineItemRarity.Rare);

            AddItem("Wellies_Black", ExpansionHardlineItemRarity.Rare);
            AddItem("Wellies_Brown", ExpansionHardlineItemRarity.Rare);
            AddItem("Wellies_Grey", ExpansionHardlineItemRarity.Rare);
            AddItem("Wellies_Green", ExpansionHardlineItemRarity.Rare);

            AddItem("TTSKOBoots", ExpansionHardlineItemRarity.Epic);

            AddItem("MilitaryBoots_Redpunk", ExpansionHardlineItemRarity.Epic);
            AddItem("MilitaryBoots_Bluerock", ExpansionHardlineItemRarity.Epic);
            AddItem("MilitaryBoots_Beige", ExpansionHardlineItemRarity.Epic);
            AddItem("MilitaryBoots_Black", ExpansionHardlineItemRarity.Epic);
            AddItem("MilitaryBoots_Brown", ExpansionHardlineItemRarity.Epic);

            AddItem("NBCBootsGray", ExpansionHardlineItemRarity.Legendary);
            AddItem("NBCBootsYellow", ExpansionHardlineItemRarity.Legendary);

            //! Buttstocks
            AddItem("MP5k_StockBttstck", ExpansionHardlineItemRarity.Uncommon);

            AddItem("Fal_OeBttstck", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Fal_FoldingBttstck", ExpansionHardlineItemRarity.Uncommon);

            AddItem("Saiga_Bttstck", ExpansionHardlineItemRarity.Rare);

            AddItem("AKS74U_Bttstck", ExpansionHardlineItemRarity.Rare);

            AddItem("AK74_WoodBttstck", ExpansionHardlineItemRarity.Rare);

            AddItem("AK_PlasticBttstck", ExpansionHardlineItemRarity.Rare);
            AddItem("AK_WoodBttstck", ExpansionHardlineItemRarity.Rare);
            AddItem("AK_FoldingBttstck", ExpansionHardlineItemRarity.Rare);

            AddItem("M4_OEBttstck", ExpansionHardlineItemRarity.Epic);
            AddItem("M4_MPBttstck", ExpansionHardlineItemRarity.Epic);
            AddItem("M4_CQBBttstck", ExpansionHardlineItemRarity.Epic);

            AddItem("PP19_Bttstck", ExpansionHardlineItemRarity.Epic);

            AddItem("GhillieAtt_Tan", ExpansionHardlineItemRarity.Epic);
            AddItem("GhillieAtt_Woodland", ExpansionHardlineItemRarity.Epic);
            AddItem("GhillieAtt_Mossy", ExpansionHardlineItemRarity.Epic);

            //! Caps
            AddItem("BaseballCap_CMMG_Pink", ExpansionHardlineItemRarity.Common);
            AddItem("BaseballCap_Pink", ExpansionHardlineItemRarity.Uncommon);
            AddItem("BaseballCap_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("BaseballCap_Beige", ExpansionHardlineItemRarity.Common);
            AddItem("BaseballCap_Red", ExpansionHardlineItemRarity.Common);
            AddItem("BaseballCap_CMMG_Black", ExpansionHardlineItemRarity.Common);
            AddItem("BaseballCap_Black", ExpansionHardlineItemRarity.Common);
            AddItem("BaseballCap_Olive", ExpansionHardlineItemRarity.Common);
            AddItem("BaseballCap_Camo", ExpansionHardlineItemRarity.Common);

            AddItem("PrisonerCap", ExpansionHardlineItemRarity.Common);
            AddItem("PilotkaCap", ExpansionHardlineItemRarity.Common);
            AddItem("PoliceCap", ExpansionHardlineItemRarity.Common);

            AddItem("FlatCap_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("FlatCap_Red", ExpansionHardlineItemRarity.Common);
            AddItem("FlatCap_Brown", ExpansionHardlineItemRarity.Common);
            AddItem("FlatCap_Grey", ExpansionHardlineItemRarity.Common);
            AddItem("FlatCap_BrownCheck", ExpansionHardlineItemRarity.Common);
            AddItem("FlatCap_GreyCheck", ExpansionHardlineItemRarity.Common);
            AddItem("FlatCap_Black", ExpansionHardlineItemRarity.Common);
            AddItem("FlatCap_BlackCheck", ExpansionHardlineItemRarity.Common);

            AddItem("ZmijovkaCap_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("ZmijovkaCap_Brown", ExpansionHardlineItemRarity.Common);
            AddItem("ZmijovkaCap_Red", ExpansionHardlineItemRarity.Common);
            AddItem("ZmijovkaCap_Black", ExpansionHardlineItemRarity.Common);
            AddItem("ZmijovkaCap_Green", ExpansionHardlineItemRarity.Common);

            AddItem("RadarCap_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("RadarCap_Brown", ExpansionHardlineItemRarity.Common);
            AddItem("RadarCap_Red", ExpansionHardlineItemRarity.Common);
            AddItem("RadarCap_Black", ExpansionHardlineItemRarity.Common);
            AddItem("RadarCap_Green", ExpansionHardlineItemRarity.Common);

            //! Coats and Jackets
            AddItem("LabCoat", ExpansionHardlineItemRarity.Common);

            AddItem("TrackSuitJacket_Black", ExpansionHardlineItemRarity.Common);
            AddItem("TrackSuitJacket_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("TrackSuitJacket_Green", ExpansionHardlineItemRarity.Common);
            AddItem("TrackSuitJacket_LightBlue", ExpansionHardlineItemRarity.Common);
            AddItem("TrackSuitJacket_Red", ExpansionHardlineItemRarity.Common);

            AddItem("DenimJacket", ExpansionHardlineItemRarity.Common);

            AddItem("WoolCoat_Red", ExpansionHardlineItemRarity.Common);
            AddItem("WoolCoat_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("WoolCoat_Beige", ExpansionHardlineItemRarity.Common);
            AddItem("WoolCoat_RedCheck", ExpansionHardlineItemRarity.Common);
            AddItem("WoolCoat_BlueCheck", ExpansionHardlineItemRarity.Common);
            AddItem("WoolCoat_GreyCheck", ExpansionHardlineItemRarity.Common);
            AddItem("WoolCoat_BrownCheck", ExpansionHardlineItemRarity.Common);
            AddItem("WoolCoat_Black", ExpansionHardlineItemRarity.Common);
            AddItem("WoolCoat_BlackCheck", ExpansionHardlineItemRarity.Common);
            AddItem("WoolCoat_Green", ExpansionHardlineItemRarity.Common);
            AddItem("RidersJacket_Black", ExpansionHardlineItemRarity.Common);

            AddItem("FirefighterJacket_Beige", ExpansionHardlineItemRarity.Common);
            AddItem("FirefighterJacket_Black", ExpansionHardlineItemRarity.Common);

            AddItem("JumpsuitJacket_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("JumpsuitJacket_Gray", ExpansionHardlineItemRarity.Common);
            AddItem("JumpsuitJacket_Green", ExpansionHardlineItemRarity.Common);
            AddItem("JumpsuitJacket_Red", ExpansionHardlineItemRarity.Common);

            AddItem("BomberJacket_Brown", ExpansionHardlineItemRarity.Common);
            AddItem("BomberJacket_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("BomberJacket_Grey", ExpansionHardlineItemRarity.Common);
            AddItem("BomberJacket_Maroon", ExpansionHardlineItemRarity.Common);
            AddItem("BomberJacket_SkyBlue", ExpansionHardlineItemRarity.Common);
            AddItem("BomberJacket_Black", ExpansionHardlineItemRarity.Common);
            AddItem("BomberJacket_Olive", ExpansionHardlineItemRarity.Common);

            AddItem("QuiltedJacket_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("QuiltedJacket_Red", ExpansionHardlineItemRarity.Common);
            AddItem("QuiltedJacket_Grey", ExpansionHardlineItemRarity.Common);
            AddItem("QuiltedJacket_Orange", ExpansionHardlineItemRarity.Common);
            AddItem("QuiltedJacket_Yellow", ExpansionHardlineItemRarity.Common);
            AddItem("QuiltedJacket_Violet", ExpansionHardlineItemRarity.Common);
            AddItem("QuiltedJacket_Black", ExpansionHardlineItemRarity.Common);
            AddItem("QuiltedJacket_Green", ExpansionHardlineItemRarity.Common);

            AddItem("PrisonUniformJacket", ExpansionHardlineItemRarity.Common);

            AddItem("PoliceJacketOrel", ExpansionHardlineItemRarity.Uncommon);
            AddItem("PoliceJacket", ExpansionHardlineItemRarity.Uncommon);

            AddItem("ParamedicJacket_Blue", ExpansionHardlineItemRarity.Uncommon);
            AddItem("ParamedicJacket_Crimson", ExpansionHardlineItemRarity.Uncommon);
            AddItem("ParamedicJacket_Green", ExpansionHardlineItemRarity.Uncommon);

            AddItem("HikingJacket_Black", ExpansionHardlineItemRarity.Uncommon);
            AddItem("HikingJacket_Red", ExpansionHardlineItemRarity.Uncommon);
            AddItem("HikingJacket_Blue", ExpansionHardlineItemRarity.Uncommon);
            AddItem("HikingJacket_Green", ExpansionHardlineItemRarity.Uncommon);

            AddItem("Raincoat_Pink", ExpansionHardlineItemRarity.Rare);
            AddItem("Raincoat_Orange", ExpansionHardlineItemRarity.Rare);
            AddItem("Raincoat_Yellow", ExpansionHardlineItemRarity.Rare);
            AddItem("Raincoat_Red", ExpansionHardlineItemRarity.Rare);
            AddItem("Raincoat_Blue", ExpansionHardlineItemRarity.Rare);
            AddItem("Raincoat_Black", ExpansionHardlineItemRarity.Rare);
            AddItem("Raincoat_Green", ExpansionHardlineItemRarity.Rare);

            AddItem("TTsKOJacket_Camo", ExpansionHardlineItemRarity.Epic);

            AddItem("BDUJacket", ExpansionHardlineItemRarity.Epic);

            AddItem("HuntingJacket_Autumn", ExpansionHardlineItemRarity.Epic);
            AddItem("HuntingJacket_Brown", ExpansionHardlineItemRarity.Epic);
            AddItem("HuntingJacket_Spring", ExpansionHardlineItemRarity.Epic);
            AddItem("HuntingJacket_Summer", ExpansionHardlineItemRarity.Epic);
            AddItem("HuntingJacket_Winter", ExpansionHardlineItemRarity.Epic);

            AddItem("M65Jacket_Black", ExpansionHardlineItemRarity.Epic);
            AddItem("M65Jacket_Khaki", ExpansionHardlineItemRarity.Epic);
            AddItem("M65Jacket_Tan", ExpansionHardlineItemRarity.Epic);
            AddItem("M65Jacket_Olive", ExpansionHardlineItemRarity.Epic);

            AddItem("GorkaEJacket_Summer", ExpansionHardlineItemRarity.Epic);
            AddItem("GorkaEJacket_Flat", ExpansionHardlineItemRarity.Epic);
            AddItem("GorkaEJacket_Autumn", ExpansionHardlineItemRarity.Epic);
            AddItem("GorkaEJacket_PautRev", ExpansionHardlineItemRarity.Epic);

            AddItem("USMCJacket_Desert", ExpansionHardlineItemRarity.Epic);
            AddItem("USMCJacket_Woodland", ExpansionHardlineItemRarity.Epic);

            AddItem("NBCJacketGray", ExpansionHardlineItemRarity.Legendary);
            AddItem("NBCJacketYellow", ExpansionHardlineItemRarity.Legendary);

            //! Containers
            AddItem("SmallProtectorCase", ExpansionHardlineItemRarity.Rare);
            AddItem("AmmoBox", ExpansionHardlineItemRarity.Epic);
            AddItem("BarrelHoles_Blue", ExpansionHardlineItemRarity.Epic);
            AddItem("BarrelHoles_Green", ExpansionHardlineItemRarity.Epic);
            AddItem("BarrelHoles_Red", ExpansionHardlineItemRarity.Epic);
            AddItem("BarrelHoles_Yellow", ExpansionHardlineItemRarity.Epic);
            AddItem("Barrel_Blue", ExpansionHardlineItemRarity.Epic);
            AddItem("Barrel_Green", ExpansionHardlineItemRarity.Epic);
            AddItem("Barrel_Red", ExpansionHardlineItemRarity.Epic);
            AddItem("Barrel_Yellow", ExpansionHardlineItemRarity.Epic);
            AddItem("SeaChest", ExpansionHardlineItemRarity.Legendary);
            AddItem("WoodenCrate", ExpansionHardlineItemRarity.Legendary);

            //! Crossbows
            AddItem("Crossbow_Autumn", ExpansionHardlineItemRarity.Rare);
            AddItem("Crossbow_Black", ExpansionHardlineItemRarity.Rare);
            AddItem("Crossbow_Summer", ExpansionHardlineItemRarity.Rare);
            AddItem("Crossbow_Wood", ExpansionHardlineItemRarity.Rare);

            //! Drinks
            AddItem("SodaCan_Pipsi", ExpansionHardlineItemRarity.Uncommon);
            AddItem("SodaCan_Cola", ExpansionHardlineItemRarity.Rare);
            AddItem("SodaCan_Spite", ExpansionHardlineItemRarity.Rare);
            AddItem("SodaCan_Kvass", ExpansionHardlineItemRarity.Uncommon);
            AddItem("SodaCan_Fronta", ExpansionHardlineItemRarity.Uncommon);
            AddItem("WaterBottle", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Canteen", ExpansionHardlineItemRarity.Rare);
            AddItem("Vodka", ExpansionHardlineItemRarity.Uncommon);

            //! Electronics
            AddItem("PersonalRadio", ExpansionHardlineItemRarity.Rare);
            AddItem("Megaphone", ExpansionHardlineItemRarity.Rare);
            AddItem("ElectronicRepairKit", ExpansionHardlineItemRarity.Rare);
            AddItem("CableReel", ExpansionHardlineItemRarity.Rare);
            AddItem("BatteryCharger", ExpansionHardlineItemRarity.Rare);
            AddItem("BaseRadio", ExpansionHardlineItemRarity.Epic);
            AddItem("Rangefinder", ExpansionHardlineItemRarity.Epic);
            AddItem("NVGoggles", ExpansionHardlineItemRarity.Epic);

            AddItem("AlarmClock_Blue", ExpansionHardlineItemRarity.Uncommon);
            AddItem("KitchenTimer", ExpansionHardlineItemRarity.Uncommon);

            //! Explosive Grenades
            AddItem("RGD5Grenade", ExpansionHardlineItemRarity.Epic);
            AddItem("M67Grenade", ExpansionHardlineItemRarity.Epic);

            AddItem("RemoteDetonator", ExpansionHardlineItemRarity.Epic);
            AddItem("RemoteDetonatorTrigger", ExpansionHardlineItemRarity.Epic);
            AddItem("ImprovisedExplosive", ExpansionHardlineItemRarity.Epic);
            AddItem("Plastic_Explosive", ExpansionHardlineItemRarity.Epic);
            AddItem("Grenade_ChemGas", ExpansionHardlineItemRarity.Epic);

            //! Flashbangs
            AddItem("FlashGrenade", ExpansionHardlineItemRarity.Rare);

            //! Smoke Grenades
            AddItem("M18SmokeGrenade_Red", ExpansionHardlineItemRarity.Rare);
            AddItem("M18SmokeGrenade_Green", ExpansionHardlineItemRarity.Rare);
            AddItem("M18SmokeGrenade_Yellow", ExpansionHardlineItemRarity.Rare);
            AddItem("M18SmokeGrenade_Purple", ExpansionHardlineItemRarity.Rare);
            AddItem("M18SmokeGrenade_White", ExpansionHardlineItemRarity.Rare);
            AddItem("RDG2SmokeGrenade_Black", ExpansionHardlineItemRarity.Rare);
            AddItem("RDG2SmokeGrenade_White", ExpansionHardlineItemRarity.Rare);

            //! Explosive Charges
            AddItem("ClaymoreMine", ExpansionHardlineItemRarity.Epic);
            AddItem("LandMineTrap", ExpansionHardlineItemRarity.Epic);

            //! Eyewear
            AddItem("SportGlasses_Orange", ExpansionHardlineItemRarity.Uncommon);
            AddItem("SportGlasses_Blue", ExpansionHardlineItemRarity.Uncommon);
            AddItem("SportGlasses_Black", ExpansionHardlineItemRarity.Uncommon);
            AddItem("SportGlasses_Green", ExpansionHardlineItemRarity.Uncommon);
            AddItem("ThinFramesGlasses", ExpansionHardlineItemRarity.Uncommon);
            AddItem("ThickFramesGlasses", ExpansionHardlineItemRarity.Uncommon);
            AddItem("DesignerGlasses", ExpansionHardlineItemRarity.Uncommon);
            AddItem("AviatorGlasses", ExpansionHardlineItemRarity.Uncommon);
            AddItem("TacticalGoggles", ExpansionHardlineItemRarity.Rare);
            AddItem("NVGHeadstrap", ExpansionHardlineItemRarity.Epic);
            AddItem("EyePatch_Improvised", ExpansionHardlineItemRarity.Common);
            AddItem("EyeMask_Black", ExpansionHardlineItemRarity.Uncommon);
            AddItem("EyeMask_Blue", ExpansionHardlineItemRarity.Uncommon);
            AddItem("EyeMask_Christmas", ExpansionHardlineItemRarity.Uncommon);
            AddItem("EyeMask_Dead", ExpansionHardlineItemRarity.Uncommon);
            AddItem("EyeMask_NewYears", ExpansionHardlineItemRarity.Uncommon);
            AddItem("EyeMask_Red", ExpansionHardlineItemRarity.Uncommon);
            AddItem("EyeMask_Valentines", ExpansionHardlineItemRarity.Uncommon);
            AddItem("EyeMask_Yellow", ExpansionHardlineItemRarity.Uncommon);

            //! Fish
            AddItem("CarpFilletMeat", ExpansionHardlineItemRarity.Uncommon);
            AddItem("MackerelFilletMeat", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Carp", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Sardines", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Mackerel", ExpansionHardlineItemRarity.Uncommon);

            //! Fishing
            AddItem("Worm", ExpansionHardlineItemRarity.Uncommon);
            AddItem("BoneHook", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Hook", ExpansionHardlineItemRarity.Uncommon);
            AddItem("FishingRod", ExpansionHardlineItemRarity.Uncommon);
            AddItem("ImprovisedFishingRod", ExpansionHardlineItemRarity.Common);

            //! Flags
            AddItem("Flag_Chernarus", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_Chedaki", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_NAPA", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_CDF", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_Livonia", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_Altis", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_SSahrani", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_NSahrani", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_DayZ", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_LivoniaArmy", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_White", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_Bohemia", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_APA", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_UEC", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_Pirates", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_Cannibals", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_Bear", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_Wolf", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_BabyDeer", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_Rooster", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_LivoniaPolice", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_CMC", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_TEC", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_CHEL", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_Zenit", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_HunterZ", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_BrainZ", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_Refuge", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_RSTA", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_Snake", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_Crook", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_Rex", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Flag_Zagorky", ExpansionHardlineItemRarity.Uncommon);

            //! Food
            AddItem("Zagorky", ExpansionHardlineItemRarity.Uncommon);
            AddItem("ZagorkyChocolate", ExpansionHardlineItemRarity.Uncommon);
            AddItem("ZagorkyPeanuts", ExpansionHardlineItemRarity.Uncommon);

            AddItem("PowderedMilk", ExpansionHardlineItemRarity.Uncommon);
            AddItem("BoxCerealCrunchin", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Rice", ExpansionHardlineItemRarity.Uncommon);

            AddItem("Marmalade", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Honey", ExpansionHardlineItemRarity.Uncommon);

            AddItem("SaltySticks", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Crackers", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Chips", ExpansionHardlineItemRarity.Uncommon);

            AddItem("Pajka", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Pate", ExpansionHardlineItemRarity.Uncommon);
            AddItem("BrisketSpread", ExpansionHardlineItemRarity.Uncommon);

            AddItem("SardinesCan", ExpansionHardlineItemRarity.Uncommon);
            AddItem("TunaCan", ExpansionHardlineItemRarity.Uncommon);
            AddItem("DogFoodCan", ExpansionHardlineItemRarity.Uncommon);
            AddItem("CatFoodCan", ExpansionHardlineItemRarity.Uncommon);
            AddItem("PorkCan", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Lunchmeat", ExpansionHardlineItemRarity.Uncommon);

            AddItem("UnknownFoodCan", ExpansionHardlineItemRarity.Uncommon);
            AddItem("PeachesCan", ExpansionHardlineItemRarity.Uncommon);
            AddItem("SpaghettiCan", ExpansionHardlineItemRarity.Uncommon);
            AddItem("BakedBeansCan", ExpansionHardlineItemRarity.Uncommon);
            AddItem("TacticalBaconCan", ExpansionHardlineItemRarity.Uncommon);

            AddItem("BakedBeansCan_Opened", ExpansionHardlineItemRarity.Uncommon);
            AddItem("PeachesCan_Opened", ExpansionHardlineItemRarity.Uncommon);
            AddItem("SpaghettiCan_Opened", ExpansionHardlineItemRarity.Uncommon);
            AddItem("SardinesCan_Opened", ExpansionHardlineItemRarity.Uncommon);
            AddItem("TunaCan_Opened", ExpansionHardlineItemRarity.Uncommon);
            AddItem("UnknownFoodCan_Opened", ExpansionHardlineItemRarity.Uncommon);
            AddItem("DogFoodCan_Opened", ExpansionHardlineItemRarity.Uncommon);
            AddItem("CatFoodCan_Opened", ExpansionHardlineItemRarity.Uncommon);
            AddItem("PorkCan_Opened", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Lunchmeat_Opened", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Pajka_Opened", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Pate_Opened", ExpansionHardlineItemRarity.Uncommon);

            //! Gardening
            AddItem("GardenLime", ExpansionHardlineItemRarity.Uncommon);
            AddItem("PepperSeeds", ExpansionHardlineItemRarity.Uncommon);
            AddItem("TomatoSeeds", ExpansionHardlineItemRarity.Uncommon);
            AddItem("ZucchiniSeeds", ExpansionHardlineItemRarity.Uncommon);
            AddItem("PumpkinSeeds", ExpansionHardlineItemRarity.Uncommon);

            //! Ghillies
            AddItem("GhillieHood_Tan", ExpansionHardlineItemRarity.Legendary);
            AddItem("GhillieHood_Woodland", ExpansionHardlineItemRarity.Legendary);
            AddItem("GhillieHood_Mossy", ExpansionHardlineItemRarity.Legendary);

            AddItem("GhillieBushrag_Tan", ExpansionHardlineItemRarity.Legendary);
            AddItem("GhillieBushrag_Woodland", ExpansionHardlineItemRarity.Legendary);
            AddItem("GhillieBushrag_Mossy", ExpansionHardlineItemRarity.Legendary);

            AddItem("GhillieTop_Tan", ExpansionHardlineItemRarity.Legendary);
            AddItem("GhillieTop_Woodland", ExpansionHardlineItemRarity.Legendary);
            AddItem("GhillieTop_Mossy", ExpansionHardlineItemRarity.Legendary);

            AddItem("GhillieSuit_Tan", ExpansionHardlineItemRarity.Legendary);
            AddItem("GhillieSuit_Woodland", ExpansionHardlineItemRarity.Legendary);
            AddItem("GhillieSuit_Mossy", ExpansionHardlineItemRarity.Legendary);

            //! Gloves
            AddItem("SurgicalGloves_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("SurgicalGloves_LightBlue", ExpansionHardlineItemRarity.Common);
            AddItem("SurgicalGloves_Green", ExpansionHardlineItemRarity.Common);
            AddItem("SurgicalGloves_White", ExpansionHardlineItemRarity.Common);

            AddItem("WorkingGloves_Yellow", ExpansionHardlineItemRarity.Uncommon);
            AddItem("WorkingGloves_Black", ExpansionHardlineItemRarity.Uncommon);
            AddItem("WorkingGloves_Beige", ExpansionHardlineItemRarity.Uncommon);
            AddItem("WorkingGloves_Brown", ExpansionHardlineItemRarity.Uncommon);
            AddItem("TacticalGloves_Black", ExpansionHardlineItemRarity.Rare);
            AddItem("TacticalGloves_Beige", ExpansionHardlineItemRarity.Rare);
            AddItem("TacticalGloves_Green", ExpansionHardlineItemRarity.Rare);

            AddItem("OMNOGloves_Gray", ExpansionHardlineItemRarity.Epic);
            AddItem("OMNOGloves_Brown", ExpansionHardlineItemRarity.Epic);

            AddItem("NBCGlovesGray", ExpansionHardlineItemRarity.Legendary);
            AddItem("NBCGlovesYellow", ExpansionHardlineItemRarity.Legendary);

            //! Handguards
            AddItem("MP5_PlasticHndgrd", ExpansionHardlineItemRarity.Uncommon);
            AddItem("MP5_RailHndgrd", ExpansionHardlineItemRarity.Uncommon);
            AddItem("AK74_Hndgrd", ExpansionHardlineItemRarity.Rare);

            AddItem("AK_WoodHndgrd", ExpansionHardlineItemRarity.Epic);
            AddItem("AK_RailHndgrd", ExpansionHardlineItemRarity.Epic);
            AddItem("AK_PlasticHndgrd", ExpansionHardlineItemRarity.Epic);

            AddItem("M4_PlasticHndgrd", ExpansionHardlineItemRarity.Rare);
            AddItem("M4_RISHndgrd", ExpansionHardlineItemRarity.Epic);
            AddItem("M4_MPHndgrd", ExpansionHardlineItemRarity.Epic);

            //! Hats
            AddItem("MedicalScrubsHat_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("MedicalScrubsHat_White", ExpansionHardlineItemRarity.Common);
            AddItem("MedicalScrubsHat_Green", ExpansionHardlineItemRarity.Common);

            AddItem("MilitaryBeret_ChDKZ", ExpansionHardlineItemRarity.Uncommon);
            AddItem("MilitaryBeret_Red", ExpansionHardlineItemRarity.Uncommon);
            AddItem("MilitaryBeret_UN", ExpansionHardlineItemRarity.Uncommon);
            AddItem("MilitaryBeret_CDF", ExpansionHardlineItemRarity.Uncommon);
            AddItem("MilitaryBeret_NZ", ExpansionHardlineItemRarity.Uncommon);

            AddItem("BeanieHat_Pink", ExpansionHardlineItemRarity.Uncommon);
            AddItem("BeanieHat_Beige", ExpansionHardlineItemRarity.Uncommon);
            AddItem("BeanieHat_Blue", ExpansionHardlineItemRarity.Uncommon);
            AddItem("BeanieHat_Brown", ExpansionHardlineItemRarity.Uncommon);
            AddItem("BeanieHat_Grey", ExpansionHardlineItemRarity.Uncommon);
            AddItem("BeanieHat_Red", ExpansionHardlineItemRarity.Uncommon);
            AddItem("BeanieHat_Black", ExpansionHardlineItemRarity.Uncommon);
            AddItem("BeanieHat_Green", ExpansionHardlineItemRarity.Uncommon);

            AddItem("Ushanka_Black", ExpansionHardlineItemRarity.Epic);
            AddItem("Ushanka_Blue", ExpansionHardlineItemRarity.Epic);
            AddItem("Ushanka_Green", ExpansionHardlineItemRarity.Epic);

            AddItem("BoonieHat_Blue", ExpansionHardlineItemRarity.Rare);
            AddItem("BoonieHat_NavyBlue", ExpansionHardlineItemRarity.Rare);
            AddItem("BoonieHat_Orange", ExpansionHardlineItemRarity.Rare);
            AddItem("BoonieHat_Red", ExpansionHardlineItemRarity.Rare);
            AddItem("BoonieHat_Black", ExpansionHardlineItemRarity.Rare);
            AddItem("BoonieHat_Tan", ExpansionHardlineItemRarity.Rare);
            AddItem("BoonieHat_Olive", ExpansionHardlineItemRarity.Rare);
            AddItem("BoonieHat_DPM", ExpansionHardlineItemRarity.Rare);
            AddItem("BoonieHat_Dubok", ExpansionHardlineItemRarity.Rare);
            AddItem("BoonieHat_Flecktran", ExpansionHardlineItemRarity.Rare);

            AddItem("OfficerHat", ExpansionHardlineItemRarity.Epic);

            AddItem("NBCHoodGray", ExpansionHardlineItemRarity.Legendary);
            AddItem("NBCHoodYellow", ExpansionHardlineItemRarity.Legendary);

            //! Helmets
            AddItem("ConstructionHelmet_Blue", ExpansionHardlineItemRarity.Rare);
            AddItem("ConstructionHelmet_Orange", ExpansionHardlineItemRarity.Rare);
            AddItem("ConstructionHelmet_Red", ExpansionHardlineItemRarity.Rare);
            AddItem("ConstructionHelmet_White", ExpansionHardlineItemRarity.Rare);
            AddItem("ConstructionHelmet_Yellow", ExpansionHardlineItemRarity.Rare);
            AddItem("ConstructionHelmet_Lime", ExpansionHardlineItemRarity.Rare);

            AddItem("SkateHelmet_Blue", ExpansionHardlineItemRarity.Rare);
            AddItem("SkateHelmet_Gray", ExpansionHardlineItemRarity.Rare);
            AddItem("SkateHelmet_Red", ExpansionHardlineItemRarity.Rare);
            AddItem("SkateHelmet_Black", ExpansionHardlineItemRarity.Rare);
            AddItem("SkateHelmet_Green", ExpansionHardlineItemRarity.Rare);

            AddItem("HockeyHelmet_Blue", ExpansionHardlineItemRarity.Rare);
            AddItem("HockeyHelmet_Red", ExpansionHardlineItemRarity.Rare);
            AddItem("HockeyHelmet_White", ExpansionHardlineItemRarity.Rare);
            AddItem("HockeyHelmet_Black", ExpansionHardlineItemRarity.Rare);

            AddItem("DirtBikeHelmet_Mouthguard", ExpansionHardlineItemRarity.Rare);
            AddItem("DirtBikeHelmet_Visor", ExpansionHardlineItemRarity.Rare);

            AddItem("DirtBikeHelmet_Chernarus", ExpansionHardlineItemRarity.Rare);
            AddItem("DirtBikeHelmet_Police", ExpansionHardlineItemRarity.Rare);
            AddItem("DirtBikeHelmet_Red", ExpansionHardlineItemRarity.Rare);
            AddItem("DirtBikeHelmet_Green", ExpansionHardlineItemRarity.Rare);
            AddItem("DirtBikeHelmet_Blue", ExpansionHardlineItemRarity.Rare);
            AddItem("DirtBikeHelmet_Black", ExpansionHardlineItemRarity.Rare);
            AddItem("DirtBikeHelmet_Khaki", ExpansionHardlineItemRarity.Rare);

            AddItem("MotoHelmet_Lime", ExpansionHardlineItemRarity.Rare);
            AddItem("MotoHelmet_Blue", ExpansionHardlineItemRarity.Rare);
            AddItem("MotoHelmet_Red", ExpansionHardlineItemRarity.Rare);
            AddItem("MotoHelmet_White", ExpansionHardlineItemRarity.Rare);
            AddItem("MotoHelmet_Grey", ExpansionHardlineItemRarity.Rare);
            AddItem("MotoHelmet_Black", ExpansionHardlineItemRarity.Rare);
            AddItem("MotoHelmet_Green", ExpansionHardlineItemRarity.Rare);

            AddItem("DarkMotoHelmet_Grey", ExpansionHardlineItemRarity.Epic);
            AddItem("DarkMotoHelmet_Lime", ExpansionHardlineItemRarity.Epic);
            AddItem("DarkMotoHelmet_Blue", ExpansionHardlineItemRarity.Epic);
            AddItem("DarkMotoHelmet_Red", ExpansionHardlineItemRarity.Epic);
            AddItem("DarkMotoHelmet_White", ExpansionHardlineItemRarity.Epic);
            AddItem("DarkMotoHelmet_Black", ExpansionHardlineItemRarity.Epic);
            AddItem("DarkMotoHelmet_Green", ExpansionHardlineItemRarity.Epic);

            AddItem("TankerHelmet", ExpansionHardlineItemRarity.Epic);

            AddItem("GreatHelm", ExpansionHardlineItemRarity.Epic);

            AddItem("ZSh3PilotHelmet", ExpansionHardlineItemRarity.Epic);
            AddItem("ZSh3PilotHelmet_Green", ExpansionHardlineItemRarity.Epic);
            AddItem("ZSh3PilotHelmet_Black", ExpansionHardlineItemRarity.Epic);

            AddItem("FirefightersHelmet_Red", ExpansionHardlineItemRarity.Epic);
            AddItem("FirefightersHelmet_White", ExpansionHardlineItemRarity.Epic);
            AddItem("FirefightersHelmet_Yellow", ExpansionHardlineItemRarity.Epic);

            AddItem("GorkaHelmetVisor", ExpansionHardlineItemRarity.Epic);
            AddItem("GorkaHelmet", ExpansionHardlineItemRarity.Epic);
            AddItem("GorkaHelmet_Black", ExpansionHardlineItemRarity.Epic);

            AddItem("Ssh68Helmet", ExpansionHardlineItemRarity.Epic);

            AddItem("BallisticHelmet_UN", ExpansionHardlineItemRarity.Epic);
            AddItem("BallisticHelmet_Black", ExpansionHardlineItemRarity.Epic);
            AddItem("BallisticHelmet_Green", ExpansionHardlineItemRarity.Epic);

            //! Holsters/Pouches
            AddItem("ChestHolster", ExpansionHardlineItemRarity.Rare);
            AddItem("PlateCarrierHolster", ExpansionHardlineItemRarity.Rare);
            AddItem("PlateCarrierPouches", ExpansionHardlineItemRarity.Rare);
            AddItem("NylonKnifeSheath", ExpansionHardlineItemRarity.Rare);

            //! Kits
            AddItem("SewingKit", ExpansionHardlineItemRarity.Rare);
            AddItem("LeatherSewingKit", ExpansionHardlineItemRarity.Rare);
            AddItem("WeaponCleaningKit", ExpansionHardlineItemRarity.Rare);

            //! Knifes
            AddItem("KitchenKnife", ExpansionHardlineItemRarity.Rare);
            AddItem("SteakKnife", ExpansionHardlineItemRarity.Common);
            AddItem("HuntingKnife", ExpansionHardlineItemRarity.Rare);
            AddItem("CombatKnife", ExpansionHardlineItemRarity.Epic);
            AddItem("KukriKnife", ExpansionHardlineItemRarity.Rare);
            AddItem("FangeKnife", ExpansionHardlineItemRarity.Common);
            AddItem("Machete", ExpansionHardlineItemRarity.Rare);
            AddItem("CrudeMachete", ExpansionHardlineItemRarity.Rare);
            AddItem("OrientalMachete", ExpansionHardlineItemRarity.Rare);
            AddItem("Cleaver", ExpansionHardlineItemRarity.Rare);

            //! Launchers
            AddItem("Flaregun", ExpansionHardlineItemRarity.Rare);
            AddItem("M79", ExpansionHardlineItemRarity.Legendary);

            //! Lights
            AddItem("Chemlight_Blue", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Chemlight_Green", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Chemlight_Red", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Chemlight_White", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Chemlight_Yellow", ExpansionHardlineItemRarity.Uncommon);

            AddItem("Roadflare", ExpansionHardlineItemRarity.Rare);
            AddItem("Matchbox", ExpansionHardlineItemRarity.Rare);
            AddItem("PetrolLighter", ExpansionHardlineItemRarity.Rare);
            AddItem("Flashlight", ExpansionHardlineItemRarity.Rare);
            AddItem("XmasLights", ExpansionHardlineItemRarity.Rare);
            AddItem("PortableGasLamp", ExpansionHardlineItemRarity.Rare);
            AddItem("PortableGasStove", ExpansionHardlineItemRarity.Rare);
            AddItem("Headtorch_Black", ExpansionHardlineItemRarity.Rare);
            AddItem("Headtorch_Grey", ExpansionHardlineItemRarity.Rare);
            AddItem("Spotlight", ExpansionHardlineItemRarity.Rare);

            AddItem("UniversalLight", ExpansionHardlineItemRarity.Rare);
            AddItem("TLRLight", ExpansionHardlineItemRarity.Rare);

            //! Liquids
            AddItem("EngineOil", ExpansionHardlineItemRarity.Epic);
            AddItem("CanisterGasoline", ExpansionHardlineItemRarity.Epic);

            //! Locks
            AddItem("CombinationLock", ExpansionHardlineItemRarity.Rare);
            AddItem("CombinationLock4", ExpansionHardlineItemRarity.Epic);

            //! Magazines
            AddItem("Mag_IJ70_8Rnd", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Mag_CZ75_15Rnd", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Mag_Glock_15Rnd", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Mag_MKII_10Rnd", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Mag_1911_7Rnd", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Mag_FNX45_15Rnd", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Mag_Deagle_9Rnd", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Mag_CZ527_5rnd", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Mag_CZ61_20Rnd", ExpansionHardlineItemRarity.Rare);
            AddItem("Mag_PP19_64Rnd", ExpansionHardlineItemRarity.Rare);
            AddItem("Mag_UMP_25Rnd", ExpansionHardlineItemRarity.Rare);
            AddItem("Mag_MP5_15Rnd", ExpansionHardlineItemRarity.Rare);
            AddItem("Mag_MP5_30Rnd", ExpansionHardlineItemRarity.Epic);
            AddItem("Mag_FAL_20Rnd", ExpansionHardlineItemRarity.Rare);
            AddItem("Mag_Saiga_5Rnd", ExpansionHardlineItemRarity.Rare);
            AddItem("Mag_Saiga_8Rnd", ExpansionHardlineItemRarity.Epic);
            AddItem("Mag_Saiga_Drum20Rnd", ExpansionHardlineItemRarity.Epic);
            AddItem("Mag_AKM_30Rnd", ExpansionHardlineItemRarity.Epic);
            AddItem("Mag_AKM_Palm30Rnd", ExpansionHardlineItemRarity.Epic);
            AddItem("Mag_AKM_Drum75Rnd", ExpansionHardlineItemRarity.Epic);
            AddItem("Mag_AK101_30Rnd", ExpansionHardlineItemRarity.Epic);
            AddItem("Mag_AK74_30Rnd", ExpansionHardlineItemRarity.Epic);
            AddItem("Mag_AK74_45Rnd", ExpansionHardlineItemRarity.Epic);
            AddItem("Mag_STANAG_30Rnd", ExpansionHardlineItemRarity.Epic);
            AddItem("Mag_STANAGCoupled_30Rnd", ExpansionHardlineItemRarity.Epic);
            AddItem("Mag_STANAG_60Rnd", ExpansionHardlineItemRarity.Epic);
            AddItem("Mag_CMAG_10Rnd", ExpansionHardlineItemRarity.Epic);
            AddItem("Mag_CMAG_20Rnd", ExpansionHardlineItemRarity.Epic);
            AddItem("Mag_CMAG_30Rnd", ExpansionHardlineItemRarity.Epic);
            AddItem("Mag_CMAG_40Rnd", ExpansionHardlineItemRarity.Epic);
            AddItem("Mag_VSS_10Rnd", ExpansionHardlineItemRarity.Epic);
            AddItem("Mag_VAL_20Rnd", ExpansionHardlineItemRarity.Epic);
            AddItem("Mag_Ruger1022_15Rnd", ExpansionHardlineItemRarity.Epic);
            AddItem("Mag_Ruger1022_30Rnd", ExpansionHardlineItemRarity.Epic);
            AddItem("Mag_SVD_10Rnd", ExpansionHardlineItemRarity.Epic);
            AddItem("Mag_Scout_5Rnd", ExpansionHardlineItemRarity.Epic);
            AddItem("Mag_FAMAS_25Rnd", ExpansionHardlineItemRarity.Epic);
            AddItem("Mag_Aug_30Rnd", ExpansionHardlineItemRarity.Epic);
            AddItem("Mag_P1_8Rnd", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Mag_CZ550_4rnd", ExpansionHardlineItemRarity.Rare);
            AddItem("Mag_CZ550_10rnd", ExpansionHardlineItemRarity.Epic);

            //! Masks
            AddItem("SurgicalMask", ExpansionHardlineItemRarity.Common);
            AddItem("NioshFaceMask", ExpansionHardlineItemRarity.Uncommon);
            AddItem("HockeyMask", ExpansionHardlineItemRarity.Uncommon);
            AddItem("BalaclavaMask_Beige", ExpansionHardlineItemRarity.Rare);
            AddItem("BalaclavaMask_Blue", ExpansionHardlineItemRarity.Rare);
            AddItem("BalaclavaMask_Pink", ExpansionHardlineItemRarity.Rare);
            AddItem("BalaclavaMask_White", ExpansionHardlineItemRarity.Rare);
            AddItem("BalaclavaMask_Blackskull", ExpansionHardlineItemRarity.Rare);
            AddItem("BalaclavaMask_Black", ExpansionHardlineItemRarity.Rare);
            AddItem("BalaclavaMask_Green", ExpansionHardlineItemRarity.Rare);
            AddItem("Balaclava3Holes_Beige", ExpansionHardlineItemRarity.Rare);
            AddItem("Balaclava3Holes_Blue", ExpansionHardlineItemRarity.Rare);
            AddItem("Balaclava3Holes_Black", ExpansionHardlineItemRarity.Rare);
            AddItem("Balaclava3Holes_Green", ExpansionHardlineItemRarity.Rare);
            AddItem("WeldingMask", ExpansionHardlineItemRarity.Rare);
            AddItem("GasMask", ExpansionHardlineItemRarity.Epic);
            AddItem("GP5GasMask", ExpansionHardlineItemRarity.Epic);
            AddItem("AirborneMask", ExpansionHardlineItemRarity.Epic);
            AddItem("MimeMask_Female", ExpansionHardlineItemRarity.Uncommon);
            AddItem("MimeMask_Male", ExpansionHardlineItemRarity.Uncommon);

            //! Meat
            AddItem("BearSteakMeat", ExpansionHardlineItemRarity.Uncommon);
            AddItem("GoatSteakMeat", ExpansionHardlineItemRarity.Uncommon);
            AddItem("BoarSteakMeat", ExpansionHardlineItemRarity.Uncommon);
            AddItem("PigSteakMeat", ExpansionHardlineItemRarity.Uncommon);
            AddItem("DeerSteakMeat", ExpansionHardlineItemRarity.Uncommon);
            AddItem("WolfSteakMeat", ExpansionHardlineItemRarity.Uncommon);
            AddItem("CowSteakMeat", ExpansionHardlineItemRarity.Uncommon);
            AddItem("SheepSteakMeat", ExpansionHardlineItemRarity.Uncommon);
            AddItem("ChickenBreastMeat", ExpansionHardlineItemRarity.Uncommon);

            //! Medical
            AddItem("CharcoalTablets", ExpansionHardlineItemRarity.Uncommon);
            AddItem("BandageDressing", ExpansionHardlineItemRarity.Uncommon);
            AddItem("DisinfectantAlcohol", ExpansionHardlineItemRarity.Rare);
            AddItem("PurificationTablets", ExpansionHardlineItemRarity.Rare);
            AddItem("BloodTestKit", ExpansionHardlineItemRarity.Rare);
            AddItem("Thermometer", ExpansionHardlineItemRarity.Uncommon);
            AddItem("VitaminBottle", ExpansionHardlineItemRarity.Rare);
            AddItem("DisinfectantSpray", ExpansionHardlineItemRarity.Rare);
            AddItem("TetracyclineAntibiotics", ExpansionHardlineItemRarity.Rare);
            AddItem("PainkillerTablets", ExpansionHardlineItemRarity.Rare);
            AddItem("StartKitIV", ExpansionHardlineItemRarity.Rare);
            AddItem("Heatpack", ExpansionHardlineItemRarity.Rare);
            AddItem("SalineBag", ExpansionHardlineItemRarity.Epic);
            AddItem("BloodBagEmpty", ExpansionHardlineItemRarity.Epic);
            AddItem("FirstAidKit", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Epinephrine", ExpansionHardlineItemRarity.Epic);
            AddItem("Morphine", ExpansionHardlineItemRarity.Epic);
            AddItem("IodineTincture", ExpansionHardlineItemRarity.Epic);
            AddItem("AntiChemInjector", ExpansionHardlineItemRarity.Epic);

            //! Melee
            AddItem("BrassKnuckles_Dull", ExpansionHardlineItemRarity.Uncommon);
            AddItem("BrassKnuckles_Shiny", ExpansionHardlineItemRarity.Uncommon);
            AddItem("StunBaton", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Broom", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Broom_Birch", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Pipe", ExpansionHardlineItemRarity.Uncommon);
            AddItem("CattleProd", ExpansionHardlineItemRarity.Uncommon);
            AddItem("BaseballBat", ExpansionHardlineItemRarity.Uncommon);
            AddItem("NailedBaseballBat", ExpansionHardlineItemRarity.Uncommon);
            AddItem("BarbedBaseballBat", ExpansionHardlineItemRarity.Uncommon);

            //! Compensators
            AddItem("Mosin_Compensator", ExpansionHardlineItemRarity.Rare);
            AddItem("MP5_Compensator", ExpansionHardlineItemRarity.Rare);

            //! Supressors
            AddItem("M4_Suppressor", ExpansionHardlineItemRarity.Epic);
            AddItem("AK_Suppressor", ExpansionHardlineItemRarity.Epic);
            AddItem("PistolSuppressor", ExpansionHardlineItemRarity.Rare);

            //! Navigation
            AddItem("Compass", ExpansionHardlineItemRarity.Uncommon);
            AddItem("OrienteeringCompass", ExpansionHardlineItemRarity.Uncommon);
            AddItem("ChernarusMap", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Binoculars", ExpansionHardlineItemRarity.Rare);
            AddItem("GPSReceiver", ExpansionHardlineItemRarity.Rare);

            //! Optics
            AddItem("PistolOptic", ExpansionHardlineItemRarity.Rare);
            AddItem("ReflexOptic", ExpansionHardlineItemRarity.Rare);
            AddItem("M4_CarryHandleOptic", ExpansionHardlineItemRarity.Rare);
            AddItem("BUISOptic", ExpansionHardlineItemRarity.Rare);
            AddItem("FNP45_MRDSOptic", ExpansionHardlineItemRarity.Rare);
            AddItem("ACOGOptic", ExpansionHardlineItemRarity.Rare);
            AddItem("ACOGOptic_6x", ExpansionHardlineItemRarity.Rare);
            AddItem("M68Optic", ExpansionHardlineItemRarity.Rare);
            AddItem("M4_T3NRDSOptic", ExpansionHardlineItemRarity.Rare);
            AddItem("KobraOptic", ExpansionHardlineItemRarity.Rare);
            AddItem("KashtanOptic", ExpansionHardlineItemRarity.Rare);
            AddItem("PUScopeOptic", ExpansionHardlineItemRarity.Rare);
            AddItem("HuntingOptic", ExpansionHardlineItemRarity.Rare);
            AddItem("PSO1Optic", ExpansionHardlineItemRarity.Rare);
            AddItem("PSO11Optic", ExpansionHardlineItemRarity.Rare);
            AddItem("KazuarOptic", ExpansionHardlineItemRarity.Rare);
            AddItem("StarlightOptic", ExpansionHardlineItemRarity.Rare);

            //! Pants
            AddItem("MedicalScrubsPants_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("MedicalScrubsPants_Green", ExpansionHardlineItemRarity.Common);
            AddItem("MedicalScrubsPants_White", ExpansionHardlineItemRarity.Common);

            AddItem("TrackSuitPants_Black", ExpansionHardlineItemRarity.Common);
            AddItem("TrackSuitPants_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("TrackSuitPants_Green", ExpansionHardlineItemRarity.Common);
            AddItem("TrackSuitPants_Red", ExpansionHardlineItemRarity.Common);
            AddItem("TrackSuitPants_LightBlue", ExpansionHardlineItemRarity.Common);

            AddItem("PrisonUniformPants", ExpansionHardlineItemRarity.Common);

            AddItem("Breeches_Pink", ExpansionHardlineItemRarity.Common);
            AddItem("Breeches_Red", ExpansionHardlineItemRarity.Common);
            AddItem("Breeches_White", ExpansionHardlineItemRarity.Common);
            AddItem("Breeches_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("Breeches_Beetcheck", ExpansionHardlineItemRarity.Common);
            AddItem("Breeches_Beige", ExpansionHardlineItemRarity.Common);
            AddItem("Breeches_Browncheck", ExpansionHardlineItemRarity.Common);
            AddItem("Breeches_Black", ExpansionHardlineItemRarity.Common);
            AddItem("Breeches_Blackcheck", ExpansionHardlineItemRarity.Common);
            AddItem("Breeches_Green", ExpansionHardlineItemRarity.Common);

            AddItem("SlacksPants_Beige", ExpansionHardlineItemRarity.Common);
            AddItem("SlacksPants_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("SlacksPants_DarkGrey", ExpansionHardlineItemRarity.Common);
            AddItem("SlacksPants_LightGrey", ExpansionHardlineItemRarity.Common);
            AddItem("SlacksPants_White", ExpansionHardlineItemRarity.Common);
            AddItem("SlacksPants_Brown", ExpansionHardlineItemRarity.Common);
            AddItem("SlacksPants_Black", ExpansionHardlineItemRarity.Common);
            AddItem("SlacksPants_Khaki", ExpansionHardlineItemRarity.Common);

            AddItem("CanvasPantsMidi_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("CanvasPantsMidi_Grey", ExpansionHardlineItemRarity.Common);
            AddItem("CanvasPantsMidi_Red", ExpansionHardlineItemRarity.Common);
            AddItem("CanvasPantsMidi_Violet", ExpansionHardlineItemRarity.Common);
            AddItem("CanvasPantsMidi_Beige", ExpansionHardlineItemRarity.Common);

            AddItem("CanvasPants_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("CanvasPants_Grey", ExpansionHardlineItemRarity.Common);
            AddItem("CanvasPants_Red", ExpansionHardlineItemRarity.Common);
            AddItem("CanvasPants_Violet", ExpansionHardlineItemRarity.Common);
            AddItem("CanvasPants_Beige", ExpansionHardlineItemRarity.Common);

            AddItem("JumpsuitPants_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("JumpsuitPants_Green", ExpansionHardlineItemRarity.Common);
            AddItem("JumpsuitPants_Grey", ExpansionHardlineItemRarity.Common);
            AddItem("JumpsuitPants_Red", ExpansionHardlineItemRarity.Common);

            AddItem("PolicePants", ExpansionHardlineItemRarity.Uncommon);

            AddItem("ParamedicPants_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("ParamedicPants_Crimson", ExpansionHardlineItemRarity.Common);
            AddItem("ParamedicPants_Green", ExpansionHardlineItemRarity.Common);

            AddItem("FirefightersPants_Beige", ExpansionHardlineItemRarity.Rare);
            AddItem("FirefightersPants_Black", ExpansionHardlineItemRarity.Rare);

            AddItem("CargoPants_Beige", ExpansionHardlineItemRarity.Rare);
            AddItem("CargoPants_Black", ExpansionHardlineItemRarity.Rare);
            AddItem("CargoPants_Blue", ExpansionHardlineItemRarity.Rare);
            AddItem("CargoPants_Green", ExpansionHardlineItemRarity.Rare);
            AddItem("CargoPants_Grey", ExpansionHardlineItemRarity.Rare);

            AddItem("ShortJeans_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("ShortJeans_Brown", ExpansionHardlineItemRarity.Common);
            AddItem("ShortJeans_Darkblue", ExpansionHardlineItemRarity.Common);
            AddItem("ShortJeans_Red", ExpansionHardlineItemRarity.Common);
            AddItem("ShortJeans_Black", ExpansionHardlineItemRarity.Common);
            AddItem("ShortJeans_Green", ExpansionHardlineItemRarity.Common);

            AddItem("Jeans_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("Jeans_Brown", ExpansionHardlineItemRarity.Common);
            AddItem("Jeans_Grey", ExpansionHardlineItemRarity.Common);
            AddItem("Jeans_BlueDark", ExpansionHardlineItemRarity.Common);
            AddItem("Jeans_Green", ExpansionHardlineItemRarity.Common);
            AddItem("Jeans_Black", ExpansionHardlineItemRarity.Common);

            AddItem("TTSKOPants", ExpansionHardlineItemRarity.Rare);

            AddItem("BDUPants", ExpansionHardlineItemRarity.Rare);

            AddItem("USMCPants_Desert", ExpansionHardlineItemRarity.Rare);
            AddItem("USMCPants_Woodland", ExpansionHardlineItemRarity.Rare);

            AddItem("PolicePantsOrel", ExpansionHardlineItemRarity.Uncommon);

            AddItem("HunterPants_Winter", ExpansionHardlineItemRarity.Rare);
            AddItem("HunterPants_Autumn", ExpansionHardlineItemRarity.Rare);
            AddItem("HunterPants_Brown", ExpansionHardlineItemRarity.Rare);
            AddItem("HunterPants_Spring", ExpansionHardlineItemRarity.Rare);
            AddItem("HunterPants_Summer", ExpansionHardlineItemRarity.Rare);

            AddItem("GorkaPants_Summer", ExpansionHardlineItemRarity.Rare);
            AddItem("GorkaPants_Autumn", ExpansionHardlineItemRarity.Rare);
            AddItem("GorkaPants_Flat", ExpansionHardlineItemRarity.Rare);
            AddItem("GorkaPants_PautRev", ExpansionHardlineItemRarity.Rare);

            AddItem("NBCPantsGray", ExpansionHardlineItemRarity.Legendary);
            AddItem("NBCPantsYellow", ExpansionHardlineItemRarity.Legendary);

            AddItem("Chainmail_Leggings", ExpansionHardlineItemRarity.Exotic);

            //! Pistols
            AddItem("MakarovIJ70", ExpansionHardlineItemRarity.Rare);
            AddItem("Derringer_Black", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Derringer_Grey", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Derringer_Pink", ExpansionHardlineItemRarity.Uncommon);
            AddItem("CZ75", ExpansionHardlineItemRarity.Rare);
            AddItem("FNX45", ExpansionHardlineItemRarity.Rare);
            AddItem("Glock19", ExpansionHardlineItemRarity.Rare);
            AddItem("MKII", ExpansionHardlineItemRarity.Rare);
            AddItem("Colt1911", ExpansionHardlineItemRarity.Rare);
            AddItem("Engraved1911", ExpansionHardlineItemRarity.Rare);
            AddItem("Magnum", ExpansionHardlineItemRarity.Rare);
            AddItem("Deagle", ExpansionHardlineItemRarity.Rare);
            AddItem("Deagle_Gold", ExpansionHardlineItemRarity.Rare);
            AddItem("P1", ExpansionHardlineItemRarity.Rare);
            AddItem("Longhorn", ExpansionHardlineItemRarity.Rare);

            //! Rifles
            AddItem("Izh18", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Ruger1022", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Repeater", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Mosin9130", ExpansionHardlineItemRarity.Rare);
            AddItem("CZ527", ExpansionHardlineItemRarity.Rare);
            AddItem("CZ550", ExpansionHardlineItemRarity.Rare);
            AddItem("Winchester70", ExpansionHardlineItemRarity.Rare);
            AddItem("SSG82", ExpansionHardlineItemRarity.Rare);
            AddItem("SKS", ExpansionHardlineItemRarity.Rare);

            //! Shirts and T-Shirts
            AddItem("TShirt_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("TShirt_OrangeWhiteStripes", ExpansionHardlineItemRarity.Common);
            AddItem("TShirt_Red", ExpansionHardlineItemRarity.Common);
            AddItem("TShirt_RedBlackStripes", ExpansionHardlineItemRarity.Common);
            AddItem("TShirt_Beige", ExpansionHardlineItemRarity.Common);
            AddItem("TShirt_Grey", ExpansionHardlineItemRarity.Common);
            AddItem("TShirt_Black", ExpansionHardlineItemRarity.Common);
            AddItem("TShirt_Green", ExpansionHardlineItemRarity.Common);

            AddItem("TelnyashkaShirt", ExpansionHardlineItemRarity.Common);

            AddItem("Shirt_BlueCheck", ExpansionHardlineItemRarity.Common);
            AddItem("Shirt_BlueCheckBright", ExpansionHardlineItemRarity.Common);
            AddItem("Shirt_RedCheck", ExpansionHardlineItemRarity.Common);
            AddItem("Shirt_WhiteCheck", ExpansionHardlineItemRarity.Common);
            AddItem("Shirt_PlaneBlack", ExpansionHardlineItemRarity.Common);
            AddItem("Shirt_GreenCheck", ExpansionHardlineItemRarity.Common);

            AddItem("MedicalScrubsShirt_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("MedicalScrubsShirt_Green", ExpansionHardlineItemRarity.Common);
            AddItem("MedicalScrubsShirt_White", ExpansionHardlineItemRarity.Common);

            AddItem("ChernarusSportShirt", ExpansionHardlineItemRarity.Common);

            AddItem("TacticalShirt_Grey", ExpansionHardlineItemRarity.Rare);
            AddItem("TacticalShirt_Tan", ExpansionHardlineItemRarity.Rare);
            AddItem("TacticalShirt_Black", ExpansionHardlineItemRarity.Rare);
            AddItem("TacticalShirt_Olive", ExpansionHardlineItemRarity.Rare);

            //! Shotguns
            AddItem("Mp133Shotgun", ExpansionHardlineItemRarity.Rare);
            AddItem("Izh43Shotgun", ExpansionHardlineItemRarity.Rare);
            AddItem("Izh18Shotgun", ExpansionHardlineItemRarity.Rare);
            AddItem("Saiga", ExpansionHardlineItemRarity.Rare);

            //! Skirts and Dresses
            AddItem("Skirt_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("Skirt_Red", ExpansionHardlineItemRarity.Common);
            AddItem("Skirt_White", ExpansionHardlineItemRarity.Common);
            AddItem("Skirt_Yellow", ExpansionHardlineItemRarity.Common);

            AddItem("MiniDress_Pink", ExpansionHardlineItemRarity.Common);
            AddItem("MiniDress_PinkChecker", ExpansionHardlineItemRarity.Common);
            AddItem("MiniDress_RedChecker", ExpansionHardlineItemRarity.Common);
            AddItem("MiniDress_BlueChecker", ExpansionHardlineItemRarity.Common);
            AddItem("MiniDress_BlueWithDots", ExpansionHardlineItemRarity.Common);
            AddItem("MiniDress_WhiteChecker", ExpansionHardlineItemRarity.Common);
            AddItem("MiniDress_BrownChecker", ExpansionHardlineItemRarity.Common);
            AddItem("MiniDress_GreenChecker", ExpansionHardlineItemRarity.Common);

            AddItem("NurseDress_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("NurseDress_White", ExpansionHardlineItemRarity.Common);

            //! Sniper Rifles
            AddItem("VSS", ExpansionHardlineItemRarity.Rare);
            AddItem("ASVAL", ExpansionHardlineItemRarity.Rare);
            AddItem("B95", ExpansionHardlineItemRarity.Rare);
            AddItem("SVD", ExpansionHardlineItemRarity.Rare);
            AddItem("Scout", ExpansionHardlineItemRarity.Rare);

            //! Submachine-Guns
            AddItem("CZ61", ExpansionHardlineItemRarity.Rare);
            AddItem("UMP45", ExpansionHardlineItemRarity.Rare);
            AddItem("MP5K", ExpansionHardlineItemRarity.Rare);
            AddItem("AKS74U", ExpansionHardlineItemRarity.Rare);
            AddItem("PP19", ExpansionHardlineItemRarity.Rare);

            //! Supply Items
            AddItem("Paper", ExpansionHardlineItemRarity.Common);
            AddItem("Pen_Black", ExpansionHardlineItemRarity.Common);
            AddItem("Pen_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("Pen_Green", ExpansionHardlineItemRarity.Common);
            AddItem("Pen_Red", ExpansionHardlineItemRarity.Common);
            AddItem("Rope", ExpansionHardlineItemRarity.Uncommon);
            AddItem("TannedLeather", ExpansionHardlineItemRarity.Uncommon);
            AddItem("AntiPestsSpray", ExpansionHardlineItemRarity.Rare);
            AddItem("MetalWire", ExpansionHardlineItemRarity.Rare);
            AddItem("EpoxyPutty", ExpansionHardlineItemRarity.Rare);
            AddItem("DuctTape", ExpansionHardlineItemRarity.Rare);
            AddItem("Pot", ExpansionHardlineItemRarity.Rare);
            AddItem("HandcuffKeys", ExpansionHardlineItemRarity.Rare);
            AddItem("Handcuffs", ExpansionHardlineItemRarity.Rare);
            AddItem("Netting", ExpansionHardlineItemRarity.Uncommon);
            AddItem("BurlapSack", ExpansionHardlineItemRarity.Uncommon);
            AddItem("WoodenPlank", ExpansionHardlineItemRarity.Uncommon);
            AddItem("MetalPlate", ExpansionHardlineItemRarity.Uncommon);
            AddItem("SmallGasCanister", ExpansionHardlineItemRarity.Uncommon);
            AddItem("MediumGasCanister", ExpansionHardlineItemRarity.Uncommon);
            AddItem("LargeGasCanister", ExpansionHardlineItemRarity.Uncommon);
            AddItem("NailBox", ExpansionHardlineItemRarity.Rare);
            AddItem("Nail", ExpansionHardlineItemRarity.Rare);
            AddItem("BarbedWire", ExpansionHardlineItemRarity.Rare);
            AddItem("Fabric", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Camonet", ExpansionHardlineItemRarity.Rare);
            AddItem("HescoBox", ExpansionHardlineItemRarity.Rare);
            AddItem("PowerGenerator", ExpansionHardlineItemRarity.Rare);

            //! Sweaters and Hoodies
            AddItem("Sweater_Gray", ExpansionHardlineItemRarity.Common);
            AddItem("Sweater_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("Sweater_Red", ExpansionHardlineItemRarity.Common);
            AddItem("Sweater_Green", ExpansionHardlineItemRarity.Common);

            AddItem("Hoodie_Blue", ExpansionHardlineItemRarity.Common);
            AddItem("Hoodie_Black", ExpansionHardlineItemRarity.Common);
            AddItem("Hoodie_Brown", ExpansionHardlineItemRarity.Common);
            AddItem("Hoodie_Grey", ExpansionHardlineItemRarity.Common);
            AddItem("Hoodie_Red", ExpansionHardlineItemRarity.Common);
            AddItem("Hoodie_Green", ExpansionHardlineItemRarity.Common);

            //! Tents
            AddItem("PartyTent", ExpansionHardlineItemRarity.Uncommon);
            AddItem("PartyTent_Blue", ExpansionHardlineItemRarity.Uncommon);
            AddItem("PartyTent_Brown", ExpansionHardlineItemRarity.Uncommon);
            AddItem("PartyTent_Lunapark", ExpansionHardlineItemRarity.Uncommon);
            AddItem("MediumTent", ExpansionHardlineItemRarity.Uncommon);
            AddItem("MediumTent_Orange", ExpansionHardlineItemRarity.Uncommon);
            AddItem("MediumTent_Green", ExpansionHardlineItemRarity.Uncommon);
            AddItem("LargeTent", ExpansionHardlineItemRarity.Uncommon);
            AddItem("CarTent", ExpansionHardlineItemRarity.Uncommon);

            //! Tools
            AddItem("Screwdriver", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Wrench", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Pliers", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Whetstone", ExpansionHardlineItemRarity.Rare);
            AddItem("Hammer", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Hacksaw", ExpansionHardlineItemRarity.Uncommon);
            AddItem("HandSaw", ExpansionHardlineItemRarity.Uncommon);
            AddItem("CanOpener", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Hatchet", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Lockpick", ExpansionHardlineItemRarity.Rare);

            AddItem("LugWrench", ExpansionHardlineItemRarity.Uncommon);
            AddItem("PipeWrench", ExpansionHardlineItemRarity.Uncommon);
            AddItem("FryingPan", ExpansionHardlineItemRarity.Rare);
            AddItem("Sickle", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Crowbar", ExpansionHardlineItemRarity.Rare);
            AddItem("Shovel", ExpansionHardlineItemRarity.Rare);
            AddItem("Pickaxe", ExpansionHardlineItemRarity.Rare);
            AddItem("SledgeHammer", ExpansionHardlineItemRarity.Uncommon);
            AddItem("FarmingHoe", ExpansionHardlineItemRarity.Uncommon);
            AddItem("WoodAxe", ExpansionHardlineItemRarity.Rare);
            AddItem("FirefighterAxe", ExpansionHardlineItemRarity.Rare);
            AddItem("Pitchfork", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Blowtorch", ExpansionHardlineItemRarity.Rare);

            //! Vegetables
            AddItem("Apple", ExpansionHardlineItemRarity.Uncommon);
            AddItem("GreenBellPepper", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Zucchini", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Pumpkin", ExpansionHardlineItemRarity.Uncommon);
            AddItem("SlicedPumpkin", ExpansionHardlineItemRarity.Uncommon);
            AddItem("PotatoSeed", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Potato", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Tomato", ExpansionHardlineItemRarity.Uncommon);
            AddItem("SambucusBerry", ExpansionHardlineItemRarity.Uncommon);
            AddItem("CaninaBerry", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Plum", ExpansionHardlineItemRarity.Uncommon);
            AddItem("Pear", ExpansionHardlineItemRarity.Uncommon);
            //AddItem("Kiwi", ExpansionHardlineItemRarity.Uncommon);
            //AddItem("Orange", ExpansionHardlineItemRarity.Uncommon);
            //AddItem("Banana", ExpansionHardlineItemRarity.Uncommon);

            AddItem("AgaricusMushroom", ExpansionHardlineItemRarity.Uncommon);
            AddItem("AmanitaMushroom", ExpansionHardlineItemRarity.Uncommon);
            AddItem("MacrolepiotaMushroom", ExpansionHardlineItemRarity.Uncommon);
            AddItem("LactariusMushroom", ExpansionHardlineItemRarity.Uncommon);
            AddItem("PsilocybeMushroom", ExpansionHardlineItemRarity.Uncommon);
            AddItem("AuriculariaMushroom", ExpansionHardlineItemRarity.Uncommon);
            AddItem("BoletusMushroom", ExpansionHardlineItemRarity.Uncommon);
            AddItem("PleurotusMushroom", ExpansionHardlineItemRarity.Uncommon);

            //! Vehicle parts
            AddItem("HeadlightH7_Box", ExpansionHardlineItemRarity.Uncommon);
            AddItem("HeadlightH7", ExpansionHardlineItemRarity.Uncommon);
            AddItem("CarRadiator", ExpansionHardlineItemRarity.Rare);

            AddItem("TireRepairKit", ExpansionHardlineItemRarity.Rare);
            AddItem("SparkPlug", ExpansionHardlineItemRarity.Rare);
            AddItem("GlowPlug", ExpansionHardlineItemRarity.Rare);

            AddItem("HatchbackHood", ExpansionHardlineItemRarity.Rare);
            AddItem("HatchbackHood_Blue", ExpansionHardlineItemRarity.Rare);
            AddItem("HatchbackHood_White", ExpansionHardlineItemRarity.Rare);

            AddItem("HatchbackTrunk", ExpansionHardlineItemRarity.Rare);
            AddItem("HatchbackTrunk_Blue", ExpansionHardlineItemRarity.Rare);
            AddItem("HatchbackTrunk_White", ExpansionHardlineItemRarity.Rare);

            AddItem("HatchbackDoors_Driver", ExpansionHardlineItemRarity.Rare);
            AddItem("HatchbackDoors_Driver_Blue", ExpansionHardlineItemRarity.Rare);
            AddItem("HatchbackDoors_Driver_White", ExpansionHardlineItemRarity.Rare);

            AddItem("HatchbackDoors_CoDriver", ExpansionHardlineItemRarity.Rare);
            AddItem("HatchbackDoors_CoDriver_Blue", ExpansionHardlineItemRarity.Rare);
            AddItem("HatchbackDoors_CoDriver_White", ExpansionHardlineItemRarity.Rare);

            AddItem("HatchbackWheel", ExpansionHardlineItemRarity.Rare);

            AddItem("Hatchback_02_Hood", ExpansionHardlineItemRarity.Rare);
            AddItem("Hatchback_02_Hood_Black", ExpansionHardlineItemRarity.Rare);
            AddItem("Hatchback_02_Hood_Blue", ExpansionHardlineItemRarity.Rare);

            AddItem("Hatchback_02_Trunk", ExpansionHardlineItemRarity.Rare);
            AddItem("Hatchback_02_Trunk_Black", ExpansionHardlineItemRarity.Rare);
            AddItem("Hatchback_02_Trunk_Blue", ExpansionHardlineItemRarity.Rare);

            AddItem("Hatchback_02_Door_1_1", ExpansionHardlineItemRarity.Rare);
            AddItem("Hatchback_02_Door_1_1_Black", ExpansionHardlineItemRarity.Rare);
            AddItem("Hatchback_02_Door_1_1_Blue", ExpansionHardlineItemRarity.Rare);

            AddItem("Hatchback_02_Door_1_2", ExpansionHardlineItemRarity.Rare);
            AddItem("Hatchback_02_Door_1_2_Black", ExpansionHardlineItemRarity.Rare);
            AddItem("Hatchback_02_Door_1_2_Blue", ExpansionHardlineItemRarity.Rare);

            AddItem("Hatchback_02_Door_2_1", ExpansionHardlineItemRarity.Rare);
            AddItem("Hatchback_02_Door_2_1_Black", ExpansionHardlineItemRarity.Rare);
            AddItem("Hatchback_02_Door_2_1_Blue", ExpansionHardlineItemRarity.Rare);

            AddItem("Hatchback_02_Door_2_2", ExpansionHardlineItemRarity.Rare);
            AddItem("Hatchback_02_Door_2_2_Black", ExpansionHardlineItemRarity.Rare);
            AddItem("Hatchback_02_Door_2_2_Blue", ExpansionHardlineItemRarity.Rare);

            AddItem("Hatchback_02_Wheel", ExpansionHardlineItemRarity.Rare);

            AddItem("CivSedanHood", ExpansionHardlineItemRarity.Rare);
            AddItem("CivSedanHood_Wine", ExpansionHardlineItemRarity.Rare);
            AddItem("CivSedanHood_Black", ExpansionHardlineItemRarity.Rare);

            AddItem("CivSedanTrunk", ExpansionHardlineItemRarity.Rare);
            AddItem("CivSedanTrunk_Wine", ExpansionHardlineItemRarity.Rare);
            AddItem("CivSedanTrunk_Black", ExpansionHardlineItemRarity.Rare);

            AddItem("CivSedanDoors_Driver", ExpansionHardlineItemRarity.Rare);
            AddItem("CivSedanDoors_Driver_Wine", ExpansionHardlineItemRarity.Rare);
            AddItem("CivSedanDoors_Driver_Black", ExpansionHardlineItemRarity.Rare);

            AddItem("CivSedanDoors_CoDriver", ExpansionHardlineItemRarity.Rare);
            AddItem("CivSedanDoors_CoDriver_Wine", ExpansionHardlineItemRarity.Rare);
            AddItem("CivSedanDoors_CoDriver_Black", ExpansionHardlineItemRarity.Rare);

            AddItem("CivSedanDoors_BackLeft", ExpansionHardlineItemRarity.Rare);
            AddItem("CivSedanDoors_BackLeft_Wine", ExpansionHardlineItemRarity.Rare);
            AddItem("CivSedanDoors_BackLeft_Black", ExpansionHardlineItemRarity.Rare);

            AddItem("CivSedanDoors_BackRight", ExpansionHardlineItemRarity.Rare);
            AddItem("CivSedanDoors_BackRight_Wine", ExpansionHardlineItemRarity.Rare);
            AddItem("CivSedanDoors_BackRight_Black", ExpansionHardlineItemRarity.Rare);

            AddItem("CivSedanWheel", ExpansionHardlineItemRarity.Rare);

            AddItem("Sedan_02_Hood", ExpansionHardlineItemRarity.Rare);
            AddItem("Sedan_02_Hood_Red", ExpansionHardlineItemRarity.Rare);
            AddItem("Sedan_02_Hood_Grey", ExpansionHardlineItemRarity.Rare);

            AddItem("Sedan_02_Trunk", ExpansionHardlineItemRarity.Rare);
            AddItem("Sedan_02_Trunk_Red", ExpansionHardlineItemRarity.Rare);
            AddItem("Sedan_02_Trunk_Grey", ExpansionHardlineItemRarity.Rare);

            AddItem("Sedan_02_Door_1_1", ExpansionHardlineItemRarity.Rare);
            AddItem("Sedan_02_Door_1_1_Red", ExpansionHardlineItemRarity.Rare);
            AddItem("Sedan_02_Door_1_1_Grey", ExpansionHardlineItemRarity.Rare);

            AddItem("Sedan_02_Door_1_2", ExpansionHardlineItemRarity.Rare);
            AddItem("Sedan_02_Door_1_2_Red", ExpansionHardlineItemRarity.Rare);
            AddItem("Sedan_02_Door_1_2_Grey", ExpansionHardlineItemRarity.Rare);

            AddItem("Sedan_02_Door_2_1", ExpansionHardlineItemRarity.Rare);
            AddItem("Sedan_02_Door_2_1_Red", ExpansionHardlineItemRarity.Rare);
            AddItem("Sedan_02_Door_2_1_Grey", ExpansionHardlineItemRarity.Rare);

            AddItem("Sedan_02_Door_2_2", ExpansionHardlineItemRarity.Rare);
            AddItem("Sedan_02_Door_2_2_Red", ExpansionHardlineItemRarity.Rare);
            AddItem("Sedan_02_Door_2_2_Grey", ExpansionHardlineItemRarity.Rare);

            AddItem("Sedan_02_Wheel", ExpansionHardlineItemRarity.Rare);

            AddItem("Truck_01_Hood", ExpansionHardlineItemRarity.Rare);
            AddItem("Truck_01_Hood_Blue", ExpansionHardlineItemRarity.Rare);
            AddItem("Truck_01_Hood_Orange", ExpansionHardlineItemRarity.Rare);

            AddItem("Truck_01_Door_1_1", ExpansionHardlineItemRarity.Rare);
            AddItem("Truck_01_Door_1_1_Blue", ExpansionHardlineItemRarity.Rare);
            AddItem("Truck_01_Door_1_1_Orange", ExpansionHardlineItemRarity.Rare);

            AddItem("Truck_01_Door_2_1", ExpansionHardlineItemRarity.Rare);
            AddItem("Truck_01_Door_2_1_Blue", ExpansionHardlineItemRarity.Rare);
            AddItem("Truck_01_Door_2_1_Orange", ExpansionHardlineItemRarity.Rare);

            AddItem("Truck_01_Wheel", ExpansionHardlineItemRarity.Rare);
            AddItem("Truck_01_WheelDouble", ExpansionHardlineItemRarity.Rare);

            AddItem("Offroad_02_Hood", ExpansionHardlineItemRarity.Rare);

            AddItem("Offroad_02_Trunk", ExpansionHardlineItemRarity.Rare);

            AddItem("Offroad_02_Door_1_1", ExpansionHardlineItemRarity.Rare);

            AddItem("Offroad_02_Door_1_2", ExpansionHardlineItemRarity.Rare);

            AddItem("Offroad_02_Door_2_1", ExpansionHardlineItemRarity.Rare);

            AddItem("Offroad_02_Door_2_2", ExpansionHardlineItemRarity.Rare);

            AddItem("Offroad_02_Wheel", ExpansionHardlineItemRarity.Rare);

            //! Vests
            AddItem("ReflexVest", ExpansionHardlineItemRarity.Uncommon);

            AddItem("PoliceVest", ExpansionHardlineItemRarity.Rare);

            AddItem("PressVest_Blue", ExpansionHardlineItemRarity.Rare);
            AddItem("PressVest_LightBlue", ExpansionHardlineItemRarity.Rare);

            AddItem("UKAssVest_Black", ExpansionHardlineItemRarity.Rare);
            AddItem("UKAssVest_Khaki", ExpansionHardlineItemRarity.Rare);
            AddItem("UKAssVest_Olive", ExpansionHardlineItemRarity.Rare);
            AddItem("UKAssVest_Camo", ExpansionHardlineItemRarity.Rare);

            AddItem("SmershVest", ExpansionHardlineItemRarity.Rare);

            AddItem("HighCapacityVest_Black", ExpansionHardlineItemRarity.Rare);
            AddItem("HighCapacityVest_Olive", ExpansionHardlineItemRarity.Rare);

            AddItem("PlateCarrierVest", ExpansionHardlineItemRarity.Epic);
            AddItem("PlateCarrierVest_Green", ExpansionHardlineItemRarity.Epic);
            AddItem("PlateCarrierVest_Black", ExpansionHardlineItemRarity.Epic);
            AddItem("PlateCarrierVest_Camo", ExpansionHardlineItemRarity.Epic);
            AddItem("HuntingVest", ExpansionHardlineItemRarity.Rare);

            AddItem("Chestplate", ExpansionHardlineItemRarity.Epic);
        }
        void AddItem(string type, ExpansionHardlineItemRarity rarity)
        {
            type.ToLower();
            ItemRarity.Add(type, (int)rarity);
        }
    }
    public class EntityReputationlevels
    {
        public string Classname { get; set; }
        public int Level { get; set; }

        public EntityReputationlevels(string _classname, int _level)
        {
            Classname = _classname;
            Level = _level;
        }

        public override string ToString()
        {
            return Classname;
        }

    }
    public class ExpansionHardlinePlayerDataList
    {
        private string m_ExpansionHardlinePlayerDataPath;

        public BindingList<ExpansionHardlinePlayerData> HardlinePlayerDataList { get; set; }

        public ExpansionHardlinePlayerDataList(string HardlinePlayerDataPath)
        {
            m_ExpansionHardlinePlayerDataPath = HardlinePlayerDataPath;
            HardlinePlayerDataList = new BindingList<ExpansionHardlinePlayerData>();
            DirectoryInfo d = new DirectoryInfo(m_ExpansionHardlinePlayerDataPath);
            FileInfo[] Files = d.GetFiles();
            foreach (FileInfo file in Files)
            {
                try
                {
                    ExpansionHardlinePlayerData ExpansionHardlinePlayerData = new ExpansionHardlinePlayerData(file.FullName);
                    ExpansionHardlinePlayerData.Filename = Path.GetFileNameWithoutExtension(file.Name); ;
                    ExpansionHardlinePlayerData.isDirty = false;
                    HardlinePlayerDataList.Add(ExpansionHardlinePlayerData);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("there is an error in the following file\n" + file.FullName + Environment.NewLine + ex.InnerException.Message);
                }
            }
        }
    }
    public class ExpansionHardlinePlayerData
    {
        public string Filename { get; set; }
        public bool isDirty { get; set; }

        const int CONFIGVERSION = 8;

        public int Reputation;
        public int factionRepCount;
        public BindingList<FactionReps> FactionReputation;
        public int FactionID;
        public int PersonalStorageLevel;


        public ExpansionHardlinePlayerData(string fileName)
        {
            FactionReputation = new BindingList<FactionReps>();
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                if (br.ReadInt32() != CONFIGVERSION) return;
                Reputation = br.ReadInt32();
                factionRepCount = br.ReadInt32();
                for (int i = 0; i < factionRepCount; i++)
                {
                    FactionReputation.Add(new FactionReps(br));
                }
                FactionID = br.ReadInt32();
                PersonalStorageLevel = br.ReadInt32();
            }
        }
        public void SaveFIle(string path)
        {
            using (FileStream fs = new FileStream(path + "//" + Filename + ".bin", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                bw.Write(CONFIGVERSION);
                bw.Write(Reputation);
                bw.Write(FactionReputation.Count());
                foreach (FactionReps fr in FactionReputation)
                {
                    bw.Write(fr.FactionID);
                    bw.Write(fr.FactionID);
                }
                bw.Write(FactionID);
                bw.Write(PersonalStorageLevel);
            }
        }
        public override string ToString()
        {
            return Filename;
        }
        public void backupandDelete(string HardlineplayerdataPath)
        {
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            string Fullfilename = HardlineplayerdataPath + "\\" + Filename + ".json";
            if (File.Exists(Fullfilename))
            {
                Directory.CreateDirectory(HardlineplayerdataPath + "\\Backup\\" + SaveTime);
                File.Copy(Fullfilename, HardlineplayerdataPath + "\\Backup\\" + SaveTime + "\\" + Filename + ".bak");
                File.Delete(Fullfilename);
            }
        }
        public int GetReputation()
        {
            return Reputation;
        }
        public void SetReputation(int value)
        {
            Reputation = value;
        }
    }
    public class FactionReps
    {
        public int FactionID;
        public int FactionRep;

        public FactionReps(BinaryReader br)
        {
            FactionID = br.ReadInt32();
            FactionRep = br.ReadInt32();
        }
        public override string ToString()
        {
            return FactionID.ToString();
        }
    }
}
