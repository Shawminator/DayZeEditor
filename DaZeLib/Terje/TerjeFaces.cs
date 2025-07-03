using System;
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
    }
}
