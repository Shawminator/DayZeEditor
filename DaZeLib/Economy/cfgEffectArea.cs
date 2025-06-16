using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DayZeLib
{
    public class cfgEffectArea
    {
        public BindingList<Areas> Areas { get; set; }
        public BindingList<decimal[]> SafePositions { get; set; }

        public cfgEffectArea()
        {
        }

        [JsonIgnore]
        public BindingList<Position> _positions { get; set; }

        public void convertpositionstolist()
        {
            if (SafePositions != null)
            {
                _positions = new BindingList<Position>();
                for (int i = 0; i < SafePositions.Count; i++)
                {
                    _positions.Add(new Position()
                    {
                        X = SafePositions[i][0],
                        Z = SafePositions[i][1],
                        Name = SafePositions[i][0].ToString("0.##") + "," + SafePositions[i][1].ToString("0.##")
                    }
                    );
                }
                SafePositions = null;
            }
        }
        public void convertlisttopositions()
        {
            if (_positions != null)
            {
                SafePositions = new BindingList<decimal[]>();
                foreach (Position pos in _positions)
                {
                    SafePositions.Add(new decimal[] { pos.X, pos.Z });
                }
            }
            else
            {
                SafePositions = null;
            }
        }
    }
    public class Position
    {
        public decimal X { get; set; }
        public decimal Z { get; set; }
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
        public decimal[] Pos { get; set; }
        public decimal Radius { get; set; }
        public decimal PosHeight { get; set; }
        public decimal NegHeight { get; set; }
        public int? InnerRingCount { get; set; }
        public int? InnerPartDist { get; set; }
        public bool? OuterRingToggle { get; set; }
        public int? OuterPartDist { get; set; }
        public int? OuterOffset { get; set; }
        public int? VerticalLayers { get; set; }
        public int? VerticalOffset { get; set; }
        public string ParticleName { get; set; }
        public int? EffectInterval { get; set; }
        public int? EffectDuration { get; set; }
        public bool? EffectModifier { get; set; }

        public void SetIntValue(string mytype, int myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
        public void SetdecimalValue(string mytype, decimal myvalue)
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
