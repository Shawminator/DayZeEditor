using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{

    public class cfggameplay
    {
        public int version { get; set; }
        public Generaldata GeneralData { get; set; }
        public Playerdata PlayerData { get; set; }
        public Worldsdata WorldsData { get; set; }
        public Basebuildingdata BaseBuildingData { get; set; }
        public Uidata UIData { get; set; }
    }

    public class Generaldata
    {
        public int disableBaseDamage { get; set; }
        public int disableContainerDamage { get; set; }
        public int disableRespawnDialog { get; set; }
    }

    public class Playerdata
    {
        public int disablePersonalLight { get; set; }
        public Staminadata StaminaData { get; set; }
    }

    public class Staminadata
    {
        public decimal sprintStaminaModifierErc { get; set; }
        public decimal sprintStaminaModifierCro { get; set; }
        public decimal staminaWeightLimitThreshold { get; set; }
        public decimal staminaMax { get; set; }
        public decimal staminaKgToStaminaPercentPenalty { get; set; }
        public decimal staminaMinCap { get; set; }
    }

    public class Worldsdata
    {
        public int lightingConfig { get; set; }
        public object[] objectSpawnersArr { get; set; }
    }

    public class Basebuildingdata
    {
        public Hologramdata HologramData { get; set; }
        public Constructiondata ConstructionData { get; set; }
    }

    public class Hologramdata
    {
        public int disableIsCollidingBBoxCheck { get; set; }
        public int disableIsCollidingPlayerCheck { get; set; }
        public int disableIsClippingRoofCheck { get; set; }
        public int disableIsBaseViableCheck { get; set; }
        public int disableIsCollidingGPlotCheck { get; set; }
        public int disableIsCollidingAngleCheck { get; set; }
        public int disableIsPlacementPermittedCheck { get; set; }
        public int disableHeightPlacementCheck { get; set; }
        public int disableIsUnderwaterCheck { get; set; }
        public int disableIsInTerrainCheck { get; set; }
    }

    public class Constructiondata
    {
        public int disablePerformRoofCheck { get; set; }
        public int disableIsCollidingCheck { get; set; }
        public int disableDistanceCheck { get; set; }
    }

    public class Uidata
    {
        public Hitindicationdata HitIndicationData { get; set; }
    }

    public class Hitindicationdata
    {
        public int hitDirectionOverrideEnabled { get; set; }
        public int hitDirectionBehaviour { get; set; }
        public int hitDirectionStyle { get; set; }
        public string hitDirectionIndicatorColorStr { get; set; }
        public decimal hitDirectionMaxDuration { get; set; }
        public decimal hitDirectionBreakPointRelative { get; set; }
        public decimal hitDirectionScatter { get; set; }
        public int hitIndicationPostProcessEnabled { get; set; }
    }

}
