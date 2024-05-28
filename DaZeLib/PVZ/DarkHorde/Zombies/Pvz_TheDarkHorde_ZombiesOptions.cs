using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVZ.DarkHorde.Zombies.ZombiesOptions
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

        private typesTypeDistance_To_Teleport_Master_When_Horde_Is_Calm distance_To_Teleport_Master_When_Horde_Is_CalmField;

        private typesTypeDistance_To_Teleport_Master_When_Horde_Is_Not_Calm distance_To_Teleport_Master_When_Horde_Is_Not_CalmField;

        private typesTypeMaster_Despawn_Time_After_Master_Have_Been_Killed master_Despawn_Time_After_Master_Have_Been_KilledField;

        private typesTypeRadius_To_Despawn_Zombie_When_Horde_Is_Calm radius_To_Despawn_Zombie_When_Horde_Is_CalmField;

        private typesTypeRadius_To_Despawn_Zombie_When_Horde_Is_Not_Calm radius_To_Despawn_Zombie_When_Horde_Is_Not_CalmField;

        private typesTypeZombie_Despawn_Time_When_They_Are_Outside_Radius zombie_Despawn_Time_When_They_Are_Outside_RadiusField;

        private typesTypeZombie_Despawn_Time_After_Zombie_Have_Been_Killed zombie_Despawn_Time_After_Zombie_Have_Been_KilledField;

        private typesTypeZombie_Despawn_Time_After_Master_Have_Been_Killed zombie_Despawn_Time_After_Master_Have_Been_KilledField;

        private typesTypeZombie_Spawn_Timer zombie_Spawn_TimerField;

        private typesTypeZombie_Spawn_Shift zombie_Spawn_ShiftField;

        private typesTypeZombie_Spawn_Radius zombie_Spawn_RadiusField;

        private typesTypeMaster_Spawn_Radius master_Spawn_RadiusField;

        private typesTypeMaster_Regeneration_Rate master_Regeneration_RateField;

        private typesTypeMaster_Is_Bulletproof master_Is_BulletproofField;

        private typesTypeMaster_Is_Immune_To_Explosions master_Is_Immune_To_ExplosionsField;

        private typesTypeMaster_Is_Teleport_When_Hit_By_MeleeWeapon master_Is_Teleport_When_Hit_By_MeleeWeaponField;

        private typesTypeMaster_Is_Teleport_When_Hit_By_FireWeapon master_Is_Teleport_When_Hit_By_FireWeaponField;

        private typesTypeMaster_Can_Dodge master_Can_DodgeField;

        private typesTypeMaster_Size_Ratio master_Size_RatioField;

        private typesTypeMaster_Stun_Bullet_Resistance master_Stun_Bullet_ResistanceField;

        private typesTypeHealth_Points_Ratio_For_The_Master health_Points_Ratio_For_The_MasterField;

        private typesTypeResistance_To_The_Cars_For_The_Master resistance_To_The_Cars_For_The_MasterField;

        private typesTypeAllow_Ghost_Zombies_To_Go_Through_Walls allow_Ghost_Zombies_To_Go_Through_WallsField;

        private typesTypeAllow_Night_Zombies_To_Teleport allow_Night_Zombies_To_TeleportField;

        private typesTypeHealth_Points_Ratio_For_Zombies_From_The_Horde health_Points_Ratio_For_Zombies_From_The_HordeField;

        private typesTypeHealth_Points_Ratio_For_Zombies_Outside_The_Horde health_Points_Ratio_For_Zombies_Outside_The_HordeField;

        private typesTypeResistance_To_The_Cars_For_Zombies_From_The_Horde resistance_To_The_Cars_For_Zombies_From_The_HordeField;

        private typesTypeResistance_To_The_Cars_For_Zombies_Outside_The_Horde resistance_To_The_Cars_For_Zombies_Outside_The_HordeField;

        private string nameField;

        /// <remarks/>
        public typesTypeDistance_To_Teleport_Master_When_Horde_Is_Calm Distance_To_Teleport_Master_When_Horde_Is_Calm
        {
            get
            {
                return this.distance_To_Teleport_Master_When_Horde_Is_CalmField;
            }
            set
            {
                this.distance_To_Teleport_Master_When_Horde_Is_CalmField = value;
            }
        }

        /// <remarks/>
        public typesTypeDistance_To_Teleport_Master_When_Horde_Is_Not_Calm Distance_To_Teleport_Master_When_Horde_Is_Not_Calm
        {
            get
            {
                return this.distance_To_Teleport_Master_When_Horde_Is_Not_CalmField;
            }
            set
            {
                this.distance_To_Teleport_Master_When_Horde_Is_Not_CalmField = value;
            }
        }

        /// <remarks/>
        public typesTypeMaster_Despawn_Time_After_Master_Have_Been_Killed Master_Despawn_Time_After_Master_Have_Been_Killed
        {
            get
            {
                return this.master_Despawn_Time_After_Master_Have_Been_KilledField;
            }
            set
            {
                this.master_Despawn_Time_After_Master_Have_Been_KilledField = value;
            }
        }

        /// <remarks/>
        public typesTypeRadius_To_Despawn_Zombie_When_Horde_Is_Calm Radius_To_Despawn_Zombie_When_Horde_Is_Calm
        {
            get
            {
                return this.radius_To_Despawn_Zombie_When_Horde_Is_CalmField;
            }
            set
            {
                this.radius_To_Despawn_Zombie_When_Horde_Is_CalmField = value;
            }
        }

        /// <remarks/>
        public typesTypeRadius_To_Despawn_Zombie_When_Horde_Is_Not_Calm Radius_To_Despawn_Zombie_When_Horde_Is_Not_Calm
        {
            get
            {
                return this.radius_To_Despawn_Zombie_When_Horde_Is_Not_CalmField;
            }
            set
            {
                this.radius_To_Despawn_Zombie_When_Horde_Is_Not_CalmField = value;
            }
        }

        /// <remarks/>
        public typesTypeZombie_Despawn_Time_When_They_Are_Outside_Radius Zombie_Despawn_Time_When_They_Are_Outside_Radius
        {
            get
            {
                return this.zombie_Despawn_Time_When_They_Are_Outside_RadiusField;
            }
            set
            {
                this.zombie_Despawn_Time_When_They_Are_Outside_RadiusField = value;
            }
        }

        /// <remarks/>
        public typesTypeZombie_Despawn_Time_After_Zombie_Have_Been_Killed Zombie_Despawn_Time_After_Zombie_Have_Been_Killed
        {
            get
            {
                return this.zombie_Despawn_Time_After_Zombie_Have_Been_KilledField;
            }
            set
            {
                this.zombie_Despawn_Time_After_Zombie_Have_Been_KilledField = value;
            }
        }

        /// <remarks/>
        public typesTypeZombie_Despawn_Time_After_Master_Have_Been_Killed Zombie_Despawn_Time_After_Master_Have_Been_Killed
        {
            get
            {
                return this.zombie_Despawn_Time_After_Master_Have_Been_KilledField;
            }
            set
            {
                this.zombie_Despawn_Time_After_Master_Have_Been_KilledField = value;
            }
        }

        /// <remarks/>
        public typesTypeZombie_Spawn_Timer Zombie_Spawn_Timer
        {
            get
            {
                return this.zombie_Spawn_TimerField;
            }
            set
            {
                this.zombie_Spawn_TimerField = value;
            }
        }

        /// <remarks/>
        public typesTypeZombie_Spawn_Shift Zombie_Spawn_Shift
        {
            get
            {
                return this.zombie_Spawn_ShiftField;
            }
            set
            {
                this.zombie_Spawn_ShiftField = value;
            }
        }

        /// <remarks/>
        public typesTypeZombie_Spawn_Radius Zombie_Spawn_Radius
        {
            get
            {
                return this.zombie_Spawn_RadiusField;
            }
            set
            {
                this.zombie_Spawn_RadiusField = value;
            }
        }

        /// <remarks/>
        public typesTypeMaster_Spawn_Radius Master_Spawn_Radius
        {
            get
            {
                return this.master_Spawn_RadiusField;
            }
            set
            {
                this.master_Spawn_RadiusField = value;
            }
        }

        /// <remarks/>
        public typesTypeMaster_Regeneration_Rate Master_Regeneration_Rate
        {
            get
            {
                return this.master_Regeneration_RateField;
            }
            set
            {
                this.master_Regeneration_RateField = value;
            }
        }

        /// <remarks/>
        public typesTypeMaster_Is_Bulletproof Master_Is_Bulletproof
        {
            get
            {
                return this.master_Is_BulletproofField;
            }
            set
            {
                this.master_Is_BulletproofField = value;
            }
        }

        /// <remarks/>
        public typesTypeMaster_Is_Immune_To_Explosions Master_Is_Immune_To_Explosions
        {
            get
            {
                return this.master_Is_Immune_To_ExplosionsField;
            }
            set
            {
                this.master_Is_Immune_To_ExplosionsField = value;
            }
        }

        /// <remarks/>
        public typesTypeMaster_Is_Teleport_When_Hit_By_MeleeWeapon Master_Is_Teleport_When_Hit_By_MeleeWeapon
        {
            get
            {
                return this.master_Is_Teleport_When_Hit_By_MeleeWeaponField;
            }
            set
            {
                this.master_Is_Teleport_When_Hit_By_MeleeWeaponField = value;
            }
        }

        /// <remarks/>
        public typesTypeMaster_Is_Teleport_When_Hit_By_FireWeapon Master_Is_Teleport_When_Hit_By_FireWeapon
        {
            get
            {
                return this.master_Is_Teleport_When_Hit_By_FireWeaponField;
            }
            set
            {
                this.master_Is_Teleport_When_Hit_By_FireWeaponField = value;
            }
        }

        /// <remarks/>
        public typesTypeMaster_Can_Dodge Master_Can_Dodge
        {
            get
            {
                return this.master_Can_DodgeField;
            }
            set
            {
                this.master_Can_DodgeField = value;
            }
        }

        /// <remarks/>
        public typesTypeMaster_Size_Ratio Master_Size_Ratio
        {
            get
            {
                return this.master_Size_RatioField;
            }
            set
            {
                this.master_Size_RatioField = value;
            }
        }

        /// <remarks/>
        public typesTypeMaster_Stun_Bullet_Resistance Master_Stun_Bullet_Resistance
        {
            get
            {
                return this.master_Stun_Bullet_ResistanceField;
            }
            set
            {
                this.master_Stun_Bullet_ResistanceField = value;
            }
        }

        /// <remarks/>
        public typesTypeHealth_Points_Ratio_For_The_Master Health_Points_Ratio_For_The_Master
        {
            get
            {
                return this.health_Points_Ratio_For_The_MasterField;
            }
            set
            {
                this.health_Points_Ratio_For_The_MasterField = value;
            }
        }

        /// <remarks/>
        public typesTypeResistance_To_The_Cars_For_The_Master Resistance_To_The_Cars_For_The_Master
        {
            get
            {
                return this.resistance_To_The_Cars_For_The_MasterField;
            }
            set
            {
                this.resistance_To_The_Cars_For_The_MasterField = value;
            }
        }

        /// <remarks/>
        public typesTypeAllow_Ghost_Zombies_To_Go_Through_Walls Allow_Ghost_Zombies_To_Go_Through_Walls
        {
            get
            {
                return this.allow_Ghost_Zombies_To_Go_Through_WallsField;
            }
            set
            {
                this.allow_Ghost_Zombies_To_Go_Through_WallsField = value;
            }
        }

        /// <remarks/>
        public typesTypeAllow_Night_Zombies_To_Teleport Allow_Night_Zombies_To_Teleport
        {
            get
            {
                return this.allow_Night_Zombies_To_TeleportField;
            }
            set
            {
                this.allow_Night_Zombies_To_TeleportField = value;
            }
        }

        /// <remarks/>
        public typesTypeHealth_Points_Ratio_For_Zombies_From_The_Horde Health_Points_Ratio_For_Zombies_From_The_Horde
        {
            get
            {
                return this.health_Points_Ratio_For_Zombies_From_The_HordeField;
            }
            set
            {
                this.health_Points_Ratio_For_Zombies_From_The_HordeField = value;
            }
        }

        /// <remarks/>
        public typesTypeHealth_Points_Ratio_For_Zombies_Outside_The_Horde Health_Points_Ratio_For_Zombies_Outside_The_Horde
        {
            get
            {
                return this.health_Points_Ratio_For_Zombies_Outside_The_HordeField;
            }
            set
            {
                this.health_Points_Ratio_For_Zombies_Outside_The_HordeField = value;
            }
        }

        /// <remarks/>
        public typesTypeResistance_To_The_Cars_For_Zombies_From_The_Horde Resistance_To_The_Cars_For_Zombies_From_The_Horde
        {
            get
            {
                return this.resistance_To_The_Cars_For_Zombies_From_The_HordeField;
            }
            set
            {
                this.resistance_To_The_Cars_For_Zombies_From_The_HordeField = value;
            }
        }

        /// <remarks/>
        public typesTypeResistance_To_The_Cars_For_Zombies_Outside_The_Horde Resistance_To_The_Cars_For_Zombies_Outside_The_Horde
        {
            get
            {
                return this.resistance_To_The_Cars_For_Zombies_Outside_The_HordeField;
            }
            set
            {
                this.resistance_To_The_Cars_For_Zombies_Outside_The_HordeField = value;
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
    public partial class typesTypeDistance_To_Teleport_Master_When_Horde_Is_Calm
    {

        private int miniField;

        private int maxiField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Mini
        {
            get
            {
                return this.miniField;
            }
            set
            {
                this.miniField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Maxi
        {
            get
            {
                return this.maxiField;
            }
            set
            {
                this.maxiField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeDistance_To_Teleport_Master_When_Horde_Is_Not_Calm
    {

        private int miniField;

        private int maxiField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Mini
        {
            get
            {
                return this.miniField;
            }
            set
            {
                this.miniField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Maxi
        {
            get
            {
                return this.maxiField;
            }
            set
            {
                this.maxiField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeMaster_Despawn_Time_After_Master_Have_Been_Killed
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
    public partial class typesTypeRadius_To_Despawn_Zombie_When_Horde_Is_Calm
    {

        private int miniField;

        private int maxiField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Mini
        {
            get
            {
                return this.miniField;
            }
            set
            {
                this.miniField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Maxi
        {
            get
            {
                return this.maxiField;
            }
            set
            {
                this.maxiField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeRadius_To_Despawn_Zombie_When_Horde_Is_Not_Calm
    {

        private int miniField;

        private int maxiField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Mini
        {
            get
            {
                return this.miniField;
            }
            set
            {
                this.miniField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Maxi
        {
            get
            {
                return this.maxiField;
            }
            set
            {
                this.maxiField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeZombie_Despawn_Time_When_They_Are_Outside_Radius
    {

        private int valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Value
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
    public partial class typesTypeZombie_Despawn_Time_After_Zombie_Have_Been_Killed
    {

        private int valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Value
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
    public partial class typesTypeZombie_Despawn_Time_After_Master_Have_Been_Killed
    {

        private int valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Value
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
    public partial class typesTypeZombie_Spawn_Timer
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
    public partial class typesTypeZombie_Spawn_Shift
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
    public partial class typesTypeZombie_Spawn_Radius
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

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeMaster_Spawn_Radius
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

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeMaster_Regeneration_Rate
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
    public partial class typesTypeMaster_Is_Bulletproof
    {

        private int dayField;

        private int nightField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Day
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
        public int Night
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
    public partial class typesTypeMaster_Is_Immune_To_Explosions
    {

        private int dayField;

        private int nightField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Day
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
        public int Night
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
    public partial class typesTypeMaster_Is_Teleport_When_Hit_By_MeleeWeapon
    {

        private int dayField;

        private int nightField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Day
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
        public int Night
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
    public partial class typesTypeMaster_Is_Teleport_When_Hit_By_FireWeapon
    {

        private int dayField;

        private int nightField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Day
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
        public int Night
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
    public partial class typesTypeMaster_Can_Dodge
    {

        private int dayField;

        private int nightField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Day
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
        public int Night
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
    public partial class typesTypeMaster_Size_Ratio
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
    public partial class typesTypeMaster_Stun_Bullet_Resistance
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
    public partial class typesTypeHealth_Points_Ratio_For_The_Master
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
    public partial class typesTypeResistance_To_The_Cars_For_The_Master
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

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeAllow_Ghost_Zombies_To_Go_Through_Walls
    {

        private int valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Value
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
    public partial class typesTypeAllow_Night_Zombies_To_Teleport
    {

        private int valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Value
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
    public partial class typesTypeHealth_Points_Ratio_For_Zombies_From_The_Horde
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
    public partial class typesTypeHealth_Points_Ratio_For_Zombies_Outside_The_Horde
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

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeResistance_To_The_Cars_For_Zombies_From_The_Horde
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

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeResistance_To_The_Cars_For_Zombies_Outside_The_Horde
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
