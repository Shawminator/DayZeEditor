using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{
    public enum KOZDayOfWeek
    {
        Sunday = 1,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday


    };
    public enum WeekNumber
    {
        Week1,
        Week2
    };
    public class KosZoneconfig
    {
        public int IsKosZoneActive { get; set; }
        public int ForceFPPinPVP { get; set; }
        public int KOSCheck { get; set; }
        public BindingList<Koszonearealocation> KosZoneAreaLocation { get; set; }

        [JsonIgnore]
        public string FullFilename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }
    }

    public class Koszonearealocation
    {
        public string KosZoneStatut { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Radius { get; set; }
        public int IsMsgActive { get; set; }
        public string MsgEnterZone { get; set; }
        public string MsgExitZone { get; set; }
        public int ShowOnMap { get; set; }

        public override string ToString()
        {
            return KosZoneStatut;
        }
    }

    public class KosPurgeConfig
    {
        public int IsPurgeEnabled { get; set; }
        public int IsDynPurgeEnabled { get; set; }
        public int ScheduleCycle { get; set; }
        public int RestartCycle { get; set; }
        public BindingList<Purgeschedule> PurgeSchedules { get; set; }
        public BindingList<Dynamicpurgeschedule> DynamicPurgeSchedules { get; set; }
        public BindingList<LockedDoorLocations> LockedDoorLocations { get; set; }

        [JsonIgnore]
        public string FullFilename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }
    }

    public class Purgeschedule
    {
        public string PurgeName { get; set; }
        public int WeekNumber { get; set; }
        public int Day { get; set; }
        public int StartHour { get; set; }
        public int StartMin { get; set; }
        public int EndHour { get; set; }
        public int EndMin { get; set; }
        public int AllowRaiding { get; set; }

        public override string ToString()
        {
            return PurgeName;
        }
    }

    public class Dynamicpurgeschedule
    {
        public string DynamicPurgeName { get; set; }
        public BindingList<int> WeekNumber { get; set; }
        public BindingList<int> Day { get; set; }
        public decimal Chance { get; set; }
        public int DurationMin { get; set; }
        public int DurationMax { get; set; }
        public int StartHour { get; set; }
        public int StartMin { get; set; }
        public int EndHour { get; set; }
        public int EndMin { get; set; }
        public int AllowRaiding { get; set; }

        public override string ToString()
        {
            return DynamicPurgeName;
        }
    }
    public class LockedDoorLocations
    {
        public float Building_X { get; set; }
        public float Building_Y { get; set; }
        public float Building_Z { get; set; }
        public int DoorIndex { get; set; }
        public string BuildingClassName { get; set; }

        public override string ToString()
        {
            return BuildingClassName;
        }
    }


}
