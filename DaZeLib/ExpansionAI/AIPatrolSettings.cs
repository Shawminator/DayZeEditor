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
        const int CurrentVersion = 6;
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public int m_Version { get; set; }
        public int Enabled { get; set; }
        public decimal RespawnTime { get; set; }
        public decimal MinDistRadius { get; set; }
        public decimal MaxDistRadius { get; set; }
        public decimal DespawnRadius { get; set; }
        public BindingList<ObjectPatrols> ObjectPatrols { get; set; }
        public BindingList<Patrols> Patrols { get; set; }


        public AIPatrolSettings()
        {
            m_Version = CurrentVersion;
            ObjectPatrols = new BindingList<ObjectPatrols>();
            Patrols = new BindingList<Patrols>();
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
            for (int i =0; i < Patrols.Count; i++)
            {
                Patrols[i].Name = "Patrol " + i.ToString();
            }
            for (int j = 0; j < ObjectPatrols.Count; j++)
            {
                ObjectPatrols[j].Name = "Object Patrol " + j.ToString();
            }
        }
    }

    public class ObjectPatrols
    {
        [JsonIgnore]
        public string Name { get; set; }

        public string Faction { get; set; }
        public string LoadoutFile { get; set; }
        public int NumberOfAI { get; set; }
        public string Behaviour { get; set; }
        public string Speed { get; set; }
        public string UnderThreatSpeed { get; set; }
        public int CanBeLooted { get; set; }
        public int UnlimitedReload { get; set; }
        public decimal MinDistRadius { get; set; }
        public decimal MaxDistRadius { get; set; }
        public decimal DespawnRadius { get; set; }
        public decimal MinSpreadRadius { get; set; }
        public decimal MaxSpreadRadius { get; set; }
        public decimal Chance { get; set; }
        public string ClassName { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Patrols
    {
        [JsonIgnore]
        public string Name { get; set; }

        public string Faction { get; set; }
        public string LoadoutFile { get; set; }
        public int NumberOfAI { get; set; }
        public string Behaviour { get; set; }
        public string Speed { get; set; }
        public string UnderThreatSpeed { get; set; }
        public int CanBeLooted { get; set; }
        public int UnlimitedReload { get; set; }
        public decimal MinDistRadius { get; set; }
        public decimal MaxDistRadius { get; set; }
        public decimal DespawnRadius { get; set;  }
        public decimal MinSpreadRadius { get; set; }
        public decimal MaxSpreadRadius { get; set; }
        public decimal Chance { get; set; }
        public decimal RespawnTime { get; set; }
        public BindingList<float[]> Waypoints { get; set; }

        public Patrols()
        {
            Waypoints = new BindingList<float[]>();
        }
        public override string ToString()
        {
            return Name;
        }
    }

}
