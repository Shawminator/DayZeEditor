using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{

    public class AIPatrolSettings
    {
        [JsonIgnore]
        const int CurrentVersion = 1;
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public int m_Version { get; set; }
        public int Enabled { get; set; }
        public decimal RespawnTime { get; set; }
        public decimal MinDistRadius { get; set; }
        public decimal MaxDistRadius { get; set; }
        public BindingList<Eventcrashpatrol> EventCrashPatrol { get; set; }
        public BindingList<Patrol> Patrol { get; set; }


        public AIPatrolSettings()
        {
            m_Version = CurrentVersion;
            EventCrashPatrol = new BindingList<Eventcrashpatrol>();
            Patrol = new BindingList<Patrol>();
        }
        public bool checkver()
        {
            if (m_Version != CurrentVersion)
            {
                m_Version = CurrentVersion;
                isDirty = true;
                return true;
            }
            return false;
        }
        public void SetPatrolNames()
        {
            for (int i =0; i < Patrol.Count; i++)
            {
                Patrol[i].Name = "Patrol " + i.ToString();
            }
            for (int j = 0; j < EventCrashPatrol.Count; j++)
            {
                EventCrashPatrol[j].Name = "Event Crash Patrol " + j.ToString();
            }
        }
    }

    public class Eventcrashpatrol
    {
        [JsonIgnore]
        public string Name { get; set; }

        public string Faction { get; set; }
        public string EventName { get; set; }
        public string LoadoutFile { get; set; }
        public int NumberOfAI { get; set; }
        public string Behaviour { get; set; }
        public string Speed { get; set; }
        public string UnderThreatSpeed { get; set; }
        public decimal MinDistRadius { get; set; }
        public decimal MaxDistRadius { get; set; }
        public int CanBeLooted { get; set; }
        public int UnlimitedReload { get; set; }
        public decimal Chance { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Patrol
    {
        [JsonIgnore]
        public string Name { get; set; }

        public string Faction { get; set; }
        public string LoadoutFile { get; set; }
        public int NumberOfAI { get; set; }
        public string Behaviour { get; set; }
        public string Speed { get; set; }
        public string UnderThreatSpeed { get; set; }
        public decimal RespawnTime { get; set; }
        public decimal MinDistRadius { get; set; }
        public decimal MaxDistRadius { get; set; }
        public decimal WaypointsSpreadRadius { get; set; }
        public float[] StartPos { get; set; }
        public BindingList<float[]> Waypoints { get; set; }
        public int CanBeLooted { get; set; }
        public int UnlimitedReload { get; set; }
        public decimal Chance { get; set; }

        public Patrol()
        {
            StartPos = new float[] { 0, 0, 0 };
            Waypoints = new BindingList<float[]>();
        }
        public override string ToString()
        {
            return Name;
        }
    }

}
