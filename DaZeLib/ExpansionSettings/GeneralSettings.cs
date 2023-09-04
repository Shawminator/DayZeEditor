using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public enum Lamps
    {
        [Description("The streets lights are off")]
        Disabled = 0,
        [Description("Currently unused. Would require you to fix a generator to make street lights work. - CURRENTLY DOESNT WORK")]
        Generators = 1,
        [Description("Street lights are emitting lights but some of them will stay off intentionnaly")]
        AlwaysOn = 2,
        [Description("Force every lights to be turned on")]
        AlwaysOnEverywhere = 3
    };
    public enum buildingIvy
    {
        [Description("No custom ivies will be added to the map")]
        Disabled = 0,
        [Description("Custom Ivies in specific locations will be added to the map")]
        Specific_locations = 1,
        [Description("on all buildings on the map, not just predefined areas")]
        All_Buildings = 2,
    };
    public class GeneralSettings
    {
        static int CurrentVersion = 12;

        public int m_Version { get; set; }
        public int DisableShootToUnlock { get; set; }
        public int EnableGravecross { get; set; }
        public int GravecrossDeleteBody { get; set; }
        public decimal GravecrossTimeThreshold { get; set; }
        public decimal GravecrossSpawnTimeDelay { get; set; }
        public CustomMapping Mapping { get; set; }
        public int EnableLamps { get; set; }
        public int EnableGenerators { get; set; }
        public int EnableLighthouses { get; set; }
        public int EnableHUDNightvisionOverlay { get; set; }
        public int DisableMagicCrosshair { get; set; }
        public int EnableAutoRun { get; set; }
        public int UnlimitedStamina { get; set; }
        public int UseDeathScreen { get; set; }
        public int UseDeathScreenStatistics { get; set; }
        public int UseNewsFeedInGameMenu { get; set; }
        public Hudcolors HUDColors { get; set; }
        public int EnableEarPlugs { get; set; }
        public string InGameMenuLogoPath { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public GeneralSettings()
        {
            m_Version = CurrentVersion;
            Mapping = new CustomMapping();
            isDirty = true;
            HUDColors = new Hudcolors();
            InGameMenuLogoPath = "set:expansion_iconset image:logo_expansion_white";
        }

        public bool checkver()
        {
            if (m_Version != CurrentVersion)
            {
                m_Version = CurrentVersion;
                isDirty = true;
                if (HUDColors == null)
                    HUDColors = new Hudcolors();
                return true;
            }
            return false;
        }

        public int getIntValue(string mytype)
        {
            return (int)GetType().GetProperty(mytype).GetValue(this);
        }
        public void SetIntValue(string mytype, int myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
        public string getcolourfromcontrol(string name)
        {
            switch (name)
            {
                case "StaminaBarColorPB":
                    return HUDColors.StaminaBarColor;
                case "NotifierDividerColorPB":
                    return HUDColors.NotifierDividerColor;
                case "TemperatureBurningColorPB":
                    return HUDColors.TemperatureBurningColor;
                case "TemperatureHotColorPB":
                    return HUDColors.TemperatureHotColor;
                case "TemperatureIdealColorPB":
                    return HUDColors.TemperatureIdealColor;
                case "TemperatureColdColorPB":
                    return HUDColors.TemperatureColdColor;
                case "TemperatureFreezingColorPB":
                    return HUDColors.TemperatureFreezingColor;
                case "NotifiersIdealColorPB":
                    return HUDColors.NotifiersIdealColor;
                case "NotifiersHalfColorPB":
                    return HUDColors.NotifiersHalfColor;
                case "NotifiersLowColorPB":
                    return HUDColors.NotifiersLowColor;
                case "ReputationBaseColorPB":
                    return HUDColors.ReputationBaseColor;
                case "ReputationMedColorPB":
                    return HUDColors.ReputationMedColor;
                case "ReputationHighColorPB":
                    return HUDColors.ReputationHighColor;
            }
            return "";
        }
        public void setcolour(string name, string Colour)
        {
            Colour = Colour.Substring(2) + Colour.Substring(0, 2);
            switch (name)
            {
                case "StaminaBarColorPB":
                    HUDColors.StaminaBarColor = Colour;
                    break;
                case "NotifierDividerColorPB":
                    HUDColors.NotifierDividerColor = Colour;
                    break;
                case "TemperatureBurningColorPB":
                    HUDColors.TemperatureBurningColor = Colour;
                    break;
                case "TemperatureHotColorPB":
                    HUDColors.TemperatureHotColor = Colour;
                    break;
                case "TemperatureIdealColorPB":
                    HUDColors.TemperatureIdealColor = Colour;
                    break;
                case "TemperatureColdColorPB":
                    HUDColors.TemperatureColdColor = Colour;
                    break;
                case "TemperatureFreezingColorPB":
                    HUDColors.TemperatureFreezingColor = Colour;
                    break;
                case "NotifiersIdealColorPB":
                    HUDColors.NotifiersIdealColor = Colour;
                    break;
                case "NotifiersHalfColorPB":
                    HUDColors.NotifiersHalfColor = Colour;
                    break;
                case "NotifiersLowColorPB":
                    HUDColors.NotifiersLowColor = Colour;
                    break;
                case "ReputationBaseColorPB":
                    HUDColors.ReputationBaseColor = Colour;
                    break;
                case "ReputationMedColorPB":
                    HUDColors.ReputationMedColor = Colour;
                    break;
                case "ReputationHighColorPB":
                    HUDColors.ReputationHighColor = Colour;
                    break;
            }
        }
    }
    public class CustomMapping
    {
        public int UseCustomMappingModule { get; set; }
        public BindingList<string> Mapping { get; set; }
        public int BuildingInteriors { get; set; }
        public BindingList<string> Interiors { get; set; }
        public int BuildingIvys { get; set; }

        public CustomMapping()
        {
            Mapping = new BindingList<string>();
            Interiors = new BindingList<string>();
        }
        public void SetIntValue(string mytype, int myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
    }

    public class Hudcolors
    {
        public string StaminaBarColor { get; set; }
        public string NotifierDividerColor { get; set; }
        public string TemperatureBurningColor { get; set; }
        public string TemperatureHotColor { get; set; }
        public string TemperatureIdealColor { get; set; }
        public string TemperatureColdColor { get; set; }
        public string TemperatureFreezingColor { get; set; }
        public string NotifiersIdealColor { get; set; }
        public string NotifiersHalfColor { get; set; }
        public string NotifiersLowColor { get; set; }
        public string ReputationBaseColor { get; set; }
        public string ReputationMedColor { get; set; }
        public string ReputationHighColor { get; set; }

        public Hudcolors()
        {
            StaminaBarColor = "FFFFFFFF";
            NotifierDividerColor = "DCDCDCFF";
            TemperatureBurningColor = "DC0000FF";
            TemperatureHotColor = "DCDC00FF";
            TemperatureIdealColor = "DCDCDCFF";
            TemperatureColdColor = "00CED1FF";
            TemperatureFreezingColor = "1E90DCFF";
            NotifiersIdealColor = "DCDCDCFF";
            NotifiersHalfColor = "DCDC00FF";
            NotifiersLowColor = "DC0000FF";
            ReputationBaseColor = "DCDCDCFF";
            ReputationMedColor = "DCDC00FF";
            ReputationHighColor = "DC0000FF";
        }
    }
}
