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
    public class ExpansionMapSettings
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
        public BindingList<ExpansionServerMarkerData> ServerMarkers { get; set; }
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

        public ExpansionMapSettings()
        {
            m_Version = CurrentVersion;
            EnableMap = 1;
            UseMapOnMapItem = 1;
            NeedPenItemForCreateMarker = 0;
            NeedGPSItemForCreateMarker = 0;
            ShowPlayerPosition = 1;
            ShowMapStats = 1;
            CanCreateMarker = 1;
            CanCreate3DMarker = 1;
            ShowDistanceOnPersonalMarkers = 1;

            EnableServerMarkers = 1;

            CanOpenMapWithKeyBinding = 1;

            ShowNameOnServerMarkers = 1;
            ShowDistanceOnServerMarkers = 1;

            EnableHUDGPS = 1;

            NeedGPSItemForKeyBinding = 1;
            NeedMapItemForKeyBinding = 0;

            EnableHUDCompass = 1;
            NeedCompassItemForHUDCompass = 1;
            NeedGPSItemForHUDCompass = 1;
            CompassColor = -1;
            CreateDeathMarker = 1;

            PlayerLocationNotifier = 1;

            CompassBadgesColor = -1946157056;

            DefaultChernarusMarkers();
        }
        void DefaultChernarusMarkers()
        {
            ServerMarkers = new BindingList<ExpansionServerMarkerData>()
            {
                //! Krasnostrav Airsrip Trader
                new ExpansionServerMarkerData()
                {
                    m_UID = "ServerMarker_Trader_Krasno",
                    m_Visibility = 6,
                    m_Is3D = 1,
                    m_Text = "Traders - Krasnostav Airstrip",
                    m_IconName = "Trader",
                    m_Color = -13710223,
                    m_Position = new float[]
                    {
                        11882.0f,
                        143.0f,
                        12466.0f
                    }
                },
                new ExpansionServerMarkerData()
                {
                    m_UID = "ServerMarker_Trader_Kamenka",
                    m_Visibility = 6,
                    m_Is3D = 1,
                    m_Text = "Traders - Kamenka",
                    m_IconName = "Trader",
                    m_Color = -13710223,
                    m_Position = new float[]
                    {
                        1101.0f,
                                    8.0f,
                                    2382.0f
                    },
                    m_Locked = 0,
                    m_Persist = 1
                },
                new ExpansionServerMarkerData()
                {
                    m_UID = "ServerMarker_Boats_Kamenka",
                    m_Visibility = 6,
                    m_Is3D = 1,
                    m_Text = "Boats - Kamenka",
                    m_IconName = "Boat",
                    m_Color = -13710223,
                    m_Position = new float[] {
                                    1756.0f,
                                    4.0f,
                                    2027.0f
                    },
                    m_Locked = 0,
                    m_Persist = 1
                },
                new ExpansionServerMarkerData()
                {
                    m_UID = "ServerMarker_Aircrafts_Balota",
                    m_Visibility = 6,
                    m_Is3D = 1,
                    m_Text = "Aircrafts - Balota",
                    m_IconName = "Helicopter",
                    m_Color = -13710223,
                    m_Position = new float[]{
                                    4973.0f,
                                    12.0f,
                                    2436.0f
                    },

                    m_Locked = 0,
                    m_Persist = 1
                },
                new ExpansionServerMarkerData()
                {
                    m_UID = "ServerMarker_Boats_Svetloyarsk",
                    m_Visibility = 6,
                    m_Is3D = 1,
                    m_Text = "Boats & Fishing - Svetloyarsk",
                    m_IconName = "Boat",
                    m_Color = -13710223,
                    m_Position = new float[]{
                                    14379.0f,
                                    6.0f,
                                    13256.0f
                    },
                    m_Locked = 0,
                    m_Persist = 1
                },
                new ExpansionServerMarkerData()
                {
                    m_UID = "ServerMarker_Trader_Green_Montain",
                    m_Visibility = 6,
                    m_Is3D = 1,
                    m_Text = "Trader - Green Montain",
                    m_IconName = "Trader",
                    m_Color = -13710223,
                    m_Position = new float[] {
                                    3698.0f,
                                    405.0f,
                                    5988.0f
                    },
                    m_Locked = 0,
                    m_Persist = 1
                }
            };
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
    public class ExpansionServerMarkerData
    {
        public string m_UID { get; set; }
        public int m_Visibility { get; set; }
        public int m_Is3D { get; set; }
        public string m_Text { get; set; }
        public string m_IconName { get; set; }
        public int m_Color { get; set; }
        public float[] m_Position { get; set; }
        public int m_Locked { get; set; }
        public int m_Persist { get; set; }

        public override string ToString()
        {
            return m_UID;
        }
    }
}
