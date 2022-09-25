using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class LootPool
    {
        public BindingList<Rhlprewardtable> RHLPRewardTables { get; set; }
        public BindingList<Rhlploottable> RHLPLootTables { get; set; }
        public BindingList<Rhlpdefinedweapon> RHLPdefinedWeapons { get; set; }
        public int NumberOfExtraMagsForDefinedWeapons { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty;
    }

    public class Rhlprewardtable
    {
        public string RewardName { get; set; }
        public BindingList<string> Rewards { get; set; }

        public override string ToString()
        {
            return RewardName;
        }
    }

    public class Rhlploottable
    {
        public string TableName { get; set; }
        public BindingList<string> LootItems { get; set; }

        public override string ToString()
        {
            return TableName;
        }
    }

    public class Rhlpdefinedweapon
    {
        public string DefineName { get; set; }
        public string weapon { get; set; }
        public string magazine { get; set; }
        public BindingList<string> attachments { get; set; }
        public string optic { get; set; }

        public override string ToString()
        {
            return DefineName;
        }
    }

}
