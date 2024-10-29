using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class P2PMarketSettings
    {
        public const int CurrentVersion = 3;

        public int m_Version { get; set; }
        public int Enabled { get; set; }
        public int MaxListingTime { get; set; }
        public int MaxListings { get; set; }
        public int ListingOwnerDiscountPercent { get; set; }
        public int ListingPricePercent { get; set; }
        public BindingList<string> ExcludedClassNames { get; set; }
        public BindingList<ExpansionMenuCategory> MenuCategories { get; set; }
        public int SalesDepositTime { get; set; }
        public int DisallowUnpersisted { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty = false;

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
        public P2PMarketSettings()
        {
            isDirty = true;
            Enabled = 1;
            MaxListingTime = 691200; //! 8 days by default.
            MaxListings = 20; //! Max. listings a player can create.
            ListingOwnerDiscountPercent = 70; //! 70% discount on the listed item price for the item owner.
            ListingPricePercent = 30; //! 30% of the given listing price need to be paied to list a item.
            SalesDepositTime = 691200; //! 8 days by default.

            ExcludedClassNames = new BindingList<string>();

            MenuCategories = new BindingList<ExpansionMenuCategory>();
        }
        public P2PMarketSettings(string name)
        {
            isDirty = true;
            Filename = name;
            m_Version = CurrentVersion;
            Enabled = 1;
            MaxListingTime = 691200;
            MaxListings = 20;
            ListingOwnerDiscountPercent = 70;
            ListingPricePercent = 30;
            SalesDepositTime = 691200;
            DisallowUnpersisted = 0;
            ExcludedClassNames = new BindingList<string>();
            MenuCategories = new BindingList<ExpansionMenuCategory>();

        }

        public void DefaultMenuCategories()
        {
            //! Ammo
            ExpansionMenuCategory cat_Ammo = new ExpansionMenuCategory();
            cat_Ammo.SetDisplayName("Ammo");
            cat_Ammo.SetIconPath("set:dayz_inventory image:bullets");
            cat_Ammo.AddIncluded("Ammunition_Base");
            cat_Ammo.AddIncluded("Box_Base");
            MenuCategories.Add(cat_Ammo);

            //! Ammo - Ammo-Boxes
            ExpansionMenuSubCategory sub_Cat_Ammo_Boxes = new ExpansionMenuSubCategory();
            sub_Cat_Ammo_Boxes.SetDisplayName("Ammo-Boxes");
            sub_Cat_Ammo_Boxes.SetIconPath("set:dayz_inventory image:bullets");
            sub_Cat_Ammo_Boxes.AddIncluded("Box_Base");
            cat_Ammo.SubCategories.Add(sub_Cat_Ammo_Boxes);

            //! Ammo - Bullets
            ExpansionMenuSubCategory sub_Cat_Ammo_Bullets = new ExpansionMenuSubCategory();
            sub_Cat_Ammo_Bullets.SetDisplayName("Ammo-Bullets");
            sub_Cat_Ammo_Bullets.SetIconPath("set:dayz_inventory image:bullets");
            sub_Cat_Ammo_Bullets.AddIncluded("Ammunition_Base");
            cat_Ammo.SubCategories.Add(sub_Cat_Ammo_Bullets);

            //! Containers
            ExpansionMenuCategory cat_Containers = new ExpansionMenuCategory();
            cat_Containers.SetDisplayName("Containers");
            cat_Containers.SetIconPath("set:dayz_inventory image:barrel");
            cat_Containers.AddIncluded("Container_Base");
            MenuCategories.Add(cat_Containers);

            //! Consumables
            ExpansionMenuCategory cat_Consumables = new ExpansionMenuCategory();
            cat_Consumables.SetDisplayName("Consumables");
            cat_Consumables.SetIconPath("set:dayz_inventory image:food");
            cat_Consumables.AddIncluded("Edible_Base");
            MenuCategories.Add(cat_Consumables);

            //! Consumables - Drinks
            ExpansionMenuSubCategory sub_Cat_Consumables_Drinks = new ExpansionMenuSubCategory();
            sub_Cat_Consumables_Drinks.SetDisplayName("Drinks");
            sub_Cat_Consumables_Drinks.SetIconPath("set:dayz_inventory image:canteen");
            sub_Cat_Consumables_Drinks.AddIncluded("SodaCan_ColorBase");
            sub_Cat_Consumables_Drinks.AddIncluded("WaterBottle");
            sub_Cat_Consumables_Drinks.AddIncluded("Canteen");
            sub_Cat_Consumables_Drinks.AddIncluded("Vodka");
            cat_Consumables.SubCategories.Add(sub_Cat_Consumables_Drinks);

            //! Consumables - Fish
            ExpansionMenuSubCategory sub_Cat_Consumables_Fish = new ExpansionMenuSubCategory();
            sub_Cat_Consumables_Fish.SetDisplayName("Fish");
            sub_Cat_Consumables_Fish.SetIconPath("set:dayz_inventory image:food");
            sub_Cat_Consumables_Fish.AddIncluded("CarpFilletMeat");
            sub_Cat_Consumables_Fish.AddIncluded("MackerelFilletMeat");
            sub_Cat_Consumables_Fish.AddIncluded("Carp");
            sub_Cat_Consumables_Fish.AddIncluded("Sardines");
            sub_Cat_Consumables_Fish.AddIncluded("Mackerel");
            cat_Consumables.SubCategories.Add(sub_Cat_Consumables_Fish);

            //! Consumables - Food
            ExpansionMenuSubCategory sub_Cat_Consumables_Food = new ExpansionMenuSubCategory();
            sub_Cat_Consumables_Food.SetDisplayName("Food");
            sub_Cat_Consumables_Food.SetIconPath("set:dayz_inventory image:food");
            sub_Cat_Consumables_Food.AddIncluded("Zagorky");
            sub_Cat_Consumables_Food.AddIncluded("ZagorkyChocolate");
            sub_Cat_Consumables_Food.AddIncluded("ZagorkyPeanuts");
            sub_Cat_Consumables_Food.AddIncluded("PowderedMilk");
            sub_Cat_Consumables_Food.AddIncluded("BoxCerealCrunchin");
            sub_Cat_Consumables_Food.AddIncluded("Rice");
            sub_Cat_Consumables_Food.AddIncluded("Marmalade");
            sub_Cat_Consumables_Food.AddIncluded("Honey");
            sub_Cat_Consumables_Food.AddIncluded("SaltySticks");
            sub_Cat_Consumables_Food.AddIncluded("Crackers");
            sub_Cat_Consumables_Food.AddIncluded("Chips");
            sub_Cat_Consumables_Food.AddIncluded("Pajka");
            sub_Cat_Consumables_Food.AddIncluded("Pate");
            sub_Cat_Consumables_Food.AddIncluded("BrisketSpread");
            sub_Cat_Consumables_Food.AddIncluded("SardinesCan");
            sub_Cat_Consumables_Food.AddIncluded("TunaCan");
            sub_Cat_Consumables_Food.AddIncluded("DogFoodCan");
            sub_Cat_Consumables_Food.AddIncluded("CatFoodCan");
            sub_Cat_Consumables_Food.AddIncluded("PorkCan");
            sub_Cat_Consumables_Food.AddIncluded("Lunchmeat");
            sub_Cat_Consumables_Food.AddIncluded("UnknownFoodCan");
            sub_Cat_Consumables_Food.AddIncluded("PeachesCan");
            sub_Cat_Consumables_Food.AddIncluded("SpaghettiCan");
            sub_Cat_Consumables_Food.AddIncluded("BakedBeansCan");
            sub_Cat_Consumables_Food.AddIncluded("TacticalBaconCan");
            cat_Consumables.SubCategories.Add(sub_Cat_Consumables_Food);

            //! Consumables - Meat
            ExpansionMenuSubCategory sub_Cat_Consumables_Meat = new ExpansionMenuSubCategory();
            sub_Cat_Consumables_Meat.SetDisplayName("Meat");
            sub_Cat_Consumables_Meat.SetIconPath("set:dayz_inventory image:food");
            sub_Cat_Consumables_Meat.AddIncluded("BearSteakMeat");
            sub_Cat_Consumables_Meat.AddIncluded("GoatSteakMeat");
            sub_Cat_Consumables_Meat.AddIncluded("BoarSteakMeat");
            sub_Cat_Consumables_Meat.AddIncluded("PigSteakMeat");
            sub_Cat_Consumables_Meat.AddIncluded("DeerSteakMeat");
            sub_Cat_Consumables_Meat.AddIncluded("WolfSteakMeat");
            sub_Cat_Consumables_Meat.AddIncluded("CowSteakMeat");
            sub_Cat_Consumables_Meat.AddIncluded("SheepSteakMeat");
            sub_Cat_Consumables_Meat.AddIncluded("ChickenBreastMeat");
            sub_Cat_Consumables_Meat.AddIncluded("RabbitLegMeat");
            cat_Consumables.SubCategories.Add(sub_Cat_Consumables_Meat);

            //! Consumables - Meds
            ExpansionMenuSubCategory sub_Cat_Consumables_Meds = new ExpansionMenuSubCategory();
            sub_Cat_Consumables_Meds.SetDisplayName("Meds");
            sub_Cat_Consumables_Meds.SetIconPath("set:dayz_inventory image:medicalbandage");
            sub_Cat_Consumables_Meds.AddIncluded("CharcoalTablets");
            sub_Cat_Consumables_Meds.AddIncluded("DisinfectantAlcohol");
            sub_Cat_Consumables_Meds.AddIncluded("PurificationTablets");
            sub_Cat_Consumables_Meds.AddIncluded("VitaminBottle");
            sub_Cat_Consumables_Meds.AddIncluded("DisinfectantSpray");
            sub_Cat_Consumables_Meds.AddIncluded("TetracyclineAntibiotics");
            sub_Cat_Consumables_Meds.AddIncluded("PainkillerTablets");
            sub_Cat_Consumables_Meds.AddIncluded("IodineTincture");
            cat_Consumables.SubCategories.Add(sub_Cat_Consumables_Meds);

            //! Consumables - Vegetables
            ExpansionMenuSubCategory sub_Cat_Consumables_Vegetables = new ExpansionMenuSubCategory();
            sub_Cat_Consumables_Vegetables.SetDisplayName("Vegetables");
            sub_Cat_Consumables_Vegetables.SetIconPath("set:dayz_inventory image:food");
            sub_Cat_Consumables_Vegetables.AddIncluded("Apple");
            sub_Cat_Consumables_Vegetables.AddIncluded("GreenBellPepper");
            sub_Cat_Consumables_Vegetables.AddIncluded("Zucchini");
            sub_Cat_Consumables_Vegetables.AddIncluded("Pumpkin");
            sub_Cat_Consumables_Vegetables.AddIncluded("SlicedPumpkin");
            sub_Cat_Consumables_Vegetables.AddIncluded("PotatoSeed");
            sub_Cat_Consumables_Vegetables.AddIncluded("Potato");
            sub_Cat_Consumables_Vegetables.AddIncluded("Tomato");
            sub_Cat_Consumables_Vegetables.AddIncluded("SambucusBerry");
            sub_Cat_Consumables_Vegetables.AddIncluded("CaninaBerry");
            sub_Cat_Consumables_Vegetables.AddIncluded("Plum");
            sub_Cat_Consumables_Vegetables.AddIncluded("Pear");
            sub_Cat_Consumables_Vegetables.AddIncluded("MushroomBase");
            cat_Consumables.SubCategories.Add(sub_Cat_Consumables_Vegetables);

            //! Equipment
            ExpansionMenuCategory cat_Equipment = new ExpansionMenuCategory();
            cat_Equipment.SetDisplayName("Equipment");
            cat_Equipment.SetIconPath("set:dayz_inventory image:body");
            cat_Equipment.AddIncluded("ClothingBase");
            cat_Equipment.AddIncluded("Clothing_Base");
            MenuCategories.Add(cat_Equipment);

            //! Equipment - Armbands
            ExpansionMenuSubCategory sub_Cat_Equipment_Armbands = new ExpansionMenuSubCategory();
            sub_Cat_Equipment_Armbands.SetDisplayName("Armbands");
            sub_Cat_Equipment_Armbands.SetIconPath("set:dayz_inventory image:armband");
            sub_Cat_Equipment_Armbands.AddIncluded("Armband_ColorBase");
            cat_Equipment.SubCategories.Add(sub_Cat_Equipment_Armbands);

            //! Equipment - Backpacks
            ExpansionMenuSubCategory sub_Cat_Equipment_Backpacks = new ExpansionMenuSubCategory();
            sub_Cat_Equipment_Backpacks.SetDisplayName("Backpacks");
            sub_Cat_Equipment_Backpacks.SetIconPath("set:dayz_inventory image:back");
            sub_Cat_Equipment_Backpacks.AddIncluded("Backpack_Base");
            cat_Equipment.SubCategories.Add(sub_Cat_Equipment_Backpacks);

            //! Equipment - Belts
            ExpansionMenuSubCategory sub_Cat_Equipment_Belts = new ExpansionMenuSubCategory();
            sub_Cat_Equipment_Belts.SetDisplayName("Belts");
            sub_Cat_Equipment_Belts.SetIconPath("set:dayz_inventory image:hips");
            sub_Cat_Equipment_Belts.AddIncluded("Belt_Base");
            cat_Equipment.SubCategories.Add(sub_Cat_Equipment_Belts);

            //! Equipment - Blouses & Suits
            ExpansionMenuSubCategory sub_Cat_Equipment_BlousesAndSuits = new ExpansionMenuSubCategory();
            sub_Cat_Equipment_BlousesAndSuits.SetDisplayName("Blouses & Suits");
            sub_Cat_Equipment_BlousesAndSuits.SetIconPath("set:dayz_inventory image:body");
            sub_Cat_Equipment_BlousesAndSuits.AddIncluded("Blouse_ColorBase");
            sub_Cat_Equipment_BlousesAndSuits.AddIncluded("ManSuit_ColorBase");
            sub_Cat_Equipment_BlousesAndSuits.AddIncluded("WomanSuit_ColorBase");
            cat_Equipment.SubCategories.Add(sub_Cat_Equipment_BlousesAndSuits);

            //! Equipment - Boots & Shoes
            ExpansionMenuSubCategory sub_Cat_Equipment_BootsAndShoes = new ExpansionMenuSubCategory();
            sub_Cat_Equipment_BootsAndShoes.SetDisplayName("Boots & Shoes");
            sub_Cat_Equipment_BootsAndShoes.SetIconPath("set:dayz_inventory image:feet");
            sub_Cat_Equipment_BootsAndShoes.AddIncluded("Shoes_Base");
            cat_Equipment.SubCategories.Add(sub_Cat_Equipment_BootsAndShoes);

            //! Equipment - Caps
            ExpansionMenuSubCategory sub_Cat_Equipment_Caps = new ExpansionMenuSubCategory();
            sub_Cat_Equipment_Caps.SetDisplayName("Caps");
            sub_Cat_Equipment_Caps.SetIconPath("set:dayz_inventory image:headgear");
            sub_Cat_Equipment_Caps.AddIncluded("BaseballCap_ColorBase");
            sub_Cat_Equipment_Caps.AddIncluded("PrisonerCap");
            sub_Cat_Equipment_Caps.AddIncluded("PilotkaCap");
            sub_Cat_Equipment_Caps.AddIncluded("PoliceCap");
            sub_Cat_Equipment_Caps.AddIncluded("FlatCap_ColorBase");
            sub_Cat_Equipment_Caps.AddIncluded("ZmijovkaCap_ColorBase");
            sub_Cat_Equipment_Caps.AddIncluded("RadarCap_ColorBase");
            cat_Equipment.SubCategories.Add(sub_Cat_Equipment_Caps);

            //! Equipment - Coats & Jackets
            ExpansionMenuSubCategory sub_Cat_Equipment_CoatsAndJackets = new ExpansionMenuSubCategory();
            sub_Cat_Equipment_CoatsAndJackets.SetDisplayName("Coats & Jackets");
            sub_Cat_Equipment_CoatsAndJackets.SetIconPath("set:dayz_inventory image:body");
            sub_Cat_Equipment_CoatsAndJackets.AddIncluded("LabCoat");
            sub_Cat_Equipment_CoatsAndJackets.AddIncluded("TrackSuitJacket_ColorBase");
            sub_Cat_Equipment_CoatsAndJackets.AddIncluded("DenimJacket");
            sub_Cat_Equipment_CoatsAndJackets.AddIncluded("WoolCoat_ColorBase");
            sub_Cat_Equipment_CoatsAndJackets.AddIncluded("RidersJacket_ColorBase");
            sub_Cat_Equipment_CoatsAndJackets.AddIncluded("FirefighterJacket_ColorBase");
            sub_Cat_Equipment_CoatsAndJackets.AddIncluded("JumpsuitJacket_ColorBase");
            sub_Cat_Equipment_CoatsAndJackets.AddIncluded("BomberJacket_ColorBase");
            sub_Cat_Equipment_CoatsAndJackets.AddIncluded("QuiltedJacket_ColorBase");
            sub_Cat_Equipment_CoatsAndJackets.AddIncluded("PrisonUniformJacket");
            sub_Cat_Equipment_CoatsAndJackets.AddIncluded("PoliceJacketOrel");
            sub_Cat_Equipment_CoatsAndJackets.AddIncluded("PoliceJacket");
            sub_Cat_Equipment_CoatsAndJackets.AddIncluded("ParamedicJacket_ColorBase");
            sub_Cat_Equipment_CoatsAndJackets.AddIncluded("HikingJacket_ColorBase");
            sub_Cat_Equipment_CoatsAndJackets.AddIncluded("Raincoat_ColorBase");
            sub_Cat_Equipment_CoatsAndJackets.AddIncluded("TTsKOJacket_ColorBase");
            sub_Cat_Equipment_CoatsAndJackets.AddIncluded("BDUJacket");
            sub_Cat_Equipment_CoatsAndJackets.AddIncluded("HuntingJacket_ColorBase");
            sub_Cat_Equipment_CoatsAndJackets.AddIncluded("M65Jacket_ColorBase");
            sub_Cat_Equipment_CoatsAndJackets.AddIncluded("GorkaEJacket_ColorBase");
            sub_Cat_Equipment_CoatsAndJackets.AddIncluded("USMCJacket_ColorBase");
            sub_Cat_Equipment_CoatsAndJackets.AddIncluded("NBCJacketBase");
            cat_Equipment.SubCategories.Add(sub_Cat_Equipment_CoatsAndJackets);

            //! Equipment - Eyewear
            ExpansionMenuSubCategory sub_Cat_Equipment_Eyewear = new ExpansionMenuSubCategory();
            sub_Cat_Equipment_Eyewear.SetDisplayName("Eyewear");
            sub_Cat_Equipment_Eyewear.SetIconPath("set:dayz_inventory image:eyewear");
            sub_Cat_Equipment_Eyewear.AddIncluded("Glasses_Base");
            cat_Equipment.SubCategories.Add(sub_Cat_Equipment_Eyewear);

            //! Equipment - Ghillies
            ExpansionMenuSubCategory sub_Cat_Equipment_Ghillies = new ExpansionMenuSubCategory();
            sub_Cat_Equipment_Ghillies.SetDisplayName("Ghillies");
            sub_Cat_Equipment_Ghillies.SetIconPath("set:dayz_inventory image:missing");
            sub_Cat_Equipment_Ghillies.AddIncluded("GhillieHood_ColorBase");
            sub_Cat_Equipment_Ghillies.AddIncluded("GhillieBushrag_ColorBase");
            sub_Cat_Equipment_Ghillies.AddIncluded("GhillieTop_ColorBase");
            sub_Cat_Equipment_Ghillies.AddIncluded("GhillieSuit_ColorBase");
            cat_Equipment.SubCategories.Add(sub_Cat_Equipment_Ghillies);

            //! Equipment - Gloves
            ExpansionMenuSubCategory sub_Cat_Equipment_Gloves = new ExpansionMenuSubCategory();
            sub_Cat_Equipment_Gloves.SetDisplayName("Gloves");
            sub_Cat_Equipment_Gloves.SetIconPath("set:dayz_inventory image:gloves");
            sub_Cat_Equipment_Gloves.AddIncluded("Gloves_Base");
            cat_Equipment.SubCategories.Add(sub_Cat_Equipment_Gloves);

            //! Equipment - Hats & Hoods
            ExpansionMenuSubCategory sub_Cat_Equipment_HatsAndHoods = new ExpansionMenuSubCategory();
            sub_Cat_Equipment_HatsAndHoods.SetDisplayName("Hats & Hoods");
            sub_Cat_Equipment_HatsAndHoods.SetIconPath("set:dayz_inventory image:headgear");
            sub_Cat_Equipment_HatsAndHoods.AddIncluded("MedicalScrubsHat_ColorBase");
            sub_Cat_Equipment_HatsAndHoods.AddIncluded("CowboyHat_ColorBase");
            sub_Cat_Equipment_HatsAndHoods.AddIncluded("MilitaryBeret_ColorBase");
            sub_Cat_Equipment_HatsAndHoods.AddIncluded("BeanieHat_ColorBase");
            sub_Cat_Equipment_HatsAndHoods.AddIncluded("Ushanka_ColorBase");
            sub_Cat_Equipment_HatsAndHoods.AddIncluded("BoonieHat_ColorBase");
            sub_Cat_Equipment_HatsAndHoods.AddIncluded("OfficerHat");
            sub_Cat_Equipment_HatsAndHoods.AddIncluded("NBCHood");
            cat_Equipment.SubCategories.Add(sub_Cat_Equipment_HatsAndHoods);

            //! Equipment - Helmets
            ExpansionMenuSubCategory sub_Cat_Equipment_Helmets = new ExpansionMenuSubCategory();
            sub_Cat_Equipment_Helmets.SetDisplayName("Helmets");
            sub_Cat_Equipment_Helmets.SetIconPath("set:dayz_inventory image:headgear");
            sub_Cat_Equipment_Helmets.AddIncluded("HelmetBase");
            cat_Equipment.SubCategories.Add(sub_Cat_Equipment_Helmets);

            //! Equipment - Masks
            ExpansionMenuSubCategory sub_Cat_Equipment_Masks = new ExpansionMenuSubCategory();
            sub_Cat_Equipment_Masks.SetDisplayName("Masks");
            sub_Cat_Equipment_Masks.SetIconPath("set:dayz_inventory image:mask");
            sub_Cat_Equipment_Masks.AddIncluded("Mask_Base");
            cat_Equipment.SubCategories.Add(sub_Cat_Equipment_Masks);

            //! Equipment - Pants
            ExpansionMenuSubCategory sub_Cat_Equipment_Pants = new ExpansionMenuSubCategory();
            sub_Cat_Equipment_Pants.SetDisplayName("Pants");
            sub_Cat_Equipment_Pants.SetIconPath("set:dayz_inventory image:legs");
            sub_Cat_Equipment_Pants.AddIncluded("Pants_Base");
            cat_Equipment.SubCategories.Add(sub_Cat_Equipment_Pants);

            //! Equipment - Shirts
            ExpansionMenuSubCategory sub_Cat_Equipment_Shirts = new ExpansionMenuSubCategory();
            sub_Cat_Equipment_Shirts.SetDisplayName("Shirts");
            sub_Cat_Equipment_Shirts.SetIconPath("set:dayz_inventory image:body");
            sub_Cat_Equipment_Shirts.AddIncluded("TShirt_ColorBase");
            sub_Cat_Equipment_Shirts.AddIncluded("Shirt_ColorBase");
            cat_Equipment.SubCategories.Add(sub_Cat_Equipment_Shirts);

            //! Equipment - Skirts & Dresses
            ExpansionMenuSubCategory sub_Cat_Equipment_SkirtsAndDresses = new ExpansionMenuSubCategory();
            sub_Cat_Equipment_SkirtsAndDresses.SetDisplayName("Skirts & Dresses");
            sub_Cat_Equipment_SkirtsAndDresses.SetIconPath("set:dayz_inventory image:body");
            sub_Cat_Equipment_SkirtsAndDresses.AddIncluded("Skirt_ColorBase");
            sub_Cat_Equipment_SkirtsAndDresses.AddIncluded("MiniDress_ColorBase");
            sub_Cat_Equipment_SkirtsAndDresses.AddIncluded("NurseDress_ColorBase");
            cat_Equipment.SubCategories.Add(sub_Cat_Equipment_SkirtsAndDresses);

            //! Equipment - Sweaters & Hoodies
            ExpansionMenuSubCategory sub_Cat_Equipment_SweatersAndHoodies = new ExpansionMenuSubCategory();
            sub_Cat_Equipment_SweatersAndHoodies.SetDisplayName("Sweaters & Hoodies");
            sub_Cat_Equipment_SweatersAndHoodies.SetIconPath("set:dayz_inventory image:body");
            sub_Cat_Equipment_SweatersAndHoodies.AddIncluded("Sweater_ColorBase");
            sub_Cat_Equipment_SweatersAndHoodies.AddIncluded("Hoodie_ColorBase");
            cat_Equipment.SubCategories.Add(sub_Cat_Equipment_SweatersAndHoodies);

            //! Equipment - Vests
            ExpansionMenuSubCategory sub_Cat_Equipment_Vests = new ExpansionMenuSubCategory();
            sub_Cat_Equipment_Vests.SetDisplayName("Vests");
            sub_Cat_Equipment_Vests.SetIconPath("set:dayz_inventory image:vest");
            sub_Cat_Equipment_Vests.AddIncluded("Vest_Base");
            cat_Equipment.SubCategories.Add(sub_Cat_Equipment_Vests);

            //! Explosives
            ExpansionMenuCategory cat_Explosives = new ExpansionMenuCategory();
            cat_Explosives.SetDisplayName("Explosives");
            cat_Explosives.SetIconPath("set:dayz_inventory image:explosive");
            cat_Explosives.AddIncluded("ExplosivesBase");
            cat_Explosives.AddIncluded("RemoteDetonator");
            cat_Explosives.AddIncluded("RemoteDetonatorTrigger");
            MenuCategories.Add(cat_Explosives);

            //! Vehicles
            ExpansionMenuCategory cat_Vehicles = new ExpansionMenuCategory();
            cat_Vehicles.SetDisplayName("Vehicles");
            cat_Vehicles.SetIconPath("set:dayz_inventory image:cat_vehicle_body");
            cat_Vehicles.AddIncluded("CarScript");
            MenuCategories.Add(cat_Vehicles);

            //! Vehicles - Cars
            ExpansionMenuSubCategory sub_Cat_Vehicles_Cars = new ExpansionMenuSubCategory();
            sub_Cat_Vehicles_Cars.SetDisplayName("Cars");
            sub_Cat_Vehicles_Cars.SetIconPath("set:dayz_inventory image:cat_vehicle_body");
            sub_Cat_Vehicles_Cars.AddIncluded("OffroadHatchback");
            sub_Cat_Vehicles_Cars.AddIncluded("Hatchback_02");
            sub_Cat_Vehicles_Cars.AddIncluded("Sedan_02");
            sub_Cat_Vehicles_Cars.AddIncluded("CivilianSedan");
            sub_Cat_Vehicles_Cars.AddIncluded("Truck_01_Base");
            sub_Cat_Vehicles_Cars.AddIncluded("Offroad_02");
            sub_Cat_Vehicles_Cars.AddIncluded("ExpansionTractor");
            sub_Cat_Vehicles_Cars.AddIncluded("ExpansionUAZ");
            sub_Cat_Vehicles_Cars.AddIncluded("ExpansionBus");
            sub_Cat_Vehicles_Cars.AddIncluded("ExpansionVodnik");
            sub_Cat_Vehicles_Cars.AddIncluded("Expansion_Landrover");
            cat_Vehicles.SubCategories.Add(sub_Cat_Vehicles_Cars);

            //! Vehicles - Helicopters
            ExpansionMenuSubCategory sub_Cat_Vehicles_Helis = new ExpansionMenuSubCategory();
            sub_Cat_Vehicles_Helis.SetDisplayName("Helicopters");
            sub_Cat_Vehicles_Helis.SetIconPath("set:dayz_inventory image:cat_vehicle_body");
            sub_Cat_Vehicles_Helis.AddIncluded("ExpansionHelicopterScript");
            cat_Vehicles.SubCategories.Add(sub_Cat_Vehicles_Helis);

            //! Vehicles - Boats
            ExpansionMenuSubCategory sub_Cat_Vehicles_Boats = new ExpansionMenuSubCategory();
            sub_Cat_Vehicles_Boats.SetDisplayName("Boats");
            sub_Cat_Vehicles_Boats.SetIconPath("set:dayz_inventory image:cat_vehicle_body");
            sub_Cat_Vehicles_Boats.AddIncluded("ExpansionBoatScript");
            cat_Vehicles.SubCategories.Add(sub_Cat_Vehicles_Boats);

            //! Weapons
            ExpansionMenuCategory cat_Weapons = new ExpansionMenuCategory();
            cat_Weapons.SetDisplayName("Weapons");
            cat_Weapons.SetIconPath("set:dayz_inventory image:pistol");
            cat_Weapons.AddIncluded("Weapon_Base");
            cat_Weapons.AddIncluded("Weapon");
            cat_Weapons.AddIncluded("ButtstockBase");
            cat_Weapons.AddIncluded("OpticBase");
            cat_Weapons.AddIncluded("Magazine_Base");
            cat_Weapons.AddExcluded("Ammunition_Base");
            MenuCategories.Add(cat_Weapons);

            //! Weapons - Attachments
            ExpansionMenuSubCategory sub_Cat_Weapons_Attachments = new ExpansionMenuSubCategory();
            sub_Cat_Weapons_Attachments.SetDisplayName("Attachments");
            sub_Cat_Weapons_Attachments.SetIconPath("set:dayz_inventory image:weaponmuzzle");
            sub_Cat_Weapons_Attachments.AddIncluded("ButtstockBase");
            sub_Cat_Weapons_Attachments.AddIncluded("OpticBase");
            sub_Cat_Weapons_Attachments.AddIncluded("Magazine_Base");
            sub_Cat_Weapons_Attachments.AddExcluded("Ammunition_Base");
            cat_Weapons.SubCategories.Add(sub_Cat_Weapons_Attachments);

            //! Weapons - Assault Rifles
            ExpansionMenuSubCategory sub_Cat_Weapons_AR = new ExpansionMenuSubCategory();
            sub_Cat_Weapons_AR.SetDisplayName("Assault Rifles");
            sub_Cat_Weapons_AR.SetIconPath("set:dayz_inventory image:pistol");
            sub_Cat_Weapons_AR.AddIncluded("RifleBoltFree_Base");
            cat_Weapons.SubCategories.Add(sub_Cat_Weapons_AR);

            //! Weapons - Pistols
            ExpansionMenuSubCategory sub_Cat_Weapons_Pistols = new ExpansionMenuSubCategory();
            sub_Cat_Weapons_Pistols.SetDisplayName("Pistols");
            sub_Cat_Weapons_Pistols.SetIconPath("set:dayz_inventory image:pistol");
            sub_Cat_Weapons_Pistols.AddIncluded("Pistol_Base");
            cat_Weapons.SubCategories.Add(sub_Cat_Weapons_Pistols);

            //! Weapons - Rifles
            ExpansionMenuSubCategory sub_Cat_Weapons_Rifles = new ExpansionMenuSubCategory();
            sub_Cat_Weapons_Rifles.SetDisplayName("Rifles");
            sub_Cat_Weapons_Rifles.SetIconPath("set:dayz_inventory image:pistol");
            sub_Cat_Weapons_Rifles.AddIncluded("Izh18_Base");
            sub_Cat_Weapons_Rifles.AddIncluded("Ruger1022_Base");
            sub_Cat_Weapons_Rifles.AddIncluded("Repeater_Base");
            sub_Cat_Weapons_Rifles.AddIncluded("Mosin9130_Base");
            sub_Cat_Weapons_Rifles.AddIncluded("CZ527_Base");
            sub_Cat_Weapons_Rifles.AddIncluded("CZ550_Base");
            sub_Cat_Weapons_Rifles.AddIncluded("Winchester70_Base");
            sub_Cat_Weapons_Rifles.AddIncluded("SSG82_Base");
            cat_Weapons.SubCategories.Add(sub_Cat_Weapons_Rifles);

            //! Weapons - Shotguns
            ExpansionMenuSubCategory sub_Cat_Weapons_Shotguns = new ExpansionMenuSubCategory();
            sub_Cat_Weapons_Shotguns.SetDisplayName("Shotguns");
            sub_Cat_Weapons_Shotguns.SetIconPath("set:dayz_inventory image:pistol");
            sub_Cat_Weapons_Shotguns.AddIncluded("Izh18Shotgun_Base");
            sub_Cat_Weapons_Shotguns.AddIncluded("Izh43Shotgun_Base");
            sub_Cat_Weapons_Shotguns.AddIncluded("Mp133Shotgun_Base");
            sub_Cat_Weapons_Shotguns.AddIncluded("Remington12");
            sub_Cat_Weapons_Shotguns.AddIncluded("Saiga_Base");
            sub_Cat_Weapons_Shotguns.AddIncluded("SawedoffIzh18Shotgun");
            cat_Weapons.SubCategories.Add(sub_Cat_Weapons_Shotguns);

            //! Weapons - Sniper-Rifles
            ExpansionMenuSubCategory sub_Cat_Weapons_SniperRifles = new ExpansionMenuSubCategory();
            sub_Cat_Weapons_SniperRifles.SetDisplayName("Sniper-Rifles");
            sub_Cat_Weapons_SniperRifles.SetIconPath("set:dayz_inventory image:pistol");
            sub_Cat_Weapons_SniperRifles.AddIncluded("VSS_Base");
            sub_Cat_Weapons_SniperRifles.AddIncluded("B95_base");
            sub_Cat_Weapons_SniperRifles.AddIncluded("SVD_Base");
            sub_Cat_Weapons_SniperRifles.AddIncluded("Scout_Base");
            cat_Weapons.SubCategories.Add(sub_Cat_Weapons_SniperRifles);
        }
        public void DefaultExcludedClassNames()
        {
            ExcludedClassNames.Add("SurvivorBase");
            ExcludedClassNames.Add("ExpansionMoneyBase");
        }
    }

    public class ExpansionMenuCategory
    {
        public string DisplayName { get; set; }
        public string IconPath { get; set; }
        public BindingList<string> Included { get; set; }
        public BindingList<string> Excluded { get; set; }
        public BindingList<ExpansionMenuSubCategory> SubCategories { get; set; }
        public override string ToString()
        {
            return DisplayName;
        }
        public ExpansionMenuCategory()
        {
            DisplayName = "New Menu Category";
            IconPath = "";
            Included = new BindingList<string>();
            Excluded = new BindingList<string>();
            SubCategories = new BindingList<ExpansionMenuSubCategory>();
        }
        public void SetDisplayName(string name)
        {
            DisplayName = name;
        }

        public string GetDisplayName()
        {
            return DisplayName;
        }

        public void SetIconPath(string path)
        {
            IconPath = path;
        }

        public string GetIconPath()
        {
            return IconPath;
        }

        public void AddIncluded(string typeName)
        {
            Included.Add(typeName);
        }

        public BindingList<string> GetIncluded()
        {
            return Included;
        }

        public void AddExcluded(string typeName)
        {
            Excluded.Add(typeName);
        }

        public BindingList<string> GetExcluded()
        {
            return Excluded;
        }
    }

    public class ExpansionMenuSubCategory
    {
        public string DisplayName { get; set; }
        public string IconPath { get; set; }
        public BindingList<string> Included { get; set; }
        public BindingList<string> Excluded { get; set; }
        public override string ToString()
        {
            return DisplayName;
        }
        public ExpansionMenuSubCategory()
        {
            DisplayName = "New Sub Category";
            IconPath = "";
            Included = new BindingList<string>();
            Excluded = new BindingList<string>();
        }
        public void SetDisplayName(string name)
        {
            DisplayName = name;
        }

        public string GetDisplayName()
        {
            return DisplayName;
        }

        public void SetIconPath(string path)
        {
            IconPath = path;
        }

        public string GetIconPath()
        {
            return IconPath;
        }

        public void AddIncluded(string typeName)
        {
            Included.Add(typeName);
        }

        public BindingList<string> GetIncluded()
        {
            return Included;
        }

        public void AddExcluded(string typeName)
        {
            Excluded.Add(typeName);
        }

        public BindingList<string> GetExcluded()
        {
            return Excluded;
        }
    }

}
