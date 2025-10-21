using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace DayZeLib
{
    public class AILoadoutsConfig
    {
        public BindingList<AILoadouts> LoadoutsData { get; set; }
        public BindingList<AILootDrops> AILootDropsData { get; set; }
    }
    public class AILootDrops
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }
        [JsonIgnore]
        public string Name { get; set; }

        public AILootDrops()
        {
            LootdropList = new BindingList<AILoadouts>();
        }
        public AILootDrops(string name)
        {
            name = name;
            LootdropList = new BindingList<AILoadouts>();
        }
        public BindingList<AILoadouts> LootdropList { get; set; }
        public void Setname()
        {
            Name = Path.GetFileNameWithoutExtension(Filename);
        }
        public override string ToString()
        {
            if (Name == "")
                return Name;

            return Name;
        }
    }

    public class AILoadouts
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }
        [JsonIgnore]
        public string Name { get; set; }

        public AILoadouts()
        {
            ClassName = "";
            Chance = (decimal)1;
            Quantity = new Quantity();
            Health = new BindingList<Health>();
            InventoryAttachments = new BindingList<Inventoryattachment>();
            InventoryCargo = new BindingList<AILoadouts>();
            ConstructionPartsBuilt = new BindingList<object>();
            Sets = new BindingList<AILoadouts>();
        }
        public AILoadouts(string name)
        {
            name = name;
            ClassName = "";
            Chance = (decimal)1;
            Quantity = new Quantity();
            Health = new BindingList<Health>();
            InventoryAttachments = new BindingList<Inventoryattachment>();
            InventoryCargo = new BindingList<AILoadouts>();
            ConstructionPartsBuilt = new BindingList<object>();
            Sets = new BindingList<AILoadouts>();
        }

        public string ClassName { get; set; }
        public decimal Chance { get; set; }
        public Quantity Quantity { get; set; }
        public BindingList<Health> Health { get; set; }
        public BindingList<Inventoryattachment> InventoryAttachments { get; set; }
        public BindingList<AILoadouts> InventoryCargo { get; set; }
        public BindingList<object> ConstructionPartsBuilt { get; set; }
        public BindingList<AILoadouts> Sets { get; set; }

        public void Setname()
        {
            Name = Path.GetFileNameWithoutExtension(Filename);
        }
        public override string ToString()
        {
            if (ClassName == "")
                return Name;

            return ClassName;
        }
    }

    public class Quantity
    {
        public decimal Min { get; set; }
        public decimal Max { get; set; }
    }

    public class Health
    {
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public string Zone { get; set; }

        public override string ToString()
        {
            if (Zone == "")
                return "No Zone";
            return Zone;
        }

    }

    public class Inventoryattachment
    {
        public string SlotName { get; set; }
        public BindingList<AILoadouts> Items { get; set; }

        public override string ToString()
        {
            return SlotName; ;
        }
    }
}
