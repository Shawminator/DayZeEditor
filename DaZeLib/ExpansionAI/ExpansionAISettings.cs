using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class ExpansionAISettings
    {
        [JsonIgnore]
        const int CurrentVersion = 13;
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public int m_Version { get; set; }
        public decimal AccuracyMin { get; set; }
        public decimal AccuracyMax { get; set; }
        public decimal ThreatDistanceLimit { get; set; }
        public decimal NoiseInvestigationDistanceLimit { get; set; }
        public decimal DamageMultiplier { get; set; }
        public decimal DamageReceivedMultiplier { get; set; }
        public BindingList<string> Admins { get; set; }
        //public int MaximumDynamicPatrols { get; set; } removed in version 13
        public int Vaulting { get; set; }
        public decimal SniperProneDistanceThreshold { get; set; }
        public int Manners { get; set; }
        public int MemeLevel { get; set; }
        public int CanRecruitFriendly { get; set; }
        public int CanRecruitGuards { get; set; }
        public BindingList<string> PreventClimb { get; set; }
        public decimal FormationScale { get; set; } //added in version 13
        public BindingList<string> PlayerFactions { get; set; }
        public int LogAIHitBy { get; set; }
        public int LogAIKilled { get; set; }

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

        public ExpansionAISettings()
        {
            m_Version = CurrentVersion;
            AccuracyMin = (decimal)0.35;
            AccuracyMax = (decimal)0.95;
            ThreatDistanceLimit = (decimal)1000.0;
            NoiseInvestigationDistanceLimit = (decimal)500.0;
            DamageMultiplier = (decimal)1.0;
            Admins = new BindingList<string>();
            Vaulting = 1;
            SniperProneDistanceThreshold = (decimal)0.0;
            Manners = 0;
            MemeLevel = 1;
            CanRecruitFriendly = 1;
            CanRecruitGuards = 0;
            PreventClimb = new BindingList<string>();
            FormationScale = (decimal)1.0;
            PlayerFactions = new BindingList<string>();
            LogAIHitBy = 1;
            LogAIKilled = 1;
        }
    }

}
