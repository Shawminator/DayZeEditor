using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class QuestObjectivesCrafting : QuestObjectivesBase
    {
        public string ObjectiveText { get; set; }
        public int TimeLimit { get; set; }
        public BindingList<string> ItemNames { get; set; }
        public int ExecutionAmount { get; set; }
        public int Active { get; set; }

        public override string ToString()
        {
            return ObjectiveText;
        }
    }
}
