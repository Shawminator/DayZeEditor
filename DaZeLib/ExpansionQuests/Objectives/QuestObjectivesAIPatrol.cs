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
        public int Active { get; set; }
        public ExpansionQuestAISpawn AISpawn { get; set; }
        public decimal MaxDistance { get; set; }
        public decimal MinDistance { get; set; }
        public BindingList<string> AllowedWeapons { get; set; }
        public BindingList<string> AllowedDamageZones { get; set; }

        public QuestObjectivesAIPatrol()
        {
           
        }
        public override string ToString()
        {
            return ObjectiveText;
        }
    }
}
