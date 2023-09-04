using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Drawing.Design;

namespace DayZeLib
{
    public class PersonalStorageSettings
    {
        const int CurrentVersion = 1;

        [Browsable(false)]
        [JsonIgnore]
        public string Filename { get; set; }

        [Browsable(false)]
        [JsonIgnore]
        public bool isDirty { get; set; }

        [Browsable(false)]
        public int m_Version { get; set; }

        public int Enabled { get; set; }
        public int UsePersonalStorageCase { get; set; }
        public int MaxItemsPerStorage { get; set; }
        public BindingList<string> ExcludedClassNames { get; set; }
        public BindingList<Menucategory> MenuCategories { get; set; }

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

     public class PersonalStorageSettingsNew
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
        public StorageLevel StorageLevel01 { get; set; }
        [JsonPropertyName("2")]
        public StorageLevel StorageLevel02 { get; set; }
        [JsonPropertyName("3")]
        public StorageLevel StorageLevel03 { get; set; }
        [JsonPropertyName("4")]
        public StorageLevel StorageLevel04 { get; set; }
        [JsonPropertyName("5")]
        public StorageLevel StorageLevel05 { get; set; }
        [JsonPropertyName("6")]
        public StorageLevel StorageLevel06 { get; set; }
        [JsonPropertyName("7")]
        public StorageLevel StorageLevel07 { get; set; }
        [JsonPropertyName("8")]
        public StorageLevel StorageLevel08 { get; set; }
        [JsonPropertyName("9")]
        public StorageLevel StorageLevel09 { get; set; }
        [JsonPropertyName("10")]
        public StorageLevel StorageLevel10 { get; set; }
    }

    public class StorageLevel
    {
        public int ReputationRequirement { get; set; }
        public BindingList<string> ExcludedSlots { get; set; }
        public int AllowAttachmentCargo { get; set; }
    }
}
