using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVZ.DarkHorde.Zombies.ZombiesLoot
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class types
    {

        private typesType[] typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("type")]
        public typesType[] type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesType
    {

        private typesTypeItems_List_A items_List_AField;

        private typesTypeItems_List_B items_List_BField;

        private typesTypeItems_List_C items_List_CField;

        private typesTypeItems_List_D items_List_DField;

        private string zombie_Category_Class_NamesField;

        /// <remarks/>
        public typesTypeItems_List_A Items_List_A
        {
            get
            {
                return this.items_List_AField;
            }
            set
            {
                this.items_List_AField = value;
            }
        }

        /// <remarks/>
        public typesTypeItems_List_B Items_List_B
        {
            get
            {
                return this.items_List_BField;
            }
            set
            {
                this.items_List_BField = value;
            }
        }

        /// <remarks/>
        public typesTypeItems_List_C Items_List_C
        {
            get
            {
                return this.items_List_CField;
            }
            set
            {
                this.items_List_CField = value;
            }
        }

        /// <remarks/>
        public typesTypeItems_List_D Items_List_D
        {
            get
            {
                return this.items_List_DField;
            }
            set
            {
                this.items_List_DField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Zombie_Category_Class_Names
        {
            get
            {
                return this.zombie_Category_Class_NamesField;
            }
            set
            {
                this.zombie_Category_Class_NamesField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeItems_List_A
    {

        private decimal quantity_Or_Chance_To_SpawnField;

        private string stateField;

        private string listField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Quantity_Or_Chance_To_Spawn
        {
            get
            {
                return this.quantity_Or_Chance_To_SpawnField;
            }
            set
            {
                this.quantity_Or_Chance_To_SpawnField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string state
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string List
        {
            get
            {
                return this.listField;
            }
            set
            {
                this.listField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeItems_List_B
    {

        private decimal quantity_Or_Chance_To_SpawnField;

        private string stateField;

        private string listField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Quantity_Or_Chance_To_Spawn
        {
            get
            {
                return this.quantity_Or_Chance_To_SpawnField;
            }
            set
            {
                this.quantity_Or_Chance_To_SpawnField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string state
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string List
        {
            get
            {
                return this.listField;
            }
            set
            {
                this.listField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeItems_List_C
    {

        private decimal quantity_Or_Chance_To_SpawnField;

        private string stateField;

        private string listField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Quantity_Or_Chance_To_Spawn
        {
            get
            {
                return this.quantity_Or_Chance_To_SpawnField;
            }
            set
            {
                this.quantity_Or_Chance_To_SpawnField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string state
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string List
        {
            get
            {
                return this.listField;
            }
            set
            {
                this.listField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeItems_List_D
    {

        private decimal quantity_Or_Chance_To_SpawnField;

        private string stateField;

        private string listField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Quantity_Or_Chance_To_Spawn
        {
            get
            {
                return this.quantity_Or_Chance_To_SpawnField;
            }
            set
            {
                this.quantity_Or_Chance_To_SpawnField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string state
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string List
        {
            get
            {
                return this.listField;
            }
            set
            {
                this.listField = value;
            }
        }
    }


}
