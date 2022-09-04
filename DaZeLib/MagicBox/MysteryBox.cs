using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class MysteryBox
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty;


        public string MagicCrateExchangeType { get; set; }
        public int MagicCratePrice { get; set; }
        public int CanCrateChangeLocation { get; set; }
        public int MinimumRollBeforeFailChance { get; set; }
        public int BoxFailChance { get; set; }
        public BindingList<Possibleboxposition> PossibleBoxPositions { get; set; }
        public BindingList<Possibleboxitem> PossibleBoxItems { get; set; }

        public void SetBoxNames()
        {
            int i = 0;
            foreach(Possibleboxposition pbp in PossibleBoxPositions)
            {
                pbp.BoxName = "possible Box Position " + i;
                i++;
            }
        }
        public string GetNextName()
        {
            return "possible Box Position " + PossibleBoxPositions.Count().ToString();
        }
    }

    public class Possibleboxposition
    {
        public float[] Position { get; set; }
        public float[] Orientation { get; set; }
        [JsonIgnore]
        public string BoxName { get; set; }
        public override string ToString()
        {
            return BoxName;
        }
    }

    public class Possibleboxitem
    {
        public string Item { get; set; }
        public BindingList<string> Attachments { get; set; }
        public string Mag { get; set; }

        public override string ToString()
        {
            return Item;
        }
    }

}
