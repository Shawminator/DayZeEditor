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
    public partial class UtopiaAirdropManager : DarkForm
    {
        public Project currentproject { get; set; }
        public TypesFile vanillatypes;
        public List<TypesFile> ModTypes;

        public string UtopiaAirdropSettingssPath { get; private set; }
        public UtopiaAirdropSettings UtopiaAirdropSettings { get; private set; }

        public string Projectname;
        private bool _useraction = false;

        private Droplocation currentDroplocation;
        private Airdropcontainer currentAirdropcontainer;
        private Airdropcontainer currentFlareAirdropcontainer;
        private Lootpool currentlootpool;

        public bool useraction
        {
            get { return _useraction; }
            set
            {
                _useraction = value;
            }
        }

        public TreeNode FocusNode { get; private set; }
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
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    toolStripButton3.Checked = false;
                    toolStripButton1.Checked = false;
                    break;
                case 1:
                    toolStripButton8.Checked = false;
                    toolStripButton1.Checked = false;
                    break;
                case 2:
                    toolStripButton8.Checked = false;
                    toolStripButton3.Checked = false;
                    break;
                default:
                    break;
            }
        }
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            if (tabControl1.SelectedIndex == 0)
                toolStripButton8.Checked = true;
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            if (tabControl1.SelectedIndex == 1)
                toolStripButton3.Checked = true;
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
            if (tabControl1.SelectedIndex == 2)
                toolStripButton1.Checked = true;
        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            SaveUtopiaAirdrop();
        }
        private void SaveUtopiaAirdrop()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");

            if (UtopiaAirdropSettings.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(UtopiaAirdropSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(UtopiaAirdropSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(UtopiaAirdropSettings.Filename, Path.GetDirectoryName(UtopiaAirdropSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(UtopiaAirdropSettings.Filename) + ".bak", true);
                }
                UtopiaAirdropSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(UtopiaAirdropSettings, options);
                File.WriteAllText(UtopiaAirdropSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(Path.GetFileName(UtopiaAirdropSettingssPath)));
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
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\UtopiaAirdrop\\Config");
        }
        private void UtopiaAirdropManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool needtosave = false;
            if (UtopiaAirdropSettings.isDirty)
            {
                needtosave = true;
            }
            if (needtosave)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SaveUtopiaAirdrop();
                }
            }
        }

        public UtopiaAirdropManager()
        {
            InitializeComponent();
        }
        private void UtopiaAirdropManager_Load(object sender, EventArgs e)
        {
            tabControl1.ItemSize = new Size(0, 1);
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Projectname = currentproject.ProjectName;
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            UtopiaAirdropSettingssPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Utopia_Airdrop\\Config\\UtopiaAirdropSettings.json";
            UtopiaAirdropSettings = JsonSerializer.Deserialize<UtopiaAirdropSettings>(File.ReadAllText(UtopiaAirdropSettingssPath));
            UtopiaAirdropSettings.isDirty = false;
            UtopiaAirdropSettings.Filename = UtopiaAirdropSettingssPath;

            LoadUtopiaAirdrop();

            pictureBox1.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Map Size is 15360 x 15360, 0,0 bottom left, middle 7680 x 7680
            pictureBox1.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox1.Paint += new PaintEventHandler(DrawAll);
            trackBar2.Value = 1;
            SetUtopiaAirdropScale();
        }
        private void LoadUtopiaAirdrop()
        {
            useraction = false;

            versionTB.Text = UtopiaAirdropSettings.version;
            heliHeightFromGroundNUD.Value = UtopiaAirdropSettings.heliHeightFromGround;
            heliSpeedNUD.Value = UtopiaAirdropSettings.heliSpeed;
            dropCrateContainerLifetimeInSecondsNUD.Value = UtopiaAirdropSettings.dropCrateContainerLifetimeInSeconds;
            maxCreaturesNUD.Value = UtopiaAirdropSettings.maxCreatures;
            startDelayMinNUD.Value = UtopiaAirdropSettings.startDelayMin;
            pkgIntervalMinNUD.Value = UtopiaAirdropSettings.pkgIntervalMin;
            awayMinNUD.Value = UtopiaAirdropSettings.awayMin;
            titleTB.Text = UtopiaAirdropSettings.title;
            droppedMsgTB.Text = UtopiaAirdropSettings.droppedMsg;
            startMsgTB.Text = UtopiaAirdropSettings.startMsg;
            showMarkerForFlareDropCB.Checked = UtopiaAirdropSettings.showMarkerForFlareDrop == 1 ? true : false;

            BottomLeftXNUD.Value = UtopiaAirdropSettings.leftBottomCornerMap[0];
            BottomLeftZNUD.Value = UtopiaAirdropSettings.leftBottomCornerMap[2];
            TopRightXNUD.Value = UtopiaAirdropSettings.rightUpCornerMap[0];
            TopRightZNUD.Value = UtopiaAirdropSettings.rightUpCornerMap[2];

            DropLocationsLB.DisplayMember = "DisplayName";
            DropLocationsLB.ValueMember = "Value";
            DropLocationsLB.DataSource = UtopiaAirdropSettings.dropLocations;

            FlareAirdropContainersLB.DisplayMember = "DisplayName";
            FlareAirdropContainersLB.ValueMember = "Value";
            FlareAirdropContainersLB.DataSource = UtopiaAirdropSettings.flareAirdropContainers;

            SpawnCreaturesLB.DisplayMember = "DisplayName";
            SpawnCreaturesLB.ValueMember = "Value";
            SpawnCreaturesLB.DataSource = UtopiaAirdropSettings.spawnCreatures;

            LootPoolsLB.DisplayMember = "DisplayName";
            LootPoolsLB.ValueMember = "Value";
            LootPoolsLB.DataSource = UtopiaAirdropSettings.lootPools;

            useraction = true;
        }
        private void DropLocationsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropLocationsLB.SelectedItems.Count == 0) { return; }
            currentDroplocation = DropLocationsLB.SelectedItem as Droplocation;
            useraction = false;
            SetupDropZone();
            pictureBox1.Invalidate();
            useraction = true;
        }
        private void SetupDropZone()
        {
            DropLocationNameTB.Text = currentDroplocation.name;
            DropLocationXNUD.Value = currentDroplocation.dropPosition[0];
            DropLocationZNUD.Value = currentDroplocation.dropPosition[2];
            DroplocationRadiusNUD.Value = currentDroplocation.radius;
            DropLocationContainerLB.DataSource = currentDroplocation.airdropContainers;

        }
        private void DropLocationContainerLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropLocationContainerLB.SelectedItems.Count == 0) { return; }
            currentAirdropcontainer = DropLocationContainerLB.SelectedItem as Airdropcontainer;
            useraction = false;

            DropLocationContainerNameCB.SelectedIndex = DropLocationContainerNameCB.FindStringExact(currentAirdropcontainer.containerName);
            dropLocationContainerIsCarDropCB.Checked = currentAirdropcontainer.isCarDrop == 1 ? true : false;
            DropLocationContainerLootPoolLB.DataSource = currentAirdropcontainer.lootPools;
            useraction = true;
        }
        private void DropLocationNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentDroplocation.name = DropLocationNameTB.Text;
            UtopiaAirdropSettings.isDirty = true;
        }
        private void DropLocationXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentDroplocation.dropPosition[0] = (int)DropLocationXNUD.Value;
            pictureBox1.Invalidate();
            UtopiaAirdropSettings.isDirty = true;
        }
        private void DropLocationNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentDroplocation.dropPosition[2] = (int)DropLocationZNUD.Value;
            pictureBox1.Invalidate();
            UtopiaAirdropSettings.isDirty = true;
        }
        private void droplocationRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentDroplocation.radius = (int)DroplocationRadiusNUD.Value;
            pictureBox1.Invalidate();
            UtopiaAirdropSettings.isDirty = true;
        }
        private void darkButton5_Click(object sender, EventArgs e)
        {
            Droplocation NewDropzone = new Droplocation()
            {
                name = "New drop location",
                dropPosition = new decimal[] { currentproject.MapSize / 2, 0, currentproject.MapSize / 2 },
                radius = 50,
                airdropContainers = new BindingList<Airdropcontainer>()
            };
            UtopiaAirdropSettings.dropLocations.Add(NewDropzone);
            pictureBox1.Invalidate();
            UtopiaAirdropSettings.isDirty = true;
        }
        private void darkButton6_Click(object sender, EventArgs e)
        {
            UtopiaAirdropSettings.dropLocations.Remove(currentDroplocation);
            UtopiaAirdropSettings.isDirty = true;
        }
        private void darkButton39_Click(object sender, EventArgs e)
        {
            Airdropcontainer newAirdropcontainer = new Airdropcontainer()
            {
                isCarDrop = 0,
                containerName = "Utopia_Crate_Parachute",
                lootPools = new BindingList<string>()
            };
            currentDroplocation.airdropContainers.Add(newAirdropcontainer);
            UtopiaAirdropSettings.isDirty = true;
        }
        private void darkButton38_Click(object sender, EventArgs e)
        {
            if (DropLocationContainerLB.SelectedItems.Count <= 0) return;
            List<Airdropcontainer> removeitems = new List<Airdropcontainer>();
            foreach (var item in DropLocationContainerLB.SelectedItems)
            {
                removeitems.Add(item as Airdropcontainer);
            }
            foreach (Airdropcontainer removeitem in removeitems)
            {
                currentDroplocation.airdropContainers.Remove(removeitem);
                UtopiaAirdropSettings.isDirty = true;
            }
            DropLocationContainerLB.Refresh();


        }
        private void DropLocationContainerNameCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentAirdropcontainer.containerName = DropLocationContainerNameCB.GetItemText(DropLocationContainerNameCB.SelectedItem);
            DropLocationContainerLB.Invalidate();
            UtopiaAirdropSettings.isDirty = true;
        }
        private void dropLocationContainerIsCarDropCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentAirdropcontainer.isCarDrop = dropLocationContainerIsCarDropCB.Checked == true ? 1 : 0;
            if (currentAirdropcontainer.isCarDrop == 1)
            {
                currentAirdropcontainer.containerName = "Utopia_Car_Parachute";
            }
            else
            {
                currentAirdropcontainer.containerName = "Utopia_Container_Parachute";
            }
            useraction = false;
            DropLocationContainerNameCB.SelectedIndex = DropLocationContainerNameCB.FindStringExact(currentAirdropcontainer.containerName);
            DropLocationContainerLB.Invalidate();
            useraction = true;
            UtopiaAirdropSettings.isDirty = true;
        }
        private void FlareAirdropContainersLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FlareAirdropContainersLB.SelectedItems.Count <= 0) { return; }
            currentFlareAirdropcontainer = FlareAirdropContainersLB.SelectedItem as Airdropcontainer;
            useraction = false;
            FlareContainerNameCB.SelectedIndex = FlareContainerNameCB.FindStringExact(currentFlareAirdropcontainer.containerName);
            FlareContainerIsCarDropCB.Checked = currentFlareAirdropcontainer.isCarDrop == 1 ? true : false;
            FlareAirdropContainerLootPoolsLB.DataSource = currentFlareAirdropcontainer.lootPools;
            useraction = true;
        }
        private void FlareContainerNameCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentFlareAirdropcontainer.containerName = FlareContainerNameCB.GetItemText(FlareContainerNameCB.SelectedItem);
            FlareAirdropContainersLB.Invalidate();
            UtopiaAirdropSettings.isDirty = true;
        }
        private void FlareContainerIsCarDropCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentFlareAirdropcontainer.isCarDrop = FlareContainerIsCarDropCB.Checked == true ? 1 : 0;
            if (currentFlareAirdropcontainer.isCarDrop == 1)
            {
                currentFlareAirdropcontainer.containerName = "Utopia_Car_Parachute";
            }
            else
            {
                currentFlareAirdropcontainer.containerName = "Utopia_Container_Parachute";
            }
            useraction = false;
            FlareContainerNameCB.SelectedIndex = FlareContainerNameCB.FindStringExact(currentFlareAirdropcontainer.containerName);
            FlareAirdropContainersLB.Invalidate();
            useraction = true;
            UtopiaAirdropSettings.isDirty = true;
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            AddfromPredefinedItems form = new AddfromPredefinedItems()
            {
                UtopiaAirdropLootPools = UtopiaAirdropSettings.lootPools,
                _titlellabel = "Add Loot Pools",
                isLootList = false,
                isRewardTable = false,
                isRHTableList = false,
                ispredefinedweapon = false,
                isRHPredefinedWeapon = false,
                isLootchest = false,
                isLootBoxList = false,
                isUtopiaAirdroplootPools = true
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> predefweapon = form.WeaponList;
                foreach (string weapon in predefweapon)
                {
                    if (!currentAirdropcontainer.lootPools.Contains(weapon))
                    {
                        currentAirdropcontainer.lootPools.Add(weapon);
                        UtopiaAirdropSettings.isDirty = true;
                    }
                }
            }
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
            if (DropLocationContainerLootPoolLB.SelectedItems.Count <= 0) return;
            List<string> removeitems = new List<string>();
            foreach (var item in DropLocationContainerLootPoolLB.SelectedItems)
            {
                removeitems.Add(item as string);
            }
            foreach (string removeitem in removeitems)
            {
                currentAirdropcontainer.lootPools.Remove(removeitem);
                UtopiaAirdropSettings.isDirty = true;
            }
            DropLocationContainerLootPoolLB.Refresh();
        }
        private void darkButton8_Click(object sender, EventArgs e)
        {
            if (FlareAirdropContainersLB.SelectedItems.Count <= 0) return;
            List<Airdropcontainer> removeitems = new List<Airdropcontainer>();
            foreach (var item in FlareAirdropContainersLB.SelectedItems)
            {
                removeitems.Add(item as Airdropcontainer);
            }
            foreach (Airdropcontainer removeitem in removeitems)
            {
                UtopiaAirdropSettings.flareAirdropContainers.Remove(removeitem);
                if (FlareAirdropContainersLB.Items.Count <= 0)
                {
                    currentFlareAirdropcontainer = null;
                    FlareAirdropContainerLootPoolsLB.DataSource = null;
                    FlareContainerNameCB.Text = "";
                }
                UtopiaAirdropSettings.isDirty = true;
            }
            FlareAirdropContainersLB.Refresh();
        }
        private void darkButton7_Click(object sender, EventArgs e)
        {
            Airdropcontainer newAirdropcontainer = new Airdropcontainer()
            {
                isCarDrop = 0,
                containerName = "Utopia_Crate_Parachute",
                lootPools = new BindingList<string>()
            };
            UtopiaAirdropSettings.flareAirdropContainers.Add(newAirdropcontainer);
            UtopiaAirdropSettings.isDirty = true;
        }
        private void darkButton4_Click(object sender, EventArgs e)
        {
            if (FlareAirdropContainerLootPoolsLB.SelectedItems.Count <= 0) return;
            List<string> removeitems = new List<string>();
            foreach (var item in FlareAirdropContainerLootPoolsLB.SelectedItems)
            {
                removeitems.Add(item as string);
            }
            foreach (string removeitem in removeitems)
            {
                currentFlareAirdropcontainer.lootPools.Remove(removeitem);
                UtopiaAirdropSettings.isDirty = true;
            }
            FlareAirdropContainerLootPoolsLB.Refresh();
        }
        private void darkButton3_Click(object sender, EventArgs e)
        {
            AddfromPredefinedItems form = new AddfromPredefinedItems()
            {
                UtopiaAirdropLootPools = UtopiaAirdropSettings.lootPools,
                _titlellabel = "Add Loot Pools",
                isLootList = false,
                isRewardTable = false,
                isRHTableList = false,
                ispredefinedweapon = false,
                isRHPredefinedWeapon = false,
                isLootchest = false,
                isLootBoxList = false,
                isUtopiaAirdroplootPools = true
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> predefweapon = form.WeaponList;
                foreach (string weapon in predefweapon)
                {
                    if (!currentFlareAirdropcontainer.lootPools.Contains(weapon))
                    {
                        currentFlareAirdropcontainer.lootPools.Add(weapon);
                        UtopiaAirdropSettings.isDirty = true;
                    }
                }
            }
        }
        private void darkButton12_Click(object sender, EventArgs e)
        {
            if (SpawnCreaturesLB.SelectedItems.Count <= 0) return;
            List<string> removeitems = new List<string>();
            foreach (var item in SpawnCreaturesLB.SelectedItems)
            {
                removeitems.Add(item as string);
            }
            foreach (string removeitem in removeitems)
            {
                UtopiaAirdropSettings.spawnCreatures.Remove(removeitem);
                UtopiaAirdropSettings.isDirty = true;
            }
            SpawnCreaturesLB.Refresh();
        }
        private void darkButton11_Click(object sender, EventArgs e)
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
                    if (!UtopiaAirdropSettings.spawnCreatures.Any(x => x == l))
                    {
                        UtopiaAirdropSettings.spawnCreatures.Add(l);
                        UtopiaAirdropSettings.isDirty = true;
                    }
                    else
                    {
                        MessageBox.Show("There is allready a Creature added : " + l);
                    }
                }
            }
        }

        private void LootPoolsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LootPoolsLB.SelectedItems.Count <= 0) return;
            currentlootpool = LootPoolsLB.SelectedItem as Lootpool;
            useraction = false;

            LootPoolTV.Nodes.Clear();
            TreeNode root = new TreeNode(currentlootpool.lootPoolName)
            {
                Tag = "Parent"
            };
            TreeNode maxitems = new TreeNode("Max Items")
            {
                Tag = "Maxitems"
            };
            root.Nodes.Add(maxitems);
            foreach (Item item in currentlootpool.items)
            {
                root.Nodes.Add(CreateLootNode(item));
            }
            LootPoolTV.Nodes.Add(root);
            root.Expand();

            useraction = true;
        }

        private TreeNode CreateLootNode(Item eL)
        {
            TreeNode ExpansionLootTN = new TreeNode(eL.name)
            {
                Tag = eL
            };
            TreeNode QuantitytTN = new TreeNode("Quantity")
            {
                Tag = "Quantity"
            };
            ExpansionLootTN.Nodes.Add(QuantitytTN);
            TreeNode AttachmentTN = new TreeNode("Attachments")
            {
                Tag = "Attachments"
            };
            foreach (attachments elv in eL.attachments)
            {
                AttachmentTN.Nodes.Add(getLootattachemnts(elv));
            }
            ExpansionLootTN.Nodes.Add(AttachmentTN);
            return ExpansionLootTN;
        }
        private TreeNode getLootattachemnts(attachments elv)
        {
            TreeNode ExpansionLootVarientTN = new TreeNode(elv.attachName)
            {
                Tag = elv
            };
            TreeNode AttachmentTN = new TreeNode("Quantity")
            {
                Tag = "Quantity"
            };
            ExpansionLootVarientTN.Nodes.Add(AttachmentTN);
            return ExpansionLootVarientTN;
        }

        public int HeliCrashMapscale = 1;
        private Point _mouseLastPosition;
        private Point _newscrollPosition;


        private void trackBar2_MouseUp(object sender, MouseEventArgs e)
        {
            HeliCrashMapscale = trackBar2.Value;
            SetUtopiaAirdropScale();

        }
        private void SetUtopiaAirdropScale()
        {
            float scalevalue = HeliCrashMapscale * 0.05f;
            float mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            pictureBox1.Size = new Size(newsize, newsize);
        }
        private void DrawAll(object sender, PaintEventArgs e)
        {
            float scalevalue = HeliCrashMapscale * 0.05f;
            foreach (Droplocation zones in UtopiaAirdropSettings.dropLocations)
            {
                int centerX = (int)(Math.Round((float)zones.dropPosition[0], 0) * scalevalue);
                int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round((float)zones.dropPosition[2], 0) * scalevalue);

                int radius = (int)((float)zones.radius * scalevalue);
                Point center = new Point(centerX, centerY);
                Pen pen = new Pen(Color.Red)
                {
                    Width = 4
                };
                if (currentDroplocation == zones)
                    pen.Color = Color.LimeGreen;
                else
                    pen.Color = Color.Red;
                getCircle(e.Graphics, pen, center, radius);
            }
        }
        private void getCircle(Graphics drawingArea, Pen penToUse, Point center, int radius)
        {
            Rectangle rect = new Rectangle(center.X - 1, center.Y - 1, 2, 2);
            drawingArea.DrawEllipse(penToUse, rect);
            Rectangle rect2 = new Rectangle(center.X - radius, center.Y - radius, radius * 2, radius * 2);
            drawingArea.DrawEllipse(penToUse, rect2);
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _mouseLastPosition = e.Location;
            }
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point changePoint = new Point(e.Location.X - _mouseLastPosition.X, e.Location.Y - _mouseLastPosition.Y);
                _newscrollPosition = new Point(-panel3.AutoScrollPosition.X - changePoint.X, -panel3.AutoScrollPosition.Y - changePoint.Y);
                if (_newscrollPosition.X <= 0)
                    _newscrollPosition.X = 0;
                if (_newscrollPosition.Y <= 0)
                    _newscrollPosition.Y = 0;
                panel3.AutoScrollPosition = _newscrollPosition;
                pictureBox1.Invalidate();
            }
        }
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            if (pictureBox1.Focused == false)
            {
                pictureBox1.Focus();
                panel3.AutoScrollPosition = _newscrollPosition;
                pictureBox1.Invalidate();
            }
        }
        private void PicBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                ZoomOut();
            }
            else
            {
                ZoomIn();
            }

        }
        private void ZoomIn()
        {
            int oldpictureboxhieght = pictureBox1.Height;
            int oldpitureboxwidht = pictureBox1.Width;
            Point oldscrollpos = panel3.AutoScrollPosition;
            int tbv = trackBar2.Value;
            int newval = tbv + 1;
            if (newval >= 20)
                newval = 20;
            trackBar2.Value = newval;
            HeliCrashMapscale = trackBar2.Value;
            SetUtopiaAirdropScale();
            if (pictureBox1.Height > panel3.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox1.Height * newy);
                _newscrollPosition.Y = y * -1;
                panel3.AutoScrollPosition = _newscrollPosition;
            }
            if (pictureBox1.Width > panel3.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox1.Width * newy);
                _newscrollPosition.X = x * -1;
                panel3.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox1.Invalidate();
        }
        private void ZoomOut()
        {
            int oldpictureboxhieght = pictureBox1.Height;
            int oldpitureboxwidht = pictureBox1.Width;
            Point oldscrollpos = panel3.AutoScrollPosition;
            int tbv = trackBar2.Value;
            int newval = tbv - 1;
            if (newval <= 1)
                newval = 1;
            trackBar2.Value = newval;
            HeliCrashMapscale = trackBar2.Value;
            SetUtopiaAirdropScale();
            if (pictureBox1.Height > panel3.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox1.Height * newy);
                _newscrollPosition.Y = y * -1;
                panel3.AutoScrollPosition = _newscrollPosition;
            }
            if (pictureBox1.Width > panel3.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox1.Width * newy);
                _newscrollPosition.X = x * -1;
                panel3.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox1.Invalidate();
        }
        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            var mouseEventArgs = e as MouseEventArgs;
            if (mouseEventArgs != null)
            {
                float scalevalue = HeliCrashMapscale * 0.05f;
                float mapsize = currentproject.MapSize;
                int newsize = (int)(mapsize * scalevalue);
                if (currentDroplocation == null) { return; }
                Cursor.Current = Cursors.WaitCursor;
                DropLocationXNUD.Value = (decimal)(mouseEventArgs.X / scalevalue);
                DropLocationZNUD.Value = (decimal)((newsize - mouseEventArgs.Y) / scalevalue);
                Cursor.Current = Cursors.Default;
                UtopiaAirdropSettings.isDirty = true;
                pictureBox1.Invalidate();
            }
        }


        public TreeNode currenttreenode;
        public Item currentitem;
        public attachments currentattchments;
        private void LootPoolTV_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            useraction = false;
            LootPoolNameGB.Visible = false;
            LootPoolQuantityGB.Visible = false;
            LootPoolMaxItemsGB.Visible = false;
            LootPoolItemGB.Visible = false;
            addNewItemToolStripMenuItem.Visible = false;
            removeItemToolStripMenuItem.Visible = false;
            addNewAttchmentToolStripMenuItem.Visible = false;
            removeAttachmentToolStripMenuItem.Visible = false;


            LootPoolTV.SelectedNode = e.Node;
            currenttreenode = e.Node;

            if (e.Node.Tag is string)
            {
                if (e.Node.Tag.ToString() == "Parent")
                {
                    LootPoolNameGB.Visible = true;
                    LootPoolNameTB.Text = currentlootpool.lootPoolName;
                    if (e.Button == MouseButtons.Right)
                    {
                        addNewItemToolStripMenuItem.Visible = true;
                        contextMenuStrip1.Show(Cursor.Position);
                    }
                }
                else if (e.Node.Tag.ToString() == "Attachments")
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        addNewAttchmentToolStripMenuItem.Visible = true;
                        contextMenuStrip1.Show(Cursor.Position);
                    }
                }
                else if (e.Node.Tag.ToString() == "Maxitems")
                {
                    LootPoolMaxItemsGB.Visible = true;
                    LootPoolMaxItemsNUD.Value = currentlootpool.maxItems;
                }
                else if (e.Node.Tag.ToString() == "Quantity")
                {
                    LootPoolQuantityGB.Visible = true;
                    if (e.Node.Parent.Tag is Item)
                    {
                        currentitem = e.Node.Parent.Tag as Item;
                        LootPoolQuantityNUD.Value = currentitem.quantity;
                    }
                    else if (e.Node.Parent.Tag is attachments)
                    {
                        currentattchments = e.Node.Parent.Tag as attachments;
                        LootPoolQuantityNUD.Value = currentattchments.quantity;
                    }
                }
            }
            else if (e.Node.Tag is Item)
            {
                LootPoolItemGB.Visible = true;
                LootPoolItemGB.Text = "Item";
                currentitem = e.Node.Tag as Item;
                LootPoolItemTB.Text = currentitem.name;
                if (e.Button == MouseButtons.Right)
                {
                    removeItemToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
            else if (e.Node.Tag is attachments)
            {
                LootPoolItemGB.Visible = true;
                LootPoolItemGB.Text = "Attchemnts";
                currentattchments = e.Node.Tag as attachments;
                LootPoolItemTB.Text = currentattchments.attachName;
                if (e.Button == MouseButtons.Right)
                {
                    removeAttachmentToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }


            useraction = true;
        }
        private void LootPoolNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentlootpool.lootPoolName = LootPoolNameTB.Text;
            currenttreenode.Name = currenttreenode.Text = currentlootpool.lootPoolName;
            LootPoolsLB.Invalidate();
            UtopiaAirdropSettings.isDirty = true;
        }
        private void LootPoolMaxItemsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentlootpool.maxItems = (int)LootPoolMaxItemsNUD.Value;
            UtopiaAirdropSettings.isDirty = true;
        }
        private void LootPoolQuantityNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (currenttreenode.Parent.Tag is Item)
            {
                currentitem.quantity = (int)LootPoolQuantityNUD.Value;
            }
            else if (currenttreenode.Parent.Tag is attachments)
            {
                currentattchments.quantity = (int)LootPoolQuantityNUD.Value;
            }
            UtopiaAirdropSettings.isDirty = true;
        }
        private void addNewItemToolStripMenuItem_Click(object sender, EventArgs e)
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
                    Item NewItem = new Item()
                    {
                        name = l,
                        attachments = new BindingList<attachments>(),
                        quantity = 1
                    };
                    currentlootpool.items.Add(NewItem);
                    TreeNode tn = CreateLootNode(NewItem);
                    LootPoolTV.SelectedNode.Nodes.Add(tn);
                    FocusNode = tn;
                    UtopiaAirdropSettings.isDirty = true;
                }
                LootPoolTV.SelectedNode = FocusNode;
                LootPoolTV.Focus();
                currentitem = LootPoolTV.SelectedNode.Tag as Item;
            }
        }
        private void removeItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentlootpool.items.Remove(currentitem);
            LootPoolTV.SelectedNode.Remove();
            UtopiaAirdropSettings.isDirty = true;
        }
        private void addNewAttchmentToolStripMenuItem_Click(object sender, EventArgs e)
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
                    attachments NewItem = new attachments()
                    {
                        attachName = l,
                        quantity = 1
                    };
                    Item item = LootPoolTV.SelectedNode.Parent.Tag as Item;
                    item.attachments.Add(NewItem);
                    TreeNode tn = getLootattachemnts(NewItem);
                    LootPoolTV.SelectedNode.Nodes.Add(tn);
                    FocusNode = tn;
                    UtopiaAirdropSettings.isDirty = true;
                }
                LootPoolTV.SelectedNode = FocusNode;
                LootPoolTV.Focus();
                currentitem = LootPoolTV.SelectedNode.Tag as Item;
            }
        }
        private void removeAttachmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Item item = currenttreenode.Parent.Parent.Tag as Item;
            item.attachments.Remove(currentattchments);
            LootPoolTV.SelectedNode.Remove();
            UtopiaAirdropSettings.isDirty = true;
        }
        private void darkButton13_Click(object sender, EventArgs e)
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
                    LootPoolItemTB.Text = l;
                }
            }
        }
        private void LootPoolItemTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (currenttreenode.Tag is Item)
            {
                currentitem.name = LootPoolItemTB.Text;
            }
            else if (currenttreenode.Tag is attachments)
            {
                currentattchments.attachName = LootPoolItemTB.Text;
            }
            currenttreenode.Name = currenttreenode.Text = LootPoolItemTB.Text;
            UtopiaAirdropSettings.isDirty = true;
        }
        private void darkButton9_Click(object sender, EventArgs e)
        {
            Lootpool newlootpool = new Lootpool()
            {
                lootPoolName = "New Loot Pool",
                items = new BindingList<Item>(),
                maxItems = 10
            };
            UtopiaAirdropSettings.lootPools.Add(newlootpool);
            UtopiaAirdropSettings.isDirty = true;
        }
        private void darkButton10_Click(object sender, EventArgs e)
        {
            if (LootPoolsLB.SelectedItems.Count <= 0) return;
            UtopiaAirdropSettings.lootPools.Remove(currentlootpool);
            UtopiaAirdropSettings.isDirty = true;
        }
        
        private void heliHeightFromGroundNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            UtopiaAirdropSettings.heliHeightFromGround = (int)heliHeightFromGroundNUD.Value;
            UtopiaAirdropSettings.isDirty = true;
        }
        private void heliSpeedNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            UtopiaAirdropSettings.heliSpeed = (int)heliSpeedNUD.Value;
            UtopiaAirdropSettings.isDirty = true;
        }
        private void dropCrateContainerLifetimeInSecondsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            UtopiaAirdropSettings.dropCrateContainerLifetimeInSeconds = (int)dropCrateContainerLifetimeInSecondsNUD.Value;
            UtopiaAirdropSettings.isDirty = true;
        }
        private void minPlayersToStartAirdropNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            UtopiaAirdropSettings.minPlayersToStartAirdrop = (int)minPlayersToStartAirdropNUD.Value;
            UtopiaAirdropSettings.isDirty = true;
        }
        private void maxCreaturesNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            UtopiaAirdropSettings.maxCreatures = (int)maxCreaturesNUD.Value;
            UtopiaAirdropSettings.isDirty = true;
        }
        private void startDelayMinNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            UtopiaAirdropSettings.startDelayMin = (int)startDelayMinNUD.Value;
            UtopiaAirdropSettings.isDirty = true;
        }
        private void pkgIntervalMinNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            UtopiaAirdropSettings.pkgIntervalMin = (int)pkgIntervalMinNUD.Value;
            UtopiaAirdropSettings.isDirty = true;
        }
        private void awayMinNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            UtopiaAirdropSettings.awayMin = (int)awayMinNUD.Value;
            UtopiaAirdropSettings.isDirty = true;
        }
        private void titleTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            UtopiaAirdropSettings.title = titleTB.Text;
            UtopiaAirdropSettings.isDirty = true;
        }
        private void droppedMsgTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            UtopiaAirdropSettings.droppedMsg = droppedMsgTB.Text;
            UtopiaAirdropSettings.isDirty = true;
        }
        private void startMsgTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            UtopiaAirdropSettings.startMsg = startMsgTB.Text;
            UtopiaAirdropSettings.isDirty = true;
        }
        private void showMarkerForFlareDropCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            UtopiaAirdropSettings.showMarkerForFlareDrop = showMarkerForFlareDropCB.Checked == true ? 1 : 0;
            UtopiaAirdropSettings.isDirty = true;
        }
        private void TopRightXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            UtopiaAirdropSettings.rightUpCornerMap[0] = TopRightXNUD.Value;
            UtopiaAirdropSettings.isDirty = true;

        }
        private void TopRightZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            UtopiaAirdropSettings.rightUpCornerMap[2] = TopRightZNUD.Value;
            UtopiaAirdropSettings.isDirty = true;
        }
    }
}
