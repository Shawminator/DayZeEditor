using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public enum MapMarkerVisibility
    {
        [Description("Not visable")]
        Not_visible = 0,
        [Description("Visible on the World")]
        Visible_on_the_World = 2,                  // (f "m_Is3D" is set to 1, you should probably put "m_Visibility" to 2).,
        [Description("Visible on the Map only")]
        Visible_on_the_Map_only = 4,
        [Description("Visible on the Map and World")]
        Visible_on_the_Map_and_on_the_World = 6        // (If \"m_Is3D\" is set to 1, you should probably put \"m_Visibility\" to 2)."
    };
    public class MapSettings
    {
        const int CurrentVersion = 5;

        public int m_Version { get; set; } //current version 4
        public int EnableMap { get; set; }
        public int UseMapOnMapItem { get; set; }
        public int ShowPlayerPosition { get; set; }
        public int ShowMapStats { get; set; }
        public int NeedPenItemForCreateMarker { get; set; }
        public int NeedGPSItemForCreateMarker { get; set; }
        public int CanCreateMarker { get; set; }
        public int CanCreate3DMarker { get; set; }
        public int CanOpenMapWithKeyBinding { get; set; }
        public int ShowDistanceOnPersonalMarkers { get; set; }
        public int EnableHUDGPS { get; set; }
        public int NeedGPSItemForKeyBinding { get; set; }
        public int NeedMapItemForKeyBinding { get; set; }
        public int EnableServerMarkers { get; set; }
        public int ShowNameOnServerMarkers { get; set; }
        public int ShowDistanceOnServerMarkers { get; set; }
        public BindingList<ServerMarkers> ServerMarkers { get; set; }
        public int EnableHUDCompass { get; set; }
        public int NeedCompassItemForHUDCompass { get; set; }
        public int NeedGPSItemForHUDCompass { get; set; }
        public int CompassColor { get; set; }
        public int CreateDeathMarker { get; set; }
        public int PlayerLocationNotifier { get; set; }
        public int CompassBadgesColor { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public MapSettings()
        {
            m_Version = CurrentVersion;
            ServerMarkers = new BindingList<ServerMarkers>();
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
    public class ServerMarkers
    {
        public string m_UID { get; set; }
        public int m_Visibility { get; set; }
        public int m_Is3D { get; set; }
        public string m_Text { get; set; }
        public string m_IconName { get; set; }
        public int m_Color { get; set; }
        public float[] m_Position { get; set; }
        public int m_Locked { get; set; }

        public override string ToString()
        {
            return m_UID;
        }
    }
}
