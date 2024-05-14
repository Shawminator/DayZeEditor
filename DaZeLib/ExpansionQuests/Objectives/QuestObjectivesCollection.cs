using System.ComponentModel;

namespace DayZeLib
{
    public class QuestObjectivesCollection : QuestObjectivesBase
    {
        public string ObjectiveText { get; set; }
        public int TimeLimit { get; set; }
        public BindingList<Collections> Collections { get; set; }
        public decimal MaxDistance { get; set; }
        public string MarkerName { get; set; }
        public int ShowDistance { get; set; }
        public int AddItemsToNearbyMarketZone { get; set; }
        public int NeedAnyCollection { get; set; }
        public int Active { get; set; }

        public QuestObjectivesCollection()
        {
            Collections = new BindingList<Collections>();
        }
        public override string ToString()
        {
            return ObjectiveText;
        }
    }

    public class Collections
    {
        public int Amount { get; set; }
        public string ClassName { get; set; }
        public int QuantityPercent { get; set; }
        public int MinQuantityPercent { get; set; }

        public Collections()
        { }

        public override string ToString()
        {
            return ClassName;
        }
    }

}
