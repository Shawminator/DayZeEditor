using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class KeyCard_General_Config
    {
		[JsonIgnore]
		public string Filename { get; set; }
		[JsonIgnore]
		public bool isDirty { get; set; }

		public BindingList<DNA_Config_Loot> m_DNAConfig_Loot { get; set; }

		public KeyCard_General_Config()
        {
			m_DNAConfig_Loot = new BindingList<DNA_Config_Loot>();
			CreateDefaultConfig();
        }

		public void CreateDefaultConfig()
		{
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("yellow", "proprietary", ""));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("yellow", "medical", "BloodTestKit"));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("yellow", "food", "BoxCerealCrunchin"));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("yellow", "drink", "SodaCan_Spite"));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("yellow", "tools", "Pickaxe"));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("yellow", "material", "WoodenPlank"));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("yellow", "valuable", ""));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("yellow", "misc", ""));

			m_DNAConfig_Loot.Add(new DNA_Config_Loot("green", "proprietary", ""));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("green", "medical", "BloodBagIV"));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("green", "food", "PeachesCan_Opened"));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("green", "drink", "SodaCan_Cola"));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("green", "tools", "Screwdriver"));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("green", "material", "NailBox"));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("green", "valuable", ""));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("green", "misc", ""));

			m_DNAConfig_Loot.Add(new DNA_Config_Loot("blue", "proprietary", ""));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("blue", "medical", "TetracyclineAntibiotics"));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("blue", "food", "TacticalBaconCan"));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("blue", "drink", "SodaCan_Fronta"));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("blue", "tools", "Shovel"));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("blue", "material", "FenceKit"));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("blue", "valuable", ""));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("blue", "misc", ""));

			m_DNAConfig_Loot.Add(new DNA_Config_Loot("purple", "proprietary", ""));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("purple", "medical", "BandageDressing"));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("purple", "food", "BakedBeansCan"));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("purple", "drink", "WaterBottle"));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("purple", "tools", "WoodAxe"));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("purple", "material", "TerritoryFlagKit"));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("purple", "valuable", ""));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("purple", "misc", ""));

			m_DNAConfig_Loot.Add(new DNA_Config_Loot("red", "proprietary", ""));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("red", "medical", "Morphine"));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("red", "food", "BakedBeansCan_Opened"));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("red", "drink", "SodaCan_Pipsi"));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("red", "tools", "FirefighterAxe_Green"));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("red", "material", "ShelterKit"));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("red", "valuable", ""));
			m_DNAConfig_Loot.Add(new DNA_Config_Loot("red", "misc", ""));
		}
	}

    public class DNA_Config_Loot
    {
        public string dna_Tier { get; set; }
        public string dna_Category { get; set; }
        public string dna_Type { get; set; }
		public DNA_Config_Loot() { }
		public DNA_Config_Loot(string tier, string category, string type)
        {
            dna_Tier = tier;
            dna_Category = category;
            dna_Type = type;
        }
    }

}
