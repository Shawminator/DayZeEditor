using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DayZeLib;

namespace DayZeLib
{
    public class DNA_Keycards
    {
        public string dna_ProfilePath { get; set; }
        public string dna_ConfigFolderName = "DNA_Keycards";
        public string dna_ConfigName = "/KeyCard_";
        public string dna_ConfigExtension = "_Config.json";
        public string dna_MainSystemConfigPath { get; set; }
        public string dna_LootContainersSystemConfigPath { get; set; }
        public string dna_MobSystemConfigPath { get; set; }
        public string dna_LootConfigPath { get; set; }
        public string dna_ClothingConfigPath { get; set; }
        public string dna_WeaponConfigPath { get; set; }
        public string SmolCrateConfigDescription { get; set; }
        public string SmolCrateYellowConfig { get; set; }
        public string SmolCrateGreenConfig { get; set; }
        public string SmolCrateBlueConfig { get; set; }
        public string SmolCratePurpleConfig { get; set; }
        public string SmolCrateRedConfig { get; set; }

        public string SecondaryConfigRoot { get; set; }
        public string DNATimerSettings { get; set; }
        public string SmolCrateConfig { get; set; }
        public string NewAlarmConfig { get; set; }

        public KeyCard_Main_System_Config KeyCard_Main_System_Config { get; set; }
        public KeyCard_Mob_System_Config KeyCard_Mob_System_Config { get; set; }
        public KeyCard_LootContainers_System_Config KeyCard_LootContainers_System_Config { get; set; }
        public KeyCard_Clothing_Config KeyCard_Clothing_Config { get; set; }
        public KeyCard_General_Config KeyCard_General_Config { get; set; }
        public KeyCard_Weapons_Config KeyCard_Weapons_Config { get; set; }
        public DNA_CrateSmol_Data DNA_CrateSmol_Data { get; set; }
        public DNA_ResetTimer_Settings DNA_ResetTimer_Settings { get; set; }

        public DNA_Keycards(string ProfilePath)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Converters = { new BoolConverter() }
            };

            dna_ProfilePath = ProfilePath;
            //System
            //Main
            dna_MainSystemConfigPath = dna_ProfilePath + dna_ConfigFolderName + "/System/Main/" + dna_ConfigName + "Main_System" + dna_ConfigExtension;
            if(File.Exists(dna_MainSystemConfigPath))
                KeyCard_Main_System_Config = JsonSerializer.Deserialize<KeyCard_Main_System_Config>(File.ReadAllText(dna_MainSystemConfigPath));
            else
            {
                KeyCard_Main_System_Config = new KeyCard_Main_System_Config();
                KeyCard_Main_System_Config.CreateDefaultConfig();
            }
            KeyCard_Main_System_Config.Filename = dna_MainSystemConfigPath;
            //Lootcontainers
            dna_LootContainersSystemConfigPath = dna_ProfilePath + dna_ConfigFolderName + "/System/LootContainers/" + dna_ConfigName + "LootContainers_System" + dna_ConfigExtension;
            if(File.Exists(dna_LootContainersSystemConfigPath))
                KeyCard_LootContainers_System_Config = JsonSerializer.Deserialize<KeyCard_LootContainers_System_Config>(File.ReadAllText(dna_LootContainersSystemConfigPath));
            else
            {
                KeyCard_LootContainers_System_Config = new KeyCard_LootContainers_System_Config();
                KeyCard_LootContainers_System_Config.CreateDefaultConfig();
            }
            KeyCard_LootContainers_System_Config.Filename = dna_LootContainersSystemConfigPath;
            //mobs
            dna_MobSystemConfigPath = dna_ProfilePath + dna_ConfigFolderName + "/System/Mobs/" + dna_ConfigName + "Mob_System" + dna_ConfigExtension;
            if (File.Exists(dna_MobSystemConfigPath))
                KeyCard_Mob_System_Config = JsonSerializer.Deserialize<KeyCard_Mob_System_Config>(File.ReadAllText(dna_MobSystemConfigPath));
            else
            {
                KeyCard_Mob_System_Config = new KeyCard_Mob_System_Config();
                KeyCard_Mob_System_Config.CreateDefaultConfig();
            }
            KeyCard_Mob_System_Config.Filename = dna_MobSystemConfigPath;
            //Others
            SecondaryConfigRoot = dna_ProfilePath + dna_ConfigFolderName + "/System/Other/";
            DNATimerSettings = SecondaryConfigRoot + "ResetTimer" + dna_ConfigExtension;
            if (File.Exists(DNATimerSettings))
            {
                DNA_ResetTimer_Settings = JsonSerializer.Deserialize<DNA_ResetTimer_Settings>(File.ReadAllText(DNATimerSettings), options);
            }
            else
            {
                DNA_ResetTimer_Settings = new DNA_ResetTimer_Settings();
                DNA_ResetTimer_Settings.CreateDefaultSettings();
            }
            DNA_ResetTimer_Settings.Filename = DNATimerSettings;
            //the following are loaded in the DNA_CrateSmol_Data class
            SmolCrateConfig = SecondaryConfigRoot + "SmolCrates" + dna_ConfigExtension;
            NewAlarmConfig = SecondaryConfigRoot + "DoorAlarmAndNotifications" + dna_ConfigExtension;

