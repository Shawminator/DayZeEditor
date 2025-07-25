﻿using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DayZeLib
{
    [XmlRoot("Faces")]
    public partial class TerjeFaces
    {
        [XmlIgnore]
        public string Filename { get; set; }
        [XmlIgnore]
        public bool isDirty { get; set; }


        private BindingList<TerjeFace> faceField;

        [XmlElementAttribute("Face")]
        public BindingList<TerjeFace> Face
        {
            get => faceField;
            set => faceField = value;
        }
    }

    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeFace
    {
        private string classnameField;
        private string iconField;
        private string backgroundField;
        private bool backgroundFieldSpecified;

        private TerjeConditions ConditionsField;

        [XmlAttributeAttribute()]
        public string classname
        {
            get => classnameField;
            set => classnameField = value;
        }

        [XmlAttributeAttribute()]
        public string icon
        {
            get => iconField;
            set => iconField = value;
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string background
        {
            get { return this.backgroundField; }
            set { this.backgroundField = value; }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool backgroundSpecified
        {
            get { return this.backgroundFieldSpecified; }
            set { this.backgroundFieldSpecified = value; }
        }

        [System.Xml.Serialization.XmlElementAttribute("Conditions")]
        public TerjeConditions Conditions
        {
            get
            {
                return this.ConditionsField;
            }
            set
            {
                this.ConditionsField = value;
            }
        }
    }
}
