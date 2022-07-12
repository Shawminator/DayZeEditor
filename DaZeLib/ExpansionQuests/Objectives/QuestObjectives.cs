using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
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
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty = false;
        [JsonIgnore]
        public QuestType QuestType { get; set; }

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
            return this.ConfigVersion == other.ConfigVersion &&
                   this.ID == other.ID &&
                   this.ObjectiveType == other.ObjectiveType;
        }
    }
    

    public class QuestObjectives
    {
        public static readonly string[] Objectvetypesname = { "Action", "AICamp", "AIPatrol", "AIVIP", "Collection", "Delivery", "Target", "Travel", "TreasureHunt" };
        public string QuestObjectiovesPath { get; set; }
        public BindingList<QuestObjectivesBase> Objectives { get; set; }

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

    }
}
