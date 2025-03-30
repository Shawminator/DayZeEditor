using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{
    public enum LootchestOpenable
    {
        KeyOnly,
        KeyAndLockpick,
        KeyAndLockPickAndTools

    };
    public class LootChestTools
    {
        public BindingList<LCTools> LCTools { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }
    }
    public class LCTools
    {
        public string name { get; set; }
        public int time { get; set; }
        public int dmg { get; set; }
        public string desc { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
    public class LootChestTable
    {
        public int EnableDebug { get; set; }
        public int DeleteLogs { get; set; }
        public int MaxSpareMags { get; set; }
        public int RandomQuantity { get; set; }
        public BindingList<LootChestsLocations> LootChestsLocations { get; set; }
        public BindingList<LCPredefinedWeapons> LCPredefinedWeapons { get; set; }
        public BindingList<LootCategories> LootCategories { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }
        public void SetAllLists()
        {
            foreach (LootChestsLocations locations in LootChestsLocations)
            {
                locations.setSpawnpositions();
            }
        }
        public void Getalllists()
        {
            foreach (LootChestsLocations locations in LootChestsLocations)
            {
                locations.getSpawnpositions();
            }
        }
    }
    public class LootChestsLocations
    {
        public string name { get; set; }
        public int number { get; set; }
        public BindingList<string> pos { get; set; }
        public string keyclass { get; set; }
        public int openable { get; set; }
        public BindingList<string> chest { get; set; }
        public decimal LootRandomization { get; set; }
        public int light { get; set; }
        public BindingList<string> loot { get; set; }

        [JsonIgnore]
        public BindingList<Vec3PandR> _pos { get; set; }

        public void getSpawnpositions()
        {
            _pos = new BindingList<Vec3PandR>();
            foreach (string s in pos)
            {
                _pos.Add(new Vec3PandR(s));
            }
        }

        public void ImportDZE(DZE Importfile)
        {
            if (_pos == null)
                _pos = new BindingList<Vec3PandR>();
            foreach (Editorobject eo in Importfile.EditorObjects)
            {
                if (eo.DisplayName.Contains("CJ_LootChest"))
                {
                    _pos.Add(new Vec3PandR(eo.Position, eo.Orientation, true));
                }
            }
        }

        public void ImportMap(string[] fileContent)
        {
            if (_pos == null)
                _pos = new BindingList<Vec3PandR>();
            for (int i = 0; i < fileContent.Length; i++)
            {
                if (fileContent[i] == "") continue;
                string[] linesplit = fileContent[i].Split('|');
                string[] XYZ = linesplit[1].Split(' ');
                string[] YPR = linesplit[2].Split(' ');
                if (linesplit[0].Contains("CJ_LootChest"))
                {
                    _pos.Add(new Vec3PandR(XYZ + "|" + YPR));
                }
            }
        }

        public void ImportObjectSpawner(ObjectSpawnerArr newobjectspawner)
        {
            if (_pos == null)
                _pos = new BindingList<Vec3PandR>();
            foreach (SpawnObjects so in newobjectspawner.Objects)
            {
                if (so.name.Contains("CJ_LootChest"))
                {
                    _pos.Add(new Vec3PandR(so.pos, so.ypr, true));
                }
            }
        }

        public void setSpawnpositions()
        {
            pos = new BindingList<string>();
            foreach (Vec3PandR vec3PandR in _pos)
            {
                pos.Add(vec3PandR.GetString());
            }
        }
        public override string ToString()
        {
            return name;
        }
    }

    public class CJLoot
    {
        public string Classname { get; set; }
        public decimal Rarity { get; set; }

        public override string ToString()
        {
            return Classname;
        }
    }

    public class LCPredefinedWeapons
    {
        public string defname { get; set; }
        public string weapon { get; set; }
        public string magazine { get; set; }
        public BindingList<string> attachments { get; set; }
        public string optic { get; set; }
        public int opticbattery { get; set; }

        public override string ToString()
        {
            return defname;
        }
    }
    public class LootCategories
    {
        public string name { get; set; }
        public BindingList<string> Loot { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}
