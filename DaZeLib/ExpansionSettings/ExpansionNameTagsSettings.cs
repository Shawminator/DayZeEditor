﻿using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class ExpansionNameTagsSettings
    {
        const int CurrentVersion = 4;

        public int m_Version { get; set; }
        public int EnablePlayerTags { get; set; }
        public int PlayerTagViewRange { get; set; }
        public string PlayerTagsIcon { get; set; }
        public int PlayerTagsColor { get; set; }
        public int PlayerNameColor { get; set; }
        public int OnlyInSafeZones { get; set; }
        public int OnlyInTerritories { get; set; }
        public int ShowPlayerItemInHands { get; set; }
        public int ShowNPCTags { get; set; }
        public int ShowPlayerFaction { get; set; }
        public int UseRarityColorForItemInHands { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public ExpansionNameTagsSettings()
        {
            m_Version = CurrentVersion;
            EnablePlayerTags = 0;
            PlayerTagViewRange = 5;
            PlayerTagsIcon = "Persona";
            PlayerTagsColor = -1;
            PlayerNameColor = -1;
            OnlyInSafeZones = 0;
            OnlyInTerritories = 0;
            ShowPlayerItemInHands = 0;
            ShowNPCTags = 0;
            ShowPlayerFaction = 0;
            UseRarityColorForItemInHands = 0;
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
}
