using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class KeyCard_LootContainers_System_Config
    {
		[JsonIgnore]
		public string Filename { get; set; }
		[JsonIgnore]
		public bool isDirty { get; set; }

		public BindingList<DNA_Config_Container_System> m_DNAConfig_Container_System { get; set; }

		public KeyCard_LootContainers_System_Config()
        {
			m_DNAConfig_Container_System = new BindingList<DNA_Config_Container_System>();
			CreateDefaultConfig();
        }

		public void CreateDefaultConfig()
		{
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(0) [YELLOW TIER] Min Main Weapons To Give (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(1) [YELLOW TIER] Max Main Weapons To Give (Set to 0 to turn off)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(2) [YELLOW TIER] Spawn Main Weapon Attachments(1 true, 0 false)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(3) [YELLOW TIER] Min Extra Ammo to give for Main Weapon (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(4) [YELLOW TIER] Max Extra Ammo to give for Main Weapon (Set to 0 to turn off, If on, don't set lower than min!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(5) [YELLOW TIER] Min Extra Mags to give for Main Weapon (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(6) [YELLOW TIER] Max Extra Mags to give for Main Weapon (Set to 0 to turn off)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(7) [YELLOW TIER] Min Side Arms To Give (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(8) [YELLOW TIER] Max Side Arms To Give (Set to 0 to turn off)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(9) [YELLOW TIER] Spawn Side Arm Attachments (1 true, 0 false)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(10) [YELLOW TIER] Min Extra Ammo to give for Side Arm (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(11) [YELLOW TIER] Max Extra Ammo to give for Side Arm (Set to 0 to turn off, If on, don't set lower than min!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(12) [YELLOW TIER] Min Extra Mags to give for Side Arm (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(13) [YELLOW TIER] Max Extra Mags to give for Side Arm (Set to 0 to turn off)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(14) [YELLOW TIER] Min Food Types to give (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(15) [YELLOW TIER] Max Food Types to give (Set to 0 to turn off - If on, don't set higher than food type entry count or lower than min!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(16) [YELLOW TIER] Min Amount Of Each Food Type to give (If on, don't set lower than 1))", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(17) [YELLOW TIER] Max Amount Of Each Food Type to give (Don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(18) [YELLOW TIER] Min Drink Types to give (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(19) [YELLOW TIER] Max Drink Types to give (Set to 0 to turn off - If on, don't set higher than drink type entry count or lower than min!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(20) [YELLOW TIER] Min Amount Of Each Drink Type to give (If on, don't set lower than 1)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(21) [YELLOW TIER] Max Amount Of Each Drink Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(22) [YELLOW TIER] Min Tool Types to give (Set to 0 for chance to not spawn!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(23) [YELLOW TIER] Max Tool Types to give (Set to 0 to turn off - If on, DO NOT set lower than min, or higher than available type entries! Each type will only be chosen once.)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(24) [YELLOW TIER] Min Amount Of Each Tool Type to give (If on, don't set lower than 1)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(25) [YELLOW TIER] Max Amount Of Each Tool Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(26) [YELLOW TIER] Min Med Types to give (Set to 0 for chance to not spawn!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(27) [YELLOW TIER] Max Med Types to give (Set to 0 to turn off - If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(28) [YELLOW TIER] Min Amount Of Each Med Type to give (If on, don't set lower than 1)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(29) [YELLOW TIER] Max Amount Of Each Med Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(30) [YELLOW TIER] Min Material Types to give (Set to 0 for chance to not spawn!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(31) [YELLOW TIER] Max Material Types to give (Set to 0 to turn off - If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(32) [YELLOW TIER] Min Amount Of Each Material Type to give (If on, don't set lower than 1)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(33) [YELLOW TIER] Max Amount Of Each Material Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(34) [YELLOW TIER] Spawn Valuables ODDS % (0-100) 0 = OFF (Default)", 0, 0));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(35) [YELLOW TIER] Min Valuable Types to give (Don't set below 1)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(36) [YELLOW TIER] Max Valuable Types to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(37) [YELLOW TIER] Min Amount Of Each Valuable Type to give (Set to 0 for chance to not spawn!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(38) [YELLOW TIER] Max Amount Of Each Valuable Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(39) [YELLOW TIER] Min Miscellaneous Types to give (Set to 0 for chance to spawn none.)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(40) [YELLOW TIER] Max Miscellaneous Types to give (0 to turn off)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(41) [YELLOW TIER] Min Amount Of Each Miscellaneous Type to give (Set to 0 for chance to not spawn current type!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(42) [YELLOW TIER] Max Amount Of Each Miscellaneous Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(43) [YELLOW TIER] Min Amount Of Clothes/Outfits to give (Set to 0 for chance to not spawn!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(44) [YELLOW TIER] Max Amount Of Clothes/Outfits to give (If on, don't set lower than min!)", 3, 3));


			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(45) [GREEN TIER] Min Main Weapons To Give (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(46) [GREEN TIER] Max Main Weapons To Give (Set to 0 to turn off)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(47) [GREEN TIER] Spawn Main Weapon Attachments(1 true, 0 false)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(48) [GREEN TIER] Min Extra Ammo to give for Main Weapon (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(49) [GREEN TIER] Max Extra Ammo to give for Main Weapon (Set to 0 to turn off, If on, don't set lower than min!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(50) [GREEN TIER] Min Extra Mags to give for Main Weapon (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(51) [GREEN TIER] Max Extra Mags to give for Main Weapon (Set to 0 to turn off)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(52) [GREEN TIER] Min Side Arms To Give (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(53) [GREEN TIER] Max Side Arms To Give (Set to 0 to turn off)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(54) [GREEN TIER] Spawn Side Arm Attachments (1 true, 0 false)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(55) [GREEN TIER] Min Extra Ammo to give for Side Arm (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(56) [GREEN TIER] Max Extra Ammo to give for Side Arm (Set to 0 to turn off, If on, don't set lower than min!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(57) [GREEN TIER] Min Extra Mags to give for Side Arm (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(58) [GREEN TIER] Max Extra Mags to give for Side Arm (Set to 0 to turn off)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(59) [GREEN TIER] Min Food Types to give (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(60) [GREEN TIER] Max Food Types to give (Set to 0 to turn off - If on, don't set higher than food type entry count or lower than min!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(61) [GREEN TIER] Min Amount Of Each Food Type to give (If on, don't set lower than 1))", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(62) [GREEN TIER] Max Amount Of Each Food Type to give (Don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(63) [GREEN TIER] Min Drink Types to give (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(64) [GREEN TIER] Max Drink Types to give (Set to 0 to turn off - If on, don't set higher than drink type entry count or lower than min!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(65) [GREEN TIER] Min Amount Of Each Drink Type to give (If on, don't set lower than 1)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(66) [GREEN TIER] Max Amount Of Each Drink Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(67) [GREEN TIER] Min Tool Types to give (Set to 0 for chance to not spawn!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(68) [GREEN TIER] Max Tool Types to give (Set to 0 to turn off - If on, DO NOT set lower than min, or higher than available type entries! Each type will only be chosen once.)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(69) [GREEN TIER] Min Amount Of Each Tool Type to give (If on, don't set lower than 1)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(70) [GREEN TIER] Max Amount Of Each Tool Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(71) [GREEN TIER] Min Med Types to give (Set to 0 for chance to not spawn!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(72) [GREEN TIER] Max Med Types to give (Set to 0 to turn off - If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(73) [GREEN TIER] Min Amount Of Each Med Type to give (If on, don't set lower than 1)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(74) [GREEN TIER] Max Amount Of Each Med Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(75) [GREEN TIER] Min Material Types to give (Set to 0 for chance to not spawn!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(76) [GREEN TIER] Max Material Types to give (Set to 0 to turn off - If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(77) [GREEN TIER] Min Amount Of Each Material Type to give (If on, don't set lower than 1)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(78) [GREEN TIER] Max Amount Of Each Material Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(79) [GREEN TIER] Spawn Valuables ODDS % (0-100) 0 = OFF (Default)", 0, 0));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(80) [GREEN TIER] Min Valuable Types to give (Don't set below 1)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(81) [GREEN TIER] Max Valuable Types to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(82) [GREEN TIER] Min Amount Of Each Valuable Type to give (Set to 0 for chance to not spawn!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(83) [GREEN TIER] Max Amount Of Each Valuable Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(84) [GREEN TIER] Min Miscellaneous Types to give (Set to 0 for chance to spawn none.)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(85) [GREEN TIER] Max Miscellaneous Types to give (0 to turn off)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(86) [GREEN TIER] Min Amount Of Each Miscellaneous Type to give (Set to 0 for chance to not spawn current type!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(87) [GREEN TIER] Max Amount Of Each Miscellaneous Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(88) [GREEN TIER] Min Amount Of Clothes/Outfits to give (Set to 0 for chance to not spawn!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(89) [GREEN TIER] Max Amount Of Clothes/Outfits to give (If on, don't set lower than min!)", 3, 3));


			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(90) [BLUE TIER] Min Main Weapons To Give (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(91) [BLUE TIER] Max Main Weapons To Give (Set to 0 to turn off)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(92) [BLUE TIER] Spawn Main Weapon Attachments(1 true, 0 false)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(93) [BLUE TIER] Min Extra Ammo to give for Main Weapon (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(94) [BLUE TIER] Max Extra Ammo to give for Main Weapon (Set to 0 to turn off, If on, don't set lower than min!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(95) [BLUE TIER] Min Extra Mags to give for Main Weapon (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(96) [BLUE TIER] Max Extra Mags to give for Main Weapon (Set to 0 to turn off)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(97) [BLUE TIER] Min Side Arms To Give (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(98) [BLUE TIER] Max Side Arms To Give (Set to 0 to turn off)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(99) [BLUE TIER] Spawn Side Arm Attachments (1 true, 0 false)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(100) [BLUE TIER] Min Extra Ammo to give for Side Arm (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(101) [BLUE TIER] Max Extra Ammo to give for Side Arm (Set to 0 to turn off, If on, don't set lower than min!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(102) [BLUE TIER] Min Extra Mags to give for Side Arm (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(103) [BLUE TIER] Max Extra Mags to give for Side Arm (Set to 0 to turn off)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(104) [BLUE TIER] Min Food Types to give (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(105) [BLUE TIER] Max Food Types to give (Set to 0 to turn off - If on, don't set higher than food type entry count or lower than min!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(106) [BLUE TIER] Min Amount Of Each Food Type to give (If on, don't set lower than 1))", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(107) [BLUE TIER] Max Amount Of Each Food Type to give (Don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(108) [BLUE TIER] Min Drink Types to give (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(109) [BLUE TIER] Max Drink Types to give (Set to 0 to turn off - If on, don't set higher than drink type entry count or lower than min!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(110) [BLUE TIER] Min Amount Of Each Drink Type to give (If on, don't set lower than 1)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(111) [BLUE TIER] Max Amount Of Each Drink Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(112) [BLUE TIER] Min Tool Types to give (Set to 0 for chance to not spawn!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(113) [BLUE TIER] Max Tool Types to give (Set to 0 to turn off - If on, DO NOT set lower than min, or higher than available type entries! Each type will only be chosen once.)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(114) [BLUE TIER] Min Amount Of Each Tool Type to give (If on, don't set lower than 1)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(115) [BLUE TIER] Max Amount Of Each Tool Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(116) [BLUE TIER] Min Med Types to give (Set to 0 for chance to not spawn!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(117) [BLUE TIER] Max Med Types to give (Set to 0 to turn off - If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(118) [BLUE TIER] Min Amount Of Each Med Type to give (If on, don't set lower than 1)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(119) [BLUE TIER] Max Amount Of Each Med Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(120) [BLUE TIER] Min Material Types to give (Set to 0 for chance to not spawn!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(121) [BLUE TIER] Max Material Types to give (Set to 0 to turn off - If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(122) [BLUE TIER] Min Amount Of Each Material Type to give (If on, don't set lower than 1)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(123) [BLUE TIER] Max Amount Of Each Material Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(124) [BLUE TIER] Spawn Valuables ODDS % (0-100) 0 = OFF (Default)", 0, 0));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(125) [BLUE TIER] Min Valuable Types to give (Don't set below 1)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(126) [BLUE TIER] Max Valuable Types to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(127) [BLUE TIER] Min Amount Of Each Valuable Type to give (Set to 0 for chance to not spawn!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(128) [BLUE TIER] Max Amount Of Each Valuable Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(129) [BLUE TIER] Min Miscellaneous Types to give (Set to 0 for chance to spawn none.)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(130) [BLUE TIER] Max Miscellaneous Types to give (0 to turn off)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(131) [BLUE TIER] Min Amount Of Each Miscellaneous Type to give (Set to 0 for chance to not spawn current type!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(132) [BLUE TIER] Max Amount Of Each Miscellaneous Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(133) [BLUE TIER] Min Amount Of Clothes/Outfits to give (Set to 0 for chance to not spawn!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(134) [BLUE TIER] Max Amount Of Clothes/Outfits to give (If on, don't set lower than min!)", 3, 3));


			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(135) [PURPLE TIER] Min Main Weapons To Give (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(136) [PURPLE TIER] Max Main Weapons To Give (Set to 0 to turn off)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(137) [PURPLE TIER] Spawn Main Weapon Attachments(1 true, 0 false)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(138) [PURPLE TIER] Min Extra Ammo to give for Main Weapon (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(139) [PURPLE TIER] Max Extra Ammo to give for Main Weapon (Set to 0 to turn off, If on, don't set lower than min!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(140) [PURPLE TIER] Min Extra Mags to give for Main Weapon (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(141) [PURPLE TIER] Max Extra Mags to give for Main Weapon (Set to 0 to turn off)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(142) [PURPLE TIER] Min Side Arms To Give (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(143) [PURPLE TIER] Max Side Arms To Give (Set to 0 to turn off)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(144) [PURPLE TIER] Spawn Side Arm Attachments (1 true, 0 false)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(145) [PURPLE TIER] Min Extra Ammo to give for Side Arm (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(146) [PURPLE TIER] Max Extra Ammo to give for Side Arm (Set to 0 to turn off, If on, don't set lower than min!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(147) [PURPLE TIER] Min Extra Mags to give for Side Arm (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(148) [PURPLE TIER] Max Extra Mags to give for Side Arm (Set to 0 to turn off)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(149) [PURPLE TIER] Min Food Types to give (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(150) [PURPLE TIER] Max Food Types to give (Set to 0 to turn off - If on, don't set higher than food type entry count or lower than min!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(151) [PURPLE TIER] Min Amount Of Each Food Type to give (If on, don't set lower than 1))", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(152) [PURPLE TIER] Max Amount Of Each Food Type to give (Don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(153) [PURPLE TIER] Min Drink Types to give (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(154) [PURPLE TIER] Max Drink Types to give (Set to 0 to turn off - If on, don't set higher than drink type entry count or lower than min!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(155) [PURPLE TIER] Min Amount Of Each Drink Type to give (If on, don't set lower than 1)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(156) [PURPLE TIER] Max Amount Of Each Drink Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(157) [PURPLE TIER] Min Tool Types to give (Set to 0 for chance to not spawn!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(158) [PURPLE TIER] Max Tool Types to give (Set to 0 to turn off - If on, DO NOT set lower than min, or higher than available type entries! Each type will only be chosen once.)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(159) [PURPLE TIER] Min Amount Of Each Tool Type to give (If on, don't set lower than 1)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(160) [PURPLE TIER] Max Amount Of Each Tool Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(161) [PURPLE TIER] Min Med Types to give (Set to 0 for chance to not spawn!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(162) [PURPLE TIER] Max Med Types to give (Set to 0 to turn off - If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(163) [PURPLE TIER] Min Amount Of Each Med Type to give (If on, don't set lower than 1)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(164) [PURPLE TIER] Max Amount Of Each Med Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(165) [PURPLE TIER] Min Material Types to give (Set to 0 for chance to not spawn!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(166) [PURPLE TIER] Max Material Types to give (Set to 0 to turn off - If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(167) [PURPLE TIER] Min Amount Of Each Material Type to give (If on, don't set lower than 1)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(168) [PURPLE TIER] Max Amount Of Each Material Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(169) [PURPLE TIER] Spawn Valuables ODDS % (0-100) 0 = OFF (Default)", 0, 0));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(170) [PURPLE TIER] Min Valuable Types to give (Don't set below 1)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(171) [PURPLE TIER] Max Valuable Types to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(172) [PURPLE TIER] Min Amount Of Each Valuable Type to give (Set to 0 for chance to not spawn!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(173) [PURPLE TIER] Max Amount Of Each Valuable Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(174) [PURPLE TIER] Min Miscellaneous Types to give (Set to 0 for chance to spawn none.)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(175) [PURPLE TIER] Max Miscellaneous Types to give (0 to turn off)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(176) [PURPLE TIER] Min Amount Of Each Miscellaneous Type to give (Set to 0 for chance to not spawn current type!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(177) [PURPLE TIER] Max Amount Of Each Miscellaneous Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(178) [PURPLE TIER] Min Amount Of Clothes/Outfits to give (Set to 0 for chance to not spawn!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(179) [PURPLE TIER] Max Amount Of Clothes/Outfits to give (If on, don't set lower than min!)", 3, 3));


			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(180) [RED TIER] Min Main Weapons To Give (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(181) [RED TIER] Max Main Weapons To Give (Set to 0 to turn off)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(182) [RED TIER] Spawn Main Weapon Attachments(1 true, 0 false)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(183) [RED TIER] Min Extra Ammo to give for Main Weapon (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(184) [RED TIER] Max Extra Ammo to give for Main Weapon (Set to 0 to turn off, If on, don't set lower than min!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(185) [RED TIER] Min Extra Mags to give for Main Weapon (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(186) [RED TIER] Max Extra Mags to give for Main Weapon (Set to 0 to turn off)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(187) [RED TIER] Min Side Arms To Give (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(188) [RED TIER] Max Side Arms To Give (Set to 0 to turn off)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(189) [RED TIER] Spawn Side Arm Attachments (1 true, 0 false)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(190) [RED TIER] Min Extra Ammo to give for Side Arm (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(191) [RED TIER] Max Extra Ammo to give for Side Arm (Set to 0 to turn off, If on, don't set lower than min!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(192) [RED TIER] Min Extra Mags to give for Side Arm (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(193) [RED TIER] Max Extra Mags to give for Side Arm (Set to 0 to turn off)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(194) [RED TIER] Min Food Types to give (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(195) [RED TIER] Max Food Types to give (Set to 0 to turn off - If on, don't set higher than food type entry count or lower than min!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(196) [RED TIER] Min Amount Of Each Food Type to give (If on, don't set lower than 1))", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(197) [RED TIER] Max Amount Of Each Food Type to give (Don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(198) [RED TIER] Min Drink Types to give (Set to 0 for chance to not spawn!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(199) [RED TIER] Max Drink Types to give (Set to 0 to turn off - If on, don't set higher than drink type entry count or lower than min!)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(200) [RED TIER] Min Amount Of Each Drink Type to give (If on, don't set lower than 1)", 1, 1));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(201) [RED TIER] Max Amount Of Each Drink Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(202) [RED TIER] Min Tool Types to give (Set to 0 for chance to not spawn!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(203) [RED TIER] Max Tool Types to give (Set to 0 to turn off - If on, DO NOT set lower than min, or higher than available type entries! Each type will only be chosen once.)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(204) [RED TIER] Min Amount Of Each Tool Type to give (If on, don't set lower than 1)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(205) [RED TIER] Max Amount Of Each Tool Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(206) [RED TIER] Min Med Types to give (Set to 0 for chance to not spawn!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(207) [RED TIER] Max Med Types to give (Set to 0 to turn off - If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(208) [RED TIER] Min Amount Of Each Med Type to give (If on, don't set lower than 1)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(209) [RED TIER] Max Amount Of Each Med Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(210) [RED TIER] Min Material Types to give (Set to 0 for chance to not spawn!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(211) [RED TIER] Max Material Types to give (Set to 0 to turn off - If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(212) [RED TIER] Min Amount Of Each Material Type to give (If on, don't set lower than 1)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(213) [RED TIER] Max Amount Of Each Material Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(214) [RED TIER] Spawn Valuables ODDS % (0-100) 0 = OFF (Default)", 0, 0));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(215) [RED TIER] Min Valuable Types to give (Don't set below 1)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(216) [RED TIER] Max Valuable Types to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(217) [RED TIER] Min Amount Of Each Valuable Type to give (Set to 0 for chance to not spawn!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(218) [RED TIER] Max Amount Of Each Valuable Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(219) [RED TIER] Min Miscellaneous Types to give (Set to 0 for chance to spawn none.)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(220) [RED TIER] Max Miscellaneous Types to give (0 to turn off)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(221) [RED TIER] Min Amount Of Each Miscellaneous Type to give (Set to 0 for chance to not spawn current type!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(222) [RED TIER] Max Amount Of Each Miscellaneous Type to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(223) [RED TIER] Min Amount Of Clothes/Outfits to give (Set to 0 for chance to not spawn!)", 2, 2));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(224) [RED TIER] Max Amount Of Clothes/Outfits to give (If on, don't set lower than min!)", 3, 3));
			m_DNAConfig_Container_System.Add(new DNA_Config_Container_System("(225) DO NOT EDIT THIS VALUE!!!!", 0, 0));

		}
	}

    public class DNA_Config_Container_System
    {
        public string dna_Option { get; set; }
        public int dna_CrateSetting { get; set; }
        public int dna_StrongroomSetting { get; set; }
		public DNA_Config_Container_System() { }
		public DNA_Config_Container_System(string option, int c_setting, int sr_setting)
        {
            dna_Option = option;
            dna_CrateSetting = c_setting;
            dna_StrongroomSetting = sr_setting;
        }
    }

}
