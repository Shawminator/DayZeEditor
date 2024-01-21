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
    public partial class KOTHManager : DarkForm
    {
        public Project currentproject { get; set; }
        public TypesFile vanillatypes;
        public List<TypesFile> ModTypes;

        public string KOTHConfigPath { get; private set; }
        public string KOTHLootPath { get; private set; }
        public MDCKOTHConfig MDCKOTHConfig { get; set; }
        public MDCKOTHLoot MDCKOTHLoot { get; set; }

        public MapData MapData { get; private set; }

        public string Projectname;
        private bool _useraction = false;
        private MDCKOTHZones currentKOTHZoneAreaLocation;

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


        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripButton1.Checked = false;
            toolStripButton3.Checked = false;
            toolStripButton4.Checked = false;
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    toolStripButton3.Checked = true;
                    break;
                case 1:
                    toolStripButton1.Checked = true;
                    if(HillsLB.Items.Count <= 0)
                    {
                        tabControl2.Visible = false;
                    }
                    else
                    {
                        tabControl2.Visible = true;
                        HillsLB.SelectedIndex = -1;
                        HillsLB.SelectedIndex = 0;
                    }
                    break;
                case 2:
                    toolStripButton4.Checked = true;
                    break;
                default:
                    break;
            }
        }


        public int ZoneScale = 1;
        private void SetHillZonescale()
        {
            float scalevalue = ZoneScale * 0.05f;
            float mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            pictureBox2.Size = new Size(newsize, newsize);
        }
        private void DrawHillZones(object sender, PaintEventArgs e)
        {
            foreach (MDCKOTHZones Hills in MDCKOTHConfig.zones)
            {
                float scalevalue = ZoneScale * 0.05f;
                int centerX = (int)(Math.Round(Hills.zonePosition[0]) * scalevalue);
                int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(Hills.zonePosition[2], 0) * scalevalue);
                int eventradius = (int)(Math.Round((float)Hills.zoneRadius, 0) * scalevalue);
                Point center = new Point(centerX, centerY);
                Pen pen = new Pen(Color.Red, 4);
                if (Hills == currentKOTHZoneAreaLocation)
                    pen.Color = Color.LimeGreen;
                getCircle(e.Graphics, pen, center, eventradius);
            }
        }
        private void getCircle(Graphics drawingArea, Pen penToUse, Point center, int radius)
        {
            Rectangle rect = new Rectangle(center.X - 1, center.Y - 1, 2, 2);
            drawingArea.DrawEllipse(penToUse, rect);
            Rectangle rect2 = new Rectangle(center.X - radius, center.Y - radius, radius * 2, radius * 2);
            drawingArea.DrawEllipse(penToUse, rect2);
        }
        private void trackBar4_MouseUp(object sender, MouseEventArgs e)
        {
            ZoneScale = trackBar4.Value;
            SetHillZonescale();
        }

        public KOTHManager()
        {
            InitializeComponent();
            tabControl1.ItemSize = new Size(0, 1);
        }
        private void KOTHManager_Load(object sender, EventArgs e)
        {
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            doubleClickTimer.Interval = 100;
            doubleClickTimer.Tick += new EventHandler(doubleClickTimer_Tick);

            KOTHConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\KOTH\\" + currentproject.mpmissionpath.Split('.').Last() + ".json";
            KOTHLootPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\KOTH\\Loot.json";
            if (!File.Exists(KOTHConfigPath))
            {
                MDCKOTHConfig = new MDCKOTHConfig();
            }
            else
            {
                MDCKOTHConfig = JsonSerializer.Deserialize<MDCKOTHConfig>(File.ReadAllText(KOTHConfigPath));
                MDCKOTHConfig.isDirty = false;
            }
            MDCKOTHConfig.FullFilename = KOTHConfigPath;
            if(!File.Exists(KOTHLootPath))
            {
                MDCKOTHLoot = new MDCKOTHLoot();
            }
            else
            {
                MDCKOTHLoot = JsonSerializer.Deserialize<MDCKOTHLoot>(File.ReadAllText(KOTHLootPath));
                MDCKOTHLoot.isDirty = false;
            }
            MDCKOTHLoot.FullFilename = KOTHLootPath;

            SetupKingOfTheHillConfig();

            MapData = new MapData(Application.StartupPath + currentproject.MapPath + ".xyz");

            pictureBox2.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Livonia maop size is 12800 x 12800, 0,0 bottom left, center 6400 x 6400
            pictureBox2.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox2.Paint += new PaintEventHandler(DrawHillZones);
            trackBar4.Value = 1;
            SetHillZonescale();
            setupHillZombies(ZombiestochoosefromLB);
            setupHillZombies(ZoneenemiesListLB);
        }
        private void SetupKingOfTheHillConfig()
        {
            useraction = false;

            enabledCB.Checked = MDCKOTHConfig.enabled == 1 ? true : false;
            loggingLevelNUD.Value = MDCKOTHConfig.loggingLevel;
            useLocationTextCB.Checked = MDCKOTHConfig.useLocationText == 1 ? true : false;
            useMapMarkerCB.Checked = MDCKOTHConfig.useMapMarker == 1 ? true : false;
            useNotificationsCB.Checked = MDCKOTHConfig.useNotifications == 1 ? true : false;
            reduceProgressOnAbandonedCB.Checked = MDCKOTHConfig.reduceProgressOnAbandoned == 1 ? true : false;
            reduceProgressOnDeathFromOutsideCB.Checked = MDCKOTHConfig.reduceProgressOnDeathFromOutside == 1 ? true : false;
            requireFlagConstructionCB.Checked = MDCKOTHConfig.requireFlagConstruction == 1 ? true : false;
            estimateLocationCB.Checked = MDCKOTHConfig.estimateLocation == 1 ? true : false;
            celebrateWinCB.Checked = MDCKOTHConfig.celebrateWin == 1 ? true : false;
            punishLossCB.Checked = MDCKOTHConfig.punishLoss == 1 ? true : false;
            baseCaptureTimeNUD.Value = MDCKOTHConfig.baseCaptureTime;
            maxTimeBetweenEventsNUD.Value = MDCKOTHConfig.maxTimeBetweenEvents;
            minTimeBetweenEventsNUD.Value = MDCKOTHConfig.minTimeBetweenEvents;
            playerTimeMultiplierNUD.Value = MDCKOTHConfig.playerTimeMultiplier;
            timeDespawnNUD.Value = MDCKOTHConfig.timeDespawn;
            timeLimitNUD.Value = MDCKOTHConfig.timeLimit;
            timeStartNUD.Value = MDCKOTHConfig.timeStart;
            timeSpawnNUD.Value = MDCKOTHConfig.timeSpawn;
            timeZoneCooldownNUD.Value = MDCKOTHConfig.timeZoneCooldown;

            minPlayerCountNUD.Value = MDCKOTHConfig.minPlayerCount;
            maxEnemyCountNUD.Value = MDCKOTHConfig.maxEnemyCount;
            minEnemyCountNUD.Value = MDCKOTHConfig.minEnemyCount;
            maxEventsNUD.Value = MDCKOTHConfig.maxEvents;
            minimumDeathsNUD.Value = MDCKOTHConfig.minimumDeaths;
            minimumPlayersNUD.Value = MDCKOTHConfig.minimumPlayers;
            maximumPlayersNUD.Value = MDCKOTHConfig.maximumPlayers;
            rewardCountNUD.Value = MDCKOTHConfig.rewardCount;

            flagClassnameTB.Text = MDCKOTHConfig.flagClassname;
            lootCrateTB.Text = MDCKOTHConfig.lootCrate;
            crateLifeTimeNUD.Value = MDCKOTHConfig.crateLifeTime;
            ZombiesClassNamesLB.DataSource = MDCKOTHConfig.enemies;
            HillsLB.DataSource = MDCKOTHConfig.zones;
            LootSetsLB.DataSource = MDCKOTHLoot.lootSets;

            useraction = true;
        }
        
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\KOTH");
        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            SaveKOTHZoneconfigs();
        }
        private void SaveKOTHZoneconfigs()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (MDCKOTHConfig.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(MDCKOTHConfig.FullFilename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(MDCKOTHConfig.FullFilename) + "\\Backup\\" + SaveTime);
                    File.Copy(MDCKOTHConfig.FullFilename, Path.GetDirectoryName(MDCKOTHConfig.FullFilename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(MDCKOTHConfig.FullFilename) + ".bak", true);
                }
                MDCKOTHConfig.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(MDCKOTHConfig, options);
                File.WriteAllText(MDCKOTHConfig.FullFilename, jsonString);
                midifiedfiles.Add(Path.GetFileName(Path.GetFileName(KOTHConfigPath)));
            }
            if (MDCKOTHLoot.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(MDCKOTHLoot.FullFilename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(MDCKOTHLoot.FullFilename) + "\\Backup\\" + SaveTime);
                    File.Copy(MDCKOTHLoot.FullFilename, Path.GetDirectoryName(MDCKOTHLoot.FullFilename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(MDCKOTHLoot.FullFilename) + ".bak", true);
                }
                MDCKOTHLoot.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(MDCKOTHLoot, options);
                File.WriteAllText(MDCKOTHLoot.FullFilename, jsonString);
                midifiedfiles.Add(Path.GetFileName(Path.GetFileName(KOTHLootPath)));
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
        private void KOTHManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool needtosave = false;
            if (MDCKOTHConfig.isDirty)
            {
                needtosave = true;
            }
            if(MDCKOTHLoot.isDirty)
            {
                needtosave = true;
            }
            if (needtosave)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SaveKOTHZoneconfigs();
                }
            }
        }

        /// <summary>
        /// general Config Settings value changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private Point _mouseLastPosition;
        private Point _newscrollPosition;
        private Rectangle doubleClickRectangle = new Rectangle();
        private Timer doubleClickTimer = new Timer();
        private bool isFirstClick = true;
        private bool isDoubleClick = false;
        private int milliseconds = 0;
        private MouseEventArgs mouseeventargs;

        private void minTimeBetweenEventsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.minTimeBetweenEvents = minTimeBetweenEventsNUD.Value;
            MDCKOTHConfig.isDirty = true;
        }
        private void playerTimeMultiplierNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.playerTimeMultiplier = playerTimeMultiplierNUD.Value;
            MDCKOTHConfig.isDirty = true;
        }
        private void maxTimeBetweenEventsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.maxTimeBetweenEvents = maxTimeBetweenEventsNUD.Value;
            MDCKOTHConfig.isDirty = true;
        }
        private void timeDespawnNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.timeDespawn = timeDespawnNUD.Value;
            MDCKOTHConfig.isDirty = true;
        }
        private void timeLimitNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.timeLimit = timeLimitNUD.Value;
            MDCKOTHConfig.isDirty = true;
        }
        private void timeStartNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.timeStart = timeStartNUD.Value;
            MDCKOTHConfig.isDirty = true;
        }
        private void loggingLevelNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.loggingLevel = (int)loggingLevelNUD.Value;
            MDCKOTHConfig.isDirty = true;
        }
        private void baseCaptureTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.baseCaptureTime = baseCaptureTimeNUD.Value;
            MDCKOTHConfig.isDirty = true;
        }
        private void enabledCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.enabled = enabledCB.Checked == true ? 1 : 0;
            MDCKOTHConfig.isDirty = true;
        }
        private void useLocationTextCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.useLocationText = useLocationTextCB.Checked == true ? 1 : 0;
            MDCKOTHConfig.isDirty = true;
        }
        private void useMapMarkerCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.useMapMarker = useMapMarkerCB.Checked == true ? 1 : 0;
            MDCKOTHConfig.isDirty = true;
        }
        private void useNotificationsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.useNotifications = useNotificationsCB.Checked == true ? 1 : 0;
            MDCKOTHConfig.isDirty = true;
        }
        private void reduceProgressOnAbandonedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.reduceProgressOnAbandoned = reduceProgressOnAbandonedCB.Checked == true ? 1 : 0;
            MDCKOTHConfig.isDirty = true;
        }
        private void reduceProgressOnDeathFromOutsideCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.reduceProgressOnDeathFromOutside = reduceProgressOnDeathFromOutsideCB.Checked == true ? 1 : 0;
            MDCKOTHConfig.isDirty = true;
        }
        private void celebrateWinCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.celebrateWin = celebrateWinCB.Checked == true ? 1 : 0;
            MDCKOTHConfig.isDirty = true;
        }
        private void punishLossCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.punishLoss = punishLossCB.Checked == true ? 1 : 0;
            MDCKOTHConfig.isDirty = true;
        }
        private void requireFlagConstructionCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.requireFlagConstruction = requireFlagConstructionCB.Checked == true ? 1 : 0;
            MDCKOTHConfig.isDirty = true;
        }
        private void estimateLocationCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.estimateLocation = estimateLocationCB.Checked == true ? 1 : 0;
            MDCKOTHConfig.isDirty = true;
        }
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
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
                    if (currentKOTHZoneAreaLocation == null) return;
                    //if (e is MouseEventArgs mouseEventArgs)
                    //{
                    Cursor.Current = Cursors.WaitCursor;
                    decimal scalevalue = ZoneScale * (decimal)0.05;
                    decimal mapsize = currentproject.MapSize;
                    int newsize = (int)(mapsize * scalevalue);
                    currentKOTHZoneAreaLocation.zonePosition[0] = (float)Decimal.Round((decimal)(mouseeventargs.X / scalevalue), 4);
                    currentKOTHZoneAreaLocation.zonePosition[2] = (float)Decimal.Round((decimal)((newsize - mouseeventargs.Y) / scalevalue), 4);
                    if (MapData.FileExists)
                    {
                        ZoneYNUD.Value = (decimal)(MapData.gethieght(currentKOTHZoneAreaLocation.zonePosition[0], currentKOTHZoneAreaLocation.zonePosition[2]));
                    }
                    Cursor.Current = Cursors.Default;
                    MDCKOTHConfig.isDirty = true;
                    pictureBox2.Invalidate();
                    //}
                }
                else
                {
                    //Console.WriteLine("Perform single click action");
                    if (currentKOTHZoneAreaLocation == null) return;
                    decimal scalevalue = ZoneScale * (decimal)0.05;
                    decimal mapsize = currentproject.MapSize;
                    int newsize = (int)(mapsize * scalevalue);
                    PointF pC = new PointF((float)Decimal.Round((decimal)(mouseeventargs.X / scalevalue), 4), (float)Decimal.Round((decimal)((newsize - mouseeventargs.Y) / scalevalue), 4));
                    foreach (MDCKOTHZones tz in MDCKOTHConfig.zones)
                    {
                        PointF pP = new PointF(tz.zonePosition[0], tz.zonePosition[2]);
                        if (IsWithinCircle(pC, pP, (float)tz.zoneRadius))
                        {
                            HillsLB.SelectedItem = tz;
                            HillsLB.Refresh();
                            continue;
                        }
                    }
                    //}
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
        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            if (pictureBox2.Focused == false)
            {
                pictureBox2.Focus();
                panel2.AutoScrollPosition = _newscrollPosition;
                pictureBox2.Invalidate();
            }
        }
        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point changePoint = new Point(e.Location.X - _mouseLastPosition.X, e.Location.Y - _mouseLastPosition.Y);
                _newscrollPosition = new Point(-panel2.AutoScrollPosition.X - changePoint.X, -panel2.AutoScrollPosition.Y - changePoint.Y);
                if (_newscrollPosition.X <= 0)
                    _newscrollPosition.X = 0;
                if (_newscrollPosition.Y <= 0)
                    _newscrollPosition.Y = 0;
                panel2.AutoScrollPosition = _newscrollPosition;
                pictureBox2.Invalidate();
            }
            decimal scalevalue = ZoneScale * (decimal)0.05;
            decimal mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            label1.Text = Decimal.Round((decimal)(e.X / scalevalue), 4) + "," + Decimal.Round((decimal)((newsize - e.Y) / scalevalue), 4);
        }
        private void pictureBox2_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                pictureBox2_ZoomOut();
            }
            else
            {
                pictureBox2_ZoomIn();
            }

        }
        private void pictureBox2_ZoomIn()
        {
            int oldpictureboxhieght = pictureBox2.Height;
            int oldpitureboxwidht = pictureBox2.Width;
            Point oldscrollpos = panel2.AutoScrollPosition;
            int tbv = trackBar4.Value;
            int newval = tbv + 1;
            if (newval >= 20)
                newval = 20;
            trackBar4.Value = newval;
            ZoneScale = trackBar4.Value;
            SetHillZonescale();
            if (pictureBox2.Height > panel2.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox2.Height * newy);
                _newscrollPosition.Y = y * -1;
                panel2.AutoScrollPosition = _newscrollPosition;
            }
            if (pictureBox2.Width > panel2.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox2.Width * newy);
                _newscrollPosition.X = x * -1;
                panel2.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox2.Invalidate();
        }
        private void pictureBox2_ZoomOut()
        {
            int oldpictureboxhieght = pictureBox2.Height;
            int oldpitureboxwidht = pictureBox2.Width;
            Point oldscrollpos = panel2.AutoScrollPosition;
            int tbv = trackBar4.Value;
            int newval = tbv - 1;
            if (newval <= 1)
                newval = 1;
            trackBar4.Value = newval;
            ZoneScale = trackBar4.Value;
            SetHillZonescale();
            if (pictureBox2.Height > panel2.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox2.Height * newy);
                _newscrollPosition.Y = y * -1;
                panel2.AutoScrollPosition = _newscrollPosition;
            }
            if (pictureBox2.Width > panel2.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox2.Width * newy);
                _newscrollPosition.X = x * -1;
                panel2.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox2.Invalidate();
        }
        private void timeSpawnNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.timeSpawn = (int)timeSpawnNUD.Value;
            MDCKOTHConfig.isDirty = true;
        }
        private void timeZoneCooldownNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.timeZoneCooldown = (int)timeZoneCooldownNUD.Value;
            MDCKOTHConfig.isDirty = true;
        }
        private void minPlayerCountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.minPlayerCount = (int)minPlayerCountNUD.Value;
            MDCKOTHConfig.isDirty = true;
        }
        private void maxEnemyCountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.maxEnemyCount = (int)maxEnemyCountNUD.Value;
            MDCKOTHConfig.isDirty = true;
        }
        private void minEnemyCountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.minEnemyCount = (int)minEnemyCountNUD.Value;
            MDCKOTHConfig.isDirty = true;

        }
        private void maxEventsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.maxEvents = (int)maxEventsNUD.Value;
            MDCKOTHConfig.isDirty = true;
        }
        private void minimumDeathsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.minimumDeaths = (int)minimumDeathsNUD.Value;
            MDCKOTHConfig.isDirty = true;
        }
        private void minimumPlayersNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.minimumPlayers = (int)minimumPlayersNUD.Value;
            MDCKOTHConfig.isDirty = true;
        }
        private void maximumPlayersNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.maximumPlayers = (int)maximumPlayersNUD.Value;
            MDCKOTHConfig.isDirty = true;
        }
        private void rewardCountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.timeDespawn = timeDespawnNUD.Value;
            MDCKOTHConfig.isDirty = true;
        }
        private void flagClassnameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.flagClassname = flagClassnameTB.Text;
            MDCKOTHConfig.isDirty = true;
        }
        private void lootCrateTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.lootCrate = lootCrateTB.Text;
            MDCKOTHConfig.isDirty = true;
        }
        private void crateLifeTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            MDCKOTHConfig.crateLifeTime = (int)crateLifeTimeNUD.Value;
            MDCKOTHConfig.isDirty = true;
        }

        /// <summary>
        /// Hill Config Value Changes and Add remove Hills
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HillsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (HillsLB.SelectedItems.Count < 1) return;
            tabControl2.Visible = true;
            currentKOTHZoneAreaLocation = HillsLB.SelectedItem as MDCKOTHZones;
            useraction = false;
            ZoneNameTB.Text = currentKOTHZoneAreaLocation.zoneName;
            ZoneXNUD.Value = (decimal)currentKOTHZoneAreaLocation.zonePosition[0];
            ZoneYNUD.Value = (decimal)currentKOTHZoneAreaLocation.zonePosition[1];
            ZoneZNUD.Value = (decimal)currentKOTHZoneAreaLocation.zonePosition[2];
            ZoneRadiusNUD.Value = currentKOTHZoneAreaLocation.zoneRadius;
            ZonebaseCaptureTimeNUD.Value = currentKOTHZoneAreaLocation.baseCaptureTime == -1 ? MDCKOTHConfig.baseCaptureTime : currentKOTHZoneAreaLocation.baseCaptureTime;
            ZoneplayerTimeMultiplierNUD.Value = currentKOTHZoneAreaLocation.playerTimeMultiplier == -1 ? MDCKOTHConfig.playerTimeMultiplier : currentKOTHZoneAreaLocation.playerTimeMultiplier;
            ZonetimeDespawnNUD.Value = currentKOTHZoneAreaLocation.timeDespawn == -1 ? MDCKOTHConfig.timeDespawn : currentKOTHZoneAreaLocation.timeDespawn;
            ZonetimeLimitNUD.Value = currentKOTHZoneAreaLocation.timeLimit == -1 ? MDCKOTHConfig.timeLimit : currentKOTHZoneAreaLocation.timeLimit;
            ZonetimeStartNUD.Value = currentKOTHZoneAreaLocation.timeStart == -1 ? MDCKOTHConfig.timeStart : currentKOTHZoneAreaLocation.timeStart;
            ZonemaxEnemyCountNUD.Value = (int)currentKOTHZoneAreaLocation.maxEnemyCount == -1 ? MDCKOTHConfig.maxEnemyCount : (int)currentKOTHZoneAreaLocation.maxEnemyCount;
            ZoneminEnemyCountNUD.Value = (int)currentKOTHZoneAreaLocation.minEnemyCount == -1 ? MDCKOTHConfig.minEnemyCount : (int)currentKOTHZoneAreaLocation.minEnemyCount;
            ZoneminimumDeathsNUD.Value = (int)currentKOTHZoneAreaLocation.minimumDeaths == -1 ? MDCKOTHConfig.minimumDeaths : (int)currentKOTHZoneAreaLocation.minimumDeaths;
            ZoneminimumPlayersNUD.Value = (int)currentKOTHZoneAreaLocation.minimumPlayers == -1 ? MDCKOTHConfig.minimumPlayers : (int)currentKOTHZoneAreaLocation.minimumPlayers;
            ZonemaximumPlayersNUD.Value = (int)currentKOTHZoneAreaLocation.maximumPlayers == -1 ? MDCKOTHConfig.maximumPlayers : (int)currentKOTHZoneAreaLocation.maximumPlayers;
            ZonerewardCountNUD.Value = (int)currentKOTHZoneAreaLocation.rewardCount == -1 ? MDCKOTHConfig.rewardCount : (int)currentKOTHZoneAreaLocation.rewardCount;
            ZoneflagClassnameTB.Text = currentKOTHZoneAreaLocation.flagClassname == "" ? MDCKOTHConfig.flagClassname : currentKOTHZoneAreaLocation.flagClassname;
            ZonelootCrateTB.Text = currentKOTHZoneAreaLocation.lootCrate == "" ? MDCKOTHConfig.lootCrate : currentKOTHZoneAreaLocation.lootCrate;
            ZonecrateLifeTimeNUD.Value = (int)currentKOTHZoneAreaLocation.crateLifeTime == -1 ? MDCKOTHConfig.crateLifeTime : (int)currentKOTHZoneAreaLocation.crateLifeTime;
            ZoneenemiesLB.DataSource = currentKOTHZoneAreaLocation.enemies;
            ZoneObjectsLB.DataSource = currentKOTHZoneAreaLocation.objects;
            ZoneLootSetTV.Nodes.Clear();
            ZoneLootSetsLB.DataSource = currentKOTHZoneAreaLocation.lootSets;
            if (ZoneObjectsLB.SelectedItems.Count < 1)
            {
                useraction = false;
                currentKOTHillZOneObjects = null;
                ZoneObjectclassnameTB.Text = "";
                ZoneObjectPosXNUD.Value = 0;
                ZoneObjectPosYNUD.Value = 0;
                ZoneObjectPosZNUD.Value = 0;
                ZoneObjectOriXNUD.Value = 0;
                ZoneObjectOriYNUD.Value = 0;
                ZoneObjectOriZNUD.Value = 0;
                ZoneObjectabsolutePlacementCB.Checked = false;
                ZoneObjectalignToTerrainCB.Checked = false;
                ZoneObjectplaceOnSurfaceCB.Checked = false;
            }
            pictureBox2.Invalidate();
            useraction = true;
        }
        private void darkButton12_Click(object sender, EventArgs e)
        {
            float[] centre = new float[] { currentproject.MapSize / 2, 0, currentproject.MapSize / 2 };
            MDCKOTHZones newzone = new MDCKOTHZones()
            {
                zoneName = "New Zone",
                zonePosition = centre,
                zoneRadius = 50,
                baseCaptureTime = -1,
                playerTimeMultiplier = -1,
                timeDespawn = -1,
                timeStart = -1,
                maxEnemyCount = -1,
                minEnemyCount = -1,
                minimumDeaths = -1,
                minimumPlayers = -1,
                maximumPlayers = -1,
                rewardCount = -1,
                flagClassname = "",
                objects = new BindingList<KOTHObject>(),
                enemies = new BindingList<string>(),
                lootCrate = "",
                crateLifeTime = -1,
                lootSets = new BindingList<KOTHLootset>()
            };
            MDCKOTHConfig.zones.Add(newzone);
            MDCKOTHConfig.isDirty = true;
            pictureBox2.Invalidate();
            if (MDCKOTHConfig.zones.Count == 1)
            {
                HillsLB.SelectedIndex = -1;
                HillsLB.SelectedIndex = 0;
            }
            tabControl2.Visible = true;
        }
        private void darkButton11_Click(object sender, EventArgs e)
        {
            int index = HillsLB.SelectedIndex;
            List<MDCKOTHZones> removeKOTHZones = new List<MDCKOTHZones>();
            foreach (var item in HillsLB.SelectedItems)
            {
                removeKOTHZones.Add(item as MDCKOTHZones);
            }
            foreach (MDCKOTHZones KOTHz in removeKOTHZones)
            {
                MDCKOTHConfig.zones.Remove(KOTHz);
                MDCKOTHConfig.isDirty = true;
            }
            if (HillsLB.SelectedIndex != -1)
            {
                HillsLB.SelectedIndex = -1;
                int newindex = index - 1;
                if (newindex == -1)
                    HillsLB.SelectedIndex = 0;
                else
                    HillsLB.SelectedIndex = index - 1;
            }
            else
            {
                tabControl2.Visible = false;
            }
            pictureBox2.Invalidate();
        }
        private void ZobneenemiesAddBT_Click(object sender, EventArgs e)
        {
            foreach (var item in ZoneenemiesListLB.SelectedItems)
            {
                string zombie = ZoneenemiesListLB.GetItemText(ZoneenemiesListLB.SelectedItem);
                if (!currentKOTHZoneAreaLocation.enemies.Contains(zombie))
                    currentKOTHZoneAreaLocation.enemies.Add(zombie);
            }
            MDCKOTHConfig.isDirty = true;
        }
        private void ZoneenemiesRemoveBT_Click(object sender, EventArgs e)
        {
            int index = ZoneenemiesLB.SelectedIndex;

            List<string> removezombies = new List<string>();
            foreach (var item in ZoneenemiesLB.SelectedItems)
            {
                removezombies.Add(item.ToString());
            }
            foreach (string s in removezombies)
            {
                currentKOTHZoneAreaLocation.enemies.Remove(s);
            }
            if (ZoneenemiesLB.SelectedIndex != -1)
            {
                ZoneenemiesLB.SelectedIndex = -1;
                int newindex = index - 1;
                if (newindex == -1)
                    ZoneenemiesLB.SelectedIndex = 0;
                else
                    ZoneenemiesLB.SelectedIndex = index - 1;
            }
            MDCKOTHConfig.isDirty = true;
        }
        private void ZoneNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHZoneAreaLocation.zoneName = ZoneNameTB.Text;
            MDCKOTHConfig.isDirty = true;
            HillsLB.Refresh();
        }
        private void XNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHZoneAreaLocation.zonePosition[0] = (float)ZoneObjectPosXNUD.Value;
            MDCKOTHConfig.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void YNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHZoneAreaLocation.zonePosition[1] = (float)ZoneObjectPosYNUD.Value;
            MDCKOTHConfig.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void ZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHZoneAreaLocation.zonePosition[2] = (float)ZoneObjectPosZNUD.Value;
            MDCKOTHConfig.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void ZoneRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHZoneAreaLocation.zoneRadius = (int)ZoneRadiusNUD.Value;
            MDCKOTHConfig.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void ZonebaseCaptureTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHZoneAreaLocation.baseCaptureTime = ZonebaseCaptureTimeNUD.Value == MDCKOTHConfig.baseCaptureTime ? -1 : ZonebaseCaptureTimeNUD.Value;
            if (ZonebaseCaptureTimeNUD.Value == -1)
                ZonebaseCaptureTimeNUD.Value = MDCKOTHConfig.baseCaptureTime;
            MDCKOTHConfig.isDirty = true;
        }
        private void ZoneplayerTimeMultiplierNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHZoneAreaLocation.playerTimeMultiplier = ZoneplayerTimeMultiplierNUD.Value == MDCKOTHConfig.playerTimeMultiplier ? -1 : ZoneplayerTimeMultiplierNUD.Value;
            if (ZoneplayerTimeMultiplierNUD.Value == -1)
                ZoneplayerTimeMultiplierNUD.Value = MDCKOTHConfig.playerTimeMultiplier;
            MDCKOTHConfig.isDirty = true;
        }
        private void ZonetimeDespawnNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHZoneAreaLocation.timeDespawn = ZonetimeDespawnNUD.Value == MDCKOTHConfig.timeDespawn ? -1 : ZonetimeDespawnNUD.Value;
            if (ZonetimeDespawnNUD.Value == -1)
                ZonetimeDespawnNUD.Value = MDCKOTHConfig.timeDespawn;
            MDCKOTHConfig.isDirty = true;
        }
        private void ZonetimeLimitNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHZoneAreaLocation.timeLimit = ZonetimeLimitNUD.Value == MDCKOTHConfig.timeLimit ? -1 : ZonetimeLimitNUD.Value;
            if (ZonetimeLimitNUD.Value == -1)
                ZonetimeLimitNUD.Value = MDCKOTHConfig.timeLimit;
            MDCKOTHConfig.isDirty = true;
        }
        private void ZonetimeStartNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHZoneAreaLocation.timeStart = ZonetimeStartNUD.Value == MDCKOTHConfig.timeStart ? -1 : ZonetimeStartNUD.Value;
            if (ZonetimeStartNUD.Value == -1)
                ZonetimeStartNUD.Value = MDCKOTHConfig.timeStart;
            MDCKOTHConfig.isDirty = true;
        }
        private void ZoneminEnemyCountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHZoneAreaLocation.minEnemyCount = (int)ZoneminEnemyCountNUD.Value == MDCKOTHConfig.minEnemyCount ? -1 : (int)ZoneminEnemyCountNUD.Value;
            if ((int)ZoneminEnemyCountNUD.Value == -1)
                ZoneminEnemyCountNUD.Value = MDCKOTHConfig.minEnemyCount;
            MDCKOTHConfig.isDirty = true;
        }
        private void ZonemaxEnemyCountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHZoneAreaLocation.maxEnemyCount = (int)ZonemaxEnemyCountNUD.Value == MDCKOTHConfig.maxEnemyCount ? -1 : (int)ZonemaxEnemyCountNUD.Value;
            if ((int)ZonemaxEnemyCountNUD.Value == -1)
                ZonemaxEnemyCountNUD.Value = MDCKOTHConfig.maxEnemyCount;
            MDCKOTHConfig.isDirty = true;
        }
        private void ZoneminimumDeathsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHZoneAreaLocation.minimumDeaths = (int)ZoneminimumDeathsNUD.Value == MDCKOTHConfig.minimumDeaths ? -1 : (int)ZoneminimumDeathsNUD.Value;
            if ((int)ZoneminimumDeathsNUD.Value == -1)
                ZoneminimumDeathsNUD.Value = MDCKOTHConfig.minimumDeaths;
            MDCKOTHConfig.isDirty = true;
        }
        private void ZoneminimumPlayersNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHZoneAreaLocation.minimumPlayers = (int)ZoneminimumPlayersNUD.Value == MDCKOTHConfig.minimumPlayers ? -1 : (int)ZoneminimumPlayersNUD.Value;
            if ((int)ZoneminimumPlayersNUD.Value == -1)
                ZoneminimumPlayersNUD.Value = MDCKOTHConfig.minimumPlayers;
            MDCKOTHConfig.isDirty = true;
        }
        private void ZonemaximumPlayersNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHZoneAreaLocation.maximumPlayers = (int)ZonemaximumPlayersNUD.Value == MDCKOTHConfig.maximumPlayers ? -1 : (int)ZonemaximumPlayersNUD.Value;
            if ((int)ZonemaximumPlayersNUD.Value == -1)
                ZonemaximumPlayersNUD.Value = MDCKOTHConfig.maximumPlayers;
            MDCKOTHConfig.isDirty = true;
        }
        private void ZonerewardCountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHZoneAreaLocation.rewardCount = (int)ZonerewardCountNUD.Value == MDCKOTHConfig.rewardCount ? -1 : (int)ZonerewardCountNUD.Value;
            if ((int)ZonerewardCountNUD.Value == -1)
                ZonerewardCountNUD.Value = MDCKOTHConfig.rewardCount;
            MDCKOTHConfig.isDirty = true;
        }
        private void ZoneflagClassnameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHZoneAreaLocation.flagClassname = ZoneflagClassnameTB.Text == MDCKOTHConfig.flagClassname ? "" : ZoneflagClassnameTB.Text;
            if (ZoneflagClassnameTB.Text == "")
                ZoneflagClassnameTB.Text = MDCKOTHConfig.flagClassname;
            MDCKOTHConfig.isDirty = true;
        }
        private void ZonelootCrateTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHZoneAreaLocation.lootCrate = ZonelootCrateTB.Text == MDCKOTHConfig.lootCrate ? "" : ZonelootCrateTB.Text;
            if (ZonelootCrateTB.Text == "")
                ZonelootCrateTB.Text = MDCKOTHConfig.lootCrate;
            MDCKOTHConfig.isDirty = true;
        }
        private void ZonecrateLifeTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHZoneAreaLocation.crateLifeTime = (int)ZonecrateLifeTimeNUD.Value;
            MDCKOTHConfig.isDirty = true;
        }
        private void darkButton9_Click(object sender, EventArgs e)
        {
            string[] fileContent = new string[] { };
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    DZE importfile = DZEHelpers.LoadFile(filePath);
                    DialogResult dialogResult = MessageBox.Show("Clear All Exisitng Flags?", "Clear Flags", MessageBoxButtons.YesNoCancel);
                    if (dialogResult == DialogResult.Cancel)
                    {
                        return;
                    }
                    else if (dialogResult == DialogResult.Yes)
                    {
                        MDCKOTHConfig.zones.Clear();
                    }
                    MDCKOTHZones newzone = new MDCKOTHZones();
                    foreach (Editorobject eo in importfile.EditorObjects)
                    {
                        if (eo.Type == "KOTH_Flag")
                        {
                            newzone.zoneName = Path.GetFileNameWithoutExtension(filePath);
                            newzone.zonePosition = eo.Position;
                        }
                        else
                        {
                            KOTHObject newobject = new KOTHObject()
                            {
                                classname = eo.Type,
                                position = new decimal[] { (decimal)eo.Position[0], (decimal)eo.Position[1], (decimal)eo.Position[2] },
                                orientation = new decimal[] { (decimal)eo.Orientation[0], (decimal)eo.Orientation[1], (decimal)eo.Orientation[2] },
                                absolutePlacement = 1,
                                alignToTerrain = 0,
                                placeOnSurface = 0
                            };
                            newzone.objects.Add(newobject);
                        }
                    }
                    MDCKOTHConfig.zones.Add(newzone);
                    MDCKOTHConfig.isDirty = true;
                    pictureBox2.Invalidate();
                    if (MDCKOTHConfig.zones.Count == 1)
                    {
                        HillsLB.SelectedIndex = -1;
                        HillsLB.SelectedIndex = 0;
                    }
                    tabControl2.Visible = true;
                }
            }
        }
        private void darkButton10_Click(object sender, EventArgs e)
        {
            string[] fileContent = new string[] { };
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    DZE importfile = DZEHelpers.LoadFile(filePath);
                    DialogResult dialogResult = MessageBox.Show("Clear All Exisitng Flags?", "Clear Flags", MessageBoxButtons.YesNoCancel);
                    if (dialogResult == DialogResult.Cancel)
                    {
                        return;
                    }
                    else if (dialogResult == DialogResult.Yes)
                    {
                        MDCKOTHConfig.zones.Clear();
                    }
                    int i = 1;
                    foreach (Editorobject eo in importfile.EditorObjects)
                    {
                        MDCKOTHZones newzone = new MDCKOTHZones();
                        if (eo.Type == "KOTH_Flag")
                        {
                            newzone.zoneName = "Zone " + i.ToString();
                            i++;
                            newzone.zonePosition = eo.Position;
                            MDCKOTHConfig.zones.Add(newzone);
                        }
                    }
                    
                    MDCKOTHConfig.isDirty = true;
                    pictureBox2.Invalidate();
                    if (MDCKOTHConfig.zones.Count == 1)
                    {
                        HillsLB.SelectedIndex = -1;
                        HillsLB.SelectedIndex = 0;
                    }
                    tabControl2.Visible = true;
                }
            }
        }

        /// <summary>
        /// Zone Lootsets
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public KOTHLootset currentZonelootset;
        public object CurrentZoneTreeNodeTag;
        public KOTHLootset currentZoneLootSet;
        public KOTHItem currentZoneLootitem;
        private void ZoneLootSetsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZoneLootSetTV.Nodes.Clear();
            if (ZoneLootSetsLB.SelectedItems.Count < 1) return;
            
            useraction = false;
            currentZonelootset = ZoneLootSetsLB.SelectedItem as KOTHLootset;
            SetupLootTreeview(ZoneLootSetTV, currentZonelootset);
            useraction = true;
        }
        private void darkButton14_Click(object sender, EventArgs e)
        {
            currentKOTHZoneAreaLocation.lootSets.Add(new KOTHLootset()
            {
                name = "New Loot Set",
                items = new BindingList<KOTHItem>()
            });
            MDCKOTHConfig.isDirty = true;
            if (currentKOTHZoneAreaLocation.lootSets.Count == 1)
            {
                ZoneLootSetsLB.SelectedIndex = -1;
                ZoneLootSetsLB.SelectedIndex = 0;
            }
        }
        private void darkButton13_Click(object sender, EventArgs e)
        {
            int index = ZoneLootSetsLB.SelectedIndex;

            List<KOTHLootset> removelootsets = new List<KOTHLootset>();
            foreach (var item in ZoneLootSetsLB.SelectedItems)
            {
                removelootsets.Add(item as KOTHLootset);
            }
            foreach (KOTHLootset Zonels in removelootsets)
            {
                currentKOTHZoneAreaLocation.lootSets.Remove(Zonels);
                MDCKOTHConfig.isDirty = true;
            }
            if (ZoneLootSetsLB.SelectedIndex != -1)
            {
                ZoneLootSetsLB.SelectedIndex = -1;
                int newindex = index - 1;
                if (newindex == -1)
                    ZoneLootSetsLB.SelectedIndex = 0;
                else
                    ZoneLootSetsLB.SelectedIndex = index - 1;
            }

        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
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
                    KOTHItem newkothattachment = new KOTHItem()
                    {
                        name = l,
                        quantity = -1,
                        attachments = new BindingList<KOTHItem>(),
                        cargo = new BindingList<KOTHItem>()
                    };
                    ZoneLootSetTV.SelectedNode.Nodes.Add(Kothitems(newkothattachment));
                    ZoneLootSetTV.SelectedNode.Expand();
                    currentZoneLootitem.attachments.Add(newkothattachment);
                    MDCKOTHConfig.isDirty = true;
                }
            }
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            KOTHItem parent = ZoneLootSetTV.SelectedNode.Parent.Parent.Tag as KOTHItem;
            parent.attachments.Remove(currentZoneLootitem);
            ZoneLootSetTV.SelectedNode.Remove();
            MDCKOTHConfig.isDirty = true;
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
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
                    KOTHItem newkothcargo = new KOTHItem()
                    {
                        name = l,
                        quantity = -1,
                        attachments = new BindingList<KOTHItem>(),
                        cargo = new BindingList<KOTHItem>()
                    };
                    ZoneLootSetTV.SelectedNode.Nodes.Add(Kothitems(newkothcargo));
                    ZoneLootSetTV.SelectedNode.Expand();
                    currentZoneLootitem.cargo.Add(newkothcargo);
                    MDCKOTHConfig.isDirty = true;
                }
            }
        }
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            KOTHItem parent = ZoneLootSetTV.SelectedNode.Parent.Parent.Tag as KOTHItem;
            parent.cargo.Remove(currentZoneLootitem);
            ZoneLootSetTV.SelectedNode.Remove();
            MDCKOTHConfig.isDirty = true;
        }
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
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
                    KOTHItem newkothitem = new KOTHItem()
                    {
                        name = l,
                        quantity = -1,
                        attachments = new BindingList<KOTHItem>(),
                        cargo = new BindingList<KOTHItem>()
                    };
                    ZoneLootSetTV.SelectedNode.Nodes.Add(Kothitems(newkothitem));
                    ZoneLootSetTV.SelectedNode.Expand();
                    currentZonelootset.items.Add(newkothitem);
                    MDCKOTHConfig.isDirty = true;
                }
            }
        }
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            currentZonelootset.items.Remove(currentZoneLootitem);
            ZoneLootSetTV.SelectedNode.Remove();
            MDCKOTHConfig.isDirty = true;
        }
        private void ZoneLootSetTV_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            useraction = false;
            ZoneLootSetTV.SelectedNode = e.Node;
            CurrentZoneTreeNodeTag = e.Node.Tag;
            ZoneLootSetNameGB.Visible = false;
            ZoneLootSetQuantityGB.Visible = false;
            currentZoneLootSet = null;
            currentZoneLootitem = null;

            ZoneaddAttchmentToolStripMenuItem.Visible = false;
            ZoneremoveAttachmentToolStripMenuItem.Visible = false;
            ZoneaddCargoToolStripMenuItem.Visible = false;
            ZoneremoveCargoToolStripMenuItem.Visible = false;
            ZoneaddLootItemToolStripMenuItem.Visible = false;
            ZoneremoveLootItemToolStripMenuItem.Visible = false;

            if (e.Node.Tag is KOTHLootset)
            {
                currentZoneLootSet = e.Node.Tag as KOTHLootset;
                ZoneLootSetNameGB.Visible = true;
                darkButton15.Visible = false;
                ZoneLootitemNameTB.Text = currentZoneLootSet.name;
                if (e.Button == MouseButtons.Right)
                {
                    ZoneaddLootItemToolStripMenuItem.Visible = true;
                    contextMenuStrip2.Show(Cursor.Position);
                }
            }
            else if (e.Node.Tag is KOTHItem)
            {
                currentZoneLootitem = e.Node.Tag as KOTHItem;
                ZoneLootSetNameGB.Visible = true;
                darkButton15.Visible = true;
                ZoneLootitemNameTB.Text = currentZoneLootitem.name;
                if (e.Button == MouseButtons.Right)
                {
                    if (e.Node.Parent.Tag is KOTHLootset)
                        ZoneremoveLootItemToolStripMenuItem.Visible = true;
                    else
                    {
                        if (e.Node.Parent.Tag.ToString() == "Cargo")
                            ZoneremoveCargoToolStripMenuItem.Visible = true;
                        else if (e.Node.Parent.Tag.ToString() == "Attachments")
                            ZoneremoveAttachmentToolStripMenuItem.Visible = true;
                    }
                    contextMenuStrip2.Show(Cursor.Position);
                }
            }
            else if (e.Node.Tag is string)
            {
                switch (e.Node.Tag.ToString())
                {
                    case "Quantity":
                        currentZoneLootitem = e.Node.Parent.Tag as KOTHItem;
                        ZoneLootSetQuantityGB.Visible = true;
                        ZoneLootItemQuantityNUD.Value = currentZoneLootitem.quantity;
                        break;
                    case "Attachments":
                        if (e.Button == MouseButtons.Right)
                        {
                            currentZoneLootitem = e.Node.Parent.Tag as KOTHItem;
                            ZoneaddAttchmentToolStripMenuItem.Visible = true;
                            ZoneaddAttchmentToolStripMenuItem.Text = "Add Attachment";
                            contextMenuStrip2.Show(Cursor.Position);
                        }
                        break;
                    case "Cargo":
                        if (e.Button == MouseButtons.Right)
                        {
                            currentZoneLootitem = e.Node.Parent.Tag as KOTHItem;
                            ZoneaddCargoToolStripMenuItem.Visible = true;
                            contextMenuStrip2.Show(Cursor.Position);
                        }
                        break;
                }
            }

            useraction = true;
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentZoneLootitem.quantity = (int)ZoneLootItemQuantityNUD.Value;
            MDCKOTHConfig.isDirty = true;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;

            if (currentZoneLootSet != null && currentZoneLootitem == null)
            {
                currentZoneLootSet.name = ZoneLootSetTV.SelectedNode.Text = ZoneLootitemNameTB.Text;
                ZoneLootSetsLB.Refresh();
            }
            else if (currentZoneLootSet == null && currentZoneLootitem != null)
                currentZoneLootitem.name = ZoneLootSetTV.SelectedNode.Text = ZoneLootitemNameTB.Text;
            MDCKOTHConfig.isDirty = true;
        }
        private void darkButton15_Click(object sender, EventArgs e)
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
                    ZoneLootitemNameTB.Text = l;
                }
            }
        }
        /// <summary>
        /// ZOneObjects
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private KOTHObject currentKOTHillZOneObjects;
        private void ZoneObjectsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ZoneObjectsLB.SelectedItems.Count< 1) return;
            currentKOTHillZOneObjects = ZoneObjectsLB.SelectedItem as KOTHObject;
            useraction = false;
            ZoneObjectclassnameTB.Text = currentKOTHillZOneObjects.classname;
            ZoneObjectPosXNUD.Value = currentKOTHillZOneObjects.position[0];
            ZoneObjectPosYNUD.Value = currentKOTHillZOneObjects.position[1];
            ZoneObjectPosZNUD.Value = currentKOTHillZOneObjects.position[2];
            ZoneObjectOriXNUD.Value = currentKOTHillZOneObjects.orientation[0];
            ZoneObjectOriYNUD.Value = currentKOTHillZOneObjects.orientation[1];
            ZoneObjectOriZNUD.Value = currentKOTHillZOneObjects.orientation[2];
            ZoneObjectabsolutePlacementCB.Checked = currentKOTHillZOneObjects.absolutePlacement == 1 ? true : false;
            ZoneObjectalignToTerrainCB.Checked = currentKOTHillZOneObjects.alignToTerrain == 1 ? true : false;
            ZoneObjectplaceOnSurfaceCB.Checked = currentKOTHillZOneObjects.placeOnSurface == 1 ? true : false;
            useraction = true;
        }
        private void darkButton5_Click(object sender, EventArgs e)
        {
            if (ZoneObjectsLB.SelectedItems.Count < 1) return;

            int index = ZoneObjectsLB.SelectedIndex;

            List<KOTHObject> KOTHObject = new List<KOTHObject>();
            foreach (var item in ZoneObjectsLB.SelectedItems)
            {
                KOTHObject.Add(item as KOTHObject);
            }
            foreach (KOTHObject KO in KOTHObject)
            {
                currentKOTHZoneAreaLocation.objects.Remove(KO);
            }
            if (ZoneObjectsLB.SelectedIndex != -1)
            {
                ZoneObjectsLB.SelectedIndex = -1;
                int newindex = index - 1;
                if (newindex == -1)
                    ZoneObjectsLB.SelectedIndex = 0;
                else
                    ZoneObjectsLB.SelectedIndex = index - 1;
            }
            if (currentKOTHZoneAreaLocation.objects.Count ==0)
            {
                useraction = false;
                currentKOTHillZOneObjects = null;
                ZoneObjectclassnameTB.Text = "";
                ZoneObjectPosXNUD.Value = 0;
                ZoneObjectPosYNUD.Value = 0;
                ZoneObjectPosZNUD.Value = 0;
                ZoneObjectOriXNUD.Value = 0;
                ZoneObjectOriYNUD.Value = 0;
                ZoneObjectOriZNUD.Value = 0;
                ZoneObjectabsolutePlacementCB.Checked = false;
                ZoneObjectalignToTerrainCB.Checked = false;
                ZoneObjectplaceOnSurfaceCB.Checked = false;
                useraction = true;
            }
            MDCKOTHConfig.isDirty = true;
        }
        private void ZoneObjectclassnameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHillZOneObjects.classname = ZoneObjectclassnameTB.Text;
            MDCKOTHConfig.isDirty = true;
        }
        private void ZoneObjectPosXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHillZOneObjects.position[0] = ZoneObjectPosXNUD.Value;
            MDCKOTHConfig.isDirty = true;
        }
        private void ZoneObjectPosYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHillZOneObjects.position[1] = ZoneObjectPosYNUD.Value;
            MDCKOTHConfig.isDirty = true;
        }
        private void ZoneObjectPosZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHillZOneObjects.position[2] = ZoneObjectPosZNUD.Value;
            MDCKOTHConfig.isDirty = true;
        }
        private void ZoneObjectOriXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHillZOneObjects.orientation[0] = ZoneObjectOriXNUD.Value;
            MDCKOTHConfig.isDirty = true;
        }
        private void ZoneObjectOriYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHillZOneObjects.orientation[1] = ZoneObjectOriYNUD.Value;
            MDCKOTHConfig.isDirty = true;
        }
        private void ZoneObjectOriZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHillZOneObjects.orientation[2] = ZoneObjectOriZNUD.Value;
            MDCKOTHConfig.isDirty = true;
        }
        private void ZoneObjectabsolutePlacementCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHillZOneObjects.absolutePlacement = ZoneObjectabsolutePlacementCB.Checked == true ? 1 : 0;
            MDCKOTHConfig.isDirty = true;
        }
        private void ZoneObjectalignToTerrainCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHillZOneObjects.alignToTerrain = ZoneObjectalignToTerrainCB.Checked == true ? 1 : 0;
            MDCKOTHConfig.isDirty = true;
        }
        private void ZoneObjectplaceOnSurfaceCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentKOTHillZOneObjects.placeOnSurface = ZoneObjectplaceOnSurfaceCB.Checked == true ? 1 : 0;
            MDCKOTHConfig.isDirty = true;
        }
        private void darkButton6_Click(object sender, EventArgs e)
        {
            string[] fileContent = new string[] { };
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    DZE importfile = DZEHelpers.LoadFile(filePath);
                    DialogResult dialogResult = MessageBox.Show("Clear All Exisitng Objects?", "Clear Objects", MessageBoxButtons.YesNoCancel);
                    if (dialogResult == DialogResult.Cancel)
                    {
                        return;
                    }
                    else if (dialogResult == DialogResult.Yes)
                    {
                        currentKOTHZoneAreaLocation.objects.Clear();
                    }
                    foreach (Editorobject eo in importfile.EditorObjects)
                    {
                        KOTHObject newobject = new KOTHObject()
                        {
                            classname = eo.Type,
                            position = new decimal[] { (decimal)eo.Position[0], (decimal)eo.Position[1], (decimal)eo.Position[2] } ,
                            orientation = new decimal[] { (decimal)eo.Orientation[0], (decimal)eo.Orientation[1], (decimal)eo.Orientation[2] },
                            absolutePlacement = 1,
                            alignToTerrain  = 0,
                            placeOnSurface = 0
                        };
                        currentKOTHZoneAreaLocation.objects.Add(newobject);
                    }
                    MDCKOTHConfig.isDirty = true;
                }
            }
        }
        private void darkButton4_Click(object sender, EventArgs e)
        {
            KOTHObject newobject = new KOTHObject()
            {
                classname = "New Object",
                position = new decimal[] { 0,0,0 },
                orientation = new decimal[] {0,0,0 },
                absolutePlacement = 0,
                alignToTerrain = 0,
                placeOnSurface = 0
            };
            currentKOTHZoneAreaLocation.objects.Add(newobject);
            MDCKOTHConfig.isDirty = true;
        }
        /// <summary>
        /// Zombie List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setupHillZombies(ListBox enemiesLB)
        {
            enemiesLB.Items.Clear();
            foreach (typesType type in vanillatypes.SerachTypes("zmbm_"))
            {
                enemiesLB.Items.Add(type.name);
            }
            foreach (typesType type in vanillatypes.SerachTypes("zmbf_"))
            {
                enemiesLB.Items.Add(type.name);
            }
            foreach (typesType type in vanillatypes.SerachTypes("Animal_"))
            {
                enemiesLB.Items.Add(type.name);
            }
            foreach (TypesFile tf in ModTypes)
            {
                foreach (typesType type in tf.SerachTypes("zmbm_"))
                {
                    enemiesLB.Items.Add(type.name);
                }
            }
            foreach (TypesFile tf in ModTypes)
            {
                foreach (typesType type in tf.SerachTypes("zmbf_"))
                {
                    enemiesLB.Items.Add(type.name);
                }
            }
            foreach (TypesFile tf in ModTypes)
            {
                foreach (typesType type in tf.SerachTypes("Animal_"))
                {
                    enemiesLB.Items.Add(type.name);
                }
            }
        }
        private void darkButton8_Click(object sender, EventArgs e)
        {
            int index = ZombiesClassNamesLB.SelectedIndex;
            List<string> removezombies = new List<string>();
            foreach (var item in ZombiesClassNamesLB.SelectedItems)
            {
                removezombies.Add(item.ToString());
            }
            foreach (string s in removezombies)
            {
                MDCKOTHConfig.enemies.Remove(s);
            }
            if (ZombiesClassNamesLB.SelectedIndex != -1)
            {
                ZombiesClassNamesLB.SelectedIndex = -1;
                int newindex = index - 1;
                if (newindex == -1)
                    ZombiesClassNamesLB.SelectedIndex = 0;
                else
                    ZombiesClassNamesLB.SelectedIndex = index - 1;
            }
            MDCKOTHConfig.isDirty = true;
        }
        private void darkButton7_Click(object sender, EventArgs e)
        {
            foreach (var item in ZombiestochoosefromLB.SelectedItems)
            {
                string zombie = ZombiestochoosefromLB.GetItemText(item);
                if (!MDCKOTHConfig.enemies.Contains(zombie))
                    MDCKOTHConfig.enemies.Add(zombie);
            }
            MDCKOTHConfig.isDirty = true;
        }


        /// <summary>
        /// LootSets
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        public KOTHLootset currentlootset { get; set; }
        private void LootSetsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            LootSetTV.Nodes.Clear();
            if (LootSetsLB.SelectedItems.Count < 1) return;
            useraction = false;
            currentlootset = LootSetsLB.SelectedItem as KOTHLootset;
            SetupLootTreeview(LootSetTV, currentlootset);
            useraction = true;
        }
        private void SetupLootTreeview(TreeView TreeV, KOTHLootset lootset)
        {
            if (lootset == null) return;
            Console.WriteLine("populating LootSet treeView");
            TreeV.Nodes.Clear();
            TreeNode TLootSet = new TreeNode(lootset.name)
            {
                Tag = lootset
            };
            foreach (KOTHItem item in lootset.items)
            {
                TLootSet.Nodes.Add(Kothitems(item));
            }
            TreeV.Nodes.Add(TLootSet);
        }
        TreeNode Kothitems(KOTHItem item)
        {
            TreeNode Titem = new TreeNode(item.name)
            {
                Tag = item
            };
            TreeNode Tquantiy = new TreeNode("Quantity")
            {
                Tag = "Quantity"
            };
            Titem.Nodes.Add(Tquantiy);
            TreeNode Tattachments = new TreeNode("Attachments")
            {
                Tag = "Attachments"
            };
            foreach (KOTHItem Attachemnts in item.attachments)
            {
                Tattachments.Nodes.Add(Kothitems(Attachemnts));
            }
            Titem.Nodes.Add(Tattachments);
            TreeNode Tcargo = new TreeNode("Cargo")
            {
                Tag = "Cargo"
            };
            foreach (KOTHItem cargo in item.cargo)
            {
                Tcargo.Nodes.Add(Kothitems(cargo));
            }
            Titem.Nodes.Add(Tcargo);
            return Titem;
        }
        public object CurrentTreeNodeTag;
        public KOTHLootset currentLootSet;
        public KOTHItem currentLootitem;
        private void darkButton3_Click(object sender, EventArgs e)
        {
            MDCKOTHLoot.lootSets.Add(new KOTHLootset()
            {
                name = "New Loot Set",
                items = new BindingList<KOTHItem>()
            });
            MDCKOTHLoot.isDirty = true;
            if (MDCKOTHLoot.lootSets.Count == 1)
            {
                LootSetsLB.SelectedIndex = -1;
                LootSetsLB.SelectedIndex = 0;
            }
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
            int index = LootSetsLB.SelectedIndex;

            List<KOTHLootset> removelootsets = new List<KOTHLootset>();
            foreach (var item in LootSetsLB.SelectedItems)
            {
                removelootsets.Add(item as KOTHLootset);
            }
            foreach(KOTHLootset ls in removelootsets)
            {
                MDCKOTHLoot.lootSets.Remove(ls);
                MDCKOTHLoot.isDirty = true;
            }
            if (LootSetsLB.SelectedIndex != -1)
            {
                LootSetsLB.SelectedIndex = -1;
                int newindex = index - 1;
                if (newindex == -1)
                    LootSetsLB.SelectedIndex = 0;
                else
                    LootSetsLB.SelectedIndex = index - 1;
            }
        }
        private void LootsetTV_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            useraction = false;
            LootSetTV.SelectedNode = e.Node;
            CurrentTreeNodeTag = e.Node.Tag;
            LootSetNameGB.Visible = false;
            LootSetQuantityGB.Visible = false;
            currentLootSet = null;
            currentLootitem = null;

            addAttchmentToolStripMenuItem.Visible = false;
            removeAttachmentToolStripMenuItem.Visible = false;
            addCargoToolStripMenuItem.Visible = false;
            removeCargoToolStripMenuItem.Visible = false;
            addLootItemToolStripMenuItem.Visible = false;
            removeLootItemToolStripMenuItem.Visible = false;

            if (e.Node.Tag is KOTHLootset)
            {
                currentLootSet = e.Node.Tag as KOTHLootset;
                LootSetNameGB.Visible = true;
                darkButton1.Visible = false;
                LootitemNameTB.Text = currentLootSet.name;
                if (e.Button == MouseButtons.Right)
                {
                    addLootItemToolStripMenuItem.Visible = true;
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
            else if (e.Node.Tag is KOTHItem)
            {
                currentLootitem = e.Node.Tag as KOTHItem;
                LootSetNameGB.Visible = true;
                darkButton1.Visible = true;
                LootitemNameTB.Text = currentLootitem.name;
                if(e.Button == MouseButtons.Right)
                {
                    if (e.Node.Parent.Tag is KOTHLootset)
                        removeLootItemToolStripMenuItem.Visible = true;
                    else
                    {
                        if (e.Node.Parent.Tag.ToString() == "Cargo")
                            removeCargoToolStripMenuItem.Visible = true;
                        else if (e.Node.Parent.Tag.ToString() == "Attachments")
                            removeAttachmentToolStripMenuItem.Visible = true;
                    }
                        
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
            else if (e.Node.Tag is string)
            {
                switch (e.Node.Tag.ToString())
                {
                    case "Quantity":
                        currentLootitem = e.Node.Parent.Tag as KOTHItem;
                        LootSetQuantityGB.Visible = true;
                        LootItemQuantityNUD.Value = currentLootitem.quantity;
                        break;
                    case "Attachments":
                        if(e.Button == MouseButtons.Right)
                        {
                            currentLootitem = e.Node.Parent.Tag as KOTHItem;
                            addAttchmentToolStripMenuItem.Visible = true;
                            addAttchmentToolStripMenuItem.Text = "Add Attachment";
                            contextMenuStrip1.Show(Cursor.Position);
                        }
                        break;
                    case "Cargo":
                        if(e.Button == MouseButtons.Right)
                        {
                            currentLootitem = e.Node.Parent.Tag as KOTHItem;
                            addCargoToolStripMenuItem.Visible = true;
                            contextMenuStrip1.Show(Cursor.Position);
                        }
                        break;
                }
            }

            useraction = true;
        }
        private void darkButton1_Click(object sender, EventArgs e)
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
                    LootitemNameTB.Text = l;
                }
            }
        }
        private void LootitemNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;

            if (currentLootSet != null && currentLootitem == null)
            {
                currentLootSet.name = LootSetTV.SelectedNode.Text = LootitemNameTB.Text;
                LootSetsLB.Refresh();
            }
            else if (currentLootSet == null && currentLootitem != null)
                currentLootitem.name = LootSetTV.SelectedNode.Text = LootitemNameTB.Text;
            MDCKOTHLoot.isDirty = true;
        }
        private void LootItemQuantityNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentLootitem.quantity = (int)LootItemQuantityNUD.Value;
            MDCKOTHLoot.isDirty = true;
        }
        private void addNewAttToolStripMenuItem_Click(object sender, EventArgs e)
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
                    KOTHItem newkothattachment = new KOTHItem()
                    {
                        name = l,
                        quantity = -1,
                        attachments = new BindingList<KOTHItem>(),
                        cargo = new BindingList<KOTHItem>()
                    };
                    LootSetTV.SelectedNode.Nodes.Add(Kothitems(newkothattachment));
                    LootSetTV.SelectedNode.Expand();
                    currentLootitem.attachments.Add(newkothattachment);
                    MDCKOTHLoot.isDirty = true;
                }
            }
        }
        private void removeAttachmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KOTHItem parent = LootSetTV.SelectedNode.Parent.Parent.Tag as KOTHItem;
            parent.attachments.Remove(currentLootitem);
            LootSetTV.SelectedNode.Remove();
            MDCKOTHLoot.isDirty = true;
        }
        private void addCargoToolStripMenuItem_Click(object sender, EventArgs e)
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
                    KOTHItem newkothcargo = new KOTHItem()
                    {
                        name = l,
                        quantity = -1,
                        attachments = new BindingList<KOTHItem>(),
                        cargo = new BindingList<KOTHItem>()
                    };
                    LootSetTV.SelectedNode.Nodes.Add(Kothitems(newkothcargo));
                    LootSetTV.SelectedNode.Expand();
                    currentLootitem.cargo.Add(newkothcargo);
                    MDCKOTHLoot.isDirty = true;
                }
            }
        }
        private void removeCargoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KOTHItem parent = LootSetTV.SelectedNode.Parent.Parent.Tag as KOTHItem;
            parent.cargo.Remove(currentLootitem);
            LootSetTV.SelectedNode.Remove();
            MDCKOTHLoot.isDirty = true;
        }
        private void addLootItemToolStripMenuItem_Click(object sender, EventArgs e)
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
                    KOTHItem newkothitem = new KOTHItem()
                    {
                        name = l,
                        quantity = -1,
                        attachments = new BindingList<KOTHItem>(),
                        cargo = new BindingList<KOTHItem>()
                    };
                    LootSetTV.SelectedNode.Nodes.Add(Kothitems(newkothitem));
                    LootSetTV.SelectedNode.Expand();
                    currentlootset.items.Add(newkothitem);
                    MDCKOTHLoot.isDirty = true;
                }
            }
        }
        private void removeLootItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentlootset.items.Remove(currentLootitem);
            LootSetTV.SelectedNode.Remove();
            MDCKOTHLoot.isDirty = true;
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }




        /// <summary>
        /// Copy/paste lootsets between loot config and zone config.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public List<KOTHLootset> copiedlootsets;
        private void darkButton16_Click(object sender, EventArgs e)
        {
            copiedlootsets = new List<KOTHLootset>();
            foreach (var item in ZoneLootSetsLB.SelectedItems)
            {
                copiedlootsets.Add(item as KOTHLootset);
            }
            Console.WriteLine("\nCopied to Clipboard:\n" + string.Join(Environment.NewLine, copiedlootsets.Cast<object>().Select(o => o.ToString()).ToArray()));
        }
        private void darkButton17_Click(object sender, EventArgs e)
        {
            foreach(KOTHLootset SET in copiedlootsets)
            {
                currentKOTHZoneAreaLocation.lootSets.Add(SET.Clone());
            }
            MDCKOTHConfig.isDirty = true;
        }
        private void darkButton19_Click(object sender, EventArgs e)
        {
            copiedlootsets = new List<KOTHLootset>();
            foreach(var item in LootSetsLB.SelectedItems)
            {
                copiedlootsets.Add(item as KOTHLootset);
            }
            Console.WriteLine("\nCopied to Clipboard:\n" + string.Join(Environment.NewLine, copiedlootsets.Cast<object>().Select(o => o.ToString()).ToArray()));
        }
        private void darkButton18_Click(object sender, EventArgs e)
        {
            foreach (KOTHLootset SET in copiedlootsets)
            {
                MDCKOTHLoot.lootSets.Add(SET.Clone());
            }
            MDCKOTHLoot.isDirty = true;
        }
    }
}
