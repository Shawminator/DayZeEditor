using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PVZ.CustomZombiesGlobals
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
        [XmlIgnore]
        public bool isDirty = false;
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesType
    {

        private typesTypeDebug_Mod debug_ModField;

        private typesTypeSpecial_HeadShot_Weapons special_HeadShot_WeaponsField;

        private typesTypeItems_Protect_HeadShots items_Protect_HeadShotsField;

        private typesTypeFriendly_Wolves friendly_WolvesField;

        private typesTypeZombies_Throw_Stones_Activated zombies_Throw_Stones_ActivatedField;

        private typesTypeZombies_Throw_Stones_Only_If_Player_On_Obstacle zombies_Throw_Stones_Only_If_Player_On_ObstacleField;

        private typesTypeZombies_Throw_Stones_Damage_Health zombies_Throw_Stones_Damage_HealthField;

        private typesTypeZombies_Throw_Stones_Damage_Shock zombies_Throw_Stones_Damage_ShockField;

        private typesTypeZombies_Throw_Stones_Keep_Minimum_Health zombies_Throw_Stones_Keep_Minimum_HealthField;

        private typesTypeZombies_Throw_Stones_Rate zombies_Throw_Stones_RateField;

        private typesTypeZombies_Throw_Stones_Force zombies_Throw_Stones_ForceField;

        private typesTypeZombies_Throw_Stones_Distance_Maxi zombies_Throw_Stones_Distance_MaxiField;

        private typesTypeAttack_Stopped_Vehicles[] attack_Stopped_VehiclesField;

        private typesTypeDamages_To_Vehicles[] damages_To_VehiclesField;

        private typesTypeOverRide_Vanilla_Night_Time overRide_Vanilla_Night_TimeField;

        private typesTypeNight_Beginning night_BeginningField;

        private typesTypeNight_End night_EndField;

        private typesTypeMaximum_Number_Of_Bleeding_Sources_From_Zombies_Combat maximum_Number_Of_Bleeding_Sources_From_Zombies_CombatField;

        private typesTypeShoes_Degradation_When_Hit_By_Crawling_Zombie shoes_Degradation_When_Hit_By_Crawling_ZombieField;

        private typesTypeFist_Fighting[] fist_FightingField;

        private typesTypeSpecial_Mask_To_Hide_From_Zombies special_Mask_To_Hide_From_ZombiesField;

        private typesTypeSpecial_Mask_Type_Of_Slot special_Mask_Type_Of_SlotField;

        private typesTypeSpecial_Mask_Lifetime special_Mask_LifetimeField;

        private typesTypeMulti_Hits_On_Heavy_Attack[] multi_Hits_On_Heavy_AttackField;

        private typesTypeZombies_Health_Ratio zombies_Health_RatioField;

        private typesTypeZombies_Strength_Ratio zombies_Strength_RatioField;

        private typesTypeZombies_Speed_Ratio zombies_Speed_RatioField;

        private typesTypeZombies_Clamp_Speed_Mini zombies_Clamp_Speed_MiniField;

        private typesTypeZombies_Clamp_Speed_Maxi zombies_Clamp_Speed_MaxiField;

        private typesTypeZombies_Hearing_Standing_Players_Ratio zombies_Hearing_Standing_Players_RatioField;

        private typesTypeZombies_Hearing_Crouching_Players_Ratio zombies_Hearing_Crouching_Players_RatioField;

        private typesTypeZombies_Size_Ratio zombies_Size_RatioField;

        private typesTypeZombies_Stun_By_Bullet_Resistance_Ratio zombies_Stun_By_Bullet_Resistance_RatioField;

        private typesTypeZombies_Maximum_Size_In_Buildings zombies_Maximum_Size_In_BuildingsField;

        private DayNight disable_All_FeaturesField;

        private DayNight zombies_Health_ActivatedField;

        private DayNight zombies_Resistance_ActivatedField;

        private DayNight zombies_Speed_ActivatedField;

        private DayNight zombies_Speed_Adjust_ActivatedField;

        private DayNight zombies_CrawlControl_ActivatedField;

        private DayNight zombies_Can_DodgeField;

        private DayNight zombies_Strenght_ActivatedField;

        private DayNight bleeding_Chance_ActivatedField;

        private typesTypeZombies_Vision_Activated zombies_Vision_ActivatedField;

        private DayNight player_Fist_Fighting_ActivatedField;

        private DayNight zombies_Hit_Players_On_Obstacles_ActivatedField;

        private DayNight zombies_Hit_Unconscious_Players_ActivatedField;

        private DayNight zombies_Hearing_Doors_Player_Standing_ActivatedField;

        private DayNight zombies_Hearing_Doors_Player_Crouching_ActivatedField;

        private DayNight zombies_Breaking_Doors_ActivatedField;

        private DayNight zombies_Breaking_LockedDoors_ActivatedField;

        private DayNight zombies_Aggressive_Doors_Detection_ActivatedField;

        private DayNight zombies_Size_ActivatedField;

        private DayNight zombies_Stun_By_Bullet_Resistance_ActivatedField;

        private typesTypeCustomisable_Zombies_List customisable_Zombies_ListField;

        private string nameField;

        /// <remarks/>
        public typesTypeDebug_Mod Debug_Mod
        {
            get
            {
                return this.debug_ModField;
            }
            set
            {
                this.debug_ModField = value;
            }
        }

        /// <remarks/>
        public typesTypeSpecial_HeadShot_Weapons Special_HeadShot_Weapons
        {
            get
            {
                return this.special_HeadShot_WeaponsField;
            }
            set
            {
                this.special_HeadShot_WeaponsField = value;
            }
        }

        /// <remarks/>
        public typesTypeItems_Protect_HeadShots Items_Protect_HeadShots
        {
            get
            {
                return this.items_Protect_HeadShotsField;
            }
            set
            {
                this.items_Protect_HeadShotsField = value;
            }
        }

        /// <remarks/>
        public typesTypeFriendly_Wolves Friendly_Wolves
        {
            get
            {
                return this.friendly_WolvesField;
            }
            set
            {
                this.friendly_WolvesField = value;
            }
        }

        /// <remarks/>
        public typesTypeZombies_Throw_Stones_Activated Zombies_Throw_Stones_Activated
        {
            get
            {
                return this.zombies_Throw_Stones_ActivatedField;
            }
            set
            {
                this.zombies_Throw_Stones_ActivatedField = value;
            }
        }

        /// <remarks/>
        public typesTypeZombies_Throw_Stones_Only_If_Player_On_Obstacle Zombies_Throw_Stones_Only_If_Player_On_Obstacle
        {
            get
            {
                return this.zombies_Throw_Stones_Only_If_Player_On_ObstacleField;
            }
            set
            {
                this.zombies_Throw_Stones_Only_If_Player_On_ObstacleField = value;
            }
        }

        /// <remarks/>
        public typesTypeZombies_Throw_Stones_Damage_Health Zombies_Throw_Stones_Damage_Health
        {
            get
            {
                return this.zombies_Throw_Stones_Damage_HealthField;
            }
            set
            {
                this.zombies_Throw_Stones_Damage_HealthField = value;
            }
        }

        /// <remarks/>
        public typesTypeZombies_Throw_Stones_Damage_Shock Zombies_Throw_Stones_Damage_Shock
        {
            get
            {
                return this.zombies_Throw_Stones_Damage_ShockField;
            }
            set
            {
                this.zombies_Throw_Stones_Damage_ShockField = value;
            }
        }

        /// <remarks/>
        public typesTypeZombies_Throw_Stones_Keep_Minimum_Health Zombies_Throw_Stones_Keep_Minimum_Health
        {
            get
            {
                return this.zombies_Throw_Stones_Keep_Minimum_HealthField;
            }
            set
            {
                this.zombies_Throw_Stones_Keep_Minimum_HealthField = value;
            }
        }

        /// <remarks/>
        public typesTypeZombies_Throw_Stones_Rate Zombies_Throw_Stones_Rate
        {
            get
            {
                return this.zombies_Throw_Stones_RateField;
            }
            set
            {
                this.zombies_Throw_Stones_RateField = value;
            }
        }

        /// <remarks/>
        public typesTypeZombies_Throw_Stones_Force Zombies_Throw_Stones_Force
        {
            get
            {
                return this.zombies_Throw_Stones_ForceField;
            }
            set
            {
                this.zombies_Throw_Stones_ForceField = value;
            }
        }

        /// <remarks/>
        public typesTypeZombies_Throw_Stones_Distance_Maxi Zombies_Throw_Stones_Distance_Maxi
        {
            get
            {
                return this.zombies_Throw_Stones_Distance_MaxiField;
            }
            set
            {
                this.zombies_Throw_Stones_Distance_MaxiField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Attack_Stopped_Vehicles")]
        public typesTypeAttack_Stopped_Vehicles[] Attack_Stopped_Vehicles
        {
            get
            {
                return this.attack_Stopped_VehiclesField;
            }
            set
            {
                this.attack_Stopped_VehiclesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Damages_To_Vehicles")]
        public typesTypeDamages_To_Vehicles[] Damages_To_Vehicles
        {
            get
            {
                return this.damages_To_VehiclesField;
            }
            set
            {
                this.damages_To_VehiclesField = value;
            }
        }

        /// <remarks/>
        public typesTypeOverRide_Vanilla_Night_Time OverRide_Vanilla_Night_Time
        {
            get
            {
                return this.overRide_Vanilla_Night_TimeField;
            }
            set
            {
                this.overRide_Vanilla_Night_TimeField = value;
            }
        }

        /// <remarks/>
        public typesTypeNight_Beginning Night_Beginning
        {
            get
            {
                return this.night_BeginningField;
            }
            set
            {
                this.night_BeginningField = value;
            }
        }

        /// <remarks/>
        public typesTypeNight_End Night_End
        {
            get
            {
                return this.night_EndField;
            }
            set
            {
                this.night_EndField = value;
            }
        }

        /// <remarks/>
        public typesTypeMaximum_Number_Of_Bleeding_Sources_From_Zombies_Combat Maximum_Number_Of_Bleeding_Sources_From_Zombies_Combat
        {
            get
            {
                return this.maximum_Number_Of_Bleeding_Sources_From_Zombies_CombatField;
            }
            set
            {
                this.maximum_Number_Of_Bleeding_Sources_From_Zombies_CombatField = value;
            }
        }

        /// <remarks/>
        public typesTypeShoes_Degradation_When_Hit_By_Crawling_Zombie Shoes_Degradation_When_Hit_By_Crawling_Zombie
        {
            get
            {
                return this.shoes_Degradation_When_Hit_By_Crawling_ZombieField;
            }
            set
            {
                this.shoes_Degradation_When_Hit_By_Crawling_ZombieField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Fist_Fighting")]
        public typesTypeFist_Fighting[] Fist_Fighting
        {
            get
            {
                return this.fist_FightingField;
            }
            set
            {
                this.fist_FightingField = value;
            }
        }

        /// <remarks/>
        public typesTypeSpecial_Mask_To_Hide_From_Zombies Special_Mask_To_Hide_From_Zombies
        {
            get
            {
                return this.special_Mask_To_Hide_From_ZombiesField;
            }
            set
            {
                this.special_Mask_To_Hide_From_ZombiesField = value;
            }
        }

        /// <remarks/>
        public typesTypeSpecial_Mask_Type_Of_Slot Special_Mask_Type_Of_Slot
        {
            get
            {
                return this.special_Mask_Type_Of_SlotField;
            }
            set
            {
                this.special_Mask_Type_Of_SlotField = value;
            }
        }

        /// <remarks/>
        public typesTypeSpecial_Mask_Lifetime Special_Mask_Lifetime
        {
            get
            {
                return this.special_Mask_LifetimeField;
            }
            set
            {
                this.special_Mask_LifetimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Multi_Hits_On_Heavy_Attack")]
        public typesTypeMulti_Hits_On_Heavy_Attack[] Multi_Hits_On_Heavy_Attack
        {
            get
            {
                return this.multi_Hits_On_Heavy_AttackField;
            }
            set
            {
                this.multi_Hits_On_Heavy_AttackField = value;
            }
        }

        /// <remarks/>
        public typesTypeZombies_Health_Ratio Zombies_Health_Ratio
        {
            get
            {
                return this.zombies_Health_RatioField;
            }
            set
            {
                this.zombies_Health_RatioField = value;
            }
        }

        /// <remarks/>
        public typesTypeZombies_Strength_Ratio Zombies_Strength_Ratio
        {
            get
            {
                return this.zombies_Strength_RatioField;
            }
            set
            {
                this.zombies_Strength_RatioField = value;
            }
        }

        /// <remarks/>
        public typesTypeZombies_Speed_Ratio Zombies_Speed_Ratio
        {
            get
            {
                return this.zombies_Speed_RatioField;
            }
            set
            {
                this.zombies_Speed_RatioField = value;
            }
        }

        /// <remarks/>
        public typesTypeZombies_Clamp_Speed_Mini Zombies_Clamp_Speed_Mini
        {
            get
            {
                return this.zombies_Clamp_Speed_MiniField;
            }
            set
            {
                this.zombies_Clamp_Speed_MiniField = value;
            }
        }

        /// <remarks/>
        public typesTypeZombies_Clamp_Speed_Maxi Zombies_Clamp_Speed_Maxi
        {
            get
            {
                return this.zombies_Clamp_Speed_MaxiField;
            }
            set
            {
                this.zombies_Clamp_Speed_MaxiField = value;
            }
        }

        /// <remarks/>
        public typesTypeZombies_Hearing_Standing_Players_Ratio Zombies_Hearing_Standing_Players_Ratio
        {
            get
            {
                return this.zombies_Hearing_Standing_Players_RatioField;
            }
            set
            {
                this.zombies_Hearing_Standing_Players_RatioField = value;
            }
        }

        /// <remarks/>
        public typesTypeZombies_Hearing_Crouching_Players_Ratio Zombies_Hearing_Crouching_Players_Ratio
        {
            get
            {
                return this.zombies_Hearing_Crouching_Players_RatioField;
            }
            set
            {
                this.zombies_Hearing_Crouching_Players_RatioField = value;
            }
        }

        /// <remarks/>
        public typesTypeZombies_Size_Ratio Zombies_Size_Ratio
        {
            get
            {
                return this.zombies_Size_RatioField;
            }
            set
            {
                this.zombies_Size_RatioField = value;
            }
        }

        /// <remarks/>
        public typesTypeZombies_Stun_By_Bullet_Resistance_Ratio Zombies_Stun_By_Bullet_Resistance_Ratio
        {
            get
            {
                return this.zombies_Stun_By_Bullet_Resistance_RatioField;
            }
            set
            {
                this.zombies_Stun_By_Bullet_Resistance_RatioField = value;
            }
        }

        /// <remarks/>
        public typesTypeZombies_Maximum_Size_In_Buildings Zombies_Maximum_Size_In_Buildings
        {
            get
            {
                return this.zombies_Maximum_Size_In_BuildingsField;
            }
            set
            {
                this.zombies_Maximum_Size_In_BuildingsField = value;
            }
        }

        /// <remarks/>
        public DayNight Disable_All_Features
        {
            get
            {
                return this.disable_All_FeaturesField;
            }
            set
            {
                this.disable_All_FeaturesField = value;
            }
        }

        /// <remarks/>
        public DayNight Zombies_Health_Activated
        {
            get
            {
                return this.zombies_Health_ActivatedField;
            }
            set
            {
                this.zombies_Health_ActivatedField = value;
            }
        }

        /// <remarks/>
        public DayNight Zombies_Resistance_Activated
        {
            get
            {
                return this.zombies_Resistance_ActivatedField;
            }
            set
            {
                this.zombies_Resistance_ActivatedField = value;
            }
        }

        /// <remarks/>
        public DayNight Zombies_Speed_Activated
        {
            get
            {
                return this.zombies_Speed_ActivatedField;
            }
            set
            {
                this.zombies_Speed_ActivatedField = value;
            }
        }

        /// <remarks/>
        public DayNight Zombies_Speed_Adjust_Activated
        {
            get
            {
                return this.zombies_Speed_Adjust_ActivatedField;
            }
            set
            {
                this.zombies_Speed_Adjust_ActivatedField = value;
            }
        }

        /// <remarks/>
        public DayNight Zombies_CrawlControl_Activated
        {
            get
            {
                return this.zombies_CrawlControl_ActivatedField;
            }
            set
            {
                this.zombies_CrawlControl_ActivatedField = value;
            }
        }

        /// <remarks/>
        public DayNight Zombies_Can_Dodge
        {
            get
            {
                return this.zombies_Can_DodgeField;
            }
            set
            {
                this.zombies_Can_DodgeField = value;
            }
        }

        /// <remarks/>
        public DayNight Zombies_Strenght_Activated
        {
            get
            {
                return this.zombies_Strenght_ActivatedField;
            }
            set
            {
                this.zombies_Strenght_ActivatedField = value;
            }
        }

        /// <remarks/>
        public DayNight Bleeding_Chance_Activated
        {
            get
            {
                return this.bleeding_Chance_ActivatedField;
            }
            set
            {
                this.bleeding_Chance_ActivatedField = value;
            }
        }

        /// <remarks/>
        public typesTypeZombies_Vision_Activated Zombies_Vision_Activated
        {
            get
            {
                return this.zombies_Vision_ActivatedField;
            }
            set
            {
                this.zombies_Vision_ActivatedField = value;
            }
        }

        /// <remarks/>
        public DayNight Player_Fist_Fighting_Activated
        {
            get
            {
                return this.player_Fist_Fighting_ActivatedField;
            }
            set
            {
                this.player_Fist_Fighting_ActivatedField = value;
            }
        }

        /// <remarks/>
        public DayNight Zombies_Hit_Players_On_Obstacles_Activated
        {
            get
            {
                return this.zombies_Hit_Players_On_Obstacles_ActivatedField;
            }
            set
            {
                this.zombies_Hit_Players_On_Obstacles_ActivatedField = value;
            }
        }

        /// <remarks/>
        public DayNight Zombies_Hit_Unconscious_Players_Activated
        {
            get
            {
                return this.zombies_Hit_Unconscious_Players_ActivatedField;
            }
            set
            {
                this.zombies_Hit_Unconscious_Players_ActivatedField = value;
            }
        }

        /// <remarks/>
        public DayNight Zombies_Hearing_Doors_Player_Standing_Activated
        {
            get
            {
                return this.zombies_Hearing_Doors_Player_Standing_ActivatedField;
            }
            set
            {
                this.zombies_Hearing_Doors_Player_Standing_ActivatedField = value;
            }
        }

        /// <remarks/>
        public DayNight Zombies_Hearing_Doors_Player_Crouching_Activated
        {
            get
            {
                return this.zombies_Hearing_Doors_Player_Crouching_ActivatedField;
            }
            set
            {
                this.zombies_Hearing_Doors_Player_Crouching_ActivatedField = value;
            }
        }

        /// <remarks/>
        public DayNight Zombies_Breaking_Doors_Activated
        {
            get
            {
                return this.zombies_Breaking_Doors_ActivatedField;
            }
            set
            {
                this.zombies_Breaking_Doors_ActivatedField = value;
            }
        }

        /// <remarks/>
        public DayNight Zombies_Breaking_LockedDoors_Activated
        {
            get
            {
                return this.zombies_Breaking_LockedDoors_ActivatedField;
            }
            set
            {
                this.zombies_Breaking_LockedDoors_ActivatedField = value;
            }
        }

        /// <remarks/>
        public DayNight Zombies_Aggressive_Doors_Detection_Activated
        {
            get
            {
                return this.zombies_Aggressive_Doors_Detection_ActivatedField;
            }
            set
            {
                this.zombies_Aggressive_Doors_Detection_ActivatedField = value;
            }
        }

        /// <remarks/>
        public DayNight Zombies_Size_Activated
        {
            get
            {
                return this.zombies_Size_ActivatedField;
            }
            set
            {
                this.zombies_Size_ActivatedField = value;
            }
        }

        /// <remarks/>
        public DayNight Zombies_Stun_By_Bullet_Resistance_Activated
        {
            get
            {
                return this.zombies_Stun_By_Bullet_Resistance_ActivatedField;
            }
            set
            {
                this.zombies_Stun_By_Bullet_Resistance_ActivatedField = value;
            }
        }

        /// <remarks/>
        public typesTypeCustomisable_Zombies_List Customisable_Zombies_List
        {
            get
            {
                return this.customisable_Zombies_ListField;
            }
            set
            {
                this.customisable_Zombies_ListField = value;
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
    public partial class typesTypeDebug_Mod
    {

        private int activatedField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Activated
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeSpecial_HeadShot_Weapons
    {

        private string listField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string List
        {
            get
            {
                return this.listField;
            }
            set
            {
                this.listField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeItems_Protect_HeadShots
    {

        private string listField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string List
        {
            get
            {
                return this.listField;
            }
            set
            {
                this.listField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeFriendly_Wolves
    {

        private string listField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string List
        {
            get
            {
                return this.listField;
            }
            set
            {
                this.listField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeZombies_Throw_Stones_Activated
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
    public partial class typesTypeZombies_Throw_Stones_Only_If_Player_On_Obstacle
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
    public partial class typesTypeZombies_Throw_Stones_Damage_Health
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
    public partial class typesTypeZombies_Throw_Stones_Damage_Shock
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
    public partial class typesTypeZombies_Throw_Stones_Keep_Minimum_Health
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
    public partial class typesTypeZombies_Throw_Stones_Rate
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
    public partial class typesTypeZombies_Throw_Stones_Force
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
    public partial class typesTypeZombies_Throw_Stones_Distance_Maxi
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
    public partial class typesTypeAttack_Stopped_Vehicles
    {

        private int activatedField;

        private bool activatedFieldSpecified;

        private string vehicle_Type_ResistanceField;

        private bool vehicle_Type_ResistanceSpecified;

        private int moving_Vehicles_TooField;

        private bool moving_Vehicles_TooFieldSpecified;

        private decimal damage_On_StructureField;

        private bool damage_On_StructureFieldSpecified;

        private decimal damage_On_AttachmentsField;

        private bool damage_On_AttachmentsFieldSpecified;

        private decimal attack_Speed_FactorField;

        private bool attack_Speed_FactorFieldSpecified;

        private int activate_SoundsField;

        private bool activate_SoundsFieldSpecified;

        private int player_Inside_Can_Be_HitField;

        private bool player_Inside_Can_Be_HitFieldSpecified;

        private int physics_Force_ApplyField;

        private bool physics_Force_ApplyFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Activated
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActivatedSpecified
        {
            get
            {
                return this.activatedFieldSpecified;
            }
            set
            {
                this.activatedFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Vehicle_Type_Resistance
        {
            get
            {
                return this.vehicle_Type_ResistanceField;
            }
            set
            {
                this.vehicle_Type_ResistanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Vehicle_Type_ResistanceSpecified
        {
            get
            {
                return this.vehicle_Type_ResistanceSpecified;
            }
            set
            {
                this.vehicle_Type_ResistanceSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Moving_Vehicles_Too
        {
            get
            {
                return this.moving_Vehicles_TooField;
            }
            set
            {
                this.moving_Vehicles_TooField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Moving_Vehicles_TooSpecified
        {
            get
            {
                return this.moving_Vehicles_TooFieldSpecified;
            }
            set
            {
                this.moving_Vehicles_TooFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Damage_On_Structure
        {
            get
            {
                return this.damage_On_StructureField;
            }
            set
            {
                this.damage_On_StructureField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Damage_On_StructureSpecified
        {
            get
            {
                return this.damage_On_StructureFieldSpecified;
            }
            set
            {
                this.damage_On_StructureFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Damage_On_Attachments
        {
            get
            {
                return this.damage_On_AttachmentsField;
            }
            set
            {
                this.damage_On_AttachmentsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Damage_On_AttachmentsSpecified
        {
            get
            {
                return this.damage_On_AttachmentsFieldSpecified;
            }
            set
            {
                this.damage_On_AttachmentsFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Attack_Speed_Factor
        {
            get
            {
                return this.attack_Speed_FactorField;
            }
            set
            {
                this.attack_Speed_FactorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Attack_Speed_FactorSpecified
        {
            get
            {
                return this.attack_Speed_FactorFieldSpecified;
            }
            set
            {
                this.attack_Speed_FactorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Activate_Sounds
        {
            get
            {
                return this.activate_SoundsField;
            }
            set
            {
                this.activate_SoundsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Activate_SoundsSpecified
        {
            get
            {
                return this.activate_SoundsFieldSpecified;
            }
            set
            {
                this.activate_SoundsFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Player_Inside_Can_Be_Hit
        {
            get
            {
                return this.player_Inside_Can_Be_HitField;
            }
            set
            {
                this.player_Inside_Can_Be_HitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Player_Inside_Can_Be_HitSpecified
        {
            get
            {
                return this.player_Inside_Can_Be_HitFieldSpecified;
            }
            set
            {
                this.player_Inside_Can_Be_HitFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Physics_Force_Apply
        {
            get
            {
                return this.physics_Force_ApplyField;
            }
            set
            {
                this.physics_Force_ApplyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Physics_Force_ApplySpecified
        {
            get
            {
                return this.physics_Force_ApplyFieldSpecified;
            }
            set
            {
                this.physics_Force_ApplyFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeDamages_To_Vehicles
    {

        private int activatedField;

        private bool activatedFieldSpecified;

        private string vehicle_Type_ResistanceField;

        private bool vehicle_Type_ResistanceFieldSpecified;

        private decimal damages_Per_ImpactField;

        private bool damages_Per_ImpactFieldSpecified;

        private int timer_Between_ImpactsField;

        private bool timer_Between_ImpactsFieldSpecified;

        private int speed_MinimumField;

        private bool speed_MinimumFieldSpecified;

        private decimal speed_MultiplierField;

        private bool speed_MultiplierFieldSpecified;

        private int cruch_Physics_Force_FactorField;

        private bool cruch_Physics_Force_FactorFieldSpecified;

        private int cruch_Physics_Force_MiniField;

        private bool cruch_Physics_Force_MiniFieldSpecified;

        private int cruch_Physics_Force_MaxiField;

        private bool cruch_Physics_Force_MaxiFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Activated
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActivatedSpecified
        {
            get
            {
                return this.activatedFieldSpecified;
            }
            set
            {
                this.activatedFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Vehicle_Type_Resistance
        {
            get
            {
                return this.vehicle_Type_ResistanceField;
            }
            set
            {
                this.vehicle_Type_ResistanceField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Vehicle_Type_ResistanceSpecified
        {
            get
            {
                return this.vehicle_Type_ResistanceFieldSpecified;
            }
            set
            {
                this.vehicle_Type_ResistanceFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Damages_Per_Impact
        {
            get
            {
                return this.damages_Per_ImpactField;
            }
            set
            {
                this.damages_Per_ImpactField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Damages_Per_ImpactSpecified
        {
            get
            {
                return this.damages_Per_ImpactFieldSpecified;
            }
            set
            {
                this.damages_Per_ImpactFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Timer_Between_Impacts
        {
            get
            {
                return this.timer_Between_ImpactsField;
            }
            set
            {
                this.timer_Between_ImpactsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Timer_Between_ImpactsSpecified
        {
            get
            {
                return this.timer_Between_ImpactsFieldSpecified;
            }
            set
            {
                this.timer_Between_ImpactsFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Speed_Minimum
        {
            get
            {
                return this.speed_MinimumField;
            }
            set
            {
                this.speed_MinimumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Speed_MinimumSpecified
        {
            get
            {
                return this.speed_MinimumFieldSpecified;
            }
            set
            {
                this.speed_MinimumFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Speed_Multiplier
        {
            get
            {
                return this.speed_MultiplierField;
            }
            set
            {
                this.speed_MultiplierField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Speed_MultiplierSpecified
        {
            get
            {
                return this.speed_MultiplierFieldSpecified;
            }
            set
            {
                this.speed_MultiplierFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Cruch_Physics_Force_Factor
        {
            get
            {
                return this.cruch_Physics_Force_FactorField;
            }
            set
            {
                this.cruch_Physics_Force_FactorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Cruch_Physics_Force_FactorSpecified
        {
            get
            {
                return this.cruch_Physics_Force_FactorFieldSpecified;
            }
            set
            {
                this.cruch_Physics_Force_FactorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Cruch_Physics_Force_Mini
        {
            get
            {
                return this.cruch_Physics_Force_MiniField;
            }
            set
            {
                this.cruch_Physics_Force_MiniField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Cruch_Physics_Force_MiniSpecified
        {
            get
            {
                return this.cruch_Physics_Force_MiniFieldSpecified;
            }
            set
            {
                this.cruch_Physics_Force_MiniFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Cruch_Physics_Force_Maxi
        {
            get
            {
                return this.cruch_Physics_Force_MaxiField;
            }
            set
            {
                this.cruch_Physics_Force_MaxiField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Cruch_Physics_Force_MaxiSpecified
        {
            get
            {
                return this.cruch_Physics_Force_MaxiFieldSpecified;
            }
            set
            {
                this.cruch_Physics_Force_MaxiFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeOverRide_Vanilla_Night_Time
    {

        private int activatedField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Activated
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeNight_Beginning
    {

        private string timeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeNight_End
    {

        private string timeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeMaximum_Number_Of_Bleeding_Sources_From_Zombies_Combat
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
    public partial class typesTypeShoes_Degradation_When_Hit_By_Crawling_Zombie
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
    public partial class typesTypeFist_Fighting
    {

        private decimal bleeding_Chance_If_Fist_Fighting_Without_GlovesField;

        private bool bleeding_Chance_If_Fist_Fighting_Without_GlovesFieldSpecified;

        private decimal player_Dammage_If_Fist_Fighting_Without_GlovesField;

        private bool player_Dammage_If_Fist_Fighting_Without_GlovesFieldSpecified;

        private decimal player_Health_Limite_To_Take_Dammage_From_Fist_FightingField;

        private bool player_Health_Limite_To_Take_Dammage_From_Fist_FightingFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Bleeding_Chance_If_Fist_Fighting_Without_Gloves
        {
            get
            {
                return this.bleeding_Chance_If_Fist_Fighting_Without_GlovesField;
            }
            set
            {
                this.bleeding_Chance_If_Fist_Fighting_Without_GlovesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Bleeding_Chance_If_Fist_Fighting_Without_GlovesSpecified
        {
            get
            {
                return this.bleeding_Chance_If_Fist_Fighting_Without_GlovesFieldSpecified;
            }
            set
            {
                this.bleeding_Chance_If_Fist_Fighting_Without_GlovesFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Player_Dammage_If_Fist_Fighting_Without_Gloves
        {
            get
            {
                return this.player_Dammage_If_Fist_Fighting_Without_GlovesField;
            }
            set
            {
                this.player_Dammage_If_Fist_Fighting_Without_GlovesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Player_Dammage_If_Fist_Fighting_Without_GlovesSpecified
        {
            get
            {
                return this.player_Dammage_If_Fist_Fighting_Without_GlovesFieldSpecified;
            }
            set
            {
                this.player_Dammage_If_Fist_Fighting_Without_GlovesFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Player_Health_Limite_To_Take_Dammage_From_Fist_Fighting
        {
            get
            {
                return this.player_Health_Limite_To_Take_Dammage_From_Fist_FightingField;
            }
            set
            {
                this.player_Health_Limite_To_Take_Dammage_From_Fist_FightingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Player_Health_Limite_To_Take_Dammage_From_Fist_FightingSpecified
        {
            get
            {
                return this.player_Health_Limite_To_Take_Dammage_From_Fist_FightingFieldSpecified;
            }
            set
            {
                this.player_Health_Limite_To_Take_Dammage_From_Fist_FightingFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeSpecial_Mask_To_Hide_From_Zombies
    {

        private string listField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string List
        {
            get
            {
                return this.listField;
            }
            set
            {
                this.listField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeSpecial_Mask_Type_Of_Slot
    {

        private string slotField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Slot
        {
            get
            {
                return this.slotField;
            }
            set
            {
                this.slotField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeSpecial_Mask_Lifetime
    {

        private decimal minutesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Minutes
        {
            get
            {
                return this.minutesField;
            }
            set
            {
                this.minutesField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeMulti_Hits_On_Heavy_Attack
    {

        private decimal radiusField;

        private bool radiusFieldSpecified;

        private decimal dammage_RatioField;

        private bool dammage_RatioFieldSpecified;

        private int min_Dammage_To_ActivateField;

        private bool min_Dammage_To_ActivateFieldSpecified;

        private int stamina_BonusField;

        private bool stamina_BonusFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Radius
        {
            get
            {
                return this.radiusField;
            }
            set
            {
                this.radiusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RadiusSpecified
        {
            get
            {
                return this.radiusFieldSpecified;
            }
            set
            {
                this.radiusFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Dammage_Ratio
        {
            get
            {
                return this.dammage_RatioField;
            }
            set
            {
                this.dammage_RatioField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Dammage_RatioSpecified
        {
            get
            {
                return this.dammage_RatioFieldSpecified;
            }
            set
            {
                this.dammage_RatioFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Min_Dammage_To_Activate
        {
            get
            {
                return this.min_Dammage_To_ActivateField;
            }
            set
            {
                this.min_Dammage_To_ActivateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Min_Dammage_To_ActivateSpecified
        {
            get
            {
                return this.min_Dammage_To_ActivateFieldSpecified;
            }
            set
            {
                this.min_Dammage_To_ActivateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Stamina_Bonus
        {
            get
            {
                return this.stamina_BonusField;
            }
            set
            {
                this.stamina_BonusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Stamina_BonusSpecified
        {
            get
            {
                return this.stamina_BonusFieldSpecified;
            }
            set
            {
                this.stamina_BonusFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeZombies_Health_Ratio
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
    public partial class typesTypeZombies_Strength_Ratio
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
    public partial class typesTypeZombies_Speed_Ratio
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
    public partial class typesTypeZombies_Clamp_Speed_Mini
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
    public partial class typesTypeZombies_Clamp_Speed_Maxi
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
    public partial class typesTypeZombies_Hearing_Standing_Players_Ratio
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
    public partial class typesTypeZombies_Hearing_Crouching_Players_Ratio
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
    public partial class typesTypeZombies_Size_Ratio
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
    public partial class typesTypeZombies_Stun_By_Bullet_Resistance_Ratio
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
    public partial class typesTypeZombies_Maximum_Size_In_Buildings
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
    public partial class DayNight
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
    public partial class typesTypeZombies_Vision_Activated
    {

        private int dayField;

        private int nightField;

        private int withBloodyHandsField;

        private int withSpecialMaskField;

        private int miniMumRatioDistanceField;

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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int WithBloodyHands
        {
            get
            {
                return this.withBloodyHandsField;
            }
            set
            {
                this.withBloodyHandsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int WithSpecialMask
        {
            get
            {
                return this.withSpecialMaskField;
            }
            set
            {
                this.withSpecialMaskField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int MiniMumRatioDistance
        {
            get
            {
                return this.miniMumRatioDistanceField;
            }
            set
            {
                this.miniMumRatioDistanceField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeCustomisable_Zombies_List
    {

        private string file_NameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string File_Name
        {
            get
            {
                return this.file_NameField;
            }
            set
            {
                this.file_NameField = value;
            }
        }
    }


}
