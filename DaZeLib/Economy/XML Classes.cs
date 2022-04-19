using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DayZeLib
{
    //public class Optional<T> where T : struct, IComparable
    //{
    //    public Optional(T valueObject)
    //    {
    //        Value = valueObject;
    //    }
    //    public Optional()
    //    {
    //    }

    //    [XmlText]
    //    public T Value { get; set; }

    //    public static implicit operator T(Optional<T> objectToCast)
    //    {
    //        return objectToCast.Value;
    //    }
    //    public static implicit operator Optional<T>(T objectToCast)
    //    {
    //        return new Optional<T>(objectToCast);
    //    }
    //}
    //# region Shared Classes
    //public class category
    //{
    //    [XmlAttribute(AttributeName = "name")]
    //    public string name { get; set; }

    //    public override string ToString()
    //    {
    //        return name;
    //    }
    //}
    //public class tag
    //{
    //    [XmlAttribute(AttributeName = "name")]
    //    public string name { get; set; }

    //    public override string ToString()
    //    {
    //        return name;
    //    }
    //}
    //public class usage
    //{
    //    [XmlAttribute(AttributeName = "name")]
    //    public string name { get; set; }

    //    public override string ToString()
    //    {
    //        return name;
    //    }
    //}
    //public class user
    //{
    //    [XmlAttribute(AttributeName = "name")]
    //    public string name { get; set; }
    //    [XmlElement("value")]
    //    public BindingList<value> value { get; set; }

    //    public override string ToString()
    //    {
    //        return name;
    //    }
    //}
    //public class value
    //{
    //    [XmlAttribute(AttributeName = "name")]
    //    public string name { get; set; }
    //    [XmlAttribute(AttributeName = "user")]
    //    public string user { get; set; }

    //}
   // #endregion Shared Classes
    //#region types
    //public class types
    //{
    //    [XmlElement("type")]
    //    public BindingList<type> type { get; set; }
    //}
    //[Serializable]
    //public class type
    //{
    //    [XmlAttribute(AttributeName = "name")]
    //    public string name { get; set; }
    //    [XmlElement(ElementName = "nominal", IsNullable = false)]
    //    public Optional<int> nominal { get; set; }
    //    [XmlElement(ElementName = "lifetime", IsNullable = false)]
    //    public Optional<int> lifetime { get; set; }
    //    [XmlElement(ElementName = "restock", IsNullable = false)]
    //    public Optional<int> restock { get; set; }
    //    [XmlElement(ElementName = "min", IsNullable = false)]
    //    public Optional<int> min { get; set; }
    //    [XmlElement(ElementName = "quantmin", IsNullable = false)]
    //    public Optional<int> quantmin { get; set; }
    //    [XmlElement(ElementName = "quantmax", IsNullable = false)]
    //    public Optional<int> quantmax { get; set; }
    //    [XmlElement(ElementName = "cost", IsNullable = false)]
    //    public Optional<int> cost { get; set; }

    //    [XmlElement("flags")]
    //    public flags flags { get; set; }
    //    [XmlElement("category")]
    //    public listsCategory category { get; set; }
    //    [XmlElement("tag")]
    //    public BindingList<listsTag> tag { get; set; }
    //    [XmlElement("usage")]
    //    public BindingList<listsUsage> usage { get; set; }
    //    [XmlElement("value")]
    //    public BindingList<listsValue> value { get; set; }
    //    [XmlIgnore]
    //    public bool Usinguserdifinitions { get; set; }

    //    public override string ToString()
    //    {
    //        return name;
    //    }
    //    public void AddnewUsage(listsUsage u)
    //    {
    //        if (usage == null)
    //            usage = new BindingList<listsUsage>();
    //        if (!usage.Any(x => x.name == u.name))
    //        {
    //            usage.Add(u);
    //        }
    //    }
    //    public void removeusage(listsUsage u)
    //    {
    //        if (usage == null) return;
    //        listsUsage usagetoremove = usage.FirstOrDefault(x => x.name == u.name);
    //        if(usagetoremove != null)
    //            usage.Remove(usagetoremove);
    //    }
    //    public void Addnewtag(listsTag t)
    //    {
    //        if (tag == null)
    //            tag = new BindingList<listsTag>();
    //        if (!tag.Any(x => x.name == t.name))
    //        {
    //            tag.Add(t);
    //        }
    //    }
    //    public void removetag(listsTag t)
    //    {
    //        if (tag == null) return;
    //        listsTag tagtoremove = tag.FirstOrDefault(x => x.name == t.name);
    //        if(tagtoremove != null)
    //            tag.Remove(tagtoremove);
    //    }
    //    public void changecategory(listsCategory c)
    //    {
    //        category = c;
    //    }
    //    public void AddTier(string tier)
    //    {
    //        if (value == null)
    //            value = new BindingList<listsValue>();
    //        value.Add(new listsValue() { name = tier });
    //        Usinguserdifinitions = false;
    //        for (int i = 0; i < value.Count; i++)
    //        {
    //            if (value[i].name == null)
    //            {
    //                value.RemoveAt(i);
    //                i--;
    //            }
    //        }
    //    }
    //    public void removetier(string tier)
    //    {
    //        if (value == null) return;
    //        if (value.Any(x=> x.name == tier))
    //            value.Remove(value.First(X => X.name == tier));
    //        if (value.Count == 0)
    //            value = null;
    //    }
    //    public void AdduserTier(string tier)
    //    {
    //        //if (value == null)
    //        //    value = new BindingList<value>();
    //        //value.Add(new value() { user = tier });
    //        //Usinguserdifinitions = true;
    //        //for(int i = 0; i < value.Count; i++)
    //        //{
    //        //    if(value[i].user == null)
    //        //    {
    //        //        value.RemoveAt(i);
    //        //        i--;
    //        //    }
    //        //}
    //    }
    //    public void removeusertier(string tier)
    //    {
    //        //value.Remove(value.First(X => X.user == tier));
    //        //if (value.Count == 0)
    //        //{
    //        //    value = null;
    //        //    Usinguserdifinitions = false;
    //        //}
    //    }

    //    public void removetiers()
    //    {
    //        if (value != null)
    //            value = null;
    //    }
    //}
    //[Serializable]
    //public class flags
    //{
    //    [XmlAttribute(AttributeName = "count_in_cargo")]
    //    public int count_in_cargo { get; set; }
    //    [XmlAttribute(AttributeName = "count_in_hoarder")]
    //    public int count_in_hoarder { get; set; }
    //    [XmlAttribute(AttributeName = "count_in_map")]
    //    public int count_in_map { get; set; }
    //    [XmlAttribute(AttributeName = "count_in_player")]
    //    public int count_in_player { get; set; }
    //    [XmlAttribute(AttributeName = "crafted")]
    //    public int crafted { get; set; }
    //    [XmlAttribute(AttributeName = "deloot")]
    //    public int deloot { get; set; }

    //}
   // #endregion types
}
