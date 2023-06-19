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
    public partial class prototype
    {

        private BindingList<prototypeDefault> defaultsField;

        private BindingList<prototypeGroup> groupField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("default", IsNullable = false)]
        public BindingList<prototypeDefault> defaults
        {
            get
            {
                return this.defaultsField;
            }
            set
            {
                this.defaultsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("group")]
        public BindingList<prototypeGroup> group
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class prototypeDefault
    {

        private string nameField;

        private int lootmaxField;

        private bool lootmaxFieldSpecified;

        private string enabledField;

        private string deField;

        private int widthField;

        private bool widthFieldSpecified;

        private int heightField;

        private bool heightFieldSpecified;

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
        public string enabled
        {
            get
            {
                return this.enabledField;
            }
            set
            {
                this.enabledField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string de
        {
            get
            {
                return this.deField;
            }
            set
            {
                this.deField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int width
        {
            get
            {
                return this.widthField;
            }
            set
            {
                this.widthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool widthSpecified
        {
            get
            {
                return this.widthFieldSpecified;
            }
            set
            {
                this.widthFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int height
        {
            get
            {
                return this.heightField;
            }
            set
            {
                this.heightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool heightSpecified
        {
            get
            {
                return this.heightFieldSpecified;
            }
            set
            {
                this.heightFieldSpecified = value;
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
    public partial class prototypeGroup
    {

        private BindingList<prototypeGroupValue> valueField;

        private BindingList<prototypeGroupUsage> usageField;

        private BindingList<prototypeGroupContainer> containerField;

        private BindingList<prototypeGroupProxy> dispatchField;

        private string nameField;

        private int lootmaxField;

        private bool lootmaxFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value")]
        public BindingList<prototypeGroupValue> value
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
        [System.Xml.Serialization.XmlElementAttribute("usage")]
        public BindingList<prototypeGroupUsage> usage
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
        [System.Xml.Serialization.XmlElementAttribute("container")]
        public BindingList<prototypeGroupContainer> container
        {
            get
            {
                return this.containerField;
            }
            set
            {
                this.containerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("proxy", IsNullable = false)]
        public BindingList<prototypeGroupProxy> dispatch
        {
            get
            {
                return this.dispatchField;
            }
            set
            {
                this.dispatchField = value;
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

        public override string ToString()
        {
            return name;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class prototypeGroupValue
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


    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class prototypeGroupUsage
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
    public partial class prototypeGroupContainer
    {

        private BindingList<prototypeGroupContainerCategory> categoryField;

        private BindingList<prototypeGroupContainerTag> tagField;

        private BindingList<prototypeGroupContainerPoint> pointField;

        private string nameField;

        private int lootmaxField;

        private bool lootmaxFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("category")]
        public BindingList<prototypeGroupContainerCategory> category
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
        [System.Xml.Serialization.XmlElementAttribute("tag")]
        public BindingList<prototypeGroupContainerTag> tag
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
        [System.Xml.Serialization.XmlElementAttribute("point")]
        public BindingList<prototypeGroupContainerPoint> point
        {
            get
            {
                return this.pointField;
            }
            set
            {
                this.pointField = value;
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
        public override string ToString()
        {
            return name;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class prototypeGroupContainerCategory
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
    public partial class prototypeGroupContainerTag
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
    public partial class prototypeGroupContainerPoint
    {

        private string posField;

        private decimal rangeField;

        private decimal heightField;

        private int flagsField;

        private bool flagsFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string pos
        {
            get
            {
                return this.posField;
            }
            set
            {
                this.posField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal range
        {
            get
            {
                return this.rangeField;
            }
            set
            {
                this.rangeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal height
        {
            get
            {
                return this.heightField;
            }
            set
            {
                this.heightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int flags
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool flagsSpecified
        {
            get
            {
                return this.flagsFieldSpecified;
            }
            set
            {
                this.flagsFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class prototypeGroupProxy
    {

        private string typeField;

        private string posField;

        private string rpyField;

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
        public string pos
        {
            get
            {
                return this.posField;
            }
            set
            {
                this.posField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string rpy
        {
            get
            {
                return this.rpyField;
            }
            set
            {
                this.rpyField = value;
            }
        }
    }


}
