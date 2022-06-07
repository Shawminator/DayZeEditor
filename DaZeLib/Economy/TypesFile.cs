using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace DayZeLib
{
    public class LootPart
    {
        public string name, tag;
        public int Index, nominal, lifetime, restock, min, quantmin, quantmax, cost;
        public bool count_in_cargo, count_in_hoarder, count_in_map, count_in_player, crafted, deloot, Disabled;
        public List<string> usage;
        public List<string> TierValue;
        public string category;

        public override string ToString()
        {
            return name.ToString();
        }
    }
    public class TypesFile
    {
        public string Filename { get; set; }
        public types types { get; set; }
        public bool isDirty { get; set; }
        public string modname { get; set; }

        public TypesFile (string filename)
        {
            Filename = filename;
            try
            {
                var mySerializer = new XmlSerializer(typeof(types));
                using (var myFileStream = new FileStream(filename, FileMode.Open))
                {
                    try
                    {
                        types = (types)mySerializer.Deserialize(myFileStream);
                        types.type = new BindingList<typesType>(types.type.OrderBy(x => x.name).ToList());
                    }
                    catch (Exception ex)
                    {
                        var form = Application.OpenForms["SplashForm"];
                        if (form != null)
                        {
                            form.Invoke(new Action(() => { form.Close(); }));
                        }
                        MessageBox.Show("Error in " + Path.GetFileName(Filename) + "\n" + ex.Message.ToString() + "\n" + ex.InnerException.Message.ToString());
                    }
                }
            }
            catch(Exception ex)
            {
                var form = Application.OpenForms["SplashForm"];
                if (form != null)
                {
                    form.Invoke(new Action(() => { form.Close(); }));
                }

                MessageBox.Show(ex.Message);
            }
        }
        public bool SaveTyes(string saveTime = null)
        {
            foreach (typesType t in types.type)
            {
                if (t.value != null)
                    t.value = new BindingList<typesTypeValue>(new BindingList<typesTypeValue>(t.value.OrderBy(x => x.name).ToList()));
                if (t.usage != null)
                    t.usage = new BindingList<typesTypeUsage>(new BindingList<typesTypeUsage>(t.usage.OrderBy(x => x.name).ToList()));
                if (t.tag != null)
                    t.tag = new BindingList<typesTypeTag>(new BindingList<typesTypeTag>(t.tag.OrderBy(x => x.name).ToList()));
            }

            if (saveTime != null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Filename) + "\\Backup\\" + saveTime);
                File.Copy(Filename, Path.GetDirectoryName(Filename) + "\\Backup\\" + saveTime + "\\" + Path.GetFileName(Filename), true);
            }
            var serializer = new XmlSerializer(typeof(types));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var sw = new StringWriter();
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true ,});
            serializer.Serialize(xmlWriter, types, ns);
            Console.WriteLine(sw.ToString());
            File.WriteAllText(Filename, sw.ToString());
            return true;
        }
        public List<typesType> SerachTypes(string Searchterm, bool exact = false)
        {
            if (exact)
                return types.type.Where(x => x.name == Searchterm).ToList();
            else
                return types.type.Where(x => x.name.ToLower().Contains(Searchterm)).ToList();
            
        }
        public List<typesType> SerachTypes(string[] Searchterm, bool exact = false)
        {
            List<typesType> list = new List<typesType>();
            foreach(string s in Searchterm)
            {
                list.AddRange(types.type.Where(x => x.name.ToLower().Contains(s)).ToList());
            }
            return list;

        }
    }
}
