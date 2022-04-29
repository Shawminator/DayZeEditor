using System;
using System.Collections.Generic;
using System.ComponentModel;
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


        [JsonIgnore]
        const int currentversion = 116;
        public cfggameplay()
        {
            GeneralData = new Generaldata();
            PlayerData = new Playerdata();
            WorldsData = new Worldsdata();
            BaseBuildingData = new Basebuildingdata();
            UIData = new Uidata();
        }

        internal bool checkver()
        {
            if (version != currentversion)
            {
                version = currentversion;
                return true;
            }
            return false;
        }
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
        public Shockhandlingdata ShockHandlingData { get; set; }

        public Playerdata()
        {
            StaminaData = new Staminadata();
            ShockHandlingData = new Shockhandlingdata();
        }
    }

    public class Staminadata
    {
        public float sprintStaminaModifierErc { get; set; }
        public float sprintStaminaModifierCro { get; set; }
        public float staminaWeightLimitThreshold { get; set; }
        public float staminaMax { get; set; }
        public float staminaKgToStaminaPercentPenalty { get; set; }
        public float staminaMinCap { get; set; }
    }

    public class Shockhandlingdata
    {
        public float shockRefillSpeedConscious { get; set; }
        public float shockRefillSpeedUnconscious { get; set; }
        public bool allowRefillSpeedModifier { get; set; }
    }

    public class Worldsdata
    {
        public int lightingConfig { get; set; }
        public BindingList<object> objectSpawnersArr { get; set; }
        public BindingList<int> environmentMinTemps { get; set; }
        public BindingList<int> environmentMaxTemps { get; set; }

        public Worldsdata()
        {
            environmentMaxTemps = new BindingList<int>();
            environmentMinTemps = new BindingList<int>();
            for (int i = 0; i < 12; i++)
            {
                environmentMaxTemps.Add(0);
                environmentMinTemps.Add(0);
            }
            objectSpawnersArr = new BindingList<object>();
        }
    }

    public class Basebuildingdata
    {
        public Hologramdata HologramData { get; set; }
        public Constructiondata ConstructionData { get; set; }

        public Basebuildingdata()
        {
            HologramData = new Hologramdata();
            ConstructionData = new Constructiondata();
        }
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
        public int use3DMap { get; set; }
        public Hitindicationdata HitIndicationData { get; set; }

        public Uidata()
        {
            HitIndicationData = new Hitindicationdata();
        }
    }

    public class Hitindicationdata
    {
        public int hitDirectionOverrideEnabled { get; set; }
        public int hitDirectionBehaviour { get; set; }
        public int hitDirectionStyle { get; set; }
        public string hitDirectionIndicatorColorStr { get; set; }
        public float hitDirectionMaxDuration { get; set; }
        public float hitDirectionBreakPointRelative { get; set; }
        public float hitDirectionScatter { get; set; }
        public int hitIndicationPostProcessEnabled { get; set; }

        public Hitindicationdata()
        {
            hitDirectionIndicatorColorStr = "0xffbb0a1e";
        }
    }


}
