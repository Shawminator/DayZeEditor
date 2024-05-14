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
    public enum QuExpansionQuestObjectiveTypeestType
    {
        NONE = 1,
        TARGET = 2,
        TRAVEL = 3,
        COLLECT = 4,
        DELIVERY = 5,
        TREASUREHUNT = 6,
        AIPATROL = 7,
        AICAMP = 8,
        AIVIP = 9,
        ACTION = 10,
        CRAFTING = 11,
        SCRIPTED = 12
    }

    public class QuestObjectivesBase : IEquatable<QuestObjectivesBase>
    {
        const int m_currentConfigVersion = 27;

        [JsonIgnore]
        public static readonly string[] Objectvetypesname = {"","", "Target", "Travel", "Collection", "Delivery", "TreasureHunt", "AIPatrol", "AICamp", "AIVIP", "Action", "Crafting"};
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public string OriginalFilename { get; set; }
        [JsonIgnore]
        public bool isDirty = false;
        [JsonIgnore]
        public QuExpansionQuestObjectiveTypeestType _ObjectiveTypeEnum { get; set; }

        public int ConfigVersion { get; set; }
        public int ID { get; set; }
        public int ObjectiveType { get; set; }



        static public int GetconfigVersion
        {
            get
            {
                return m_currentConfigVersion;
            }
            
        }

        public string[] getfoldernames()
        {
            return Objectvetypesname;
        }
        public override string ToString()
        {
            return "ObjectiveType: " + (QuExpansionQuestObjectiveTypeestType)ObjectiveType + "  ID: "+ID.ToString();
        }
        public bool Equals(QuestObjectivesBase other)
        {
            return this.ID == other.ID &&
                   this.ObjectiveType == other.ObjectiveType;
        }
        public void backupandDelete(string questObjectivesPath)
        {
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmmss");
            questObjectivesPath += "\\" + Objectvetypesname[(int)_ObjectiveTypeEnum];
            string Fullfilename = questObjectivesPath + "\\" + Filename + ".json";
            if (File.Exists(Fullfilename))
            {
                Directory.CreateDirectory(questObjectivesPath + "\\Backup\\" + SaveTime);
                File.Copy(Fullfilename, questObjectivesPath + "\\Backup\\" + SaveTime + "\\" + Filename + ".bak");
                File.Delete(Fullfilename);
            }
        }
        public virtual void SetVec3List(){}
        public virtual void GetVec3List(){}
    }
    

    public class QuestObjectives
    {
        public static readonly string[] Objectvetypesname = { "Action", "AICamp", "AIPatrol", "AIVIP", "Collection", "Crafting", "Delivery", "Target", "Travel", "TreasureHunt" };
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
                if (Directory.Exists(QuestObjectiovesPath + "\\" + m_type))
                {
                    DirectoryInfo dinfo = new DirectoryInfo(QuestObjectiovesPath + "\\" + m_type);
                    FileInfo[] Files = dinfo.GetFiles("*.json");
                    Console.WriteLine("\nGetting Quest Objective Base");
                    Console.WriteLine(Files.Length.ToString() + " Found");
                    foreach (FileInfo file in Files)
                    {
                        try
                        {
                            Console.WriteLine("\tserializing " + file.Name);
                            QuestObjectivesBase newobjective = JsonSerializer.Deserialize<QuestObjectivesBase>(File.ReadAllText(file.FullName));
                            newobjective.Filename = Path.GetFileNameWithoutExtension(file.Name);
                            newobjective.OriginalFilename = Path.GetFileNameWithoutExtension(file.Name);
                            newobjective._ObjectiveTypeEnum = (QuExpansionQuestObjectiveTypeestType)newobjective.ObjectiveType;
                            if(newobjective.ConfigVersion != QuestObjectivesBase.GetconfigVersion)
                            {
                                Console.WriteLine("\t\tUpdating " + file.Name + " to latest version " + QuestObjectivesBase.GetconfigVersion.ToString());
                                newobjective.ConfigVersion = QuestObjectivesBase.GetconfigVersion;
                                newobjective.isDirty = true;
                            }    
                            Objectives.Add(newobjective);
                        }
                        catch (Exception ex)
                        {
                            if (ex.InnerException != null)
                            {
                                Console.WriteLine("there is an error in the following file\n" + file.FullName + Environment.NewLine + ex.InnerException.Message);
                                MessageBox.Show("there is an error in the following file\n" + file.FullName + Environment.NewLine + ex.InnerException.Message);
                            }
                            else
                            {
                                Console.WriteLine(ex.Message);
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    Directory.CreateDirectory(QuestObjectiovesPath + "\\" + m_type);
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

        public int GetNextQuestID(QuExpansionQuestObjectiveTypeestType type)
        {
            List<int> Numberofobjectives = new List<int>();
            foreach (QuestObjectivesBase objectives in Objectives)
            {
                if(objectives._ObjectiveTypeEnum == type)
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

        public void Checkver()
        {
            foreach (QuestObjectivesBase obj in Objectives)
            {
                if (obj.ConfigVersion != QuestObjectivesBase.GetconfigVersion)
                {
                    obj.ConfigVersion = QuestObjectivesBase.GetconfigVersion;
                    obj.isDirty = true;
                }
            }
        }
    }
}
