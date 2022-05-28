using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class AILoadouts
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }
        [JsonIgnore]
        public string Name { get; set; }

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
            if(ClassName == "")
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
