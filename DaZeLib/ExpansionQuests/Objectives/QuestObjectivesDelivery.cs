using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class QuestObjectivesDelivery: QuestObjectivesBase
    {
        public string ObjectiveText { get; set; }
        public int TimeLimit { get; set; }
        public BindingList<Delivery> Collections { get; set; }
        public decimal MaxDistance { get; set; }
        public string MarkerName { get; set; }
        public int ShowDistance { get; set; }
        public int AddItemsToNearbyMarketZone { get; set; }

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

        public Delivery() { }
        public override string ToString()
        {
            return ClassName;
        }
    }

}
