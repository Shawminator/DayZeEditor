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
        public float[] Position { get; set; }
        public decimal MaxDistance { get; set; }
        public AIVIP AIVIP { get; set; }
        public string MarkerName { get; set; }
        public int ShowDistance { get; set; }

        public QuestObjectivesAIVIP()
        {
            AIVIP = new AIVIP();
        }
        public override string ToString()
        {
            return ObjectiveText;
        }
    }

    public class AIVIP
    {
        public string NPCLoadoutFile { get; set; }

        public AIVIP(){ }
    }

}
