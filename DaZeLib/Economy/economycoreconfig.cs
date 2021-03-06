using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace DayZeLib
{
    public class economycoreconfig
    {
        public economycore economycore { get; set; }
        public string Filename { get; set; }
        public economycoreconfig(string filename)
        {
            Filename = filename;
            var mySerializer = new XmlSerializer(typeof(economycore));
            // To read the file, create a FileStream.
            try
            {
                using (var myFileStream = new FileStream(filename, FileMode.Open))
                {
                    // Call the Deserialize method and cast to the object type.
                    economycore = (economycore)mySerializer.Deserialize(myFileStream);
                }
            }
            catch(Exception ex)
            {
                var form = Application.OpenForms["SplashForm"];
                if (form != null)
                {
                    form.Invoke(new Action(() => { form.Close(); }));
                }
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
            if (economycore.ce.Any(x => x.folder == path))
            {
                economycore.ce.FirstOrDefault(x => x.folder == path).file.Add(newfile);
            }
            else
            {
                economycoreCE newce = new economycoreCE
                {
                    folder = path,
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
                try
                {
                    spawnabletypes = (spawnabletypes)mySerializer.Deserialize(ms);
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
    public class cfgrandompresetsconfig
    {
        public randompresets randompresets { get; set; }
        public string Filename { get; set; }
        public bool isDirty = false;

        public cfgrandompresetsconfig(string filename)
        {
            Filename = filename;
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
            var mySerializer = new XmlSerializer(typeof(prototype));
            // To read the file, create a FileStream.
            using (var myFileStream = new FileStream(filename, FileMode.Open))
            {
                try
                {
                    // Call the Deserialize method and cast to the object type.
                    prototypeGroup = (prototype)mySerializer.Deserialize(myFileStream);
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
    }
    public class mapgrouppos
    {
        public map map { get; set; }
        public string Filename { get; set; }
        public bool isDirty = false;
        public mapgrouppos(string filename)
        {
            Filename = filename;
            var mySerializer = new XmlSerializer(typeof(map));
            // To read the file, create a FileStream.
            using (var myFileStream = new FileStream(filename, FileMode.Open))
            {
                try
                {
                    // Call the Deserialize method and cast to the object type.
                    map = (map)mySerializer.Deserialize(myFileStream);
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
    }
    public class eventscofig
    {
        public events events { get; set; }
        public string Filename { get; set; }
        public bool isDirty = false;
        public eventscofig(string filename)
        {
            Filename = filename;
            var mySerializer = new XmlSerializer(typeof(events));
            // To read the file, create a FileStream.
            using (var myFileStream = new FileStream(filename, FileMode.Open))
            {
                try
                {
                    // Call the Deserialize method and cast to the object type.
                    events = (events)mySerializer.Deserialize(myFileStream);
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
            var mySerializer = new XmlSerializer(typeof(eventposdef));
            // To read the file, create a FileStream.
            using (var myFileStream = new FileStream(filename, FileMode.Open))
            {
                try
                {
                    // Call the Deserialize method and cast to the object type.
                    eventposdef = (eventposdef)mySerializer.Deserialize(myFileStream);
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
            using (var myFileStream = new FileStream(filename, FileMode.Open))
            {
                try
                {
                    // Call the Deserialize method and cast to the object type.
                    eventgroupdef = (eventgroupdef)mySerializer.Deserialize(myFileStream);
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
            var mySerializer = new XmlSerializer(typeof(variables));
            // To read the file, create a FileStream.
            using (var myFileStream = new FileStream(filename, FileMode.Open))
            {
                try
                {
                    // Call the Deserialize method and cast to the object type.
                    variables = (variables)mySerializer.Deserialize(myFileStream);
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
    public class cfgplayerspawnpoints
    {
        public playerspawnpoints playerspawnpoints { get; set; }
        public string Filename { get; set; }
        public bool isDirty { get; set; }
        public cfgplayerspawnpoints(string filename)
        {
            Filename = filename;
            var mySerializer = new XmlSerializer(typeof(playerspawnpoints));
            // To read the file, create a FileStream.
            using (var myFileStream = new FileStream(filename, FileMode.Open))
            {
                try
                {
                    // Call the Deserialize method and cast to the object type.
                    playerspawnpoints = (playerspawnpoints)mySerializer.Deserialize(myFileStream);
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
                try
                {
                    cfggameplay = JsonSerializer.Deserialize<cfggameplay>(File.ReadAllText(Filename));
                    if (cfggameplay.checkver())
                        SaveCFGGameplay();
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
            else
            {
                cfggameplay = new cfggameplay();
                SaveCFGGameplay();
            }

        }
        public void SaveCFGGameplay()
        {
            var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
            string jsonString = JsonSerializer.Serialize(cfggameplay, options);
            File.WriteAllText(Filename, jsonString);
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
            try
            {
                if (File.Exists(Filename))
                {
                    cfgEffectArea = JsonSerializer.Deserialize<cfgEffectArea>(File.ReadAllText(Filename));
                }
                else
                {
                    cfgEffectArea = new cfgEffectArea();
                    SavecfgEffectArea();

                }
                cfgEffectArea.convertpositionstolist();
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
            if(!File.Exists(Filename))
            {
                weather = new weather();
                SaveWeather();
            }
            var mySerializer = new XmlSerializer(typeof(weather));
            // To read the file, create a FileStream.
            using (var myFileStream = new FileStream(filename, FileMode.Open))
            {
                try
                {
                    // Call the Deserialize method and cast to the object type.
                    weather = (weather)mySerializer.Deserialize(myFileStream);
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
    public class PlayerDB
    {
        private SQLiteConnection sqlite;

        public DataTable PlayerTable;
        public PlayerDB(string filename)
        {
            
        }

    }
}