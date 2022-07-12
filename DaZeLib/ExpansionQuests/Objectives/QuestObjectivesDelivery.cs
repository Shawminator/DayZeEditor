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
        public BindingList<Delivery> Deliveries { get; set; }
        public float[] Position { get; set; }
        public decimal MaxDistance { get; set; }
        public string MarkerName { get; set; }

        public QuestObjectivesDelivery()
        {
            Deliveries = new BindingList<Delivery>();
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
