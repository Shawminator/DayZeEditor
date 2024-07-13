using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class InediaInfectedAI_Config
    {
        [JsonIgnore]
        public bool isDirty;
        [JsonIgnore]
        public string Filename { get; set; }

        public InediaInfectedAI_ConfigZombies Zombies { get; set; }
        public InediaInfectedAI_ConfigPlayers Players { get; set; }
        public InediaInfectedAI_ConfigVehicles Vehicles { get; set; }
        public float LossInterestToIrritantAfterSeconds { get; set; }
        public float LightInHouseMultiplier { get; set; }
        public float SoundInHouseMultiplier { get; set; }
        public int SaveConfigAfterInit { get; set; }

    }
    public class InediaInfectedAI_ConfigZombies
    {
        public Dictionary<string, List<string>> Groups { get; set; }
        public Dictionary<string, InediaInfectedAI_CustomIrritant> CustomIrritants { get; set; }
        public Dictionary<string, InediaInfectedAI_ConfigThrowingProjectile> ThrowingProjectiles { get; set; }
        public Dictionary<string, bool> BreakingDoorsHandlerIsActive { get; set; }
        public Dictionary<string, Dictionary<string, List<int>>> BreakingDoorsRestrictedDoors { get; set; }
        public Dictionary<string, float> BreakingDoorsDestroyAfterOpenChancePercent { get; set; }
        public Dictionary<string, float> BreakingDoorsOpenChancePercent { get; set; }
        public Dictionary<string, float> BreakingDoorsOpenLockPickChancePercent { get; set; }
        public Dictionary<string, float> BreakingDoorsLossInterestAfterHitChancePercent { get; set; }
        public Dictionary<string, float> BreakingDoorsLossInterestAfterHitLockPickChancePercent { get; set; }
        public Dictionary<string, float> BreakingDoorsCrawlersChanceMultiplier { get; set; }

        public Dictionary<string, bool> AttackPlayersUnconscious { get; set; }

        public Dictionary<string, bool> AttackAnimalsHandlerIsActive { get; set; }
        public Dictionary<string, bool> AttackAnimalsWildBoar { get; set; }
        public Dictionary<string, bool> AttackAnimalsPig { get; set; }
        public Dictionary<string, bool> AttackAnimalsSheep { get; set; }
        //public Dictionary<string, bool> AttackAnimalsChicken { get; set; }
        public Dictionary<string, bool> AttackAnimalsDeer { get; set; }
        public Dictionary<string, bool> AttackAnimalsRoeDeer { get; set; }
        public Dictionary<string, bool> AttackAnimalsGoat { get; set; }
        public Dictionary<string, bool> AttackAnimalsCow { get; set; }
        public Dictionary<string, Dictionary<string, bool>> AttackAnimalsCustom { get; set; }

        public Dictionary<string, List<string>> FriendlyNPC { get; set; }

        public Dictionary<string, bool> SearchHandlerIsActive { get; set; }
        public Dictionary<string, float> SearchSphereRadiusIncreaseEverySeconds { get; set; }
        public Dictionary<string, float> SearchSphereDistanceToRadiusMultiplier { get; set; }
        public Dictionary<string, float> SearchSphereRadiusMin { get; set; }
        public Dictionary<string, float> SearchSphereRadiusMax { get; set; }
        public Dictionary<string, float> SearchSphereRadiusBurst { get; set; }

        public Dictionary<string, bool> SpeedHandlerIsActive { get; set; }
        public Dictionary<string, float> SpeedMinimumInCalmMode { get; set; }
        public Dictionary<string, float> SpeedLimitInCalmMode { get; set; }
        public Dictionary<string, float> SpeedMultiplierInCalmMode { get; set; }
        public Dictionary<string, float> SpeedMinimumInSearchMode { get; set; }
        public Dictionary<string, float> SpeedLimitInSearchMode { get; set; }
        public Dictionary<string, float> SpeedMultiplierInSearchMode { get; set; }
        public Dictionary<string, float> SpeedMinimumInChaseMode { get; set; }
        public Dictionary<string, float> SpeedLimitInChaseMode { get; set; }
        public Dictionary<string, float> SpeedMultiplierInChaseMode { get; set; }
        public Dictionary<string, float> SpeedNoLimitsFromDistanceMeters { get; set; }

        public Dictionary<string, bool> SmellsHandlerIsActive { get; set; }
        public Dictionary<string, float> SmellsSphereRadius { get; set; }
        public Dictionary<string, bool> SmellsAllowStealthKills { get; set; }
        public Dictionary<string, float> SmellsLossInterestAfterReactionForSeconds { get; set; }

        public Dictionary<string, bool> UpJumpHandlerIsActive { get; set; }
        public Dictionary<string, float> UpJumpHeightMax { get; set; }
        public Dictionary<string, float> UpJumpImpulseDamageChancePercent { get; set; }
        public Dictionary<string, float> UpJumpChancePercent { get; set; }
        public Dictionary<string, float> UpJumpCooldownSeconds { get; set; }

        public Dictionary<string, bool> DownJumpHandlerIsActive { get; set; }
        public Dictionary<string, float> DownJumpHeightMax { get; set; }

        public Dictionary<string, bool> StuckJumpHandlerIsActive { get; set; }

        public Dictionary<string, bool> FallHandlerIsActive { get; set; }
        public Dictionary<string, float> FallHandlerLegBreakHeightMin { get; set; }
        public Dictionary<string, float> FallHandlerLegBreakHeightMax { get; set; }
        public Dictionary<string, float> FallHandlerDeathHeightMin { get; set; }
        public Dictionary<string, float> FallHandlerDeathHeightMax { get; set; }

        public Dictionary<string, bool> InjuryHandlerIsActive { get; set; }
        public Dictionary<string, float> InjuryHandlerInjuryOnExplosiveDamageChancePercent { get; set; }
        public Dictionary<string, float> InjuryHandlerDamageThresholdToInjuryArm { get; set; }
        public Dictionary<string, float> InjuryHandlerRestoreInjuredArmAfterSeconds { get; set; }
        public Dictionary<string, float> InjuryHandlerInjuredOneArmAttackMultiplier { get; set; }
        public Dictionary<string, float> InjuryHandlerInjuredBothArmsAttackMultiplier { get; set; }
        public Dictionary<string, float> InjuryHandlerInjuredOneArmBreakingDoorChanceMultiplier { get; set; }
        public Dictionary<string, float> InjuryHandlerInjuredBothArmsBreakingDoorChanceMultiplier { get; set; }
        public Dictionary<string, float> InjuryHandlerInjuredOneArmStunChanceMultiplier { get; set; }
        public Dictionary<string, float> InjuryHandlerInjuredBothArmsStunChanceMultiplier { get; set; }
        public Dictionary<string, float> InjuryHandlerInjuredOneArmPainMultiplier { get; set; }
        public Dictionary<string, float> InjuryHandlerInjuredBothArmsPainMultiplier { get; set; }
        public Dictionary<string, float> InjuryHandlerInjuredOneArmBleedingChanceMultiplier { get; set; }
        public Dictionary<string, float> InjuryHandlerInjuredBothArmsBleedingChanceMultiplier { get; set; }
        public Dictionary<string, float> InjuryHandlerSpawnWithInjuredOneArmChancePercent { get; set; }
        public Dictionary<string, float> InjuryHandlerSpawnWithInjuredBothArmsChancePercent { get; set; }
        public Dictionary<string, float> InjuryHandlerDamageThresholdToInjuryLeg { get; set; }
        public Dictionary<string, float> InjuryHandlerRestoreInjuredLegAfterSeconds { get; set; }
        public Dictionary<string, float> InjuryHandlerInjuredOneLegSpeedLimit { get; set; }
        public Dictionary<string, float> InjuryHandlerInjuredBothLegsSpeedLimit { get; set; }
        public Dictionary<string, float> InjuryHandlerInjuredOneLegJumpHeightMultiplier { get; set; }
        public Dictionary<string, float> InjuryHandlerInjuredBothLegsJumpHeightMultiplier { get; set; }
        public Dictionary<string, float> InjuryHandlerSpawnWithInjuredOneLegChancePercent { get; set; }
        public Dictionary<string, float> InjuryHandlerSpawnWithInjuredBothLegsChancePercent { get; set; }

        public Dictionary<string, bool> AttackCarHandlerIsActive { get; set; }
        public Dictionary<string, bool> AttackCarPlayersIsActive { get; set; }
        public Dictionary<string, float> AttackCarPlayersDistanceMeters { get; set; }
        public Dictionary<string, bool> AttackCarElementsIsActive { get; set; }
        public Dictionary<string, float> AttackCarElementsDistanceMeters { get; set; }
        public Dictionary<string, float> AttackCarElementsGlobalDamageMultiplier { get; set; }
        public Dictionary<string, bool> AttackCarElementsByCollisionsIsActive { get; set; }
        public Dictionary<string, float> AttackCarElementsByCollisionsGlobalDamageMultiplier { get; set; }
        public Dictionary<string, Dictionary<string, float>> AttackCarElementsMultiplierByCarClassId { get; set; }
        public Dictionary<string, float> AttackCarWindowDamagePercent { get; set; }
        public Dictionary<string, float> AttackCarDoorDamagePercent { get; set; }
        public Dictionary<string, float> AttackCarDoorChancePercent { get; set; }
        public Dictionary<string, float> AttackCarRadiatorDamagePercent { get; set; }
        public Dictionary<string, float> AttackCarCarBatteryDamagePercent { get; set; }
        public Dictionary<string, float> AttackCarPlugDamagePercent { get; set; }
        public Dictionary<string, float> AttackCarEngineDamagePercent { get; set; }
        public Dictionary<string, float> AttackCarLightsDamagePercent { get; set; }
        public Dictionary<string, float> AttackCarFenderDamagePercent { get; set; }
        public Dictionary<string, float> AttackCarBumperDamagePercent { get; set; }
        public Dictionary<string, float> AttackCarHoodDamagePercent { get; set; }
        public Dictionary<string, float> AttackCarFuelTankDamagePercent { get; set; }

        public Dictionary<string, bool> ReactHandlerIsActive { get; set; }
        public Dictionary<string, float> ReactBehindVisualMultiplier { get; set; }
        public Dictionary<string, float> ReactBehindNoiseMultiplier { get; set; }
        public Dictionary<string, float> ReactFireplaceVisual { get; set; }
        public Dictionary<string, float> ReactFireplaceDayVisual { get; set; }
        public Dictionary<string, float> ReactFlashlightsVisual { get; set; }
        public Dictionary<string, float> ReactHeadtorchRedVisual { get; set; }
        public Dictionary<string, float> ReactRoadflareVisual { get; set; }
        public Dictionary<string, float> ReactRoadflareNoise { get; set; }
        public Dictionary<string, float> ReactChemlightVisual { get; set; }
        public Dictionary<string, float> ReactChemlightRedVisual { get; set; }
        public Dictionary<string, float> ReactPortableGasLampVisual { get; set; }
        public Dictionary<string, float> ReactFlammablesVisual { get; set; }
        public Dictionary<string, float> ReactSpotlightVisual { get; set; }
        public Dictionary<string, float> ReactCarLightVisual { get; set; }
        public Dictionary<string, float> ReactPowerGeneratorNoise { get; set; }
        public Dictionary<string, float> ReactCarHornNoise { get; set; }
        public Dictionary<string, float> ReactAlarmClockNoise { get; set; }
        public Dictionary<string, float> ReactKitchenTimerNoise { get; set; }
        public Dictionary<string, float> ReactFuelStationNoise { get; set; }
        public Dictionary<string, float> ReactFireworksNoise { get; set; }
        public Dictionary<string, float> ReactExplosiveItemNoise { get; set; }
        public Dictionary<string, float> ReactCarEngineNoise { get; set; }
        public Dictionary<string, float> ReactSmokeGrenadeVisual { get; set; }
        public Dictionary<string, float> ReactScreamNoise { get; set; }
        public Dictionary<string, float> ReactWeaponShotNoise { get; set; }
        public Dictionary<string, float> ReactFootstepsJogDayNoise { get; set; }
        public Dictionary<string, float> ReactFootstepsJogNightNoise { get; set; }
        public Dictionary<string, float> ReactFootstepsCrouchSprintDayNoise { get; set; }
        public Dictionary<string, float> ReactFootstepsCrouchSprintNightNoise { get; set; }
        public Dictionary<string, float> ReactFootstepsSprintDayNoise { get; set; }
        public Dictionary<string, float> ReactFootstepsSprintNightNoise { get; set; }
        public Dictionary<string, float> ReactDoorsDayNoise { get; set; }
        public Dictionary<string, float> ReactDoorsNightNoise { get; set; }
        public Dictionary<string, float> ReactPlayerFallDayNoise { get; set; }
        public Dictionary<string, float> ReactPlayerFallNightNoise { get; set; }
        public Dictionary<string, float> ReactBodyfallDayNoise { get; set; }
        public Dictionary<string, float> ReactBodyfallNightNoise { get; set; }
        public Dictionary<string, float> ReactBodyfallBackstabDayNoise { get; set; }
        public Dictionary<string, float> ReactBodyfallBackstabNightNoise { get; set; }
        public Dictionary<string, float> ReactSymptomsDayNoise { get; set; }
        public Dictionary<string, float> ReactSymptomsNightNoise { get; set; }
        public Dictionary<string, float> ReactVoiceWhisperDayNoise { get; set; }
        public Dictionary<string, float> ReactVoiceWhisperNightNoise { get; set; }
        public Dictionary<string, float> ReactVoiceTalkDayNoise { get; set; }
        public Dictionary<string, float> ReactVoiceTalkNightNoise { get; set; }
        public Dictionary<string, float> ReactVoiceShoutDayNoise { get; set; }
        public Dictionary<string, float> ReactVoiceShoutNightNoise { get; set; }
        public Dictionary<string, float> ReactMiningLightDayNoise { get; set; }
        public Dictionary<string, float> ReactMiningLightNightNoise { get; set; }
        public Dictionary<string, float> ReactMiningHardDayNoise { get; set; }
        public Dictionary<string, float> ReactMiningHardNightNoise { get; set; }
        public Dictionary<string, float> ReactBuildingDayNoise { get; set; }
        public Dictionary<string, float> ReactBuildingNightNoise { get; set; }
        public Dictionary<string, float> ReactSawPlanksDayNoise { get; set; }
        public Dictionary<string, float> ReactSawPlanksNightNoise { get; set; }
        public Dictionary<string, float> ReactVanillaMindstateChange { get; set; }
        public Dictionary<string, Dictionary<string, InediaInfectedAI_ReactCustomIrritant>> ReactCustomIrritants { get; set; }

        public Dictionary<string, float> ReactAndDestroyFlashlightsVisual { get; set; }
        public Dictionary<string, float> ReactAndDestroyHeadtorchRedVisual { get; set; }
        public Dictionary<string, float> ReactAndDestroyRoadflareVisual { get; set; }
        public Dictionary<string, float> ReactAndDestroyRoadflareNoise { get; set; }
        public Dictionary<string, float> ReactAndDestroyChemlightVisual { get; set; }
        public Dictionary<string, float> ReactAndDestroyChemlightRedVisual { get; set; }
        public Dictionary<string, float> ReactAndDestroyPortableGasLampVisual { get; set; }
        public Dictionary<string, float> ReactAndDestroyFlammablesVisual { get; set; }
        public Dictionary<string, float> ReactAndDestroySpotlightVisual { get; set; }
        public Dictionary<string, float> ReactAndDestroyFireplaceVisual { get; set; }
        public Dictionary<string, float> ReactAndDestroyFireplaceDayVisual { get; set; }
        public Dictionary<string, float> ReactAndDestroyCarLightVisual { get; set; }
        public Dictionary<string, float> ReactAndDestroyFireworksNoise { get; set; }
        public Dictionary<string, float> ReactAndDestroyPowerGeneratorNoise { get; set; }
        public Dictionary<string, float> ReactAndDestroyAlarmClockNoise { get; set; }
        public Dictionary<string, float> ReactAndDestroyKitchenTimerNoise { get; set; }
        public Dictionary<string, float> ReactAndDestroySmokeGrenadeVisual { get; set; }

        public Dictionary<string, bool> DamageToPlayerHandlerIsActive { get; set; }
        public Dictionary<string, float> DamageToPlayerHealthMultiplier { get; set; }
        public Dictionary<string, float> DamageToPlayerShockMultiplier { get; set; }
        public Dictionary<string, float> DamageToPlayerStaminaPercent { get; set; }
        public Dictionary<string, float> DamageToPlayerBloodPercent { get; set; }
        public Dictionary<string, bool> DamageToPlayerBleedingHandlerIsActive { get; set; }
        public Dictionary<string, float> DamageToPlayerBleedingChancePercent { get; set; }
        public Dictionary<string, float> DamageToPlayerInBlockHealthMultiplier { get; set; }
        public Dictionary<string, float> DamageToPlayerInBlockShockMultiplier { get; set; }
        public Dictionary<string, float> DamageToPlayerInBlockStaminaPercent { get; set; }
        public Dictionary<string, float> DamageToPlayerInBlockBloodPercent { get; set; }
        public Dictionary<string, float> DamageToPlayerInBlockBleedingChancePercent { get; set; }

        public Dictionary<string, float> StunToPlayerChancePercent { get; set; }
        public Dictionary<string, float> StunToPlayerInBlockChancePercent { get; set; }

        public Dictionary<string, bool> DiseasesToPlayerHandlerIsActive { get; set; }
        public Dictionary<string, List<InediaInfectedAI_ConfigZombieDeseaseAgent>> DiseasesToPlayerAgents { get; set; }

        public Dictionary<string, float> CameraShakeToPlayerIntensity { get; set; }

        public Dictionary<string, bool> PainToPlayerHandlerIsActive { get; set; }
        public Dictionary<string, float> PainToPlayerHeadMultiplier { get; set; }
        public Dictionary<string, float> PainToPlayerHeadInBlockMultiplier { get; set; }
        public Dictionary<string, float> PainToPlayerArmsMultiplier { get; set; }
        public Dictionary<string, float> PainToPlayerArmsInBlockMultiplier { get; set; }
        public Dictionary<string, float> PainToPlayerArmsDisarmMultiplier { get; set; }
        public Dictionary<string, float> PainToPlayerArmsInBlockDisarmMultiplier { get; set; }
        public Dictionary<string, float> PainToPlayerLegsMultiplier { get; set; }
        public Dictionary<string, float> PainToPlayerLegsInBlockMultiplier { get; set; }
        public Dictionary<string, float> PainToPlayerTorsoMultiplier { get; set; }
        public Dictionary<string, float> PainToPlayerTorsoInBlockMultiplier { get; set; }

        public Dictionary<string, bool> DamageToZombieHandlerIsActive { get; set; }
        public Dictionary<string, float> DamageToZombieHeadMeleeMultiplier { get; set; }
        public Dictionary<string, float> DamageToZombieHeadRangeMultiplier { get; set; }
        public Dictionary<string, float> DamageToZombieBodyMeleeMultiplier { get; set; }
        public Dictionary<string, float> DamageToZombieBodyRangeMultiplier { get; set; }
        public Dictionary<string, float> DamageToZombieFromCarsMultiplier { get; set; }
        public Dictionary<string, float> DamageToZombieFromExplosionsMultiplier { get; set; }
        public Dictionary<string, float> DamageToZombieFromHotItemsHp { get; set; }
        public Dictionary<string, bool> DamageToZombieShockToStunHandlerIsActive { get; set; }
        public Dictionary<string, float> DamageToZombieShockToStunLightHeavyAnimationThresholdMeleeHeavy { get; set; }
        public Dictionary<string, float> DamageToZombieShockToStunLightHeavyAnimationThresholdRange { get; set; }
        public Dictionary<string, float> DamageToZombieShockToStunThresholdMeleeHeavy { get; set; }
        public Dictionary<string, float> DamageToZombieShockToStunThresholdRange { get; set; }
        public Dictionary<string, float> DamageToZombieShockToStunFromButtstockHit { get; set; }
        public Dictionary<string, bool> DamageToZombieShockToStunCumulativeDamage { get; set; }
        public Dictionary<string, Dictionary<string, float>> DamageToZombieWeaponsMultipliers { get; set; }

        public Dictionary<string, bool> MeleeAttacksDodgeHandlerIsActive { get; set; }
        public Dictionary<string, float> MeleeAttacksDodgeChance { get; set; }
        public Dictionary<string, float> MeleeAttacksDodgeCooldownSeconds { get; set; }

        public Dictionary<string, bool> BloodHandlerIsActive { get; set; }
        public Dictionary<string, float> BloodVolumeMl { get; set; }
        public Dictionary<string, float> BloodLossRateMinMl { get; set; }
        public Dictionary<string, float> BloodLossRateMaxMl { get; set; }
        public Dictionary<string, float> BloodLossRateRegenMl { get; set; }
        public Dictionary<string, float> BloodOnExplosiveDamageChancePercent { get; set; }

        public Dictionary<string, bool> ResistContaminatedEffectHandlerIsActive { get; set; }
        public Dictionary<string, bool> ResistContaminatedEffect { get; set; }

        public Dictionary<string, bool> AllowCrawling { get; set; }
        public Dictionary<string, float> RespawnInCrawlingChancePercent { get; set; }

        public Dictionary<string, bool> ScreamHandlerIsActive { get; set; }
        public Dictionary<string, float> ScreamAttractsZombiesInRadius { get; set; }
        public Dictionary<string, float> ScreamChancePercent { get; set; }
        public Dictionary<string, float> ScreamCooldownSeconds { get; set; }
        public Dictionary<string, float> ScreamNearbyInfectedSilenceSeconds { get; set; }

        public Dictionary<string, bool> SearchBodyToViewCargo { get; set; }
        public Dictionary<string, float> SearchBodyToViewCargoSeconds { get; set; }
        public Dictionary<string, float> SearchBodyExtendLootingToPlayersInRadiusMeters { get; set; }
        public Dictionary<string, bool> SearchBodyOnlyEmptyHands { get; set; }
        public Dictionary<string, float> SearchBodyWithoutGlovesBloodyHandsChancePercent { get; set; }
        public Dictionary<string, float> SearchBodyWithoutMaskVomitChancePercent { get; set; }

        public Dictionary<string, bool> CanBeButchered { get; set; }
        public Dictionary<string, float> ButcheringSeconds { get; set; }
        public Dictionary<string, float> ButcheringWithoutGlovesBloodyHandsChancePercent { get; set; }
        public Dictionary<string, float> ButcheringWithoutMaskVomitChancePercent { get; set; }
        public Dictionary<string, List<InediaInfectedAI_ConfigZombieItemAfterButchering>> ItemsAfterButchering { get; set; }

        public Dictionary<string, bool> CanBeBackstabbedHandlerIsActive { get; set; }
        public Dictionary<string, bool> CanBeBackstabbed { get; set; }

        public Dictionary<string, float> VisionRangeMultiplierDay { get; set; }
        public Dictionary<string, float> VisionRangeMultiplierNight { get; set; }

        public Dictionary<string, bool> SizeHandlerIsActive { get; set; }
        public Dictionary<string, float> SizeFromMultiplier { get; set; }
        public Dictionary<string, float> SizeToMultiplier { get; set; }
        public Dictionary<string, bool> SizeDamageScalingIsActive { get; set; }
        public Dictionary<string, bool> SizeBloodVolumeScalingIsActive { get; set; }
        public Dictionary<string, bool> SizeSpeedMultiplierScalingIsActive { get; set; }

        public Dictionary<string, bool> ThrowingProjectilesHandlerIsActive { get; set; }
        public Dictionary<string, float> ThrowingProjectilesHandlerDamageMultiplier { get; set; }
        public Dictionary<string, float> ThrowingProjectilesHandlerVehiclesDamageMultiplier { get; set; }
        public Dictionary<string, List<string>> ThrowingProjectilesHandlerProjectilesList { get; set; }

        public Dictionary<string, bool> RangeAttacksHandlerIsActive { get; set; }
        public Dictionary<string, float> RangeAttacksHandlerZombiePlayerDistance { get; set; }
        public Dictionary<string, float> RangeAttacksHandlerPlayerOnObstacleHeight { get; set; }

        public Dictionary<string, bool> PreventClustering { get; set; }
    }
    public class InediaInfectedAI_CustomIrritant
    {
        public List<string> Classes { get; set; }
        public float RadiusOutdoorDay { get; set; }
        public float RadiusOutdoorNight { get; set; }
        public float RadiusIndoorDay { get; set; }
        public float RadiusIndoorNight { get; set; }
        public float Type { get; set; }
        public float Priority { get; set; }

       }
    public class InediaInfectedAI_ReactCustomIrritant
    {
        public float React { get; set; }
        public float Destroy { get; set; }

    }
    public class InediaInfectedAI_ConfigThrowingProjectile
    {
        public List<string> ItemClasses { get; set; }
        public Dictionary<string, float> Parameters { get; set; }
    }
    public class InediaInfectedAI_ConfigZombieItemAfterButchering
    {
        public string ClassId { get; set; }
        public float DropChancePercent { get; set; }
        public float QuantityInStackFromPercent { get; set; }
        public float QuantityInStackToPercent { get; set; }
        public int ConditionFrom { get; set; }
        public int ConditionTo { get; set; }
        public List<int> Foodstages { get; set; }

    }
    public class InediaInfectedAI_ConfigZombieDeseaseAgent
    {
        public int AgentId { get; set; }
        public float AddChance { get; set; }
        public float AddChanceInBlock { get; set; }
        public float AddAmount { get; set; }
        public float AddAmountInBlock { get; set; }
        public float AddAmountNoMoreThan { get; set; }
        public bool AddOnlyIfBleeding { get; set; }
    }
    public class InediaInfectedAI_ConfigPlayers
    {
        public bool SmellsHandlerIsActive { get; set; }
        public float SmellLifetimeSeconds { get; set; }
        public int SmellsCountOnMapLimit { get; set; }
        public bool FootstepsNoiseHandlerIsActive { get; set; }
        public bool SearchBodyToViewCargo { get; set; }
        public float SearchBodyToViewCargoSeconds { get; set; }
        public float SearchBodyExtendLootingToPlayersInRadiusMeters { get; set; }
        public bool SearchBodyOnlyEmptyHands { get; set; }

        public bool QuietDoorOpeningMechanicIsActive { get; set; }
        public bool QuietDoorOpeningMechanicDisabledForOpening { get; set; }
        public float QuietDoorOpeningMechanicSeconds { get; set; }
        public List<string> QuietDoorOpeningMechanicRestrictedBuildings { get; set; }

        /*
        public float FootstepsNoiseJogRadius  { get; set; }
        public float FootstepsNoiseCrouchSprintRadius  { get; set; }
        public float FootstepsNoiseSprintRadius  { get; set; }
        */

        public bool DisableVanillaDoorNoiseSystem { get; set; }

        public bool PainSystemIsActive { get; set; }
        public bool ShowPainBadges { get; set; }
        public float PainHealingRatePerSecondPercent { get; set; }
        public float HealthDrainWithOverPainedLimbPerSecondPercent { get; set; }
        public float BleedingChanceMultiplierWhenOverPainedLimbReceivingHits { get; set; }
        public bool DisableVanillaLegsDamageSystem { get; set; }
        public bool ShowPainBlur { get; set; }
        public bool UnconsciousWhenOverPainedHeadReceivingHits { get; set; }
        public bool BreakOverPainedLegsWhenReceivingHits { get; set; }
        public bool BreakOverPainedLegsWhenOtherFactors { get; set; }
        public bool CrouchWhenOverPainedTorsoReceivingHits { get; set; }
        public bool ArmsLowerWhenAimingWithInjuredArms { get; set; }
        public bool ArmsLowerWhenOverPainedArmsReceivingHits { get; set; }
        public bool AuditoryHallucinationsWithPainedHead { get; set; }
        public bool TinnitusWhenHeadReceivingHits { get; set; }
        public bool BlurWhenHeadReceivingHits { get; set; }
        public bool SoundAttenuationWhenHeadReceivingHits { get; set; }
        public bool MovementCoordinationImpairmentWhenHeadReceivingHits { get; set; }
        public bool PainArmsWhenHitWithoutGloves { get; set; }
        public bool ShockIfRunsWithPainedLegs { get; set; }
        public bool ShockIfAttackingWithPainedArms { get; set; }
        public bool AllowPainFromGrenades { get; set; }

        public float NoiseMultiplierDay { get; set; }
        public float NoiseMultiplierNight { get; set; }
        public float NoiseMultiplierCrouchDay { get; set; }
        public float NoiseMultiplierCrouchNight { get; set; }

        public Dictionary<string, float> CamoClothingVisibilityMultipliers { get; set; }
    }
    public class InediaInfectedAI_ConfigVehicles
    {
        public Dictionary<string, Dictionary<string, float>> Noise { get; set; }
        public Dictionary<string, float> CollisionsSoundThreshold { get; set; }
        public Dictionary<string, float> CollisionsSpeedReductionMultiplier { get; set; }
    }
}
