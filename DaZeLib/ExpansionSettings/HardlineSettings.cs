using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZeLib
{
    public class HardLineSettings
    {
        const int CurrentVersion = 7;

        public int m_Version { get; set; }
        public int ReputationOnKillInfected { get; set; }
        public int ReputationOnKillPlayer { get; set; }
        public int ReputationOnKillAnimal { get; set; }
        public int ReputationOnKillAI { get; set; }
        public int ReputationLossOnDeath { get; set; }
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
        public int UseFactionReputation { get; set;}
        public int EnableFactionPersistence { get; set; }
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

        public HardLineSettings()
        {
            m_Version = CurrentVersion;
            ItemRarity = new Dictionary<string, int>();
            isDirty = true;

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

        const int CONFIGVERSION = 4;

        private int Reputation;
        private int PlayerKills;
        private int AIKills;
        private int InfectedKills;
        private int PlayerDeaths;

        public ExpansionHardlinePlayerData(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                if (br.ReadInt32() != CONFIGVERSION) return;
                Reputation = br.ReadInt32();
                PlayerKills = br.ReadInt32();
                AIKills = br.ReadInt32();
                InfectedKills = br.ReadInt32();
                PlayerDeaths = br.ReadInt32();
            }
        }
        public void SaveFIle(string path)
        {
            using (FileStream fs = new FileStream(path + "//" + Filename + ".bin", FileMode.Open, FileAccess.ReadWrite))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                bw.Write(CONFIGVERSION);
                bw.Write(Reputation);
                bw.Write(PlayerKills);
                bw.Write(AIKills);
                bw.Write(InfectedKills);
                bw.Write(PlayerDeaths);
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
        public int GetPlayerKills()
        {
            return PlayerKills;
        }
        public int GetAIKills()
        {
            return AIKills;
        }
        public int GetInfectedKills()
        {
            return InfectedKills;
        }
        public int GetPlayerDeaths()
        {
            return PlayerDeaths;
        }
    }
}
