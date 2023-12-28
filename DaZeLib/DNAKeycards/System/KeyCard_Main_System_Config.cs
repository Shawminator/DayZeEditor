using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class KeyCard_Main_System_Config
    {
        const int CURRENTVERSION = 2;

        public BindingList<DNA_Config_Version> m_DNAConfig_Version { get; set; }
        public BindingList<DNA_Config_Main_System> m_DNAConfig_Main_System { get; set; }
        public BindingList<DNA_Crate_Locations> m_DNAYellow_Crate_Locations { get; set; }
        public BindingList<DNA_Crate_Locations> m_DNAGreen_Crate_Locations { get; set; }
        public BindingList<DNA_Crate_Locations> m_DNABlue_Crate_Locations { get; set; }
        public BindingList<DNA_Crate_Locations> m_DNAPurple_Crate_Locations { get; set; }
        public BindingList<DNA_Crate_Locations> m_DNARed_Crate_Locations { get; set; }
        public BindingList<DNA_Strongroom_Locations> m_DNAYellow_Strongroom_Locations { get; set; }
        public BindingList<DNA_Strongroom_Locations> m_DNAGreen_Strongroom_Locations { get; set; }
        public BindingList<DNA_Strongroom_Locations> m_DNABlue_Strongroom_Locations { get; set; }
        public BindingList<DNA_Strongroom_Locations> m_DNAPurple_Strongroom_Locations { get; set; }
        public BindingList<DNA_Strongroom_Locations> m_DNARed_Strongroom_Locations { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public KeyCard_Main_System_Config()
        {
			m_DNAConfig_Version = new BindingList<DNA_Config_Version>();
			m_DNAConfig_Main_System = new BindingList<DNA_Config_Main_System>();
			m_DNAYellow_Crate_Locations = new BindingList<DNA_Crate_Locations>();
			m_DNAGreen_Crate_Locations = new BindingList<DNA_Crate_Locations>();
			m_DNABlue_Crate_Locations = new BindingList<DNA_Crate_Locations>();
			m_DNAPurple_Crate_Locations = new BindingList<DNA_Crate_Locations>();
			m_DNARed_Crate_Locations = new BindingList<DNA_Crate_Locations>();
			m_DNAYellow_Strongroom_Locations = new BindingList<DNA_Strongroom_Locations>();
			m_DNAGreen_Strongroom_Locations = new BindingList<DNA_Strongroom_Locations>();
			m_DNABlue_Strongroom_Locations = new BindingList<DNA_Strongroom_Locations>();
			m_DNAPurple_Strongroom_Locations = new BindingList<DNA_Strongroom_Locations>();
			m_DNARed_Strongroom_Locations = new BindingList<DNA_Strongroom_Locations>();
			CreateDefaultConfig();
		}


        public void CreateDefaultConfig()
		{
			m_DNAConfig_Version.Add(new DNA_Config_Version("DO NOT CHANGE THIS! IT WILL DELETE YOUR CONFIGS!!!!!!(but also creates backups ;) )", 2));

			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(0) Use DNA To Spawn Crates (0 = off, 1 = random, 2 = static) If off, you will need to place the crates yourself", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(1) Use DNA To Spawn Strongrooms (0 = off, 1 = random, 2 = static) If off, you will need to place the Strongrooms yourself", 0));

			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(2) Yellow Crates Spawn Count (If static or off, ignore setting. 0 to turn off this tier, don't set higher than number of available positions", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(3) Green Crates Spawn Count (If static or off, ignore setting. 0 to turn off this tier, don't set higher than number of available positions", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(4) Blue Crates Spawn Count (If static or off, ignore setting. 0 to turn off this tier, don't set higher than number of available positions", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(5) Purple Crates Spawn Count (If static or off, ignore setting. 0 to turn off this tier, don't set higher than number of available positions", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(6) Red Crates Spawn Count (If static or off, ignore setting. 0 to turn off this tier, don't set higher than number of available positions", 0));

			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(7) Yellow Strongrooms Spawn Count (If static or off, ignore setting. 0 to turn off this tier, don't set higher than number of available positions", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(8) Green Strongrooms Spawn Count (If static or off, ignore setting. 0 to turn off this tier, don't set higher than number of available positions", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(9) Blue Strongrooms Spawn Count (If static or off, ignore setting. 0 to turn off this tier, don't set higher than number of available positions", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(10) Purple Strongrooms Spawn Count (If static or off, ignore setting. 0 to turn off this tier, don't set higher than number of available positions", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(11) Red Strongrooms Spawn Count (If static or off, ignore setting. 0 to turn off this tier, don't set higher than number of available positions", 0));


			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(12) Yellow Card Usage Allotment", 2));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(13) Green Card Usage Allotment", 2));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(14) Blue Card Usage Allotment", 1));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(15) Purple Card Usage Allotment", 1));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(16) Red Card Usage Allotment", 1));

			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(17) Separate Sidearms by tier (1 = Yes, 0 = No)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(18) Separate Food and Drink by tier (1 = Yes, 0 = No)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(19) Separate Tools by tier (1 = Yes, 0 = No)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(20) Separate Meds by tier (1 = Yes, 0 = No)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(21) Separate Materials by tier (1 = Yes, 0 = No)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(22) Separate Miscellaneous by tier (1 = Yes, 0 = No)", 1));

			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(23) Yellow Strongrooms spawn this many wolves when opened - (0 = off - 10 =  max)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(24) Yellow Strongrooms spawn this many bears when opened - (0 = off - 6 =  max)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(25) Yellow Strongrooms spawn this many infected when opened - (0 = off - 40 =  max)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(26) Yellow Strongrooms alarm toggle - (0 = Off - 1 = On)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(27) Yellow Strongrooms notification range - (0 = Off - Max 2000m - 2001+ shows full server)", 0));

			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(28) Green Strongrooms spawn this many wolves when opened - (0 = off - 10 =  max)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(29) Green Strongrooms spawn this many bears when opened - (0 = off - 6 =  max)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(30) Green Strongrooms spawn this many infected when opened - (0 = off - 40 =  max)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(31) Green Strongrooms alarm toggle - (0 = Off - 1 = On)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(32) Green Strongrooms notification range - (0 = Off - Max 2000m - 2001+ shows full server)", 0));

			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(33) Blue Strongrooms spawn this many wolves when opened - (0 = off - 10 =  max)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(34) Blue Strongrooms spawn this many bears when opened - (0 = off - 6 =  max)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(35) Blue Strongrooms spawn this many infected when opened - (0 = off - 40 =  max)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(36) Blue Strongrooms alarm toggle - (0 = Off - 1 = On)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(37) Blue Strongrooms notification range - (0 = Off - Max 2000m - 2001+ shows full server)", 0));

			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(38) Purple Strongrooms spawn this many wolves when opened - (0 = off - 10 =  max)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(39) Purple Strongrooms spawn this many bears when opened - (0 = off - 6 =  max)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(40) Purple Strongrooms spawn this many infected when opened - (0 = off - 40 =  max)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(41) Purple Strongrooms alarm toggle - (0 = Off - 1 = On)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(42) Purple Strongrooms notification range - (0 = Off - Max 2000m - 2001+ shows full server)", 0));

			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(43) Red Strongrooms spawn this many wolves when opened - (0 = off - 10 =  max)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(44) Red Strongrooms spawn this many bears when opened - (0 = off - 6 =  max)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(45) Red Strongrooms spawn this many infected when opened - (0 = off - 40 =  max)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(46) Red Strongrooms alarm toggle - (0 = Off - 1 = On)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(47) Red Strongrooms notification range - (0 = Off - Max 2000m - 2001+ shows full server)", 0));


			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(48) Yellow Crates alarm toggle - (0 = Off - 1 = On)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(49) Yellow Crates notification range - (0 = Off - Max 2000m - 2001+ shows full server)", 0));

			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(50) Green Crates alarm toggle - (0 = Off - 1 = On)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(51) Green Crates notification range - (0 = Off - Max 2000m - 2001+ shows full server)", 0));

			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(52) Blue Crates alarm toggle - (0 = Off - 1 = On)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(53) Blue Crates notification range - (0 = Off - Max 2000m - 2001+ shows full server)", 0));

			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(54) Purple Crates alarm toggle - (0 = Off - 1 = On)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(55) Purple Crates notification range - (0 = Off - Max 2000m - 2001+ shows full server)", 0));

			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(56) Red Crates alarm toggle - (0 = Off - 1 = On)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(57) Red Crates notification range - (0 = Off - Max 2000m - 2001+ shows full server)", 0));


			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(58) Yellow Lockout Doors alarm toggle - (0 = Off - 1 = On)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(59) Yellow Lockout Doors notification range - (0 = Off - Max 2000m - 2001+ shows full server)", 0));

			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(60) Green Lockout Doors alarm toggle - (0 = Off - 1 = On)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(61) Green Lockout Doors notification range - (0 = Off - Max 2000m - 2001+ shows full server)", 0));

			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(62) Blue Lockout Doors alarm toggle - (0 = Off - 1 = On)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(63) Blue Lockout Doors notification range - (0 = Off - Max 2000m - 2001+ shows full server)", 0));

			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(64) Purple Lockout Doors alarm toggle - (0 = Off - 1 = On)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(65) Purple Lockout Doors notification range - (0 = Off - Max 2000m - 2001+ shows full server)", 0));

			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(66) Red Lockout Doors alarm toggle - (0 = Off - 1 = On)", 0));
			m_DNAConfig_Main_System.Add(new DNA_Config_Main_System("(67) Red Lockout Doors notification range - (0 = Off - Max 2000m - 2001+ shows full server)", 0));


			m_DNAYellow_Crate_Locations.Add(new DNA_Crate_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNAYellow_Crate_Locations.Add(new DNA_Crate_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNAYellow_Crate_Locations.Add(new DNA_Crate_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNAYellow_Crate_Locations.Add(new DNA_Crate_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNAYellow_Crate_Locations.Add(new DNA_Crate_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));

			m_DNAGreen_Crate_Locations.Add(new DNA_Crate_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNAGreen_Crate_Locations.Add(new DNA_Crate_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNAGreen_Crate_Locations.Add(new DNA_Crate_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNAGreen_Crate_Locations.Add(new DNA_Crate_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNAGreen_Crate_Locations.Add(new DNA_Crate_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));

			m_DNABlue_Crate_Locations.Add(new DNA_Crate_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNABlue_Crate_Locations.Add(new DNA_Crate_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNABlue_Crate_Locations.Add(new DNA_Crate_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNABlue_Crate_Locations.Add(new DNA_Crate_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNABlue_Crate_Locations.Add(new DNA_Crate_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));

			m_DNAPurple_Crate_Locations.Add(new DNA_Crate_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNAPurple_Crate_Locations.Add(new DNA_Crate_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNAPurple_Crate_Locations.Add(new DNA_Crate_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNAPurple_Crate_Locations.Add(new DNA_Crate_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNAPurple_Crate_Locations.Add(new DNA_Crate_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));

			m_DNARed_Crate_Locations.Add(new DNA_Crate_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNARed_Crate_Locations.Add(new DNA_Crate_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNARed_Crate_Locations.Add(new DNA_Crate_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNARed_Crate_Locations.Add(new DNA_Crate_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNARed_Crate_Locations.Add(new DNA_Crate_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));

			m_DNAYellow_Strongroom_Locations.Add(new DNA_Strongroom_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNAYellow_Strongroom_Locations.Add(new DNA_Strongroom_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNAYellow_Strongroom_Locations.Add(new DNA_Strongroom_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNAYellow_Strongroom_Locations.Add(new DNA_Strongroom_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNAYellow_Strongroom_Locations.Add(new DNA_Strongroom_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));

			m_DNAGreen_Strongroom_Locations.Add(new DNA_Strongroom_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNAGreen_Strongroom_Locations.Add(new DNA_Strongroom_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNAGreen_Strongroom_Locations.Add(new DNA_Strongroom_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNAGreen_Strongroom_Locations.Add(new DNA_Strongroom_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNAGreen_Strongroom_Locations.Add(new DNA_Strongroom_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));

			m_DNABlue_Strongroom_Locations.Add(new DNA_Strongroom_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNABlue_Strongroom_Locations.Add(new DNA_Strongroom_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNABlue_Strongroom_Locations.Add(new DNA_Strongroom_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNABlue_Strongroom_Locations.Add(new DNA_Strongroom_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNABlue_Strongroom_Locations.Add(new DNA_Strongroom_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));

			m_DNAPurple_Strongroom_Locations.Add(new DNA_Strongroom_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNAPurple_Strongroom_Locations.Add(new DNA_Strongroom_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNAPurple_Strongroom_Locations.Add(new DNA_Strongroom_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNAPurple_Strongroom_Locations.Add(new DNA_Strongroom_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNAPurple_Strongroom_Locations.Add(new DNA_Strongroom_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));

			m_DNARed_Strongroom_Locations.Add(new DNA_Strongroom_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNARed_Strongroom_Locations.Add(new DNA_Strongroom_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNARed_Strongroom_Locations.Add(new DNA_Strongroom_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNARed_Strongroom_Locations.Add(new DNA_Strongroom_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
			m_DNARed_Strongroom_Locations.Add(new DNA_Strongroom_Locations("0.0 0.0 0.0", "0.0 0.0 0.0"));
		}
	}

    public class DNA_Config_Version
    {
        public string dna_WarningMessage { get; set; }
        public int dna_ConfigVersion { get; set; }

		public DNA_Config_Version() { }
		public DNA_Config_Version(string warning, int version)
        {
            dna_WarningMessage = warning;
            dna_ConfigVersion = version;
        }

    }

    public class DNA_Config_Main_System
    {
        public string dna_Option { get; set; }
        public int dna_Setting { get; set; }
		public DNA_Config_Main_System() { }
		public DNA_Config_Main_System(string option, int setting)
        {
            dna_Option = option;
            dna_Setting = setting;
		}
		public override string ToString()
		{
			return dna_Option;
		}
	}

    public class DNA_Crate_Locations
    {
        public string dna_Location { get; set; }
        public string dna_Rotation { get; set; }
		public DNA_Crate_Locations() { }
		public DNA_Crate_Locations(string location, string rotation)
        {
            dna_Location = location;
            dna_Rotation = rotation;
        }

        public override string ToString()
        {
			string[] loc = dna_Location.TrimStart().TrimEnd().Split(' ');
            return "X:" + loc[0] + ",\tY:" + loc[1] + ",\tZ:" + loc[2];
        }
    }

    public class DNA_Strongroom_Locations
    {
        public string dna_Location { get; set; }
        public string dna_Rotation { get; set; }

		public DNA_Strongroom_Locations() { }
		public DNA_Strongroom_Locations(string location, string rotation)
        {
            dna_Location = location;
            dna_Rotation = rotation;
        }

		public override string ToString()
		{
			string[] loc = dna_Location.TrimStart().TrimEnd().Split(' ');
			return "X:" + loc[0] + ",\tY:" + loc[1] + ",\tZ:" + loc[2];
		}
	}


}
