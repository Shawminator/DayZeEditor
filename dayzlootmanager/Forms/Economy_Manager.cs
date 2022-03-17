using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.Xsl;
using DarkUI.Forms;
using DayZeLib;

namespace DayZeEditor
{
    public partial class Economy_Manager : DarkForm
    {
        public bool isUserInteraction = true;
        public Project currentproject { get; set; }
        public TypesFile vanillatypes;
        public List<TypesFile> ModTypes;
        public TypesFile currentTypesFile;
        public type currentlootpart;
        public string currentcollection;
        public string filename;
        public bool Fileloaded = false;
        public bool FullTypes;
        private int searchnum;
        private List<TreeNode> searchtreeNodes;
        private List<type> foundparts;

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
            tabControl4.ItemSize = new Size(0, 1);

            filename = currentproject.ProjectName;
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();
            comboBox1.DataSource = currentproject.limitfefinitions.lists.categories.category;
            comboBox2.DataSource = currentproject.limitfefinitions.lists.usageflags.usage;
            comboBox4.DataSource = currentproject.limitfefinitions.lists.tags.tag;

            comboBox8.DataSource = currentproject.limitfefinitions.lists.categories.category;
            comboBox7.DataSource = currentproject.limitfefinitions.lists.usageflags.usage;
            comboBox6.DataSource = currentproject.limitfefinitions.lists.tags.tag;

            PopulateTreeView();
            Loadevents();
            LoadSpawnableTypes();
            populateEconmyTreeview();

