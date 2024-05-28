using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PVZ.CustomZombiesCharacteristics
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class types
    {

        private BindingList<typesType> typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("type")]
        public BindingList<typesType> type
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

        private DayNightInt health_PointsField;

        private DayNightDecimal resistance_to_BulletsField;

        private DayNightDecimal resistance_to_MeleesField;

        private DayNightDecimal resistance_to_HeavyAttackField;

        private DayNightDecimal resistance_to_VehiclesField;

        private DayNightDecimal resistance_to_ExplosionsField;

        private DayNightDecimal resistance_to_Stun_BulletsField;

        private DayNightDecimal resistance_to_HeadShotsField;

        private DayNightDecimal resist_to_Melee_HeadShotsField;

        private DayNightInt special_HeadShot_WeaponsField;

        private DayNightDecimal move_Speed_MinField;

        private DayNightDecimal move_Speed_MaxField;

        private DayNightDecimal move_Speed_Adjust_MaxField;

        private DayNightInt animation_TypeField;

        private DayNightDecimal chance_To_Spawn_CrawlingField;

        private DayNightInt can_DodgeField;

        private DayNightInt hit_Players_On_ObstaclesField;

        private DayNightInt immune_To_MultiHitField;

        private DayNightDecimal attack_SpeedField;

        private BlockingStuff ratio_Damage_HealthField;

        private BlockingStuff ratio_Damage_ShockField;

        private BlockingStuff bleeding_ChanceField;

        private BlockingStuff damage_BloodField;

        private BlockingStuff damage_StaminaField;

        private DayNightBloodHandsSpecialMaskMinRatioDistance vision_Distance_RatioField;

        private DayNightInt can_Be_BackstabbedField;

        private DayNightInt resist_Contaminated_EffectField;

        private DayNightInt numb_Of_Hit_To_Break_DoorsField;

        private DayNightDecimal size_MiniField;

        private DayNightDecimal size_MaxiField;

        private DayNightInt can_Throw_StonesField;

        private string nameField;

        /// <remarks/>
        public DayNightInt Health_Points
        {
            get
            {
                return this.health_PointsField;
            }
            set
            {
                this.health_PointsField = value;
            }
        }

        /// <remarks/>
        public DayNightDecimal Resistance_to_Bullets
        {
            get
            {
                return this.resistance_to_BulletsField;
            }
            set
            {
                this.resistance_to_BulletsField = value;
            }
        }

        /// <remarks/>
        public DayNightDecimal Resistance_to_Melees
        {
            get
            {
                return this.resistance_to_MeleesField;
            }
            set
            {
                this.resistance_to_MeleesField = value;
            }
        }

        /// <remarks/>
        public DayNightDecimal Resistance_to_HeavyAttack
        {
            get
            {
                return this.resistance_to_HeavyAttackField;
            }
            set
            {
                this.resistance_to_HeavyAttackField = value;
            }
        }

        /// <remarks/>
        public DayNightDecimal Resistance_to_Vehicles
        {
            get
            {
                return this.resistance_to_VehiclesField;
            }
            set
            {
                this.resistance_to_VehiclesField = value;
            }
        }

        /// <remarks/>
        public DayNightDecimal Resistance_to_Explosions
        {
            get
            {
                return this.resistance_to_ExplosionsField;
            }
            set
            {
                this.resistance_to_ExplosionsField = value;
            }
        }

        /// <remarks/>
        public DayNightDecimal Resistance_to_Stun_Bullets
        {
            get
            {
                return this.resistance_to_Stun_BulletsField;
            }
            set
            {
                this.resistance_to_Stun_BulletsField = value;
            }
        }

        /// <remarks/>
        public DayNightDecimal Resistance_to_HeadShots
        {
            get
            {
                return this.resistance_to_HeadShotsField;
            }
            set
            {
                this.resistance_to_HeadShotsField = value;
            }
        }

        /// <remarks/>
        public DayNightDecimal Resist_to_Melee_HeadShots
        {
            get
            {
                return this.resist_to_Melee_HeadShotsField;
            }
            set
            {
                this.resist_to_Melee_HeadShotsField = value;
            }
        }

        /// <remarks/>
        public DayNightInt Special_HeadShot_Weapons
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
        public DayNightDecimal Move_Speed_Min
        {
            get
            {
                return this.move_Speed_MinField;
            }
            set
            {
                this.move_Speed_MinField = value;
            }
        }

        /// <remarks/>
        public DayNightDecimal Move_Speed_Max
        {
            get
            {
                return this.move_Speed_MaxField;
            }
            set
            {
                this.move_Speed_MaxField = value;
            }
        }

        /// <remarks/>
        public DayNightDecimal Move_Speed_Adjust_Max
        {
            get
            {
                return this.move_Speed_Adjust_MaxField;
            }
            set
            {
                this.move_Speed_Adjust_MaxField = value;
            }
        }

        /// <remarks/>
        public DayNightInt Animation_Type
        {
            get
            {
                return this.animation_TypeField;
            }
            set
            {
                this.animation_TypeField = value;
            }
        }

        /// <remarks/>
        public DayNightDecimal Chance_To_Spawn_Crawling
        {
            get
            {
                return this.chance_To_Spawn_CrawlingField;
            }
            set
            {
                this.chance_To_Spawn_CrawlingField = value;
            }
        }

        /// <remarks/>
        public DayNightInt Can_Dodge
        {
            get
            {
                return this.can_DodgeField;
            }
            set
            {
                this.can_DodgeField = value;
            }
        }

        /// <remarks/>
        public DayNightInt Hit_Players_On_Obstacles
        {
            get
            {
                return this.hit_Players_On_ObstaclesField;
            }
            set
            {
                this.hit_Players_On_ObstaclesField = value;
            }
        }

        /// <remarks/>
        public DayNightInt Immune_To_MultiHit
        {
            get
            {
                return this.immune_To_MultiHitField;
            }
            set
            {
                this.immune_To_MultiHitField = value;
            }
        }

        /// <remarks/>
        public DayNightDecimal Attack_Speed
        {
            get
            {
                return this.attack_SpeedField;
            }
            set
            {
                this.attack_SpeedField = value;
            }
        }

        /// <remarks/>
        public BlockingStuff Ratio_Damage_Health
        {
            get
            {
                return this.ratio_Damage_HealthField;
            }
            set
            {
                this.ratio_Damage_HealthField = value;
            }
        }

        /// <remarks/>
        public BlockingStuff Ratio_Damage_Shock
        {
            get
            {
                return this.ratio_Damage_ShockField;
            }
            set
            {
                this.ratio_Damage_ShockField = value;
            }
        }

        /// <remarks/>
        public BlockingStuff Bleeding_Chance
        {
            get
            {
                return this.bleeding_ChanceField;
            }
            set
            {
                this.bleeding_ChanceField = value;
            }
        }

        /// <remarks/>
        public BlockingStuff Damage_Blood
        {
            get
            {
                return this.damage_BloodField;
            }
            set
            {
                this.damage_BloodField = value;
            }
        }

        /// <remarks/>
        public BlockingStuff Damage_Stamina
        {
            get
            {
                return this.damage_StaminaField;
            }
            set
            {
                this.damage_StaminaField = value;
            }
        }

        /// <remarks/>
        public DayNightBloodHandsSpecialMaskMinRatioDistance Vision_Distance_Ratio
        {
            get
            {
                return this.vision_Distance_RatioField;
            }
            set
            {
                this.vision_Distance_RatioField = value;
            }
        }

        /// <remarks/>
        public DayNightInt Can_Be_Backstabbed
        {
            get
            {
                return this.can_Be_BackstabbedField;
            }
            set
            {
                this.can_Be_BackstabbedField = value;
            }
        }

        /// <remarks/>
        public DayNightInt Resist_Contaminated_Effect
        {
            get
            {
                return this.resist_Contaminated_EffectField;
            }
            set
            {
                this.resist_Contaminated_EffectField = value;
            }
        }

        /// <remarks/>
        public DayNightInt Numb_Of_Hit_To_Break_Doors
        {
            get
            {
                return this.numb_Of_Hit_To_Break_DoorsField;
            }
            set
            {
                this.numb_Of_Hit_To_Break_DoorsField = value;
            }
        }

        /// <remarks/>
        public DayNightDecimal Size_Mini
        {
            get
            {
                return this.size_MiniField;
            }
            set
            {
                this.size_MiniField = value;
            }
        }

        /// <remarks/>
        public DayNightDecimal Size_Maxi
        {
            get
            {
                return this.size_MaxiField;
            }
            set
            {
                this.size_MaxiField = value;
            }
        }

        /// <remarks/>
        public DayNightInt Can_Throw_Stones
        {
            get
            {
                return this.can_Throw_StonesField;
            }
            set
            {
                this.can_Throw_StonesField = value;
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
    public partial class BlockingStuff
    {

        private decimal lightAttack_NotBlockedField;

        private decimal lightAttack_BlockedField;

        private decimal heavyAttack_NotBlockedField;

        private decimal heavyAttack_BlockedField;

        private decimal nightRatioField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal LightAttack_NotBlocked
        {
            get
            {
                return this.lightAttack_NotBlockedField;
            }
            set
            {
                this.lightAttack_NotBlockedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal LightAttack_Blocked
        {
            get
            {
                return this.lightAttack_BlockedField;
            }
            set
            {
                this.lightAttack_BlockedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal HeavyAttack_NotBlocked
        {
            get
            {
                return this.heavyAttack_NotBlockedField;
            }
            set
            {
                this.heavyAttack_NotBlockedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal HeavyAttack_Blocked
        {
            get
            {
                return this.heavyAttack_BlockedField;
            }
            set
            {
                this.heavyAttack_BlockedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal NightRatio
        {
            get
            {
                return this.nightRatioField;
            }
            set
            {
                this.nightRatioField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class DayNightBloodHandsSpecialMaskMinRatioDistance
    {

        private decimal dayField;

        private decimal nightField;

        private decimal withBloodyHandsField;

        private decimal withSpecialMaskField;

        private decimal minimumRatioDistanceField;

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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal WithBloodyHands
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
        public decimal WithSpecialMask
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
        public decimal MinimumRatioDistance
        {
            get
            {
                return this.minimumRatioDistanceField;
            }
            set
            {
                this.minimumRatioDistanceField = value;
            }
        }
    }


    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class DayNightDecimal
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
    public partial class DayNightInt
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


}
