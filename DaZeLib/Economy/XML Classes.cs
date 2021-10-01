using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DayZeLib
{
    public class Optional<T> where T : struct, IComparable
    {
        public Optional(T valueObject)
        {
            Value = valueObject;
        }
        public Optional()
        {
        }

        [XmlText]
        public T Value { get; set; }

        public static implicit operator T(Optional<T> objectToCast)
        {
            return objectToCast.Value;
        }
        public static implicit operator Optional<T>(T objectToCast)
        {
            return new Optional<T>(objectToCast);
        }
    }
    # region Shared Classes
    public class category
    {
        [XmlAttribute(AttributeName = "name")]
        public string name { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
    public class tag
    {
        [XmlAttribute(AttributeName = "name")]
        public string name { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
    public class usage
    {
        [XmlAttribute(AttributeName = "name")]
        public string name { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
    public class user
    {
        [XmlAttribute(AttributeName = "name")]
        public string name { get; set; }
        [XmlElement("value")]
        public BindingList<value> value { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
    public class value
    {
        [XmlAttribute(AttributeName = "name")]
        public string name { get; set; }
        [XmlAttribute(AttributeName = "user")]
        public string user { get; set; }

    }
    #endregion Shared Classes
    #region user definition files
    public class user_lists
    {
        [XmlElement("usageflags")]
        public usageflags usageflags { get; set; }
        [XmlElement("valueflags")]
        public valueflags valueflags { get; set; }
    }
    #endregion user definition files
    #region definition files
    public class lists
    {
        [XmlElement("categories")]
        public categories categories { get; set; }
        [XmlElement("tags")]
        public tags tags { get; set; }
        [XmlElement("usageflags")]
        public usageflags usageflags { get; set; }
        [XmlElement("valueflags")]
        public valueflags valueflags { get; set; }
    }
    public class categories
    {
        [XmlElement("category")]
        public BindingList<category> category { get; set; }
    }
    public class tags
    {
        [XmlElement("tag")]
        public BindingList<tag> tag { get; set; }
   }
    public class usageflags
    {
        [XmlElement("usage")]
        public BindingList<usage> usage { get; set; }
        [XmlElement("user")]
        public BindingList<user> user { get; set; }
    }
    public class valueflags
    {
        [XmlElement("value")]
        public BindingList<value> value { get; set; }
        [XmlElement("user")]
        public BindingList<user> user { get; set; }
    }
    #endregion definition files
    #region economycore
    [Serializable]
    public class file
    {
        [XmlAttribute(AttributeName = "name")]
        public string name { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string type { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
    [Serializable]
    public class ce
    {
        [XmlAttribute(AttributeName = "folder")]
        public string folder { get; set; }
        [XmlElement("file")]
        public BindingList<file> file { get; set; }

        public override string ToString()
        {
            return folder;
        }
        public file getfile(string modname)
        {
            foreach(file _file in file)
            {
                if(_file.name == modname + ".xml")
                {
                    return _file;
                }
            }
            return null;
        }
        public void removefile(file _file)
        {
            file.Remove(_file);
        }
    }
    public class _default
    {
        [XmlAttribute(AttributeName = "name")]
        public string name { get; set; }
        [XmlAttribute(AttributeName = "value")]
        public string value { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
    [Serializable]
    public class defaults
    {
        [XmlElement("default")]
        public BindingList<_default> _defaults { get; set; }
    }
    [Serializable]
    public class rootclass
    {
        [XmlAttribute(AttributeName = "name")]
        public string name { get; set; }
        [XmlAttribute(AttributeName = "reportMemoryLOD")]
        public string reportMemoryLOD { get; set; }
        [XmlAttribute(AttributeName = "act")]
        public string act { get; set; }
    }
    [Serializable]
    public class classes
    {
        [XmlElement("rootclass")]
        public BindingList<rootclass> rootclass { get; set; }
    }
    [Serializable]
    public class economycore
    {
        [XmlElement("classes")]
        public classes classes { get; set; }
        [XmlElement("defaults")]
        public defaults defaults { get; set; }
        [XmlElement("ce")]
        public BindingList<ce> ce { get; set; }

        public void Addtoce(string path, string file, string type)
        {
            if (ce == null)
                ce = new BindingList<ce>();
        }
        public ce findFile(string modname)
        {
            foreach(ce ce in ce)
            {
                if(ce.file.Any(x => x.name == modname + ".xml"))
                {
                    return ce;
                }
            }
            return null;
        }
    }
    #endregion economycore
    #region types
    public class types
    {
        [XmlElement("type")]
        public BindingList<type> type { get; set; }
    }
    [Serializable]
    public class type
    {
        [XmlAttribute(AttributeName = "name")]
        public string name { get; set; }
        [XmlElement(ElementName = "nominal", IsNullable = false)]
        public Optional<int> nominal { get; set; }
        [XmlElement(ElementName = "lifetime", IsNullable = false)]
        public Optional<int> lifetime { get; set; }
        [XmlElement(ElementName = "restock", IsNullable = false)]
        public Optional<int> restock { get; set; }
        [XmlElement(ElementName = "min", IsNullable = false)]
        public Optional<int> min { get; set; }
        [XmlElement(ElementName = "quantmin", IsNullable = false)]
        public Optional<int> quantmin { get; set; }
        [XmlElement(ElementName = "quantmax", IsNullable = false)]
        public Optional<int> quantmax { get; set; }
        [XmlElement(ElementName = "cost", IsNullable = false)]
        public Optional<int> cost { get; set; }

        [XmlElement("flags")]
        public flags flags { get; set; }
        [XmlElement("category")]
        public category category { get; set; }
        [XmlElement("tag")]
        public BindingList<tag> tag { get; set; }
        [XmlElement("usage")]
        public BindingList<usage> usage { get; set; }
        [XmlElement("value")]
        public BindingList<value> value { get; set; }
        [XmlIgnore]
        public bool Usinguserdifinitions { get; set; }

        public override string ToString()
        {
            return name;
        }
        public void AddnewUsage(usage u)
        {
            if (usage == null)
                usage = new BindingList<usage>();
            if (!usage.Any(x => x.name == u.name))
            {
                usage.Add(u);
            }
        }
        public void removeusage(usage u)
        {
            if (usage == null) return;
            usage.Remove(u);
        }
        public void Addnewtag(tag t)
        {
            if (tag == null)
                tag = new BindingList<tag>();
            if (!tag.Any(x => x.name == t.name))
            {
                tag.Add(t);
            }
        }
        public void removetag(tag t)
        {
            if (tag == null) return;
            tag.Remove(t);
        }
        public void changecategory(category c)
        {
            category = c;
        }
        public void AddTier(string tier)
        {
            if (value == null)
                value = new BindingList<value>();
            value.Add(new value() { name = tier });
            Usinguserdifinitions = false;
            for (int i = 0; i < value.Count; i++)
            {
                if (value[i].name == null)
                {
                    value.RemoveAt(i);
                    i--;
                }
            }
        }
        public void removetier(string tier)
        {
            value.Remove(value.First(X => X.name == tier));
        }
        public void AdduserTier(string tier)
        {
            if (value == null)
                value = new BindingList<value>();
            value.Add(new value() { user = tier });
            Usinguserdifinitions = true;
            for(int i = 0; i < value.Count; i++)
            {
                if(value[i].user == null)
                {
                    value.RemoveAt(i);
                    i--;
                }
            }
        }
        public void removeusertier(string tier)
        {
            value.Remove(value.First(X => X.user == tier));
        }
    }
    [Serializable]
    public class flags
    {
        [XmlAttribute(AttributeName = "count_in_cargo")]
        public int count_in_cargo { get; set; }
        [XmlAttribute(AttributeName = "count_in_hoarder")]
        public int count_in_hoarder { get; set; }
        [XmlAttribute(AttributeName = "count_in_map")]
        public int count_in_map { get; set; }
        [XmlAttribute(AttributeName = "count_in_player")]
        public int count_in_player { get; set; }
        [XmlAttribute(AttributeName = "crafted")]
        public int crafted { get; set; }
        [XmlAttribute(AttributeName = "deloot")]
        public int deloot { get; set; }

    }
    #endregion types
}
