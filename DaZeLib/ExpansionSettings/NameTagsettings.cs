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
        public int m_Version { get; set; }
        public int EnablePlayerTags { get; set; }
        public int PlayerTagViewRange { get; set; }
        public string PlayerTagsIcon { get; set; }
        public int ShowPlayerTagsInSafeZones { get; set; }
        public int ShowPlayerTagsInTerritories { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public NameTagsettings()
        {
            m_Version = 0;
            isDirty = true;
        }
    }
}
