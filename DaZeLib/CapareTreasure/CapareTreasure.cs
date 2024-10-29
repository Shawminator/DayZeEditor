using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class CapareTreasureConfig
    {
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public bool isDirty;
        public BindingList<M_Capare_Treasure_Stashs> m_Capare_Treasure_Stashs { get; set; }
        public void getstashpositions()
        {
            foreach(M_Capare_Treasure_Stashs stash in m_Capare_Treasure_Stashs)
            {
                stash.getstashposition();
            }
        }
        public void setStashPositions()
        {
            foreach (M_Capare_Treasure_Stashs stash in m_Capare_Treasure_Stashs)
            {
                stash.setstashposition();
            }
        }
    }

    public class M_Capare_Treasure_Stashs
    {
        public string StashName { get; set; }
        public Stashposition StashPosition { get; set; }
        public float TriggerRadius { get; set; }
        public string Description { get; set; }
        public string ContainerType { get; set; }
        public bool IsUnderground { get; set; }
        public BindingList<string> RewardTables { get; set; }

        public override string ToString()
        {
            return StashName;
        }
        internal void getstashposition()
        {
            StashPosition.getStashPosition();
        }
        internal void setstashposition()
        {
            StashPosition.setStashPosition();
        }
    }
    public class Stashposition
    {
        public float[] Position { get; set; }
        public float[] Rotation { get; set; }
        [JsonIgnore]
        public Vec3 _Position { get; set; }
        [JsonIgnore] 
        public Vec3 _Rotation { get; set;}
        internal void getStashPosition()
        {
            _Position = new Vec3(Position);
            _Rotation = new Vec3(Rotation);
        }
        internal void setStashPosition()
        {
            Position = _Position.getfloatarray();
            Rotation = _Rotation.getfloatarray();
        }
    }
}
