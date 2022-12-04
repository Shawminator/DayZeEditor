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
        public string NPCClassName { get; set; }
        public string NPCSpeed { get; set; }
        public string NPCMode { get; set; }
        public string NPCFaction { get; set; }
        public string NPCLoadoutFile { get; set; }

        public AIVIP(){ }
    }

}
