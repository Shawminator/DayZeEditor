using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class RagBasebuilding
    {
        const float CurrentVersion = 2.0f;

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty;

        public float version { get; set; }

        public BindingList<string> BaseBuildTools { get; set; }
        public BindingList<string> BaseDismantleTools { get; set; }
        public BindingList<string> BaseDestroyTools { get; set; }

        public int BaseBuildTime { get; set; }
        public int BaseDismantleTime { get; set; }
        public int BaseDestroyTimeDefault { get; set; }
        public int BaseDestroyTime { get; set; }

        public int BaseBuildToolDamage { get; set; }
        public int BaseDismantleToolDamage { get; set; }
        public int BaseDestroyToolDamage { get; set; }

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

        public float HologramRotationSpeed { get; set; }

        public bool EnableHideShowInventory { get; set; }
        public bool ReturnKitAfterBuilt { get; set; }
        public bool EnableBuildHatchLadder { get; set; }
        public bool EnableAttachPortableGasLamp { get; set; }

        public RaG_BB_Materials Materials { get; set; }
        public RaG_BB_Crafting Crafting { get; set; }

        public RagBasebuilding()
        {
            version = 2;

            BaseBuildTools = new BindingList<string>(){ "Hatchet", "Hammer"};
            BaseDismantleTools = new BindingList<string>(){ "Hatchet", "Crowbar"} ;
            BaseDestroyTools = new BindingList<string>(){ "SledgeHammer", "Pickaxe", "FirefighterAxe", "WoodAxe"};

            BaseBuildTime = 8;
            BaseDismantleTime = 15;
            BaseDestroyTimeDefault = 600;
            BaseDestroyTime = 600;

            BaseBuildToolDamage = 1;
            BaseDismantleToolDamage = 3;
            BaseDestroyToolDamage = 50;

            BaseCanAttachXmasLights = true;
            BaseInfiniteLifetime = false;
            BaseRaidOnlyDoors = false;
            BaseBuildAnywhere = false;
            FlagBuildAnywhere = false;
            TentBuildAnywhere = false;
            VanillaBuildAnywhere = false;
            DisableCraftVanillaFence = false;
            DisableCraftVanillaWatchtower = false;

            DisableDestroy = false;
            DisableDismantle = false;

            HologramRotationSpeed = 0.1f;

            EnableHideShowInventory = true;
            ReturnKitAfterBuilt = false;
            EnableBuildHatchLadder = true;
            EnableAttachPortableGasLamp = true;

            Materials = new RaG_BB_Materials();
            Crafting = new RaG_BB_Crafting();
        }

        public bool checkver()
        {
            if (version != CurrentVersion)
            {
                version = CurrentVersion;
                isDirty = true;
                return true;
            }
            return false;
        }
    }
    public class RaG_BB_Materials
    {
        public int Pillar_NailAmount{ get; set;  }
        public int Pillar_PlanksAmount { get; set; }
        public int Pillar_LogsAmount { get; set; }
        public int Wall_NailAmount { get; set; }
        public int Wall_PlanksAmount { get; set; }
        public int Wall_LogsAmount { get; set; }
        public int DoorFrame_NailAmount { get; set; }
        public int DoorFrame_PlanksAmount { get; set; }
        public int DoorFrame_LogsAmount { get; set; }
        public int Door_NailAmount { get; set; }
        public int Door_PlanksAmount { get; set; }
        public int HatchFrame_NailAmount { get; set; }
        public int HatchFrame_PlanksAmount { get; set; }
        public int HatchFrame_LogsAmount { get; set; }
        public int HatchDoor_NailAmount { get; set; }
        public int HatchDoor_PlanksAmount { get; set; }
        public int HatchStaircase_NailAmount { get; set; }
        public int HatchStaircase_PlanksAmount { get; set; }
        public int HatchLadder_NailAmount { get; set; }
        public int HatchLadder_PlanksAmount { get; set; }
        public int GateFrame_NailAmount { get; set; }
        public int GateFrame_PlanksAmount { get; set; }
        public int GateFrame_LogsAmount { get; set; }
        public int GateDoorLeft_NailAmount { get; set; }
        public int GateDoorLeft_PlanksAmount { get; set; }
        public int GateDoorRight_NailAmount { get; set; }
        public int GateDoorRight_PlanksAmount { get; set; }
        public int GateLeftFrame_NailAmount { get; set; }
        public int GateLeftFrame_PlanksAmount { get; set; }
        public int GateLeftFrame_LogsAmount { get; set; }
        public int GateLeftDoor_NailAmount { get; set; }
        public int GateLeftDoor_PlanksAmount { get; set; }
        public int GateRightFrame_NailAmount { get; set; }
        public int GateRightFrame_PlanksAmount { get; set; }
        public int GateRightFrame_LogsAmount { get; set; }
        public int GateRightDoor_NailAmount { get; set; }
        public int GateRightDoor_PlanksAmount { get; set; }
        public int WindowBigFrame_NailAmount { get; set; }
        public int WindowBigFrame_PlanksAmount { get; set; }
        public int WindowBigFrame_LogsAmount { get; set; }
        public int WindowBig_NailAmount { get; set; }
        public int WindowBig_PlanksAmount { get; set; }
        public int WindowSmallFrame_NailAmount { get; set; }
        public int WindowSmallFrame_PlanksAmount { get; set; }
        public int WindowSmallFrame_LogsAmount { get; set; }
        public int WindowSmall_NailAmount { get; set; }
        public int WindowSmall_PlanksAmount { get; set; }
        public int AngledRoof_NailAmount { get; set; }
        public int AngledRoof_PlanksAmount { get; set; }
        public int AngledRoof_LogsAmount { get; set; }
        public int AngledRoofEndLeft_NailAmount { get; set; }
        public int AngledRoofEndLeft_PlanksAmount { get; set; }
        public int AngledRoofEndRight_NailAmount { get; set; }
        public int AngledRoofEndRight_PlanksAmount { get; set; }
        public int Floor_NailAmount { get; set; }
        public int Floor_PlanksAmount { get; set; }
        public int Floor_LogsAmount { get; set; }
        public int Ramp_NailAmount { get; set; }
        public int Ramp_PlanksAmount { get; set; }
        public int Ramp_LogsAmount { get; set; }
        public int Ceiling_NailAmount { get; set; }
        public int Ceiling_PlanksAmount { get; set; }
        public int Ceiling_LogsAmount { get; set; }
        public int StairCase1_NailAmount { get; set; }
        public int StairCase1_PlanksAmount { get; set; }
        public int StairCase1_LogsAmount { get; set; }
        public int StairCase2_NailAmount { get; set; }
        public int StairCase2_PlanksAmount { get; set; }
        public int StairCase2_LogsAmount { get; set; }
        public int Stairs_NailAmount { get; set; }
        public int Stairs_PlanksAmount { get; set; }
        public int Stairs_LogsAmount { get; set; }
        public int Foundation_NailAmount { get; set; }
        public int Foundation_PlanksAmount { get; set; }
        public int Foundation_LogsAmount { get; set; }
        public int LowWall_NailAmount { get; set; }
        public int LowWall_PlanksAmount { get; set; }
        public int LowWall_LogsAmount { get; set; }
        public int WoodStorageFloor_NailAmount { get; set; }
        public int WoodStorageFloor_PlanksAmount { get; set; }
        public int WoodStorageFloor_LogsAmount { get; set; }
        public int WoodStorageFrame_NailAmount { get; set; }
        public int WoodStorageFrame_PlanksAmount { get; set; }
        public int WoodStorageRoof_NailAmount { get; set; }
        public int WoodStorageRoof_PlanksAmount { get; set; }
        public int SingleDoorFrame_NailAmount { get; set; }
        public int SingleDoorFrame_PlanksAmount { get; set; }
        public int SingleDoorFrame_LogsAmount { get; set; }
        public int SingleDoor_NailAmount { get; set; }
        public int SingleDoor_PlanksAmount { get; set; }
        public int Ladder_NailAmount { get; set; }
        public int Ladder_PlanksAmount { get; set; }
    };
    public class RaG_BB_Crafting
    {
        public bool DisableAllRaGBBKitCrafting { get; set; }

        public float WoodenPlankCraftAmount { get; set; }
        public float NailCraftAmount { get; set; }

        public bool EnableCraftCeilingKit { get; set; }
        public bool EnableCraftDoorKit { get; set; }
        public bool EnableCraftFloorKit { get; set; }
        public bool EnableCraftFoundationKit { get; set; }
        public bool EnableCraftGateKit { get; set; }
        public bool EnableCraftGateLeftKit { get; set; }
        public bool EnableCraftGateRightKit { get; set; }
        public bool EnableCraftHatchKit { get; set; }
        public bool EnableCraftLowWallKit { get; set; }
        public bool EnableCraftPillarKit { get; set; }
        public bool EnableCraftRampKit { get; set; }
        public bool EnableCraftRoofKit { get; set; }
        public bool EnableCraftSingleDoorKit { get; set; }
        public bool EnableCraftSmallWindowKit { get; set; }
        public bool EnableCraftStaircase2Kit { get; set; }
        public bool EnableCraftStaircaseKit { get; set; }
        public bool EnableCraftStairsKit { get; set; }
        public bool EnableCraftStepLadder { get; set; }
        public bool EnableCraftWallKit { get; set; }
        public bool EnableCraftWindowKit { get; set; }
        public bool EnableCraftWoodStorageKit { get; set; }
    };
}
