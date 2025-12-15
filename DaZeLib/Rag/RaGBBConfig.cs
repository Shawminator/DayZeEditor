using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class RaGBBConfig
    {
        const int CurrentVersion = 1;

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty;

        public int Version { get; set; }

        public bool DisableAllDamage { get; set; }
        public bool DisableDamageButDoors { get; set; }

        public bool DisableDismantle { get; set; }
        public bool DisableCraftVanillaFence { get; set; }
        public bool DisableCraftVanillaWatchtower { get; set; }

        public bool BaseCanAttachXmasLights { get; set; }
        public bool BaseInfiniteLifetime { get; set; }

        public bool BaseBuildAnywhere { get; set; }
        public bool FlagBuildAnywhere { get; set; }

        public bool EnableHideShowInventory { get; set; }
        public bool ReturnKitAfterBuilt { get; set; }
        public bool EnableBuildHatchLadder { get; set; }
        public bool EnableAttachPortableGasLamp { get; set; }

        public float BaseBuildTime { get; set; }
        public float BaseDismantleTime { get; set; }
        public float BaseBuildToolDamage { get; set; }
        public float BaseDismantleToolDamage { get; set; }
        public float BaseDestroyToolDamage { get; set; }
        public float ToolDamageOnVanillaLockDestroy { get; set; }

        public float HologramRotationSpeed { get; set; }

        public bool DisableAllRaGBBKitCrafting { get; set; }
        public bool EnableBookKitCrafting { get; set; }

        public float PlanksPerKitCraft { get; set; }
        public float NailsPerKitCraft { get; set; }

        public BindingList<string> BaseBuildTools { get; set; }
        public BindingList<string> BaseDismantleTools { get; set; }
        public BindingList<string> BaseDestroyTools { get; set; }

        public BindingList<RaGBBPartCost> PartCosts { get; set; }
        public BindingList<RaGBBCraftToggle> CraftToggles { get; set; }

        //--- Materials ---//
        static float Pillar_NailAmount = 5;
        static float Pillar_PlanksAmount = 5;
        static float Pillar_LogsAmount = 1;
        static float Wall_NailAmount = 25;
        static float Wall_PlanksAmount = 10;
        static float Wall_LogsAmount = 4;
        static float DoorFrame_NailAmount = 20;
        static float DoorFrame_PlanksAmount = 10;
        static float DoorFrame_LogsAmount = 4;
        static float Door_NailAmount = 5;
        static float Door_PlanksAmount = 5;
        static float HatchFrame_NailAmount = 20;
        static float HatchFrame_PlanksAmount = 10;
        static float HatchFrame_LogsAmount = 4;
        static float HatchDoor_NailAmount = 5;
        static float HatchDoor_PlanksAmount = 5;
        static float HatchStaircase_NailAmount = 20;
        static float HatchStaircase_PlanksAmount = 10;
        static float HatchLadder_NailAmount = 10;
        static float HatchLadder_PlanksAmount = 5;
        static float GateFrame_NailAmount = 20;
        static float GateFrame_PlanksAmount = 10;
        static float GateFrame_LogsAmount = 2;
        static float GateDoorLeft_NailAmount = 10;
        static float GateDoorLeft_PlanksAmount = 10;
        static float GateDoorRight_NailAmount = 10;
        static float GateDoorRight_PlanksAmount = 10;
        static float GateLeftFrame_NailAmount = 20;
        static float GateLeftFrame_PlanksAmount = 10;
        static float GateLeftFrame_LogsAmount = 1;
        static float GateLeftDoor_NailAmount = 10;
        static float GateLeftDoor_PlanksAmount = 10;
        static float GateRightFrame_NailAmount = 20;
        static float GateRightFrame_PlanksAmount = 10;
        static float GateRightFrame_LogsAmount = 1;
        static float GateRightDoor_NailAmount = 10;
        static float GateRightDoor_PlanksAmount = 10;
        static float WindowBigFrame_NailAmount = 25;
        static float WindowBigFrame_PlanksAmount = 10;
        static float WindowBigFrame_LogsAmount = 4;
        static float WindowBig_NailAmount = 5;
        static float WindowBig_PlanksAmount = 5;
        static float WindowSmallFrame_NailAmount = 25;
        static float WindowSmallFrame_PlanksAmount = 10;
        static float WindowSmallFrame_LogsAmount = 4;
        static float WindowSmall_NailAmount = 5;
        static float WindowSmall_PlanksAmount = 5;
        static float AngledRoof_NailAmount = 25;
        static float AngledRoof_PlanksAmount = 15;
        static float AngledRoof_LogsAmount = 4;
        static float AngledRoofEndLeft_NailAmount = 5;
        static float AngledRoofEndLeft_PlanksAmount = 5;
        static float AngledRoofEndRight_NailAmount = 5;
        static float AngledRoofEndRight_PlanksAmount = 5;
        static float Floor_NailAmount = 25;
        static float Floor_PlanksAmount = 10;
        static float Floor_LogsAmount = 2;
        static float Ramp_NailAmount = 15;
        static float Ramp_PlanksAmount = 10;
        static float Ramp_LogsAmount = 2;
        static float Ceiling_NailAmount = 25;
        static float Ceiling_PlanksAmount = 10;
        static float Ceiling_LogsAmount = 2;
        static float StairCase1_NailAmount = 60;
        static float StairCase1_PlanksAmount = 15;
        static float StairCase1_LogsAmount = 2;
        static float StairCase2_NailAmount = 60;
        static float StairCase2_PlanksAmount = 20;
        static float StairCase2_LogsAmount = 2;
        static float Stairs_NailAmount = 15;
        static float Stairs_PlanksAmount = 10;
        static float Stairs_LogsAmount = 2;
        static float Foundation_NailAmount = 30;
        static float Foundation_PlanksAmount = 15;
        static float Foundation_LogsAmount = 4;
        static float LowWall_NailAmount = 15;
        static float LowWall_PlanksAmount = 5;
        static float LowWall_LogsAmount = 2;
        static float WoodStorageFloor_NailAmount = 10;
        static float WoodStorageFloor_PlanksAmount = 5;
        static float WoodStorageFloor_LogsAmount = 1;
        static float WoodStorageFrame_NailAmount = 10;
        static float WoodStorageFrame_PlanksAmount = 5;
        static float WoodStorageRoof_NailAmount = 10;
        static float WoodStorageRoof_PlanksAmount = 5;
        static float SingleDoorFrame_NailAmount = 20;
        static float SingleDoorFrame_PlanksAmount = 10;
        static float SingleDoorFrame_LogsAmount = 2;
        static float SingleDoor_NailAmount = 5;
        static float SingleDoor_PlanksAmount = 5;
        static float Ladder_NailAmount = 10;
        static float Ladder_PlanksAmount = 5;

        //--- Kit Crafting ---//

        static bool EnableCraftCeilingKit = true;
        static bool EnableCraftDoorKit = true;
        static bool EnableCraftFloorKit = true;
        static bool EnableCraftFoundationKit = true;
        static bool EnableCraftGateKit = true;
        static bool EnableCraftGateLeftKit = true;
        static bool EnableCraftGateRightKit = true;
        static bool EnableCraftHatchKit = true;
        static bool EnableCraftLowWallKit = true;
        static bool EnableCraftPillarKit = true;
        static bool EnableCraftRampKit = true;
        static bool EnableCraftRoofKit = true;
        static bool EnableCraftSingleDoorKit = true;
        static bool EnableCraftSmallWindowKit = true;
        static bool EnableCraftStaircase2Kit = true;
        static bool EnableCraftStaircaseKit = true;
        static bool EnableCraftStairsKit = true;
        static bool EnableCraftStepLadder = true;
        static bool EnableCraftWallKit = true;
        static bool EnableCraftWindowKit = true;
        static bool EnableCraftWoodStorageKit = true;

        public RaGBBConfig()
        {
            Version = CurrentVersion;

            BaseBuildTools = new BindingList<string>(){ "Hatchet", "Hammer"};
            BaseDismantleTools = new BindingList<string>(){ "Hatchet", "Crowbar"} ;
            BaseDestroyTools = new BindingList<string>(){ "SledgeHammer", "Pickaxe", "FirefighterAxe", "WoodAxe"};

            DisableAllDamage = false;
            DisableDamageButDoors = false;

            DisableDismantle = false;
            DisableCraftVanillaFence = false;
            DisableCraftVanillaWatchtower = false;

            BaseCanAttachXmasLights = true;
            BaseInfiniteLifetime = false;
            
            BaseBuildAnywhere = false;
            FlagBuildAnywhere = false;

            EnableHideShowInventory = true;
            ReturnKitAfterBuilt = false;
            EnableBuildHatchLadder = true;
            EnableAttachPortableGasLamp = true;

            BaseBuildTime = 8.0f;
            BaseDismantleTime = 15.0f;
            BaseBuildToolDamage = 1.0f;
            BaseDismantleToolDamage = 3.0f;
            BaseDestroyToolDamage = 50.0f;
            ToolDamageOnVanillaLockDestroy = 150.0f;
            
            HologramRotationSpeed = 0.1f;

            DisableAllRaGBBKitCrafting = false;
            EnableBookKitCrafting = true;
            
            PlanksPerKitCraft = 4;
            NailsPerKitCraft = 10;

            PartCosts = new BindingList<RaGBBPartCost>();
            BuildPartCostsFromScalars();
            CraftToggles = new BindingList<RaGBBCraftToggle>();
            BuildCraftTogglesFromScalars();
        }

        public bool checkver()
        {
            if (Version != CurrentVersion)
            {
                Version = CurrentVersion;
                isDirty = true;
                return true;
            }
            return false;
        }
        public void BuildPartCostsFromScalars()
        {
            PartCosts.Add(new RaGBBPartCost("Pillar", Pillar_NailAmount, Pillar_PlanksAmount, Pillar_LogsAmount));
            PartCosts.Add(new RaGBBPartCost("Wall", Wall_NailAmount, Wall_PlanksAmount, Wall_LogsAmount));
            PartCosts.Add(new RaGBBPartCost("DoorFrame", DoorFrame_NailAmount, DoorFrame_PlanksAmount, DoorFrame_LogsAmount));
            PartCosts.Add(new RaGBBPartCost("Door", Door_NailAmount, Door_PlanksAmount, 0.0f));
            PartCosts.Add(new RaGBBPartCost("HatchFrame", HatchFrame_NailAmount, HatchFrame_PlanksAmount, HatchFrame_LogsAmount));
            PartCosts.Add(new RaGBBPartCost("HatchDoor", HatchDoor_NailAmount, HatchDoor_PlanksAmount, 0.0f));
            PartCosts.Add(new RaGBBPartCost("HatchStaircase", HatchStaircase_NailAmount, HatchStaircase_PlanksAmount, 0.0f));
            PartCosts.Add(new RaGBBPartCost("HatchLadder", HatchLadder_NailAmount, HatchLadder_PlanksAmount, 0.0f));
            PartCosts.Add(new RaGBBPartCost("GateFrame", GateFrame_NailAmount, GateFrame_PlanksAmount, GateFrame_LogsAmount));
            PartCosts.Add(new RaGBBPartCost("GateDoorLeft", GateDoorLeft_NailAmount, GateDoorLeft_PlanksAmount, 0.0f));
            PartCosts.Add(new RaGBBPartCost("GateDoorRight", GateDoorRight_NailAmount, GateDoorRight_PlanksAmount, 0.0f));
            PartCosts.Add(new RaGBBPartCost("GateLeftFrame", GateLeftFrame_NailAmount, GateLeftFrame_PlanksAmount, GateLeftFrame_LogsAmount));
            PartCosts.Add(new RaGBBPartCost("GateLeftDoor", GateLeftDoor_NailAmount, GateLeftDoor_PlanksAmount, 0.0f));
            PartCosts.Add(new RaGBBPartCost("GateRightFrame", GateRightFrame_NailAmount, GateRightFrame_PlanksAmount, GateRightFrame_LogsAmount));
            PartCosts.Add(new RaGBBPartCost("GateRightDoor", GateRightDoor_NailAmount, GateRightDoor_PlanksAmount, 0.0f));
            PartCosts.Add(new RaGBBPartCost("WindowBigFrame", WindowBigFrame_NailAmount, WindowBigFrame_PlanksAmount, WindowBigFrame_LogsAmount));
            PartCosts.Add(new RaGBBPartCost("WindowBig", WindowBig_NailAmount, WindowBig_PlanksAmount, 0.0f));
            PartCosts.Add(new RaGBBPartCost("WindowSmallFrame", WindowSmallFrame_NailAmount, WindowSmallFrame_PlanksAmount, WindowSmallFrame_LogsAmount));
            PartCosts.Add(new RaGBBPartCost("WindowSmall", WindowSmall_NailAmount, WindowSmall_PlanksAmount, 0.0f));
            PartCosts.Add(new RaGBBPartCost("AngledRoof", AngledRoof_NailAmount, AngledRoof_PlanksAmount, AngledRoof_LogsAmount));
            PartCosts.Add(new RaGBBPartCost("AngledRoofEndLeft", AngledRoofEndLeft_NailAmount, AngledRoofEndLeft_PlanksAmount, 0.0f));
            PartCosts.Add(new RaGBBPartCost("AngledRoofEndRight", AngledRoofEndRight_NailAmount, AngledRoofEndRight_PlanksAmount, 0.0f));
            PartCosts.Add(new RaGBBPartCost("Floor", Floor_NailAmount, Floor_PlanksAmount, Floor_LogsAmount));
            PartCosts.Add(new RaGBBPartCost("Ramp", Ramp_NailAmount, Ramp_PlanksAmount, Ramp_LogsAmount));
            PartCosts.Add(new RaGBBPartCost("Ceiling", Ceiling_NailAmount, Ceiling_PlanksAmount, Ceiling_LogsAmount));
            PartCosts.Add(new RaGBBPartCost("StairCase1", StairCase1_NailAmount, StairCase1_PlanksAmount, StairCase1_LogsAmount));
            PartCosts.Add(new RaGBBPartCost("StairCase2", StairCase2_NailAmount, StairCase2_PlanksAmount, StairCase2_LogsAmount));
            PartCosts.Add(new RaGBBPartCost("Stairs", Stairs_NailAmount, Stairs_PlanksAmount, Stairs_LogsAmount));
            PartCosts.Add(new RaGBBPartCost("Foundation", Foundation_NailAmount, Foundation_PlanksAmount, Foundation_LogsAmount));
            PartCosts.Add(new RaGBBPartCost("LowWall", LowWall_NailAmount, LowWall_PlanksAmount, LowWall_LogsAmount));
            PartCosts.Add(new RaGBBPartCost("WoodStorageFloor", WoodStorageFloor_NailAmount, WoodStorageFloor_PlanksAmount, WoodStorageFloor_LogsAmount));
            PartCosts.Add(new RaGBBPartCost("WoodStorageFrame", WoodStorageFrame_NailAmount, WoodStorageFrame_PlanksAmount, 0.0f));
            PartCosts.Add(new RaGBBPartCost("WoodStorageRoof", WoodStorageRoof_NailAmount, WoodStorageRoof_PlanksAmount, 0.0f));
            PartCosts.Add(new RaGBBPartCost("SingleDoorFrame", SingleDoorFrame_NailAmount, SingleDoorFrame_PlanksAmount, SingleDoorFrame_LogsAmount));
            PartCosts.Add(new RaGBBPartCost("SingleDoor", SingleDoor_NailAmount, SingleDoor_PlanksAmount, 0.0f));
            PartCosts.Add(new RaGBBPartCost("Ladder", Ladder_NailAmount, Ladder_PlanksAmount, 0.0f));
        }
        public void BuildCraftTogglesFromScalars()
        {
            CraftToggles.Add(new RaGBBCraftToggle("CeilingKit", EnableCraftCeilingKit));
            CraftToggles.Add(new RaGBBCraftToggle("DoorKit", EnableCraftDoorKit));
            CraftToggles.Add(new RaGBBCraftToggle("FloorKit", EnableCraftFloorKit));
            CraftToggles.Add(new RaGBBCraftToggle("FoundationKit", EnableCraftFoundationKit));
            CraftToggles.Add(new RaGBBCraftToggle("GateKit", EnableCraftGateKit));
            CraftToggles.Add(new RaGBBCraftToggle("GateLeftKit", EnableCraftGateLeftKit));
            CraftToggles.Add(new RaGBBCraftToggle("GateRightKit", EnableCraftGateRightKit));
            CraftToggles.Add(new RaGBBCraftToggle("HatchKit", EnableCraftHatchKit));
            CraftToggles.Add(new RaGBBCraftToggle("LowWallKit", EnableCraftLowWallKit));
            CraftToggles.Add(new RaGBBCraftToggle("PillarKit", EnableCraftPillarKit));
            CraftToggles.Add(new RaGBBCraftToggle("RampKit", EnableCraftRampKit));
            CraftToggles.Add(new RaGBBCraftToggle("RoofKit", EnableCraftRoofKit));
            CraftToggles.Add(new RaGBBCraftToggle("SingleDoorKit", EnableCraftSingleDoorKit));
            CraftToggles.Add(new RaGBBCraftToggle("SmallWindowKit", EnableCraftSmallWindowKit));
            CraftToggles.Add(new RaGBBCraftToggle("Staircase2Kit", EnableCraftStaircase2Kit));
            CraftToggles.Add(new RaGBBCraftToggle("StaircaseKit", EnableCraftStaircaseKit));
            CraftToggles.Add(new RaGBBCraftToggle("StairsKit", EnableCraftStairsKit));
            CraftToggles.Add(new RaGBBCraftToggle("StepLadder", EnableCraftStepLadder));
            CraftToggles.Add(new RaGBBCraftToggle("WallKit", EnableCraftWallKit));
            CraftToggles.Add(new RaGBBCraftToggle("WindowKit", EnableCraftWindowKit));
            CraftToggles.Add(new RaGBBCraftToggle("WoodStorageKit", EnableCraftWoodStorageKit));
        }
    }

    public class RaGBBPartCost
    {
        public string Part { get; set; }
        public float NAILS { get; set; }
        public float PLANKS { get; set; }
        public float LOGS { get; set; }

        public RaGBBPartCost(string part = "", float nails = 0.0f, float planks = 0.0f, float logs = 0.0f)
        {
            Part = part;
            NAILS = nails;
            PLANKS = planks;
            LOGS = logs;
        }
        public override string ToString()
        {
            return Part;
        }
    };
    public class RaGBBCraftToggle
    {
        public string Kit { get; set; }
        public bool ENABLED { get; set; }

        public RaGBBCraftToggle(string kit = "", bool enabled = true)
        {
            Kit = kit;
            ENABLED = enabled;
        }
        public override string ToString()
        {
            return Kit;
        }
    };
}
