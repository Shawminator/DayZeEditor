using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class DebugSettings
    {
        const int CurrentVersion = 2;

        public int m_Version { get; set; }
        public int ShowVehicleDebugMarkers { get; set; }
        public int DebugVehicleSync { get; set; }
        public int DebugVehicleTransformSet { get; set; }
        public int DebugVehiclePlayerNetworkBubbleMode { get; set; }
        public int ServerUpdateRateLimit { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public DebugSettings()
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

        public void SetIntValue(string mytype, int myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
    }
}
