using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{

    public class Helicrash
    {
        [JsonIgnore]
        public const int CONFIGVERSION = 1;
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

        public int ConfigVersion { get; set; }
        public int HeliCrashEnabled { get; set; }
        public int MakeCrashAreaPVP { get; set; }
        public int ShowCrashMapMarker { get; set; }
        public int CheckForPlayersbeforeDespawn { get; set; }
        public int ShowNotifications { get; set; }
        public int HelicrashStartTime { get; set; }
        public int HelicrashSpawnTime { get; set; }
        public int HelicrashDespawnTime { get; set; }
        public BindingList<Crashpoint> CrashPoints { get; set; }
        public HeliCrashHeliConfig HeliCrashHeliConfig { get; set; }
        public AnimalSpawn AnimalSpawn { get; set; }
        public ZombieSpawn ZombieSpawn { get; set; }
        public BindingList<string> RandomRewards { get; set; }

        public bool CheckVersion()
        {
            if (ConfigVersion != CONFIGVERSION)
            {
                return false;
            }
            return true;
        }
    }

    public class Crashpoint
    {
        public decimal x { get; set; }
        public decimal y { get; set; }
        public decimal Radius { get; set; }
        public string Crash_Message { get; set; }
        public BindingList<string> RewardTables { get; set; }

        public override string ToString()
        {
            return Crash_Message;
        }
    }

    public class HeliCrashHeliConfig
    {
        public int start_height { get; set; }
        public int speed { get; set; }
        public int minimum_speed { get; set; }
    }

    public class AnimalSpawn
    {
        public BindingList<string> animal_name { get; set; }
        public int radius { get; set; }
        public int amount_minimum { get; set; }
        public int amount_maximum { get; set; }
    }

    public class ZombieSpawn
    {
        public BindingList<string> zombie_name { get; set; }
        public int radius { get; set; }
        public int amount_minimum { get; set; }
        public int amount_maximum { get; set; }
    }
}
