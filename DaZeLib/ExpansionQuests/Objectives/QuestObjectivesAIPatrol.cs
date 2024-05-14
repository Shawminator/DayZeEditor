using System.ComponentModel;

namespace DayZeLib
{
    public class QuestObjectivesAIPatrol : QuestObjectivesBase
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
        public override void SetVec3List()
        {
            AISpawn.SetVec3List();
        }
        public override void GetVec3List()
        {
            AISpawn.GetVec3List();
        }
    }
}
