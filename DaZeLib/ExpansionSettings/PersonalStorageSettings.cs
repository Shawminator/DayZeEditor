using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class PersonalStorageSettings
    {
        const int CurrentVersion = 2;

        public int m_Version { get; set; }
        public int UseCategoryMenu { get; set; }
        public Storagelevels StorageLevels { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

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

    public class Storagelevels
    {
        [JsonPropertyName("1")]
        public _1 _1 { get; set; }
        [JsonPropertyName("2")]
        public _2 _2 { get; set; }
        [JsonPropertyName("3")]
        public _3 _3 { get; set; }
        [JsonPropertyName("4")]
        public _4 _4 { get; set; }
        [JsonPropertyName("5")]
        public _5 _5 { get; set; }
        [JsonPropertyName("6")]
        public _6 _6 { get; set; }
        [JsonPropertyName("7")]
        public _7 _7 { get; set; }
        [JsonPropertyName("8")]
        public _8 _8 { get; set; }
        [JsonPropertyName("9")]
        public _9 _9 { get; set; }
        [JsonPropertyName("10")]
        public _10 _10 { get; set; }
    }

    public class _1
    {
        public int ReputationRequirement { get; set; }
        public BindingList<string> ExcludedSlots { get; set; }
        public int AllowAttachmentCargo { get; set; }
    }

    public class _2
    {
        public int ReputationRequirement { get; set; }
        public BindingList<string> ExcludedSlots { get; set; }
        public int AllowAttachmentCargo { get; set; }
    }

    public class _3
    {
        public int ReputationRequirement { get; set; }
        public BindingList<string> ExcludedSlots { get; set; }
        public int AllowAttachmentCargo { get; set; }
    }

    public class _4
    {
        public int ReputationRequirement { get; set; }
        public BindingList<string> ExcludedSlots { get; set; }
        public int AllowAttachmentCargo { get; set; }
    }

    public class _5
    {
        public int ReputationRequirement { get; set; }
        public BindingList<string> ExcludedSlots { get; set; }
        public int AllowAttachmentCargo { get; set; }
    }

    public class _6
    {
        public int ReputationRequirement { get; set; }
        public BindingList<string> ExcludedSlots { get; set; }
        public int AllowAttachmentCargo { get; set; }
    }

    public class _7
    {
        public int ReputationRequirement { get; set; }
        public BindingList<string> ExcludedSlots { get; set; }
        public int AllowAttachmentCargo { get; set; }
    }

    public class _8
    {
        public int ReputationRequirement { get; set; }
        public BindingList<string> ExcludedSlots { get; set; }
        public int AllowAttachmentCargo { get; set; }
    }

    public class _9
    {
        public int ReputationRequirement { get; set; }
        public BindingList<string> ExcludedSlots { get; set; }
        public int AllowAttachmentCargo { get; set; }
    }

    public class _10
    {
        public int ReputationRequirement { get; set; }
        public BindingList<string> ExcludedSlots { get; set; }
        public int AllowAttachmentCargo { get; set; }
    }

}
