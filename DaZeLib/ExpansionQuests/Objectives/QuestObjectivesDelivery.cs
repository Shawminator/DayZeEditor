using System.ComponentModel;

namespace DayZeLib
{
    public class QuestObjectivesDelivery : QuestObjectivesBase
    {
        public override int ConfigVersion { get; set; }
        public override int ID { get; set; }
        public override int ObjectiveType { get; set; }
        public string ObjectiveText { get; set; }
        public int TimeLimit { get; set; }
        public BindingList<Delivery> Collections { get; set; }
        public decimal MaxDistance { get; set; }
        public string MarkerName { get; set; }
        public int ShowDistance { get; set; }
        public int AddItemsToNearbyMarketZone { get; set; }
        public int Active { get; set; }

        public QuestObjectivesDelivery()
        {
            Collections = new BindingList<Delivery>();
        }
        public override string ToString()
        {
            return ObjectiveText;
        }
    }

    public class Delivery
    {
        public int Amount { get; set; }
        public string ClassName { get; set; }
        public int QuantityPercent { get; set; }
        public int MinQuantityPercent { get; set; }

        public Delivery() { }
        public override string ToString()
        {
            return ClassName;
        }
    }

}
