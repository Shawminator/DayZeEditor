using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class BookSettings
    {
        public int m_Version { get; set; } //currently 2
        public int EnableStatusTab { get; set; }
        public int EnablePartyTab { get; set; }
        public int EnableServerInfoTab { get; set; }
        public int EnableServerRulesTab { get; set; }
        public int EnableTerritoryTab { get; set; }
        public int EnableBookMenu { get; set; }
        public int CreateBookmarks { get; set; }
        public BindingList<Rulecats> RuleCategories { get; set; }
        public int DisplayServerSettingsInServerInfoTab { get; set; }
        public BindingList<SettingCategories> SettingCategories { get; set; }
        public BindingList<Links> Links { get; set; }
        public BindingList<Descript> Descriptions { get; set; }
        public BindingList<CraftingCategories> CraftingCategories { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public BookSettings()
        {
            m_Version = 2;
            RuleCategories = new BindingList<Rulecats>();
            SettingCategories = new BindingList<SettingCategories>();
            Links = new BindingList<Links>();
            Descriptions = new BindingList<Descript>();
            CraftingCategories = new BindingList<CraftingCategories>();
            isDirty = true;
        }
        public void RenameRules()
        {
            for(int i = 0; i < RuleCategories.Count; i++)
            {
                RuleCategories[i].renamerules(i+1);
            }
        }
    }
    public class Rulecats
    {
        public string CategoryName { get; set; }
        public BindingList<Rules> Rules { get; set; }

        public override string ToString()
        {
            return CategoryName;
        }
        public void renamerules(int i)
        {
            for (int j = 0; j < Rules.Count; j++)
            {
                Rules[j].RuleParagraph = i.ToString() + "." + (j + 1).ToString();
            }
        }
    }
    public class Rules
    {
        public string RuleParagraph { get; set; }
        public string RuleText { get; set; }

        public override string ToString()
        {
            return RuleParagraph;
        }
    }
    public class SettingCategories
    {
        public string CategoryName { get; set; }
        public BindingList<Settings> Settings { get; set; }

        public override string ToString()
        {
            return CategoryName;
        }
    }
    public class Settings
    {
        public string SettingTitle { get; set; }
        public string SettingText { get; set; }
        public string SettingValue { get; set; }
    }
    public class Links
    {
        public string Name { get; set; }
        public string URL { get; set; }
        public string IconName { get; set; }
        public int IconColor { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
    public class Descript
    {
        public string CategoryName { get; set; }
        public BindingList<DT> Descriptions { get; set; }
       
        public override string ToString()
        {
            return CategoryName;
        }
    }
    public class DT
    {
        public string DescriptionText { get; set; }

        [JsonIgnore]
        public string DTName { get; set; }

        public override string ToString()
        {
            return DTName;
        }
    }
    public class CraftingCategories
    {
        public string CategoryName { get; set; }
        public BindingList<string> Results { get; set; }

        public override string ToString()
        {
            return CategoryName;
        }
    }
}
