using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Drawing.Design;

namespace DayZeLib
{
    public class PersonalStorageSettings
    {
        const int CurrentVersion = 1;

        [Browsable(false)]
        [JsonIgnore]
        public string Filename { get; set; }

        [Browsable(false)]
        [JsonIgnore]
        public bool isDirty { get; set; }

        [Browsable(false)]
        public int m_Version { get; set; }

        public int Enabled { get; set; }
        public int UsePersonalStorageCase { get; set; }
        public int MaxItemsPerStorage { get; set; }
        public BindingList<string> ExcludedClassNames { get; set; }
        public BindingList<ExpansionMenuCategory> MenuCategories { get; set; }

        public PersonalStorageSettings()
        {
            m_Version = CurrentVersion;
            Enabled = 1;
            UsePersonalStorageCase = 1;
            MaxItemsPerStorage = 15;
            ExcludedClassNames = new BindingList<string>();
            MenuCategories = new BindingList<ExpansionMenuCategory>();
			isDirty = true;
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

		public void DefaultMenuCategories()
        {
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
			sub_Cat_Equipment_Backpacks.AddIncluded("ChildBag_ColorBase");
			sub_Cat_Equipment_Backpacks.AddIncluded("DryBag_ColorBase");
			sub_Cat_Equipment_Backpacks.AddIncluded("TaloonBag_ColorBase");
			sub_Cat_Equipment_Backpacks.AddIncluded("SmershBag");
			sub_Cat_Equipment_Backpacks.AddIncluded("AssaultBag_ColorBase");
			sub_Cat_Equipment_Backpacks.AddIncluded("HuntingBag");
			sub_Cat_Equipment_Backpacks.AddIncluded("TortillaBag");
			sub_Cat_Equipment_Backpacks.AddIncluded("CoyoteBag_ColorBase");
			sub_Cat_Equipment_Backpacks.AddIncluded("MountainBag_ColorBase");
			sub_Cat_Equipment_Backpacks.AddIncluded("AliceBag_ColorBase");
			cat_Equipment.SubCategories.Add(sub_Cat_Equipment_Backpacks);

			//! Equipment - Belts
			ExpansionMenuSubCategory sub_Cat_Equipment_Belts = new ExpansionMenuSubCategory();
			sub_Cat_Equipment_Belts.SetDisplayName("Belts");
			sub_Cat_Equipment_Belts.SetIconPath("set:dayz_inventory image:hips");
			sub_Cat_Equipment_Belts.AddIncluded("CivilianBelt");
			sub_Cat_Equipment_Belts.AddIncluded("MilitaryBelt");
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
			sub_Cat_Equipment_BootsAndShoes.AddIncluded("AthleticShoes_ColorBase");
			sub_Cat_Equipment_BootsAndShoes.AddIncluded("JoggingShoes_ColorBase");
			sub_Cat_Equipment_BootsAndShoes.AddIncluded("Sneakers_ColorBase");
			sub_Cat_Equipment_BootsAndShoes.AddIncluded("Ballerinas_ColorBase");
			sub_Cat_Equipment_BootsAndShoes.AddIncluded("DressShoes_ColorBase");
			sub_Cat_Equipment_BootsAndShoes.AddIncluded("HikingBootsLow_ColorBase");
			sub_Cat_Equipment_BootsAndShoes.AddIncluded("WorkingBoots_ColorBase");
			sub_Cat_Equipment_BootsAndShoes.AddIncluded("HikingBoots_ColorBase");
			sub_Cat_Equipment_BootsAndShoes.AddIncluded("CombatBoots_ColorBase");
			sub_Cat_Equipment_BootsAndShoes.AddIncluded("JungleBoots_ColorBase");
			sub_Cat_Equipment_BootsAndShoes.AddIncluded("Wellies_ColorBase");
			sub_Cat_Equipment_BootsAndShoes.AddIncluded("TTSKOBoots");
			sub_Cat_Equipment_BootsAndShoes.AddIncluded("MilitaryBoots_ColorBase");
			sub_Cat_Equipment_BootsAndShoes.AddIncluded("NBCBootsBase");
			cat_Equipment.SubCategories.Add(sub_Cat_Equipment_BootsAndShoes);

			//! Equipment - Caps
			ExpansionMenuSubCategory sub_Cat_Equipment_Caps = new ExpansionMenuSubCategory();
			sub_Cat_Equipment_Caps.SetDisplayName("Caps");
			sub_Cat_Equipment_Caps.SetIconPath("set:dayz_inventory image:headgear");
			sub_Cat_Equipment_Caps.AddIncluded("BaseballCap_CMMG_Pink");
			sub_Cat_Equipment_Caps.AddIncluded("BaseballCap_CMMG_Black");
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
			sub_Cat_Equipment_Eyewear.AddIncluded("SportGlasses_ColorBase");
			sub_Cat_Equipment_Eyewear.AddIncluded("ThinFramesGlasses");
			sub_Cat_Equipment_Eyewear.AddIncluded("ThickFramesGlasses");
			sub_Cat_Equipment_Eyewear.AddIncluded("DesignerGlasses");
			sub_Cat_Equipment_Eyewear.AddIncluded("AviatorGlasses");
			sub_Cat_Equipment_Eyewear.AddIncluded("TacticalGoggles");
			sub_Cat_Equipment_Eyewear.AddIncluded("NVGHeadstrap");
			sub_Cat_Equipment_Eyewear.AddIncluded("EyePatch_Improvised");
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
			sub_Cat_Equipment_Gloves.AddIncluded("SurgicalGloves_ColorBase");
			sub_Cat_Equipment_Gloves.AddIncluded("WorkingGloves_ColorBase");
			sub_Cat_Equipment_Gloves.AddIncluded("TacticalGloves_ColorBase");
			sub_Cat_Equipment_Gloves.AddIncluded("OMNOGloves_ColorBase");
			sub_Cat_Equipment_Gloves.AddIncluded("NBCGloves_ColorBase");
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
			sub_Cat_Equipment_Masks.AddIncluded("SurgicalMask");
			sub_Cat_Equipment_Masks.AddIncluded("NioshFaceMask");
			sub_Cat_Equipment_Masks.AddIncluded("HockeyMask");
			sub_Cat_Equipment_Masks.AddIncluded("BalaclavaMask_ColorBase");
			sub_Cat_Equipment_Masks.AddIncluded("Balaclava3Holes_ColorBase");
			sub_Cat_Equipment_Masks.AddIncluded("WeldingMask");
			sub_Cat_Equipment_Masks.AddIncluded("GasMask");
			sub_Cat_Equipment_Masks.AddIncluded("GP5GasMask");
			sub_Cat_Equipment_Masks.AddIncluded("AirborneMask");
			cat_Equipment.SubCategories.Add(sub_Cat_Equipment_Masks);

			//! Equipment - Pants
			ExpansionMenuSubCategory sub_Cat_Equipment_Pants = new ExpansionMenuSubCategory();
			sub_Cat_Equipment_Pants.SetDisplayName("Pants");
			sub_Cat_Equipment_Pants.SetIconPath("set:dayz_inventory image:legs");
			sub_Cat_Equipment_Pants.AddIncluded("MedicalScrubsPants_ColorBase");
			sub_Cat_Equipment_Pants.AddIncluded("TrackSuitPants_ColorBase");
			sub_Cat_Equipment_Pants.AddIncluded("PrisonUniformPants");
			sub_Cat_Equipment_Pants.AddIncluded("Breeches_ColorBase");
			sub_Cat_Equipment_Pants.AddIncluded("SlacksPants_ColorBase");
			sub_Cat_Equipment_Pants.AddIncluded("CanvasPantsMidi_ColorBase");
			sub_Cat_Equipment_Pants.AddIncluded("CanvasPants_ColorBase");
			sub_Cat_Equipment_Pants.AddIncluded("JumpsuitPants_ColorBase");
			sub_Cat_Equipment_Pants.AddIncluded("PolicePants");
			sub_Cat_Equipment_Pants.AddIncluded("ParamedicPants_ColorBase");
			sub_Cat_Equipment_Pants.AddIncluded("FirefightersPants_ColorBase");
			sub_Cat_Equipment_Pants.AddIncluded("CargoPants_ColorBase");
			sub_Cat_Equipment_Pants.AddIncluded("ShortJeans_ColorBase");
			sub_Cat_Equipment_Pants.AddIncluded("Jeans_ColorBase");
			sub_Cat_Equipment_Pants.AddIncluded("TTSKOPants");
			sub_Cat_Equipment_Pants.AddIncluded("BDUPants");
			sub_Cat_Equipment_Pants.AddIncluded("USMCPants_ColorBase");
			sub_Cat_Equipment_Pants.AddIncluded("PolicePantsOrel");
			sub_Cat_Equipment_Pants.AddIncluded("HunterPants_ColorBase");
			sub_Cat_Equipment_Pants.AddIncluded("GorkaPants_ColorBase");
			sub_Cat_Equipment_Pants.AddIncluded("NBCPantsBase");
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
			sub_Cat_Equipment_Vests.AddIncluded("ReflexVest");
			sub_Cat_Equipment_Vests.AddIncluded("PoliceVest");
			sub_Cat_Equipment_Vests.AddIncluded("PressVest_ColorBase");
			sub_Cat_Equipment_Vests.AddIncluded("UKAssVest_ColorBase");
			sub_Cat_Equipment_Vests.AddIncluded("SmershVest");
			sub_Cat_Equipment_Vests.AddIncluded("HighCapacityVest_ColorBase");
			sub_Cat_Equipment_Vests.AddIncluded("PlateCarrierVest");
			sub_Cat_Equipment_Vests.AddIncluded("HuntingVest");
			cat_Equipment.SubCategories.Add(sub_Cat_Equipment_Vests);

			//! Explosives
			ExpansionMenuCategory cat_Explosives = new ExpansionMenuCategory();
			cat_Explosives.SetDisplayName("Explosives");
			cat_Explosives.SetIconPath("set:dayz_inventory image:explosive");
			cat_Explosives.AddIncluded("ExplosivesBase");
			cat_Explosives.AddIncluded("RemoteDetonator");
			cat_Explosives.AddIncluded("RemoteDetonatorTrigger");
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
			sub_Cat_Weapons_AR.AddIncluded("FAL_Base");
			sub_Cat_Weapons_AR.AddIncluded("AKM_Base");
			sub_Cat_Weapons_AR.AddIncluded("AK74_Base");
			sub_Cat_Weapons_AR.AddIncluded("M4A1_Base");
			sub_Cat_Weapons_AR.AddIncluded("M16A2_Base");
			sub_Cat_Weapons_AR.AddIncluded("Famas_Base");
			sub_Cat_Weapons_AR.AddIncluded("Aug_Base");
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
    }

     public class PersonalStorageSettingsNew
    {
        const int CurrentVersion = 2;

        public int m_Version { get; set; }
        public int UseCategoryMenu { get; set; }
        public Storagelevels StorageLevels { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }


        public PersonalStorageSettingsNew()
        {
            m_Version = CurrentVersion;
            UseCategoryMenu = 0;
            StorageLevels = new Storagelevels();
			isDirty = true;
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
    }

    public class Storagelevels
    {
        [JsonPropertyName("1")]
        public StorageLevel StorageLevel01 { get; set; }
        [JsonPropertyName("2")]
        public StorageLevel StorageLevel02 { get; set; }
        [JsonPropertyName("3")]
        public StorageLevel StorageLevel03 { get; set; }
        [JsonPropertyName("4")]
        public StorageLevel StorageLevel04 { get; set; }
        [JsonPropertyName("5")]
        public StorageLevel StorageLevel05 { get; set; }
        [JsonPropertyName("6")]
        public StorageLevel StorageLevel06 { get; set; }
        [JsonPropertyName("7")]
        public StorageLevel StorageLevel07 { get; set; }
        [JsonPropertyName("8")]
        public StorageLevel StorageLevel08 { get; set; }
        [JsonPropertyName("9")]
        public StorageLevel StorageLevel09 { get; set; }
        [JsonPropertyName("10")]
        public StorageLevel StorageLevel10 { get; set; }

        public Storagelevels()
        {
            StorageLevel01 = new StorageLevel()
            {
                ExcludedSlots = new BindingList<string>() { "Vest", "Body", "Hips", "Legs", "Back" }
            };
            StorageLevel02 = new StorageLevel()
            {
                ExcludedSlots = new BindingList<string>() { "Vest", "Body", "Hips", "Legs", "Back" }
            };
            StorageLevel03 = new StorageLevel()
            {
                ExcludedSlots = new BindingList<string>() { "Vest", "Body", "Hips", "Legs", "Back" }
            };
            StorageLevel04 = new StorageLevel()
            {
                ExcludedSlots = new BindingList<string>() { "Vest", "Body", "Hips", "Legs", "Back" }
            };
            StorageLevel05 = new StorageLevel()
            {
                ExcludedSlots = new BindingList<string>() { "Vest", "Body", "Hips", "Legs", "Back" }
            };
            StorageLevel06 = new StorageLevel();
            StorageLevel07 = new StorageLevel();
            StorageLevel08 = new StorageLevel();
            StorageLevel09 = new StorageLevel();
            StorageLevel10 = new StorageLevel()
            {
                AllowAttachmentCargo = 1
            };
        }
    }

    public class StorageLevel
    {
        public int ReputationRequirement { get; set; }
        public BindingList<string> ExcludedSlots { get; set; }
        public int AllowAttachmentCargo { get; set; }

        public StorageLevel()
        {
            ReputationRequirement = -1;
            ExcludedSlots = new BindingList<string>();
            AllowAttachmentCargo = 0;
        }
    }
}
