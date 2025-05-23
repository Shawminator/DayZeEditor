﻿using System.ComponentModel;
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
    public class ExpansionVehicleSettings
    {
        const int CurrentVersion = 21;

        public int m_Version { get; set; }
        public int VehicleSync { get; set; }
        public int VehicleRequireKeyToStart { get; set; }
        public int VehicleRequireAllDoors { get; set; }
        public int VehicleLockedAllowInventoryAccess { get; set; }
        public int VehicleLockedAllowInventoryAccessWithoutDoors { get; set; }
        public int MasterKeyPairingMode { get; set; }
        public int MasterKeyUses { get; set; }
        public int CanPickLock { get; set; }
        public BindingList<string> PickLockTools { get; set; }
        public decimal PickLockChancePercent { get; set; }
        public int PickLockTimeSeconds { get; set; }
        public decimal PickLockToolDamagePercent { get; set; }
        public int EnableWindAerodynamics { get; set; }
        public int EnableTailRotorDamage { get; set; }
        public int Towing { get; set; }
        public int EnableHelicopterExplosions { get; set; }
        public int DisableVehicleDamage { get; set; }
        public decimal VehicleCrewDamageMultiplier { get; set; }
        public decimal VehicleSpeedDamageMultiplier { get; set; }
        public decimal VehicleRoadKillDamageMultiplier { get; set; }
        public int CollisionDamageIfEngineOff { get; set; }
        public decimal CollisionDamageMinSpeedKmh { get; set; }
        public int CanChangeLock { get; set; }
        public BindingList<string> ChangeLockTools { get; set; }
        public decimal ChangeLockTimeSeconds { get; set; }
        public decimal ChangeLockToolDamagePercent { get; set; }
        public int PlacePlayerOnGroundOnReconnectInVehicle { get; set; }
        public int RevvingOverMaxRPMRuinsEngineInstantly { get; set; }
        public int VehicleDropsRuinedDoors { get; set; }
        public int ExplodingVehicleDropsAttachments { get; set; }
        public decimal DesyncInvulnerabilityTimeoutSeconds { get; set; }
        public decimal DamagedEngineStartupChancePercent { get; set; }
        public decimal FuelConsumptionPercent { get;set; }
        public int EnableVehicleCovers { get; set; }
        public int AllowCoveringDEVehicles { get; set; }
        public int CanCoverWithCargo { get; set; }
        public int UseVirtualStorageForCoverCargo { get; set; }
        public decimal VehicleAutoCoverTimeSeconds { get; set; }
        public int VehicleAutoCoverRequireCamonet { get; set; }
        public int EnableAutoCoveringDEVehicles { get; set; }
        public string CFToolsHeliCoverIconName { get; set; }
        public string CFToolsBoatCoverIconName { get; set; }
        public string CFToolsCarCoverIconName { get; set; }
        public int ShowVehicleOwners { get; set; }
        public BindingList<ExpansionVehiclesConfig> VehiclesConfig { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public ExpansionVehicleSettings()
        {
            m_Version = CurrentVersion;
            VehicleSync = 0;
            VehicleRequireKeyToStart = 1;
            VehicleRequireAllDoors = 1;
            VehicleLockedAllowInventoryAccess = 0;
            VehicleLockedAllowInventoryAccessWithoutDoors = 1;
            MasterKeyPairingMode = 2;
            MasterKeyUses = 2;
            CanPickLock = 0;
            PickLockTools = new BindingList<string>() {
                "Lockpick"
                };
            PickLockChancePercent = (decimal)40.0;
            PickLockTimeSeconds = 120;
            PickLockToolDamagePercent = (decimal)10.0;
            EnableWindAerodynamics = 0;
            EnableTailRotorDamage = 1;
            Towing = 1;
            EnableHelicopterExplosions = 1;
            DisableVehicleDamage = 0;
            VehicleCrewDamageMultiplier = (decimal)1.0;
            VehicleSpeedDamageMultiplier = (decimal)1.0;
            VehicleRoadKillDamageMultiplier = (decimal)1.0;
            CollisionDamageIfEngineOff = 0;
            CollisionDamageMinSpeedKmh = (decimal)30.0;
            CanChangeLock = 0;
            ChangeLockTools = new BindingList<string>() {
                "Screwdriver"
                };
            ChangeLockTimeSeconds = 120;
            ChangeLockToolDamagePercent = (decimal)10.0;
            PlacePlayerOnGroundOnReconnectInVehicle = 2;
            RevvingOverMaxRPMRuinsEngineInstantly = 0;
            VehicleDropsRuinedDoors = 1;
            ExplodingVehicleDropsAttachments = 1;
            DesyncInvulnerabilityTimeoutSeconds = (decimal)3.0;
            DamagedEngineStartupChancePercent = (decimal)100.0;
            FuelConsumptionPercent = (decimal)100.0;
            EnableVehicleCovers = 1;
            AllowCoveringDEVehicles = 0;
            CanCoverWithCargo = 1;
            UseVirtualStorageForCoverCargo = 0;
            VehicleAutoCoverTimeSeconds = (decimal)0.0;
            VehicleAutoCoverRequireCamonet = 0;
            EnableAutoCoveringDEVehicles = 0;
            CFToolsHeliCoverIconName = "helicopter";
            CFToolsBoatCoverIconName = "ship";
            CFToolsCarCoverIconName = "car";
            ShowVehicleOwners = 0;
            VehiclesConfig = new BindingList<ExpansionVehiclesConfig>()
            {
                new ExpansionVehiclesConfig(){
                ClassName = "ExpansionUAZCargoRoofless",
                LockComplexity = (decimal)1.0
                },
                new ExpansionVehiclesConfig(){
                ClassName = "ExpansionUAZ",
                LockComplexity = (decimal)1.0
                },
                new ExpansionVehiclesConfig() {
                ClassName = "ExpansionBus",
                LockComplexity = (decimal)1.5
                },
                new ExpansionVehiclesConfig() {
                ClassName = "ExpansionVodnik",
                LockComplexity = (decimal)2.0
                },
                new ExpansionVehiclesConfig() {
                ClassName = "ExpansionUtilityBoat",
                LockComplexity = (decimal)1.25
                },
                new ExpansionVehiclesConfig(){
                ClassName = "ExpansionZodiacBoat",
                LockComplexity = (decimal)0.5
                },
                new ExpansionVehiclesConfig(){
                ClassName = "ExpansionLHD",
                LockComplexity = (decimal)100.0
                },
                new ExpansionVehiclesConfig(){
                ClassName = "ExpansionMerlin",
                LockComplexity = (decimal)4.0
                },
                new ExpansionVehiclesConfig(){
                ClassName = "ExpansionMh6",
                LockComplexity = (decimal)3.0
                },
                new ExpansionVehiclesConfig(){
                ClassName = "ExpansionUh1h",
                LockComplexity = (decimal)2.5
                }
            };
        }
        public bool checkver()
        {
            if (m_Version != CurrentVersion)
            {
                m_Version = CurrentVersion;
                isDirty = true;
                return true;
            }
            return false;
        }
        public void SetIntValue(string mytype, int myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
        public void SetFloatValue(string mytype, decimal myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
    }
    public class ExpansionVehiclesConfig
    {
        public string ClassName { get; set; }
        public decimal LockComplexity { get; set; }

        public override string ToString()
        {
            return ClassName;
        }
    }
}
