using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class QuestObjectivesAction: QuestObjectivesBase
    {
        public BindingList<string> ActionNames { get; set; }

        public QuestObjectivesAction() { }
        public override string ToString()
        {
            return ObjectiveText;
        }
    }

}
