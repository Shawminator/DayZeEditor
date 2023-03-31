using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{

    public class Helicrash
    {
        [JsonIgnore]
        public const string Filename = "Helicrash.json";
        [JsonIgnore]
        public bool isDirty 
        {
            get;
            set; 
        }
        [JsonIgnore]
        public string FullFilename { get; set; }

        public int admin_log { get; set; }
        public int HeliCrashEnabled { get; set; }
        public int HelicrashSpawnTime { get; set; }
        public int HelicrashDespawnTime { get; set; }
        public BindingList<Crashpoint> CrashPoints { get; set; }
        public HelicopterArray[] HelicopterArray { get; set; }
        public BindingList<Animalspawnarray> AnimalSpawnArray { get; set; }
        public BindingList<Zombiespawnarray> ZombieSpawnArray { get; set; }
        public BindingList<string> LootTables { get; set; }
    }

    public class Crashpoint
    {
        public decimal x { get; set; }
        public decimal y { get; set; }
        public decimal Radius { get; set; }
        public string Crash_Message { get; set; }

        public override string ToString()
        {
            return Crash_Message;
        }
    }

    public class HelicopterArray
    {
        public int start_height { get; set; }
        public int minimum_height { get; set; }
        public int speed { get; set; }
        public int minimum_speed { get; set; }
    }

    public class Animalspawnarray
    {
        public BindingList<string> animal_name { get; set; }
        public int radius { get; set; }
        public int amount_minimum { get; set; }
        public int amount_maximum { get; set; }
    }

    public class Zombiespawnarray
    {
        public BindingList<string> zombie_name { get; set; }
        public int radius { get; set; }
        public int amount_minimum { get; set; }
        public int amount_maximum { get; set; }
    }
}
