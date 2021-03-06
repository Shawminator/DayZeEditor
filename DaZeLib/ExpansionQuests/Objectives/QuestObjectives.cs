using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace DayZeLib
{
    public enum QuestType
    {
        TARGET = 2,
        TRAVEL = 3,
        COLLECT = 4,
        DELIVERY = 5,
        TREASUREHUNT = 6,
        AIPATROL = 7,
        AICAMP = 8,
        AIVIP = 9,
        ACTION = 10
    }

    public class QuestObjectivesBase : IEquatable<QuestObjectivesBase>
    {
        [JsonIgnore]
        public static readonly string[] Objectvetypesname = {"","", "Target", "Travel", "Collection", "Delivery", "TreasureHunt", "AIPatrol", "AICamp", "AIVIP", "Action"};
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty = false;
        [JsonIgnore]
        public QuestType QuestType { get; set; }

        public string[] getfoldernames()
        {
            return Objectvetypesname;
        }
        public int ConfigVersion { get; set; }
        public int ID { get; set; }
        public int ObjectiveType { get; set; }
        public string ObjectiveText { get; set; }
        public int TimeLimit { get; set; }

        public override string ToString()
        {
            return ObjectiveText + ", type:" + (QuestType)ObjectiveType + ", ID:"+ID.ToString();
        }

        public bool Equals(QuestObjectivesBase other)
        {
            return this.ID == other.ID &&
                   this.ObjectiveType == other.ObjectiveType;
        }

        public void backupandDelete(string questObjectivesPath)
        {
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            questObjectivesPath += "\\" + Objectvetypesname[(int)QuestType];
            string Fullfilename = questObjectivesPath + "\\" + Filename + ".json";
            if (File.Exists(Fullfilename))
            {
                Directory.CreateDirectory(questObjectivesPath + "\\Backup\\" + SaveTime);
                File.Copy(Fullfilename, questObjectivesPath + "\\Backup\\" + SaveTime + "\\" + Filename + ".bak");
                File.Delete(Fullfilename);
            }
        }
    }
    

    public class QuestObjectives
    {
        public static readonly string[] Objectvetypesname = { "Action", "AICamp", "AIPatrol", "AIVIP", "Collection", "Delivery", "Target", "Travel", "TreasureHunt" };
        public string QuestObjectiovesPath { get; set; }
        public BindingList<QuestObjectivesBase> Objectives { get; set; }
        public List<QuestObjectivesBase> Markedfordelete { get; set; }

        public QuestObjectives(string m_QuestObjectiovesPath)
        {
            QuestObjectiovesPath = m_QuestObjectiovesPath;
            Objectives = new BindingList<QuestObjectivesBase>();
            for (int i = 0; i < Objectvetypesname.Length; i++ )
            {
                string m_type = Objectvetypesname[i];
                DirectoryInfo dinfo = new DirectoryInfo(QuestObjectiovesPath + "\\" + m_type);
                FileInfo[] Files = dinfo.GetFiles("*.json");
                Console.WriteLine("Getting Quest Objectives");
                Console.WriteLine(Files.Length.ToString() + " Found");
                foreach (FileInfo file in Files)
                {
                    try
                    {
                        Console.WriteLine("serializing " + file.Name);
                        QuestObjectivesBase newobjective = JsonSerializer.Deserialize<QuestObjectivesBase>(File.ReadAllText(file.FullName));
                        newobjective.Filename = Path.GetFileNameWithoutExtension(file.Name);
                        newobjective.QuestType = (QuestType)newobjective.ObjectiveType;
                        Objectives.Add(newobjective);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("there is an error in the following file\n" + file.FullName + Environment.NewLine + ex.InnerException.Message);
                    }
                }
            }
        }
        public QuestObjectivesBase CheckObjectiveExists(QuestObjectivesBase objective)
        {
            foreach(QuestObjectivesBase obj in Objectives)
            {
                if (obj.Equals(objective))
                    return obj;
            }

            return null;
        }

        public int GetNextQuestID(QuestType type)
        {
            List<int> Numberofobjectives = new List<int>();
            foreach (QuestObjectivesBase objectives in Objectives)
            {
                if(objectives.QuestType == type)
                    Numberofobjectives.Add(objectives.ID);
            }
            Numberofobjectives.Sort();
            if (Numberofobjectives.Count == 0)
                return 1;
            List<int> result = Enumerable.Range(1, Numberofobjectives.Max() + 1).Except(Numberofobjectives).ToList();
            result.Sort();
            return result[0];
        }
        public void deleteObjective(QuestObjectivesBase quest)
        {
            if ( Markedfordelete == null)  Markedfordelete = new List<QuestObjectivesBase>();
             Markedfordelete.Add(quest);
            Objectives.Remove(quest);
        }
    }
}
