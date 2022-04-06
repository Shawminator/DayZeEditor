﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private eventposdefEvent[] eventField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("event")]
        public eventposdefEvent[] @event
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class eventposdefEvent
    {

        private eventposdefEventZone zoneField;

        private eventposdefEventPos[] posField;

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
        public eventposdefEventPos[] pos
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class eventposdefEventZone
    {

        private byte sminField;

        private byte smaxField;

        private byte dminField;

        private byte dmaxField;

        private byte rField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte smin
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
        public byte smax
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
        public byte dmin
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
        public byte dmax
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
        public byte r
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
    }


}