using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DayZeLib
{
    public class BoolConverter : JsonConverter<bool>
    {
        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options) =>
            writer.WriteNumberValue(value ? 1: 0);

        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Number:
                    return reader.TryGetInt64(out long l) ? Convert.ToBoolean(l) : reader.TryGetDouble(out double d) ? Convert.ToBoolean(d) : false;
                case JsonTokenType.True:
                    return true;
                case JsonTokenType.False:
                    return false;
            }
            return false;
        }
    }
    public class economycoreconfig
    {
        public economycore economycore { get; set; }
        public string Filename { get; set; }
        public economycoreconfig(string filename)
        {
            Filename = filename;
            var mySerializer = new XmlSerializer(typeof(economycore));
            // To read the file, create a FileStream.
            Console.Write("serializing " + Path.GetFileName(Filename));
            try
            {
                using (var myFileStream = new FileStream(filename, FileMode.Open, FileAccess.Read ,FileShare.Read))
                {
                    // Call the Deserialize method and cast to the object type.
                    economycore = (economycore)mySerializer.Deserialize(myFileStream);
                    if(economycore != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("  OK....");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
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
                if(ex.InnerException == null)
                    MessageBox.Show(ex.Message.ToString());
                else
                    MessageBox.Show("Error in " + Path.GetFileName(Filename) + "\n" + ex.Message.ToString() + "\n" + ex.InnerException.Message.ToString());
            }

        }
        public bool SaveEconomycore()
        {
            var serializer = new XmlSerializer(typeof(economycore));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var sw = new StringWriter();
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true });
            serializer.Serialize(xmlWriter, economycore, ns);
            Console.WriteLine(sw.ToString());
            File.WriteAllText(Filename, sw.ToString());
            return true;
        }
        public void AddCe(string path, string filename, string type)
        {
            economycoreCEFile newfile = new economycoreCEFile();
            string _path = path.Replace("\\", "/");
            switch (type)
            {
                case "types":
                    newfile.name = filename;
                    newfile.type = "types";
                    break;
                case "events":
                    newfile.name = filename;
                    newfile.type = "events";
                    break;
                case "spawnabletypes":
                    newfile.name = filename;
                    newfile.type = "spawnabletypes";
                    break;
                default:
                    break;
            }
            if (economycore.ce.Any(x => x.folder == _path))
            {
                economycore.ce.FirstOrDefault(x => x.folder == _path).file.Add(newfile);
            }
            else
            {
                economycoreCE newce = new economycoreCE
                {
                    folder = _path,
                    file = new BindingList<economycoreCEFile>()
                };
                newce.file.Add(newfile);
                if (economycore.ce == null)
                {
                    economycore.ce = new BindingList<economycoreCE>();
                }
                economycore.ce.Add(newce);
            }
        }
        public economycoreCE getfolder(string path)
        {
            return economycore.ce.FirstOrDefault(x => x.folder == path);
        }
        public void RemoveCe(string modname, out string Folderpath, out string filename, out bool deletedirectory)
        {
            economycoreCE ce = economycore.findFile(modname);
            Folderpath = ce.folder;
            economycoreCEFile file = ce.getfile(modname);
            filename = file.name;
            ce.removefile(file);
            deletedirectory = false;
            if (ce.file.Count == 0)
            {
                economycore.ce.Remove(ce);
                deletedirectory = true;
            }
        }

        public bool checkiftodelete(string modname)
        {
            economycoreCE ce = economycore.findFile(modname);
            if (ce != null)
                return false;
            return true;
        }
    }
    public class Spawnabletypesconfig
    {
        public spawnabletypes spawnabletypes { get; set; }
        public string Filename { get; set; }
        public bool isDirty = false;
        public Spawnabletypesconfig(string filename)
        {
            Filename = filename;
            if (File.Exists(Filename))
            {
                var mySerializer = new XmlSerializer(typeof(spawnabletypes));
                StringBuilder sb = new StringBuilder();
                List<string> filearray = File.ReadAllLines(Filename).ToList();
                foreach (String line in filearray)
                {
                    if (line.Contains("<!-- ---"))
                    {
                        isDirty = true;
                        continue;
                    }
                    sb.Append(line + Environment.NewLine);
                }
                using (Stream ms = Helper.GenerateStreamFromString(sb.ToString()))
                {
                    Console.Write("serializing " + Path.GetFileName(Filename));
                    try
                    {
                        spawnabletypes = (spawnabletypes)mySerializer.Deserialize(ms);
                        if (spawnabletypes != null)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("  OK....");
                            Console.ForegroundColor = ConsoleColor.White;
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
            }
            else
            {
                Console.WriteLine(Path.GetFileName(Filename) + "Does not exist, Creating new empty file.");
                spawnabletypes = new spawnabletypes();
                Savespawnabletypes();
            }
        }
        public void Savespawnabletypes(string saveTime = null)
        {
            if (saveTime != null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Filename) + "\\Backup\\" + saveTime);
                File.Copy(Filename, Path.GetDirectoryName(Filename) + "\\Backup\\" + saveTime + "\\" + Path.GetFileName(Filename), true);
            }
            var serializer = new XmlSerializer(typeof(spawnabletypes));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var sw = new StringWriter();
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, });
            serializer.Serialize(xmlWriter, spawnabletypes, ns);
            Console.WriteLine(sw.ToString());
            File.WriteAllText(Filename, sw.ToString());
        }
        public override string ToString()
        {
            return Path.GetFileNameWithoutExtension(Filename);
        }
    }
    public class territoriesConfig
    {
        public territorytype territorytype { get; set; }
        public string Filename { get; set; }
        public bool isDirty = false;
        public territoriesConfig(string filename)
        {
            Filename = filename;
            var mySerializer = new XmlSerializer(typeof(territorytype));
            StringBuilder sb = new StringBuilder();
            List<string> filearray = File.ReadAllLines(Filename).ToList();
            foreach (String line in filearray)
            {
                if (line.Contains("<!-- ---"))
                {
                    isDirty = true;
                    continue;
                }
                sb.Append(line + Environment.NewLine);
            }
            using (Stream ms = Helper.GenerateStreamFromString(sb.ToString()))
            {
                Console.Write("\tserializing " + Path.GetFileName(Filename));
                try
                {
                    territorytype = (territorytype)mySerializer.Deserialize(ms);
                    if (territorytype != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("  OK....");
                        Console.ForegroundColor = ConsoleColor.White;
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
        }
        public void SaveTerritories(string saveTime = null)
        {
            if (saveTime != null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Filename) + "\\Backup\\" + saveTime);
                File.Copy(Filename, Path.GetDirectoryName(Filename) + "\\Backup\\" + saveTime + "\\" + Path.GetFileName(Filename), true);
            }
            var serializer = new XmlSerializer(typeof(territorytype));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var sw = new StringWriter();
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, });
            serializer.Serialize(xmlWriter, territorytype, ns);
            Console.WriteLine(sw.ToString());
            File.WriteAllText(Filename, sw.ToString());
        }
        public override string ToString()
        {
            return Path.GetFileNameWithoutExtension(Filename);
        }
    }

    public class cfgrandompresetsconfig
    {
        public randompresets randompresets { get; set; }
        public string Filename { get; set; }
        public bool isDirty = false;

        public cfgrandompresetsconfig(string filename)
        {
            Filename = filename;
            if (File.Exists(Filename))
            {
                var mySerializer = new XmlSerializer(typeof(randompresets));
                StringBuilder sb = new StringBuilder();
                List<string> filearray = File.ReadAllLines(Filename).ToList();
                foreach (String line in filearray)
                {
                    if (line.Contains("<!-- ---"))
                    {
                        isDirty = true;
                        continue;
                    }
                    sb.Append(line + Environment.NewLine);
                }
                using (Stream ms = Helper.GenerateStreamFromString(sb.ToString()))
                {
                    Console.WriteLine("serializing " + Path.GetFileName(Filename));
                    try
                    {
                        randompresets = (randompresets)mySerializer.Deserialize(ms);
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
            else
            {
                Console.WriteLine(Path.GetFileName(Filename) + "Does not exist, Creating new empty file.");
                randompresets = new randompresets();
                SaveRandomPresets();
            }
        }
        public void SaveRandomPresets(string saveTime = null)
        {
            if (saveTime != null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Filename) + "\\Backup\\" + saveTime);
                File.Copy(Filename, Path.GetDirectoryName(Filename) + "\\Backup\\" + saveTime + "\\" + Path.GetFileName(Filename), true);
            }
            var serializer = new XmlSerializer(typeof(randompresets));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var sw = new StringWriter();
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, });
            serializer.Serialize(xmlWriter, randompresets, ns);
            Console.WriteLine(sw.ToString());
            File.WriteAllText(Filename, sw.ToString());
        }
    }
    public class mapgroupproto
    {
        public prototype prototypeGroup { get; set; }
        public string Filename { get; set; }
        public bool isDirty = false;
        public mapgroupproto(string filename)
        {
            Filename = filename;
            if (File.Exists(Filename))
            {
                var mySerializer = new XmlSerializer(typeof(prototype));
                // To read the file, create a FileStream.
                using (var myFileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    Console.Write("serializing " + Path.GetFileName(Filename));
                    try
                    {
                        // Call the Deserialize method and cast to the object type.
                        prototypeGroup = (prototype)mySerializer.Deserialize(myFileStream);
                        if (prototypeGroup != null)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("  OK....");
                            Console.ForegroundColor = ConsoleColor.White;
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
            }
            else
            {
                Console.WriteLine(Path.GetFileName(Filename) + "Does not exist, Creating new empty file.");
                prototypeGroup = new prototype();
            }
        }
        public void Savemapgroupproto(string saveTime = null)
        {
            if (saveTime != null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Filename) + "\\Backup\\" + saveTime);
                File.Copy(Filename, Path.GetDirectoryName(Filename) + "\\Backup\\" + saveTime + "\\" + Path.GetFileName(Filename), true);
            }
            var serializer = new XmlSerializer(typeof(prototype));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var sw = new StringWriter();
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, });
            serializer.Serialize(xmlWriter, prototypeGroup, ns);
            Console.WriteLine(sw.ToString());
            File.WriteAllText(Filename, sw.ToString());
        }
    }
    public class mapgrouppos
    {
        public map map { get; set; }
        public string Filename { get; set; }
        public bool isDirty = false;
        public mapgrouppos(string filename)
        {
            Filename = filename;
            if (File.Exists(Filename))
            {
                var mySerializer = new XmlSerializer(typeof(map));
                // To read the file, create a FileStream.
                using (var myFileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    Console.Write("serializing " + Path.GetFileName(Filename));
                    try
                    {
                        // Call the Deserialize method and cast to the object type.
                        map = (map)mySerializer.Deserialize(myFileStream);
                        if (map != null)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("  OK....");
                            Console.ForegroundColor = ConsoleColor.White;
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
            }
            else
            {
                Console.WriteLine(Path.GetFileName(Filename) + "Does not exist, Creating new empty file.");
                map = new map();
            }
        }
        public void Savemapgrouppos(string saveTime = null)
        {
            if (saveTime != null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Filename) + "\\Backup\\" + saveTime);
                File.Copy(Filename, Path.GetDirectoryName(Filename) + "\\Backup\\" + saveTime + "\\" + Path.GetFileName(Filename), true);
            }
            var serializer = new XmlSerializer(typeof(map));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var sw = new StringWriter();
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, });
            serializer.Serialize(xmlWriter, map, ns);
            Console.WriteLine(sw.ToString());
            File.WriteAllText(Filename, sw.ToString());
        }
    }
    public class eventscofig
    {
        public events events { get; set; }
        public string Filename { get; set; }
        public bool isDirty = false;
        public eventscofig(string filename)
        {
            Filename = filename;
            if (File.Exists(Filename))
            {
                var mySerializer = new XmlSerializer(typeof(events));
                // To read the file, create a FileStream.
                using (var myFileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    Console.Write("serializing " + Path.GetFileName(Filename));
                    try
                    {
                        // Call the Deserialize method and cast to the object type.
                        events = (events)mySerializer.Deserialize(myFileStream);
                        if (events != null)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("  OK....");
                            Console.ForegroundColor = ConsoleColor.White;
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
            }
            else
            {
                Console.WriteLine(Path.GetFileName(Filename) + "Does not exist, Creating new empty file.");
                events = new events();
                SaveEvent();
            }
        }

        public void SaveEvent(string saveTime = null)
        {
            if (saveTime != null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Filename) + "\\Backup\\" + saveTime);
                File.Copy(Filename, Path.GetDirectoryName(Filename) + "\\Backup\\" + saveTime + "\\" + Path.GetFileName(Filename), true);
            }
            var serializer = new XmlSerializer(typeof(events));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var sw = new StringWriter();
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, });
            serializer.Serialize(xmlWriter, events, ns);
            Console.WriteLine(sw.ToString());
            File.WriteAllText(Filename, sw.ToString());
        }
        public override string ToString()
        {
            return Path.GetFileNameWithoutExtension(Filename);
        }
    }
    public class cfgeventspawns
    {
        public eventposdef eventposdef { get; set; }
        public string Filename { get; set; }
        public bool isDirty = false;
        public cfgeventspawns(string filename)
        {
            Filename = filename;
            if (File.Exists(Filename))
            {
                var mySerializer = new XmlSerializer(typeof(eventposdef));
                // To read the file, create a FileStream.
                using (var myFileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    Console.Write("serializing " + Path.GetFileName(Filename));
                    try
                    {
                        // Call the Deserialize method and cast to the object type.
                        eventposdef = (eventposdef)mySerializer.Deserialize(myFileStream);
                        if (eventposdef != null)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("  OK....");
                            Console.ForegroundColor = ConsoleColor.White;
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
            }
            else
            {
                Console.WriteLine(Path.GetFileName(Filename) + "Does not exist, Creating new empty file.");
                eventposdef = new eventposdef();
                SaveEventSpawns();
            }
        }

        public void SaveEventSpawns(string saveTime = null)
        {
            if (saveTime != null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Filename) + "\\Backup\\" + saveTime);
                File.Copy(Filename, Path.GetDirectoryName(Filename) + "\\Backup\\" + saveTime + "\\" + Path.GetFileName(Filename), true);
            }
            var serializer = new XmlSerializer(typeof(eventposdef));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var sw = new StringWriter();
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, });
            serializer.Serialize(xmlWriter, eventposdef, ns);
            Console.WriteLine(sw.ToString());
            File.WriteAllText(Filename, sw.ToString());
        }
        public override string ToString()
        {
            return Path.GetFileNameWithoutExtension(Filename);
        }
    }
    public class cfgeventgroups
    {
        public eventgroupdef eventgroupdef { get; set; }
        public string Filename { get; set; }
        public bool isDirty = false;
        public cfgeventgroups(string filename)
        {
            Filename = filename;
            var mySerializer = new XmlSerializer(typeof(eventgroupdef));
            // To read the file, create a FileStream.
            if (File.Exists(Filename))
            {
                using (var myFileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    Console.Write("serializing " + Path.GetFileName(Filename));
                    try
                    {
                        // Call the Deserialize method and cast to the object type.
                        eventgroupdef = (eventgroupdef)mySerializer.Deserialize(myFileStream);
                        if (eventgroupdef != null)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("  OK....");
                            Console.ForegroundColor = ConsoleColor.White;
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
            }
            else
            {
                Console.WriteLine(Path.GetFileName(Filename) + "Does not exist, Creating new empty file.");
                eventgroupdef = new eventgroupdef();
                SaveEventGroups();
            }
        }
        public void SaveEventGroups(string saveTime = null)
        {
            if (saveTime != null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Filename) + "\\Backup\\" + saveTime);
                File.Copy(Filename, Path.GetDirectoryName(Filename) + "\\Backup\\" + saveTime + "\\" + Path.GetFileName(Filename), true);
            }
            var serializer = new XmlSerializer(typeof(eventgroupdef));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var sw = new StringWriter();
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, });
            serializer.Serialize(xmlWriter, eventgroupdef, ns);
            Console.WriteLine(sw.ToString());
            File.WriteAllText(Filename, sw.ToString());
        }
        public override string ToString()
        {
            return Path.GetFileNameWithoutExtension(Filename);
        }
    }
    public class globalsconfig
    {
        public variables variables { get; set; }
        public string Filename { get; set; }
        public bool isDirty = false;
        public globalsconfig(string filename)
        {
            Filename = filename;
            if (File.Exists(Filename))
            {
                var mySerializer = new XmlSerializer(typeof(variables));
                // To read the file, create a FileStream.
                using (var myFileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    Console.Write("serializing " + Path.GetFileName(Filename));
                    try
                    {
                        // Call the Deserialize method and cast to the object type.
                        variables = (variables)mySerializer.Deserialize(myFileStream);
                        if (variables != null)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("  OK....");
                            Console.ForegroundColor = ConsoleColor.White;
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
            }
            else
            {
                Console.WriteLine(Path.GetFileName(Filename) + " File not found, Creating new....");
                variables = new variables();
            }
        }
        public void SaveGlobals(string saveTime = null)
        {
            if (saveTime != null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Filename) + "\\Backup\\" + saveTime);
                File.Copy(Filename, Path.GetDirectoryName(Filename) + "\\Backup\\" + saveTime + "\\" + Path.GetFileName(Filename), true);
            }
            var serializer = new XmlSerializer(typeof(variables));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var sw = new StringWriter();
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, });
            serializer.Serialize(xmlWriter, variables, ns);
            Console.WriteLine(sw.ToString());
            File.WriteAllText(Filename, sw.ToString());
        }
    }
    public class cfgignorelist
    {
        public ignore ignore { get; set; }
        public string Filename { get; set; }
        public bool isDirty = false;
        public cfgignorelist(string filename)
        {
            Filename = filename;
            var mySerializer = new XmlSerializer(typeof(ignore));
            // To read the file, create a FileStream.
            if (File.Exists(Filename))
            {
                using (var myFileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    Console.Write("serializing " + Path.GetFileName(Filename));
                    try
                    {
                        // Call the Deserialize method and cast to the object type.
                        ignore = (ignore)mySerializer.Deserialize(myFileStream);
                        if (ignore != null)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("  OK....");
                            Console.ForegroundColor = ConsoleColor.White;
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
            }
            else
            {
                Console.WriteLine(Path.GetFileName(Filename) + "Does not exist, Creating new empty file.");
                ignore = new ignore();
                Saveignorelist();
            }
        }
        public void Saveignorelist(string saveTime = null)
        {
            if (saveTime != null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Filename) + "\\Backup\\" + saveTime);
                File.Copy(Filename, Path.GetDirectoryName(Filename) + "\\Backup\\" + saveTime + "\\" + Path.GetFileName(Filename), true);
            }
            var sw = new StringWriter();
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            sw.WriteLine("<ignore>");
            if (ignore.type != null)
            {
                foreach (ignoreType ignoretype in ignore.type)
                {
                    sw.WriteLine("\t<type name=\"" + ignoretype.name + "\"></type>");
                }
            }
            sw.WriteLine("</ignore>");
            Console.WriteLine(sw.ToString());
            File.WriteAllText(Filename, sw.ToString());
        }
        public override string ToString()
        {
            return Path.GetFileNameWithoutExtension(Filename);
        }
    }
    public class cfgplayerspawnpoints
    {
        public playerspawnpoints playerspawnpoints { get; set; }
        public string Filename { get; set; }
        public bool isDirty { get; set; }
        public bool ValidateSchema(string xmlPath, string xsdPath)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(xmlPath);

            xml.Schemas.Add(null, xsdPath);

            try
            {
                xml.Validate(null);
            }
            catch (XmlSchemaValidationException)
            {
                return false;
            }
            return true;
        }
        public cfgplayerspawnpoints(string filename)
        {
            Filename = filename;
            if (File.Exists(Filename))
            {
                if (!ValidateSchema(Filename, Application.StartupPath + "\\TraderNPCs\\PlayerSpawnSchema.xml"))
                {
                    Console.Write("Old PlayerSpawnPoint file found converting to new.......");
                    var myoldSerializer = new XmlSerializer(typeof(playerspawnpoints_old));
                    using (var myFileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        playerspawnpoints_old PSPOld = (playerspawnpoints_old)myoldSerializer.Deserialize(myFileStream);
                        playerspawnpoints = PSPOld.convertToNewFormat();
                    }
                    if (playerspawnpoints != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("  OK....");
                        Console.ForegroundColor = ConsoleColor.White;
                        Savecfgplayerspawnpoints();
                    }
                    
                }
                else
                {
                    var mySerializer = new XmlSerializer(typeof(playerspawnpoints));
                    using (var myFileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        Console.Write("serializing " + Path.GetFileName(Filename));
                        try
                        {
                            playerspawnpoints = (playerspawnpoints)mySerializer.Deserialize(myFileStream);
                            if (playerspawnpoints != null)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("  OK....");
                                Console.ForegroundColor = ConsoleColor.White;
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
                            Console.WriteLine("Error in " + Path.GetFileName(Filename) + "\n" + ex.Message.ToString() + "\n" + ex.InnerException.Message.ToString() + "\n***** Please Fix this before continuing to use the editor *****\n");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine(Path.GetFileName(Filename) + " File not found, Creating new....\n");
                playerspawnpoints = new playerspawnpoints();
                Savecfgplayerspawnpoints();
            }
        }
        public void Savecfgplayerspawnpoints(string saveTime = null)
        {
            if (saveTime != null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Filename) + "\\Backup\\" + saveTime);
                File.Copy(Filename, Path.GetDirectoryName(Filename) + "\\Backup\\" + saveTime + "\\" + Path.GetFileName(Filename), true);
            }
            var serializer = new XmlSerializer(typeof(playerspawnpoints));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var sw = new StringWriter();
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, });
            serializer.Serialize(xmlWriter, playerspawnpoints, ns);
            Console.WriteLine(sw.ToString());
            File.WriteAllText(Filename, sw.ToString());
        }
        public override string ToString()
        {
            return Path.GetFileNameWithoutExtension(Filename);
        }
    }
    public class CFGGameplayConfig
    {
        public cfggameplay cfggameplay { get; set; }
        public string Filename { get; set; }
        public bool isDirty { get; set; }
        public CFGGameplayConfig(string filename)
        {
            Filename = filename;
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
            if (File.Exists(Filename))
            {
                Console.Write("serializing " + Path.GetFileName(Filename));
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        Converters = { new BoolConverter() },
                    };
                    cfggameplay = JsonSerializer.Deserialize<cfggameplay>(File.ReadAllText(Filename), options);
                    if (cfggameplay.checkver())
                    {
                        SaveCFGGameplay();
                        Console.WriteLine("CFGGameplayConfig Version Updated........\n");
                    }
                    if (cfggameplay != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("  OK....");
                        Console.ForegroundColor = ConsoleColor.White;
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
            else
            {
                Console.WriteLine(Path.GetFileName(Filename) + " File not found, Creating new....");
                cfggameplay = new cfggameplay();
                SaveCFGGameplay();
            }

        }
        public void SaveCFGGameplay()
        {
            SetSpawnGearFiles();
            var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
            string jsonString = JsonSerializer.Serialize(cfggameplay, options);
            File.WriteAllText(Filename, jsonString);
        }
        public void SetSpawnGearFiles()
        {
            cfggameplay.PlayerData.spawnGearPresetFiles = new BindingList<string>();
            foreach (SpawnGearPresetFiles SGPF in cfggameplay.SpawnGearPresetFiles)
            {
                cfggameplay.PlayerData.spawnGearPresetFiles.Add(SGPF.Filename);
            }
        }
        public void GetSpawnGearFiles(string SpawnGearPath)
        {
            cfggameplay.SpawnGearPresetFiles = new BindingList<SpawnGearPresetFiles>();
            Console.Write("## Starting SpawnGearPresets ##" + Environment.NewLine);
            foreach (string file in cfggameplay.PlayerData.spawnGearPresetFiles)
            {
                SpawnGearPresetFiles SpawnGearPresetFiles;

                Console.Write("\tserializing " + file);
                try
                {
                    SpawnGearPresetFiles = JsonSerializer.Deserialize<SpawnGearPresetFiles>(File.ReadAllText(SpawnGearPath + "/" + file));
                    if (SpawnGearPresetFiles != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("  OK....");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    SpawnGearPresetFiles.Filename = file;
                    cfggameplay.SpawnGearPresetFiles.Add(SpawnGearPresetFiles);
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
            Console.Write("## End  SpawnGearPresets ##" + Environment.NewLine);
        }

        public void SaveSpawnGearPresetFiles(string MissionPath)
        {
            foreach (SpawnGearPresetFiles SGFP in cfggameplay.SpawnGearPresetFiles)
            {
                if (SGFP.isDirty == true)
                {
                    var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                    string jsonString = JsonSerializer.Serialize(SGFP, options);
                    File.WriteAllText(MissionPath + SGFP.Filename, jsonString);
                    SGFP.isDirty = false;
                }
            }
        }
    }
    public class cfgEffectAreaConfig
    {
        public cfgEffectArea cfgEffectArea { get; set; }
        public string Filename { get; set; }
        public bool isDirty { get; set; }
        public cfgEffectAreaConfig(string filename)
        {
            Filename = filename;
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
            if (File.Exists(Filename))
            {
                Console.Write("serializing " + Path.GetFileName(Filename));
                try
                {
                    cfgEffectArea = JsonSerializer.Deserialize<cfgEffectArea>(File.ReadAllText(Filename));
                    cfgEffectArea.convertpositionstolist();
                    if (cfgEffectArea != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("  OK....");
                        Console.ForegroundColor = ConsoleColor.White;
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
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(Path.GetFileName(Filename) + " File not found, Creating new....");
                Console.ForegroundColor = ConsoleColor.White;
                cfgEffectArea = new cfgEffectArea();
                SavecfgEffectArea();
                cfgEffectArea.convertpositionstolist();
            }
        }
        public void SavecfgEffectArea()
        {
            var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
            string jsonString = JsonSerializer.Serialize(cfgEffectArea, options);
            File.WriteAllText(Filename, jsonString);
        }
    }
    public class weatherconfig
    {
        public weather weather { get; set; }
        public string Filename { get; set; }
        public bool isDirty = false;

        public weatherconfig(string filename)
        {
            Filename = filename;
            if (File.Exists(Filename))
            {
                var mySerializer = new XmlSerializer(typeof(weather));
                // To read the file, create a FileStream.
                using (var myFileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    Console.Write("serializing " + Path.GetFileName(Filename));
                    try
                    {
                        // Call the Deserialize method and cast to the object type.
                        weather = (weather)mySerializer.Deserialize(myFileStream);
                        if (weather != null)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("  OK....");
                            Console.ForegroundColor = ConsoleColor.White;
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
            }
            else
            {
                Console.WriteLine(Path.GetFileName(Filename) + " File not found, Creating new....");
                weather = new weather();
                SaveWeather();
            }
        }
        public void SaveWeather(string saveTime = null)
        {
            if (saveTime != null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Filename) + "\\Backup\\" + saveTime);
                File.Copy(Filename, Path.GetDirectoryName(Filename) + "\\Backup\\" + saveTime + "\\" + Path.GetFileName(Filename), true);
            }
            var serializer = new XmlSerializer(typeof(weather));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var sw = new StringWriter();
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, });
            serializer.Serialize(xmlWriter, weather, ns);
            Console.WriteLine(sw.ToString());
            File.WriteAllText(Filename, sw.ToString());
        }
    }
    public class PlayerDataBase
    {
        public SQLiteConnection sqlite;

        public DataSet m_DataSet { get; set; }
        public PlayerDataBase(string filename)
        {
            sqlite = new SQLiteConnection("Data Source=" + filename);
            m_DataSet = new DataSet();
            foreach (string tableName in GetTables())
            {
                m_DataSet.Tables.Add(GetDataTable("select * from " + tableName));
            }

        }

        public List<string> GetTables()
        {
            List<string> list = new List<string>();

            // executes query that select names of all tables in master table of the database
            String query = "SELECT name FROM sqlite_master WHERE type = 'table'ORDER BY 1";
            try
            {

                DataTable table = GetDataTable(query);

                // Return all table names in the ArrayList

                foreach (DataRow row in table.Rows)
                {
                    list.Add(row.ItemArray[0].ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return list;
        }

        public DataTable GetDataTable(string sql)
        {
            try
            {
                DataTable dt = new DataTable();
                using (var c = new SQLiteConnection(sqlite))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                    {
                        using (SQLiteDataReader rdr = cmd.ExecuteReader())
                        {
                            dt.Load(rdr);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}