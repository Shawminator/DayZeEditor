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

        public string KingOfTheHillConfigPath { get; private set; }
        public KingOfTheHillConfig KingOfTheHillConfig { get; set; }

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
            foreach (Hill Hills in KingOfTheHillConfig.Hills)
            {
                float scalevalue = ZoneScale * 0.05f;
                int centerX = (int)(Math.Round(Hills.X) * scalevalue);
                int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(Hills.Z, 0) * scalevalue);
                int eventradius = (int)(Math.Round((float)Hills.EventRadius, 0) * scalevalue);
                int captureradius = (int)(Math.Round((float)Hills.CaptureRadius, 0) * scalevalue);
                Point center = new Point(centerX, centerY);
                Pen pen = new Pen(Color.Red, 4);
                if (Hills == currentHill)
                    pen.Color = Color.LimeGreen;
                getCircle(e.Graphics, pen, center, eventradius);
                getCircle(e.Graphics, pen, center, captureradius);
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

            KingOfTheHillConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\KingOfTheHill.json";
            if (!File.Exists(KingOfTheHillConfigPath))
            {
                KingOfTheHillConfig = new KingOfTheHillConfig();
            }
            else
            {
                KingOfTheHillConfig = JsonSerializer.Deserialize<KingOfTheHillConfig>(File.ReadAllText(KingOfTheHillConfigPath));
                KingOfTheHillConfig.isDirty = false;
            }
            KingOfTheHillConfig.FullFilename = KingOfTheHillConfigPath;
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

            IntervalNUD.Value = KingOfTheHillConfig.Interval;
            StartDelayNUD.Value = KingOfTheHillConfig.StartDelay;
            CaptureTimeNUD.Value = KingOfTheHillConfig.CaptureTime;
            EmptyEventTimeOutNUD.Value = KingOfTheHillConfig.EmptyEventTimeOut;
            CleanUpTimeNUD.Value = KingOfTheHillConfig.CleanUpTime;
            PreStartDelayNUD.Value = KingOfTheHillConfig.PreStartDelay;
            FullMapCheckTimerNUD.Value = KingOfTheHillConfig.FullMapCheckTimer;
            EventTickTimeNUD.Value = KingOfTheHillConfig.EventTickTime;
            LoggingCB.Checked = KingOfTheHillConfig.Logging == 1 ? true : false;

            HillsLB.DisplayMember = "Name";
            HillsLB.ValueMember = "Value";
            HillsLB.DataSource = KingOfTheHillConfig.Hills;
            useraction = false;
            Hills2LB.DisplayMember = "Name";
            Hills2LB.ValueMember = "Value";
            Hills2LB.DataSource = KingOfTheHillConfig.Hills;
            useraction = false;
            RewardPoolsLB.DisplayMember = "Name";
            RewardPoolsLB.ValueMember = "Value";
            RewardPoolsLB.DataSource = KingOfTheHillConfig.RewardPools;
            useraction = false;
            ZombiesClassNamesLB.DisplayMember = "Name";
            ZombiesClassNamesLB.ValueMember = "Value";
            ZombiesClassNamesLB.DataSource = KingOfTheHillConfig.ZombiesClassNames;
            useraction = true;
        }


        public Hill currentHill;
        private void HillsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (HillsLB.SelectedItems.Count < 1) return;
            currentHill = HillsLB.SelectedItem as Hill;
            useraction = false;
            NameTB.Text = currentHill.Name;
            XNUD.Value = (decimal)currentHill.X;
            YNUD.Value = (decimal)currentHill.Y;
            ZNUD.Value = (decimal)currentHill.Z;
            CaptureRadiusNUD.Value = currentHill.CaptureRadius;
            EventRadiusNUD.Value = currentHill.EventRadius;
            ZombieCountNUD.Value = currentHill.ZombieCount;

            pictureBox2.Invalidate();
            useraction = true;
        }
        public Rewardpool CurrentrewardPool;
        private void RewardPoolsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RewardPoolsLB.SelectedItems.Count < 1) return;
            CurrentrewardPool = RewardPoolsLB.SelectedItem as Rewardpool;
            useraction = false;
            RewardsPoolNameTB.Text = CurrentrewardPool.Name;
            RewardContainerTB.Text = CurrentrewardPool.RewardContainer;

            RewardsLB.DisplayMember = "Name";
            RewardsLB.ValueMember = "Value";
            RewardsLB.DataSource = CurrentrewardPool.Rewards;
            useraction = false;
            if (CurrentrewardPool.Rewards.Count == 0)
            {
                RewardItemTB.Text = "";
                ItemAttachmentsLB.DataSource = null;
            }

            useraction = true;
        }
        public Reward currentReward;
        private void RewardsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RewardsLB.SelectedItems.Count < 1) return;
            currentReward = RewardsLB.SelectedItem as Reward;
            useraction = false;
            RewardItemTB.Text = currentReward.Item;
            ItemAttachmentsLB.DisplayMember = "Name";
            ItemAttachmentsLB.ValueMember = "Value";
            ItemAttachmentsLB.DataSource = currentReward.ItemAttachments;

            useraction = true;
        }

        /// <summary>
        /// general Config Settings value changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IntervalNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KingOfTheHillConfig.Interval = IntervalNUD.Value;
            KingOfTheHillConfig.isDirty = true;
        }
        private void StartDelayNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KingOfTheHillConfig.StartDelay = StartDelayNUD.Value;
            KingOfTheHillConfig.isDirty = true;
        }
        private void CaptureTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KingOfTheHillConfig.CaptureTime = CaptureTimeNUD.Value;
            KingOfTheHillConfig.isDirty = true;
        }
        private void EmptyEventTimeOutNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KingOfTheHillConfig.CaptureTime = CaptureTimeNUD.Value;
            KingOfTheHillConfig.isDirty = true;
        }
        private void CleanUpTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KingOfTheHillConfig.Interval = IntervalNUD.Value;
            KingOfTheHillConfig.isDirty = true;
        }
        private void PreStartDelayNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KingOfTheHillConfig.PreStartDelay = PreStartDelayNUD.Value;
            KingOfTheHillConfig.isDirty = true;
        }
        private void FullMapCheckTimerNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KingOfTheHillConfig.FullMapCheckTimer = FullMapCheckTimerNUD.Value;
            KingOfTheHillConfig.isDirty = true;
        }
        private void EventTickTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KingOfTheHillConfig.EventTickTime = EventTickTimeNUD.Value;
            KingOfTheHillConfig.isDirty = true;
        }
        private void LoggingCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KingOfTheHillConfig.Logging = LoggingCB.Checked == true ? 1 : 0;
            KingOfTheHillConfig.isDirty = true;
        }

        /// <summary>
        /// Hill Config Value Changes and Add remove Hills
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void darkButton12_Click(object sender, EventArgs e)
        {
            float[] centre = new float[] { currentproject.MapSize / 2, 0, currentproject.MapSize / 2 };
            Hill newHill = new Hill()
            {
                Name = "New Hill",
                X = centre[0],
                Y = centre[1],
                Z = centre[2],
                CaptureRadius = 20,
                EventRadius = 100,
                ZombieCount = 20,
                Objects = new BindingList<KOTHObject>()
            };
            KingOfTheHillConfig.Hills.Add(newHill);
            KingOfTheHillConfig.isDirty = true;
        }
        private void darkButton11_Click(object sender, EventArgs e)
        {
            KingOfTheHillConfig.Hills.Remove(currentHill);
            KingOfTheHillConfig.isDirty = true;
            pictureBox2.Invalidate();
            if (HillsLB.Items.Count == 0)
                HillsLB.SelectedIndex = -1;
            else
                HillsLB.SelectedIndex = 0;
        }
        private void NameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentHill.Name = NameTB.Text;
            KingOfTheHillConfig.isDirty = true;
            HillsLB.Refresh();
        }
        private void XNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentHill.X = (float)XNUD.Value;
            KingOfTheHillConfig.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void YNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentHill.Y = (float)YNUD.Value;
            KingOfTheHillConfig.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void ZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentHill.Z = (float)ZNUD.Value;
            KingOfTheHillConfig.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void CaptureRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentHill.CaptureRadius = CaptureRadiusNUD.Value;
            KingOfTheHillConfig.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void EventRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentHill.EventRadius = EventRadiusNUD.Value;
            KingOfTheHillConfig.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void ZombieCountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentHill.ZombieCount = (int)ZombieCountNUD.Value;
            KingOfTheHillConfig.isDirty = true;
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
                    YNUD.Value = (decimal)(MapData.gethieght(currentHill.X, currentHill.Z));
                }
                Cursor.Current = Cursors.Default;
                KingOfTheHillConfig.isDirty = true;
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
                KingOfTheHillConfig.ZombiesClassNames.Remove(s);
                
            }
            KingOfTheHillConfig.isDirty = true;
        }
        private void darkButton7_Click(object sender, EventArgs e)
        {
            foreach (var item in ZombiestochoosefromLB.SelectedItems)
            {
                string zombie = item.ToString();
                if (!KingOfTheHillConfig.ZombiesClassNames.Contains(zombie))
                {
                    KingOfTheHillConfig.ZombiesClassNames.Add(zombie);
                    KingOfTheHillConfig.isDirty = true;
                }
                else
                {
                    MessageBox.Show("Infected Type allready in the list.....");
                }
            }
        }

        /// <summary>
        /// Rewrds Value changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void darkButton2_Click(object sender, EventArgs e)
        {
            Rewardpool Newpool = new Rewardpool()
            {
                Name = "New Rweards Pool",
                RewardContainer = "SeaChest",
                Rewards = new BindingList<Reward>()
            };
            KingOfTheHillConfig.RewardPools.Add(Newpool);
            KingOfTheHillConfig.isDirty = true;
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            KingOfTheHillConfig.RewardPools.Remove(CurrentrewardPool);
            KingOfTheHillConfig.isDirty = true;
            if (RewardPoolsLB.Items.Count == 0)
                RewardPoolsLB.SelectedIndex = -1;
            else
                RewardPoolsLB.SelectedIndex = 0;
        }
        private void RewardsPoolNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentrewardPool.Name = RewardsPoolNameTB.Text;
            KingOfTheHillConfig.isDirty = true;
            RewardPoolsLB.Refresh();
        }
        private void RewardContainerTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentrewardPool.RewardContainer = RewardContainerTB.Text;
            KingOfTheHillConfig.isDirty = true;
        }

        /// <summary>
        /// Rewards Value Chages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void darkButton4_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseMultiple = false,
                isCategoryitem = true,
                LowerCase = false
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    Reward NewReward = new Reward()
                    {
                        Item = l,
                        ItemAttachments = new BindingList<string>()
                    };
                    CurrentrewardPool.Rewards.Add(NewReward);
                    KingOfTheHillConfig.isDirty = true;
                }
            }
        }
        private void darkButton3_Click(object sender, EventArgs e)
        {
            CurrentrewardPool.Rewards.Remove(currentReward);
            KingOfTheHillConfig.isDirty = true;
            if (RewardsLB.Items.Count == 0)
                RewardsLB.SelectedIndex = -1;
            else
                RewardsLB.SelectedIndex = 0;
        }
        private void RewardItemTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentReward.Item = RewardItemTB.Text;
            KingOfTheHillConfig.isDirty = true;
            RewardsLB.Refresh();
        }
        private void darkButton6_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseMultiple = false,
                isCategoryitem = false,
                LowerCase = false
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (!currentReward.ItemAttachments.Contains(l))
                    {
                        currentReward.ItemAttachments.Add(l);
                        KingOfTheHillConfig.isDirty = true;
                    }
                    else
                        MessageBox.Show("Attachments Type allready in the list.....");
                }
            }
        }
        private void darkButton5_Click(object sender, EventArgs e)
        {
            currentReward.ItemAttachments.Remove(ItemAttachmentsLB.GetItemText(ItemAttachmentsLB.SelectedItem));
            KingOfTheHillConfig.isDirty = true;
            if (ItemAttachmentsLB.Items.Count == 0)
                ItemAttachmentsLB.SelectedIndex = -1;
            else
                ItemAttachmentsLB.SelectedIndex = 0;
        }

        /// <summary>
        /// Objects Value Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hills2LB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Hills2LB.SelectedItems.Count < 1) return;

            useraction = false;

            ObjectsLB.DisplayMember = "Name";
            ObjectsLB.ValueMember = "Value";
            ObjectsLB.DataSource = currentHill.Objects;

            if (currentHill.Objects.Count == 0)
            {
                ObjectItemTB.Text = "";
                ObjectPosXNUD.Value = 0;
                ObjectPosYNUD.Value = 0;
                ObjectPosZNUD.Value = 0;
                ObjectOrentaionXNUD.Value = 0;
                ObjectOrentaionYNUD.Value = 0;
                ObjectOrentaionZNUD.Value = 0;
            }

            useraction = true;
        }
        public KOTHObject currentObject;
        private void ObjectsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ObjectsLB.SelectedItems.Count < 1) return;
            currentObject = ObjectsLB.SelectedItem as KOTHObject;

            useraction = false;

            ObjectItemTB.Text = currentObject.Item;
            ObjectPosXNUD.Value = (decimal)currentObject.Position[0];
            ObjectPosYNUD.Value = (decimal)currentObject.Position[1];
            ObjectPosZNUD.Value = (decimal)currentObject.Position[2];
            ObjectOrentaionXNUD.Value = (decimal)currentObject.Orientation[0];
            ObjectOrentaionYNUD.Value = (decimal)currentObject.Orientation[1];
            ObjectOrentaionZNUD.Value = (decimal)currentObject.Orientation[2];

            useraction = true;
        }
        private void darkButton10_Click(object sender, EventArgs e)
        {
            KOTHObject newkothObject = new KOTHObject()
            {
                Item = "NewItem",
                Position = new float[] { 0, 0, 0 },
                Orientation = new float[] { 0, 0, 0 }
            };
            currentHill.Objects.Add(newkothObject);
            KingOfTheHillConfig.isDirty = true;
        }
        private void darkButton9_Click(object sender, EventArgs e)
        {
            currentHill.Objects.Remove(currentObject);
            KingOfTheHillConfig.isDirty = true;
            if (ObjectsLB.Items.Count == 0)
                ObjectsLB.SelectedIndex = -1;
            else
                ObjectsLB.SelectedIndex = 0;
        }
        private void ObjectItemTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentObject.Item = ObjectItemTB.Text;
            KingOfTheHillConfig.isDirty = true;
        }
        private void ObjectPosXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentObject.Position[0] = (float)ObjectPosXNUD.Value;
            KingOfTheHillConfig.isDirty = true;
        }
        private void ObjectPosYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentObject.Position[1] = (float)ObjectPosYNUD.Value;
            KingOfTheHillConfig.isDirty = true;
        }
        private void ObjectPosZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentObject.Position[2] = (float)ObjectPosZNUD.Value;
            KingOfTheHillConfig.isDirty = true;
        }
        private void ObjectOrentaionXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentObject.Orientation[0] = (float)ObjectOrentaionXNUD.Value;
            KingOfTheHillConfig.isDirty = true;
        }
        private void ObjectOrentaionYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentObject.Orientation[1] = (float)ObjectOrentaionYNUD.Value;
            KingOfTheHillConfig.isDirty = true;
        }
        private void ObjectOrentaionZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentObject.Orientation[2] = (float)ObjectOrentaionZNUD.Value;
            KingOfTheHillConfig.isDirty = true;
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
            if (KingOfTheHillConfig.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(KingOfTheHillConfig.FullFilename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(KingOfTheHillConfig.FullFilename) + "\\Backup\\" + SaveTime);
                    File.Copy(KingOfTheHillConfig.FullFilename, Path.GetDirectoryName(KingOfTheHillConfig.FullFilename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(KingOfTheHillConfig.FullFilename) + ".bak", true);
                }
                KingOfTheHillConfig.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(KingOfTheHillConfig, options);
                File.WriteAllText(KingOfTheHillConfig.FullFilename, jsonString);
                midifiedfiles.Add(Path.GetFileName(Path.GetFileName(KingOfTheHillConfigPath)));
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

        private void darkButton23_Click(object sender, EventArgs e)
        {
            string[] fileContent = new string[] { };
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    var fileStream = openFileDialog.OpenFile();
                    fileContent = File.ReadAllLines(filePath);
                    DialogResult dialogResult = MessageBox.Show("Clear Exisitng Position?", "Clear position", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        currentHill.Objects = new BindingList<KOTHObject>();
                    }
                    foreach (string line in fileContent)
                    {
                        string[] linesplit = line.Split('|');
                        KOTHObject newobject = new KOTHObject();
                        newobject.Item = linesplit[0];
                        newobject.Position = new float[] {Convert.ToSingle(linesplit[1].Split(' ')[0]), Convert.ToSingle(linesplit[1].Split(' ')[1]), Convert.ToSingle(linesplit[1].Split(' ')[2]) };
                        newobject.Orientation = new float[] { Convert.ToSingle(linesplit[2].Split(' ')[0]), Convert.ToSingle(linesplit[2].Split(' ')[1]), Convert.ToSingle(linesplit[2].Split(' ')[2]) };
                        currentHill.Objects.Add(newobject);
                        KingOfTheHillConfig.isDirty = true;
                    }
                    ObjectsLB.Refresh();

                }
            }
        }
    }
}
