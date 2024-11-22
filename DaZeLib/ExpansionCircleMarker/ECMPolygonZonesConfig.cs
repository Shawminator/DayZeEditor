using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class ECMPolygonZonesConfig
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty { get; set; }

        public BindingList<Polygonzone> PolygonZones { get; set; }

        public void GetVec3List()
        {
            foreach(Polygonzone pz in PolygonZones)
            {
                pz.GetVec3List();
            }
        }

        public void SetVec3List()
        {
            foreach (Polygonzone pz in PolygonZones)
            {
                pz.SetVec3List();
            }
        }
    }

    public class Polygonzone
    {
        public string polyzoneName { get; set; }
        public BindingList<float[]> vertices { get; set; }
        public int polyzoneAlpha { get; set; }
        public int polyzoneRed { get; set; }
        public int polyzoneGreen { get; set; }
        public int polyzoneBlue { get; set; }
        public int polydrawPolygon { get; set; }
        public int polyisPvPZone { get; set; }
        public Zoneschedule ZoneSchedule { get; set; }
        public int EnableCustomMessages { get; set; }
        public string CustomTitle { get; set; }
        public string CustomMessageEnter { get; set; }
        public string CustomMessageExit { get; set; }
        public string CustomIcon { get; set; }
        public BindingList<string> OnlyAllowedWeapons { get; set; }

        [JsonIgnore]
        public BindingList<Vec3> _vertices { get; set; }

        internal void GetVec3List()
        {
            _vertices = new BindingList<Vec3>();
            foreach (float[] v in vertices)
            {
                _vertices.Add(new Vec3(v));
            }
        }

        internal void SetVec3List()
        {
            vertices = new BindingList<float[]>();
            foreach (Vec3 v in _vertices)
            {
                vertices.Add(v.getfloatarray());
            }
        }
    }
}
