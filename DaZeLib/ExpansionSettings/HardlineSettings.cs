using System;
using System.Collections.Generic;
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
    }
}
