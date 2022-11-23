using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class AISettings
    {
        [JsonIgnore]
        const int CurrentVersion = 4;
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public int m_Version { get; set; }
        public decimal AccuracyMin { get; set; }
        public decimal AccuracyMax { get; set; }
        public decimal ThreatDistanceLimit { get; set; }
        public decimal DamageMultiplier { get; set; }
        public BindingList<string> Admins { get; set; }
        public int MaximumDynamicPatrols { get; set; }
        public int Vaulting { get; set; }
        public int Manners { get; set; }
        public BindingList<string> PlayerFactions { get; set; }

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

        public AISettings()
        {
            m_Version = CurrentVersion;
            Admins = new BindingList<string>();
        }
    }

}
