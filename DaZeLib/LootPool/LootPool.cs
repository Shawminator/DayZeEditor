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
        public BindingList<RHLPdefinedItems> RHLPdefinedItems { get; set; }
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

    public class RHLPdefinedItems
    {
        public string DefineName { get; set; }
        public string Item { get; set; }
        public int SpawnExact { get; set; }
        public BindingList<string> magazines { get; set; }
        public BindingList<RHLPAttachment> attachments { get; set; }

        public override string ToString()
        {
            return DefineName;
        }
    }
    public class RHLPAttachment
    {
        public BindingList<string> attachments { get; set; }

        public override string ToString()
        {
            return "Attachemnts List";
        }
    }
}
