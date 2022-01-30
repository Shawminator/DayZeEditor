using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class cfgEffectArea
    {
        public BindingList<Areas> Areas { get; set; }
        public BindingList<float[]> SafePositions { get; set; }

        [JsonIgnore]
        public bool isDirty;
        [JsonIgnore]
        public string Filename { get; set; }
        [JsonIgnore]
        public BindingList<Position> _positions { get; set; }

        public void convertpositionstolist()
        {
            if (SafePositions == null)
                SafePositions = new BindingList<float[]>();

            _positions = new BindingList<Position>();
            for(int i = 0; i < SafePositions.Count; i++)
            {
                _positions.Add(new Position()
                {
                    X = SafePositions[i][0],
                    Z = SafePositions[i][1],
                    Name = SafePositions[i][0].ToString() + "," + SafePositions[i][1]
                }
                );
            }
        }
        public void convertlisttopositions()
        {
            SafePositions = new BindingList<float[]>();
            foreach (Position pos in _positions)
            {
                SafePositions.Add(new float[] {pos.X, pos.Z });
            }
        }
    }
    public class Position
    {
        public float X { get; set; }
        public float Z { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
    public class Areas
    {
        public string AreaName { get; set; }
        public string Type { get; set; }
        public string TriggerType { get; set; }
        public Data Data { get; set; }
        public PlayerData PlayerData { get; set; }

        public override string ToString()
        {
            return AreaName;
        }
    }
    public class Data
    {
        public int[] Pos { get; set; }
        public int Radius { get; set; }
        public int PosHeight { get; set; }
        public int NegHeight { get; set; }
        public int InnerRingCount { get; set; }
        public int InnerPartDist { get; set; }
        public int OuterRingToggle { get; set; }
        public int OuterPartDist { get; set; }
        public int OuterOffset { get; set; }
        public int VerticalLayers { get; set; }
        public int VerticalOffset { get; set; }
        public string ParticleName { get; set; }

        public void SetIntValue(string mytype, int myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
    }
    public class PlayerData
    {
        public string AroundPartName { get; set; }
        public string TinyPartName { get; set; }
        public string PPERequesterType { get; set; }
    }
}
