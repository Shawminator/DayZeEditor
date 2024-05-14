using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class ExpansionSpawnSettings
    {
        const int CurrentVersion = 7;

        public int m_Version { get; set; }
        public BindingList<ExpansionSpawnLocation> SpawnLocations { get; set; }
        public ExpansionStartingClothing StartingClothing { get; set; }
        public int EnableSpawnSelection { get; set; }
        public int SpawnOnTerritory { get; set; }
        public ExpansionStartingGear StartingGear { get; set; }
        public int UseLoadouts { get; set; }
        public BindingList<ExpansionSpawnGearLoadouts> MaleLoadouts { get; set; }
        public BindingList<ExpansionSpawnGearLoadouts> FemaleLoadouts { get; set; }
        public decimal SpawnHealthValue { get; set; }
        public decimal SpawnEnergyValue { get; set; }
        public decimal SpawnWaterValue { get; set; }
        public int EnableRespawnCooldowns { get; set; }
        public int RespawnCooldown { get; set; }
        public int TerritoryRespawnCooldown { get; set; }
        public int PunishMultispawn { get; set; }
        public int PunishCooldown { get; set; }
        public int PunishTimeframe { get; set; }
        public int CreateDeathMarker { get; set; }
        public string BackgroundImagePath { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty
        {
            get { return _isdirty; }
            set
            {
                _isdirty = value;
            }
        }
        [JsonIgnore]
        private bool _isdirty = false;

        public ExpansionSpawnSettings()
        {
            m_Version = CurrentVersion;
            StartingGear = new ExpansionStartingGear();
            StartingClothing = new ExpansionStartingClothing();

            UseLoadouts = 1;
            MaleLoadouts = new BindingList<ExpansionSpawnGearLoadouts>();
            MaleLoadouts.Add(new ExpansionSpawnGearLoadouts("PlayerSurvivorLoadout", (decimal)1.0));
            FemaleLoadouts = new BindingList<ExpansionSpawnGearLoadouts>();
            FemaleLoadouts.Add(new ExpansionSpawnGearLoadouts("PlayerSurvivorLoadout", (decimal)1.0));

            EnableSpawnSelection = 1;       //! Will be enabled if the map have a configured spawn location on generation

            SpawnHealthValue = (decimal)100.0;   //! 100 is max
            SpawnEnergyValue = (decimal)500.0;   //! 7500 is max
            SpawnWaterValue = (decimal)500.0;    //! 5000 is max

            EnableRespawnCooldowns = 1; //! Enable cooldown system for the spawn selection menu
            RespawnCooldown = 120; //! Respawn delay time in seconds
            TerritoryRespawnCooldown = 240; //! Respawn delay time for territories in seconds
            PunishMultispawn = 1; //! If player uses the same spawn point twice or more then punish the player with additonal cooldown time
            PunishCooldown = 120; // ! If "PunishMultispawn" is enabled and a player uses the same spawn point twice or more then punish the player with additonal cooldown time that is set here.
            PunishTimeframe = 300; //! If "PunishMultispawn" is enabled and a player respawns twice or more on the same spawn point then he will get a additonal cooldown punishment set in the "PunishCooldown" setting. This setting here will mark the timeframe for when the player gets this punishment or not.
            CreateDeathMarker = 1; //! Create a marker on the spawn selection map on the players last position where the player died.
            BackgroundImagePath = "DayZExpansion/SpawnSelection/GUI/textures/wood_background.edds";

            ExpansionSpawnsChernarus();
        }
        public void ExpansionSpawnsChernarus()
        {
            SpawnLocations = new BindingList<ExpansionSpawnLocation>()
            {
                new ExpansionSpawnLocation()
                {
                    Name = "Svetloyarsk",
                    UseCooldown = 1,
                    Positions = new BindingList<float[]>()
                    {
                        new float[] { 14273.2f, 2.4f, 13053.3f },
                        new float[] {14407.3f, 2.0f, 13253.0f },
                        new float[] { 14142.4f, 3.3f, 13290.2f },
                        new float[] { 13910.9f, 4.3f, 13624.9f }
                    }
                },
                new ExpansionSpawnLocation()
                {
                    Name = "Berezino",
                    UseCooldown = 1,
                    Positions = new BindingList<float[]>()
                    {
                        new float[] {
                            12915.7001953125f,
                            3.4000000953674318f,
                            9278.2001953125f
                        },
                        new float[]{
                            13057.2001953125f,
                            2.299999952316284f,
                            9584.48046875f
                        },
                        new float[]{
                            13052.900390625f,
                            6.099999904632568f,
                            9894.7001953125f
                        },
                        new float[]{
                            13207.2001953125f,
                            2.299999952316284f,
                            10193.7001953125f
                        }
                    }
                },
                new ExpansionSpawnLocation()
                {
                    Name = "Solnich",
                    UseCooldown = 1,
                    Positions = new BindingList<float[]>()
                    {
                        new float[]  {
                            13169.5f,
                            3.0756099224090578f,
                            7504.02978515625f
                        },
                        new float[]  {
                            13274.0f,
                            1.7834999561309815f,
                            7258.81005859375f
                        },
                        new float[]   {
                            13345.599609375f,
                            1.8779300451278687f,
                            6987.35986328125f
                        },
                        new float[]  {
                            13383.0f,
                            2.755160093307495f,
                            6815.89013671875f
                        }
                    }
                },
                new ExpansionSpawnLocation()
                {
                    Name = "Solnichniy",
                    UseCooldown = 1,
                    Positions = new BindingList<float[]>()
                    {
                        new float[]  {
                            13529.5302734375f,
                            2.251228094100952f,
                            6455.61279296875f
                        },
                        new float[] {
                            13484.724609375f,
                            1.7466460466384888f,
                            5911.0947265625f
                        },
                        new float[]  {
                            13515.912109375f,
                            2.679647922515869f,
                            6117.38427734375f
                        },
                        new float[] {
                            13534.671875f,
                            1.6446690559387208f,
                            6234.75f
                        }
                    }
                },
                new ExpansionSpawnLocation()
                {
                    Name = "Kamyshovo",
                    UseCooldown = 1,
                    Positions = new BindingList<float[]>()
                    {
                        new float[]  {
                            12321.939453125f,
                            1.9261399507522584f,
                            3446.666748046875f
                        },
                        new float[]  {
                            12188.5703125f,
                            1.7272900342941285f,
                            3422.332275390625f
                        },
                        new float[]  {
                            11992.25f,
                            1.9820810556411744f,
                            3404.554443359375f
                        },
                        new float[]  {
                            11859.34375f,
                            1.901515007019043f,
                            3367.71484375f
                        }
                    }
                },
                new ExpansionSpawnLocation()
                {
                    Name = "Elektrozavodsk",
                    UseCooldown = 1,
                    Positions = new BindingList<float[]>()
                    {
                        new float[]  {
                            11099.068359375f,
                            2.2976760864257814f,
                            2735.5625f
                        },
                        new float[] {
                            10858.4130859375f,
                            2.9117209911346437f,
                            2328.290283203125f
                        },
                        new float[] {
                            10490.9140625f,
                            1.8469020128250123f,
                            1950.1484375f
                        },
                        new float[] {
                            9826.8857421875f,
                            1.7118209600448609f,
                            1757.3746337890626f
                        },
                        new float[] {
                            9428.4580078125f,
                            2.25445294380188f,
                            1826.218505859375f
                        },
                        new float[] {
                            9153.5361328125f,
                            3.421117067337036f,
                            1914.3006591796876f
                        }
                    }
                },
                new ExpansionSpawnLocation()
                {
                    Name = "Chernogorsk",
                    UseCooldown = 1,
                    Positions = new BindingList<float[]>()
                    {
                        new float[] { 6044.10302734375f, 6.429995059967041f,1871.5006103515626f },
                        new float[] {6220.4404296875f, 1.917814016342163f, 2101.123291015625f },
                        new float[] { 7118.1220703125f, 1.8241829872131348f, 2533.971923828125f },
                        new float[] { 7419.4970703125f, 1.7683860063552857f, 2576.50390625f },
                        new float[] {   8139.25f,  1.1517109870910645f,2802.3564453125f }
                    }
                },
                new ExpansionSpawnLocation()
                {
                    Name = "Balota",
                    UseCooldown = 1,
                    Positions = new BindingList<float[]>()
                    {
                        new float[] { 4654.59423828125f, 1.4597179889678956f, 2132.86669921875f },
                        new float[] {4543.99072265625f,1.9016389846801758f, 2198.166259765625f },
                        new float[] { 4269.4228515625f, 1.28923499584198f, 2245.660888671875f },
                        new float[] { 4111.90576171875f, 1.566264033317566f,2193.9326171875f }
                    }
                },
                new ExpansionSpawnLocation()
                {
                    Name = "Komarovo",
                    UseCooldown = 1,
                    Positions = new BindingList<float[]>()
                    {
                        new float[] { 3887.55908203125f,1.5955090522766114f,2207.158935546875f },
                        new float[] {3746.65576171875f, 2.4453859329223635f, 2199.878173828125f },
                        new float[] { 3507.4228515625f, 2.0086090564727785f, 2101.45458984375f },
                        new float[] { 3366.9853515625f, 1.902521014213562f, 2002.4140625f }
                    }
                },
                new ExpansionSpawnLocation()
                {
                    Name = "Kamenka",
                    UseCooldown = 1,
                    Positions = new BindingList<float[]>()
                    {
                        new float[] { 2164.7001953125f,1.7280139923095704f,2049.44384765625f },
                        new float[] {2031.4254150390626f,1.2907429933547974f,2150.743408203125f },
                        new float[] { 1708.5230712890626f,1.9583090543746949f,2031.263671875f },
                        new float[] { 1563.32568359375f,2.1741321086883547f,2063.2548828125f }
                    }
                }
            };
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
        public void SetStartingWeapons()
        {

            if (StartingGear.PrimaryWeapon != null && StartingGear.PrimaryWeapon.ClassName == null)
                StartingGear.PrimaryWeapon = null;
            if (StartingGear.SecondaryWeapon != null && StartingGear.SecondaryWeapon.ClassName == null)
                StartingGear.SecondaryWeapon = null;
        }
    }
    public class ExpansionStartingClothing
    {
        public int EnableCustomClothing { get; set; }
        public int SetRandomHealth { get; set; }
        public BindingList<string> Headgear { get; set; }
        public BindingList<string> Glasses { get; set; }
        public BindingList<string> Masks { get; set; }
        public BindingList<string> Tops { get; set; }
        public BindingList<string> Vests { get; set; }
        public BindingList<string> Gloves { get; set; }
        public BindingList<string> Pants { get; set; }
        public BindingList<string> Belts { get; set; }
        public BindingList<string> Shoes { get; set; }
        public BindingList<string> Armbands { get; set; }
        public BindingList<string> Backpacks { get; set; }

        public ExpansionStartingClothing()
        {
            EnableCustomClothing = 1;
            SetRandomHealth = 1;
            Headgear = new BindingList<string>();
            Glasses = new BindingList<string>();
            Masks = new BindingList<string>();
            Tops = new BindingList<string>();
            Vests = new BindingList<string>();
            Gloves = new BindingList<string>();
            Pants = new BindingList<string>();
            Belts = new BindingList<string>();
            Shoes = new BindingList<string>();
            Armbands = new BindingList<string>();
            Backpacks = new BindingList<string>();

            Tops.Add("TShirt_Green");
            Tops.Add("TShirt_Blue");
            Tops.Add("TShirt_Black");
            Tops.Add("TShirt_Beige");
            Tops.Add("TShirt_Red");
            Tops.Add("TShirt_OrangeWhiteStripes");
            Tops.Add("TShirt_White");
            Tops.Add("TShirt_Red");
            Tops.Add("TShirt_Grey");
            Tops.Add("TShirt_RedBlackStripes");

            Pants.Add("CanvasPants_Beige");
            Pants.Add("CanvasPants_Blue");
            Pants.Add("CanvasPants_Grey");
            Pants.Add("CanvasPants_Red");
            Pants.Add("CanvasPants_Violet");
            Pants.Add("CanvasPantsMidi_Beige");
            Pants.Add("CanvasPantsMidi_Blue");
            Pants.Add("CanvasPantsMidi_Grey");
            Pants.Add("CanvasPantsMidi_Red");
            Pants.Add("CanvasPantsMidi_Violet");

            Shoes.Add("AthleticShoes_Blue");
            Shoes.Add("AthleticShoes_Grey");
            Shoes.Add("AthleticShoes_Brown");
            Shoes.Add("AthleticShoes_Green");
            Shoes.Add("AthleticShoes_Black");

            Backpacks.Add("TaloonBag_Blue");
            Backpacks.Add("TaloonBag_Green");
            Backpacks.Add("TaloonBag_Orange");
            Backpacks.Add("TaloonBag_Violet");
        }
    }
    public class ExpansionSpawnLocation
    {
        public string Name { get; set; }
        public BindingList<float[]> Positions { get; set; }
        public int UseCooldown { get; set; }

        public ExpansionSpawnLocation()
        {
            Name = "New SpawnLocation";
            Positions = new BindingList<float[]>();

        }
        public override string ToString()
        {
            return Name;
        }
    }
    public class ExpansionStartingGear
    {
        public int EnableStartingGear { get; set; }
        public int UseUpperGear { get; set; }
        public int UsePantsGear { get; set; }
        public int UseBackpackGear { get; set; }
        public int UseVestGear { get; set; }
        public int UsePrimaryWeapon { get; set; }
        public int UseSecondaryWeapon { get; set; }
        public int ApplyEnergySources { get; set; }
        public int SetRandomHealth { get; set; }
        public BindingList<ExpansionStartingGearItem> UpperGear { get; set; }
        public BindingList<ExpansionStartingGearItem> PantsGear { get; set; }
        public BindingList<ExpansionStartingGearItem> BackpackGear { get; set; }
        public BindingList<ExpansionStartingGearItem> VestGear { get; set; }
        public ExpansionStartingGearItem PrimaryWeapon { get; set; }
        public ExpansionStartingGearItem SecondaryWeapon { get; set; }

        public ExpansionStartingGear()
        {
            EnableStartingGear = 1;

            ApplyEnergySources = 1;
            SetRandomHealth = 1;

            UpperGear = new BindingList<ExpansionStartingGearItem>();
            PantsGear = new BindingList<ExpansionStartingGearItem>();
            BackpackGear = new BindingList<ExpansionStartingGearItem>();
            VestGear = new BindingList<ExpansionStartingGearItem>();
            PrimaryWeapon = null;
            SecondaryWeapon = null;
            UpperGear.Add(new ExpansionStartingGearItem("Rag", 4));
            UpperGear.Add(new ExpansionStartingGearItem("Apple"));

        }
    }
    public class ExpansionStartingGearItem : EmptyGear
    {
        public string ClassName { get; set; }
        public int Quantity { get; set; }
        public BindingList<string> Attachments { get; set; }

        public ExpansionStartingGearItem()
        {

        }
        public ExpansionStartingGearItem(string className, int quantity = -1, List<string> attachments = null)
        {
            ClassName = className;
            Quantity = quantity;

            if (attachments == null)
            {
                Attachments = new BindingList<string>();
            }
            else
            {
                Attachments = new BindingList<string>(attachments); ;
            }
        }

        public override string ToString()
        {
            return ClassName;
        }
    }
    public class EmptyGear
    {
    }
    public class ExpansionSpawnGearLoadouts
    {
        public string Loadout { get; set; }
        public decimal Chance { get; set; }

        public ExpansionSpawnGearLoadouts() { }
        public ExpansionSpawnGearLoadouts(string loadout, decimal chance)
        {
            Loadout = loadout;
            Chance = chance;
        }
        public override string ToString()
        {
            return Loadout;
        }
    }
}
