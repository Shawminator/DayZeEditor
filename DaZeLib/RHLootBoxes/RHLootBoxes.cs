using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZeLib
{
    class RHLootBoxes
    {
        public BindingList<Rhlootboxconfig> RHLootBoxConfigs { get; set; }
        public int StaticLootBoxEnabled { get; set; }
        public BindingList<Rhlootboxstaticbox> RHLootBoxStaticBoxes { get; set; }
    }

    public class Rhlootboxconfig
    {
        public string LootBoxName { get; set; }
        public BindingList<string> PossibleLootList { get; set; }
    }

    public class Rhlootboxstaticbox
    {
        public BindingList<string> PossibleLootBoxNames { get; set; }
        public int NumberOfPositions { get; set; }
        public BindingList<Staticboxposition> StaticBoxPositions { get; set; }
        public BindingList<string> PossibleLootList { get; set; }
    }

    public class Staticboxposition
    {
        public float[] Position { get; set; }
        public float[] Rotation { get; set; }
    }

}
