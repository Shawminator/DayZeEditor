using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DayZeLib
{
    [XmlRoot("Recipes")]
    public class TerjeRecipes
    {
        [XmlIgnore]
        public string Filename { get; set; }
        [XmlIgnore]
        public bool isDirty { get; set; }


        private BindingList<object> itemsField;

        [XmlElement("Recipe", typeof(TerjeRecipe))]
        public BindingList<object> Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }

    public class TerjeRecipe
    {
        [XmlAttribute("displayName")]
        public string DisplayName { get; set; }

        [XmlAttribute("enabled")]
        public int Enabled { get; set; }

        [XmlAttribute("time")]
        public float Time { get; set; }

        [XmlIgnore]
        public bool TimeSpecified { get; set; }

        public TerjeRecipeIngredient FirstIngredient { get; set; }
        public TerjeRecipeIngredient SecondIngredient { get; set; }
        public TerjeCraftingResults CraftingResults { get; set; }

        private TerjeConditions ConditionsField;
        [System.Xml.Serialization.XmlElementAttribute("Conditions")]
        public TerjeConditions Conditions
        {
            get
            {
                return this.ConditionsField;
            }
            set
            {
                this.ConditionsField = value;
            }
        }
    }
    public class TerjeRecipeIngredient
    {
        [XmlAttribute("singleUse")]
        public int SingleUse { get; set; }

        [XmlIgnore]
        public bool SingleUseSpecified { get; set; }

        [XmlAttribute("minQuantity")]
        public float MinQuantity { get; set; }

        [XmlIgnore]
        public bool MinQuantitySpecified { get; set; }

        [XmlAttribute("maxQuantity")]
        public float MaxQuantity { get; set; }

        [XmlIgnore]
        public bool MaxQuantitySpecified { get; set; }

        [XmlAttribute("minDamage")]
        public int MinDamage { get; set; }

        [XmlIgnore]
        public bool MinDamageSpecified { get; set; }

        [XmlAttribute("maxDamage")]
        public int MaxDamage { get; set; }

        [XmlIgnore]
        public bool MaxDamageSpecified { get; set; }

        [XmlAttribute("addHealth")]
        public float AddHealth { get; set; }

        [XmlIgnore]
        public bool AddHealthSpecified { get; set; }

        [XmlAttribute("setHealth")]
        public float SetHealth { get; set; }

        [XmlIgnore]
        public bool SetHealthSpecified { get; set; }

        [XmlAttribute("addQuantity")]
        public float AddQuantity { get; set; }

        [XmlIgnore]
        public bool AddQuantitySpecified { get; set; }

        [XmlElement("Item")]
        public List<string> Items { get; set; } = new List<string>();
    }

    public class TerjeCraftingResults
    {
        [XmlElement("Result")]
        public List<TerjeCraftingResult> Results { get; set; } = new List<TerjeCraftingResult>();
    }

    public class TerjeCraftingResult
    {
        [XmlText]
        public string ClassName { get; set; }

        [XmlAttribute("setFullQuantity")]
        public int SetFullQuantity { get; set; }

        [XmlIgnore]
        public bool SetFullQuantitySpecified { get; set; }

        [XmlAttribute("setQuantity")]
        public float SetQuantity { get; set; }

        [XmlIgnore]
        public bool SetQuantitySpecified { get; set; }

        [XmlAttribute("setHealth")]
        public float SetHealth { get; set; }

        [XmlIgnore]
        public bool SetHealthSpecified { get; set; }

        [XmlAttribute("inheritsHealth")]
        public int InheritsHealth { get; set; }

        [XmlIgnore]
        public bool InheritsHealthSpecified { get; set; }

        [XmlAttribute("inheritsColor")]
        public int InheritsColor { get; set; }

        [XmlIgnore]
        public bool InheritsColorSpecified { get; set; }

        [XmlAttribute("spawnMode")]
        public int SpawnMode { get; set; }

        [XmlIgnore]
        public bool SpawnModeSpecified { get; set; }
    }
}
