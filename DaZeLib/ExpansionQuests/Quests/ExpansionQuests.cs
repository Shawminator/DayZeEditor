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
    public enum ExpansionQuestsQuestType
    {
        NORMAL = 1
    }
    public class ExpansioQuestList
    {
        const int m_QuestConfigVersion = 21;
        public static int getQuestConfigVersion
        {
            get { return m_QuestConfigVersion; }
        }

        public BindingList<Quests> QuestList { get; set; }
        public List<Quests>Markedfordelete { get; set; }
        public string QuestsPath { get; set; }

        public ExpansioQuestList()
        {
            QuestList = new BindingList<Quests>();
        }
        public ExpansioQuestList(string m_Path, bool createfolder = true)
        {
            QuestsPath = m_Path;
            QuestList = new BindingList<Quests>();
            if (createfolder)
            {
                if (!Directory.Exists(m_Path))
                    Directory.CreateDirectory(m_Path);
            }
            DirectoryInfo dinfo = new DirectoryInfo(m_Path);
            FileInfo[] Files = dinfo.GetFiles("*.json");
            Console.WriteLine("\nGetting expansion Quests");
            Console.WriteLine(Files.Length.ToString() + " Found");
            foreach (FileInfo file in Files)
            {
                try
                {
                    Console.WriteLine("\tserializing " + file.Name);
                    Quests Quest = JsonSerializer.Deserialize<Quests>(File.ReadAllText(file.FullName));
                    Quest.Filename = Path.GetFileNameWithoutExtension(file.Name);
                    Quest.OriginalFilename = Path.GetFileNameWithoutExtension(file.Name);
                    Quest.CreateLists();
                    QuestList.Add(Quest);
                    if (Quest.ConfigVersion != getQuestConfigVersion)
                    {
                        Quest.ConfigVersion = getQuestConfigVersion;
                        Quest.isDirty = true;
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine("\t\tthere is an error in the following file\n" + file.FullName + Environment.NewLine + ex.InnerException.Message);
                        MessageBox.Show("there is an error in the following file\n" + file.FullName + Environment.NewLine + ex.InnerException.Message);
                    }
                    else
                    { 
                        Console.WriteLine("\t\t" + ex.Message);
                        MessageBox.Show(ex.Message);
                    }

                }
            }
            QuestList = new BindingList<Quests>(QuestList.OrderBy(x => x.ID).ToList());
        }
        public void RemoveQuest(Quests Questfordelete)
        {
            if (Markedfordelete == null) Markedfordelete = new List<Quests>();
            Markedfordelete.Add(Questfordelete);
            QuestList.Remove(Questfordelete);
            foreach(Quests Quests in QuestList)
            {
                if(Quests.PreQuests.Any(x => x.ID == Questfordelete.ID ))
                {
                    Quests.isDirty = true;
                    Quests.PreQuestIDs.Remove(Questfordelete.ID);
                    Quests.PreQuests.Remove(Questfordelete);
                    Quests.isDirty = true;
                }
            }
        }
        public void CreateNewQuest(int id)
        {
            string[] newdescription = new string[] { "Description on getting quest.", "Description while quest is active.", "Description when take in quest." };
            Quests newquest = new Quests()
            {
                isDirty = true,
                Filename = "Quest_" + id.ToString(),
                OriginalFilename = "Quest_" + id.ToString(),
                ConfigVersion = m_QuestConfigVersion,
                ID = id,
                Type = 1,
                Title = "New Quest with id:" + id.ToString(),
                Descriptions = new BindingList<string>(newdescription.ToList()),
                ObjectiveText = "Short objective desctiption text.",
                FollowUpQuest = -1,
                Repeatable = 0,
                IsDailyQuest = 0,
                IsWeeklyQuest = 0,
                CancelQuestOnPlayerDeath = 0,
                Autocomplete = 0,
                IsGroupQuest = 0,
                ObjectSetFileName = "",
                QuestItems = new BindingList<Questitem>(),
                Rewards = new BindingList<QuestReward>(),
                NeedToSelectReward = 0,
                RewardsForGroupOwnerOnly = 1,
                QuestGiverIDs = new BindingList<int>(),
                QuestGivers = new BindingList<ExpansionQuestNPCs>(),
                QuestTurnInIDs = new BindingList<int>(),
                QuestTurnIns = new BindingList<ExpansionQuestNPCs>(),
                IsAchievement = 0,
                Objectives = new BindingList<QuestObjectivesBase>(),
                QuestColor = 0,
                ReputationReward = 0,
                ReputationRequirement = -1,
                PreQuestIDs = new BindingList<int>(),
                PreQuests = new BindingList<Quests>(),
                RequiredFaction = "",
                FactionReward = "",
                PlayerNeedQuestItems = 0,
                DeleteQuestItems = 0,
                FactionReputationRequirements = new Dictionary<string, int>(),
                FactionReputationRewards = new Dictionary<string, int>(),
                FactionReputationRequirementsList = new BindingList<FactionQuestReps>(),
                FactionReputationRewardsList = new BindingList<FactionQuestReps>()

            };
            QuestList.Add(newquest);
        }
        public List<int> GetAllQuestIDS()
        {
            List<int> Numberofquests = new List<int>();
            foreach (Quests quest in QuestList)
            {
                Numberofquests.Add(quest.ID);
            }
            Numberofquests.Sort();
            return Numberofquests;


            //List<int> result = Enumerable.Range(1, Numberofquests.Max() + 1).Except(Numberofquests).ToList();
            //result.Sort();
            //return result[0];
        }
        public void RemoveNPCFromQuests(ExpansionQuestNPCs currentQuestNPC)
        {
            foreach(Quests quest in QuestList)
            {
                if (quest.QuestGiverIDs.Contains(currentQuestNPC.ID))
                {
                    quest.QuestGiverIDs.Remove(currentQuestNPC.ID);
                    quest.QuestGivers.Remove(currentQuestNPC);
                    quest.isDirty = true;
                }
                if (quest.QuestTurnInIDs.Contains(currentQuestNPC.ID))
                {
                    quest.QuestTurnInIDs.Remove(currentQuestNPC.ID);
                    quest.QuestTurnIns.Remove(currentQuestNPC);
                    quest.isDirty = true;
                }
                
            }
        }
        public void RemoveObjectivesfromQuests(QuestObjectivesBase basequest)
        {
            foreach (Quests quest in QuestList)
            {
                List<QuestObjectivesBase> objectivestoremove = new List<QuestObjectivesBase>();
                foreach (QuestObjectivesBase objective in quest.Objectives)
                {
                    if (objective.ObjectiveType == basequest.ObjectiveType && objective.ID == basequest.ID)
                        objectivestoremove.Add(objective);
                }
                foreach(QuestObjectivesBase objbase in objectivestoremove)
                {
                    quest.Objectives.Remove(objbase);
                    quest.isDirty = true;
                }
            }
        }
        public Quests GetQuestfromid(int id)
        {
            return QuestList.FirstOrDefault(x => x.ID == id);
        }
        public bool GetNPCLists(QuestNPCLists questNPCs)
        {
            Console.WriteLine("\nSetting up Quest Giver and Quest Turn in NPCs");
            bool needtosave = false;
            foreach(Quests q in QuestList)
            {
                needtosave = q.GetNPCLists(questNPCs);
            }
            return needtosave;
        }
        public void GetPreQuests()
        {
            Console.WriteLine("\nsetting up Pre Quests.");
            foreach(Quests q in QuestList)
            {
                List<int> toberemoved = new List<int>();
                q.PreQuests = new BindingList<Quests>();
                foreach(int prequest in q.PreQuestIDs)
                {
                    Quests quest = QuestList.FirstOrDefault(x => x.ID == prequest);
                    if (quest == null)
                    {
                        Console.WriteLine("\t\tQuest Title:" + q.Title + " Quest ID:" + q.ID.ToString() + " has a pre quest (Quest Id:" + prequest.ToString() + ") that doesnt exist,\nit will be removed from the pre quest list and the file saved.");
                        MessageBox.Show("Quest Title:" + q.Title + " Quest ID:" + q.ID.ToString() + " has a pre quest (Quest ID:" + prequest.ToString() + ") that doesnt exist,\nit will be removed from the pre quest list and the file saved.");
                        toberemoved.Add(prequest);
                    }
                    else
                        q.PreQuests.Add(quest);
                }
                foreach(int id in toberemoved)
                {
                    q.PreQuestIDs.Remove(id);
                    q.isDirty = true;
                }
            }
        }
        public void setobjectiveenums()
        {
            Console.WriteLine("\nSetting up Objective Enums.");
            foreach (Quests q in QuestList)
            {
                foreach(QuestObjectivesBase obj in q.Objectives)
                {
                    obj._ObjectiveTypeEnum = (QuExpansionQuestObjectiveTypeestType)obj.ObjectiveType;
                }
            }
        }

        public List<Quests> GetallQuests(QuestObjectivesBase objective)
        {
            List<Quests> quests = new List<Quests>();

            foreach(Quests q in QuestList)
            {
                foreach(QuestObjectivesBase obj in q.Objectives)
                {
                    if (objective.ID == obj.ID && objective.ObjectiveType == obj.ObjectiveType)
                        quests.Add(q);
                }
            }
            return quests;
        }
    }
    public class Quests
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public string OriginalFilename { get; set; }
        [JsonIgnore]
        private bool _isDirty;
        [JsonIgnore]
        public bool isDirty
        {
            get { return _isDirty; }
            set { _isDirty = value; }
        }
        [JsonIgnore]
        public bool isusingdescription { get; set; }
        [JsonIgnore]
        public BindingList<ExpansionQuestNPCs> QuestGivers;
        [JsonIgnore]
        public BindingList<ExpansionQuestNPCs> QuestTurnIns;
        [JsonIgnore]
        public BindingList<Quests> PreQuests;
        [JsonIgnore]
        public BindingList<FactionQuestReps> FactionReputationRequirementsList;
        [JsonIgnore]
        public BindingList<FactionQuestReps> FactionReputationRewardsList;

        public int ConfigVersion { get; set; }
        public int ID { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public BindingList<string> Descriptions { get; set; } 
        public string ObjectiveText { get; set; }
        public int FollowUpQuest { get; set; }
        public int Repeatable { get; set; }
        public int IsDailyQuest { get; set; }
        public int IsWeeklyQuest { get; set; }
        public int CancelQuestOnPlayerDeath { get; set; }
        public int Autocomplete { get; set; }
        public int IsGroupQuest { get; set; }
        public string ObjectSetFileName { get; set; }
        public BindingList<Questitem> QuestItems { get; set; }
        public BindingList<QuestReward> Rewards { get; set; }
        public int NeedToSelectReward { get; set; }
        public int RandomReward { get; set; }
        public int RandomRewardAmount { get; set; }
        public int RewardsForGroupOwnerOnly { get; set; }
        public BindingList<int> QuestGiverIDs { get; set; }
        public BindingList<int> QuestTurnInIDs { get; set; }
        public int IsAchievement { get; set; }
        public BindingList<QuestObjectivesBase> Objectives { get; set; }
        public int QuestColor { get; set; }
        public int ReputationReward { get; set; }
        public int ReputationRequirement { get; set; }
        public BindingList<int> PreQuestIDs { get; set; }
        public string RequiredFaction { get; set; }
        public string FactionReward { get; set; }
        public int PlayerNeedQuestItems { get; set; }
        public int DeleteQuestItems { get; set; }
        public int SequentialObjectives { get; set; }
        public Dictionary<string, int> FactionReputationRequirements { get; set; }
        public Dictionary<string, int> FactionReputationRewards {get;set;}
        public int SuppressQuestLogOnCompetion { get; set; }
        public int Active { get; set; }

        public Quests()
        {
            ConfigVersion = ExpansioQuestList.getQuestConfigVersion;
            Descriptions = new BindingList<string>();
            Objectives = new BindingList<QuestObjectivesBase>();
            QuestItems = new BindingList<Questitem>();
            Rewards = new BindingList<QuestReward>();
            FactionReputationRequirements = new Dictionary<string, int>();
            FactionReputationRewards = new Dictionary<string, int>();
        }
        public override string ToString()
        {
            return Title;
        }
        public bool GetNPCLists(QuestNPCLists questNPCs)
        {
            bool needtosave = false;
            QuestGivers = new BindingList<ExpansionQuestNPCs>();
            QuestTurnIns = new BindingList<ExpansionQuestNPCs>();
            foreach (int id in QuestGiverIDs)
            {
                ExpansionQuestNPCs npc = questNPCs.GetNPCFromID(id);
                if (npc != null)
                    QuestGivers.Add(questNPCs.GetNPCFromID(id));
                else
                {
                    MessageBox.Show("Quest Title:" + Title + " Quest ID:" + ID.ToString() + " Has a quest giver npc id (" + id.ToString() + ") that doesnt exist.\n the id will be removed and the file saved");
                    Console.WriteLine("\t\tQuest Title:" + Title + " Quest ID:" + ID.ToString() + " Has a quest giver npc id (" + id.ToString() + ") that doesnt exist.\n the id will be removed and the file saved");
                    isDirty = true;
                    needtosave = true;
                }
             }
            foreach (int id in QuestTurnInIDs)
            {
                ExpansionQuestNPCs npc = questNPCs.GetNPCFromID(id);
                if (npc != null)
                    QuestTurnIns.Add(npc);
                else
                {
                    MessageBox.Show("Quest Title:" + Title + " Quest ID:" + ID.ToString() + " Has a quest turn in npc id (" + id.ToString() + ") that doesnt exist.\n the id will be removed and the file saved");
                    Console.WriteLine("\t\tQuest Title:" + Title + " Quest ID:" + ID.ToString() + " Has a quest turn in npc id (" + id.ToString() + ") that doesnt exist.\n the id will be removed and the file saved");
                    isDirty = true;
                    needtosave = true;
                }
            }
            return needtosave;
        }
        public void SetNPCList()
        {
            QuestGiverIDs = new BindingList<int>();
            QuestTurnInIDs = new BindingList<int>();
            foreach (ExpansionQuestNPCs npc in QuestGivers)
            {
                QuestGiverIDs.Add(npc.ID);
            }
            foreach (ExpansionQuestNPCs npc in QuestTurnIns)
            {
                QuestTurnInIDs.Add(npc.ID);
            }
        }
        public void backupandDelete(string questsPath)
        {
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            string Fullfilename = questsPath + "\\" + Filename + ".json";
            if (File.Exists(Fullfilename))
            {
                Directory.CreateDirectory(questsPath + "\\Backup\\" + SaveTime);
                File.Copy(Fullfilename, questsPath + "\\Backup\\" + SaveTime + "\\" + Filename + ".bak");
                File.Delete(Fullfilename);
            }
        }

        public void SetPreQuests()
        {
            PreQuestIDs = new BindingList<int>();
            foreach(Quests q in PreQuests)
            {
                PreQuestIDs.Add(q.ID);
            }
        }
        internal void CreateLists()
        {
            FactionReputationRequirementsList = new BindingList<FactionQuestReps>();
            foreach (KeyValuePair<string, int> keyValuePair in FactionReputationRequirements)
            {
                FactionReputationRequirementsList.Add(new FactionQuestReps()
                {
                    faction = keyValuePair.Key,
                    rep = keyValuePair.Value
                });
            }
            FactionReputationRewardsList = new BindingList<FactionQuestReps>();
            foreach (KeyValuePair<string, int> keyValuePair in FactionReputationRewards)
            {
                FactionReputationRewardsList.Add(new FactionQuestReps()
                {
                    faction = keyValuePair.Key,
                    rep = keyValuePair.Value
                });
    }
        }
        public void SetLists()
        {
            FactionReputationRequirements = new Dictionary<string, int>();
            foreach(FactionQuestReps fqr in FactionReputationRequirementsList)
            {
                FactionReputationRequirements.Add(fqr.faction, fqr.rep);
            }
            FactionReputationRewards = new Dictionary<string, int>();
            foreach (FactionQuestReps fqr in FactionReputationRewardsList)
            {
                FactionReputationRewards.Add(fqr.faction, fqr.rep);
            }
        }
    }

    public class Questitem
    {
        public string ClassName { get; set; }
        public int Amount { get; set; }

        public override string ToString()
        {
            return ClassName;
        }
    }

    public class QuestReward
    {
        public string ClassName { get; set; }
        public int Amount { get; set; }
        public BindingList<string> Attachments { get; set; }
        public int DamagePercent { get; set; }
        public int HealthPercent { get; set; }
        public int QuestID { get; set; }

        public QuestReward()
        {
            Attachments = new BindingList<string>();
            QuestID = -1;
        }


        public override string ToString()
        {
            return ClassName;
        }
    }
    public class FactionQuestReps
    {
        public string faction { get; set; }
        public int rep { get; set; }

        public FactionQuestReps()
        {

        }

        public override string ToString()
        {
            return faction;
        }
    }

}
