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
            Console.Write("serializing " + Path.GetFileName(Filename));
            try
            {
                bool savefile = false;
                var mySerializer = new XmlSerializer(typeof(types));
                using (var myFileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    try
                    {
                        types = (types)mySerializer.Deserialize(myFileStream);
                        if (types != null)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("  OK....");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        types.type = new BindingList<typesType>(types.type.OrderBy(x => x.name).ToList());
                        List<string> typeslist = new List<string>();
                        foreach (typesType type in types.type)
                        {
                            if (type.min > type.nominal)
                            {
                                typeslist.Add(type.name);
                            }
                            if (type.usage.Count > 0)
                            {
                                int usagecount = type.usage.Count;
                                for (int i = 0; i < usagecount; i++)
                                {
                                    if (type.usage[i].name == null || type.usage[i].name == "")
                                    {
                                        type.usage.Remove(type.usage[i]);
                                        i--;
                                        usagecount--;
                                        savefile = true;
                                    }
                                }

                            }
                            if (type.value.Count > 0)
                            {
                                int valuecount = type.value.Count;
                                for (int i = 0; i < valuecount; i++)
                                {
                                    if (type.value[i].name == null && type.value[i].user == null
                                        || type.value[i].name == null && type.value[i].user == ""
                                        || type.value[i].name == "" && type.value[i].user == null)
                                    {
                                        type.value.Remove(type.value[i]);
                                        i--;
                                        valuecount--;
                                        savefile = true;
                                    }
                                }
                            }
                            if (type.category != null && type.category.name == "")
                            {
                                type.category = null;
                                savefile = true;
                            }
                        }
                        if(typeslist.Count > 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(Environment.NewLine + "### Warning ### ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Some Items within " + Path.GetFileName(filename) + " have as greater min than nominal, please fix\n" + Path.GetFileNameWithoutExtension(filename) + "_Noms.txt has been saved to\n" + Path.GetDirectoryName(filename) + "\n");
                            File.WriteAllLines(Path.GetDirectoryName(filename) + "\\" + Path.GetFileNameWithoutExtension(filename) + "_Noms.txt", typeslist.ToArray());
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("  Failed....");
                        Console.ForegroundColor = ConsoleColor.White;
                        var form = Application.OpenForms["SplashForm"];
                        if (form != null)
                        {
                            form.Invoke(new Action(() => { form.Close(); }));
                        }
                        MessageBox.Show("Error in " + Path.GetFileName(Filename) + "\n" + ex.Message.ToString() + "\n" + ex.InnerException.Message.ToString());
                    }
                }
                if (savefile == true)
                    SaveTyes();
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
                return types.type.Where(x => x.name.ToLower().Contains(Searchterm.ToLower())).ToList();
            
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
        public typesType Gettypebyname(string name)
        { 
            return types.type.FirstOrDefault(x => x.name == name); 
        }
    }
}
