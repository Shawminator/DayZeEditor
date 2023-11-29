﻿using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class ExpansionDebugSettings
    {
        const int CurrentVersion = 3;

        public int m_Version { get; set; }
        public int DebugVehiclePlayerNetworkBubbleMode { get; set; }
        public int ServerUpdateRateLimit { get; set; }
        public int EnableProneDeathHandItemDropFix { get; set; }
        public int EnableItemDropPlacementFix { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public ExpansionDebugSettings()
        {
            m_Version = CurrentVersion;
            DebugVehiclePlayerNetworkBubbleMode = 0;
            ServerUpdateRateLimit = 0;
            EnableProneDeathHandItemDropFix = 0;
            EnableItemDropPlacementFix = 0;
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

        public void SetIntValue(string mytype, int myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
    }
}