using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DayZeLib
{
    public class variables
    {
        [XmlElement("var")]
        public BindingList<_var> _var { get; set; }
    }
    public class _var
    {
        [XmlAttribute(AttributeName = "name")]
        public string name { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string type { get; set; }
        [XmlAttribute(AttributeName = "value")]
        public string value { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}
