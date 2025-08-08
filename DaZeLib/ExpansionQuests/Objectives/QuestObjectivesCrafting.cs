using System.ComponentModel;

namespace DayZeLib
{
    public class QuestObjectivesCrafting : QuestObjectivesBase
    {
        public override int ConfigVersion { get; set; }
        public override int ID { get; set; }
        public override int ObjectiveType { get; set; }
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
