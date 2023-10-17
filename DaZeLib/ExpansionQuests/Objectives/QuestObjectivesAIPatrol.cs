using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class QuestObjectivesAIPatrol: QuestObjectivesBase
    {
        public string ObjectiveText { get; set; }
        public int TimeLimit { get; set; }
        public AIPatrol AIPatrol { get; set; }
        public decimal MinDistRadius { get; set; }
        public decimal MaxDistRadius { get; set; }
        public decimal DespawnRadius { get; set; }
        public int CanLootAI { get; set; }

        public QuestObjectivesAIPatrol()
        {
            AIPatrol = new AIPatrol();
        }
        public override string ToString()
        {
            return ObjectiveText;
        }
    }

    public class AIPatrol
    {
        public int NPCUnits { get; set; }
        public BindingList<decimal[]> Waypoints { get; set; }
        public string NPCSpeed { get; set; }
        public string NPCMode { get; set; }
        public string NPCFaction { get; set; }
        public string NPCFormation { get; set; }
        public string NPCLoadoutFile { get; set; }
        public decimal NPCAccuracyMin { get; set; }
        public decimal NPCAccuracyMax { get; set; }
        public BindingList<string> ClassNames { get; set; }
        public int SpecialWeapon { get; set; }
        public BindingList<string> AllowedWeapons { get; set; }

        public AIPatrol()
        {
            Waypoints = new BindingList<decimal[]>();
            ClassNames = new BindingList<string>();
            AllowedWeapons = new BindingList<string>();
        }
        public override string ToString()
        {
            return NPCFaction;
        }
    }

}
