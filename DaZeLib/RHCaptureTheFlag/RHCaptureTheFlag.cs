using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZeLib
{
    class RHCaptureTheFlag
    {
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
        public int LoadMarkerAtWarning { get; set; }
        public decimal PreWarning { get; set; }
        public float[] Position { get; set; }
        public float CaptureRadius { get; set; }
        public float EventRadius { get; set; }
        public float EventTime { get; set; }
        public float CaptureTime { get; set; }
        public int AICount { get; set; }
        public BindingList<string> Rewards { get; set; }
        public int CleanUpTime { get; set; }
        public int StartWithFlag { get; set; }
        public int SpawnSmoke { get; set; }
        public string SmokeColour { get; set; }
        public decimal SmokeRadiusFromFlag { get; set; }
        public string ObjectMapFile { get; set; }
    }

}
