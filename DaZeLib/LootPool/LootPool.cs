using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZeLib
{
    class LootPool
    {
        public BindingList<Rhlprewardtable> RHLPRewardTables { get; set; }
        public BindingList<Rhlploottable> RHLPLootTables { get; set; }
        public BindingList<Rhlpdefinedweapon> RHLPdefinedWeapons { get; set; }
        public int NumberOfExtraMagsForDefinedWeapons { get; set; }
    }

    public class Rhlprewardtable
    {
        public string RewardName { get; set; }
        public BindingList<string> Rewards { get; set; }
    }

    public class Rhlploottable
    {
        public string TableName { get; set; }
        public BindingList<string> LootItems { get; set; }
    }

    public class Rhlpdefinedweapon
    {
        public string DefineName { get; set; }
        public string weapon { get; set; }
        public string magazine { get; set; }
        public BindingList<string> attachments { get; set; }
        public string optic { get; set; }
    }

}
