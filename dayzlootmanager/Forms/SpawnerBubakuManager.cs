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
    public partial class SpawnerBubakuManager : DarkForm
    {
        public Project currentproject { get; set; }
        public TypesFile vanillatypes;
        public TypesFile Expansiontypes;
        public List<TypesFile> ModTypes;
        public string SpawnerBubakuConfigPath { get; private set; }
        public SpawnerBubaku SpawnerBubakuConfig;
        public string Projectname;

        public Bubaklocation CurrentBubaklocation { get; private set; }

        public Vec3PandR currentspawnPosition;
        private bool useraction = false;
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

        public SpawnerBubakuManager()
        {
            InitializeComponent();
        }

        private void SpawnerBukakuManager_Load(object sender, EventArgs e)
        {
            useraction = false;

            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Projectname = currentproject.ProjectName;
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            useraction = false;
            SpawnerBubakuConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\SpawnerBubaku\\SpawnerBubaku.json";
            SpawnerBubakuConfig = JsonSerializer.Deserialize<SpawnerBubaku>(File.ReadAllText(SpawnerBubakuConfigPath));
            SpawnerBubakuConfig.Getalllists();
            SpawnerBubakuConfig.isDirty = false;
            SpawnerBubakuConfig.Filename = SpawnerBubakuConfigPath;

            LogLevelNUD.Value = (int)SpawnerBubakuConfig.getLogLevel();

            SpawnerBukakuLocationsLB.DisplayMember = "DisplayName";
            SpawnerBukakuLocationsLB.ValueMember = "Value";
            SpawnerBukakuLocationsLB.DataSource = SpawnerBubakuConfig.BubakLocations;

            pictureBox1.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Livonia maop size is 12800 x 12800, 0,0 bottom left, center 6400 x 6400
            pictureBox1.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox1.Paint += new PaintEventHandler(DrawSpawnerBubakuSpawner);
            trackBar1.Value = 1;
            SetSpawnerBubakuScale();

            tabControl1.ItemSize = new Size(0, 1);
            toolStripButton12.Checked = true;

            useraction = true;
        }
        private void SpawnerBukakuManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SpawnerBubakuConfig.isDirty)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Savefiles();
                }
            }
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\SpawnerBubaku");
        }
        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }
        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripButton12.Checked = false;
            toolStripButton13.Checked = false;
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    toolStripButton12.Checked = true;
                    break;
                case 1:
                    toolStripButton13.Checked = true;
                    break;
            }
        }
        private void Savefiles(bool shownotification = true)
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (SpawnerBubakuConfig.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(SpawnerBubakuConfig.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(SpawnerBubakuConfig.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(SpawnerBubakuConfig.Filename, Path.GetDirectoryName(SpawnerBubakuConfig.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(SpawnerBubakuConfig.Filename) + ".bak", true);
                }
                SpawnerBubakuConfig.SetSpawnerPointFiles();
                SpawnerBubakuConfig.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(SpawnerBubakuConfig, options);
                File.WriteAllText(SpawnerBubakuConfig.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(SpawnerBubakuConfig.Filename));
            }
            if (shownotification)
            {
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
        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            Savefiles();
        }

        private void LogLevelNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            SpawnerBubakuConfig.SetLogLevel((int)LogLevelNUD.Value);
        }
        private void darkButton4_Click(object sender, EventArgs e)
        {
            SpawnerBubakuConfig.AddNewLocation();
            SpawnerBukakuLocationsLB.Refresh();
            SpawnerBukakuLocationsLB.SelectedIndex = -1;
            SpawnerBukakuLocationsLB.SelectedIndex = SpawnerBukakuLocationsLB.Items.Count - 1;
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    DZE importfile = DZEHelpers.LoadFile(filePath);
                    SpawnerBubakuConfig.AddNewLocation(importfile);
                    SpawnerBukakuLocationsLB.Refresh();
                }
            }
        }
        private void darkButton3_Click(object sender, EventArgs e)
        {
            if (SpawnerBukakuLocationsLB.Items.Count <= 0) return;
            SpawnerBubakuConfig.removeLocation(SpawnerBukakuLocationsLB.SelectedItem as Bubaklocation);
            SpawnerBukakuLocationsLB.Refresh();
            if (SpawnerBukakuLocationsLB.Items.Count <= 0)
                BubakLocationGB.Visible = false;
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
            DZE newdze = new DZE()
            {
                MapName = Path.GetFileNameWithoutExtension(currentproject.MapPath).Split('_')[0]
            };
            int m_Id = 0;
            string filename = "";
            var result = MessageBox.Show("Would yo ulike to export the trigger as well?", "Export options", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel)
            {
                return;
            }
            else if (result == DialogResult.Yes)
            {
                Editorobject Triggerobject = new Editorobject()
                {
                    Type = "GiftBox_Large_1",
                    DisplayName = "GiftBox_Large_1",
                    Position = CurrentBubaklocation._triggerpos.GetPositionFloatArray(),
                    Orientation = CurrentBubaklocation._triggerpos.GetRotationFloatArray(),
                    Scale = 1.0f,
                    Model = "",
                    Flags = 2147483647,
                    m_Id = m_Id
                };
                newdze.EditorObjects.Add(Triggerobject);
                m_Id++;
            }
            foreach (Vec3PandR vec3pandr in CurrentBubaklocation._spawnerpos)
            {
                Editorobject SpawnObject = new Editorobject()
                {
                    Type = "GiftBox_Small_1",
                    DisplayName = "GiftBox_Small_1",
                    Position = vec3pandr.GetPositionFloatArray(),
                    Orientation = vec3pandr.GetRotationFloatArray(),
                    Scale = 1.0f,
                    Model = "",
                    Flags = 2147483647,
                    m_Id = m_Id
                };
                newdze.EditorObjects.Add(SpawnObject);
                m_Id++;
            }
            filename = "Bubaku Spawner Location - " + CurrentBubaklocation.name;
            newdze.CameraPosition = newdze.EditorObjects[0].Position;
            SaveFileDialog save = new SaveFileDialog();
            save.FileName = filename;
            if (save.ShowDialog() == DialogResult.OK)
            {
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(newdze, options);
                File.WriteAllText(save.FileName + ".dze", jsonString);
            }
        }
        private void SpawnerBukakuLocationsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SpawnerBukakuLocationsLB.SelectedItems.Count < 1) return;
            BubakLocationGB.Visible = true;
            CurrentBubaklocation = SpawnerBukakuLocationsLB.SelectedItem as Bubaklocation;
            useraction = false;
            BubakLocationNameTB.Text = CurrentBubaklocation.GetName();
            BubakLocationWorkingHoursStartNUD.Value = CurrentBubaklocation.getworkinghours()[0];
            BubakLocationWorkingHoursEndNUD.Value = CurrentBubaklocation.getworkinghours()[1];
            BubakLocationTriggerPosXNUD.Value = (decimal)CurrentBubaklocation._triggerpos.Position.X;
            BubakLocationTriggerPosYNUD.Value = (decimal)CurrentBubaklocation._triggerpos.Position.Y;
            BubakLocationTriggerPosZNUD.Value = (decimal)CurrentBubaklocation._triggerpos.Position.Z;
            if (BubakLocationTriggerPositionRotSpecifiedCB.Checked = CurrentBubaklocation._triggerpos.rotspecified)
            {
                BubakLocationTriggerRotXNUD.Value = (decimal)CurrentBubaklocation._triggerpos.Rotation.X;
                BubakLocationTriggerRotYNUD.Value = (decimal)CurrentBubaklocation._triggerpos.Rotation.Y;
                BubakLocationTriggerRotZNUD.Value = (decimal)CurrentBubaklocation._triggerpos.Rotation.Z;
            }
            BubakLocationTriggerMinXNUD.Value = CurrentBubaklocation.gettriggermins()[0];
            BubakLocationTriggerMinYNUD.Value = CurrentBubaklocation.gettriggermins()[1];
            BubakLocationTriggerMinZNUD.Value = CurrentBubaklocation.gettriggermins()[2];
            BubakLocationTriggerMaxXNUD.Value = CurrentBubaklocation.gettriggermaxs()[0];
            BubakLocationTriggerMaxYNUD.Value = CurrentBubaklocation.gettriggermaxs()[1];
            BubakLocationTriggerMaxZNUD.Value = CurrentBubaklocation.gettriggermaxs()[2];
            BukabLocationtriggerradiusNUD.Value = CurrentBubaklocation.triggerradius;
            BubakLocationtriggercylradiusNUD.Value = CurrentBubaklocation.triggercylradius;
            BubakLocationtriggercylheightNUD.Value = CurrentBubaklocation.triggercylheight;
            BubakLocationnotificationTB.Text = CurrentBubaklocation.notification;
            BubakLocationnotificationtimeNUD.Value = CurrentBubaklocation.notificationtime;
            BubakLocationtriggerdelayNUD.Value = CurrentBubaklocation.triggerdelay;
            BubakLocationspawnradiusNUD.Value = CurrentBubaklocation.spawnradius;
            BubakLocationbubaknumNUD.Value = CurrentBubaklocation.bubaknum;
            BubakLocationonlyfilluptobubaknumNUD.Value = CurrentBubaklocation.onlyfilluptobubaknum;
            BubakLocationitemrandomdmgCB.Checked = CurrentBubaklocation.itemrandomdmg == 1 ? true : false;


            BubakLocationSpawnerPosLB.DisplayMember = "DisplayName";
            BubakLocationSpawnerPosLB.ValueMember = "Value";
            BubakLocationSpawnerPosLB.DataSource = CurrentBubaklocation._spawnerpos;

            BubakLocationBubaciLB.DisplayMember = "DisplayName";
            BubakLocationBubaciLB.ValueMember = "Value";
            BubakLocationBubaciLB.DataSource = CurrentBubaklocation.bubaci;

            BubakLocationbubakinventoryLB.DisplayMember = "DisplayName";
            BubakLocationbubakinventoryLB.ValueMember = "Value";
            BubakLocationbubakinventoryLB.DataSource = CurrentBubaklocation.bubakinventory;

            SpawnerBubakuConfig.isDirty = CurrentBubaklocation.needtosetdirty;
            CurrentBubaklocation.needtosetdirty = false;
            if (SpawnerBubakuConfig.isDirty)
                Savefiles(false);
            useraction = true;

            pictureBox1.Invalidate();
        }
        private void BubakLocationTriggerPositionRotSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (BubakLocationTriggerPositionRotSpecifiedCB.Checked)
            {
                TrigerposRotationLabel.Visible = true;
                BubakLocationTriggerRotXNUD.Visible = true;
                BubakLocationTriggerRotYNUD.Visible = true;
                BubakLocationTriggerRotZNUD.Visible = true;
                CurrentBubaklocation._triggerpos.rotspecified = true;
                BubakLocationTriggerRotXNUD.Value = (decimal)CurrentBubaklocation._triggerpos.Rotation.X;
                BubakLocationTriggerRotYNUD.Value = (decimal)CurrentBubaklocation._triggerpos.Rotation.Y;
                BubakLocationTriggerRotZNUD.Value = (decimal)CurrentBubaklocation._triggerpos.Rotation.Z;

            }
            else
            {
                CurrentBubaklocation._triggerpos.rotspecified = false;
                TrigerposRotationLabel.Visible = false;
                BubakLocationTriggerRotXNUD.Visible = false;
                BubakLocationTriggerRotYNUD.Visible = false;
                BubakLocationTriggerRotZNUD.Visible = false;
            }
            SpawnerBubakuConfig.isDirty = true;
        }
        private void BubakLocationSpawnerPosLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BubakLocationSpawnerPosLB.SelectedItems.Count <= 0) return;
            currentspawnPosition = BubakLocationSpawnerPosLB.SelectedItem as Vec3PandR;
            useraction = false;
            BubakLocationSpawnPositionXNUD.Value = (decimal)currentspawnPosition.Position.X;
            BubakLocationSpawnPositionYNUD.Value = (decimal)currentspawnPosition.Position.Y;
            BubakLocationSpawnPositionZNUD.Value = (decimal)currentspawnPosition.Position.Z;
            if (checkBox1.Checked = currentspawnPosition.rotspecified)
            {
                BubakLocationSpawnRotationXNUD.Value = (decimal)currentspawnPosition.Rotation.X;
                BubakLocationSpawnRotationYNUD.Value = (decimal)currentspawnPosition.Rotation.Y;
                BubakLocationSpawnRotationZNUD.Value = (decimal)currentspawnPosition.Rotation.Z;
            }
            useraction = true;

        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                BubakLocationSpawnPositionRotaionLabel.Visible = true;
                BubakLocationSpawnRotationXNUD.Visible = true;
                BubakLocationSpawnRotationYNUD.Visible = true;
                BubakLocationSpawnRotationZNUD.Visible = true;
                currentspawnPosition.rotspecified = true;
                BubakLocationSpawnRotationXNUD.Value = (decimal)currentspawnPosition.Rotation.X;
                BubakLocationSpawnRotationYNUD.Value = (decimal)currentspawnPosition.Rotation.Y;
                BubakLocationSpawnRotationZNUD.Value = (decimal)currentspawnPosition.Rotation.Z;

            }
            else
            {
                currentspawnPosition.rotspecified = false;
                BubakLocationSpawnPositionRotaionLabel.Visible = false;
                BubakLocationSpawnRotationXNUD.Visible = false;
                BubakLocationSpawnRotationYNUD.Visible = false;
                BubakLocationSpawnRotationZNUD.Visible = false;
            }
            SpawnerBubakuConfig.isDirty = true;
            BubakLocationSpawnerPosLB.Invalidate();
        }

        private void darkButton6_Click(object sender, EventArgs e)
        {
            CurrentBubaklocation.spawnerpos.Add("0 0 0");
            BubakLocationSpawnerPosLB.Refresh();
            SpawnerBubakuConfig.isDirty = true;
        }
        private void darkButton12_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    DZE importfile = DZEHelpers.LoadFile(filePath);
                    bool ImportTrigger = false;
                    var result = MessageBox.Show("Would you like to import the trigger as well?", "Import options", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                        ImportTrigger = true;
                    bool importrtotation = false;
                    result = MessageBox.Show("Would you like to import rotations?", "Import options", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                        importrtotation = true;
                    result = MessageBox.Show("Would you like to clear existing Spawn Points??", "Import options", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if ((result == DialogResult.Cancel))
                    {
                        return;
                    }
                    else if (result == DialogResult.Yes)
                    {
                        CurrentBubaklocation._spawnerpos = new BindingList<Vec3PandR>();
                    }
                    CurrentBubaklocation.ImportDZE(importfile, ImportTrigger, importrtotation);
                    useraction = false;

                    BubakLocationTriggerPosXNUD.Value = (decimal)CurrentBubaklocation._triggerpos.Position.X;
                    BubakLocationTriggerPosYNUD.Value = (decimal)CurrentBubaklocation._triggerpos.Position.Y;
                    BubakLocationTriggerPosZNUD.Value = (decimal)CurrentBubaklocation._triggerpos.Position.Z;
                    BubakLocationSpawnerPosLB.DataSource = CurrentBubaklocation._spawnerpos;
                    SpawnerBubakuConfig.isDirty = true;
                    useraction = true;
                    BubakLocationTriggerPositionRotSpecifiedCB.Checked = importrtotation;
                }
            }
        }
        private void darkButton10_Click(object sender, EventArgs e)
        {
            if (BubakLocationSpawnerPosLB.SelectedItems.Count <= 0) return;
            List<string> removelist = new List<string>();
            int index = BubakLocationSpawnerPosLB.SelectedIndex;
            CurrentBubaklocation.spawnerpos.RemoveAt(index);
            SpawnerBubakuConfig.isDirty = true;
        }
        private void darkButton11_Click(object sender, EventArgs e)
        {
            DZE newdze = new DZE()
            {
                MapName = Path.GetFileNameWithoutExtension(currentproject.MapPath).Split('_')[0]
            };
            int m_Id = 0;
            string filename = "";
            var result = MessageBox.Show("Would yo ulike to export the trigger as well?", "Export options", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel)
            {
                return;
            }
            else if (result == DialogResult.Yes)
            {
                Editorobject Triggerobject = new Editorobject()
                {
                    Type = "GiftBox_Large_1",
                    DisplayName = "GiftBox_Large_1",
                    Position = CurrentBubaklocation._triggerpos.GetPositionFloatArray(),
                    Orientation = CurrentBubaklocation._triggerpos.GetRotationFloatArray(),
                    Scale = 1.0f,
                    Model = "",
                    Flags = 2147483647,
                    m_Id = m_Id
                };
                newdze.EditorObjects.Add(Triggerobject);
                m_Id++;
            }
            foreach (Vec3PandR vec3pandr in CurrentBubaklocation._spawnerpos)
            {
                Editorobject SpawnObject = new Editorobject()
                {
                    Type = "GiftBox_Small_1",
                    DisplayName = "GiftBox_Small_1",
                    Position = vec3pandr.GetPositionFloatArray(),
                    Orientation = vec3pandr.GetRotationFloatArray(),
                    Scale = 1.0f,
                    Model = "",
                    Flags = 2147483647,
                    m_Id = m_Id
                };
                newdze.EditorObjects.Add(SpawnObject);
                m_Id++;
            }
            filename = "Bubaku Spawner Location - " + CurrentBubaklocation.name;
            newdze.CameraPosition = newdze.EditorObjects[0].Position;
            SaveFileDialog save = new SaveFileDialog();
            save.FileName = filename;
            if (save.ShowDialog() == DialogResult.OK)
            {
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(newdze, options);
                File.WriteAllText(save.FileName + ".dze", jsonString);
            }
        }
        private void darkButton5_Click(object sender, EventArgs e)
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
                    if (!CurrentBubaklocation.bubaci.Contains(l))
                    {
                        CurrentBubaklocation.bubaci.Add(l);
                        SpawnerBubakuConfig.isDirty = true;
                    }
                }
                BubakLocationBubaciLB.Refresh();
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton7_Click(object sender, EventArgs e)
        {
            if (BubakLocationBubaciLB.SelectedItems.Count < 1) return;
            List<string> removelist = new List<string>();
            foreach (var item in BubakLocationBubaciLB.SelectedItems)
            {
                removelist.Add(BubakLocationBubaciLB.GetItemText(item));
            }
            foreach (string bubac in removelist)
            {
                CurrentBubaklocation.bubaci.Remove(bubac);
            }
            SpawnerBubakuConfig.isDirty = true;
        }
        private void darkButton9_Click(object sender, EventArgs e)
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
                    if (!CurrentBubaklocation.bubakinventory.Contains(l))
                    {
                        CurrentBubaklocation.bubakinventory.Add(l);
                        SpawnerBubakuConfig.isDirty = true;
                    }
                }
                BubakLocationBubaciLB.Refresh();
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton8_Click(object sender, EventArgs e)
        {
            if (BubakLocationbubakinventoryLB.SelectedItems.Count < 1) return;
            List<string> removelist = new List<string>();
            foreach (var item in BubakLocationbubakinventoryLB.SelectedItems)
            {
                removelist.Add(BubakLocationbubakinventoryLB.GetItemText(item));
            }
            foreach (string item in removelist)
            {
                CurrentBubaklocation.bubakinventory.Remove(item);
            }
            SpawnerBubakuConfig.isDirty = true;
        }
        private void BubakLocationNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentBubaklocation.Setname(BubakLocationNameTB.Text);
            SpawnerBubakuConfig.isDirty = true;
            SpawnerBukakuLocationsLB.Invalidate();
        }
        private void BubakLocationSetWorkingHours_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentBubaklocation.setworkinghours(new int[] { (int)BubakLocationWorkingHoursStartNUD.Value, (int)BubakLocationWorkingHoursEndNUD.Value });
            SpawnerBubakuConfig.isDirty = true;
        }
        private void BubakLocationSetTrigger_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CurrentBubaklocation._triggerpos.rotspecified = BubakLocationTriggerPositionRotSpecifiedCB.Checked)
            {
                CurrentBubaklocation._triggerpos.Rotation = new Vec3((float)BubakLocationTriggerRotXNUD.Value, (float)BubakLocationTriggerRotYNUD.Value, (float)BubakLocationSpawnRotationZNUD.Value);
            }
            CurrentBubaklocation._triggerpos.Position = new Vec3((float)BubakLocationTriggerPosXNUD.Value, (float)BubakLocationTriggerPosYNUD.Value, (float)BubakLocationTriggerPosZNUD.Value);
            SpawnerBubakuConfig.isDirty = true;
        }
        private void BubakLocationSetTriggerMins_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentBubaklocation.setTriggermins(new decimal[] { BubakLocationTriggerMinXNUD.Value, BubakLocationTriggerMinYNUD.Value, BubakLocationTriggerMinZNUD.Value });
            SpawnerBubakuConfig.isDirty = true;
        }
        private void BubakLocationSetTriggerMaxs_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentBubaklocation.setTriggermaxs(new decimal[] { BubakLocationTriggerMaxXNUD.Value, BubakLocationTriggerMaxYNUD.Value, BubakLocationTriggerMaxZNUD.Value });
            SpawnerBubakuConfig.isDirty = true;
        }
        private void BukabLocationtriggerradiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentBubaklocation.triggercylradius = BukabLocationtriggerradiusNUD.Value;
            SpawnerBubakuConfig.isDirty = true;
        }
        private void BubakLocationtriggercylradiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentBubaklocation.triggercylradius = BubakLocationtriggercylradiusNUD.Value;
            SpawnerBubakuConfig.isDirty = true;
        }
        private void BubakLocationtriggercylheightNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentBubaklocation.triggercylheight = BubakLocationtriggercylheightNUD.Value;
            SpawnerBubakuConfig.isDirty = true;
        }
        private void BubakLocationnotificationTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentBubaklocation.notification = BubakLocationnotificationTB.Text;
            SpawnerBubakuConfig.isDirty = true;
        }
        private void BubakLocationnotificationtimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentBubaklocation.notificationtime = (int)BubakLocationnotificationtimeNUD.Value;
            SpawnerBubakuConfig.isDirty = true;
        }
        private void BubakLocationtriggerdelayNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentBubaklocation.triggerdelay = (int)BubakLocationtriggerdelayNUD.Value;
            SpawnerBubakuConfig.isDirty = true;
        }
        private void BubakLocationSetSpawnPosition_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (currentspawnPosition.rotspecified)
            {
                currentspawnPosition.Rotation = new Vec3((float)BubakLocationSpawnRotationXNUD.Value, (float)BubakLocationSpawnRotationYNUD.Value, (float)BubakLocationSpawnRotationZNUD.Value);
            }
            currentspawnPosition.Position = new Vec3((float)BubakLocationSpawnPositionXNUD.Value, (float)BubakLocationSpawnPositionYNUD.Value, (float)BubakLocationSpawnPositionZNUD.Value);
            SpawnerBubakuConfig.isDirty = true;
            BubakLocationSpawnerPosLB.Invalidate();
        }
        private void BubakLocationspawnradiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentBubaklocation.spawnradius = BubakLocationspawnradiusNUD.Value;
            SpawnerBubakuConfig.isDirty = true;
        }
        private void BubakLocationbubaknumNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentBubaklocation.bubaknum = (int)BubakLocationbubaknumNUD.Value;
            SpawnerBubakuConfig.isDirty = true;
        }
        private void BubakLocationonlyfilluptobubaknumNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentBubaklocation.onlyfilluptobubaknum = (int)BubakLocationonlyfilluptobubaknumNUD.Value;
            SpawnerBubakuConfig.isDirty = true;
        }
        private void BubakLocationitemrandomdmgCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentBubaklocation.itemrandomdmg = BubakLocationitemrandomdmgCB.Checked == true ? 1 : 0;
            SpawnerBubakuConfig.isDirty = true;
        }

        public int SpawnerBubakuScale = 1;
        private Point _mouseLastPosition;
        private Point _newscrollPosition;

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            SpawnerBubakuScale = trackBar1.Value;
            SetSpawnerBubakuScale();
        }
        private void SetSpawnerBubakuScale()
        {
            float scalevalue = SpawnerBubakuScale * 0.05f;
            float mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            pictureBox1.Size = new Size(newsize, newsize);
        }
        private void DrawSpawnerBubakuSpawner(object sender, PaintEventArgs e)
        {
            if (checkBox9.Checked == true)
            {
                foreach (Bubaklocation spc in SpawnerBubakuConfig.BubakLocations)
                {
                    float scalevalue = SpawnerBubakuScale * 0.05f;
                    int centerX = (int)(Math.Round(spc._triggerpos.Position.X) * scalevalue);
                    int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(spc._triggerpos.Position.Z, 0) * scalevalue);
                    int eventradius = (int)((float)spc.triggerradius * scalevalue);
                    Point center = new Point(centerX, centerY);
                    Pen pen = new Pen(Color.Red, 4);
                    if (spc == CurrentBubaklocation)
                        pen = new Pen(Color.Green, 4);
                    getCircleDynamicAI(e.Graphics, pen, center, eventradius, "\n" + spc.name);
                    foreach (Vec3PandR v3pandr in spc._spawnerpos)
                    {
                        scalevalue = SpawnerBubakuScale * 0.05f;
                        centerX = (int)(Math.Round(v3pandr.Position.X) * scalevalue);
                        centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(v3pandr.Position.Z, 0) * scalevalue);
                        eventradius = (int)((float)spc.spawnradius * scalevalue);
                        center = new Point(centerX, centerY);
                        pen = new Pen(Color.Yellow, 4);
                        getCircleDynamicAI(e.Graphics, pen, center, eventradius, "");
                    }
                }
            }
            else
            {
                if (CurrentBubaklocation == null) return;
                float scalevalue = SpawnerBubakuScale * 0.05f;
                int centerX = (int)(Math.Round((float)CurrentBubaklocation._triggerpos.Position.X) * scalevalue);
                int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round((float)CurrentBubaklocation._triggerpos.Position.Z, 0) * scalevalue);
                int eventradius = (int)((float)CurrentBubaklocation.triggerradius * scalevalue);
                Point center = new Point(centerX, centerY);
                Pen pen = new Pen(Color.Green, 4);
                getCircleDynamicAI(e.Graphics, pen, center, eventradius, "\n" + CurrentBubaklocation.name);
                foreach (Vec3PandR v3pandr in CurrentBubaklocation._spawnerpos)
                {
                    scalevalue = SpawnerBubakuScale * 0.05f;
                    centerX = (int)(Math.Round(v3pandr.Position.X) * scalevalue);
                    centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(v3pandr.Position.Z, 0) * scalevalue);
                    eventradius = (int)((float)CurrentBubaklocation.spawnradius * scalevalue);
                    center = new Point(centerX, centerY);
                    pen = new Pen(Color.Yellow, 4);
                    getCircleDynamicAI(e.Graphics, pen, center, eventradius, "");
                }
            }
        }
        private void getCircleDynamicAI(Graphics drawingArea, Pen penToUse, Point center, int radius, string c)
        {
            Rectangle rect = new Rectangle(center.X - 1, center.Y - 1, 2, 2);
            drawingArea.DrawEllipse(penToUse, rect);
            Rectangle rect2 = new Rectangle(center.X - radius, center.Y - radius, radius * 2, radius * 2);
            drawingArea.DrawEllipse(penToUse, rect2);
            Rectangle rect3 = new Rectangle(center.X - radius, center.Y - (radius + 30), 200, 40);
            drawingArea.DrawString(c, new Font("Tahoma", 9), Brushes.White, rect3);
        }
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            if (pictureBox1.Focused == false)
            {
                pictureBox1.Focus();
                panelEx1.AutoScrollPosition = _newscrollPosition;
                pictureBox1.Invalidate();
            }
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point changePoint = new Point(e.Location.X - _mouseLastPosition.X, e.Location.Y - _mouseLastPosition.Y);
                _newscrollPosition = new Point(-panelEx1.AutoScrollPosition.X - changePoint.X, -panelEx1.AutoScrollPosition.Y - changePoint.Y);
                if (_newscrollPosition.X <= 0)
                    _newscrollPosition.X = 0;
                if (_newscrollPosition.Y <= 0)
                    _newscrollPosition.Y = 0;
                panelEx1.AutoScrollPosition = _newscrollPosition;
                pictureBox1.Invalidate();
            }
            decimal scalevalue = SpawnerBubakuScale * (decimal)0.05;
            decimal mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            label2.Text = Decimal.Round((decimal)(e.X / scalevalue), 4) + "," + Decimal.Round((decimal)((newsize - e.Y) / scalevalue), 4);
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Cursor.Current = Cursors.SizeAll;
                _mouseLastPosition = e.Location;
            }
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
            Point oldscrollpos = panelEx1.AutoScrollPosition;
            int tbv = trackBar1.Value;
            int newval = tbv + 1;
            if (newval >= 20)
                newval = 20;
            trackBar1.Value = newval;
            SpawnerBubakuScale = trackBar1.Value;
            SetSpawnerBubakuScale();
            if (pictureBox1.Height > panelEx1.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox1.Height * newy);
                _newscrollPosition.Y = y * -1;
                panelEx1.AutoScrollPosition = _newscrollPosition;
            }
            if (pictureBox1.Width > panelEx1.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox1.Width * newy);
                _newscrollPosition.X = x * -1;
                panelEx1.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox1.Invalidate();
        }
        private void pictureBox1_ZoomOut()
        {
            int oldpictureboxhieght = pictureBox1.Height;
            int oldpitureboxwidht = pictureBox1.Width;
            Point oldscrollpos = panelEx1.AutoScrollPosition;
            int tbv = trackBar1.Value;
            int newval = tbv - 1;
            if (newval <= 1)
                newval = 1;
            trackBar1.Value = newval;
            SpawnerBubakuScale = trackBar1.Value;
            SetSpawnerBubakuScale();
            if (pictureBox1.Height > panelEx1.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox1.Height * newy);
                _newscrollPosition.Y = y * -1;
                panelEx1.AutoScrollPosition = _newscrollPosition;
            }
            if (pictureBox1.Width > panelEx1.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox1.Width * newy);
                _newscrollPosition.X = x * -1;
                panelEx1.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox1.Invalidate();
        }
        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }
    }
}
