using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVZ.DarkHorde.Coordinates.SafeZones
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

        private typesTypeActivated activatedField;

        private typesTypeCorner_UpperLeft corner_UpperLeftField;

        private typesTypeCorner_LowerRight corner_LowerRightField;

        private typesTypeGlobal_Activation global_ActivationField;

        private typesTypeTeleport_Distance_When_Entering_Safe_Zone teleport_Distance_When_Entering_Safe_ZoneField;

        private typesTypeSecurity_Distance_To_Players security_Distance_To_PlayersField;

        private typesTypeAlso_Count_Players_In_Safe_Zone_To_Trigger_Hunting_Mod also_Count_Players_In_Safe_Zone_To_Trigger_Hunting_ModField;

        private typesTypeHunting_Mod_Can_Focus_On_Players_In_Safe_Zone hunting_Mod_Can_Focus_On_Players_In_Safe_ZoneField;

        private typesTypeSafe_Zone_Timer safe_Zone_TimerField;

        private string nameField;

        /// <remarks/>
        public typesTypeActivated Activated
        {
            get
            {
                return this.activatedField;
            }
            set
            {
                this.activatedField = value;
            }
        }

        /// <remarks/>
        public typesTypeCorner_UpperLeft Corner_UpperLeft
        {
            get
            {
                return this.corner_UpperLeftField;
            }
            set
            {
                this.corner_UpperLeftField = value;
            }
        }

        /// <remarks/>
        public typesTypeCorner_LowerRight Corner_LowerRight
        {
            get
            {
                return this.corner_LowerRightField;
            }
            set
            {
                this.corner_LowerRightField = value;
            }
        }

        /// <remarks/>
        public typesTypeGlobal_Activation Global_Activation
        {
            get
            {
                return this.global_ActivationField;
            }
            set
            {
                this.global_ActivationField = value;
            }
        }

        /// <remarks/>
        public typesTypeTeleport_Distance_When_Entering_Safe_Zone Teleport_Distance_When_Entering_Safe_Zone
        {
            get
            {
                return this.teleport_Distance_When_Entering_Safe_ZoneField;
            }
            set
            {
                this.teleport_Distance_When_Entering_Safe_ZoneField = value;
            }
        }

        /// <remarks/>
        public typesTypeSecurity_Distance_To_Players Security_Distance_To_Players
        {
            get
            {
                return this.security_Distance_To_PlayersField;
            }
            set
            {
                this.security_Distance_To_PlayersField = value;
            }
        }

        /// <remarks/>
        public typesTypeAlso_Count_Players_In_Safe_Zone_To_Trigger_Hunting_Mod Also_Count_Players_In_Safe_Zone_To_Trigger_Hunting_Mod
        {
            get
            {
                return this.also_Count_Players_In_Safe_Zone_To_Trigger_Hunting_ModField;
            }
            set
            {
                this.also_Count_Players_In_Safe_Zone_To_Trigger_Hunting_ModField = value;
            }
        }

        /// <remarks/>
        public typesTypeHunting_Mod_Can_Focus_On_Players_In_Safe_Zone Hunting_Mod_Can_Focus_On_Players_In_Safe_Zone
        {
            get
            {
                return this.hunting_Mod_Can_Focus_On_Players_In_Safe_ZoneField;
            }
            set
            {
                this.hunting_Mod_Can_Focus_On_Players_In_Safe_ZoneField = value;
            }
        }

        /// <remarks/>
        public typesTypeSafe_Zone_Timer Safe_Zone_Timer
        {
            get
            {
                return this.safe_Zone_TimerField;
            }
            set
            {
                this.safe_Zone_TimerField = value;
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
    public partial class typesTypeActivated
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
    public partial class typesTypeCorner_UpperLeft
    {

        private string coordinatesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Coordinates
        {
            get
            {
                return this.coordinatesField;
            }
            set
            {
                this.coordinatesField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeCorner_LowerRight
    {

        private string coordinatesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Coordinates
        {
            get
            {
                return this.coordinatesField;
            }
            set
            {
                this.coordinatesField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeGlobal_Activation
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
    public partial class typesTypeTeleport_Distance_When_Entering_Safe_Zone
    {

        private ushort valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort Value
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
    public partial class typesTypeSecurity_Distance_To_Players
    {

        private ushort valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort Value
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
    public partial class typesTypeAlso_Count_Players_In_Safe_Zone_To_Trigger_Hunting_Mod
    {

        private bool valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool Value
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
    public partial class typesTypeHunting_Mod_Can_Focus_On_Players_In_Safe_Zone
    {

        private bool valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool Value
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
    public partial class typesTypeSafe_Zone_Timer
    {

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Value
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
