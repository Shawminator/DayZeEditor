using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class QuestObjectivesAIVIP: QuestObjectivesBase
    {
        public string ObjectiveText { get; set; }
        public int TimeLimit { get; set; }
        public decimal[] Position { get; set; }
        public decimal MaxDistance { get; set; }
        public string MarkerName { get; set; }
        public int ShowDistance { get; set; }
        public int CanLootAI { get; set; }
        public int Active { get; set; }
        public string NPCLoadoutFile { get; set; }
        public string NPCClassName { get; set; }
        public string NPCName { get; set; }

        public QuestObjectivesAIVIP()
        {
            
        }
        public override string ToString()
        {
            return ObjectiveText;
        }
    }

}
