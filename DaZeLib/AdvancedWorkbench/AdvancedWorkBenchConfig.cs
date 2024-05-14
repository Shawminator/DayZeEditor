using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class AdvancedWorkBenchConfig
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty;

        public BindingList<Craftitem> CraftItems { get; set; }
    }

    public class Craftitem
    {
        public string Result { get; set; }
        public int ResultCount { get; set; }
        public string CraftType { get; set; }
        public string RecipeName { get; set; }
        public string RecipeCategory { get; set; }
        public BindingList<Craftcomponent> CraftComponents { get; set; }
        public BindingList<string> AttachmentsNeed { get; set; }

        public override string ToString()
        {
            return RecipeName;
        }
    }

    public class Craftcomponent
    {
        public string Classname { get; set; }
        public int Amount { get; set; }
        public int Destroy { get; set; }
        public decimal Changehealth { get; set; }

        public override string ToString()
        {
            return Classname;
        }
    }

}
