using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{
    public enum VehicleSync
    {
        [Description("Vehicles will be simulated by the Server and only the server")]
        Server = 0,
        [Description("Vehicles will be simulated by the Server prediciting player actions. To make it easy to understand, it's Client and server sync.")]
        ClientAndServer,
        [Description("Vehicles will be simulated by the client (player).")]
        Client,
    };
    public enum VehicleRequireKeyToStart
    {
        [Description("Even if your car is paired to a key, you don't need to have the key in the vehicle inventory or on yourself")]
        No_Key_Required = 0,
        [Description("You will need a car key paired to the vehicle in your inventory or in the inventory of the vehicle to start the engine")]
        Have_Paired_Key_In_Inventory,
        [Description("You need to have the key in your hands to start the engine")]
        Have_Key_In_Hands,
    }
    public enum MasterKeyPairingMode
    {
        [Description("infinite master pairing uses")]
        Infinite = -1,
        [Description("disabled. You can pair any keys to your already paired car")]
        Disabled = 0,
        [Description("limited uses before becoming a normal car key (MasterKeyUses)")]
        Limited_Uses = 1,
        [Description("enewable with a electronicalrepairkit or a keygrinder (will also use MasterKeyUses)")]
        Renewable = 2, 
        [Description("renewable with a keygrinder (will also use MasterKeyUses). Currently only configured for MuchCarKeys, a futur update will also add a Expansion Grinder.")]
        Renewable_With_Grinder = 2,
    };
    public enum PlacePlayerOnGroundOnReconnectInVehicle
    {
        [Description("disabled")]
        disabled = 0,
        [Description("Always")]
        Always = 1,
        [Description(" Only on server restarts")]
        Restarts_Only = 2,
    }
    public class VehicleSettings
    {
        public int m_Version { get; set; } // current version 8
        public int VehicleSync { get; set; }
        public int VehicleRequireKeyToStart { get; set; }
        public int VehicleRequireAllDoors { get; set; }
        public int VehicleLockedAllowInventoryAccess { get; set; }
        public int VehicleLockedAllowInventoryAccessWithoutDoors { get; set; }
        public int MasterKeyPairingMode { get; set; }
        public int MasterKeyUses { get; set; }
        public int CanPickLock { get; set; }
        public BindingList<string> PickLockTools { get; set; }
        public float PickLockChancePercent { get; set; }
        public int PickLockTimeSeconds { get; set; }
        public float PickLockToolDamagePercent { get; set; }
        public int EnableWindAerodynamics { get; set; }
        public int EnableTailRotorDamage { get; set; }
        public int PlayerAttachment { get; set; }
        public int Towing { get; set; }
        public int EnableHelicopterExplosions { get; set; }
        public int DisableVehicleDamage { get; set; }
        public float VehicleCrewDamageMultiplier { get; set; }
        public float VehicleSpeedDamageMultiplier { get; set; }
        public int CanChangeLock { get; set; }
        public BindingList<string> ChangeLockTools { get; set; }
        public float ChangeLockTimeSeconds { get; set; }
        public float ChangeLockToolDamagePercent { get; set; }
        public int PlacePlayerOnGroundOnReconnectInVehicle { get; set; }
        public int RevvingOverMaxRPMRuinsEngineInstantly { get; set; }
        public int VehicleDropsRuinedDoors { get; set; }
        public int ExplodingVehicleDropsAttachments { get; set; }
        public float ForcePilotSyncIntervalSeconds { get; set; }
        public BindingList<VConfigs> VehiclesConfig { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public VehicleSettings()
        {
            m_Version = 8;
            PickLockTools = new BindingList<string>();
            ChangeLockTools = new BindingList<string>();
            VehiclesConfig = new BindingList<VConfigs>();
            isDirty = true;
        }

        public void SetIntValue(string mytype, int myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
        public void SetFloatValue(string mytype, float myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
    }
    public class VConfigs
    {
        public string ClassName { get; set; }
        public int CanPlayerAttach { get; set; }

        public override string ToString()
        {
            return ClassName;
        }
    }
}
