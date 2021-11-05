using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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

            PopulateTreeView();
            Loadevents();
            populateEconmyTreeview();
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
        private void tabControl4_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl4.SelectedIndex)
            {
                case 0:
                    toolStripButton5.Checked = false;
                    toolStripButton6.Checked = false;
                    break;
                case 1:
                    toolStripButton3.Checked = false;
                    toolStripButton6.Checked = false;
                    break;
                case 2:
                    toolStripButton3.Checked = false;
                    toolStripButton5.Checked = false;
                    break;
                default:
                    break;
            }
        }
        #region Types
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

        private void darkButton4_Click(object sender, EventArgs e)
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
                    currentproject.EconomyCore.AddCe(path.Replace(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\", ""), modname + "_types.xml", "types");
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
                    currentlootpart.AdduserTier(tier);
                else
                    currentlootpart.removeusertier(tier);
                currentTypesFile.isDirty = true;
            }
        }
        private void darkButton3_Click(object sender, EventArgs e)
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
        private DynamicEvent CurrentEvent;
        private child CurrentChild;
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
            currenteventsfile = EventsLIstLB.SelectedItem as eventscofig;
            positionComboBox.DataSource = Enum.GetValues(typeof(position));
            limitComboBox.DataSource = Enum.GetValues(typeof(limit));
            EventsLB.DisplayMember = "DisplayName";
            EventsLB.ValueMember = "Value";
            EventsLB.DataSource = currenteventsfile.events.DynamicEvent;
            isUserInteraction = true;
        }
        private void EventsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EventsLB.SelectedItem as DynamicEvent == CurrentEvent) { return; }
            if (EventsLB.SelectedIndex == -1) { return; }
            CurrentEvent = EventsLB.SelectedItem as DynamicEvent;

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
            ChildrenLB.DataSource = CurrentEvent.children.child;

            isUserInteraction = true;
        }
        private void ClearChildren()
        {
            ChildrenLB.DataSource = null;
            ChildGB.Visible = false;
        }
        private void ChildrenLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ChildrenLB.SelectedItem as child == CurrentChild) { return; }
            if (ChildrenLB.SelectedIndex == -1) { return; }
            CurrentChild = ChildrenLB.SelectedItem as child;
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

        #endregion events


    }
}
