using Cyotek.Windows.Forms;
using DarkUI.Forms;
using DayZeLib;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows.Forms;

namespace DayZeEditor
{
    public partial class ExpansionCircleMarkerManager : DarkForm
    {
        public Project currentproject { get; set; }
        public TypesFile vanillatypes;
        public List<TypesFile> ModTypes;

        public string ECMAdminConfigPath { get; private set; }
        public ECMAdminConfig ECMAdminConfig { get; set; }
        public string ECMdynamicpvpzonePath { get; private set; }
        public ECMDynamicPVPZoneConfig ECMDynamicPVPZoneConfig { get; set; }
        public string ECMExpansionCircleMarkerConfigPath { get; private set; }
        public ECMExpansionCircleMarkerConfig ECMExpansionCircleMarkerConfig { get; set; }
        public string ECMItemRulesConfigPath { get; set; }
        public ECMItemRulesConfig ECMItemRulesConfig { get; set; }
        public string ECMPolygonZonesConfigPath { get; set; }
        public ECMPolygonZonesConfig ECMPolygonZonesConfig { get; set; }

        public MapData MapData { get; private set; }

        public string Projectname;
        private bool _useraction = false;
        private Scanzone currentScanZone;

        public bool useraction
        {
            get { return _useraction; }
            set
            {
                _useraction = value;
            }
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
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
        
        
        public ExpansionCircleMarkerManager()
        {
            InitializeComponent();
            tabControl1.ItemSize = new Size(0, 1);
            tabControl2.ItemSize = new Size(0, 1);
        }
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripButton6.Checked = false;
            toolStripButton1.Checked = false;
            toolStripButton3.Checked = false;
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    toolStripButton6.Checked = true;
                    break;
                case 1:
                    toolStripButton1.Checked = true;
                    break;
                case 2:
                    toolStripButton3.Checked = true;
                    break;
                default:
                    break;
            }
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionCircleMarker\\Config");
        }
        private void ExpansionCircleMarkerManager_Load(object sender, EventArgs e)
        {
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            ECMAdminConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionCircleMarker\\Config\\admins.json";
            if (!File.Exists(ECMAdminConfigPath))
            {
                ECMAdminConfig = new ECMAdminConfig();
            }
            else
            {
                ECMAdminConfig = JsonSerializer.Deserialize<ECMAdminConfig>(File.ReadAllText(ECMAdminConfigPath));
                ECMAdminConfig.isDirty = false;
            }
            ECMAdminConfig.Filename = ECMAdminConfigPath;
            SetupAdminConfig();

            ECMdynamicpvpzonePath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionCircleMarker\\Config\\dynamicpvpzone.json";
            if (!File.Exists(ECMdynamicpvpzonePath))
            {
                ECMDynamicPVPZoneConfig = new ECMDynamicPVPZoneConfig();
            }
            else
            {
                ECMDynamicPVPZoneConfig = JsonSerializer.Deserialize<ECMDynamicPVPZoneConfig>(File.ReadAllText(ECMdynamicpvpzonePath));
                ECMDynamicPVPZoneConfig.isDirty = false;
            }
            ECMDynamicPVPZoneConfig.Filename = ECMdynamicpvpzonePath;

            ECMExpansionCircleMarkerConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionCircleMarker\\Config\\ExpansionCircleMarker.json";
            if (!File.Exists(ECMExpansionCircleMarkerConfigPath))
            {
                ECMExpansionCircleMarkerConfig = new ECMExpansionCircleMarkerConfig();
            }
            else
            {
                ECMExpansionCircleMarkerConfig = JsonSerializer.Deserialize<ECMExpansionCircleMarkerConfig>(File.ReadAllText(ECMExpansionCircleMarkerConfigPath));
                ECMExpansionCircleMarkerConfig.isDirty = false;
            }
            ECMExpansionCircleMarkerConfig.Filename = ECMExpansionCircleMarkerConfigPath;

            ECMItemRulesConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionCircleMarker\\Config\\itemrules.json";
            if (!File.Exists(ECMItemRulesConfigPath))
            {
                ECMItemRulesConfig = new ECMItemRulesConfig();
            }
            else
            {
                ECMItemRulesConfig = JsonSerializer.Deserialize<ECMItemRulesConfig>(File.ReadAllText(ECMItemRulesConfigPath));
                ECMItemRulesConfig.isDirty = false;
            }
            ECMItemRulesConfig.Filename = ECMItemRulesConfigPath;
            SetupitemRules();

            ECMPolygonZonesConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionCircleMarker\\Config\\polygonzones.json";
            if (!File.Exists(ECMPolygonZonesConfigPath))
            {
                ECMPolygonZonesConfig = new ECMPolygonZonesConfig();
            }
            else
            {
                ECMPolygonZonesConfig = JsonSerializer.Deserialize<ECMPolygonZonesConfig>(File.ReadAllText(ECMPolygonZonesConfigPath));
                ECMPolygonZonesConfig.isDirty = false;
            }
            ECMPolygonZonesConfig.Filename = ECMPolygonZonesConfigPath;
            ECMPolygonZonesConfig.GetVec3List();

            setupgeneralsettings();
            setupzonesandshit();
            EMCZonesTV.HideSelection = false;

            MapData = new MapData(Application.StartupPath + currentproject.MapPath + ".xyz", currentproject.MapSize);

            doubleClickTimer.Interval = 100;
            doubleClickTimer.Tick += new EventHandler(doubleClickTimer_Tick);

            pictureBox1.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Livonia maop size is 12800 x 12800, 0,0 bottom left, center 6400 x 6400
            pictureBox1.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox1.Paint += new PaintEventHandler(DrawECMZones);
            trackBar1.Value = 1;
            SetECMZonescale();
        }



        private void ExpansionCircleMarkerManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool needtosave = false;
            if (ECMAdminConfig.isDirty)
            {
                needtosave = true;
            }
            if (ECMDynamicPVPZoneConfig.isDirty)
            {
                needtosave = true;
            }
            if (ECMExpansionCircleMarkerConfig.isDirty)
            {
                needtosave = true;
            }
            if (ECMItemRulesConfig.isDirty)
            {
                needtosave = true;
            }
            if (ECMPolygonZonesConfig.isDirty)
            {
                needtosave = true;
            }
            if (needtosave)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SaveECMConfigs();
                }
            }
        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            SaveECMConfigs();
        }
        private void SaveECMConfigs()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (ECMAdminConfig.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(ECMAdminConfig.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(ECMAdminConfig.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(ECMAdminConfig.Filename, Path.GetDirectoryName(ECMAdminConfig.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(ECMAdminConfig.Filename) + ".bak", true);
                }
                ECMAdminConfig.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(ECMAdminConfig, options);
                File.WriteAllText(ECMAdminConfig.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(Path.GetFileName(ECMAdminConfigPath)));
            }
            if (ECMDynamicPVPZoneConfig.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(ECMDynamicPVPZoneConfig.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(ECMDynamicPVPZoneConfig.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(ECMDynamicPVPZoneConfig.Filename, Path.GetDirectoryName(ECMDynamicPVPZoneConfig.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(ECMDynamicPVPZoneConfig.Filename) + ".bak", true);
                }
                ECMDynamicPVPZoneConfig.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(ECMDynamicPVPZoneConfig, options);
                File.WriteAllText(ECMDynamicPVPZoneConfig.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(Path.GetFileName(ECMdynamicpvpzonePath)));
            }
            if (ECMExpansionCircleMarkerConfig.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(ECMExpansionCircleMarkerConfig.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(ECMExpansionCircleMarkerConfig.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(ECMExpansionCircleMarkerConfig.Filename, Path.GetDirectoryName(ECMExpansionCircleMarkerConfig.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(ECMExpansionCircleMarkerConfig.Filename) + ".bak", true);
                }
                ECMExpansionCircleMarkerConfig.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(ECMExpansionCircleMarkerConfig, options);
                File.WriteAllText(ECMExpansionCircleMarkerConfig.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(Path.GetFileName(ECMExpansionCircleMarkerConfigPath)));
            }
            if (ECMItemRulesConfig.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(ECMItemRulesConfig.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(ECMItemRulesConfig.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(ECMItemRulesConfig.Filename, Path.GetDirectoryName(ECMItemRulesConfig.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(ECMItemRulesConfig.Filename) + ".bak", true);
                }
                ECMItemRulesConfig.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(ECMItemRulesConfig, options);
                File.WriteAllText(ECMItemRulesConfig.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(Path.GetFileName(ECMItemRulesConfigPath)));
            }
            if (ECMPolygonZonesConfig.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(ECMPolygonZonesConfig.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(ECMPolygonZonesConfig.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(ECMPolygonZonesConfig.Filename, Path.GetDirectoryName(ECMPolygonZonesConfig.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(ECMPolygonZonesConfig.Filename) + ".bak", true);
                }
                ECMPolygonZonesConfig.SetVec3List();
                ECMPolygonZonesConfig.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(ECMPolygonZonesConfig, options);
                File.WriteAllText(ECMPolygonZonesConfig.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(Path.GetFileName(ECMPolygonZonesConfigPath)));
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
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            tabControl2.SelectedIndex = 0;
        }
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            tabControl2.SelectedIndex = 1;
        }
        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripButton4.Checked = false;
            toolStripButton5.Checked = false;
            switch (tabControl2.SelectedIndex)
            {
                case 0:
                    toolStripButton4.Checked = true;
                    break;
                case 1:
                    toolStripButton5.Checked = true;
                    break;
            }
        }

        #region EMCAdminsConfig
        private void SetupAdminConfig()
        {
            useraction = false;

            EMCAdminsConfigLB.DisplayMember = "Name";
            EMCAdminsConfigLB.ValueMember = "Value";
            EMCAdminsConfigLB.DataSource = ECMAdminConfig.AdminGUIDs;

            useraction = true;
        }
        private void EMCAdminsConfigLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EMCAdminsConfigLB.SelectedItem == null) { return; }
            useraction = false;
            EMCAdminsConfigTB.Text = EMCAdminsConfigLB.SelectedItem as string;
            useraction = true;
        }
        private void EMCAdminsConfigTB_TextChanged(object sender, EventArgs e)
        {
            if(!useraction) { return; }
            ECMAdminConfig.AdminGUIDs[EMCAdminsConfigLB.SelectedIndex] = EMCAdminsConfigTB.Text;
            ECMAdminConfig.isDirty = true;
        }
        private void darkButton30_Click(object sender, EventArgs e)
        {
            if (EMCAdminsConfigLB.SelectedItem == null) { return; }
            ECMAdminConfig.AdminGUIDs.Remove(EMCAdminsConfigLB.GetItemText(EMCAdminsConfigLB.SelectedItem));
            ECMAdminConfig.isDirty = true;
        }
        private void darkButton31_Click(object sender, EventArgs e)
        {
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    ECMAdminConfig.AdminGUIDs.Add(l.ToLower());
                    ECMAdminConfig.isDirty = true;
                }
            }
        }

        #endregion
        #region itemrules
        public TreeNode CurrentTreenode { get; set; }
        public void SetupitemRules()
        {
            ItemRulesTV.Nodes.Clear();
            TreeNode CantBeDamagedOutsidePvProot = new TreeNode("Cant Be Damaged Outside PvP")
            {
                Tag = "CantBeDamagedOutsidePvPRoot"
            };
            foreach(Cantbedamagedoutsidepvp cbdopvp in ECMItemRulesConfig.CantBeDamagedOutsidePvP) 
            {
                bool edog = cbdopvp.EnableDamageOnGround == 1 ? true : false;
                TreeNode Cantbedamagedoutsidepvp = new TreeNode($"Item Name: {cbdopvp.itemName}, Enable Damage On Ground: {edog}")
                {
                    Tag = cbdopvp
                };
                CantBeDamagedOutsidePvProot.Nodes.Add(Cantbedamagedoutsidepvp);
            }
            ItemRulesTV.Nodes.Add(CantBeDamagedOutsidePvProot);
            TreeNode CantDoDamageOutsidePvProot = new TreeNode("Cant Do Damage Outside PvP")
            {
                Tag = "CantDoDamageOutsidePvProot"
            };
            foreach(string cddopvp in ECMItemRulesConfig.CantDoDamageOutsidePvP)
            {
                TreeNode CantDoDamageOutsidePvP = new TreeNode($"Item Name: {cddopvp}")
                {
                    Tag = cddopvp
                };
                CantDoDamageOutsidePvProot.Nodes.Add(CantDoDamageOutsidePvP);
            }
            ItemRulesTV.Nodes.Add(CantDoDamageOutsidePvProot);
            TreeNode CantBeDamagedAtAllroot = new TreeNode("Cant Be Damaged At All")
            {
                Tag = "CantBeDamagedAtAllroot"
            };
            foreach (string cbdaa in ECMItemRulesConfig.CantBeDamagedAtAll)
            {
                TreeNode CantBeDamagedAtAll = new TreeNode($"Item Name: {cbdaa}")
                {
                    Tag = cbdaa
                };
                CantBeDamagedAtAllroot.Nodes.Add(CantBeDamagedAtAll);
            }
            ItemRulesTV.Nodes.Add(CantBeDamagedAtAllroot);
            TreeNode CantBeUnpinnedOutsidePvProot = new TreeNode("Cant Be Unpinned Outside PvP")
            {
                Tag = "CantBeUnpinnedOutsidePvProot"
            };
            foreach (string cbuopvp in ECMItemRulesConfig.CantBeUnpinnedOutsidePvP)
            {
                TreeNode CantBeUnpinnedOutsidePvP = new TreeNode($"Item Name: {cbuopvp}")
                {
                    Tag = cbuopvp
                };
                CantBeUnpinnedOutsidePvProot.Nodes.Add(CantBeUnpinnedOutsidePvP);
            }
            ItemRulesTV.Nodes.Add(CantBeUnpinnedOutsidePvProot);
        }
        private void ItemRulesTV_AfterSelect(object sender, TreeViewEventArgs e)
        {
            CurrentTreenode = e.Node;
            CantBeDamagedOutsidePvPGB.Visible = false;
            useraction = false;
            if (CurrentTreenode.Parent == null) return;
            switch (CurrentTreenode.Parent.Tag.ToString()) 
            {
                case "CantBeDamagedOutsidePvPRoot":
                    CantBeDamagedOutsidePvPGB.Visible = true;
                    Cantbedamagedoutsidepvp cbdopvp = e.Node.Tag as Cantbedamagedoutsidepvp;
                    textBox1.Text = cbdopvp.itemName;
                    EnableDamageOnGroundCB.Visible = true;
                    EnableDamageOnGroundCB.Checked = cbdopvp.EnableDamageOnGround == 1 ? true :false;
                    break;
                case "CantDoDamageOutsidePvProot":
                case "CantBeDamagedAtAllroot":
                case "CantBeUnpinnedOutsidePvProot":
                    CantBeDamagedOutsidePvPGB.Visible = true;
                    textBox1.Text = e.Node.Tag.ToString();
                    EnableDamageOnGroundCB.Visible = false;
                    break;
            }
            useraction = true;
        }
        private void ItemRulesTV_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ItemRulesTV.SelectedNode = e.Node;
            if(e.Node.Parent == null)
            {
                if (e.Button == MouseButtons.Right)
                {
                    addNewToolStripMenuItem.Visible = true;
                    removeSelectedItemToolStripMenuItem.Visible = false;
                    moveUpToolStripMenuItem.Visible = false;
                    moveDownToolStripMenuItem.Visible = false;
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
            else
            {
                if (e.Button == MouseButtons.Right)
                {
                    switch (e.Node.Parent.Tag.ToString())
                    {
                        case "CantBeDamagedOutsidePvPRoot":
                            addNewToolStripMenuItem.Visible = false;
                            removeSelectedItemToolStripMenuItem.Visible = true;
                            moveUpToolStripMenuItem.Visible = true;
                            moveDownToolStripMenuItem.Visible = true;
                            contextMenuStrip1.Show(Cursor.Position);
                            break;
                        case "CantDoDamageOutsidePvProot":
                        case "CantBeDamagedAtAllroot":
                        case "CantBeUnpinnedOutsidePvProot":
                            addNewToolStripMenuItem.Visible = false;
                            removeSelectedItemToolStripMenuItem.Visible = true;
                            moveUpToolStripMenuItem.Visible = false;
                            moveDownToolStripMenuItem.Visible = false;
                            contextMenuStrip1.Show(Cursor.Position);
                            break;
                    }
                }
            }
            
        }
        private void addNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (CurrentTreenode.Tag.ToString())
            {
                case "CantBeDamagedOutsidePvPRoot":
                    Cantbedamagedoutsidepvp newcbdopvp = new Cantbedamagedoutsidepvp()
                    {
                        itemName = "ChangeMe",
                        EnableDamageOnGround = 0
                    };
                    ECMItemRulesConfig.CantBeDamagedOutsidePvP.Add(newcbdopvp);
                    bool edog = newcbdopvp.EnableDamageOnGround == 1 ? true : false;
                    TreeNode Cantbedamagedoutsidepvp = new TreeNode($"Item Name: {newcbdopvp.itemName}, Enable Damage On Ground: {edog}")
                    {
                        Tag = newcbdopvp
                    };
                    CurrentTreenode.Nodes.Add(Cantbedamagedoutsidepvp);
                    break;
                case "CantDoDamageOutsidePvProot":
                    ECMItemRulesConfig.CantDoDamageOutsidePvP.Add("ChangeMe");
                    TreeNode CantDoDamageOutsidePvProot = new TreeNode($"Item Name: ChangeMe")
                    {
                        Tag = "ChangeMe"
                    };
                    CurrentTreenode.Nodes.Add(CantDoDamageOutsidePvProot);
                    break;
                case "CantBeDamagedAtAllroot":
                    ECMItemRulesConfig.CantBeDamagedAtAll.Add("ChangeMe");
                    TreeNode CantBeDamagedAtAllroot = new TreeNode($"Item Name: ChangeMe")
                    {
                        Tag = "ChangeMe"
                    };
                    CurrentTreenode.Nodes.Add(CantBeDamagedAtAllroot);
                    break;
                case "CantBeUnpinnedOutsidePvProot":
                    ECMItemRulesConfig.CantBeUnpinnedOutsidePvP.Add("ChangeMe");
                    TreeNode CantBeUnpinnedOutsidePvProot = new TreeNode($"Item Name: ChangeMe")
                    {
                        Tag = "ChangeMe"
                    };
                    CurrentTreenode.Nodes.Add(CantBeUnpinnedOutsidePvProot);
                    break;
            }
            ECMItemRulesConfig.isDirty = true;
        }
        private void removeSelectedItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (CurrentTreenode.Parent.Tag.ToString())
            {
                case "CantBeDamagedOutsidePvPRoot":
                    Cantbedamagedoutsidepvp cbdopvp = CurrentTreenode.Tag as Cantbedamagedoutsidepvp;
                    ECMItemRulesConfig.CantBeDamagedOutsidePvP.Remove(cbdopvp);
                    break;
                case "CantDoDamageOutsidePvProot":
                    ECMItemRulesConfig.CantDoDamageOutsidePvP.Remove(CurrentTreenode.Tag.ToString());
                    break;
                case "CantBeDamagedAtAllroot":
                    ECMItemRulesConfig.CantBeDamagedAtAll.Remove(CurrentTreenode.Tag.ToString());
                    break;
                case "CantBeUnpinnedOutsidePvProot":
                    ECMItemRulesConfig.CantBeUnpinnedOutsidePvP.Remove(CurrentTreenode.Tag.ToString());
                    break;
            }
            CurrentTreenode.Parent.Nodes.Remove(CurrentTreenode);
            ECMItemRulesConfig.isDirty = true;
        }
        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cantbedamagedoutsidepvp cbdopvp = CurrentTreenode.Tag as Cantbedamagedoutsidepvp;
            int index = ECMItemRulesConfig.CantBeDamagedOutsidePvP.IndexOf(cbdopvp);
            if (index == 0) return;
            ECMItemRulesConfig.CantBeDamagedOutsidePvP.RemoveAt(index);
            ECMItemRulesConfig.CantBeDamagedOutsidePvP.Insert(index -1, cbdopvp);
            ECMItemRulesConfig.isDirty = true;
            TreeNode parent = CurrentTreenode.Parent;
            TreeNode exisiting = CurrentTreenode;
            parent.Nodes.Remove(CurrentTreenode);
            parent.Nodes.Insert(index -1, exisiting);
            ItemRulesTV.SelectedNode = exisiting;
        }
        private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cantbedamagedoutsidepvp cbdopvp = CurrentTreenode.Tag as Cantbedamagedoutsidepvp;
            int index = ECMItemRulesConfig.CantBeDamagedOutsidePvP.IndexOf(cbdopvp);
            if (index == ECMItemRulesConfig.CantBeDamagedOutsidePvP.Count() -1) return;
            ECMItemRulesConfig.CantBeDamagedOutsidePvP.RemoveAt(index);
            ECMItemRulesConfig.CantBeDamagedOutsidePvP.Insert(index + 1, cbdopvp);
            ECMItemRulesConfig.isDirty = true;
            TreeNode parent = CurrentTreenode.Parent;
            TreeNode exisiting = CurrentTreenode;
            parent.Nodes.Remove(CurrentTreenode);
            parent.Nodes.Insert(index + 1, exisiting);
            ItemRulesTV.SelectedNode = exisiting;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(!useraction) { return; }
            if (CurrentTreenode.Parent == null) return;
            switch (CurrentTreenode.Parent.Tag.ToString())
            {
                case "CantBeDamagedOutsidePvPRoot":
                    Cantbedamagedoutsidepvp cbdopvp = CurrentTreenode.Tag as Cantbedamagedoutsidepvp;
                    cbdopvp.itemName = textBox1.Text;
                    bool edog = cbdopvp.EnableDamageOnGround == 1 ? true : false;
                    CurrentTreenode.Text = $"Item Name: {cbdopvp.itemName}, Enable Damage On Ground: {edog}";
                    break;
                case "CantDoDamageOutsidePvProot":
                    string text = CurrentTreenode.Tag.ToString();
                    for(int i = 0; i < ECMItemRulesConfig.CantDoDamageOutsidePvP.Count(); i++ )
                    {
                        if (ECMItemRulesConfig.CantDoDamageOutsidePvP[i] == text)
                        {
                            ECMItemRulesConfig.CantDoDamageOutsidePvP[i] = textBox1.Text;
                            CurrentTreenode.Tag = ECMItemRulesConfig.CantDoDamageOutsidePvP[i];
                            CurrentTreenode.Text = $"Item Name: {ECMItemRulesConfig.CantDoDamageOutsidePvP[i]}";
                            break;
                        }
                    }
                    break;
                case "CantBeDamagedAtAllroot":
                    text = CurrentTreenode.Tag.ToString();
                    for (int i = 0; i < ECMItemRulesConfig.CantBeDamagedAtAll.Count(); i++)
                    {
                        if (ECMItemRulesConfig.CantBeDamagedAtAll[i] == text)
                        {
                            ECMItemRulesConfig.CantBeDamagedAtAll[i] = textBox1.Text;
                            CurrentTreenode.Tag = ECMItemRulesConfig.CantBeDamagedAtAll[i];
                            CurrentTreenode.Text = $"Item Name: {ECMItemRulesConfig.CantBeDamagedAtAll[i]}";
                            break;
                        }
                    }
                    break;
                case "CantBeUnpinnedOutsidePvProot":
                    text = CurrentTreenode.Tag.ToString();
                    for (int i = 0; i < ECMItemRulesConfig.CantBeUnpinnedOutsidePvP.Count(); i++)
                    {
                        if (ECMItemRulesConfig.CantBeUnpinnedOutsidePvP[i] == text)
                        {
                            ECMItemRulesConfig.CantBeUnpinnedOutsidePvP[i] = textBox1.Text;
                            CurrentTreenode.Tag = ECMItemRulesConfig.CantBeUnpinnedOutsidePvP[i];
                            CurrentTreenode.Text = $"Item Name: {ECMItemRulesConfig.CantBeUnpinnedOutsidePvP[i]}";
                            break;
                        }
                    }
                    break;
            }

            ECMItemRulesConfig.isDirty = true;
        }
        private void darkButton26_Click(object sender, EventArgs e)
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
                    textBox1.Text = l;
                    ECMItemRulesConfig.isDirty = true;
                }

            }
        }
        private void EnableDamageOnGroundCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            if(CurrentTreenode.Tag is Cantbedamagedoutsidepvp)
            {
                Cantbedamagedoutsidepvp cbdopvp = CurrentTreenode.Tag as Cantbedamagedoutsidepvp;
                cbdopvp.EnableDamageOnGround = EnableDamageOnGroundCB.Checked == true ? 1 : 0;
                bool edog = cbdopvp.EnableDamageOnGround == 1 ? true : false;
                CurrentTreenode.Text = $"Item Name: {cbdopvp.itemName}, Enable Damage On Ground: {edog}";
            }
            ECMItemRulesConfig.isDirty = true;
        }

        #endregion
        #region zones and shit
        public float ZoneScale = 1;
        private Point _mouseLastPosition;
        private Point _newscrollPosition;
        private Rectangle doubleClickRectangle = new Rectangle();
        private Timer doubleClickTimer = new Timer();
        private bool isFirstClick = true;
        private bool isDoubleClick = false;
        private int milliseconds = 0;
        private MouseEventArgs mouseeventargs;
        private bool isDynamicZone;
        

        public object CurrentSelectedZone { get; set; }
        public TreeNode Currentzonetag { get; set; }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Cursor.Current = Cursors.SizeAll;
                _mouseLastPosition = e.Location;
            }
            else if (e.Button == MouseButtons.Left)
            {
                mouseeventargs = e;
                // This is the first mouse click.
                if (isFirstClick)
                {
                    isFirstClick = false;

                    // Determine the location and size of the double click
                    // rectangle area to draw around the cursor point.
                    doubleClickRectangle = new Rectangle(
                        e.X - (SystemInformation.DoubleClickSize.Width / 2),
                        e.Y - (SystemInformation.DoubleClickSize.Height / 2),
                        SystemInformation.DoubleClickSize.Width,
                        SystemInformation.DoubleClickSize.Height);
                    Invalidate();

                    // Start the double click timer.
                    doubleClickTimer.Start();
                }

                // This is the second mouse click.
                else
                {
                    // Verify that the mouse click is within the double click
                    // rectangle and is within the system-defined double
                    // click period.
                    if (doubleClickRectangle.Contains(e.Location) &&
                        milliseconds < SystemInformation.DoubleClickTime)
                    {
                        isDoubleClick = true;
                    }
                }
            }
        }
        void doubleClickTimer_Tick(object sender, EventArgs e)
        {
            milliseconds += 100;

            // The timer has reached the double click time limit.
            if (milliseconds >= SystemInformation.DoubleClickTime)
            {
                doubleClickTimer.Stop();

                if (isDoubleClick)
                {
                    //Console.WriteLine("Perform double click action");
                    if (CurrentSelectedZone == null) return;
                    if (CurrentSelectedZone is Customzone)
                    {
                        Customzone currentcustomzone = CurrentSelectedZone as Customzone;
                        Cursor.Current = Cursors.WaitCursor;
                        decimal scalevalue = (decimal)ZoneScale * (decimal)0.05;
                        decimal mapsize = currentproject.MapSize;
                        int newsize = (int)(mapsize * scalevalue);
                        currentcustomzone.x = Decimal.Round((decimal)(mouseeventargs.X / scalevalue), 4);
                        currentcustomzone.z = Decimal.Round((decimal)((newsize - mouseeventargs.Y) / scalevalue), 4);
                        Cursor.Current = Cursors.Default;
                        ECMExpansionCircleMarkerConfig.isDirty = true;
                        EMCCustomZoneXNUD.Value = currentcustomzone.x;
                        EMCCustomZoneZNUD.Value = currentcustomzone.z;
                        pictureBox1.Invalidate();
                    }
                    else if (CurrentSelectedZone is Polygonzone)
                    {
                        if (Currentzonetag.Tag is Vec3)
                        {
                            Polygonzone currentpolyzone = CurrentSelectedZone as Polygonzone;
                            Cursor.Current = Cursors.WaitCursor;
                            decimal scalevalue = (decimal)ZoneScale * (decimal)0.05;
                            decimal mapsize = currentproject.MapSize;
                            int newsize = (int)(mapsize * scalevalue);
                            Vec3 v3 = Currentzonetag.Tag as Vec3;
                            v3.X = (float)Decimal.Round((decimal)(mouseeventargs.X / scalevalue), 4);
                            v3.Z = (float)Decimal.Round((decimal)((newsize - mouseeventargs.Y) / scalevalue), 4);
                            if (MapData.FileExists)
                            {
                                v3.Y = MapData.gethieght(v3.X, v3.Z);
                            }
                            Currentzonetag.Text = v3.ToString();
                            EMCPolygonVerticePOSXNUD.Value = (decimal)v3.X;
                            EMCPolygonVerticePOSYNUD.Value = (decimal)v3.Y;
                            EMCPolygonVerticePOSZNUD.Value = (decimal)v3.Z;
                            Cursor.Current = Cursors.Default;
                            ECMPolygonZonesConfig.isDirty = true;
                            pictureBox1.Invalidate();
                        }
                    }
                    else if (CurrentSelectedZone is Objectstocreatedynamiczone && isDynamicZone)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        decimal scalevalue = (decimal)ZoneScale * (decimal)0.05;
                        decimal mapsize = currentproject.MapSize;
                        int newsize = (int)(mapsize * scalevalue);
                        currentScanZone.X = Decimal.Round((decimal)(mouseeventargs.X / scalevalue), 4);
                        currentScanZone.Z = Decimal.Round((decimal)((newsize - mouseeventargs.Y) / scalevalue), 4);
                        if (MapData.FileExists)
                        {
                            currentScanZone.Y = (decimal)(MapData.gethieght((float)currentScanZone.X, (float)currentScanZone.Z));
                        }
                        Currentzonetag.Text = $"Vector3:{currentScanZone.X},{currentScanZone.Y},{currentScanZone.Z}";
                        Cursor.Current = Cursors.Default;
                        ECMDynamicPVPZoneConfig.isDirty = true;
                        pictureBox1.Invalidate();
                    }
                }
                else
                {
                    decimal scalevalue = (decimal)ZoneScale * (decimal)0.05;
                    decimal mapsize = currentproject.MapSize;
                    int newsize = (int)(mapsize * scalevalue);
                    PointF pC = new PointF((float)Decimal.Round((decimal)(mouseeventargs.X / scalevalue), 4), (float)Decimal.Round((decimal)((newsize - mouseeventargs.Y) / scalevalue), 4));

                    foreach (Customzone tz in ECMExpansionCircleMarkerConfig.CustomZones)
                    {
                        PointF pP = new PointF((float)tz.x, (float)tz.z);
                        if (IsWithinCircle(pC, pP, (float)tz.zoneRadius))
                        {
                            TreeNode customzonesparent = EMCZonesTV.Nodes["emcCustomZones"];
                            foreach (TreeNode n in customzonesparent.Nodes)
                            {
                                Customzone cz = n.Tag as Customzone;
                                if (cz == tz)
                                {
                                    EMCZonesTV.SelectedNode = n;
                                    break;
                                }
                            }
                            continue;
                        }
                    }
                    foreach (Polygonzone pz in ECMPolygonZonesConfig.PolygonZones)
                    {
                        Rectangle rect2 = getpolyrectangle(pz._vertices, 1, false);
                        if ((pC.X >= rect2.X && pC.X <= rect2.X +rect2.Width) && (pC.Y >= rect2.Y && pC.Y <= rect2.Y + rect2.Height))
                        {
                            EMCZonesTV.SelectedNode = EMCZonesTV.Nodes["emcPolygonZones"].Nodes[pz.polyzoneName];
                        }


                        foreach (Vec3 points in pz._vertices)
                        {
                            PointF pP = new PointF((float)points.X, (float)points.Z);
                            if (IsWithinCircle(pC, pP, (float)10))
                            {
                                foreach (TreeNode n in EMCZonesTV.Nodes["emcPolygonZones"].Nodes[pz.polyzoneName].Nodes["PolygonzoneVerts"].Nodes)
                                {
                                    Vec3 cz = n.Tag as Vec3;
                                    if (cz == points)
                                    {
                                        EMCZonesTV.SelectedNode = n;
                                        break;
                                    }
                                }
                                continue;
                            }
                        }
                    }
                    foreach (Objectstocreatedynamiczone dynamiczone in ECMDynamicPVPZoneConfig.ObjectsToCreateDynamicZones)
                    {
                        foreach (Scanzone sz in dynamiczone.ScanZones)
                        {
                            PointF pP = new PointF((float)sz.X, (float)sz.Z);
                            if (IsWithinCircle(pC, pP, (float)sz.Radius))
                            {
                                foreach (TreeNode n in EMCZonesTV.Nodes["emcDynamicZones"].Nodes[dynamiczone.itemName].Nodes["DynamicScanZones"].Nodes)
                                {
                                    Scanzone scanz = n.Tag as Scanzone;
                                    if (sz == scanz)
                                    {
                                        EMCZonesTV.SelectedNode = n;
                                        break;
                                    }
                                }
                                continue;
                            }
                        }
                    }
                }

                // Allow the MouseDown event handler to process clicks again.
                isFirstClick = true;
                isDoubleClick = false;
                milliseconds = 0;
            }
        }
        public bool IsWithinCircle(PointF pC, PointF pP, Single fRadius)
        {
            return Distance(pC, pP) <= fRadius;
        }
        public Single Distance(PointF p1, PointF p2)
        {
            Single dX = p1.X - p2.X;
            Single dY = p1.Y - p2.Y;
            Single multi = dX * dX + dY * dY;
            Single dist = (Single)Math.Round((Single)Math.Sqrt(multi), 3);
            return (Single)dist;
        }
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            if (pictureBox1.Focused == false)
            {
                pictureBox1.Focus();
                panel1.AutoScrollPosition = _newscrollPosition;
                pictureBox1.Invalidate();
            }
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point changePoint = new Point(e.Location.X - _mouseLastPosition.X, e.Location.Y - _mouseLastPosition.Y);
                _newscrollPosition = new Point(-panel1.AutoScrollPosition.X - changePoint.X, -panel1.AutoScrollPosition.Y - changePoint.Y);
                if (_newscrollPosition.X <= 0)
                    _newscrollPosition.X = 0;
                if (_newscrollPosition.Y <= 0)
                    _newscrollPosition.Y = 0;
                panel1.AutoScrollPosition = _newscrollPosition;
                pictureBox1.Invalidate();
            }
            decimal scalevalue = (decimal)ZoneScale * (decimal)0.05;
            decimal mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            label155.Text = Decimal.Round((decimal)(e.X / scalevalue), 4) + "," + Decimal.Round((decimal)((newsize - e.Y) / scalevalue), 4);
        }
        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                pictureBox1_ZoomOut();
            }
            else
            {
                pictureBox1_ZoomIn();
            }

        }
        private void pictureBox1_ZoomIn()
        {
            int oldpictureboxhieght = pictureBox1.Height;
            int oldpitureboxwidht = pictureBox1.Width;
            Point oldscrollpos = panel1.AutoScrollPosition;
            int tbv = trackBar1.Value;
            int newval = tbv + 1;
            if (newval >= 20)
                newval = 20;
            trackBar1.Value = newval;
            ZoneScale = trackBar1.Value;
            SetECMZonescale();
            if (pictureBox1.Height > panel1.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox1.Height * newy);
                _newscrollPosition.Y = y * -1;
                panel1.AutoScrollPosition = _newscrollPosition;
            }
            if (pictureBox1.Width > panel1.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox1.Width * newy);
                _newscrollPosition.X = x * -1;
                panel1.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox1.Invalidate();
        }
        private void pictureBox1_ZoomOut()
        {
            int oldpictureboxhieght = pictureBox1.Height;
            int oldpitureboxwidht = pictureBox1.Width;
            Point oldscrollpos = panel1.AutoScrollPosition;
            int tbv = trackBar1.Value;
            int newval = tbv - 1;
            if (newval <= 1)
                newval = 1;
            trackBar1.Value = newval;
            ZoneScale = trackBar1.Value;
            SetECMZonescale();
            if (pictureBox1.Height > panel1.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox1.Height * newy);
                _newscrollPosition.Y = y * -1;
                panel1.AutoScrollPosition = _newscrollPosition;
            }
            if (pictureBox1.Width > panel1.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox1.Width * newy);
                _newscrollPosition.X = x * -1;
                panel1.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox1.Invalidate();
        }
        private void SetECMZonescale()
        {
            float scalevalue = ZoneScale * 0.05f;
            float mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            pictureBox1.Size = new Size(newsize, newsize);
        }
        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            ZoneScale = trackBar1.Value;
            SetECMZonescale();
        }
        private void DrawECMZones(object sender, PaintEventArgs e)
        {
            foreach (Customzone radiuszone in ECMExpansionCircleMarkerConfig.CustomZones)
            {
                float scalevalue = ZoneScale * 0.05f;
                int centerX = (int)(Math.Round((float)radiuszone.x) * scalevalue);
                int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round((float)radiuszone.z, 0) * scalevalue);
                int radius = (int)(Math.Round((float)radiuszone.zoneRadius, 0) * scalevalue);
                Point center = new Point(centerX, centerY);
                Pen pen = new Pen(Color.Red, 1);
                if (radiuszone == CurrentSelectedZone as Customzone)
                {
                    Rectangle rect2 = new Rectangle(center.X - radius, center.Y - radius, radius * 2, radius * 2);
                    getSquare(e.Graphics, pen, rect2);
                }
                pen.Color = Color.FromArgb(radiuszone.zoneAlpha, radiuszone.zoneRed, radiuszone.zoneGreen, radiuszone.zoneBlue);
                getCircle(e.Graphics, pen, center, radius);
            }
            foreach (Polygonzone pz in ECMPolygonZonesConfig.PolygonZones)
            {
                if (pz._vertices.Count > 1)
                {
                    float scalevalue = ZoneScale * 0.05f;
                    for (int i = 0; i < pz._vertices.Count; i++)
                    {
                        int ax = (int)(Math.Round(pz._vertices[i].X, 0) * scalevalue);
                        int ay = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(pz._vertices[i].Z, 0) * scalevalue);
                        Pen pointpen = new Pen(Color.Purple, 5);
                        if (pz._vertices[i] == Currentzonetag.Tag as Vec3)
                            pointpen.Color = Color.Blue;
                        Rectangle rect = new Rectangle(ax -2 , ay - 2, 5, 5);
                        e.Graphics.DrawEllipse(pointpen, rect);
                    }
                    Pen pen = new Pen(Color.Purple, 1);
                    
                    for (int i = 0; i < pz._vertices.Count; i++)
                    {
                        int ax = (int)(Math.Round(pz._vertices[i].X, 0) * scalevalue);
                        int ay = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(pz._vertices[i].Z, 0) * scalevalue);
                        int bx = 0;
                        int by = 0;
                        if (i == pz._vertices.Count - 1)
                        {
                            bx = (int)(Math.Round(pz._vertices[0].X, 0) * scalevalue);
                            by = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(pz._vertices[0].Z, 0) * scalevalue);
                        }
                        else
                        {
                            bx = (int)(Math.Round(pz._vertices[i + 1].X, 0) * scalevalue);
                            by = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(pz._vertices[i + 1].Z, 0) * scalevalue);
                        }
                        
                        pen.Color = Color.FromArgb(pz.polyzoneAlpha, pz.polyzoneRed, pz.polyzoneGreen, pz.polyzoneBlue);
                        e.Graphics.DrawLine(pen, ax, ay, bx, by);
                        
                    }
                    
                    if (pz == CurrentSelectedZone as Polygonzone)
                    {
                        Rectangle rect2 = getpolyrectangle(pz._vertices, scalevalue);
                        getSquare(e.Graphics, pen, rect2);
                    }
                    
                }
            }
            foreach (Objectstocreatedynamiczone objectzone in ECMDynamicPVPZoneConfig.ObjectsToCreateDynamicZones)
            {
                float scalevalue = ZoneScale * 0.05f;
                foreach (Scanzone sz in objectzone.ScanZones)
                {
                    int centerX = (int)(Math.Round((float)sz.X) * scalevalue);
                    int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round((float)sz.Z, 0) * scalevalue);
                    int radius = (int)(Math.Round((float)sz.Radius, 0) * scalevalue);
                    Point center = new Point(centerX, centerY);
                    Pen pen = new Pen(Color.Red, 1);
                    if (sz == currentScanZone && isDynamicZone)
                    {
                        Rectangle rect2 = new Rectangle(center.X - radius, center.Y - radius, radius * 2, radius * 2);
                        getSquare(e.Graphics, pen, rect2);
                    }
                    pen.Color = Color.FromArgb(objectzone.zoneAlpha, objectzone.zoneRed, objectzone.zoneGreen, objectzone.zoneBlue);
                    getCircle(e.Graphics, pen, center, radius);
                }
            }
        }
        public Rectangle getpolyrectangle(BindingList<Vec3> points, float scalevalue, bool invert = true)
        {
            // Add checks here, if necessary, to make sure that points is not null,
            // and that it contains at least one (or perhaps two?) elements

            var minX = points.Min(p => p.X);
            var minZ = points.Min(p => p.Z);
            var maxX = points.Max(p => p.X);
            var maxZ = points.Max(p => p.Z);

            if (invert)
            {
                int minx = (int)(Math.Round((float)minX) * scalevalue);
                int minz = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round((float)maxZ, 0) * scalevalue);
                int maxx = (int)(Math.Round((float)maxX) * scalevalue);
                int maxz = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round((float)minZ, 0) * scalevalue);

                return new Rectangle(new Point(minx, minz), new Size(maxx - minx, maxz - minz));
            }
            return new Rectangle(new Point((int)minX, (int)minZ), new Size((int)maxX - (int)minX, (int)maxZ - (int)minZ));
        }
        private void getSquare(Graphics drawingArea, Pen penToUse, Rectangle rect2)
        {
            Pen pen = new Pen(Color.LimeGreen)
            {
                Width = 2,
                DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
            };
            drawingArea.DrawRectangle(pen, rect2);
        }
        private void getCircle(Graphics drawingArea, Pen penToUse, Point center, int radius)
        {
            Rectangle rect = new Rectangle(center.X - 1, center.Y - 1, 2, 2);
            drawingArea.DrawEllipse(penToUse, rect);
            Rectangle rect2 = new Rectangle(center.X - radius, center.Y - radius, radius * 2, radius * 2);
            drawingArea.DrawEllipse(penToUse, rect2);
        }

        private void setupgeneralsettings()
        {
            useraction = false;
            PvPeverywhereCB.Checked = ECMExpansionCircleMarkerConfig.PvPeverywhere == 1 ? true : false;
            ReflectDamageCB.Checked = ECMExpansionCircleMarkerConfig.ReflectDamage == 1 ? true : false;
            BlockChemGasGrenadeOutsidePvPCB.Checked = ECMExpansionCircleMarkerConfig.BlockChemGasGrenadeOutsidePvP == 1 ? true : false;
            PvPIconPathTB.Text = ECMExpansionCircleMarkerConfig.PvPIconPath;
            m_PvPIconXNUD.Value = ECMExpansionCircleMarkerConfig.m_PvPIconX;
            m_PvPIconYNUD.Value = ECMExpansionCircleMarkerConfig.m_PvPIconY;
            PvPExitCountdownNUD.Value = ECMExpansionCircleMarkerConfig.PvPExitCountdown;
            DisableExpansionGroupsFriendlyFireCB.Checked = ECMExpansionCircleMarkerConfig.DisableExpansionGroupsFriendlyFire == 1 ? true : false;
            DisableExpansionGroupsFriendlyFireItemDamageCB.Checked = ECMExpansionCircleMarkerConfig.DisableExpansionGroupsFriendlyFireItemDamage == 1 ? true : false;
            PvPForceFirstPersonCB.Checked = ECMExpansionCircleMarkerConfig.PvPForceFirstPerson == 1 ? true : false;
            ForceFirstPersonInVehicleCB.Checked = ECMExpansionCircleMarkerConfig.ForceFirstPersonInVehicle == 1 ? true : false;
            ThirdPersonOnlyDriverAndCoDriverCB.Checked = ECMExpansionCircleMarkerConfig.ThirdPersonOnlyDriverAndCoDriver == 1 ? true : false;
            ScheduleZoneCheckIntervalNUD.Value = ECMExpansionCircleMarkerConfig.ScheduleZoneCheckInterval;
            PlayerZoneCheckIntervalNUD.Value = ECMExpansionCircleMarkerConfig.PlayerZoneCheckInterval;
            DisablePlayerZoneCheckIntervalCB.Checked = ECMExpansionCircleMarkerConfig.DisablePlayerZoneCheckInterval  == 1 ? true : false;
            PlayerMinMoveDistanceNUD.Value = ECMExpansionCircleMarkerConfig.PlayerMinMoveDistance;
            DisablePlayerMinMoveDistanceCB.Checked = ECMExpansionCircleMarkerConfig.DisablePlayerMinMoveDistance == 1 ? true : false;
            AllowAIToDoDamageEverywhereCB.Checked = ECMExpansionCircleMarkerConfig.AllowAIToDoDamageEverywhere == 1 ? true : false;
            OnlyAllowAIToDoDamageIfPlayerIsPvPCB.Checked = ECMExpansionCircleMarkerConfig.OnlyAllowAIToDoDamageIfPlayerIsPvP == 1 ? true : false;
            AllowDamageToAIEverywhereCB.Checked = ECMExpansionCircleMarkerConfig.AllowDamageToAIEverywhere == 1 ? true : false;
            AllowDamageToAIOnlyIfPlayerHasPvPStatusCB.Checked = ECMExpansionCircleMarkerConfig.AllowDamageToAIOnlyIfPlayerHasPvPStatus == 1 ? true : false;
            EnablePvPZoneCreationWhileLockpickingVehicleCB.Checked = ECMExpansionCircleMarkerConfig.EnablePvPZoneCreationWhileLockpickingVehicle == 1 ? true : false;
            EnableLockpickingBroadcastCB.Checked = ECMExpansionCircleMarkerConfig.EnableLockpickingBroadcast == 1 ? true : false;
            LockpickingBroadcastRadiusNUD.Value = ECMExpansionCircleMarkerConfig.LockpickingBroadcastRadius;
            EnableTerritoryFlagPvPZonesCB.Checked = ECMExpansionCircleMarkerConfig.EnableTerritoryFlagPvPZones == 1 ? true : false;
            TerritoryFlagPvPZoneCheckIntervalNUD.Value = ECMExpansionCircleMarkerConfig.TerritoryFlagPvPZoneCheckInterval;
            TerritoryFlagPvPRadiusNUD.Value = ECMExpansionCircleMarkerConfig.TerritoryFlagPvPRadius;
            useraction = true;
        }
        public void setupzonesandshit()
        {
            EMCZonesTV.Nodes.Clear();
            TreeNode emcCustomZones = new TreeNode("Radius Zones")
            {
                Name = "emcCustomZones",
                Tag = "emcCustomZones"
            };
            foreach (Customzone radiuszone in ECMExpansionCircleMarkerConfig.CustomZones)
            {
                TreeNode radiuszoneTN = new TreeNode($"Zone Name: {radiuszone.zoneName}")
                {
                    Tag = radiuszone
                };
                TreeNode radiusTN = new TreeNode($"Radius: {radiuszone.zoneRadius}")
                {
                    Tag = "CustomZoneRadius"
                };
                radiuszoneTN.Nodes.Add(radiusTN);
                TreeNode ColourTN = new TreeNode($"RGBA Colour: {radiuszone.zoneAlpha},{radiuszone.zoneRed},{radiuszone.zoneGreen},{radiuszone.zoneBlue}")
                {
                    Tag = "CustomZoneMarkerColour"
                };
                radiuszoneTN.Nodes.Add(ColourTN);
                emcCustomZones.Nodes.Add(radiuszoneTN);
            }
            EMCZonesTV.Nodes.Add(emcCustomZones);
            TreeNode emcPolygonZones = new TreeNode("Polygon Zones")
            {
                Name = "emcPolygonZones",
                Tag = "emcPolygonZones"
            };
            foreach (Polygonzone Polygonzone in ECMPolygonZonesConfig.PolygonZones)
            {
                TreeNode PolygonzoneTN = new TreeNode($"Zone Name: {Polygonzone.polyzoneName}")
                {
                    Tag = Polygonzone,
                    Name = Polygonzone.polyzoneName
                };
                TreeNode polyverts = new TreeNode("Vertices")
                {
                    Tag = "PolygonzoneVerts",
                    Name = "PolygonzoneVerts"
                };
                for (int i = 0; i < Polygonzone._vertices.Count; i++)
                {
                    TreeNode vert = new TreeNode($"{Polygonzone._vertices[i]}")
                    {
                        Tag = Polygonzone._vertices[i]
                    };
                    polyverts.Nodes.Add(vert);
                };
                PolygonzoneTN.Nodes.Add(polyverts);
                TreeNode ColourTN = new TreeNode($"RGBA Colour: {Polygonzone.polyzoneAlpha},{Polygonzone.polyzoneRed},{Polygonzone.polyzoneGreen},{Polygonzone.polyzoneBlue}")
                {
                    Tag = "PolygonZoneMarkerColour"
                };
                PolygonzoneTN.Nodes.Add(ColourTN);
                emcPolygonZones.Nodes.Add(PolygonzoneTN);
            }
            EMCZonesTV.Nodes.Add(emcPolygonZones);
            TreeNode ObjectsToCreateDynamicZones = new TreeNode("Dynamic Zones")
            {
                Tag = "emcDynamicZones",
                Name = "emcDynamicZones"
            };
            foreach (Objectstocreatedynamiczone dynamiczone in ECMDynamicPVPZoneConfig.ObjectsToCreateDynamicZones)
            {
                TreeNode dynamiczoneTN = new TreeNode($"Zone Name: {dynamiczone.itemName}")
                {
                    Tag = dynamiczone,
                    Name= dynamiczone.itemName
                };
                TreeNode dynamicscanzonesTN = new TreeNode($"Scan Zones")
                {
                    Tag = "DynamicScanZones",
                    Name = "DynamicScanZones"
                };
                foreach (Scanzone sz in dynamiczone.ScanZones)
                {
                    TreeNode dynamiczonesTN = new TreeNode($"Vector3:{sz.X},{sz.Y},{sz.Z}")
                    {
                        Tag = sz
                    };
                    TreeNode radiusTN = new TreeNode($"Radius: {sz.Radius}")
                    {
                        Tag = "DynamicZoneRadius"
                    };
                    dynamiczonesTN.Nodes.Add(radiusTN);
                    dynamicscanzonesTN.Nodes.Add(dynamiczonesTN);
                }
                dynamiczoneTN.Nodes.Add(dynamicscanzonesTN);
                TreeNode ColourTN = new TreeNode($"RGBA Colour: {dynamiczone.zoneAlpha},{dynamiczone.zoneRed},{dynamiczone.zoneGreen},{dynamiczone.zoneBlue}")
                {
                    Tag = "DynamicZoneMarkerColour"
                };
                dynamiczoneTN.Nodes.Add(ColourTN);
                ObjectsToCreateDynamicZones.Nodes.Add(dynamiczoneTN);
            }
            EMCZonesTV.Nodes.Add(ObjectsToCreateDynamicZones);
            EMCZonesTV.Nodes.Add(emcPolygonZones);
            TreeNode Statichelizones = new TreeNode("Static event Zones")
            {
                Tag = "emcStaticEventZones",
                Name = "emcStaticEventZones"
            };
            TreeNode Mi8Node = new TreeNode("Mi8 Crash")
            {
               Tag = ECMDynamicPVPZoneConfig.Mi8,
            };
            Statichelizones.Nodes.Add(Mi8Node);
            TreeNode UH1YNode = new TreeNode("UH1Y Crash")
            {
                Tag = ECMDynamicPVPZoneConfig.UH1Y,
            };
            Statichelizones.Nodes.Add(UH1YNode);
            TreeNode EADNode = new TreeNode("Expansion Airdrop")
            {
                Tag = ECMExpansionCircleMarkerConfig.AIRDROP_ZONES,
            };
            Statichelizones.Nodes.Add(EADNode);
            EMCZonesTV.Nodes.Add(Statichelizones);
        }
        private void EMCZonesTV_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Currentzonetag = e.Node;
            EMCZonesTV.SelectedNode = e.Node;
            addNewPointToolStripMenuItem.Visible = false;
            removePointToolStripMenuItem.Visible = false;
            addNewCustomToolStripMenuItem.Visible = false;
            removeCustomZoneToolStripMenuItem.Visible = false;
            addNewPolygonZoneToolStripMenuItem.Visible = false;
            removePolygonZoneToolStripMenuItem.Visible = false;
            addNewDynamicObjectZoneToolStripMenuItem.Visible = false;
            removeDynamicObjectZoneToolStripMenuItem.Visible= false;
            addNewScanZoneToolStripMenuItem.Visible = false;
            removeScanZoneToolStripMenuItem.Visible = false;
            if (e.Button == MouseButtons.Right)
            {
                if (e.Node.Tag.ToString() == "emcCustomZones")
                {
                    addNewCustomToolStripMenuItem.Visible = true;
                    contextMenuStrip2.Show(Cursor.Position);
                }
                else if (e.Node.Tag is Customzone)
                {
                    removeCustomZoneToolStripMenuItem.Visible = true;
                    contextMenuStrip2.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "emcPolygonZones")
                {
                    addNewPolygonZoneToolStripMenuItem.Visible = true;
                    contextMenuStrip2.Show(Cursor.Position);
                }
                else if (e.Node.Tag is Polygonzone)
                {
                    removePolygonZoneToolStripMenuItem.Visible = true;
                    contextMenuStrip2.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "PolygonzoneVerts")
                {
                    addNewPointToolStripMenuItem.Visible = true;
                    contextMenuStrip2.Show(Cursor.Position);
                }
                else if (e.Node.Tag is Vec3)
                {
                    removePointToolStripMenuItem.Visible = true;
                    contextMenuStrip2.Show(Cursor.Position);
                }
                else if (e.Node.Tag.ToString() == "emcDynamicZones")
                {
                    addNewDynamicObjectZoneToolStripMenuItem.Visible = true;
                    contextMenuStrip2.Show(Cursor.Position);
                }
                else if (e.Node.Tag is Objectstocreatedynamiczone)
                {
                    removeDynamicObjectZoneToolStripMenuItem.Visible = true;
                    contextMenuStrip2.Show(Cursor.Position);
                }
                else if(e.Node.Tag.ToString() == "DynamicScanZones")
                {
                    addNewScanZoneToolStripMenuItem.Visible = true;
                    contextMenuStrip2.Show(Cursor.Position);
                }
                else if (e.Node.Tag is Scanzone)
                {
                    removeScanZoneToolStripMenuItem.Visible = true;
                    contextMenuStrip2.Show(Cursor.Position);
                }
            }
        }
        private void addNewPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Polygonzone pz = Currentzonetag.Parent.Tag as Polygonzone;
            float scalevalue = ZoneScale * 0.05f;
            Rectangle rect2 = getpolyrectangle(pz._vertices, scalevalue, false);
            Vec3 v = new Vec3(rect2.X, 0, rect2.Y);
            pz._vertices.Add(v);
            TreeNode vert = new TreeNode($"{v}")
            {
                Tag = v
            };
            Currentzonetag.Nodes.Add(vert);
            ECMPolygonZonesConfig.isDirty = true;
            pictureBox1.Invalidate();
        }
        private void removePointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Vec3 v = Currentzonetag.Tag as Vec3;
            Polygonzone pz = Currentzonetag.Parent.Parent.Tag as Polygonzone;
            pz._vertices.Remove(v);
            Currentzonetag.Parent.Nodes.Remove(Currentzonetag);
            ECMPolygonZonesConfig.isDirty = true;
            pictureBox1.Invalidate();
        }
        private void EMCZonesTV_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Currentzonetag = e.Node;
            useraction = false;
            helicrashGB.Visible = false;
            groupBox2.Visible = false;
            groupBox3.Visible = false;
            EMCCustomZonesPointGB.Visible = false;
            EMCDynamicscanZonesGB.Visible = false;
            EMCPolygonPointsGB.Visible = false;
            EMCZonesMessagesGB.Visible = false;
            EMCZonesAllowedWeaponsGB.Visible = false;
            isDynamicZone = false;
            if (Currentzonetag.Tag.ToString() == "emcCustomZones" ||
               Currentzonetag.Tag.ToString() == "emcPolygonZones" ||
               Currentzonetag.Tag.ToString() == "emcDynamicZones" ||
               Currentzonetag.Tag.ToString() == "emcStaticEventZones")
            {
                return;
            }
            else if (Currentzonetag.Tag is Customzone ||
                     Currentzonetag.Parent.Tag is Customzone)
            {
                if (Currentzonetag.Tag is Customzone)
                    CurrentSelectedZone = Currentzonetag.Tag as Customzone;
                else if (Currentzonetag.Parent.Tag is Customzone)
                    CurrentSelectedZone = Currentzonetag.Parent.Tag as Customzone;
                useraction = false;
                groupBox2.Visible = true;
                EMCZoneNameTB.Text = (CurrentSelectedZone as Customzone).zoneName;
                EMCZoneDrawCB.Checked = (CurrentSelectedZone as Customzone).drawCircle == 1 ? true : false;
                EMCZoneTypeCB.SelectedIndex = (CurrentSelectedZone as Customzone).isPvPZone;
                EMCCustomZonesPointGB.Visible = true;
                EMCCustomZoneXNUD.Value = (CurrentSelectedZone as Customzone).x;
                EMCCustomZoneZNUD.Value = (CurrentSelectedZone as Customzone).z;
                EMCCustomZoneRadiusNUD.Value = (CurrentSelectedZone as Customzone).zoneRadius;
                groupBox3.Visible = true;
                EMCZoneStartHourNUD.Value = (CurrentSelectedZone as Customzone).ZoneSchedule.StartHour;
                EMCZoneEndHourNUD.Value = (CurrentSelectedZone as Customzone).ZoneSchedule.EndHour;
                string[] days = (CurrentSelectedZone as Customzone).ZoneSchedule.Days.Split(' ');
                SundayDOWCB.Checked = false;
                MondayDOWCB.Checked = false;
                TuesdayDOWCB.Checked = false;
                WednesdayDOWCB.Checked = false;
                ThursdayDOWCB.Checked = false;
                FridayDOWCB.Checked = false;
                SaturdayDOWCB.Checked = false;

                foreach (string dayofweek in days)
                {
                    switch (dayofweek)
                    {
                        case "Sunday":
                            SundayDOWCB.Checked = true;
                            break;
                        case "Monday":
                            MondayDOWCB.Checked = true;
                            break;
                        case "Tuesday":
                            TuesdayDOWCB.Checked = true;
                            break;
                        case "Wednesday":
                            WednesdayDOWCB.Checked = true;
                            break;
                        case "Thursday":
                            ThursdayDOWCB.Checked = true;
                            break;
                        case "Friday":
                            FridayDOWCB.Checked = true;
                            break;
                        case "Saturday":
                            SaturdayDOWCB.Checked = true;
                            break;
                    }
                }
                EMCZonesMessagesGB.Visible = true;
                EMCEnableCustomMessagesCB.Checked = (CurrentSelectedZone as Customzone).EnableCustomMessages == 1 ? true : false;
                EMCCustomTitleTB.Text = (CurrentSelectedZone as Customzone).CustomTitle;
                EMCCustomMessageEnterTB.Text = (CurrentSelectedZone as Customzone).CustomMessageEnter;
                EMCCustomMessageExitTB.Text = (CurrentSelectedZone as Customzone).CustomMessageExit;
                EMCCustomIconTB.Text = (CurrentSelectedZone as Customzone).CustomIcon;
                EMCZonesAllowedWeaponsGB.Visible = true;
                EMCZonesAllowedWeaponsLB.DataSource = (CurrentSelectedZone as Customzone).OnlyAllowedWeapons;

            }
            else if (Currentzonetag.Tag is Polygonzone ||
                     Currentzonetag.Tag is Vec3 ||
                     Currentzonetag.Parent.Tag is Polygonzone)
            {
                if (Currentzonetag.Tag is Polygonzone)
                    CurrentSelectedZone = Currentzonetag.Tag as Polygonzone;
                else if (Currentzonetag.Tag is Vec3)
                {
                    EMCPolygonZonesVerticeListLB.SelectedItem = Currentzonetag.Tag as Vec3;
                    CurrentSelectedZone = Currentzonetag.Parent.Parent.Tag as Polygonzone;
                }
                else if (Currentzonetag.Parent.Tag is Polygonzone)
                    CurrentSelectedZone = Currentzonetag.Parent.Tag as Polygonzone;

                useraction = false;
                groupBox2.Visible = true;
                EMCZoneNameTB.Text = (CurrentSelectedZone as Polygonzone).polyzoneName;
                EMCZoneDrawCB.Checked = (CurrentSelectedZone as Polygonzone).polydrawPolygon == 1 ? true : false;
                EMCZoneTypeCB.SelectedIndex = (CurrentSelectedZone as Polygonzone).polyisPvPZone;
                groupBox3.Visible = true;
                EMCZoneStartHourNUD.Value = (CurrentSelectedZone as Polygonzone).ZoneSchedule.StartHour;
                EMCZoneEndHourNUD.Value = (CurrentSelectedZone as Polygonzone).ZoneSchedule.EndHour;
                string[] days = (CurrentSelectedZone as Polygonzone).ZoneSchedule.Days.Split(' ');
                foreach (string dayofweek in days)
                {
                    switch (dayofweek)
                    {
                        case "Sunday":
                            SundayDOWCB.Checked = true;
                            break;
                        case "Monday":
                            MondayDOWCB.Checked = true;
                            break;
                        case "Tuesday":
                            TuesdayDOWCB.Checked = true;
                            break;
                        case "Wednesday":
                            WednesdayDOWCB.Checked = true;
                            break;
                        case "Thursday":
                            ThursdayDOWCB.Checked = true;
                            break;
                        case "Friday":
                            FridayDOWCB.Checked = true;
                            break;
                        case "Saturday":
                            SaturdayDOWCB.Checked = true;
                            break;
                    }
                }
                EMCZonesMessagesGB.Visible = true;
                EMCEnableCustomMessagesCB.Checked = (CurrentSelectedZone as Polygonzone).EnableCustomMessages == 1 ? true : false;
                EMCCustomTitleTB.Text = (CurrentSelectedZone as Polygonzone).CustomTitle;
                EMCCustomMessageEnterTB.Text = (CurrentSelectedZone as Polygonzone).CustomMessageEnter;
                EMCCustomMessageExitTB.Text = (CurrentSelectedZone as Polygonzone).CustomMessageExit;
                EMCCustomIconTB.Text = (CurrentSelectedZone as Polygonzone).CustomIcon;
                EMCPolygonPointsGB.Visible = true;
                EMCZonesAllowedWeaponsGB.Visible = true;
                EMCZonesAllowedWeaponsLB.DataSource = (CurrentSelectedZone as Polygonzone).OnlyAllowedWeapons;
                EMCPolygonZonesVerticeListLB.DataSource = (CurrentSelectedZone as Polygonzone)._vertices;
            }
            else if (Currentzonetag.Tag is Objectstocreatedynamiczone ||
                     Currentzonetag.Parent.Tag is Objectstocreatedynamiczone ||
                     Currentzonetag.Tag is Scanzone ||
                     (Currentzonetag.Parent.Parent != null && Currentzonetag.Parent.Parent.Parent != null &&
                     Currentzonetag.Parent.Parent.Parent.Tag is Objectstocreatedynamiczone))
            {
                isDynamicZone = true;
                if (Currentzonetag.Tag is Objectstocreatedynamiczone)
                    CurrentSelectedZone = Currentzonetag.Tag as Objectstocreatedynamiczone;
                else if (Currentzonetag.Parent.Tag is Objectstocreatedynamiczone)
                    CurrentSelectedZone = Currentzonetag.Parent.Tag as Objectstocreatedynamiczone;
                else if (Currentzonetag.Tag is Scanzone)
                {
                    currentScanZone = Currentzonetag.Tag as Scanzone;
                    EMCDynamicZonesScanZonesLB.SelectedItem = currentScanZone;
                    CurrentSelectedZone = Currentzonetag.Parent.Parent.Tag as Objectstocreatedynamiczone;
                }
                else if (Currentzonetag.Parent.Parent.Parent.Tag is Objectstocreatedynamiczone)
                {
                    currentScanZone = Currentzonetag.Parent.Tag as Scanzone;
                    EMCDynamicZonesScanZonesLB.SelectedItem = currentScanZone;
                    CurrentSelectedZone = Currentzonetag.Parent.Parent.Parent.Tag as Objectstocreatedynamiczone;
                }

                useraction = false;
                groupBox2.Visible = true;
                EMCZoneNameTB.Text = (CurrentSelectedZone as Objectstocreatedynamiczone).itemName;
                EMCZoneDrawCB.Checked = (CurrentSelectedZone as Objectstocreatedynamiczone).drawCircle == 1 ? true : false;
                EMCZoneTypeCB.SelectedIndex = (CurrentSelectedZone as Objectstocreatedynamiczone).isPvPZone;
                groupBox3.Visible = true;
                EMCZoneStartHourNUD.Value = (CurrentSelectedZone as Objectstocreatedynamiczone).ZoneSchedule.StartHour;
                EMCZoneEndHourNUD.Value = (CurrentSelectedZone as Objectstocreatedynamiczone).ZoneSchedule.EndHour;
                string[] days = (CurrentSelectedZone as Objectstocreatedynamiczone).ZoneSchedule.Days.Split(' ');
                foreach (string dayofweek in days)
                {
                    switch (dayofweek)
                    {
                        case "Sunday":
                            SundayDOWCB.Checked = true;
                            break;
                        case "Monday":
                            MondayDOWCB.Checked = true;
                            break;
                        case "Tuesday":
                            TuesdayDOWCB.Checked = true;
                            break;
                        case "Wednesday":
                            WednesdayDOWCB.Checked = true;
                            break;
                        case "Thursday":
                            ThursdayDOWCB.Checked = true;
                            break;
                        case "Friday":
                            FridayDOWCB.Checked = true;
                            break;
                        case "Saturday":
                            SaturdayDOWCB.Checked = true;
                            break;
                    }
                }
                EMCDynamicscanZonesGB.Visible = true;
                EMCDynamicZonesScanZonesLB.DataSource = (CurrentSelectedZone as Objectstocreatedynamiczone).ScanZones;
            }
            else if (Currentzonetag.Tag is Mi8 ||
                     Currentzonetag.Tag is UH1Y ||
                     Currentzonetag.Tag is AIRDROP_ZONES)
            {
                useraction = false;
                helicrashGB.Visible = true;
                if(Currentzonetag.Tag is Mi8)
                {
                    CurrentSelectedZone = Currentzonetag.Tag as Mi8;
                    HeliCrashEnabledCB.Checked = ECMDynamicPVPZoneConfig.Mi8_Crashed == 1 ? true : false;
                    checkBox1.Checked = ECMDynamicPVPZoneConfig.Mi8.drawCircle == 1 ? true : false;
                    numericUpDown1.Value = ECMDynamicPVPZoneConfig.Mi8.Radius;
                    comboBox1.SelectedIndex = ECMDynamicPVPZoneConfig.Mi8.isPvPZone;
                }
                else if (Currentzonetag.Tag is UH1Y)
                {
                    CurrentSelectedZone = Currentzonetag.Tag as UH1Y;
                    HeliCrashEnabledCB.Checked = ECMDynamicPVPZoneConfig.UH1Y_Crashed == 1 ? true : false;
                    checkBox1.Checked = ECMDynamicPVPZoneConfig.UH1Y.drawCircle == 1 ? true : false;
                    numericUpDown1.Value = ECMDynamicPVPZoneConfig.UH1Y.Radius;
                    comboBox1.SelectedIndex = ECMDynamicPVPZoneConfig.UH1Y.isPvPZone;
                }
                else if (Currentzonetag.Tag is AIRDROP_ZONES)
                {
                    CurrentSelectedZone = Currentzonetag.Tag as AIRDROP_ZONES;
                    HeliCrashEnabledCB.Checked = true;
                    checkBox1.Checked = (CurrentSelectedZone as AIRDROP_ZONES).drawCircle == 1 ? true : false;
                    numericUpDown1.Value = (CurrentSelectedZone as AIRDROP_ZONES).Radius;
                    comboBox1.SelectedIndex = (CurrentSelectedZone as AIRDROP_ZONES).isPvPZone;
                }
            }
            CategorycolourPB.Invalidate();
            pictureBox1.Invalidate();
            useraction = true;
        }
        private void StaticPatrolWayPointsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EMCPolygonZonesVerticeListLB.SelectedItems.Count == 0) { return; }
            useraction = false;
            Vec3 vec3 = EMCPolygonZonesVerticeListLB.SelectedItem as Vec3;
            EMCPolygonVerticePOSXNUD.Value = (decimal)vec3.X;
            EMCPolygonVerticePOSYNUD.Value = (decimal)vec3.Y;
            EMCPolygonVerticePOSZNUD.Value = (decimal)vec3.Z;
            var result = EMCZonesTV.Nodes["emcPolygonZones"].Nodes.Descendants().Where(x => (x.Tag as Vec3) ==vec3 ).FirstOrDefault();
            if (result != null)
                EMCZonesTV.SelectedNode = result;
            useraction = true;
        }
        private void EMCZonesTV_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node.Tag.ToString() != "CustomZoneRadius" &&
                e.Node.Tag.ToString() != "DynamicZoneRadius")
                e.CancelEdit = true;
        }
        private void EMCZonesTV_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag.ToString() == "CustomZoneRadius" ||
                e.Node.Tag.ToString() == "DynamicZoneRadius")
                e.Node.BeginEdit();
            else if (e.Node.Tag.ToString() == "CustomZoneMarkerColour")
            {
                ColorPickerDialog cpick = new ColorPickerDialog
                {
                    StartPosition = FormStartPosition.CenterParent
                };
                Color colour = Color.Black;
                if (CurrentSelectedZone is Customzone)
                {
                    colour = Color.FromArgb(
                        (CurrentSelectedZone as Customzone).zoneAlpha,
                        (CurrentSelectedZone as Customzone).zoneRed,
                        (CurrentSelectedZone as Customzone).zoneGreen,
                        (CurrentSelectedZone as Customzone).zoneBlue
                        );
                    cpick.Color = colour;
                    if (cpick.ShowDialog() == DialogResult.OK)
                    {
                        (CurrentSelectedZone as Customzone).zoneAlpha = cpick.Color.A;
                        (CurrentSelectedZone as Customzone).zoneRed = cpick.Color.R;
                        (CurrentSelectedZone as Customzone).zoneGreen = cpick.Color.G;
                        (CurrentSelectedZone as Customzone).zoneBlue = cpick.Color.B;
                        CategorycolourPB.Invalidate();
                        ECMExpansionCircleMarkerConfig.isDirty = true;
                        Currentzonetag.Text = $"RGBA Colour: {(CurrentSelectedZone as Customzone).zoneAlpha},{(CurrentSelectedZone as Customzone).zoneRed},{(CurrentSelectedZone as Customzone).zoneGreen},{(CurrentSelectedZone as Customzone).zoneBlue}";
                    }
                }
            }
            else if (e.Node.Tag.ToString() == "PolygonZoneMarkerColour")
            {
                ColorPickerDialog cpick = new ColorPickerDialog
                {
                    StartPosition = FormStartPosition.CenterParent
                };
                Color colour = Color.Black;
                colour = Color.FromArgb(
                    (CurrentSelectedZone as Polygonzone).polyzoneAlpha,
                    (CurrentSelectedZone as Polygonzone).polyzoneRed,
                    (CurrentSelectedZone as Polygonzone).polyzoneGreen,
                    (CurrentSelectedZone as Polygonzone).polyzoneBlue
                    );
                cpick.Color = colour;
                if (cpick.ShowDialog() == DialogResult.OK)
                {
                    (CurrentSelectedZone as Polygonzone).polyzoneAlpha = cpick.Color.A;
                    (CurrentSelectedZone as Polygonzone).polyzoneRed = cpick.Color.R;
                    (CurrentSelectedZone as Polygonzone).polyzoneGreen = cpick.Color.G;
                    (CurrentSelectedZone as Polygonzone).polyzoneBlue = cpick.Color.B;
                    CategorycolourPB.Invalidate();
                    Currentzonetag.Text = $"RGBA Colour: {(CurrentSelectedZone as Polygonzone).polyzoneAlpha},{(CurrentSelectedZone as Polygonzone).polyzoneRed},{(CurrentSelectedZone as Polygonzone).polyzoneGreen},{(CurrentSelectedZone as Polygonzone).polyzoneBlue}";
                    ECMPolygonZonesConfig.isDirty = true;
                }
            }
            else if (e.Node.Tag.ToString() == "DynamicZoneMarkerColour")
            {
                ColorPickerDialog cpick = new ColorPickerDialog
                {
                    StartPosition = FormStartPosition.CenterParent
                };
                Color colour = Color.Black;
                if (CurrentSelectedZone is Objectstocreatedynamiczone)
                {
                    colour = Color.FromArgb(
                        (CurrentSelectedZone as Objectstocreatedynamiczone).zoneAlpha,
                        (CurrentSelectedZone as Objectstocreatedynamiczone).zoneRed,
                        (CurrentSelectedZone as Objectstocreatedynamiczone).zoneGreen,
                        (CurrentSelectedZone as Objectstocreatedynamiczone).zoneBlue
                        );
                    cpick.Color = colour;
                    if (cpick.ShowDialog() == DialogResult.OK)
                    {
                        (CurrentSelectedZone as Objectstocreatedynamiczone).zoneAlpha = cpick.Color.A;
                        (CurrentSelectedZone as Objectstocreatedynamiczone).zoneRed = cpick.Color.R;
                        (CurrentSelectedZone as Objectstocreatedynamiczone).zoneGreen = cpick.Color.G;
                        (CurrentSelectedZone as Objectstocreatedynamiczone).zoneBlue = cpick.Color.B;
                        CategorycolourPB.Invalidate();
                        ECMDynamicPVPZoneConfig.isDirty = true;
                        Currentzonetag.Text = $"RGBA Colour: {(CurrentSelectedZone as Objectstocreatedynamiczone).zoneAlpha},{(CurrentSelectedZone as Objectstocreatedynamiczone).zoneRed},{(CurrentSelectedZone as Objectstocreatedynamiczone).zoneGreen},{(CurrentSelectedZone as Objectstocreatedynamiczone).zoneBlue}";
                    }
                }
            }
        }
        private void EMCZonesTV_RequestDisplayText(object sender, TreeViewMS.NodeRequestTextEventArgs e)
        {
            if (CurrentSelectedZone is Customzone)
            {
                Customzone cz = CurrentSelectedZone as Customzone;
                cz.zoneRadius = Convert.ToDecimal(e.Label) + 0.0M;
                EMCCustomZoneRadiusNUD.Value = cz.zoneRadius;
                e.Label = $"Radius: {cz.zoneRadius}";
                pictureBox1.Invalidate();
                ECMExpansionCircleMarkerConfig.isDirty = true;
            }
            else if (CurrentSelectedZone is Objectstocreatedynamiczone)
            {
                Scanzone sz = Currentzonetag.Parent.Tag as Scanzone;
                sz.Radius = Convert.ToDecimal(e.Label) + 0.0M;
                EMCDynamicZoneScanRadiusNUD.Value = sz.Radius;
                e.Label = $"Radius: {sz.Radius}";
                pictureBox1.Invalidate();
                ECMDynamicPVPZoneConfig.isDirty = true;
            }

        }
        private void EMCZonesTV_RequestEditText(object sender, TreeViewMS.NodeRequestTextEventArgs e)
        {
            if (CurrentSelectedZone is Customzone)
            {
                Customzone cz = CurrentSelectedZone as Customzone;
                if (e.Node.Tag.ToString() == "CustomZoneRadius")
                    e.Label = cz.zoneRadius.ToString();
            }
            else if (CurrentSelectedZone is Objectstocreatedynamiczone)
            {
                Scanzone sz = Currentzonetag.Parent.Tag as Scanzone;
                if(e.Node.Tag.ToString() == "DynamicZoneRadius")
                {
                    e.Label = sz.Radius.ToString();
                }
            }
        }

        private void CategorycolourPB_Paint(object sender, PaintEventArgs e)
        {
            if (CurrentSelectedZone == null) { return; }
            PictureBox pb = sender as PictureBox;
            Rectangle region;
            region = pb.ClientRectangle;

            Color colour = Color.Black;
            if (CurrentSelectedZone is Customzone)
            {
                colour = Color.FromArgb(
                    (CurrentSelectedZone as Customzone).zoneAlpha,
                    (CurrentSelectedZone as Customzone).zoneRed,
                    (CurrentSelectedZone as Customzone).zoneGreen,
                    (CurrentSelectedZone as Customzone).zoneBlue
                    );
            }
            else if (CurrentSelectedZone is Polygonzone)
            {
                colour = Color.FromArgb(
                    (CurrentSelectedZone as Polygonzone).polyzoneAlpha,
                    (CurrentSelectedZone as Polygonzone).polyzoneRed,
                    (CurrentSelectedZone as Polygonzone).polyzoneGreen,
                    (CurrentSelectedZone as Polygonzone).polyzoneBlue
                    );
            }
            else if (CurrentSelectedZone is Objectstocreatedynamiczone)
            {
                colour = Color.FromArgb(
                    (CurrentSelectedZone as Objectstocreatedynamiczone).zoneAlpha,
                    (CurrentSelectedZone as Objectstocreatedynamiczone).zoneRed,
                    (CurrentSelectedZone as Objectstocreatedynamiczone).zoneGreen,
                    (CurrentSelectedZone as Objectstocreatedynamiczone).zoneBlue
                    );
            }
            using (Brush brush = new SolidBrush(colour))
            {
                e.Graphics.FillRectangle(brush, region);
            }
            e.Graphics.DrawRectangle(SystemPens.ControlText, region.Left, region.Top, region.Width - 1, region.Height - 1);
        }
        private void CategorycolourPB_Click(object sender, EventArgs e)
        {
            if (CurrentSelectedZone == null) { return; }
            PictureBox pb = sender as PictureBox;
            ColorPickerDialog cpick = new ColorPickerDialog
            {
                StartPosition = FormStartPosition.CenterParent
            };
            Color colour = Color.Black;
            if (CurrentSelectedZone is Customzone)
            {
                colour = Color.FromArgb(
                    (CurrentSelectedZone as Customzone).zoneAlpha,
                    (CurrentSelectedZone as Customzone).zoneRed,
                    (CurrentSelectedZone as Customzone).zoneGreen,
                    (CurrentSelectedZone as Customzone).zoneBlue
                    );
                cpick.Color = colour;
                if (cpick.ShowDialog() == DialogResult.OK)
                {
                    (CurrentSelectedZone as Customzone).zoneAlpha = cpick.Color.A;
                    (CurrentSelectedZone as Customzone).zoneRed = cpick.Color.R;
                    (CurrentSelectedZone as Customzone).zoneGreen = cpick.Color.G;
                    (CurrentSelectedZone as Customzone).zoneBlue = cpick.Color.B;
                    CategorycolourPB.Invalidate();
                    ECMExpansionCircleMarkerConfig.isDirty = true;
                }
            }
            else if (CurrentSelectedZone is Polygonzone)
            {
                colour = Color.FromArgb(
                    (CurrentSelectedZone as Polygonzone).polyzoneAlpha,
                    (CurrentSelectedZone as Polygonzone).polyzoneRed,
                    (CurrentSelectedZone as Polygonzone).polyzoneGreen,
                    (CurrentSelectedZone as Polygonzone).polyzoneBlue
                    );
                cpick.Color = colour;
                if (cpick.ShowDialog() == DialogResult.OK)
                {
                    (CurrentSelectedZone as Polygonzone).polyzoneAlpha = cpick.Color.A;
                    (CurrentSelectedZone as Polygonzone).polyzoneRed = cpick.Color.R;
                    (CurrentSelectedZone as Polygonzone).polyzoneGreen = cpick.Color.G;
                    (CurrentSelectedZone as Polygonzone).polyzoneBlue = cpick.Color.B;
                    CategorycolourPB.Invalidate();
                    ECMPolygonZonesConfig.isDirty = true;
                }
            }
            else if (CurrentSelectedZone is Objectstocreatedynamiczone)
            {
                colour = Color.FromArgb(
                    (CurrentSelectedZone as Objectstocreatedynamiczone).zoneAlpha,
                    (CurrentSelectedZone as Objectstocreatedynamiczone).zoneRed,
                    (CurrentSelectedZone as Objectstocreatedynamiczone).zoneGreen,
                    (CurrentSelectedZone as Objectstocreatedynamiczone).zoneBlue
                    );
                cpick.Color = colour;
                if (cpick.ShowDialog() == DialogResult.OK)
                {
                    (CurrentSelectedZone as Objectstocreatedynamiczone).zoneAlpha = cpick.Color.A;
                    (CurrentSelectedZone as Objectstocreatedynamiczone).zoneRed = cpick.Color.R;
                    (CurrentSelectedZone as Objectstocreatedynamiczone).zoneGreen = cpick.Color.G;
                    (CurrentSelectedZone as Objectstocreatedynamiczone).zoneBlue = cpick.Color.B;
                    CategorycolourPB.Invalidate();
                    ECMDynamicPVPZoneConfig.isDirty = true;
                }
            }
        }
        private void EMCDynamicZonesScanZonesLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EMCDynamicZonesScanZonesLB.SelectedItems.Count <= 0) return;
            currentScanZone = EMCDynamicZonesScanZonesLB.SelectedItem as Scanzone;
            useraction = false;
            EMCDynamicZoneScanXNUD.Value = currentScanZone.X;
            EMCDynamicZoneScanYNUD.Value = currentScanZone.Y;
            EMCDynamicZoneScanZNUD.Value = currentScanZone.Z;
            EMCDynamicZoneScanRadiusNUD.Value = currentScanZone.Radius;
            var result = EMCZonesTV.Nodes["emcDynamicZones"].Nodes.Descendants().Where(x => (x.Tag as Scanzone) == currentScanZone).FirstOrDefault();
            if (result != null)
                EMCZonesTV.SelectedNode = result;
            useraction = true;
        }
        private void EMCZoneNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CurrentSelectedZone is Customzone)
            {
                (CurrentSelectedZone as Customzone).zoneName = EMCZoneNameTB.Text;
                ECMExpansionCircleMarkerConfig.isDirty = true;
                Currentzonetag.Text = $"Zone Name: {(CurrentSelectedZone as Customzone).zoneName}";
                Currentzonetag.Name = $"{(CurrentSelectedZone as Customzone).zoneName}";
            }
            else if (CurrentSelectedZone is Polygonzone)
            {
                (CurrentSelectedZone as Polygonzone).polyzoneName = EMCZoneNameTB.Text;
                ECMPolygonZonesConfig.isDirty = true;
                Currentzonetag.Text = $"Zone Name: {(CurrentSelectedZone as Polygonzone).polyzoneName}";
                Currentzonetag.Name = $"{(CurrentSelectedZone as Polygonzone).polyzoneName}";
            }
            else if (CurrentSelectedZone is Objectstocreatedynamiczone)
            {
                (CurrentSelectedZone as Objectstocreatedynamiczone).itemName = EMCZoneNameTB.Text;
                ECMDynamicPVPZoneConfig.isDirty = true;
                Currentzonetag.Text = $"Zone Name: {(CurrentSelectedZone as Objectstocreatedynamiczone).itemName}";
                Currentzonetag.Name = $"{(CurrentSelectedZone as Objectstocreatedynamiczone).itemName}";
            }
        }
        private void EMCZoneDrawCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CurrentSelectedZone is Customzone)
            {
                (CurrentSelectedZone as Customzone).drawCircle = EMCZoneDrawCB.Checked == true ? 1 : 0;
                ECMExpansionCircleMarkerConfig.isDirty = true;
            }
            else if (CurrentSelectedZone is Polygonzone)
            {
                (CurrentSelectedZone as Polygonzone).polydrawPolygon = EMCZoneDrawCB.Checked == true ? 1 : 0;
                ECMPolygonZonesConfig.isDirty = true;
            }
            else if (CurrentSelectedZone is Objectstocreatedynamiczone)
            {
                (CurrentSelectedZone as Objectstocreatedynamiczone).drawCircle = EMCZoneDrawCB.Checked == true ? 1 : 0;
                ECMDynamicPVPZoneConfig.isDirty = true;
            }
        }
        private void EMCZoneTypeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CurrentSelectedZone is Customzone)
            {
                (CurrentSelectedZone as Customzone).isPvPZone = EMCZoneTypeCB.SelectedIndex;
                ECMExpansionCircleMarkerConfig.isDirty = true;
            }
            else if (CurrentSelectedZone is Polygonzone)
            {
                (CurrentSelectedZone as Polygonzone).polyisPvPZone = EMCZoneTypeCB.SelectedIndex;
                ECMPolygonZonesConfig.isDirty = true;
            }
            else if (CurrentSelectedZone is Objectstocreatedynamiczone)
            {
                (CurrentSelectedZone as Objectstocreatedynamiczone).isPvPZone = EMCZoneTypeCB.SelectedIndex;
                ECMDynamicPVPZoneConfig.isDirty = true;
            }
        }
        private void EMCZoneStartHourNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CurrentSelectedZone is Customzone)
            {
                (CurrentSelectedZone as Customzone).ZoneSchedule.StartHour = (int)EMCZoneStartHourNUD.Value;
                ECMExpansionCircleMarkerConfig.isDirty = true;
            }
            else if (CurrentSelectedZone is Polygonzone)
            {
                (CurrentSelectedZone as Polygonzone).ZoneSchedule.StartHour = (int)EMCZoneStartHourNUD.Value;
                ECMPolygonZonesConfig.isDirty = true;
            }
            else if (CurrentSelectedZone is Objectstocreatedynamiczone)
            {
                (CurrentSelectedZone as Objectstocreatedynamiczone).ZoneSchedule.StartHour = (int)EMCZoneStartHourNUD.Value;
                ECMDynamicPVPZoneConfig.isDirty = true;
            }
        }
        private void EMCZoneEndHourNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CurrentSelectedZone is Customzone)
            {
                (CurrentSelectedZone as Customzone).ZoneSchedule.EndHour = (int)EMCZoneEndHourNUD.Value;
                ECMExpansionCircleMarkerConfig.isDirty = true;
            }
            else if (CurrentSelectedZone is Polygonzone)
            {
                (CurrentSelectedZone as Polygonzone).ZoneSchedule.EndHour = (int)EMCZoneEndHourNUD.Value;
                ECMPolygonZonesConfig.isDirty = true;
            }
            else if (CurrentSelectedZone is Objectstocreatedynamiczone)
            {
                (CurrentSelectedZone as Objectstocreatedynamiczone).ZoneSchedule.EndHour = (int)EMCZoneEndHourNUD.Value;
                ECMDynamicPVPZoneConfig.isDirty = true;
            }
        }
        private void WeekdaysCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            string daysofweek = "";
            if (MondayDOWCB.Checked)
                daysofweek += "Monday ";
            if (TuesdayDOWCB.Checked)
                daysofweek += "Tuesday ";
            if (WednesdayDOWCB.Checked)
                daysofweek += "Wednesday ";
            if (ThursdayDOWCB.Checked)
                daysofweek += "Thursday ";
            if (FridayDOWCB.Checked)
                daysofweek += "Friday ";
            if (SaturdayDOWCB.Checked)
                daysofweek += "Saturday ";
            if (SundayDOWCB.Checked)
                daysofweek += "Sunday";

            daysofweek = daysofweek.TrimEnd();

            if (CurrentSelectedZone is Customzone)
            {
                (CurrentSelectedZone as Customzone).ZoneSchedule.Days = daysofweek;
                ECMExpansionCircleMarkerConfig.isDirty = true;
            }
            else if (CurrentSelectedZone is Polygonzone)
            {
                (CurrentSelectedZone as Polygonzone).ZoneSchedule.Days = daysofweek;
                ECMPolygonZonesConfig.isDirty = true;
            }
            else if (CurrentSelectedZone is Objectstocreatedynamiczone)
            {
                (CurrentSelectedZone as Objectstocreatedynamiczone).ZoneSchedule.Days = daysofweek;
                ECMDynamicPVPZoneConfig.isDirty = true;
            }
        }
        private void EMCEnableCustomMessagesCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CurrentSelectedZone is Customzone)
            {
                (CurrentSelectedZone as Customzone).EnableCustomMessages = EMCEnableCustomMessagesCB.Checked == true ? 1 : 0;
                ECMExpansionCircleMarkerConfig.isDirty = true;
            }
            else if (CurrentSelectedZone is Polygonzone)
            {
                (CurrentSelectedZone as Polygonzone).EnableCustomMessages = EMCEnableCustomMessagesCB.Checked == true ? 1 : 0;
                ECMPolygonZonesConfig.isDirty = true;
            }
        }
        private void EMCCustomTitleTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CurrentSelectedZone is Customzone)
            {
                (CurrentSelectedZone as Customzone).CustomTitle = EMCCustomTitleTB.Text;
                ECMExpansionCircleMarkerConfig.isDirty = true;
            }
            else if (CurrentSelectedZone is Polygonzone)
            {
                (CurrentSelectedZone as Polygonzone).CustomTitle = EMCCustomTitleTB.Text;
                ECMPolygonZonesConfig.isDirty = true;
            }
        }
        private void EMCCustomMessageEnterTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CurrentSelectedZone is Customzone)
            {
                (CurrentSelectedZone as Customzone).CustomMessageEnter = EMCCustomMessageEnterTB.Text;
                ECMExpansionCircleMarkerConfig.isDirty = true;
            }
            else if (CurrentSelectedZone is Polygonzone)
            {
                (CurrentSelectedZone as Polygonzone).CustomMessageEnter = EMCCustomMessageEnterTB.Text;
                ECMPolygonZonesConfig.isDirty = true;
            }

        }
        private void EMCCustomMessageExitTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CurrentSelectedZone is Customzone)
            {
                (CurrentSelectedZone as Customzone).CustomMessageExit = EMCCustomMessageExitTB.Text;
                ECMExpansionCircleMarkerConfig.isDirty = true;
            }
            else if (CurrentSelectedZone is Polygonzone)
            {
                (CurrentSelectedZone as Polygonzone).CustomMessageExit = EMCCustomMessageExitTB.Text;
                ECMPolygonZonesConfig.isDirty = true;
            }
        }
        private void EMCCustomIconTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CurrentSelectedZone is Customzone)
            {
                (CurrentSelectedZone as Customzone).CustomIcon = EMCCustomIconTB.Text;
                ECMExpansionCircleMarkerConfig.isDirty = true;
            }
            else if (CurrentSelectedZone is Polygonzone)
            {
                (CurrentSelectedZone as Polygonzone).CustomIcon = EMCCustomIconTB.Text;
                ECMPolygonZonesConfig.isDirty = true;
            }
        }
        private void ObjectivesAICampAllowedWeaponsAddButton_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                if (CurrentSelectedZone is Customzone)
                {
                    foreach (string l in addedtypes)
                    {
                        if (!(CurrentSelectedZone as Customzone).OnlyAllowedWeapons.Contains(l))
                        {
                            (CurrentSelectedZone as Customzone).OnlyAllowedWeapons.Add(l);
                        }
                    }
                    ECMExpansionCircleMarkerConfig.isDirty = true;
                }
                else if (CurrentSelectedZone is Polygonzone)
                {
                    foreach (string l in addedtypes)
                    {
                        if (!(CurrentSelectedZone as Polygonzone).OnlyAllowedWeapons.Contains(l))
                        {
                            (CurrentSelectedZone as Polygonzone).CustomIcon = EMCCustomIconTB.Text;
                        }
                    }
                    ECMPolygonZonesConfig.isDirty = true;
                }
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void ObjectivesAICampAllowedWeaponsRemoveButton_Click(object sender, EventArgs e)
        {
            if (EMCZonesAllowedWeaponsLB.SelectedItems.Count < 1) return;
            if (CurrentSelectedZone is Customzone)
            {
                for (int i = 0; i < EMCZonesAllowedWeaponsLB.SelectedItems.Count; i++)
                {
                    (CurrentSelectedZone as Customzone).OnlyAllowedWeapons.Remove(EMCZonesAllowedWeaponsLB.GetItemText(EMCZonesAllowedWeaponsLB.SelectedItems[0]));
                }
                ECMExpansionCircleMarkerConfig.isDirty = true;
            }
            if (CurrentSelectedZone is Polygonzone)
            {
                for (int i = 0; i < EMCZonesAllowedWeaponsLB.SelectedItems.Count; i++)
                {
                    (CurrentSelectedZone as Polygonzone).OnlyAllowedWeapons.Remove(EMCZonesAllowedWeaponsLB.GetItemText(EMCZonesAllowedWeaponsLB.SelectedItems[0]));
                }
                ECMPolygonZonesConfig.isDirty = true;
            }
        }
        private void EMCCustomZoneXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CurrentSelectedZone is Customzone)
            {
                (CurrentSelectedZone as Customzone).x = EMCCustomZoneXNUD.Value;
                ECMExpansionCircleMarkerConfig.isDirty = true;
            }
        }
        private void EMCCustomZoneZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CurrentSelectedZone is Customzone)
            {
                (CurrentSelectedZone as Customzone).z = EMCCustomZoneZNUD.Value;
                ECMExpansionCircleMarkerConfig.isDirty = true;
            }
        }
        private void EMCCustomZoneRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CurrentSelectedZone is Customzone)
            {
                (CurrentSelectedZone as Customzone).zoneRadius = EMCCustomZoneRadiusNUD.Value;
                ECMExpansionCircleMarkerConfig.isDirty = true;
            }
        }
        private void darkButton22_Click(object sender, EventArgs e)
        {
            float scalevalue = ZoneScale * 0.05f;
            Rectangle rect2 = getpolyrectangle((CurrentSelectedZone as Polygonzone)._vertices, scalevalue, false);
            Vec3 v = new Vec3(rect2.X, 0, rect2.Y);
            (CurrentSelectedZone as Polygonzone)._vertices.Add(v);
            TreeNode vert = new TreeNode($"{v}")
            {
                Tag = v
            };
            if(Currentzonetag.Tag is Polygonzone)
                Currentzonetag.Nodes["PolygonzoneVerts"].Nodes.Add(vert);
            else if (Currentzonetag.Tag.ToString() == "PolygonzoneVerts")
                Currentzonetag.Nodes.Add(vert);
            else if (Currentzonetag.Tag is Vec3)
                Currentzonetag.Parent.Nodes.Add(vert);
            

            ECMPolygonZonesConfig.isDirty = true;
            pictureBox1.Invalidate();
        }
        private void darkButton21_Click(object sender, EventArgs e)
        {
            if (EMCPolygonZonesVerticeListLB.SelectedItems.Count < 1) return;
            Vec3 v = EMCPolygonZonesVerticeListLB.SelectedItem as Vec3;
            (CurrentSelectedZone as Polygonzone)._vertices.Remove(v);
            var result = EMCZonesTV.Nodes["emcPolygonZones"].Nodes.Descendants().Where(x => (x.Tag as Vec3) == v).FirstOrDefault();
            Currentzonetag.Parent.Nodes.Remove(result);
            ECMPolygonZonesConfig.isDirty = true;
            pictureBox1.Invalidate();
        }
        private void EMCPolygonVerticePOSXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Vec3 vec3 = EMCPolygonZonesVerticeListLB.SelectedItem as Vec3;
            TreeNode result = EMCZonesTV.Nodes["emcPolygonZones"].Nodes.Descendants().Where(x => (x.Tag as Vec3) == vec3).FirstOrDefault();
            EMCZonesTV.SelectedNode = result;
            vec3.X = (float)EMCPolygonVerticePOSXNUD.Value;
            if (MapData.FileExists)
            {
                vec3.Y = MapData.gethieght(vec3.X, vec3.Z);
                EMCPolygonVerticePOSYNUD.Value = (decimal)vec3.Y;
            }
            result.Text = $"{vec3}";
            ECMPolygonZonesConfig.isDirty = true;
            EMCPolygonZonesVerticeListLB.Refresh();
        }
        private void EMCPolygonVerticePOSYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Vec3 vec3 = EMCPolygonZonesVerticeListLB.SelectedItem as Vec3;
            TreeNode result = EMCZonesTV.Nodes["emcPolygonZones"].Nodes.Descendants().Where(x => (x.Tag as Vec3) == vec3).FirstOrDefault();
            EMCZonesTV.SelectedNode = result;
            vec3.Y = (float)EMCPolygonVerticePOSYNUD.Value;
            result.Text = $"{vec3}";
            ECMPolygonZonesConfig.isDirty = true;
            EMCPolygonZonesVerticeListLB.Refresh();
        }
        private void EMCPolygonVerticePOSZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Vec3 vec3 = EMCPolygonZonesVerticeListLB.SelectedItem as Vec3;
            TreeNode result = EMCZonesTV.Nodes["emcPolygonZones"].Nodes.Descendants().Where(x => (x.Tag as Vec3) == vec3).FirstOrDefault();
            EMCZonesTV.SelectedNode = result;
            vec3.Z = (float)EMCPolygonVerticePOSZNUD.Value;
            if (MapData.FileExists)
            {
                vec3.Y = MapData.gethieght(vec3.X, vec3.Z);
                EMCPolygonVerticePOSYNUD.Value = (decimal)vec3.Y;
            }
            result.Text = $"{vec3}";
            ECMPolygonZonesConfig.isDirty = true;
            EMCPolygonZonesVerticeListLB.Refresh();
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {

        }
        private void darkButton1_Click(object sender, EventArgs e)
        {

        }
        private void EMCDynamicZoneScanXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            var result = EMCZonesTV.Nodes["emcDynamicZones"].Nodes.Descendants().Where(x => (x.Tag as Scanzone) == currentScanZone).FirstOrDefault();
            if (result != null)
            {
                EMCZonesTV.SelectedNode = result;
                result.Text = $"Vector3:{EMCDynamicZoneScanXNUD.Value},{currentScanZone.Y},{currentScanZone.Z}";
            }
            currentScanZone.X = EMCDynamicZoneScanXNUD.Value;
            ECMDynamicPVPZoneConfig.isDirty = true;
            pictureBox1.Invalidate();
            EMCDynamicZonesScanZonesLB.Refresh();
        }
        private void EMCDynamicZoneScanYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            var result = EMCZonesTV.Nodes["emcDynamicZones"].Nodes.Descendants().Where(x => (x.Tag as Scanzone) == currentScanZone).FirstOrDefault();
            if (result != null)
            {
                EMCZonesTV.SelectedNode = result;
                result.Text = $"Vector3:{currentScanZone.X},{EMCDynamicZoneScanYNUD.Value},{currentScanZone.Z}";
            }
            currentScanZone.Y = EMCDynamicZoneScanYNUD.Value;
            ECMDynamicPVPZoneConfig.isDirty = true;
            pictureBox1.Invalidate();
            EMCDynamicZonesScanZonesLB.Refresh();
        }
        private void EMCDynamicZoneScanZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            var result = EMCZonesTV.Nodes["emcDynamicZones"].Nodes.Descendants().Where(x => (x.Tag as Scanzone) == currentScanZone).FirstOrDefault();
            if (result != null)
            {
                EMCZonesTV.SelectedNode = result;
                result.Text = $"Vector3:{currentScanZone.X},{currentScanZone.Y},{EMCDynamicZoneScanZNUD.Value}";
            }
            currentScanZone.Z = EMCDynamicZoneScanZNUD.Value;
            ECMDynamicPVPZoneConfig.isDirty = true;
            pictureBox1.Invalidate();
            EMCDynamicZonesScanZonesLB.Refresh();
        }
        private void EMCDynamicZoneScanRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            var result = EMCZonesTV.Nodes["emcDynamicZones"].Nodes.Descendants().Where(x => (x.Tag as Scanzone) == currentScanZone).FirstOrDefault();
            if (result != null)
            {
                EMCZonesTV.SelectedNode = result;
                result.Nodes[0].Text = $"Radius: {EMCDynamicZoneScanRadiusNUD.Value}";
            }
            currentScanZone.Radius = EMCDynamicZoneScanRadiusNUD.Value;
            ECMDynamicPVPZoneConfig.isDirty = true;
            pictureBox1.Invalidate();
            EMCDynamicZonesScanZonesLB.Refresh();
        }
        private void addNewCustomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Customzone newzone = new Customzone()
            {
                zoneName = "New Custom Zone",
                x = currentproject.MapSize / 2,
                z = currentproject.MapSize / 2,
                zoneRadius = 500,
                zoneAlpha = 255,
                zoneRed = 255,
                zoneGreen = 0,
                zoneBlue = 0,
                drawCircle = 1,
                isPvPZone = 1,
                ZoneSchedule = new Zoneschedule()
                {
                    Days = "Monday Tuesday Wednesday Thursday Friday Saturday Sunday",
                    StartHour = 12,
                    EndHour = 24
                },
                EnableCustomMessages = 1,
                CustomTitle = "Custom Zone",
                CustomMessageEnter = "You have entered Custom Zone",
                CustomMessageExit = "You have left Custom Zone",
                CustomIcon = "emm/gui/information.edds",
                OnlyAllowedWeapons = new BindingList<string>()
            };
            ECMExpansionCircleMarkerConfig.CustomZones.Add(newzone);
            ECMExpansionCircleMarkerConfig.isDirty = true;
            TreeNode radiuszoneTN = new TreeNode($"Zone Name: {newzone.zoneName}")
            {
                Tag = newzone
            };
            TreeNode radiusTN = new TreeNode($"Radius: {newzone.zoneRadius}")
            {
                Tag = "CustomZoneRadius"
            };
            radiuszoneTN.Nodes.Add(radiusTN);
            TreeNode ColourTN = new TreeNode($"RGBA Colour: {newzone.zoneAlpha},{newzone.zoneRed},{newzone.zoneGreen},{newzone.zoneBlue}")
            {
                Tag = "CustomZoneMarkerColour"
            };
            radiuszoneTN.Nodes.Add(ColourTN);
            Currentzonetag.Nodes.Add(radiuszoneTN);
            EMCZonesTV.SelectedNode = radiuszoneTN;
            pictureBox1.Invalidate();

        }
        private void removeCustomZoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentSelectedZone == null) return;
            if (CurrentSelectedZone is Customzone)
            {
                ECMExpansionCircleMarkerConfig.CustomZones.Remove(CurrentSelectedZone as Customzone);
                ECMExpansionCircleMarkerConfig.isDirty = true;
                Currentzonetag.Parent.Nodes.Remove(Currentzonetag);
                pictureBox1.Invalidate(true);
            }
        }
        private void addNewPolygonZoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Polygonzone newzone = new Polygonzone()
            {
                polyzoneName = "New Polygon Zone",
                _vertices = new BindingList<Vec3>()
                {
                    new Vec3 ( (currentproject.MapSize / 2) -250, 0, (currentproject.MapSize / 2) -250 ),
                    new Vec3 ( (currentproject.MapSize / 2) +250, 0, (currentproject.MapSize / 2) -250 ),
                    new Vec3 ( (currentproject.MapSize / 2) +250, 0, (currentproject.MapSize / 2) +250 ),
                    new Vec3 ( (currentproject.MapSize / 2) -250, 0, (currentproject.MapSize / 2) +250 )
                },
                polyzoneAlpha = 255,
                polyzoneRed = 255,
                polyzoneGreen = 0,
                polyzoneBlue = 0,
                polydrawPolygon = 1,
                polyisPvPZone = 1,
                ZoneSchedule = new Zoneschedule()
                {
                    Days = "Monday Tuesday Wednesday Thursday Friday Saturday Sunday",
                    StartHour = 12,
                    EndHour = 24
                },
                EnableCustomMessages = 1,
                CustomTitle = "Polygon Zone",
                CustomMessageEnter = "You have entered Polygon Zone",
                CustomMessageExit = "You have left Polygon Zone",
                CustomIcon = "emm/gui/information.edds",
                OnlyAllowedWeapons = new BindingList<string>()
            };
            if (MapData.FileExists)
            {
                for (int i = 0; i < newzone._vertices.Count; i++)
                {
                    newzone._vertices[i].Y = MapData.gethieght(newzone._vertices[i].X, newzone._vertices[i].Z);
                }
            }
            ECMPolygonZonesConfig.PolygonZones.Add(newzone);
            ECMPolygonZonesConfig.isDirty = true;
            TreeNode PolygonzoneTN = new TreeNode($"Zone Name: {newzone.polyzoneName}")
            {
                Tag = newzone,
                Name = newzone.polyzoneName
            };
            TreeNode polyverts = new TreeNode("Vertices")
            {
                Tag = "PolygonzoneVerts",
                Name = "PolygonzoneVerts"
            };
            for (int i = 0; i < newzone._vertices.Count; i++)
            {
                TreeNode vert = new TreeNode($"{newzone._vertices[i]}")
                {
                    Tag = newzone._vertices[i]
                };
                polyverts.Nodes.Add(vert);
            };
            PolygonzoneTN.Nodes.Add(polyverts);
            TreeNode ColourTN = new TreeNode($"RGBA Colour: {newzone.polyzoneAlpha},{newzone.polyzoneRed},{newzone.polyzoneGreen},{newzone.polyzoneBlue}")
            {
                Tag = "PolygonZoneMarkerColour"
            };
            PolygonzoneTN.Nodes.Add(ColourTN);
            Currentzonetag.Nodes.Add(PolygonzoneTN);
            EMCZonesTV.SelectedNode = PolygonzoneTN;
        }
        private void removePolygonZoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentSelectedZone == null) return;
            if (CurrentSelectedZone is Polygonzone)
            {
                ECMPolygonZonesConfig.PolygonZones.Remove(CurrentSelectedZone as Polygonzone);
                ECMPolygonZonesConfig.isDirty = true;
                Currentzonetag.Parent.Nodes.Remove(Currentzonetag);
                pictureBox1.Invalidate(true);
            }
        }
        private void addNewDynamicObjectZoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Objectstocreatedynamiczone newzone = new Objectstocreatedynamiczone()
            {
                itemName = "ObjectNameHere",
                Radius = 100,
                drawCircle = 1,
                isPvPZone = 1,
                zoneAlpha = 255,
                zoneRed = 0,
                zoneGreen = 255,
                zoneBlue = 0,
                ScanZones = new BindingList<Scanzone>()
                {
                    new Scanzone()
                    {
                        X = currentproject.MapSize / 2,
                        Y = 0,
                        Z = currentproject.MapSize / 2,
                        Radius = 500
                    }
                },
                ZoneSchedule = new Zoneschedule()
                {
                    Days = "Monday Tuesday Wednesday Thursday Friday Saturday Sunday",
                    StartHour = 12,
                    EndHour = 24
                }
            };
            if (MapData.FileExists)
            {
                for (int i = 0; i < newzone.ScanZones.Count; i++)
                {
                    newzone.ScanZones[i].Y = (decimal)(MapData.gethieght((float)newzone.ScanZones[i].X, (float)newzone.ScanZones[i].Z));
                }
            }
            ECMDynamicPVPZoneConfig.ObjectsToCreateDynamicZones.Add(newzone);
            ECMDynamicPVPZoneConfig.isDirty = true;
            TreeNode dynamiczoneTN = new TreeNode($"Zone Name: {newzone.itemName}")
            {
                Tag = newzone,
                Name = newzone.itemName
            };
            TreeNode dynamicscanzonesTN = new TreeNode($"Scan Zones")
            {
                Tag = "DynamicScanZones",
                Name = "DynamicScanZones"
            };
            foreach (Scanzone sz in newzone.ScanZones)
            {
                TreeNode dynamiczonesTN = new TreeNode($"Vector3:{sz.X},{sz.Y},{sz.Z}")
                {
                    Tag = sz
                };
                TreeNode radiusTN = new TreeNode($"Radius: {sz.Radius}")
                {
                    Tag = "DynamicZoneRadius"
                };
                dynamiczonesTN.Nodes.Add(radiusTN);
                dynamicscanzonesTN.Nodes.Add(dynamiczonesTN);
            }
            dynamiczoneTN.Nodes.Add(dynamicscanzonesTN);
            TreeNode ColourTN = new TreeNode($"RGBA Colour: {newzone.zoneAlpha},{newzone.zoneRed},{newzone.zoneGreen},{newzone.zoneBlue}")
            {
                Tag = "DynamicZoneMarkerColour"
            };
            dynamiczoneTN.Nodes.Add(ColourTN);
            Currentzonetag.Nodes.Add(dynamiczoneTN);
            EMCZonesTV.SelectedNode = dynamiczoneTN;
        }
        private void removeDynamicObjectZoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentSelectedZone == null) return;
            if (CurrentSelectedZone is Objectstocreatedynamiczone)
            {
                ECMDynamicPVPZoneConfig.ObjectsToCreateDynamicZones.Remove(CurrentSelectedZone as Objectstocreatedynamiczone);
                ECMDynamicPVPZoneConfig.isDirty = true;
                Currentzonetag.Parent.Nodes.Remove(Currentzonetag);
                pictureBox1.Invalidate(true);
            }
        }
        private void addNewScanZoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Scanzone newsz = new Scanzone()
            {
                X = currentproject.MapSize / 2,
                Y = 0,
                Z = currentproject.MapSize / 2,
                Radius = 500
            };
            if (MapData.FileExists)
            {
                newsz.Y = (decimal)(MapData.gethieght((float)newsz.X, (float)newsz.Z));
            }
            (CurrentSelectedZone as Objectstocreatedynamiczone).ScanZones.Add(newsz);
            ECMDynamicPVPZoneConfig.isDirty = true;
            TreeNode dynamiczonesTN = new TreeNode($"Vector3:{newsz.X},{newsz.Y},{newsz.Z}")
            {
                Tag = newsz
            };
            TreeNode radiusTN = new TreeNode($"Radius: {newsz.Radius}")
            {
                Tag = "DynamicZoneRadius"
            };
            dynamiczonesTN.Nodes.Add(radiusTN);
            Currentzonetag.Parent.Nodes.Add(dynamiczonesTN);
            EMCZonesTV.SelectedNode = dynamiczonesTN;
            pictureBox1.Invalidate(true);
        }
        private void removeScanZoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentSelectedZone == null) return;
            if (CurrentSelectedZone is Objectstocreatedynamiczone)
            {
                (CurrentSelectedZone as Objectstocreatedynamiczone).ScanZones.Remove(currentScanZone);
                ECMDynamicPVPZoneConfig.isDirty = true;
                Currentzonetag.Parent.Nodes.Remove(Currentzonetag);
                pictureBox1.Invalidate(true);
            }
        }

        private void HeliCrashEnabledCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CurrentSelectedZone is Mi8)
            {
                ECMDynamicPVPZoneConfig.Mi8_Crashed = HeliCrashEnabledCB.Checked == true ? 1 : 0;
                ECMDynamicPVPZoneConfig.isDirty = true;
            }
            else if (CurrentSelectedZone is UH1Y)
            {
                ECMDynamicPVPZoneConfig.UH1Y_Crashed = HeliCrashEnabledCB.Checked == true ? 1 : 0;
                ECMDynamicPVPZoneConfig.isDirty = true;
            }
            else if (CurrentSelectedZone is AIRDROP_ZONES)
            {
                HeliCrashEnabledCB.Checked = true;
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CurrentSelectedZone is Mi8)
            {
                ECMDynamicPVPZoneConfig.Mi8.drawCircle = checkBox1.Checked == true ? 1 : 0;
                ECMDynamicPVPZoneConfig.isDirty = true;
            }
            else if (CurrentSelectedZone is UH1Y)
            {
                ECMDynamicPVPZoneConfig.UH1Y.drawCircle = checkBox1.Checked == true ? 1 : 0;
                ECMDynamicPVPZoneConfig.isDirty = true;
            }
            else if (CurrentSelectedZone is AIRDROP_ZONES)
            {
                ECMExpansionCircleMarkerConfig.AIRDROP_ZONES.drawCircle = checkBox1.Checked == true ? 1 : 0;
                ECMExpansionCircleMarkerConfig.isDirty = true;
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CurrentSelectedZone is Mi8)
            {
                ECMDynamicPVPZoneConfig.Mi8.isPvPZone = comboBox1.SelectedIndex;
                ECMDynamicPVPZoneConfig.isDirty = true;
            }
            else if (CurrentSelectedZone is UH1Y)
            {
                ECMDynamicPVPZoneConfig.UH1Y.isPvPZone = comboBox1.SelectedIndex;
                ECMDynamicPVPZoneConfig.isDirty = true;
            }
            else if (CurrentSelectedZone is AIRDROP_ZONES)
            {
                ECMExpansionCircleMarkerConfig.AIRDROP_ZONES.isPvPZone = comboBox1.SelectedIndex;
                ECMExpansionCircleMarkerConfig.isDirty = true;
            }
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            ColorPickerDialog cpick = new ColorPickerDialog
            {
                StartPosition = FormStartPosition.CenterParent
            };
            Color colour = Color.Black;
            if (CurrentSelectedZone is Mi8)
            {
                colour = Color.FromArgb(
                    (CurrentSelectedZone as Mi8).alpha,
                    (CurrentSelectedZone as Mi8).red,
                    (CurrentSelectedZone as Mi8).green,
                    (CurrentSelectedZone as Mi8).blue
                    );
                cpick.Color = colour;
                if (cpick.ShowDialog() == DialogResult.OK)
                {
                    (CurrentSelectedZone as Mi8).alpha = cpick.Color.A;
                    (CurrentSelectedZone as Mi8).red = cpick.Color.R;
                    (CurrentSelectedZone as Mi8).green = cpick.Color.G;
                    (CurrentSelectedZone as Mi8).blue = cpick.Color.B;
                    pictureBox2.Invalidate();
                    ECMDynamicPVPZoneConfig.isDirty = true;
                }
            }
            else if (CurrentSelectedZone is UH1Y)
            {
                colour = Color.FromArgb(
                    (CurrentSelectedZone as UH1Y).alpha,
                    (CurrentSelectedZone as UH1Y).red,
                    (CurrentSelectedZone as UH1Y).green,
                    (CurrentSelectedZone as UH1Y).blue
                    );
                cpick.Color = colour;
                if (cpick.ShowDialog() == DialogResult.OK)
                {
                    (CurrentSelectedZone as UH1Y).alpha = cpick.Color.A;
                    (CurrentSelectedZone as UH1Y).red = cpick.Color.R;
                    (CurrentSelectedZone as UH1Y).green = cpick.Color.G;
                    (CurrentSelectedZone as UH1Y).blue = cpick.Color.B;
                    pictureBox2.Invalidate();
                    ECMDynamicPVPZoneConfig.isDirty = true;
                }
            }
            else if (CurrentSelectedZone is AIRDROP_ZONES)
            {
                colour = Color.FromArgb(
                    (CurrentSelectedZone as AIRDROP_ZONES).zoneAlpha,
                    (CurrentSelectedZone as AIRDROP_ZONES).zoneRed,
                    (CurrentSelectedZone as AIRDROP_ZONES).zoneGreen,
                    (CurrentSelectedZone as AIRDROP_ZONES).zoneBlue
                    );
                cpick.Color = colour;
                if (cpick.ShowDialog() == DialogResult.OK)
                {
                    (CurrentSelectedZone as AIRDROP_ZONES).zoneAlpha = cpick.Color.A;
                    (CurrentSelectedZone as AIRDROP_ZONES).zoneRed = cpick.Color.R;
                    (CurrentSelectedZone as AIRDROP_ZONES).zoneGreen = cpick.Color.G;
                    (CurrentSelectedZone as AIRDROP_ZONES).zoneBlue = cpick.Color.B;
                    pictureBox2.Invalidate();
                    ECMExpansionCircleMarkerConfig.isDirty = true;
                }
            }
        }
        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            if (CurrentSelectedZone == null) { return; }
            PictureBox pb = sender as PictureBox;
            Rectangle region;
            region = pb.ClientRectangle;

            Color colour = Color.Black;
            if (CurrentSelectedZone is Mi8)
            {
                colour = Color.FromArgb(
                    (CurrentSelectedZone as Mi8).alpha,
                    (CurrentSelectedZone as Mi8).red,
                    (CurrentSelectedZone as Mi8).green,
                    (CurrentSelectedZone as Mi8).blue
                    );
            }
            else if (CurrentSelectedZone is UH1Y)
            {
                colour = Color.FromArgb(
                    (CurrentSelectedZone as UH1Y).alpha,
                    (CurrentSelectedZone as UH1Y).red,
                    (CurrentSelectedZone as UH1Y).green,
                    (CurrentSelectedZone as UH1Y).blue
                    );
            }
            else if (CurrentSelectedZone is AIRDROP_ZONES)
            {
                colour = Color.FromArgb(
                    (CurrentSelectedZone as AIRDROP_ZONES).zoneAlpha,
                    (CurrentSelectedZone as AIRDROP_ZONES).zoneRed,
                    (CurrentSelectedZone as AIRDROP_ZONES).zoneGreen,
                    (CurrentSelectedZone as AIRDROP_ZONES).zoneBlue
                    );
            }
            using (Brush brush = new SolidBrush(colour))
            {
                e.Graphics.FillRectangle(brush, region);
            }
            e.Graphics.DrawRectangle(SystemPens.ControlText, region.Left, region.Top, region.Width - 1, region.Height - 1);
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CurrentSelectedZone is Mi8)
            {
                ECMDynamicPVPZoneConfig.Mi8.Radius = numericUpDown1.Value;
                ECMDynamicPVPZoneConfig.isDirty = true;
            }
            else if (CurrentSelectedZone is UH1Y)
            {
                ECMDynamicPVPZoneConfig.UH1Y.Radius = numericUpDown1.Value;
                ECMDynamicPVPZoneConfig.isDirty = true;
            }
            else if (CurrentSelectedZone is AIRDROP_ZONES)
            {
                ECMExpansionCircleMarkerConfig.AIRDROP_ZONES.Radius = numericUpDown1.Value;
                ECMExpansionCircleMarkerConfig.isDirty = true;
            }
        }

        private void generaslSetiingsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CheckBox cb = sender as CheckBox;
            ECMExpansionCircleMarkerConfig.SetBoolValue(cb.Name.Substring(0, cb.Name.Length - 2), cb.Checked == true ? 1 : 0);
            ECMExpansionCircleMarkerConfig.isDirty = true;
        }
        private void generaslSetiingsTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TextBox tb = sender as TextBox;
            ECMExpansionCircleMarkerConfig.SetTextValue(tb.Name.Substring(0, tb.Name.Length - 2), tb.Text);
            ECMExpansionCircleMarkerConfig.isDirty = true;
        }
        private void generaslSetiingsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            NumericUpDown nud = sender as NumericUpDown;
            ECMExpansionCircleMarkerConfig.SetDecimalValue(nud.Name.Substring(0, nud.Name.Length - 3), nud.Value);
            ECMExpansionCircleMarkerConfig.isDirty = true;
        }
        #endregion
    }
}
