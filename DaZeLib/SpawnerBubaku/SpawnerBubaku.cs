using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace DayZeLib
{
    public class SpawnerBubaku
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty;

        public int loglevel { get; set; }
        public BindingList<Bubaklocation> BubakLocations { get; set; }

        public int getLogLevel()
        {
            return loglevel;
        }
        public void SetLogLevel(int _level)
        {
            loglevel = _level;
            isDirty = true;
        }
        public void AddNewLocation()
        {
            Bubaklocation newlocation = new Bubaklocation();
            BubakLocations.Add(newlocation);
            isDirty = true;
        }
        public void AddNewLocation(DZE Importfile)
        {
            Bubaklocation newlocation = new Bubaklocation();
            newlocation.ImportDZE(Importfile, true, false);
            BubakLocations.Add(newlocation);
            isDirty = true;
        }
        public void removeLocation(Bubaklocation location)
        {
            BubakLocations.Remove(location);
            isDirty = true;
        }

        public void Getalllists()
        {
            foreach (Bubaklocation bubaklocation in BubakLocations)
            {
                bubaklocation.getTriggerposition();
                bubaklocation.getSpawnpositions();
            }
        }

        public void SetSpawnerPointFiles()
        {
            foreach (Bubaklocation bubaklocation in BubakLocations)
            {
                bubaklocation.setTriggerposition();
                bubaklocation.setSpawnpositions();
            }
        }
    }

    public class Bubaklocation
    {
        public string name { get; set; }
        public string workinghours { get; set; }
        public string triggerpos { get; set; }
        public string triggermins { get; set; }
        public string triggermaxs { get; set; }
        public decimal triggerradius { get; set; }
        public decimal triggercylradius { get; set; }
        public decimal triggercylheight { get; set; }
        public string notification { get; set; }
        public int notificationtime { get; set; }
        public int triggerdelay { get; set; }
        public BindingList<string> spawnerpos { get; set; }
        public decimal spawnradius { get; set; }
        public int bubaknum { get; set; }
        public int onlyfilluptobubaknum { get; set; }
        public int itemrandomdmg { get; set; }
        public BindingList<string> bubaci { get; set; }
        public BindingList<string> bubakinventory { get; set; }

        [JsonIgnore]
        public bool needtosetdirty { get; set; }
        [JsonIgnore]
        public Vec3PandR _triggerpos { get; set; }
        [JsonIgnore]
        public BindingList<Vec3PandR> _spawnerpos { get; set; }

        public Bubaklocation()
        {
            name = "New Location";
            workinghours = "0-24";
            triggerpos = "0.0 0.0 0.0";
            triggermins = "-1 -0.2 -1";
            triggermaxs = "1 1 1";
            triggerradius = (decimal)0.0;
            triggercylradius = (decimal)0.0;
            triggercylheight = (decimal)0.0;
            notification = "";
            notificationtime = 2;
            triggerdelay = 3600;
            spawnerpos = new BindingList<string>();
            spawnradius = (decimal)0.0;
            bubaknum = 4;
            onlyfilluptobubaknum = 0;
            itemrandomdmg = 0;
            bubaci = new BindingList<string>();
            bubakinventory = new BindingList<string>();
        }
        public string GetName()
        {
            return name;
        }
        public void Setname(string _name)
        {
            name = _name;
        }
        public int[] getworkinghours()
        {
            string[] fasplit = workinghours.Split('-');
            return new int[] { Convert.ToInt32(fasplit[0]), Convert.ToInt32(fasplit[1]) };
        }
        public void setworkinghours(int[] _workinghours)
        {
            workinghours = _workinghours[0].ToString() + "-" + _workinghours[1].ToString();
        }
        public void getTriggerposition()
        {
            _triggerpos = new Vec3PandR(triggerpos);
        }
        public void setTriggerposition()
        {
            triggerpos = _triggerpos.GetString();
        }
        public void getSpawnpositions()
        {
            _spawnerpos = new BindingList<Vec3PandR>();
            foreach (string s in spawnerpos)
            {
                _spawnerpos.Add(new Vec3PandR(s));
            }
        }
        public void setSpawnpositions()
        {
            spawnerpos = new BindingList<string>();
            foreach (Vec3PandR vec3PandR in _spawnerpos)
            {
                spawnerpos.Add(vec3PandR.GetString());
            }
        }
        public decimal[] gettriggermins()
        {
            if (triggermins == "")
            {
                var result = MessageBox.Show("TriggerMins is blank, do you want me to add default value?", "Blank Trigger Mins", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    triggermins = "-1 -0.2 -1";
                    needtosetdirty = true;
                }
                else
                {
                    return null;
                }

            }
            string[] fasplit = triggermins.Split(' ');
            return new decimal[] { Convert.ToDecimal(fasplit[0]), Convert.ToDecimal(fasplit[1]), Convert.ToDecimal(fasplit[2]) };
        }
        public void setTriggermins(decimal[] _triggermins)
        {
            triggermins = _triggermins[0] + " " + _triggermins[1] + " " + _triggermins[2];
        }
        public decimal[] gettriggermaxs()
        {
            if (triggermaxs == "")
            {
                var result = MessageBox.Show("TriggerMaxs is blank, do you want me to add default value?", "Blank Trigger Maxs", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    triggermaxs = "1 1 1";
                    needtosetdirty = true;
                }
                else
                {
                    return null;
                }

            }
            string[] fasplit = triggermaxs.Split(' ');
            return new decimal[] { Convert.ToDecimal(fasplit[0]), Convert.ToDecimal(fasplit[1]), Convert.ToDecimal(fasplit[2]) };
        }
        public void setTriggermaxs(decimal[] _triggermaxs)
        {
            triggerpos = _triggermaxs[0] + " " + _triggermaxs[1] + " " + _triggermaxs[2];
        }
        public void UpdateTriggerAndPositionsfromDze(DZE importFile)
        {
            spawnerpos = new BindingList<string>();
            foreach (Editorobject eo in importFile.EditorObjects)
            {
                if (eo.DisplayName == "")
                {
                    string floatarry = eo.Position[0].ToString() + " " + eo.Position[1].ToString() + " " + eo.Position[2];
                    triggerpos = floatarry;
                }
                else if (eo.DisplayName == "")
                {
                    string floatarry = eo.Position[0].ToString() + " " + eo.Position[1].ToString() + " " + eo.Position[2];
                    spawnerpos.Add(floatarry);
                }
            }
        }
        public void ImportDZE(DZE Importfile, bool importTrigger, bool importrotation)
        {
            if (_spawnerpos == null)
                _spawnerpos = new BindingList<Vec3PandR>();
            foreach (Editorobject eo in Importfile.EditorObjects)
            {
                if (eo.DisplayName == "GiftBox_Large_1")
                {
                    if (importTrigger)
                    {
                        _triggerpos = new Vec3PandR(eo.Position, eo.Orientation, importrotation);
                    }
                }
                else if (eo.DisplayName == "GiftBox_Small_1")
                {
                    _spawnerpos.Add(new Vec3PandR(eo.Position, eo.Orientation, importrotation));
                }
            }
        }
        public override string ToString()
        {
            return name;
        }
    }
    public class BukakuPosRot
    {
        public decimal[] Position { get; set; }
        public bool RotationSpecifioed { get; set; }
        public decimal[] Rotation { get; set; }

        public BukakuPosRot()
        {
            Position = new decimal[] { 0, 0, 0 };
            RotationSpecifioed = false;
            Rotation = new decimal[] { 0, 0, 0 };
        }

    }
}
