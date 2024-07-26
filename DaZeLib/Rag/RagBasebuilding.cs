using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class RagBasebuilding
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty;

        public BindingList<string> BaseBuildTools { get; set; }
        public BindingList<string> BaseDismantleTools { get; set; }
        public BindingList<string> BaseDestroyTools { get; set; }

        public int BaseBuildTime { get; set; }
        public int BaseDismantleTime { get; set; }
        public int BaseDestroyTimeDefault { get; set; }
        public int BaseDestroyTime { get; set; }

        public int BaseBuildToolDamage { get; set; }
        public int BaseDismantleToolDamage { get; set; }
        public int BaseDestroyToolDamageWood { get; set; }

        public bool BaseCanAttachXmasLights { get; set; }
        public bool BaseInfiniteLifetime { get; set; }
        public bool BaseRaidOnlyDoors { get; set; }
        public bool BaseBuildAnywhere { get; set; }
        public bool FlagBuildAnywhere { get; set; }
        public bool TentBuildAnywhere { get; set; }
        public bool VanillaBuildAnywhere { get; set; }
        public bool DisableCraftVanillaFence { get; set; }
        public bool DisableCraftVanillaWatchtower { get; set; }

        public bool DisableDestroy { get; set; }
        public bool DisableDismantle { get; set; }

        public bool EnableHideShowInventory { get; set; }
        public bool ReturnKitAfterBuilt { get; set; }
        public bool EnableBuildHatchLadder { get; set; }
        public bool EnableAttachPortableGasLamp { get; set; }

        public Dictionary<string,int> Materials { get; set; }
    }

}
