using System.ComponentModel;

namespace DayZeLib
{
    public class QuestObjectivesTreasureHunt: QuestObjectivesBase
    {
        public string ObjectiveText { get; set; }
        public int TimeLimit { get; set; }
        public int Active { get; set; }
        public int ShowDistance { get; set; }
        public string ContainerName { get; set; }
        public int DigInStash { get; set; }
        public string MarkerName { get; set; }
        public int MarkerVisibility { get; set; }
        public BindingList<decimal[]> Positions { get; set; }
        public BindingList<ExpansionLoot> Loot { get; set; }
        public int LootItemsAmount { get; set; }
        public decimal MaxDistance { get; set; }

        public override string ToString()
        {
            return ObjectiveText;
        }
    }
}
