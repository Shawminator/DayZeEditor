using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class MarketSettings
    {
        const int CurrentVersion = 17;

        public int m_Version { get; set; }
        public int MarketSystemEnabled { get; set; }
        public BindingList<string[]> NetworkCategories { get; set; } //empty atm
        public string CurrencyIcon { get; set; }
        public int ATMSystemEnabled { get; set; }
        public int MaxDepositMoney { get; set; }
        public int DefaultDepositMoney { get; set; }
        public int ATMPlayerTransferEnabled { get; set; }
        public int ATMPartyLockerEnabled { get; set; }
        public int MaxPartyDepositMoney { get; set; }
        //public BindingList<string[]> MarketVIPs { get; set; }
        public int UseWholeMapForATMPlayerList { get; set; }
        public decimal SellPricePercent { get; set; }
        public int NetworkBatchSize { get; set; }
        public decimal MaxVehicleDistanceToTrader { get; set; }
        public decimal MaxLargeVehicleDistanceToTrader { get; set; }

        public BindingList<string> LargeVehicles { get; set; }
        public BindingList<ExpansionMarketSpawnPosition> LandSpawnPositions { get; set; }
        public BindingList<ExpansionMarketSpawnPosition> AirSpawnPositions { get; set; }
        public BindingList<ExpansionMarketSpawnPosition> WaterSpawnPositions { get; set; }
        public BindingList<ExpansionMarketSpawnPosition> TrainSpawnPositions { get; set; }
        public MarketMenuColours MarketMenuColors { get; set; }
        public BindingList<string> Currencies { get; set; }
        public BindingList<string> VehicleKeys { get; set; }

        public decimal MaxSZVehicleParkingTime { get; set; }
        public int SZVehicleParkingTicketFine { get; set; }
        public int SZVehicleParkingFineUseKey { get; set; }
        public int DisallowUnpersisted { get; set; }
        public int DisableClientSellTransactionDetails { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty = false;

        public MarketSettings()
        {
            m_Version = 12;
            CurrencyIcon = "DayZExpansion/Core/GUI/icons/misc/coinstack2_64x64.edds";
            NetworkCategories = new BindingList<string[]>();
            //MarketVIPs = new BindingList<string[]>();
            LandSpawnPositions = new BindingList<ExpansionMarketSpawnPosition>();
            AirSpawnPositions = new BindingList<ExpansionMarketSpawnPosition>();
            WaterSpawnPositions = new BindingList<ExpansionMarketSpawnPosition>();
            TrainSpawnPositions = new BindingList<ExpansionMarketSpawnPosition>();
            MarketMenuColors = new MarketMenuColours();
            Currencies = new BindingList<string>();
            VehicleKeys = new BindingList<string>();
            isDirty = true;

        }
        public MarketSettings(string name)
        {
            Filename = name;
            m_Version = CurrentVersion;
            MarketSystemEnabled = 1;
            NetworkCategories = new BindingList<string[]>();
            MaxVehicleDistanceToTrader = 120;
            MaxLargeVehicleDistanceToTrader = 744;
            LargeVehicles = new BindingList<string>()
            {
                "expansionlhd"
            };
            LandSpawnPositions = new BindingList<ExpansionMarketSpawnPosition>();
            AirSpawnPositions = new BindingList<ExpansionMarketSpawnPosition>();
            WaterSpawnPositions = new BindingList<ExpansionMarketSpawnPosition>();
            TrainSpawnPositions = new BindingList<ExpansionMarketSpawnPosition>();
            Deafultspawnspositions();
            MarketMenuColors = new MarketMenuColours();

            CurrencyIcon = "DayZExpansion/Core/GUI/icons/misc/coinstack2_64x64.edds";
            SellPricePercent = 75;
            NetworkBatchSize = 100;  //! Sync at most n items per batch

            ATMSystemEnabled = 1;
            MaxDepositMoney = 100000;
            DefaultDepositMoney = 10000;
            ATMPlayerTransferEnabled = 1;
            ATMPartyLockerEnabled = 1;
            MaxPartyDepositMoney = 100000;
            UseWholeMapForATMPlayerList = 0;
            Currencies = new BindingList<string>()
            {
                "expansionbanknotehryvnia"
            };
            VehicleKeys = new BindingList<string>()
            {
                "ExpansionCarKey"
            };
            MaxSZVehicleParkingTime = 1800;  //! 30 minutes
            SZVehicleParkingTicketFine = 0;
            SZVehicleParkingFineUseKey = 1;
            DisallowUnpersisted = 0;
            DisableClientSellTransactionDetails = 0;

        }

        private void Deafultspawnspositions()
        {
            ExpansionMarketSpawnPosition position = new ExpansionMarketSpawnPosition();
            position.Position = new float[] { 11903.4f, 140.0f, 12455.1f };
            position.Orientation = new float[] { 24.0f, 0.0f, 0.0f };
            LandSpawnPositions.Add(position);

            position = new ExpansionMarketSpawnPosition();
            position.Position = new float[] { 11898.4f, 140.0f, 12481.6f };
            position.Orientation = new float[] { 24.0f, 0.0f, 0.0f };
            LandSpawnPositions.Add(position);

            position = new ExpansionMarketSpawnPosition();
            position.Position = new float[] { 11878.0f, 140.0f, 12482.8f };
            position.Orientation = new float[] { 24.0f, 0.0f, 0.0f };
            LandSpawnPositions.Add(position);

            //! Cars - Vehicle Trader - Kamenka
            position = new ExpansionMarketSpawnPosition();
            position.Position = new float[] { 1145.0f, 6.0f, 2405.0f };
            position.Orientation = new float[] { 0.0f, 0.0f, 0.0f };
            LandSpawnPositions.Add(position);

            // Cars - Vehicle Trader - Green Mountain Trader
            position = new ExpansionMarketSpawnPosition();
            position.Position = new float[] { 3722.77f, 402.0f, 6018.93f };
            position.Orientation = new float[] { 138.0f, 0.0f, 0.0f };
            LandSpawnPositions.Add(position);

            position = new ExpansionMarketSpawnPosition();
            position.Position = new float[] { 3737.19f, 402.7f, 6001.95f };
            position.Orientation = new float[] { 138.0f, 0.0f, 0.0f };
            LandSpawnPositions.Add(position);

            //! Aircraft - Aircraft Trader - Krasno
            position = new ExpansionMarketSpawnPosition();
            position.Position = new float[] { 12178.9f, 140.0f, 12638.4f };
            position.Orientation = new float[] { -157.2f, 0.0f, 0.0f };
            AirSpawnPositions.Add(position);

            position = new ExpansionMarketSpawnPosition();
            position.Position = new float[] { 12126.7f, 140.0f, 12664.7f };
            position.Orientation = new float[] { -66.6f, 0.0f, 0.0f };
            AirSpawnPositions.Add(position);

            //! Aircraft - Aircraft Trader - Balota
            position = new ExpansionMarketSpawnPosition();
            position.Position = new float[] { 5006.27f, 9.5f, 2491.1f };
            position.Orientation = new float[] { -131.7f, 0.0f, 0.0f };
            AirSpawnPositions.Add(position);

            position = new ExpansionMarketSpawnPosition();
            position.Position = new float[] { 4982.0f, 9.5f, 2468.0f };
            position.Orientation = new float[] { -131.7f, 0.0f, 0.0f };
            AirSpawnPositions.Add(position);

            position = new ExpansionMarketSpawnPosition();
            position.Position = new float[] { 4968.0f, 9.5f, 2513.0f };
            position.Orientation = new float[] { -131.7f, 0.0f, 0.0f };
            AirSpawnPositions.Add(position);

            //! Water - Boats Trader - Kamenka
            position = new ExpansionMarketSpawnPosition();
            position.Position = new float[] { 1759.0f, 0.0f, 1994.0f };
            position.Orientation = new float[] { 0.0f, 0.0f, 0.0f };
            WaterSpawnPositions.Add(position);

            //! Water - Boats Trader - Sventlo
            position = new ExpansionMarketSpawnPosition();
            position.Position = new float[] { 14347.8f, 0.0f, 13235.8f };
            position.Orientation = new float[] { -147.5f, 0.0f, 0.0f };
            WaterSpawnPositions.Add(position);

            position = new ExpansionMarketSpawnPosition();
            position.Position = new float[] { 14344.1f, 0.0f, 13219.7f };
            position.Orientation = new float[] { -147.5f, 0.0f, 0.0f };
            WaterSpawnPositions.Add(position);

            position = new ExpansionMarketSpawnPosition();
            position.Position = new float[] { 14360.9f, 0.0f, 13246.7f };
            position.Orientation = new float[] { -147.5f, 0.0f, 0.0f };
            WaterSpawnPositions.Add(position);

            //! Water - LHD - Boats Trader - Kamenka
            position = new ExpansionMarketSpawnPosition();
            position.Position = new float[] { 1760.0f, 0.0f, 1730.0f };
            position.Orientation = new float[] { 0.0f, 0.0f, 0.0f };
            WaterSpawnPositions.Add(position);

            //! Water - LHD - Boats Trader - Sventlo
            position = new ExpansionMarketSpawnPosition();
            position.Position = new float[] { 14540.0f, 0.0f, 12995.0f };
            position.Orientation = new float[] { 0.0f, 0.0f, 0.0f };
            WaterSpawnPositions.Add(position);
        }

        public void setspawnnames()
        {
            int i = 0;
            foreach (ExpansionMarketSpawnPosition sp in LandSpawnPositions)
            {
                sp.name = "Land Spawn position " + i.ToString();
                i++;
            }
            i = 0;
            foreach (ExpansionMarketSpawnPosition sp in AirSpawnPositions)
            {
                sp.name = "Air Spawn position " + i.ToString();
                i++;
            }
            i = 0;
            foreach (ExpansionMarketSpawnPosition sp in WaterSpawnPositions)
            {
                sp.name = "Water Spawn position " + i.ToString();
                i++;
            }
            i = 0;
            foreach (ExpansionMarketSpawnPosition sp in TrainSpawnPositions)
            {
                sp.name = "Train Spawn position " + i.ToString();
                i++;
            }
        }
        public ExpansionMarketSpawnPosition getSpawnbyindex(int type, int index)
        {
            if (index == -1) return null;
            switch (type)
            {
                case 0:
                    return LandSpawnPositions[index];
                case 1:
                    return AirSpawnPositions[index];
                case 2:
                    return WaterSpawnPositions[index];
                case 3:
                    return TrainSpawnPositions[index];
            }
            return null;
        }
        public void RemoveSpawn(int type, int index)
        {
            switch (type)
            {
                case 0:
                    LandSpawnPositions.RemoveAt(index);
                    break;
                case 1:
                    AirSpawnPositions.RemoveAt(index);
                    break;
                case 2:
                    WaterSpawnPositions.RemoveAt(index);
                    break;
                case 3:
                    TrainSpawnPositions.RemoveAt(index);
                    break;
            }
            setspawnnames();

        }
        public string getcolourfromcontrol(string name)
        {
            switch (name)
            {
                case "BaseColorVignetteColour":
                    return MarketMenuColors.BaseColorVignette;
                case "BaseColorHeadersColour":
                    return MarketMenuColors.BaseColorHeaders;
                case "BaseColorLabelsColour":
                    return MarketMenuColors.BaseColorLabels;
                case "BaseColorTextColour":
                    return MarketMenuColors.BaseColorText;
                case "BaseColorCheckboxesColour":
                    return MarketMenuColors.BaseColorCheckboxes;
                case "BaseColorInfoSectionBackgroundColour":
                    return MarketMenuColors.BaseColorInfoSectionBackground;
                case "BaseColorTooltipsHeadersColour":
                    return MarketMenuColors.BaseColorTooltipsHeaders;
                case "BaseColorTooltipsBackgroundColour":
                    return MarketMenuColors.BaseColorTooltipsBackground;
                case "ColorDecreaseQuantityButtonColour":
                    return MarketMenuColors.ColorDecreaseQuantityButton;
                case "ColorDecreaseQuantityIconColour":
                    return MarketMenuColors.ColorDecreaseQuantityIcon;
                case "ColorSetQuantityButtonColour":
                    return MarketMenuColors.ColorSetQuantityButton;
                case "ColorIncreaseQuantityButtonColour":
                    return MarketMenuColors.ColorIncreaseQuantityButton;
                case "ColorIncreaseQuantityIconColour":
                    return MarketMenuColors.ColorIncreaseQuantityIcon;
                case "ColorSellPanelColour":
                    return MarketMenuColors.ColorSellPanel;
                case "ColorSellButtonColour":
                    return MarketMenuColors.ColorSellButton;
                case "ColorBuyPanelColour":
                    return MarketMenuColors.ColorBuyPanel;
                case "ColorBuyButtonColour":
                    return MarketMenuColors.ColorBuyButton;
                case "ColorMarketIconColour":
                    return MarketMenuColors.ColorMarketIcon;
                case "ColorFilterOptionsButtonColour":
                    return MarketMenuColors.ColorFilterOptionsButton;
                case "ColorFilterOptionsIconColour":
                    return MarketMenuColors.ColorFilterOptionsIcon;
                case "ColorSearchFilterButtonColour":
                    return MarketMenuColors.ColorSearchFilterButton;
                case "ColorCategoryButtonColour":
                    return MarketMenuColors.ColorCategoryButton;
                case "ColorCategoryCollapseIconColour":
                    return MarketMenuColors.ColorCategoryCollapseIcon;
                case "ColorCurrencyDenominationTextColour":
                    return MarketMenuColors.ColorCurrencyDenominationText;
                case "ColorItemButtonColour":
                    return MarketMenuColors.ColorItemButton;
                case "ColorItemInfoIconColour":
                    return MarketMenuColors.ColorItemInfoIcon;
                case "ColorItemInfoTitleColour":
                    return MarketMenuColors.ColorItemInfoTitle;
                case "ColorItemInfoHasContainerItemsColour":
                    return MarketMenuColors.ColorItemInfoHasContainerItems;
                case "ColorItemInfoHasAttachmentsColour":
                    return MarketMenuColors.ColorItemInfoHasAttachments;
                case "ColorItemInfoHasBulletsColour":
                    return MarketMenuColors.ColorItemInfoHasBullets;
                case "ColorItemInfoIsAttachmentColour":
                    return MarketMenuColors.ColorItemInfoIsAttachment;
                case "ColorItemInfoIsEquipedColour":
                    return MarketMenuColors.ColorItemInfoIsEquiped;
                case "ColorItemInfoAttachmentsColour":
                    return MarketMenuColors.ColorItemInfoAttachments;
                case "ColorToggleCategoriesTextColour":
                    return MarketMenuColors.ColorToggleCategoriesText;
                case "ColorCategoryCornersColour":
                    return MarketMenuColors.ColorCategoryCorners;
                case "ColorCategoryBackgroundColour":
                    return MarketMenuColors.ColorCategoryBackground;
                case "ColorPlayerStockColour":
                    return MarketMenuColors.ColorPlayerStock;
                case "ColorRequirementsNotMetColour":
                    return MarketMenuColors.ColorRequirementsNotMet;
            }
            return "";
        }
        public void setcolour(string name, string Colour)
        {
            Colour = Colour.Substring(2) + Colour.Substring(0, 2);
            switch (name)
            {
                case "BaseColorVignetteColour":
                    MarketMenuColors.BaseColorVignette = Colour;
                    break;
                case "BaseColorHeadersColour":
                    MarketMenuColors.BaseColorHeaders = Colour;
                    break;
                case "BaseColorLabelsColour":
                    MarketMenuColors.BaseColorLabels = Colour;
                    break;
                case "BaseColorTextColour":
                    MarketMenuColors.BaseColorText = Colour;
                    break;
                case "BaseColorCheckboxesColour":
                    MarketMenuColors.BaseColorCheckboxes = Colour;
                    break;
                case "BaseColorInfoSectionBackgroundColour":
                    MarketMenuColors.BaseColorInfoSectionBackground = Colour;
                    break;
                case "BaseColorTooltipsHeadersColour":
                    MarketMenuColors.BaseColorTooltipsHeaders = Colour;
                    break;
                case "BaseColorTooltipsBackgroundColour":
                    MarketMenuColors.BaseColorTooltipsBackground = Colour;
                    break;
                case "ColorDecreaseQuantityButtonColour":
                    MarketMenuColors.ColorDecreaseQuantityButton = Colour;
                    break;
                case "ColorDecreaseQuantityIconColour":
                    MarketMenuColors.ColorDecreaseQuantityIcon = Colour;
                    break;
                case "ColorSetQuantityButtonColour":
                    MarketMenuColors.ColorSetQuantityButton = Colour;
                    break;
                case "ColorIncreaseQuantityButtonColour":
                    MarketMenuColors.ColorIncreaseQuantityButton = Colour;
                    break;
                case "ColorIncreaseQuantityIconColour":
                    MarketMenuColors.ColorIncreaseQuantityIcon = Colour;
                    break;
                case "ColorSellPanelColour":
                    MarketMenuColors.ColorSellPanel = Colour;
                    break;
                case "ColorSellButtonColour":
                    MarketMenuColors.ColorSellButton = Colour;
                    break;
                case "ColorBuyPanelColour":
                    MarketMenuColors.ColorBuyPanel = Colour;
                    break;
                case "ColorBuyButtonColour":
                    MarketMenuColors.ColorBuyButton = Colour;
                    break;
                case "ColorMarketIconColour":
                    MarketMenuColors.ColorMarketIcon = Colour;
                    break;
                case "ColorFilterOptionsButtonColour":
                    MarketMenuColors.ColorFilterOptionsButton = Colour;
                    break;
                case "ColorFilterOptionsIconColour":
                    MarketMenuColors.ColorFilterOptionsIcon = Colour;
                    break;
                case "ColorSearchFilterButtonColour":
                    MarketMenuColors.ColorSearchFilterButton = Colour;
                    break;
                case "ColorCategoryButtonColour":
                    MarketMenuColors.ColorCategoryButton = Colour;
                    break;
                case "ColorCategoryCollapseIconColour":
                    MarketMenuColors.ColorCategoryCollapseIcon = Colour;
                    break;
                case "ColorCurrencyDenominationTextColour":
                    MarketMenuColors.ColorCurrencyDenominationText = Colour;
                    break;
                case "ColorItemButtonColour":
                    MarketMenuColors.ColorItemButton = Colour;
                    break;
                case "ColorItemInfoIconColour":
                    MarketMenuColors.ColorItemInfoIcon = Colour;
                    break;
                case "ColorItemInfoTitleColour":
                    MarketMenuColors.ColorItemInfoTitle = Colour;
                    break;
                case "ColorItemInfoHasContainerItemsColour":
                    MarketMenuColors.ColorItemInfoHasContainerItems = Colour;
                    break;
                case "ColorItemInfoHasAttachmentsColour":
                    MarketMenuColors.ColorItemInfoHasAttachments = Colour;
                    break;
                case "ColorItemInfoHasBulletsColour":
                    MarketMenuColors.ColorItemInfoHasBullets = Colour;
                    break;
                case "ColorItemInfoIsAttachmentColour":
                    MarketMenuColors.ColorItemInfoIsAttachment = Colour;
                    break;
                case "ColorItemInfoIsEquipedColour":
                    MarketMenuColors.ColorItemInfoIsEquiped = Colour;
                    break;
                case "ColorItemInfoAttachmentsColour":
                    MarketMenuColors.ColorItemInfoAttachments = Colour;
                    break;
                case "ColorToggleCategoriesTextColour":
                    MarketMenuColors.ColorToggleCategoriesText = Colour;
                    break;
                case "ColorCategoryCornersColour":
                    MarketMenuColors.ColorCategoryCorners = Colour;
                    break;
                case "ColorCategoryBackgroundColour":
                    MarketMenuColors.ColorCategoryBackground = Colour;
                    break;
                case "ColorPlayerStockColour":
                    MarketMenuColors.ColorPlayerStock = Colour;
                    break;
                case "ColorRequirementsNotMetColour":
                    MarketMenuColors.ColorRequirementsNotMet = Colour;
                    break;
            }
        }
        public void AddnewSpawn(int v)
        {
            switch (v)
            {
                case 0:
                    LandSpawnPositions.Add(new ExpansionMarketSpawnPosition() { name = getnewname(0), Position = new float[] { 0, 0, 0 }, Orientation = new float[] { 0, 0, 0 } });
                    break;
                case 1:
                    AirSpawnPositions.Add(new ExpansionMarketSpawnPosition() { name = getnewname(1), Position = new float[] { 0, 0, 0 }, Orientation = new float[] { 0, 0, 0 } });
                    break;
                case 2:
                    WaterSpawnPositions.Add(new ExpansionMarketSpawnPosition() { name = getnewname(2), Position = new float[] { 0, 0, 0 }, Orientation = new float[] { 0, 0, 0 } });
                    break;
                case 3:
                    TrainSpawnPositions.Add(new ExpansionMarketSpawnPosition() { name = getnewname(3), Position = new float[] { 0, 0, 0 }, Orientation = new float[] { 0, 0, 0 } });
                    break;
            }
        }
        public string getnewname(int v)
        {
            switch (v)
            {
                case 0:
                    return "Land Spawn position " + LandSpawnPositions.Count.ToString();
                case 1:
                    return "Air Spawn position " + AirSpawnPositions.Count.ToString();
                case 2:
                    return "Water Spawn position " + WaterSpawnPositions.Count.ToString();
                case 3:
                    return "Train Spawn position " + TrainSpawnPositions.Count.ToString();
                default:
                    return "";
            }
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
    public class MarketMenuColours
    {
        public string BaseColorVignette { get; set; }
        public string BaseColorHeaders { get; set; }
        public string BaseColorLabels { get; set; }
        public string BaseColorText { get; set; }
        public string BaseColorCheckboxes { get; set; }
        public string BaseColorInfoSectionBackground { get; set; }
        public string BaseColorTooltipsHeaders { get; set; }
        public string BaseColorTooltipsBackground { get; set; }
        public string ColorDecreaseQuantityButton { get; set; }
        public string ColorDecreaseQuantityIcon { get; set; }
        public string ColorSetQuantityButton { get; set; }
        public string ColorIncreaseQuantityButton { get; set; }
        public string ColorIncreaseQuantityIcon { get; set; }
        public string ColorSellPanel { get; set; }
        public string ColorSellButton { get; set; }
        public string ColorBuyPanel { get; set; }
        public string ColorBuyButton { get; set; }
        public string ColorMarketIcon { get; set; }
        public string ColorFilterOptionsButton { get; set; }
        public string ColorFilterOptionsIcon { get; set; }
        public string ColorSearchFilterButton { get; set; }
        public string ColorCategoryButton { get; set; }
        public string ColorCategoryCollapseIcon { get; set; }
        public string ColorCurrencyDenominationText { get; set; }
        public string ColorItemButton { get; set; }
        public string ColorItemInfoIcon { get; set; }
        public string ColorItemInfoTitle { get; set; }
        public string ColorItemInfoHasContainerItems { get; set; }
        public string ColorItemInfoHasAttachments { get; set; }
        public string ColorItemInfoHasBullets { get; set; }
        public string ColorItemInfoIsAttachment { get; set; }
        public string ColorItemInfoIsEquiped { get; set; }
        public string ColorItemInfoAttachments { get; set; }
        public string ColorToggleCategoriesText { get; set; }
        public string ColorCategoryCorners { get; set; }
        public string ColorCategoryBackground { get; set; }
        public string ColorPlayerStock { get; set; }
        public string ColorRequirementsNotMet { get; set; }

        public MarketMenuColours()
        {
            BaseColorVignette = "000000C8";
            BaseColorHeaders = "13171BFF";
            BaseColorLabels = "27272DFF";
            BaseColorText = "FBFCFEFF";
            BaseColorCheckboxes = "FBFCFEFF";
            BaseColorInfoSectionBackground = "2225268C";
            BaseColorTooltipsHeaders = "000000F0";
            BaseColorTooltipsBackground = "000000DC";
            ColorDecreaseQuantityButton = "DD262614";
            ColorDecreaseQuantityIcon = "DD262628";
            ColorSetQuantityButton = "C7265114";
            ColorIncreaseQuantityButton = "A0CC7114";
            ColorIncreaseQuantityIcon = "A0CC7128";
            ColorSellPanel = "27272DFF";
            ColorSellButton = "DD262628";
            ColorBuyPanel = "27272DFF";
            ColorBuyButton = "A0CC7128";
            ColorMarketIcon = "E241428C";
            ColorFilterOptionsButton = "C726518C";
            ColorFilterOptionsIcon = "C726518C";
            ColorSearchFilterButton = "C726518C";
            ColorCategoryButton = "C726518C";
            ColorCategoryCollapseIcon = "C726518C";
            ColorCurrencyDenominationText = "FFB418FF";
            ColorItemButton = "C726518C";
            ColorItemInfoIcon = "FFB418FF";
            ColorItemInfoTitle = "FFB418FF";
            ColorItemInfoHasContainerItems = "FFB418FF";
            ColorItemInfoHasAttachments = "FFB418FF";
            ColorItemInfoHasBullets = "FFB418FF";
            ColorItemInfoIsAttachment = "FFB418FF";
            ColorItemInfoIsEquiped = "FFB418FF";
            ColorItemInfoAttachments = "FFB418FF";
            ColorToggleCategoriesText = "C726518C";
            ColorCategoryCorners = "FBFCFEFF";
            ColorCategoryBackground = "222526FF";
            ColorPlayerStock = "2980B9FF";
            ColorRequirementsNotMet = "EB4635FF";
        }
    }
    public class ExpansionMarketSpawnPosition
    {
        public float[] Position { get; set; }
        public float[] Orientation { get; set; }

        [JsonIgnore]
        public string name { get; set; }
        public override string ToString()
        {
            return name;
        }
    }
}