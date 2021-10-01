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
        Expansion_basebuilding_and_Fences = 2,
        [Description("Expansion Builds, Tents and Fences")]
        Expansion_basebuilding_Tents_Fences = 3
    }
    public class BaseBuildingSettings
    {
        public int m_Version { get; set; }  // Current Version is 2
        public int CanBuildAnywhere { get; set; }
        public int AllowBuildingWithoutATerritory { get; set; }
        public BindingList<string> DeployableOutsideATerritory { get; set; }
        public BindingList<string> DeployableInsideAEnemyTerritory { get; set; }
        public int CanCraftVanillaBasebuilding { get; set; }
        public int CanCraftExpansionBasebuilding { get; set; }
        public int DestroyFlagOnDismantle { get; set; }
        public int DismantleFlagRequireTools { get; set; }
        public int DismantleOutsideTerritory { get; set; }
        public int DismantleInsideTerritory { get; set; }
        public int DismantleAnywhere { get; set; }
        public int CanAttachCodelock { get; set; }
        public int CodelockActionsAnywhere { get; set; }
        public int CodeLockLength { get; set; }
        public int DoDamageWhenEnterWrongCodeLock { get; set; }
        public float DamageWhenEnterWrongCodeLock { get; set; }
        public int RememberCode { get; set; }
        public int CanCraftTerritoryFlagKit { get; set; }
        public int SimpleTerritory { get; set; }
        public int AutomaticFlagOnCreation { get; set; }
        public int EnableFlagMenu { get; set; }
        public int GetTerritoryFlagKitAfterBuild { get; set; }
        public string BuildZoneRequiredCustomMessage { get; set; }
        public BindingList<NoBuildZones> Zones { get; set; }
        public int ZonesAreNoBuildZones { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }
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
