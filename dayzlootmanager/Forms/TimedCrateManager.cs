using DarkUI.Forms;
using DayZeLib;
using DayZeLib.DynamicWeatherPlugin;
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
using System.Windows.Forms;

namespace DayZeEditor
{
    public partial class TimedCrateManager : DarkForm
    {
        public Project currentproject { get; internal set; }
        public TypesFile vanillatypes;
        public TypesFile Expansiontypes;
        public List<TypesFile> ModTypes;
        public string TimedCreateSettingsPath { get; private set; }
        public string TimedCreateLootPath { get; private set; }
        public string Projectname { get; private set; }

        private bool useraction;
        public TimedCrates TimedCrates;
        public TimedCrateLoot TimedCrateLoot;

        public Crateconfig currentcrate { get; set; }
        public Cratelocation currentcratelocation { get; set; }
        public Vec3PandR currentposrot { get; set; }
        public TreeNode currenmttreenode { get; private set; }

        public Crate currentlootcrate { get; set; }
        public List currentlootlist { get; set; }

        public TimedCrateManager()
        {
            InitializeComponent();
        }
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
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            SaveFile();
        }
        private void SaveFile()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (TimedCrates.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(TimedCrates.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(TimedCrates.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(TimedCrates.Filename, Path.GetDirectoryName(TimedCrates.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(TimedCrates.Filename) + ".bak", true);
                }
                TimedCrates.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, Converters = { new BoolConverter() } };
                string jsonString = JsonSerializer.Serialize(TimedCrates, options);
                File.WriteAllText(TimedCrates.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(TimedCrates.Filename));
            }
            if (TimedCrateLoot.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(TimedCrateLoot.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(TimedCrateLoot.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(TimedCrateLoot.Filename, Path.GetDirectoryName(TimedCrateLoot.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(TimedCrateLoot.Filename) + ".bak", true);
                }
                TimedCrateLoot.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, Converters = { new BoolConverter() } };
                string jsonString = JsonSerializer.Serialize(TimedCrateLoot, options);
                File.WriteAllText(TimedCrateLoot.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(TimedCrateLoot.Filename));
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
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\MB_TimedCrate");
        }
        private void TimedCrateManager_Load(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Projectname = currentproject.ProjectName;
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            useraction = false;
            TimedCreateSettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\MB_TimedCrate\\CrateSettings.json";
            var options = new JsonSerializerOptions { Converters = { new BoolConverter() } };
            TimedCrates = JsonSerializer.Deserialize<TimedCrates>(File.ReadAllText(TimedCreateSettingsPath), options);
            TimedCrates.isDirty = false;
            TimedCrates.Filename = TimedCreateSettingsPath;
            TimedCrates.SetVectors();

            TimedCreateLootPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\MB_TimedCrate\\CustomLootData.json";
            TimedCrateLoot = JsonSerializer.Deserialize<TimedCrateLoot>(File.ReadAllText(TimedCreateLootPath), options);
            TimedCrateLoot.isDirty = false;
            TimedCrateLoot.Filename = TimedCreateLootPath;

            ServerStartGracePeriodNUD.Value = TimedCrates.ServerStartGracePeriod;

            LoadTimedCreateTreeView();

            useraction = true;

        }
        private void TimedCrateManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TimedCrates.isDirty || TimedCrateLoot.isDirty)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SaveFile();
                }
            }
        }

        public void LoadTimedCreateTreeView()
        {
            Cursor.Current = Cursors.WaitCursor;
            TimedCrateTV.Nodes.Clear();
            TreeNode RootNode = new TreeNode("Timed Crates:-")
            {
                Tag = "ParentRoot"
            };
            RootNode.Nodes.Add(AddSettings());
            RootNode.Nodes.Add(AddLoot());
            TimedCrateTV.Nodes.Add(RootNode);
            Cursor.Current = Cursors.WaitCursor;
        }

        private TreeNode AddSettings()
        {
            TreeNode TCBaseNode = new TreeNode("settings")
            {
                Tag = TimedCrates
            };
            TCBaseNode.Nodes.Add(new TreeNode($"Server Start Grace Period: - {TimedCrates.ServerStartGracePeriod}")
            {
                Tag = "ServerStartGracePeriod",
                Name = "ServerStartGracePeriod"
            });
            TreeNode CreateConfigNode = new TreeNode($"Crates:")
            {
                Tag = "Crateconfigs",
                Name = "Crateconfigs"
            };
            foreach(Crateconfig cf in TimedCrates.CrateConfigs)
            {
                CreateConfigNode.Nodes.Add(new TreeNode($"Crate - {cf.CrateType}")
                {
                    Tag = cf,
                    Name = "Crateconfig"
                });
            }
            TCBaseNode.Nodes.Add(CreateConfigNode);
            return TCBaseNode;
        }
        private TreeNode AddLoot()
        {
            TreeNode KRBaseNode = new TreeNode("Loot")
            {
                Tag = TimedCrateLoot
            };
            foreach(Crate c in TimedCrateLoot.Crates)
            {
                KRBaseNode.Nodes.Add(new TreeNode($"Loot Config -  {c.cratetype}")
                {
                    Tag = c,
                    Name = "LootConfig"
                });
            }
            return KRBaseNode;
        }

        private void TimedCrateTV_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            currenmttreenode = e.Node;
            TimedCrateTV.SelectedNode = e.Node;
            if (e.Button == MouseButtons.Right)
            {
                addNewCrateToolStripMenuItem.Visible = false;
                removeCrateToolStripMenuItem.Visible = false;
                addNewLootConfigToolStripMenuItem.Visible = false;
                removeLootConfigToolStripMenuItem.Visible = false;
                if (e.Node.Tag.ToString() == "Crateconfigs")
                {
                    addNewCrateToolStripMenuItem.Visible = true;
                }
                else if(e.Node.Tag is Crateconfig)
                {
                    removeCrateToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag is TimedCrateLoot)
                {
                    addNewLootConfigToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag is Crate)
                {
                    removeLootConfigToolStripMenuItem.Visible = true;
                }
                contextMenuStrip1.Show(Cursor.Position);
            }
        }
        private void addNewCrateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Crateconfig newcc = new Crateconfig()
            {
                StaticSpawns = false,
                ActiveCrateCount = 1,
                AutoRefresh = true,
                RefreshInterval = 2,
                CrateType = "Locked_Rust_Crate",
                UseCrateNotifications = true,
                CrateSpawnNotification = "A crate has spawned!",
                CrateStartNotification  = "Crate countdown started!",
                CrateEndNotification = "Crate unlocked! Come get some loot!",
                CountdownTime = 1,
                ZombieSpawnInterval = 60,
                ZombieSpawnMin = 3,
                ZombieSpawnMax = 6,
                ZombieSpawningEnabled = true,
                UseBasicMarkers = false,
                UseMarkersDuringCountdownOnly = false,
                ZombieClasses = new BindingList<string>(),
                CrateLocations = new BindingList<Cratelocation>()
            };
            TimedCrates.CrateConfigs.Add(newcc);
            TimedCrates.isDirty = true;
            currenmttreenode.Nodes.Add(new TreeNode($"Crate - {newcc.CrateType}")
            {
                Tag = newcc,
                Name = "Crateconfig"
            });
        }
        private void removeCrateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TimedCrates.CrateConfigs.Remove(currentcrate);
            TimedCrates.isDirty = true;
            currenmttreenode.Parent.Nodes.Remove(currenmttreenode);
        }
        private void addNewLootConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Crate newcrate = new Crate()
            {
                cratetype = currentcrate.CrateType,
                numberOfItemsToSpawn = 0,
                IsRandom = false,
                List = new BindingList<List>()
            };
            TimedCrateLoot.Crates.Add(newcrate);
            TimedCrateLoot.isDirty = true;
            currenmttreenode.Nodes.Add(new TreeNode($"Loot Config -  {newcrate.cratetype}")
            {
                Tag = newcrate,
                Name = "LootConfig"
            });
        }
        private void removeLootConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TimedCrateLoot.Crates.Remove(currentlootcrate);
            TimedCrateLoot.isDirty = true;
            currenmttreenode.Parent.Nodes.Remove(currenmttreenode);
        }
        private void TimedCrateTV_AfterSelect(object sender, TreeViewEventArgs e)
        {
            CrateConfigsGB.Visible = false;
            GeneralSettingsGB.Visible = false;
            LootConfigGB.Visible = false;
            currenmttreenode = e.Node;
            if (e.Node.Tag is string)
            {
                if(e.Node.Tag.ToString() == "ServerStartGracePeriod")
                {
                    GeneralSettingsGB.Visible=true; 
                }
            }
            else if (e.Node.Tag is Crateconfig)
            {
                currentcrate = e.Node.Tag as Crateconfig;
                CrateConfigsGB.Visible = true;
                useraction = false;
                StaticSpawnsCB.Checked = currentcrate.StaticSpawns;
                ActiveCrateCountNUD.Value = currentcrate.ActiveCrateCount;
                AutoRefreshCB.Checked = currentcrate.AutoRefresh;
                RefreshIntervalNUD.Value = currentcrate.RefreshInterval;
                CrateTypeCB.SelectedIndex = CrateTypeCB.FindStringExact(currentcrate.CrateType);
                UseCrateNotificationsCB.Checked = currentcrate.UseCrateNotifications;
                CrateSpawnNotificationTB.Text = currentcrate.CrateSpawnNotification;
                CrateStartNotificationTB.Text = currentcrate.CrateStartNotification;
                CrateEndNotificationTB.Text = currentcrate.CrateEndNotification;
                CountdownTimeNUD.Value = currentcrate.CountdownTime;
                ZombieSpawnIntervalNUD.Value = currentcrate.ZombieSpawnInterval;
                ZombieSpawnMinNUD.Value = currentcrate.ZombieSpawnMin;
                ZombieSpawnMaxNUD.Value = currentcrate.ZombieSpawnMax;
                ZombieSpawningEnabledCB.Checked = currentcrate.ZombieSpawningEnabled;
                UseBasicMarkersCB.Checked = currentcrate.UseBasicMarkers;
                UseMarkersDuringCountdownOnlyCB.Checked = currentcrate.UseMarkersDuringCountdownOnly;

                ZombieClassesLB.DisplayMember = "DisplayName";
                ZombieClassesLB.ValueMember = "Value";
                ZombieClassesLB.DataSource = currentcrate.ZombieClasses;

                CrateLocationsLB.DisplayMember = "DisplayName";
                CrateLocationsLB.ValueMember = "Value";
                CrateLocationsLB.DataSource = currentcrate.CrateLocations;

                if(CrateLocationsLB.Items.Count ==0)
                {
                    useraction = false;
                    CrateLocationPOSXNUD.Value = (decimal)0;
                    CrateLocationPOSYNUD.Value = (decimal)0;
                    CrateLocationPOSZNUD.Value = (decimal)0;

                    CrateLocationROTXNUD.Value = (decimal)0;
                    CrateLocationROTYNUD.Value = (decimal)0;
                    CrateLocationROTZNUD.Value = (decimal)0;

                    useraction = true;
                }

                CheckLootConfig();

                useraction = true;
            }
            else if (e.Node.Tag is Crate)
            {
                LootConfigGB.Visible = true;
                currentlootcrate = e.Node.Tag as Crate;
                useraction = false;
                LootCrateTypeCB.SelectedIndex = LootCrateTypeCB.FindStringExact(currentlootcrate.cratetype);
                LootnumberOfItemsToSpawnNUD.Value = currentlootcrate.numberOfItemsToSpawn;
                LootIsRandomCB.Checked = currentlootcrate.IsRandom;
                LootListLB.DisplayMember = "DisplayName";
                LootListLB.ValueMember = "Value";
                LootListLB.DataSource = currentlootcrate.List;
                useraction = true;
            }

        }
        private void CrateConfigBool_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CheckBox chk = sender as CheckBox;
            Helper.SetBoolValue(currentcrate, chk.Name.Substring(0, chk.Name.Length - 2), chk.Checked);
            TimedCrates.isDirty = true;
        }
        private void CrateConfigInt_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            NumericUpDown nud = sender as NumericUpDown;
            Helper.SetIntValue(currentcrate, nud.Name.Substring(0, nud.Name.Length - 3), (int)nud.Value);
            TimedCrates.isDirty = true;
        }
        private void CrateConfigText_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            TextBox textbox = sender as TextBox;
            Helper.SetStringValue(currentcrate, textbox.Name.Substring(0, textbox.Name.Length - 2), textbox.Text);
            TimedCrates.isDirty = true;
        }
        private void CrateTypeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentcrate.CrateType = CrateTypeCB.GetItemText(CrateTypeCB.SelectedItem);
            currenmttreenode.Text = $"Crate - {currentcrate.CrateType}";
            TimedCrates.isDirty = true;
            CheckLootConfig();
        }
        private void CheckLootConfig()
        {
            if (TimedCrateLoot.Crates.FirstOrDefault(x => x.cratetype == currentcrate.CrateType) == null)
            {
                DialogResult dialogResult = MessageBox.Show($"There is no matching Loot Config for {currentcrate.CrateType}.\nWould you like me to crate a blank one for you?", "Loot Config", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Crate newcrate = new Crate()
                    {
                        cratetype = currentcrate.CrateType,
                        numberOfItemsToSpawn = 0,
                        IsRandom = false,
                        List = new BindingList<List>()
                    };
                    TimedCrateLoot.Crates.Add(newcrate);
                    TimedCrateLoot.isDirty = true;
                    TimedCrateTV.Nodes[0].Nodes[1].Nodes.Add(new TreeNode($"Loot Config -  {newcrate.cratetype}")
                    {
                        Tag = newcrate,
                        Name = "LootConfig"
                    });
                }
            }
        }
        private void darkButton14_Click(object sender, EventArgs e)
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
                foreach (string l in addedtypes)
                {
                    currentcrate.ZombieClasses.Add(l);
                }
            }
            TimedCrates.isDirty = true;
        }
        private void darkButton13_Click(object sender, EventArgs e)
        {
            if (ZombieClassesLB.SelectedItems.Count < 1) return;
            for (int i = 0; i < ZombieClassesLB.SelectedItems.Count; i++)
            {
                currentcrate.ZombieClasses.Remove(ZombieClassesLB.GetItemText(ZombieClassesLB.SelectedItems[0]));
            }
            TimedCrates.isDirty = true;
        }
        private void CrateLocationsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CrateLocationsLB.SelectedItems.Count < 1) return;
            currentcratelocation = CrateLocationsLB.SelectedItem as Cratelocation;
            currentposrot = (CrateLocationsLB.SelectedItem as Cratelocation)._POSROT;
            useraction = false;
            CrateLocationPOSXNUD.Value = (decimal)currentposrot.Position.X;
            CrateLocationPOSYNUD.Value = (decimal)currentposrot.Position.Y;
            CrateLocationPOSZNUD.Value = (decimal)currentposrot.Position.Z;

            CrateLocationROTXNUD.Value = (decimal)currentposrot.Rotation.X;
            CrateLocationROTYNUD.Value = (decimal)currentposrot.Rotation.Y;
            CrateLocationROTZNUD.Value = (decimal)currentposrot.Rotation.Z;

            useraction = true;
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
            Cratelocation newcl = new Cratelocation()
            {
                _POSROT = new Vec3PandR(new float[] {0,0,0}, new float[] { 0,0,0}, true)
            };
            currentcrate.CrateLocations.Add(newcl);
            TimedCrates.isDirty = true;
            CrateLocationsLB.SelectedIndex = -1;
            CrateLocationsLB.SelectedIndex = CrateLocationsLB.Items.Count - 1;
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            if (CrateLocationsLB.SelectedItems.Count < 1) return;
            int index = CrateLocationsLB.SelectedIndex;
            currentcrate.CrateLocations.Remove(currentcratelocation);
            TimedCrates.isDirty = true;
            if (index == CrateLocationsLB.Items.Count)
                CrateLocationsLB.SelectedIndex = index - 1;
            else
            {
                CrateLocationsLB.SelectedIndex = -1;
                CrateLocationsLB.SelectedIndex = index;
            }

            if (CrateLocationsLB.Items.Count == 0)
            {
                useraction = false;
                CrateLocationPOSXNUD.Value = (decimal)0;
                CrateLocationPOSYNUD.Value = (decimal)0;
                CrateLocationPOSZNUD.Value = (decimal)0;

                CrateLocationROTXNUD.Value = (decimal)0;
                CrateLocationROTYNUD.Value = (decimal)0;
                CrateLocationROTZNUD.Value = (decimal)0;

                useraction = true;
            }
        }
        private void darkButton66_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Import CRate Locations";
            openFileDialog.Filter = "Expansion Map|*.map|Object Spawner|*.json|DayZ Editor|*.dze";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                DialogResult dialogResult = MessageBox.Show("Clear Exisitng locations?", "Clear position", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    currentcrate.CrateLocations.Clear();
                }
                switch (openFileDialog.FilterIndex)
                {
                    case 1:
                        string[] fileContent = File.ReadAllLines(filePath);
                        for (int i = 0; i < fileContent.Length; i++)
                        {
                            if (fileContent[i] == "") continue;
                            string[] linesplit = fileContent[i].Split('|');
                            string[] XYZ = linesplit[1].Split(' ');
                            string[] YPR = linesplit[2].Split(' ');
                            float[] newposition = new float[] { Convert.ToSingle(XYZ[0]), Convert.ToSingle(XYZ[1]), Convert.ToSingle(XYZ[2]) };
                            float[] newrotaion = new float[] { Convert.ToSingle(YPR[0]), Convert.ToSingle(YPR[1]), Convert.ToSingle(YPR[2]) };
                            currentcrate.CrateLocations.Add(new Cratelocation()
                            {
                                _POSROT = new Vec3PandR(newposition, newrotaion, true)
                            });

                        }
                        break;
                    case 2:
                        ObjectSpawnerArr newobjectspawner = JsonSerializer.Deserialize<ObjectSpawnerArr>(File.ReadAllText(filePath));
                        foreach (SpawnObjects so in newobjectspawner.Objects)
                        {
                            currentcrate.CrateLocations.Add(new Cratelocation()
                            {
                                _POSROT = new Vec3PandR(so.pos, so.ypr, true)
                            });
                        }
                        break;
                    case 3:
                        DZE importfile = DZEHelpers.LoadFile(filePath);
                        foreach (Editorobject eo in importfile.EditorObjects)
                        {
                            currentcrate.CrateLocations.Add(new Cratelocation()
                            {
                                _POSROT = new Vec3PandR(eo.Position, eo.Orientation, true)
                            });
                        }
                        break;
                }
                CrateLocationsLB.SelectedIndex = -1;
                CrateLocationsLB.SelectedIndex = CrateLocationsLB.Items.Count - 1;
                CrateLocationsLB.Refresh();
                TimedCrates.isDirty = true;
            }
        }
        private void darkButton65_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "ExportTimes Crate locations";
            save.Filter = "Expansion Map |*.map|Object Spawner|*.json";
            save.FileName = "TimedCrate_Location_" + currentcrate.CrateType.ToString();
            if (save.ShowDialog() == DialogResult.OK)
            {
                switch (save.FilterIndex)
                {
                    case 1:
                        StringBuilder SB = new StringBuilder();
                        foreach (Cratelocation cl in currentcrate.CrateLocations)
                        {
                            SB.AppendLine(currentcrate.CrateType + "|" + cl._POSROT.GetExpansionString());
                        }
                        File.WriteAllText(save.FileName, SB.ToString());
                        break;
                    case 2:
                        ObjectSpawnerArr newobjectspawner = new ObjectSpawnerArr();
                        newobjectspawner.Objects = new BindingList<SpawnObjects>();
                        foreach (Cratelocation cl in currentcrate.CrateLocations)
                        {
                            SpawnObjects newobject = new SpawnObjects();
                            newobject.name = currentcrate.CrateType;
                            newobject.pos = cl._POSROT.Position.getfloatarray();
                            newobject.ypr = cl._POSROT.Rotation.getfloatarray();
                            newobject.scale = 1;
                            newobject.enableCEPersistency = false;
                            newobjectspawner.Objects.Add(newobject);
                        }
                        var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                        string jsonString = JsonSerializer.Serialize(newobjectspawner, options);
                        File.WriteAllText(save.FileName, jsonString);
                        break;
                }
            }
        }
        private void CrateLocationPOSXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentposrot.Position.X = (float)CrateLocationPOSXNUD.Value;
            TimedCrates.isDirty = true;
        }
        private void CrateLocationPOSYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentposrot.Position.Y = (float)CrateLocationPOSYNUD.Value;
            TimedCrates.isDirty = true;
        }
        private void CrateLocationPOSZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentposrot.Position.Z = (float)CrateLocationPOSZNUD.Value;
            TimedCrates.isDirty = true;
        }
        private void CrateLocationROTXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentposrot.Rotation.X = (float)CrateLocationROTXNUD.Value;
            TimedCrates.isDirty = true;
        }
        private void CrateLocationROTYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentposrot.Rotation.Y = (float)CrateLocationROTYNUD.Value;
            TimedCrates.isDirty = true;
        }
        private void CrateLocationROTZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentposrot.Rotation.Z = (float)CrateLocationROTZNUD.Value;
            TimedCrates.isDirty = true;
        }
        private void ServerStartGracePeriodNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TimedCrates.ServerStartGracePeriod = (int)ServerStartGracePeriodNUD.Value;
            currenmttreenode.Text = $"Server Start Grace Period: - {TimedCrates.ServerStartGracePeriod}";
            TimedCrates.isDirty = true;
        }
        private void LootCrateTypeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentlootcrate.cratetype = LootCrateTypeCB.GetItemText(LootCrateTypeCB.SelectedItem);
            currenmttreenode.Text = $"Loot Config - {currentlootcrate.cratetype}";
            TimedCrateLoot.isDirty = true;
        }
        private void LootnumberOfItemsToSpawnNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentlootcrate.numberOfItemsToSpawn = (int)LootnumberOfItemsToSpawnNUD.Value;
            TimedCrateLoot.isDirty = true;
        }
        private void LootIsRandomCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentlootcrate.IsRandom = LootIsRandomCB.Checked;
            TimedCrateLoot.isDirty = true;
        }
        private void LootListLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LootListLB.SelectedItems.Count < 1) return;
            currentlootlist = LootListLB.SelectedItem as List;
            useraction = false;
            LootitemTB.Text = currentlootlist.item;
            LootquantityNUD.Value = currentlootlist.quantity;

            LootattachmentsLB.DisplayMember = "DisplayName";
            LootattachmentsLB.ValueMember = "Value";
            LootattachmentsLB.DataSource = currentlootlist.attachments;

            useraction = true;
        }
        private void darkButton4_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseMultipleofSameItem = true
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    List newlist = new List()
                    {
                        item = l,
                        attachments = new BindingList<string>(),
                        quantity = 1 
                    };
                    currentlootcrate.List.Add(newlist);
                }
            }
            TimedCrateLoot.isDirty = true;
        }
        private void darkButton3_Click(object sender, EventArgs e)
        {
            if (LootListLB.SelectedItems.Count < 1) return;
            List<List> removable = new List<List>();
            for (int i = 0; i < LootListLB.SelectedItems.Count; i++)
            {
                removable.Add(LootListLB.SelectedItems[i] as List);
            }
            foreach(List l in removable)
            {
                currentlootcrate.List.Remove(l);
            }
            TimedCrateLoot.isDirty = true;
        }
        private void LootitemTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentlootlist.item = LootitemTB.Text;
            TimedCrateLoot.isDirty = true;
        }
        private void LootquantityNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentlootlist.quantity = (int)LootquantityNUD.Value;
            TimedCrateLoot.isDirty = true;
        }
        private void darkButton6_Click(object sender, EventArgs e)
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
                foreach (string l in addedtypes)
                {
                    if(!currentlootlist.attachments.Contains(l))
                        currentlootlist.attachments.Add(l);
                }
            }
            TimedCrateLoot.isDirty = true;
        }
        private void darkButton5_Click(object sender, EventArgs e)
        {
            if (LootattachmentsLB.SelectedItems.Count < 1) return;
            List<string> removable = new List<string>();
            for (int i = 0; i < LootattachmentsLB.SelectedItems.Count; i++)
            {
                removable.Add(LootattachmentsLB.SelectedItems[i] as string);
            }
            foreach (string s in removable)
            {
                currentlootlist.attachments.Remove(s);
            }
            TimedCrateLoot.isDirty = true;
        }


    }
}
