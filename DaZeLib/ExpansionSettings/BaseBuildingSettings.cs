using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{
    public enum canattacchcodelock
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
    public enum FlagMenuMode
    {
        [Description("he player will not be able to create a territory")]
        Disabled = 0,
        [Description("The player will be able to create a territory to protect his base.")]
        Create_Territory = 1,
        [Description("The player will be able to create a territory to protect his base but he won't be able to customize his flag from the flag menu")]
        Create_Territory_No_Custom_Flags = 2,
    }
    public enum DismantleFlagMode
    {
        [Description("Only territory members can dismantle the flag pole.")]
        OnlyMembers = -1,
        [Description("You can dismantle the flag pole with your hands.")]
        Dismantle_With_Hands = 0,
        [Description("You need tools to dismantle the flag pole.")]
        Dismantle_With_Tools = 1,
    }
    public class BaseBuildingSettings
    {
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
        public float DamageWhenEnterWrongCodeLock { get; set; }
        public int RememberCode { get; set; }
        public int CanCraftTerritoryFlagKit { get; set; }
        public int SimpleTerritory { get; set; }
        public int AutomaticFlagOnCreation { get; set; }
        public int GetTerritoryFlagKitAfterBuild { get; set; }
        public string BuildZoneRequiredCustomMessage { get; set; }
        public BindingList<NoBuildZones> Zones { get; set; }
        public int ZonesAreNoBuildZones { get; set; }
        public int CodelockAttachMode { get; set; }
        public int DismantleFlagMode { get; set; }
        public int FlagMenuMode { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public BaseBuildingSettings()
        {
            m_Version = 3;
            DeployableInsideAEnemyTerritory = new BindingList<string>();
            DeployableOutsideATerritory = new BindingList<string>();
            Zones = new BindingList<NoBuildZones>();
            isDirty = true;
        }
    }
    public class NoBuildZones
    {
        public string Name { get; set; }
        public float[] Center { get; set; }
        public float Radius { get; set; }
        public BindingList<string> Items { get; set; }
        public int IsWhitelist { get; set; }
        public string CustomMessage { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
