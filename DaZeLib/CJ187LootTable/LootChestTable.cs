using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public enum LootchestOpenable
    {
        KeyOnly,
        KeyAndLockpick,
        KeyAndLockPickAndTools

    };
    public class LootChestTools
    {
        public BindingList<LCTools> LCTools { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }
    }
    public class LCTools
    {
        public string name { get; set; }
        public int time { get; set; }
        public int dmg { get; set; }
        public string desc { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
    public class LootChestTable
    {
        public int EnableDebug { get; set; }
        public int DeleteLogs { get; set; }
        public int MaxSpareMags { get; set; }
        public int RandomQuantity { get; set; }
        public BindingList<LootChestsLocations> LootChestsLocations { get; set; }
        public BindingList<LCPredefinedWeapons> LCPredefinedWeapons { get; set; }
        public BindingList<LootCategories> LootCategories { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }
    }
    public class LootChestsLocations
    {
        public string name { get; set; }
        public int number { get; set; }
        public BindingList<string> pos { get; set; }
        public string keyclass { get; set; }
        public int openable { get; set; }
        public BindingList<string> chest { get; set; }
        public decimal LootRandomization { get; set; }
        public int light { get; set; }
        public BindingList<string> loot { get; set; }


        public override string ToString()
        {
            return name;
        }
    }

    public class CJLoot
    {
        public string Classname { get; set; }
        public decimal Rarity { get; set; }

        public override string ToString()
        {
            return Classname;
        }
    }

    public class LCPredefinedWeapons
    {
        public string defname { get; set; }
        public string weapon { get; set; }
        public string magazine { get; set; }
        public BindingList<string> attachments { get; set; }
        public string optic { get; set; }
        public int opticbattery { get; set; }

        public override string ToString()
        {
            return defname;
        }
    }
    public class LootCategories
    {
        public string name { get; set; }
        public BindingList<string> Loot { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}
