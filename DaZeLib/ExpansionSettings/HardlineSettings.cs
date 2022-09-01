using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{

    public class HardLineSettings
    {
        const int CurrentVersion = 3;

        public int m_Version { get; set; }
        public int RankBambi { get; set; }
        public int RankSurvivor { get; set; }
        public int RankScout { get; set; }
        public int RankPathfinder { get; set; }
        public int RankHero { get; set; }
        public int RankSuperhero { get; set; }
        public int RankLegend { get; set; }
        public int RankKleptomaniac { get; set; }
        public int RankBully { get; set; }
        public int RankBandit { get; set; }
        public int RankKiller { get; set; }
        public int RankMadman { get; set; }
        public int HumanityBandageTarget { get; set; }
        public int HumanityOnKillInfected { get; set; }
        public int HumanityOnKillAI { get; set; }
        public int HumanityOnKillBambi { get; set; }
        public int HumanityOnKillHero { get; set; }
        public int HumanityOnKillBandit { get; set; }
        public int HumanityLossOnDeath { get; set; }
        public int PoorItemRequirement { get; set; }
        public int CommonItemRequirement { get; set; }
        public int UncommonItemRequirement { get; set; }
        public int RareItemRequirement { get; set; }
        public int EpicItemRequirement { get; set; }
        public int LegendaryItemRequirement { get; set; }
        public int MythicItemRequirement { get; set; }
        public int ExoticItemRequirement { get; set; }
        public int ShowHardlineHUD { get; set; }
        public int UseHumanity { get; set; }
        public int EnableItemRarity { get; set; }
        public int UseItemRarityForMarketPurchase { get; set; }
        public int UseItemRarityForMarketSell { get; set; }
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
            PoorItems = new BindingList<string>();
            CommonItems = new BindingList<string>();
            UncommonItems = new BindingList<string>();
            RareItems = new BindingList<string>();
            EpicItems = new BindingList<string>();
            LegendaryItems = new BindingList<string>();
            MythicItems = new BindingList<string>();
            ExoticItems = new BindingList<string>();

            foreach (KeyValuePair<string,int> item in ItemRarity)
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
                        PoorItems.Add(useitem);
                        break;
                    case 2:
                        CommonItems.Add(useitem);
                        break;
                    case 3:
                        UncommonItems.Add(useitem);
                        break;
                    case 4:
                        RareItems.Add(useitem);
                        break;
                    case 5:
                        EpicItems.Add(useitem);
                        break;
                    case 6:
                        LegendaryItems.Add(useitem);
                        break;
                    case 7:
                        MythicItems.Add(useitem);
                        break;
                    case 8:
                        ExoticItems.Add(useitem);
                        break;

                }
            }
        }
        public void convertliststoDict()
        {
            ItemRarity = new Dictionary<string, int>();
            foreach(string item in PoorItems)
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
    }
}
