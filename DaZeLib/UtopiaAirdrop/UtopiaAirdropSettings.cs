using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class UtopiaAirdropSettings
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty;

        public string version { get; set; }
        public int heliHeightFromGround { get; set; }
        public decimal heliSpeed { get; set; }
        public int dropCrateContainerLifetimeInSeconds { get; set; }
        public int maxCreatures { get; set; }
        public int startDelayMin { get; set; }
        public int pkgIntervalMin { get; set; }
        public decimal awayMin { get; set; }
        public string title { get; set; }
        public string droppedMsg { get; set; }
        public string startMsg { get; set; }
        public int showMarkerForFlareDrop { get; set; }
        public decimal[] leftBottomCornerMap { get; set; }
        public decimal[] rightUpCornerMap { get; set; }
        public BindingList<Flareairdropcontainer> flareAirdropContainers { get; set; }
        public BindingList<Droplocation> dropLocations { get; set; }
        public BindingList<Lootpool> lootPools { get; set; }
        public BindingList<string> spawnCreatures { get; set; }
    }

    public class Flareairdropcontainer
    {
        public string containerName { get; set; }
        public BindingList<string> lootPools { get; set; }

        public override string ToString()
        {
            return containerName;
        }
    }

    public class Droplocation
    {
        public string name { get; set; }
        public decimal[] dropPosition { get; set; }
        public int radius { get; set; }
        public BindingList<Airdropcontainer> airdropContainers { get; set; }

        public override string ToString()
        {
            return name;
        }
    }

    public class Airdropcontainer
    {
        public string containerName { get; set; }
        public BindingList<string> lootPools { get; set; }

        public override string ToString()
        {
            return containerName;
        }
    }

    public class Lootpool
    {
        public string lootPoolName { get; set; }
        public int maxItems { get; set; }
        public BindingList<Item> items { get; set; }

        public override string ToString()
        {
            return lootPoolName;
        }

    }

    public class Item
    {
        public string name { get; set; }
        public int quantity { get; set; }
        public BindingList<string> attachments { get; set; }

        public override string ToString()
        {
            return name;
        }
    }


    public class UtopiaAirdropLoggingsettings
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty;

        public int logLevel { get; set; }
        public int refreshRateInSeconds { get; set; }

        public UtopiaAirdropLoggingsettings()
        {
            logLevel = 0;
            refreshRateInSeconds = 60;
        }
    }

}
