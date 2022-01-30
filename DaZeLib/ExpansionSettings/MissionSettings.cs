using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DayZeLib
{ 
    public class MissionSettings
    {
        public int m_Version { get; set; }
        public int Enabled { get; set; }
        public int InitialMissionStartDelay { get; set; }
        public int TimeBetweenMissions { get; set; }
        public int MinMissions { get; set; }
        public int MaxMissions { get; set; }
        public int MinPlayersToStartMissions { get; set; }
        //public BindingList<Missions> Missions { get; set; } removed in version 2


        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }
        [JsonIgnore]
        public BindingList<MissionSettingFiles> MissionSettingFiles { get; set; }

        public MissionSettings()
        {
            m_Version = 2;
            //Missions = new BindingList<Missions>();
            isDirty = true;
        }
        public void LoadIndividualMissions(string Missionpath)
        {
            MissionSettingFiles = new BindingList<MissionSettingFiles>();
            string path = Missionpath + "\\expansion\\missions";
            foreach (var file in Directory.EnumerateFiles(path, "*.json"))
            {
                MissionSettingFiles msf = JsonSerializer.Deserialize<MissionSettingFiles>(File.ReadAllText(file));
                msf.isDirty = false;
                msf.Filename = file;
                msf.MissionPath = Missionpath;
                MissionSettingFiles.Add(msf);
            }
        }
        public void SetIntValue(string mytype, int myvalue)
        {
            if(mytype == "InitialMissionStartDelay" || mytype == "TimeBetweenMissions")
                GetType().GetProperty(mytype).SetValue(this, (int)Helper.ConvertMinutesToMilliseconds(myvalue), null);
            else
                GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
    }
    public class Missions
    {
        public string MissionType { get; set; }
        public string MissionPath { get; set; }
       
        public override string ToString()
        {
            return MissionType;
        }

    }

}
