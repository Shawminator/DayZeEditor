using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZeLib
{
    public class QuestObjectivesTreasureHunt: QuestObjectivesBase
    {
        public string ObjectiveText { get; set; }
        public int TimeLimit { get; set; }
        public int ShowDistance { get; set; }
        public string ContainerName { get; set; }
        public int DigInStash { get; set; }
        public string MarkerName { get; set; }
        public int MarkerVisibility { get; set; }
        public BindingList<float[]> Positions { get; set; }
        public BindingList<TreasureHuntItems> Loot { get; set; }
        public int LootitemsAmount { get; set; }
        public decimal MaxDistance { get; set; }

        public override string ToString()
        {
            return ObjectiveText;
        }
    }

    public class TreasureHuntItems
    {
        public string ClassName { get; set; }
        public BindingList<string> Attachments { get; set; }
        public decimal Chance { get; set; }
        public int QuantityPercent { get; set; }
        public int Max { get; set; }
        public BindingList<TreasureHuntItems> Variants { get; set; }

        public TreasureHuntItems() { }
        public override string ToString()
        {
            return ClassName;
        }
    }
}
