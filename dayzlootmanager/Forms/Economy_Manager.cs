using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.Xsl;
using Cyotek.Windows.Forms;
using DarkUI.Forms;
using DayZeLib;

namespace DayZeEditor
{
    public partial class Economy_Manager : DarkForm
    {
        public bool isUserInteraction = true;
        public Project currentproject { get; set; }
        public int ZoneScale = 1;
        public TypesFile vanillatypes;
        public List<TypesFile> ModTypes;
        public TypesFile currentTypesFile;
        public typesType currentlootpart;
        public string currentcollection;
        public string filename;
        public bool Fileloaded = false;
        public bool FullTypes;
        private int searchnum;
        private List<TreeNode> searchtreeNodes;
        private List<typesType> foundparts;
        public playerspawnpoints playerspawnpoints;
        public BindingList<randompresetsAttachments> cargoAttachments = new BindingList<randompresetsAttachments>();
        public BindingList<randompresetsCargo> cargoItems = new BindingList<randompresetsCargo>();
        #region formsstuff
        public Economy_Manager()
        {
            InitializeComponent();
            isUserInteraction = false;

            isUserInteraction = true;

        }
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
        private void Economy_Manager_Load(object sender, EventArgs e)
        {
            isUserInteraction = false;
            WeatherTabPage.ItemSize = new Size(0, 1);

            filename = currentproject.ProjectName;
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            comboBox1.DataSource = currentproject.limitfefinitions.lists.categories;
            comboBox2.DataSource = currentproject.limitfefinitions.lists.usageflags;
            comboBox4.DataSource = currentproject.limitfefinitions.lists.tags;

            comboBox5.DataSource = currentproject.limitfefinitions.lists.tags;

            comboBox8.DataSource = currentproject.limitfefinitions.lists.categories;
            comboBox7.DataSource = currentproject.limitfefinitions.lists.usageflags;
            comboBox6.DataSource = currentproject.limitfefinitions.lists.tags;
            SetuprandomPresetsForSpawnabletypes();

            PopulateTreeView();
            Loadevents();
            LoadRandomPresets();
            LoadSpawnableTypes();
            populateEconmyTreeview();
            LoadPlayerSpawns();
            LoadCFGGamelplay();
            LoadContaminatoedArea();
            LoadGlobals();
            Loadweather();
            SetSummarytiers();
            NomCountLabel.Text = "Total Nominal Count :- " + currentproject.TotalNomCount.ToString();



            pictureBox2.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Livonia maop size is 12800 x 12800, 0,0 bottom left, center 6400 x 6400
            pictureBox2.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox2.Paint += new PaintEventHandler(DrawAllPlayerSpawns);
            pictureBox2.Invalidate();
            trackBar4.Value = 1;
            SetSpawnScale();

            isUserInteraction = true;
        }
        private void SetuprandomPresetsForSpawnabletypes()
        {
            foreach (var item in currentproject.cfgrandompresetsconfig.randompresets.Items)
            {
                if (item is randompresetsAttachments)
                {
                    cargoAttachments.Add(item as randompresetsAttachments);
                }
                else if (item is randompresetsCargo)
                {
                    cargoItems.Add(item as randompresetsCargo);
                }
            }

            CargoPresetComboBox.DataSource = cargoItems;
            AttachmentPresetComboBox.DataSource = cargoAttachments;
        }
        private void populateEconmyTreeview()
        {
            var serializer = new XmlSerializer(typeof(economycore));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var sw = new StringWriter();
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true });
            serializer.Serialize(xmlWriter, currentproject.EconomyCore.economycore, ns);
            

            // Load the xslt used by IE to render the xml
            XslCompiledTransform xTrans = new XslCompiledTransform();
            xTrans.Load(Application.StartupPath + "//lib//defaultss.xsl");

            // Read the xml string.
            StringReader sr = new StringReader(sw.ToString());
            XmlReader xReader = XmlReader.Create(sr);

            // Transform the XML data
            MemoryStream ms = new MemoryStream();
            xTrans.Transform(xReader, null, ms);

            ms.Position = 0;

