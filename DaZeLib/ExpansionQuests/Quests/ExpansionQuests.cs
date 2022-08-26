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

    public class ExpansioQuestList
    {
        const int m_QuestConfigVersion = 4;
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
            Console.WriteLine("Getting expansion Quests");
            Console.WriteLine(Files.Length.ToString() + " Found");
            foreach (FileInfo file in Files)
            {
                try
                {
                    bool savefile = false;
                    Console.WriteLine("serializing " + file.Name);
                    Quests Quest = JsonSerializer.Deserialize<Quests>(File.ReadAllText(file.FullName));
                    Quest.Filename = Path.GetFileNameWithoutExtension(file.Name);
                    QuestList.Add(Quest);
                    if (savefile)
                    {
                        File.Delete(file.FullName);
                        var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                        string jsonString = JsonSerializer.Serialize(Quest, options);
                        File.WriteAllText(QuestsPath + "\\" + Quest.Filename + ".json", jsonString);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("there is an error in the following file\n" + file.FullName + Environment.NewLine + ex.InnerException.Message);

                }
            }
            QuestList = new BindingList<Quests>(QuestList.OrderBy(x => x.ID).ToList());
        }
        public void RemoveQuest(Quests Questfordelete)
        {
            if (Markedfordelete == null) Markedfordelete = new List<Quests>();
            Markedfordelete.Add(Questfordelete);
            QuestList.Remove(Questfordelete);
        }
        public void CreateNewQuest()
        {
            int id = GetNextQuestID();
            string[] newdescription = new string[] { "Description on getting quest.", "Description while quest is active.", "Description when take in quest." };
            Quests newquest = new Quests()
            {
                isDirty = true,
                Filename = "Quest_" + id.ToString(),
                ConfigVersion = m_QuestConfigVersion,
                ID = id,
                Type = 0,
                Title = "New Quest with id:" + id.ToString(),
                Descriptions = new BindingList<string>(newdescription),
                ObjectiveText = "Short objective desctiption text.",
                PreQuest = -1,
                FollowUpQuest = -1,
                QuestGiverID = -1,
                QuestTurnInID = -1,
                IsAchivement = 0,
                Repeatable = 0,
                IsDailyQuest = 0,
                IsWeeklyQuest = 0,
                CancelQuestOnPlayerDeath = 0,
                Autocomplete = 0,
                IsGroupQuest = 0,
                IsBanditQuest = 0,
                IsHeroQuest = 0,
                ObjectSetFileName = "",
                QuestClassName = "",
                Objectives = new BindingList<QuestObjectivesBase>(),
                QuestItems = new BindingList<Questitem>(),
                Rewards = new BindingList<QuestReward>(),
                NeedToSelectReward = 0,
                RewardsForGroupOwnerOnly = 1,
                HumanityReward = 0
            };
            QuestList.Add(newquest);
        }
        private int GetNextQuestID()
        {
            List<int> Numberofquests = new List<int>();
            foreach (Quests quest in QuestList)
            {
                Numberofquests.Add(quest.ID);
            }
            Numberofquests.Sort();
            List<int> result = Enumerable.Range(1, Numberofquests.Max() + 1).Except(Numberofquests).ToList();
            result.Sort();
            return result[0];
        }
        public void RemoveNPCFromQuests(ExpansionQuestNPCs currentQuestNPC)
        {
            foreach(Quests quest in QuestList)
            {
                if (quest.QuestGiverID == currentQuestNPC.ID)
                {
                    quest.QuestGiverID = -1;
                    quest.isDirty = true;
                }
                if (quest.QuestTurnInID == currentQuestNPC.ID)
                {
                    quest.QuestTurnInID = -1;
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
    }
    public class Quests
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty = false;
        [JsonIgnore]
        public bool isusingdescription { get; set; }

        public int ConfigVersion { get; set; }
        public int ID { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public BindingList<string> Descriptions { get; set; } 
        public string ObjectiveText { get; set; }
        public int PreQuest { get; set; }
        public int FollowUpQuest { get; set; }
        public int QuestGiverID { get; set; }
        public int QuestTurnInID { get; set; }
        public int IsAchivement { get; set; }
        public int Repeatable { get; set; }
        public int IsDailyQuest { get; set; }
        public int IsWeeklyQuest { get; set; }
        public int CancelQuestOnPlayerDeath { get; set; }
        public int Autocomplete { get; set; }
        public int IsGroupQuest { get; set; }
        public int IsBanditQuest { get; set; }
        public int IsHeroQuest { get; set; }
        public string ObjectSetFileName { get; set; }
        public string QuestClassName { get; set; }
        public BindingList<QuestObjectivesBase> Objectives { get; set; }
        public BindingList<Questitem> QuestItems { get; set; }
        public BindingList<QuestReward> Rewards { get; set; }
        public int NeedToSelectReward { get; set; }
        public int RewardsForGroupOwnerOnly { get; set; }
        public int HumanityReward { get; set; }
        

        public Quests()
        {
            ConfigVersion = 3;
            Descriptions = new BindingList<string>();
            Objectives = new BindingList<QuestObjectivesBase>();
            QuestItems = new BindingList<Questitem>();
            Rewards = new BindingList<QuestReward>();
        }
        public override string ToString()
        {
            return Title;
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
    }
    //public class Objective
    //{
    //    public int ConfigVersion { get; set; }
    //    public int ID { get; set; }
    //    public int ObjectiveType { get; set; }
    //    public string ObjectiveText { get; set; }
    //    public int TimeLimit { get; set; }

    //    public Objective() { }

    //    public override string ToString()
    //    {
    //        return ObjectiveText;
    //    }
    //}

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

        public QuestReward() { Attachments = new BindingList<string>(); }

        public override string ToString()
        {
            return ClassName;
        }
    }

}
