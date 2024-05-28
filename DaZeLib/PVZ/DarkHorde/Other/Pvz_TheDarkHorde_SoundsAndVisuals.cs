using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVZ.DarkHorde.Other.SoundsAndVisuals
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

        private typesTypeTime_Between_Bell_Alarms time_Between_Bell_AlarmsField;

        private typesTypeActivate_Zombie_Spawn_Sound activate_Zombie_Spawn_SoundField;

        private typesTypeActivate_Zombie_Teleport_Sound activate_Zombie_Teleport_SoundField;

        private typesTypeActivate_Thunderbolt_Sound_When_The_Master_Die activate_Thunderbolt_Sound_When_The_Master_DieField;

        private typesTypeActivate_Thunder_Sound_When_The_Master_Is_Disturb activate_Thunder_Sound_When_The_Master_Is_DisturbField;

        private typesTypeActivate_Clouds_Above_The_Horde_When_The_Master_Is_Disturbed activate_Clouds_Above_The_Horde_When_The_Master_Is_DisturbedField;

        private typesTypeDistance_To_Display_Little_Cloud_When_Zombies_Spawn distance_To_Display_Little_Cloud_When_Zombies_SpawnField;

        private typesTypeFog_Color fog_ColorField;

        private typesTypeParticleSystem_Birth_Rate_Ratio particleSystem_Birth_Rate_RatioField;

        private typesTypeParticleSystem_Size_Ratio particleSystem_Size_RatioField;

        private typesTypeParticleSystem_Lifetime_Ratio particleSystem_Lifetime_RatioField;

        private string[] textField;

        private string nameField;

        /// <remarks/>
        public typesTypeTime_Between_Bell_Alarms Time_Between_Bell_Alarms
        {
            get
            {
                return this.time_Between_Bell_AlarmsField;
            }
            set
            {
                this.time_Between_Bell_AlarmsField = value;
            }
        }

        /// <remarks/>
        public typesTypeActivate_Zombie_Spawn_Sound Activate_Zombie_Spawn_Sound
        {
            get
            {
                return this.activate_Zombie_Spawn_SoundField;
            }
            set
            {
                this.activate_Zombie_Spawn_SoundField = value;
            }
        }

        /// <remarks/>
        public typesTypeActivate_Zombie_Teleport_Sound Activate_Zombie_Teleport_Sound
        {
            get
            {
                return this.activate_Zombie_Teleport_SoundField;
            }
            set
            {
                this.activate_Zombie_Teleport_SoundField = value;
            }
        }

        /// <remarks/>
        public typesTypeActivate_Thunderbolt_Sound_When_The_Master_Die Activate_Thunderbolt_Sound_When_The_Master_Die
        {
            get
            {
                return this.activate_Thunderbolt_Sound_When_The_Master_DieField;
            }
            set
            {
                this.activate_Thunderbolt_Sound_When_The_Master_DieField = value;
            }
        }

        /// <remarks/>
        public typesTypeActivate_Thunder_Sound_When_The_Master_Is_Disturb Activate_Thunder_Sound_When_The_Master_Is_Disturb
        {
            get
            {
                return this.activate_Thunder_Sound_When_The_Master_Is_DisturbField;
            }
            set
            {
                this.activate_Thunder_Sound_When_The_Master_Is_DisturbField = value;
            }
        }

        /// <remarks/>
        public typesTypeActivate_Clouds_Above_The_Horde_When_The_Master_Is_Disturbed Activate_Clouds_Above_The_Horde_When_The_Master_Is_Disturbed
        {
            get
            {
                return this.activate_Clouds_Above_The_Horde_When_The_Master_Is_DisturbedField;
            }
            set
            {
                this.activate_Clouds_Above_The_Horde_When_The_Master_Is_DisturbedField = value;
            }
        }

        /// <remarks/>
        public typesTypeDistance_To_Display_Little_Cloud_When_Zombies_Spawn Distance_To_Display_Little_Cloud_When_Zombies_Spawn
        {
            get
            {
                return this.distance_To_Display_Little_Cloud_When_Zombies_SpawnField;
            }
            set
            {
                this.distance_To_Display_Little_Cloud_When_Zombies_SpawnField = value;
            }
        }

        /// <remarks/>
        public typesTypeFog_Color Fog_Color
        {
            get
            {
                return this.fog_ColorField;
            }
            set
            {
                this.fog_ColorField = value;
            }
        }

        /// <remarks/>
        public typesTypeParticleSystem_Birth_Rate_Ratio ParticleSystem_Birth_Rate_Ratio
        {
            get
            {
                return this.particleSystem_Birth_Rate_RatioField;
            }
            set
            {
                this.particleSystem_Birth_Rate_RatioField = value;
            }
        }

        /// <remarks/>
        public typesTypeParticleSystem_Size_Ratio ParticleSystem_Size_Ratio
        {
            get
            {
                return this.particleSystem_Size_RatioField;
            }
            set
            {
                this.particleSystem_Size_RatioField = value;
            }
        }

        /// <remarks/>
        public typesTypeParticleSystem_Lifetime_Ratio ParticleSystem_Lifetime_Ratio
        {
            get
            {
                return this.particleSystem_Lifetime_RatioField;
            }
            set
            {
                this.particleSystem_Lifetime_RatioField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string[] Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
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
    public partial class typesTypeTime_Between_Bell_Alarms
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
    public partial class typesTypeActivate_Zombie_Spawn_Sound
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
    public partial class typesTypeActivate_Zombie_Teleport_Sound
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
    public partial class typesTypeActivate_Thunderbolt_Sound_When_The_Master_Die
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
    public partial class typesTypeActivate_Thunder_Sound_When_The_Master_Is_Disturb
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
    public partial class typesTypeActivate_Clouds_Above_The_Horde_When_The_Master_Is_Disturbed
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
    public partial class typesTypeDistance_To_Display_Little_Cloud_When_Zombies_Spawn
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
    public partial class typesTypeFog_Color
    {

        private byte dayField;

        private byte nightField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte Day
        {
            get
            {
                return this.dayField;
            }
            set
            {
                this.dayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte Night
        {
            get
            {
                return this.nightField;
            }
            set
            {
                this.nightField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeParticleSystem_Birth_Rate_Ratio
    {

        private decimal dayField;

        private decimal nightField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Day
        {
            get
            {
                return this.dayField;
            }
            set
            {
                this.dayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Night
        {
            get
            {
                return this.nightField;
            }
            set
            {
                this.nightField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeParticleSystem_Size_Ratio
    {

        private decimal dayField;

        private decimal nightField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Day
        {
            get
            {
                return this.dayField;
            }
            set
            {
                this.dayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Night
        {
            get
            {
                return this.nightField;
            }
            set
            {
                this.nightField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeParticleSystem_Lifetime_Ratio
    {

        private decimal dayField;

        private decimal nightField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Day
        {
            get
            {
                return this.dayField;
            }
            set
            {
                this.dayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Night
        {
            get
            {
                return this.nightField;
            }
            set
            {
                this.nightField = value;
            }
        }
    }


}
