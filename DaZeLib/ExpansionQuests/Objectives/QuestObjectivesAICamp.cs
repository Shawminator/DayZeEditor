using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class QuestObjectivesAICamp: QuestObjectivesBase
    {
        public string ObjectiveText { get; set; }
        public int TimeLimit { get; set; }
        public Aicamp AICamp { get; set; }
        public decimal MinDistRadius { get; set; }
        public decimal MaxDistRadius { get; set; }
        public decimal DespawnRadius { get; set; }
        public int CanLootAI { get; set; }
        public int InfectedDeletionRadius { get; set; }

        public QuestObjectivesAICamp() 
        {
            AICamp = new Aicamp();
        }
        public override string ToString()
        {
            return ObjectiveText;
        }
    }

    public class Aicamp
    {
        public BindingList<float[]> Positions { get; set; }
        public string NPCSpeed { get; set; }
        public string NPCMode { get; set; }
        public string NPCFaction { get; set; }
        public string NPCLoadoutFile { get; set; }
        public decimal NPCAccuracyMin { get; set; }
        public decimal NPCAccuracyMax { get; set; }
        public BindingList<string> ClassNames { get; set; }
        public int SpecialWeapon { get; set; }
        public BindingList<string> AllowedWeapons { get; set; }

        public Aicamp()
        {
            Positions = new BindingList<float[]>();
            ClassNames = new BindingList<string>();
            AllowedWeapons = new BindingList<string>();
        }
        public override string ToString()
        {
            return NPCFaction;
        }
    }

}
