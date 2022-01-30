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
        public int m_Version { get; set; }
        public int ServerMarkerOnDropLocation { get; set; }
        public int Server3DMarkerOnDropLocation { get; set; }
        public int ShowAirdropTypeOnMarker { get; set; }
        public int HeightIsRelativeToGroundLevel { get; set; }
        public float Height { get; set; }
        public float FollowTerrainFraction { get; set; }
        public float Speed { get; set; }
        public float Radius { get; set; }
        public float InfectedSpawnRadius { get; set; }
        public int InfectedSpawnInterval { get; set; }
        public int ItemCount { get; set; }
        public BindingList<AirdropContainers> Containers { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public AirdropsettingsJson()
        {
            m_Version = 2;
            Containers = new BindingList<AirdropContainers>();
            isDirty = true;
        }
    }
    public class AirdropContainers
    {
        public string Container { get; set; }
        public int Usage { get; set; }
        public float Weight { get; set; }
        public BindingList<containerLoot> Loot { get; set; }
        public BindingList<string> Infected { get; set; }
        public int ItemCount { get; set; }
        public int InfectedCount { get; set; }
        public int SpawnInfectedForPlayerCalledDrops { get; set; }

        public AirdropContainers()
        {
            Container = "ExpansionAirdropContainer";
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
        public float Chance { get; set; }
        public int QuantityPercent { get; set; }
        public BindingList<lootVarients> Variants { get; set; }
        public int Max { get; set; }

        public containerLoot()
        {

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
        public float Chance { get; set; }
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
