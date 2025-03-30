using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace DayZeLib
{
    public class P2PMarketList
    {
        public const int CurrentVersion = 8;
        public BindingList<p2pmarket> p2pmarketList { get; set; }
        public string Expansionp2pmarketPath { get; set; }
        public List<p2pmarket> Markedfordelete { get; set; }
        public List<int> UsedIDS = new List<int>();
        public P2PMarketList()
        {
            p2pmarketList = new BindingList<p2pmarket>();
        }

        public P2PMarketList(string Path)
        {
            Expansionp2pmarketPath = Path;
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
            p2pmarketList = new BindingList<p2pmarket>();
            DirectoryInfo dinfo = new DirectoryInfo(Path);
            FileInfo[] Files = dinfo.GetFiles("*.json");
            Console.WriteLine("Getting P2P Market configs....");
            Console.WriteLine(Files.Length.ToString() + " Found");
            foreach (FileInfo file in Files)
            {
                try
                {
                    bool savefile = false;
                    Console.WriteLine("\tserializing " + file.Name);
                    p2pmarket p2pm = JsonSerializer.Deserialize<p2pmarket>(File.ReadAllText(file.FullName));
                    p2pm.Filename = file.FullName;
                    if (p2pm.m_Version != CurrentVersion)
                    {
                        p2pm.m_Version = CurrentVersion;
                        savefile = true;
                    }
                    p2pm.getroamingWaypoint();
                    p2pmarketList.Add(p2pm);
                    UsedIDS.Add(p2pm.m_TraderID);
                    if (savefile)
                    {
                        File.Delete(file.FullName);
                        var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                        string jsonString = JsonSerializer.Serialize(p2pm, options);
                        File.WriteAllText(Expansionp2pmarketPath + "\\" + p2pm.Filename + ".json", jsonString);
                    }
                }
                catch
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

        public void AddnewP2P(int newid)
        {
            UsedIDS.Add(newid);
            p2pmarket newps = new p2pmarket()
            {
                Filename = Expansionp2pmarketPath + "//P2PTrader_" + newid + ".json",
                m_Version = CurrentVersion,
                m_TraderID = newid,
                m_ClassName = "ExpansionP2PTraderMirek",
                m_DisplayName = "Unknown",
                m_DisplayIcon = "Deliver",
                m_Position = new decimal[] { 0, 0, 0 },
                m_Orientation = new decimal[] { 0, 0, 0 },
                m_LoadoutFile = "YellowKingLoadout",
                m_VehicleSpawnPosition = new decimal[] { 0, 0, 0 },
                m_WatercraftSpawnPosition = new decimal[] { 0, 0, 0 },
                m_AircraftSpawnPosition = new decimal[] { 0, 0, 0 },
                m_Faction = "InvincibleObservers",
                m_Waypoints = new BindingList<decimal[]>(),
                Roamingwaypoints = new BindingList<Vec3>(),
                m_EmoteID = 46,
                m_EmoteIsStatic = 0,
                m_RequiredFaction = "",
                m_UseReputation = 0,
                m_MinRequiredReputation = 0,
                m_MaxRequiredReputation = 2147483647,
                m_RequiredCompletedQuestID = -1,
                m_IsGlobalTrader = 0,
                m_Currencies = new BindingList<string>(),
                m_DisplayCurrencyValue = 1,
                m_DisplayCurrencyName = "",
                isDirty = true
            };
            p2pmarketList.Add(newps);
        }

        public void Removep2pmarket(p2pmarket currentp2pmarket)
        {
            if (Markedfordelete == null) Markedfordelete = new List<p2pmarket>();
            Markedfordelete.Add(currentp2pmarket);
            p2pmarketList.Remove(currentp2pmarket);
            UsedIDS.Remove(currentp2pmarket.m_TraderID);
        }
    }
    public class p2pmarket
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty = false;
        [JsonIgnore]
        public BindingList<Vec3> Roamingwaypoints { get; set; }

        public int m_Version { get; set; }
        public int m_TraderID { get; set; }
        public string m_ClassName { get; set; }
        public decimal[] m_Position { get; set; }
        public decimal[] m_Orientation { get; set; }
        public string m_LoadoutFile { get; set; }
        public decimal[] m_VehicleSpawnPosition { get; set; }
        public decimal[] m_WatercraftSpawnPosition { get; set; }
        public decimal[] m_AircraftSpawnPosition { get; set; }
        public string m_DisplayName { get; set; }
        public string m_DisplayIcon { get; set; }
        public string m_Faction { get; set; }
        public BindingList<decimal[]> m_Waypoints { get; set; }
        public int m_EmoteID { get; set; }
        public int m_EmoteIsStatic { get; set; }
        public string m_RequiredFaction { get; set; }
        public int m_UseReputation { get;set; }
        public int m_MinRequiredReputation { get; set; }
        public int m_MaxRequiredReputation { get; set; }
        public int m_RequiredCompletedQuestID { get; set; }
        public int m_IsGlobalTrader { get; set; }
        public BindingList<string> m_Currencies { get; set; }
        public int m_DisplayCurrencyValue { get; set; }
        public string m_DisplayCurrencyName { get; set; }

        public p2pmarket()
        {
            m_Waypoints = new BindingList<decimal[]>();
            Roamingwaypoints = new BindingList<Vec3>();
        }

        public override string ToString()
        {
            return "P2p Tarder ID:" + m_TraderID + " " + m_ClassName;
        }

        public void backupandDelete(string p2PMarketPath)
        {
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");

            string Fullfilename = p2PMarketPath + "\\" + Path.GetFileNameWithoutExtension(Filename) + ".json";
            if (File.Exists(Fullfilename))
            {
                Directory.CreateDirectory(p2PMarketPath + "\\Backup\\" + SaveTime);
                File.Copy(Fullfilename, p2PMarketPath + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(Filename) + ".bak");
                File.Delete(Fullfilename);
            }
        }

        internal void getroamingWaypoint()
        {
            Roamingwaypoints = new BindingList<Vec3>();
            foreach (decimal[] array in m_Waypoints)
            {
                Roamingwaypoints.Add(new Vec3()
                {
                    X = Convert.ToSingle(array[0]),
                    Y = Convert.ToSingle(array[1]),
                    Z = Convert.ToSingle(array[2])
                });
            }
        }
        public void setRoamingwaypoints()
        {
            m_Waypoints = new BindingList<decimal[]>();
            foreach (Vec3 vec3 in Roamingwaypoints)
            {
                m_Waypoints.Add(new decimal[]
                {
                    Convert.ToDecimal(vec3.X),
                    Convert.ToDecimal(vec3.Y),
                    Convert.ToDecimal(vec3.Z)
                });
            }
        }

        public string GetExpansionMapPosition()
        {
            return m_Position[0].ToString("F6") + " " + m_Position[1].ToString("F6") + " " + m_Position[2].ToString("F6");
        }
        public string getExpansionMapOrientation()
        {
            return m_Orientation[0].ToString("F6") + " " + m_Orientation[1].ToString("F6") + " " + m_Orientation[2].ToString("F6");
        }

        public float[] GetExpansionMapPositionfloatarray()
        {
            return new float[] { (float)m_Position[0], (float)m_Position[1], (float)m_Position[2] };
        }
        public float[] getExpansionMapOrientationfloatarray()
        {
            return new float[] { (float)m_Orientation[0], (float)m_Orientation[1], (float)m_Orientation[2] };
        }
    }
}
