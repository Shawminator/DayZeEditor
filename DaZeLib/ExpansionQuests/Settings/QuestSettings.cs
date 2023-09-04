using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class QuestSettings
    {
        [JsonIgnore]
        public const int CurrentVersion = 10;
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public int m_Version { get; set; }
        public int EnableQuests { get; set; }
        public int EnableQuestLogTab { get; set; }
        public int CreateQuestNPCMarkers { get; set; }
        public string QuestAcceptedTitle { get; set; }
        public string QuestAcceptedText { get; set; }
        public string QuestCompletedTitle { get; set; }
        public string QuestCompletedText { get; set; }
        public string QuestFailedTitle { get; set; }
        public string QuestFailedText { get; set; }
        public string QuestCanceledTitle { get; set; }
        public string QuestCanceledText { get; set; }
        public string QuestTurnInTitle { get; set; }
        public string QuestTurnInText { get; set; }
        public string QuestObjectiveCompletedTitle { get; set; }
        public string QuestObjectiveCompletedText { get; set; }
        public string QuestCooldownTitle { get; set; }
        public string QuestCooldownText { get; set; }
        public string QuestNotInGroupTitle { get; set; }
        public string QuestNotInGroupText { get; set; }
        public string QuestNotGroupOwnerTitle { get; set; }
        public string QuestNotGroupOwnerText { get; set; }
        public int GroupQuestMode { get; set; }
        public string AchievementCompletedTitle { get; set; }
        public string AchievementCompletedText { get; set; }
        public string WeeklyResetDay { get; set; }
        public int WeeklyResetHour { get; set; }
        public int WeeklyResetMinute { get; set; }
        public int DailyResetHour { get; set; }
        public int DailyResetMinute { get; set; }
        public int UseUTCTime { get; set; }
        public int UseQuestNPCIndicators { get; set; }
        public int MaxActiveQuests { get; set; }

        public QuestSettings() 
        {
            MaxActiveQuests = -1;
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
    }
}
