using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class QuestObjectivesTravel: QuestObjectivesBase
    {
        public string ObjectiveText { get; set; }
        public int TimeLimit { get; set; }
        public decimal[] Position { get; set; }
        public decimal MaxDistance { get; set; }
        public string MarkerName { get; set; }
        public int ShowDistance { get; set; }
        public int TriggerOnEnter { get; set; }
        public int TriggerOnExit { get; set; }

        public QuestObjectivesTravel() { }
        public override string ToString()
        {
            return ObjectiveText;
        }

    }

}
