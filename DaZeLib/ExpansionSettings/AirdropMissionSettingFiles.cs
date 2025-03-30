using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class AirdropMissionSettingFiles
    {
        const int CurrentVersion = 3;

        public int m_Version { get; set; }
        public int Enabled { get; set; }
        public decimal Weight { get; set; }
        public int MissionMaxTime { get; set; }
        public string MissionName { get; set; }
        public int Difficulty { get; set; }
        public int Objective { get; set; }
        public string Reward { get; set; }
        public int ShowNotification { get; set; }
        public decimal Height { get; set; }
        public decimal DropZoneHeight { get;set; }
        public decimal Speed { get; set; }
        public decimal DropZoneSpeed { get; set; }
        public string Container { get; set; }
        public decimal FallSpeed { get; set; }
        public ExpansionAirdropLocation DropLocation { get; set; }
        public BindingList<string> Infected { get; set; }
        public int ItemCount { get; set; }
        public int InfectedCount { get; set; }
        public string AirdropPlaneClassName { get; set; }
        public BindingList<ExpansionLoot> Loot { get; set; }


        [JsonIgnore]
        public string MissionPath { get; set; }
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public AirdropMissionSettingFiles()
        {
            m_Version = CurrentVersion;
            Enabled = 1;
            Weight = (decimal)1.0;
            MissionMaxTime = 1200;
            MissionName = "";
            Difficulty = 0;
            Objective = 0;
            Reward = "";
            ShowNotification = 1;
            Height = (decimal)450.0;
            DropZoneHeight = (decimal)450.0;
            Speed = (decimal)25.0;
            DropZoneSpeed = (decimal)25.0;
            Container = "Random";
            FallSpeed = (decimal)4.5;
            DropLocation = new ExpansionAirdropLocation();
            Loot = new BindingList<ExpansionLoot>();
            Infected = new BindingList<string>();
            isDirty = true;
            ItemCount = -1;
            AirdropPlaneClassName = "";
            InfectedCount = -1;
        }
        public void SetIntValue(string mytype, int myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
        public void SetdecimalValue(string mytype, decimal myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
        public override string ToString()
        {
            return MissionName;
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
    }
    public class ExpansionAirdropLocation
    {
        public decimal x { get; set; }
        public decimal z { get; set; }
        public string Name { get; set; }
        public decimal Radius { get; set; }
    }

    public class ContaminatedAreaMissionSettingFiles
    {
        const int CurrentVersion = 0;
        [JsonIgnore]
        public string MissionPath { get; set; }
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public int m_Version { get; set; }
        public int Enabled { get; set; }
        public decimal Weight { get; set; }
        public int MissionMaxTime { get; set; }
        public string MissionName { get; set; }
        public int Difficulty { get; set; }
        public int Objective { get; set; }
        public string Reward { get; set; }
        public ContaminatedAreaMissionData Data { get; set; }
        public ContaminatedAreaMissionPlayerdata PlayerData { get; set; }
        public decimal StartDecayLifetime { get; set; }
        public decimal FinishDecayLifetime { get; set; }

        public override string ToString()
        {
            return MissionName;
        }
    }
    public class ContaminatedAreaMissionData
    {
        public decimal[] Pos { get; set; }
        public decimal Radius { get; set; }
        public decimal PosHeight { get; set; }
        public decimal NegHeight { get; set; }
        public int InnerRingCount { get; set; }
        public int InnerPartDist { get; set; }
        public int OuterRingToggle { get; set; }
        public int OuterPartDist { get; set; }
        public int OuterOffset { get; set; }
        public int VerticalLayers { get; set; }
        public int VerticalOffset { get; set; }
        public string ParticleName { get; set; }
    }
    public class ContaminatedAreaMissionPlayerdata
    {
        public string AroundPartName { get; set; }
        public string TinyPartName { get; set; }
        public string PPERequesterType { get; set; }
    }
}