            SetSummarytiers();
            NomCountLabel.Text = "Total Nominal Count :- " + currentproject.TotalNomCount.ToString();
            isUserInteraction = true;
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
            tabControl4.SelectedIndex = 0;
            if (tabControl3.SelectedIndex == 0)
                toolStripButton3.Checked = true;
        }
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            tabControl4.SelectedIndex = 1;
            if (tabControl3.SelectedIndex == 1)
                toolStripButton5.Checked = true;
        }
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            tabControl4.SelectedIndex = 2;
            if (tabControl3.SelectedIndex == 2)
                toolStripButton6.Checked = true;
        }
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            tabControl4.SelectedIndex = 3;
            if (tabControl3.SelectedIndex== 3)
                toolStripButton8.Checked = true;
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            tabControl4.SelectedIndex = 4;
            if (tabControl3.SelectedIndex == 4)
                toolStripButton8.Checked = true;
        }
        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            tabControl4.SelectedIndex = 5;
            if (tabControl3.SelectedIndex == 5)
                toolStripButton8.Checked = true;
        }
        private void tabControl4_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl4.SelectedIndex)
            {
                case 0:
                    toolStripButton5.Checked = false;
                    toolStripButton6.Checked = false;
                    toolStripButton8.Checked = false;
                    toolStripButton9.Checked = false;
                    toolStripButton10.Checked = false;
                    break;
                case 1:
                    toolStripButton3.Checked = false;
                    toolStripButton6.Checked = false;
                    toolStripButton8.Checked = false;
                    toolStripButton9.Checked = false;
                    toolStripButton10.Checked = false;
                    break;
                case 2:
                    toolStripButton3.Checked = false;
                    toolStripButton5.Checked = false;
                    toolStripButton8.Checked = false;
                    toolStripButton9.Checked = false;
                    toolStripButton10.Checked = false;
                    break;
                case 3:
                    toolStripButton3.Checked = false;
                    toolStripButton5.Checked = false;
                    toolStripButton6.Checked = false;
                    toolStripButton9.Checked = false;
                    toolStripButton10.Checked = false;
                    break;
                case 4:
                    toolStripButton3.Checked = false;
                    toolStripButton5.Checked = false;
                    toolStripButton6.Checked = false;
                    toolStripButton8.Checked = false;
                    toolStripButton10.Checked = false;
                    break;
                case 5:
                    toolStripButton3.Checked = false;
                    toolStripButton6.Checked = false;
                    toolStripButton8.Checked = false;
                    toolStripButton9.Checked = false;
                    toolStripButton5.Checked = false;
                    break;
                default:
                    break;
            }
        }
        #region Types
        public void SetSummarytiers()
        {
            List<CheckBox> checkboxesSummary = SetdefinitionsTPSummary.Controls.OfType<CheckBox>().ToList();
            foreach (CheckBox cb in checkboxesSummary)
            {
                cb.Visible = false;
            }
            int index = 15;
            foreach (value value in currentproject.limitfefinitions.lists.valueflags.value)
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
            foreach (user user in currentproject.limitfefinitionsuser.user_lists.valueflags.user)
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

            int index = 14;
            foreach (value value in currentproject.limitfefinitions.lists.valueflags.value)
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
            foreach (user user in currentproject.limitfefinitionsuser.user_lists.valueflags.user)
            {
                CheckBox cb = checkboxes[index];
                cb.Tag = user.name;
                cb.Visible = true;
                cb.Checked = false;
                cb.Text = user.name;
                index++;
            }

            if (currentlootpart.Usinguserdifinitions)
            {
                tabControl3.SelectedIndex = 1;
            }
            else
                tabControl3.SelectedIndex = 0;
        }
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            Loot_Info f2 = new Loot_Info();
            f2.ShowDialog();
        }
        private void PopulateTreeView()
        {
            treeView1.Nodes.Clear();
            TreeNode root = new TreeNode(Path.GetFileName(filename))
            {
                Tag = "Parent"
            };
            //Set Vanilla Treenode types
            TreeNode vanilla = new TreeNode("Vanilla Types")
            {
                Tag = "VanillaTypes"
            };
            foreach (type type in vanillatypes.types.type)
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
                    foreach (type type in tf.types.type)
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

            treeView1.Nodes.Add(root);
        }
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag != null && e.Node.Tag is type)
            {
                TreeNode parent = e.Node.Parent;
                TreeNode mainparent = parent.Parent;
                currentcollection = parent.Text;
                String typesfile = mainparent.Text;
                if(typesfile == "Vanilla Types")
                    currentTypesFile = vanillatypes;
                else
                    currentTypesFile = ModTypes.FirstOrDefault(x => x.modname == typesfile);
                isUserInteraction = false;
                tabControl1.SelectedIndex = 1;
                currentlootpart = e.Node.Tag as type;
                PopulateLootPartInfo();
                isUserInteraction = true;
                if (e.Button == MouseButtons.Right)
                {
                    // Display context menu for eg:
                    DeleteTypesTSMI.Visible = false;
                    AddTypesTSMI.Visible = false;
                    DeleteSpecificTypeTSMI.Visible = true;
                    DeleteSpecificTypeTSMI.Text = "Delete " + currentlootpart.name + " from " + currentcollection + " in " + typesfile;
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
                        AddTypesTSMI.Text = "Add new Types to Custom Folder";
                        TypesContextMenu.Show(Cursor.Position);
                    }
                }
            }
            this.treeView1.SelectedNode = e.Node;
        }
        private void DeleteSpecificTypeTSMI_Click(object sender, EventArgs e)
        {
            currentTypesFile.types.type.Remove(currentlootpart);
            currentTypesFile.SaveTyes(DateTime.Now.ToString("ddMMyy_HHmm"));
            var savedExpansionState = treeView1.Nodes.GetExpansionState();
            treeView1.BeginUpdate();
            PopulateTreeView();
            treeView1.Nodes.SetExpansionState(savedExpansionState);
            treeView1.EndUpdate();
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
                    File.Delete(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\" + foflderpath + "\\" + filename);
                    if (deletedirectory)
                        Directory.Delete(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\" + foflderpath, true);
                    currentproject.EconomyCore.SaveEconomycore();
                    currentproject.removeMod(currentTypesFile.modname);
                    ModTypes = currentproject.getModList();

                    var savedExpansionState = treeView1.Nodes.GetExpansionState();
                    treeView1.BeginUpdate();
                    PopulateTreeView();
                    treeView1.Nodes.SetExpansionState(savedExpansionState);
                    treeView1.EndUpdate();
                    MessageBox.Show("Mod Removed from Project....", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            else
            {
                List<type> typetoremove = new List<type>();
                foreach (type type in currentTypesFile.types.type)
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
                foreach(type t in typetoremove)
                {
                    currentTypesFile.types.type.Remove(t);
                }
                currentTypesFile.SaveTyes(DateTime.Now.ToString("ddMMyy_HHmm"));
                var savedExpansionState = treeView1.Nodes.GetExpansionState();
                treeView1.BeginUpdate();
                PopulateTreeView();
                treeView1.Nodes.SetExpansionState(savedExpansionState);
                treeView1.EndUpdate();
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
                    var savedExpansionState = treeView1.Nodes.GetExpansionState();
                    treeView1.BeginUpdate();
                    PopulateTreeView();
                    treeView1.Nodes.SetExpansionState(savedExpansionState);
                    treeView1.EndUpdate();
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
                    foreach (type type in test.types.type)
                    {
                        currentTypesFile.types.type.Add(type);
                    }
                    currentTypesFile.SaveTyes(DateTime.Now.ToString("ddMMyy_HHmm"));
                    var savedExpansionState = treeView1.Nodes.GetExpansionState();
                    treeView1.BeginUpdate();
                    PopulateTreeView();
                    treeView1.Nodes.SetExpansionState(savedExpansionState);
                    treeView1.EndUpdate();
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
                    if (currentlootpart.Usinguserdifinitions)
                        try
                        {
                            UserdefinitionsTP.Controls.OfType<CheckBox>().First(x => x.Tag.ToString() == currentlootpart.value[i].user).Checked = true;
                        }
                        catch
                        {
                            currentlootpart.value.RemoveAt(i);
                            i--;
                        }
                            
                    else
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
        private void PopulateCounts()
        {
            if (currentlootpart.nominal == null)
            {
                numericUpDown1.Visible = false;
            }
            else
            {
                numericUpDown1.Visible = true;
                numericUpDown1.Value = (decimal)currentlootpart.nominal;
            }
            if (currentlootpart.min == null)
            {
                numericUpDown2.Visible = false;
            }
            else
            {
                numericUpDown2.Visible = true;
                numericUpDown2.Value = (decimal)currentlootpart.min;
            }
            if (currentlootpart.lifetime == null)
            {
                numericUpDown3.Visible = false;
            }
            else
            {
                numericUpDown3.Visible = true;
                numericUpDown3.Value = (decimal)currentlootpart.lifetime;
            }
            if (currentlootpart.restock == null)
            {
                numericUpDown4.Visible = false;
            }
            else
            {
                numericUpDown4.Visible = true;
                numericUpDown4.Value = (decimal)currentlootpart.restock;
            }
            if (currentlootpart.quantmin == null)
            {
                numericUpDown5.Visible = false;
            }
            else
            {
                numericUpDown5.Visible = true;
                numericUpDown5.Value = (decimal)currentlootpart.quantmin;
            }
            if (currentlootpart.quantmax == null)
            {
                numericUpDown6.Visible = false;
            }
            else
            {
                numericUpDown6.Visible = true;
                numericUpDown6.Value = (decimal)currentlootpart.quantmax;
            }
            if (currentlootpart.cost == null)
            {
                numericUpDown7.Visible = false;
            }
            else
            {
                numericUpDown7.Visible = true;
                numericUpDown7.Value = (decimal)currentlootpart.cost;
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
            usage u = comboBox2.SelectedItem as usage;
            currentlootpart.AddnewUsage(u);
            populateUsage();
            currentTypesFile.isDirty = true;
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            usage u = listBox1.SelectedItem as usage;
            currentlootpart.removeusage(u);
            currentTypesFile.isDirty = true;
        }
        private void darkButton8_Click(object sender, EventArgs e)
        {
            tag t = comboBox4.SelectedItem as tag;
            currentlootpart.Addnewtag(t);
            currentTypesFile.isDirty = true;
            listBox2.Invalidate();
        }
        private void darkButton7_Click(object sender, EventArgs e)
        {
            tag t = listBox2.SelectedItem as tag;
            currentlootpart.removetag(t);
            currentTypesFile.isDirty = true;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                if(currentlootpart == null) { return; }
                category c = comboBox1.SelectedItem as category;
                currentlootpart.changecategory(c);
                currentTypesFile.isDirty = true;
                PopulateTreeView();
                isUserInteraction = false;
            }
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                if (currentlootpart.nominal != null)
                    currentlootpart.nominal = (int)numericUpDown1.Value;
                if (currentlootpart.min != null)
                    currentlootpart.min = (int)numericUpDown2.Value;
                if (currentlootpart.lifetime != null)
                    currentlootpart.lifetime = (int)numericUpDown3.Value;
                if (currentlootpart.restock != null)
                    currentlootpart.restock = (int)numericUpDown4.Value;
                if (currentlootpart.quantmin != null)
                    currentlootpart.quantmin = (int)numericUpDown5.Value;
                if (currentlootpart.quantmax != null)
                    currentlootpart.quantmax = (int)numericUpDown6.Value;
                if (currentlootpart.cost != null)
                    currentlootpart.cost = (int)numericUpDown7.Value;
                currentTypesFile.isDirty = true;
                currentproject.SetTotNomCount();
                NomCountLabel.Text = "Total Nominal Count :- " + currentproject.TotalNomCount.ToString();
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                if (currentlootpart.flags == null) return;
                currentlootpart.flags.count_in_cargo = checkBox1.Checked == true ? 1 : 0;
                currentlootpart.flags.count_in_hoarder = checkBox2.Checked == true ? 1 : 0;
                currentlootpart.flags.count_in_map = checkBox3.Checked == true ? 1 : 0;
                currentlootpart.flags.count_in_player = checkBox4.Checked == true ? 1 : 0;
                currentlootpart.flags.crafted = checkBox5.Checked == true ? 1 : 0;
                currentlootpart.flags.deloot = checkBox6.Checked == true ? 1 : 0;
                currentTypesFile.isDirty = true;
            }
        }
        private void TierCheckBoxchanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                CheckBox cb = sender as CheckBox;
                string tier = cb.Tag.ToString();
                if (cb.Checked)
                    currentlootpart.AddTier(tier);
                else
                    currentlootpart.removetier(tier);
                PopulateLootPartInfo();
                currentTypesFile.isDirty = true;
            }
        }
        private void UserdefiniedTiersChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                CheckBox cb = sender as CheckBox;
                string tier = cb.Tag.ToString();
                if (cb.Checked)
                {
                    if (currentlootpart.value == null)
                        currentlootpart.value = new BindingList<value>();
                    //lets check if the defined tier are set, if so we remove them
                    if (currentlootpart.value.Count > 0)
                    {
                        int count = currentlootpart.value.Count;
                        for (int i = 0; i < count; i++)
                        {
                            currentlootpart.removetier(currentlootpart.value[0].name);
                        }
                    }
                    currentlootpart.AdduserTier(tier);
                }
                else
                    currentlootpart.removeusertier(tier);
                PopulateLootPartInfo();
                currentTypesFile.isDirty = true;
            }
        }
        private void darkButton3_Click(object sender, EventArgs e)
        {
            List<type> loot = new List<type>();

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
        public void editnommax(List<type> loot)
        {
            currentTypesFile.isDirty = true;
            Domultiplier(loot);
        }
        private void Domultiplier(List<type> list)
        {
            foreach (type item in list)
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
            List<type> loot = new List<type>();
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
        private void SetFlags(Button SB, List<type> list)
        {
            switch (SB.Name)
            {
                case "CargoButton":
                    foreach (type item in list)
                    {
                        item.flags.count_in_cargo = checkBox17.Checked == true ? 1 : 0;
                    }
                    break;
                case "HoarderButton":
                    foreach (type item in list)
                    {
                        item.flags.count_in_hoarder = checkBox16.Checked == true ? 1 : 0;
                    }
                    break;
                case "MapButton":
                    foreach (type item in list)
                    {
                        item.flags.count_in_map = checkBox15.Checked == true ? 1 : 0;
                    }
                    break;
                case "PlayerButton":
                    foreach (type item in list)
                    {
                        item.flags.count_in_player = checkBox14.Checked == true ? 1 : 0;
                    }
                    break;
                case "CraftedButton":
                    foreach (type item in list)
                    {
                        item.flags.crafted = checkBox9.Checked == true ? 1 : 0;
                    }
                    break;
                case "DelootButton":
                    foreach (type item in list)
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
            List<type> loot = new List<type>();
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
        private void DoSyncMintoNom(List<type> list)
        {
            foreach (type item in list)
            {
                item.min = item.nominal;
            }
        }
        private void darkButton6_Click_1(object sender, EventArgs e)
        {
            List<type> loot = new List<type>();
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
        private void DoSyncNomtoMin(List<type> list)
        {
            foreach (type item in list)
            {
                item.nominal = item.min;
            }
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (treeView1.Nodes.Count < 1)
                return;
            string text = toolStripTextBox1.Text;
            searchnum = 0;
            searchtreeNodes = new List<TreeNode>();
            foundparts = new List<type>();
            foreach (type type in vanillatypes.types.type)
            {
                if (type.name.ToLower().Contains(text.ToLower()))
                {
                    foundparts.Add(type);
                }
            }
            foreach (TypesFile tf in ModTypes)
            {
                foreach (type type in tf.types.type)
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
            foreach (type obj in foundparts)
            {
                foreach (TreeNode node in treeView1.Nodes)
                {
                    searchtree(obj.name.ToLower(), node, searchtreeNodes);
                }
            }
            treeView1.SelectedNode = searchtreeNodes[searchnum];
            treeView1.Focus();
            if (treeView1.SelectedNode.Tag != null && treeView1.SelectedNode.Tag is type)
            {
                isUserInteraction = false;
                tabControl1.SelectedIndex = 1;
                currentlootpart = treeView1.SelectedNode.Tag as type;
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
            treeView1.SelectedNode = searchtreeNodes[searchnum];
            treeView1.Focus();
            if (treeView1.SelectedNode.Tag != null && treeView1.SelectedNode.Tag is type)
            {
                isUserInteraction = false;
                tabControl1.SelectedIndex = 1;
                currentlootpart = treeView1.SelectedNode.Tag as type;
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
            if(currentspawnabletypesfile.spawnabletypes.damage != null)
            {
                DamageMinNUD.Value = currentspawnabletypesfile.spawnabletypes.damage.min;
                DamageMaxNUD.Value = currentspawnabletypesfile.spawnabletypes.damage.max;
                DamageMinNUD.Visible = true;
                DamageMaxNUD.Visible = true;
            }
            else
            {
                DamageMinNUD.Visible = false;
                DamageMaxNUD.Visible = false;
            }

            SpawnabletpesLB.DisplayMember = "DisplayName";
            SpawnabletpesLB.ValueMember = "Value";
            SpawnabletpesLB.DataSource = currentspawnabletypesfile.spawnabletypes.type;
            isUserInteraction = true;
        }
        private void SpawnabletpesLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SpawnabletpesLB.SelectedItem as spawnabletypesType == CurrentspawnabletypesType) { return; }
            if (SpawnabletpesLB.SelectedIndex == -1) { return; }
            CurrentspawnabletypesType = SpawnabletpesLB.SelectedItem as spawnabletypesType;

            listBox6.DisplayMember = "DisplayName";
            listBox6.ValueMember = "Value";
            listBox6.DataSource = CurrentspawnabletypesType.Items;


        }
        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox6.SelectedItem as object == CurrentspawnabletypesTypetype) { return; }
            if (listBox6.SelectedIndex == -1) { return; }
            CurrentspawnabletypesTypetype = listBox6.SelectedItem as object;
            HoarderCB.Checked = false;
            checkBox49.Checked = false;
            checkBox50.Checked = false;
            checkBox51.Checked = false;
            if (CurrentspawnabletypesTypetype is spawnabletypesTypeHoarder)
            {
                HoarderCB.Checked = true;
            }
            else if (CurrentspawnabletypesTypetype is spawnabletypesTypeTag)
            {
                checkBox51.Checked = true;
            }
            else if (CurrentspawnabletypesTypetype is spawnabletypesTypeCargo)
            {
                checkBox49.Checked = true;
            }
            else if(CurrentspawnabletypesTypetype is spawnabletypesTypeAttachments)
            {
                checkBox50.Checked = true;
            }
        }
        #endregion spawnabletypes


        public bool FilterTiers = false;
        public bool FilterCategories = false;
        public bool FilterLocations = false;
        public bool FilterTags = false;
        public bool FilterFlags = false;
        private void darkButton15_Click(object sender, EventArgs e)
        {
            treeView2.Nodes.Clear();
        }
        public bool typeinfilterlist(type type, List<string> Queryitems)
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
                                if (!type.value.Any(x => x.user == def))
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
                        category c = item as category;
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
                        usage u = item as usage;
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
                        tag c = item as tag;
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
            string stop = "";







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
            foreach (type type in vanillatypes.types.type)
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
                    foreach (type type in tf.types.type)
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
            category c = comboBox8.SelectedItem as category;
            if(!listBox5.Items.Contains(c))
                listBox5.Items.Add(c);
        }
        private void darkButton13_Click(object sender, EventArgs e)
        {
            if (listBox5.SelectedItems.Count > 0)
            {
                category c = listBox5.SelectedItem as category;
                listBox5.Items.Remove(c);
            }
        }
        private void darkButton10_Click(object sender, EventArgs e)
        {
            usage u = comboBox7.SelectedItem as usage;
            if (!listBox4.Items.Contains(u))
                listBox4.Items.Add(u);
        }
        private void darkButton11_Click(object sender, EventArgs e)
        {
            if (listBox4.SelectedItems.Count > 0)
            {
                usage u = listBox4.SelectedItem as usage;
                listBox4.Items.Remove(u);
            }
        }
        private void darkButton4_Click(object sender, EventArgs e)
        {
            tag t = comboBox6.SelectedItem as tag;
            if (!listBox3.Items.Contains(t))
                listBox3.Items.Add(t);
        }
        private void darkButton9_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedItems.Count > 0)
            {
                tag t = listBox3.SelectedItem as tag;
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

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
