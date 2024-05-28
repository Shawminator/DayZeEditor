using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVZ.DarkHorde.Other.EventManager
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

        private typesTypeBlack_Screen_Timer black_Screen_TimerField;

        private typesTypeInvincibility_Timer invincibility_TimerField;

        private typesTypeTeleport_Players_On_Connection teleport_Players_On_ConnectionField;

        private typesTypeTeleport_Players_On_Respawn teleport_Players_On_RespawnField;

        private typesTypeItems_To_Identify_Bases items_To_Identify_BasesField;

        private typesTypeMinimum_Distance_From_Bases_To_Spawn_The_Horde minimum_Distance_From_Bases_To_Spawn_The_HordeField;

        private typesTypeMinimum_Distance_From_Bases_To_Spawn_The_Player minimum_Distance_From_Bases_To_Spawn_The_PlayerField;

        private typesTypeTeleport_The_Players_Jumping_In_The_Anomalie teleport_The_Players_Jumping_In_The_AnomalieField;

        private typesTypeTeleport_All_Players teleport_All_PlayersField;

        private typesTypeTeleport_Players_Who_Are_In_A_Safe_Location teleport_Players_Who_Are_In_A_Safe_LocationField;

        private typesTypeTeleport_Players_Who_Are_Not_In_A_Safe_Location teleport_Players_Who_Are_Not_In_A_Safe_LocationField;

        private typesTypeTeleport_The_Players_Wearing_An_Apsi teleport_The_Players_Wearing_An_ApsiField;

        private typesTypeTeleport_The_Players_Not_Wearing_An_Apsi teleport_The_Players_Not_Wearing_An_ApsiField;

        private typesTypeDistance_To_The_Horde distance_To_The_HordeField;

        private typesTypeDistance_To_The_Other_Players distance_To_The_Other_PlayersField;

        private typesTypeTeleport_The_Horde_To_An_Empty_Place teleport_The_Horde_To_An_Empty_PlaceField;

        private typesTypeTeleport_Zone_Limits teleport_Zone_LimitsField;

        private typesTypeAllowed_Terrain_Heights allowed_Terrain_HeightsField;

        private typesTypeMinimum_Distance_To_Non_Teleported_Players minimum_Distance_To_Non_Teleported_PlayersField;

        private typesTypeHorde_Spawn_Timer horde_Spawn_TimerField;

        private typesTypeHorde_Lifetime horde_LifetimeField;

        private typesTypeHorde_Created_Or_Teleported_Only_If_At_Least_One_Player_Is_Teleported horde_Created_Or_Teleported_Only_If_At_Least_One_Player_Is_TeleportedField;

        private typesTypeActivate_Namalsk_Event_System activate_Namalsk_Event_SystemField;

        private typesTypeActivate_Custom_Event_System activate_Custom_Event_SystemField;

        private string nameField;

        /// <remarks/>
        public typesTypeBlack_Screen_Timer Black_Screen_Timer
        {
            get
            {
                return this.black_Screen_TimerField;
            }
            set
            {
                this.black_Screen_TimerField = value;
            }
        }

        /// <remarks/>
        public typesTypeInvincibility_Timer Invincibility_Timer
        {
            get
            {
                return this.invincibility_TimerField;
            }
            set
            {
                this.invincibility_TimerField = value;
            }
        }

        /// <remarks/>
        public typesTypeTeleport_Players_On_Connection Teleport_Players_On_Connection
        {
            get
            {
                return this.teleport_Players_On_ConnectionField;
            }
            set
            {
                this.teleport_Players_On_ConnectionField = value;
            }
        }

        /// <remarks/>
        public typesTypeTeleport_Players_On_Respawn Teleport_Players_On_Respawn
        {
            get
            {
                return this.teleport_Players_On_RespawnField;
            }
            set
            {
                this.teleport_Players_On_RespawnField = value;
            }
        }

        /// <remarks/>
        public typesTypeItems_To_Identify_Bases Items_To_Identify_Bases
        {
            get
            {
                return this.items_To_Identify_BasesField;
            }
            set
            {
                this.items_To_Identify_BasesField = value;
            }
        }

        /// <remarks/>
        public typesTypeMinimum_Distance_From_Bases_To_Spawn_The_Horde Minimum_Distance_From_Bases_To_Spawn_The_Horde
        {
            get
            {
                return this.minimum_Distance_From_Bases_To_Spawn_The_HordeField;
            }
            set
            {
                this.minimum_Distance_From_Bases_To_Spawn_The_HordeField = value;
            }
        }

        /// <remarks/>
        public typesTypeMinimum_Distance_From_Bases_To_Spawn_The_Player Minimum_Distance_From_Bases_To_Spawn_The_Player
        {
            get
            {
                return this.minimum_Distance_From_Bases_To_Spawn_The_PlayerField;
            }
            set
            {
                this.minimum_Distance_From_Bases_To_Spawn_The_PlayerField = value;
            }
        }

        /// <remarks/>
        public typesTypeTeleport_The_Players_Jumping_In_The_Anomalie Teleport_The_Players_Jumping_In_The_Anomalie
        {
            get
            {
                return this.teleport_The_Players_Jumping_In_The_AnomalieField;
            }
            set
            {
                this.teleport_The_Players_Jumping_In_The_AnomalieField = value;
            }
        }

        /// <remarks/>
        public typesTypeTeleport_All_Players Teleport_All_Players
        {
            get
            {
                return this.teleport_All_PlayersField;
            }
            set
            {
                this.teleport_All_PlayersField = value;
            }
        }

        /// <remarks/>
        public typesTypeTeleport_Players_Who_Are_In_A_Safe_Location Teleport_Players_Who_Are_In_A_Safe_Location
        {
            get
            {
                return this.teleport_Players_Who_Are_In_A_Safe_LocationField;
            }
            set
            {
                this.teleport_Players_Who_Are_In_A_Safe_LocationField = value;
            }
        }

        /// <remarks/>
        public typesTypeTeleport_Players_Who_Are_Not_In_A_Safe_Location Teleport_Players_Who_Are_Not_In_A_Safe_Location
        {
            get
            {
                return this.teleport_Players_Who_Are_Not_In_A_Safe_LocationField;
            }
            set
            {
                this.teleport_Players_Who_Are_Not_In_A_Safe_LocationField = value;
            }
        }

        /// <remarks/>
        public typesTypeTeleport_The_Players_Wearing_An_Apsi Teleport_The_Players_Wearing_An_Apsi
        {
            get
            {
                return this.teleport_The_Players_Wearing_An_ApsiField;
            }
            set
            {
                this.teleport_The_Players_Wearing_An_ApsiField = value;
            }
        }

        /// <remarks/>
        public typesTypeTeleport_The_Players_Not_Wearing_An_Apsi Teleport_The_Players_Not_Wearing_An_Apsi
        {
            get
            {
                return this.teleport_The_Players_Not_Wearing_An_ApsiField;
            }
            set
            {
                this.teleport_The_Players_Not_Wearing_An_ApsiField = value;
            }
        }

        /// <remarks/>
        public typesTypeDistance_To_The_Horde Distance_To_The_Horde
        {
            get
            {
                return this.distance_To_The_HordeField;
            }
            set
            {
                this.distance_To_The_HordeField = value;
            }
        }

        /// <remarks/>
        public typesTypeDistance_To_The_Other_Players Distance_To_The_Other_Players
        {
            get
            {
                return this.distance_To_The_Other_PlayersField;
            }
            set
            {
                this.distance_To_The_Other_PlayersField = value;
            }
        }

        /// <remarks/>
        public typesTypeTeleport_The_Horde_To_An_Empty_Place Teleport_The_Horde_To_An_Empty_Place
        {
            get
            {
                return this.teleport_The_Horde_To_An_Empty_PlaceField;
            }
            set
            {
                this.teleport_The_Horde_To_An_Empty_PlaceField = value;
            }
        }

        /// <remarks/>
        public typesTypeTeleport_Zone_Limits Teleport_Zone_Limits
        {
            get
            {
                return this.teleport_Zone_LimitsField;
            }
            set
            {
                this.teleport_Zone_LimitsField = value;
            }
        }

        /// <remarks/>
        public typesTypeAllowed_Terrain_Heights Allowed_Terrain_Heights
        {
            get
            {
                return this.allowed_Terrain_HeightsField;
            }
            set
            {
                this.allowed_Terrain_HeightsField = value;
            }
        }

        /// <remarks/>
        public typesTypeMinimum_Distance_To_Non_Teleported_Players Minimum_Distance_To_Non_Teleported_Players
        {
            get
            {
                return this.minimum_Distance_To_Non_Teleported_PlayersField;
            }
            set
            {
                this.minimum_Distance_To_Non_Teleported_PlayersField = value;
            }
        }

        /// <remarks/>
        public typesTypeHorde_Spawn_Timer Horde_Spawn_Timer
        {
            get
            {
                return this.horde_Spawn_TimerField;
            }
            set
            {
                this.horde_Spawn_TimerField = value;
            }
        }

        /// <remarks/>
        public typesTypeHorde_Lifetime Horde_Lifetime
        {
            get
            {
                return this.horde_LifetimeField;
            }
            set
            {
                this.horde_LifetimeField = value;
            }
        }

        /// <remarks/>
        public typesTypeHorde_Created_Or_Teleported_Only_If_At_Least_One_Player_Is_Teleported Horde_Created_Or_Teleported_Only_If_At_Least_One_Player_Is_Teleported
        {
            get
            {
                return this.horde_Created_Or_Teleported_Only_If_At_Least_One_Player_Is_TeleportedField;
            }
            set
            {
                this.horde_Created_Or_Teleported_Only_If_At_Least_One_Player_Is_TeleportedField = value;
            }
        }

        /// <remarks/>
        public typesTypeActivate_Namalsk_Event_System Activate_Namalsk_Event_System
        {
            get
            {
                return this.activate_Namalsk_Event_SystemField;
            }
            set
            {
                this.activate_Namalsk_Event_SystemField = value;
            }
        }

        /// <remarks/>
        public typesTypeActivate_Custom_Event_System Activate_Custom_Event_System
        {
            get
            {
                return this.activate_Custom_Event_SystemField;
            }
            set
            {
                this.activate_Custom_Event_SystemField = value;
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
    public partial class typesTypeBlack_Screen_Timer
    {

        private byte for_Namalsk_EVR_Storm_EventField;

        private byte for_Custom_EventField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte For_Namalsk_EVR_Storm_Event
        {
            get
            {
                return this.for_Namalsk_EVR_Storm_EventField;
            }
            set
            {
                this.for_Namalsk_EVR_Storm_EventField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte For_Custom_Event
        {
            get
            {
                return this.for_Custom_EventField;
            }
            set
            {
                this.for_Custom_EventField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeInvincibility_Timer
    {

        private byte for_Namalsk_EVR_Storm_EventField;

        private byte for_Custom_EventField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte For_Namalsk_EVR_Storm_Event
        {
            get
            {
                return this.for_Namalsk_EVR_Storm_EventField;
            }
            set
            {
                this.for_Namalsk_EVR_Storm_EventField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte For_Custom_Event
        {
            get
            {
                return this.for_Custom_EventField;
            }
            set
            {
                this.for_Custom_EventField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeTeleport_Players_On_Connection
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
    public partial class typesTypeTeleport_Players_On_Respawn
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
    public partial class typesTypeItems_To_Identify_Bases
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
    public partial class typesTypeMinimum_Distance_From_Bases_To_Spawn_The_Horde
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
    public partial class typesTypeMinimum_Distance_From_Bases_To_Spawn_The_Player
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
    public partial class typesTypeTeleport_The_Players_Jumping_In_The_Anomalie
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
    public partial class typesTypeTeleport_All_Players
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
    public partial class typesTypeTeleport_Players_Who_Are_In_A_Safe_Location
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
    public partial class typesTypeTeleport_Players_Who_Are_Not_In_A_Safe_Location
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
    public partial class typesTypeTeleport_The_Players_Wearing_An_Apsi
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
    public partial class typesTypeTeleport_The_Players_Not_Wearing_An_Apsi
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
    public partial class typesTypeDistance_To_The_Horde
    {

        private byte minimumField;

        private byte maximumField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte Minimum
        {
            get
            {
                return this.minimumField;
            }
            set
            {
                this.minimumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte Maximum
        {
            get
            {
                return this.maximumField;
            }
            set
            {
                this.maximumField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeDistance_To_The_Other_Players
    {

        private byte minimumField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte Minimum
        {
            get
            {
                return this.minimumField;
            }
            set
            {
                this.minimumField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeTeleport_The_Horde_To_An_Empty_Place
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
    public partial class typesTypeTeleport_Zone_Limits
    {

        private string coords_TopLeft_CornerField;

        private string coords_LowerRight_CornerField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Coords_TopLeft_Corner
        {
            get
            {
                return this.coords_TopLeft_CornerField;
            }
            set
            {
                this.coords_TopLeft_CornerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Coords_LowerRight_Corner
        {
            get
            {
                return this.coords_LowerRight_CornerField;
            }
            set
            {
                this.coords_LowerRight_CornerField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeAllowed_Terrain_Heights
    {

        private byte minimumField;

        private ushort maximumField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte Minimum
        {
            get
            {
                return this.minimumField;
            }
            set
            {
                this.minimumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort Maximum
        {
            get
            {
                return this.maximumField;
            }
            set
            {
                this.maximumField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class typesTypeMinimum_Distance_To_Non_Teleported_Players
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
    public partial class typesTypeHorde_Spawn_Timer
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
    public partial class typesTypeHorde_Lifetime
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
    public partial class typesTypeHorde_Created_Or_Teleported_Only_If_At_Least_One_Player_Is_Teleported
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
    public partial class typesTypeActivate_Namalsk_Event_System
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
    public partial class typesTypeActivate_Custom_Event_System
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
