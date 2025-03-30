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
    public partial class TimedCrateManager : DarkForm
    {
        public Project currentproject { get; set; }
        public string TimedCreateSettingsPath { get; private set; }
        public string TimedCreateLootPath { get; private set; }
        public string Projectname { get; private set; }

        private bool useraction;
        public TimedCrates TimedCrates;
        public TimedCrateLoot TimedCrateLoot;

        public TimedCrateManager()
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
            if (TimedCrates.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(TimedCrates.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(TimedCrates.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(TimedCrates.Filename, Path.GetDirectoryName(TimedCrates.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(TimedCrates.Filename) + ".bak", true);
                }
                TimedCrates.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(TimedCrates, options);
                File.WriteAllText(TimedCrates.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(TimedCrates.Filename));
            }
            if (TimedCrateLoot.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(TimedCrateLoot.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(TimedCrateLoot.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(TimedCrateLoot.Filename, Path.GetDirectoryName(TimedCrateLoot.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(TimedCrateLoot.Filename) + ".bak", true);
                }
                TimedCrateLoot.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(TimedCrateLoot, options);
                File.WriteAllText(TimedCrateLoot.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(TimedCrateLoot.Filename));
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
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\MB_TimedCrate");
        }
        private void TimedCrateManager_Load(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Projectname = currentproject.ProjectName;

            useraction = false;
            TimedCreateSettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\MB_TimedCrate\\CrateSettings.json";
            TimedCrates = JsonSerializer.Deserialize<TimedCrates>(File.ReadAllText(TimedCreateSettingsPath));
            TimedCrates.isDirty = false;
            TimedCrates.Filename = TimedCreateSettingsPath;

            TimedCreateLootPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\MB_TimedCrate\\CustomLootData.json";
            TimedCrateLoot = JsonSerializer.Deserialize<TimedCrateLoot>(File.ReadAllText(TimedCreateLootPath));
            TimedCrateLoot.isDirty = false;
            TimedCrateLoot.Filename = TimedCreateLootPath;

            LoadTimedCreateTreeView();

            useraction = true;

        }
        private void TimedCrateManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TimedCrates.isDirty || TimedCrateLoot.isDirty)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SaveFile();
                }
            }
        }

        public void LoadTimedCreateTreeView()
        {
            Cursor.Current = Cursors.WaitCursor;
            TimedCrateTV.Nodes.Clear();
            TreeNode RootNode = new TreeNode("Timed Crates:-")
            {
                Tag = "ParentRoot"
            };
            RootNode.Nodes.Add(AddSettings());
            RootNode.Nodes.Add(AddLoot());
            TimedCrateTV.Nodes.Add(RootNode);
            Cursor.Current = Cursors.WaitCursor;
        }

        private TreeNode AddSettings()
        {
            TreeNode TCBaseNode = new TreeNode("settings")
            {
                Tag = TimedCrates
            };
            TCBaseNode.Nodes.Add(new TreeNode($"Server Start Grace Period: - {TimedCrates.ServerStartGracePeriod}")
            {
                Tag = "ServerStartGracePeriod",
                Name = "ServerStartGracePeriod"
            });
            TreeNode CreateConfigNode = new TreeNode($"Crates:")
            {
                Tag = "Crateconfigs",
                Name = "Crateconfigs"
            };
            for( int i = 1; i <= TimedCrates.CrateConfigs.Count; i++)
            {
                CreateConfigNode.Nodes.Add(new TreeNode($"Crate {i.ToString()}")
                {
                    Tag = TimedCrates.CrateConfigs[i-1],
                    Name = "Crateconfig"
                });
            }
            TCBaseNode.Nodes.Add(CreateConfigNode);
            return TCBaseNode;
        }
        private TreeNode AddLoot()
        {
            TreeNode KRBaseNode = new TreeNode("Loot")
            {
                Tag = TimedCrateLoot
            };
            
            return KRBaseNode;
        }
    }
}
