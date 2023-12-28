using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class CapareLootPool
    {
        public BindingList<caparelprewardtable> CapareLPRewardTables { get; set; }
        public BindingList<caparelploottable> CapareLPLootTables { get; set; }
        public BindingList<capareLPdefinedItems> CapareLPdefinedItems { get; set; }
        public int NumberOfExtraMagsForDefinedWeapons { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty;
    }

    public class caparelprewardtable
    {
        public string RewardName { get; set; }
        public BindingList<string> Rewards { get; set; }

        public override string ToString()
        {
            return RewardName;
        }
    }

    public class caparelploottable
    {
        public string TableName { get; set; }
        public BindingList<string> LootItems { get; set; }

        public override string ToString()
        {
            return TableName;
        }
    }

    public class capareLPdefinedItems
    {
        public string DefineName { get; set; }
        public string Item { get; set; }
        public int SpawnExact { get; set; }
        public BindingList<string> magazines { get; set; }
        public BindingList<capareLPAttachment> attachments { get; set; }

        public override string ToString()
        {
            return DefineName;
        }
    }
    public class capareLPAttachment
    {
        public BindingList<string> attachments { get; set; }

        public override string ToString()
        {
            return "Attachemnts List";
        }
    }
}
