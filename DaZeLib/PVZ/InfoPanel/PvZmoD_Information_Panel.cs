using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVZ.InfoPanel
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

        private typesTypeCustom_Server_Link custom_Server_LinkField;

        private typesTypePvZmoD_Information_Panel_Enable_In_Main_Menu pvZmoD_Information_Panel_Enable_In_Main_MenuField;

        private typesTypePvZmoD_Information_Panel_Enable_In_Game_Menu pvZmoD_Information_Panel_Enable_In_Game_MenuField;

        private string nameField;

        /// <remarks/>
        public typesTypeCustom_Server_Link Custom_Server_Link
        {
            get
            {
                return this.custom_Server_LinkField;
            }
            set
            {
                this.custom_Server_LinkField = value;
            }
        }

        /// <remarks/>
        public typesTypePvZmoD_Information_Panel_Enable_In_Main_Menu PvZmoD_Information_Panel_Enable_In_Main_Menu
        {
            get
            {
                return this.pvZmoD_Information_Panel_Enable_In_Main_MenuField;
            }
            set
            {
                this.pvZmoD_Information_Panel_Enable_In_Main_MenuField = value;
            }
        }

        /// <remarks/>
        public typesTypePvZmoD_Information_Panel_Enable_In_Game_Menu PvZmoD_Information_Panel_Enable_In_Game_Menu
        {
            get
            {
                return this.pvZmoD_Information_Panel_Enable_In_Game_MenuField;
            }
            set
            {
                this.pvZmoD_Information_Panel_Enable_In_Game_MenuField = value;
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
    public partial class typesTypeCustom_Server_Link
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
    public partial class typesTypePvZmoD_Information_Panel_Enable_In_Main_Menu
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
    public partial class typesTypePvZmoD_Information_Panel_Enable_In_Game_Menu
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

}
