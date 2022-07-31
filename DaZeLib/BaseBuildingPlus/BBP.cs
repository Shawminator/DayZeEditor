using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{

    public class BBPSettings
    {
        public int BBP_DisableCraftVanillaFence { get; set; }
        public int BBP_DisableCraftVanillaWatchtower { get; set; }
        public int BBP_BaseBuildAnywhere { get; set; }
        public int BBP_FlagBuildAnywhere { get; set; }
        public int BBP_TentBuildAnywhere { get; set; }
        public int BBP_VanillaBuildAnywhere { get; set; }
        public int BBP_SetInfiniteLifetime { get; set; }
        public int BBP_AdvancedRotation { get; set; }
        public int BBP_FloatingPlacement { get; set; }
        public int BBP_CanRaiseAndLowerHologram { get; set; }
        public int BBP_HologramHasCollision { get; set; }
        public int BBP_InventoryToggle { get; set; }
        public int BBP_HideKitWhilePlacing { get; set; }
        public int BBP_BarbedWireRemoveOutside { get; set; }
        public int BBP_CanAttachFlashlights { get; set; }
        public int BBP_CanAttachXmaslights { get; set; }
        public int BBP_CanAttachCamonets { get; set; }
        public int BBP_CanAttachWallpaper { get; set; }
        public int BBP_CanAttachCarpet { get; set; }
        public int BBP_CanAttachPlaster { get; set; }
        public int BBP_RaidOnlyDoors { get; set; }
        public int BBP_DisableDestroy { get; set; }
        public int BBP_DisableDismantle { get; set; }
        public int BBP_BuildTime { get; set; }
        public int BBP_DismantleTime { get; set; }
        public int BBP_Tier1RaidTime { get; set; }
        public int BBP_Tier2RaidTime { get; set; }
        public int BBP_Tier3RaidTime { get; set; }
        public int BBP_BaseBuildToolDamage { get; set; }
        public int BBP_BaseDismantleToolDamage { get; set; }
        public int BBP_Tier1RaidToolDamage { get; set; }
        public int BBP_Tier2RaidToolDamage { get; set; }
        public int BBP_Tier3RaidToolDamage { get; set; }
        public BindingList<string> BBP_BuildTools { get; set; }
        public BindingList<string> BBP_DismantleTools { get; set; }
        public BindingList<string> BBP_Tier1RaidTools { get; set; }
        public BindingList<string> BBP_Tier2RaidTools { get; set; }
        public BindingList<string> BBP_Tier3RaidTools { get; set; }
        public int BBP_CementMixerTime { get; set; }
        public BindingList<BBP_Cementmixerlocations> BBP_CementMixerLocations { get; set; }
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty;
    }

    public class BBP_Cementmixerlocations
    {
        public float[] position { get; set; }
        public float[] orientation { get; set; }

        public override string ToString()
        {
            return "BBP Cement Mixer Location";
        }
    }

}
