using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class P2PMarketSettings
    {
        public const int CurrentVersion = 2;

        public int m_Version { get; set; }
        public int Enabled { get; set; }
        public int MaxListingTime { get; set; }
        public int MaxListings { get; set; }
        public int ListingOwnerDiscountPercent { get; set; }
        public int ListingPricePercent { get; set; }
        public BindingList<string> ExcludedClassNames { get; set; }
        public BindingList<Menucategory> MenuCategories { get; set; }
        public int SalesDepositTime { get; set; }

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
            ExcludedClassNames = new BindingList<string>(){ "SurvivorBase", "ExpansionMoneyBase" };
            MenuCategories = new BindingList<Menucategory>();
            MenuCategories.Add(new Menucategory()
            {
                DisplayName = "Ammo",
                IconPath = "set:dayz_inventory image:bullets",
                Included = new BindingList<string>() { "Ammunition_Base", "Box_Base" },
                Excluded = new BindingList<string>(),
                SubCategories = new BindingList<Subcategory>()
                {
                    new Subcategory()
                    {
                        DisplayName = "Ammo-Boxes",
                        IconPath = "set:dayz_inventory image:bullets",
                        Included = new BindingList<string>(){ "Box_Base"},
                        Excluded = new BindingList<string>()
                    },
                    new Subcategory()
                    {
                        DisplayName = "Ammo-Bullets",
                        IconPath = "set:dayz_inventory image:bullets",
                        Included = new BindingList<string>(){ "Ammunition_Base"},
                        Excluded = new BindingList<string>()
                    }
                }
            });
            MenuCategories.Add(new Menucategory()
            {
                DisplayName = "Containers",
                IconPath = "set:dayz_inventory image:barrel",
                Included = new BindingList<string>() { "Container_Base" },
                Excluded = new BindingList<string>(),
                SubCategories = new BindingList<Subcategory>()
            });
            MenuCategories.Add(new Menucategory()
            {
                DisplayName = "Consumables",
                IconPath = "set:dayz_inventory image:food",
                Included = new BindingList<string>() { "Edible_Base" },
                Excluded = new BindingList<string>(),
                SubCategories = new BindingList<Subcategory>()
                {
                    new Subcategory()
                    {
                        DisplayName = "Drinks",
                        IconPath = "set:dayz_inventory image:canteen",
                        Included = new BindingList<string>()
                        {
                            "SodaCan_ColorBase",
                            "WaterBottle",
                            "Canteen",
                            "Vodka",
                            "ExpansionMilkBottle"
                        },
                        Excluded = new BindingList<string>()
                    },
                    new Subcategory()
                    {
                        DisplayName = "Fish",
                        IconPath = "set:dayz_inventory image:food",
                        Included = new BindingList<string>()
                        {
                            "CarpFilletMeat",
                            "MackerelFilletMeat",
                            "Carp",
                            "Sardines",
                            "Mackerel"
                        },
                        Excluded = new BindingList<string>()
                    },
                    new Subcategory()
                    {
                        DisplayName = "Food",
                        IconPath = "set:dayz_inventory image:food",
                        Included = new BindingList<string>()
                        {
                            "Zagorky",
                            "ZagorkyChocolate",
                            "ZagorkyPeanuts",
                            "PowderedMilk",
                            "BoxCerealCrunchin",
                            "Rice",
                            "Marmalade",
                            "Honey",
                            "SaltySticks",
                            "Crackers",
                            "Chips",
                            "Pajka",
                            "Pate",
                            "BrisketSpread",
                            "SardinesCan",
                            "TunaCan",
                            "DogFoodCan",
                            "CatFoodCan",
                            "PorkCan",
                            "Lunchmeat",
                            "UnknownFoodCan",
                            "PeachesCan",
                            "SpaghettiCan",
                            "BakedBeansCan",
                            "TacticalBaconCan"
                        },
                        Excluded = new BindingList<string>()
                    },
                    new Subcategory()
                    {
                        DisplayName = "Meat",
                        IconPath = "set:dayz_inventory image:food",
                        Included = new BindingList<string>()
                        {
                            "BearSteakMeat",
                            "GoatSteakMeat",
                            "BoarSteakMeat",
                            "PigSteakMeat",
                            "DeerSteakMeat",
                            "WolfSteakMeat",
                            "CowSteakMeat",
                            "SheepSteakMeat",
                            "ChickenBreastMeat",
                            "RabbitLegMeat"
                        },
                        Excluded = new BindingList<string>()
                    },
                    new Subcategory()
                    {
                        DisplayName = "Meds",
                        IconPath = "set:dayz_inventory image:medicalbandage",
                        Included = new BindingList<string>()
                        {
                            "CharcoalTablets",
                            "DisinfectantAlcohol",
                            "PurificationTablets",
                            "VitaminBottle",
                            "DisinfectantSpray",
                            "TetracyclineAntibiotics",
                            "PainkillerTablets",
                            "IodineTincture"
                        },
                        Excluded = new BindingList<string>()
                    },
                    new Subcategory()
                    {
                        DisplayName = "Vegetables",
                        IconPath = "set:dayz_inventory image:food",
                        Included = new BindingList<string>()
                        {
                            "Apple",
                            "GreenBellPepper",
                            "Zucchini",
                            "Pumpkin",
                            "SlicedPumpkin",
                            "PotatoSeed",
                            "Potato",
                            "Tomato",
                            "SambucusBerry",
                            "CaninaBerry",
                            "Plum",
                            "Pear",
                            "MushroomBase"
                        },
                        Excluded = new BindingList<string>()
                    }
                }
            });
        }
    }

    public class Menucategory
    {
        public string DisplayName { get; set; }
        public string IconPath { get; set; }
        public BindingList<string> Included { get; set; }
        public BindingList<string> Excluded { get; set; }
        public BindingList<Subcategory> SubCategories { get; set; }
        public override string ToString()
        {
            return DisplayName;
        }
        public Menucategory()
        {
            DisplayName = "New Menu Category";
            IconPath = "";
            Included = new BindingList<string>();
            Excluded = new BindingList<string>();
            SubCategories = new BindingList<Subcategory>();
                
        }
    }

    public class Subcategory
    {
        public string DisplayName { get; set; }
        public string IconPath { get; set; }
        public BindingList<string> Included { get; set; }
        public BindingList<string> Excluded { get; set; }
        public override string ToString()
        {
            return DisplayName;
        }
        public Subcategory()
        {
            DisplayName = "New Sub Category";
            IconPath = "";
            Included = new BindingList<string>();
            Excluded = new BindingList<string>();
        }
    }

}
