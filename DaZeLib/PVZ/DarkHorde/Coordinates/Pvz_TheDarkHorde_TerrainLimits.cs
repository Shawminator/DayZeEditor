using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVZ.DarkHorde.Coordinates.TerrainLimits
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

        private typesTypeTopLeft_Corner_Coordinates topLeft_Corner_CoordinatesField;

        private typesTypeLowRight_Corner_Coordinates lowRight_Corner_CoordinatesField;

        private string zONE_TO_USEField;

        private string zone_NameField;

        /// <remarks/>
        public typesTypeTopLeft_Corner_Coordinates TopLeft_Corner_Coordinates
        {
            get
            {
                return this.topLeft_Corner_CoordinatesField;
            }
            set
            {
                this.topLeft_Corner_CoordinatesField = value;
            }
        }

        /// <remarks/>
        public typesTypeLowRight_Corner_Coordinates LowRight_Corner_Coordinates
        {
            get
            {
                return this.lowRight_Corner_CoordinatesField;
            }
            set
            {
                this.lowRight_Corner_CoordinatesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ZONE_TO_USE
        {
            get
            {
                return this.zONE_TO_USEField;
            }
            set
            {
                this.zONE_TO_USEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Zone_Name
        {
            get
            {
                return this.zone_NameField;
            }
            set
            {
                this.zone_NameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeTopLeft_Corner_Coordinates
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
    public partial class typesTypeLowRight_Corner_Coordinates
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
