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
        public int StaticSpawns { get; set; }
        public int ActiveCrateCount { get; set; }
        public int AutoRefresh { get; set; }
        public int RefreshInterval { get; set; }
        public string CrateType { get; set; }
        public int UseCrateNotifications { get; set; }
        public string CrateSpawnNotification { get; set; }
        public string CrateStartNotification { get; set; }
        public string CrateEndNotification { get; set; }
        public int CountdownTime { get; set; }
        public int ZombieSpawnInterval { get; set; }
        public int ZombieSpawnMin { get; set; }
        public int ZombieSpawnMax { get; set; }
        public int ZombieSpawningEnabled { get; set; }
        public int UseBasicMarkers { get; set; }
        public int UseMarkersDuringCountdownOnly { get; set; }
        public BindingList<string> ZombieClasses { get; set; }
        public BindingList<Cratelocation> CrateLocations { get; set; }

        public void SetVectors()
        {
            foreach(Cratelocation cl in CrateLocations)
            {
                cl._Position = new Vec3(cl.Position);
                cl._Rotation = new Vec3(cl.Rotation);
            }
        }
        public void GetVectorstring()
        {
            foreach (Cratelocation cl in CrateLocations)
            {
                cl.Position = cl._Position.GetString();
                cl.Rotation = cl._Rotation.GetString();
            }
        }
    }

    public class Cratelocation
    {
        public string Position { get; set; }
        public string Rotation { get; set; }

        [JsonIgnore]
        public Vec3 _Position { get; set; }
        [JsonIgnore]
        public Vec3 _Rotation { get; set; }
    }

}
