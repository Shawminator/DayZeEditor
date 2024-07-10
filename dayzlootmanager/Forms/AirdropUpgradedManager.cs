using Cyotek.Windows.Forms;
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
    public partial class AirdropUpgradedManager : DarkForm
    {
        public Project currentproject { get; set; }
        public TypesFile vanillatypes;
        public List<TypesFile> ModTypes;

        public string AirdropUpgradedSettingsPath { get; private set; }
        public AirdropUpgraded AirdropUpgradedSettings { get; private set; }
        public string AirdropUpgradedSafeZonePath { get; private set; }
        public AirdropUpgradedSafeZones AirdropUpgradedSafeZone { get; private set; }

        public string Projectname;
        private bool _useraction = false;

        private Dropzone currentdropZone;
        private Droptype currentDropType;

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
                    toolStripButton4.Checked = false;
                    toolStripButton5.Checked = false;
                    break;
                case 1:
                    toolStripButton8.Checked = false;
                    toolStripButton1.Checked = false;
                    toolStripButton4.Checked = false;
                    toolStripButton5.Checked = false;
                    break;
                case 2:
                    toolStripButton3.Checked = false;
                    toolStripButton8.Checked = false;
                    toolStripButton4.Checked = false;
                    toolStripButton5.Checked = false;
                    break;
                case 3:
                    toolStripButton3.Checked = false;
                    toolStripButton8.Checked = false;
                    toolStripButton1.Checked = false;
                    toolStripButton5.Checked = false;
                    break;
                case 4:
                    toolStripButton3.Checked = false;
                    toolStripButton8.Checked = false;
                    toolStripButton1.Checked = false;
                    toolStripButton4.Checked = false;
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
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
            if (tabControl1.SelectedIndex == 3)
                toolStripButton4.Checked = true;
        }
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 4;
            if (tabControl1.SelectedIndex == 4)
                toolStripButton5.Checked = true;
        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            SaveAidropUpgraded();
        }
        private void SaveAidropUpgraded()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");

            if (AirdropUpgradedSettings.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(AirdropUpgradedSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(AirdropUpgradedSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(AirdropUpgradedSettings.Filename, Path.GetDirectoryName(AirdropUpgradedSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(AirdropUpgradedSettings.Filename) + ".bak", true);
                }
                AirdropUpgradedSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(AirdropUpgradedSettings, options);
                File.WriteAllText(AirdropUpgradedSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(Path.GetFileName(AirdropUpgradedSettings.Filename)));
            }

            if (AirdropUpgradedSafeZone.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(AirdropUpgradedSafeZone.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(AirdropUpgradedSafeZone.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(AirdropUpgradedSafeZone.Filename, Path.GetDirectoryName(AirdropUpgradedSafeZone.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(AirdropUpgradedSafeZone.Filename) + ".bak", true);
                }
                AirdropUpgradedSafeZone.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(AirdropUpgradedSafeZone, options);
                File.WriteAllText(AirdropUpgradedSafeZone.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(Path.GetFileName(AirdropUpgradedSafeZone.Filename)));
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
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Airdrop");
        }
        private void AirdropUpgradedManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool needtosave = false;
            if (AirdropUpgradedSafeZone.isDirty)
            {
                needtosave = true;
            }
            if (AirdropUpgradedSettings.isDirty)
            {
                needtosave = true;
            }
            if (needtosave)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SaveAidropUpgraded();
                }
            }
        }

        public AirdropUpgradedManager()
        {
            InitializeComponent();
        }
        private void AirdropUpgradedManager_Load(object sender, EventArgs e)
        {
            tabControl1.ItemSize = new Size(0, 1);
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Projectname = currentproject.ProjectName;
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            AirdropUpgradedSettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Airdrop\\AirdropSettings.json";
            AirdropUpgradedSettings = JsonSerializer.Deserialize<AirdropUpgraded>(File.ReadAllText(AirdropUpgradedSettingsPath));
            AirdropUpgradedSettings.isDirty = false;
            AirdropUpgradedSettings.Filename = AirdropUpgradedSettingsPath;

            AirdropUpgradedSafeZonePath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Airdrop\\AirdropSafezones.json";
            AirdropUpgradedSafeZone = JsonSerializer.Deserialize<AirdropUpgradedSafeZones>(File.ReadAllText(AirdropUpgradedSafeZonePath));
            AirdropUpgradedSafeZone.isDirty = false;
            AirdropUpgradedSafeZone.Filename = AirdropUpgradedSafeZonePath;


            LoadAirdropUpgraded();

            pictureBox1.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Map Size is 15360 x 15360, 0,0 bottom left, middle 7680 x 7680
            pictureBox1.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox1.Paint += new PaintEventHandler(DrawAll);
            trackBar2.Value = 1;
            SetAirdropUpgradedScale();

            pictureBox2.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Map Size is 15360 x 15360, 0,0 bottom left, middle 7680 x 7680
            pictureBox2.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox2.Paint += new PaintEventHandler(DrawAllSafezone);
            trackBar1.Value = 1;
            SetAirdropUpgradedSafeZoneScale();
        }
        private void LoadAirdropUpgraded()
        {
            useraction = false;

            UpdateDropTypeCB();
            UpdateLocationCB();

            VersionTB.Text = AirdropUpgradedSettings.Controls.Version;
            IntervalNUD.Value = AirdropUpgradedSettings.Controls.Interval;
            ModeCB.SelectedIndex = AirdropUpgradedSettings.Controls.Mode - 1;
            AD_LogManagerCB.Checked = AirdropUpgradedSettings.Controls.AD_LogManager == 1 ? true : false;
            AD_LogAircraftCB.Checked = AirdropUpgradedSettings.Controls.AD_LogContainer == 1 ? true : false;
            AD_LogContainerCB.Checked = AirdropUpgradedSettings.Controls.AD_LogContainer == 1 ? true : false;
            MinimumPlayersNUD.Value = AirdropUpgradedSettings.Controls.MinimumPlayers;
            MaxBackupDaysNUD.Value = AirdropUpgradedSettings.Controls.MaxBackupDays;
            MaxLogDaysNUD.Value = AirdropUpgradedSettings.Controls.MaxLogDays;
            SmokeTrailsCB.SelectedIndex = AirdropUpgradedSettings.Controls.SmokeTrails;

            HeightNUD.Value = AirdropUpgradedSettings.Map.Height;
            WidthNUD.Value = AirdropUpgradedSettings.Map.Width;
            OffsetNUD.Value = AirdropUpgradedSettings.Map.Offset;

            AirSpeedKIASNUD.Value = AirdropUpgradedSettings.Aircraft.AirSpeedKIAS;
            StartAltMSLNUD.Value = AirdropUpgradedSettings.Aircraft.StartAltMSL;
            DropAGLNUD.Value = AirdropUpgradedSettings.Aircraft.DropAGL;
            DropOffsetNUD.Value = AirdropUpgradedSettings.Aircraft.DropOffset;
            DropAccuracyNUD.Value = AirdropUpgradedSettings.Aircraft.DropAccuracy;
            TerrainFollowingNUD.Value = AirdropUpgradedSettings.Aircraft.TerrainFollowing;

            MessaggesModeCB.SelectedIndex = AirdropUpgradedSettings.Messages.Mode;
            DurationNUD.Value = AirdropUpgradedSettings.Messages.Duration;
            ProximityNUD.Value = AirdropUpgradedSettings.Messages.Proximity;
            ImperialUnitsNUD.Value = AirdropUpgradedSettings.Messages.ImperialUnits;

            TriggerAGLNUD.Value = AirdropUpgradedSettings.Container.TriggerAGL;
            FallRateNUD.Value = AirdropUpgradedSettings.Container.FallRate;
            StandUpTimerNUD.Value = AirdropUpgradedSettings.Container.StandUpTimer;
            SpawnMinNUD.Value = AirdropUpgradedSettings.Container.StandUpTimer;
            SpawnMaxNUD.Value = AirdropUpgradedSettings.Container.SpawnMax;
            SpawnOffsetNUD.Value = AirdropUpgradedSettings.Container.SpawnOffset;
            WindStrengthNUD.Value = AirdropUpgradedSettings.Container.WindStrength;
            LifespanNUD.Value = AirdropUpgradedSettings.Container.Lifespan;

            CreateDropLocationsTreeview();
            DropTypesLB.DisplayMember = "DisplayName";
            DropTypesLB.ValueMember = "Value";
            DropTypesLB.DataSource = AirdropUpgradedSettings.DropTypes;

            DropZoneLB.DisplayMember = "DisplayName";
            DropZoneLB.ValueMember = "Value";
            DropZoneLB.DataSource = AirdropUpgradedSettings.DropZones;

            SafeZoneLB.DisplayMember = "DisplayName";
            SafeZoneLB.ValueMember = "Value";
            SafeZoneLB.DataSource = AirdropUpgradedSafeZone.SafeZones;

            useraction = true;
        }
        private void CreateDropLocationsTreeview()
        {
            DropLocationsTV.Nodes.Clear();
            TreeNode root = new TreeNode("Locations")
            {
                Tag = "Locations"
            };
            foreach (Location item in AirdropUpgradedSettings.Locations)
            {
                root.Nodes.Add(Createlocation(item));
            }
            DropLocationsTV.Nodes.Add(root);
            root.Expand();
        }
        private TreeNode Createlocation(Location item)
        {
            TreeNode AirdropLocation = new TreeNode(item.Title)
            {
                Tag = item
            };
            return AirdropLocation;
        }

        private void UpdateLocationCB()
        {
            List<Location> droplocations = new List<Location>();
            droplocations.Add(new Location()
            {
                Title = "RANDOM"
            });
            droplocations.AddRange(AirdropUpgradedSettings.Locations);

            DropZoneLocationCB.DataSource = droplocations;
        }
        private void UpdateDropTypeCB()
        {
            List<Droptype> droptypes = new List<Droptype>();
            droptypes.Add(new Droptype()
            {
                Title = "RANDOM"
            });
            droptypes.AddRange(AirdropUpgradedSettings.DropTypes);

            DropZoneDropTypeCB.DataSource = droptypes;
        }
        private void DropZoneLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropZoneLB.SelectedItems.Count == 0) { return; }
            currentdropZone = DropZoneLB.SelectedItem as Dropzone;
            useraction = false;
            SetupDropZone();
            pictureBox1.Invalidate();
            useraction = true;
        }
        private void SetupDropZone()
        {
            DropZoneTitleTB.Text = currentdropZone.Title;
            DropZoneXNUD.Value = currentdropZone.X;
            DropZoneZNUD.Value = currentdropZone.Z;
            DropZoneRadiusNUD.Value = currentdropZone.Radius;
            DropZoneDropAccuracyNUD.Value = currentdropZone.DropAccuracy;
            DropZoneZombiesNUD.Value = currentdropZone.Zombies;
            DropZoneDropTypeCB.SelectedIndex = DropZoneDropTypeCB.FindStringExact(currentdropZone.DropType);
            DropZoneLocationCB.SelectedIndex = DropZoneLocationCB.FindStringExact(currentdropZone.Location);
        }

        private void DropZoneTitleTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentdropZone.Title = DropZoneTitleTB.Text;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void DropZoneXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentdropZone.X = (int)DropZoneXNUD.Value;
            pictureBox1.Invalidate();
            AirdropUpgradedSettings.isDirty = true;
        }
        private void DropZoneZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentdropZone.Z = (int)DropZoneZNUD.Value;
            pictureBox1.Invalidate();
            AirdropUpgradedSettings.isDirty = true;
        }
        private void DropZoneRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentdropZone.Radius = (int)DropZoneRadiusNUD.Value;
            pictureBox1.Invalidate();
            AirdropUpgradedSettings.isDirty = true;
        }
        private void DropZoneLocationCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentdropZone.Location = DropZoneLocationCB.GetItemText(DropZoneLocationCB.SelectedItem);
            AirdropUpgradedSettings.isDirty = true;
        }
        private void DropZoneDropTypeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentdropZone.DropType = DropZoneDropTypeCB.GetItemText(DropZoneDropTypeCB.SelectedItem);
            AirdropUpgradedSettings.isDirty = true;
        }
        private void DropZoneDropAccuracyNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentdropZone.DropAccuracy = (int)DropZoneDropAccuracyNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void DropZoneZombiesNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentdropZone.Zombies = (int)DropZoneZombiesNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void darkButton5_Click(object sender, EventArgs e)
        {
            Dropzone NewDropzone = new Dropzone()
            {
                Title = "New drop Zone",
                X = currentproject.MapSize / 2,
                Z = currentproject.MapSize / 2,
                Radius = 500,
                Zombies = 10,
                DropType = "RANDOM",
                DropAccuracy = 100,
                Location = "RANDOM"
            };
            AirdropUpgradedSettings.DropZones.Add(NewDropzone);
            pictureBox1.Invalidate();
            AirdropUpgradedSettings.isDirty = true;
        }
        private void darkButton6_Click(object sender, EventArgs e)
        {
            AirdropUpgradedSettings.DropZones.Remove(currentdropZone);
            AirdropUpgradedSettings.isDirty = true;
            pictureBox1.Invalidate();
        }


        public int HeliCrashMapscale = 1;
        private Point _mouseLastPosition;
        private Point _newscrollPosition;
        private TreeNode currenttreenode;
        private Location currentLocation;

        private void trackBar2_MouseUp(object sender, MouseEventArgs e)
        {
            HeliCrashMapscale = trackBar2.Value;
            SetAirdropUpgradedScale();

        }
        private void SetAirdropUpgradedScale()
        {
            float scalevalue = HeliCrashMapscale * 0.05f;
            float mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            pictureBox1.Size = new Size(newsize, newsize);
        }
        private void DrawAll(object sender, PaintEventArgs e)
        {
            float scalevalue = HeliCrashMapscale * 0.05f;
            foreach (Dropzone zones in AirdropUpgradedSettings.DropZones)
            {
                int centerX = (int)(Math.Round((float)zones.X, 0) * scalevalue);
                int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round((float)zones.Z, 0) * scalevalue);

                int radius = (int)((float)zones.Radius * scalevalue);
                Point center = new Point(centerX, centerY);
                Pen pen = new Pen(Color.Red)
                {
                    Width = 4
                };
                if (currentdropZone == zones)
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
            SetAirdropUpgradedScale();
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
            SetAirdropUpgradedScale();
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
                if (currentdropZone == null) { return; }
                Cursor.Current = Cursors.WaitCursor;
                DropZoneXNUD.Value = (decimal)(mouseEventArgs.X / scalevalue);
                DropZoneZNUD.Value = (decimal)((newsize - mouseEventArgs.Y) / scalevalue);
                Cursor.Current = Cursors.Default;
                AirdropUpgradedSettings.isDirty = true;
                pictureBox1.Invalidate();
            }
        }

        private void IntervalNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Controls.Interval = (int)IntervalNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void ModeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Controls.Mode = ModeCB.SelectedIndex + 1;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void AD_LogManagerCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Controls.AD_LogManager = AD_LogManagerCB.Checked == true ? 1 : 0;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void AD_LogAircraftCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Controls.AD_LogAircraft = AD_LogAircraftCB.Checked == true ? 1 : 0;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void AD_LogContainerCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Controls.AD_LogContainer = AD_LogContainerCB.Checked == true ? 1 : 0;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void MinimumPlayersNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Controls.MinimumPlayers = (int)MinimumPlayersNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void MaxBackupDaysNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Controls.MaxBackupDays = (int)MaxBackupDaysNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void MaxLogDaysNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Controls.MaxLogDays = (int)MaxLogDaysNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void SmokeTrailsCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Controls.SmokeTrails = SmokeTrailsCB.SelectedIndex;
            AirdropUpgradedSettings.isDirty = true;
        }

        private void WidthNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Map.Width = WidthNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void HeightNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Map.Height = HeightNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void OffsetNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Map.Offset = OffsetNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }

        private void AirSpeedKIASNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Aircraft.AirSpeedKIAS = (int)AirSpeedKIASNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void StartAltMSLNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Aircraft.StartAltMSL = (int)StartAltMSLNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void DropAGLNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Aircraft.DropAGL = (int)DropAGLNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void DropOffsetNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Aircraft.DropOffset = (int)DropOffsetNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void DropAccuracyNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Aircraft.DropAccuracy = (int)DropAccuracyNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void TerrainFollowingNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Aircraft.TerrainFollowing = TerrainFollowingNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }

        private void NotificationARGBPB_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            Rectangle region;
            region = pb.ClientRectangle;
            Color colour = Color.FromArgb(AirdropUpgradedSettings.Messages.NotificationARGB[0], AirdropUpgradedSettings.Messages.NotificationARGB[1], AirdropUpgradedSettings.Messages.NotificationARGB[2], AirdropUpgradedSettings.Messages.NotificationARGB[3]);
            using (Brush brush = new SolidBrush(colour))
            {
                e.Graphics.FillRectangle(brush, region);
            }
            e.Graphics.DrawRectangle(SystemPens.ControlText, region.Left, region.Top, region.Width - 1, region.Height - 1);
        }
        private void NotificationARGBPB_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            ColorPickerDialog cpick = new ColorPickerDialog();
            cpick.StartPosition = FormStartPosition.CenterParent;
            cpick.Color = Color.FromArgb(AirdropUpgradedSettings.Messages.NotificationARGB[0], AirdropUpgradedSettings.Messages.NotificationARGB[1], AirdropUpgradedSettings.Messages.NotificationARGB[2], AirdropUpgradedSettings.Messages.NotificationARGB[3]);
            if (cpick.ShowDialog() == DialogResult.OK)
            {

                AirdropUpgradedSettings.Messages.NotificationARGB[0] = cpick.Color.A;
                AirdropUpgradedSettings.Messages.NotificationARGB[1] = cpick.Color.R;
                AirdropUpgradedSettings.Messages.NotificationARGB[2] = cpick.Color.G;
                AirdropUpgradedSettings.Messages.NotificationARGB[3] = cpick.Color.B;
                NotificationARGBPB.Invalidate();
                AirdropUpgradedSettings.isDirty = true;
            }
        }

        private void ImperialUnitsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Messages.ImperialUnits = (int)ImperialUnitsNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void ProximityNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Messages.Proximity = (int)ProximityNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void DurationNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Messages.Duration = (int)DurationNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void MessaghesModeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Messages.Mode = MessaggesModeCB.SelectedIndex;
            AirdropUpgradedSettings.isDirty = true;
        }

        private void TriggerAGLNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Container.TriggerAGL = TriggerAGLNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void FallRateNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Container.FallRate = FallRateNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void StandUpTimerNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Container.StandUpTimer = StandUpTimerNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void SpawnMinNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Container.SpawnMin = SpawnMinNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void SpawnMaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Container.SpawnMax = SpawnMaxNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void SpawnOffset_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Container.SpawnOffset = SpawnOffsetNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void WindStrengthNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Container.WindStrength = WindStrengthNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void LifespanNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AirdropUpgradedSettings.Container.Lifespan = (int)LifespanNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }

        private void DropLocationsTV_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            DropLocationsTV.SelectedNode = e.Node;
            currenttreenode = e.Node;

            addNewLocationToolStripMenuItem.Visible = false;
            removeLocationToolStripMenuItem.Visible = false;
            LocationTitleGB.Visible = false;
            LocationZombiesGB.Visible = false;


            if (e.Node.Tag is string)
            {
                if (e.Node.Tag.ToString() == "Locations")
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        addNewLocationToolStripMenuItem.Visible = true;
                        contextMenuStrip1.Show(Cursor.Position);
                    }
                }
            }
            else if (e.Node.Tag is Location)
            {
                currentLocation = e.Node.Tag as Location;
                SetLocation();


                if (e.Button == MouseButtons.Right)
                {
                    removeLocationToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
        }
        private void SetLocation()
        {
            LocationTitleGB.Visible = true;
            LocationZombiesGB.Visible = true;
            LocationTitleTB.Text = currentLocation.Title;
            LocationZombiesLB.DataSource = currentLocation.Zombies;
        }
        private void LocationTitleTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentLocation.Title = LocationTitleTB.Text;
            currenttreenode.Name = currenttreenode.Text = currentLocation.Title;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void addNewLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Location newloc = new Location()
            {
                Title = "New location",
                Zombies = new BindingList<string>()
            };
            if (!AirdropUpgradedSettings.Locations.Any(x => x.Title == newloc.Title))
            {
                AirdropUpgradedSettings.Locations.Add(newloc);
                TreeNode tn = Createlocation(newloc);
                currenttreenode.Nodes.Add(tn);
                FocusNode = tn;
                AirdropUpgradedSettings.isDirty = true;
            }
            else
            {
                MessageBox.Show("There is another location with the same name.....");
                return;
            }
            DropLocationsTV.SelectedNode = FocusNode;
            DropLocationsTV.Focus();
            currentLocation = DropLocationsTV.SelectedNode.Tag as Location;
            SetLocation();
            UpdateLocationCB();
        }
        private void removeLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AirdropUpgradedSettings.Locations.Remove(currentLocation);
            DropLocationsTV.SelectedNode.Remove();
            AirdropUpgradedSettings.isDirty = true;
            UpdateLocationCB();
        }
        private void darkButton1_Click(object sender, EventArgs e)
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
                    if (!currentLocation.Zombies.Any(x => x == l))
                    {
                        currentLocation.Zombies.Add(l);
                        AirdropUpgradedSettings.isDirty = true;
                    }
                    else
                    {
                        MessageBox.Show("There is allready a Creature added : " + l);
                    }
                }
            }
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
            if (LocationZombiesLB.SelectedItems.Count <= 0) return;
            List<string> removeitems = new List<string>();
            foreach (var item in LocationZombiesLB.SelectedItems)
            {
                removeitems.Add(item as string);
            }
            foreach (string removeitem in removeitems)
            {
                currentLocation.Zombies.Remove(removeitem);
                AirdropUpgradedSettings.isDirty = true;
            }
            LocationZombiesLB.Refresh();
        }

        public int safezonescale = 1;
        private Safezone currentsafezone;

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            safezonescale = trackBar1.Value;
            SetAirdropUpgradedSafeZoneScale();
        }
        private void SetAirdropUpgradedSafeZoneScale()
        {
            float scalevalue = safezonescale * 0.05f;
            float mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            pictureBox2.Size = new Size(newsize, newsize);
        }
        private void DrawAllSafezone(object sender, PaintEventArgs e)
        {
            float scalevalue = safezonescale * 0.05f;
            foreach (Safezone zones in AirdropUpgradedSafeZone.SafeZones)
            {
                int centerX = (int)(Math.Round((float)zones.X, 0) * scalevalue);
                int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round((float)zones.Z, 0) * scalevalue);

                int radius = (int)((float)zones.Radius * scalevalue);
                Point center = new Point(centerX, centerY);
                Pen pen = new Pen(Color.Red)
                {
                    Width = 4
                };
                if (currentsafezone == zones)
                    pen.Color = Color.LimeGreen;
                else
                    pen.Color = Color.Red;
                getCircle(e.Graphics, pen, center, radius);
            }
        }
        private void PicBox2_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                ZoomOut2();
            }
            else
            {
                ZoomIn2();
            }

        }
        private void ZoomIn2()
        {
            int oldpictureboxhieght = pictureBox2.Height;
            int oldpitureboxwidht = pictureBox2.Width;
            Point oldscrollpos = panelEx1.AutoScrollPosition;
            int tbv = trackBar1.Value;
            int newval = tbv + 1;
            if (newval >= 20)
                newval = 20;
            trackBar1.Value = newval;
            safezonescale = trackBar1.Value;
            SetAirdropUpgradedSafeZoneScale();
            if (pictureBox2.Height > panelEx1.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox2.Height * newy);
                _newscrollPosition.Y = y * -1;
                panelEx1.AutoScrollPosition = _newscrollPosition;
            }
            if (pictureBox2.Width > panelEx1.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox2.Width * newy);
                _newscrollPosition.X = x * -1;
                panelEx1.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox2.Invalidate();
        }
        private void ZoomOut2()
        {
            int oldpictureboxhieght = pictureBox2.Height;
            int oldpitureboxwidht = pictureBox2.Width;
            Point oldscrollpos = panelEx1.AutoScrollPosition;
            int tbv = trackBar1.Value;
            int newval = tbv - 1;
            if (newval <= 1)
                newval = 1;
            trackBar1.Value = newval;
            safezonescale = trackBar1.Value;
            SetAirdropUpgradedSafeZoneScale();
            if (pictureBox2.Height > panelEx1.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox2.Height * newy);
                _newscrollPosition.Y = y * -1;
                panelEx1.AutoScrollPosition = _newscrollPosition;
            }
            if (pictureBox1.Width > panelEx1.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox2.Width * newy);
                _newscrollPosition.X = x * -1;
                panelEx1.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox2.Invalidate();
        }
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _mouseLastPosition = e.Location;
            }
        }
        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
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
                pictureBox2.Invalidate();
            }
        }
        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            if (pictureBox2.Focused == false)
            {
                pictureBox2.Focus();
                panelEx1.AutoScrollPosition = _newscrollPosition;
                pictureBox2.Invalidate();
            }
        }
        private void pictureBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var mouseEventArgs = e as MouseEventArgs;
            if (mouseEventArgs != null)
            {
                float scalevalue = safezonescale * 0.05f;
                float mapsize = currentproject.MapSize;
                int newsize = (int)(mapsize * scalevalue);
                if (currentsafezone == null) { return; }
                Cursor.Current = Cursors.WaitCursor;
                SafeZoneXNUD.Value = (decimal)(mouseEventArgs.X / scalevalue);
                SafeZoneZNUD.Value = (decimal)((newsize - mouseEventArgs.Y) / scalevalue);
                Cursor.Current = Cursors.Default;
                AirdropUpgradedSafeZone.isDirty = true;
                pictureBox2.Invalidate();
            }
        }

        private void SafeZoneLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SafeZoneLB.SelectedItems.Count <= 0) return;
            currentsafezone = SafeZoneLB.SelectedItem as Safezone;
            useraction = false;
            SafeZoneTitleTB.Text = currentsafezone.Title;
            SafeZoneMessageTB.Text = currentsafezone.Message;
            SafeZoneXNUD.Value = currentsafezone.X;
            SafeZoneZNUD.Value = currentsafezone.Z;
            SafeZoneRadiusNUD.Value = currentsafezone.Radius;
            pictureBox2.Invalidate();
            useraction = true;
        }
        private void SafeZoneZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentsafezone.Z = (int)SafeZoneZNUD.Value;
            AirdropUpgradedSafeZone.isDirty = true;
            pictureBox2.Invalidate();

        }
        private void SafeZoneXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentsafezone.X = (int)SafeZoneXNUD.Value;
            AirdropUpgradedSafeZone.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void SafeZoneRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentsafezone.Radius = (int)SafeZoneRadiusNUD.Value;
            AirdropUpgradedSafeZone.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void SafeZoneMessageTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentsafezone.Message = SafeZoneMessageTB.Text;
            AirdropUpgradedSafeZone.isDirty = true;
        }
        private void SafeZoneTitleTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentsafezone.Title = SafeZoneTitleTB.Text;
            AirdropUpgradedSafeZone.isDirty = true;
        }
        private void darkButton3_Click(object sender, EventArgs e)
        {
            Safezone newsz = new Safezone()
            {
                Title = "New Safe Zone",
                Message = "Airdrops cannot be dispatched to your current location ... Airdrop cancelled!",
                X = currentproject.MapSize / 2,
                Z = currentproject.MapSize / 2,
                Radius = 500
            };
            AirdropUpgradedSafeZone.SafeZones.Add(newsz);
            pictureBox2.Invalidate();
            AirdropUpgradedSafeZone.isDirty = true;
        }
        private void darkButton4_Click(object sender, EventArgs e)
        {
            AirdropUpgradedSafeZone.SafeZones.Remove(currentsafezone);
            AirdropUpgradedSafeZone.isDirty = true;
            pictureBox2.Invalidate(true);
        }

        private void DropTypesLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropTypesLB.SelectedItems.Count <= 0) return;
            currentDropType = DropTypesLB.SelectedItem as Droptype;
            useraction = false;
            DropTypeTitleTB.Text = currentDropType.Title;
            DropTypeContainerCB.SelectedIndex = DropTypeContainerCB.FindStringExact(currentDropType.Container);
            DropTypeQuantityNUD.Value = currentDropType.Quantity;
            DropTypeAddFlareNUD.Value = currentDropType.AddFlare;
            DropTypeSpawnMinNUD.Value = currentDropType.SpawnMin;
            DropTypeSpawnMaxNUD.Value = currentDropType.SpawnMax;
            DropTypeSpawnOffsetNUD.Value = currentDropType.SpawnOffset;
            DropTypeLifespanNUD.Value = currentDropType.Lifespan;

            DropTypeItemsLB.DisplayMember = "DisplayName";
            DropTypeItemsLB.ValueMember = "Value";
            DropTypeItemsLB.DataSource = currentDropType.Items;
            useraction = true;
        }
        private void darkButton7_Click(object sender, EventArgs e)
        {
            Droptype newdroptype = new Droptype()
            {
                Title = "New Type",
                Container = "AirdropContainer",
                Quantity = 10,
                AddFlare = (decimal)10.0,
                SpawnMin = (decimal)1.0,
                SpawnMax = (decimal)3.0,
                SpawnOffset = (decimal)0.05,
                Lifespan = 60,
                Items = new BindingList<string>()
            };
            AirdropUpgradedSettings.DropTypes.Add(newdroptype);
            AirdropUpgradedSettings.isDirty = true;
            UpdateDropTypeCB();
        }
        private void darkButton8_Click(object sender, EventArgs e)
        {
            AirdropUpgradedSettings.DropTypes.Remove(currentDropType);
            AirdropUpgradedSettings.isDirty = true;
            UpdateDropTypeCB();
        }
        private void DropTypeTitleTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentDropType.Title = DropTypeTitleTB.Text;
            DropTypesLB.Invalidate();
            AirdropUpgradedSettings.isDirty = true;
        }
        private void DropTypeContainerCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentDropType.Container = DropTypeContainerCB.GetItemText(DropTypeContainerCB.SelectedItem);
            AirdropUpgradedSettings.isDirty = true;
        }
        private void DropTypeQuantityNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentDropType.Quantity = (int)DropTypeQuantityNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void DropTypeAddFlareNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentDropType.AddFlare = DropTypeAddFlareNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void DropTypeSpawnMinNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentDropType.SpawnMin = DropTypeSpawnMinNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void DropTypeSpawnMaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentDropType.SpawnMax = DropTypeSpawnMaxNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void DropTypeSpawnOffsetNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentDropType.SpawnOffset = DropTypeSpawnOffsetNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void DropTypeLifespanNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentDropType.Lifespan = (int)DropTypeLifespanNUD.Value;
            AirdropUpgradedSettings.isDirty = true;
        }
        private void darkButton9_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes();
            form.vanillatypes = vanillatypes;
            form.ModTypes = ModTypes;
            form.currentproject = currentproject;
            form.UseOnlySingleitem = false;
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    currentDropType.Items.Add(l);
                    AirdropUpgradedSettings.isDirty = true;
                }
            }
        }
        private void darkButton10_Click(object sender, EventArgs e)
        {
            if (DropTypeItemsLB.SelectedItems.Count <= 0) return;
            List<string> removeitems = new List<string>();
            foreach (var item in DropTypeItemsLB.SelectedItems)
            {
                removeitems.Add(item as string);
            }
            foreach (string removeitem in removeitems)
            {
                currentDropType.Items.Remove(removeitem);
                AirdropUpgradedSettings.isDirty = true;
            }
            DropTypeItemsLB.Refresh();
        }
    }
}
