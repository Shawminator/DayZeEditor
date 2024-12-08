using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class ECMDynamicPVPZoneConfig
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public int Mi8_Crashed { get; set; }
        public int UH1Y_Crashed { get; set; }
        public int ObjectScanInterval { get; set; }
        public int EnableObjectScanning { get; set; }
        public BindingList<Objectstocreatedynamiczone> ObjectsToCreateDynamicZones { get; set; }
        public Mi8 Mi8 { get; set; }
        public UH1Y UH1Y { get; set; }
    }

    public class Mi8
    {
        public int alpha { get; set; }
        public int red { get; set; }
        public int green { get; set; }
        public int blue { get; set; }
        public decimal Radius { get; set; }
        public int drawCircle { get; set; }
        public int isPvPZone { get; set; }
        public int priority { get; set; }
    }

    public class UH1Y
    {
        public int alpha { get; set; }
        public int red { get; set; }
        public int green { get; set; }
        public int blue { get; set; }
        public decimal Radius { get; set; }
        public int drawCircle { get; set; }
        public int isPvPZone { get; set; }
        public int priority { get; set; }
    }

    public class Objectstocreatedynamiczone
    {
        public string itemName { get; set; }
        public decimal Radius { get; set; }
        public int drawCircle { get; set; }
        public int isPvPZone { get; set; }
        public int zoneAlpha { get; set; }
        public int zoneRed { get; set; }
        public int zoneGreen { get; set; }
        public int zoneBlue { get; set; }
        public int priority { get; set; }
        public BindingList<Scanzone> ScanZones { get; set; }
        public Zoneschedule ZoneSchedule { get; set; }
    }

    public class Zoneschedule
    {
        public string Days { get; set; }
        public int StartHour { get; set; }
        public int EndHour { get; set; }
    }

    public class Scanzone
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal Z { get; set; }
        public decimal Radius { get; set; }

        public override string ToString()
        {
            return X.ToString() + "," + Y.ToString() + "," + Z.ToString();
        }
    }

}
