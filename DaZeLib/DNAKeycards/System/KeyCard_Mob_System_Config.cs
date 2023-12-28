using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class KeyCard_Mob_System_Config
    {
        public BindingList<DNA_Config_Mob_System> m_DNAConfig_Mob_System { get; set; }

		[JsonIgnore]
		public string Filename { get; set; }
		[JsonIgnore]
		public bool isDirty { get; set; }

		public KeyCard_Mob_System_Config()
		{
			m_DNAConfig_Mob_System = new BindingList<DNA_Config_Mob_System>();
			CreateDefaultConfig();
		}

		public void CreateDefaultConfig()
		{
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("wolf", "Animal_CanisLupus_Grey"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("wolf", "Animal_CanisLupus_White"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("bear", "Animal_UrsusArctos"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_CitizenASkinny_Brown"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_JournalistNormal_White"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_priestPopSkinny"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_Clerk_Normal_White"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_HermitSkinny_Beige"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_CitizenANormal_Blue"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_CitizenBFat_Red"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_CitizenBSkinny"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_FishermanOld_Green"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_HikerSkinny_Grey"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_HunterOld_Autumn"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_SurvivorNormal_Orange"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_CitizenBFat_Blue"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_HikerSkinny_Green"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_MotobikerFat_Black"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_JoggerSkinny_Green"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_JoggerSkinny_Red"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_SkaterYoung_Striped"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_MechanicSkinny_Grey"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_BlueCollarFat_Red"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_HandymanNormal_Green"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_MechanicNormal_Beige"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_HeavyIndustryWorker"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_PatientOld"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_Jacket_black"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_ShortSkirt_beige"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_Jacket_stripes"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_VillagerOld_Red"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_HikerSkinny_Blue"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_JoggerSkinny_Red"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_HikerSkinny_Yellow"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_MilkMaidOld_Beige"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_PolicemanFat"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_VillagerOld_Green"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_PatrolNormal_Summer"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_ShortSkirt_yellow"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_JoggerSkinny_Blue"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_NurseFat"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_VillagerOld_White"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_PoliceWomanNormal"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_SkaterYoung_Brown"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_HikerSkinny_Blue"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_MechanicSkinny_Green"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_ParamedicNormal_Green"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_DoctorFat"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_JournalistNormal_Red"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_PatientSkinny"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_SurvivorNormal_White"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_ClerkFat_Brown"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_JoggerSkinny_Brown"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_ClerkFat_White"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_MechanicNormal_Grey"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_Jacket_magenta"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_BlueCollarFat_Green"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbM_PolicemanSpecForce"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("infected", "ZmbF_DoctorSkinny"));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("bossYellow", ""));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("bossGreen", ""));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("bossBlue", ""));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("bossPurple", ""));
			m_DNAConfig_Mob_System.Add(new DNA_Config_Mob_System("bossRed", ""));
		}
	}

    public class DNA_Config_Mob_System
    {
        public string dna_DefaultMob { get; set; }
        public string dna_MobType { get; set; }
		public DNA_Config_Mob_System() { }
		public DNA_Config_Mob_System(string defaultMob, string mobType)
        {
            dna_DefaultMob = defaultMob;
            dna_MobType = mobType;
        }
        public override string ToString()
        {
            return dna_DefaultMob + ":" + dna_MobType;
        }
    }

}
