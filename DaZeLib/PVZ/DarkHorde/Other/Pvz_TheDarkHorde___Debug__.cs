using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVZ.DarkHorde.Other.___Debug__
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

        private typesTypeNumber_Of_Hordes number_Of_HordesField;

        private typesTypeRelative_Position_Of_The_Players relative_Position_Of_The_PlayersField;

        private typesTypeTimer_Before_Teleporting_Players timer_Before_Teleporting_PlayersField;

        private typesTypeNumber_Of_The_Horde_On_Which_To_Teleport number_Of_The_Horde_On_Which_To_TeleportField;

        private typesTypeActivate_Test_Zone activate_Test_ZoneField;

        private typesTypeTest_Zone_Limits test_Zone_LimitsField;

        private typesTypeShow_Debug_Messages show_Debug_MessagesField;

        private typesTypeUse_Debug_Fog_Blue_Color use_Debug_Fog_Blue_ColorField;

        private string nameField;

        /// <remarks/>
        public typesTypeNumber_Of_Hordes Number_Of_Hordes
        {
            get
            {
                return this.number_Of_HordesField;
            }
            set
            {
                this.number_Of_HordesField = value;
            }
        }

        /// <remarks/>
        public typesTypeRelative_Position_Of_The_Players Relative_Position_Of_The_Players
        {
            get
            {
                return this.relative_Position_Of_The_PlayersField;
            }
            set
            {
                this.relative_Position_Of_The_PlayersField = value;
            }
        }

        /// <remarks/>
        public typesTypeTimer_Before_Teleporting_Players Timer_Before_Teleporting_Players
        {
            get
            {
                return this.timer_Before_Teleporting_PlayersField;
            }
            set
            {
                this.timer_Before_Teleporting_PlayersField = value;
            }
        }

        /// <remarks/>
        public typesTypeNumber_Of_The_Horde_On_Which_To_Teleport Number_Of_The_Horde_On_Which_To_Teleport
        {
            get
            {
                return this.number_Of_The_Horde_On_Which_To_TeleportField;
            }
            set
            {
                this.number_Of_The_Horde_On_Which_To_TeleportField = value;
            }
        }

        /// <remarks/>
        public typesTypeActivate_Test_Zone Activate_Test_Zone
        {
            get
            {
                return this.activate_Test_ZoneField;
            }
            set
            {
                this.activate_Test_ZoneField = value;
            }
        }

        /// <remarks/>
        public typesTypeTest_Zone_Limits Test_Zone_Limits
        {
            get
            {
                return this.test_Zone_LimitsField;
            }
            set
            {
                this.test_Zone_LimitsField = value;
            }
        }

        /// <remarks/>
        public typesTypeShow_Debug_Messages Show_Debug_Messages
        {
            get
            {
                return this.show_Debug_MessagesField;
            }
            set
            {
                this.show_Debug_MessagesField = value;
            }
        }

        /// <remarks/>
        public typesTypeUse_Debug_Fog_Blue_Color Use_Debug_Fog_Blue_Color
        {
            get
            {
                return this.use_Debug_Fog_Blue_ColorField;
            }
            set
            {
                this.use_Debug_Fog_Blue_ColorField = value;
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
    public partial class typesTypeNumber_Of_Hordes
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
    public partial class typesTypeRelative_Position_Of_The_Players
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
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
    public partial class typesTypeTimer_Before_Teleporting_Players
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
    public partial class typesTypeNumber_Of_The_Horde_On_Which_To_Teleport
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
    public partial class typesTypeActivate_Test_Zone
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
    public partial class typesTypeTest_Zone_Limits
    {

        private string upLeft_CornerField;

        private string lowRight_CornerField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string UpLeft_Corner
        {
            get
            {
                return this.upLeft_CornerField;
            }
            set
            {
                this.upLeft_CornerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string LowRight_Corner
        {
            get
            {
                return this.lowRight_CornerField;
            }
            set
            {
                this.lowRight_CornerField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeShow_Debug_Messages
    {

        private byte valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte value
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
    public partial class typesTypeUse_Debug_Fog_Blue_Color
    {

        private byte valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte value
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
