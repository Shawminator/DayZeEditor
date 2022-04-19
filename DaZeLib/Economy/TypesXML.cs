using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZeLib
{


    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class types
    {

        private BindingList<typesType> typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("type")]
        public BindingList<typesType> type
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

        private int nominalField;

        private bool nominalFieldSpecified;

        private int lifetimeField;

        private int restockField;

        private bool restockFieldSpecified;

        private int minField;

        private bool minFieldSpecified;

        private int quantminField;

        private bool quantminFieldSpecified;

        private int quantmaxField;

        private bool quantmaxFieldSpecified;

        private int costField;

        private bool costFieldSpecified;

        private typesTypeFlags flagsField;

        private typesTypeCategory categoryField;

        private BindingList<typesTypeUsage> usageField;

        private BindingList<typesTypeTag> tagField;

        private BindingList<typesTypeValue> valueField;

        private string nameField;

        /// <remarks/>
        public int nominal
        {
            get
            {
                return this.nominalField;
            }
            set
            {
                this.nominalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool nominalSpecified
        {
            get
            {
                return this.nominalFieldSpecified;
            }
            set
            {
                this.nominalFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int lifetime
        {
            get
            {
                return this.lifetimeField;
            }
            set
            {
                this.lifetimeField = value;
            }
        }

        /// <remarks/>
        public int restock
        {
            get
            {
                return this.restockField;
            }
            set
            {
                this.restockField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool restockSpecified
        {
            get
            {
                return this.restockFieldSpecified;
            }
            set
            {
                this.restockFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int min
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool minSpecified
        {
            get
            {
                return this.minFieldSpecified;
            }
            set
            {
                this.minFieldSpecified = value;
            }
        }

        /// <remarks/>
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

        /// <remarks/>
        public int cost
        {
            get
            {
                return this.costField;
            }
            set
            {
                this.costField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool costSpecified
        {
            get
            {
                return this.costFieldSpecified;
            }
            set
            {
                this.costFieldSpecified = value;
            }
        }

        /// <remarks/>
        public typesTypeFlags flags
        {
            get
            {
                return this.flagsField;
            }
            set
            {
                this.flagsField = value;
            }
        }

        /// <remarks/>
        public typesTypeCategory category
        {
            get
            {
                return this.categoryField;
            }
            set
            {
                this.categoryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("usage")]
        public BindingList<typesTypeUsage> usage
        {
            get
            {
                return this.usageField;
            }
            set
            {
                this.usageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("tag")]
        public BindingList<typesTypeTag> tag
        {
            get
            {
                return this.tagField;
            }
            set
            {
                this.tagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value")]
        public BindingList<typesTypeValue> value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
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
        public void AddnewUsage(typesTypeUsage u)
        {
            if (usage == null)
                usage = new BindingList<typesTypeUsage>();
            if (!usage.Any(x => x.name == u.name))
            {
                usage.Add(u);
            }
        }
        public void removeusage(typesTypeUsage u)
        {
            if (usage == null) return;
            typesTypeUsage usagetoremove = usage.FirstOrDefault(x => x.name == u.name);
            if (usagetoremove != null)
                usage.Remove(usagetoremove);
        }
        public void Addnewtag(typesTypeTag t)
        {
            if (tag == null)
                tag = new BindingList<typesTypeTag>();
            if (!tag.Any(x => x.name == t.name))
            {
                tag.Add(t);
            }
        }
        public void removetag(typesTypeTag t)
        {
            if (tag == null) return;
            typesTypeTag tagtoremove = tag.FirstOrDefault(x => x.name == t.name);
            if (tagtoremove != null)
                tag.Remove(tagtoremove);
        }
        public void changecategory(typesTypeCategory c)
        {
            category = c;
        }
        public void AddTier(string tier)
        {
            if (value == null)
                value = new BindingList<typesTypeValue>();
            value.Add(new typesTypeValue() { name = tier });
           // Usinguserdifinitions = false;
            for (int i = 0; i < value.Count; i++)
            {
                if (value[i].name == null)
                {
                    value.RemoveAt(i);
                    i--;
                }
            }
        }
        public void removetier(string tier)
        {
            if (value == null) return;
            if (value.Any(x => x.name == tier))
                value.Remove(value.First(X => X.name == tier));
            if (value.Count == 0)
                value = null;
        }
        public void AdduserTier(string tier)
        {
            if (value == null)
                value = new BindingList<typesTypeValue>();
            value.Add(new typesTypeValue() { user = tier });
            //Usinguserdifinitions = true;
            for (int i = 0; i < value.Count; i++)
            {
                if (value[i].user == null)
                {
                    value.RemoveAt(i);
                    i--;
                }
            }
        }
        public void removeusertier(string tier)
        {
            value.Remove(value.First(X => X.user == tier));
            if (value.Count == 0)
            {
                value = null;
                //Usinguserdifinitions = false;
            }
        }

        public void removetiers()
        {
            if (value != null)
                value = null;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeFlags
    {

        private int count_in_cargoField;

        private int count_in_hoarderField;

        private int count_in_mapField;

        private int count_in_playerField;

        private int craftedField;

        private int delootField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int count_in_cargo
        {
            get
            {
                return this.count_in_cargoField;
            }
            set
            {
                this.count_in_cargoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int count_in_hoarder
        {
            get
            {
                return this.count_in_hoarderField;
            }
            set
            {
                this.count_in_hoarderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int count_in_map
        {
            get
            {
                return this.count_in_mapField;
            }
            set
            {
                this.count_in_mapField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int count_in_player
        {
            get
            {
                return this.count_in_playerField;
            }
            set
            {
                this.count_in_playerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int crafted
        {
            get
            {
                return this.craftedField;
            }
            set
            {
                this.craftedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int deloot
        {
            get
            {
                return this.delootField;
            }
            set
            {
                this.delootField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeCategory
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
            return name;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeUsage
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
            return name;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeTag
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
            return name;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeValue
    {

        private string userField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string user
        {
            get
            {
                return this.userField;
            }
            set
            {
                this.userField = value;
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
    }




}
