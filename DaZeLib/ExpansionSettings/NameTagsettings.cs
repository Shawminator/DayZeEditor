using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class NameTagsettings
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

        public NameTagsettings()
        {
            m_Version = CurrentVersion;
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
}
