using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DayZeLib
{
    #region Events
    public enum position
    {
        [Description("Fixed")]
        @fixed,
        [Description("Player")]
        player,
        [Description("Uniform")]
        uniform
    };
    public enum limit
    {
        [Description("Mixed")]
        mixed,
        [Description("Custom")]
        custom,
        [Description("Child")]
        child,
        [Description("Parent")]
        parent
    };
    public class events
    {
        [XmlElement("event")]
        public BindingList<DynamicEvent> DynamicEvent { get; set; }
    }
    [Serializable]
    public class DynamicEvent
    {
        [XmlAttribute(AttributeName = "name")]
        public string name { get; set; }
        [XmlElement(ElementName = "nominal", IsNullable = false)]
        public int nominal { get; set; }
        [XmlElement(ElementName = "min", IsNullable = false)]
        public int min { get; set; }
        [XmlElement(ElementName = "max", IsNullable = false)]
        public int max { get; set; }
        [XmlElement(ElementName = "lifetime", IsNullable = false)]
        public int lifetime { get; set; }
        [XmlElement(ElementName = "restock", IsNullable = false)]
        public int restock { get; set; }
        [XmlElement(ElementName = "saferadius", IsNullable = false)]
        public int saferadius { get; set; }
        [XmlElement(ElementName = "distanceradius", IsNullable = false)]
        public int distanceradius { get; set; }
        [XmlElement(ElementName = "cleanupradius", IsNullable = false)]
        public int cleanupradius { get; set; }
        [XmlElement(ElementName = "flags")]
        public eventflags flags { get; set; }
        [XmlElement(ElementName = "position")]
        public position position { get; set; }
        [XmlElement(ElementName = "limit")]
        public limit limit { get; set; }
        [XmlElement(ElementName = "active", IsNullable = false)]
        public int active { get; set; }
        [XmlElement(ElementName = "children")]
        public children children { get; set; }

        public override string ToString()
        {
            return name;
        }

        public void SetIntValue(string mytype, int myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
    }
    public class eventflags
    {
        [XmlAttribute(AttributeName = "deletable")]
        public int deletable { get; set; }
        [XmlAttribute(AttributeName = "init_random")]
        public int init_random { get; set; }
        [XmlAttribute(AttributeName = "remove_damaged")]
        public int remove_damaged { get; set; }

    }

    public class children
    {
        [XmlElement("child")]
        public BindingList<child> child { get; set; }
    }
    public class child
    {
        [XmlAttribute(AttributeName = "lootmax")]
        public int lootmax { get; set; }
        [XmlAttribute(AttributeName = "lootmin")]
        public int lootmin { get; set; }
        [XmlAttribute(AttributeName = "max")]
        public int max { get; set; }
        [XmlAttribute(AttributeName = "min")]
        public int min { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string type { get; set; }

        public override string ToString()
        {
            return type;
        }
        public void SetIntValue(string mytype, int myvalue)
        {
            GetType().GetProperty(mytype).SetValue(this, myvalue, null);
        }
    }
    #endregion events
}
