using DarkUI.Forms;
using DayZeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows.Forms;

namespace DayZeEditor
{
    public partial class SearchForLootManager : DarkForm
    {
        public Project currentproject { get; set; }
        public TypesFile vanillatypes;
        public List<TypesFile> ModTypes;
        public BindingList<string> CurrentList { get; set; }
        public string SFLSettingsPath { get; private set; }
        public SFLConfig SFLSettings;
        public string Projectname;
        private bool useraction = false;
 
        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            ListBox lb = sender as ListBox;
            var CurrentItemWidth = (int)this.CreateGraphics().MeasureString(lb.Items[lb.Items.Count - 1].ToString(), lb.Font, TextRenderer.MeasureText(lb.Items[lb.Items.Count - 1].ToString(), new Font("Arial", 20.0F))).Width;
            lb.HorizontalExtent = CurrentItemWidth + 5;
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
        public SearchForLootManager()
        {
            InitializeComponent();
        }
        private void SearchForLootManager_Load(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Projectname = currentproject.ProjectName;
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            useraction = false;
            SFLSettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\SearchForLoot\\SearchForLoot.json";
            SFLSettings = JsonSerializer.Deserialize<SFLConfig>(File.ReadAllText(SFLSettingsPath));
            SFLSettings.isDirty = false;
            SFLSettings.Filename = SFLSettingsPath;

            setupGeneralsettings();
            LoadOthershit();

            useraction = true;
        }
        private void SearchForLootManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SFLSettings.isDirty)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Savefiles();
                }
            }
        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            Savefiles();
        }
        private void Savefiles()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (SFLSettings.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(SFLSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(SFLSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(SFLSettings.Filename, Path.GetDirectoryName(SFLSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(SFLSettings.Filename) + ".bak", true);
                }
                SFLSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(SFLSettings, options);
                File.WriteAllText(SFLSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(SFLSettings.Filename));
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
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\SearchForLoot");
        }
        private void generaslSetiingsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CheckBox cb = sender as CheckBox;
            SFLSettings.SetBoolValue(cb.Name.Substring(0, cb.Name.Length - 2), cb.Checked == true ? 1 : 0);
            SFLSettings.isDirty = true;
        }
        private void generaslSetiingsTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TextBox tb = sender as TextBox;
            SFLSettings.SetTextValue(tb.Name.Substring(0, tb.Name.Length - 2), tb.Text);
            SFLSettings.isDirty = true;
        }
        private void generaslSetiingsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            NumericUpDown nud = sender as NumericUpDown;
            if(nud.Tag.ToString() == "float")
                SFLSettings.SetDecimalValue(nud.Name.Substring(0, nud.Name.Length - 3), nud.Value);
            else if (nud.Tag.ToString() == "int")
                SFLSettings.SetIntValue(nud.Name.Substring(0, nud.Name.Length - 3), (int)nud.Value);
            SFLSettings.isDirty = true;
        }
        private void setupGeneralsettings()
        {
            useraction = false;
            EnableDebugCB.Checked = SFLSettings.EnableDebug == 1 ? true : false;
            RarityNUD.Value = SFLSettings.Rarity;
            InitialCooldownNUD.Value = SFLSettings.InitialCooldown;
            XPGainNUD.Value = SFLSettings.XPGain;
            SoundEnabledCB.Checked = SFLSettings.SoundEnabled == 1 ? true : false;
            DisableNotificationsCB.Checked = SFLSettings.DisableNotifications == 1 ? true : false;
            NotificationHeadingTB.Text = SFLSettings.NotificationHeading;
            NotificationTextTB.Text = SFLSettings.NotificationText;
            NotificationText2TB.Text = SFLSettings.NotificationText2;
            MaxHealthCoefNUD.Value = SFLSettings.MaxHealthCoef;
            useraction = true;
        }
        private void LoadOthershit()
        {
            SFLTV.Nodes.Clear();
            TreeNode SFLRoot = new TreeNode("Search For Loot")
            {
                Name = "SFLRoot",
                Tag = "SFLRoot"
            };
            TreeNode SFLBuildingsNode = new TreeNode("Buildings")
            {
                Tag = "SFLBuildings",
                Name = "SFLBuildings"
            };
            foreach(SFBuildingCategory sflbc in SFLSettings.SFLBuildings)
            {
                TreeNode sflbcNode = new TreeNode($"Name: - {sflbc.name}")
                {
                    Tag = sflbc,
                    Name = "SFLBuildings"
                };
                TreeNode sflbcbuildingsnode = new TreeNode($"Buildings:")
                {
                    Tag = "SFLBCBuildings",
                    Name = "SFLBCBuildings"
                };
                foreach(string building in sflbc.buildings)
                {
                    TreeNode buildingnode = new TreeNode($"Building Type: - {building}")
                    {
                        Tag = "BuildingType",
                        Name = "BuildingType"

                    };
                    sflbcbuildingsnode.Nodes.Add( buildingnode );
                }
                sflbcNode.Nodes.Add(sflbcbuildingsnode);

                SFLBuildingsNode.Nodes.Add( sflbcNode );
            }
            SFLRoot.Nodes.Add(SFLBuildingsNode);
            TreeNode SFLLootNode = new TreeNode("Loot")
            {
                Tag = "SFLLoot",
                Name = "SFLoot"
            };
            foreach (SFLootCategory sfllc in SFLSettings.SFLLootCategory)
            {
                TreeNode sfllcNode = new TreeNode($"Name: - {sfllc.name}")
                {
                    Tag = sfllc,
                    Name = "sfllc"
                };
                TreeNode sfllcrarity = new TreeNode($"Rarity: - {sfllc.rarity}")
                {
                    Tag = "LootRarity"
                };
                sfllcNode.Nodes.Add( sfllcrarity );
                TreeNode sfllcloot = new TreeNode("Loot:")
                {
                    Tag = "LootList",
                };
                foreach (string item in sfllc.loot)
                {
                    TreeNode itemnode = new TreeNode($"Item Name: - {item}")
                    {
                        Tag = "ItemName",
                        Name = "ItemName"
                    };
                    sfllcloot.Nodes.Add(itemnode);
                }
                sfllcNode.Nodes.Add(sfllcloot);
                SFLLootNode.Nodes.Add(sfllcNode);
            }
            SFLRoot.Nodes.Add(SFLLootNode);
            TreeNode SFLProxycNode = new TreeNode("Proxy")
            {
                Tag = "SFLProxy",
                Name = "SFLProxy"
            };
            foreach (SFProxyCategory sflpc in SFLSettings.SFLProxyCategory)
            {
                TreeNode sflpcNode = new TreeNode($"Name: - {sflpc.name}")
                {
                    Tag = sflpc,
                    Name = "sflpc"
                };
                TreeNode sflpcproxiesNode = new TreeNode($"Proxies: ")
                {
                    Tag = "sflpcproxies",
                    Name = "sflpcproxies"
                };
                foreach(string p in sflpc.proxies)
                {
                    TreeNode proxycNode = new TreeNode($"Proxies: - {p}")
                    {
                        Tag = "Proxies",
                        Name = "Proxies"
                    };
                    sflpcproxiesNode.Nodes.Add(proxycNode);
                }
                sflpcNode.Nodes.Add(sflpcproxiesNode);
                SFLProxycNode.Nodes.Add(sflpcNode);
            }
            SFLRoot.Nodes.Add(SFLProxycNode);
            SFLTV.Nodes.Add(SFLRoot);
        }

        public TreeNode currentTreeNode { get; set; }
        private bool _preventExpand = false;
        private DateTime _lastMouseDown = DateTime.Now;
        private void SFLTV_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = _preventExpand;
            _preventExpand = false;
        }
        private void SFLTV_MouseDown(object sender, MouseEventArgs e)
        {
            int delta = (int)DateTime.Now.Subtract(_lastMouseDown).TotalMilliseconds;
            _preventExpand = (delta < SystemInformation.DoubleClickTime);
            _lastMouseDown = DateTime.Now;
        }
        private void SFLTV_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = _preventExpand;
            _preventExpand = false;
        }
        private void SFLTV_AfterSelect(object sender, TreeViewEventArgs e)
        {
            currentTreeNode = e.Node;
        }
        private void SFLTV_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            SFBuildingCategory sFBuildingCategory = null;
            SFLootCategory sFLootCategory = null;
            SFProxyCategory sFProxyCategory = null;
            sFBuildingCategory = e.Node.Tag as SFBuildingCategory;
            sFLootCategory = e.Node.Tag as SFLootCategory;
            sFProxyCategory = e.Node.Tag as SFProxyCategory;
            if (e.Node.Tag.ToString() != "BuildingType" &&
               sFBuildingCategory == null &&
               sFLootCategory == null &&
               e.Node.Tag.ToString() != "Proxies" &&
               sFProxyCategory == null &&
               e.Node.Tag.ToString() != "LootRarity")
                e.CancelEdit = true;
        }

        private void SFLTV_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            SFLTV.SelectedNode = e.Node;
            if (e.Button == MouseButtons.Right)
            {
                addLootItemsToolStripMenuItem.Visible = false;
                removeToolStripMenuItem.Visible= false;
                addNewLootCategoryToolStripMenuItem.Visible= false;
                removeLootCategoryToolStripMenuItem.Visible = false;
                addBuildingCategoryToolStripMenuItem.Visible= false;
                removeToolStripMenuItem1.Visible= false;
                addNewBuildingToolStripMenuItem.Visible= false;
                removeToolStripMenuItem2.Visible= false;
                addNewProxyCategoryToolStripMenuItem.Visible= false;
                removeToolStripMenuItem3.Visible= false;
                addNewProxyToolStripMenuItem.Visible= false;
                removeToolStripMenuItem4.Visible= false;
                if (e.Node.Tag.ToString() == "LootList")
                {
                    addLootItemsToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "ItemName")
                {
                    removeToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "SFLLoot")
                {
                    addNewLootCategoryToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag is SFLootCategory)
                {
                    removeLootCategoryToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "SFLBuildings")
                {
                    addBuildingCategoryToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag is SFBuildingCategory)
                {
                    removeToolStripMenuItem1.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "SFLBCBuildings")
                {
                    addNewBuildingToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "BuildingType")
                {
                    removeToolStripMenuItem2.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                if (e.Node.Tag.ToString() == "SFLProxy")
                {
                    addNewProxyCategoryToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if(e.Node.Tag is SFProxyCategory)
                {
                    removeToolStripMenuItem3.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "sflpcproxies")
                {
                    addNewProxyToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "Proxies")
                {
                    removeToolStripMenuItem4.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
        }
        private void SFLTV_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag.ToString() == "BuildingType" ||
                e.Node.Tag is SFBuildingCategory ||
                e.Node.Tag is SFLootCategory ||
                e.Node.Tag.ToString() == "Proxies" ||
                e.Node.Tag is SFProxyCategory ||
                e.Node.Tag.ToString() == "LootRarity"
                )
                e.Node.BeginEdit();
            else if (e.Node.Tag.ToString() == "ItemName")
            {
                AddItemfromTypes form = new AddItemfromTypes
                {
                    vanillatypes = vanillatypes,
                    ModTypes = ModTypes,
                    currentproject = currentproject,
                    UseOnlySingleitem = true
                };
                DialogResult result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    List<string> addedtypes = form.addedtypes.ToList();
                    foreach (string l in addedtypes)
                    {
                        string currentitem = e.Node.Text.Substring(13);
                        SFLootCategory sFLootCategory = e.Node.Parent.Parent.Tag as SFLootCategory;
                        int index = sFLootCategory.loot.IndexOf(currentitem);
                        sFLootCategory.loot[index] = l;
                        e.Node.Text = $"Item Name: - {l}";
                        SFLSettings.isDirty = true;
                    }

                }
            }
        }
        private void SFLTV_RequestDisplayText(object sender, TreeViewMS.NodeRequestTextEventArgs e)
        {
            if (e.Node.Tag.ToString() == "BuildingType")
            {
                string building = e.Node.Text.Substring(17);
                SFBuildingCategory sFBuildingCategory = e.Node.Parent.Parent.Tag as SFBuildingCategory;
                int index = sFBuildingCategory.buildings.IndexOf(building);
                sFBuildingCategory.buildings[index] = e.Label;
                e.Node.Text = e.Label = $"Building Type: - {e.Label}";
                SFLSettings.isDirty = true;
            }
            else if (e.Node.Tag.ToString() == "Proxies")
            {
                string building = e.Node.Text.Substring(11);
                SFProxyCategory sFProxyCategory = e.Node.Parent.Parent.Tag as SFProxyCategory;
                int index = sFProxyCategory.proxies.IndexOf(building);
                sFProxyCategory.proxies[index] = e.Label;
                e.Node.Text = e.Label = $"Proxies: - {e.Label}";
                SFLSettings.isDirty = true;
            }
            else if (e.Node.Tag.ToString() == "LootRarity")
            {
                SFLootCategory sFLootCategory = e.Node.Parent.Tag as SFLootCategory;
                sFLootCategory.rarity = Convert.ToDecimal(e.Label) + 0.0M;
                e.Label = $"Rarity: - {sFLootCategory.rarity}";
                SFLSettings.isDirty = true;
            }
            else if (e.Node.Tag is SFBuildingCategory)
            {
                SFBuildingCategory sFBuildingCategory = (SFBuildingCategory)e.Node.Tag;
                sFBuildingCategory.name = e.Label;
                e.Label = $"Name: - {sFBuildingCategory.name}";
                SFLSettings.isDirty = true;
            }
            else if (e.Node.Tag is SFLootCategory)
            {
                SFLootCategory SFLootCategory = (SFLootCategory)e.Node.Tag;
                SFLootCategory.name = e.Label;
                e.Label = $"Name: - {SFLootCategory.name}";
                SFLSettings.isDirty = true;
            }
            else if (e.Node.Tag is SFProxyCategory)
            {
                SFProxyCategory SFProxyCategory = (SFProxyCategory)e.Node.Tag;
                SFProxyCategory.name = e.Label;
                e.Label = $"Name: - {SFProxyCategory.name}";
                SFLSettings.isDirty = true;
            }
        }
        private void SFLTV_RequestEditText(object sender, TreeViewMS.NodeRequestTextEventArgs e)
        {
            if (e.Node.Tag.ToString() == "BuildingType")
            {
                e.Label = e.Node.Text.Substring(17);
            }
            else if (e.Node.Tag.ToString() == "Proxies")
            {
                e.Label = e.Node.Text.Substring(11);
            }
            else if (e.Node.Tag.ToString() == "LootRarity")
            {
                e.Label = e.Node.Text.Substring(10);
            }
            else if (e.Node.Tag is SFBuildingCategory || e.Node.Tag is SFLootCategory || e.Node.Tag is SFProxyCategory)
            {
                e.Label = e.Node.Text.Substring(8);
            }
        }
        private void addLootItemsToolStripMenuItem_Click(object sender, EventArgs e)
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
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    SFLootCategory sFLootCategory = currentTreeNode.Parent.Tag as SFLootCategory;
                    sFLootCategory.loot.Add(l);
                    SFLSettings.isDirty = true;
                    TreeNode itemnode = new TreeNode($"Item Name: - {l}")
                    {
                        Tag = "ItemName",
                        Name = "ItemName"
                    };
                    currentTreeNode.Nodes.Add(itemnode);
                 }

            }
        }
        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SFLootCategory sFLootCategory = currentTreeNode.Parent.Parent.Tag as SFLootCategory;
            string item = currentTreeNode.Text.Substring(13);
            sFLootCategory.loot.Remove(item);
            currentTreeNode.Parent.Nodes.Remove(currentTreeNode);
            SFLSettings.isDirty = true;
        }
        private void addNewLootCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SFLootCategory newcat = new SFLootCategory()
            {
                name = "Change Me",
                rarity = 50,
                loot = new BindingList<string>()
            };
            SFLSettings.SFLLootCategory.Add(newcat);
            SFLSettings.isDirty = true;
            TreeNode sfllcNode = new TreeNode($"Name: - {newcat.name}")
            {
                Tag = newcat,
                Name = "sfllc"
            };
            TreeNode sfllcrarity = new TreeNode($"Rarity: - {newcat.rarity}")
            {
                Tag = "LootRarity"
            };
            sfllcNode.Nodes.Add(sfllcrarity);
            TreeNode sfllcloot = new TreeNode("Loot:")
            {
                Tag = "LootList",
            };
            foreach (string item in newcat.loot)
            {
                TreeNode itemnode = new TreeNode($"Item Name: - {item}")
                {
                    Tag = "ItemName",
                    Name = "ItemName"
                };
                sfllcloot.Nodes.Add(itemnode);
            }
            sfllcNode.Nodes.Add(sfllcloot);
            currentTreeNode.Nodes.Add(sfllcNode);
            SFLTV.SelectedNode = sfllcNode;
        }
        private void removeLootCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SFLootCategory sFLootCategory = currentTreeNode.Tag as SFLootCategory;
            SFLSettings.SFLLootCategory.Remove(sFLootCategory);
            SFLSettings.isDirty = true;
            currentTreeNode.Parent.Nodes.Remove(currentTreeNode);
        }
        private void addBuildingCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SFBuildingCategory sFBuildingCategory = new SFBuildingCategory()
            {
                name = "New Building Category",
                buildings = new BindingList<string>()
            };
            SFLSettings.SFLBuildings.Add(sFBuildingCategory);
            SFLSettings.isDirty = true;
            TreeNode sflbcNode = new TreeNode($"Name: - {sFBuildingCategory.name}")
            {
                Tag = sFBuildingCategory,
                Name = "SFLBuildings"
            };
            TreeNode sflbcbuildingsnode = new TreeNode($"Buildings:")
            {
                Tag = "SFLBCBuildings",
                Name = "SFLBCBuildings"
            };
            foreach (string building in sFBuildingCategory.buildings)
            {
                TreeNode buildingnode = new TreeNode($"Building Type: - {building}")
                {
                    Tag = "BuildingType",
                    Name = "BuildingType"

                };
                sflbcbuildingsnode.Nodes.Add(buildingnode);
            }
            sflbcNode.Nodes.Add(sflbcbuildingsnode);
            currentTreeNode.Nodes.Add(sflbcNode);
            SFLTV.SelectedNode = sflbcNode;
        }
        private void removeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SFBuildingCategory sFBuildingCategory = currentTreeNode.Tag as SFBuildingCategory;
            SFLSettings.SFLBuildings.Remove(sFBuildingCategory);
            SFLSettings.isDirty = true;
            currentTreeNode.Parent.Nodes.Remove(currentTreeNode);
        }
        private void addNewBuildingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SFBuildingCategory sFBuildingCategory = currentTreeNode.Parent.Tag as SFBuildingCategory;
            string building = "New Building, Change Me";
            sFBuildingCategory.buildings.Add(building);
            SFLSettings.isDirty = true;
            TreeNode buildiongnode = new TreeNode($"Building Type: - {building}")
            {
                Tag = "BuildingType",
                Name = "BuildingType"
            };
            currentTreeNode.Nodes.Add(buildiongnode);
            SFLTV.SelectedNode = buildiongnode;
        }
        private void removeToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SFBuildingCategory sFBuildingCategory = currentTreeNode.Parent.Parent.Tag as SFBuildingCategory;
            string item = currentTreeNode.Text.Substring(17);
            sFBuildingCategory.buildings.Remove(item);
            currentTreeNode.Parent.Nodes.Remove(currentTreeNode);
            SFLSettings.isDirty = true;
        }

        private void addNewProxyCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SFProxyCategory sFProxyCategory = new SFProxyCategory()
            {
                name = "New Proxy Category",
                proxies = new BindingList<string>()
            };
            SFLSettings.SFLProxyCategory.Add(sFProxyCategory);
            SFLSettings.isDirty = true;
            TreeNode sflpcNode = new TreeNode($"Name: - {sFProxyCategory.name}")
            {
                Tag = sFProxyCategory,
                Name = "sflpc"
            };
            TreeNode sflpcproxiesNode = new TreeNode($"Proxies: ")
            {
                Tag = "sflpcproxies",
                Name = "sflpcproxies"
            };
            foreach (string p in sFProxyCategory.proxies)
            {
                TreeNode proxycNode = new TreeNode($"Proxies: - {p}")
                {
                    Tag = "Proxies",
                    Name = "Proxies"
                };
                sflpcproxiesNode.Nodes.Add(proxycNode);
            }
            sflpcNode.Nodes.Add(sflpcproxiesNode);
            currentTreeNode.Nodes.Add(sflpcNode);
            SFLTV.SelectedNode = sflpcNode;
        }

        private void removeToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            SFProxyCategory sFProxyCategory = currentTreeNode.Tag as SFProxyCategory;
            SFLSettings.SFLProxyCategory.Remove(sFProxyCategory);
            SFLSettings.isDirty = true;
            currentTreeNode.Parent.Nodes.Remove(currentTreeNode);
        }

        private void addNewProxyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SFProxyCategory sFProxyCategory = currentTreeNode.Parent.Tag as SFProxyCategory;
            string proxy = "New Proxy, Change Me";
            sFProxyCategory.proxies.Add(proxy);
            SFLSettings.isDirty = true;
            TreeNode buildiongnode = new TreeNode($"Proxies: - {proxy}")
            {
                Tag = "Proxies",
                Name = "Proxies"
            };
            currentTreeNode.Nodes.Add(buildiongnode);
            SFLTV.SelectedNode = buildiongnode;
        }

        private void removeToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            SFProxyCategory sFProxyCategory = currentTreeNode.Parent.Parent.Tag as SFProxyCategory;
            string item = currentTreeNode.Text.Substring(11);
            sFProxyCategory.proxies.Remove(item);
            currentTreeNode.Parent.Nodes.Remove(currentTreeNode);
            SFLSettings.isDirty = true;
        }
    }
}
