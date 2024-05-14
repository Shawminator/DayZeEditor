using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class AirdropUpgradedSafeZones
    {
        public BindingList<Safezone> SafeZones { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty;

        public AirdropUpgradedSafeZones()
        {
            SafeZones = new BindingList<Safezone>();
        }
    }

    public class Safezone
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public int X { get; set; }
        public int Z { get; set; }
        public int Radius { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }

}
