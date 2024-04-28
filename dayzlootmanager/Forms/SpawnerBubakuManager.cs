using DarkUI.Forms;
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
            SpawnerBubakuConfig.isDirty = false;
            SpawnerBubakuConfig.Filename = SpawnerBubakuConfigPath;

            LogLevelNUD.Value = (int)SpawnerBubakuConfig.getLogLevel();

            SpawnerBukakuLocationsLB.DisplayMember = "DisplayName";
            SpawnerBukakuLocationsLB.ValueMember = "Value";
            SpawnerBukakuLocationsLB.DataSource = SpawnerBubakuConfig.BubakLocations;

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
            if(SpawnerBukakuLocationsLB.Items.Count <= 0)
                BubakLocationGB.Visible = false;
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
            DZE newdze = new DZE()
            {
                MapName = Path.GetFileNameWithoutExtension(currentproject.MapPath).Split('_')[0]
            };
            int m_Id = 0;
            BukakuPosRot triggerposrot = CurrentBubaklocation.gettriggerposition();
            Editorobject Triggerobject = new Editorobject()
            {
                Type = "GiftBox_Large_1",
                DisplayName = "GiftBox_Large_1",
                Position = new float[] { Convert.ToSingle(triggerposrot.Position[0]), Convert.ToSingle(triggerposrot.Position[1]), Convert.ToSingle(triggerposrot.Position[2]) },
                Orientation = new float[] { Convert.ToSingle(triggerposrot.Rotation[0]), Convert.ToSingle(triggerposrot.Rotation[1]), Convert.ToSingle(triggerposrot.Rotation[2]) },
                Scale = 1.0f,
                Flags = 2147483647,
                m_Id = m_Id
            };
            newdze.EditorObjects.Add(Triggerobject);
            m_Id++;
            for (int i = 0; i < CurrentBubaklocation.spawnerpos.Count; i++)
            {
                BukakuPosRot spawnposrot = CurrentBubaklocation.getPosRot(i);
                Editorobject SpawnObject = new Editorobject()
                {
                    Type = "GiftBox_Small_1",
                    DisplayName = "GiftBox_Small_1",
                    Position = new float[] { Convert.ToSingle(spawnposrot.Position[0]), Convert.ToSingle(spawnposrot.Position[1]), Convert.ToSingle(spawnposrot.Position[2]) },
                    Orientation = new float[] { Convert.ToSingle(spawnposrot.Rotation[0]), Convert.ToSingle(spawnposrot.Rotation[1]), Convert.ToSingle(spawnposrot.Rotation[2]) },
                    Scale = 1.0f,
                    Flags = 2147483647,
                    m_Id = m_Id
                };
                newdze.EditorObjects.Add(SpawnObject);
                m_Id++;
            }
            newdze.CameraPosition = newdze.EditorObjects[0].Position;
            SaveFileDialog save = new SaveFileDialog();
            save.FileName = CurrentBubaklocation.name;
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
            BukakuPosRot posrot = CurrentBubaklocation.gettriggerposition();
            BubakLocationTriggerPosXNUD.Value = posrot.Position[0];
            BubakLocationTriggerPosYNUD.Value = posrot.Position[1];
            BubakLocationTriggerPosZNUD.Value = posrot.Position[2];
            if (BubakLocationTriggerPositionRotSpecifiedCB.Checked = posrot.RotationSpecifioed)
            {
                BubakLocationTriggerRotXNUD.Value = posrot.Rotation[0];
                BubakLocationTriggerRotYNUD.Value = posrot.Rotation[1];
                BubakLocationTriggerRotZNUD.Value = posrot.Rotation[2];
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
            BubakLocationSpawnerPosLB.DataSource = CurrentBubaklocation.spawnerpos;

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
        }
        private void BubakLocationTriggerPositionRotSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            BukakuPosRot posrot = CurrentBubaklocation.gettriggerposition();
            if (BubakLocationTriggerPositionRotSpecifiedCB.Checked)
            {
                TrigerposRotationLabel.Visible = true;
                BubakLocationTriggerRotXNUD.Visible = true;
                BubakLocationTriggerRotYNUD.Visible = true;
                BubakLocationTriggerRotZNUD.Visible = true;
                posrot.RotationSpecifioed = true;
                BubakLocationTriggerRotXNUD.Value = posrot.Rotation[0];
                BubakLocationTriggerRotYNUD.Value = posrot.Rotation[1];
                BubakLocationTriggerRotZNUD.Value = posrot.Rotation[2];
                   
            }
            else
            {
                posrot.RotationSpecifioed = false;
                TrigerposRotationLabel.Visible = false;
                BubakLocationTriggerRotXNUD.Visible = false;
                BubakLocationTriggerRotYNUD.Visible = false;
                BubakLocationTriggerRotZNUD.Visible = false;
            }
            CurrentBubaklocation.setTriggerposition(posrot);
            SpawnerBubakuConfig.isDirty = true;
        }
        private void BubakLocationSpawnerPosLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BubakLocationSpawnerPosLB.SelectedItems.Count <= 0) return;
            BukakuPosRot currentSpawnPosRot = CurrentBubaklocation.getPosRot(BubakLocationSpawnerPosLB.SelectedIndex);
            useraction = false;
            BubakLocationSpawnPositionXNUD.Value = currentSpawnPosRot.Position[0];
            BubakLocationSpawnPositionYNUD.Value = currentSpawnPosRot.Position[1];
            BubakLocationSpawnPositionZNUD.Value = currentSpawnPosRot.Position[2];
            if (checkBox1.Checked = currentSpawnPosRot.RotationSpecifioed)
            {
                BubakLocationSpawnRotationXNUD.Value = currentSpawnPosRot.Rotation[0];
                BubakLocationSpawnRotationYNUD.Value = currentSpawnPosRot.Rotation[1];
                BubakLocationSpawnRotationZNUD.Value = currentSpawnPosRot.Rotation[2];
            }
            useraction = true;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            BukakuPosRot posrot = CurrentBubaklocation.getPosRot(BubakLocationSpawnerPosLB.SelectedIndex);
            if (checkBox1.Checked)
            {
                BubakLocationSpawnPositionRotaionLabel.Visible = true;
                BubakLocationSpawnRotationXNUD.Visible = true;
                BubakLocationSpawnRotationYNUD.Visible = true;
                BubakLocationSpawnRotationZNUD.Visible = true;
                posrot.RotationSpecifioed = true;
                BubakLocationSpawnRotationXNUD.Value = posrot.Rotation[0];
                BubakLocationSpawnRotationYNUD.Value = posrot.Rotation[1];
                BubakLocationSpawnRotationZNUD.Value = posrot.Rotation[2];

            }
            else
            {
                posrot.RotationSpecifioed = false;
                BubakLocationSpawnPositionRotaionLabel.Visible = false;
                BubakLocationSpawnRotationXNUD.Visible = false;
                BubakLocationSpawnRotationYNUD.Visible = false;
                BubakLocationSpawnRotationZNUD.Visible = false;
            }
            CurrentBubaklocation.setPosRot(BubakLocationSpawnerPosLB.SelectedIndex, posrot);
            SpawnerBubakuConfig.isDirty = true;
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
                    if(result == DialogResult.Yes)
                        ImportTrigger = true;
                    result = MessageBox.Show("Would you like to clear existing Spawn Points??", "Import options", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if ((result == DialogResult.Cancel))
                    {
                        return;
                    }
                    else if (result == DialogResult.Yes)
                    {
                        CurrentBubaklocation.spawnerpos = new BindingList<string>();
                    }
                    CurrentBubaklocation.ImportDZE(importfile, ImportTrigger);
                    if(ImportTrigger)
                    {
                        BukakuPosRot posrot = CurrentBubaklocation.gettriggerposition();
                        BubakLocationTriggerPosXNUD.Value = posrot.Position[0];
                        BubakLocationTriggerPosYNUD.Value = posrot.Position[1];
                        BubakLocationTriggerPosZNUD.Value = posrot.Position[2];
                        if (BubakLocationTriggerPositionRotSpecifiedCB.Checked = posrot.RotationSpecifioed)
                        {
                            BubakLocationTriggerRotXNUD.Value = posrot.Rotation[0];
                            BubakLocationTriggerRotYNUD.Value = posrot.Rotation[1];
                            BubakLocationTriggerRotZNUD.Value = posrot.Rotation[2];
                        }
                    }
                    BubakLocationSpawnerPosLB.DataSource = CurrentBubaklocation.spawnerpos;
                    SpawnerBubakuConfig.isDirty = true;
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
            var result = MessageBox.Show("Would yo ulike to export the trigger as well?", "Export options", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if(result == DialogResult.Cancel)
            {
                return;
            }
            else if (result == DialogResult.Yes)
            {
                BukakuPosRot triggerposrot = CurrentBubaklocation.gettriggerposition();
                Editorobject Triggerobject = new Editorobject()
                {
                    Type = "GiftBox_Large_1",
                    DisplayName = "GiftBox_Large_1",
                    Position = new float[] { Convert.ToSingle(triggerposrot.Position[0]), Convert.ToSingle(triggerposrot.Position[1]), Convert.ToSingle(triggerposrot.Position[2]) },
                    Orientation = new float[] { Convert.ToSingle(triggerposrot.Rotation[0]), Convert.ToSingle(triggerposrot.Rotation[1]), Convert.ToSingle(triggerposrot.Rotation[2]) },
                    Scale = 1.0f,
                    Flags = 2147483647,
                    m_Id = m_Id
                };
                newdze.EditorObjects.Add(Triggerobject);
                m_Id++;
            }
            for (int i = 0; i < CurrentBubaklocation.spawnerpos.Count; i++)
            {
                BukakuPosRot spawnposrot = CurrentBubaklocation.getPosRot(i);
                Editorobject SpawnObject = new Editorobject()
                {
                    Type = "GiftBox_Small_1",
                    DisplayName = "GiftBox_Small_1",
                    Position = new float[] { Convert.ToSingle(spawnposrot.Position[0]), Convert.ToSingle(spawnposrot.Position[1]), Convert.ToSingle(spawnposrot.Position[2]) },
                    Orientation = new float[] { Convert.ToSingle(spawnposrot.Rotation[0]), Convert.ToSingle(spawnposrot.Rotation[1]), Convert.ToSingle(spawnposrot.Rotation[2]) },
                    Scale = 1.0f,
                    Flags = 2147483647,
                    m_Id = m_Id
                };
                newdze.EditorObjects.Add(SpawnObject);
                m_Id++;
            }
            newdze.CameraPosition = newdze.EditorObjects[0].Position;
            SaveFileDialog save = new SaveFileDialog();
            save.FileName = CurrentBubaklocation.name;
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
            foreach(string bubac in removelist)
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
            BukakuPosRot posrot = new BukakuPosRot();
            if(posrot.RotationSpecifioed = BubakLocationTriggerPositionRotSpecifiedCB.Checked)
            {
                posrot.Rotation = new decimal[] { BubakLocationTriggerRotXNUD.Value, BubakLocationTriggerRotYNUD.Value, BubakLocationSpawnRotationZNUD.Value };
            }
            posrot.Position = new decimal[] { BubakLocationTriggerPosXNUD.Value, BubakLocationTriggerPosYNUD.Value, BubakLocationTriggerPosZNUD.Value };
            CurrentBubaklocation.setTriggerposition(posrot);
            SpawnerBubakuConfig.isDirty = true;
        }
        private void BubakLocationSetTriggerMins_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentBubaklocation.setTriggermins(new decimal[] {BubakLocationTriggerMinXNUD.Value, BubakLocationTriggerMinYNUD.Value, BubakLocationTriggerMinZNUD.Value });
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
            BukakuPosRot posrot = new BukakuPosRot();
            if (posrot.RotationSpecifioed = checkBox1.Checked)
            {
                posrot.Rotation = new decimal[] { BubakLocationSpawnRotationXNUD.Value, BubakLocationSpawnRotationYNUD.Value, BubakLocationSpawnRotationZNUD.Value };
            }
            posrot.Position = new decimal[] { BubakLocationSpawnPositionXNUD.Value, BubakLocationSpawnPositionYNUD.Value, BubakLocationSpawnPositionZNUD.Value };
            CurrentBubaklocation.setPosRot(BubakLocationSpawnerPosLB.SelectedIndex, posrot);
            SpawnerBubakuConfig.isDirty = true;
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

    }
}
