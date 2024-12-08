using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public static class KillrewardStatics
    {
        public static string KillReward_CONFIG_FOLDER = "\\KillReward_Config";
        public static string KillRewardBase_CONFIG_JSON = KillReward_CONFIG_FOLDER + "\\KRBase_Config.json";
        public static string KillRewardZombie_CONFIG_JSON = KillReward_CONFIG_FOLDER + "\\KRZombie_Config.json";
        public static string KillRewardPlayer_CONFIG_JSON = KillReward_CONFIG_FOLDER + "\\KRPlayer_Config.json";
        public static string KillRewardHunting_CONFIG_JSON = KillReward_CONFIG_FOLDER + "\\KRHunting_Config.json";
        public static string KillRewardWeaponBox_CONFIG_JSON = KillReward_CONFIG_FOLDER + "\\KRWeponbox_Config.json";
        public static string KillRewardGift_CONFIG_JSON = KillReward_CONFIG_FOLDER + "\\KRGift_Config.json";

        public static bool Checkallfiles(string profile)
        {
            if(File.Exists(profile + KillRewardBase_CONFIG_JSON) &&
                File.Exists(profile + KillRewardZombie_CONFIG_JSON) &&
                File.Exists(profile + KillRewardPlayer_CONFIG_JSON) &&
                File.Exists(profile + KillRewardHunting_CONFIG_JSON) &&
                File.Exists(profile + KillRewardWeaponBox_CONFIG_JSON) &&
                File.Exists(profile + KillRewardGift_CONFIG_JSON)) 
            {
                return true;
            }
            return false;
        }
    }
    public class KillRewardBase_Config
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public KillReward_KillRewardBASESettings KillRewardBASE { get; set; }
        public int version { get; set; }

        public KillRewardBase_Config()
        {
            KillRewardBASE = new KillReward_KillRewardBASESettings();
            version = 9;
        }

    }
    public class KillReward_KillRewardBASESettings
    {
        public int ZombieGlobalMessage { get; set; }
        public int PlayerGlobalMessage { get; set; }
        public int MessageSystem { get; set; }
        public int PointLoseByDeath { get; set; }
        public int Statslose { get; set; }
        public int DebugLog { get; set; }
        public int PVPVE { get; set; }
        public BindingList<string> NotListedIDs { get; set; }

        public KillReward_KillRewardBASESettings()
        {
            ZombieGlobalMessage = 1;
            PlayerGlobalMessage = 1;
            MessageSystem = 2;
            PointLoseByDeath = 500;
            Statslose = 0;
            DebugLog = 0;
            PVPVE = 0;
            NotListedIDs = new BindingList<string>(){
                "0",
                "0"};
        }

        public void SetIntValue(string mytype, int myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
        public void SetDecimalValue(string mytype, decimal myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
        public void SetBoolValue(string mytype, int myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
        public void SetTextValue(string mytype, string myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
    }
    public class KillRewardGift_Config
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public KillReward_KillRewardGIFTSettings KillRewardGIFT { get; set; }
        public int version { get; set; }
        public KillRewardGift_Config()
        {
            KillRewardGIFT = new KillReward_KillRewardGIFTSettings();
            version = 9;
        }
    }
    public class KillReward_KillRewardGIFTSettings
    {
        public int PlayerGift { get; set; }
        public int GiftLifetime { get; set; }
        public BindingList<string> Giftbox { get; set; }
        public BindingList<string> Giftdrink { get; set; }
        public BindingList<string> Gifteat { get; set; }
        public BindingList<string> Gifttools { get; set; }
        public BindingList<string> Giftmedical { get; set; }

        public KillReward_KillRewardGIFTSettings()
        {
            PlayerGift = 1;
            GiftLifetime = 5;
            Giftbox = new BindingList<string>(){
                "FS_GiftBox_Medium_1",
                "FS_GiftBox_Medium_2",
                "FS_GiftBox_Medium_3"};
            Giftdrink = new BindingList<string>(){
                "SodaCan_Cola",
                "SodaCan_Kvass",
                "SodaCan_Pipsi",
                "SodaCan_Spite",
                "Canteen",
                "WaterBottle"};

            Gifteat = new BindingList<string>(){
                "BakedBeansCan",
                "TacticalBaconCan",
                "Apple",
                "BoxCerealCrunchin",
                "CaninaBerry",
                "Canteen",
                "GreenBellPepper",
                "Pear",
                "Plum",
                "Potato",
                "PowderedMilk",
                "Pumpkin",
                "Rice",
                "SambucusBerry",
                "SpaghettiCan_Opened",
                "Tomato",
                "TunaCan_Opened",
                "Zucchini" };

            Gifttools = new BindingList<string>(){
                "Battery9V",
                "CanOpener",
                "ChernarusMap",
                "Compass",
                "Flashlight",
                "Hacksaw",
                "Hammer",
                "Hatchet",
                "HuntingKnife",
                "LeatherSewingKit",
                "Matchbox",
                "Pickaxe",
                "Rangefinder",
                "Roadflare",
                "Screwdriver",
                "Whetstone"};

            Giftmedical = new BindingList<string>(){
                "BandageDressing",
                "Epinephrine",
                "Heatpack",
                "Morphine",
                "PainkillerTablets",
                "PurificationTablets",
                "Rag",
                "TetracyclineAntibiotics",
                "VitaminBottle"};
        }
    }
    public class KillRewardHunting_Config
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public KillReward_KillRewardHUNTINGSettings KillRewardHUNTING { get; set; }
        public int version { get; set; }

        public KillRewardHunting_Config()
        {
            KillRewardHUNTING = new KillReward_KillRewardHUNTINGSettings();
            version = 9;
        }


    }
    public class KillReward_KillRewardHUNTINGSettings
    {
        public int HuntingReward { get; set; }
        public int WolfPoints { get; set; }
        public BindingList<int> WolfKills { get; set; }
        public BindingList<int> WolfMoney { get; set; }
        public int BearPoints { get; set; }
        public BindingList<int> BearKills { get; set; }
        public BindingList<int> BearMoney { get; set; }
        public int StagPoints { get; set; }
        public BindingList<int> StagKills { get; set; }
        public BindingList<int> StagMoney { get; set; }
        public int HindPoints { get; set; }
        public BindingList<int> HindKills { get; set; }
        public BindingList<int> HindMoney { get; set; }
        public int RoebuckPoints { get; set; }
        public BindingList<int> RoebuckKills { get; set; }
        public BindingList<int> RoebuckMoney { get; set; }
        public int DoePoints { get; set; }
        public BindingList<int> DoeKills { get; set; }
        public BindingList<int> DoeMoney { get; set; }
        public int Wild_BoarPoints { get; set; }
        public BindingList<int> Wild_BoarKills { get; set; }
        public BindingList<int> Wild_BoarMoney { get; set; }
        public int PigPoints { get; set; }
        public BindingList<int> PigKills { get; set; }
        public BindingList<int> PigMoney { get; set; }
        public int RamPoints { get; set; }
        public BindingList<int> RamKills { get; set; }
        public BindingList<int> RamMoney { get; set; }
        public int EwePoints { get; set; }
        public BindingList<int> EweKills { get; set; }
        public BindingList<int> EweMoney { get; set; }
        public int billy_goatPoints { get; set; }
        public BindingList<int> billy_goatKills { get; set; }
        public BindingList<int> billy_goatMoney { get; set; }
        public int goatPoints { get; set; }
        public BindingList<int> goatKills { get; set; }
        public BindingList<int> goatMoney { get; set; }
        public int BullPoints { get; set; }
        public BindingList<int> BullKills { get; set; }
        public BindingList<int> BullMoney { get; set; }
        public int CowPoints { get; set; }
        public BindingList<int> CowKills { get; set; }
        public BindingList<int> CowMoney { get; set; }
        public int RoosterPoints { get; set; }
        public BindingList<int> RoosterKills { get; set; }
        public BindingList<int> RoosterMoney { get; set; }
        public int HenPoints { get; set; }
        public BindingList<int> HenKills { get; set; }
        public BindingList<int> HenMoney { get; set; }

        [JsonIgnore]
        public BindingList<KillReward_KillrewardHuntingAnimals> KRAnimals { get; set; }
        [JsonIgnore]
        public List<string> animalList = new List<string>()
        {
            "Wolf",
            "Bear",
            "Stag",
            "Hind",
            "Roebuck",
            "Doe",
            "Wild_Boar",
            "Pig",
            "Ram",
            "Ewe",
            "billy_goat",
            "goat",
            "Bull",
            "Cow",
            "Rooster",
            "Hen"
        };

        public KillReward_KillRewardHUNTINGSettings()
        {
            HuntingReward = 1;
            WolfPoints = 5;
            WolfKills = new BindingList<int>() { 5, 15, 25, 50, 75, 100 };                   // Reward for 1,2,4 animal kills
            WolfMoney = new BindingList<int>() { 50, 150, 250, 500, 750, 1000 };         // Money for Kills

            BearPoints = 5;
            BearKills = new BindingList<int>() { 5, 15, 25, 50, 75, 100 };                   // Reward for 1,2,4 animal kills
            BearMoney = new BindingList<int>() { 50, 150, 250, 500, 750, 1000 };             // Money for Kills

            StagPoints = 5;
            StagKills = new BindingList<int>() { 5, 15, 25, 50, 75, 100 };                   // Reward for 1,2,4 animal kills
            StagMoney = new BindingList<int>() { 50, 150, 250, 500, 750, 1000 };         // Money for Kills

            HindPoints = 5;
            HindKills = new BindingList<int>() { 5, 15, 25, 50, 75, 100 };                   // Reward for 1,2,4 animal kills
            HindMoney = new BindingList<int>() { 50, 150, 250, 500, 750, 1000 };         // Money for Kills

            RoebuckPoints = 5;
            RoebuckKills = new BindingList<int>() { 5, 15, 25, 50, 75, 100 };                // Reward for 1,2,4 animal kills
            RoebuckMoney = new BindingList<int>() { 50, 150, 250, 500, 750, 1000 };          // Money for Kills

            DoePoints = 5;
            DoeKills = new BindingList<int>() { 5, 15, 25, 50, 75, 100 };                    // Reward for 1,2,4 animal kills
            DoeMoney = new BindingList<int>() { 50, 150, 250, 500, 750, 1000 };              // Money for Kills

            Wild_BoarPoints = 5;
            Wild_BoarKills = new BindingList<int>() { 5, 15, 25, 50, 75, 100 };          // Reward for 1,2,4 animal kills
            Wild_BoarMoney = new BindingList<int>() { 50, 150, 250, 500, 750, 1000 };    // Money for Kills

            PigPoints = 5;
            PigKills = new BindingList<int>() { 5, 15, 25, 50, 75, 100 };                    // Reward for 1,2,4 animal kills
            PigMoney = new BindingList<int>() { 50, 150, 250, 500, 750, 1000 };              // Money for Kills

            RamPoints = 5;
            RamKills = new BindingList<int>() { 5, 15, 25, 50, 75, 100 };                    // Reward for 1,2,4 animal kills
            RamMoney = new BindingList<int>() { 50, 150, 250, 500, 750, 1000 };              // Money for Kills

            EwePoints = 5;
            EweKills = new BindingList<int>() { 5, 15, 25, 50, 75, 100 };                    // Reward for 1,2,4 animal kills
            EweMoney = new BindingList<int>() { 50, 150, 250, 500, 750, 1000 };              // Money for Kills

            billy_goatPoints = 5;
            billy_goatKills = new BindingList<int>() { 5, 15, 25, 50, 75, 100 };         // Reward for 1,2,4 animal kills
            billy_goatMoney = new BindingList<int>() { 50, 150, 250, 500, 750, 1000 };       // Money for Kills

            goatPoints = 5;
            goatKills = new BindingList<int>() { 5, 15, 25, 50, 75, 100 };                   // Reward for 1,2,4 animal kills
            goatMoney = new BindingList<int>() { 50, 150, 250, 500, 750, 1000 };         // Money for Kills

            BullPoints = 5;
            BullKills = new BindingList<int>() { 5, 15, 25, 50, 75, 100 };                   // Reward for 1,2,4 animal kills
            BullMoney = new BindingList<int>() { 50, 150, 250, 500, 750, 1000 };         // Money for Kills

            CowPoints = 5;
            CowKills = new BindingList<int>() { 5, 15, 25, 50, 75, 100 };                    // Reward for 1,2,4 animal kills
            CowMoney = new BindingList<int>() { 50, 150, 250, 500, 750, 1000 };              // Money for Kills

            RoosterPoints = 5;
            RoosterKills = new BindingList<int>() { 5, 15, 25, 50, 75, 100 };                // Reward for 1,2,4 animal kills
            RoosterMoney = new BindingList<int>() { 50, 150, 250, 500, 750, 1000 };          // Money for Kills

            HenPoints = 5;
            HenKills = new BindingList<int>() { 5, 15, 25, 50, 75, 100 };                    // Reward for 1,2,4 animal kills
            HenMoney = new BindingList<int>() { 50, 150, 250, 500, 750, 1000 };   			// Money for Kills
        }

        public int getPoints(string type)
        {
            return (int)GetType().GetProperty(type).GetValue(this, null);
        }
        public void setPoints(string v1, int v2)
        {
           GetType().GetProperty(v1).SetValue(this, v2, null);
        }
        public BindingList<int> GetIntList(string v)
        {
            return (BindingList<int>)GetType().GetProperty(v).GetValue(this, null);
        }
        public void SetIntList(string v, BindingList<int> array)
        {
            GetType().GetProperty(v).SetValue(this, array, null);
        }
        public void GetAnimaList()
        {
            KRAnimals = new BindingList<KillReward_KillrewardHuntingAnimals>();
            foreach (string animal in animalList)
            {
                KillReward_KillrewardHuntingAnimals newanimal = new KillReward_KillrewardHuntingAnimals()
                {
                    AnimalName = animal,
                    killsmoney = new BindingList<AnimalKillMoney>()
                };
                BindingList<int> kills = GetIntList(animal + "Kills");
                BindingList<int> Money = GetIntList(animal + "Money");
                for (int i = 0; i < kills.Count; i++)
                {
                    AnimalKillMoney newkm = new AnimalKillMoney()
                    {
                        kills = kills[i],
                        money = Money[i]
                    };
                    newanimal.killsmoney.Add(newkm);
                }
                KRAnimals.Add(newanimal);
            }
        }

        public void SetAnimalList()
        {
            foreach(KillReward_KillrewardHuntingAnimals animal in KRAnimals)
            {
                BindingList<int> KillArray = new BindingList<int>();
                BindingList<int> MoneyArray = new BindingList<int>();
                var sortedListInstance = new BindingList<AnimalKillMoney>(animal.killsmoney.OrderBy(x => x.kills).ToList());
                foreach (AnimalKillMoney akm in sortedListInstance)
                {
                    KillArray.Add(akm.kills);
                    MoneyArray.Add(akm.money);
                }
                SetIntList(animal.AnimalName + "Kills", KillArray);
                SetIntList(animal.AnimalName + "Money", MoneyArray);
            }
        }


    }
    public class KillReward_KillrewardHuntingAnimals
    {
        public string AnimalName { get; set; }
        public BindingList<AnimalKillMoney> killsmoney { get;set; }
    }
    public class AnimalKillMoney
    {
        public int kills { get; set; }
        public int money { get; set; }
    }
    public class KillRewardPlayer_Config
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public KillReward_KillRewardNEGATIVESettings KillRewardNEGATIVE { get; set; }
        public KillReward_KillRewardPLAYERSettings KillRewardPLAYER { get; set; }
        public KillReward_KillRewardSHOOTDISTANCESettings KillRewardSHOOTDISTANCE { get; set; }
        public int version { get; set; }

        public KillRewardPlayer_Config()
        {
            KillRewardNEGATIVE = new KillReward_KillRewardNEGATIVESettings();
            KillRewardPLAYER = new KillReward_KillRewardPLAYERSettings();
            KillRewardSHOOTDISTANCE = new KillReward_KillRewardSHOOTDISTANCESettings();
            version = 9;
        }
    }
    public class KillReward_KillRewardNEGATIVESettings
    {
        public int PlayerKillNegativeReward { get; set; }
        public int PlayerKill { get; set; }
        public int Pointlose { get; set; }
        public int LoseMoney { get; set; }
        public int PlayerKillNegativeRewardBonusQuantity { get; set; }
        public BindingList<string> PlayerKillNegativeRewardBonus { get; set; }

        public KillReward_KillRewardNEGATIVESettings()
        {
            PlayerKillNegativeReward = 0;                       // 1 = ON   0 = OFF for PVE is on "1" PlayerKillReward = "0"!
            PlayerKill = 0;
            Pointlose = 100;
            LoseMoney = 1000;
            PlayerKillNegativeRewardBonusQuantity = 15;
            PlayerKillNegativeRewardBonus = new BindingList<string>(){
                "Animal_UrsusArctos",
                "Animal_CanisLupus_White",
                "ZmbF_CitizenANormal_Brown",
                "ZmbF_CitizenBSkinny",
                "ZmbF_Clerk_Normal_White",
                "ZmbF_DoctorSkinny",
                "ZmbF_JoggerSkinny_Blue",
                "ZmbF_JournalistNormal_Red",
                "ZmbF_NurseFat",
                "ZmbF_ParamedicNormal_Red",
                "ZmbF_PatientOld",
                "ZmbF_PoliceWomanNormal",
                "ZmbF_ShortSkirt_black",
                "ZmbF_ShortSkirt_red",
                "ZmbF_ShortSkirt_yellow",
                "ZmbF_SkaterYoung_Violet",
                "ZmbM_CitizenASkinny_Red",
                "ZmbM_CitizenBFat_Blue",
                "ZmbM_ClerkFat_Brown",
                "ZmbM_CommercialPilotOld_Olive",
                "ZmbM_DoctorFat",
                "ZmbM_JacketNormal_greenchecks",
                "ZmbM_Jacket_stripes",
                "ZmbM_JoggerSkinny_Blue",
                "ZmbM_ParamedicNormal_Red",
                "ZmbM_PatientSkinny",
                "ZmbM_PatrolNormal_Autumn",
                "ZmbM_PolicemanFat",
                "ZmbM_PolicemanSpecForce",
                "ZmbM_PrisonerSkinny",
                "ZmbM_SkaterYoung_Grey"};
        }
    }
    public class KillReward_KillRewardPLAYERSettings
    {
        public int PlayerKillReward { get; set; }
        public int PlayerKillPoints { get; set; }
        public BindingList<int> PlayerKills { get; set; }
        public BindingList<int> PlayerMoney { get; set; }
        public BindingList<int> PlayerWeaponBox { get; set; }
        public BindingList<int> PlayerWeaponBoxNumber { get; set; }

        [JsonIgnore]
        public BindingList<Killreward_Player_Weapons> _playuerWeapons { get; set; }

        public KillReward_KillRewardPLAYERSettings()
        {
            PlayerKillReward = 1;                                           // 1 = ON   0 = OFF
            PlayerKillPoints = 100;                                         // Points for Playerkill
            PlayerKills = new BindingList<int>() { 5, 15, 35, 50, 75, 100 };             // Reward for 1,2,4 Palyer kills
            PlayerMoney = new BindingList<int>() { 500, 1500, 2500, 5000, 7500, 10000 }; // Money for Kills	
            PlayerWeaponBox = new BindingList<int>() { 50, 75, 100 };                        // WeaponBox for 3,5 Player kills
            PlayerWeaponBoxNumber = new BindingList<int>() { 1, 2, 3 };
        }

        public void GetPlayerlist()
        {
            _playuerWeapons = new BindingList<Killreward_Player_Weapons>();
            for (int i = 0;i< PlayerKills.Count(); i++)
            {
                Killreward_Player_Weapons newpw = new Killreward_Player_Weapons()
                {
                    kills = PlayerKills[i],
                    money = PlayerMoney[i]
                };
                if(PlayerWeaponBox.Contains(newpw.kills))
                {
                    int index = PlayerWeaponBox.IndexOf(newpw.kills);
                    newpw.hasWeaponsBox = true;
                    newpw.WeaponsBoxNumber = PlayerWeaponBoxNumber[index];
                }
                _playuerWeapons.Add(newpw);
            }
        }

        public void SetPlayerList()
        {
            PlayerKills = new BindingList<int>();
            PlayerMoney = new BindingList<int>();
            PlayerWeaponBox = new BindingList<int>();
            PlayerWeaponBoxNumber = new BindingList<int>();
            foreach(Killreward_Player_Weapons KRPW in _playuerWeapons)
            {
                PlayerKills.Add(KRPW.kills);
                PlayerMoney.Add(KRPW.money);
                if(KRPW.hasWeaponsBox)
                {
                    PlayerWeaponBox.Add(KRPW.kills);
                    PlayerWeaponBoxNumber.Add(KRPW.WeaponsBoxNumber);
                }
            }
        }
    }
    public class Killreward_Player_Weapons
    {
        public int kills { get; set; }
        public int money { get; set; }
        public bool hasWeaponsBox = false;
        public int WeaponsBoxNumber { get; set; }

    }
    public class KillReward_KillRewardSHOOTDISTANCESettings
    {
        public int SHOOTReward { get; set; }
        public float PlayerSHOOTDISTANCE { get; set; }

        public KillReward_KillRewardSHOOTDISTANCESettings()
        {
            SHOOTReward = 1;
            PlayerSHOOTDISTANCE = 500;
        }

    }
    public class KillRewardWeaponBox_Config
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public BindingList<KillReward_KillRewardWEAPONBOXSettings> KillRewardWEAPONBOX { get; set; }
        public int version { get; set; }

        public KillRewardWeaponBox_Config()
        {
            KillRewardWEAPONBOX = new BindingList<KillReward_KillRewardWEAPONBOXSettings>();
            version = 9;
        }
    }
    public class KillReward_KillRewardWEAPONBOXSettings
    {
        public int BoxNumber { get; set; }
        public string Box { get; set; }
        public string BoxWeapon { get; set; }
        public string BoxWeaponBayonet { get; set; }
        public string BoxWeaponButtstock { get; set; }
        public string BoxWeaponHandguard { get; set; }
        public string BoxWeaponsuppressor { get; set; }
        public string BoxWeaponSight { get; set; }
        public string BoxWeaponMagazin { get; set; }
        public int BoxMagazinQuantity { get; set; }
        public BindingList<string> BoxBonus { get; set; }

        public KillReward_KillRewardWEAPONBOXSettings()
        {
            BoxBonus = new BindingList<string>();
        }
        public KillReward_KillRewardWEAPONBOXSettings(int boxNumber, string box, string boxWeapon, string boxWeaponBayonet, string boxWeaponButtstock, string boxWeaponHandguard, string boxWeaponsuppressor, string boxWeaponSight, string boxWeaponMagazin, int boxMagazinQuantity, BindingList<string> boxBonus)
        {
            BoxNumber = boxNumber;
            Box = box;
            BoxWeapon = boxWeapon;
            BoxWeaponBayonet = boxWeaponBayonet;
            BoxWeaponButtstock = boxWeaponButtstock;
            BoxWeaponHandguard = boxWeaponHandguard;
            BoxWeaponsuppressor = boxWeaponsuppressor;
            BoxWeaponSight = boxWeaponSight;
            BoxWeaponMagazin = boxWeaponMagazin;
            BoxMagazinQuantity = boxMagazinQuantity;
            BoxBonus = boxBonus;
        }
    }


    public class KillRewardZombie_Config
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public KillReward_KillRewardZOMBIESettings KillRewardZOMBIE { get; set; }
        public int version { get; set; }

        public KillRewardZombie_Config()
        {
            KillRewardZOMBIE = new KillReward_KillRewardZOMBIESettings();
            version = 9;
        }
    }

    public class KillReward_KillRewardZOMBIESettings
    {
        public int ZombieKillReward { get; set; }
        public int ZombieKillPoints { get; set; }
        public int ZombieKillsbyCar { get; set; }
        public BindingList<int> ZombieKills { get; set; }
        public BindingList<int> ZombieMoney { get; set; }
        public BindingList<int> ZombieWeaponBox { get; set; }
        public BindingList<int> ZombieWeaponBoxNumber { get; set; }

        public KillReward_KillRewardZOMBIESettings()
        {
            ZombieKillReward = 1;                                       // 1 = ON   0 = OFF
            ZombieKillPoints = 10;                                      // Points for Zombiekills
            ZombieKillsbyCar = 0;                                       // 1 = ON   0 = OFF		
            ZombieKills = new BindingList<int>() { 5, 15, 35, 50, 75, 100 };         // Reward for 1,2,4 Zombie kills
            ZombieMoney = new BindingList<int>() { 50, 150, 250, 500, 750, 1000 };       // Money for Kills
            ZombieWeaponBox = new BindingList<int>() { 50, 75, 100 };                    // WeaponBox for 3,5 Zombie kills
            ZombieWeaponBoxNumber = new BindingList<int>() { 1, 2, 3 };
        }
    }

}
