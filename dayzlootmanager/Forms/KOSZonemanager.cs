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
        public KozRestartConfig KozRestartConfig { get; private set; }
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
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
            if (tabControl1.SelectedIndex == 2)
                toolStripButton4.Checked = true;
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripButton1.Checked = false;
            toolStripButton3.Checked = false;
            toolStripButton4.Checked = false;
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    toolStripButton1.Checked = true;
                    break;
                case 1:
                    toolStripButton3.Checked = true;
                    break;
                case 2:
                    toolStripButton4.Checked = true;
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

            KozRestartConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\KosZone\\KZConfig\\RestartConfigV1.json";
            if (!File.Exists(KozRestartConfigPath))
            {
                KozRestartConfig = new KozRestartConfig();
            }
            else
            {
                KozRestartConfig = JsonSerializer.Deserialize<KozRestartConfig>(File.ReadAllText(KozRestartConfigPath));
                KozRestartConfig.isDirty = false;
            }
            KozRestartConfig.FullFilename = KosPurgeConfigPath;
            SetupKozRestartConfig();

            MapData = new MapData(Application.StartupPath + currentproject.MapPath + ".xyz");

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
            if (KosZoneconfig.isDirty)
            {
                KosZoneconfig.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(KosZoneconfig, options);
                File.WriteAllText(KosZoneconfig.FullFilename, jsonString);
                midifiedfiles.Add(Path.GetFileName(Path.GetFileName(KosZoneconfigPath)));
            }
            if (KosPurgeConfig.isDirty)
            {
                KosPurgeConfig.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(KosPurgeConfig, options);
                File.WriteAllText(KosPurgeConfig.FullFilename, jsonString);
                midifiedfiles.Add(Path.GetFileName(Path.GetFileName(KosPurgeConfigPath)));
            }
            if (KozRestartConfig.isDirty)
            {
                KozRestartConfig.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(KozRestartConfig, options);
                File.WriteAllText(KozRestartConfig.FullFilename, jsonString);
                midifiedfiles.Add(Path.GetFileName(Path.GetFileName(KozRestartConfigPath)));
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
        private int currentrestarthour;
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
            KOSZoneRadiusNUD.Value = (decimal) currentKOSZoneAreaLocation.Radius;
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
        private void SetupKosPurgeConfig()
        {
            useraction = false;

            IsPurgeActiveCB.Checked = KosPurgeConfig.IsPurgeActive == 1 ? true : false;
            IsDynPurgeActiveCB.Checked = KosPurgeConfig.IsDynPurgeActive == 1 ? true : false;
            TimeZoneNUD.Value = KosPurgeConfig.TimeZone;


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
            if(!useraction) return;
            KosPurgeConfig.IsPurgeActive = IsPurgeActiveCB.Checked == true ? 1 : 0;
            KosPurgeConfig.isDirty = true;
        }
        private void IsDynPurgeActiveCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KosPurgeConfig.IsDynPurgeActive = IsDynPurgeActiveCB.Checked == true ? 1 : 0;
            KosPurgeConfig.isDirty = true;
        }
        private void TimeZoneNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KosPurgeConfig.TimeZone = (int)TimeZoneNUD.Value;
            KosPurgeConfig.isDirty = true;
        }
        private void PurgeSchedulesLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PurgeSchedulesLB.SelectedItems.Count < 1) return;
            currentPurgeschedule = PurgeSchedulesLB.SelectedItem as Purgeschedule;

            numericUpDown1.Value = currentPurgeschedule.Day;
            numericUpDown2.Value = currentPurgeschedule.StartHour;
            numericUpDown3.Value = currentPurgeschedule.StartMin;
            numericUpDown4.Value = currentPurgeschedule.EndHour;
            numericUpDown5.Value = currentPurgeschedule.EndMin;
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
            Purgeschedule newPurgeschedule = new Purgeschedule()
            {
                Day = 0,
                StartHour = 0,
                StartMin = 0,
                EndHour = 0,
                EndMin = 0
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

            numericUpDown6.Value = currentDynamicpurgeschedule.Day;
            numericUpDown7.Value = (decimal) currentDynamicpurgeschedule.Chance;
            numericUpDown8.Value = currentDynamicpurgeschedule.Duration;
        }
        private void darkButton4_Click(object sender, EventArgs e)
        {
            Dynamicpurgeschedule newDynamicpurgeschedule = new Dynamicpurgeschedule()
            {
                Day = 0,
                Chance = 0,
                Duration = 0
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
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentPurgeschedule.Day = (int)numericUpDown1.Value;
            KosPurgeConfig.isDirty = true;
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentPurgeschedule.StartHour = (int)numericUpDown3.Value;
            KosPurgeConfig.isDirty = true;
        }
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentPurgeschedule.StartMin = (int)numericUpDown3.Value;
            KosPurgeConfig.isDirty = true;
        }
        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentPurgeschedule.EndHour = (int)numericUpDown4.Value;
            KosPurgeConfig.isDirty = true;
        }
        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentPurgeschedule.EndMin = (int)numericUpDown5.Value;
            KosPurgeConfig.isDirty = true;
        }
        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentDynamicpurgeschedule.Day = (int)numericUpDown6.Value;
            KosPurgeConfig.isDirty = true;
        }
        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentDynamicpurgeschedule.Chance = (float)numericUpDown7.Value;
            KosPurgeConfig.isDirty = true;
        }
        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentDynamicpurgeschedule.Duration = (int)numericUpDown8.Value;
            KosPurgeConfig.isDirty = true;
        }
        #endregion KOSZonePurge
  
        #region restart
        private void SetupKozRestartConfig()
        {
            useraction = false;
            IsRestartMsgActiveCB.Checked = KozRestartConfig.IsRestartMsgActive == 1 ? true : false;

            m_hoursLB.DisplayMember = "Name";
            m_hoursLB.ValueMember = "Value";
            m_hoursLB.DataSource = KozRestartConfig.m_hours;

            useraction = true;
        }
        private void darkButton6_Click(object sender, EventArgs e)
        {
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    KozRestartConfig.m_hours.Add(Convert.ToInt32(l));
                    KozRestartConfig.isDirty = true;
                }
            }
        }
        private void m_hoursLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_hoursLB.SelectedItems.Count == 0) return;
            currentrestarthour = (int)m_hoursLB.SelectedItem;
        }
        private void darkButton5_Click(object sender, EventArgs e)
        {
            KozRestartConfig.m_hours.Remove(currentrestarthour);
            KozRestartConfig.isDirty = true;
            if (m_hoursLB.Items.Count == 0)
                m_hoursLB.SelectedIndex = -1;
            else
                m_hoursLB.SelectedIndex = 0;
        }
        private void IsRestartMsgActiveCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KozRestartConfig.IsRestartMsgActive = IsRestartMsgActiveCB.Checked == true ? 1 : 0;
            KozRestartConfig.isDirty = true;
        }
        #endregion restart
    }
}
