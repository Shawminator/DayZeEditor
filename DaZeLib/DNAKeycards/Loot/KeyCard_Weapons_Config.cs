using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class KeyCard_Weapons_Config
    {
        public BindingList<DNA_Config_Weapons> m_DNAConfig_Weapons { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public KeyCard_Weapons_Config()
        {
            m_DNAConfig_Weapons = new BindingList<DNA_Config_Weapons>();
            CreateDefaultConfig();
        }

        public void CreateDefaultConfig()
        {
            m_DNAConfig_Weapons.Add(new DNA_Config_Weapons("yellow", "main", "Mp133Shotgun", "", "AmmoBox_00buck_10rnd", "", "", "", "", "", ""));
            m_DNAConfig_Weapons.Add(new DNA_Config_Weapons("yellow", "main", "Izh43Shotgun", "", "AmmoBox_00buck_10rnd", "", "", "", "", "", ""));
            m_DNAConfig_Weapons.Add(new DNA_Config_Weapons("yellow", "main", "SawedoffIzh43Shotgun", "", "AmmoBox_00buck_10rnd", "", "", "", "", "", ""));
            m_DNAConfig_Weapons.Add(new DNA_Config_Weapons("yellow", "side", "Magnum", "", "Ammo_357", "", "", "", "", "", ""));
            m_DNAConfig_Weapons.Add(new DNA_Config_Weapons("yellow", "side", "MKII", "Mag_MKII_10Rnd", "Ammo_22", "", "", "", "", "", ""));

            m_DNAConfig_Weapons.Add(new DNA_Config_Weapons("green", "main", "Izh18", "", "AmmoBox_762x39_20Rnd", "", "", "", "", "", ""));
            m_DNAConfig_Weapons.Add(new DNA_Config_Weapons("green", "main", "Mosin9130_Camo", "", "AmmoBox_762x54_20Rnd", "PUScopeOptic", "Mosin_Compensator", "", "", "", ""));
            m_DNAConfig_Weapons.Add(new DNA_Config_Weapons("green", "main", "SKS_Black", "", "AmmoBox_762x39_20Rnd", "PUScopeOptic", "SKS_Bayonet", "", "", "", ""));
            m_DNAConfig_Weapons.Add(new DNA_Config_Weapons("green", "side", "Colt1911", "Mag_1911_7Rnd", "Ammo_45ACP", "", "", "", "", "", ""));

            m_DNAConfig_Weapons.Add(new DNA_Config_Weapons("blue", "main", "MP5K", "Mag_MP5_30Rnd", "AmmoBox_9x19_25rnd", "ReflexOptic", "PistolSuppressor", "UniversalLight", "MP5k_StockBttstck", "MP5_RailHndgrd", ""));
            m_DNAConfig_Weapons.Add(new DNA_Config_Weapons("blue", "main", "AKS74U_Black", "Mag_AK74_30Rnd_Black", "AmmoBox_545x39_20Rnd", "", "AK_Suppressor", "", "AKS74U_Bttstck_Black", "", ""));
            m_DNAConfig_Weapons.Add(new DNA_Config_Weapons("blue", "side", "Engraved1911", "Mag_1911_7Rnd", "Ammo_45ACP", "", "", "", "", "", ""));

            m_DNAConfig_Weapons.Add(new DNA_Config_Weapons("purple", "main", "FAL", "Mag_FAL_20Rnd", "AmmoBox_308WIN_20Rnd", "ReflexOptic", "", "", "Fal_FoldingBttstck", "", ""));
            m_DNAConfig_Weapons.Add(new DNA_Config_Weapons("purple", "main", "M4A1_Black", "Mag_STANAG_30Rnd", "AmmoBox_556x45_20Rnd", "ACOGOptic", "M4_Suppressor", "UniversalLight", "M4_MPBttstck", "M4_RISHndgrd_Black", ""));
            m_DNAConfig_Weapons.Add(new DNA_Config_Weapons("purple", "side", "Glock19", "Mag_Glock_15Rnd", "Ammo_9x19", "FNP45_MRDSOptic", "PistolSuppressor", "TLRLight", "", "", ""));

            m_DNAConfig_Weapons.Add(new DNA_Config_Weapons("red", "main", "SVD", "Mag_SVD_10Rnd", "AmmoBox_762x54_20Rnd", "PSO1Optic", "AK_Suppressor", "", "", "", ""));
            m_DNAConfig_Weapons.Add(new DNA_Config_Weapons("red", "main", "Winchester70", "", "AmmoBox_308WIN_20Rnd", "HuntingOptic", "", "", "", "", ""));
            m_DNAConfig_Weapons.Add(new DNA_Config_Weapons("red", "side", "FNX45", "Mag_FNX45_15Rnd", "Ammo_45ACP", "FNP45_MRDSOptic", "PistolSuppressor", "TLRLight", "", "", ""));
        }
    }

    public class DNA_Config_Weapons
    {
        public string dna_Tier { get; set; }
        public string dna_WeaponCategory { get; set; }
        public string dna_TheChosenOne { get; set; }
        public string dna_Magazine { get; set; }
        public string dna_Ammunition { get; set; }
        public string dna_OpticType { get; set; }
        public string dna_Suppressor { get; set; }
        public string dna_UnderBarrel { get; set; }
        public string dna_ButtStock { get; set; }
        public string dna_HandGuard { get; set; }
        public string dna_Wrap { get; set; }
        public DNA_Config_Weapons() { }
        public DNA_Config_Weapons(string tier, string weaponCategory, string theChosenOne, string magazine, string ammunition, string opticType, string suppressor, string underBarrel, string buttStock, string handGuard, string wrap)
        {
            dna_Tier = tier;
            dna_WeaponCategory = weaponCategory;
            dna_TheChosenOne = theChosenOne;
            dna_Magazine = magazine;
            dna_Ammunition = ammunition;
            dna_OpticType = opticType;
            dna_Suppressor = suppressor;
            dna_UnderBarrel = underBarrel;
            dna_ButtStock = buttStock;
            dna_HandGuard = handGuard;
            dna_Wrap = wrap;
        }
    }

}
