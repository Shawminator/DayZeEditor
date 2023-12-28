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
    public partial class DNAKeyCardsManager : DarkForm
    {
        public Project currentproject { get; set; }
        public string DNAKeyCardPAth { get; private set; }
        public string Projectname { get; private set; }

        public DNA_Keycards DNA_Keycards { get; set; }

        public bool useraction = true;

        public DNAKeyCardsManager()
        {
            InitializeComponent();
            tabControl1.ItemSize = new Size(0, 1);
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
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            SaveFiles();
        }

        private void SaveFiles()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            string message = "The Following Files were saved....\n";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            JsonSerializerOptions options2 = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                Converters = { new BoolConverter() }
            };
            if (DNA_Keycards.KeyCard_Main_System_Config.isDirty)
            {
                DNA_Keycards.KeyCard_Main_System_Config.isDirty = false;
                string jsonString = JsonSerializer.Serialize(DNA_Keycards.KeyCard_Main_System_Config, options);
                File.WriteAllText(DNA_Keycards.KeyCard_Main_System_Config.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(DNA_Keycards.KeyCard_Main_System_Config.Filename));
            }
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
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\DNA_Keycards");
        }
        private void DNAKeyCardsManager_Load(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Projectname = currentproject.ProjectName;
            DNAKeyCardPAth = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\";
            Console.WriteLine("serializing " + Path.GetFileName(DNAKeyCardPAth));
            DNA_Keycards = new DNA_Keycards(DNAKeyCardPAth);
            loadDNSKeycardSettings();
            loadDNSMobSettings();
            loadDNAResetTimerSettings();
        }

        private void loadDNSMobSettings()
        {
            useraction = false;

            m_DNAConfig_Mob_SystemLB.DisplayMember = "DisplayName";
            m_DNAConfig_Mob_SystemLB.ValueMember = "Value";
            m_DNAConfig_Mob_SystemLB.DataSource = DNA_Keycards.KeyCard_Mob_System_Config.m_DNAConfig_Mob_System;

            useraction = true;
        }
        private void m_DNAConfig_Mob_SystemLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_DNAConfig_Mob_SystemLB.SelectedItems.Count < 1) return;
            useraction = false;
            DNA_Config_Mob_System DMS = m_DNAConfig_Mob_SystemLB.SelectedItem as DNA_Config_Mob_System;
            dna_DefaultMobTB.Text = DMS.dna_DefaultMob;
            dna_MobTypeTB.Text = DMS.dna_MobType;
            useraction = true;
        }
        private void loadDNSKeycardSettings()
        {
            useraction = false;

            m_DNAConfig_Main_SystemLB.DisplayMember = "DisplayName";
            m_DNAConfig_Main_SystemLB.ValueMember = "Value";
            m_DNAConfig_Main_SystemLB.DataSource = DNA_Keycards.KeyCard_Main_System_Config.m_DNAConfig_Main_System;

            useraction = true;
        }
        private void m_DNAConfig_Main_SystemLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_DNAConfig_Main_SystemLB.SelectedItems.Count < 1) return;
            useraction = false;
            DNA_Config_Main_System CMS = m_DNAConfig_Main_SystemLB.SelectedItem as DNA_Config_Main_System;
            dna_SettingNUD.Value = CMS.dna_Setting;
            useraction = true;
        }
        private void DNAKeyCardsManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool savefiles = false;
            if (DNA_Keycards.KeyCard_Main_System_Config.isDirty)
            {
                savefiles = true;
            }
            if (savefiles == true)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SaveFiles();
                }
            }
        }

        private void Crate_LocationsCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (Crate_LocationsCB.GetItemText(Crate_LocationsCB.SelectedItem))
            {
                case "Yellow":
                    CrateLocationLB.DataSource = DNA_Keycards.KeyCard_Main_System_Config.m_DNAYellow_Crate_Locations;
                    break;
                case "Green":
                    CrateLocationLB.DataSource = DNA_Keycards.KeyCard_Main_System_Config.m_DNAGreen_Crate_Locations;
                    break;
                case "Blue":
                    CrateLocationLB.DataSource = DNA_Keycards.KeyCard_Main_System_Config.m_DNABlue_Crate_Locations;
                    break;
                case "Purple":
                    CrateLocationLB.DataSource = DNA_Keycards.KeyCard_Main_System_Config.m_DNAPurple_Crate_Locations;
                    break;
                case "Red":
                    CrateLocationLB.DataSource = DNA_Keycards.KeyCard_Main_System_Config.m_DNARed_Crate_Locations;
                    break;
            }

        }
        private void CrateLocationLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CrateLocationLB.SelectedItems.Count < 1) return;
            useraction = false;
            DNA_Crate_Locations CL = CrateLocationLB.SelectedItem as DNA_Crate_Locations;
            CrateLocationPOSXNUD.Value = Convert.ToDecimal(CL.dna_Location.TrimStart().TrimEnd().Split(' ')[0]);
            CrateLocationPOSYNUD.Value = Convert.ToDecimal(CL.dna_Location.TrimStart().TrimEnd().Split(' ')[1]);
            CrateLocationPOSZNUD.Value = Convert.ToDecimal(CL.dna_Location.TrimStart().TrimEnd().Split(' ')[2]);

            CrateLocationROTXNUD.Value = Convert.ToDecimal(CL.dna_Rotation.TrimStart().TrimEnd().Split(' ')[0]);
            CrateLocationROTYNUD.Value = Convert.ToDecimal(CL.dna_Rotation.TrimStart().TrimEnd().Split(' ')[1]);
            CrateLocationROTZNUD.Value = Convert.ToDecimal(CL.dna_Rotation.TrimStart().TrimEnd().Split(' ')[2]);

            useraction = true;
        }
        private void Strongroom_LocationsCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (Strongroom_LocationsCB.GetItemText(Strongroom_LocationsCB.SelectedItem))
            {
                case "Yellow":
                    StrongRoomLocationLB.DataSource = DNA_Keycards.KeyCard_Main_System_Config.m_DNAYellow_Strongroom_Locations;
                    break;
                case "Green":
                    StrongRoomLocationLB.DataSource = DNA_Keycards.KeyCard_Main_System_Config.m_DNAGreen_Strongroom_Locations;
                    break;
                case "Blue":
                    StrongRoomLocationLB.DataSource = DNA_Keycards.KeyCard_Main_System_Config.m_DNABlue_Strongroom_Locations;
                    break;
                case "Purple":
                    StrongRoomLocationLB.DataSource = DNA_Keycards.KeyCard_Main_System_Config.m_DNAPurple_Strongroom_Locations;
                    break;
                case "Red":
                    StrongRoomLocationLB.DataSource = DNA_Keycards.KeyCard_Main_System_Config.m_DNARed_Strongroom_Locations;
                    break;
            }
        }
        private void StrongRoomLocationLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StrongRoomLocationLB.SelectedItems.Count < 1) return;
            useraction = false;
            DNA_Strongroom_Locations SRL = StrongRoomLocationLB.SelectedItem as DNA_Strongroom_Locations;
            StrongRoomLocationPOSXNUD.Value = Convert.ToDecimal(SRL.dna_Location.Split(' ')[0]);
            StrongRoomLocationPOSYNUD.Value = Convert.ToDecimal(SRL.dna_Location.Split(' ')[1]);
            StrongRoomLocationPOSZNUD.Value = Convert.ToDecimal(SRL.dna_Location.Split(' ')[2]);

            StrongRoomLocationROTXNUD.Value = Convert.ToDecimal(SRL.dna_Rotation.Split(' ')[0]);
            StrongRoomLocationROTYNUD.Value = Convert.ToDecimal(SRL.dna_Rotation.Split(' ')[1]);
            StrongRoomLocationROTZNUD.Value = Convert.ToDecimal(SRL.dna_Rotation.Split(' ')[2]);

            useraction = true;
        }
        private void dna_SettingNUD_ValueChanged(object sender, EventArgs e)
        {
            if (m_DNAConfig_Main_SystemLB.SelectedItems.Count < 1) return;
            if (!useraction) return;
            DNA_Config_Main_System CMS = m_DNAConfig_Main_SystemLB.SelectedItem as DNA_Config_Main_System;
            CMS.dna_Setting = (int)dna_SettingNUD.Value;
            DNA_Keycards.KeyCard_Main_System_Config.isDirty = true;
        }

        private void loadDNAResetTimerSettings()
        {
            useraction = false;

            ResetTimerLB.DisplayMember = "DisplayName";
            ResetTimerLB.ValueMember = "Value";
            ResetTimerLB.DataSource = DNA_Keycards.DNA_ResetTimer_Settings.GetType().GetProperties();

            useraction = true;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MainSettingsButton.Checked = false;
            MobButton.Checked = false;
            ResetTimerButton.Checked = false;
        }
        private void MainSettingsButton_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            if (tabControl1.SelectedIndex == 0)
                MainSettingsButton.Checked = true;
        }
        private void MobButton_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            if (tabControl1.SelectedIndex == 1)
                MobButton.Checked = true;
        }
        private void ResetTimerButton_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
            if (tabControl1.SelectedIndex == 2)
                ResetTimerButton.Checked = true;
        }

        private void ResetTimerLB_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
