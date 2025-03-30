using System.ComponentModel;

namespace DayZeLib
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class eventposdef
    {

        private BindingList<eventposdefEvent> eventField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("event")]
        public BindingList<eventposdefEvent> @event
        {
            get
            {
                return this.eventField;
            }
            set
            {
                this.eventField = value;
            }
        }

        public eventposdefEventPos findeventgroup(string eventgroupname)
        {
            foreach (eventposdefEvent eventposdefEvent in @event)
            {
                foreach (eventposdefEventPos eventposdefEventPos in eventposdefEvent.pos)
                {
                    if (eventposdefEventPos.group != null && eventposdefEventPos.group == eventgroupname)
                    {
                        return eventposdefEventPos;
                    }
                }
            }
            return null;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class eventposdefEvent
    {

        private eventposdefEventZone zoneField;

        private BindingList<eventposdefEventPos> posField;

        private string nameField;

        /// <remarks/>
        public eventposdefEventZone zone
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
        [System.Xml.Serialization.XmlElementAttribute("pos")]
        public BindingList<eventposdefEventPos> pos
        {
            get
            {
                return this.posField;
            }
            set
            {
                this.posField = value;
            }
        }

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

        public override string ToString()
        {
            return name;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class eventposdefEventZone
    {

        private int sminField;

        private int smaxField;

        private int dminField;

        private int dmaxField;

        private int rField;


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
        public int r
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class eventposdefEventPos
    {

        private decimal xField;

        private decimal yField;

        private bool yFieldSpecified;

        private decimal zField;

        private decimal aField;

        private bool aFieldSpecified;

        private string groupField;

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
        public decimal a
        {
            get
            {
                return this.aField;
            }
            set
            {
                this.aField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool aSpecified
        {
            get
            {
                return this.aFieldSpecified;
            }
            set
            {
                this.aFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string group
        {
            get
            {
                return this.groupField;
            }
            set
            {
                this.groupField = value;
            }
        }

        public override string ToString()
        {
            return x.ToString() + "," + z.ToString();
        }
        public string ToExpansionMapString(float y, float a)
        {
            return x.ToString() + " " + y + " " + z.ToString() + "|" + a.ToString() + " 0.0 0.0";
        }
    }
}
