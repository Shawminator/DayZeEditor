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
        public bool isDirty { get; set; }
        [JsonIgnore]
        public string FullFilename { get; set; }

        public int admin_log { get; set; }
        public int HelicrashSpawnTime { get; set; }
        public int HelicrashDespawnTime { get; set; }
        public BindingList<Crashpoint> CrashPoints { get; set; }
        public Helicopterus_[] HelicopterUS_ { get; set; }
        public BindingList<Animalspawnarray> AnimalSpawnArray { get; set; }
        public BindingList<Zombiespawnarray> ZombieSpawnArray { get; set; }
        public BindingList<string> Loot_Helicrash { get; set; }
        public BindingList<Weaponloottable> WeaponLootTables { get; set; }
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

    public class Helicopterus_
    {
        public int start_height { get; set; }
        public int minimum_height { get; set; }
        public int speed { get; set; }
        public int minimum_speed { get; set; }
        public int Maximum_Loot_Helicrash { get; set; }
        public int Maximum_Weapons_Helicrash { get; set; }
        public int Minimum_Loot_Helicrash { get; set; }
        public int Minimum_Weapons_Helicrash { get; set; }
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

    public class Weaponloottable
    {
        public string WeaponName { get; set; }
        public string MagazineName { get; set; }
        public BindingList<string> Attachments { get; set; }
        public string Sight { get; set; }

        public override string ToString()
        {
            return WeaponName;
        }
    }

}
