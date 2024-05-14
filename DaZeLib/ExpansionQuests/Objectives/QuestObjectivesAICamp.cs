using System.ComponentModel;

namespace DayZeLib
{
    public class QuestObjectivesAICamp : QuestObjectivesBase
    {
        public string ObjectiveText { get; set; }
        public int TimeLimit { get; set; }
        public int Active { get; set; }
        public decimal InfectedDeletionRadius { get; set; }
        public BindingList<ExpansionQuestAISpawn> AISpawns { get; set; }
        public decimal MaxDistance { get; set; }
        public decimal MinDistance { get; set; }
        public BindingList<string> AllowedWeapons { get; set; }
        public BindingList<string> AllowedDamageZones { get; set; }

        public override void SetVec3List()
        {
            foreach (ExpansionQuestAISpawn aispawn in AISpawns)
            {
                aispawn.SetVec3List();
            }
        }
        public override void GetVec3List()
        {
            foreach (ExpansionQuestAISpawn aispawn in AISpawns)
            {
                aispawn.GetVec3List();
            }
        }

        public QuestObjectivesAICamp()
        {
            AISpawns = new BindingList<ExpansionQuestAISpawn>();
        }
        public override string ToString()
        {
            return ObjectiveText;
        }
    }



}
