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
    public partial class KOSZonemanager : DarkForm
    {
        public Project currentproject { get; set; }
        public TypesFile vanillatypes;
        public List<TypesFile> ModTypes;

        public string KosZoneconfigPath { get; private set; }
        public KosZoneconfig KosZoneconfig { get; set; }
        public string KosPurgeConfigPath { get; private set; }
        public KosPurgeConfig KosPurgeConfig { get; set; }
        public string KozRestartConfigPath { get; private set; }
        public MapData MapData { get; private set; }

        public string Projectname;
        private bool _useraction = false;
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
        public KOSZonemanager()
        {
            InitializeComponent();
            tabControl1.ItemSize = new Size(0, 1);
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            if (tabControl1.SelectedIndex == 0)
                toolStripButton1.Checked = true;
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            if (tabControl1.SelectedIndex == 1)
                toolStripButton3.Checked = true;
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripButton1.Checked = false;
            toolStripButton3.Checked = false;
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    toolStripButton1.Checked = true;
                    break;
                case 1:
                    toolStripButton3.Checked = true;
                    break;
                default:
                    break;
            }
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\KosZone\\KZConfig");
        }
        private void KOSZonemanager_Load(object sender, EventArgs e)
        {
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            KosZoneconfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\KosZone\\KZConfig\\KosZoneConfig.json";
            if (!File.Exists(KosZoneconfigPath))
            {
                KosZoneconfig = new KosZoneconfig();
            }
            else
            {
                KosZoneconfig = JsonSerializer.Deserialize<KosZoneconfig>(File.ReadAllText(KosZoneconfigPath));
                KosZoneconfig.isDirty = false;
            }
            KosZoneconfig.FullFilename = KosZoneconfigPath;
            SetupKosZoneconfig();

            KosPurgeConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\KosZone\\KZConfig\\PurgeConfigV1.json";
            if (!File.Exists(KosPurgeConfigPath))
            {
                KosPurgeConfig = new KosPurgeConfig();
            }
            else
            {
                KosPurgeConfig = JsonSerializer.Deserialize<KosPurgeConfig>(File.ReadAllText(KosPurgeConfigPath));
                KosPurgeConfig.isDirty = false;
            }
            KosPurgeConfig.FullFilename = KosPurgeConfigPath;
            SetupKosPurgeConfig();

            MapData = new MapData(Application.StartupPath + currentproject.MapPath + ".xyz", currentproject.MapSize);

            pictureBox2.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Livonia maop size is 12800 x 12800, 0,0 bottom left, center 6400 x 6400
            pictureBox2.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox2.Paint += new PaintEventHandler(DrawKOSZones);
            trackBar4.Value = 1;
            SetKOSZonescale();
        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            SaveKOSZoneconfigs();
        }
        private void SaveKOSZoneconfigs()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (KosZoneconfig.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(KosZoneconfig.FullFilename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(KosZoneconfig.FullFilename) + "\\Backup\\" + SaveTime);
                    File.Copy(KosZoneconfig.FullFilename, Path.GetDirectoryName(KosZoneconfig.FullFilename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(KosZoneconfig.FullFilename) + ".bak", true);
                }
                KosZoneconfig.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(KosZoneconfig, options);
                File.WriteAllText(KosZoneconfig.FullFilename, jsonString);
                midifiedfiles.Add(Path.GetFileName(Path.GetFileName(KosZoneconfigPath)));
            }
            if (KosPurgeConfig.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(KosPurgeConfig.FullFilename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(KosPurgeConfig.FullFilename) + "\\Backup\\" + SaveTime);
                    File.Copy(KosPurgeConfig.FullFilename, Path.GetDirectoryName(KosPurgeConfig.FullFilename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(KosPurgeConfig.FullFilename) + ".bak", true);
                }
                KosPurgeConfig.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(KosPurgeConfig, options);
                File.WriteAllText(KosPurgeConfig.FullFilename, jsonString);
                midifiedfiles.Add(Path.GetFileName(Path.GetFileName(KosPurgeConfigPath)));
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

        #region KOSZoneLocations
        public Koszonearealocation currentKOSZoneAreaLocation { get; set; }
        public int ZoneScale = 1;
        private void SetupKosZoneconfig()
        {
            useraction = false;

            IsKosZoneActiveCB.Checked = KosZoneconfig.IsKosZoneActive == 1 ? true : false;
            KOSCheckNUD.Value = (int)KosZoneconfig.KOSCheck;

            KosZoneAreaLocationLB.DisplayMember = "Name";
            KosZoneAreaLocationLB.ValueMember = "Value";
            KosZoneAreaLocationLB.DataSource = KosZoneconfig.KosZoneAreaLocation;

            useraction = true;
        }
        private void KosZoneAreaLocationLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (KosZoneAreaLocationLB.SelectedItems.Count < 1) return;
            currentKOSZoneAreaLocation = KosZoneAreaLocationLB.SelectedItem as Koszonearealocation;
            useraction = false;
            KosZoneStatutTB.Text = currentKOSZoneAreaLocation.KosZoneStatut;
            KOSZoneXNUD.Value = (decimal)currentKOSZoneAreaLocation.X;
            KOSZoneYNUD.Value = (decimal)currentKOSZoneAreaLocation.Y;
            KOSZoneRadiusNUD.Value = (decimal)currentKOSZoneAreaLocation.Radius;
            IsMsgActiveCB.Checked = currentKOSZoneAreaLocation.IsMsgActive == 1 ? true : false;
            MsgEnterZoneTB.Text = currentKOSZoneAreaLocation.MsgEnterZone;
            MsgExitZoneTB.Text = currentKOSZoneAreaLocation.MsgExitZone;

            pictureBox2.Invalidate();
            useraction = true;
        }
        private void darkButton12_Click(object sender, EventArgs e)
        {
            float[] centre = new float[] { currentproject.MapSize / 2, 0, currentproject.MapSize / 2 };
            Koszonearealocation newzone = new Koszonearealocation()
            {
                KosZoneStatut = "NewZone",
                X = centre[0],
                Y = centre[2],
                Radius = 500,
                IsMsgActive = 1,
                MsgEnterZone = "YOU ARE IN A KOS AREA!",
                MsgExitZone = "YOU LEFT THE KOS AREA!"
            };
            KosZoneconfig.KosZoneAreaLocation.Add(newzone);
            KosZoneconfig.isDirty = true;
        }
        private void IsKosZoneActiveCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KosZoneconfig.IsKosZoneActive = IsKosZoneActiveCB.Checked == true ? 1 : 0;
            KosZoneconfig.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void KOSCheckNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KosZoneconfig.KOSCheck = (int)KOSCheckNUD.Value;
            KosZoneconfig.isDirty = true;
        }
        private void darkButton11_Click(object sender, EventArgs e)
        {
            KosZoneconfig.KosZoneAreaLocation.Remove(currentKOSZoneAreaLocation);
            KosZoneconfig.isDirty = true;
            pictureBox2.Invalidate();
            if (KosZoneAreaLocationLB.Items.Count == 0)
                KosZoneAreaLocationLB.SelectedIndex = -1;
            else
                KosZoneAreaLocationLB.SelectedIndex = 0;
        }
        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {
            if (e is MouseEventArgs mouseEventArgs)
            {
                Cursor.Current = Cursors.WaitCursor;
                float scalevalue = ZoneScale * 0.05f;
                float mapsize = currentproject.MapSize;
                int newsize = (int)(mapsize * scalevalue);
                KOSZoneXNUD.Value = (decimal)(mouseEventArgs.X / scalevalue);
                KOSZoneYNUD.Value = (decimal)((newsize - mouseEventArgs.Y) / scalevalue);
                Cursor.Current = Cursors.Default;
                KosZoneconfig.isDirty = true;
                pictureBox2.Invalidate();
            }
        }
        private void SetKOSZonescale()
        {
            float scalevalue = ZoneScale * 0.05f;
            float mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            pictureBox2.Size = new Size(newsize, newsize);
        }
        private void trackBar4_MouseUp(object sender, MouseEventArgs e)
        {
            ZoneScale = trackBar4.Value;
            SetKOSZonescale();
        }
        private void DrawKOSZones(object sender, PaintEventArgs e)
        {
            if (KosZoneconfig.IsKosZoneActive == 0) return;
            foreach (Koszonearealocation zones in KosZoneconfig.KosZoneAreaLocation)
            {
                float scalevalue = ZoneScale * 0.05f;
                int centerX = (int)(Math.Round(zones.X) * scalevalue);
                int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(zones.Y, 0) * scalevalue);
                int radius = (int)(Math.Round(zones.Radius, 0) * scalevalue);
                Point center = new Point(centerX, centerY);
                Pen pen = new Pen(Color.Red, 4);
                if (zones == currentKOSZoneAreaLocation)
                    pen.Color = Color.LimeGreen;
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
        private void KOSZoneXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOSZoneAreaLocation.X = (float)KOSZoneXNUD.Value;
            KosZoneconfig.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void KOSZoneYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOSZoneAreaLocation.Y = (float)KOSZoneYNUD.Value;
            KosZoneconfig.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void KOSZoneRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOSZoneAreaLocation.Radius = (float)KOSZoneRadiusNUD.Value;
            KosZoneconfig.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void KosZoneStatutTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOSZoneAreaLocation.KosZoneStatut = KosZoneStatutTB.Text;
            KosZoneconfig.isDirty = true;
            KosZoneAreaLocationLB.Refresh();
        }
        private void IsMsgActiveCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOSZoneAreaLocation.IsMsgActive = IsMsgActiveCB.Checked == true ? 1 : 0;
            KosZoneconfig.isDirty = true;
        }
        private void MsgEnterZoneTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOSZoneAreaLocation.MsgEnterZone = MsgEnterZoneTB.Text;
            KosZoneconfig.isDirty = true;
        }
        private void MsgExitZoneTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOSZoneAreaLocation.MsgExitZone = MsgExitZoneTB.Text;
            KosZoneconfig.isDirty = true;
        }
        #endregion KOSZoneLocations

        #region KOSZonePurge
        public Purgeschedule currentPurgeschedule { get; set; }
        public Dynamicpurgeschedule currentDynamicpurgeschedule { get; set; }
        public void SetNumberofweeks()
        {
            List<CheckBox> checkboxes = DynamicPurgeWeeknumberGB.Controls.OfType<CheckBox>().ToList();

            foreach (CheckBox cb in checkboxes)
            {
                cb.Visible = false;
            }

            int Schedule = KosPurgeConfig.ScheduleCycle;
            for (int i = 0; i < Schedule; i++)
            {
                CheckBox cb = checkboxes[i];
                cb.Tag = (i + 1).ToString();
                cb.Checked = false;
                cb.Visible = true;
                cb.Text = (i + 1).ToString();
            }
        }
        private void SetupKosPurgeConfig()
        {
            useraction = false;

            SetNumberofweeks();

            StaticPurgeDOWCB.DataSource = Enum.GetValues(typeof(KOZDayOfWeek));
            SPWeekNumCB.DataSource = Enum.GetValues(typeof(WeekNumber));

            IsPurgeActiveCB.Checked = KosPurgeConfig.IsPurgeEnabled == 1 ? true : false;
            IsDynPurgeActiveCB.Checked = KosPurgeConfig.IsDynPurgeEnabled == 1 ? true : false;
            ScheduleCycleNUD.Value = KosPurgeConfig.ScheduleCycle;
            ServerrestartCycleNUD.Value = KosPurgeConfig.RestartCycle;


            PurgeSchedulesLB.DisplayMember = "Name";
            PurgeSchedulesLB.ValueMember = "Value";
            PurgeSchedulesLB.DataSource = KosPurgeConfig.PurgeSchedules;


            DynamicPurgeSchedulesLB.DisplayMember = "Name";
            DynamicPurgeSchedulesLB.ValueMember = "Value";
            DynamicPurgeSchedulesLB.DataSource = KosPurgeConfig.DynamicPurgeSchedules;


            useraction = true;
        }
        private void IsPurgeActiveCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KosPurgeConfig.IsPurgeEnabled = IsPurgeActiveCB.Checked == true ? 1 : 0;
            KosPurgeConfig.isDirty = true;
        }
        private void IsDynPurgeActiveCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KosPurgeConfig.IsDynPurgeEnabled = IsDynPurgeActiveCB.Checked == true ? 1 : 0;
            KosPurgeConfig.isDirty = true;
        }
        private void TimeZoneNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KosPurgeConfig.ScheduleCycle = (int)ScheduleCycleNUD.Value;
            KosPurgeConfig.isDirty = true;
        }
        private void ServerrestartCycleNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KosPurgeConfig.RestartCycle = (int)ServerrestartCycleNUD.Value;
            KosPurgeConfig.isDirty = true;
        }
        private void PurgeSchedulesLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PurgeSchedulesLB.SelectedItems.Count < 1) return;
            currentPurgeschedule = PurgeSchedulesLB.SelectedItem as Purgeschedule;
            useraction = false;

            SPNameTB.Text = currentPurgeschedule.PurgeName;
            SPWeekNumCB.SelectedItem = (WeekNumber)currentPurgeschedule.WeekNumber;
            StaticPurgeDOWCB.SelectedItem = (KOZDayOfWeek)currentPurgeschedule.Day;
            DateTime Dtime = DateTime.Now.Date;
            Dtime = Dtime.AddHours(currentPurgeschedule.StartHour);
            Dtime = Dtime.AddMinutes(currentPurgeschedule.StartMin);
            SPStartDT.Value = Dtime;
            DateTime Dtime2 = DateTime.Now.Date;
            Dtime2 = Dtime2.AddHours(currentPurgeschedule.EndHour);
            Dtime2 = Dtime2.AddMinutes(currentPurgeschedule.EndMin);
            SPEndDT.Value = Dtime2;

            AllowRaidingCB.Checked = currentPurgeschedule.AllowRaiding == 1 ? true : false;
            useraction = true;
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
            Purgeschedule newPurgeschedule = new Purgeschedule()
            {
                PurgeName = "New Purge",
                WeekNumber = 1,
                Day = 1,
                StartHour = 0,
                StartMin = 0,
                EndHour = 0,
                EndMin = 0,
                AllowRaiding = 0
            };
            KosPurgeConfig.PurgeSchedules.Add(newPurgeschedule);
            KosPurgeConfig.isDirty = true;
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            KosPurgeConfig.PurgeSchedules.Remove(currentPurgeschedule);
            KosPurgeConfig.isDirty = true;
            if (PurgeSchedulesLB.Items.Count == 0)
                PurgeSchedulesLB.SelectedIndex = -1;
            else
                PurgeSchedulesLB.SelectedIndex = 0;
        }
        private void DynamicPurgeSchedulesLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DynamicPurgeSchedulesLB.SelectedItems.Count < 1) return;
            currentDynamicpurgeschedule = DynamicPurgeSchedulesLB.SelectedItem as Dynamicpurgeschedule;
            useraction = false;

            DPNameTB.Text = currentDynamicpurgeschedule.DynamicPurgeName;
            List<CheckBox> checkboxes = DynamicPurgeWeeknumberGB.Controls.OfType<CheckBox>().ToList();
            foreach (CheckBox cb in checkboxes)
            {
                cb.Checked = false;
            }
            for (int i = 0; i < currentDynamicpurgeschedule.WeekNumber.Count; i++)
            {
                int weeknum = currentDynamicpurgeschedule.WeekNumber[i];
                foreach (CheckBox c in DynamicPurgeWeeknumberGB.Controls.OfType<CheckBox>())
                {
                    if (c.Tag.ToString() == weeknum.ToString())
                    {
                        c.Checked = true;
                    }
                }
            }
            List<CheckBox> checkboxes1 = groupBox1.Controls.OfType<CheckBox>().ToList();

            foreach (CheckBox cb in checkboxes1)
            {
                cb.Checked = false;
            }
            foreach (int dayofweek in currentDynamicpurgeschedule.Day)
            {
                switch (dayofweek)
                {
                    case 1:
                        SundayDOWCB.Checked = true;
                        break;
                    case 2:
                        MondayDOWCB.Checked = true;
                        break;
                    case 3:
                        TuesdayDOWCB.Checked = true;
                        break;
                    case 4:
                        WednesdayDOWCB.Checked = true;
                        break;
                    case 5:
                        ThursdayDOWCB.Checked = true;
                        break;
                    case 6:
                        FridayDOWCB.Checked = true;
                        break;
                    case 7:
                        SaturdayDOWCB.Checked = true;
                        break;

                }
            }
            numericUpDown7.Value = (decimal)currentDynamicpurgeschedule.Chance;
            DynamicPurgeDurationMinNUD.Value = currentDynamicpurgeschedule.DurationMin;
            DynamicPurgeDurationMaxNUD.Value = currentDynamicpurgeschedule.DurationMax;

            DateTime Dtime = DateTime.Now.Date;
            Dtime = Dtime.AddHours(currentDynamicpurgeschedule.StartHour);
            Dtime = Dtime.AddMinutes(currentDynamicpurgeschedule.StartMin);
            DPStartDT.Value = Dtime;
            DateTime Dtime2 = DateTime.Now.Date;
            Dtime2 = Dtime2.AddHours(currentDynamicpurgeschedule.EndHour);
            Dtime2 = Dtime2.AddMinutes(currentDynamicpurgeschedule.EndMin);
            DPEndDT.Value = Dtime2;
            useraction = true;
        }
        private void darkButton4_Click(object sender, EventArgs e)
        {
            Dynamicpurgeschedule newDynamicpurgeschedule = new Dynamicpurgeschedule()
            {
                DynamicPurgeName = "New Dynamic Purge",
                WeekNumber = new BindingList<int>(),
                Day = new BindingList<int>(),
                Chance = (decimal)0.5,
                DurationMin = 15,
                DurationMax = 60
            };
            KosPurgeConfig.DynamicPurgeSchedules.Add(newDynamicpurgeschedule);
            KosPurgeConfig.isDirty = true;
        }
        private void darkButton3_Click(object sender, EventArgs e)
        {
            KosPurgeConfig.DynamicPurgeSchedules.Remove(currentDynamicpurgeschedule);
            KosPurgeConfig.isDirty = true;
            pictureBox2.Invalidate();
            if (DynamicPurgeSchedulesLB.Items.Count == 0)
                DynamicPurgeSchedulesLB.SelectedIndex = -1;
            else
                DynamicPurgeSchedulesLB.SelectedIndex = 0;
        }
        private void SPNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentPurgeschedule.PurgeName = SPNameTB.Text;
            KosPurgeConfig.isDirty = true;
        }
        private void SPWeekNumCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            WeekNumber cacl = (WeekNumber)SPWeekNumCB.SelectedItem;
            currentPurgeschedule.WeekNumber = (int)cacl;
            KosPurgeConfig.isDirty = true;
        }
        private void StaticPurgeDOWCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KOZDayOfWeek cacl = (KOZDayOfWeek)StaticPurgeDOWCB.SelectedItem;
            currentPurgeschedule.Day = (int)cacl;
            KosPurgeConfig.isDirty = true;

        }
        private void SPStartDT_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            DateTime Dtime = SPStartDT.Value;
            currentPurgeschedule.StartHour = Dtime.Hour;
            currentPurgeschedule.StartMin = Dtime.Minute;
            KosPurgeConfig.isDirty = true;
        }
        private void SPEndDT_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            DateTime Dtime = SPEndDT.Value;
            currentPurgeschedule.EndHour = Dtime.Hour;
            currentPurgeschedule.EndMin = Dtime.Minute;
            KosPurgeConfig.isDirty = true;
        }
        private void AllowRaidingCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentPurgeschedule.AllowRaiding = AllowRaidingCB.Checked == true ? 1 : 0;
            KosPurgeConfig.isDirty = true;
        }
        private void DPNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentDynamicpurgeschedule.DynamicPurgeName = DPNameTB.Text;
            KosPurgeConfig.isDirty = true;
        }
        private void Week1CB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (Week1CB.Checked)
            {
                if (!currentDynamicpurgeschedule.WeekNumber.Contains(1))
                {
                    currentDynamicpurgeschedule.WeekNumber.Add(1);
                }
            }
            else
            {
                if (currentDynamicpurgeschedule.WeekNumber.Contains(1))
                {
                    currentDynamicpurgeschedule.WeekNumber.Remove(1);
                }
            }
            var sortedListInstance = new List<int>(currentDynamicpurgeschedule.WeekNumber);
            sortedListInstance.Sort();
            currentDynamicpurgeschedule.WeekNumber = new BindingList<int>(sortedListInstance);
            KosPurgeConfig.isDirty = true;
        }
        private void Week2CB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (Week2CB.Checked)
            {
                if (!currentDynamicpurgeschedule.WeekNumber.Contains(2))
                {
                    currentDynamicpurgeschedule.WeekNumber.Add(2);
                }
            }
            else
            {
                if (currentDynamicpurgeschedule.WeekNumber.Contains(2))
                {
                    currentDynamicpurgeschedule.WeekNumber.Remove(2);
                }
            }
            var sortedListInstance = new List<int>(currentDynamicpurgeschedule.WeekNumber);
            sortedListInstance.Sort();
            currentDynamicpurgeschedule.WeekNumber = new BindingList<int>(sortedListInstance);
            KosPurgeConfig.isDirty = true;
        }
        private void Week3CB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (Week3CB.Checked)
            {
                if (!currentDynamicpurgeschedule.WeekNumber.Contains(3))
                {
                    currentDynamicpurgeschedule.WeekNumber.Add(3);
                }
            }
            else
            {
                if (currentDynamicpurgeschedule.WeekNumber.Contains(3))
                {
                    currentDynamicpurgeschedule.WeekNumber.Remove(3);
                }
            }
            var sortedListInstance = new List<int>(currentDynamicpurgeschedule.WeekNumber);
            sortedListInstance.Sort();
            currentDynamicpurgeschedule.WeekNumber = new BindingList<int>(sortedListInstance);
            KosPurgeConfig.isDirty = true;
        }
        private void Week4CB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (Week4CB.Checked)
            {
                if (!currentDynamicpurgeschedule.WeekNumber.Contains(4))
                {
                    currentDynamicpurgeschedule.WeekNumber.Add(4);
                }
            }
            else
            {
                if (currentDynamicpurgeschedule.WeekNumber.Contains(4))
                {
                    currentDynamicpurgeschedule.WeekNumber.Remove(4);
                }
            }
            var sortedListInstance = new List<int>(currentDynamicpurgeschedule.WeekNumber);
            sortedListInstance.Sort();
            currentDynamicpurgeschedule.WeekNumber = new BindingList<int>(sortedListInstance);
            KosPurgeConfig.isDirty = true;
        }
        private void SundayDOWCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (SundayDOWCB.Checked)
            {
                if (!currentDynamicpurgeschedule.Day.Contains(1))
                {
                    currentDynamicpurgeschedule.Day.Add(1);
                }
            }
            else
            {
                if (currentDynamicpurgeschedule.Day.Contains(1))
                {
                    currentDynamicpurgeschedule.Day.Remove(1);
                }
            }
            var sortedListInstance = new List<int>(currentDynamicpurgeschedule.Day);
            sortedListInstance.Sort();
            currentDynamicpurgeschedule.Day = new BindingList<int>(sortedListInstance);
            KosPurgeConfig.isDirty = true;
        }
        private void MondayDOWCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (MondayDOWCB.Checked)
            {
                if (!currentDynamicpurgeschedule.Day.Contains(2))
                {
                    currentDynamicpurgeschedule.Day.Add(2);
                }
            }
            else
            {
                if (currentDynamicpurgeschedule.Day.Contains(2))
                {
                    currentDynamicpurgeschedule.Day.Remove(2);
                }
            }
            var sortedListInstance = new List<int>(currentDynamicpurgeschedule.Day);
            sortedListInstance.Sort();
            currentDynamicpurgeschedule.Day = new BindingList<int>(sortedListInstance);
            KosPurgeConfig.isDirty = true;
        }
        private void TuesdayDOWCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (TuesdayDOWCB.Checked)
            {
                if (!currentDynamicpurgeschedule.Day.Contains(3))
                {
                    currentDynamicpurgeschedule.Day.Add(3);
                }
            }
            else
            {
                if (currentDynamicpurgeschedule.Day.Contains(3))
                {
                    currentDynamicpurgeschedule.Day.Remove(3);
                }
            }
            var sortedListInstance = new List<int>(currentDynamicpurgeschedule.Day);
            sortedListInstance.Sort();
            currentDynamicpurgeschedule.Day = new BindingList<int>(sortedListInstance);
            KosPurgeConfig.isDirty = true;
        }
        private void WednesdayDOWCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (WednesdayDOWCB.Checked)
            {
                if (!currentDynamicpurgeschedule.Day.Contains(4))
                {
                    currentDynamicpurgeschedule.Day.Add(4);
                }
            }
            else
            {
                if (currentDynamicpurgeschedule.Day.Contains(4))
                {
                    currentDynamicpurgeschedule.Day.Remove(4);
                }
            }
            var sortedListInstance = new List<int>(currentDynamicpurgeschedule.Day);
            sortedListInstance.Sort();
            currentDynamicpurgeschedule.Day = new BindingList<int>(sortedListInstance);
            KosPurgeConfig.isDirty = true;
        }
        private void ThursdayDOWCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (ThursdayDOWCB.Checked)
            {
                if (!currentDynamicpurgeschedule.Day.Contains(5))
                {
                    currentDynamicpurgeschedule.Day.Add(5);
                }
            }
            else
            {
                if (currentDynamicpurgeschedule.Day.Contains(5))
                {
                    currentDynamicpurgeschedule.Day.Remove(5);
                }
            }
            var sortedListInstance = new List<int>(currentDynamicpurgeschedule.Day);
            sortedListInstance.Sort();
            currentDynamicpurgeschedule.Day = new BindingList<int>(sortedListInstance);
            KosPurgeConfig.isDirty = true;
        }
        private void FridayDOWCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (FridayDOWCB.Checked)
            {
                if (!currentDynamicpurgeschedule.Day.Contains(6))
                {
                    currentDynamicpurgeschedule.Day.Add(6);
                }
            }
            else
            {
                if (currentDynamicpurgeschedule.Day.Contains(6))
                {
                    currentDynamicpurgeschedule.Day.Remove(6);
                }
            }
            var sortedListInstance = new List<int>(currentDynamicpurgeschedule.Day);
            sortedListInstance.Sort();
            currentDynamicpurgeschedule.Day = new BindingList<int>(sortedListInstance);
            KosPurgeConfig.isDirty = true;
        }
        private void SaturdayDOWCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (SaturdayDOWCB.Checked)
            {
                if (!currentDynamicpurgeschedule.Day.Contains(7))
                {
                    currentDynamicpurgeschedule.Day.Add(7);
                }
            }
            else
            {
                if (currentDynamicpurgeschedule.Day.Contains(7))
                {
                    currentDynamicpurgeschedule.Day.Remove(7);
                }
            }
            var sortedListInstance = new List<int>(currentDynamicpurgeschedule.Day);
            sortedListInstance.Sort();
            currentDynamicpurgeschedule.Day = new BindingList<int>(sortedListInstance);
            KosPurgeConfig.isDirty = true;
        }
        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentDynamicpurgeschedule.Chance = numericUpDown7.Value;
            KosPurgeConfig.isDirty = true;
        }
        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentDynamicpurgeschedule.DurationMin = (int)DynamicPurgeDurationMinNUD.Value;
            KosPurgeConfig.isDirty = true;
        }
        private void DynamicPurgeDurationMaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentDynamicpurgeschedule.DurationMax = (int)DynamicPurgeDurationMaxNUD.Value;
            KosPurgeConfig.isDirty = true;
        }
        private void DPStartDT_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            DateTime Dtime = DPStartDT.Value;
            currentDynamicpurgeschedule.StartHour = Dtime.Hour;
            currentDynamicpurgeschedule.StartMin = Dtime.Minute;
            KosPurgeConfig.isDirty = true;
        }
        private void DPEndDT_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            DateTime Dtime = DPEndDT.Value;
            currentDynamicpurgeschedule.EndHour = Dtime.Hour;
            currentDynamicpurgeschedule.EndMin = Dtime.Minute;
            KosPurgeConfig.isDirty = true;
        }

        #endregion KOSZonePurg

        private void KOSZonemanager_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool needtosave = false;
            if (KosZoneconfig.isDirty)
            {
                needtosave = true;
            }
            if (KosPurgeConfig.isDirty)
            {
                needtosave = true;
            }
            if (needtosave)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SaveKOSZoneconfigs();
                }
            }
        }


    }
}
