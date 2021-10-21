using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class LootChestTable
    {
        public float LootRandomization { get; set; }
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
        public BindingList<string> chest { get; set; }
        public BindingList<string> loot { get; set; }


        public override string ToString()
        {
            return name;
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
