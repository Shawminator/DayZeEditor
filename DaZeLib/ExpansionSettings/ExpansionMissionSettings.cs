using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DayZeLib
{ 
    public class ExpansionMissionSettings
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

        public ExpansionMissionSettings()
        {
            m_Version = CurrentVersion;
            Enabled = 1;

            InitialMissionStartDelay = 300000; // 5 minutes
            TimeBetweenMissions = 3600000; // 1 hour

            MinMissions = 0;
            MaxMissions = 1;

            MinPlayersToStartMissions = 0;
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
            Console.WriteLine("Getting Expansion Mission files....");
            DirectoryInfo dinfo = new DirectoryInfo(path);
            FileInfo[] Files = dinfo.GetFiles("*.json");
            Console.WriteLine(Files.Length.ToString() + " Found");
            foreach (FileInfo file in Files)
            {
                Console.WriteLine("\tserializing " + file.Name);
                switch (Path.GetFileNameWithoutExtension(file.FullName).Split('_')[0])
                {
                    case "Airdrop":
                        AirdropMissionSettingFiles Amsf = JsonSerializer.Deserialize<AirdropMissionSettingFiles>(File.ReadAllText(file.FullName));
                        Amsf.isDirty = false;
                        Amsf.Filename = file.FullName;
                        Amsf.MissionPath = Missionpath;
                        MissionSettingFiles.Add(Amsf);
                        break;

                    case "ContaminatedArea":
                        ContaminatedAreaMissionSettingFiles CAmsf = JsonSerializer.Deserialize<ContaminatedAreaMissionSettingFiles>(File.ReadAllText(file.FullName));
                        CAmsf.isDirty = false;
                        CAmsf.Filename = file.FullName;
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
