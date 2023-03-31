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
    public partial class HelicrashMissionsManager : DarkForm
    {
        public Project currentproject { get; set; }
        public TypesFile vanillatypes;
        public List<TypesFile> ModTypes;

        public string HelicrashMissionsPath { get; private set; }
        public Helicrash Helicrash { get; private set; }

        public string Projectname;
        private bool _useraction = false;
        public string LootPoolConfigPath { get; private set; }
        public LootPool LootPool { get; private set; }

        private Crashpoint currentCrashpoint;

        public bool useraction
        {
            get { return _useraction; }
            set
            {
                _useraction = value;
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
                    toolStripButton7.Checked = false;
                    break;
                case 1:
                    toolStripButton8.Checked = false;
                    toolStripButton7.Checked = false;
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
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
            if (tabControl1.SelectedIndex == 2)
                toolStripButton7.Checked = true;
        }
 
        public HelicrashMissionsManager()
        {
            InitializeComponent();
        }
        private void HelicrashMissionsManager_Load(object sender, EventArgs e)
        {
            tabControl1.ItemSize = new Size(0, 1);
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Projectname = currentproject.ProjectName;
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            LootPoolConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\LootPool\\LootPoolConfig.json";
            LootPool = JsonSerializer.Deserialize<LootPool>(File.ReadAllText(LootPoolConfigPath));

            HelicrashMissionsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\HeliCrashMissions\\Helicrash.json";
            Helicrash = JsonSerializer.Deserialize<Helicrash>(File.ReadAllText(HelicrashMissionsPath));
            Helicrash.isDirty = false;
            Helicrash.FullFilename = HelicrashMissionsPath;

            LoadHeliCrash();

            pictureBox1.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Map Size is 15360 x 15360, 0,0 bottom left, middle 7680 x 7680
            pictureBox1.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox1.Paint += new PaintEventHandler(DrawAll);
            trackBar2.Value = 1;
            SetHeliCrashScale();
        }
        private void trackBar2_MouseUp(object sender, MouseEventArgs e)
        {
            HeliCrashMapscale = trackBar2.Value;
            SetHeliCrashScale();

        }
        public int HeliCrashMapscale = 1;
        private void SetHeliCrashScale()
        {
            float scalevalue = HeliCrashMapscale * 0.05f;
            float mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            pictureBox1.Size = new Size(newsize, newsize);
        }
        private void DrawAll(object sender, PaintEventArgs e)
        {
            float scalevalue = HeliCrashMapscale * 0.05f;
            foreach (Crashpoint zones in Helicrash.CrashPoints)
            {
                int centerX = (int)(Math.Round((float)zones.x, 0) * scalevalue);
                int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round((float)zones.y, 0) * scalevalue);

                int radius = (int)((float)zones.Radius * scalevalue);
                Point center = new Point(centerX, centerY);
                Pen pen = new Pen(Color.Red)
                {
                    Width = 4
                };
                if (currentCrashpoint == zones)
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

        private void LoadHeliCrash()
        {
            useraction = false;

            adminlogCB.Checked = Helicrash.admin_log == 1 ? true : false;
            HeliCrashEnabledCB.Checked = Helicrash.HeliCrashEnabled == 1 ? true : false;
            HelicrashSpawnTimeNUD.Value = Helicrash.HelicrashSpawnTime;
            HelicrashDespawnTimeNUD.Value = Helicrash.HelicrashDespawnTime;

            AnimalMaxNUD.Value = Helicrash.AnimalSpawnArray[0].amount_maximum;
            AnimalMinNUD.Value = Helicrash.AnimalSpawnArray[0].amount_minimum;
            AnimalRadiusNUD.Value = Helicrash.AnimalSpawnArray[0].radius;

            ZombieMaxNUD.Value = Helicrash.ZombieSpawnArray[0].amount_maximum;
            ZombieMinNUD.Value = Helicrash.ZombieSpawnArray[0].amount_minimum;
            ZombieRadiusNUD.Value = Helicrash.ZombieSpawnArray[0].radius;

            start_heightNUD.Value = Helicrash.HelicopterArray[0].start_height;
            minimum_heightNUD.Value = Helicrash.HelicopterArray[0].minimum_height;
            speedNUD.Value = Helicrash.HelicopterArray[0].speed;
            minimum_speedNUD.Value = Helicrash.HelicopterArray[0].minimum_speed;

            zombie_nameLB.DisplayMember = "DisplayName";
            zombie_nameLB.ValueMember = "Value";
            zombie_nameLB.DataSource = Helicrash.ZombieSpawnArray[0].zombie_name;

            CrashpointLB.DisplayMember = "DisplayName";
            CrashpointLB.ValueMember = "Value";
            CrashpointLB.DataSource = Helicrash.CrashPoints;

            animal_nameLB.DisplayMember = "DisplayName";
            animal_nameLB.ValueMember = "Value";
            animal_nameLB.DataSource = Helicrash.AnimalSpawnArray[0].animal_name;

            Loot_HelicrashLB.DisplayMember = "DisplayName";
            Loot_HelicrashLB.ValueMember = "Value";
            Loot_HelicrashLB.DataSource = Helicrash.LootTables;


            useraction = true;
        }
        private void WeaponLootTablesLB_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void CrashpointLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(CrashpointLB.SelectedItems.Count == 0) { return; }
            currentCrashpoint = CrashpointLB.SelectedItem as Crashpoint;
            useraction = false;
            SetupcrashPoint();
            pictureBox1.Invalidate();
            useraction = true;
        }
        private void SetupcrashPoint()
        {
            CrashPointXNUD.Value = currentCrashpoint.x;
            CrashpointYNUD.Value = currentCrashpoint.y;
            CrashPointradiusNUD.Value = currentCrashpoint.Radius;
            Crash_MessageTB.Text = currentCrashpoint.Crash_Message;
        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            SaveHeliCrashMissions();
        }

        private void SaveHeliCrashMissions()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (Helicrash.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(Helicrash.FullFilename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(Helicrash.FullFilename) + "\\Backup\\" + SaveTime);
                    File.Copy(Helicrash.FullFilename, Path.GetDirectoryName(Helicrash.FullFilename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(Helicrash.FullFilename) + ".bak", true);
                }
                Helicrash.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(Helicrash, options);
                File.WriteAllText(Helicrash.FullFilename, jsonString);
                midifiedfiles.Add(Path.GetFileName(Path.GetFileName(HelicrashMissionsPath)));
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
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\HeliCrashMissions");
        }

        private void adminlogCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helicrash.admin_log = adminlogCB.Checked == true ? 1 : 0;
            Helicrash.isDirty = true;
        }
        private void HeliCrashEnabledCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helicrash.HeliCrashEnabled = HeliCrashEnabledCB.Checked == true ? 1 : 0;
            Helicrash.isDirty = true;
        }
        private void HelicrashSpawnTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helicrash.HelicrashSpawnTime = (int)HelicrashSpawnTimeNUD.Value;
            Helicrash.isDirty = true;
        }
        private void HelicrashDespawnTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helicrash.HelicrashDespawnTime = (int)HelicrashDespawnTimeNUD.Value;
            Helicrash.isDirty = true;
        }
        private void CrashPointXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentCrashpoint.x = CrashPointXNUD.Value;
            Helicrash.isDirty = true;
        }
        private void CrashpointYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentCrashpoint.y = CrashpointYNUD.Value;
            Helicrash.isDirty = true;
        }
        private void CrashPointradiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentCrashpoint.Radius = CrashPointradiusNUD.Value;
            pictureBox1.Invalidate();
            Helicrash.isDirty = true;
        }
        private void Crash_MessageTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentCrashpoint.Crash_Message = Crash_MessageTB.Text;
            Helicrash.isDirty = true;
        }
        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            var mouseEventArgs = e as MouseEventArgs;
            if (mouseEventArgs != null)
            {
                float scalevalue = HeliCrashMapscale * 0.05f;
                float mapsize = currentproject.MapSize;
                int newsize = (int)(mapsize * scalevalue);
                if (currentCrashpoint == null) { return; }
                Cursor.Current = Cursors.WaitCursor;
                CrashPointXNUD.Value = (decimal)(mouseEventArgs.X / scalevalue);
                CrashpointYNUD.Value = (decimal)((newsize - mouseEventArgs.Y) / scalevalue);
                Cursor.Current = Cursors.Default;
                Helicrash.isDirty = true;
                pictureBox1.Invalidate();
            }
        }

        private void start_heightNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helicrash.HelicopterArray[0].start_height = (int)start_heightNUD.Value;
            Helicrash.isDirty = true;
        }
        private void minimum_heightNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helicrash.HelicopterArray[0].start_height = (int)start_heightNUD.Value;
            Helicrash.isDirty = true;
        }
        private void speedNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helicrash.HelicopterArray[0].speed = (int)speedNUD.Value;
            Helicrash.isDirty = true;
        }
        private void minimum_speedNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helicrash.HelicopterArray[0].minimum_speed = (int)minimum_speedNUD.Value;
            Helicrash.isDirty = true;
        }
        private void AnimalMaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helicrash.AnimalSpawnArray[0].amount_maximum = (int)AnimalMaxNUD.Value;
            Helicrash.isDirty = true;
        }
        private void AnimalMinNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helicrash.AnimalSpawnArray[0].amount_minimum = (int)AnimalMinNUD.Value;
            Helicrash.isDirty = true;
        }
        private void AnimalRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helicrash.AnimalSpawnArray[0].radius = (int)AnimalRadiusNUD.Value;
            Helicrash.isDirty = true;
        }
        private void darkButton18_Click(object sender, EventArgs e)
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
                    if (!Helicrash.AnimalSpawnArray[0].animal_name.Contains(l))
                    {
                        Helicrash.AnimalSpawnArray[0].animal_name.Add(l);
                        Helicrash.isDirty = true;
                    }
                }
            }
        }
        private void darkButton17_Click(object sender, EventArgs e)
        {
            Helicrash.AnimalSpawnArray[0].animal_name.Remove(animal_nameLB.GetItemText(animal_nameLB.SelectedItem));
            Helicrash.isDirty = true;
        }
        private void ZombieMaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helicrash.ZombieSpawnArray[0].amount_maximum = (int)ZombieMaxNUD.Value;
            Helicrash.isDirty = true;
        }
        private void ZombieMinNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helicrash.ZombieSpawnArray[0].amount_minimum = (int)ZombieMinNUD.Value;
            Helicrash.isDirty = true;
        }
        private void ZombieRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            Helicrash.ZombieSpawnArray[0].radius = (int)ZombieRadiusNUD.Value;
            Helicrash.isDirty = true;
        }
        private void darkButton2_Click(object sender, EventArgs e)
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
                    if (!Helicrash.ZombieSpawnArray[0].zombie_name.Contains(l))
                    {
                        Helicrash.ZombieSpawnArray[0].zombie_name.Add(l);
                        Helicrash.isDirty = true;
                    }
                }
            }
        }
        private void darkButton3_Click(object sender, EventArgs e)
        {
            Helicrash.ZombieSpawnArray[0].zombie_name.Remove(zombie_nameLB.GetItemText(zombie_nameLB.SelectedItem));
            Helicrash.isDirty = true;
        }
        private void darkButton21_Click(object sender, EventArgs e)
        {
            Helicrash.LootTables.Remove(Loot_HelicrashLB.GetItemText(Loot_HelicrashLB.SelectedItem));
            Helicrash.isDirty = true;
        }

        private void darkButton5_Click(object sender, EventArgs e)
        {
            Crashpoint newcrashpoint = new Crashpoint()
            {
                Crash_Message = "New Crash",
                x = currentproject.MapSize / 2,
                y = currentproject.MapSize / 2,
                Radius = 100
            };
            Helicrash.CrashPoints.Add(newcrashpoint);
            pictureBox1.Invalidate();
            Helicrash.isDirty = true;
        }

        private void darkButton6_Click(object sender, EventArgs e)
        {
            Helicrash.CrashPoints.Remove(currentCrashpoint);
            pictureBox1.Invalidate();
            Helicrash.isDirty = true;
        }

        private void HelicrashMissionsManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool needtosave = false;
            if (Helicrash.isDirty)
            {
                needtosave = true;
            }
            if (needtosave)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SaveHeliCrashMissions();
                }
            }
        }

        private void darkButton10_Click_1(object sender, EventArgs e)
        {
            AddfromPredefinedWeapons form = new AddfromPredefinedWeapons
            {
                Rhlprewardtable = LootPool.RHLPRewardTables,
                titellabel = "Add Items from Loot list",
                isLootList = false,
                isRHTableList = false,
                isRewardTable = true,
                ispredefinedweapon = false,
                isRHPredefinedWeapon = false,
                isLootchest = false,
                isLootBoxList = false
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> predefweapon = form.WeaponList;
                foreach (string weapon in predefweapon)
                {
                    Helicrash.LootTables.Add(weapon);
                    Helicrash.isDirty = true;
                }
            }
        }


    }
}
