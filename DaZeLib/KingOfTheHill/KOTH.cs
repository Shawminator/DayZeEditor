using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZeLib
{

    public class KingOfTheHill
    {
        public int LastCapturePoint { get; set; }
        public string ConfigVersion { get; set; }
        public float Interval { get; set; }
        public float StartDelay { get; set; }
        public float CaptureTime { get; set; }
        public float EmptyEventTimeOut { get; set; }
        public float CleanUpTime { get; set; }
        public float PreStartDelay { get; set; }
        public BindingList<Hill> Hills { get; set; }
        public BindingList<Rewardpool> RewardPools { get; set; }
        public string[] ZombiesClassNames { get; set; }
        public float FullMapCheckTimer { get; set; }
        public float EventTickTime { get; set; }
        public int Logging { get; set; }
    }

    public class Hill
    {
        public string Name { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float CaptureRadius { get; set; }
        public float EventRadius { get; set; }
        public int ZombieCount { get; set; }
        public BindingList<Object> Objects { get; set; }
    }

    public class Object
    {
        public string Item { get; set; }
        public float[] Position { get; set; }
        public float[] Orientation { get; set; }
    }

    public class Rewardpool
    {
        public string Name { get; set; }
        public string RewardContainer { get; set; }
        public BindingList<Reward> Rewards { get; set; }
    }

    public class Reward
    {
        public string Item { get; set; }
        public BindingList<string> ItemAttachments { get; set; }
    }

}
