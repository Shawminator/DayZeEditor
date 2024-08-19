using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class CapareWorkBench
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty;

        public Repairing Repairing { get; set; }
        public BindingList<Workbenchrecipe> WorkbenchRecipes { get; set; }
    }

    public class Repairing
    {
        public bool AllowRepairFromRuined { get; set; }
        public int RuinedPunishmentMultiplier { get; set; }
        public int SewingKitUsage { get; set; }
        public int LeatherSewingKitUsage { get; set; }
        public int WeaponCleaningKitUsage { get; set; }
        public int WhetstoneUsage { get; set; }
        public int TireRepairKitUsage { get; set; }
        public int ElectronicRepairKitUsage { get; set; }
        public int EpoxyPuttyUsage { get; set; }
    }

    public class Workbenchrecipe
    {
        public string Recipe_Name { get; set; }
        public string ResultName { get; set; }
        public bool isQuantity { get; set; }
        public int ResultCount { get; set; }
        public string CraftType { get; set; }
        public string RecipeCategory { get; set; }
        public int CraftTimeSeconds { get; set; }
        public BindingList<Recipe_Ingredients> Recipe_Ingredients { get; set; }
        public BindingList<string> AttachmentsNeeded { get; set; }

        public override string ToString()
        {
            return Recipe_Name;
        }
    }

    public class Recipe_Ingredients
    {
        public string IngredientName { get; set; }
        public bool isQuantity { get; set; }
        public int IngredientAmount { get; set; }
        public bool DestroyAfterUse { get; set; }
        public decimal HealthDecrease { get; set; }

        public override string ToString()
        {
            return IngredientName;
        }
    }

}
