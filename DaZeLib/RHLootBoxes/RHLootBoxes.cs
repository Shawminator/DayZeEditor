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

        public int LootBoxLifetime { get; set; }
        public BindingList<caparelootboxconfig> CapareLootBoxConfigs { get; set; }
        public int StaticLootBoxEnabled { get; set; }
        public BindingList<caparelootboxstaticbox> CapareLootBoxStaticBoxes { get; set; }
    }

    public class caparelootboxconfig
    {
        public string LootBoxName { get; set; }
        public BindingList<string> PossibleLootList { get; set; }


        public override string ToString()
        {
            return LootBoxName;
        }

    }

    public class caparelootboxstaticbox
    {
        public string LootBoxSetName { get; set; }
        public BindingList<string> PossibleLootBoxNames { get; set; }
        public int NumberOfPositions { get; set; }
        public BindingList<Staticboxposition> StaticBoxPositions { get; set; }
        public int UseCustomLootList { get; set; }
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
