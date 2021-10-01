using DayZeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace DayZeEditor
{
    public class MissionTemplate
    {
        public string m_TemplatePath { get; set; }
        public string m_DisplayName { get; set; }
        public string m_mpmissionName { get; set; }
        public int m_Mapsize { get; set; }
        public override string ToString()
        {
            return m_DisplayName;
        }
    }
    public class ProjectList
    {
        public BindingList<MissionTemplate> AvailableTemplates { get; set; }
        public string ActiveProject { get; set; }
        public List<Project> Projects { get; set; }

        public ProjectList()
        {
            Projects = new List<Project>();
        }
        public void addtoprojects(Project project)
        {
            Projects.Add(project);
            ActiveProject = project.ProjectName;
            SaveProject();
        }
        public Project getprojectfromname(string name)
        {
            return Projects.FirstOrDefault(x => x.ProjectName == name);
        }
        public void SaveProject()
        {
            var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
            string jsonString = JsonSerializer.Serialize(this, options);
            Directory.CreateDirectory(Application.StartupPath + "\\Project");
            File.WriteAllText(Application.StartupPath + "\\Project\\Projects.json", jsonString);
        }

        public void SetActiveProject(string profilename)
        {
            Project p = getprojectfromname(profilename);
            ActiveProject = profilename;
            SaveProject();
        }
        public Project getActiveProject()
        {
            return getprojectfromname(ActiveProject);
        }

        internal void DeleteProject(string profilename)
        {
            Project p = getprojectfromname(profilename);
            Projects.Remove(p);
        }
    }
    public class Project
    {
        public string ProjectName { get; set; }
        public string projectFullName { get; set; }
        public string ProfilePath { get; set; }
        public bool isExpansion { get; set; }
        public bool usingDrJoneTrader { get; set; }
        public bool usingexpansionMarket { get; set; }
        public string MapPath { get; set; }
        public int MapSize { get; set; }
        public string mpmissionpath { get; set; }

        [JsonIgnore]
        public economycoreconfig EconomyCore { get; set; }
        [JsonIgnore]
        public Limitsdefinitions limitfefinitions { get; set; }
        [JsonIgnore]
        public Limitsdefinitionsuser limitfefinitionsuser { get; set; }

        [JsonIgnore]
        public eventscofig eventfile { get; set; }

        private TypesFile vanillaTypes;
        private BindingList<TypesFile> ModTypesList;


        public Project()
        {
   
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
        public void SetModListtypes()
        {
            ModTypesList = new BindingList<TypesFile>();
            foreach(ce mods in EconomyCore.economycore.ce)
            {
                string path = projectFullName + "\\mpmissions\\" + mpmissionpath + "\\" + mods.folder;
                foreach(file file in mods.file)
                {
                    if (file.type == "types")
                        SetmodTypes(path + "\\" + file.name);
                }
            }


            //foreach (string mod in Mods)
            //{
            //    SetmodTypes(mod);
            //}
        }
        internal void removeMod(string modname)
        {
            ModTypesList.Remove(GetModTypebyname(modname));
        }
        internal void seteconomycore()
        {
            EconomyCore = new economycoreconfig(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\cfgeconomycore.xml");
        }
        internal void seteconomydefinitions()
        {
            limitfefinitions = new Limitsdefinitions(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\cfglimitsdefinition.xml");
        }
        internal void SetEvents()
        {
            eventfile = new eventscofig(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\db\\events.xml");
        }
        internal void setuserdefinitions()
        {
            limitfefinitionsuser  = new Limitsdefinitionsuser(projectFullName + "\\mpmissions\\" + mpmissionpath + "\\cfglimitsdefinitionuser.xml");
        }
    }

}
