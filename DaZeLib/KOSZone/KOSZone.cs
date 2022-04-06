using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{

    public class KosZoneconfig
    {
        public int IsKosZoneActive { get; set; }
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

        public override string ToString()
        {
            return KosZoneStatut;
        }
    } 

    public class KosPurgeConfig
    {
        public int IsPurgeActive { get; set; }
        public int IsDynPurgeActive { get; set; }
        public int TimeZone { get; set; }
        public BindingList<Purgeschedule> PurgeSchedules { get; set; }
        public BindingList<Dynamicpurgeschedule> DynamicPurgeSchedules { get; set; }

        [JsonIgnore]
        public string FullFilename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }
    }

    public class Purgeschedule
    {
        public int Day { get; set; }
        public int StartHour { get; set; }
        public int StartMin { get; set; }
        public int EndHour { get; set; }
        public int EndMin { get; set; }

        public override string ToString()
        {
            return Day.ToString();
        }
    }

    public class Dynamicpurgeschedule
    {
        public int Day { get; set; }
        public float Chance { get; set; }
        public int Duration { get; set; }

        public override string ToString()
        {
            return Day.ToString();
        }
    }

    public class KozRestartConfig
    {
        public int IsRestartMsgActive { get; set; }
        public BindingList<int> m_hours { get; set; }

        [JsonIgnore]
        public string FullFilename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

    }

}
