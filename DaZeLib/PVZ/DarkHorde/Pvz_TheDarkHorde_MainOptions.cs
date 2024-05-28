using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVZ.DarkHordeMainOptions
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

        private typesTypeTime_Before_The_Horde_Respawn_After_Been_Defeated time_Before_The_Horde_Respawn_After_Been_DefeatedField;

        private typesTypePersistant_Position_When_Server_Restart persistant_Position_When_Server_RestartField;

        private typesTypeSecurity_Distance_To_Avoid_Horde_Spawning_On_Players security_Distance_To_Avoid_Horde_Spawning_On_PlayersField;

        private typesTypeActivate_Bandit_And_Heroes_mod_Rewards activate_Bandit_And_Heroes_mod_RewardsField;

        private typesTypeMinimum_Player_Number_To_Activate_The_Horde minimum_Player_Number_To_Activate_The_HordeField;

        private typesTypeMaximum_Player_Number_To_Activate_The_Horde maximum_Player_Number_To_Activate_The_HordeField;

        private typesTypeHorde_Speed_When_Calm horde_Speed_When_CalmField;

        private typesTypeHorde_Speed_When_Not_Calm horde_Speed_When_Not_CalmField;

        private typesTypeHorde_Speed_Ratio_When_No_Player_Around horde_Speed_Ratio_When_No_Player_AroundField;

        private typesTypeDistance_Between_Random_Direction_Changes distance_Between_Random_Direction_ChangesField;

        private typesTypeMinimum_Of_Players_To_Activate_The_Hunt minimum_Of_Players_To_Activate_The_HuntField;

        private typesTypeMaximum_Number_Of_Hunt_Per_Session maximum_Number_Of_Hunt_Per_SessionField;

        private typesTypeDistance_To_Player_To_Stop_Hunting distance_To_Player_To_Stop_HuntingField;

        private typesTypeStart_Waypoint_Number start_Waypoint_NumberField;

        private typesTypeRandom_Waypoint_Order random_Waypoint_OrderField;

        private typesTypeStart_Position start_PositionField;

        private typesTypeEnd_Position end_PositionField;

        private typesTypeNumber_Of_Zombies number_Of_ZombiesField;

        private typesTypeHorde_Movement_Type horde_Movement_TypeField;

        private string nameField;

        /// <remarks/>
        public typesTypeTime_Before_The_Horde_Respawn_After_Been_Defeated Time_Before_The_Horde_Respawn_After_Been_Defeated
        {
            get
            {
                return this.time_Before_The_Horde_Respawn_After_Been_DefeatedField;
            }
            set
            {
                this.time_Before_The_Horde_Respawn_After_Been_DefeatedField = value;
            }
        }

        /// <remarks/>
        public typesTypePersistant_Position_When_Server_Restart Persistant_Position_When_Server_Restart
        {
            get
            {
                return this.persistant_Position_When_Server_RestartField;
            }
            set
            {
                this.persistant_Position_When_Server_RestartField = value;
            }
        }

        /// <remarks/>
        public typesTypeSecurity_Distance_To_Avoid_Horde_Spawning_On_Players Security_Distance_To_Avoid_Horde_Spawning_On_Players
        {
            get
            {
                return this.security_Distance_To_Avoid_Horde_Spawning_On_PlayersField;
            }
            set
            {
                this.security_Distance_To_Avoid_Horde_Spawning_On_PlayersField = value;
            }
        }

        /// <remarks/>
        public typesTypeActivate_Bandit_And_Heroes_mod_Rewards Activate_Bandit_And_Heroes_mod_Rewards
        {
            get
            {
                return this.activate_Bandit_And_Heroes_mod_RewardsField;
            }
            set
            {
                this.activate_Bandit_And_Heroes_mod_RewardsField = value;
            }
        }

        /// <remarks/>
        public typesTypeMinimum_Player_Number_To_Activate_The_Horde Minimum_Player_Number_To_Activate_The_Horde
        {
            get
            {
                return this.minimum_Player_Number_To_Activate_The_HordeField;
            }
            set
            {
                this.minimum_Player_Number_To_Activate_The_HordeField = value;
            }
        }

        /// <remarks/>
        public typesTypeMaximum_Player_Number_To_Activate_The_Horde Maximum_Player_Number_To_Activate_The_Horde
        {
            get
            {
                return this.maximum_Player_Number_To_Activate_The_HordeField;
            }
            set
            {
                this.maximum_Player_Number_To_Activate_The_HordeField = value;
            }
        }

        /// <remarks/>
        public typesTypeHorde_Speed_When_Calm Horde_Speed_When_Calm
        {
            get
            {
                return this.horde_Speed_When_CalmField;
            }
            set
            {
                this.horde_Speed_When_CalmField = value;
            }
        }

        /// <remarks/>
        public typesTypeHorde_Speed_When_Not_Calm Horde_Speed_When_Not_Calm
        {
            get
            {
                return this.horde_Speed_When_Not_CalmField;
            }
            set
            {
                this.horde_Speed_When_Not_CalmField = value;
            }
        }

        /// <remarks/>
        public typesTypeHorde_Speed_Ratio_When_No_Player_Around Horde_Speed_Ratio_When_No_Player_Around
        {
            get
            {
                return this.horde_Speed_Ratio_When_No_Player_AroundField;
            }
            set
            {
                this.horde_Speed_Ratio_When_No_Player_AroundField = value;
            }
        }

        /// <remarks/>
        public typesTypeDistance_Between_Random_Direction_Changes Distance_Between_Random_Direction_Changes
        {
            get
            {
                return this.distance_Between_Random_Direction_ChangesField;
            }
            set
            {
                this.distance_Between_Random_Direction_ChangesField = value;
            }
        }

        /// <remarks/>
        public typesTypeMinimum_Of_Players_To_Activate_The_Hunt Minimum_Of_Players_To_Activate_The_Hunt
        {
            get
            {
                return this.minimum_Of_Players_To_Activate_The_HuntField;
            }
            set
            {
                this.minimum_Of_Players_To_Activate_The_HuntField = value;
            }
        }

        /// <remarks/>
        public typesTypeMaximum_Number_Of_Hunt_Per_Session Maximum_Number_Of_Hunt_Per_Session
        {
            get
            {
                return this.maximum_Number_Of_Hunt_Per_SessionField;
            }
            set
            {
                this.maximum_Number_Of_Hunt_Per_SessionField = value;
            }
        }

        /// <remarks/>
        public typesTypeDistance_To_Player_To_Stop_Hunting Distance_To_Player_To_Stop_Hunting
        {
            get
            {
                return this.distance_To_Player_To_Stop_HuntingField;
            }
            set
            {
                this.distance_To_Player_To_Stop_HuntingField = value;
            }
        }

        /// <remarks/>
        public typesTypeStart_Waypoint_Number Start_Waypoint_Number
        {
            get
            {
                return this.start_Waypoint_NumberField;
            }
            set
            {
                this.start_Waypoint_NumberField = value;
            }
        }

        /// <remarks/>
        public typesTypeRandom_Waypoint_Order Random_Waypoint_Order
        {
            get
            {
                return this.random_Waypoint_OrderField;
            }
            set
            {
                this.random_Waypoint_OrderField = value;
            }
        }

        /// <remarks/>
        public typesTypeStart_Position Start_Position
        {
            get
            {
                return this.start_PositionField;
            }
            set
            {
                this.start_PositionField = value;
            }
        }

        /// <remarks/>
        public typesTypeEnd_Position End_Position
        {
            get
            {
                return this.end_PositionField;
            }
            set
            {
                this.end_PositionField = value;
            }
        }

        /// <remarks/>
        public typesTypeNumber_Of_Zombies Number_Of_Zombies
        {
            get
            {
                return this.number_Of_ZombiesField;
            }
            set
            {
                this.number_Of_ZombiesField = value;
            }
        }

        /// <remarks/>
        public typesTypeHorde_Movement_Type Horde_Movement_Type
        {
            get
            {
                return this.horde_Movement_TypeField;
            }
            set
            {
                this.horde_Movement_TypeField = value;
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
    public partial class typesTypeTime_Before_The_Horde_Respawn_After_Been_Defeated
    {

        private ushort miniField;

        private ushort maxiField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort Mini
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
        public ushort Maxi
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
    public partial class typesTypePersistant_Position_When_Server_Restart
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
    public partial class typesTypeSecurity_Distance_To_Avoid_Horde_Spawning_On_Players
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
    public partial class typesTypeActivate_Bandit_And_Heroes_mod_Rewards
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
    public partial class typesTypeMinimum_Player_Number_To_Activate_The_Horde
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
    public partial class typesTypeMaximum_Player_Number_To_Activate_The_Horde
    {

        private ushort dayField;

        private ushort nightField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort Day
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
        public ushort Night
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
    public partial class typesTypeHorde_Speed_When_Calm
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
    public partial class typesTypeHorde_Speed_When_Not_Calm
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
    public partial class typesTypeHorde_Speed_Ratio_When_No_Player_Around
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
    public partial class typesTypeDistance_Between_Random_Direction_Changes
    {

        private ushort miniField;

        private ushort maxiField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort Mini
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
        public ushort Maxi
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
    public partial class typesTypeMinimum_Of_Players_To_Activate_The_Hunt
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
    public partial class typesTypeMaximum_Number_Of_Hunt_Per_Session
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
    public partial class typesTypeDistance_To_Player_To_Stop_Hunting
    {

        private ushort dayField;

        private ushort nightField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort Day
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
        public ushort Night
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
    public partial class typesTypeStart_Waypoint_Number
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
    public partial class typesTypeRandom_Waypoint_Order
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
    public partial class typesTypeStart_Position
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
    public partial class typesTypeEnd_Position
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
    public partial class typesTypeNumber_Of_Zombies
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
    public partial class typesTypeHorde_Movement_Type
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


}
