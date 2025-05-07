using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class TimedCrates
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty;

        public int ServerStartGracePeriod { get; set; }
        public BindingList<Crateconfig> CrateConfigs { get; set; }

        public void SetVectors()
        {
            foreach (Crateconfig crateconfig in CrateConfigs)
            {
                crateconfig.SetVectors();
            }
        }
        public void GetVectorstring()
        {
            foreach (Crateconfig crateconfig in CrateConfigs)
            {
                crateconfig.GetVectorstring();
            }
        }
    }

    public class Crateconfig
    {
        public bool StaticSpawns { get; set; }
        public int ActiveCrateCount { get; set; }
        public bool AutoRefresh { get; set; }
        public int RefreshInterval { get; set; }
        public string CrateType { get; set; }
        public bool UseCrateNotifications { get; set; }
        public string CrateSpawnNotification { get; set; }
        public string CrateStartNotification { get; set; }
        public string CrateEndNotification { get; set; }
        public int CountdownTime { get; set; }
        public int ZombieSpawnInterval { get; set; }
        public int ZombieSpawnMin { get; set; }
        public int ZombieSpawnMax { get; set; }
        public bool ZombieSpawningEnabled { get; set; }
        public bool UseBasicMarkers { get; set; }
        public bool UseMarkersDuringCountdownOnly { get; set; }
        public BindingList<string> ZombieClasses { get; set; }
        public BindingList<Cratelocation> CrateLocations { get; set; }

        public void SetVectors()
        {
            foreach(Cratelocation cl in CrateLocations)
            {
                cl._POSROT = new Vec3PandR(cl.Position + "|" + cl.Rotation, true);
            }
        }
        public void GetVectorstring()
        {
            foreach (Cratelocation cl in CrateLocations)
            {
                cl.Position = cl._POSROT.GetPositionString();
                cl.Rotation = cl._POSROT.GetRotationString();
            }
        }
    }

    public class Cratelocation
    {
        public string Position { get; set; }
        public string Rotation { get; set; }

        [JsonIgnore]
        public Vec3PandR _POSROT { get; set; }

        public override string ToString()
        {
            return _POSROT.GetString(); 
        }
    }

}
