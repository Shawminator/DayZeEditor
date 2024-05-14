using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class QuestObjectivesTreasureHunt : QuestObjectivesBase
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

        [JsonIgnore]
        public BindingList<Vec3> _Positions { get; set; }

        public override void SetVec3List()
        {
            Positions = new BindingList<decimal[]>();
            foreach (Vec3 v3 in _Positions)
            {
                Positions.Add(v3.getDecimalArray());
            }
        }
        public override void GetVec3List()
        {
            _Positions = new BindingList<Vec3>();
            foreach (decimal[] array in Positions)
            {
                _Positions.Add(new Vec3(array));
            }
        }
        public override string ToString()
        {
            return ObjectiveText;
        }
    }
}
