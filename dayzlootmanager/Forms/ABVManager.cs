using DarkUI.Forms;
using DayZeLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows.Forms;

namespace DayZeEditor
{
    public partial class ABVManager : DarkForm
    {
        public Project currentproject { get; set; }
        public string AbandonedVehicleRemovePath { get; private set; }

        private bool useraction;
        public AbandonedVehicleRemover AbandonedVehicleRemover;

        public ABVManager()
        {
            InitializeComponent();
        }

        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void SaveFile()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (AbandonedVehicleRemover.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(AbandonedVehicleRemover.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(AbandonedVehicleRemover.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(AbandonedVehicleRemover.Filename, Path.GetDirectoryName(AbandonedVehicleRemover.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(AbandonedVehicleRemover.Filename) + ".bak", true);
                }
                AbandonedVehicleRemover.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(AbandonedVehicleRemover, options);
                File.WriteAllText(AbandonedVehicleRemover.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(AbandonedVehicleRemover.Filename));
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
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\AbandonedVehicleRemover");
        }

        private void ABVManager_Load(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;


            useraction = false;
            AbandonedVehicleRemovePath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\AbandonedVehicleRemover\\Settings.json";
            AbandonedVehicleRemover = JsonSerializer.Deserialize<AbandonedVehicleRemover>(File.ReadAllText(AbandonedVehicleRemovePath));
            AbandonedVehicleRemover.isDirty = false;
            AbandonedVehicleRemover.Filename = AbandonedVehicleRemovePath;

            LifetimeNUD.Value = AbandonedVehicleRemover.Lifetime;
            UpdateIntervalNUD.Value = AbandonedVehicleRemover.UpdateInterval;
            SaveIntervalNUD.Value = AbandonedVehicleRemover.SaveInterval;
            LoggingCB.Checked = AbandonedVehicleRemover.Logging == 1 ? true : false;

            useraction = true;

        }

        private void LifetimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AbandonedVehicleRemover.Lifetime = (int)LifetimeNUD.Value;
            AbandonedVehicleRemover.isDirty = true;
        }

        private void UpdateIntervalNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AbandonedVehicleRemover.UpdateInterval = (int)UpdateIntervalNUD.Value;
            AbandonedVehicleRemover.isDirty = true;
        }

        private void SaveIntervalNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AbandonedVehicleRemover.SaveInterval = (int)SaveIntervalNUD.Value;
            AbandonedVehicleRemover.isDirty = true;
        }

        private void LoggingCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            AbandonedVehicleRemover.Logging = LoggingCB.Checked == true ? 1 : 0;
            AbandonedVehicleRemover.isDirty = true;
        }

        private void ABVManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (AbandonedVehicleRemover.isDirty)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SaveFile();
                }
            }
        }
    }
}
