using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
	public class DNA_CrateSmol_Data
	{
		public CrateSmol_Settings_Description dna_Description { get; set; }
		public CrateSmol_Yellow_Settings dna_YellowSettings { get; set; }
		public CrateSmol_Green_Settings dna_GreenSettings { get; set; }
		public CrateSmol_Blue_Settings dna_BlueSettings { get; set; }
		public CrateSmol_Purple_Settings dna_PurpleSettings { get; set; }
		public CrateSmol_Red_Settings dna_RedSettings { get; set; }
		public CrateSmol_TimerSettings dna_TimerSettings { get; set; }
		public NewDoors_AlarmsAndNotifications dna_AlarmSettings { get; set; }
	}
	
	public class CrateSmol_Settings_Description
	{
		[JsonIgnore]
		public string Filename { get; set; }
		[JsonIgnore]
		public bool isDirty { get; set; }

		public string dna_ConfigDescription { get; set; }
		public string dna_ConfigDescriptionCont { get; set; }
		public string dna_ConfigDescriptionCont2 { get; set; }
		public bool dna_UseConfig { get; set; }

		public CrateSmol_Settings_Description()
        {
			
		}
		public void CreateDefaultConfig()
        {
			dna_ConfigDescription = "This section of configs is used to fill smol crates and other crates we may add in the future that you place with editor";
			dna_ConfigDescriptionCont = "(or whatever you choose to use). If you are already using the smol crates and loading them another way, you can leave";
			dna_ConfigDescriptionCont2 = "the setting below to false (0) and it will not interfere with your current setup. Set below to true (1) to use configs.";
			dna_UseConfig = false;
		}
	}

	public class CrateSmol_Settings_Base
	{
		[JsonIgnore]
		public string Filename { get; set; }
		[JsonIgnore]
		public bool isDirty { get; set; }

		public string dna_Description { get; set; }
		public string dna_DescriptionCont { get; set; }
		public string dna_DescriptionCont2 { get; set; }
		public bool dna_SpawnMainWeapons { get; set; }
		public bool dna_RandomizeWeapons { get; set; }
		public int dna_WeaponCount { get; set; }
		public bool dna_SpawnLootSet1 { get; set; }
		public bool dna_RandomizeLootSet1 { get; set; }
		public int dna_LootSet1Count { get; set; }
		public bool dna_SpawnLootSet2 { get; set; }
		public bool dna_RandomizeLootSet2 { get; set; }
		public int dna_LootSet2Count { get; set; }
		public bool dna_SpawnLootSet3 { get; set; }
		public bool dna_RandomizeLootSet3 { get; set; }
		public int dna_LootSet3Count { get; set; }
		public bool dna_SpawnLootSet4 { get; set; }
		public bool dna_RandomizeLootSet4 { get; set; }
		public int dna_LootSet4Count { get; set; }

	}

	public class CrateSmol_Yellow_Settings : CrateSmol_Settings_Base
	{
		public BindingList<DNA_Config_Weapons_Smol> weaponTypesYellow { get; set; }
		public BindingList<string> dna_LootSetY1 { get; set; }
		public BindingList<string> dna_LootSetY2 { get; set; }
		public BindingList<string> dna_LootSetY3 { get; set; }
		public BindingList<string> dna_LootSetY4 { get; set; }

		public CrateSmol_Yellow_Settings()
		{
			weaponTypesYellow = new BindingList<DNA_Config_Weapons_Smol>();
			dna_LootSetY1 = new BindingList<string>();
			dna_LootSetY2 = new BindingList<string>();
			dna_LootSetY3 = new BindingList<string>();
			dna_LootSetY4 = new BindingList<string>();
		}
		public void CreateDefaultYellow()
        {
			dna_Description = "DNA SMOL CRATE CONFIG (Yellow Tier). Below are settings to spawn either static or randomized loot. To spawn all items in array, use randomize setting 0,";
			dna_DescriptionCont = "if set to 1, it will use the count setting found just below the randomize setting. When using randomized loot, the system can and likely WILL";
			dna_DescriptionCont2 = "spawn duplicates. I suggest using static and manually adding desired duplicates. Each loot set, including weapons can be turned off as well.";
			dna_SpawnMainWeapons = true;
			dna_RandomizeWeapons = true;
			dna_WeaponCount = 1;
			weaponTypesYellow.Add(new DNA_Config_Weapons_Smol("Mp133Shotgun", "", 0, "AmmoBox_00buck_10rnd", 1, "", "", "", "", "", ""));
			weaponTypesYellow.Add(new DNA_Config_Weapons_Smol("Izh43Shotgun", "", 0, "AmmoBox_00buck_10rnd", 1, "", "", "", "", "", ""));
			dna_SpawnLootSet1 = true;
			dna_RandomizeLootSet1 = true;
			dna_LootSet1Count = 3;
			dna_LootSetY1 = new BindingList<string>(){ "BloodTestKit","TetracyclineAntibiotics","BandageDressing","BloodBagIV","Morphine","Epinephrine"};
			dna_SpawnLootSet2 = true;
			dna_RandomizeLootSet2 = true;
			dna_LootSet2Count = 3;
			dna_LootSetY2 = new BindingList<string>() { "BoxCerealCrunchin","PeachesCan_Opened","TacticalBaconCan","BakedBeansCan","BakedBeansCan_Opened"};
			dna_SpawnLootSet3 = true;
			dna_RandomizeLootSet3 = true;
			dna_LootSet3Count = 3;
			dna_LootSetY3 = new BindingList<string>() { "SodaCan_Pipsi","WaterBottle","SodaCan_Fronta","SodaCan_Cola","SodaCan_Spite"};
			dna_SpawnLootSet4 = true;
			dna_RandomizeLootSet4 = false;
			dna_LootSet4Count = 3;
			dna_LootSetY4 = new BindingList<string>() { "WoodenPlank","NailBox","Shovel","WoodAxe","Screwdriver"};
		}
    }

	public class CrateSmol_Green_Settings : CrateSmol_Settings_Base
	{
		public BindingList<DNA_Config_Weapons_Smol> weaponTypesGreen { get; set; }
		public BindingList<string> dna_LootSetG1 { get; set; }
		public BindingList<string> dna_LootSetG2 { get; set; }
		public BindingList<string> dna_LootSetG3 { get; set; }
		public BindingList<string> dna_LootSetG4 { get; set; }

		public CrateSmol_Green_Settings()
		{
			weaponTypesGreen = new BindingList<DNA_Config_Weapons_Smol>();
			dna_LootSetG1 = new BindingList<string>();
			dna_LootSetG2 = new BindingList<string>();
			dna_LootSetG3 = new BindingList<string>();
			dna_LootSetG4 = new BindingList<string>();
		}
		public void CreateDefaultGreen()
        {
			dna_Description = "DNA SMOL CRATE CONFIG (Green Tier). Below are settings to spawn either static or randomized loot. To spawn all items in array, use randomize setting 0,";
			dna_DescriptionCont = "if set to 1, it will use the count setting found just below the randomize setting. When using randomized loot, the system can and likely WILL";
			dna_DescriptionCont2 = "spawn duplicates. I suggest using static and manually adding desired duplicates. Each loot set, including weapons can be turned off as well.";
			dna_SpawnMainWeapons = true;
			dna_RandomizeWeapons = true;
			dna_WeaponCount = 1;
			weaponTypesGreen.Add(new DNA_Config_Weapons_Smol("Izh18", "", 0, "AmmoBox_762x39_20Rnd", 1, "", "", "", "", "", ""));
			weaponTypesGreen.Add(new DNA_Config_Weapons_Smol("SKS_Black", "", 0, "AmmoBox_762x39_20Rnd", 1, "PUScopeOptic", "SKS_Bayonet", "", "", "", ""));
			dna_SpawnLootSet1 = true;
			dna_RandomizeLootSet1 = true;
			dna_LootSet1Count = 3;
			dna_LootSetG1 = new BindingList<string>() { "BloodTestKit","TetracyclineAntibiotics","BandageDressing","BloodBagIV","Morphine","Epinephrine"};
			dna_SpawnLootSet2 = true;
			dna_RandomizeLootSet2 = true;
			dna_LootSet2Count = 3;
			dna_LootSetG2 = new BindingList<string>() { "BoxCerealCrunchin","PeachesCan_Opened","TacticalBaconCan","BakedBeansCan","BakedBeansCan_Opened"};
			dna_SpawnLootSet3 = true;
			dna_RandomizeLootSet3 = true;
			dna_LootSet3Count = 3;
			dna_LootSetG3 = new BindingList<string>() { "SodaCan_Pipsi","WaterBottle","SodaCan_Fronta","SodaCan_Cola","SodaCan_Spite"};
			dna_SpawnLootSet4 = true;
			dna_RandomizeLootSet4 = false;
			dna_LootSet4Count = 3;
			dna_LootSetG4 = new BindingList<string>() { "WoodenPlank","NailBox","Shovel","WoodAxe","Screwdriver"};
		}
    }

	public class CrateSmol_Blue_Settings : CrateSmol_Settings_Base
	{
		public BindingList<DNA_Config_Weapons_Smol> weaponTypesBlue { get; set; }
		public BindingList<string> dna_LootSetB1 { get; set; }
		public BindingList<string> dna_LootSetB2 { get; set; }
		public BindingList<string> dna_LootSetB3 { get; set; }
		public BindingList<string> dna_LootSetB4 { get; set; }

		public CrateSmol_Blue_Settings()
		{
			
		}

        public void CreateDefaultBlue()
        {
           
        }
    }

	public class CrateSmol_Purple_Settings : CrateSmol_Settings_Base
	{
		public BindingList<DNA_Config_Weapons_Smol> weaponTypesPurple { get; set; }
		public BindingList<string> dna_LootSetP1 { get; set; }
		public BindingList<string> dna_LootSetP2 { get; set; }
		public BindingList<string> dna_LootSetP3 { get; set; }
		public BindingList<string> dna_LootSetP4 { get; set; }

		public CrateSmol_Purple_Settings()
		{
			
		}

        public void CreateDefaultPurple()
        {
            
        }
    }

	public class CrateSmol_Red_Settings : CrateSmol_Settings_Base
	{
		public BindingList<DNA_Config_Weapons_Smol> weaponTypesRed { get; set; }
		public BindingList<string> dna_LootSetR1 { get; set; }
		public BindingList<string> dna_LootSetR2 { get; set; }
		public BindingList<string> dna_LootSetR3 { get; set; }
		public BindingList<string> dna_LootSetR4 { get; set; }

		public CrateSmol_Red_Settings()
		{
			weaponTypesRed = new BindingList<DNA_Config_Weapons_Smol>();
			dna_LootSetR1 = new BindingList<string>();
			dna_LootSetR2 = new BindingList<string>();
			dna_LootSetR3 = new BindingList<string>();
			dna_LootSetR4 = new BindingList<string>();
		}
		public void CreateDefaultRed()
		{
			dna_Description = "DNA SMOL CRATE CONFIG (Red Tier). Below are settings to spawn either static or randomized loot. To spawn all items in array, use randomize setting 0,";
			dna_DescriptionCont = "if set to 1, it will use the count setting found just below the randomize setting. When using randomized loot, the system can and likely WILL";
			dna_DescriptionCont2 = "spawn duplicates. I suggest using static and manually adding desired duplicates. Each loot set, including weapons can be turned off as well.";
			dna_SpawnMainWeapons = true;
			dna_RandomizeWeapons = true;
			dna_WeaponCount = 1;
			weaponTypesRed = new BindingList<DNA_Config_Weapons_Smol>()
			{
				new DNA_Config_Weapons_Smol("SVD", "Mag_SVD_10Rnd", 1, "AmmoBox_762x54_20Rnd", 1, "PSO1Optic", "AK_Suppressor", "", "", "", ""),
				new DNA_Config_Weapons_Smol("Winchester70", "", 0, "AmmoBox_308WIN_20Rnd", 1, "HuntingOptic", "", "", "", "", "")
			};
			dna_SpawnLootSet1 = true;
			dna_RandomizeLootSet1 = true;
			dna_LootSet1Count = 3;
			dna_LootSetR1 = new BindingList<string>() { "BloodTestKit", "TetracyclineAntibiotics", "BandageDressing", "BloodBagIV", "Morphine", "Epinephrine" };
			dna_SpawnLootSet2 = true;
			dna_RandomizeLootSet2 = true;
			dna_LootSet2Count = 3;
			dna_LootSetR2 = new BindingList<string>()  { "BoxCerealCrunchin","PeachesCan_Opened","TacticalBaconCan","BakedBeansCan","BakedBeansCan_Opened"};
			dna_SpawnLootSet3 = true;
			dna_RandomizeLootSet3 = true;
			dna_LootSet3Count = 3;
			dna_LootSetR3 = new BindingList<string>()  { "SodaCan_Pipsi","WaterBottle","SodaCan_Fronta","SodaCan_Cola","SodaCan_Spite"};
			dna_SpawnLootSet4 = true;
			dna_RandomizeLootSet4 = false;
			dna_LootSet4Count = 3;
			dna_LootSetR4 = new BindingList<string>()  { "WoodenPlank","NailBox","Shovel","WoodAxe","Screwdriver"};
		}
	}
	public class DNA_Config_Weapons_Smol
	{
		[JsonIgnore]
		public string Filename { get; set; }
		[JsonIgnore]
		public bool isDirty { get; set; }

		public string dna_TheChosenOne{get;set;}
		public string dna_Magazine{get;set;}
		public int dna_SpareMagCount{get;set;}
		public string dna_Ammunition{get;set;}
		public int dna_SpareAmmoCount{get;set;}
		public string dna_OpticType{get;set;}
		public string dna_Suppressor{get;set;}
		public string dna_UnderBarrel{get;set;}
		public string dna_ButtStock{get;set;}
		public string dna_HandGuard{get;set;}
		public string dna_Wrap{get;set;}

		public DNA_Config_Weapons_Smol() { }
		public DNA_Config_Weapons_Smol(string theChosenOne, string magazine, int spareMagCount, string ammunition, int spareAmmoCount, string opticType, string suppressor, string underBarrel, string buttStock, string handGuard, string wrap)
		{
			dna_TheChosenOne = theChosenOne;
			dna_Magazine = magazine;
			dna_SpareMagCount = spareMagCount;
			dna_Ammunition = ammunition;
			dna_SpareAmmoCount = spareAmmoCount;
			dna_OpticType = opticType;
			dna_Suppressor = suppressor;
			dna_UnderBarrel = underBarrel;
			dna_ButtStock = buttStock;
			dna_HandGuard = handGuard;
			dna_Wrap = wrap;
		}
	}
}
