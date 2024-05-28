using DarkUI.Forms;
using DayZeLib;
using Microsoft.VisualBasic.ApplicationServices;
using PVZ.CustomZombiesGlobals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using TreeViewMS;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using pvzmodcharactertypes = PVZ.CustomZombiesCharacteristics.typesType;
using pvzmodglobaltypes = PVZ.CustomZombiesGlobals.typesType;
using pvzmodinfopaneltypes = PVZ.InfoPanel.typesType;

namespace DayZeEditor
{
    public partial class PVZCZManager : DarkForm
    {
        public Project currentproject { get; set; }
        public string Projectname { get; set; }
        public TypesFile vanillatypes { get; set; }
        public List<TypesFile> ModTypes { get; set; }

        public myXmlDocument PvzmodTemplates { get; set; }

        private bool useraction;

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            ListBox lb = sender as ListBox;
            e.DrawBackground();
            if (lb.Items.Count == 0) return;
            Brush myBrush = Brushes.Black;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.White, e.Bounds);
            }
            else
            {
                myBrush = Brushes.White;
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(60, 63, 65)), e.Bounds);
            }
            e.Graphics.DrawString(lb.Items[e.Index].ToString(), e.Font, myBrush, e.Bounds);
            e.DrawFocusRectangle();
        }


        public PVZCZManager()
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
            foreach (TreeNode rootNode in PVZModTV.Nodes[0].Nodes)
            {
                if (rootNode.Tag is XmlElement myelement)
                {
                    if (myelement.OwnerDocument is myXmlDocument myxmlDoc)
                    {
                        if (myxmlDoc.isDirty == true)
                        {
                            myxmlDoc.Save(myxmlDoc.Filename);
                            midifiedfiles.Add(Path.GetFileNameWithoutExtension(myxmlDoc.Filename));
                            myxmlDoc.isDirty = false;
                        }
                    }
                }
            }

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
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_CustomisableZombies_Profile");
        }
        private void PVZCZManager_Load(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Projectname = currentproject.ProjectName;
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            PvzmodTemplates = new myXmlDocument();
            PvzmodTemplates.Load(Application.StartupPath + "\\TraderNPCs\\PVZMODnodeTemplates.xml");

            useraction = false;
            List<string> filepaths = new List<string>();
            if (File.Exists(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_Information_Panel\\PvZmoD_Information_Panel.xml"))
                filepaths.Add(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_Information_Panel\\PvZmoD_Information_Panel.xml");
            if (File.Exists(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_CustomisableZombies_Profile\\PvZmoD_CustomisableZombies_Globals.xml") &&
                File.Exists(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_CustomisableZombies_Profile\\PvZmoD_CustomisableZombies_Characteristics.xml"))
            {
                filepaths.Add(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_CustomisableZombies_Profile\\PvZmoD_CustomisableZombies_Globals.xml");
                filepaths.Add(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_CustomisableZombies_Profile\\PvZmoD_CustomisableZombies_Characteristics.xml");
            }
            if (File.Exists(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Pvz_TheDarkHorde_MainOptions.xml") &&
                File.Exists(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Coordinates\\Pvz_TheDarkHorde_CustomZones.xml") &&
                File.Exists(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Coordinates\\Pvz_TheDarkHorde_SafeZones.xml") &&
                File.Exists(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Coordinates\\Pvz_TheDarkHorde_TerrainLimits.xml") &&
                File.Exists(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Coordinates\\Pvz_TheDarkHorde_Waypoints.xml") &&
                File.Exists(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Other\\Pvz_TheDarkHorde___Debug__.xml") &&
                File.Exists(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Other\\Pvz_TheDarkHorde_CustomNightTime.xml") &&
                File.Exists(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Other\\Pvz_TheDarkHorde_EventManager.xml") &&
                File.Exists(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Other\\Pvz_TheDarkHorde_ScreenMessages.xml") &&
                File.Exists(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Other\\Pvz_TheDarkHorde_SoundsAndVisuals.xml") &&
                File.Exists(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Other\\SaveData\\Pvz_TheDarkHorde_SaveData.xml") &&
                File.Exists(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Zombies\\Pvz_TheDarkHorde_ZombiesList.xml") &&
                File.Exists(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Zombies\\Pvz_TheDarkHorde_ZombiesLoot.xml") &&
                File.Exists(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Zombies\\Pvz_TheDarkHorde_ZombiesOptions.xml")
            )
            {
                filepaths.Add(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Pvz_TheDarkHorde_MainOptions.xml");
                filepaths.Add(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Coordinates\\Pvz_TheDarkHorde_CustomZones.xml");
                filepaths.Add(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Coordinates\\Pvz_TheDarkHorde_SafeZones.xml");
                filepaths.Add(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Coordinates\\Pvz_TheDarkHorde_TerrainLimits.xml");
                filepaths.Add(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Coordinates\\Pvz_TheDarkHorde_Waypoints.xml");
                filepaths.Add(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Other\\Pvz_TheDarkHorde___Debug__.xml");
                filepaths.Add(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Other\\Pvz_TheDarkHorde_CustomNightTime.xml");
                filepaths.Add(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Other\\Pvz_TheDarkHorde_EventManager.xml");
                filepaths.Add(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Other\\Pvz_TheDarkHorde_ScreenMessages.xml");
                filepaths.Add(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Other\\Pvz_TheDarkHorde_SoundsAndVisuals.xml");
                filepaths.Add(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Other\\SaveData\\Pvz_TheDarkHorde_SaveData.xml");
                filepaths.Add(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Zombies\\Pvz_TheDarkHorde_ZombiesList.xml");
                filepaths.Add(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Zombies\\Pvz_TheDarkHorde_ZombiesLoot.xml");
                filepaths.Add(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\PvZmoD_TheDarkHorde_Profile_V2\\Zombies\\Pvz_TheDarkHorde_ZombiesOptions.xml");
            }
            PVZModTV.Nodes.Clear();
            TreeNode rootNode = new TreeNode("PvZmoD");
            PVZModTV.Nodes.Add(rootNode);
            foreach (string filepath in filepaths)
            {
                myXmlDocument xmlDoc = new myXmlDocument();
                xmlDoc.Load(filepath);
                xmlDoc.Filename = filepath;
                AddNode(xmlDoc.DocumentElement, rootNode);
                //PVZModTV.ExpandAll();
            }

            useraction = true;

        }
        private void AddNode(XmlNode xmlNode, TreeNode treeNode, int insertPosition = -1)
        {
            string displayText = xmlNode.Name;
            if (xmlNode.Name == "type" && xmlNode.Attributes["name"] != null)
            {
                displayText = xmlNode.Attributes["name"].Value;
            }
            if (xmlNode.Name == "type" && xmlNode.Attributes["Zombie_Category_Class_Names"] != null)
            {
                displayText = xmlNode.Attributes["Zombie_Category_Class_Names"].Value;
            }
            if (xmlNode.Name == "type" && xmlNode.Attributes["Zone_Name"] != null)
            {
                displayText = $"{xmlNode.Attributes["Zone_Name"].Name} = {xmlNode.Attributes["Zone_Name"].Value}";
            }
            if (xmlNode.Name == "type" && xmlNode.Attributes["NightTime_Master"] != null)
            {
                displayText = $"{xmlNode.Attributes["NightTime_Master"].Name} = {xmlNode.Attributes["NightTime_Master"].Value}";
            }
            if (xmlNode.Name == "type" && xmlNode.Attributes["DayTime_Master"] != null)
            {
                displayText = $"{xmlNode.Attributes["DayTime_Master"].Name} = {xmlNode.Attributes["DayTime_Master"].Value}";
            }
            if (xmlNode.Name == "type" && xmlNode.Attributes["Waypoints_List_To_Use"] != null)
            {
                displayText = $"{xmlNode.Attributes["Waypoints_List_To_Use"].Name} = {xmlNode.Attributes["Waypoints_List_To_Use"].Value}";
            }
            if (xmlNode.Name == "type" && xmlNode.Attributes["Waypoints_List_Name"] != null)
            {
                displayText = $"{xmlNode.Attributes["Waypoints_List_Name"].Name} = {xmlNode.Attributes["Waypoints_List_Name"].Value}";
            }
            if (xmlNode.Name == "type" && xmlNode.Attributes["Zone_To_Use"] != null)
            {
                displayText = $"{xmlNode.Attributes["Zone_To_Use"].Name} = {xmlNode.Attributes["Zone_To_Use"].Value}";
            }
            if (xmlNode.Name == "type" && xmlNode.Attributes["ZONE_TO_USE"] != null)
            {
                displayText = $"{xmlNode.Attributes["ZONE_TO_USE"].Name} = {xmlNode.Attributes["ZONE_TO_USE"].Value}";
            }
            if (xmlNode.Name == "NightTime_Zombie" && xmlNode.Attributes["Name"] != null)
            {
                displayText = $"{xmlNode.Attributes["Name"].Name} = {xmlNode.Attributes["Name"].Value}";
            }
            if (xmlNode.Name == "DayTime_Zombie" && xmlNode.Attributes["Name"] != null)
            {
                displayText = $"{xmlNode.Attributes["Name"].Name} = {xmlNode.Attributes["Name"].Value}";
            }
            if (xmlNode.Name == "Zone_Data" && xmlNode.Attributes["Name"] != null)
            {
                displayText = $"{xmlNode.Attributes["Name"].Name} = {xmlNode.Attributes["Name"].Value}";
            }
            if (xmlNode.Name == "types")
            {
                displayText = Path.GetFileNameWithoutExtension(xmlNode.BaseURI);
            }



            // Add the current node
            TreeNode childTreeNode = new TreeNode(displayText) { Tag = xmlNode };
            // Insert the new TreeNode at the specified position
            if (insertPosition >= 0 && insertPosition <= treeNode.Nodes.Count)
            {
                treeNode.Nodes.Insert(insertPosition, childTreeNode);
            }
            else
            {
                treeNode.Nodes.Add(childTreeNode);
            }

            // Add attributes as child nodes, except 'File_Name' for 'type' nodes
            if (xmlNode.Attributes != null)
            {
                foreach (XmlAttribute attribute in xmlNode.Attributes)
                {
                    if (!(xmlNode.Name == "type" &&
                          attribute.Name == "File_Name" &&
                          xmlNode.Name == "NightTime_Zombie" &&
                          xmlNode.Name == "DayTime_Zombie"
                          ) 
                          && 
                          (attribute.Name != "name" &&
                          attribute.Name != "Zone_To_Use" &&
                          attribute.Name != "ZONE_TO_USE" &&
                          attribute.Name != "Waypoints_List_To_Use" &&
                          attribute.Name != "Waypoints_List_Name" &&
                          attribute.Name != "NightTime_Master" &&
                          attribute.Name != "DayTime_Master" && 
                          attribute.Name != "Name" &&
                          attribute.Name != "Zombie_Category_Class_Names" &&
                          attribute.Name != "Zone_Name"
                          ))
                    {
                        TreeNode attributeNode = new TreeNode($"{attribute.Name} = {attribute.Value}") { Tag = attribute };
                        childTreeNode.Nodes.Add(attributeNode);
                    }
                }
            }

            // Recursively add child nodes and handle comments within them
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                if (childNode.NodeType != XmlNodeType.Comment) // Exclude #comment nodes
                {
                    AddNode(childNode, childTreeNode);
                }
            }
        }
        private void ABVManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool savefiles = false;
            foreach (TreeNode rootNode in PVZModTV.Nodes[0].Nodes)
            {
                if (rootNode.Tag is XmlElement myelement)
                {
                    if (myelement.OwnerDocument is myXmlDocument myxmlDoc)
                    {
                        if (myxmlDoc.isDirty == true)
                        {
                            savefiles = true;
                        }
                    }
                }
            }
            if (savefiles == true)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SaveFile();
                }
            }
        }
        public List<string> slotlist = new List<string>()
        {
            "Mask",
            "HeadGear",
            "Shoulder",
            "Melee",
            "Headgear",
            "Eyewear",
            "Gloves",
            "Armband",
            "Vest",
            "Body",
            "Back",
            "Hips",
            "Legs",
            "Feet",
            "Splint_Right",
            "Hands"
        };
        private void PVZModTV_AfterSelect(object sender, TreeViewEventArgs e)
        {
            useraction = false;
            listBox1.Items.Clear();
            listBox1.Visible = false;
            groupBox1.Height = 100;
            darkButton8.Visible = false;
            darkButton9.Visible = false;
            numericUpDown2.Visible = false;
            textBoxAttributeValue.ReadOnly = false;
            textBoxAttributeValue.Visible = true;
            isVehicle_Type_Resistance = false;
            comboBox1.Visible = false;
            TreeNode selectedNode = PVZModTV.SelectedNode;
            if (selectedNode != null)
            {
                if (selectedNode.Tag is XmlAttribute attribute)
                {
                    if (attribute.Name == "Slot")
                    {
                        groupBox1.Visible = true;
                        comboBox1.Visible = true;
                        comboBox1.Items.Clear();
                        comboBox1.Items.AddRange(slotlist.ToArray());
                        groupBox1.Text = GetattributeName(selectedNode);
                        textBoxAttributeName.Text = attribute.Name;
                        textBoxAttributeValue.Visible = false;
                        comboBox1.SelectedIndex = comboBox1.FindStringExact(attribute.Value);
                        textBoxComment.Text = GetComment(selectedNode);
                    }
                    else
                    {
                        groupBox1.Visible = true;
                        groupBox1.Text = GetattributeName(selectedNode);
                        textBoxAttributeName.Text = attribute.Name;
                        textBoxAttributeValue.Text = attribute.Value;
                        textBoxComment.Text = GetComment(selectedNode); // Get and display the associated comment
                        if (attribute.Name == "List")
                        {
                            textBoxAttributeValue.ReadOnly = true;
                            darkButton8.Visible = true;
                            darkButton9.Visible = true;
                            groupBox1.Height = 270;
                            listBox1.Visible = true;
                            foreach (string item in attribute.Value.Split(','))
                            {
                                if (item == "") continue;
                                listBox1.Items.Add(item);
                            }
                            if (listBox1.Items.Count > 0)
                                listBox1.SelectedIndex = 0;
                        }
                        else if (attribute.Name == "Vehicle_Type_Resistance")
                        {
                            isVehicle_Type_Resistance = true;
                            textBoxAttributeValue.ReadOnly = true;
                            darkButton8.Visible = true;
                            darkButton9.Visible = true;
                            numericUpDown2.Visible = true;
                            groupBox1.Height = 270;
                            listBox1.Visible = true;
                            foreach (string item in attribute.Value.Split(','))
                            {
                                if (item == "") continue;
                                listBox1.Items.Add(new Vehicle_Type_Resistance(item));
                            }
                            if (listBox1.Items.Count > 0)
                                listBox1.SelectedIndex = 0;
                        }
                    }
                }
                else if (e.Node.Tag is XmlElement element && e.Node.Parent.Text == "PvZmoD_CustomisableZombies_Characteristics")
                {
                    if (element.Attributes != null)
                    {
                        groupBox1.Visible = true;
                        groupBox1.Text = "Zombie Classname";
                        XmlAttribute zombieattribute = element.Attributes[0];
                        textBoxAttributeName.Text = zombieattribute.Name;
                        textBoxAttributeValue.Text = zombieattribute.Value;
                    }
                    textBoxComment.Text = "Zombie Classname, can be specific or a base class";
                }
                else if (e.Node.Tag is XmlElement element5 && e.Node.Parent.Text == "Pvz_TheDarkHorde_ZombiesList")
                {
                    if (element5.Attributes != null)
                    {
                        groupBox1.Visible = true;
                        groupBox1.Text = "Zombie Classname";
                        XmlAttribute zombieattribute = element5.Attributes[0];
                        textBoxAttributeName.Text = zombieattribute.Name;
                        textBoxAttributeValue.Text = zombieattribute.Value;
                    }
                    textBoxComment.Text = "Zombie Classname, Must Be Specific";
                }
                else if (e.Node.Tag is XmlElement szelement && e.Node.Parent.Text == "Pvz_TheDarkHorde_SafeZones")
                {
                    if (szelement.Attributes != null)
                    {
                        groupBox1.Visible = true;
                        groupBox1.Text = "Safe Zone Name";
                        XmlAttribute zombieattribute = szelement.Attributes[0];
                        textBoxAttributeName.Text = zombieattribute.Name;
                        textBoxAttributeValue.Text = zombieattribute.Value;
                    }
                    textBoxComment.Text = "";
                }
                else if (e.Node.Tag is XmlElement element1 && e.Node.Parent.Text == "Pvz_TheDarkHorde_CustomZones")
                {
                    if (element1.Attributes != null)
                    {
                        XmlAttribute zombieattribute = element1.Attributes[0];
                        comboBox1.Items.Clear();
                        comboBox1.Items.Add("Random");
                        foreach (XmlNode ele in element1.ChildNodes)
                        {
                            if (ele.NodeType == XmlNodeType.Element && ele.Name == "Zone_Data")
                            {
                                XmlAttribute zoneattribute = ele.Attributes[0];
                                comboBox1.Items.Add(zoneattribute.Value);
                            }
                        }
                        groupBox1.Visible = true;
                        groupBox1.Text = "Zone to use";
                        textBoxAttributeName.Text = zombieattribute.Name;
                        textBoxAttributeValue.Visible = false;
                        textBoxComment.Text = "Zone Name to use, if not found in the list a random one will be chosen";
                        groupBox1.Height = 270;
                        comboBox1.Visible = true;
                        comboBox1.SelectedIndex = comboBox1.FindStringExact(zombieattribute.Value);

                    }
                }
                else if (e.Node.Tag is XmlElement element6 && e.Node.Parent.Parent != null && e.Node.Parent.Parent.Text == "Pvz_TheDarkHorde_CustomZones")
                {
                    if (element6.Attributes != null)
                    {
                        groupBox1.Visible = true;
                        groupBox1.Text = "Zone Data";
                        XmlAttribute zombieattribute = element6.Attributes[0];
                        textBoxAttributeName.Text = zombieattribute.Name;
                        textBoxAttributeValue.Text = zombieattribute.Value;
                    }
                    textBoxComment.Text = "Zone Name";
                }
                else if (e.Node.Tag is XmlElement element2 && e.Node.Parent.Text == "Pvz_TheDarkHorde_TerrainLimits")
                {
                    if (element2.Attributes != null)
                    {
                        XmlAttribute zombieattribute = element2.Attributes[0];
                        if(zombieattribute.Name == "Zone_Name")
                        {
                            groupBox1.Visible = true;
                            groupBox1.Text = "Zone Data";
                            textBoxAttributeName.Text = zombieattribute.Name;
                            textBoxAttributeValue.Text = zombieattribute.Value;
                            textBoxComment.Text = "Zone Name";
                        }
                        else if (zombieattribute.Name == "ZONE_TO_USE")
                        {
                            comboBox1.Items.Clear();
                            XmlElement parent = e.Node.Parent.Tag as XmlElement;
                            foreach (XmlNode ele in parent.ChildNodes)
                            {
                                if (ele.NodeType == XmlNodeType.Element && ele.Attributes[0].Name == "Zone_Name")
                                {
                                    XmlAttribute zoneattribute = ele.Attributes[0];
                                    comboBox1.Items.Add(zoneattribute.Value);
                                }
                            }
                            groupBox1.Visible = true;
                            groupBox1.Text = "Zone to use";
                            textBoxAttributeName.Text = zombieattribute.Name;
                            textBoxAttributeValue.Visible = false;
                            textBoxComment.Text = "Zone Name to use, if not found in the list a random one will be chosen";
                            groupBox1.Height = 270;
                            comboBox1.Visible = true;
                            comboBox1.SelectedIndex = comboBox1.FindStringExact(zombieattribute.Value);
                        }
                    }
                }
                else if (e.Node.Tag is XmlElement element3 && e.Node.Parent.Text == "Pvz_TheDarkHorde_Waypoints")
                {
                    if (element3.Attributes != null)
                    {
                        XmlAttribute zombieattribute = element3.Attributes[0];
                        if (zombieattribute.Name == "Waypoints_List_To_Use")
                        {
                            comboBox1.Items.Clear();
                            XmlElement parent = e.Node.Parent.Tag as XmlElement;
                            foreach (XmlNode ele in parent.ChildNodes)
                            {
                                if (ele.NodeType == XmlNodeType.Element && ele.Attributes[0].Name == "Waypoints_List_Name")
                                {
                                    XmlAttribute zoneattribute = ele.Attributes[0];
                                    comboBox1.Items.Add(zoneattribute.Value);
                                }
                            }
                            groupBox1.Visible = true;
                            groupBox1.Text = "Zone to use";
                            textBoxAttributeName.Text = zombieattribute.Name;
                            textBoxAttributeValue.Visible = false;
                            textBoxComment.Text = "Zone Name to use, if not found in the list a random one will be chosen";
                            comboBox1.Visible = true;
                            comboBox1.SelectedIndex = comboBox1.FindStringExact(zombieattribute.Value);
                        }
                        else if (zombieattribute.Name == "Waypoints_List_Name")
                        {
                            groupBox1.Visible = true;
                            groupBox1.Text = "Way Point List";
                            textBoxAttributeName.Text = zombieattribute.Name;
                            textBoxAttributeValue.Text = zombieattribute.Value;
                            textBoxComment.Text = "Waypoint List Name";
                        }
                    }
                }
                else if (e.Node.Tag is XmlElement element4 && e.Node.Parent.Parent != null && e.Node.Parent.Parent.Text == "Pvz_TheDarkHorde_ZombiesList")
                {
                    if (element4.Attributes != null)
                    {
                        groupBox1.Visible = true;
                        groupBox1.Text = "Zombie Classname";
                        XmlAttribute zombieattribute = element4.Attributes[0];
                        textBoxAttributeName.Text = zombieattribute.Name;
                        textBoxAttributeValue.Text = zombieattribute.Value;
                    }
                    textBoxComment.Text = "Zombie Classname, Must Be specific";
                }
                else
                {
                    groupBox1.Visible = false;
                }
            }
            useraction = true;
        }
        private void PVZModTV_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode usingtreenode = e.Node;
            if (e.Button == MouseButtons.Right)
            {
                if(e.Node.Text == "PvZmoD_CustomisableZombies_Characteristics")
                {
                    removeSelectedZombieToolStripMenuItem.Visible = false;
                    addNewToolStripMenuItem.Visible = true;
                    addNewCustomZoneToolStripMenuItem.Visible = false;
                    removeSelectedCutomZoneToolStripMenuItem.Visible = false;
                    addNewSafeZoneToolStripMenuItem.Visible = false;
                    removeSelectedSafeZoneToolStripMenuItem.Visible = false;
                    contextMenuStrip1.Show(Cursor.Position);
                    
                }
                else if (e.Node.Text == "Pvz_TheDarkHorde_SafeZones")
                {
                    removeSelectedZombieToolStripMenuItem.Visible = false;
                    addNewToolStripMenuItem.Visible = false;
                    addNewCustomZoneToolStripMenuItem.Visible = false;
                    removeSelectedCutomZoneToolStripMenuItem.Visible = false;
                    addNewSafeZoneToolStripMenuItem.Visible = true;
                    removeSelectedSafeZoneToolStripMenuItem.Visible = false;
                    contextMenuStrip1.Show(Cursor.Position);

                }
                else if (e.Node.Parent.Text == "PvZmoD_CustomisableZombies_Characteristics")
                {
                    removeSelectedZombieToolStripMenuItem.Visible = true;
                    addNewToolStripMenuItem.Visible = false;
                    addNewCustomZoneToolStripMenuItem.Visible = false;
                    removeSelectedCutomZoneToolStripMenuItem.Visible = false;
                    addNewSafeZoneToolStripMenuItem.Visible = false;
                    removeSelectedSafeZoneToolStripMenuItem.Visible = false;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Parent.Text == "Pvz_TheDarkHorde_SafeZones")
                {
                    if (e.Node.Tag is XmlElement element)
                    {
                        if (element.Attributes[0].Value != "Global_Safe_Zones_Options")
                        {
                            removeSelectedZombieToolStripMenuItem.Visible = false;
                            addNewToolStripMenuItem.Visible = false;
                            addNewCustomZoneToolStripMenuItem.Visible = false;
                            removeSelectedCutomZoneToolStripMenuItem.Visible = false;
                            addNewSafeZoneToolStripMenuItem.Visible = false;
                            removeSelectedSafeZoneToolStripMenuItem.Visible = true;
                            contextMenuStrip1.Show(Cursor.Position);
                        }
                    }
                }
                else if (e.Node.Parent.Parent != null && e.Node.Parent.Text == "Pvz_TheDarkHorde_CustomZones")
                {
                    removeSelectedZombieToolStripMenuItem.Visible = false;
                    addNewToolStripMenuItem.Visible = false;
                    addNewCustomZoneToolStripMenuItem.Visible = true;
                    removeSelectedCutomZoneToolStripMenuItem.Visible = false;
                    addNewSafeZoneToolStripMenuItem.Visible = false;
                    removeSelectedSafeZoneToolStripMenuItem.Visible = false;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Parent.Parent != null && e.Node.Parent.Parent.Text == "Pvz_TheDarkHorde_CustomZones")
                {
                    removeSelectedZombieToolStripMenuItem.Visible = false;
                    addNewToolStripMenuItem.Visible = false;
                    addNewCustomZoneToolStripMenuItem.Visible = false;
                    removeSelectedCutomZoneToolStripMenuItem.Visible = true;
                    addNewSafeZoneToolStripMenuItem.Visible = false;
                    removeSelectedSafeZoneToolStripMenuItem.Visible = false;
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
            PVZModTV.SelectedNode = usingtreenode;
        }
        private string GetattributeName(TreeNode selectedNode)
        {
            TreeNode parentNode = selectedNode.Parent;
            if (parentNode != null && parentNode.Nodes.Count > 0)
            {
                if (parentNode.Tag is XmlNode)
                {
                    XmlNode commentnode = parentNode.Tag as XmlNode;
                    return commentnode.Name; // Return the comment associated with the attribute
                }
            }
            return "";
        }
        private string GetComment(TreeNode attributeNode)
        {
            TreeNode parentNode = attributeNode.Parent;
            if (parentNode.Parent.Parent.Text == "PvZmoD_CustomisableZombies_Characteristics")
            {
                string text = parentNode.Text;
                XmlNode lastnode = parentNode.Parent.Parent.LastNode.Tag as XmlNode;
                foreach(XmlNode child in lastnode.ChildNodes)
                {
                    if(child.Name == text)
                        return child.NextSibling.Value;
                }
            }
            else
            {
                if (parentNode != null && parentNode.Nodes.Count > 0)
                {
                    if (parentNode.Tag is XmlNode)
                    {
                        XmlNode commentnode = parentNode.Tag as XmlNode;
                        if (commentnode.NextSibling != null && commentnode.NextSibling.NodeType == XmlNodeType.Comment)
                            return commentnode.NextSibling.Value; // Return the comment associated with the attribute
                    }
                }
            }
            return ""; // Return empty string if no comment is found
        }
        private void textBoxAttributeValue_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TreeNode selectedNode = PVZModTV.SelectedNode;
            if (selectedNode != null)
            {
                if (selectedNode.Tag is XmlAttribute attribute)
                {
                    // Update attribute value
                    attribute.Value = textBoxAttributeValue.Text;
                    selectedNode.Text = $"{attribute.Name} = {attribute.Value}";
                    MarkasDirty(attribute);
                }
                else if (selectedNode.Tag is XmlElement element && 
                        (selectedNode.Parent.Text == "PvZmoD_CustomisableZombies_Characteristics" ||
                        selectedNode.Parent.Text == "Pvz_TheDarkHorde_SafeZones" ||
                        selectedNode.Parent.Text == "Pvz_TheDarkHorde_Waypoints" ||
                        selectedNode.Parent.Text == "Pvz_TheDarkHorde_ZombiesList" ||
                        selectedNode.Parent.Text == "Pvz_TheDarkHorde_TerrainLimits" ||
                        selectedNode.Parent.Parent.Text == "Pvz_TheDarkHorde_Waypoints" ||
                        selectedNode.Parent.Parent.Text == "Pvz_TheDarkHorde_ZombiesList" ||
                        selectedNode.Parent.Parent.Text == "Pvz_TheDarkHorde_CustomZones"))
                {
                    if (element.Attributes != null)
                    {
                        XmlAttribute zombieattribute = element.Attributes[0];
                        zombieattribute.Value = textBoxAttributeValue.Text;
                            if(selectedNode.Parent.Text == "Pvz_TheDarkHorde_Waypoints" ||
                            selectedNode.Parent.Text == "Pvz_TheDarkHorde_ZombiesList" ||
                            selectedNode.Parent.Text == "Pvz_TheDarkHorde_TerrainLimits" ||
                            selectedNode.Parent.Parent.Text == "Pvz_TheDarkHorde_Waypoints" ||
                            selectedNode.Parent.Parent.Text == "Pvz_TheDarkHorde_ZombiesList" ||
                            selectedNode.Parent.Parent.Text == "Pvz_TheDarkHorde_CustomZones")
                            selectedNode.Text = $"{zombieattribute.Name} = {zombieattribute.Value}";
                        else
                            selectedNode.Text = zombieattribute.Value;
                        MarkasDirty(zombieattribute);
                    }
                }
            }
            
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TreeNode selectedNode = PVZModTV.SelectedNode;
            if (selectedNode != null)
            {
                if (selectedNode.Tag is XmlAttribute attribute)
                {
                    attribute.Value = comboBox1.GetItemText(comboBox1.SelectedItem);
                    selectedNode.Text = $"{attribute.Name} = {attribute.Value}";
                    MarkasDirty(attribute);
                }
                else if (selectedNode.Tag is XmlElement element && (selectedNode.Parent.Text == "Pvz_TheDarkHorde_CustomZones" || selectedNode.Parent.Text == "Pvz_TheDarkHorde_TerrainLimits"))
                {
                    if (element.Attributes != null)
                    {
                        XmlAttribute zoneattribute = element.Attributes[0];
                        zoneattribute.Value = comboBox1.GetItemText(comboBox1.SelectedItem); ;
                        selectedNode.Text = $"{zoneattribute.Name} = {zoneattribute.Value}";
                        MarkasDirty(zoneattribute);
                    }
                }
            }
        }
        private void MarkasDirty(XmlAttribute attribute)
        {
            myXmlDocument mydoc = attribute.OwnerDocument as myXmlDocument;
            mydoc.isDirty = true;
        }
        private void MarkasDirty(XmlElement element)
        {
            myXmlDocument mydoc = element.OwnerDocument as myXmlDocument;
            mydoc.isDirty = true;
        }

        public bool isVehicle_Type_Resistance = false;
        private void darkButton8_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseOnlySingleitem = false
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (isVehicle_Type_Resistance == true)
                {
                    List<string> addedtypes = form.addedtypes.ToList();
                    foreach (string l in addedtypes)
                    {
                        Vehicle_Type_Resistance newVehicle_Type_Resistance = new Vehicle_Type_Resistance()
                        {
                            Classname = l,
                            Value = 100
                        };
                        if(!listBox1.Items.Cast<Vehicle_Type_Resistance>().Any(x => x.Classname == newVehicle_Type_Resistance.Classname))
                        {
                            listBox1.Items.Add(newVehicle_Type_Resistance);
                        }
                    }
                    var myOtherList = listBox1.Items.Cast<Vehicle_Type_Resistance>().ToList();
                    string att = "";
                    foreach (Vehicle_Type_Resistance l in myOtherList)
                    {
                        att += l.GetString() + ",";
                    }
                    textBoxAttributeValue.Text = att;
                }
                else
                {
                    List<string> addedtypes = form.addedtypes.ToList();
                    foreach (string l in addedtypes)
                    {
                        if (!listBox1.Items.Contains(l))
                            listBox1.Items.Add(l);
                    }
                    var myOtherList = listBox1.Items.Cast<String>().ToList();
                    string att = "";
                    foreach (string l in myOtherList)
                    {
                        att += l + ",";
                    }
                    textBoxAttributeValue.Text = att;
                }
            }
        }
        private void darkButton9_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem is Vehicle_Type_Resistance Vehicle_Type_Resistance)
            {
                listBox1.Items.Remove(Vehicle_Type_Resistance);
                var myOtherList = listBox1.Items.Cast<Vehicle_Type_Resistance>().ToList();
                string att = "";
                foreach (Vehicle_Type_Resistance l in myOtherList)
                {
                    att += l.GetString() + ",";
                }
                textBoxAttributeValue.Text = att;
            }
            else
            {
                string item = listBox1.GetItemText(listBox1.SelectedItem);
                listBox1.Items.Remove(item);
                var myOtherList = listBox1.Items.Cast<String>().ToList();
                string att = "";
                foreach (string l in myOtherList)
                {
                    att += l + ",";
                }
                textBoxAttributeValue.Text = att;
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedItems.Count <= 0) { return; }
            useraction = false;
            if (listBox1.SelectedItem is Vehicle_Type_Resistance Vehicle_Type_Resistance)
            {
                numericUpDown2.Value = Vehicle_Type_Resistance.Value;
            }
            useraction = true;
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if(!useraction) { return; }
            if (listBox1.SelectedItem is Vehicle_Type_Resistance Vehicle_Type_Resistance)
            {
                Vehicle_Type_Resistance.Value = (int)numericUpDown2.Value;
                var myOtherList = listBox1.Items.Cast<Vehicle_Type_Resistance>().ToList();
                string att = "";
                foreach (Vehicle_Type_Resistance l in myOtherList)
                {
                    att += l.GetString() + ",";
                }
                textBoxAttributeValue.Text = att;
            }
            
        }
        private void addNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XmlElement element = PVZModTV.SelectedNode.Tag as XmlElement;
            XmlNode clonednode = null;
            foreach (XmlNode node in PvzmodTemplates.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.XmlDeclaration) continue;

                if (node.Name == "PvZmoD")
                {
                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        if (node2.Name == "PvZmoD_CustomisableZombies_Characteristics")
                        {
                            clonednode = node2.ChildNodes[0].CloneNode(true);
                        }
                    }
                }
            }
            XmlDocument currentDocument = element.OwnerDocument;
            XmlNode importedNode = currentDocument.ImportNode(clonednode, true);
            element.InsertAfter(importedNode, element.ChildNodes[1]);
            MarkasDirty(element);
            AddNode(importedNode, PVZModTV.SelectedNode, 0);
        }
        private void removeSelectedZombieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XmlElement element = PVZModTV.SelectedNode.Tag as XmlElement;
            XmlElement Parentelement = PVZModTV.SelectedNode.Parent.Tag as XmlElement;
            Parentelement.RemoveChild(element);
            MarkasDirty(Parentelement);
            PVZModTV.SelectedNode.Remove();
        }
        private void addNewCustomZoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XmlElement element = PVZModTV.SelectedNode.Tag as XmlElement;
            XmlNode clonednode = null;
            int lastindex = 0;
            bool zonedata = false;
            foreach (XmlNode node in element.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Comment)
                {
                    if (zonedata != true)
                        lastindex++;
                    continue;
                }
                if(node.Attributes != null)
                {
                    if (node.Name == "Zone_Data")
                    {
                        lastindex++;
                        zonedata = true;
                    }
                }
            }
            foreach (XmlNode node in PvzmodTemplates.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.XmlDeclaration) continue;

                if (node.Name == "PvZmoD")
                {
                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        if (node2.Name == "Pvz_TheDarkHorde_CustomZones")
                        {
                            clonednode = node2.ChildNodes[0].CloneNode(true);
                        }
                    }
                }
            }
            XmlDocument currentDocument = element.OwnerDocument;
            XmlNode importedNode = currentDocument.ImportNode(clonednode, true);
            element.InsertAfter(importedNode, element.ChildNodes[lastindex-1]);
            MarkasDirty(element);
            AddNode(importedNode, PVZModTV.SelectedNode, PVZModTV.SelectedNode.Nodes.Count);
        }
        private void removeSelectedCutomZoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XmlElement element = PVZModTV.SelectedNode.Tag as XmlElement;
            XmlElement Parentelement = PVZModTV.SelectedNode.Parent.Tag as XmlElement;
            Parentelement.RemoveChild(element);
            MarkasDirty(Parentelement);
            PVZModTV.SelectedNode.Remove();
        }
        private void addNewSafeZoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XmlElement element = PVZModTV.SelectedNode.Tag as XmlElement;
            XmlNode clonednode = null;
            int lastindex = 0;
            bool zonedata = false;
            foreach (XmlNode node in element.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Comment)
                {
                    if (zonedata != true)
                        lastindex++;
                    continue;
                }
                if (node.Attributes != null)
                {
                    if (node.Name == "Zone_Data")
                    {
                        lastindex++;
                        zonedata = true;
                    }
                }
            }
            foreach (XmlNode node in PvzmodTemplates.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.XmlDeclaration) continue;

                if (node.Name == "PvZmoD")
                {
                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        if (node2.Name == "Pvz_TheDarkHorde_SafeZones")
                        {
                            clonednode = node2.ChildNodes[0].CloneNode(true);
                            break;
                        }
                    }
                }
            }
            XmlDocument currentDocument = element.OwnerDocument;
            XmlNode importedNode = currentDocument.ImportNode(clonednode, true);
            element.InsertAfter(importedNode, element.ChildNodes[element.ChildNodes.Count -1]);
            MarkasDirty(element);
            AddNode(importedNode, PVZModTV.SelectedNode, PVZModTV.SelectedNode.Nodes.Count);
        }
        private void removeSelectedSafeZoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XmlElement element = PVZModTV.SelectedNode.Tag as XmlElement;
            XmlElement Parentelement = PVZModTV.SelectedNode.Parent.Tag as XmlElement;
            Parentelement.RemoveChild(element);
            MarkasDirty(Parentelement);
            PVZModTV.SelectedNode.Remove();
        }
    }
    public class Vehicle_Type_Resistance
    {
        public string Classname { get; set; }
        public int Value { get; set; }

        public Vehicle_Type_Resistance()
        {

        }
        public Vehicle_Type_Resistance(string _string)
        {
            string[] line = _string.Split(':');
            Classname = line[0];
            Value = int.Parse(line[1]);
        }
        public string GetString()
        {
            return Classname + ":" + Value.ToString();
        }
        public override string ToString()
        {
            return Classname;
        }
    }
}
