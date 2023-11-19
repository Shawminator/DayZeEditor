using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{
    public enum ExpansionCodelockAttachMode
    {
        [Description("Only on Expansion Builds")]
        Only_on_expansion_basebuilding = 0,
        [Description("Expansion Builds and Tents")]
        Expansion_basebuilding_and_Tents = 1,
        [Description("Expansion builds and Fences")]
        Expansion_basebuilding_and_Fences = 3,
        [Description("Expansion Builds, Tents and Fences")]
        Expansion_basebuilding_Tents_Fences = 2
    }
    public enum ExpansionFlagMenuMode
    {
        [Description("he player will not be able to create a territory")]
        Disabled = 0,
        [Description("The player will be able to create a territory to protect his base.")]
        Create_Territory = 1,
        [Description("The player will be able to create a territory to protect his base but he won't be able to customize his flag from the flag menu")]
        Create_Territory_No_Custom_Flags = 2,
    }
    public enum ExpansionDismantleFlagMode
    {
        [Description("Only territory members can dismantle the flag pole.")]
        OnlyMembers = -1,
        [Description("You can dismantle the flag pole with your hands.")]
        Dismantle_With_Hands = 0,
        [Description("You need tools to dismantle the flag pole.")]
        Dismantle_With_Tools = 1,
    }
    public class ExpansionBaseBuildingSettings
    {
        const int CurrentVersion = 4;

        public int m_Version { get; set; }  // Current Version is 3
        public int CanBuildAnywhere { get; set; }
        public int AllowBuildingWithoutATerritory { get; set; }
        public BindingList<string> DeployableOutsideATerritory { get; set; }
        public BindingList<string> DeployableInsideAEnemyTerritory { get; set; }
        public int CanCraftVanillaBasebuilding { get; set; }
        public int CanCraftExpansionBasebuilding { get; set; }
        public int DestroyFlagOnDismantle { get; set; }
        public int DismantleOutsideTerritory { get; set; }
        public int DismantleInsideTerritory { get; set; }
        public int DismantleAnywhere { get; set; }
        public int CodelockActionsAnywhere { get; set; }
        public int CodeLockLength { get; set; }
        public int DoDamageWhenEnterWrongCodeLock { get; set; }
        public decimal DamageWhenEnterWrongCodeLock { get; set; }
        public int RememberCode { get; set; }
        public int CanCraftTerritoryFlagKit { get; set; }
        public int SimpleTerritory { get; set; }
        public int AutomaticFlagOnCreation { get; set; }
        public int GetTerritoryFlagKitAfterBuild { get; set; }
        public string BuildZoneRequiredCustomMessage { get; set; }
        public BindingList<ExpansionBuildNoBuildZone> Zones { get; set; }
        public int ZonesAreNoBuildZones { get; set; }
        public int CodelockAttachMode { get; set; }
        public int DismantleFlagMode { get; set; }
        public int FlagMenuMode { get; set; }
        public int OverrideVanillaEntityPlacement { get; set; }
        public int EnableVirtualStorage { get; set; }
        public BindingList<string> VirtualStorageExcludedContainers { get;set;}

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public ExpansionBaseBuildingSettings()
        {
            m_Version = CurrentVersion;
            CanBuildAnywhere = 1;
            AllowBuildingWithoutATerritory = 1;
            DeployableOutsideATerritory = new BindingList<string>(){
                "ExpansionSatchel",
                "Fireplace",
                "TerritoryFlagKit",
                "MediumTent",
                "LargeTent",
                "CarTent",
                "PartyTent",
                "ExpansionCamoTentKit",
                "ExpansionCamoBoxKit",
                "ShelterKit",
                "LandMineTrap",
                "BearTrap",
                "FishNetTrap",
                "RabbitSnareTrap",
                "SmallFishTrap",
                "TripwireTrap",
                "ExpansionSafeLarge",
                "ExpansionSafeMedium",
                "ExpansionSafeSmall",
                "SeaChest",
                "WoodenCrate",
                "GardenPlot" };
            DeployableInsideAEnemyTerritory = new BindingList<string>() {
                "ExpansionSatchel",
                "LandMineTrap",
                "BearTrap",
                "FishNetTrap",
                "RabbitSnareTrap",
                "SmallFishTrap",
                "TripwireTrap"};

            ZonesAreNoBuildZones = 1;

            CanCraftVanillaBasebuilding = 0;
            CanCraftExpansionBasebuilding = 1;
            DestroyFlagOnDismantle = 1;
            DismantleFlagMode = (int)ExpansionDismantleFlagMode.Dismantle_With_Tools;
            DismantleOutsideTerritory = 0;
            DismantleInsideTerritory = 0;
            DismantleAnywhere = 0;

            CodelockAttachMode = (int)ExpansionCodelockAttachMode.Expansion_basebuilding_and_Fences;  //! Will also allow BBP if installed
            CodelockActionsAnywhere = 0;
            CodeLockLength = 4;
            DoDamageWhenEnterWrongCodeLock = 1;
            DamageWhenEnterWrongCodeLock = (decimal)10.0;
            RememberCode = 1;

            CanCraftTerritoryFlagKit = 1;
            SimpleTerritory = 1;
            AutomaticFlagOnCreation = 1;
            FlagMenuMode = (int)ExpansionFlagMenuMode.Create_Territory_No_Custom_Flags;
            GetTerritoryFlagKitAfterBuild = 0;

            VirtualStorageExcludedContainers = new BindingList<string>() { "ExpansionAirdropContainerBase" };
            Zones = new BindingList<ExpansionBuildNoBuildZone>();
            
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
    }
    public class ExpansionBuildNoBuildZone
    {
        public string Name { get; set; }
        public float[] Center { get; set; }
        public float Radius { get; set; }
        public BindingList<string> Items { get; set; }
        public int IsWhitelist { get; set; }
        public string CustomMessage { get; set; }

        public ExpansionBuildNoBuildZone()
        {
            Items = new BindingList<string>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
