using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text.Json.Serialization;

namespace DayZeLib
{

    public class Breachingcharge
    {
        public int CreateLogs { get; set; }
        public BindingList<Charge> Charges { get; set; }
        public BindingList<Tier> Tiers { get; set; }
        public Dictionary<string, string> DestroyableObjects { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty;

        public void ConvertDicttoList()
        {
            foreach (Tier t in Tiers)
            {
                t.TierDestroyableObjectsList = new BindingList<string>();
                foreach (KeyValuePair<string, string> DO in DestroyableObjects)
                {
                    if (DO.Value == t.Name)
                    {
                        t.TierDestroyableObjectsList.Add(DO.Key);
                    }
                }
            }
        }
        public void ConvertListToDict()
        {
            DestroyableObjects.Clear();
            foreach (Tier t in Tiers)
            {
                foreach (string DO in t.TierDestroyableObjectsList)
                {
                    DestroyableObjects.Add(DO, t.Name);
                }
            }
        }
    }
    public class Charge
    {
        public string Classname { get; set; }
        public int DamageToObjects { get; set; }
        public decimal DamageToDestroyableObjectsRadius { get; set; }
        public decimal VerticalDistanceModeObjects { get; set; }
        public decimal MaxVerticalDistanceObjects { get; set; }
        public decimal MaxDamageToPlayers { get; set; }
        public decimal MinDamageToPlayers { get; set; }
        public decimal DamageToPlayersRadius { get; set; }
        public decimal MaxDamageToPlayersRadius { get; set; }
        public decimal MaxVerticalDistancePlayers { get; set; }
        public int OnlyDestroyLocks { get; set; }
        public int DeleteObjectsDirectly { get; set; }
        public int DestroyLocksFirst { get; set; }
        public decimal PlacementDistance { get; set; }
        public decimal MinPlacementDistance { get; set; }
        public decimal ToolDamageOnDefuse { get; set; }
        public int DestroyOtherCharges { get; set; }
        public decimal TimeToPlant { get; set; }
        public decimal TimeToExplode { get; set; }
        public decimal TimeToDefuse { get; set; }
        public decimal LightBrightness { get; set; }
        public decimal LightRadius { get; set; }
        public float[] LightColorStart { get; set; }
        public float[] LightColorHalfway { get; set; }
        public float[] LightColorEnd { get; set; }
        public string BeepingSoundSet { get; set; }
        public decimal BeepingSoundEndTime { get; set; }
        public decimal SwitchInterval { get; set; }
        public string ExplosionSoundSet { get; set; }
        public BindingList<string> DefuseTools { get; set; }

        [JsonIgnore]
        public Color m_LightColorStart
        {
            get
            {
                return Helper.ConverToColor(LightColorStart);
            }
            set
            {
                LightColorStart = Helper.convertToRGBFloat(value);
            }
        }
        [JsonIgnore]
        public Color m_LightColorHalfway
        {
            get
            {
                return Helper.ConverToColor(LightColorHalfway);
            }
            set
            {
                LightColorHalfway = Helper.convertToRGBFloat(value);
            }
        }
        [JsonIgnore]
        public Color m_LightColorEnd
        {
            get
            {
                return Helper.ConverToColor(LightColorEnd);
            }
            set
            {
                LightColorEnd = Helper.convertToRGBFloat(value);
            }
        }

        public override string ToString()
        {
            return Classname;
        }
    }

    public class Tier
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public BindingList<string> AcceptedChargeTypes { get; set; }

        [JsonIgnore]
        public BindingList<string> TierDestroyableObjectsList { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

}
