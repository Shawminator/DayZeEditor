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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            if (tabControl1.SelectedIndex == 1)
                toolStripButton1.Checked = true;
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            if (tabControl1.SelectedIndex == 0)
                toolStripButton3.Checked = true;
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripButton1.Checked = false;
            toolStripButton3.Checked = false;
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    toolStripButton3.Checked = true;
                    break;
                case 1:
                    toolStripButton1.Checked = true;
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
            SetupKingOfTheHillConfig();

            MapData = new MapData(Application.StartupPath + currentproject.MapPath + ".xyz");

            pictureBox2.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Livonia maop size is 12800 x 12800, 0,0 bottom left, center 6400 x 6400
            pictureBox2.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox2.Paint += new PaintEventHandler(DrawHillZones);
            trackBar4.Value = 1;
            SetHillZonescale();
            setupHillZombies();
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

            useraction = true;
        }


        private void HillsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (HillsLB.SelectedItems.Count < 1) return;
            currentKOTHZoneAreaLocation = HillsLB.SelectedItem as MDCKOTHZones;
            useraction = false;
            zoneNameTB.Text = currentKOTHZoneAreaLocation.zoneName;
            XNUD.Value = (decimal)currentKOTHZoneAreaLocation.zonePosition[0];
            YNUD.Value = (decimal)currentKOTHZoneAreaLocation.zonePosition[1];
            ZNUD.Value = (decimal)currentKOTHZoneAreaLocation.zonePosition[2];
            zoneRadiusNUD.Value = currentKOTHZoneAreaLocation.zoneRadius;




            pictureBox2.Invalidate();
            useraction = true;
        }

        /// <summary>
        /// general Config Settings value changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_UpdateIntervalNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;

            MDCKOTHConfig.isDirty = true;
        }
        private void m_ServerStartDelayNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;

            MDCKOTHConfig.isDirty = true;
        }
        private void m_CaptureTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;

            MDCKOTHConfig.isDirty = true;
        }
        private void m_HillEventIntervalNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;

            MDCKOTHConfig.isDirty = true;
        }
        private void m_EventCleanupTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;

            MDCKOTHConfig.isDirty = true;
        }
        private void m_EventPreStartNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;

            MDCKOTHConfig.isDirty = true;
        }
        private void m_PlayerPopulationToStartEventsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;

            MDCKOTHConfig.isDirty = true;
        }
        private void m_MaxEventsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;

            MDCKOTHConfig.isDirty = true;
        }
        private void m_DoLogsToCFCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;

            MDCKOTHConfig.isDirty = true;
        }
        private void m_EventPreStartMessageTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;

            MDCKOTHConfig.isDirty = true;
        }
        private void m_EventCapturedMessageTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;

            MDCKOTHConfig.isDirty = true;
        }
        private void m_EventDespawnedMessageTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;

            MDCKOTHConfig.isDirty = true;
        }
        private void m_EventStartMessageTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;

            MDCKOTHConfig.isDirty = true;
        }
        private void m_FlagNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;

            MDCKOTHConfig.isDirty = true;
        }

        /// <summary>
        /// Hill Config Value Changes and Add remove Hills
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        }
        private void darkButton11_Click(object sender, EventArgs e)
        {

            MDCKOTHConfig.isDirty = true;
            pictureBox2.Invalidate();
            if (HillsLB.Items.Count == 0)
                HillsLB.SelectedIndex = -1;
            else
                HillsLB.SelectedIndex = 0;
        }
        private void NameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;

            MDCKOTHConfig.isDirty = true;
            HillsLB.Refresh();
        }
        private void XNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;

            MDCKOTHConfig.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void YNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;

            MDCKOTHConfig.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void ZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;

            MDCKOTHConfig.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void RadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;

            MDCKOTHConfig.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void AISpawnCountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;

            MDCKOTHConfig.isDirty = true;
        }
        private void pictureBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var mouseEventArgs = e as MouseEventArgs;
            if (mouseEventArgs != null)
            {
                float scalevalue = ZoneScale * 0.05f;
                float mapsize = currentproject.MapSize;
                int newsize = (int)(mapsize * scalevalue);

                Cursor.Current = Cursors.WaitCursor;
                XNUD.Value = (decimal)(mouseEventArgs.X / scalevalue);
                ZNUD.Value = (decimal)((newsize - mouseEventArgs.Y) / scalevalue);
                if (MapData.FileExists)
                {
                    //YNUD.Value = (decimal)(MapData.gethieght(currentHill.Position[0], currentHill.Position[2]));
                }
                Cursor.Current = Cursors.Default;
                MDCKOTHConfig.isDirty = true;
                pictureBox2.Invalidate();

            }
        }

        /// <summary>
        /// Zombie List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setupHillZombies()
        {
            ZombiestochoosefromLB.Items.Clear();
            foreach (typesType type in vanillatypes.SerachTypes("zmbm_"))
            {
                ZombiestochoosefromLB.Items.Add(type.name);
            }
            foreach (typesType type in vanillatypes.SerachTypes("zmbf_"))
            {
                ZombiestochoosefromLB.Items.Add(type.name);
            }
            foreach (typesType type in vanillatypes.SerachTypes("Animal_"))
            {
                ZombiestochoosefromLB.Items.Add(type.name);
            }
            foreach (TypesFile tf in ModTypes)
            {
                foreach (typesType type in tf.SerachTypes("zmbm_"))
                {
                    ZombiestochoosefromLB.Items.Add(type.name);
                }
            }
            foreach (TypesFile tf in ModTypes)
            {
                foreach (typesType type in tf.SerachTypes("zmbf_"))
                {
                    ZombiestochoosefromLB.Items.Add(type.name);
                }
            }
            foreach (TypesFile tf in ModTypes)
            {
                foreach (typesType type in tf.SerachTypes("Animal_"))
                {
                    ZombiestochoosefromLB.Items.Add(type.name);
                }
            }
        }
        private void darkButton8_Click(object sender, EventArgs e)
        {
            List<string> removezombies = new List<string>();
            foreach (var item in ZombiesClassNamesLB.SelectedItems)
            {
                removezombies.Add(item.ToString());

            }
            foreach (string s in removezombies)
            {
                MDCKOTHConfig.enemies.Remove(s);
                
            }
            MDCKOTHConfig.isDirty = true;
        }
        private void darkButton7_Click(object sender, EventArgs e)
        {
            foreach (var item in ZombiestochoosefromLB.SelectedItems)
            {
                string zombie = ZombiestochoosefromLB.GetItemText(ZombiestochoosefromLB.SelectedItem);
                if (!MDCKOTHConfig.enemies.Contains(zombie))
                    MDCKOTHConfig.enemies.Add(zombie);
            }
            MDCKOTHConfig.isDirty = true;
        }


        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath);
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
            if (needtosave)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SaveKOTHZoneconfigs();
                }
            }
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {

        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