            //Loot
            //general
            dna_LootConfigPath = dna_ProfilePath + dna_ConfigFolderName + "/Loot/General/" + dna_ConfigName + "General" + dna_ConfigExtension;
            if(File.Exists(dna_LootConfigPath))
                KeyCard_General_Config = JsonSerializer.Deserialize<KeyCard_General_Config>(File.ReadAllText(dna_LootConfigPath));
            else
            {
                KeyCard_General_Config = new KeyCard_General_Config();
                KeyCard_General_Config.CreateDefaultConfig();
            }
            KeyCard_General_Config.Filename = dna_LootConfigPath;
            //Clothes
            dna_ClothingConfigPath = dna_ProfilePath + dna_ConfigFolderName + "/Loot/Clothing/" + dna_ConfigName + "Clothing" + dna_ConfigExtension;
            if(File.Exists(dna_ClothingConfigPath))
                KeyCard_Clothing_Config = JsonSerializer.Deserialize<KeyCard_Clothing_Config>(File.ReadAllText(dna_ClothingConfigPath));
            else
            {
                KeyCard_Clothing_Config = new KeyCard_Clothing_Config();
                KeyCard_Clothing_Config.CreateDefaultConfig();
            }
            KeyCard_Clothing_Config.Filename = dna_ClothingConfigPath;
            //Weapons
            dna_WeaponConfigPath = dna_ProfilePath + dna_ConfigFolderName + "/Loot/Weapons/" + dna_ConfigName + "Weapons" + dna_ConfigExtension;
            if (File.Exists(dna_WeaponConfigPath))
                KeyCard_Weapons_Config = JsonSerializer.Deserialize<KeyCard_Weapons_Config>(File.ReadAllText(dna_WeaponConfigPath));
            else
            {
                KeyCard_Weapons_Config = new KeyCard_Weapons_Config();
                KeyCard_Weapons_Config.CreateDefaultConfig();
            }
            KeyCard_Weapons_Config.Filename = dna_WeaponConfigPath;
            //Other
            SmolCrateConfigDescription = dna_ProfilePath + dna_ConfigFolderName + "/Loot/Other/@Config_Description_and_Activation.json";
            SmolCrateYellowConfig = dna_ProfilePath + dna_ConfigFolderName + "/Loot/Other/Smol_Yellow" + dna_ConfigExtension;
            SmolCrateGreenConfig = dna_ProfilePath + dna_ConfigFolderName + "/Loot/Other/Smol_Green" + dna_ConfigExtension;
            SmolCrateBlueConfig = dna_ProfilePath + dna_ConfigFolderName + "/Loot/Other/Smol_Blue" + dna_ConfigExtension;
            SmolCratePurpleConfig = dna_ProfilePath + dna_ConfigFolderName + "/Loot/Other/Smol_Purple" + dna_ConfigExtension;
            SmolCrateRedConfig = dna_ProfilePath + dna_ConfigFolderName + "/Loot/Other/Smol_Red" + dna_ConfigExtension;
            DNA_CrateSmol_Data = new DNA_CrateSmol_Data();
            if(File.Exists(SmolCrateConfigDescription))
                DNA_CrateSmol_Data.dna_Description = JsonSerializer.Deserialize<CrateSmol_Settings_Description>(File.ReadAllText(SmolCrateConfigDescription), options);
            else
            {
                DNA_CrateSmol_Data.dna_Description = new CrateSmol_Settings_Description();
                DNA_CrateSmol_Data.dna_Description.CreateDefaultConfig();
            }
            DNA_CrateSmol_Data.dna_Description.Filename = SmolCrateConfigDescription;
            if(File.Exists(SmolCrateYellowConfig))
                DNA_CrateSmol_Data.dna_YellowSettings = JsonSerializer.Deserialize<CrateSmol_Yellow_Settings>(File.ReadAllText(SmolCrateYellowConfig), options);
            else
            {
                DNA_CrateSmol_Data.dna_YellowSettings = new CrateSmol_Yellow_Settings();
                DNA_CrateSmol_Data.dna_YellowSettings.CreateDefaultYellow();
            }
            DNA_CrateSmol_Data.dna_YellowSettings.Filename = SmolCrateYellowConfig;
            if(File.Exists(SmolCrateGreenConfig))
                DNA_CrateSmol_Data.dna_GreenSettings = JsonSerializer.Deserialize<CrateSmol_Green_Settings>(File.ReadAllText(SmolCrateGreenConfig), options);
            else
            {
                DNA_CrateSmol_Data.dna_GreenSettings = new CrateSmol_Green_Settings();
                DNA_CrateSmol_Data.dna_GreenSettings.CreateDefaultGreen();
            }
            DNA_CrateSmol_Data.dna_GreenSettings.Filename = SmolCrateGreenConfig;
            if(File.Exists(SmolCrateBlueConfig))
                DNA_CrateSmol_Data.dna_BlueSettings = JsonSerializer.Deserialize<CrateSmol_Blue_Settings>(File.ReadAllText(SmolCrateBlueConfig), options);
            else
            {
                DNA_CrateSmol_Data.dna_BlueSettings = new CrateSmol_Blue_Settings();
                    DNA_CrateSmol_Data.dna_BlueSettings.CreateDefaultBlue();
            }
            DNA_CrateSmol_Data.dna_BlueSettings.Filename = SmolCrateBlueConfig;
            if(File.Exists(SmolCratePurpleConfig))
                DNA_CrateSmol_Data.dna_PurpleSettings = JsonSerializer.Deserialize<CrateSmol_Purple_Settings>(File.ReadAllText(SmolCratePurpleConfig), options);
            else
            {
                DNA_CrateSmol_Data.dna_PurpleSettings = new CrateSmol_Purple_Settings();
                DNA_CrateSmol_Data.dna_PurpleSettings.CreateDefaultPurple();
            }
            DNA_CrateSmol_Data.dna_PurpleSettings.Filename = SmolCratePurpleConfig;
            if(File.Exists(SmolCrateRedConfig))
                DNA_CrateSmol_Data.dna_RedSettings = JsonSerializer.Deserialize<CrateSmol_Red_Settings>(File.ReadAllText(SmolCrateRedConfig), options);
            else
            {
                DNA_CrateSmol_Data.dna_RedSettings = new CrateSmol_Red_Settings();
                DNA_CrateSmol_Data.dna_RedSettings.CreateDefaultRed();
            }
            DNA_CrateSmol_Data.dna_RedSettings.Filename = SmolCrateRedConfig;
            if(File.Exists(SmolCrateConfig))
                DNA_CrateSmol_Data.dna_TimerSettings = JsonSerializer.Deserialize<CrateSmol_TimerSettings>(File.ReadAllText(SmolCrateConfig), options);
            else
            {
                DNA_CrateSmol_Data.dna_TimerSettings = new CrateSmol_TimerSettings();
                DNA_CrateSmol_Data.dna_TimerSettings.CreateDefaultConfig();
            }
            DNA_CrateSmol_Data.dna_TimerSettings.Filename = SmolCrateConfig;
            if (File.Exists(NewAlarmConfig))
                DNA_CrateSmol_Data.dna_AlarmSettings = JsonSerializer.Deserialize<NewDoors_AlarmsAndNotifications>(File.ReadAllText(NewAlarmConfig), options);
            else
            {
                DNA_CrateSmol_Data.dna_AlarmSettings = new NewDoors_AlarmsAndNotifications();
                DNA_CrateSmol_Data.dna_AlarmSettings.CreateDefaultAlarmSettings();
            }
            DNA_CrateSmol_Data.dna_AlarmSettings.Filename = NewAlarmConfig;
        }
    }
}
