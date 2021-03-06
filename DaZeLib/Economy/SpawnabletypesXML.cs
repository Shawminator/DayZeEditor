using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DayZeLib
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class spawnabletypes
    {

        private spawnabletypesDamage damageField;

        private BindingList<spawnabletypesType> typeField;

        /// <remarks/>
        public spawnabletypesDamage damage
        {
            get
            {
                return this.damageField;
            }
            set
            {
                this.damageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("type")]
        public BindingList<spawnabletypesType> type
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
    public partial class spawnabletypesDamage
    {

        private decimal minField;

        private decimal maxField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal min
        {
            get
            {
                return this.minField;
            }
            set
            {
                this.minField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal max
        {
            get
            {
                return this.maxField;
            }
            set
            {
                this.maxField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class spawnabletypesType
    {

        private BindingList<object> itemsField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("attachments", typeof(spawnabletypesTypeAttachments))]
        [System.Xml.Serialization.XmlElementAttribute("cargo", typeof(spawnabletypesTypeCargo))]
        [System.Xml.Serialization.XmlElementAttribute("hoarder", typeof(spawnabletypesTypeHoarder))]
        [System.Xml.Serialization.XmlElementAttribute("tag", typeof(spawnabletypesTypeTag))]
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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        public override string ToString()
        {
            return nameField;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class spawnabletypesTypeAttachments
    {

        private BindingList<spawnabletypesTypeAttachmentsItem> itemField;

        private decimal chanceField;

        private bool chanceFieldSpecified;

        private string presetField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("item")]
        public BindingList<spawnabletypesTypeAttachmentsItem> item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal chance
        {
            get
            {
                return this.chanceField;
            }
            set
            {
                this.chanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool chanceSpecified
        {
            get
            {
                return this.chanceFieldSpecified;
            }
            set
            {
                this.chanceFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string preset
        {
            get
            {
                return this.presetField;
            }
            set
            {
                this.presetField = value;
            }
        }

        public override string ToString()
        {
            return "Attachments";
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class spawnabletypesTypeAttachmentsItem
    {

        private string nameField;

        private decimal chanceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal chance
        {
            get
            {
                return this.chanceField;
            }
            set
            {
                this.chanceField = value;
            }
        }

        public override string ToString()
        {
            return name;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class spawnabletypesTypeCargo
    {

        private BindingList<spawnabletypesTypeCargoItem> itemField;

        private string presetField;

        private decimal chanceField;

        private bool chanceFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("item")]
        public BindingList<spawnabletypesTypeCargoItem> item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string preset
        {
            get
            {
                return this.presetField;
            }
            set
            {
                this.presetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal chance
        {
            get
            {
                return this.chanceField;
            }
            set
            {
                this.chanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool chanceSpecified
        {
            get
            {
                return this.chanceFieldSpecified;
            }
            set
            {
                this.chanceFieldSpecified = value;
            }
        }

        public override string ToString()
        {
            return "Cargo";
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class spawnabletypesTypeCargoItem
    {

        private string nameField;

        private decimal chanceField;

        private bool chanceFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal chance
        {
            get
            {
                return this.chanceField;
            }
            set
            {
                this.chanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool chanceSpecified
        {
            get
            {
                return this.chanceFieldSpecified;
            }
            set
            {
                this.chanceFieldSpecified = value;
            }
        }

        public override string ToString()
        {
            return name;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class spawnabletypesTypeTag
    {

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        public override string ToString()
        {
            return "Tag";
        }
    }
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class spawnabletypesTypeHoarder
    {

        public override string ToString()
        {
            return "Hoarder";
        }
    }
}
