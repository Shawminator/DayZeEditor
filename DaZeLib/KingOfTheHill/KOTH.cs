using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{

    public class KingOfTheHillConfig
    {
        [JsonIgnore]
        public string FullFilename;
        [JsonIgnore]
        public bool isDirty { get; set; }

        public int LastCapturePoint { get; set; }
        public string ConfigVersion { get; set; }
        public decimal Interval { get; set; }
        public decimal StartDelay { get; set; }
        public decimal CaptureTime { get; set; }
        public decimal EmptyEventTimeOut { get; set; }
        public decimal CleanUpTime { get; set; }
        public decimal PreStartDelay { get; set; }
        public BindingList<Hill> Hills { get; set; }
        public BindingList<Rewardpool> RewardPools { get; set; }
        public BindingList<string> ZombiesClassNames { get; set; }
        public decimal FullMapCheckTimer { get; set; }
        public decimal EventTickTime { get; set; }
        public int Logging { get; set; }
    }

    public class Hill
    {
        public string Name { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public decimal CaptureRadius { get; set; }
        public decimal EventRadius { get; set; }
        public int ZombieCount { get; set; }
        public BindingList<KOTHObject> Objects { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class KOTHObject
    {
        public string Item { get; set; }
        public float[] Position { get; set; }
        public float[] Orientation { get; set; }

        public override string ToString()
        {
            return Item;
        }
    }

    public class Rewardpool
    {
        public string Name { get; set; }
        public string RewardContainer { get; set; }
        public BindingList<Reward> Rewards { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Reward
    {
        public string Item { get; set; }
        public BindingList<string> ItemAttachments { get; set; }

        public override string ToString()
        {
            return Item;
        }
    }

}