            // Set to the document stream
            webBrowser1.DocumentStream = ms;
        }
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Process.Start(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath);
        }
        private void SaveFileButton_Click(object sender, EventArgs e) //empty
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (vanillatypes.isDirty)
            {
                vanillatypes.SaveTyes(SaveTime);
                vanillatypes.isDirty = false;
                midifiedfiles.Add(Path.GetFileName(vanillatypes.Filename));
            }
            foreach (TypesFile tf in ModTypes)
            {
                if (tf.isDirty)
                {
                    tf.SaveTyes(SaveTime);
                    tf.isDirty = false;
                    midifiedfiles.Add(Path.GetFileName(tf.Filename));
                }
            }

            foreach (eventscofig eventconfig in currentproject.ModEventsList)
            {
                if (eventconfig.isDirty)
                {
                    eventconfig.SaveEvent(SaveTime);
                    eventconfig.isDirty = false;
                    midifiedfiles.Add(Path.GetFileName(eventconfig.Filename));
                }
            }

            if (currentproject.spawnabletypesList != null)
            {
                foreach (Spawnabletypesconfig Spawnabletypesconfig in currentproject.spawnabletypesList)
                {
                    if (Spawnabletypesconfig.isDirty)
                    {
                        Spawnabletypesconfig.Savespawnabletypes(SaveTime);
                        Spawnabletypesconfig.isDirty = false;
                        midifiedfiles.Add(Path.GetFileName(Spawnabletypesconfig.Filename));
                    }
                }
            }
            if (currentproject.cfgrandompresetsconfig != null)
            {
                if (currentproject.cfgrandompresetsconfig.isDirty)
                {
                    currentproject.cfgrandompresetsconfig.SaveRandomPresets(SaveTime);
                    currentproject.cfgrandompresetsconfig.isDirty = false;
                    midifiedfiles.Add(Path.GetFileName(currentproject.cfgrandompresetsconfig.Filename));
                }
            }

            if (currentproject.cfgplayerspawnpoints != null)
            {
                    if (currentproject.cfgplayerspawnpoints.isDirty)
                    {
                        currentproject.cfgplayerspawnpoints.Savecfgplayerspawnpoints(SaveTime);
                        currentproject.cfgplayerspawnpoints.isDirty = false;
                        midifiedfiles.Add(Path.GetFileName(currentproject.cfgplayerspawnpoints.Filename));
                    }
            }
            if (currentproject.CFGGameplayConfig != null)
            {
                if (currentproject.CFGGameplayConfig.isDirty)
                {
                    currentproject.CFGGameplayConfig.SaveCFGGameplay();
                    currentproject.CFGGameplayConfig.isDirty = false;
                    midifiedfiles.Add(Path.GetFileName(currentproject.CFGGameplayConfig.Filename) + " Saved....");
                }
            }
            if (currentproject.cfgEffectAreaConfig != null)
            {
                if (currentproject.cfgEffectAreaConfig.isDirty)
                {
                    currentproject.cfgEffectAreaConfig.SavecfgEffectArea();
                    currentproject.cfgEffectAreaConfig.isDirty = false;
                    midifiedfiles.Add(Path.GetFileName(currentproject.cfgEffectAreaConfig.Filename) + " Saved....");
                }
            }

            if (currentproject.gloabsconfig != null)
            {
                if (currentproject.gloabsconfig.isDirty)
                {
                    currentproject.gloabsconfig.SaveGlobals(SaveTime);
                    currentproject.gloabsconfig.isDirty = false;
                    midifiedfiles.Add(Path.GetFileName(currentproject.gloabsconfig.Filename));
                }
            }
            if (currentproject.weatherconfig != null)
            {
                if (currentproject.weatherconfig.isDirty)
                {
                    currentproject.weatherconfig.SaveWeather(SaveTime);
                    currentproject.weatherconfig.isDirty = false;
                    midifiedfiles.Add(Path.GetFileName(currentproject.weatherconfig.Filename));
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
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            WeatherTabPage.SelectedIndex = 0;
            if (tabControl3.SelectedIndex == 0)
                toolStripButton3.Checked = true;
        }
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            WeatherTabPage.SelectedIndex = 1;
            if (tabControl3.SelectedIndex == 1)
                toolStripButton5.Checked = true;
        }
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            WeatherTabPage.SelectedIndex = 2;
            if (tabControl3.SelectedIndex == 2)
                toolStripButton6.Checked = true;
        }
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            WeatherTabPage.SelectedIndex = 3;
            if (tabControl3.SelectedIndex== 3)
                toolStripButton8.Checked = true;
        }
        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            WeatherTabPage.SelectedIndex = 4;
            if (tabControl3.SelectedIndex == 4)
                toolStripButton9.Checked = true;
        }
        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            WeatherTabPage.SelectedIndex = 5;
            if (tabControl3.SelectedIndex == 5)
                toolStripButton10.Checked = true;
        }
        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            WeatherTabPage.SelectedIndex = 6;
            if (tabControl3.SelectedIndex == 6)
                toolStripButton12.Checked = true;
        }
        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            WeatherTabPage.SelectedIndex = 7;
            if (tabControl3.SelectedIndex == 7)
                toolStripButton11.Checked = true;
        }
        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            WeatherTabPage.SelectedIndex = 8;
            if (tabControl3.SelectedIndex == 8)
                toolStripButton13.Checked = true;
        }
        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            WeatherTabPage.SelectedIndex = 9;
            if (tabControl3.SelectedIndex == 9)
                toolStripButton14.Checked = true;
        }
        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            WeatherTabPage.SelectedIndex = 10;
            if (tabControl3.SelectedIndex == 10)
                toolStripButton15.Checked = true;
        }
        private void tabControl4_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (WeatherTabPage.SelectedIndex)
            {
                case 0:
                    toolStripButton5.Checked = false;
                    toolStripButton6.Checked = false;
                    toolStripButton8.Checked = false;
                    toolStripButton9.Checked = false;
                    toolStripButton10.Checked = false;
                    toolStripButton11.Checked = false;
                    toolStripButton12.Checked = false;
                    toolStripButton13.Checked = false;
                    toolStripButton14.Checked = false;
                    toolStripButton15.Checked = false;
                    break;
                case 1:
                    toolStripButton3.Checked = false;
                    toolStripButton6.Checked = false;
                    toolStripButton8.Checked = false;
                    toolStripButton9.Checked = false;
                    toolStripButton10.Checked = false;
                    toolStripButton11.Checked = false;
                    toolStripButton12.Checked = false;
                    toolStripButton13.Checked = false;
                    toolStripButton14.Checked = false;
                    toolStripButton15.Checked = false;
                    break;
                case 2:
                    toolStripButton3.Checked = false;
                    toolStripButton5.Checked = false;
                    toolStripButton8.Checked = false;
                    toolStripButton9.Checked = false;
                    toolStripButton10.Checked = false;
                    toolStripButton11.Checked = false;
                    toolStripButton12.Checked = false;
                    toolStripButton13.Checked = false;
                    toolStripButton14.Checked = false;
                    toolStripButton15.Checked = false;
                    break;
                case 3:
                    toolStripButton3.Checked = false;
                    toolStripButton5.Checked = false;
                    toolStripButton6.Checked = false;
                    toolStripButton9.Checked = false;
                    toolStripButton10.Checked = false;
                    toolStripButton11.Checked = false;
                    toolStripButton12.Checked = false;
                    toolStripButton13.Checked = false;
                    toolStripButton14.Checked = false;
                    toolStripButton15.Checked = false;
                    break;
                case 4:
                    toolStripButton3.Checked = false;
                    toolStripButton5.Checked = false;
                    toolStripButton6.Checked = false;
                    toolStripButton8.Checked = false;
                    toolStripButton10.Checked = false;
                    toolStripButton11.Checked = false;
                    toolStripButton12.Checked = false;
                    toolStripButton13.Checked = false;
                    toolStripButton14.Checked = false;
                    toolStripButton15.Checked = false;
                    break;
                case 5:
                    toolStripButton3.Checked = false;
                    toolStripButton5.Checked = false;
                    toolStripButton6.Checked = false;
                    toolStripButton8.Checked = false;
                    toolStripButton9.Checked = false;
                    toolStripButton11.Checked = false;
                    toolStripButton12.Checked = false;
                    toolStripButton13.Checked = false;
                    toolStripButton14.Checked = false;
                    toolStripButton15.Checked = false;
                    pictureBox2.Invalidate();
                    break;
                case 6:
                    toolStripButton3.Checked = false;
                    toolStripButton5.Checked = false;
                    toolStripButton6.Checked = false;
                    toolStripButton8.Checked = false;
                    toolStripButton9.Checked = false;
                    toolStripButton10.Checked = false;
                    toolStripButton12.Checked = false;
                    toolStripButton13.Checked = false;
                    toolStripButton14.Checked = false;
                    toolStripButton15.Checked = false;
                    break;
                case 7:
                    toolStripButton3.Checked = false;
                    toolStripButton5.Checked = false;
                    toolStripButton6.Checked = false;
                    toolStripButton8.Checked = false;
                    toolStripButton9.Checked = false;
                    toolStripButton10.Checked = false;
                    toolStripButton11.Checked = false;
                    toolStripButton13.Checked = false;
                    toolStripButton14.Checked = false;
                    toolStripButton15.Checked = false;
                    break;
                case 8:
                    toolStripButton3.Checked = false;
                    toolStripButton5.Checked = false;
                    toolStripButton6.Checked = false;
                    toolStripButton8.Checked = false;
                    toolStripButton9.Checked = false;
                    toolStripButton10.Checked = false;
                    toolStripButton11.Checked = false;
                    toolStripButton12.Checked = false;
                    toolStripButton14.Checked = false;
                    toolStripButton15.Checked = false;
                    break;
                case 9:
                    toolStripButton3.Checked = false;
                    toolStripButton5.Checked = false;
                    toolStripButton6.Checked = false;
                    toolStripButton8.Checked = false;
                    toolStripButton9.Checked = false;
                    toolStripButton10.Checked = false;
                    toolStripButton11.Checked = false;
                    toolStripButton12.Checked = false;
                    toolStripButton13.Checked = false;
                    toolStripButton15.Checked = false;
                    break;
                case 10:
                    toolStripButton3.Checked = false;
                    toolStripButton5.Checked = false;
                    toolStripButton6.Checked = false;
                    toolStripButton8.Checked = false;
                    toolStripButton9.Checked = false;
                    toolStripButton10.Checked = false;
                    toolStripButton11.Checked = false;
                    toolStripButton12.Checked = false;
                    toolStripButton13.Checked = false;
                    toolStripButton14.Checked = false;
                    break;
                default:
                    break;
            }
        }
        #endregion formsstuff
        #region Types
        public void SetSummarytiers()
        {
            List<CheckBox> checkboxesSummary = SetdefinitionsTPSummary.Controls.OfType<CheckBox>().ToList();
            foreach (CheckBox cb in checkboxesSummary)
            {
                cb.Visible = false;
            }
            int index = 15;
            foreach (listsValue value in currentproject.limitfefinitions.lists.valueflags)
            {
                CheckBox cb = checkboxesSummary.First(x => x.Tag.ToString() == value.name);
                cb.Tag = value.name;
                cb.Checked = false;
                cb.Visible = true;
                cb.Text = value.name;
                index--;
            }
            checkboxesSummary = UserdefinitionsTPSummary.Controls.OfType<CheckBox>().ToList();
            checkboxesSummary = checkboxesSummary.OrderBy(x => x.Tag.ToString()).ToList();
            foreach (CheckBox cb in checkboxesSummary)
            {
                cb.Visible = false;
            }
            index = 0;
            foreach (user_listsUser1 user in currentproject.limitfefinitionsuser.user_lists.valueflags)
            {
                CheckBox cb = checkboxesSummary[index];
                cb.Tag = user.name;
                cb.Visible = true;
                cb.Checked = false;
                cb.Text = user.name;
                index++;
            }
        }
        public void setNumberofTiers()
        {
            List<CheckBox> checkboxes = SetdefinitionsTP.Controls.OfType<CheckBox>().ToList();
            
            foreach (CheckBox cb in checkboxes)
            {
                cb.Visible = false;
            }

            int index = currentproject.limitfefinitions.lists.valueflags.Count;
            foreach (listsValue value in currentproject.limitfefinitions.lists.valueflags)
            {
                CheckBox cb = checkboxes.First(x => x.Tag.ToString() == value.name);
                cb.Tag = value.name;
                cb.Checked = false;
                cb.Visible = true;
                cb.Text = value.name;
                index--;
                
            }

            checkboxes = UserdefinitionsTP.Controls.OfType<CheckBox>().ToList();
            checkboxes = checkboxes.OrderBy(x => x.Name).ToList();
            foreach (CheckBox cb in checkboxes)
            {
                cb.Visible = false;
            }


            index = 0;
            foreach (user_listsUser1 user in currentproject.limitfefinitionsuser.user_lists.valueflags)
            {
                CheckBox cb = checkboxes[index];
                cb.Tag = user.name;
                cb.Visible = true;
                cb.Checked = false;
                cb.Text = user.name;
                index++;
            }

            //if (currentlootpart.Usinguserdifinitions)
            //{
            //    tabControl3.SelectedIndex = 1;
            //}
            //else
            //    tabControl3.SelectedIndex = 0;
        }
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            Loot_Info f2 = new Loot_Info();
            f2.ShowDialog();
        }
        private void PopulateTreeView()
        {
            treeViewMS1.Nodes.Clear();
            TreeNode root = new TreeNode(Path.GetFileName(filename))
            {
                Tag = "Parent"
            };
            //Set Vanilla Treenode types
            TreeNode vanilla = new TreeNode("Vanilla Types")
            {
                Tag = "VanillaTypes"
            };
            foreach (typesType type in vanillatypes.types.type)
            {
                string cat = "other";
                if (type.category != null)
                    cat = type.category.name;
                TreeNode typenode = new TreeNode(type.name)
                {
                    Tag = type
                };
                if (!vanilla.Nodes.ContainsKey(cat))
                {
                    TreeNode newcatnode = new TreeNode(cat)
                    {
                        Name = cat,
                        Tag = cat
                    };
                    vanilla.Nodes.Add(newcatnode);
                }
                vanilla.Nodes[cat].Nodes.Add(typenode);
            }
            root.Nodes.Add(vanilla);
            if (ModTypes.Count > 0)
            {
                foreach (TypesFile tf in ModTypes)
                {
                    TreeNode ModTypes = new TreeNode(tf.modname)
                    {
                        Tag = tf.modname
                    };
                    foreach (typesType type in tf.types.type)
                    {
                        string cat = "other";
                        if (type.category != null)
                            cat = type.category.name;
                        TreeNode typenode = new TreeNode(type.name)
                        {
                            Tag = type
                        };
                        if (!ModTypes.Nodes.ContainsKey(cat))
                        {
                            TreeNode newcatnode = new TreeNode(cat)
                            {
                                Name = cat,
                                Tag = cat
                            };
                            ModTypes.Nodes.Add(newcatnode);
                        }
                        ModTypes.Nodes[cat].Nodes.Add(typenode);
                    }


                    root.Nodes.Add(ModTypes);
                }
            }

            //treeView1.Nodes.Add(root);
            treeViewMS1.Nodes.Add(root);
        }
        private void treeViewMS1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag != null && e.Node.Tag is typesType)
            {
                TreeNode parent = e.Node.Parent;
                TreeNode mainparent = parent.Parent;
                currentcollection = parent.Text;
                String typesfile = mainparent.Text;
                if (typesfile == "Vanilla Types")
                    currentTypesFile = vanillatypes;
                else
                    currentTypesFile = ModTypes.FirstOrDefault(x => x.modname == typesfile);
                isUserInteraction = false;
                tabControl1.SelectedIndex = 1;
                currentlootpart = e.Node.Tag as typesType;
                PopulateLootPartInfo();
                isUserInteraction = true;
                if (e.Button == MouseButtons.Right)
                {
                    // Display context menu for eg:
                    DeleteTypesTSMI.Visible = false;
                    AddTypesTSMI.Visible = false;
                    DeleteSpecificTypeTSMI.Visible = true;
                    DeleteSpecificTypeTSMI.Text = "Delete " + currentlootpart.name + " from " + currentcollection + " in " + typesfile;
                    checkForDuplicateTypesTSMI.Visible = false;
                    TypesContextMenu.Show(Cursor.Position);
                }
            }
            else if (e.Node.Tag != null && e.Node.Tag is string)
            {
                tabControl1.SelectedIndex = 0;
                currentcollection = e.Node.Tag.ToString();
                darkLabel2.Text = currentcollection;
                TreeNode parent = e.Node.Parent;

                if (parent != null && parent.Tag.ToString() == "Parent")
                {
                    FullTypes = true;
                    if (currentcollection == "VanillaTypes")
                    {
                        currentTypesFile = vanillatypes;
                        if (e.Button == MouseButtons.Right)
                        {
                            // Display context menu for eg:
                            DeleteSpecificTypeTSMI.Visible = false;
                            DeleteTypesTSMI.Visible = false;
                            AddTypesTSMI.Visible = true;
                            AddTypesTSMI.Text = "Add new Types to Vanilla Types";
                            checkForDuplicateTypesTSMI.Visible = false;
                            TypesContextMenu.Show(Cursor.Position);
                            
                        }
                    }
                    else
                    {
                        currentTypesFile = ModTypes.FirstOrDefault(x => x.modname == currentcollection);
                        if (e.Button == MouseButtons.Right)
                        {
                            DeleteSpecificTypeTSMI.Visible = false;
                            // Display context menu for eg:
                            if (currentTypesFile.modname != "expansion_types")
                                DeleteTypesTSMI.Visible = true;
                            else
                                DeleteTypesTSMI.Visible = false;
                            DeleteTypesTSMI.Text = "Delete " + currentTypesFile.modname;
                            AddTypesTSMI.Visible = true;
                            AddTypesTSMI.Text = "Add new Types to " + currentTypesFile.modname;
                            checkForDuplicateTypesTSMI.Visible = false;
                            TypesContextMenu.Show(Cursor.Position);
                        }
                    }
                }
                else if (parent != null)
                {
                    FullTypes = false;
                    if (parent.Text == "Vanilla Types")
                        currentTypesFile = vanillatypes;
                    else
                    {
                        currentTypesFile = ModTypes.FirstOrDefault(x => x.modname == parent.Text);
                    }
                    if (e.Button == MouseButtons.Right)
                    {
                        DeleteSpecificTypeTSMI.Visible = false;
                        DeleteTypesTSMI.Visible = true;
                        AddTypesTSMI.Visible = false;
                        checkForDuplicateTypesTSMI.Visible = false;
                        DeleteTypesTSMI.Text = "Delete " + currentcollection + " from " + currentTypesFile.modname;
                        TypesContextMenu.Show(Cursor.Position);
                    }
                }
                else if (currentcollection == "Parent")
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        DeleteSpecificTypeTSMI.Visible = false;
                        AddTypesTSMI.Visible = true;
                        DeleteTypesTSMI.Visible = false;
                        checkForDuplicateTypesTSMI.Visible = true;
                        AddTypesTSMI.Text = "Add new Types to Custom Folder";
                        TypesContextMenu.Show(Cursor.Position);
                    }
                }
            }
            this.treeViewMS1.SelectedNode = e.Node;
        }
        private void checkForDuplicateTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Tuple<string, string>> typecheck = new List<Tuple<string, string>>();
            List<Tuple<string, string, string>> duplicatelist = new List<Tuple<string, string, string>>();
            foreach (typesType type in vanillatypes.types.type)
            {
                if(typecheck.Any(x => x.Item1 == type.name))
                {

                    Tuple<string, string> first = typecheck.FirstOrDefault(x => x.Item1 == type.name);
                    duplicatelist.Add(new Tuple<string, string, string>(type.name, first.Item2, Path.GetFileNameWithoutExtension(vanillatypes.Filename)));
                }
                else
                {
                    typecheck.Add(new Tuple<string, string>(type.name, Path.GetFileNameWithoutExtension(vanillatypes.Filename)));
                }
            }
            if (ModTypes.Count > 0)
            {
                foreach (TypesFile tf in ModTypes)
                {
                    foreach (typesType type in tf.types.type)
                    {
                        if (typecheck.Any(x => x.Item1 == type.name))
                        {
                            Tuple<string, string> first = typecheck.FirstOrDefault(x => x.Item1 == type.name);
                            duplicatelist.Add(new Tuple<string, string, string>(type.name, first.Item2, Path.GetFileNameWithoutExtension(tf.Filename)));
                        }
                        else
                        {
                            typecheck.Add(new Tuple<string, string>(type.name, Path.GetFileNameWithoutExtension(tf.Filename)));
                        }
                    }
                }
            }
            if(duplicatelist.Count > 0)
            {
                Console.WriteLine("");
                Console.WriteLine("Duplicate types found");
                foreach (Tuple<string,string,string> tup in duplicatelist)
                {
                    Console.WriteLine(tup.Item1 + " is in both " + tup.Item2 + " and " + tup.Item3);
                }
                MessageBox.Show("Duplicates found, Please see console");
            }
        }
        private void DeleteSpecificTypeTSMI_Click(object sender, EventArgs e)
        {
            currentTypesFile.types.type.Remove(currentlootpart);
            currentTypesFile.SaveTyes(DateTime.Now.ToString("ddMMyy_HHmm"));
            var savedExpansionState = treeViewMS1.Nodes.GetExpansionState();
            treeViewMS1.BeginUpdate();
            PopulateTreeView();
            treeViewMS1.Nodes.SetExpansionState(savedExpansionState);
            treeViewMS1.EndUpdate();
            populateEconmyTreeview();
            currentproject.SetTotNomCount();
            NomCountLabel.Text = "Total Nominal Count :- " + currentproject.TotalNomCount.ToString();
        }
        private void DeleteTypesTSMI_Click(object sender, EventArgs e)
        {
            if (currentcollection == currentTypesFile.modname)
            {
                if (MessageBox.Show("This Will Remove The types file from the Project, Are you sure you want to do this?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    string Modname = currentTypesFile.modname;
                    currentproject.EconomyCore.RemoveCe(Modname, out string foflderpath, out string filename, out bool deletedirectory);
                    if (currentproject.EconomyCore.checkiftodelete(Modname))
                    {
                        File.Delete(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\" + foflderpath + "\\" + filename);
                        if (deletedirectory)
                            Directory.Delete(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\" + foflderpath, true);
                    }
                    currentproject.EconomyCore.SaveEconomycore();
                    currentproject.removeMod(currentTypesFile.modname);
                    ModTypes = currentproject.getModList();

                    var savedExpansionState = treeViewMS1.Nodes.GetExpansionState();
                    treeViewMS1.BeginUpdate();
                    PopulateTreeView();
                    treeViewMS1.Nodes.SetExpansionState(savedExpansionState);
                    treeViewMS1.EndUpdate();
                    MessageBox.Show("Mod Removed from Project....", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            else
            {
                List<typesType> typetoremove = new List<typesType>();
                foreach (typesType type in currentTypesFile.types.type)
                {
                    if(type.category == null && currentcollection == "other")
                    {
                        typetoremove.Add(type);
                    }
                    else if(type.category != null && type.category.name == currentcollection)
                    {
                        typetoremove.Add(type);
                    }
                }
                foreach(typesType t in typetoremove)
                {
                    currentTypesFile.types.type.Remove(t);
                }
                currentTypesFile.SaveTyes(DateTime.Now.ToString("ddMMyy_HHmm"));
                var savedExpansionState = treeViewMS1.Nodes.GetExpansionState();
                treeViewMS1.BeginUpdate();
                PopulateTreeView();
                treeViewMS1.Nodes.SetExpansionState(savedExpansionState);
                treeViewMS1.EndUpdate();
            }
            populateEconmyTreeview();
            currentproject.SetTotNomCount();
            NomCountLabel.Text = "Total Nominal Count :- " + currentproject.TotalNomCount.ToString();
        }
        private static string ReplaceCaseInsensitive(string input, string search, string replacement)
        {
            string result = Regex.Replace(
                input,
                Regex.Escape(search),
                replacement.Replace("$", "$$"),
                RegexOptions.IgnoreCase
            );
            return result;
        }
        private void AddtypesTSMI_Click(object sender, EventArgs e)
        {
            AddNewTypes form = new AddNewTypes
            {
                currentproject = currentproject
            };
            if (currentcollection == "Parent")
            {
                form.newlocation = true;   
            }
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (currentcollection == "Parent")
                {
                    string path = form.CustomLocation;
                    string modname = form.TypesName;
                    Directory.CreateDirectory(path);
                    string line1 = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>";
                    string line2 = "<types>";
                    List<string> modtypes = form.modtypes;
                    string last = "</types>";
                    modtypes.Insert(0, line2);
                    modtypes.Insert(0, line1);
                    modtypes.Add(last);
                    File.WriteAllLines(path + "\\" + modname + "_types.xml", modtypes);
                    TypesFile test = new TypesFile(path + "\\" + modname + "_types.xml");
                    test.SaveTyes();
                    string pathreplacer = currentproject.projectFullName + @"\mpmissions\" + currentproject.mpmissionpath + @"\";
                    string pathrep = ReplaceCaseInsensitive(path, pathreplacer, "");
                    currentproject.EconomyCore.AddCe(pathrep, modname + "_types.xml", "types");
                    currentproject.EconomyCore.SaveEconomycore();
                    currentproject.SetModListtypes();
                    ModTypes = currentproject.getModList();
                    var savedExpansionState = treeViewMS1.Nodes.GetExpansionState();
                    treeViewMS1.BeginUpdate();
                    PopulateTreeView();
                    treeViewMS1.Nodes.SetExpansionState(savedExpansionState);
                    treeViewMS1.EndUpdate();
                }
                else
                {
                    currentTypesFile = ModTypes.FirstOrDefault(x => x.modname == currentcollection);
                    string line1 = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>";
                    string line2 = "<types>";
                    List<string> modtypes = form.modtypes;
                    string last = "</types>";
                    modtypes.Insert(0, line2);
                    modtypes.Insert(0, line1);
                    modtypes.Add(last);
                    File.WriteAllLines("Temp_types.xml", modtypes);
                    TypesFile test = new TypesFile("Temp_types.xml");
                    File.Delete("Temp_types.xml");
                    foreach (typesType type in test.types.type)
                    {
                        currentTypesFile.types.type.Add(type);
                    }
                    currentTypesFile.SaveTyes(DateTime.Now.ToString("ddMMyy_HHmm"));
                    var savedExpansionState = treeViewMS1.Nodes.GetExpansionState();
                    treeViewMS1.BeginUpdate();
                    PopulateTreeView();
                    treeViewMS1.Nodes.SetExpansionState(savedExpansionState);
                    treeViewMS1.EndUpdate();
                }
            }
            populateEconmyTreeview();
            currentproject.SetTotNomCount();
            NomCountLabel.Text = "Total Nominal Count :- " + currentproject.TotalNomCount.ToString();
        }
        private void PopulateLootPartInfo()
        {
            isUserInteraction = false;
            textBox1.Text = currentlootpart.name;
            if (currentlootpart.category == null)
                comboBox1.SelectedIndex = -1;
            else
                comboBox1.SelectedIndex = comboBox1.FindStringExact(currentlootpart.category.name);

            populateUsage();
            PopulateCounts();
            PopulateFlags();
            PopulateTiers();
            PopulateTags();
            isUserInteraction = true;
        }
        private void PopulateTiers()
        {
            setNumberofTiers();
            if (currentlootpart.value != null)
            {
                for(int i = 0; i < currentlootpart.value.Count; i++)
                {
                    if (currentlootpart.value[i].user != null && currentlootpart.value[i].user.Count() > 0 && currentlootpart.value[i].name == null)
                    {
                        tabControl3.SelectedIndex = 1;
                        try
                        {
                            UserdefinitionsTP.Controls.OfType<CheckBox>().First(x => x.Tag.ToString() == currentlootpart.value[i].user).Checked = true;
                        }
                        catch
                        {
                            currentlootpart.value.RemoveAt(i);
                            i--;
                        }
                    }
                    else
                    {
                        tabControl3.SelectedIndex = 0;

                        try
                        {
                            SetdefinitionsTP.Controls.OfType<CheckBox>().First(x => x.Tag.ToString() == currentlootpart.value[i].name).Checked = true;
                        }
                        catch
                        {
                            currentlootpart.value.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
        }
        private void PopulateCounts()
        {
            if (currentlootpart.nominalSpecified)
            {
                typeNomCountNUD.Visible = false;
            }
            else
            {
                typeNomCountNUD.Visible = true;
                typeNomCountNUD.Value = (decimal)currentlootpart.nominal;
            }
            if (currentlootpart.minSpecified)
            {
                typeMinCountNUD.Visible = false;
            }
            else
            {
                typeMinCountNUD.Visible = true;
                typeMinCountNUD.Value = (decimal)currentlootpart.min;
            }
            typeLifetimeNUD.Visible = true;
            typeLifetimeNUD.Value = (decimal)currentlootpart.lifetime;

            if (currentlootpart.restockSpecified)
            {
                typeRestockNUD.Visible = false;
            }
            else
            {
                typeRestockNUD.Visible = true;
                typeRestockNUD.Value = (decimal)currentlootpart.restock;
            }
            if (currentlootpart.quantminSpecified)
            {
                typeQuantMINNUD.Visible = false;
            }
            else
            {
                typeQuantMINNUD.Visible = true;
                typeQuantMINNUD.Value = (decimal)currentlootpart.quantmin;
            }
            if (currentlootpart.quantmaxSpecified)
            {
                typeQuantMAXNUD.Visible = false;
            }
            else
            {
                typeQuantMAXNUD.Visible = true;
                typeQuantMAXNUD.Value = (decimal)currentlootpart.quantmax;
            }
            if (currentlootpart.costSpecified)
            {
                typeCostNUD.Visible = false;
            }
            else
            {
                typeCostNUD.Visible = true;
                typeCostNUD.Value = (decimal)currentlootpart.cost;
            }
        }
        private void populateUsage()
        {
                listBox1.DisplayMember = "Name";
                listBox1.ValueMember = "Value";
                listBox1.DataSource = currentlootpart.usage;
        }
        private void PopulateFlags()
        {
            if (currentlootpart.flags != null)
            {
                checkBox1.Checked = currentlootpart.flags.count_in_cargo == 1 ? true : false;
                checkBox2.Checked = currentlootpart.flags.count_in_hoarder == 1 ? true : false;
                checkBox3.Checked = currentlootpart.flags.count_in_map == 1 ? true : false;
                checkBox4.Checked = currentlootpart.flags.count_in_player == 1 ? true : false;
                checkBox5.Checked = currentlootpart.flags.crafted == 1 ? true : false;
                checkBox6.Checked = currentlootpart.flags.deloot == 1 ? true : false;
            }
            else
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;
            }
        }
        private void PopulateTags()
        {
            listBox2.DisplayMember = "Name";
            listBox2.ValueMember = "Value";
            listBox2.DataSource = currentlootpart.tag;
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
            typesTypeUsage u = comboBox2.SelectedItem as typesTypeUsage;
            foreach (TreeNode tn in treeViewMS1.SelectedNodes)
            {
                typesType looptype = tn.Tag as typesType;
                looptype.AddnewUsage(u);
                populateUsage();
                currentTypesFile.isDirty = true;
            }
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            typesTypeUsage u = listBox1.SelectedItem as typesTypeUsage;
            foreach (TreeNode tn in treeViewMS1.SelectedNodes)
            {
                typesType looptype = tn.Tag as typesType;
                looptype.removeusage(u);
                currentTypesFile.isDirty = true;
            }
        }
        private void darkButton8_Click(object sender, EventArgs e)
        {
            typesTypeTag t = comboBox4.SelectedItem as typesTypeTag;
            foreach (TreeNode tn in treeViewMS1.SelectedNodes)
            {
                typesType looptype = tn.Tag as typesType;
                looptype.Addnewtag(t);
                currentTypesFile.isDirty = true;
                PopulateTags();
            }
        }
        private void darkButton7_Click(object sender, EventArgs e)
        {
            typesTypeTag t = listBox2.SelectedItem as typesTypeTag;
            foreach (TreeNode tn in treeViewMS1.SelectedNodes)
            {
                typesType looptype = tn.Tag as typesType;
                looptype.removetag(t);
                currentTypesFile.isDirty = true;
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                typesTypeCategory c = comboBox1.SelectedItem as typesTypeCategory;
                foreach (TreeNode tn in treeViewMS1.SelectedNodes)
                {
                    typesType looptype = tn.Tag as typesType;
                    looptype.changecategory(c);
                }
                currentTypesFile.isDirty = true;
                PopulateTreeView();
                isUserInteraction = false;
            }
        }
        private void typeNomCountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                foreach (TreeNode tn in treeViewMS1.SelectedNodes)
                {
                    typesType looptype = tn.Tag as typesType;
                    if (looptype.nominalSpecified)
                    {
                        looptype.nominal = (int)typeNomCountNUD.Value;
                        currentTypesFile.isDirty = true;
                        currentproject.SetTotNomCount();
                        //PopulateLootPartInfo();
                        NomCountLabel.Text = "Total Nominal Count :- " + currentproject.TotalNomCount.ToString();
                    }
                }
            }
        }
        private void typeMinCountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                foreach (TreeNode tn in treeViewMS1.SelectedNodes)
                {
                    typesType looptype = tn.Tag as typesType;
                    if (looptype.minSpecified)
                    {
                        looptype.min = (int)typeMinCountNUD.Value;
                        currentTypesFile.isDirty = true;
                    }
                }
            }
        }
        private void typeLifetimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                foreach (TreeNode tn in treeViewMS1.SelectedNodes)
                {
                    typesType looptype = tn.Tag as typesType;
                    looptype.lifetime = (int)typeLifetimeNUD.Value;
                    currentTypesFile.isDirty = true;
                 }
            }
        }
        private void typeRestockNUD_ValueChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                foreach (TreeNode tn in treeViewMS1.SelectedNodes)
                {
                    typesType looptype = tn.Tag as typesType;
                    if (looptype.restockSpecified)
                    { 
                        looptype.restock = (int)typeRestockNUD.Value;
                        currentTypesFile.isDirty = true;
                    }
                }
            }

        }
        private void typeQuantMINNUD_ValueChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                foreach (TreeNode tn in treeViewMS1.SelectedNodes)
                {
                    typesType looptype = tn.Tag as typesType;
                    if (looptype.quantminSpecified)
                    { 
                        looptype.quantmin = (int)typeQuantMINNUD.Value;
                        currentTypesFile.isDirty = true;
                    }
                }
            }
        }
        private void typeQuantMAXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                foreach (TreeNode tn in treeViewMS1.SelectedNodes)
                {
                    typesType looptype = tn.Tag as typesType;
                    if (looptype.quantmaxSpecified)
                    { 
                        looptype.quantmax = (int)typeQuantMAXNUD.Value;
                        currentTypesFile.isDirty = true;
                    }
                }
            }
        }
        private void typeCostNUD_ValueChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                foreach (TreeNode tn in treeViewMS1.SelectedNodes)
                {
                    typesType looptype = tn.Tag as typesType;
                    if (looptype.costSpecified)
                    { 
                        looptype.cost = (int)typeCostNUD.Value; ;
                        currentTypesFile.isDirty = true;
                    }
                }
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                foreach (TreeNode tn in treeViewMS1.SelectedNodes)
                {
                    typesType looptype = tn.Tag as typesType;
                    CheckBox cb = sender as CheckBox;
                    switch (cb.Name)
                    {
                        case "checkBox1":
                            looptype.flags.count_in_cargo = checkBox1.Checked == true ? 1 : 0;
                            break;
                        case "checkBox2":
                            looptype.flags.count_in_hoarder = checkBox2.Checked == true ? 1 : 0;
                            break;
                        case "checkBox3":
                            looptype.flags.count_in_map = checkBox3.Checked == true ? 1 : 0;
                            break;
                        case "checkBox4":
                            looptype.flags.count_in_player = checkBox4.Checked == true ? 1 : 0;
                            break;
                        case "checkBox5":
                            looptype.flags.crafted = checkBox5.Checked == true ? 1 : 0;
                            break;
                        case "checkBox6":
                            looptype.flags.deloot = checkBox6.Checked == true ? 1 : 0;
                            break;
                    }
                    currentTypesFile.isDirty = true;
                }
            }
        }
        private void TierCheckBoxchanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                CheckBox cb = sender as CheckBox;
                string tier = cb.Tag.ToString();
                foreach (TreeNode tn in treeViewMS1.SelectedNodes)
                {
                    typesType looptype = tn.Tag as typesType;
                    if (cb.Checked)
                        looptype.AddTier(tier);
                    else
                        looptype.removetier(tier);
                    currentTypesFile.isDirty = true;
                }
                PopulateLootPartInfo();
            }
        }
        private void UserdefiniedTiersChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                CheckBox cb = sender as CheckBox;
                string tier = cb.Tag.ToString();
                foreach (TreeNode tn in treeViewMS1.SelectedNodes)
                {
                    typesType looptype = tn.Tag as typesType;
                    if (cb.Checked)
                    {
                        if (looptype.value != null)
                        {
                            looptype.removetiers();
                        }
                        looptype.AdduserTier(tier);
                    }
                    else
                        looptype.removeusertier(tier);
                }
                currentTypesFile.isDirty = true;
                PopulateLootPartInfo();
            }
        }
        private void darkButton28_Click(object sender, EventArgs e)
        {
            foreach (TreeNode tn in treeViewMS1.SelectedNodes)
            {
                typesType looptype = tn.Tag as typesType;
                looptype.removetiers();
                currentTypesFile.isDirty = true;
            }
            PopulateLootPartInfo();
        }
        private void darkButton3_Click(object sender, EventArgs e)
        {
            List<typesType> loot = new List<typesType>();

            if(currentcollection == "Parent")
            {
                if(vanillatypes.types.type != null)
                {
                    loot = vanillatypes.types.type.ToList();
                    editnommax(loot);
                }
                foreach(TypesFile tf in  ModTypes)
                {
                    if (tf.types.type != null)
                    {
                        loot = tf.types.type.ToList();
                        editnommax(loot);
                    }
                }
            }
            else if (FullTypes)
            {
                loot = currentTypesFile.types.type.ToList();
                editnommax(loot);
            }
            else
            {

                if (currentcollection == "other")
                {
                    loot = currentTypesFile.types.type.Where(x => x.category == null).ToList();
                    editnommax(loot);
                }
                else
                {
                    loot = currentTypesFile.types.type.Where(x => x.category != null && x.category.name == currentcollection).ToList();
                    editnommax(loot);
                }
            }
            currentproject.SetTotNomCount();
            NomCountLabel.Text = "Total Nominal Count :- " + currentproject.TotalNomCount.ToString();
        }
        public void editnommax(List<typesType> loot)
        {
            currentTypesFile.isDirty = true;
            Domultiplier(loot);
        }
        private void Domultiplier(List<typesType> list)
        {
            foreach (typesType item in list)
            {
                if(item.nominal == null) { continue; }
                if (item.nominal != 0)
                {
                    item.nominal = getmultiplier((int)item.nominal);
                }
                if (ChangeMinCheckBox.Checked)
                {
                    if (item.min != 0)
                    {
                        item.min = getmultiplier((int)item.min);
                    }
                }
            }
        }
        private int getmultiplier(int nominal)
        {
            switch (comboBox3.SelectedIndex)
            {
                case 0:
                    return nominal * 10;
                case 1:
                    return nominal * 9;
                case 2:
                    return nominal * 8;
                case 3:
                    return nominal * 7;
                case 4:
                    return nominal * 6;
                case 5:
                    return nominal * 5;
                case 6:
                    return nominal * 4;
                case 7:
                    return nominal * 3;
                case 8:
                    return nominal * 2;
                case 9:
                    return (int)((float)(nominal * 1.5));
                case 10:
                    return (int)((float)(nominal / 1.5));
                case 11:
                    return nominal / 2;
                case 12:
                    return nominal / 3;
                case 13:
                    return nominal / 4;
                case 14:
                    return nominal / 5;
                case 15:
                    return nominal / 6;
                case 16:
                    return nominal / 7;
                case 17:
                    return nominal / 8;
                case 18:
                    return nominal / 9;
                case 19:
                    return nominal / 10;
                default:
                    return 0;
            }
        }
        private void darkButton6_Click(object sender, EventArgs e)
        {
            Updatflag(sender);
        }
        private void Updatflag(object sender)
        {
            Button SB = (Button)sender;
            List<typesType> loot = new List<typesType>();
            if (FullTypes)
            {
                loot = currentTypesFile.types.type.ToList();
            }
            else
            {

                if (currentcollection == "other")
                {
                    loot = currentTypesFile.types.type.Where(x => x.category == null).ToList();
                }
                else
                {
                    loot = currentTypesFile.types.type.Where(x => x.category != null && x.category.name == currentcollection).ToList();
                }
            }
            currentTypesFile.isDirty = true;
            SetFlags(SB, loot);
        }
        private void SetFlags(Button SB, List<typesType> list)
        {
            switch (SB.Name)
            {
                case "CargoButton":
                    foreach (typesType item in list)
                    {
                        item.flags.count_in_cargo = checkBox17.Checked == true ? 1 : 0;
                    }
                    break;
                case "HoarderButton":
                    foreach (typesType item in list)
                    {
                        item.flags.count_in_hoarder = checkBox16.Checked == true ? 1 : 0;
                    }
                    break;
                case "MapButton":
                    foreach (typesType item in list)
                    {
                        item.flags.count_in_map = checkBox15.Checked == true ? 1 : 0;
                    }
                    break;
                case "PlayerButton":
                    foreach (typesType item in list)
                    {
                        item.flags.count_in_player = checkBox14.Checked == true ? 1 : 0;
                    }
                    break;
                case "CraftedButton":
                    foreach (typesType item in list)
                    {
                        item.flags.crafted = checkBox9.Checked == true ? 1 : 0;
                    }
                    break;
                case "DelootButton":
                    foreach (typesType item in list)
                    {
                        item.flags.deloot = checkBox8.Checked == true ? 1 : 0;
                    }
                    break;
                default:
                    break;
            }
        }
        private void darkButton5_Click(object sender, EventArgs e)
        {
            List<typesType> loot = new List<typesType>();
            if (FullTypes)
            {
                loot = currentTypesFile.types.type.ToList();
            }
            else
            {
                
                if (currentcollection == "other")
                {
                    loot = currentTypesFile.types.type.Where(x => x.category == null).ToList();
                }
                else
                {
                    loot = currentTypesFile.types.type.Where(x => x.category != null && x.category.name == currentcollection).ToList();
                }
            }
            currentTypesFile.isDirty = true;
            DoSyncMintoNom(loot);
        }
        private void DoSyncMintoNom(List<typesType> list)
        {
            foreach (typesType item in list)
            {
                item.min = item.nominal;
            }
        }
        private void darkButton6_Click_1(object sender, EventArgs e)
        {
            List<typesType> loot = new List<typesType>();
            if (FullTypes)
            {
                loot = currentTypesFile.types.type.ToList();
            }
            else
            {
                if (currentcollection == "other")
                {
                    loot = currentTypesFile.types.type.Where(x => x.category == null).ToList();
                }
                else
                {
                    loot = currentTypesFile.types.type.Where(x => x.category != null && x.category.name == currentcollection).ToList();
                }
            }
            currentTypesFile.isDirty = true;
            DoSyncNomtoMin(loot);
        }
        private void DoSyncNomtoMin(List<typesType> list)
        {
            foreach (typesType item in list)
            {
                item.nominal = item.min;
            }
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (treeViewMS1.Nodes.Count < 1)
                return;
            string text = toolStripTextBox1.Text;
            searchnum = 0;
            searchtreeNodes = new List<TreeNode>();
            foundparts = new List<typesType>();
            foreach (typesType type in vanillatypes.types.type)
            {
                if (type.name.ToLower().Contains(text.ToLower()))
                {
                    foundparts.Add(type);
                }
            }
            foreach (TypesFile tf in ModTypes)
            {
                foreach (typesType type in tf.types.type)
                {
                    if (type.name.ToLower().Contains(text.ToLower()))
                    {
                        foundparts.Add(type);
                    }
                }
            }
            if (foundparts.Count == 0)
            {
                MessageBox.Show("No Items found");
                return;
            }
            foreach (typesType obj in foundparts)
            {
                foreach (TreeNode node in treeViewMS1.Nodes)
                {
                    searchtree(obj.name.ToLower(), node, searchtreeNodes);
                }
            }
            treeViewMS1.SelectedNode = searchtreeNodes[searchnum];
            treeViewMS1.Focus();
            if (treeViewMS1.SelectedNode.Tag != null && treeViewMS1.SelectedNode.Tag is typesType)
            {
                isUserInteraction = false;
                tabControl1.SelectedIndex = 1;
                currentlootpart = treeViewMS1.SelectedNode.Tag as typesType;
                PopulateLootPartInfo();
                isUserInteraction = true;
            }
            if (searchtreeNodes.Count > 1)
                toolStripButton2.Visible = true;
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            searchnum++;
            if (searchnum == searchtreeNodes.Count)
            {
                MessageBox.Show("No More Items found");
                return;
            }
            treeViewMS1.SelectedNode = searchtreeNodes[searchnum];
            treeViewMS1.Focus();
            if (treeViewMS1.SelectedNode.Tag != null && treeViewMS1.SelectedNode.Tag is typesType)
            {
                isUserInteraction = false;
                tabControl1.SelectedIndex = 1;
                currentlootpart = treeViewMS1.SelectedNode.Tag as typesType;
                PopulateLootPartInfo();
                isUserInteraction = true;
            }
        }
        private void searchtree(string str, TreeNode tree, List<TreeNode> List)
        {
            if (tree.Text.ToString().ToLower().Contains(str))
                if (!List.Contains(tree))
                    List.Add(tree);
            foreach (TreeNode tn in tree.Nodes)
            {
                if (tn.Text.ToString().ToLower().Contains(str))
                    if (!List.Contains(tn))
                        List.Add(tn);
                if (tn.Nodes.Count > 0)
                {
                    foreach (TreeNode ctn in tn.Nodes)
                        searchtree(str, ctn, List);
                }
            }
        }
        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
            toolStripButton2.Visible = false;
        }
        #endregion types
        #region events
        public eventscofig currenteventsfile;
        private eventsEvent CurrentEvent;
        private eventsEventChild CurrentChild;
        private void Loadevents()
        {
            isUserInteraction = false;
            EventsLIstLB.DisplayMember = "DisplayName";
            EventsLIstLB.ValueMember = "Value";
            EventsLIstLB.DataSource = currentproject.ModEventsList;
            isUserInteraction = true;
            
        }
        private void EventsLIstLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EventsLIstLB.SelectedItem as eventscofig == currenteventsfile) { return; }
            if (EventsLIstLB.SelectedIndex == -1) { return; }
            isUserInteraction = false;
            currenteventsfile = EventsLIstLB.SelectedItem as eventscofig;
            positionComboBox.DataSource = Enum.GetValues(typeof(position));
            limitComboBox.DataSource = Enum.GetValues(typeof(limit));
            EventsLB.DisplayMember = "DisplayName";
            EventsLB.ValueMember = "Value";
            EventsLB.DataSource = currenteventsfile.events.@event;
            isUserInteraction = true;
        }
        private void EventsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EventsLB.SelectedItem as eventsEvent == CurrentEvent) { return; }
            if (EventsLB.SelectedIndex == -1) { return; }
            CurrentEvent = EventsLB.SelectedItem as eventsEvent;

            isUserInteraction = false;
            ClearChildren();
            positionComboBox.SelectedItem = (position)CurrentEvent.position; 
            limitComboBox.SelectedItem = (limit)CurrentEvent.limit;

            nameTB.Text = CurrentEvent.name;
            nominalNUD.Value = (int)CurrentEvent.nominal;
            minNUD.Value = (int)CurrentEvent.min;
            maxNUD.Value = (int)CurrentEvent.max;
            lifetimeNUD.Value = (int)CurrentEvent.lifetime;
            restockNUD.Value = (int)CurrentEvent.restock;
            saferadiusNUD.Value = (int)CurrentEvent.saferadius;
            distanceradiusNUD.Value = (int)CurrentEvent.distanceradius;
            cleanupradiusNUD.Value = (int)CurrentEvent.cleanupradius;
            deletableCB.Checked = CurrentEvent.flags.deletable == 1 ? true : false;
            init_randomCB.Checked = CurrentEvent.flags.init_random == 1 ? true : false;
            remove_damagedCB.Checked = CurrentEvent.flags.remove_damaged == 1 ? true : false;
            activeCB.Checked = CurrentEvent.active == 1 ? true : false;
            ChildrenLB.DisplayMember = "DisplayName";
            ChildrenLB.ValueMember = "Value";
            ChildrenLB.DataSource = CurrentEvent.children;

            isUserInteraction = true;
        }
        private void ClearChildren()
        {
            ChildrenLB.DataSource = null;
            ChildGB.Visible = false;
        }
        private void ChildrenLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ChildrenLB.SelectedItem as eventsEventChild == CurrentChild) { return; }
            if (ChildrenLB.SelectedIndex == -1) { return; }
            CurrentChild = ChildrenLB.SelectedItem as eventsEventChild;
            isUserInteraction = false;
            ChildGB.Visible = true;
            ClootmaxNUD.Value = (int)CurrentChild.lootmax;
            ClootminNUD.Value = (int)CurrentChild.lootmin;
            CmaxNUD.Value = (int)CurrentChild.max;
            CminNUD.Value = (int)CurrentChild.min;
            CtypeTB.Text = CurrentChild.type;
            isUserInteraction = true;
        }
        private void EventsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            NumericUpDown nud = sender as NumericUpDown;
            CurrentEvent.SetIntValue(nud.Name.Substring(0, nud.Name.Length - 3), (int)nud.Value);
            currenteventsfile.isDirty = true;
        }
        private void EventsChildNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            NumericUpDown nud = sender as NumericUpDown;
            CurrentChild.SetIntValue(nud.Name.Substring(1, nud.Name.Length - 4), (int)nud.Value);
            currenteventsfile.isDirty = true;
        }
        private void activeCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentEvent.active = activeCB.Checked == true ? 1 : 0;
            currenteventsfile.isDirty = true;
        }
        private void deletableCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentEvent.flags.deletable = deletableCB.Checked == true ? 1 : 0;
            currenteventsfile.isDirty = true;
        }
        private void init_randomCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentEvent.flags.init_random = init_randomCB.Checked == true ? 1 : 0;
            currenteventsfile.isDirty = true;
        }
        private void remove_damagedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentEvent.flags.remove_damaged = remove_damagedCB.Checked == true ? 1 : 0;
            currenteventsfile.isDirty = true;
        }
        private void positionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            position pos = (position)positionComboBox.SelectedItem;
            CurrentEvent.position = pos;
            currenteventsfile.isDirty = true;
        }
        private void limitComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            limit lim = (limit)limitComboBox.SelectedItem;
            CurrentEvent.limit = lim;
            currenteventsfile.isDirty = true;
        }
        private void nameTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentEvent.name = nameTB.Text;
            currenteventsfile.isDirty = true;
            EventsLB.Refresh();
        }
        private void CtypeTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentChild.type = CtypeTB.Text;
            currenteventsfile.isDirty = true;
            EventsLB.Refresh();
        }
        private void darkButton16_Click(object sender, EventArgs e)
        {
            eventsEvent neweventEvent = new eventsEvent()
            {
                name = "NewEvent",
                nominal = 0,
                min = 0,
                max = 0,
                lifetime = 0,
                restock = 0,
                saferadius = 0,
                distanceradius = 0,
                cleanupradius = 0,
                position = position.@fixed,
                limit = limit.child,
                active = 0,
                flags = new eventsEventFlags(),
                children = new BindingList<eventsEventChild>()
            };
            currenteventsfile.events.AddnewEvent(neweventEvent);
            currenteventsfile.isDirty = true;
        }
        private void darkButton17_Click(object sender, EventArgs e)
        {
            currenteventsfile.events.RemoveEvent(CurrentEvent);
            currenteventsfile.isDirty = true;
        }
        private void darkButton19_Click(object sender, EventArgs e)
        {
            eventsEventChild newventeventschild = new eventsEventChild()
            {
                type = "newChild",
                lootmax = 0,
                lootmin = 0,
                max = 0,
                min = 0
            };
            CurrentEvent.Addnechild(newventeventschild);
            currenteventsfile.isDirty = true;
        }
        private void darkButton18_Click(object sender, EventArgs e)
        {
            CurrentEvent.Removechild(CurrentChild);
            currenteventsfile.isDirty = true;
        }
        private void darkButton21_Click(object sender, EventArgs e)
        {
            AddNeweventFile form = new AddNeweventFile
            {
                currentproject = currentproject,
                newlocation = true
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                string path = form.CustomLocation;
                string modname = form.TypesName;
                Directory.CreateDirectory(path);
                List<string> Eventfile = new List<string>();
                Eventfile.Add("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
                Eventfile.Add("<events>");
                Eventfile.Add("</events>");
                File.WriteAllLines(path + "\\" + modname + "_events.xml", Eventfile);
                eventscofig test = new eventscofig(path + "\\" + modname + "_events.xml");
                test.SaveEvent();
                currentproject.EconomyCore.AddCe(path.Replace(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\", ""), modname + "_events.xml", "events");
                currentproject.EconomyCore.SaveEconomycore();
                currentproject.SetEvents();
                populateEconmyTreeview();
                Loadevents();
            }
        }
        private void darkButton20_Click(object sender, EventArgs e)
        {
            string Modname = Path.GetFileNameWithoutExtension(currenteventsfile.Filename);
            currentproject.EconomyCore.RemoveCe(Modname, out string foflderpath, out string filename, out bool deletedirectory);
            File.Delete(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\" + foflderpath + "\\" + filename);
            if (deletedirectory)
                Directory.Delete(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\" + foflderpath, true);
            currentproject.EconomyCore.SaveEconomycore();
            currentproject.removeevent(currenteventsfile.Filename);
            currentproject.SetEvents();
            Loadevents();
            populateEconmyTreeview();
        }
        #endregion events
        #region spawnabletypes
        public Spawnabletypesconfig currentspawnabletypesfile;
        public spawnabletypesType CurrentspawnabletypesType;
        public object CurrentspawnabletypesTypetype;
        private void LoadSpawnableTypes()
        {
            isUserInteraction = false;
            SpawnabletypeslistLB.DisplayMember = "DisplayName";
            SpawnabletypeslistLB.ValueMember = "Value";
            SpawnabletypeslistLB.DataSource = currentproject.spawnabletypesList;
            isUserInteraction = true;
        }
        private void SpawnabletypeslistLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SpawnabletypeslistLB.SelectedItem as Spawnabletypesconfig == currentspawnabletypesfile) { return; }
            if (SpawnabletypeslistLB.SelectedIndex == -1) { return; }
            currentspawnabletypesfile = SpawnabletypeslistLB.SelectedItem as Spawnabletypesconfig;
            isUserInteraction = false;
            SetDamange();

            SpawnabletpesLB.DisplayMember = "DisplayName";
            SpawnabletpesLB.ValueMember = "Value";
            SpawnabletpesLB.DataSource = currentspawnabletypesfile.spawnabletypes.type;
            isUserInteraction = true;
        }
        private void SetDamange()
        {
            if (currentspawnabletypesfile.spawnabletypes.damage != null)
            {
                label24.Visible = true;
                label25.Visible = true;
                HasDamageCB.Checked = true;
                DamageMinNUD.Value = currentspawnabletypesfile.spawnabletypes.damage.min;
                DamageMaxNUD.Value = currentspawnabletypesfile.spawnabletypes.damage.max;
                DamageMinNUD.Visible = true;
                DamageMaxNUD.Visible = true;
            }
            else
            {
                label25.Visible = false;
                label24.Visible = false;
                HasDamageCB.Checked = false;
                DamageMinNUD.Visible = false;
                DamageMaxNUD.Visible = false;
            }
        }
        private void SpawnabletpesLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearInfo();
            
            //if (SpawnabletpesLB.SelectedItem as spawnabletypesType == CurrentspawnabletypesType) { return; }
            if (SpawnabletpesLB.SelectedIndex == -1) { return; }
            CurrentspawnabletypesType = SpawnabletpesLB.SelectedItem as spawnabletypesType;
            isUserInteraction = false;
            listBox6.DisplayMember = "DisplayName";
            listBox6.ValueMember = "Value";
            listBox6.DataSource = CurrentspawnabletypesType.Items;
            listBox6.SelectedIndex = -1;
            if (listBox6.Items.Count > 0)
            {
                listBox6.SelectedIndex = 0;
            }
            isUserInteraction = true;
        }
        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (listBox6.SelectedItem as object == CurrentspawnabletypesTypetype) { return; }
            if (listBox6.SelectedIndex == -1) { return; }
            isUserInteraction = false;
            CurrentspawnabletypesTypetype = listBox6.SelectedItem as object;
            ClearInfo();
            if (CurrentspawnabletypesTypetype is spawnabletypesTypeHoarder)
            {
                
            }
            else if (CurrentspawnabletypesTypetype is spawnabletypesTypeTag)
            {
                spawnabletypesTypeTag currenttag = CurrentspawnabletypesTypetype as spawnabletypesTypeTag;
                Spaenabletypestagbox.Visible = true;
                textBox2.Text = currenttag.name;
            }
            else if (CurrentspawnabletypesTypetype is spawnabletypesTypeCargo)
            {
                CargoGB.Visible = true;
                spawnabletypesTypeCargo currentcargo = CurrentspawnabletypesTypetype as spawnabletypesTypeCargo;
                CargochanceCB.Checked = !currentcargo.chanceSpecified;
                SetCargo();
            }
            else if (CurrentspawnabletypesTypetype is spawnabletypesTypeAttachments)
            {

                AttachmentGB.Visible = true;
                spawnabletypesTypeAttachments currentAttchment = CurrentspawnabletypesTypetype as spawnabletypesTypeAttachments;
                AttchmentIsPresetCB.Checked = !currentAttchment.chanceSpecified;
                setattchments();
            }
            isUserInteraction = true;
        }
        private void CargochanceCB_CheckedChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                spawnabletypesTypeCargo currentcargo = CurrentspawnabletypesTypetype as spawnabletypesTypeCargo;
                currentcargo.chanceSpecified = !CargochanceCB.Checked;
                if (currentcargo.chanceSpecified)
                {
                    currentcargo.preset = null;
                    currentcargo.chance = 1;
                }
                else
                {
                    currentcargo.item = new BindingList<spawnabletypesTypeCargoItem>();
                    currentcargo.chance = 0;
                }
            }
            SetCargo();
        }
        private void AttchmentIsPresetCB_CheckedChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                spawnabletypesTypeAttachments currentAttchment = CurrentspawnabletypesTypetype as spawnabletypesTypeAttachments;
                currentAttchment.chanceSpecified = !AttchmentIsPresetCB.Checked;
                if (currentAttchment.chanceSpecified)
                {
                    currentAttchment.preset = null;
                    currentAttchment.chance = 1;
                    AttachemntTB.Text = "";
                    currentAttchment.item = new BindingList<spawnabletypesTypeAttachmentsItem>();
                }
                else
                {
                    currentAttchment.item = new BindingList<spawnabletypesTypeAttachmentsItem>();
                    currentAttchment.chance = 0;
                    AttachemntTB.Text = "";
                }
            }
            setattchments();
        }
        private void setattchments()
        {
            spawnabletypesTypeAttachments currentAttchment = CurrentspawnabletypesTypetype as spawnabletypesTypeAttachments;
            if (currentAttchment.chanceSpecified)
            {
                AttachmentItemLB.Visible = true;
                CargoAttachmentRemoveButton.Visible = true;
                cargoattachemntAddButton.Visible = true;
                chancAttachmentselabel.Visible = true;
                AttachmentChangeItemButton.Visible = true;
                AttachmentPresetGB.Visible = false;
                ItemAttachmentchanceNUD.Visible = true;
                AttachmemtItemChanceLabel.Visible = true;
                AttachmentchanceNUD.Visible = true;
                AttachmentchanceNUD.Value = currentAttchment.chance;
                AttachmentItemLB.DisplayMember = "DisplayName";
                AttachmentItemLB.ValueMember = "Value";
                AttachmentItemLB.DataSource = currentAttchment.item;
            }
            else
            {
                AttachmentItemLB.Visible = false;
                CargoAttachmentRemoveButton.Visible = false;
                cargoattachemntAddButton.Visible = false;
                chancAttachmentselabel.Visible = false;
                AttachmentchanceNUD.Visible = false;
                ItemAttachmentchanceNUD.Visible = false;
                AttachmemtItemChanceLabel.Visible = false;
                AttachmentChangeItemButton.Visible = false;
                AttachmentPresetGB.Visible = true;
                if (currentAttchment.preset != null)
                    AttachemntTB.Text = currentAttchment.preset;
                else
                    AttachemntTB.Text = "";
            }
        }
        private void SetCargo()
        {
            spawnabletypesTypeCargo currentcargo = CurrentspawnabletypesTypetype as spawnabletypesTypeCargo;
            if (currentcargo.chanceSpecified)
            {
                cargoItemRemoveButton.Visible = true;
                CargoItemAddButton.Visible = true;
                CargoPresetTB.Visible = false;
                CargoItemchanceNUD.Visible = true;
                CargoItemchancelabel.Visible = true;
                CargoItemLB.Visible = true;
                cargochanceLabel.Visible = true;
                CargoChangeItemButton.Visible = true;
                CargoPresetGB.Visible = false;
                CarcgoChanceNUD.Visible = true;
                CarcgoChanceNUD.Value = currentcargo.chance;
                CargoItemLB.DisplayMember = "DisplayName";
                CargoItemLB.ValueMember = "Value";
                CargoItemLB.DataSource = currentcargo.item;
            }
            else
            {
                cargoItemRemoveButton.Visible = false;
                CargoItemAddButton.Visible = false;
                CargoPresetTB.Visible = true;
                CargoItemchanceNUD.Visible = false;
                CargoItemchancelabel.Visible = false;
                CargoItemLB.Visible = false;
                CarcgoChanceNUD.Visible = false;
                cargochanceLabel.Visible = false;
                CargoChangeItemButton.Visible = false;
                CargoPresetGB.Visible = true;
                if(currentcargo.preset != null)
                    CargoPresetTB.Text = currentcargo.preset;
                else
                    CargoPresetTB.Text = "";
            }
        }
        private void darkButton36_Click(object sender, EventArgs e)
        {
            randompresetsCargo newcargopreset = CargoPresetComboBox.SelectedItem as randompresetsCargo;
            spawnabletypesTypeCargo currentcargo = CurrentspawnabletypesTypetype as spawnabletypesTypeCargo;
            currentcargo.preset = newcargopreset.name;
            SetCargo();
            currentspawnabletypesfile.isDirty = true;
        }
        private void darkButton37_Click(object sender, EventArgs e)
        {
            randompresetsAttachments newattachmentpreset = AttachmentPresetComboBox.SelectedItem as randompresetsAttachments;
            spawnabletypesTypeAttachments currentattchemnt = CurrentspawnabletypesTypetype as spawnabletypesTypeAttachments;
            currentattchemnt.preset = newattachmentpreset.name;
            setattchments();
            currentspawnabletypesfile.isDirty = true;
        }
        private void ClearInfo()
        {
             textBox2.Text = null;
            Spaenabletypestagbox.Visible = false;
            AttachmentGB.Visible = false;
            CargoGB.Visible = false;
        }
        private void darkButton34_Click(object sender, EventArgs e)
        {
            if (listBox6.SelectedItems == null) { return; }
            int index = listBox6.SelectedIndex;
            CurrentspawnabletypesType.Items.Remove(CurrentspawnabletypesTypetype);
            currentspawnabletypesfile.isDirty = true;
            listBox6.SelectedIndex = -1;
            if (index == 0 && listBox6.Items.Count > 0)
                listBox6.SelectedIndex = index;
            else
                listBox6.SelectedIndex = index - 1;
        }
        private void darkButton30_Click(object sender, EventArgs e)
        {
            if (SpawnabletpesLB.SelectedItem == null) { return; }
            spawnabletypesTypeHoarder newhoarder = new spawnabletypesTypeHoarder();
            if (CurrentspawnabletypesType.Items == null)
            {
                CurrentspawnabletypesType.Items = new BindingList<object>();
                CurrentspawnabletypesType.Items.Add(newhoarder);
            }
            else
                CurrentspawnabletypesType.Items.Insert(0, newhoarder);
            listBox6.SelectedIndex = -1;
            listBox6.SelectedIndex = 0;
            currentspawnabletypesfile.isDirty = true;

        }
        private void darkButton31_Click(object sender, EventArgs e)
        {
            if (SpawnabletpesLB.SelectedItem == null) { return; }
            spawnabletypesTypeTag newtag = new spawnabletypesTypeTag();
            if (CurrentspawnabletypesType.Items == null)
            {
                CurrentspawnabletypesType.Items = new BindingList<object>();
                CurrentspawnabletypesType.Items.Add(newtag);
            }
            else
                CurrentspawnabletypesType.Items.Insert(0, newtag);
            listBox6.SelectedIndex = -1;
            listBox6.SelectedIndex = 0;
            currentspawnabletypesfile.isDirty = true;
        }
        private void darkButton32_Click(object sender, EventArgs e)
        {
            if (SpawnabletpesLB.SelectedItem == null) { return; }
            spawnabletypesTypeCargo newcargo = new spawnabletypesTypeCargo()
            {
                item = new BindingList<spawnabletypesTypeCargoItem>()
            };
            if (CurrentspawnabletypesType.Items == null)
                CurrentspawnabletypesType.Items = new BindingList<object>();
            CurrentspawnabletypesType.Items.Add(newcargo);
            listBox6.SelectedIndex = -1;
            listBox6.SelectedIndex = 0;
            currentspawnabletypesfile.isDirty = true;
        }
        private void darkButton33_Click(object sender, EventArgs e)
        {
            if (SpawnabletpesLB.SelectedItem == null) { return; }
            spawnabletypesTypeAttachments newspawnabletypesTypeAttachments = new spawnabletypesTypeAttachments();
            if (CurrentspawnabletypesType.Items == null)
                CurrentspawnabletypesType.Items = new BindingList<object>();
            CurrentspawnabletypesType.Items.Add(newspawnabletypesTypeAttachments);
            listBox6.SelectedIndex = -1;
            listBox6.SelectedIndex = 0;
            currentspawnabletypesfile.isDirty = true;
        }
        private void darkButton29_Click(object sender, EventArgs e)
        {
            typesTypeTag t = comboBox5.SelectedItem as typesTypeTag;
            spawnabletypesTypeTag currenttag = CurrentspawnabletypesTypetype as spawnabletypesTypeTag;
            currenttag.name = t.name;
            textBox2.Text = currenttag.name;
            currentspawnabletypesfile.isDirty = true;
        }
        private void darkButton38_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseMultiple = false,
                isCategoryitem = true
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    spawnabletypesTypeAttachmentsItem currentcargoitem = AttachmentItemLB.SelectedItem as spawnabletypesTypeAttachmentsItem;
                    currentcargoitem.name = l;
                    AttachmentItemLB.Refresh();
                    currentspawnabletypesfile.isDirty = true;
                    setattchments();
                }

            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton35_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseMultiple = false,
                isCategoryitem = true
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    spawnabletypesTypeCargoItem currentcargoitem = CargoItemLB.SelectedItem as spawnabletypesTypeCargoItem;
                    currentcargoitem.name = l;
                    CargoItemLB.Refresh();
                    currentspawnabletypesfile.isDirty = true;
                    SetCargo();
                }

            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void AttachmentchanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            spawnabletypesTypeAttachments currentattachment = CurrentspawnabletypesTypetype as spawnabletypesTypeAttachments;
            currentattachment.chance = AttachmentchanceNUD.Value;
            currentspawnabletypesfile.isDirty = true;
        }
        private void CarcgoChanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            spawnabletypesTypeCargo currentcargo = CurrentspawnabletypesTypetype as spawnabletypesTypeCargo;
            currentcargo.chance = CarcgoChanceNUD.Value;
            currentspawnabletypesfile.isDirty = true;
        }
        private void listBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CargoItemLB.SelectedItems.Count == 0) return;
            spawnabletypesTypeCargoItem currentcargoitem = CargoItemLB.SelectedItem as spawnabletypesTypeCargoItem;
            isUserInteraction = false;
            CargoItemchanceNUD.Value = currentcargoitem.chance;
            isUserInteraction = true;
        }
        private void AttachmentItemLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AttachmentItemLB.SelectedItems.Count == 0) return;
            spawnabletypesTypeAttachmentsItem currentAttachmentitem = AttachmentItemLB.SelectedItem as spawnabletypesTypeAttachmentsItem;
            isUserInteraction = false;
            ItemAttachmentchanceNUD.Value = currentAttachmentitem.chance;
            isUserInteraction = true;
        }
        private void darkButton35_Click_1(object sender, EventArgs e)
        {
            spawnabletypesTypeCargoItem newitem = new spawnabletypesTypeCargoItem()
            {
                name = "New Item, Please change me...",
                chance = 1
            };
            spawnabletypesTypeCargo currentcargo = CurrentspawnabletypesTypetype as spawnabletypesTypeCargo;
            currentcargo.item.Add(newitem);
            currentspawnabletypesfile.isDirty = true;
        }
        private void darkButton39_Click(object sender, EventArgs e)
        {
            if (CargoItemLB.SelectedItems == null) { return; }
            int index = CargoItemLB.SelectedIndex;
            spawnabletypesTypeCargoItem currentcargoitem = CargoItemLB.SelectedItem as spawnabletypesTypeCargoItem;
            spawnabletypesTypeCargo currentcargo = CurrentspawnabletypesTypetype as spawnabletypesTypeCargo;
            currentcargo.item.Remove(currentcargoitem);
            currentspawnabletypesfile.isDirty = true;
            CargoItemLB.SelectedIndex = -1;
            if (index == 0 && CargoItemLB.Items.Count > 0)
                CargoItemLB.SelectedIndex = index;
            else
                CargoItemLB.SelectedIndex = index - 1;
        }
        private void CargoItemchanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            spawnabletypesTypeCargoItem currentcargoitem = CargoItemLB.SelectedItem as spawnabletypesTypeCargoItem;
            currentcargoitem.chance = CargoItemchanceNUD.Value;
            currentspawnabletypesfile.isDirty = true;
        }
        private void ItemAttachmentchanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            spawnabletypesTypeAttachmentsItem currentattachmentitem = AttachmentItemLB.SelectedItem as spawnabletypesTypeAttachmentsItem;
            currentattachmentitem.chance = ItemAttachmentchanceNUD.Value;
            currentspawnabletypesfile.isDirty = true;
        }
        private void darkButton38_Click_1(object sender, EventArgs e)
        {
            spawnabletypesTypeAttachmentsItem newitem = new spawnabletypesTypeAttachmentsItem()
            {
                name = "New Item, Please change me...",
                chance = 1
            };
            spawnabletypesTypeAttachments currentattachment = CurrentspawnabletypesTypetype as spawnabletypesTypeAttachments;
            currentattachment.item.Add(newitem);
            currentspawnabletypesfile.isDirty = true;
        }
        private void darkButton35_Click_2(object sender, EventArgs e)
        {
            if (AttachmentItemLB.SelectedItems == null) { return; }
            int index = AttachmentItemLB.SelectedIndex;
            spawnabletypesTypeAttachmentsItem currentAttachmentitem = AttachmentItemLB.SelectedItem as spawnabletypesTypeAttachmentsItem;
            spawnabletypesTypeAttachments currentAttachment = CurrentspawnabletypesTypetype as spawnabletypesTypeAttachments;
            currentAttachment.item.Remove(currentAttachmentitem);
            currentspawnabletypesfile.isDirty = true;
            AttachmentItemLB.SelectedIndex = -1;
            if (index == 0 && AttachmentItemLB.Items.Count > 0)
                AttachmentItemLB.SelectedIndex = index;
            else
                AttachmentItemLB.SelectedIndex = index - 1;
        }
        private void darkButton35_Click_3(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseMultiple = false,
                isCategoryitem = true
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    spawnabletypesType newspawnabletypesType = new spawnabletypesType()
                    {
                        name = l,
                        Items = new BindingList<object>()

                    };
                    currentspawnabletypesfile.spawnabletypes.type.Add(newspawnabletypesType);
                    currentspawnabletypesfile.isDirty = true;
                }

            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }

        }
        private void darkButton38_Click_2(object sender, EventArgs e)
        {
            currentspawnabletypesfile.spawnabletypes.type.Remove(SpawnabletpesLB.SelectedItem as spawnabletypesType);
            currentspawnabletypesfile.isDirty = true;
        }
        private void darkButton39_Click_1(object sender, EventArgs e)
        {
            AddNeweventFile form = new AddNeweventFile
            {
                currentproject = currentproject,
                newlocation = true,
                setbuttontest = "Add Spawnabletype File",
                SetTitle = "Add New Spawnabletypes File",
                settype = "Spawnabletype file Name (Use ModName.. eg. DorTags)"
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                string path = form.CustomLocation;
                string modname = form.TypesName;
                Directory.CreateDirectory(path);
                List<string> Spawnabletypesfile = new List<string>();
                Spawnabletypesfile.Add("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
                Spawnabletypesfile.Add("<spawnabletypes>");
                Spawnabletypesfile.Add("</spawnabletypes>");
                File.WriteAllLines(path + "\\" + modname + "_spawnabletypes.xml", Spawnabletypesfile);
                Spawnabletypesconfig test = new Spawnabletypesconfig(path + "\\" + modname + "_spawnabletypes.xml");
                test.Savespawnabletypes();
                currentproject.EconomyCore.AddCe(path.Replace(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\", ""), modname + "_spawnabletypes.xml", "spawnabletypes");
                currentproject.EconomyCore.SaveEconomycore();
                currentproject.SetSpawnabletypes();
                LoadSpawnableTypes();
                populateEconmyTreeview();
            }
        }
        private void darkButton40_Click(object sender, EventArgs e)
        {
            string Modname = Path.GetFileNameWithoutExtension(currentspawnabletypesfile.Filename);
            currentproject.EconomyCore.RemoveCe(Modname, out string foflderpath, out string filename, out bool deletedirectory);
            File.Delete(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\" + foflderpath + "\\" + filename);
            if (deletedirectory)
                Directory.Delete(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\" + foflderpath, true);
            currentproject.EconomyCore.SaveEconomycore();
            currentproject.removeSpawnabletype(currentspawnabletypesfile.Filename);
            currentproject.SetSpawnabletypes();
            populateEconmyTreeview();
        }
        private void HasDamageCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            if (HasDamageCB.Checked)
            {
                if (currentspawnabletypesfile.spawnabletypes.damage == null)
                {
                    currentspawnabletypesfile.spawnabletypes.damage = new spawnabletypesDamage()
                    {
                        min = 0,
                        max = 0
                    };
                }
            }
            else
            {
                if (currentspawnabletypesfile.spawnabletypes.damage != null)
                {
                    currentspawnabletypesfile.spawnabletypes.damage = null;
                }
            }
            currentspawnabletypesfile.isDirty = true;
            SetDamange();
        }
        #endregion spawnabletypes
        #region typesquery
        public bool FilterTiers = false;
        public bool FilterCategories = false;
        public bool FilterLocations = false;
        public bool FilterTags = false;
        public bool FilterFlags = false;
        private void darkButton15_Click(object sender, EventArgs e)
        {
            treeView2.Nodes.Clear();
        }
        public bool typeinfilterlist(typesType type, List<string> Queryitems)
        {
            bool istrue = true;
            if (Queryitems.Count == 0) { return true; }
            foreach (string filter in Queryitems)
            {
                string[] fsplit = filter.Split(',');
                switch (fsplit[1])
                {
                    case "definintions":
                        if (type.value == null)
                        {
                            istrue = false;
                            break;
                        }
                        switch (fsplit[2])
                        {
                            case "0":
                                string def = fsplit[0];
                                if (!type.value.Any(x => x.name == def))
                                        istrue = false;
                                break;
                            case "1":
                                def = fsplit[0];
                                if (!type.value.Any(x => x.name == def))
                                     istrue = false;
                                break;
                        }
                        break;
                    case "categories":
                        string cat = fsplit[0];
                        string typecat = "";
                        if (type.category == null)
                            typecat = "Other";
                        else
                            typecat = type.category.name;
                        if (typecat != cat)
                            istrue = false;
                        break;
                    case "usage":
                        string usage = fsplit[0];
                        if(type.usage == null) 
                        {
                            istrue = false;
                        }
                        else
                        {
                            if (!type.usage.Any(x => x.name == usage))
                                istrue = false;
                        }
                        break;
                    case "tags":
                        string tag = fsplit[0];
                        if (type.tag == null)
                        {
                            if (tag != "NULL")
                                istrue = false;
                        }
                        else
                        {
                            if (!type.tag.Any(x => x.name == tag))
                                istrue = false;
                        }
                        break;
                    case "flags":
                        {
                            switch (fsplit[0])
                            {
                                case "deloot":
                                    if (type.flags.deloot != 1)
                                        istrue = false;
                                    break;
                                case "crafted":
                                    if (type.flags.crafted != 1)
                                        istrue = false;
                                    break;
                                case "count_in_player":
                                    if (type.flags.count_in_player != 1)
                                        istrue = false;
                                    break;
                                case "count_in_map":
                                    if (type.flags.count_in_map != 1)
                                        istrue = false;
                                    break;
                                case "count_in_hoarder":
                                    if (type.flags.count_in_hoarder != 1)
                                        istrue = false;
                                    break;
                                case "count_in_cargo":
                                    if (type.flags.count_in_cargo != 1)
                                        istrue = false;
                                    break;
                            }
                        }
                        
                        break;

                }
            }
            if (istrue)
                return true;
            return false;
        }
        private void darkButton14_Click(object sender, EventArgs e)
        {
            List<string> Queryitems = new List<string>();
            
            //check Tiers
            //need to check if the fil;ter is for vanilla or user definitions
            if (FilterTiers)
            {
                switch (tabControl8.SelectedIndex)
                {
                    case 0:
                        List<CheckBox> checkboxesSummary = SetdefinitionsTPSummary.Controls.OfType<CheckBox>().ToList();
                        foreach (CheckBox cb in checkboxesSummary)
                        {
                            if (cb.Visible == true && cb.Checked)
                                Queryitems.Add(cb.Tag.ToString() + ",definintions,0");
                        }
                        break;
                    case 1:
                        checkboxesSummary = UserdefinitionsTPSummary.Controls.OfType<CheckBox>().ToList();
                        foreach (CheckBox cb in checkboxesSummary)
                        {
                            if (cb.Visible == true && cb.Checked)
                                Queryitems.Add(cb.Tag.ToString() + ",definintions,1");
                        }
                        break;
                }
            }
            //Check Categorys
            if (FilterCategories)
            {
                if (listBox5.Items.Count > 0)
                {
                    foreach (var item in listBox5.Items)
                    {
                        typesTypeCategory c = item as typesTypeCategory;
                        Queryitems.Add(c.name + ",categories");
                    }
                }
                else
                {
                    Queryitems.Add("Other,categories");
                }
            }
            // check locations (usage)
            if(FilterLocations)
            {
                if(listBox4.Items.Count > 0)
                {
                    foreach (var item in listBox4.Items)
                    {
                        typesTypeUsage u = item as typesTypeUsage;
                        Queryitems.Add(u.name + ",usage");
                    }
                }
            }
            //Check tag
            if(FilterTags)
            {
                if(listBox3.Items.Count > 0)
                {
                    foreach (var item in listBox3.Items)
                    {
                        typesTypeTag c = item as typesTypeTag;
                        Queryitems.Add(c.name + ",tags");
                    }
                }
                else
                {
                    Queryitems.Add("NULL,tags");
                }
            }
            //check flags
            if(FilterFlags)
            {
                if(checkBox43.Checked)
                    Queryitems.Add("deloot,flags");
                if (checkBox44.Checked)
                    Queryitems.Add("crafted,flags");
                if (checkBox45.Checked)
                    Queryitems.Add("count_in_player,flags");
                if (checkBox46.Checked)
                    Queryitems.Add("count_in_map,flags");
                if (checkBox47.Checked)
                    Queryitems.Add("count_in_hoarder,flags");
                if (checkBox48.Checked)
                    Queryitems.Add("count_in_cargo,flags");
            }

            int count = 0;
            treeView2.Nodes.Clear();
            TreeNode root = new TreeNode(Path.GetFileName(filename))
            {
                Tag = "Parent"
            };
            //Set Vanilla Treenode types
            TreeNode vanilla = new TreeNode("Vanilla Types")
            {
                Tag = "VanillaTypes"
            };
            foreach (typesType type in vanillatypes.types.type)
            {
                if(!typeinfilterlist(type, Queryitems)) { continue; }
                if(ZeroNomCB.Checked)
                {
                    if (type.nominal == 0)
                        continue;
                }
                string cat = "other";
                if (type.category != null)
                    cat = type.category.name;
                TreeNode typenode = new TreeNode(type.name)
                {
                    Tag = type
                };
                if (!vanilla.Nodes.ContainsKey(cat))
                {
                    TreeNode newcatnode = new TreeNode(cat)
                    {
                        Name = cat,
                        Tag = cat
                    };
                   
                    vanilla.Nodes.Add(newcatnode);
                }
                count++;
                vanilla.Nodes[cat].Nodes.Add(typenode);
            }
            root.Nodes.Add(vanilla);

            if (ModTypes.Count > 0)
            {
                foreach (TypesFile tf in ModTypes)
                {
                    TreeNode ModTypes = new TreeNode(tf.modname)
                    {
                        Tag = tf.modname
                    };
                    foreach (typesType type in tf.types.type)
                    {
                        if (!typeinfilterlist(type, Queryitems)) { continue; }
                        if (ZeroNomCB.Checked)
                        {
                            if (type.nominal == 0)
                                continue;
                        }
                        string cat = "other";
                        if (type.category != null)
                            cat = type.category.name;
                        TreeNode typenode = new TreeNode(type.name)
                        {
                            Tag = type
                        };
                        if (!ModTypes.Nodes.ContainsKey(cat))
                        {
                            TreeNode newcatnode = new TreeNode(cat)
                            {
                                Name = cat,
                                Tag = cat
                            };
                            ModTypes.Nodes.Add(newcatnode);
                        }
                        count++;
                        ModTypes.Nodes[cat].Nodes.Add(typenode);
                    }


                    root.Nodes.Add(ModTypes);
                }
            }
            darkLabel6.Text = "Found Items :- " + count.ToString();
            treeView2.Nodes.Add(root);
        }
        private void darkButton12_Click(object sender, EventArgs e)
        {
            typesTypeCategory c = comboBox8.SelectedItem as typesTypeCategory;
            if(!listBox5.Items.Contains(c))
                listBox5.Items.Add(c);
        }
        private void darkButton13_Click(object sender, EventArgs e)
        {
            if (listBox5.SelectedItems.Count > 0)
            {
                typesTypeCategory c = listBox5.SelectedItem as typesTypeCategory;
                listBox5.Items.Remove(c);
            }
        }
        private void darkButton10_Click(object sender, EventArgs e)
        {
            typesTypeUsage u = comboBox7.SelectedItem as typesTypeUsage;
            if (!listBox4.Items.Contains(u))
                listBox4.Items.Add(u);
        }
        private void darkButton11_Click(object sender, EventArgs e)
        {
            if (listBox4.SelectedItems.Count > 0)
            {
                typesTypeUsage u = listBox4.SelectedItem as typesTypeUsage;
                listBox4.Items.Remove(u);
            }
        }
        private void darkButton4_Click(object sender, EventArgs e)
        {
            typesTypeTag t = comboBox6.SelectedItem as typesTypeTag;
            if (!listBox3.Items.Contains(t))
                listBox3.Items.Add(t);
        }
        private void darkButton9_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedItems.Count > 0)
            {
                typesTypeTag t = listBox3.SelectedItem as typesTypeTag;
                listBox3.Items.Remove(t);
            }
        }
        private void checkBox69_CheckedChanged(object sender, EventArgs e)
        {
            FilterTiers = groupBox18.Enabled = checkBox69.Checked;
        }
        private void checkBox68_CheckedChanged(object sender, EventArgs e)
        {
            FilterCategories = groupBox13.Enabled = checkBox68.Checked;
        }
        private void checkBox67_CheckedChanged(object sender, EventArgs e)
        {
            FilterLocations = groupBox16.Enabled = checkBox67.Checked;
        }
        private void checkBox66_CheckedChanged(object sender, EventArgs e)
        {
            FilterTags = groupBox14.Enabled = checkBox66.Checked;
        }
        private void checkBox65_CheckedChanged(object sender, EventArgs e)
        {
            FilterFlags = groupBox19.Enabled = checkBox65.Checked;
        }
        private void darkButton54_Click(object sender, EventArgs e)
        {

        }
        #endregion typesquery
        #region PlayerSpawnPoints
        public void LoadPlayerSpawns()
        {
            isUserInteraction = false;

            playerspawnpoints = currentproject.cfgplayerspawnpoints.playerspawnpoints;

            min_dist_infectedNUD.Value = (decimal)playerspawnpoints.fresh.spawn_params.min_dist_infected;
            max_dist_infectedNUD.Value = (decimal)playerspawnpoints.fresh.spawn_params.max_dist_infected;
            min_dist_playerNUD.Value = (decimal)playerspawnpoints.fresh.spawn_params.min_dist_player;
            max_dist_playerNUD.Value = (decimal)playerspawnpoints.fresh.spawn_params.max_dist_player;
            min_dist_staticNUD.Value = (decimal)playerspawnpoints.fresh.spawn_params.min_dist_static;
            max_dist_staticNUD.Value = (decimal)playerspawnpoints.fresh.spawn_params.max_dist_static;

            grid_densityNUD.Value = (decimal)playerspawnpoints.fresh.generator_params.grid_density;
            grid_widthNUD.Value = (decimal)playerspawnpoints.fresh.generator_params.grid_width;
            grid_heightNUD.Value = (decimal)playerspawnpoints.fresh.generator_params.grid_height;
            GPmin_dist_staticNUD.Value = (decimal)playerspawnpoints.fresh.generator_params.min_dist_static;
            GPmax_dist_staticNUD.Value = (decimal)playerspawnpoints.fresh.generator_params.max_dist_static;
            min_steepnessNUD.Value = (decimal)playerspawnpoints.fresh.generator_params.min_steepness;
            max_steepnessNUD.Value = (decimal)playerspawnpoints.fresh.generator_params.max_steepness;
            SetPlayerSpawnLists();
            isUserInteraction = true;
        }
        private void SetPlayerSpawnLists()
        {
            if (playerspawnpoints.fresh != null)
            {
                PlayerFGreshSpawnLB.DisplayMember = "DisplayName";
                PlayerFGreshSpawnLB.ValueMember = "Value";
                PlayerFGreshSpawnLB.DataSource = playerspawnpoints.fresh.generator_posbubbles;
            }
            if (playerspawnpoints.hop != null)
            {
                PlayerSpanHopLB.DisplayMember = "DisplayName";
                PlayerSpanHopLB.ValueMember = "Value";
                PlayerSpanHopLB.DataSource = playerspawnpoints.hop.generator_posbubbles;
            }
            if (playerspawnpoints.travel != null)
            {
                PlayerspawntravelLB.DisplayMember = "DisplayName";
                PlayerspawntravelLB.ValueMember = "Value";
                PlayerspawntravelLB.DataSource = playerspawnpoints.travel.generator_posbubbles;
            }
        }
        private void PlayerFGreshSpawnLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PlayerFGreshSpawnLB.SelectedItems.Count == 0) return;
            playerspawnpointsFreshPos currentpos = PlayerFGreshSpawnLB.SelectedItem as playerspawnpointsFreshPos;
            isUserInteraction = false;
            FreshPosXNUD.Value = (decimal)currentpos.x;
            FreshPosZNUD.Value = (decimal)currentpos.z;
            isUserInteraction = true;

            pictureBox2.Invalidate();
        }
        private void FreshPosXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            playerspawnpointsFreshPos currentpos = PlayerFGreshSpawnLB.SelectedItem as playerspawnpointsFreshPos;
            currentpos.x = (float)FreshPosXNUD.Value;
            PlayerFGreshSpawnLB.Invalidate();
            currentproject.cfgplayerspawnpoints.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void FreshPosZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            playerspawnpointsFreshPos currentpos = PlayerFGreshSpawnLB.SelectedItem as playerspawnpointsFreshPos;
            currentpos.z = (float)FreshPosZNUD.Value;
            PlayerFGreshSpawnLB.Invalidate();
            currentproject.cfgplayerspawnpoints.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void darkButton23_Click(object sender, EventArgs e)
        {
            playerspawnpointsFreshPos newpos = new playerspawnpointsFreshPos()
            {
                x = currentproject.MapSize / 2,
                z = currentproject.MapSize /2
            };
            playerspawnpoints.fresh.generator_posbubbles.Add(newpos);
            PlayerFGreshSpawnLB.Invalidate();
            PlayerFGreshSpawnLB.SelectedIndex = PlayerFGreshSpawnLB.Items.Count - 1;
            currentproject.cfgplayerspawnpoints.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void darkButton22_Click(object sender, EventArgs e)
        {
            if (PlayerFGreshSpawnLB.SelectedItems.Count == 0) return;
            playerspawnpointsFreshPos currentpos = PlayerFGreshSpawnLB.SelectedItem as playerspawnpointsFreshPos;
            playerspawnpoints.fresh.generator_posbubbles.Remove(currentpos);
            PlayerFGreshSpawnLB.Invalidate();
            currentproject.cfgplayerspawnpoints.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {
            if (e is MouseEventArgs mouseEventArgs)
            {
                Cursor.Current = Cursors.WaitCursor;
                float scalevalue = ZoneScale * 0.05f;
                float mapsize = currentproject.MapSize;
                int newsize = (int)(mapsize * scalevalue);
                switch (tabControl16.SelectedIndex)
                {
                    case 0:
                        FreshPosXNUD.Value = (decimal)(mouseEventArgs.X / scalevalue);
                        FreshPosZNUD.Value = (decimal)((newsize - mouseEventArgs.Y) / scalevalue);
                        break;
                    case 1:
                        hopPosXNUD.Value = (decimal)(mouseEventArgs.X / scalevalue);
                        hopPosZNUD.Value = (decimal)((newsize - mouseEventArgs.Y) / scalevalue);
                        break;
                    case 2:
                        travelPosXNUD.Value = (decimal)(mouseEventArgs.X / scalevalue);
                        travelPosZNUD.Value = (decimal)((newsize - mouseEventArgs.Y) / scalevalue);
                        break;
                }
                Cursor.Current = Cursors.Default;
                currentproject.cfgplayerspawnpoints.isDirty = true;
                pictureBox2.Invalidate();
            }
        }
        private void DrawAllPlayerSpawns(object sender, PaintEventArgs e)
        {
            switch (tabControl16.SelectedIndex)
            {
                case 0:
                    if (playerspawnpoints.fresh == null) return;
                    playerspawnpointsFreshPos currentpos = PlayerFGreshSpawnLB.SelectedItem as playerspawnpointsFreshPos;
                    foreach (playerspawnpointsFreshPos newpos in playerspawnpoints.fresh.generator_posbubbles)
                    {
                        float scalevalue = ZoneScale * 0.05f;
                        int centerX = (int)(Math.Round(newpos.x) * scalevalue);
                        int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(newpos.z, 0) * scalevalue);
                        int radius = (int)(Math.Round(1f, 0) * scalevalue);
                        Point center = new Point(centerX, centerY);
                        Pen pen = new Pen(Color.Red, 4);
                        if (newpos == currentpos)
                            pen.Color = Color.LimeGreen;
                        getCircle(e.Graphics, pen, center, radius);
                    }
                    break;
                case 1:
                    if (playerspawnpoints.hop == null) return;
                    playerspawnpointsHopPos currenthoppos = PlayerSpanHopLB.SelectedItem as playerspawnpointsHopPos;
                    foreach (playerspawnpointsHopPos newpos in playerspawnpoints.hop.generator_posbubbles)
                    {
                        float scalevalue = ZoneScale * 0.05f;
                        int centerX = (int)(Math.Round(newpos.x) * scalevalue);
                        int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(newpos.z, 0) * scalevalue);
                        int radius = (int)(Math.Round(1f, 0) * scalevalue);
                        Point center = new Point(centerX, centerY);
                        Pen pen = new Pen(Color.Red, 4);
                        if (newpos == currenthoppos)
                            pen.Color = Color.LimeGreen;
                        getCircle(e.Graphics, pen, center, radius);
                    }
                    break;
                case 2:
                    if (playerspawnpoints.travel == null) return;
                    playerspawnpointsTravelPos currenttravelpos = PlayerspawntravelLB.SelectedItem as playerspawnpointsTravelPos;
                    foreach (playerspawnpointsTravelPos newpos in playerspawnpoints.travel.generator_posbubbles)
                    {
                        float scalevalue = ZoneScale * 0.05f;
                        int centerX = (int)(Math.Round(newpos.x) * scalevalue);
                        int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(newpos.z, 0) * scalevalue);
                        int radius = (int)(Math.Round(1f, 0) * scalevalue);
                        Point center = new Point(centerX, centerY);
                        Pen pen = new Pen(Color.Red, 4);
                        if (newpos == currenttravelpos)
                            pen.Color = Color.LimeGreen;
                        getCircle(e.Graphics, pen, center, radius);
                    }
                    break;
            }
            
        }
        private void SetSpawnScale()
        {
            float scalevalue = ZoneScale * 0.05f;
            float mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            pictureBox2.Size = new Size(newsize, newsize);
        }
        private void getCircle(Graphics drawingArea, Pen penToUse, Point center, int radius)
        {
            Rectangle rect = new Rectangle(center.X - 1, center.Y - 1, 2, 2);
            drawingArea.DrawEllipse(penToUse, rect);
            Rectangle rect2 = new Rectangle(center.X - radius, center.Y - radius, radius * 2, radius * 2);
            drawingArea.DrawEllipse(penToUse, rect2);
        }
        private void trackBar4_MouseUp(object sender, MouseEventArgs e)
        {
            ZoneScale = trackBar4.Value;
            SetSpawnScale();
        }
        private void min_dist_infectedNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            playerspawnpoints.fresh.spawn_params.min_dist_infected = min_dist_infectedNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void min_dist_playerNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            playerspawnpoints.fresh.spawn_params.min_dist_player = min_dist_playerNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void min_dist_staticNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            playerspawnpoints.fresh.spawn_params.min_dist_static = min_dist_staticNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void max_dist_infectedNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            playerspawnpoints.fresh.spawn_params.max_dist_infected = max_dist_infectedNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void max_dist_playerNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            playerspawnpoints.fresh.spawn_params.max_dist_player = max_dist_playerNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void max_dist_staticNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            playerspawnpoints.fresh.spawn_params.max_dist_static = max_dist_staticNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void grid_densityNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            playerspawnpoints.fresh.generator_params.grid_density = (int)grid_densityNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void grid_widthNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            playerspawnpoints.fresh.generator_params.grid_width = grid_widthNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void GPmin_dist_staticNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            playerspawnpoints.fresh.generator_params.min_dist_static = GPmin_dist_staticNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void min_steepnessNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            playerspawnpoints.fresh.generator_params.min_steepness = (int)min_steepnessNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void grid_heightNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            playerspawnpoints.fresh.generator_params.grid_height = grid_heightNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void GPmax_dist_staticNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            playerspawnpoints.fresh.generator_params.max_dist_static = GPmax_dist_staticNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void max_steepnessNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            playerspawnpoints.fresh.generator_params.max_steepness = (int)max_steepnessNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void tabControl16_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox2.Invalidate();
        }
        private void hopPosXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            playerspawnpointsHopPos currentpos = PlayerSpanHopLB.SelectedItem as playerspawnpointsHopPos;
            currentpos.x = (float)hopPosXNUD.Value;
            PlayerSpanHopLB.Invalidate();
            currentproject.cfgplayerspawnpoints.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void hopPosZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            playerspawnpointsHopPos currentpos = PlayerSpanHopLB.SelectedItem as playerspawnpointsHopPos;
            currentpos.z = (float)hopPosZNUD.Value;
            PlayerSpanHopLB.Invalidate();
            currentproject.cfgplayerspawnpoints.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void travelPosXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            playerspawnpointsTravelPos currentpos = PlayerspawntravelLB.SelectedItem as playerspawnpointsTravelPos;
            currentpos.x = (float)travelPosXNUD.Value;
            PlayerspawntravelLB.Invalidate();
            currentproject.cfgplayerspawnpoints.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void travelPosZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            playerspawnpointsTravelPos currentpos = PlayerspawntravelLB.SelectedItem as playerspawnpointsTravelPos;
            currentpos.z = (float)travelPosZNUD.Value;
            PlayerspawntravelLB.Invalidate();
            currentproject.cfgplayerspawnpoints.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void darkButton25_Click(object sender, EventArgs e)
        {
            playerspawnpointsHopPos newpos = new playerspawnpointsHopPos()
            {
                x = currentproject.MapSize / 2,
                z = currentproject.MapSize / 2
            };
            if(playerspawnpoints.hop == null)
            {
                playerspawnpoints.hop = new playerspawnpointsHop()
                {
                    generator_posbubbles = new BindingList<playerspawnpointsHopPos>()
                };
            }
            playerspawnpoints.hop.generator_posbubbles.Add(newpos);
            PlayerSpanHopLB.Invalidate();
            PlayerSpanHopLB.SelectedIndex = PlayerSpanHopLB.Items.Count - 1;
            currentproject.cfgplayerspawnpoints.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void darkButton24_Click(object sender, EventArgs e)
        {
            if (PlayerSpanHopLB.SelectedItems.Count == 0) return;
            playerspawnpointsHopPos currentpos = PlayerSpanHopLB.SelectedItem as playerspawnpointsHopPos;
            playerspawnpoints.hop.generator_posbubbles.Remove(currentpos);
            PlayerSpanHopLB.Invalidate();
            currentproject.cfgplayerspawnpoints.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void PlayerSpanHopLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PlayerSpanHopLB.SelectedItems.Count == 0) return;
            playerspawnpointsHopPos currentpos = PlayerSpanHopLB.SelectedItem as playerspawnpointsHopPos;
            isUserInteraction = false;
            hopPosXNUD.Value = (decimal)currentpos.x;
            hopPosZNUD.Value = (decimal)currentpos.z;
            isUserInteraction = true;
            pictureBox2.Invalidate();
        }
        #endregion PlayerSpawnPoints
        #region cfggameplayconfig
        private cfggameplay cfggameplay;
        private void LoadCFGGamelplay()
        {
            isUserInteraction = false;
            cfggameplay = currentproject.CFGGameplayConfig.cfggameplay;
            CFGGameplayTB.Text = cfggameplay.version.ToString();
            disableBaseDamageCB.Checked = cfggameplay.GeneralData.disableBaseDamage == 1 ? true : false;
            disableContainerDamageCB.Checked = cfggameplay.GeneralData.disableContainerDamage == 1 ? true : false;
            disableRespawnDialogCB.Checked = cfggameplay.GeneralData.disableRespawnDialog == 1 ? true : false;

            disablePersonalLightCB.Checked = cfggameplay.PlayerData.disablePersonalLight == 1 ? true : false;
            sprintStaminaModifierErcNUD.Value = (decimal)cfggameplay.PlayerData.StaminaData.sprintStaminaModifierErc;
            sprintStaminaModifierCroNUD.Value = (decimal)cfggameplay.PlayerData.StaminaData.sprintStaminaModifierCro;
            staminaWeightLimitThresholdNUD.Value = (decimal)cfggameplay.PlayerData.StaminaData.staminaWeightLimitThreshold;
            staminaMaxNUD.Value = (decimal)cfggameplay.PlayerData.StaminaData.staminaMax;
            staminaKgToStaminaPercentPenaltyNUD.Value = (decimal)cfggameplay.PlayerData.StaminaData.staminaKgToStaminaPercentPenalty;
            staminaMinCapNUD.Value = (decimal)cfggameplay.PlayerData.StaminaData.staminaMinCap;

            lightingConfigNUD.Value = cfggameplay.WorldsData.lightingConfig;

            disableIsCollidingBBoxCheckCB.Checked = cfggameplay.BaseBuildingData.HologramData.disableIsCollidingBBoxCheck == 1 ? true : false;
            disableIsCollidingPlayerCheckCB.Checked = cfggameplay.BaseBuildingData.HologramData.disableIsCollidingPlayerCheck == 1 ? true : false;
            disableIsClippingRoofCheckCB.Checked = cfggameplay.BaseBuildingData.HologramData.disableIsClippingRoofCheck == 1 ? true : false;
            disableIsBaseViableCheckCB.Checked = cfggameplay.BaseBuildingData.HologramData.disableIsBaseViableCheck == 1 ? true : false;
            disableIsCollidingGPlotCheckCB.Checked = cfggameplay.BaseBuildingData.HologramData.disableIsCollidingGPlotCheck == 1 ? true : false;
            disableIsCollidingAngleCheckCB.Checked = cfggameplay.BaseBuildingData.HologramData.disableIsCollidingAngleCheck == 1 ? true : false;
            disableIsPlacementPermittedCheckCB.Checked = cfggameplay.BaseBuildingData.HologramData.disableIsPlacementPermittedCheck == 1 ? true : false;
            disableHeightPlacementCheckCB.Checked = cfggameplay.BaseBuildingData.HologramData.disableHeightPlacementCheck == 1 ? true : false;
            disableIsUnderwaterCheckCB.Checked = cfggameplay.BaseBuildingData.HologramData.disableIsUnderwaterCheck == 1 ? true : false;
            disableIsInTerrainCheckCB.Checked = cfggameplay.BaseBuildingData.HologramData.disableIsInTerrainCheck == 1 ? true : false;

            disablePerformRoofCheckCB.Checked = cfggameplay.BaseBuildingData.ConstructionData.disablePerformRoofCheck == 1 ? true : false;
            disableIsCollidingCheckCB.Checked = cfggameplay.BaseBuildingData.ConstructionData.disableIsCollidingCheck == 1 ? true : false;
            disableDistanceCheckCB.Checked = cfggameplay.BaseBuildingData.ConstructionData.disableDistanceCheck == 1 ? true : false;

            hitDirectionOverrideEnabledCB.Checked = cfggameplay.UIData.HitIndicationData.hitDirectionOverrideEnabled == 1 ? true : false;
            hitDirectionBehaviourCB.Checked = cfggameplay.UIData.HitIndicationData.hitDirectionBehaviour == 1 ? true : false;
            hitDirectionStyleCB.Checked = cfggameplay.UIData.HitIndicationData.hitDirectionStyle == 1 ? true : false;
            hitDirectionMaxDurationNUD.Value = (decimal)cfggameplay.UIData.HitIndicationData.hitDirectionMaxDuration;
            hitDirectionBreakPointRelativeNUD.Value = (decimal)cfggameplay.UIData.HitIndicationData.hitDirectionBreakPointRelative;
            hitDirectionScatterNUD.Value = (decimal)cfggameplay.UIData.HitIndicationData.hitDirectionScatter;
            hitIndicationPostProcessEnabledCB.Checked = cfggameplay.UIData.HitIndicationData.hitIndicationPostProcessEnabled == 1 ? true : false;
            isUserInteraction = true;
        }
        private void m_Color_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            ColorPickerDialog cpick = new ColorPickerDialog();
            cpick.StartPosition = FormStartPosition.CenterParent;
            string col = cfggameplay.UIData.HitIndicationData.hitDirectionIndicatorColorStr;
            cpick.Color = ColorTranslator.FromHtml(col);
            if (cpick.ShowDialog() == DialogResult.OK)
            {
                string Colourname = "0x" + cpick.Color.Name.ToLower();
                cfggameplay.UIData.HitIndicationData.hitDirectionIndicatorColorStr = Colourname;
                pb.Invalidate();
                currentproject.CFGGameplayConfig.isDirty = true;
            }
        }
        private void m_Color_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            Rectangle region;
            region = pb.ClientRectangle;
            Color colour = ColorTranslator.FromHtml(cfggameplay.UIData.HitIndicationData.hitDirectionIndicatorColorStr);
            using (Brush brush = new SolidBrush(colour))
            {
                e.Graphics.FillRectangle(brush, region);
            }
            e.Graphics.DrawRectangle(SystemPens.ControlText, region.Left, region.Top, region.Width - 1, region.Height - 1);
        }
        private void disableBaseDamageCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.GeneralData.disableBaseDamage = disableBaseDamageCB.Checked == true ? 1 : 0;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableContainerDamageCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.GeneralData.disableContainerDamage = disableContainerDamageCB.Checked == true ? 1 : 0;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableRespawnDialogCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.GeneralData.disableRespawnDialog = disableRespawnDialogCB.Checked == true ? 1 : 0;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disablePersonalLightCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.disablePersonalLight = disablePersonalLightCB.Checked == true ? 1 : 0;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void sprintStaminaModifierErcNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.StaminaData.sprintStaminaModifierErc = (decimal)Math.Round(sprintStaminaModifierErcNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void sprintStaminaModifierCroNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.StaminaData.sprintStaminaModifierCro = (decimal)Math.Round(sprintStaminaModifierCroNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void staminaWeightLimitThresholdNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.StaminaData.staminaWeightLimitThreshold = (decimal)Math.Round(staminaWeightLimitThresholdNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void staminaMaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.StaminaData.staminaMax = (decimal)Math.Round(staminaMaxNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void staminaKgToStaminaPercentPenaltyNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.StaminaData.staminaKgToStaminaPercentPenalty = (decimal)Math.Round(staminaKgToStaminaPercentPenaltyNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void staminaMinCapNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.StaminaData.staminaMinCap = (decimal)Math.Round(staminaMinCapNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableIsCollidingBBoxCheckCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.BaseBuildingData.HologramData.disableIsCollidingBBoxCheck = disableIsCollidingBBoxCheckCB.Checked == true ? 1 : 0;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableIsCollidingPlayerCheckCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.BaseBuildingData.HologramData.disableIsCollidingPlayerCheck = disableIsCollidingPlayerCheckCB.Checked == true ? 1 : 0;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableIsClippingRoofCheckCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.BaseBuildingData.HologramData.disableIsClippingRoofCheck = disableIsClippingRoofCheckCB.Checked == true ? 1 : 0;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableIsBaseViableCheckCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.BaseBuildingData.HologramData.disableIsBaseViableCheck = disableIsBaseViableCheckCB.Checked == true ? 1 : 0;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableIsCollidingGPlotCheckCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.BaseBuildingData.HologramData.disableIsCollidingGPlotCheck = disableIsCollidingGPlotCheckCB.Checked == true ? 1 : 0;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableIsCollidingAngleCheckCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.BaseBuildingData.HologramData.disableIsCollidingAngleCheck = disableIsCollidingAngleCheckCB.Checked == true ? 1 : 0;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableIsPlacementPermittedCheckCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.BaseBuildingData.HologramData.disableIsPlacementPermittedCheck = disableIsPlacementPermittedCheckCB.Checked == true ? 1 : 0;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableHeightPlacementCheckCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.BaseBuildingData.HologramData.disableHeightPlacementCheck = disableHeightPlacementCheckCB.Checked == true ? 1 : 0;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableIsUnderwaterCheckCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.BaseBuildingData.HologramData.disableIsUnderwaterCheck = disableIsUnderwaterCheckCB.Checked == true ? 1 : 0;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableIsInTerrainCheckCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.BaseBuildingData.HologramData.disableIsInTerrainCheck = disableIsInTerrainCheckCB.Checked == true ? 1 : 0;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disablePerformRoofCheckCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.BaseBuildingData.ConstructionData.disablePerformRoofCheck = disablePerformRoofCheckCB.Checked == true ? 1 : 0;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableIsCollidingCheckCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.BaseBuildingData.ConstructionData.disableIsCollidingCheck = disableIsCollidingCheckCB.Checked == true ? 1 : 0;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableDistanceCheckCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.BaseBuildingData.ConstructionData.disableDistanceCheck = disableDistanceCheckCB.Checked == true ? 1 : 0;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void hitDirectionOverrideEnabledCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.UIData.HitIndicationData.hitDirectionOverrideEnabled = hitDirectionOverrideEnabledCB.Checked == true ? 1 : 0;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void hitDirectionBehaviourCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.UIData.HitIndicationData.hitDirectionBehaviour = hitDirectionBehaviourCB.Checked == true ? 1 : 0;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void hitDirectionStyleCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.UIData.HitIndicationData.hitDirectionStyle = hitDirectionStyleCB.Checked == true ? 1 : 0;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void hitDirectionMaxDurationNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.UIData.HitIndicationData.hitDirectionMaxDuration = (decimal)Math.Round(hitDirectionMaxDurationNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void hitDirectionBreakPointRelativeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.UIData.HitIndicationData.hitDirectionBreakPointRelative = (decimal)Math.Round(hitDirectionBreakPointRelativeNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void hitDirectionScatterNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.UIData.HitIndicationData.hitDirectionScatter = (decimal)Math.Round(hitDirectionScatterNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void hitIndicationPostProcessEnabledCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.UIData.HitIndicationData.hitIndicationPostProcessEnabled = hitIndicationPostProcessEnabledCB.Checked == true ? 1 : 0;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void lightingConfigNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.WorldsData.lightingConfig = (int)lightingConfigNUD.Value;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        #endregion cfggameplayconfig
        #region cfgrandompresets
        public object currentRandomPreset;
        private void LoadRandomPresets()
        {
            isUserInteraction = false;
            PresetItemListLB.DisplayMember = "DisplayName";
            PresetItemListLB.ValueMember = "Value";
            PresetItemListLB.DataSource = currentproject.cfgrandompresetsconfig.randompresets.Items;
            isUserInteraction = true;
        }
        private void PresetItemListLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PresetItemListLB.SelectedIndex == -1) { return; }
            isUserInteraction = false;
            currentRandomPreset = PresetItemListLB.SelectedItem as object;
            ClearInfo();
            if (currentRandomPreset is randompresetsAttachments)
            {
                randompresetsAttachments currentAttachments = currentRandomPreset as randompresetsAttachments;
                RandompresetCargoGB.Visible = false;
                RandompreseAttachmentGB.Visible = true;
                setRandompresetAttachemnt();
            }
            else if (currentRandomPreset is randompresetsCargo)
            {
                randompresetsCargo currentCargo = currentRandomPreset as randompresetsCargo;
                RandompresetCargoGB.Visible = true;
                RandompreseAttachmentGB.Visible = false;
                SetRandomPresetCargo();
            }
            isUserInteraction = true;
        }
        private void setRandompresetAttachemnt()
        {
            randompresetsAttachments currentAttachments = currentRandomPreset as randompresetsAttachments;
            RandomPresetAttachmentChanceNUD.Value = currentAttachments.chance;
            RandomPresetAttchemntNameTB.Text = currentAttachments.name;
            PresetAttachmentsItemsLB.DisplayMember = "DisplayName";
            PresetAttachmentsItemsLB.ValueMember = "Value";
            PresetAttachmentsItemsLB.DataSource = currentAttachments.item;
        }
        private void SetRandomPresetCargo()
        {
            randompresetsCargo currentcargo = currentRandomPreset as randompresetsCargo;
            RandomPresetCargoChanceNUD.Value = currentcargo.chance;
            RandomPresetNameTB.Text = currentcargo.name;
            PresetCargoItemsLB.DisplayMember = "DisplayName";
            PresetCargoItemsLB.ValueMember = "Value";
            PresetCargoItemsLB.DataSource = currentcargo.item;
        }
        private void PresetCArgoItemsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PresetCargoItemsLB.SelectedItems.Count == 0) return;
            randompresetsCargoItem currentcargoitem = PresetCargoItemsLB.SelectedItem as randompresetsCargoItem;
            isUserInteraction = false;
            RandomPresetCargoItemchanceNUD.Value = currentcargoitem.chance;
            isUserInteraction = true;
        }
        private void PresetAttachmentsItemsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PresetAttachmentsItemsLB.SelectedItems.Count == 0) return;
            randompresetsAttachmentsItem currentattachmentitem = PresetAttachmentsItemsLB.SelectedItem as randompresetsAttachmentsItem;
            isUserInteraction = false;
            RandomPresetAttachmentItemchanceNUD.Value = currentattachmentitem.chance;
            isUserInteraction = true;
        }
        private void RandomPresetCargoChanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            randompresetsCargo currentCargo = currentRandomPreset as randompresetsCargo;
            currentCargo.chance = RandomPresetCargoChanceNUD.Value;
            currentproject.cfgrandompresetsconfig.isDirty = true;
        }
        private void RandomPresetCargoItemchanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            randompresetsCargoItem currentcargoitem = PresetCargoItemsLB.SelectedItem as randompresetsCargoItem;
            currentcargoitem.chance = RandomPresetCargoItemchanceNUD.Value;
            currentproject.cfgrandompresetsconfig.isDirty = true;
        }
        private void RandomPresetAttachmentChanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            randompresetsAttachments currentAttachments = currentRandomPreset as randompresetsAttachments;
            currentAttachments.chance = RandomPresetAttachmentChanceNUD.Value;
            currentproject.cfgrandompresetsconfig.isDirty = true;
        }
        private void RandomPresetAttachmentItemchanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            randompresetsAttachmentsItem currentAttachmentitem = PresetAttachmentsItemsLB.SelectedItem as randompresetsAttachmentsItem;
            currentAttachmentitem.chance = RandomPresetAttachmentItemchanceNUD.Value;
            currentproject.cfgrandompresetsconfig.isDirty = true;
        }
        private void darkButton48_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseMultiple = false,
                isCategoryitem = true
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    randompresetsCargoItem currentcargoitem = PresetCargoItemsLB.SelectedItem as randompresetsCargoItem;
                    currentcargoitem.name = l;
                    PresetCargoItemsLB.Refresh();
                    currentproject.cfgrandompresetsconfig.isDirty = true;
                    SetRandomPresetCargo();
                }

            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton47_Click(object sender, EventArgs e)
        {
            randompresetsCargoItem newitem = new randompresetsCargoItem()
            {
                name = "New Item, Please change me...",
                chance = 1
            };
            randompresetsCargo currentcargo = currentRandomPreset as randompresetsCargo;
            currentcargo.item.Add(newitem);
            currentproject.cfgrandompresetsconfig.isDirty = true;
        }
        private void darkButton46_Click(object sender, EventArgs e)
        {
            if (PresetCargoItemsLB.SelectedItems == null) { return; }
            int index = PresetCargoItemsLB.SelectedIndex;
            randompresetsCargoItem currentcargoitem = PresetCargoItemsLB.SelectedItem as randompresetsCargoItem;
            randompresetsCargo currentcargo = currentRandomPreset as randompresetsCargo;
            currentcargo.item.Remove(currentcargoitem);
            currentproject.cfgrandompresetsconfig.isDirty = true;
            PresetCargoItemsLB.SelectedIndex = -1;
            if (index == 0 && PresetCargoItemsLB.Items.Count > 0)
                PresetCargoItemsLB.SelectedIndex = index;
            else
                PresetCargoItemsLB.SelectedIndex = index - 1;
        }
        private void darkButton49_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseMultiple = false,
                isCategoryitem = true
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    randompresetsAttachmentsItem currentAttachemntitem = PresetAttachmentsItemsLB.SelectedItem as randompresetsAttachmentsItem;
                    currentAttachemntitem.name = l;
                    PresetAttachmentsItemsLB.Refresh();
                    currentproject.cfgrandompresetsconfig.isDirty = true;
                    setRandompresetAttachemnt();
                }

            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton42_Click(object sender, EventArgs e)
        {
            randompresetsAttachmentsItem newitem = new randompresetsAttachmentsItem()
            {
                name = "New Item, Please change me...",
                chance = 1
            };
            randompresetsAttachments currentAttachemnt = currentRandomPreset as randompresetsAttachments;
            currentAttachemnt.item.Add(newitem);
            currentproject.cfgrandompresetsconfig.isDirty = true;
        }
        private void darkButton41_Click(object sender, EventArgs e)
        {
            if (PresetAttachmentsItemsLB.SelectedItems == null) { return; }
            int index = PresetAttachmentsItemsLB.SelectedIndex;
            randompresetsAttachmentsItem currentAttachemntitem = PresetAttachmentsItemsLB.SelectedItem as randompresetsAttachmentsItem;
            randompresetsAttachments currentAttachent = currentRandomPreset as randompresetsAttachments;
            currentAttachent.item.Remove(currentAttachemntitem);
            currentproject.cfgrandompresetsconfig.isDirty = true;
            PresetAttachmentsItemsLB.SelectedIndex = -1;
            if (index == 0 && PresetAttachmentsItemsLB.Items.Count > 0)
                PresetAttachmentsItemsLB.SelectedIndex = index;
            else
                PresetAttachmentsItemsLB.SelectedIndex = index - 1;
        }
        private void darkButton43_Click(object sender, EventArgs e)
        {
            if (PresetItemListLB.SelectedItems == null) { return; }
            int index = PresetItemListLB.SelectedIndex;
            currentproject.cfgrandompresetsconfig.randompresets.Items.Remove(currentRandomPreset);
            currentproject.cfgrandompresetsconfig.isDirty = true;
            PresetItemListLB.SelectedIndex = -1;
            if (index == 0 && PresetItemListLB.Items.Count > 0)
                PresetItemListLB.SelectedIndex = index;
            else
                PresetItemListLB.SelectedIndex = index - 1;
            SetuprandomPresetsForSpawnabletypes();
        }
        private void darkButton45_Click(object sender, EventArgs e)
        {
            if (PresetItemListLB.SelectedItem == null) { return; }
            randompresetsCargo newcargo = new randompresetsCargo()
            {
                name = "NewCargoList",
                chance = 1,
                item = new BindingList<randompresetsCargoItem>()
            };
            if (currentproject.cfgrandompresetsconfig.randompresets.Items == null)
                currentproject.cfgrandompresetsconfig.randompresets.Items = new BindingList<object>();
            currentproject.cfgrandompresetsconfig.randompresets.Items.Add(newcargo);
            PresetItemListLB.SelectedIndex = -1;
            PresetItemListLB.SelectedIndex = 0;
            currentproject.cfgrandompresetsconfig.isDirty = true;
            SetuprandomPresetsForSpawnabletypes();
        }
        private void darkButton44_Click(object sender, EventArgs e)
        {
            if (PresetItemListLB.SelectedItem == null) { return; }
            randompresetsAttachments newspawnabletypesTypeAttachments = new randompresetsAttachments()
            {
                item = new BindingList<randompresetsAttachmentsItem>()
            };
            if ( currentproject.cfgrandompresetsconfig.randompresets.Items == null)
                currentproject.cfgrandompresetsconfig.randompresets.Items = new BindingList<object>();
            currentproject.cfgrandompresetsconfig.randompresets.Items.Add(newspawnabletypesTypeAttachments);
            PresetItemListLB.SelectedIndex = -1;
            PresetItemListLB.SelectedIndex = 0;
            currentproject.cfgrandompresetsconfig.isDirty = true;
            SetuprandomPresetsForSpawnabletypes();
        }
        private void RandomPresetNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            randompresetsCargo currentCargo = currentRandomPreset as randompresetsCargo;
            currentCargo.name = RandomPresetNameTB.Text;
            PresetItemListLB.Refresh();
            currentproject.cfgrandompresetsconfig.isDirty = true;
            SetuprandomPresetsForSpawnabletypes();
        }
        private void RandomPresetAttchemntNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            randompresetsAttachments currentAttachment = currentRandomPreset as randompresetsAttachments;
            currentAttachment.name = RandomPresetAttchemntNameTB.Text;
            PresetItemListLB.Refresh();
            currentproject.cfgrandompresetsconfig.isDirty = true;
            SetuprandomPresetsForSpawnabletypes();
        }
        #endregion cfgrandompresets
        #region CFGAreaEffects
        public cfgEffectArea cfgEffectArea;
        public Position currentsafeposition;
        public Areas CurrentToxicArea;
        private void LoadContaminatoedArea()
        {
            isUserInteraction = false;
            cfgEffectArea = currentproject.cfgEffectAreaConfig.cfgEffectArea;
            AreasLB.DisplayMember = "DisplayName";
            AreasLB.ValueMember = "Value";
            AreasLB.DataSource = cfgEffectArea.Areas;

            SafePositionsLB.DisplayMember = "DisplayName";
            SafePositionsLB.ValueMember = "Value";
            SafePositionsLB.DataSource = cfgEffectArea._positions;

            isUserInteraction = true;
        }
        private void SafePositionsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SafePositionsLB.SelectedItems.Count < 1) return;
            currentsafeposition = SafePositionsLB.SelectedItem as Position;
            isUserInteraction = false;
            SafePositionXNUD.Value = (decimal)currentsafeposition.X;
            SafePositionZNUD.Value = (decimal)currentsafeposition.Z;

            isUserInteraction = true;
        }
        private void AreasLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AreasLB.SelectedItems.Count < 1) return;
            CurrentToxicArea = AreasLB.SelectedItem as Areas;
            isUserInteraction = false;

            AreaNameTB.Text = CurrentToxicArea.AreaName;
            TypeTB.Text = CurrentToxicArea.Type;
            TriggerTypeTB.Text = CurrentToxicArea.TriggerType;
            PosXNUD.Value = CurrentToxicArea.Data.Pos[0];
            posYNUD.Value = CurrentToxicArea.Data.Pos[1];
            posZNUD.Value = CurrentToxicArea.Data.Pos[2];
            RadiusNUD.Value = CurrentToxicArea.Data.Radius;
            PosHeightNUD.Value = CurrentToxicArea.Data.PosHeight;
            NegHeightNUD.Value = CurrentToxicArea.Data.NegHeight;
            InnerRingCountNUD.Value = CurrentToxicArea.Data.InnerRingCount;
            InnerPartDistNUD.Value = CurrentToxicArea.Data.InnerPartDist;
            OuterRingToggleCB.Checked = CurrentToxicArea.Data.OuterRingToggle == 1 ? true : false;
            OuterPartDistNUD.Value = CurrentToxicArea.Data.OuterPartDist;
            OuterOffsetNUD.Value = CurrentToxicArea.Data.OuterOffset;
            VerticalLayersNUD.Value = CurrentToxicArea.Data.VerticalLayers;
            VerticalOffsetNUD.Value = CurrentToxicArea.Data.VerticalOffset;
            ParticleNameTB.Text = CurrentToxicArea.Data.ParticleName;
            AroundPartNameTB.Text = CurrentToxicArea.PlayerData.AroundPartName;
            TinyPartNameTB.Text = CurrentToxicArea.PlayerData.TinyPartName;
            PPERequesterTypeTB.Text = CurrentToxicArea.PlayerData.PPERequesterType;

            isUserInteraction = true;
        }
        private void AreaNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentToxicArea.AreaName = AreaNameTB.Text;
            currentproject.cfgEffectAreaConfig.isDirty = true;
        }
        private void TypeTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentToxicArea.Type = TypeTB.Text;
            currentproject.cfgEffectAreaConfig.isDirty = true;
        }
        private void TriggerTypeTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentToxicArea.TriggerType = TriggerTypeTB.Text;
            currentproject.cfgEffectAreaConfig.isDirty = true;
        }
        private void PosNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentToxicArea.Data.Pos = new int[] { (int)PosXNUD.Value, (int)posYNUD.Value, (int)posZNUD.Value };
            currentproject.cfgEffectAreaConfig.isDirty = true;
        }
        private void AreaNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            NumericUpDown nud = sender as NumericUpDown;
            CurrentToxicArea.Data.SetIntValue(nud.Name.Substring(0, nud.Name.Length - 3), (int)nud.Value);
            currentproject.cfgEffectAreaConfig.isDirty = true;
        }
        private void OuterRingToggleCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentToxicArea.Data.OuterRingToggle = OuterRingToggleCB.Checked == true ? 1 : 0;
            currentproject.cfgEffectAreaConfig.isDirty = true;
        } 
        private void ParticleNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentToxicArea.Data.ParticleName = ParticleNameTB.Text;
            currentproject.cfgEffectAreaConfig.isDirty = true;
        }
        private void AroundPartNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentToxicArea.PlayerData.AroundPartName = AroundPartNameTB.Text;
            currentproject.cfgEffectAreaConfig.isDirty = true;
        }
        private void TinyPartNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentToxicArea.PlayerData.TinyPartName = TinyPartNameTB.Text;
            currentproject.cfgEffectAreaConfig.isDirty = true;
        }
        private void PPERequesterTypeTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentToxicArea.PlayerData.PPERequesterType = PPERequesterTypeTB.Text;
            currentproject.cfgEffectAreaConfig.isDirty = true;
        }
        private void SafePositionNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            currentsafeposition.X = (int)SafePositionXNUD.Value;
            currentsafeposition.Z = (int)SafePositionZNUD.Value;
            currentsafeposition.Name = currentsafeposition.X.ToString() + "," + currentsafeposition.Z.ToString();
            SafePositionsLB.Invalidate();
            currentproject.cfgEffectAreaConfig.isDirty = true;
        }
        private void darkButton52_Click(object sender, EventArgs e)
        {
            Data newdata = new Data()
            {
                Pos = new int[] { 0, 0, 0 },
                Radius = 0,
                PosHeight = 0,
                NegHeight = 0,
                InnerRingCount = 0,
                InnerPartDist = 0,
                OuterRingToggle = 1,
                OuterPartDist = 0,
                OuterOffset = 0,
                VerticalLayers = 0,
                VerticalOffset = 0,
                ParticleName = "graphics/particles/contaminated_area_gas_bigass"
            };
            PlayerData newplayerData = new PlayerData()
            {
                AroundPartName = "graphics/particles/contaminated_area_gas_around",
                TinyPartName = "graphics/particles/contaminated_area_gas_around_tiny",
                PPERequesterType = "PPERequester_ContaminatedAreaTint"
            };
            cfgEffectArea.Areas.Add(new Areas()
            {
                AreaName = "New-Toxic-Area",
                Type = "ContaminatedArea_Static",
                TriggerType = "ContaminatedTrigger",
                Data = newdata,
                PlayerData = newplayerData
            }
            );
            currentproject.cfgEffectAreaConfig.isDirty = true;
            AreasLB.SelectedIndex = -1;
            AreasLB.SelectedIndex = AreasLB.Items.Count - 1;
        }
        private void darkButton53_Click(object sender, EventArgs e)
        {
            if (AreasLB.SelectedItems.Count < 1) return;
            int index = AreasLB.SelectedIndex;
            cfgEffectArea.Areas.Remove(CurrentToxicArea);
            currentproject.cfgEffectAreaConfig.isDirty = true;
            AreasLB.SelectedIndex = -1;
            if (index - 1 == -1)
            {
                if (AreasLB.Items.Count > 0)
                    AreasLB.SelectedIndex = 0;
            }
            else
            {
                AreasLB.SelectedIndex = index - 1;
            }
        }
        private void darkButton50_Click(object sender, EventArgs e)
        {
            cfgEffectArea._positions.Add(new Position()
            {
                Name = "0,0",
                X = 0,
                Z = 0
            }
);
            currentproject.cfgEffectAreaConfig.isDirty = true;
            SafePositionsLB.SelectedIndex = -1;
            SafePositionsLB.SelectedIndex = SafePositionsLB.Items.Count - 1;
        }
        private void darkButton51_Click(object sender, EventArgs e)
        {
            if (SafePositionsLB.SelectedItems.Count < 1) return;
            int index = SafePositionsLB.SelectedIndex;
            cfgEffectArea._positions.Remove(currentsafeposition);
            currentproject.cfgEffectAreaConfig.isDirty = true;
            SafePositionsLB.SelectedIndex = -1;
            if (index - 1 == -1)
            {
                if (SafePositionsLB.Items.Count > 0)
                    SafePositionsLB.SelectedIndex = 0;
            }
            else
            {
                SafePositionsLB.SelectedIndex = index - 1;
            }
        }
        #endregion CFGAreaEffects
        #region gloabls
        private variables variables;
        public void LoadGlobals()
        {
            isUserInteraction = false;
            variablesvarvalueNUD.Maximum = int.MaxValue;
            variables = currentproject.gloabsconfig.variables;
            VariablesLB.DisplayMember = "DisplayName";
            VariablesLB.ValueMember = "Value";
            VariablesLB.DataSource = variables.var;
            isUserInteraction = true;
        }
        private void listBox7_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (VariablesLB.SelectedItems.Count < 1) return;
            variablesVar m_var = VariablesLB.SelectedItem as variablesVar;
            variablesvarnameTB.Text = m_var.name;
            variablesvartypeNUD.Value = m_var.type;
            variablesvarvalueNUD.Value = m_var.value;
        }
        private void variablesvartypeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            variablesVar m_var = VariablesLB.SelectedItem as variablesVar;
            m_var.type = (int)variablesvartypeNUD.Value;
            currentproject.gloabsconfig.isDirty = true;
        }
        private void variablesvarvalueNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            variablesVar m_var = VariablesLB.SelectedItem as variablesVar;
            m_var.value = (int)variablesvarvalueNUD.Value;
            currentproject.gloabsconfig.isDirty = true;
        }
        #endregion globals
        #region weather
        public weather weather;
        public void Loadweather()
        {
            isUserInteraction = false;
            weather = currentproject.weatherconfig.weather;

            weatherenabledCB.Checked = weather.enable == 1 ? true : false;
            weatherrestartCB.Checked = weather.reset == 1 ? true : false;
            //Overcast
            OCactualNUD.Value = weather.overcast.current.actual;
            OCtimeNUD.Value = weather.overcast.current.time;
            OCdurationNUD.Value = weather.overcast.current.duration;
            OLminNUD.Value = weather.overcast.limits.min;
            OLmaxNUD.Value = weather.overcast.limits.max;
            OTLminNUD.Value = weather.overcast.timelimits.min;
            OTLmaxNUD.Value = weather.overcast.timelimits.max;
            OCLminNUD.Value = weather.overcast.changelimits.min;
            OCLmaxNUD.Value = weather.overcast.changelimits.max;
            //fog
            FCactualNUD.Value = weather.fog.current.actual;
            FCtimeNUD.Value = weather.fog.current.time;
            FCdurationNUD.Value = weather.fog.current.duration;
            FLminNUD.Value = weather.fog.limits.min;
            FLmaxNUD.Value = weather.fog.limits.max;
            FTLminNUD.Value = weather.fog.timelimits.min;
            FTLmaxNUD.Value = weather.fog.timelimits.max;
            FCLminNUD.Value = weather.fog.changelimits.min;
            FCLmaxNUD.Value = weather.fog.changelimits.max;
            //rain
            RCactualNUD.Value = weather.rain.current.actual;
            RCtimeNUD.Value = weather.rain.current.time;
            RCdurationNUD.Value = weather.rain.current.duration;
            RLminNUD.Value = weather.rain.limit.min;
            RLmaxNUD.Value = weather.rain.limit.max;
            RTLminNUD.Value = weather.rain.timelimits.min;
            RTLmaxNUD.Value = weather.rain.timelimits.max;
            RCLminNUD.Value = weather.rain.changelimits.min;
            RCLmaxNUD.Value = weather.rain.changelimits.max;
            RTmaxNUD.Value = weather.rain.thresholds.max;
            RTminNUD.Value = weather.rain.thresholds.min;
            RTendNUD.Value = weather.rain.thresholds.end;
            //wind
            WMaxSpeedNUD.Value = weather.wind.maxspeed;
            WPminNUD.Value = weather.wind.@params.min;
            WPmaxNUD.Value = weather.wind.@params.max;
            WPfrequencyNUD.Value = weather.wind.@params.frequency;
            //storm
            SdensityNUD.Value = weather.storm.density;
            SthresholdNUD.Value = weather.storm.threshold;
            StimeoutNUD.Value = weather.storm.timeout;

            isUserInteraction = true;
        }
        private void weatherenabledCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.enable = weatherenabledCB.Checked == true ? 1 : 0;
            currentproject.weatherconfig.isDirty = true;
        }
        private void weatherrestartCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.reset = weatherrestartCB.Checked == true ? 1 : 0;
            currentproject.weatherconfig.isDirty = true;
        }
        private void RCactualNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.rain.current.actual = RCactualNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void RCtimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.rain.current.time = (int)RCtimeNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void RCdurationNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.rain.current.duration = (int)RCdurationNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void RLminNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.rain.limit.min = RLminNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void RLmaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.rain.limit.min = RLmaxNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void RTLminNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.rain.timelimits.min = (int)RTLminNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void RTLmaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.rain.timelimits.max = (int)RTLmaxNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void RCLminNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.rain.changelimits.min = RCLminNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void RCLmaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.rain.changelimits.max = RCLmaxNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void RTminNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.rain.thresholds.min = RTminNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void RTmaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.rain.thresholds.max = RTmaxNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void RTendNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.rain.thresholds.end = (int)RTendNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void OCactualNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.overcast.current.actual = OCactualNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void OCtimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.overcast.current.time = (int)OCtimeNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void OCdurationNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.overcast.current.duration = (int)OCdurationNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void OLminNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.overcast.limits.min = OLminNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void OLmaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.overcast.limits.max = OLmaxNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void OTLminNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.overcast.timelimits.min = (int)OTLminNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void OTLmaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.overcast.timelimits.max = (int)OTLmaxNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void OCLminNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.overcast.changelimits.min = OCLminNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void OCLmaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.overcast.changelimits.max = OCLmaxNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void WMaxSpeedNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.wind.maxspeed = (int)WMaxSpeedNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void WPminNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.wind.@params.min = WPminNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void WPmaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.wind.@params.max = WPmaxNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void WPfrequencyNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.wind.@params.frequency = (int)WPfrequencyNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void FCactualNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.fog.current.actual = FCactualNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void FCtimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.fog.current.time = (int)FCtimeNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void FCdurationNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.fog.current.duration = (int)FCdurationNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void FLminNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.fog.limits.min = FLminNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void FLmaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.fog.limits.max = FLmaxNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void FTLminNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.fog.timelimits.min = (int)FTLminNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void FTLmaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.fog.timelimits.max = (int)FTLmaxNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void FCLminNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.fog.changelimits.min = FCLminNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void FCLmaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.fog.changelimits.max = FCLmaxNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void SdensityNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.storm.density = SdensityNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void SthresholdNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.storm.threshold = SthresholdNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void StimeoutNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.storm.timeout = (int)StimeoutNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        #endregion weather
    }
}
