using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVZ.DarkHorde.Other.CustomNightTime
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

        private typesTypeNight_Config_Selected night_Config_SelectedField;

        private typesTypeBeginning_Of_The_Night beginning_Of_The_NightField;

        private typesTypeEnd_Of_The_Night end_Of_The_NightField;

        private string nameField;

        /// <remarks/>
        public typesTypeNight_Config_Selected Night_Config_Selected
        {
            get
            {
                return this.night_Config_SelectedField;
            }
            set
            {
                this.night_Config_SelectedField = value;
            }
        }

        /// <remarks/>
        public typesTypeBeginning_Of_The_Night Beginning_Of_The_Night
        {
            get
            {
                return this.beginning_Of_The_NightField;
            }
            set
            {
                this.beginning_Of_The_NightField = value;
            }
        }

        /// <remarks/>
        public typesTypeEnd_Of_The_Night End_Of_The_Night
        {
            get
            {
                return this.end_Of_The_NightField;
            }
            set
            {
                this.end_Of_The_NightField = value;
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeNight_Config_Selected
    {

        private byte valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte Value
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeBeginning_Of_The_Night
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Value
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeEnd_Of_The_Night
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Value
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
    }


}
