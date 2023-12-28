using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class KeyCard_Clothing_Config
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public BindingList<DNA_Config_Clothing> m_DNAConfig_Clothing { get; set; }

        public KeyCard_Clothing_Config()
        {
            m_DNAConfig_Clothing = new BindingList<DNA_Config_Clothing>();
            CreateDefaultConfig();
        }

        public void CreateDefaultConfig()   //("tier", "helmet", "shirt", "vest", "pants", "shoes", "backpack", "gloves", "belt", "face", "eyes", "armband", "nvg")
        {
            m_DNAConfig_Clothing.Add(new DNA_Config_Clothing("yellow", "BaseballCap_Blue", "Shirt_BlueCheck", "PressVest_Blue", "Jeans_Blue", "AthleticShoes_Blue", "ImprovisedBag", "SurgicalGloves_LightBlue", "", "SurgicalMask", "", "", ""));
            m_DNAConfig_Clothing.Add(new DNA_Config_Clothing("yellow", "BaseballCap_Red", "TShirt_Red", "PoliceVest", "Jeans_Black", "JoggingShoes_Red", "CourierBag", "LeatherGloves_Brown", "", "", "", "Armband_White", ""));
            m_DNAConfig_Clothing.Add(new DNA_Config_Clothing("green", "MotoHelmet_Black", "RidersJacket_Black", "LeatherStorageVest_Black", "CargoPants_Black", "HikingBoots_Black", "HuntingBag", "WorkingGloves_Black", "", "", "SportGlasses_Black", "", ""));
            m_DNAConfig_Clothing.Add(new DNA_Config_Clothing("green", "SkateHelmet_Green", "HikingJacket_Green", "HuntingVest", "CargoPants_Green", "WorkingBoots_Green", "HuntingBag", "WorkingGloves_Black", "", "", "SportGlasses_Green", "", ""));
            m_DNAConfig_Clothing.Add(new DNA_Config_Clothing("blue", "GorkaHelmet_Green", "M65Jacket_Olive", "UKAssVest_Olive", "USMCPants_Woodland", "CombatBoots_Green", "AssaultBag_Ttsko", "TacticalGloves_Green", "CivilianBelt", "", "SportGlasses_Green", "", ""));
            m_DNAConfig_Clothing.Add(new DNA_Config_Clothing("blue", "GorkaHelmet_Black", "M65Jacket_Black", "UKAssVest_Black", "USMCPants_Woodland", "CombatBoots_Black", "AssaultBag_Black", "TacticalGloves_Black", "CivilianBelt", "", "SportGlasses_Black", "", ""));
            m_DNAConfig_Clothing.Add(new DNA_Config_Clothing("purple", "BallisticHelmet_Green", "TTsKOJacket_Camo", "UKAssVest_Camo", "TTSKOPants", "MilitaryBoots_Beige", "CoyoteBag_Green", "TacticalGloves_Green", "MilitaryBelt", "GasMask", "", "", ""));
            m_DNAConfig_Clothing.Add(new DNA_Config_Clothing("purple", "BallisticHelmet_Black", "M65Jacket_Black", "UKAssVest_Black", "TTSKOPants", "MilitaryBoots_Black", "CoyoteBag_Brown", "TacticalGloves_Beige", "MilitaryBelt", "GasMask", "", "", ""));
            m_DNAConfig_Clothing.Add(new DNA_Config_Clothing("red", "Mich2001Helmet", "GorkaEJacket_Autumn", "HighCapacityVest_Olive", "GorkaPants_Autumn", "JungleBoots_Green", "AliceBag_Green", "TacticalGloves_Green", "MilitaryBelt", "GP5GasMask", "GasMask_Filter", "", "NVGoggles"));
            m_DNAConfig_Clothing.Add(new DNA_Config_Clothing("red", "Mich2001Helmet", "M65Jacket_Black", "HighCapacityVest_Black", "GorkaPants_Autumn", "JungleBoots_Black", "AliceBag_Black", "TacticalGloves_Black", "MilitaryBelt", "GP5GasMask", "GasMask_Filter", "", "NVGoggles"));
        }
    }

    public class DNA_Config_Clothing
    {
        public string dna_Tier { get; set; }
        public string dna_Helm { get; set; }
        public string dna_Shirt { get; set; }
        public string dna_Vest { get; set; }
        public string dna_Pants { get; set; }
        public string dna_Shoes { get; set; }
        public string dna_Backpack { get; set; }
        public string dna_Gloves { get; set; }
        public string dna_Belt { get; set; }
        public string dna_Facewear { get; set; }
        public string dna_Eyewear { get; set; }
        public string dna_Armband { get; set; }
        public string dna_NVG { get; set; }

        public DNA_Config_Clothing() { }
        public DNA_Config_Clothing(string tier, string helm, string shirt, string vest, string pants, string shoes, string backpack, string gloves, string belt, string facewear, string eyewear, string armband, string nvg)
        {
            dna_Tier = tier;
            dna_Helm = helm;
            dna_Shirt = shirt;
            dna_Vest = vest;
            dna_Pants = pants;
            dna_Shoes = shoes;
            dna_Backpack = backpack;
            dna_Gloves = gloves;
            dna_Belt = belt;
            dna_Facewear = facewear;
            dna_Eyewear = eyewear;
            dna_Armband = armband;
            dna_NVG = nvg;
        }
        
    }

}
