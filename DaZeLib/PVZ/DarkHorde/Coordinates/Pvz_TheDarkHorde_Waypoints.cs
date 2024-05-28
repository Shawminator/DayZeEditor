using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVZ.DarkHorde.Coordinates.Waypoints
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

        private typesTypeWaypoint_Coordinates[] waypoint_CoordinatesField;

        private string waypoints_List_To_UseField;

        private string waypoints_List_NameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Waypoint_Coordinates")]
        public typesTypeWaypoint_Coordinates[] Waypoint_Coordinates
        {
            get
            {
                return this.waypoint_CoordinatesField;
            }
            set
            {
                this.waypoint_CoordinatesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Waypoints_List_To_Use
        {
            get
            {
                return this.waypoints_List_To_UseField;
            }
            set
            {
                this.waypoints_List_To_UseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Waypoints_List_Name
        {
            get
            {
                return this.waypoints_List_NameField;
            }
            set
            {
                this.waypoints_List_NameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeWaypoint_Coordinates
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
