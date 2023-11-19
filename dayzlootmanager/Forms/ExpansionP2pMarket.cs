using DayZeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZeEditor
{
    public partial class ExpansionP2pMarket : Form
    {
        public Project currentproject { get; internal set; }
        public TypesFile vanillatypes;
        public TypesFile Expansiontypes;
        public List<TypesFile> ModTypes;
        public BindingList<AILoadouts> LoadoutList { get; set; }
        public string AILoadoutsPath { get; private set; }
        public BindingList<string> Factions { get; private set; }
        public string P2PSettingsPath { get; private set; }
        public P2PMarketSettings P2PMarketSettings { get; private set; }
        public string P2PMarketPath { get; set; }
        public P2PMarketList P2PMarketList { get; set; }
        private bool needtosave;
        private bool useraction;
        private p2pmarket currentp2pmarket;

        public ExpansionP2pMarket()
        {
            InitializeComponent();
        }
        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
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
        private void ExpansionP2pMarket_Load(object sender, EventArgs e)
        {
            tabControl1.ItemSize = new Size(0, 1);

            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            Factions = new BindingList<string>(File.ReadAllLines(Application.StartupPath + "\\TraderNPCs\\Factions.txt").ToList());
            List<string> questrequiredfaction = new List<string>();
            questrequiredfaction.Add("");
            foreach (string rf in Factions)
            {
                questrequiredfaction.Add(rf);
            }
            m_FactionCB.DataSource = new BindingList<string>(questrequiredfaction);

            LoadoutList = new BindingList<AILoadouts>();
            AILoadoutsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Loadouts";
            DirectoryInfo dinfo = new DirectoryInfo(AILoadoutsPath);
            FileInfo[] Files = dinfo.GetFiles("*.json");
            foreach (FileInfo file in Files)
            {
                try
                {
                    Console.WriteLine("serializing " + Path.GetFileName(file.FullName));
                    AILoadouts AILoadouts = JsonSerializer.Deserialize<AILoadouts>(File.ReadAllText(file.FullName));
                    AILoadouts.Filename = file.FullName;
                    AILoadouts.Setname();
                    AILoadouts.isDirty = false;
                    LoadoutList.Add(AILoadouts);
                }
                catch { }
            }
            m_LoadoutFileCB.DataSource = LoadoutList;
            P2PSettingsPath = currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\settings\\P2PMarketSettings.json";
            if (!Directory.Exists(Path.GetDirectoryName(P2PSettingsPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(P2PSettingsPath));
            if (!File.Exists(P2PSettingsPath))
            {
                P2PMarketSettings = new P2PMarketSettings(P2PSettingsPath);
                P2PMarketSettings.DefaultMenuCategories();
                P2PMarketSettings.DefaultExcludedClassNames();
                needtosave = true;
            }
            else
            {
                Console.WriteLine("Serializing " + P2PSettingsPath);
                P2PMarketSettings = JsonSerializer.Deserialize<P2PMarketSettings>(File.ReadAllText(P2PSettingsPath));
                P2PMarketSettings.isDirty = false;
                if (P2PMarketSettings.checkver())
                    needtosave = true;
            }
            P2PMarketSettings.Filename = P2PSettingsPath;
            LoadP2PSetting();

            Console.WriteLine("Loading P2P Market files....");
            P2PMarketPath = currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\p2pmarket";
            P2PMarketList = new P2PMarketList(P2PMarketPath);
            LoadP2PMarket();


            if (needtosave)
            {
                savefiles(true);
            }
        }
        private void ExpansionP2pMarket_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool needtosave = false;
            foreach (p2pmarket p2pm in P2PMarketList.p2pmarketList)
            {
                if (p2pm.isDirty)
                {
                    needtosave = true;
                }
            }
            if (P2PMarketSettings.isDirty)
                needtosave = true;
            
            if (needtosave)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    savefiles();
                }
            }
        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            savefiles();
        }
        public void savefiles(bool updated = false)
        {
            if (!Directory.Exists(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Settings"))
                Directory.CreateDirectory(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Settings");
            if (!Directory.Exists(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\settings"))
                Directory.CreateDirectory(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\settings");
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");

            if (P2PMarketSettings.isDirty)
            {
                P2PMarketSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(P2PMarketSettings, options);
                if (currentproject.Createbackups && File.Exists(P2PMarketSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(P2PMarketSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(P2PMarketSettings.Filename, Path.GetDirectoryName(P2PMarketSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(P2PMarketSettings.Filename) + ".bak", true);
                }
                File.WriteAllText(P2PMarketSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(P2PMarketSettings.Filename));
            }
            foreach(p2pmarket p2pmarket in P2PMarketList.p2pmarketList)
            {
                if(p2pmarket.isDirty)
                {
                    p2pmarket.isDirty = false;
                    var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                    string jsonString = JsonSerializer.Serialize(p2pmarket, options);
                    if (currentproject.Createbackups && File.Exists(p2pmarket.Filename))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(p2pmarket.Filename) + "\\Backup\\" + SaveTime);
                        File.Copy(p2pmarket.Filename, Path.GetDirectoryName(p2pmarket.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(p2pmarket.Filename) + ".bak", true);
                    }
                    File.WriteAllText(p2pmarket.Filename, jsonString);
                    midifiedfiles.Add(Path.GetFileName(p2pmarket.Filename));
                }
            }




            bool RemovedOnly = true;
            string message = "The Following Files were saved....\n";
            if (updated)
            {
                message = "The following files were either Created or Updated...\n";
            }
            int i = 0;
            foreach (string l in midifiedfiles)
            {
                RemovedOnly = false;
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
            if (RemovedOnly)
            {
                message = "";
            }
            else
            {
                message += "\n";
            }
            if (P2PMarketList.Markedfordelete != null)
            {
                message += "The following Personal Storage configs were Removed\n";
                i = 0;
                foreach (p2pmarket del in P2PMarketList.Markedfordelete)
                {
                    del.backupandDelete(P2PMarketPath);
                    midifiedfiles.Add(del.Filename);
                    if (i == 5)
                    {
                        message += del.Filename + "\n";
                        i = 0;
                    }
                    else
                    {
                        message += del.Filename + ", ";
                        i++;
                    }
                    P2PMarketList.UsedIDS.Remove(del.m_TraderID);
                }
                P2PMarketList.Markedfordelete = null;
            }
            if (midifiedfiles.Count > 0)
                MessageBox.Show(message, "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            else
                MessageBox.Show("No changes were made.", "Nothing Saved", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            toolStripButton8.Checked = true;
        }
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex =1;
            toolStripButton7.Checked = true;
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripButton8.Checked = false;
            toolStripButton7.Checked = false;
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            P2PMarketInfo pi = new P2PMarketInfo();
            pi.ShowDialog();
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    Process.Start(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\settings");
                    break;
                case 1:
                    Process.Start(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\p2pmarket");
                    break;
            }
        }
        #region p2psettings
        private void LoadP2PSetting()
        {
            useraction = false;
            Loadtreeview();
            checkBox1.Checked = P2PMarketSettings.Enabled == 1 ? true : false;
            numericUpDown1.Value = P2PMarketSettings.MaxListingTime;
            numericUpDown2.Value = P2PMarketSettings.MaxListings;
            numericUpDown3.Value = P2PMarketSettings.ListingOwnerDiscountPercent;
            numericUpDown4.Value = P2PMarketSettings.ListingPricePercent;
            numericUpDown5.Value = P2PMarketSettings.SalesDepositTime;
            listBox1.DataSource = P2PMarketSettings.ExcludedClassNames;
            useraction = true;
        }
        private void Loadtreeview()
        {
            treeViewMS1.Nodes.Clear();
            TreeNode root = new TreeNode(Path.GetFileNameWithoutExtension(P2PMarketSettings.Filename))
            {
                Tag = "Parent"
            };
            TreeNode MenuCategories = new TreeNode("MenuCategories")
            {
                Tag = "MenuCategoriesParent"
            };
            foreach (ExpansionMenuCategory mc in P2PMarketSettings.MenuCategories)
            {
                TreeNode mctreenode = new TreeNode(mc.DisplayName)
                {
                    Tag = mc
                };
                mctreenode.Nodes.AddRange(new TreeNode[]
                {
                        new TreeNode("IconPath")
                        {
                            Tag = "MCIconPath"
                        },
                        new TreeNode("Included")
                        {
                            Tag = "MCIncluded"
                        },
                        new TreeNode("Excluded")
                        {
                            Tag = "MCExcluded"
                        }
                });
                TreeNode mcsctreenode = new TreeNode("SubCategories")
                {
                    Tag = "SubCategoriesParent"
                };
                foreach (ExpansionMenuSubCategory tctn in mc.SubCategories)
                {
                    TreeNode sctreenode = new TreeNode(tctn.DisplayName)
                    {
                        Tag = tctn
                    };
                    sctreenode.Nodes.AddRange(new TreeNode[]
                    {
                        new TreeNode("IconPath")
                        {
                            Tag = "SCIconPath"
                        },
                        new TreeNode("Included")
                        {
                            Tag = "SCIncluded"
                        },
                        new TreeNode("Excluded")
                        {
                            Tag = "SCExcluded"
                        }
                    });
                    mcsctreenode.Nodes.Add(sctreenode);
                }
                mctreenode.Nodes.Add(mcsctreenode);
                MenuCategories.Nodes.Add(mctreenode);
            }
            root.Nodes.Add(MenuCategories);
            treeViewMS1.Nodes.Add(root);
        }
        public ExpansionMenuCategory CurrentMenucategory;
        public ExpansionMenuSubCategory CurrentSubCategory;
        private void treeViewMS1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            useraction = false;
            treeViewMS1.SelectedNode = e.Node;
            if (e.Node.Tag is string)
            {
                switch (e.Node.Tag.ToString())
                {
                    case "Parent":
                        groupBox2.Visible = false;
                        groupBox1.Visible = false;
                        CurrentMenucategory = null;
                        CurrentSubCategory = null;
                        break;
                    case "MenuCategoriesParent":
                        groupBox2.Visible = false;
                        groupBox1.Visible = false;
                        CurrentMenucategory = null;
                        CurrentSubCategory = null;
                        if (e.Button == MouseButtons.Right)
                        {
                            addNewSubMenuCategoryToolStripMenuItem.Visible = false;
                            addNewMenuCategoryToolStripMenuItem.Visible = true;
                            removeMenuCategoryToolStripMenuItem.Visible = false;
                            removeSubMenuCategoryToolStripMenuItem.Visible = false;
                            contextMenuStrip1.Show(Cursor.Position);
                        }
                        break;
                    case "SubCategoriesParent":
                        groupBox2.Visible = false;
                        groupBox1.Visible = false;
                        CurrentMenucategory = e.Node.Parent.Tag as ExpansionMenuCategory;
                        CurrentSubCategory = null;
                        if (e.Button == MouseButtons.Right)
                        {
                            addNewSubMenuCategoryToolStripMenuItem.Visible = true;
                            addNewMenuCategoryToolStripMenuItem.Visible = false;
                            removeMenuCategoryToolStripMenuItem.Visible = false;
                            removeSubMenuCategoryToolStripMenuItem.Visible = false;
                            contextMenuStrip1.Show(Cursor.Position);
                        }
                        break;
                    case "MCIncluded":
                        groupBox2.Visible = false;
                        groupBox1.Visible = true;
                        CurrentMenucategory = e.Node.Parent.Tag as ExpansionMenuCategory;
                        CurrentSubCategory = null;
                        listBox2.DataSource = CurrentMenucategory.Included;
                        break;
                    case "MCExcluded":
                        groupBox2.Visible = false;
                        groupBox1.Visible = true;
                        CurrentMenucategory = e.Node.Parent.Tag as ExpansionMenuCategory;
                        CurrentSubCategory = null;
                        listBox2.DataSource = CurrentMenucategory.Excluded;
                        break;
                    case "MCIconPath":
                        groupBox2.Visible = true;
                        groupBox1.Visible = false;
                        CurrentMenucategory = e.Node.Parent.Tag as ExpansionMenuCategory;
                        CurrentSubCategory = null;
                        textBox1.Text = CurrentMenucategory.IconPath;
                        break;
                    case "SCIncluded":
                        groupBox2.Visible = false;
                        groupBox1.Visible = true;
                        CurrentMenucategory = null;
                        CurrentSubCategory = e.Node.Parent.Tag as ExpansionMenuSubCategory;
                        listBox2.DataSource = CurrentSubCategory.Included;
                        break;
                    case "SCExcluded":
                        groupBox2.Visible = false;
                        groupBox1.Visible = true;
                        CurrentMenucategory = null;
                        CurrentSubCategory = e.Node.Parent.Tag as ExpansionMenuSubCategory;
                        listBox2.DataSource = CurrentSubCategory.Excluded;
                        break;
                    case "SCIconPath":
                        groupBox2.Visible = true;
                        groupBox1.Visible = false;
                        CurrentMenucategory = null;
                        CurrentSubCategory = e.Node.Parent.Tag as ExpansionMenuSubCategory;
                        textBox1.Text = CurrentSubCategory.IconPath;
                        break;
                }
            }
            else if (e.Node.Tag is ExpansionMenuCategory)
            {
                groupBox2.Visible = true;
                groupBox1.Visible = false;
                CurrentMenucategory = e.Node.Tag as ExpansionMenuCategory;
                textBox1.Text = CurrentMenucategory.DisplayName;
                if (e.Button == MouseButtons.Right)
                {
                    addNewSubMenuCategoryToolStripMenuItem.Visible = false;
                    addNewMenuCategoryToolStripMenuItem.Visible = false;
                    removeMenuCategoryToolStripMenuItem.Visible = true;
                    removeSubMenuCategoryToolStripMenuItem.Visible = false;
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
            else if (e.Node.Tag is ExpansionMenuSubCategory)
            {
                groupBox2.Visible = true;
                groupBox1.Visible = false;
                CurrentSubCategory = e.Node.Tag as ExpansionMenuSubCategory;
                CurrentMenucategory = e.Node.Parent.Parent.Tag as ExpansionMenuCategory;
                textBox1.Text = CurrentSubCategory.DisplayName;
                if (e.Button == MouseButtons.Right)
                {
                    addNewSubMenuCategoryToolStripMenuItem.Visible = false;
                    addNewMenuCategoryToolStripMenuItem.Visible = false;
                    removeMenuCategoryToolStripMenuItem.Visible = false;
                    removeSubMenuCategoryToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
            useraction = true;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (treeViewMS1.SelectedNode.Parent.Tag is ExpansionMenuCategory)
            {
                CurrentMenucategory.IconPath = textBox1.Text;
            }
            else if (treeViewMS1.SelectedNode.Tag is ExpansionMenuCategory)
            {
                CurrentMenucategory.DisplayName = textBox1.Text;
                treeViewMS1.SelectedNode.Text = CurrentMenucategory.DisplayName;

            }
            else if (treeViewMS1.SelectedNode.Parent.Tag is ExpansionMenuSubCategory)
            {
                CurrentSubCategory.IconPath = textBox1.Text;
            }
            else if (treeViewMS1.SelectedNode.Tag is ExpansionMenuSubCategory)
            {
                CurrentSubCategory.DisplayName = textBox1.Text;
                treeViewMS1.SelectedNode.Text = CurrentSubCategory.DisplayName;
            }
            P2PMarketSettings.isDirty = true;
        }
        private void darkButton4_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseMultipleofSameItem = false
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    switch (treeViewMS1.SelectedNode.Tag.ToString())
                    {
                        case "MCIncluded":
                            if (!CurrentMenucategory.Included.Contains(l))
                                CurrentMenucategory.Included.Add(l);
                            break;
                        case "MCExcluded":
                            if (!CurrentMenucategory.Excluded.Contains(l))
                                CurrentMenucategory.Excluded.Add(l);
                            break;
                        case "SCIncluded":
                            if (!CurrentSubCategory.Included.Contains(l))
                                CurrentSubCategory.Included.Add(l);
                            break;
                        case "SCExcluded":
                            if (!CurrentSubCategory.Excluded.Contains(l))
                                CurrentSubCategory.Excluded.Add(l);
                            break;
                    }
                    P2PMarketSettings.isDirty = true;
                }
            }
        }
        private void darkButton28_Click(object sender, EventArgs e)
        {
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    switch (treeViewMS1.SelectedNode.Tag.ToString())
                    {
                        case "MCIncluded":
                            if (!CurrentMenucategory.Included.Contains(l))
                                CurrentMenucategory.Included.Add(l);
                            break;
                        case "MCExcluded":
                            if (!CurrentMenucategory.Excluded.Contains(l))
                                CurrentMenucategory.Excluded.Add(l);
                            break;
                        case "SCIncluded":
                            if (!CurrentSubCategory.Included.Contains(l))
                                CurrentSubCategory.Included.Add(l);
                            break;
                        case "SCExcluded":
                            if (!CurrentSubCategory.Excluded.Contains(l))
                                CurrentSubCategory.Excluded.Add(l);
                            break;
                    }
                    P2PMarketSettings.isDirty = true;
                }
            }
        }
        private void darkButton3_Click(object sender, EventArgs e)
        {
            switch (treeViewMS1.SelectedNode.Tag.ToString())
            {
                case "MCIncluded":
                    CurrentMenucategory.Included.Remove(listBox2.GetItemText(listBox2.SelectedItem));
                    break;
                case "MCExcluded":
                    CurrentMenucategory.Excluded.Remove(listBox2.GetItemText(listBox2.SelectedItem));
                    break;
                case "SCIncluded":
                    CurrentSubCategory.Included.Remove(listBox2.GetItemText(listBox2.SelectedItem));
                    break;
                case "SCExcluded":
                    CurrentSubCategory.Excluded.Remove(listBox2.GetItemText(listBox2.SelectedItem));
                    break;
            }
            P2PMarketSettings.isDirty = true;
        }
        private void addNewMenuCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExpansionMenuCategory bewmc = new ExpansionMenuCategory();
            TreeNode mctreenode = new TreeNode(bewmc.DisplayName)
            {
                Tag = bewmc
            };
            mctreenode.Nodes.AddRange(new TreeNode[]
                {
                        new TreeNode("IconPath")
                        {
                            Tag = "MCIconPath"
                        },
                        new TreeNode("Included")
                        {
                            Tag = "MCIncluded"
                        },
                        new TreeNode("Excluded")
                        {
                            Tag = "MCExcluded"
                        }
                });
            TreeNode mcsctreenode = new TreeNode("SubCategories")
            {
                Tag = "SubCategoriesParent"
            };
            mctreenode.Nodes.Add(mcsctreenode);
            P2PMarketSettings.MenuCategories.Add(bewmc);
            treeViewMS1.SelectedNode.Nodes.Add(mctreenode);
            P2PMarketSettings.isDirty = true;
        }
        private void addNewSubMenuCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExpansionMenuSubCategory bewmc = new ExpansionMenuSubCategory();
            TreeNode mctreenode = new TreeNode(bewmc.DisplayName)
            {
                Tag = bewmc
            };
            mctreenode.Nodes.AddRange(new TreeNode[]
                {
                        new TreeNode("IconPath")
                        {
                            Tag = "SCIconPath"
                        },
                        new TreeNode("Included")
                        {
                            Tag = "SCIncluded"
                        },
                        new TreeNode("Excluded")
                        {
                            Tag = "SCExcluded"
                        }
                });
            CurrentMenucategory.SubCategories.Add(bewmc);
            treeViewMS1.SelectedNode.Nodes.Add(mctreenode);
            P2PMarketSettings.isDirty = true;

        }
        private void removeMenuCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            P2PMarketSettings.MenuCategories.Remove(CurrentMenucategory);
            treeViewMS1.SelectedNode.Remove();
            P2PMarketSettings.isDirty = true;
        }
        private void removeSubMenuCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentMenucategory.SubCategories.Remove(CurrentSubCategory);
            treeViewMS1.SelectedNode.Remove();
            P2PMarketSettings.isDirty = true;
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseMultipleofSameItem = false
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (!P2PMarketSettings.ExcludedClassNames.Contains(l))
                        P2PMarketSettings.ExcludedClassNames.Add(l);
                }
                P2PMarketSettings.isDirty = true;
            }
        }
        private void darkButton5_Click(object sender, EventArgs e)
        {
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (!P2PMarketSettings.ExcludedClassNames.Contains(l))
                        P2PMarketSettings.ExcludedClassNames.Add(l);
                }
                P2PMarketSettings.isDirty = true;
            }
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
            P2PMarketSettings.ExcludedClassNames.Remove(listBox1.GetItemText(listBox1.SelectedItem));
            P2PMarketSettings.isDirty = true;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            P2PMarketSettings.Enabled = checkBox1.Checked == true ? 1 : 0;
            P2PMarketSettings.isDirty = true;
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            P2PMarketSettings.MaxListingTime = (int)numericUpDown1.Value;
            P2PMarketSettings.isDirty = true;
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            P2PMarketSettings.MaxListings = (int)numericUpDown2.Value;
            P2PMarketSettings.isDirty = true;
        }
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            P2PMarketSettings.ListingOwnerDiscountPercent = (int)numericUpDown3.Value;
            P2PMarketSettings.isDirty = true;
        }
        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            P2PMarketSettings.ListingPricePercent = (int)numericUpDown4.Value;
            P2PMarketSettings.isDirty = true;
        }
        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            P2PMarketSettings.SalesDepositTime = (int)numericUpDown5.Value;
            P2PMarketSettings.isDirty = true;
        }
        #endregion p2psettings

        #region p2parket
        private void LoadP2PMarket()
        {
            useraction = false;
            P2PMarketLB.DataSource = P2PMarketList.p2pmarketList;
            useraction = true;
        }
        private void P2PMarketLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (P2PMarketLB.SelectedItem == null) return;
            currentp2pmarket = P2PMarketLB.SelectedItem as p2pmarket;
            useraction = false;
            m_Versionlabel.Text = currentp2pmarket.m_Version.ToString();
            m_TraderIDNUD.Value = currentp2pmarket.m_TraderID;
            m_ClassNameTB.Text = currentp2pmarket.m_ClassName;
            positionXNUD.Value = currentp2pmarket.m_Position[0];
            positionYNUD.Value = currentp2pmarket.m_Position[1];
            positionZNUD.Value = currentp2pmarket.m_Position[2];

            orientaionXNUD.Value = currentp2pmarket.m_Orientation[0];
            orientaionYNUD.Value = currentp2pmarket.m_Orientation[1];
            orientaionZNUD.Value = currentp2pmarket.m_Orientation[2];

            LandPositionXNUD.Value = currentp2pmarket.m_VehicleSpawnPosition[0];
            LandPositionYNUD.Value = currentp2pmarket.m_VehicleSpawnPosition[1];
            LandPositionZNUD.Value = currentp2pmarket.m_VehicleSpawnPosition[2];

            WaterPositionXNUD.Value = currentp2pmarket.m_WatercraftSpawnPosition[0];
            WaterPositionYNUD.Value = currentp2pmarket.m_WatercraftSpawnPosition[1];
            WaterPositionZNUD.Value = currentp2pmarket.m_WatercraftSpawnPosition[2];

            AirPositionXNUD.Value = currentp2pmarket.m_AircraftSpawnPosition[0];
            AirPositionYNUD.Value = currentp2pmarket.m_AircraftSpawnPosition[1];
            AirPositionZNUD.Value = currentp2pmarket.m_AircraftSpawnPosition[2];

            m_DisplayNameTB.Text = currentp2pmarket.m_DisplayName;
            m_DisplayIconTB.Text = currentp2pmarket.m_DisplayIcon;
            m_EmoteIDNUD.Value = (int)currentp2pmarket.m_EmoteID;

            m_EmoteIsStaticCB.Checked = currentp2pmarket.m_EmoteIsStatic == 1 ? true : false;
            m_IsGlobalTraderCB.Checked = currentp2pmarket.m_IsGlobalTrader == 1 ? true : false;

            m_LoadoutFileCB.SelectedIndex = m_LoadoutFileCB.FindStringExact(currentp2pmarket.m_LoadoutFile);
            m_FactionCB.SelectedIndex = m_FactionCB.FindStringExact(currentp2pmarket.m_Faction);
            useraction = true;


        }
        private void darkButton98_Click(object sender, EventArgs e)
        {
            int Newid = P2PMarketList.GetNextID();
            P2PMarketList.AddnewP2P(Newid);
            P2PMarketLB.Invalidate();
        }
        private void darkButton99_Click(object sender, EventArgs e)
        {
            if (currentp2pmarket == null) return;
            if (MessageBox.Show("This Will Remove The All reference to this P2P Market Config, Are you sure you want to do this?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                P2PMarketList.Removep2pmarket(currentp2pmarket);
                if (P2PMarketLB.Items.Count == 0)
                    P2PMarketLB.SelectedIndex = -1;
                else
                    P2PMarketLB.SelectedIndex = 0;
            }
        }
        private void m_ClassNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentp2pmarket.m_ClassName = m_ClassNameTB.Text;
            currentp2pmarket.isDirty = true;
        }
        private void positionXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentp2pmarket.m_Position[0] = positionXNUD.Value;
            currentp2pmarket.isDirty = true;
        }
        private void positionYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentp2pmarket.m_Position[1] = positionYNUD.Value;
            currentp2pmarket.isDirty = true;
        }
        private void positionZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentp2pmarket.m_Position[2] = positionZNUD.Value;
            currentp2pmarket.isDirty = true;
        }
        private void orientaionXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentp2pmarket.m_Orientation[0] = orientaionXNUD.Value;
            currentp2pmarket.isDirty = true;
        }
        private void orientaionYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentp2pmarket.m_Orientation[1] = orientaionYNUD.Value;
            currentp2pmarket.isDirty = true;
        }
        private void orientaionZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentp2pmarket.m_Orientation[2] = orientaionZNUD.Value;
            currentp2pmarket.isDirty = true;
        }
        private void m_LoadoutFileCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentp2pmarket.m_LoadoutFile = m_LoadoutFileCB.GetItemText(m_LoadoutFileCB.SelectedItem);
            currentp2pmarket.isDirty = true;
        }
        private void LandPositionXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentp2pmarket.m_VehicleSpawnPosition[0] = LandPositionXNUD.Value;
            currentp2pmarket.isDirty = true;
        }
        private void LandPositionYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentp2pmarket.m_VehicleSpawnPosition[1] = LandPositionYNUD.Value;
            currentp2pmarket.isDirty = true;
        }
        private void LandPositionZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentp2pmarket.m_VehicleSpawnPosition[2] = LandPositionZNUD.Value;
            currentp2pmarket.isDirty = true;
        }
        private void WaterPositionXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentp2pmarket.m_WatercraftSpawnPosition[0] = WaterPositionXNUD.Value;
            currentp2pmarket.isDirty = true;
        }
        private void WaterPositionYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentp2pmarket.m_WatercraftSpawnPosition[1] = WaterPositionYNUD.Value;
            currentp2pmarket.isDirty = true;
        }
        private void WaterPositionZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentp2pmarket.m_WatercraftSpawnPosition[2] = WaterPositionZNUD.Value;
            currentp2pmarket.isDirty = true;
        }
        private void AirPositionXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentp2pmarket.m_AircraftSpawnPosition[0] = AirPositionXNUD.Value;
            currentp2pmarket.isDirty = true;
        }
        private void AirPositionYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentp2pmarket.m_AircraftSpawnPosition[1] = AirPositionYNUD.Value;
            currentp2pmarket.isDirty = true;
        }
        private void AirPositionZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentp2pmarket.m_AircraftSpawnPosition[2] = AirPositionZNUD.Value;
            currentp2pmarket.isDirty = true;
        }
        private void m_DisplayNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentp2pmarket.m_DisplayName = m_DisplayNameTB.Text;
            currentp2pmarket.isDirty = true;
        }
        private void m_DisplayIconTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentp2pmarket.m_DisplayIcon = m_DisplayIconTB.Text;
            currentp2pmarket.isDirty = true;
        }
        private void m_FactionCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentp2pmarket.m_Faction = m_FactionCB.GetItemText(m_FactionCB.SelectedItem);
            currentp2pmarket.isDirty = true;
        }
        private void m_EmoteIDNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentp2pmarket.m_EmoteID = (int)m_EmoteIDNUD.Value;
            currentp2pmarket.isDirty = true;
        }
        private void m_EmoteIsStaticCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentp2pmarket.m_EmoteIsStatic = m_EmoteIsStaticCB.Checked == true ? 1 : 0;
            currentp2pmarket.isDirty = true;
        }
        private void m_IsGlobalTraderCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentp2pmarket.m_IsGlobalTrader = m_IsGlobalTraderCB.Checked == true ? 1 : 0;
            currentp2pmarket.isDirty = true;
        }






        #endregion p2pmarket




    }
}
