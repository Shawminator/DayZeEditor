using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DayZeLib
{
    [XmlRoot("General")]
    public partial class TerjeGeneral
    {
        [XmlIgnore]
        public string Filename { get; set; }
        [XmlIgnore]
        public bool isDirty { get; set; }

        private TerjeValueString backgroundImageField;
        private TerjeValueString namePageFilterField;

        public TerjeValueString BackgroundImage
        {
            get => backgroundImageField;
            set => backgroundImageField = value;
        }

        public TerjeValueString NamePageFilter
        {
            get => namePageFilterField;
            set => namePageFilterField = value;
        }
    }

    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class TerjeValueString
    {
        private string valueField;

        [XmlAttributeAttribute()]
        public string value
        {
            get => valueField;
            set => valueField = value;
        }
    }
}
