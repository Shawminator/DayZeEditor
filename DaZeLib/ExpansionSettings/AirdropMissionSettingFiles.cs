using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.ComponentModel;

namespace DayZeLib
{
    public class AirdropMissionSettingFiles
    {
        public int Enabled { get; set; }
        public float Weight { get; set; }
        public int MissionMaxTime { get; set; }
        public string MissionName { get; set; }
        public int Difficulty { get; set; }
        public int Objective { get; set; }
        public string Reward { get; set; }
        public int ShowNotification { get; set; }
        public float Height { get; set; }
        public float Speed { get; set; }
        public string Container { get; set; }
        public DropLocation DropLocation { get; set; }
        public BindingList<containerLoot> Loot { get; set; }
        public BindingList<string> Infected { get; set; }
        public int ItemCount { get; set; }
        public int InfectedCount { get; set; }


        [JsonIgnore]
        public string MissionPath { get; set; }
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public AirdropMissionSettingFiles()
        {
            DropLocation = new DropLocation();
            Loot = new BindingList<containerLoot>();
            Infected = new BindingList<string>();
            isDirty = true;
        }

        public void SetIntValue(string mytype, int myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
        public void SetFloatValue(string mytype, float myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
        public override string ToString()
        {
            return MissionName;
        }
    }
    public class DropLocation
    {
        public float x { get; set; }
        public float z { get; set; }
        public string Name { get; set; }
        public float Radius { get; set; }
    }
    public class Empty { }

    public class ContaminatedAreaMissionSettingFiles
    {
        [JsonIgnore]
        public string MissionPath { get; set; }
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

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
        public float[] Pos { get; set; }
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
