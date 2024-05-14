using System.ComponentModel;

namespace DayZeLib
{
    public class QuestObjectivesTarget : QuestObjectivesBase
    {
        public string ObjectiveText { get; set; }
        public int TimeLimit { get; set; }
        public int Active { get; set; }
        public decimal[] Position { get; set; }
        public decimal MaxDistance { get; set; }
        public decimal MinDistance { get; set; }
        public int Amount { get; set; }
        public BindingList<string> ClassNames { get; set; }
        public int CountSelfKill { get; set; }
        public BindingList<string> AllowedWeapons { get; set; }
        public BindingList<string> ExcludedClassNames { get; set; }
        public int CountAIPlayers { get; set; }
        public BindingList<string> AllowedTargetFactions { get; set; }
        public BindingList<string> AllowedDamageZones { get; set; }

        public QuestObjectivesTarget()
        {

        }
        public override string ToString()
        {
            return ObjectiveText;
        }
    }
}
