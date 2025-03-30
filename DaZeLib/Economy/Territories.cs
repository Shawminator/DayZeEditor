using System.ComponentModel;

namespace DayZeLib
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute("territory-type", Namespace = "", IsNullable = false)]
    public partial class territorytype
    {

        public territorytype()
        {
            territory = new BindingList<territorytypeTerritory>();
        }

        private BindingList<territorytypeTerritory> territoryField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("territory")]
        public BindingList<territorytypeTerritory> territory
        {
            get
            {
                return this.territoryField;
            }
            set
            {
                this.territoryField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class territorytypeTerritory
    {

        private BindingList<territorytypeTerritoryZone> zoneField;

        private long colorField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("zone")]
        public BindingList<territorytypeTerritoryZone> zone
        {
            get
            {
                return this.zoneField;
            }
            set
            {
                this.zoneField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public long color
        {
            get
            {
                return this.colorField;
            }
            set
            {
                this.colorField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class territorytypeTerritoryZone
    {

        private string nameField;

        private int sminField;

        private int smaxField;

        private int dminField;

        private int dmaxField;

        private decimal xField;

        private decimal yField;

        private bool yFieldSpecified;

        private decimal zField;

        private decimal rField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int smin
        {
            get
            {
                return this.sminField;
            }
            set
            {
                this.sminField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int smax
        {
            get
            {
                return this.smaxField;
            }
            set
            {
                this.smaxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int dmin
        {
            get
            {
                return this.dminField;
            }
            set
            {
                this.dminField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int dmax
        {
            get
            {
                return this.dmaxField;
            }
            set
            {
                this.dmaxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal x
        {
            get
            {
                return this.xField;
            }
            set
            {
                this.xField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal y
        {
            get
            {
                return this.yField;
            }
            set
            {
                this.yField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ySpecified
        {
            get
            {
                return this.yFieldSpecified;
            }
            set
            {
                this.yFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal z
        {
            get
            {
                return this.zField;
            }
            set
            {
                this.zField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal r
        {
            get
            {
                return this.rField;
            }
            set
            {
                this.rField = value;
            }
        }
        public override string ToString()
        {
            return name;
        }
        public bool Equals(territorytypeTerritoryZone p)
        {
            if ((object)p == null)
                return false;

            return (name == p.name) && (smin == p.smin) && (smax == p.smax) && (dmin == p.dmin) && (dmax == p.dmax) && (x == p.x) && (z == p.z) && (r == p.r);
        }
    }


}
