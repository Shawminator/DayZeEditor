using System.ComponentModel;

namespace DayZeLib
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class eventgroupdef
    {

        private BindingList<eventgroupdefGroup> groupField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("group")]
        public BindingList<eventgroupdefGroup> group
        {
            get
            {
                return this.groupField;
            }
            set
            {
                this.groupField = value;
            }
        }

        public eventgroupdefGroup getassociatedgroup(string name)
        {
            foreach (eventgroupdefGroup eventgroupdefGroup in group)
            {
                if (eventgroupdefGroup.name == name)
                    return eventgroupdefGroup;
            }
            return null;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class eventgroupdefGroup
    {

        private BindingList<eventgroupdefGroupChild> childField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("child")]
        public BindingList<eventgroupdefGroupChild> child
        {
            get
            {
                return this.childField;
            }
            set
            {
                this.childField = value;
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

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class eventgroupdefGroupChild
    {

        private string typeField;

        private int delootField;

        private bool delootFieldSpecified;

        private int lootmaxField;

        private bool lootmaxFieldSpecified;

        private int lootminField;

        private bool lootminFieldSpecified;

        private decimal xField;

        private decimal zField;

        private decimal aField;

        private bool yFieldSpecified;

        private decimal yField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
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

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool delootSpecified
        {
            get
            {
                return this.delootFieldSpecified;
            }
            set
            {
                this.delootFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int lootmax
        {
            get
            {
                return this.lootmaxField;
            }
            set
            {
                this.lootmaxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool lootmaxSpecified
        {
            get
            {
                return this.lootmaxFieldSpecified;
            }
            set
            {
                this.lootmaxFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int lootmin
        {
            get
            {
                return this.lootminField;
            }
            set
            {
                this.lootminField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool lootminSpecified
        {
            get
            {
                return this.lootminFieldSpecified;
            }
            set
            {
                this.lootminFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal x
        {
            get
            {
                return this.xField;
            }
            set
            {
                this.xField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal z
        {
            get
            {
                return this.zField;
            }
            set
            {
                this.zField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal a
        {
            get
            {
                return this.aField;
            }
            set
            {
                this.aField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ySpecified
        {
            get
            {
                return this.yFieldSpecified;
            }
            set
            {
                this.yFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal y
        {
            get
            {
                return this.yField;
            }
            set
            {
                this.yField = value;
            }
        }

        public override string ToString()
        {
            return type;
        }
    }


}
