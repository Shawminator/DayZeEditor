using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class AirdropsettingsJson
    {
        const int CurrentVersion = 4; 

        public int m_Version { get; set; }
        public int ServerMarkerOnDropLocation { get; set; }
        public int Server3DMarkerOnDropLocation { get; set; }
        public int ShowAirdropTypeOnMarker { get; set; }
        public int HideCargoWhileParachuteIsDeployed { get; set; }
        public int HeightIsRelativeToGroundLevel { get; set; }
        public decimal Height { get; set; }
        public decimal FollowTerrainFraction { get; set; }
        public decimal Speed { get; set; }
        public decimal Radius { get; set; }
        public decimal InfectedSpawnRadius { get; set; }
        public int InfectedSpawnInterval { get; set; }
        public int ItemCount { get; set; }
        public BindingList<AirdropContainers> Containers { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public AirdropsettingsJson()
        {
            m_Version = CurrentVersion;
            ServerMarkerOnDropLocation = 1;
            Server3DMarkerOnDropLocation = 1;
            ShowAirdropTypeOnMarker = 1;
            HideCargoWhileParachuteIsDeployed = 1;
            HeightIsRelativeToGroundLevel = 1;
            Height = 450;
            FollowTerrainFraction = (decimal)0.5;
            Speed = 35;
            Radius = 1;
            InfectedSpawnRadius = 50;
            InfectedSpawnInterval = 250;
            ItemCount = 50;
            Containers = new BindingList<AirdropContainers>();
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
    }
    public class AirdropContainers
    {
        public string Container { get; set; }
        public decimal FallSpeed { get; set; }
        public int Usage { get; set; }
        public decimal Weight { get; set; }
        public BindingList<containerLoot> Loot { get; set; }
        public BindingList<string> Infected { get; set; }
        public int ItemCount { get; set; }
        public int InfectedCount { get; set; }
        public int SpawnInfectedForPlayerCalledDrops { get; set; }

        public AirdropContainers()
        {
            Container = "ExpansionAirdropContainer";
            FallSpeed = (decimal)4.5;
            Usage = 0;
            Weight = 0;
            Loot = new BindingList<containerLoot>();
            Infected = new BindingList<string>();
            ItemCount = 0;
            InfectedCount = 0;
            SpawnInfectedForPlayerCalledDrops = 0;
        }

        public override string ToString()
        {
            return Container;
        }
    }
    public class containerLoot
    {
        public string Name { get; set; }
        public BindingList<string> Attachments { get; set; }
        public decimal Chance { get; set; }
        public int QuantityPercent { get; set; }
        public int Max { get; set; }
        public int Min { get; set; }
        public BindingList<lootVarients> Variants { get; set; }


        public containerLoot()
        {
            Chance = (decimal)0.25;
            QuantityPercent = -1;
            Max = -1;
            Min = 0;
            Attachments = new BindingList<string>();
            Variants = new BindingList<lootVarients>();
        }
        public override string ToString()
        {
            return Name;
        }
    }
    public class lootVarients
    {
        public string Name { get; set; }
        public BindingList<string> Attachments { get; set; }
        public decimal Chance { get; set; }

        public lootVarients()
        {
            Chance = (decimal)0.2;
            Attachments = new BindingList<string>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
    public enum ContainerTypes
    {
        ExpansionAirdropContainer,
        ExpansionAirdropContainer_Medical,
        ExpansionAirdropContainer_Military,
        ExpansionAirdropContainer_Basebuilding,
        ExpansionAirdropContainer_Grey,
        ExpansionAirdropContainer_Blue,
        ExpansionAirdropContainer_Olive,
        ExpansionAirdropContainer_Military_GreenCamo,
        ExpansionAirdropContainer_Military_MarineCamo,
        ExpansionAirdropContainer_Military_OliveCamo,
        ExpansionAirdropContainer_Military_OliveCamo2,
        ExpansionAirdropContainer_Military_WinterCamo
    };
}
