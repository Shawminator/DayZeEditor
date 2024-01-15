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
        public int Active { get; set; }
        public decimal InfectedDeletionRadius { get; set; }
        public BindingList<ExpansionQuestAISpawn> AISpawns { get; set; }
        public decimal MaxDistance { get; set; }
        public decimal MinDistance { get; set; }
        public BindingList<string> AllowedWeapons { get; set; }
        public BindingList<string> AllowedDamageZones { get; set; }


        public QuestObjectivesAICamp() 
        {
            AISpawns = new BindingList<ExpansionQuestAISpawn>();
        }
        public override string ToString()
        {
            return ObjectiveText;
        }
    }

   

}
