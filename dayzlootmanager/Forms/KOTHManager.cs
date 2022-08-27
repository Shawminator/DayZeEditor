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
            foreach (M_Hilllocations Hills in KingOfTheHillConfig.m_HillLocations)
            {
                float scalevalue = ZoneScale * 0.05f;
                int centerX = (int)(Math.Round(Hills.Position[0]) * scalevalue);
                int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(Hills.Position[2], 0) * scalevalue);
                int eventradius = (int)(Math.Round((float)Hills.Radius, 0) * scalevalue);
                Point center = new Point(centerX, centerY);
                Pen pen = new Pen(Color.Red, 4);
                if (Hills == currentHill)
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

            KingOfTheHillConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\KingOfTheHill\\server-config.json";
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

            m_CaptureTimeNUD.Value = KingOfTheHillConfig.m_CaptureTime;
            m_UpdateIntervalNUD.Value = KingOfTheHillConfig.m_UpdateInterval;
            m_ServerStartDelayNUD.Value = KingOfTheHillConfig.m_ServerStartDelay;
            m_HillEventIntervalNUD.Value = KingOfTheHillConfig.m_HillEventInterval;
            m_EventCleanupTimeNUD.Value = KingOfTheHillConfig.m_EventCleanupTime;
            m_EventPreStartNUD.Value = KingOfTheHillConfig.m_EventPreStart;
            m_EventPreStartMessageTB.Text = KingOfTheHillConfig.m_EventPreStartMessage;
            m_EventCapturedMessageTB.Text = KingOfTheHillConfig.m_EventCapturedMessage;
            m_EventDespawnedMessageTB.Text = KingOfTheHillConfig.m_EventDespawnedMessage;
            m_EventStartMessageTB.Text = KingOfTheHillConfig.m_EventStartMessage;
            m_DoLogsToCFCB.Checked = KingOfTheHillConfig.m_DoLogsToCF == 1 ? true : false;
            m_PlayerPopulationToStartEventsNUD.Value = (decimal)KingOfTheHillConfig.m_PlayerPopulationToStartEvents;
            m_MaxEventsNUD.Value = (decimal)KingOfTheHillConfig.m_MaxEvents;
            m_FlagNameTB.Text = KingOfTheHillConfig.m_FlagName;

            HillsLB.DisplayMember = "Name";
            HillsLB.ValueMember = "Value";
            HillsLB.DataSource = KingOfTheHillConfig.m_HillLocations;
            useraction = false;
            useraction = false;
            RewardPoolsLB.DisplayMember = "Name";
            RewardPoolsLB.ValueMember = "Value";
            RewardPoolsLB.DataSource = KingOfTheHillConfig.m_RewardPools;
            useraction = false;
            ZombiesClassNamesLB.DisplayMember = "Name";
            ZombiesClassNamesLB.ValueMember = "Value";
            ZombiesClassNamesLB.DataSource = KingOfTheHillConfig.m_Creatures;
            useraction = true;
        }

        public M_Hilllocations currentHill;
        private void HillsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (HillsLB.SelectedItems.Count < 1) return;
            currentHill = HillsLB.SelectedItem as M_Hilllocations;
            useraction = false;
            NameTB.Text = currentHill.Name;
            XNUD.Value = (decimal)currentHill.Position[0];
            YNUD.Value = (decimal)currentHill.Position[1];
            ZNUD.Value = (decimal)currentHill.Position[2];
            RadiusNUD.Value = currentHill.Radius;
            AISpawnCountNUD.Value = currentHill.AISpawnCount;

            pictureBox2.Invalidate();
            useraction = true;
        }
        public M_Rewardpools CurrentrewardPool;
        private void RewardPoolsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RewardPoolsLB.SelectedItems.Count < 1) return;
            CurrentrewardPool = RewardPoolsLB.SelectedItem as M_Rewardpools;
            useraction = false;
            RewardsPoolNameTB.Text = CurrentrewardPool.RewardContainerName;

            RewardsLB.DisplayMember = "Name";
            RewardsLB.ValueMember = "Value";
            RewardsLB.DataSource = CurrentrewardPool.m_Rewards;
            useraction = false;
            if (CurrentrewardPool.m_Rewards.Count == 0)
            {
                RewardItemTB.Text = "";
                ItemAttachmentsLB.DataSource = null;
            }

            useraction = true;
        }
        public M_Rewards currentReward;
        private void RewardsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RewardsLB.SelectedItems.Count < 1) return;
            currentReward = RewardsLB.SelectedItem as M_Rewards;
            useraction = false;
            RewardItemTB.Text = currentReward.ItemName;
            ItemAttachmentsLB.DisplayMember = "Name";
            ItemAttachmentsLB.ValueMember = "Value";
            ItemAttachmentsLB.DataSource = currentReward.Attachments;

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
            KingOfTheHillConfig.m_UpdateInterval = m_UpdateIntervalNUD.Value;
            KingOfTheHillConfig.isDirty = true;
        }
        private void m_ServerStartDelayNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KingOfTheHillConfig.m_ServerStartDelay = m_ServerStartDelayNUD.Value;
            KingOfTheHillConfig.isDirty = true;
        }
        private void m_CaptureTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KingOfTheHillConfig.m_CaptureTime = m_CaptureTimeNUD.Value;
            KingOfTheHillConfig.isDirty = true;
        }
        private void m_HillEventIntervalNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KingOfTheHillConfig.m_HillEventInterval = m_HillEventIntervalNUD.Value;
            KingOfTheHillConfig.isDirty = true;
        }
        private void m_EventCleanupTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KingOfTheHillConfig.m_EventCleanupTime = m_EventCleanupTimeNUD.Value;
            KingOfTheHillConfig.isDirty = true;
        }
        private void m_EventPreStartNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KingOfTheHillConfig.m_EventPreStart = m_EventPreStartNUD.Value;
            KingOfTheHillConfig.isDirty = true;
        }
        private void m_PlayerPopulationToStartEventsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KingOfTheHillConfig.m_PlayerPopulationToStartEvents = (int)m_PlayerPopulationToStartEventsNUD.Value;
            KingOfTheHillConfig.isDirty = true;
        }
        private void m_MaxEventsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KingOfTheHillConfig.m_MaxEvents = (int)m_MaxEventsNUD.Value;
            KingOfTheHillConfig.isDirty = true;
        }
        private void m_DoLogsToCFCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KingOfTheHillConfig.m_DoLogsToCF = m_DoLogsToCFCB.Checked == true ? 1 : 0;
            KingOfTheHillConfig.isDirty = true;
        }
        private void m_EventPreStartMessageTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KingOfTheHillConfig.m_EventPreStartMessage = m_EventPreStartMessageTB.Text;
            KingOfTheHillConfig.isDirty = true;
        }
        private void m_EventCapturedMessageTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KingOfTheHillConfig.m_EventCapturedMessage = m_EventCapturedMessageTB.Text;
            KingOfTheHillConfig.isDirty = true;
        }
        private void m_EventDespawnedMessageTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KingOfTheHillConfig.m_EventDespawnedMessage = m_EventDespawnedMessageTB.Text;
            KingOfTheHillConfig.isDirty = true;
        }
        private void m_EventStartMessageTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KingOfTheHillConfig.m_EventStartMessage = m_EventStartMessageTB.Text;
            KingOfTheHillConfig.isDirty = true;
        }
        private void m_FlagNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            KingOfTheHillConfig.m_FlagName = m_FlagNameTB.Text;
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
            M_Hilllocations newHill = new M_Hilllocations()
            {
                Name = "New Hill",
                Position = new float[]
                {
                    centre[0],
                    centre[1],
                    centre[2]
                },
                Radius = 20,
                AISpawnCount = 20
            };
            KingOfTheHillConfig.m_HillLocations.Add(newHill);
            KingOfTheHillConfig.isDirty = true;
        }
        private void darkButton11_Click(object sender, EventArgs e)
        {
            KingOfTheHillConfig.m_HillLocations.Remove(currentHill);
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
            currentHill.Position[0] = (float)XNUD.Value;
            KingOfTheHillConfig.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void YNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentHill.Position[1] = (float)YNUD.Value;
            KingOfTheHillConfig.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void ZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentHill.Position[2] = (float)ZNUD.Value;
            KingOfTheHillConfig.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void RadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentHill.Radius = RadiusNUD.Value;
            KingOfTheHillConfig.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void AISpawnCountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentHill.AISpawnCount = (int)AISpawnCountNUD.Value;
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
                    YNUD.Value = (decimal)(MapData.gethieght(currentHill.Position[0], currentHill.Position[2]));
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
                KingOfTheHillConfig.m_Creatures.Remove(s);
                
            }
            KingOfTheHillConfig.isDirty = true;
        }
        private void darkButton7_Click(object sender, EventArgs e)
        {
            foreach (var item in ZombiestochoosefromLB.SelectedItems)
            {
                string zombie = item.ToString();
                if (!KingOfTheHillConfig.m_Creatures.Contains(zombie))
                {
                    KingOfTheHillConfig.m_Creatures.Add(zombie);
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
            M_Rewardpools Newpool = new M_Rewardpools()
            {
                RewardContainerName = "New Rweards Pool",
                m_Rewards = new BindingList<M_Rewards>()
            };
            KingOfTheHillConfig.m_RewardPools.Add(Newpool);
            KingOfTheHillConfig.isDirty = true;
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            KingOfTheHillConfig.m_RewardPools.Remove(CurrentrewardPool);
            KingOfTheHillConfig.isDirty = true;
            if (RewardPoolsLB.Items.Count == 0)
                RewardPoolsLB.SelectedIndex = -1;
            else
                RewardPoolsLB.SelectedIndex = 0;
        }
        private void RewardsPoolNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentrewardPool.RewardContainerName = RewardsPoolNameTB.Text;
            KingOfTheHillConfig.isDirty = true;
            RewardPoolsLB.Refresh();
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
                UseMultipleofSameItem = true
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    M_Rewards NewReward = new M_Rewards()
                    {
                        ItemName = l,
                        Attachments = new BindingList<string>()
                    };
                    CurrentrewardPool.m_Rewards.Add(NewReward);
                    KingOfTheHillConfig.isDirty = true;
                }
            }
        }
        private void darkButton3_Click(object sender, EventArgs e)
        {
            CurrentrewardPool.m_Rewards.Remove(currentReward);
            KingOfTheHillConfig.isDirty = true;
            if (RewardsLB.Items.Count == 0)
                RewardsLB.SelectedIndex = -1;
            else
                RewardsLB.SelectedIndex = 0;
        }
        private void RewardItemTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentReward.ItemName = RewardItemTB.Text;
            KingOfTheHillConfig.isDirty = true;
            RewardsLB.Refresh();
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
                    if (!currentReward.Attachments.Contains(l))
                    {
                        currentReward.Attachments.Add(l);
                        KingOfTheHillConfig.isDirty = true;
                    }
                    else
                        MessageBox.Show("Attachments Type allready in the list.....");
                }
            }
        }
        private void darkButton5_Click(object sender, EventArgs e)
        {
            currentReward.Attachments.Remove(ItemAttachmentsLB.GetItemText(ItemAttachmentsLB.SelectedItem));
            KingOfTheHillConfig.isDirty = true;
            if (ItemAttachmentsLB.Items.Count == 0)
                ItemAttachmentsLB.SelectedIndex = -1;
            else
                ItemAttachmentsLB.SelectedIndex = 0;
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


    }
}
