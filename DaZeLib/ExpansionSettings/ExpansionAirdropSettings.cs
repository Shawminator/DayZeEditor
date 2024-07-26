using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class ExpansionAirdropSettings
    {
        const int CurrentVersion = 5;

        public int m_Version { get; set; }
        public int ServerMarkerOnDropLocation { get; set; }
        public int Server3DMarkerOnDropLocation { get; set; }
        public int ShowAirdropTypeOnMarker { get; set; }
        public int HideCargoWhileParachuteIsDeployed { get; set; }
        public int HeightIsRelativeToGroundLevel { get; set; }
        public decimal Height { get; set; }
        public decimal FollowTerrainFraction { get; set; }
        public decimal Speed { get; set; }
        public decimal Radius { get; set; }
        public decimal InfectedSpawnRadius { get; set; }
        public int InfectedSpawnInterval { get; set; }
        public int ItemCount { get; set; }
        public BindingList<ExpansionLootContainer> Containers { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public ExpansionAirdropSettings()
        {
            m_Version = CurrentVersion;
            ServerMarkerOnDropLocation = 1;
            Server3DMarkerOnDropLocation = 1;
            ShowAirdropTypeOnMarker = 1;
            HideCargoWhileParachuteIsDeployed = 1;
            HeightIsRelativeToGroundLevel = 1;
            Height = 450;
            FollowTerrainFraction = (decimal)0.5;
            Speed = 35;
            Radius = 1;
            InfectedSpawnRadius = 50;
            InfectedSpawnInterval = 250;
            ItemCount = 50;
            Containers = new BindingList<ExpansionLootContainer>();
            DefaultRegular();
            DefaultMedical();
            DefaultBaseBuilding();
            DefaultMilitary();
        }

        public bool checkver()
        {
            if (m_Version != CurrentVersion)
            {
                m_Version = CurrentVersion;
                isDirty = true;
                return true;
            }
            return false;
        }
        void DefaultRegular()
        {
            BindingList<ExpansionLoot> Loot = ExpansionLootDefaults.Airdrop_Regular();
            BindingList<string> Infected = new BindingList<string>() {
            "ZmbM_HermitSkinny_Beige",
            "ZmbM_HermitSkinny_Black",
            "ZmbM_HermitSkinny_Green",
            "ZmbM_HermitSkinny_Red",
            "ZmbM_FarmerFat_Beige",
            "ZmbM_FarmerFat_Blue",
            "ZmbM_FarmerFat_Brown",
            "ZmbM_FarmerFat_Green",
            "ZmbF_CitizenANormal_Beige",
            "ZmbF_CitizenANormal_Blue",
            "ZmbF_CitizenANormal_Brown",
            "ZmbM_CitizenBFat_Blue",
            "ZmbM_CitizenBFat_Red",
            "ZmbM_CitizenBFat_Green",
            "ZmbF_CitizenBSkinny",
            "ZmbM_FishermanOld_Blue",
            "ZmbM_FishermanOld_Green",
            "ZmbM_FishermanOld_Grey",
            "ZmbM_FishermanOld_Red",
            "ZmbM_JournalistSkinny",
            "ZmbF_JournalistNormal_Blue",
            "ZmbF_JournalistNormal_Green",
            "ZmbF_JournalistNormal_Red",
            "ZmbF_JournalistNormal_White",
            "ZmbM_HikerSkinny_Blue",
            "ZmbM_HikerSkinny_Green",
            "ZmbM_HikerSkinny_Yellow",
            "ZmbF_HikerSkinny_Blue",
            "ZmbF_HikerSkinny_Grey",
            "ZmbF_HikerSkinny_Green",
            "ZmbF_HikerSkinny_Red",
            "ZmbF_SurvivorNormal_Blue",
            "ZmbF_SurvivorNormal_Orange",
            "ZmbF_SurvivorNormal_Red",
            "ZmbF_SurvivorNormal_White",
            "ZmbM_CommercialPilotOld_Blue",
            "ZmbM_CommercialPilotOld_Olive",
            "ZmbM_CommercialPilotOld_Brown",
            "ZmbM_CommercialPilotOld_Grey",
            "ZmbM_JoggerSkinny_Blue",
            "ZmbM_JoggerSkinny_Green",
            "ZmbM_JoggerSkinny_Red",
            "ZmbF_JoggerSkinny_Blue",
            "ZmbF_JoggerSkinny_Brown",
            "ZmbF_JoggerSkinny_Green",
            "ZmbF_JoggerSkinny_Red",
            "ZmbM_MotobikerFat_Beige",
            "ZmbM_MotobikerFat_Black",
            "ZmbM_MotobikerFat_Blue",
            "ZmbM_VillagerOld_Blue",
            "ZmbM_VillagerOld_Green",
            "ZmbM_VillagerOld_White",
            "ZmbM_SkaterYoung_Blue",
            "ZmbM_SkaterYoung_Brown",
            "ZmbM_SkaterYoung_Green",
            "ZmbM_SkaterYoung_Grey",
            "ZmbF_SkaterYoung_Brown",
            "ZmbF_SkaterYoung_Striped",
            "ZmbF_SkaterYoung_Violet",
            "ZmbM_OffshoreWorker_Green",
            "ZmbM_OffshoreWorker_Orange",
            "ZmbM_OffshoreWorker_Red",
            "ZmbM_OffshoreWorker_Yellow",
            "ZmbM_Jacket_beige",
            "ZmbM_Jacket_black",
            "ZmbM_Jacket_blue",
            "ZmbM_Jacket_bluechecks",
            "ZmbM_Jacket_brown",
            "ZmbM_Jacket_greenchecks",
            "ZmbM_Jacket_grey",
            "ZmbM_Jacket_khaki",
            "ZmbM_Jacket_magenta",
            "ZmbM_Jacket_stripes",
            "ZmbF_ShortSkirt_beige",
            "ZmbF_ShortSkirt_black",
            "ZmbF_ShortSkirt_brown",
            "ZmbF_ShortSkirt_green",
            "ZmbF_ShortSkirt_grey",
            "ZmbF_ShortSkirt_checks",
            "ZmbF_ShortSkirt_red",
            "ZmbF_ShortSkirt_stripes",
            "ZmbF_ShortSkirt_white",
            "ZmbF_ShortSkirt_yellow",
            "ZmbF_VillagerOld_Blue",
            "ZmbF_VillagerOld_Green",
            "ZmbF_VillagerOld_Red",
            "ZmbF_VillagerOld_White",
            "ZmbF_MilkMaidOld_Beige",
            "ZmbF_MilkMaidOld_Black",
            "ZmbF_MilkMaidOld_Green",
            "ZmbF_MilkMaidOld_Grey",
        };

            int itemCount = 30;
            int infectedCount = 25;

            if (Containers != null)
                Containers.Add(new ExpansionLootContainer("ExpansionAirdropContainer", 0, 5, Loot, Infected, itemCount, infectedCount));
        }
        void DefaultMedical()
        {
            BindingList<ExpansionLoot> Loot = ExpansionLootDefaults.Airdrop_Medical();
            BindingList<string> Infected = new BindingList<string>(){
            "ZmbM_HermitSkinny_Beige",
            "ZmbM_HermitSkinny_Black",
            "ZmbM_HermitSkinny_Green",
            "ZmbM_HermitSkinny_Red",
            "ZmbM_FarmerFat_Beige",
            "ZmbM_FarmerFat_Blue",
            "ZmbM_FarmerFat_Brown",
            "ZmbM_FarmerFat_Green",
            "ZmbF_CitizenANormal_Beige",
            "ZmbF_CitizenANormal_Brown",
            "ZmbF_CitizenANormal_Blue",
            "ZmbM_CitizenBFat_Blue",
            "ZmbM_CitizenBFat_Red",
            "ZmbM_CitizenBFat_Green",
            "ZmbF_CitizenBSkinny",
            "ZmbM_FishermanOld_Blue",
            "ZmbM_FishermanOld_Green",
            "ZmbM_FishermanOld_Grey",
            "ZmbM_FishermanOld_Red",
            "ZmbM_JournalistSkinny",
            "ZmbF_JournalistNormal_Blue",
            "ZmbF_JournalistNormal_Green",
            "ZmbF_JournalistNormal_Red",
            "ZmbF_JournalistNormal_White",
            "ZmbM_HikerSkinny_Blue",
            "ZmbM_HikerSkinny_Green",
            "ZmbM_HikerSkinny_Yellow",
            "ZmbF_HikerSkinny_Blue",
            "ZmbF_HikerSkinny_Grey",
            "ZmbF_HikerSkinny_Green",
            "ZmbF_HikerSkinny_Red",
            "ZmbF_SurvivorNormal_Blue",
            "ZmbF_SurvivorNormal_Orange",
            "ZmbF_SurvivorNormal_Red",
            "ZmbF_SurvivorNormal_White",
            "ZmbM_CommercialPilotOld_Blue",
            "ZmbM_CommercialPilotOld_Olive",
            "ZmbM_CommercialPilotOld_Brown",
            "ZmbM_CommercialPilotOld_Grey",
            "ZmbM_JoggerSkinny_Blue",
            "ZmbM_JoggerSkinny_Green",
            "ZmbM_JoggerSkinny_Red",
            "ZmbF_JoggerSkinny_Blue",
            "ZmbF_JoggerSkinny_Brown",
            "ZmbF_JoggerSkinny_Green",
            "ZmbF_JoggerSkinny_Red",
            "ZmbM_MotobikerFat_Beige",
            "ZmbM_MotobikerFat_Black",
            "ZmbM_MotobikerFat_Blue",
            "ZmbM_VillagerOld_Blue",
            "ZmbM_VillagerOld_Green",
            "ZmbM_VillagerOld_White",
            "ZmbM_SkaterYoung_Blue",
            "ZmbM_SkaterYoung_Brown",
            "ZmbM_SkaterYoung_Green",
            "ZmbM_SkaterYoung_Grey",
            "ZmbF_SkaterYoung_Brown",
            "ZmbF_SkaterYoung_Striped",
            "ZmbF_SkaterYoung_Violet",
            "ZmbM_OffshoreWorker_Green",
            "ZmbM_OffshoreWorker_Orange",
            "ZmbM_OffshoreWorker_Red",
            "ZmbM_OffshoreWorker_Yellow",
            "ZmbM_Jacket_beige",
            "ZmbM_Jacket_black",
            "ZmbM_Jacket_blue",
            "ZmbM_Jacket_bluechecks",
            "ZmbM_Jacket_brown",
            "ZmbM_Jacket_greenchecks",
            "ZmbM_Jacket_grey",
            "ZmbM_Jacket_khaki",
            "ZmbM_Jacket_magenta",
            "ZmbM_Jacket_stripes",
            "ZmbF_ShortSkirt_beige",
            "ZmbF_ShortSkirt_black",
            "ZmbF_ShortSkirt_brown",
            "ZmbF_ShortSkirt_green",
            "ZmbF_ShortSkirt_grey",
            "ZmbF_ShortSkirt_checks",
            "ZmbF_ShortSkirt_red",
            "ZmbF_ShortSkirt_stripes",
            "ZmbF_ShortSkirt_white",
            "ZmbF_ShortSkirt_yellow",
            "ZmbF_VillagerOld_Blue",
            "ZmbF_VillagerOld_Green",
            "ZmbF_VillagerOld_Red",
            "ZmbF_VillagerOld_White",
            "ZmbF_MilkMaidOld_Beige",
            "ZmbF_MilkMaidOld_Black",
            "ZmbF_MilkMaidOld_Green",
            "ZmbF_MilkMaidOld_Grey",
            "ZmbF_DoctorSkinny",
            "ZmbF_NurseFat",
            "ZmbM_DoctorFat",
            "ZmbF_PatientOld",
            "ZmbM_PatientSkinny",
        };

            int itemCount = 25;
            int infectedCount = 15;

            if (Containers != null)
                Containers.Add(new ExpansionLootContainer("ExpansionAirdropContainer_Medical", 0, 10, Loot, Infected, itemCount, infectedCount));
        }
        void DefaultBaseBuilding()
        {
            BindingList<ExpansionLoot> Loot = ExpansionLootDefaults.Airdrop_BaseBuilding();
            BindingList<string> Infected = new BindingList<string>(){
            "ZmbM_HermitSkinny_Beige",
            "ZmbM_HermitSkinny_Black",
            "ZmbM_HermitSkinny_Green",
            "ZmbM_HermitSkinny_Red",
            "ZmbM_FarmerFat_Beige",
            "ZmbM_FarmerFat_Blue",
            "ZmbM_FarmerFat_Brown",
            "ZmbM_FarmerFat_Green",
            "ZmbF_CitizenANormal_Beige",
            "ZmbF_CitizenANormal_Brown",
            "ZmbF_CitizenANormal_Blue",
            "ZmbM_CitizenBFat_Blue",
            "ZmbM_CitizenBFat_Red",
            "ZmbM_CitizenBFat_Green",
            "ZmbF_CitizenBSkinny",
            "ZmbM_FishermanOld_Blue",
            "ZmbM_FishermanOld_Green",
            "ZmbM_FishermanOld_Grey",
            "ZmbM_FishermanOld_Red",
            "ZmbM_JournalistSkinny",
            "ZmbF_JournalistNormal_Blue",
            "ZmbF_JournalistNormal_Green",
            "ZmbF_JournalistNormal_Red",
            "ZmbF_JournalistNormal_White",
            "ZmbM_HikerSkinny_Blue",
            "ZmbM_HikerSkinny_Green",
            "ZmbM_HikerSkinny_Yellow",
            "ZmbF_HikerSkinny_Blue",
            "ZmbF_HikerSkinny_Grey",
            "ZmbF_HikerSkinny_Green",
            "ZmbF_HikerSkinny_Red",
            "ZmbF_SurvivorNormal_Blue",
            "ZmbF_SurvivorNormal_Orange",
            "ZmbF_SurvivorNormal_Red",
            "ZmbF_SurvivorNormal_White",
            "ZmbM_CommercialPilotOld_Blue",
            "ZmbM_CommercialPilotOld_Olive",
            "ZmbM_CommercialPilotOld_Brown",
            "ZmbM_CommercialPilotOld_Grey",
            "ZmbM_JoggerSkinny_Blue",
            "ZmbM_JoggerSkinny_Green",
            "ZmbM_JoggerSkinny_Red",
            "ZmbF_JoggerSkinny_Blue",
            "ZmbF_JoggerSkinny_Brown",
            "ZmbF_JoggerSkinny_Green",
            "ZmbF_JoggerSkinny_Red",
            "ZmbM_MotobikerFat_Beige",
            "ZmbM_MotobikerFat_Black",
            "ZmbM_MotobikerFat_Blue",
            "ZmbM_VillagerOld_Blue",
            "ZmbM_VillagerOld_Green",
            "ZmbM_VillagerOld_White",
            "ZmbM_SkaterYoung_Blue",
            "ZmbM_SkaterYoung_Brown",
            "ZmbM_SkaterYoung_Green",
            "ZmbM_SkaterYoung_Grey",
            "ZmbF_SkaterYoung_Brown",
            "ZmbF_SkaterYoung_Striped",
            "ZmbF_SkaterYoung_Violet",
            "ZmbM_OffshoreWorker_Green",
            "ZmbM_OffshoreWorker_Orange",
            "ZmbM_OffshoreWorker_Red",
            "ZmbM_OffshoreWorker_Yellow",
            "ZmbM_Jacket_beige",
            "ZmbM_Jacket_black",
            "ZmbM_Jacket_blue",
            "ZmbM_Jacket_bluechecks",
            "ZmbM_Jacket_brown",
            "ZmbM_Jacket_greenchecks",
            "ZmbM_Jacket_grey",
            "ZmbM_Jacket_khaki",
            "ZmbM_Jacket_magenta",
            "ZmbM_Jacket_stripes",
            "ZmbF_ShortSkirt_beige",
            "ZmbF_ShortSkirt_black",
            "ZmbF_ShortSkirt_brown",
            "ZmbF_ShortSkirt_green",
            "ZmbF_ShortSkirt_grey",
            "ZmbF_ShortSkirt_checks",
            "ZmbF_ShortSkirt_red",
            "ZmbF_ShortSkirt_stripes",
            "ZmbF_ShortSkirt_white",
            "ZmbF_ShortSkirt_yellow",
            "ZmbF_VillagerOld_Blue",
            "ZmbF_VillagerOld_Green",
            "ZmbF_VillagerOld_Red",
            "ZmbF_VillagerOld_White",
            "ZmbF_MilkMaidOld_Beige",
            "ZmbF_MilkMaidOld_Black",
            "ZmbF_MilkMaidOld_Green",
            "ZmbF_MilkMaidOld_Grey",
            "ZmbF_BlueCollarFat_Blue",
            "ZmbF_BlueCollarFat_Green",
            "ZmbF_BlueCollarFat_Red",
            "ZmbF_BlueCollarFat_White",
            "ZmbF_MechanicNormal_Beige",
            "ZmbF_MechanicNormal_Green",
            "ZmbF_MechanicNormal_Grey",
            "ZmbF_MechanicNormal_Orange",
            "ZmbM_MechanicSkinny_Blue",
            "ZmbM_MechanicSkinny_Grey",
            "ZmbM_MechanicSkinny_Green",
            "ZmbM_MechanicSkinny_Red",
            "ZmbM_ConstrWorkerNormal_Beige",
            "ZmbM_ConstrWorkerNormal_Black",
            "ZmbM_ConstrWorkerNormal_Green",
            "ZmbM_ConstrWorkerNormal_Grey",
            "ZmbM_HeavyIndustryWorker",
        };

            int itemCount = 50;
            int infectedCount = 10;

            if (Containers != null)
                Containers.Add(new ExpansionLootContainer("ExpansionAirdropContainer_Basebuilding", 0, 15, Loot, Infected, itemCount, infectedCount));
        }
        void DefaultMilitary()
        {
            BindingList<ExpansionLoot> Loot = ExpansionLootDefaults.Airdrop_Military();
            BindingList<string> Infected = new BindingList<string>(){
            "ZmbM_usSoldier_normal_Woodland",
            "ZmbM_SoldierNormal",
            "ZmbM_usSoldier_normal_Desert",
            "ZmbM_PatrolNormal_PautRev",
            "ZmbM_PatrolNormal_Autumn",
            "ZmbM_PatrolNormal_Flat",
            "ZmbM_PatrolNormal_Summer",
        };

            int itemCount = 50;
            int infectedCount = 50;

            if (Containers != null)
            {
                Containers.Add(new ExpansionLootContainer("ExpansionAirdropContainer_Military", 0, 20, Loot, Infected, itemCount, infectedCount));
            }
        }

    }
    public class ExpansionLootContainer
    {
        public string Container { get; set; }
        public decimal FallSpeed { get; set; }
        public int Usage { get; set; }
        public decimal Weight { get; set; }
        public BindingList<string> Infected { get; set; }
        public int ItemCount { get; set; }
        public int InfectedCount { get; set; }
        public int SpawnInfectedForPlayerCalledDrops { get; set; }
        public BindingList<ExpansionLoot> Loot { get; set; }

        public ExpansionLootContainer(string container, int usage, decimal weight, BindingList<ExpansionLoot> loot, BindingList<string> infected, int itemCount, int infectedCount, bool spawnInfectedForPlayerCalledDrops = false, decimal fallSpeed = (decimal)4.5)
        {
            Container = container;
            Usage = usage;
            Weight = weight;
            Loot = loot;
            Infected = infected;
            ItemCount = itemCount;
            InfectedCount = infectedCount;
            SpawnInfectedForPlayerCalledDrops = spawnInfectedForPlayerCalledDrops == true ? 1 : 0;
            FallSpeed = fallSpeed;
        }

        public ExpansionLootContainer()
        {
            Container = "ExpansionAirdropContainer";
            FallSpeed = (decimal)4.5;
            Usage = 0;
            Weight = 0;
            Loot = new BindingList<ExpansionLoot>();
            Infected = new BindingList<string>();
            ItemCount = 0;
            InfectedCount = 0;
            SpawnInfectedForPlayerCalledDrops = 0;
        }

        public override string ToString()
        {
            return Container;
        }
    }
    public class ExpansionLoot
    {
        public string Name { get; set; }
        public BindingList<ExpansionLootVariant> Attachments { get; set; }
        public decimal Chance { get; set; }
        public int QuantityPercent { get; set; }
        public int Max { get; set; }
        public int Min { get; set; }
        public BindingList<ExpansionLootVariant> Variants { get; set; }

        public ExpansionLoot(string name, BindingList<ExpansionLootVariant> attachments = null, decimal chance = 1, int quantityPercent = -1, BindingList<ExpansionLootVariant> variants = null, int max = -1, int min = 0)
        {
            Name = name;
            if (attachments != null)
                Attachments = attachments;
            else
                Attachments = new BindingList<ExpansionLootVariant>();
            Chance = chance;
            if (variants == null)
                Variants = new BindingList<ExpansionLootVariant>();
            else
                Variants = variants;

            QuantityPercent = quantityPercent;
            Max = max;
            Min = min;
        }
        public ExpansionLoot()
        {
            Chance = (decimal)0.25;
            QuantityPercent = -1;
            Max = -1;
            Min = 0;
            Attachments = new BindingList<ExpansionLootVariant>();
            Variants = new BindingList<ExpansionLootVariant>();
        }
        public override string ToString()
        {
            return Name;
        }
    }
    public class ExpansionLootVariant
    {
        public string Name { get; set; }
        public BindingList<ExpansionLootVariant> Attachments { get; set; }
        public decimal Chance { get; set; }

        public ExpansionLootVariant(string _name, BindingList<ExpansionLootVariant> _Attachments = null, decimal _Chance = 1)
        {
            Name = _name;
            if (_Attachments != null)
                Attachments = _Attachments;
            else
                Attachments = new BindingList<ExpansionLootVariant>();
            Chance = _Chance;
        }
        public ExpansionLootVariant()
        {
            Chance = (decimal)0.2;
            Attachments = new BindingList<ExpansionLootVariant>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
    public enum ContainerTypes
    {
        ExpansionAirdropContainer,
        ExpansionAirdropContainer_Medical,
        ExpansionAirdropContainer_Military,
        ExpansionAirdropContainer_Basebuilding,
        ExpansionAirdropContainer_Grey,
        ExpansionAirdropContainer_Blue,
        ExpansionAirdropContainer_Olive,
        ExpansionAirdropContainer_Military_GreenCamo,
        ExpansionAirdropContainer_Military_MarineCamo,
        ExpansionAirdropContainer_Military_OliveCamo,
        ExpansionAirdropContainer_Military_OliveCamo2,
        ExpansionAirdropContainer_Military_WinterCamo
    };
}
