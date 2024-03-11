using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
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
            foreach (Editorobject eo in Importfile.EditorObjects)
            {
                if(eo.DisplayName == "")
                {
                    string floatarry = eo.Position[0].ToString() + " " + eo.Position[1].ToString() + " " + eo.Position[2];
                    newlocation.triggerpos = floatarry;
                }
                else if(eo.DisplayName == "")
                {
                    string floatarry = eo.Position[0].ToString() + " " + eo.Position[1].ToString() + " " + eo.Position[2];
                    newlocation.spawnerpos.Add(floatarry);
                }
            }
            BubakLocations.Add(newlocation);
            isDirty = true;
        }
        public void removeLocation(Bubaklocation location) 
        {
            BubakLocations.Remove(location);
            isDirty = true;
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
            notification =  "";
            notificationtime = 2;
            triggerdelay = 3600;
            spawnerpos = new BindingList<string>();
            spawnradius = (decimal)0.0;
            bubaknum= 4;
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
            return new int[] { Convert.ToInt32(fasplit[0]), Convert.ToInt32(fasplit[1])};
        }
        public void setworkinghours(int[] _workinghours)
        {
            workinghours = _workinghours[0].ToString() + "-" + workinghours[1].ToString();
        }
        public BukakuPosRot gettriggerposition()
        {
            decimal[] Pos = new decimal[] { 0, 0, 0 };
            decimal[] Rot = new decimal[] { 0, 0, 0 };
            bool rotspecified = false;
            if (triggerpos.Contains('|'))
            {
                string[] posrotstring = triggerpos.Split('|');
                string[] possplit = posrotstring[0].Split(' ');
                Pos = new decimal[] { Convert.ToDecimal(possplit[0]), Convert.ToDecimal(possplit[1]), Convert.ToDecimal(possplit[2]) };
                string[] rotsplit = posrotstring[1].Split(' ');
                Rot = new decimal[] { Convert.ToDecimal(rotsplit[0]), Convert.ToDecimal(rotsplit[1]), Convert.ToDecimal(rotsplit[2]) };
                rotspecified = true;
            }
            else
            {
                string[] possplit = triggerpos.Split(' ');
                Pos = new decimal[] { Convert.ToDecimal(possplit[0]), Convert.ToDecimal(possplit[1]), Convert.ToDecimal(possplit[2]) };
            }
            return new BukakuPosRot()
            {
                Position = Pos,
                RotationSpecifioed = rotspecified,
                Rotation = Rot
            };
        }
        public void setTriggerposition(BukakuPosRot _posrot)
        {
            string posrot = "";
            if (_posrot.RotationSpecifioed)
            {
                posrot = _posrot.Position[0] + " " + _posrot.Position[1] + " " + _posrot.Position[2] + "|" + _posrot.Rotation[0] + " " + _posrot.Rotation[1] + " " + _posrot.Rotation[2];
            }
            else
            {
                posrot = _posrot.Position[0] + " " + _posrot.Position[1] + " " + _posrot.Position[2];
            }
            triggerpos = posrot;
        }
        public decimal[] gettriggermins()
        {
            string[] fasplit = triggermins.Split(' ');
            return new decimal[] { Convert.ToDecimal(fasplit[0]), Convert.ToDecimal(fasplit[1]), Convert.ToDecimal(fasplit[2]) };
        }
        public void setTriggermins(decimal[] _triggermins)
        {
            triggermins = _triggermins[0] + " " + _triggermins[1] + " " + _triggermins[2];
        }
        public decimal[] gettriggermaxs()
        {
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
        public BukakuPosRot getPosRot(int index)
        {
            string posrot = bubaci[index].ToString();
            decimal[] Pos = new decimal[] { 0, 0, 0 };
            decimal[] Rot = new decimal[] { 0, 0, 0 };
            bool rotspecified = false;
            if (posrot.Contains('|'))
            {
                string[] posrotstring = posrot.Split('|');
                string[] possplit = posrotstring[0].Split(' ');
                Pos = new decimal[] { Convert.ToDecimal(possplit[0]), Convert.ToDecimal(possplit[1]), Convert.ToDecimal(possplit[2]) };
                string[] rotsplit = posrotstring[1].Split(' ');
                Rot = new decimal[] { Convert.ToDecimal(rotsplit[0]), Convert.ToDecimal(rotsplit[1]), Convert.ToDecimal(rotsplit[2]) };
                rotspecified = true;
            }
            else
            {
                string[] fasplit = posrot.Split(' ');
                Pos = new decimal[] { Convert.ToDecimal(fasplit[0]), Convert.ToDecimal(fasplit[1]), Convert.ToDecimal(fasplit[2]) };
            }
            return new BukakuPosRot()
            {
                Position = Pos,
                RotationSpecifioed = rotspecified,
                Rotation = Rot
            };
            
        }
        public void setPosRot(int index, BukakuPosRot _posrot)
        {
            string posrot = "";
            if(_posrot.RotationSpecifioed)
            {
                posrot = _posrot.Position[0] + " " + _posrot.Position[1] + " " + _posrot.Position[2] + "|" + _posrot.Rotation[0] + " " + _posrot.Rotation[1] + " " + _posrot.Rotation[2];
            }
            else
            {
                posrot = _posrot.Position[0] + " " + _posrot.Position[1] + " " + _posrot.Position[2];
            }
            bubaci[index] = posrot;
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
            Position = new decimal[] { 0, 0, 0};
            RotationSpecifioed = false;
            Rotation = new decimal[] { 0, 0, 0 };
        }

    }
}
