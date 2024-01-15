using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class ExpansionQuestAISpawn
    {
        public int NumberOfAI { get; set; }
        public string NPCName { get; set; }
        public BindingList<decimal[]> Waypoints { get; set; }
        public int Behaviour { get; set; }
        public string Formation { get; set; }
        public string Loadout { get; set; }
        public string Faction { get; set; }
        public decimal Speed { get; set; }
        public decimal ThreatSpeed { get; set; }
        public decimal MinAccuracy { get; set; }
        public decimal MaxAccuracy { get; set; }
        public int CanBeLooted { get; set; }
        public int UnlimitedReload { get; set; }
        public decimal ThreatDistanceLimit { get; set; }
        public decimal DamageMultiplier { get; set; }
        public decimal DamageReceivedMultiplier { get; set; }
        public BindingList<string> ClassNames { get; set; }
	    public decimal SniperProneDistanceThreshold { get; set; }

        public decimal RespawnTime { get; set; }
        public decimal DespawnTime { get; set; }
        public decimal MinDistanceRadius { get; set; }
        public decimal MaxDistanceRadius { get; set; }
        public decimal DespawnRadius { get; set; }

        public ExpansionQuestAISpawn()
        {
            Waypoints = new BindingList<decimal[]>();
            ClassNames = new BindingList<string>();
        }

        public override string ToString()
        {
            return NPCName;
        }
    }
}
