using System.ComponentModel;

namespace DayZeLib
{
    class CaptureTheFlag
    {
        public int ConfigVersion { get; set; }
        public int IsCTFEnabled { get; set; }
        public int IsDynCTFEnabled { get; set; }
        public int ScheduleCycle { get; set; }
        public BindingList<int> RestartCycle { get; set; }
        public BindingList<Ctfschedule> CTFSchedules { get; set; }
        public BindingList<Dynamicctfschedule> DynamicCTFSchedules { get; set; }
        public BindingList<Flaglocation> FlagLocations { get; set; }
        public BindingList<string> AIList { get; set; }
    }

    public class Ctfschedule
    {
        public string CTFName { get; set; }
        public int WeekNumber { get; set; }
        public int Day { get; set; }
        public int StartHour { get; set; }
        public int StartMin { get; set; }
    }

    public class Dynamicctfschedule
    {
        public string DynamicCTFName { get; set; }
        public BindingList<int> WeekNumber { get; set; }
        public BindingList<int> Day { get; set; }
        public int MinTimeBetweenCTF { get; set; }
        public int MaxTimeBetweenCTF { get; set; }
    }

    public class Flaglocation
    {
        public string FlagName { get; set; }
        public string FLagPoleClassname { get; set; }
        public bool LoadMarkerAtWarning { get; set; }
        public float PreWarning { get; set; }
        public float[] Position { get; set; }
        public float CaptureRadius { get; set; }
        public float EventRadius { get; set; }
        public float EventTime { get; set; }
        public float CaptureTime { get; set; }
        public int AICount { get; set; }
        public BindingList<string> InitialRewards { get; set; }
        public BindingList<string> Rewards { get; set; }
        public int CleanUpTime { get; set; }
        public int StartWithFlag { get; set; }
        public int StartWithFlagRaised { get; set; } //only used when StartWithFlag is also set to true
        public int AllowFlagToBeLowered { get; set; }
        public string FlagClassName { get; set; }
        public int SpawnSmokeOnStart { get; set; }
        public int SpawnSmokeOnFlagRaised { get; set; }
        public string SmokeColour { get; set; }
        public float SmokeRadiusFromFlag { get; set; }
        public string ObjectMapFile { get; set; }
    }

}
