using System;using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class RHLootBoxes
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty;

        public BindingList<Rhlootboxconfig> RHLootBoxConfigs { get; set; }
        public int StaticLootBoxEnabled { get; set; }
        public BindingList<Rhlootboxstaticbox> RHLootBoxStaticBoxes { get; set; }
    }

    public class Rhlootboxconfig
    {
        public string LootBoxName { get; set; }
        public BindingList<string> PossibleLootList { get; set; }


        public override string ToString()
        {
            return LootBoxName;
        }

    }

    public class Rhlootboxstaticbox
    {
        public string LootBoxSetName { get; set; }
        public BindingList<string> PossibleLootBoxNames { get; set; }
        public int NumberOfPositions { get; set; }
        public BindingList<Staticboxposition> StaticBoxPositions { get; set; }
        public int UseBoxLootList { get; set; }
        public BindingList<string> CustomLootList { get; set; }

        public override string ToString()
        {
            return LootBoxSetName;
        }
    }

    public class Staticboxposition
    {
        public float[] Position { get; set; }
        public float[] Rotation { get; set; }

        public override string ToString()
        {
            return Position[0].ToString() + "," + Position[1].ToString() + "," + Position[2].ToString();
        }
    }

}
