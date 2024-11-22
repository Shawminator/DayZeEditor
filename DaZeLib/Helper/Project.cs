using DayZeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DayZeEditor
{
    public class ProjectList
    {
        public bool ShowChangeLog { get; set; }
        public string ActiveProject { get; set; }
        public List<Project> Projects { get; set; }

        public ProjectList()
        {
            Projects = new List<Project>();
        }
        public void addtoprojects(Project project, bool setactive = true)
        {
            Projects.Add(project);
            if (setactive)
                ActiveProject = project.ProjectName;
            SaveProject(false, false);
        }
        public Project getprojectfromname(string name)
        {
            if (name == null || name == "")
                return null;
            Project p = new Project();
            if (name.Contains(":"))
            {
                string[] stuff = name.Split(':');
                p = Projects.FirstOrDefault(x => x.ProjectName == stuff[0] && x.mpmissionpath.Split('.')[1] == stuff[1]);
            }
            else
            {
                p = Projects.FirstOrDefault(x => x.ProjectName == name);
                ActiveProject = p.ProjectName + ":" + p.mpmissionpath.Split('.')[1];
                SaveProject(false, false);
            }
            return p;
        }
        public void SaveProject(bool create = false, bool showmessage = true)
        {
            var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
            string jsonString = JsonSerializer.Serialize(this, options);
            Directory.CreateDirectory(Application.StartupPath + "\\Project");
            File.WriteAllText(Application.StartupPath + "\\Project\\Projects.json", jsonString);
            var form = Application.OpenForms["SplashForm"];
            if (form != null)
            {
                form.Invoke(new Action(() => { form.Close(); }));
            }
            if (showmessage)
            {
                if (create)
                    MessageBox.Show("Projects Config Created.");
                else
                    MessageBox.Show("Projects Config Saved.");
            }
        }
        public void SetActiveProject(Project p = null)
        {
            if (p == null)
            {
                ActiveProject = "";
                return;
            }
            ActiveProject = p.ProjectName + ":" + p.mpmissionpath.Split('.')[1];
            SaveProject(false, false);
        }
        public Project getActiveProject()
        {
            return getprojectfromname(ActiveProject);
        }
        public void DeleteProject(string profilename)
        {
            Project p = getprojectfromname(profilename);
            Projects.Remove(p);
        }
        public void CheckAllPaths()
        {
            bool save = false;
            foreach(Project p in  Projects) 
            {
                if(p.projectFullName.EndsWith("\\"))
                {
                    p.projectFullName = p.projectFullName.Remove(p.projectFullName.Length - 1);
                    save = true;
                }
                if(p.ProfilePath.EndsWith("\\"))
                {
                    p.ProfilePath = p.ProfilePath.Remove(p.ProfilePath.Length - 1);
                    save = true;
                }
            }
            if(save) { SaveProject(false, false); }
        }
    }
    public class Project
    {
        public string ProjectName { get; set; }
        public string projectFullName { get; set; }
        public string ProfilePath { get; set; }
        public bool usingDrJoneTrader { get; set; }
        public bool usingexpansionMarket { get; set; }
        public bool usingtraderplus { get; set; }
        public string MapPath { get; set; }
        public int MapSize { get; set; }
        public string mpmissionpath { get; set; }
        public bool Createbackups { get; set; }


        [JsonIgnore]
        public bool haswarnings { get; set; }
        [JsonIgnore]
        public economycoreconfig EconomyCore { get; set; }
        [JsonIgnore]
        public Limitsdefinitions limitfefinitions { get; set; }
        [JsonIgnore]
        public UserDefinitions limitfefinitionsuser { get; set; }
        [JsonIgnore]
        public cfgplayerspawnpoints cfgplayerspawnpoints { get; set; }
        [JsonIgnore]
        public BindingList<eventscofig> ModEventsList { get; set; }
        [JsonIgnore]
        public cfgeventspawns cfgeventspawns { get; set; }
        [JsonIgnore]
        public cfgeventgroups cfgeventgroups { get; set; }
        [JsonIgnore]
        public globalsconfig gloabsconfig { get; set; }
        [JsonIgnore]
        public cfgignorelist cfgignorelist { get; set; }
        [JsonIgnore]
        public BindingList<Spawnabletypesconfig> spawnabletypesList { get; set; }
        [JsonIgnore]
        public cfgrandompresetsconfig cfgrandompresetsconfig { get; set; }
        [JsonIgnore]
        public CFGGameplayConfig CFGGameplayConfig { get; set; }
        [JsonIgnore]
        public cfgEffectAreaConfig cfgEffectAreaConfig { get; set; }
        [JsonIgnore]
        public weatherconfig weatherconfig { get; set; }
        [JsonIgnore]
        public mapgroupproto mapgroupproto { get; set; }
        [JsonIgnore]
        public mapgrouppos mapgrouppos { get; set; }

        [JsonIgnore]
        public int TotalNomCount { get; set; }

        private TypesFile vanillaTypes;
        private BindingList<TypesFile> ModTypesList;
        public BindingList<territoriesConfig> territoriesList;

        public Project()
        {
            ProjectName = "";
            projectFullName = "";
            ProfilePath = "";
            MapPath = "";
            MapSize = 0;
            mpmissionpath = "";
            Createbackups = false;
            usingDrJoneTrader = false;
            usingexpansionMarket = false;
            usingtraderplus = false;
        }
        public override string ToString()
        {
            return ProjectName + ":" + mpmissionpath.Split('.')[1];
        }

        public bool checkMapExists()
        {
            if(File.Exists(Application.StartupPath + MapPath))
            {
                return true;
            }
            else 
            { 
                return false; 
            }
        }

        public void AddNames(string _ProjectName, string fullname)
        {
            projectFullName = fullname;
            ProjectName = _ProjectName;
        }
        public void setVanillaTypes()
        {
            vanillaTypes = new TypesFile(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\db\\types.xml");
        }
        public void SetmodTypes(string filename)
        {
            if (ModTypesList == null)
                ModTypesList = new BindingList<TypesFile>();
            TypesFile tf = new TypesFile(filename);
            tf.modname = Path.GetFileNameWithoutExtension(filename);
            ModTypesList.Add(tf);
        }
        public TypesFile GetTypesfilebyname(string v)
        {
            if (v == "VanillaTypes")
                return getvanillatypes();
            else
                return GetModTypebyname(v);
        }
        public TypesFile getvanillatypes()
        {
            return vanillaTypes;
        }
        public List<TypesFile> getModList()
        {
            return ModTypesList.ToList();
        }
        public TypesFile GetModTypebyname(string name)
        {
            return ModTypesList.FirstOrDefault(x => x.modname == name);
        }
        public typesType gettypebyname(string classname)
        {
            typesType type = vanillaTypes.Gettypebyname(classname);
            if(type != null) return type;
            else
            {
                foreach(TypesFile tf in ModTypesList)
                {
                    type = tf.Gettypebyname(classname);
                    if (type != null)
                        return type;
                }
            }
            return null;
        }
        public eventscofig geteventbyname(string name)
        {
            return ModEventsList.FirstOrDefault(x => x.Filename == name);
        }
        private Spawnabletypesconfig getspawnablwetyprbyname(string filename)
        {
            return spawnabletypesList.FirstOrDefault(x => x.Filename == filename);
        }
        public void SetModListtypes()
        {
            bool needsave = false;
            ModTypesList = new BindingList<TypesFile>();
            if (EconomyCore.economycore == null) return;
            Console.WriteLine("\n***Starting custom types from Economycore***");
            foreach (economycoreCE mods in EconomyCore.economycore.ce)
            {
                if (mods.folder.Contains("\\"))
                {
                    mods.folder = mods.folder.Replace("\\", "/");
                    needsave = true;
                }
                string path = projectFullName + "\\mpmissions\\" + mpmissionpath + "\\" + mods.folder;
                if (!Directory.Exists(path))
                {
                    haswarnings = true;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(Environment.NewLine + "### Warning ### ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(path + " does not exis, please remove full ce section from economy core." + Environment.NewLine);
                    continue;
                }
                foreach (economycoreCEFile file in mods.file)
                {
                    if (!File.Exists(path + "\\" + file.name))
                    {
                        haswarnings = true;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(Environment.NewLine + "### Warning ### ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(path + "\\" + file.name + " Does not exist, please remove from economy core." + Environment.NewLine);
                        continue;
                    }
                    if (file.type == "types")
                        SetmodTypes(path + "\\" + file.name);
                }
            }
            Console.WriteLine("***End custom types from Economycore***");
            Console.WriteLine(Environment.NewLine);
            if (needsave)
            {
                EconomyCore.SaveEconomycore();
            }
        }
        public void removeMod(string modname)
        {
            ModTypesList.Remove(GetModTypebyname(modname));
        }
        public void removeevent(string filename)
        {
            ModEventsList.Remove(geteventbyname(filename));
        }
        public void removeSpawnabletype(string filename)
        {
            spawnabletypesList.Remove(getspawnablwetyprbyname(filename));
        }
        public void seteconomycore()
        {
            EconomyCore = new economycoreconfig(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\cfgeconomycore.xml");
        }
        public void seteconomydefinitions()
        {
            limitfefinitions = new Limitsdefinitions(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\cfglimitsdefinition.xml");
        }
        public void SetEvents()
        {
            ModEventsList = new BindingList<eventscofig>();
            if (!File.Exists(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\db\\events.xml"))
            {
                XDocument xmlFile = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
                xmlFile.Add(new XElement("events"));
                xmlFile.Save(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\db\\events.xml");
                Console.WriteLine("Vanilla events.xml File not found.... Creating blank XML");
            }
            ModEventsList.Add(new eventscofig(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\db\\events.xml"));
            if (EconomyCore.economycore == null) return;
            foreach (economycoreCE mods in EconomyCore.economycore.ce)
            {
                string path = projectFullName + "\\mpmissions\\" + mpmissionpath + "\\" + mods.folder;
                foreach (economycoreCEFile file in mods.file)
                {
                    if (!Directory.Exists(path))
                    {
                        haswarnings = true;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(Environment.NewLine + "### Warning ### ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(path + " does not exis, please remove full ce section from economy core." + Environment.NewLine);
                        continue;
                    }
                    if (file.type == "events")
                    {
                        if (!File.Exists(path + "\\" + file.name))
                        {
                            haswarnings = true;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(Environment.NewLine + "### Warning ### ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine(path + "\\" + file.name + " Does not exist, please remove from economy core." + Environment.NewLine);
                            continue;
                        }
                        ModEventsList.Add(new eventscofig(path + "\\" + file.name));
                    }
                }
            }
        }
        public bool isUsingExpansion()
        {
            if (Directory.Exists(projectFullName + "\\" + ProfilePath + "\\ExpansionMod"))
                return true;
            else
                return false;
        }
        public void SetSpawnabletypes()
        {
            spawnabletypesList = new BindingList<Spawnabletypesconfig>();
            if (!File.Exists(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\cfgspawnabletypes.xml"))
            {
                XDocument xmlFile = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
                xmlFile.Add(new XElement("spawnabletypes"));
                xmlFile.Save(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\cfgspawnabletypes.xml");
                Console.WriteLine("Vanilla cfgspawnabletypes.xml File not found.... Creating blank XML");
            }
            spawnabletypesList.Add(new Spawnabletypesconfig(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\cfgspawnabletypes.xml"));
            if (EconomyCore.economycore == null) return;
            foreach (economycoreCE mods in EconomyCore.economycore.ce)
            {
                string path = projectFullName + "\\mpmissions\\" + mpmissionpath + "\\" + mods.folder;
                if (!Directory.Exists(path))
                {
                    haswarnings = true;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(Environment.NewLine + "### Warning ### ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(path + " Does not exist. please remove full ce section from economycore" + Environment.NewLine);
                    continue;
                }
                foreach (economycoreCEFile file in mods.file)
                {
                    if (file.type == "spawnabletypes")
                    {
                        if (!File.Exists(path + "\\" + file.name))
                        {
                            haswarnings = true;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(Environment.NewLine + "### Warning ### ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine(path + "\\" + file.name + "\\" + file.name + " Does not exist, please remove from economy core." + Environment.NewLine);
                            continue;
                        }
                        spawnabletypesList.Add(new Spawnabletypesconfig(path + "\\" + file.name));
                    }
                }
            }
        }
        public void SetTerritories()
        {
            territoriesList = new BindingList<territoriesConfig>();
            string[] Territoryfiles = Directory.GetFiles(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\env");
            Console.WriteLine("****Starting Territory files****");
            foreach (string file in Territoryfiles)
            {
                if (Path.GetExtension(file) == ".xml")
                    territoriesList.Add(new territoriesConfig(file));
            }
            Console.WriteLine("****End Territory files****");
        }
        public void SetRandompresets()
        {
            if (!File.Exists(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\cfgrandompresets.xml"))
            {
                XDocument xmlFile = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
                xmlFile.Add(new XElement("randompresets"));
                xmlFile.Save(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\cfgrandompresets.xml");
                Console.WriteLine("Vanilla cfgrandompresets.xml File not found.... Creating blank XML");
            }
            cfgrandompresetsconfig = new cfgrandompresetsconfig(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\cfgrandompresets.xml");
        }
        public void setuserdefinitions()
        {
            limitfefinitionsuser = new UserDefinitions(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\cfglimitsdefinitionuser.xml");
        }
        public void setplayerspawns()
        {
            cfgplayerspawnpoints = new cfgplayerspawnpoints(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\cfgplayerspawnpoints.xml");
        }
        public void seteventspawns()
        {
            cfgeventspawns = new cfgeventspawns(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\cfgeventspawns.xml");
        }
        public void seteventgroups()
        {
            cfgeventgroups = new cfgeventgroups(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\cfgeventgroups.xml");
        }
        public void SetIgnoreList()
        {
            cfgignorelist = new cfgignorelist(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\cfgignorelist.xml");
        }
        public void SetGlobals()
        {
            gloabsconfig = new globalsconfig(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\db\\globals.xml");
        }
        public void Setmapgrouproto()
        {
            mapgroupproto = new mapgroupproto(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\mapgroupproto.xml");
        }
        public void Setmapgroupos()
        {
            mapgrouppos = new mapgrouppos(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\mapgrouppos.xml");
        }
        public void SetWeather()
        {
            weatherconfig = new weatherconfig(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\cfgweather.xml");
        }
        public void SetTotNomCount()
        {
            TotalNomCount = 0;
            List<typesType> typelistforcount = new List<typesType>();
            foreach (typesType _type in vanillaTypes.types.type)
            {
                if (_type.nominalSpecified)
                {
                    if (typelistforcount.Any(x => x.name == _type.name))
                    {
                        typesType otype = typelistforcount.First(x => x.name == _type.name);

                        TotalNomCount -= otype.nominal;
                        typelistforcount.Remove(otype);
                    }
                    TotalNomCount += _type.nominal;
                    typelistforcount.Add(_type);
                }
            }
            foreach (TypesFile tf in ModTypesList)
            {
                foreach (typesType _type in tf.types.type)
                {
                    if (_type.nominalSpecified)
                    {
                        if (typelistforcount.Any(x => x.name == _type.name))
                        {
                            typesType otype = typelistforcount.First(x => x.name == _type.name);

                            TotalNomCount -= otype.nominal;
                            typelistforcount.Remove(otype);
                        }
                        TotalNomCount += _type.nominal;
                        typelistforcount.Add(_type);
                    }
                }
            }
        }
        public string getcorrectclassamefromtypes(string className)
        {
            typesType vantype = vanillaTypes.types.type.FirstOrDefault(x => x.name.ToLower() == className.ToLower());
            if (vantype != null)
                return vantype.name;
            else
            {
                foreach (TypesFile tfile in ModTypesList)
                {
                    typesType modvantype = tfile.types.type.FirstOrDefault(x => x.name.ToLower() == className.ToLower());
                    if (modvantype != null)
                        return modvantype.name;
                }
            }
            return "*** MISSING ITEM TYPE (" + className + ")***";
        }
        public void SetCFGGameplayConfig()
        {
            CFGGameplayConfig = new CFGGameplayConfig(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\cfggameplay.json");
            CFGGameplayConfig.GetSpawnGearFiles(projectFullName + "\\mpmissions\\" + mpmissionpath);
        }
        public void SetcfgEffectAreaConfig()
        {
            cfgEffectAreaConfig = new cfgEffectAreaConfig(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\cfgEffectArea.json");
        }
        public void AssignRarity()
        {
            int TotalNomCount = 0;
            List<typesType> items = new List<typesType>();

            // Gather vanilla items
            foreach (typesType vantype in vanillaTypes.types.type)
            {
                if (vantype.nominalSpecified)
                {
                    if (vantype.nominal == 0)
                    {
                        vantype.Rarity = ITEMRARITY.None;
                        continue;
                    }
                    if (items.Any(x => x.name == vantype.name))
                    {
                        typesType otype = items.First(x => x.name == vantype.name);

                        TotalNomCount -= otype.nominal;
                        items.Remove(otype);
                    }
                    TotalNomCount += vantype.nominal;
                    items.Add(vantype);
                }
            }

            // Gather mod items
            foreach (TypesFile tfile in ModTypesList)
            {
                foreach (typesType typesType in tfile.types.type)
                {
                    if (typesType.nominalSpecified)
                    {
                        if (typesType.nominal == 0)
                        {
                            typesType.Rarity = ITEMRARITY.None;
                            continue;
                        }
                        if (items.Any(x => x.name == typesType.name))
                        {
                            typesType otype = items.First(x => x.name == typesType.name);

                            TotalNomCount -= otype.nominal;
                            items.Remove(otype);
                        }
                        TotalNomCount += typesType.nominal;
                        items.Add(typesType);
                    }
                }
            }

            // Sort items by nominal value
            items = items.OrderBy(i => i.nominal).ToList();

            // Define the min and max nominal values
            int minNominal = 1;
            int maxNominal = items.Max(i => i.nominal);

            Console.WriteLine($"Min Nominal: {minNominal}");
            Console.WriteLine($"Max Nominal: {maxNominal}");

            int totalItems = items.Count;
            Console.WriteLine($"Total Items Count: {totalItems}");
            Console.WriteLine($"Total Items Nominal Count: {TotalNomCount}");

            // Calculate the thresholds for each rarity
            double range = maxNominal - minNominal + 1;
            double legendaryThreshold = minNominal + 0.05 * range;
            double epicThreshold = minNominal + 0.25 * range;
            double rareThreshold = minNominal + 0.5 * range;
            double uncommonThreshold = minNominal + 0.8 * range;

            Console.WriteLine($"Legendary Threshold: {legendaryThreshold}");
            Console.WriteLine($"Epic Threshold: {epicThreshold}");
            Console.WriteLine($"Rare Threshold: {rareThreshold}");
            Console.WriteLine($"Uncommon Threshold: {uncommonThreshold}");


            // Assign rarities based on thresholds
            foreach (var item in items)
            {
                if (item.nominal <= legendaryThreshold)
                {
                    item.Rarity = ITEMRARITY.Legendary;
                }
                else if (item.nominal <= epicThreshold)
                {
                    item.Rarity = ITEMRARITY.Epic;
                }
                else if (item.nominal <= rareThreshold)
                {
                    item.Rarity = ITEMRARITY.Rare;
                }
                else if (item.nominal <= uncommonThreshold)
                {
                    item.Rarity = ITEMRARITY.Uncommon;
                }
                else
                {
                    item.Rarity = ITEMRARITY.Common;
                }
            }
        }
        public void SetallTypesasDirty()
        {
            vanillaTypes.isDirty = true;
            foreach (TypesFile tfile in ModTypesList)
            {
                tfile.isDirty = true;
            }
        }
    }
}
