using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZeLib
{
    public class PersonalStorageList
    {
        public const int CurrentVersion = 2;
        public BindingList<PersonalStorage> personalstorageList { get; set; }
        public string ExpansionPersonalStoragePath { get; set; }
        public List<PersonalStorage> Markedfordelete { get; set; }

        public List<int> UsedIDS = new List<int>();

        public PersonalStorageList()
        {
            personalstorageList = new BindingList<PersonalStorage>();
        }
        public PersonalStorageList(string Path)
        {
            ExpansionPersonalStoragePath = Path;
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
            personalstorageList = new BindingList<PersonalStorage>();
            DirectoryInfo dinfo = new DirectoryInfo(Path);
            FileInfo[] Files = dinfo.GetFiles("*.json");
            Console.WriteLine("Getting personal storage Objects....");
            Console.WriteLine(Files.Length.ToString() + " Found");
            foreach (FileInfo file in Files)
            {
                try
                {
                    bool savefile = false;
                    Console.WriteLine("\tserializing " + file.Name);
                    PersonalStorage ps = JsonSerializer.Deserialize<PersonalStorage>(File.ReadAllText(file.FullName));
                    ps.Filename = file.FullName;
                    if (ps.ConfigVersion != CurrentVersion)
                    {
                        ps.ConfigVersion = CurrentVersion;
                        savefile = true;
                    }
                    personalstorageList.Add(ps);
                    UsedIDS.Add(ps.StorageID);
                    if (savefile)
                    {
                        File.Delete(file.FullName);
                        var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                        string jsonString = JsonSerializer.Serialize(ps, options);
                        File.WriteAllText(ExpansionPersonalStoragePath + "\\" + ps.Filename + ".json", jsonString);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("there is An error in the following file\n" + file.FullName);

                }
            }
            UsedIDS.Sort();
        }

        public int GetNextID()
        {
            if (UsedIDS.Count == 0)
                return 1;
            List<int> result = Enumerable.Range(1, UsedIDS.Max() + 1).Except(UsedIDS).ToList();
            result.Sort();
            return result[0];
        }

        public void AddNewStorage(int newid)
        {
            UsedIDS.Add(newid);
            PersonalStorage newps = new PersonalStorage()
            {
                ConfigVersion = CurrentVersion,
                StorageID = newid,
                Filename = ExpansionPersonalStoragePath + "//PersonalStorage_" + newid + ".json",
                ClassName = "ExpansionPersonalStorageChest",
                DisplayName = "Personal Storage",
                DisplayIcon = "Backpack",
                Position = new decimal[] { 0,0,0},
                Orientation = new decimal[] { 0,0,0},
                QuestID = -1,
                Reputation = 0,
                Faction = "",
                IsGlobalStorage = 0,
                isDirty = true
            };
            personalstorageList.Add(newps);
        }

        public void RemovePS(PersonalStorage PSfordelete)
        {
            if (Markedfordelete == null) Markedfordelete = new List<PersonalStorage>();
            Markedfordelete.Add(PSfordelete);
            personalstorageList.Remove(PSfordelete);
            UsedIDS.Remove(PSfordelete.StorageID);
        }
    }

    public class PersonalStorage
    {
        public int ConfigVersion { get; set; }
        public int StorageID { get; set; }
        public string ClassName { get; set; }
        public string DisplayName { get; set; }
        public string DisplayIcon { get; set; }
        public decimal[] Position { get; set; }
        public decimal[] Orientation { get; set; }
        public int QuestID { get; set; }
        public int Reputation { get; set; }
        public string Faction { get; set; }
        public int IsGlobalStorage { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty = false;

        public override string ToString()
        {
            return ClassName;
        }

        public void backupandDelete(string personalStoragePath)
        {
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            string Fullfilename = personalStoragePath + "\\" + Path.GetFileNameWithoutExtension(Filename) + ".json";
            if (File.Exists(Fullfilename))
            {
                Directory.CreateDirectory(personalStoragePath + "\\Backup\\" + SaveTime);
                File.Copy(Fullfilename, personalStoragePath + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(Filename) + ".bak");
                File.Delete(Fullfilename);
            }
        }
    }

}
