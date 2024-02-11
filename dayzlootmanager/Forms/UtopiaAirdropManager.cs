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
    public partial class UtopiaAirdropManager : DarkForm
    {
        public Project currentproject { get; set; }
        public string UtopiaAirdropsettingsPath { get; private set; }
        public string UtopiaAirdropLoggingsettingsPath { get; private set; }
        public string Projectname { get; private set; }

        private bool useraction;
        public UtopiaAirdropSettings UtopiaAirdropSettings;
        public UtopiaAirdropLoggingsettings UtopiaAirdropLoggingsettings;

        public UtopiaAirdropManager()
        {
            InitializeComponent();
            tabControl1.ItemSize = new Size(0, 1);
        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            SaveFile();
        }
        private void SaveFile()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (UtopiaAirdropSettings.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(UtopiaAirdropSettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(UtopiaAirdropSettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(UtopiaAirdropSettings.Filename, Path.GetDirectoryName(UtopiaAirdropSettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(UtopiaAirdropSettings.Filename) + ".bak", true);
                }
                UtopiaAirdropSettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(UtopiaAirdropSettings, options);
                File.WriteAllText(UtopiaAirdropSettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(UtopiaAirdropSettings.Filename));
            }

            if (UtopiaAirdropLoggingsettings.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(UtopiaAirdropLoggingsettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(UtopiaAirdropLoggingsettings.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(UtopiaAirdropLoggingsettings.Filename, Path.GetDirectoryName(UtopiaAirdropLoggingsettings.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(UtopiaAirdropLoggingsettings.Filename) + ".bak", true);
                }
                UtopiaAirdropLoggingsettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(UtopiaAirdropLoggingsettings, options);
                File.WriteAllText(UtopiaAirdropLoggingsettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(UtopiaAirdropLoggingsettings.Filename));
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
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\UtopiaAirdrop");
        }
        private void UtopiaAirdropManager_Load(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Projectname = currentproject.ProjectName;

            useraction = false;
            UtopiaAirdropsettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\UtopiaAirdrop\\Config\\UtopiaAirdropSettings.json";
            UtopiaAirdropSettings = JsonSerializer.Deserialize<UtopiaAirdropSettings>(File.ReadAllText(UtopiaAirdropsettingsPath));
            UtopiaAirdropSettings.isDirty = false;
            UtopiaAirdropSettings.Filename = UtopiaAirdropsettingsPath;

            UtopiaAirdropLoggingsettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\UtopiaAirdrop\\Logging\\Config\\LoggingSettings.json";
            UtopiaAirdropLoggingsettings = JsonSerializer.Deserialize<UtopiaAirdropLoggingsettings>(File.ReadAllText(UtopiaAirdropLoggingsettingsPath));
            UtopiaAirdropLoggingsettings.isDirty = false;
            UtopiaAirdropLoggingsettings.Filename = UtopiaAirdropLoggingsettingsPath;

            useraction = true;

        }
        private void UtopiaAirdropManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool needtosave = false;
            if (UtopiaAirdropSettings.isDirty)
            {
                needtosave = true;
            }
            if (UtopiaAirdropLoggingsettings.isDirty)
            {
                needtosave = true;
            }
            if (needtosave == true)
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
