using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DayZeLib
{
    //[System.SerializableAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    //[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    [XmlRoot("spawnabletypes")]
    public partial class SpawnableTypes
    {
        private spawnableTypeDamage damageField;
        private BindingList<SpawnableType> typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("damage")]
        public spawnableTypeDamage damage
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
        public BindingList<SpawnableType> type
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
    // <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SpawnableType
    {
        private BindingList<object> itemsField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("attachments", typeof(spawnableTypeAttachment))]
        [System.Xml.Serialization.XmlElementAttribute("cargo", typeof(spawnableTypeCargo))]
        [System.Xml.Serialization.XmlElementAttribute("damage", typeof(spawnableTypeDamage))]
        [System.Xml.Serialization.XmlElementAttribute("hoarder", typeof(spawnableTypesHoarder))]
        [System.Xml.Serialization.XmlElementAttribute("tag", typeof(spawnableTypeTag))]
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
            return name;
        }
        public bool ContainsAttchorcargo()
        {
            foreach (var item in Items)
            {
                if (item is spawnableTypeAttachment || item is spawnableTypeCargo)
                {
                    if (item is spawnableTypeAttachment)
                    {
                        spawnableTypeAttachment att = item as spawnableTypeAttachment;
                        if (att.item.Count() > 0 && att.preset == null)
                        {
                            return true;
                        }

                    }
                    if (item is spawnableTypeCargo)
                    {
                        spawnableTypeCargo cargo = item as spawnableTypeCargo;
                        if (cargo.item.Count > 0 && cargo.preset == null)
                            return true;
                    }
                }

            }
            return false;
        }
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class spawnableTypeTag
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

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class spawnableTypeDamage
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

        public override string ToString()
        {
            return "Damage";
        }
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class spawnableTypesHoarder
    {

        public override string ToString()
        {
            return "Hoarder";
        }
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class spawnableTypeAttachment
    {
        private spawnableTypeDamage damageField;

        private BindingList<spawnableTypeItem> itemField;

        private decimal chanceField;

        private bool chanceFieldSpecified;

        private string presetField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("damage")]
        public spawnableTypeDamage damage
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
        [System.Xml.Serialization.XmlElementAttribute("item")]
        public BindingList<spawnableTypeItem> item
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

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class spawnableTypeCargo
    {
        private spawnableTypeDamage damageField;

        private BindingList<spawnableTypeItem> itemField;

        private string presetField;

        private decimal chanceField;

        private bool chanceFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("damage")]
        public spawnableTypeDamage damage
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

        [System.Xml.Serialization.XmlElementAttribute("item")]
        public BindingList<spawnableTypeItem> item
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

    public partial class spawnableTypeItem
    {
        private spawnableTypeDamage damageField;

        private BindingList<spawnableTypeAttachment> attachmentsField;

        private BindingList<spawnableTypeCargo> cargoField;

        private string nameField;

        private bool equipField;

        private bool equipFieldSpecified;

        private decimal chanceField;

        private bool chanceFieldSpecified;

        private int quantminField;

        private bool quantminFieldSpecified;

        private int quantmaxField;

        private bool quantmaxFieldSpecified;


        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("damage")]
        public spawnableTypeDamage damage
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
        [System.Xml.Serialization.XmlElementAttribute("attachments")]
        public BindingList<spawnableTypeAttachment> attachments
        {
            get
            {
                return this.attachmentsField;
            }
            set
            {
                this.attachmentsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("cargo")]
        public BindingList<spawnableTypeCargo> cargo
        {
            get
            {
                return this.cargoField;
            }
            set
            {
                this.cargoField = value;
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


        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool equip
        {
            get
            {
                return this.equipField;
            }
            set
            {
                this.equipField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool equipSpecified
        {
            get
            {
                return this.equipFieldSpecified;
            }
            set
            {
                this.equipFieldSpecified = value;
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
        public int quantmin
        {
            get
            {
                return this.quantminField;
            }
            set
            {
                this.quantminField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool quantminSpecified
        {
            get
            {
                return this.quantminFieldSpecified;
            }
            set
            {
                this.quantminFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int quantmax
        {
            get
            {
                return this.quantmaxField;
            }
            set
            {
                this.quantmaxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool quantmaxSpecified
        {
            get
            {
                return this.quantmaxFieldSpecified;
            }
            set
            {
                this.quantmaxFieldSpecified = value;
            }
        }



    }

    /// <remarks/>

}
