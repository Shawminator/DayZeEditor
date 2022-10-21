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
        public CFGGameplayMapData MapData { get; set; }

        [JsonIgnore]
        const int currentversion = 119;
        
        public cfggameplay()
        {
            GeneralData = new Generaldata();
            PlayerData = new Playerdata();
            WorldsData = new Worldsdata();
            BaseBuildingData = new Basebuildingdata();
            UIData = new Uidata();
            MapData = new CFGGameplayMapData();
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

        public Generaldata() 
        {
            disableBaseDamage = 0;
            disableContainerDamage = 0;
            disableRespawnDialog = 0;
        }
    }

    public class Playerdata
    {
        public int disablePersonalLight { get; set; }
        public Staminadata StaminaData { get; set; }
        public Shockhandlingdata ShockHandlingData { get; set; }
        public MovementData MovementData { get; set; }
        public DrowningData DrowningData { get; set; }

        public Playerdata()
        {
            disablePersonalLight = 1;
            StaminaData = new Staminadata();
            ShockHandlingData = new Shockhandlingdata();
            MovementData = new MovementData();
            DrowningData = new DrowningData();
        }
    }
    public class Staminadata
    {
        public decimal sprintStaminaModifierErc { get; set; }
        public decimal sprintStaminaModifierCro { get; set; }
        public decimal staminaWeightLimitThreshold { get; set; }
        public decimal staminaMax { get; set; }
        public decimal staminaKgToStaminaPercentPenalty { get; set; }
        public decimal staminaMinCap { get; set; }

        public Staminadata() { }
    }
    public class Shockhandlingdata
    {
        public decimal shockRefillSpeedConscious { get; set; }
        public decimal shockRefillSpeedUnconscious { get; set; }
        public bool allowRefillSpeedModifier { get; set; }

        public Shockhandlingdata() { }
    }
    public class MovementData
    {
        public decimal timeToStrafeJog { get; set; }
        public decimal rotationSpeedJog { get; set; }
        public decimal timeToSprint { get; set; }
        public decimal timeToStrafeSprint { get; set; }
        public decimal rotationSpeedSprint { get; set; }

        public MovementData() 
        {
            timeToStrafeJog = (decimal)0.1;
			rotationSpeedJog = (decimal)0.3;
			timeToSprint = (decimal)0.45;
			timeToStrafeSprint = (decimal)0.3;
			rotationSpeedSprint = (decimal)0.15;
        }
    }
    public class DrowningData
    {
        public decimal staminaDepletionSpeed { get; set; }
        public decimal healthDepletionSpeed { get; set; }
        public decimal shockDepletionSpeed { get; set; }

        public DrowningData() { }
    }


    public class Worldsdata
    {
        public int lightingConfig { get; set; }
        public BindingList<object> objectSpawnersArr { get; set; }
        public BindingList<decimal> environmentMinTemps { get; set; }
        public BindingList<decimal> environmentMaxTemps { get; set; }

        public Worldsdata()
        {
            environmentMaxTemps = new BindingList<decimal>();
            environmentMinTemps = new BindingList<decimal>();
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
        public decimal hitDirectionMaxDuration { get; set; }
        public decimal hitDirectionBreakPointRelative { get; set; }
        public decimal hitDirectionScatter { get; set; }
        public int hitIndicationPostProcessEnabled { get; set; }

        public Hitindicationdata()
        {
            hitDirectionOverrideEnabled = 0;
			hitDirectionBehaviour = 1;
			hitDirectionStyle = 0;
			hitDirectionIndicatorColorStr = "0xffbb0a1e";
			hitDirectionMaxDuration = (decimal)2.0;
            hitDirectionBreakPointRelative = (decimal)0.2;
            hitDirectionScatter = (decimal)10.0;
            hitIndicationPostProcessEnabled = 1;
        }
    }

    public class CFGGameplayMapData
    {
        public int ignoreMapOwnership { get; set; }
        public int ignoreNavItemsOwnership { get; set; }
        public int displayPlayerPosition { get; set; }
        public int displayNavInfo { get; set; }

        public CFGGameplayMapData() 
        {
            ignoreMapOwnership = 0;
            ignoreNavItemsOwnership = 0;
            displayPlayerPosition = 0;
		    displayNavInfo = 1;
        }
    }

}
