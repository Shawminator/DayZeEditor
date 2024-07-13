using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class InediaStamina_Config
    {
        public bool ShowStaminaWidgetInVehicle = true;
        public bool AllowOpenInventoryWhileResting = true;

        public InediaStaminaGeneralOptions StaminaGeneralOptions { get; set; }
        public InediaStaminaSleepOptions StaminaSleepOptions { get; set; }
        public InediaStaminaInediaMovementOptions InediaMovementOptions { get; set; }
        public BindingList<InediaStaminaStimulant> StaminaStimulants { get; set; }
        public BindingList<InediaStaminaLiquidStimulant> StaminaLiquidStimulants { get; set; }
        public BindingList<InediaStaminaComfortablePlace> ComfortablePlaces { get; set; }
        public BindingList<InediaStaminaCloth> Clothes { get; set; }
        public BindingList<InediaStaminaHumanPoweredVehicle> HumanPoweredVehicles { get; set; }
        public BindingList<string> Houses { get; set; }
    }
 
    public class InediaStaminaGeneralOptions
    {
        public int IsActive { get; set; }
        public float RespawnValuePercent { get; set; }
        public float WeightOverloadThresholdKg { get; set; }
        public float WeightOverloadMultiplierMax { get; set; }
        public float CostPerSecondRestPercent { get; set; }
        public float CostPerSecondNoMovementPercent { get; set; }
        public float CostPerSecondNormalWalkingPercent { get; set; }
        public float CostPerSecondCrouchWalkingPercent { get; set; }
        public float CostPerSecondProneWalkingPercent { get; set; }
        public float CostPerSecondJogPercent { get; set; }
        public float CostPerSecondCrouchSprintPercent { get; set; }
        public float CostPerSecondSprintPercent { get; set; }
        public float CostPerRollPercent { get; set; }
        public float CostPerJumpPercent { get; set; }
        public float CostPerVaultPercent { get; set; }
        public float CostPerClimbPercent { get; set; }
        public float CostPerMeleeAttackPercent { get; set; }
        public float CostPerPushCarApplyForcePercent { get; set; }
        public float CostPerMinePercent { get; set; }
        public float CostPerMinePlanksFromPileOfWoodenPlanksPercent { get; set; }
        public float CostPerMinePlanksFromWoodenLogPercent { get; set; }
        public float CostPerBuildPercent { get; set; }
        public float WalkingWeightDebuffBorderKg { get; set; }
        public float OneDegreeOfSlopeUpMultiplierPercent { get; set; }
        public float OneDegreeOfSlopeDownMultiplierPercent { get; set; }
        public float SurfaceRoadMultiplier { get; set; }
        public float SurfaceGrassMultiplier { get; set; }
        public float SurfaceGrassTallMultiplier { get; set; }
        public float SurfaceForestMultiplier { get; set; }
        public float SurfaceStoneMultiplier { get; set; }
        public float SurfaceSandMultiplier { get; set; }
        public float SurfaceWaterMultiplier { get; set; }
        public float EpinephrineMultiplier { get; set; }
        public float AMSEnergyPillsMultiplier { get; set; }
        public float MorphineMultiplier { get; set; }
        public float HeatMultiplier { get; set; }
        public float ShockPerSecondJogCritical { get; set; }
        public float ShockPerSecondSprintCritical { get; set; }
        public float ShockPerSecondJogLow { get; set; }
        public float ShockPerSecondSprintLow { get; set; }
        public float TremorLevelAtCritical { get; set; }
        public float TremorLevelAtLow { get; set; }
        public float CriticalThresholdPercent { get; set; }
        public float LowThresholdPercent { get; set; }
        public float SyberiaProjectAthleticsImpactFrom { get; set; }
        public float SyberiaProjectAthleticsImpactTo { get; set; }
        public float SyberiaProjectStrengthImpactFrom { get; set; }
        public float SyberiaProjectStrengthImpactTo { get; set; }
    }

    public class InediaStaminaSleepOptions
    {
        public int IsActive { get; set; }
        public float RespawnValuePercent { get; set; }
        public float ReductionOfMaximumSleepWhenDaytimePercent { get; set; }
        public float ReductionOfMaximumSleepWhenOutdoorsPercent { get; set; }
        public float ReductionOfMaximumSleepWhenWetPercent { get; set; }
        public float ReductionOfMaximumSleepWhenColdPercent { get; set; }
        public float ReductionOfMaximumSleepWhenHotPercent { get; set; }
        public float DelayBeforeSleepSeconds { get; set; }
        public float DelayBeforeNextSleepTryInLyingDownSeconds { get; set; }
        public float CostPerSecondSleepPercent { get; set; }
        public float CostPerSecondRestPercent { get; set; }
        public float CostPerSecondNoMovementPercent { get; set; }
        public float CostPerSecondNormalWalkingPercent { get; set; }
        public float CostPerSecondCrouchWalkingPercent { get; set; }
        public float CostPerSecondProneWalkingPercent { get; set; }
        public float CostPerSecondJogPercent { get; set; }
        public float CostPerSecondCrouchSprintPercent { get; set; }
        public float CostPerSecondSprintPercent { get; set; }
        public float CostPerRollPercent { get; set; }
        public float CostPerJumpPercent { get; set; }
        public float CostPerVaultPercent { get; set; }
        public float CostPerClimbPercent { get; set; }
        public float CostPerMeleeAttackPercent { get; set; }
        public float CostPerPushCarApplyForcePercent { get; set; }
        public float CostPerMinePercent { get; set; }
        public float CostPerMinePlanksFromPileOfWoodenPlanksPercent { get; set; }
        public float CostPerMinePlanksFromWoodenLogPercent { get; set; }
        public float CostPerBuildPercent { get; set; }
        public float EpinephrineMultiplier { get; set; }
        public float MorphineMultiplier { get; set; }
        public float TremorLevelAtCritical { get; set; }
        public float TremorLevelAtLow { get; set; }
        public int SleepyAlertsWhenLessCriticalIsActive { get; set; }
        public int LossOfConsciousnessWhenLessCriticalIsActive { get; set; }
        public int MovementSpeedReductionWhenLessLowIsActive { get; set; }
        public float CriticalThresholdPercent { get; set; }
        public float LowThresholdPercent { get; set; }
        public float SleepinessThresholdPercent { get; set; }
    }

    public class InediaStaminaInediaMovementOptions
    {
        public int IsActive { get; set; }
        public int WeightRestrictionsHandlerIsActive { get; set; }
        public float WeakSlowdownIfLoadGreaterThanKg { get; set; }
        public float MediumSlowdownIfLoadGreaterThanKg { get; set; }
        public float StrongSlowdownIfLoadGreaterThanKg { get; set; }
        public float ExtremeSlowdownIfLoadGreaterThanKg { get; set; }
        public float ForcedWalkIfLoadGreaterThanKg { get; set; }
        public float MovementRestrictionIfLoadGreaterThanKg { get; set; }
        public int SlopeRestrictionsHandlerIsActive { get; set; }
        public float WeakSlowdownIfSlopeGreaterThanDegree { get; set; }
        public float MediumSlowdownIfSlopeGreaterThanDegree { get; set; }
        public float StrongSlowdownIfSlopeGreaterThanDegree { get; set; }
        public float ExtremeSlowdownIfSlopeGreaterThanDegree { get; set; }
        public int SurfaceRestrictionsHandlerIsActive { get; set; }
        public int SurfaceRoadSpeedReduction { get; set; }
        public int SurfaceGrassSpeedReduction { get; set; }
        public int SurfaceGrassTallSpeedReduction { get; set; }
        public int SurfaceForestSpeedReduction { get; set; }
        public int SurfaceStoneSpeedReduction { get; set; }
        public int SurfaceSandSpeedReduction { get; set; }
        public int SurfaceWaterSpeedReduction { get; set; }
        public Dictionary<string, int> SurfaceFootwearSpeedImpact { get; set; }
        public int ForcedWalkIfCharacterIsInBush { get; set; }
        public Dictionary<string, bool> BushClasses { get; set; }
        public int CumulativeSpeedReductionIsActive { get; set; }
        public float SyberiaProjectStrengthImpactFrom { get; set; }
        public float SyberiaProjectStrengthImpactTo { get; set; }
    }


    public class InediaStaminaStimulant
    {
        public string ItemClass { get; set; }
        public float StaminaGeneralIncreasePercent { get; set; }
        public float StaminaSleepIncreasePercent { get; set; }

    }
    public class InediaStaminaLiquidStimulant
    {
        public int LiquidId { get; set; }
        public float StaminaGeneralIncreasePerLiterPercent { get; set; }
        public float StaminaSleepIncreasePerLiterPercent { get; set; }

   }

    public class InediaStaminaComfortablePlace
    {
        public string ItemClass { get; set; }
        public float StaminaGeneralMultiplier { get; set; }
        public float StaminaSleepMultiplier { get; set; }
    }

    public class InediaStaminaCloth
    {
        public string ItemClass { get; set; }
        public float StaminaGeneralMultiplier { get; set; }
        public float StaminaSleepMultiplier { get; set; }
    }

    public class InediaStaminaHumanPoweredVehicle
    {
        public string ItemClass { get; set; }
        public float StaminaGeneralCostPerSecondPercent { get; set; }
        public float StaminaGeneralCostPerSecondAccelerationPercent { get; set; }
    }

}
