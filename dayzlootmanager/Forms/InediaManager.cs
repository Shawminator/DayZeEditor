using DarkUI.Forms;
using DayZeLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DayZeEditor
{
    public partial class InediaManager : DarkForm
    {
        public Project currentproject { get; set; }
        public string Projectname { get; private set; }
        public InediaMovementAdmins_Config InediaMovementAdmins_Config { get; set; }
        public string InediaMovementAdmins_ConfigPath { get; set; }
        public InediaInfectedAI_Config InediaInfectedAI_Config { get; set; }
        public string InediaInfectedAI_ConfigPath { get; set; }
        private bool useraction;

        public InediaManager()
        {
            InitializeComponent();
        }

        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void SaveFile()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");






            string message = "The Following Files were saved....\n";
            int i = 0;
            foreach (string l in midifiedfiles)
            {
                if (i == 5)
                {
                    message += l + "\n";
                    i = 0;
                }
                else
                {
                    message += l + ", ";
                    i++;
                }

            }
            if (midifiedfiles.Count > 0)
                MessageBox.Show(message, "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            else
                MessageBox.Show("No changes were made.", "Nothing Saved", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Inedia");
        }

        private void InediaManager_Load(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Projectname = currentproject.ProjectName;

            useraction = false;
            InediaMovementAdmins_ConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Inedia\\InediaAdminsConfig.json";
            InediaMovementAdmins_Config = JsonSerializer.Deserialize<InediaMovementAdmins_Config>(File.ReadAllText(InediaMovementAdmins_ConfigPath));
            InediaMovementAdmins_Config.isDirty = false;
            InediaMovementAdmins_Config.Filename = InediaMovementAdmins_ConfigPath;

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Converters = { new BoolConverter() }
            };
            InediaInfectedAI_ConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Inedia\\InediaInfectedAIConfig.json";
            InediaInfectedAI_Config = JsonSerializer.Deserialize<InediaInfectedAI_Config>(File.ReadAllText(InediaInfectedAI_ConfigPath), options);
            InediaInfectedAI_Config.isDirty = false;
            InediaInfectedAI_Config.Filename = InediaInfectedAI_ConfigPath;

            AddObjectToTreeView(InediaInfectedAI_Config, InediaModTV.Nodes, InediaInfectedAI_Config.GetType().Name);

            useraction = true;

        }
        private void AddObjectToTreeView(object obj, TreeNodeCollection nodes, string nodeName)
        {
            TreeNode node = new TreeNode(nodeName);
            nodes.Add(node);

            if (obj == null)
            {
                node.Nodes.Add("null");
                return;
            }

            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                try
                {
                    object propertyValue = property.GetValue(obj);

                    if (propertyValue == null)
                    {
                        node.Nodes.Add($"{property.Name}: null");
                    }
                    else if (propertyValue is IDictionary dictionary)
                    {
                        TreeNode dictionaryNode = new TreeNode(property.Name);
                        node.Nodes.Add(dictionaryNode);

                        foreach (DictionaryEntry entry in dictionary)
                        {
                            TreeNode keyNode = new TreeNode(entry.Key.ToString());
                            dictionaryNode.Nodes.Add(keyNode);
                            dictionaryNode.Tag = entry.Value;
                            
                            if(entry.Value is List<string> list)
                            {
                                foreach(string l in list)
                                {
                                    TreeNode snode = new TreeNode(l);
                                    keyNode.Nodes.Add(snode);
                                }
                            }
                            else
                            {
                                AddObjectToTreeView(entry.Value, keyNode.Nodes, entry.Value?.GetType().Name ?? "null");
                            }
                        }
                    }
                    else if (propertyValue is IEnumerable enumerable && !(propertyValue is string))
                    {
                        TreeNode listNode = new TreeNode(property.Name) { Tag = propertyValue };
                        node.Nodes.Add(listNode);

                        if (propertyValue is IList list)
                        {
                            foreach(string s in list)
                            {
                                TreeNode snode = new TreeNode(s);
                                listNode.Nodes.Add(snode);
                            }
                        }
                        else
                        {
                            foreach (object item in enumerable)
                            {
                                AddObjectToTreeView(item, listNode.Nodes, item?.GetType().Name ?? "null");
                            }
                        }
                    }
                    else
                    {
                        TreeNode propertyNode = new TreeNode(property.Name);
                        node.Nodes.Add(propertyNode);

                        // If the property is a class and not a simple type, add its properties as child nodes
                        if (propertyValue.GetType().IsClass && propertyValue.GetType() != typeof(string))
                        {
                            AddObjectToTreeView(propertyValue, propertyNode.Nodes, propertyValue.GetType().Name);
                        }
                    }
                }
                catch (TargetParameterCountException)
                {
                    node.Nodes.Add($"{property.Name}: [Indexer Property]");
                }
            }
        }


        private void IndeiaManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (AbandonedVehicleRemover.isDirty)
            //{
            //    DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
            //    if (dialogResult == DialogResult.Yes)
            //    {
            //        SaveFile();
            //    }
            //}
        }
    }
}
