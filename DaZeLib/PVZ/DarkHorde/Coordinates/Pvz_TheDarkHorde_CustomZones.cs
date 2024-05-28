using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVZ.DarkHorde.Coordinates.CustomZones
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class types
    {

        private typesType typeField;

        /// <remarks/>
        public typesType type
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

        private typesTypeZone_Data[] zone_DataField;

        private string zone_To_UseField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Zone_Data")]
        public typesTypeZone_Data[] Zone_Data
        {
            get
            {
                return this.zone_DataField;
            }
            set
            {
                this.zone_DataField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Zone_To_Use
        {
            get
            {
                return this.zone_To_UseField;
            }
            set
            {
                this.zone_To_UseField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeZone_Data
    {

        private string nameField;

        private string coord_TopLeft_CornerField;

        private string coord_LowRight_CornerField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
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
        public string Coord_TopLeft_Corner
        {
            get
            {
                return this.coord_TopLeft_CornerField;
            }
            set
            {
                this.coord_TopLeft_CornerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Coord_LowRight_Corner
        {
            get
            {
                return this.coord_LowRight_CornerField;
            }
            set
            {
                this.coord_LowRight_CornerField = value;
            }
        }
    }


}
