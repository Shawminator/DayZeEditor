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
        public float[] Position { get; set; }
        public decimal MaxDistance { get; set; }
        public string MarkerName { get; set; }
        public int ShowDistance { get; set; }

        public QuestObjectivesTravel() { }
        public override string ToString()
        {
            return ObjectiveText;
        }

    }

}
