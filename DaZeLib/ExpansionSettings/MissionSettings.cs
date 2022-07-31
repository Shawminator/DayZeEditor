using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DayZeLib
{ 
    public class MissionSettings
    {
        const int CurrentVersion = 2;

        public int m_Version { get; set; }
        public int Enabled { get; set; }
        public int InitialMissionStartDelay { get; set; }
        public int TimeBetweenMissions { get; set; }
        public int MinMissions { get; set; }
        public int MaxMissions { get; set; }
        public int MinPlayersToStartMissions { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }
        [JsonIgnore]
        public BindingList<object> MissionSettingFiles { get; set; }

        public MissionSettings()
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

        public void LoadIndividualMissions(string Missionpath)
        {
            MissionSettingFiles = new BindingList<object>();
            string path = Missionpath + "\\expansion\\missions";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            foreach (var file in Directory.EnumerateFiles(path, "*.json"))
            {
                switch (Path.GetFileNameWithoutExtension(file).Split('_')[0])
                {
                    case "Airdrop":
                        AirdropMissionSettingFiles Amsf = JsonSerializer.Deserialize<AirdropMissionSettingFiles>(File.ReadAllText(file));
                        Amsf.isDirty = false;
                        Amsf.Filename = file;
                        Amsf.MissionPath = Missionpath;
                        MissionSettingFiles.Add(Amsf);
                        break;

                    case "ContaminatedArea":
                        ContaminatedAreaMissionSettingFiles CAmsf = JsonSerializer.Deserialize<ContaminatedAreaMissionSettingFiles>(File.ReadAllText(file));
                        CAmsf.isDirty = false;
                        CAmsf.Filename = file;
                        CAmsf.MissionPath = Missionpath;
                        MissionSettingFiles.Add(CAmsf);
                        break;
                }
            }
        }
        public void SetIntValue(string mytype, int myvalue)
        {
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
