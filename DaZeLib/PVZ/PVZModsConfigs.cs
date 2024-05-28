using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PVZCZGlobals = PVZ.CustomZombiesGlobals.types;
using PVZCZCharateristics = PVZ.CustomZombiesCharacteristics.types;
using PVZInfoPanel = PVZ.InfoPanel.types;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml;

namespace DayZeLib
{
    public class PvZmod_Configs
    {
        public PVZCZGlobals PvZmoD_CustomisableZombies_Globals { get; set; }
        public PVZCZCharateristics PvZmoD_CustomisableZombies_Characteristics { get; set; }
        public PVZInfoPanel PvZmoD_Information_Panel { get; set; }
        public string PvZmoD_CustomisableZombies_GlobalsFilename { get; set; }
        public string PvZmoD_CustomisableZombies_CharacteristicsFilename { get; set; }
        public string PvZmoDInfoPanelFilename { get; set; }
        public bool Infopanel = false;
        public bool CustomisableZombies = false;
        public bool DarkHorde = false;
        public bool isDirty = false;

        public PvZmod_Configs(string PathName)
        {
            PvZmoDInfoPanelFilename = PathName + "\\PvZmoD_Information_Panel\\PvZmoD_Information_Panel.xml";
            PvZmoD_CustomisableZombies_GlobalsFilename = PathName + "\\PvZmoD_CustomisableZombies_Profile\\PvZmoD_CustomisableZombies_Globals.xml";
            PvZmoD_CustomisableZombies_CharacteristicsFilename = PathName + "\\PvZmoD_CustomisableZombies_Profile\\PvZmoD_CustomisableZombies_Characteristics.xml";
            if (File.Exists(PvZmoD_CustomisableZombies_GlobalsFilename))
            {
                Infopanel = true;
                var mySerializer = new XmlSerializer(typeof(PVZInfoPanel));
                // To read the file, create a FileStream.
                using (var myFileStream = new FileStream(PvZmoDInfoPanelFilename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    Console.Write("serializing " + Path.GetFileName(PvZmoDInfoPanelFilename));
                    try
                    {
                        // Call the Deserialize method and cast to the object type.
                        PvZmoD_Information_Panel = (PVZInfoPanel)mySerializer.Deserialize(myFileStream);
                        if (PvZmoD_Information_Panel != null)
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
                        MessageBox.Show("Error in " + Path.GetFileName(PvZmoDInfoPanelFilename) + "\n" + ex.Message.ToString() + "\n" + ex.InnerException.Message.ToString());
                    }
                }
            }
            if (File.Exists(PvZmoD_CustomisableZombies_GlobalsFilename) && File.Exists(PvZmoD_CustomisableZombies_CharacteristicsFilename))
            {
                CustomisableZombies = true;
                var mySerializer = new XmlSerializer(typeof(PVZCZGlobals));
                // To read the file, create a FileStream.
                using (var myFileStream = new FileStream(PvZmoD_CustomisableZombies_GlobalsFilename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    Console.Write("serializing " + Path.GetFileName(PvZmoD_CustomisableZombies_GlobalsFilename));
                    try
                    {
                        // Call the Deserialize method and cast to the object type.
                        PvZmoD_CustomisableZombies_Globals = (PVZCZGlobals)mySerializer.Deserialize(myFileStream);
                        if (PvZmoD_CustomisableZombies_Globals != null)
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
                        MessageBox.Show("Error in " + Path.GetFileName(PvZmoD_CustomisableZombies_GlobalsFilename) + "\n" + ex.Message.ToString() + "\n" + ex.InnerException.Message.ToString());
                    }
                }
                mySerializer = new XmlSerializer(typeof(PVZCZCharateristics));
                // To read the file, create a FileStream.
                using (var myFileStream = new FileStream(PvZmoD_CustomisableZombies_CharacteristicsFilename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    Console.Write("serializing " + Path.GetFileName(PvZmoD_CustomisableZombies_CharacteristicsFilename));
                    try
                    {
                        // Call the Deserialize method and cast to the object type.
                        PvZmoD_CustomisableZombies_Characteristics = (PVZCZCharateristics)mySerializer.Deserialize(myFileStream);
                        if (PvZmoD_CustomisableZombies_Characteristics != null)
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
                        MessageBox.Show("Error in " + Path.GetFileName(PvZmoD_CustomisableZombies_CharacteristicsFilename) + "\n" + ex.Message.ToString() + "\n" + ex.InnerException.Message.ToString());
                    }
                }
            }
        }
        public void SavePVZCZGlobals(string saveTime = null)
        {
            if (saveTime != null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(PvZmoD_CustomisableZombies_GlobalsFilename) + "\\Backup\\" + saveTime);
                File.Copy(PvZmoD_CustomisableZombies_GlobalsFilename, Path.GetDirectoryName(PvZmoD_CustomisableZombies_GlobalsFilename) + "\\Backup\\" + saveTime + "\\" + Path.GetFileName(PvZmoD_CustomisableZombies_GlobalsFilename), true);
            }
            var serializer = new XmlSerializer(typeof(PVZCZGlobals));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var sw = new StringWriter();
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, });
            serializer.Serialize(xmlWriter, PvZmoD_CustomisableZombies_Globals, ns);
            Console.WriteLine(sw.ToString());
            File.WriteAllText(PvZmoD_CustomisableZombies_GlobalsFilename, sw.ToString());
        }
        public void SavePVZCZCharacteristics(string saveTime = null)
        {
            if (saveTime != null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(PvZmoD_CustomisableZombies_CharacteristicsFilename) + "\\Backup\\" + saveTime);
                File.Copy(PvZmoD_CustomisableZombies_CharacteristicsFilename, Path.GetDirectoryName(PvZmoD_CustomisableZombies_CharacteristicsFilename) + "\\Backup\\" + saveTime + "\\" + Path.GetFileName(PvZmoD_CustomisableZombies_CharacteristicsFilename), true);
            }
            var serializer = new XmlSerializer(typeof(PVZCZCharateristics));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var sw = new StringWriter();
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, });
            serializer.Serialize(xmlWriter, PvZmoD_CustomisableZombies_Characteristics, ns);
            Console.WriteLine(sw.ToString());
            File.WriteAllText(PvZmoD_CustomisableZombies_CharacteristicsFilename, sw.ToString());
        }
    }
}
