using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class DebugSettings
    {
        public int m_Version { get; set; }
        public int ShowVehicleDebugMarkers { get; set; }
        public int DebugVehicleSync { get; set; }
        public int DebugVehicleTransformSet { get; set; }
        public int DebugVehiclePlayerNetworkBubbleMode { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public DebugSettings()
        {
            m_Version = 0;
            isDirty = true;
        }
        public void SetIntValue(string mytype, int myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
    }
}
