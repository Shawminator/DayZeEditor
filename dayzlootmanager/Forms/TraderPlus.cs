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
    public partial class TraderPlus : DarkForm
    {
        public Project currentproject { get; set; }
        public TypesFile vanillatypes;
        public List<TypesFile> ModTypes;
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

        public MapData MapData { get; private set; }

        public string TraderPlusBankingConfigPath { get; private set; }
        public TraderPlusBankingConfig TraderPlusBankingConfig { get; private set; }
        public string TraderPlusGarageConfigPath { get; private set; }
        public TraderPlusGarageConfig TraderPlusGarageConfig { get; private set; }
        public string TraderPlusGeneralConfigPath { get; private set; }
        public TraderPlusGeneralConfig TraderPlusGeneralConfig { get; private set; }
        public string TraderPlusVehiclesConfigPath { get; private set; }
        public TraderPlusVehiclesConfig TraderPlusVehiclesConfig { get; private set; }
        public string TraderPlusSafeZoneConfigPath { get; private set; }
        public TraderPlusSafeZoneConfig TraderPlusSafeZoneConfig { get; private set; }
        public string TraderPlusPriceConfigPath { get; private set; }
        public TraderPlusPriceConfig TraderPlusPriceConfig { get; private set; }
        public string TraderPlusIDsConfigPath { get; private set; }
        public TraderPlusIDsConfig TraderPlusIDsConfig { get; private set; }
        public string TraderPlusInsuranceConfigPath { get; private set; }
        public TraderPlusInsuranceConfig TraderPlusInsuranceConfig { get; private set; }

        public string traderPlusDatabasePath { get; private set; }
        public BindingList<TraderPlusStock> TraderPlusStock;


        private bool needtosave;

        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            SaveTraderfiles();
        }
        public void SaveTraderfiles()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (TraderPlusBankingConfig.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(TraderPlusBankingConfig.FullFilename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(TraderPlusBankingConfig.FullFilename) + "\\Backup\\" + SaveTime);
                    File.Copy(TraderPlusBankingConfig.FullFilename, Path.GetDirectoryName(TraderPlusBankingConfig.FullFilename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(TraderPlusBankingConfig.FullFilename) + ".bak", true);
                }
                TraderPlusBankingConfig.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(TraderPlusBankingConfig, options);
                File.WriteAllText(TraderPlusBankingConfig.FullFilename, jsonString);
                midifiedfiles.Add(Path.GetFileName(TraderPlusBankingConfig.fileName));
            }
            if (TraderPlusGarageConfig.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(TraderPlusGarageConfig.FullFilename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(TraderPlusGarageConfig.FullFilename) + "\\Backup\\" + SaveTime);
                    File.Copy(TraderPlusGarageConfig.FullFilename, Path.GetDirectoryName(TraderPlusGarageConfig.FullFilename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(TraderPlusGarageConfig.FullFilename) + ".bak", true);
                }
                TraderPlusGarageConfig.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(TraderPlusGarageConfig, options);
                File.WriteAllText(TraderPlusGarageConfig.FullFilename, jsonString);
                midifiedfiles.Add(Path.GetFileName(TraderPlusGarageConfig.fileName));
            }
            if (TraderPlusGeneralConfig.isDirty)
            {
                TraderPlusGeneralConfig.isDirty = false;
                TraderPlusGeneralConfig.SaveCurrencies();
                TraderPlusGeneralConfig.SaveIDS(TraderPlusIDsConfig);
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(TraderPlusGeneralConfig, options);
                File.WriteAllText(TraderPlusGeneralConfig.FullFilename, jsonString);
                midifiedfiles.Add(Path.GetFileName(TraderPlusGeneralConfig.fileName));
            }
            if (TraderPlusVehiclesConfig.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(TraderPlusVehiclesConfig.FullFilename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(TraderPlusVehiclesConfig.FullFilename) + "\\Backup\\" + SaveTime);
                    File.Copy(TraderPlusVehiclesConfig.FullFilename, Path.GetDirectoryName(TraderPlusVehiclesConfig.FullFilename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(TraderPlusVehiclesConfig.FullFilename) + ".bak", true);
                }
                TraderPlusVehiclesConfig.isDirty = false;
                TraderPlusVehiclesConfig.setInsurances(TraderPlusInsuranceConfig);
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(TraderPlusVehiclesConfig, options);
                File.WriteAllText(TraderPlusVehiclesConfig.FullFilename, jsonString);
                midifiedfiles.Add(Path.GetFileName(TraderPlusVehiclesConfig.fileName));
            }
            if (TraderPlusInsuranceConfig.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(TraderPlusInsuranceConfig.FullFilename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(TraderPlusInsuranceConfig.FullFilename) + "\\Backup\\" + SaveTime);
                    File.Copy(TraderPlusInsuranceConfig.FullFilename, Path.GetDirectoryName(TraderPlusInsuranceConfig.FullFilename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(TraderPlusInsuranceConfig.FullFilename) + ".bak", true);
                }
                TraderPlusInsuranceConfig.isDirty = false;
                TraderPlusInsuranceConfig.setInsurers(TraderPlusGeneralConfig);
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(TraderPlusInsuranceConfig, options);
                File.WriteAllText(TraderPlusInsuranceConfig.FullFilename, jsonString);
                midifiedfiles.Add(Path.GetFileName(TraderPlusInsuranceConfig.fileName));
            }
            if (TraderPlusSafeZoneConfig.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(TraderPlusSafeZoneConfig.FullFilename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(TraderPlusSafeZoneConfig.FullFilename) + "\\Backup\\" + SaveTime);
                    File.Copy(TraderPlusSafeZoneConfig.FullFilename, Path.GetDirectoryName(TraderPlusSafeZoneConfig.FullFilename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(TraderPlusSafeZoneConfig.FullFilename) + ".bak", true);
                }
                TraderPlusSafeZoneConfig.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(TraderPlusSafeZoneConfig, options);
                File.WriteAllText(TraderPlusSafeZoneConfig.FullFilename, jsonString);
                midifiedfiles.Add(Path.GetFileName(TraderPlusSafeZoneConfig.fileName));
            }
            if (TraderPlusPriceConfig.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(TraderPlusPriceConfig.FullFilename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(TraderPlusPriceConfig.FullFilename) + "\\Backup\\" + SaveTime);
                    File.Copy(TraderPlusPriceConfig.FullFilename, Path.GetDirectoryName(TraderPlusPriceConfig.FullFilename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(TraderPlusPriceConfig.FullFilename) + ".bak", true);
                }
                TraderPlusPriceConfig.isDirty = false;
                TraderPlusPriceConfig.SetProducts();
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(TraderPlusPriceConfig, options);
                File.WriteAllText(TraderPlusPriceConfig.FullFilename, jsonString);
                midifiedfiles.Add(Path.GetFileName(TraderPlusPriceConfig.fileName));
            }
            if (TraderPlusIDsConfig.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(TraderPlusIDsConfig.FullFilename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(TraderPlusIDsConfig.FullFilename) + "\\Backup\\" + SaveTime);
                    File.Copy(TraderPlusIDsConfig.FullFilename, Path.GetDirectoryName(TraderPlusIDsConfig.FullFilename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(TraderPlusIDsConfig.FullFilename) + ".bak", true);
                }
                TraderPlusIDsConfig.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(TraderPlusIDsConfig, options);
                File.WriteAllText(TraderPlusIDsConfig.FullFilename, jsonString);
                midifiedfiles.Add(Path.GetFileName(TraderPlusIDsConfig.fileName));
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
        public TraderPlus()
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
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
            if (tabControl1.SelectedIndex == 3)
                toolStripButton5.Checked = true;
        }
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 4;
            if (tabControl1.SelectedIndex == 4)
                toolStripButton6.Checked = true;
        }
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 5;
            if (tabControl1.SelectedIndex == 5)
                toolStripButton7.Checked = true;
        }
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 6;
            if (tabControl1.SelectedIndex == 6)
                toolStripButton8.Checked = true;
        }
        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 7;
            if (tabControl1.SelectedIndex == 7)
                toolStripButton10.Checked = true;

        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripMenuItem1.Visible = false;
            categoriesToolStripMenuItem.Visible = false;
            toolStripButton1.Checked = false;
            toolStripButton3.Checked = false;
            toolStripButton4.Checked = false;
            toolStripButton5.Checked = false;
            toolStripButton6.Checked = false;
            toolStripButton7.Checked = false;
            toolStripButton8.Checked = false;
            toolStripButton10.Checked = false;
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
                case 3:
                    toolStripButton5.Checked = true;
                    toolStripMenuItem1.Visible = true;
                    break;
                case 4:
                    toolStripButton6.Checked = true;
                    categoriesToolStripMenuItem.Visible = true;
                    break;
                case 5:
                    toolStripButton7.Checked = true;
                    break;
                case 6:
                    toolStripButton8.Checked = true;
                    break;
                case 7:
                    toolStripButton10.Checked = true;
                    break;
                default:
                    break;
            }
        }
        private void TraderPlus_Load(object sender, EventArgs e)
        {
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            doubleClickTimer.Interval = 100;
            doubleClickTimer.Tick += new EventHandler(doubleClickTimer_Tick);

            TraderPlusBankingConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\TraderPlus\\TraderPlusConfig\\TraderPlusBankingConfig.json";
            if (!File.Exists(TraderPlusBankingConfigPath))
            {
                TraderPlusBankingConfig = new TraderPlusBankingConfig();
                needtosave = true;
            }
            else
            {
                TraderPlusBankingConfig = JsonSerializer.Deserialize<TraderPlusBankingConfig>(File.ReadAllText(TraderPlusBankingConfigPath));
                TraderPlusBankingConfig.isDirty = false;
                if(!TraderPlusBankingConfig.CheckVersion())
                {
                    TraderPlusBankingConfig.isDirty = true;
                    needtosave = true;
                }
            }
            TraderPlusBankingConfig.FullFilename = TraderPlusBankingConfigPath;
            SetupTraderPlusBankingConfig();

            TraderPlusGarageConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\TraderPlus\\TraderPlusConfig\\TraderPlusGarageConfig.json";
            if (!File.Exists(TraderPlusGarageConfigPath))
            {
                TraderPlusGarageConfig = new TraderPlusGarageConfig();
                needtosave = true;
            }
            else
            {
                TraderPlusGarageConfig = JsonSerializer.Deserialize<TraderPlusGarageConfig>(File.ReadAllText(TraderPlusGarageConfigPath));
                TraderPlusGarageConfig.isDirty = false;
                if (!TraderPlusGarageConfig.CheckVersion())
                {
                    TraderPlusGarageConfig.isDirty = true;
                    needtosave = true;
                }
            }
            TraderPlusGarageConfig.FullFilename = TraderPlusGarageConfigPath;
            SetupTraderPlusGarageConfig();

            TraderPlusInsuranceConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\TraderPlus\\TraderPlusConfig\\TraderPlusInsuranceConfig.json";
            if (!File.Exists(TraderPlusInsuranceConfigPath))
            {
                TraderPlusInsuranceConfig = new TraderPlusInsuranceConfig();
                needtosave = true;
            }
            else
            {
                TraderPlusInsuranceConfig = JsonSerializer.Deserialize<TraderPlusInsuranceConfig>(File.ReadAllText(TraderPlusInsuranceConfigPath));
                TraderPlusInsuranceConfig.isDirty = false;
                if (!TraderPlusInsuranceConfig.CheckVersion())
                {
                    TraderPlusInsuranceConfig.isDirty = true;
                    needtosave = true;
                }
            }
            TraderPlusInsuranceConfig.FullFilename = TraderPlusInsuranceConfigPath;

            TraderPlusPriceConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\TraderPlus\\TraderPlusConfig\\TraderPlusPriceConfig.json";
            if (!File.Exists(TraderPlusPriceConfigPath))
            {
                TraderPlusPriceConfig = new TraderPlusPriceConfig();
                needtosave = true;
            }
            else
            {
                TraderPlusPriceConfig = JsonSerializer.Deserialize<TraderPlusPriceConfig>(File.ReadAllText(TraderPlusPriceConfigPath));
                TraderPlusPriceConfig.isDirty = false;
                if (!TraderPlusPriceConfig.CheckVersion())
                {
                    TraderPlusPriceConfig.isDirty = true;
                    needtosave = true;
                }

            }
            TraderPlusPriceConfig.FullFilename = TraderPlusPriceConfigPath;
            SetupTraderPlusPriceConfig();

            TraderPlusIDsConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\TraderPlus\\TraderPlusConfig\\TraderPlusIDsConfig.json";
            if (!File.Exists(TraderPlusIDsConfigPath))
            {
                TraderPlusIDsConfig = new TraderPlusIDsConfig();
                needtosave = true;
            }
            else
            {
                TraderPlusIDsConfig = JsonSerializer.Deserialize<TraderPlusIDsConfig>(File.ReadAllText(TraderPlusIDsConfigPath));
                TraderPlusIDsConfig.isDirty = false;
                if(!TraderPlusIDsConfig.CheckVersion())
                {
                    TraderPlusIDsConfig.isDirty = true;
                    needtosave = true;
                }
            }
            TraderPlusIDsConfig.FullFilename = TraderPlusIDsConfigPath;

            TraderPlusGeneralConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\TraderPlus\\TraderPlusConfig\\TraderPlusGeneralConfig.json";
            if (!File.Exists(TraderPlusGeneralConfigPath))
            {
                TraderPlusGeneralConfig = new TraderPlusGeneralConfig();
                needtosave = true;
            }
            else
            {
                TraderPlusGeneralConfig = JsonSerializer.Deserialize<TraderPlusGeneralConfig>(File.ReadAllText(TraderPlusGeneralConfigPath));
                TraderPlusGeneralConfig.SortTradersByIndex();
                TraderPlusGeneralConfig.getBankers();
                TraderPlusGeneralConfig.SortCurriences();
                TraderPlusGeneralConfig.getallcurenciesclassnames();
                TraderPlusGeneralConfig.GetCategoriesbyID(TraderPlusIDsConfig);
                TraderPlusGeneralConfig.getInsurers(TraderPlusInsuranceConfig);
                TraderPlusGeneralConfig.isDirty = false;
                if (!TraderPlusGeneralConfig.CheckVersion())
                {
                    needtosave = true;
                    TraderPlusGeneralConfig.isDirty = true;
                }
                
            }
            TraderPlusGeneralConfig.FullFilename = TraderPlusGeneralConfigPath;
            SetupTraderPlusGeneralConfig();



            TraderPlusSafeZoneConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\TraderPlus\\TraderPlusConfig\\TraderPlusSafeZoneConfig.json";
            if (!File.Exists(TraderPlusSafeZoneConfigPath))
            {
                TraderPlusSafeZoneConfig = new TraderPlusSafeZoneConfig();
                needtosave = true;
            }
            else
            {
                TraderPlusSafeZoneConfig = JsonSerializer.Deserialize<TraderPlusSafeZoneConfig>(File.ReadAllText(TraderPlusSafeZoneConfigPath));
                TraderPlusSafeZoneConfig.isDirty = false;
                if (!TraderPlusSafeZoneConfig.CheckVersion())
                {
                    TraderPlusSafeZoneConfig.isDirty = true;
                    needtosave = true;
                }
            }
            TraderPlusSafeZoneConfig.FullFilename = TraderPlusSafeZoneConfigPath;
            SetupTraderPlusSafeZoneConfig();

            TraderPlusVehiclesConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\TraderPlus\\TraderPlusConfig\\TraderPlusVehiclesConfig.json";
            if (!File.Exists(TraderPlusVehiclesConfigPath))
            {
                TraderPlusVehiclesConfig = new TraderPlusVehiclesConfig();
                needtosave = true;
            }
            else
            {
                TraderPlusVehiclesConfig = JsonSerializer.Deserialize<TraderPlusVehiclesConfig>(File.ReadAllText(TraderPlusVehiclesConfigPath));
                if (TraderPlusVehiclesConfig.getInsurance(TraderPlusInsuranceConfig))
                {
                    TraderPlusVehiclesConfig.setInsurances(TraderPlusInsuranceConfig);
                    needtosave = true;
                }
                else
                {
                    TraderPlusVehiclesConfig.isDirty = false;
                }
                if (!TraderPlusVehiclesConfig.CheckVersion())
                {
                    TraderPlusVehiclesConfig.isDirty = true;
                    needtosave = true;
                }
            }
            TraderPlusVehiclesConfig.FullFilename = TraderPlusVehiclesConfigPath;
            SetupTraderPlusVehiclesConfig();

            traderPlusDatabasePath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\TraderPlus\\TraderPlusDatabase";
            TraderPlusStock = new BindingList<TraderPlusStock>();
            DirectoryInfo dinfo = new DirectoryInfo(traderPlusDatabasePath);
            FileInfo[] Files = dinfo.GetFiles("*.json");
            Console.WriteLine("Getting TraderPlus Stock Database....");
            Console.WriteLine(Files.Length.ToString() + " Found");
            foreach (FileInfo file in Files)
            {
                TraderPlusStock stock = JsonSerializer.Deserialize<TraderPlusStock>(File.ReadAllText(file.FullName));
                stock.fileName = file.Name;
                stock.FullFilename = file.FullName;
                stock.isDirty = false;
                TraderPlusStock.Add(stock);
            }

            MapData = new MapData(Application.StartupPath + currentproject.MapPath + ".xyz");

            pictureBox2.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Livonia maop size is 12800 x 12800, 0,0 bottom left, center 6400 x 6400
            pictureBox2.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox2.Paint += new PaintEventHandler(DrawAllSafeZones);
            trackBar4.Value = 1;
            SetSafeZonescale();

            if (needtosave)
            {
                SaveTraderfiles();
            }
        }

        #region BankingConfig
        private void SetupTraderPlusBankingConfig()
        {
            useraction = false;
            IsCreditCarNeededForTransactionCB.Checked = TraderPlusBankingConfig.IsCreditCarNeededForTransaction == 1 ? true : false;
            TransactionFeesNUD.Value = (decimal)TraderPlusBankingConfig.TransactionFees;
            DefaultStartCurrencyNUD.Value = (int)TraderPlusBankingConfig.DefaultStartCurrency;
            DefaultMaxCurrencyNUD.Value = (int)TraderPlusBankingConfig.DefaultMaxCurrency;
            TheAmountHasBeenTransferedToTheAccountTB.Text = TraderPlusBankingConfig.TheAmountHasBeenTransferedToTheAccount;
            TheAmountErrorTransferAccountTB.Text = TraderPlusBankingConfig.TheAmountErrorTransferAccount;
            BankingLogsCB.Checked = TraderPlusBankingConfig.BankingLogs == 1 ? true : false;

            BankingConfigAcceptedCurrenciesLB.DisplayMember = "Name";
            BankingConfigAcceptedCurrenciesLB.ValueMember = "Value";
            BankingConfigAcceptedCurrenciesLB.DataSource = TraderPlusBankingConfig.CurrenciesAccepted;

            useraction = true;
        }
        private void darkButton8_Click(object sender, EventArgs e)
        {
            ATMAvailablecurrencyGroupBox.Visible = true;
            List<string> returnlist = TraderPlusGeneralConfig.getcurrencies();
            ATMAvailableCurrencyLB.Items.Clear();
            ATMAvailableCurrencyLB.Items.AddRange(returnlist.ToArray());
        }
        private void darkButton7_Click(object sender, EventArgs e)
        {
            string removeitem = BankingConfigAcceptedCurrenciesLB.GetItemText(BankingConfigAcceptedCurrenciesLB.SelectedItem);
            TraderPlusBankingConfig.CurrenciesAccepted.Remove(removeitem);
            TraderPlusBankingConfig.isDirty = true;
        }
        private void darkButton52_Click(object sender, EventArgs e)
        {
            foreach (var item in ATMAvailableCurrencyLB.SelectedItems)
            {
                string currency = item as string;
                if (!TraderPlusBankingConfig.CurrenciesAccepted.Contains(currency))
                    TraderPlusBankingConfig.CurrenciesAccepted.Add(currency);
            }
            TraderPlusBankingConfig.isDirty = true;
            ATMAvailablecurrencyGroupBox.Visible = false;
        }
        private void darkButton51_Click(object sender, EventArgs e)
        {
            ATMAvailablecurrencyGroupBox.Visible = false;
        }
        private void IsCreditCarNeededForTransactionCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusBankingConfig.IsCreditCarNeededForTransaction = IsCreditCarNeededForTransactionCB.Checked == true ? 1 : 0;
            TraderPlusBankingConfig.isDirty = true;
        }
        private void TransactionFeesNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusBankingConfig.TransactionFees = (float)TransactionFeesNUD.Value;
            TraderPlusBankingConfig.isDirty = true;
        }
        private void DefaultStartCurrencyNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusBankingConfig.DefaultStartCurrency = (int)DefaultStartCurrencyNUD.Value;
            TraderPlusBankingConfig.isDirty = true;
        }
        private void DefaultMaxCurrencyNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusBankingConfig.DefaultMaxCurrency = (int)DefaultMaxCurrencyNUD.Value;
            TraderPlusBankingConfig.isDirty = true;
        }
        private void TheAmountHasBeenTransferedToTheAccountTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusBankingConfig.TheAmountHasBeenTransferedToTheAccount = TheAmountHasBeenTransferedToTheAccountTB.Text;
            TraderPlusBankingConfig.isDirty = true;
        }
        private void TheAmountErrorTransferAccountTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusBankingConfig.TheAmountErrorTransferAccount = TheAmountErrorTransferAccountTB.Text;
            TraderPlusBankingConfig.isDirty = true;
        }
        private void BankingLogsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusBankingConfig.BankingLogs = BankingLogsCB.Checked == true ? 1 : 0;
            TraderPlusBankingConfig.isDirty = true;
        }
        #endregion Bankingconfig

        #region GarageConfig
        public Npc CurrentgarageNPC { get; set; }


        private void SetupTraderPlusGarageConfig()
        {
            useraction = false;
            VehicleMustHaveLockCB.Checked = TraderPlusGarageConfig.VehicleMustHaveLock == 1 ? true : false;
            SaveVehicleCargoCB.Checked = TraderPlusGarageConfig.SaveVehicleCargo == 1 ? true : false;
            SaveVehicleHealthCB.Checked = TraderPlusGarageConfig.SaveVehicleHealth == 1 ? true : false;
            SaveVehicleFuelCB.Checked = TraderPlusGarageConfig.SaveVehicleFuel == 1 ? true : false;
            PayWithBankAccountCB.Checked = TraderPlusGarageConfig.PayWithBankAccount == 1 ? true : false;
            ParkInCostNUD.Value = (int)TraderPlusGarageConfig.ParkInCost;
            ParkOutCostNUD.Value = (int)TraderPlusGarageConfig.ParkOutCost;
            ParkingNotAvailableTB.Text = TraderPlusGarageConfig.ParkingNotAvailable;
            NotEnoughMoneyTB.Text = TraderPlusGarageConfig.NotEnoughMoney;
            NotRightToParkTB.Text = TraderPlusGarageConfig.NotRightToPark;
            CarHasMemberTB.Text = TraderPlusGarageConfig.CarHasMember;
            ParkInFailTB.Text = TraderPlusGarageConfig.ParkInFail;
            ParkInSuccessTB.Text = TraderPlusGarageConfig.ParkInSuccess;
            ParkOutFailTB.Text = TraderPlusGarageConfig.ParkOutFail;
            ParkOutSuccessTB.Text = TraderPlusGarageConfig.ParkOutSuccess;
            MaxVehicleStoredReachedTB.Text = TraderPlusGarageConfig.MaxVehicleStoredReached;
            TradeVehicleWarningTB.Text = TraderPlusGarageConfig.TradeVehicleWarning;
            TradeVehicleHasBeenDeletedTB.Text = TraderPlusGarageConfig.TradeVehicleHasBeenDeleted;

            GarageWhiteListLB.DisplayMember = "Name";
            GarageWhiteListLB.ValueMember = "Value";
            GarageWhiteListLB.DataSource = TraderPlusGarageConfig.WhitelistedObjects;

            GarageNPCLB.DisplayMember = "Name";
            GarageNPCLB.ValueMember = "Value";
            GarageNPCLB.DataSource = TraderPlusGarageConfig.NPCs;

            useraction = true;
        }
        private void GarageNPCLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GarageNPCLB.SelectedItems.Count < 1) return;
            CurrentgarageNPC = GarageNPCLB.SelectedItem as Npc;
            NPCsClassNameTB.Text = CurrentgarageNPC.ClassName;
            numericUpDown1.Value = (decimal)CurrentgarageNPC.Position[0];
            numericUpDown2.Value = (decimal)CurrentgarageNPC.Position[1];
            numericUpDown3.Value = (decimal)CurrentgarageNPC.Position[2];

            numericUpDown4.Value = (decimal)CurrentgarageNPC.Orientation[0];
            numericUpDown5.Value = (decimal)CurrentgarageNPC.Orientation[1];
            numericUpDown6.Value = (decimal)CurrentgarageNPC.Orientation[2];

            numericUpDown7.Value = (decimal)CurrentgarageNPC.ParkingPosition[0];
            numericUpDown8.Value = (decimal)CurrentgarageNPC.ParkingPosition[1];
            numericUpDown9.Value = (decimal)CurrentgarageNPC.ParkingPosition[2];

            numericUpDown10.Value = (decimal)CurrentgarageNPC.ParkingOrientation[0];
            numericUpDown11.Value = (decimal)CurrentgarageNPC.ParkingOrientation[1];
            numericUpDown12.Value = (decimal)CurrentgarageNPC.ParkingOrientation[2];

            GarageNPCClothesLB.DisplayMember = "Name";
            GarageNPCClothesLB.ValueMember = "Value";
            GarageNPCClothesLB.DataSource = CurrentgarageNPC.Clothes;
        }
        private void UseGarageOnlyToTradeCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGarageConfig.UseGarageOnlyToTrade = UseGarageOnlyToTradeCB.Checked == true ? 1 : 0;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void SavedVehicleInGarageForTradeInHourCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGarageConfig.SavedVehicleInGarageForTradeInHour = SavedVehicleInGarageForTradeInHourCB.Checked == true ? 1 : 0;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void MaxVehicleStoredNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGarageConfig.MaxVehicleStored = (int)MaxVehicleStoredNUD.Value;
            TraderPlusGarageConfig.isDirty = true;

        }
        private void MaxVehicleStoredReachedTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGarageConfig.MaxVehicleStoredReached = MaxVehicleStoredReachedTB.Text;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void TradeVehicleWarningTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGarageConfig.TradeVehicleWarning = TradeVehicleWarningTB.Text;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void TradeVehicleHasBeenDeletedTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGarageConfig.TradeVehicleHasBeenDeleted = TradeVehicleHasBeenDeletedTB.Text;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void VehicleMustHaveLockCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGarageConfig.VehicleMustHaveLock = VehicleMustHaveLockCB.Checked == true ? 1 : 0;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void SaveVehicleCargoCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGarageConfig.SaveVehicleCargo = SaveVehicleCargoCB.Checked == true ? 1 : 0;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void SaveVehicleHealthCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGarageConfig.SaveVehicleHealth = SaveVehicleHealthCB.Checked == true ? 1 : 0;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void SaveVehicleFuelCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGarageConfig.SaveVehicleFuel = SaveVehicleFuelCB.Checked == true ? 1 : 0;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void PayWithBankAccountCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGarageConfig.PayWithBankAccount = PayWithBankAccountCB.Checked == true ? 1 : 0;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void ParkInCostNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGarageConfig.ParkInCost = (int)ParkInCostNUD.Value;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void ParkOutCostNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGarageConfig.ParkOutCost = (int)ParkOutCostNUD.Value;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    TraderPlusGarageConfig.WhitelistedObjects.Add(l);
                    TraderPlusGarageConfig.isDirty = true;
                }
            }
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
            TraderPlusGarageConfig.WhitelistedObjects.Remove(GarageWhiteListLB.GetItemText(GarageWhiteListLB.SelectedItem));
            TraderPlusGarageConfig.isDirty = true;
            if (GarageWhiteListLB.Items.Count == 0)
                GarageWhiteListLB.SelectedIndex = -1;
            else
                GarageWhiteListLB.SelectedIndex = 0;
        }
        private void ParkingNotAvailableTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGarageConfig.ParkingNotAvailable = ParkingNotAvailableTB.Text;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void NotEnoughMoneyTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGarageConfig.ParkingNotAvailable = ParkingNotAvailableTB.Text;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void NotRightToParkTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGarageConfig.NotEnoughMoney = NotEnoughMoneyTB.Text;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void CarHasMemberTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGarageConfig.CarHasMember = CarHasMemberTB.Text;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void ParkInFailTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGarageConfig.ParkInFail = ParkInFailTB.Text;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void ParkInSuccessTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGarageConfig.ParkInSuccess = ParkInSuccessTB.Text;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void ParkOutFailTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGarageConfig.ParkOutFail = ParkOutFailTB.Text;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void ParkOutSuccessTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGarageConfig.ParkOutSuccess = ParkOutSuccessTB.Text;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void darkButton4_Click(object sender, EventArgs e)
        {
            Npc newnpc = new Npc()
            {
                ClassName = "New Garage Dude, Please Change me!!",
                Position = new float[] { 0, 0, 0 },
                Orientation = new float[] { 0, 0, 0 },
                ParkingPosition = new float[] { 0, 0, 0 },
                ParkingOrientation = new float[] { 0, 0, 0 },
                Clothes = new BindingList<string>()
            };
            TraderPlusGarageConfig.NPCs.Add(newnpc);
            TraderPlusGarageConfig.isDirty = true;
        }
        private void darkButton3_Click(object sender, EventArgs e)
        {
            TraderPlusGarageConfig.NPCs.Remove(CurrentgarageNPC);
            TraderPlusGarageConfig.isDirty = true;
            if (GarageNPCLB.Items.Count == 0)
                GarageNPCLB.SelectedIndex = -1;
            else
                GarageNPCLB.SelectedIndex = 0;
        }
        private void NPCsClassNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentgarageNPC.ClassName = NPCsClassNameTB.Text;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentgarageNPC.Position[0] = (float)numericUpDown1.Value;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentgarageNPC.Position[1] = (float)numericUpDown2.Value;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentgarageNPC.Position[2] = (float)numericUpDown3.Value;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentgarageNPC.Orientation[0] = (float)numericUpDown4.Value;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentgarageNPC.Orientation[1] = (float)numericUpDown5.Value;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentgarageNPC.Orientation[2] = (float)numericUpDown6.Value;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentgarageNPC.ParkingPosition[0] = (float)numericUpDown7.Value;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentgarageNPC.ParkingPosition[1] = (float)numericUpDown8.Value;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentgarageNPC.ParkingPosition[2] = (float)numericUpDown9.Value;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentgarageNPC.ParkingOrientation[0] = (float)numericUpDown10.Value;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentgarageNPC.ParkingOrientation[1] = (float)numericUpDown11.Value;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentgarageNPC.ParkingOrientation[2] = (float)numericUpDown12.Value;
            TraderPlusGarageConfig.isDirty = true;
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
                    CurrentgarageNPC.Clothes.Add(l);
                }
                TraderPlusGarageConfig.isDirty = true;
            }
        }
        private void darkButton5_Click(object sender, EventArgs e)
        {
            CurrentgarageNPC.Clothes.Remove(GarageNPCClothesLB.GetItemText(GarageNPCClothesLB.SelectedItem));
            TraderPlusGarageConfig.isDirty = true;
        }
        private void darkButton21_Click_1(object sender, EventArgs e)
        {
            TraderNPCs newnpc = new TraderNPCs();
            newnpc.isTraderplus = true;
            if (newnpc.ShowDialog() == DialogResult.OK)
            {
                NPCsClassNameTB.Text = newnpc.selectedNPC;
                CurrentgarageNPC.ClassName = newnpc.selectedNPC;
                TraderPlusGarageConfig.isDirty = true;
                GarageNPCLB.Refresh();
            }
        }
        #endregion GarageConfig

        #region General
        public Currency CurrentCurrency { get; set; }
        public Traderobject currentTraderObject { get; set; }
        private void darkButton38_Click(object sender, EventArgs e)
        {
            TraderPlusGeneralConfig.SortCurriences();
            CurrenciesLB.DisplayMember = "Name";
            CurrenciesLB.ValueMember = "Value";
            CurrenciesLB.DataSource = TraderPlusGeneralConfig.Currencies;
        }
        private void SetupTraderPlusGeneralConfig()
        {
            useraction = false;



            TraderPlusTradersLB.DisplayMember = "Name";
            TraderPlusTradersLB.ValueMember = "Value";
            TraderPlusTradersLB.DataSource = TraderPlusGeneralConfig.Traders;
            useraction = false;
            CurrenciesLB.DisplayMember = "Name";
            CurrenciesLB.ValueMember = "Value";
            CurrenciesLB.DataSource = TraderPlusGeneralConfig.Currencies;

            TraderObjectsLB.DisplayMember = "Name";
            TraderObjectsLB.ValueMember = "Value";
            TraderObjectsLB.DataSource = TraderPlusGeneralConfig.TraderObjects;

            LicencesLB.DisplayMember = "Name";
            LicencesLB.ValueMember = "Value";
            LicencesLB.DataSource = TraderPlusGeneralConfig.Licences;

            LicenceKeyWordTB.Text = TraderPlusGeneralConfig.LicenceKeyWord;
            ConvertTraderConfigToTraderPlusCb.Checked = TraderPlusGeneralConfig.ConvertTraderConfigToTraderPlus == 1 ? true : false;
            ConvertTraderConfigToTraderPlusWithStockBasedOnCECB.Checked = TraderPlusGeneralConfig.ConvertTraderConfigToTraderPlusWithStockBasedOnCE == 1 ? true : false;
            UseGarageToTradeCarCB.Checked = TraderPlusGeneralConfig.UseGarageToTradeCar == 1 ? true : false;
            DisableHeightFailSafeForReceiptDeploymentCB.Checked = TraderPlusGeneralConfig.DisableHeightFailSafeForReceiptDeployment == 1 ? true : false;
            EnableShowAllPricesCB.Checked = TraderPlusGeneralConfig.EnableShowAllPrices == 1 ? true : false;
            HideInsuranceBtnCB.Checked = TraderPlusGeneralConfig.HideInsuranceBtn == 1 ? true : false;
            HideGarageBtnCB.Checked = TraderPlusGeneralConfig.HideGarageBtn == 1 ? true : false;
            HideLicenceBtnCB.Checked = TraderPlusGeneralConfig.HideLicenceBtn == 1 ? true : false;
            EnableShowAllCheckBoxCB.Checked = TraderPlusGeneralConfig.EnableShowAllCheckBox == 1 ? true : false;
            IsReceiptSaveLockCB.Checked = TraderPlusGeneralConfig.IsReceiptSaveLock == 1 ? true : false;
            IsReceiptSaveAttachmentCB.Checked = TraderPlusGeneralConfig.IsReceiptSaveAttachment == 1 ? true : false;
            IsReceiptSaveCargoCB.Checked = TraderPlusGeneralConfig.IsReceiptSaveCargo == 1 ? true : false;
            IsReceiptTraderOnlyCB.Checked = TraderPlusGeneralConfig.IsReceiptTraderOnly == 1 ? true : false;
            StoreOnlyToPristineStateCB.Checked = TraderPlusGeneralConfig.StoreOnlyToPristineState == 1 ? true : false;
            LockPickChanceNUD.Value = (decimal)TraderPlusGeneralConfig.LockPickChance;
            AcceptWornCB.Checked = TraderPlusGeneralConfig.AcceptedStates.AcceptWorn == 1 ? true : false;
            AcceptDamagedCB.Checked = TraderPlusGeneralConfig.AcceptedStates.AcceptDamaged == 1 ? true : false;
            AcceptBadlyDamagedCB.Checked = TraderPlusGeneralConfig.AcceptedStates.AcceptBadlyDamaged == 1 ? true : false;
            WornCoeffNUD.Value = TraderPlusGeneralConfig.AcceptedStates.CoefficientWorn;
            DamagedCoeffNUD.Value = TraderPlusGeneralConfig.AcceptedStates.CoefficientDamaged;
            BadlyDamagedCoeffNUD.Value = TraderPlusGeneralConfig.AcceptedStates.CoefficientBadlyDamaged;


            useraction = true;
        }
        private void TraderObjectsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TraderObjectsLB.SelectedItems.Count < 1) return;
            currentTraderObject = TraderObjectsLB.SelectedItem as Traderobject;

            ObjectClassnameTB.Text = currentTraderObject.ObjectName;
            ObjectPositionXNUD.Value = (decimal)currentTraderObject.Position[0];
            ObjectPositionYNUD.Value = (decimal)currentTraderObject.Position[1];
            ObjectPositionZNUD.Value = (decimal)currentTraderObject.Position[2];
            ObjectOrientationXNUD.Value = (decimal)currentTraderObject.Orientation[0];
            ObjectOrientationYNUD.Value = (decimal)currentTraderObject.Orientation[1];
            ObjectOrientationZNUD.Value = (decimal)currentTraderObject.Orientation[2];

        }
        private void CurrenciesLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrenciesLB.SelectedItems.Count < 1) return;
            CurrentCurrency = CurrenciesLB.SelectedItem as Currency;

            CurrencyValueNUD.Value = CurrentCurrency.Value;

            CurrencyClassnamesLB.DisplayMember = "Name";
            CurrencyClassnamesLB.ValueMember = "Value";
            CurrencyClassnamesLB.DataSource = CurrentCurrency.CurrenciesNames;

        }
        private void darkButton44_Click(object sender, EventArgs e)
        {
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (!TraderPlusGeneralConfig.Licences.Contains(l))
                    {
                        TraderPlusGeneralConfig.Licences.Add(l);
                        TraderPlusGeneralConfig.isDirty = true;
                    }
                    else
                    {
                        MessageBox.Show(l + " Allready exists.....");
                    }
                }
            }
        }
        private void darkButton43_Click(object sender, EventArgs e)
        {
            TraderPlusGeneralConfig.Licences.Remove(LicencesLB.GetItemText(LicencesLB.SelectedItem));
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void darkButton39_Click(object sender, EventArgs e)
        {
            if (CurrentCurrency == null) return;
            TraderPlusGeneralConfig.Currencies.Remove(CurrentCurrency);
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void darkButton40_Click(object sender, EventArgs e)
        {
            Currency newCurrency = new Currency()
            {
                Value = 0,
                CurrenciesNames = new BindingList<string>()
            };
            TraderPlusGeneralConfig.Currencies.Add(newCurrency);
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void darkButton41_Click(object sender, EventArgs e)
        {
            if (CurrentCurrency == null) return;
            CurrentCurrency.CurrenciesNames.Remove(CurrencyClassnamesLB.GetItemText(CurrencyClassnamesLB.SelectedItem));
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void darkButton42_Click(object sender, EventArgs e)
        {
            if (CurrentCurrency == null) { return; }
            Dictionary<string, bool> UsedTypes = new Dictionary<string, bool>();
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                usedtypes = UsedTypes
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    CurrentCurrency.CurrenciesNames.Add(l);
                    TraderPlusGeneralConfig.isDirty = true;
                }

            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton46_Click(object sender, EventArgs e)
        {
            Traderobject newobject = new Traderobject()
            {
                ObjectName = "NewObject",
                Position = new float[] { 0, 0, 0 },
                Orientation = new float[] { 0, 0, 0 }
            };
            TraderPlusGeneralConfig.TraderObjects.Add(newobject);
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void darkButton45_Click(object sender, EventArgs e)
        {
            if (currentTraderObject == null) return;
            TraderPlusGeneralConfig.TraderObjects.Remove(currentTraderObject);
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void ConvertTraderConfigToTraderPlusCb_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGeneralConfig.ConvertTraderConfigToTraderPlus = ConvertTraderConfigToTraderPlusCb.Checked == true ? 1 : 0;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void ConvertTraderConfigToTraderPlusWithStockBasedOnCECB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGeneralConfig.ConvertTraderConfigToTraderPlusWithStockBasedOnCE = ConvertTraderConfigToTraderPlusWithStockBasedOnCECB.Checked == true ? 1 : 0;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void UseGarageToTradeCarCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGeneralConfig.UseGarageToTradeCar = UseGarageToTradeCarCB.Checked == true ? 1 : 0;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void HideInsuranceBtnCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGeneralConfig.HideInsuranceBtn = HideInsuranceBtnCB.Checked == true ? 1 : 0;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void HideGarageBtnCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGeneralConfig.HideGarageBtn = HideGarageBtnCB.Checked == true ? 1 : 0;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void HideLicenceBtnCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGeneralConfig.HideLicenceBtn = HideLicenceBtnCB.Checked == true ? 1 : 0;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void DisableHeightFailSafeForReceiptDeploymentCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGeneralConfig.DisableHeightFailSafeForReceiptDeployment = DisableHeightFailSafeForReceiptDeploymentCB.Checked == true ? 1 : 0;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void EnableShowAllPricesCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGeneralConfig.EnableShowAllPrices = EnableShowAllPricesCB.Checked == true ? 1 : 0;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void EnableShowAllCheckBoxCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGeneralConfig.EnableShowAllCheckBox = EnableShowAllCheckBoxCB.Checked == true ? 1 : 0;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void IsReceiptSaveLockCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGeneralConfig.IsReceiptSaveLock = IsReceiptSaveLockCB.Checked == true ? 1 : 0;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void IsReceiptSaveAttachmentCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGeneralConfig.IsReceiptSaveAttachment = IsReceiptSaveAttachmentCB.Checked == true ? 1 : 0;
            TraderPlusGeneralConfig.isDirty = true;
        }

        private void IsReceiptSaveCargoCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGeneralConfig.IsReceiptSaveCargo = IsReceiptSaveCargoCB.Checked == true ? 1 : 0;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void IsReceiptTraderOnlyCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGeneralConfig.IsReceiptTraderOnly = IsReceiptTraderOnlyCB.Checked == true ? 1 : 0;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void StoreOnlyToPristineStateCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGeneralConfig.StoreOnlyToPristineState = StoreOnlyToPristineStateCB.Checked == true ? 1 : 0;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void LockPickChanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGeneralConfig.LockPickChance = (float)LockPickChanceNUD.Value;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void LicenceKeyWordTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGeneralConfig.LicenceKeyWord = LicenceKeyWordTB.Text;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void AcceptWornCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGeneralConfig.AcceptedStates.AcceptWorn = AcceptWornCB.Checked == true ? 1 : 0;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void AcceptDamagedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGeneralConfig.AcceptedStates.AcceptDamaged = AcceptDamagedCB.Checked == true ? 1 : 0;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void AcceptBadlyDamagedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGeneralConfig.AcceptedStates.AcceptBadlyDamaged = AcceptDamagedCB.Checked == true ? 1 : 0;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void WornCoeffNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGeneralConfig.AcceptedStates.CoefficientWorn = WornCoeffNUD.Value;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void DamagedCoeffNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGeneralConfig.AcceptedStates.CoefficientDamaged = DamagedCoeffNUD.Value;
            TraderPlusGeneralConfig.isDirty = true;

        }
        private void BadlyDamagedCoeffNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGeneralConfig.AcceptedStates.CoefficientBadlyDamaged = BadlyDamagedCoeffNUD.Value;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void CurrencyValueNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentCurrency.Value = (int)CurrencyValueNUD.Value;
            TraderPlusGeneralConfig.isDirty = true;
            CurrenciesLB.Refresh();
        }
        private void ObjectClassnameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentTraderObject.ObjectName = ObjectClassnameTB.Text;
            TraderPlusGeneralConfig.isDirty = true;
            TraderObjectsLB.Refresh();
        }
        private void ObjcetPositionXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentTraderObject.Position[0] = (float)ObjectPositionXNUD.Value;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void ObjectPositionYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentTraderObject.Position[1] = (float)ObjectPositionYNUD.Value;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void ObjectpositionZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentTraderObject.Position[2] = (float)ObjectPositionZNUD.Value;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void ObjectOrientationXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentTraderObject.Orientation[0] = (float)ObjectOrientationXNUD.Value;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void ObjectOrientationYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentTraderObject.Orientation[1] = (float)ObjectOrientationYNUD.Value;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void ObjectOrientationZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentTraderObject.Orientation[2] = (float)ObjectOrientationZNUD.Value;
            TraderPlusGeneralConfig.isDirty = true;
        }
        #endregion general

        #region Traders
        public IDs currenttraderID { get; set; }
        public Trader currenttrader { get; set; }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            TraderPlusGeneralConfig.SortTradersByIndex();
            TraderPlusTradersLB.DisplayMember = "Name";
            TraderPlusTradersLB.ValueMember = "Value";
            TraderPlusTradersLB.DataSource = TraderPlusGeneralConfig.Traders;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void TraderPlusTradersLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TraderPlusTradersLB.SelectedItems.Count < 1) return;
            currenttrader = TraderPlusTradersLB.SelectedItem as Trader;
            useraction = false;
            TraderIDLabel.Text = "ID : " + currenttrader.Id.ToString();
            IsBankerCB.Checked = currenttrader.isBanker;
            IsInsuranceCB.Checked = currenttrader.isInsurer;
            TraderNPCNameTB.Text = currenttrader.Name;
            TraderNPCGivenNameTB.Text = currenttrader.GivenName;
            TraderNPCRoleTB.Text = currenttrader.Role;

            TraderNPCPositionX.Value = (decimal)currenttrader.Position[0];
            TraderNPCPositionY.Value = (decimal)currenttrader.Position[1];
            TraderNPCPositionZ.Value = (decimal)currenttrader.Position[2];

            TraderNPCOrientationX.Value = (decimal)currenttrader.Orientation[0];
            TraderNPCOrientationY.Value = (decimal)currenttrader.Orientation[1];
            TraderNPCOrientationZ.Value = (decimal)currenttrader.Orientation[2];

            TraderNPCClothesLB.DisplayMember = "Name";
            TraderNPCClothesLB.ValueMember = "Value";
            TraderNPCClothesLB.DataSource = currenttrader.Clothes;

            if (!currenttrader.isBanker)
            {
                currenttraderID = currenttrader.TraderCategoryList;

                TPCategoriesLB.DisplayMember = "Name";
                TPCategoriesLB.ValueMember = "Value";
                TPCategoriesLB.DataSource = currenttraderID.Categories;

                LicensesRequiredLB.DisplayMember = "Name";
                LicensesRequiredLB.ValueMember = "Value";
                LicensesRequiredLB.DataSource = currenttraderID.LicencesRequired;

                CurrenciesAcceptedLB.DisplayMember = "Name";
                CurrenciesAcceptedLB.ValueMember = "Value";
                CurrenciesAcceptedLB.DataSource = currenttraderID.CurrenciesAccepted;
            }
            useraction = true;
        }
        private void darkButton33_Click(object sender, EventArgs e)
        {
            Trader newtrader = new Trader()
            {
                Id = -2,
                Name = "NewNPC",
                GivenName = "He Who shall Not Be Named!!!",
                Role = "Grand Master of Underlings",
                Position = new float[] { 0, 0, 0 },
                Orientation = new float[] { 0, 0, 0 },
                Clothes = new BindingList<string>(),
                isBanker = true
            };
            TraderPlusGeneralConfig.Traders.Add(newtrader);
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void darkButton32_Click(object sender, EventArgs e)
        {
            TraderPlusGeneralConfig.Traders.Remove(currenttrader);
            TraderPlusGeneralConfig.UpdateIndexes(TraderPlusIDsConfig);
            TraderPlusGeneralConfig.isDirty = true;

            TraderPlusTradersLB.SelectedIndex = -1;
            if (TraderPlusTradersLB.Items.Count > 0)
                TraderPlusTradersLB.SelectedIndex = 0;
        }
        private void IsBankerCB_CheckedChanged(object sender, EventArgs e)
        {
            if (IsBankerCB.Checked)
            {
                TraderInfoGroupBox.Visible = false;
                if (!useraction) return;
                currenttrader.Id = -2;
                currenttrader.isBanker = true;
                currenttrader.TraderCategoryList = null;
                TraderPlusGeneralConfig.UpdateIndexes(TraderPlusIDsConfig);
                TraderPlusGeneralConfig.SortTradersByIndex();
                TraderPlusTradersLB.DisplayMember = "Name";
                TraderPlusTradersLB.ValueMember = "Value";
                TraderPlusTradersLB.DataSource = TraderPlusGeneralConfig.Traders;
            }
            else
            {
                TraderInfoGroupBox.Visible = true;

                if (!useraction) return;
                currenttrader.Id = TraderPlusGeneralConfig.getnextavialableID();
                currenttrader.isBanker = false;
                currenttrader.TraderCategoryList = new IDs()
                {
                    Id = currenttrader.Id,
                    Categories = new BindingList<string>(),
                    LicencesRequired = new BindingList<string>(),
                    CurrenciesAccepted = new BindingList<string>()
                };
                TraderPlusGeneralConfig.isDirty = true;
                TraderPlusGeneralConfig.SortTradersByIndex();
                TraderPlusTradersLB.DisplayMember = "Name";
                TraderPlusTradersLB.ValueMember = "Value";
                TraderPlusTradersLB.DataSource = TraderPlusGeneralConfig.Traders;
            }

        }
        private void darkButton36_Click(object sender, EventArgs e)
        {
            TraderNPCs newnpc = new TraderNPCs();
            newnpc.isTraderplus = true;
            if (newnpc.ShowDialog() == DialogResult.OK)
            {
                TraderNPCNameTB.Text = newnpc.selectedNPC;
                currenttrader.Name = newnpc.selectedNPC;
                TraderPlusGeneralConfig.isDirty = true;
            }
        }
        private void darkButton21_Click(object sender, EventArgs e)
        {

        }
        private void darkButton31_Click(object sender, EventArgs e)
        {
            TraderInfoGroupBox.Visible = false;
            AvailalbeLicneseGrouBox.Visible = true;
            List<string> returnlist = TraderPlusGeneralConfig.getlicenses();
            AvailableLicenseLB.Items.Clear();
            AvailableLicenseLB.Items.AddRange(returnlist.ToArray());
        }
        private void darkButton30_Click(object sender, EventArgs e)
        {
            currenttrader.TraderCategoryList.LicencesRequired.Remove(LicensesRequiredLB.GetItemText(LicensesRequiredLB.SelectedItem));
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void darkButton29_Click(object sender, EventArgs e)
        {
            TraderInfoGroupBox.Visible = false;
            AvailablecurrenciesGroupBox.Visible = true;
            List<string> returnlist = TraderPlusGeneralConfig.getcurrencies();
            AvailabeCurrenciesLB.Items.Clear();
            AvailabeCurrenciesLB.Items.AddRange(returnlist.ToArray());
        }
        private void darkButton27_Click(object sender, EventArgs e)
        {
            currenttrader.TraderCategoryList.CurrenciesAccepted.Remove(CurrenciesAcceptedLB.GetItemText(CurrenciesAcceptedLB.SelectedItem));
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void darkButton24_Click(object sender, EventArgs e)
        {
            TraderInfoGroupBox.Visible = false;
            AddFromCategoryGroupBox.Visible = true;
            List<Tradercategory> returnlist = TraderPlusPriceConfig.getallCats();
            addfromCatLB.Items.Clear();
            addfromCatLB.Items.AddRange(returnlist.ToArray());
        }
        private void darkButton37_Click(object sender, EventArgs e)
        {
            TraderInfoGroupBox.Visible = true;
            AddFromCategoryGroupBox.Visible = false;
        }
        private void darkButton23_Click(object sender, EventArgs e)
        {
            string removeitem = TPCategoriesLB.GetItemText(TPCategoriesLB.SelectedItem);
            currenttrader.TraderCategoryList.Categories.Remove(removeitem);
            TraderPlusGeneralConfig .isDirty = true;
        }
        private void TraderNPCGivenNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currenttrader.GivenName = TraderNPCGivenNameTB.Text;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void TraderNPCRoleTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currenttrader.Role = TraderNPCRoleTB.Text;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void TraderNPCPositionX_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currenttrader.Position[0] = (float)TraderNPCPositionX.Value;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void TraderNPCPositionY_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currenttrader.Position[1] = (float)TraderNPCPositionY.Value;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void TraderNPCPositionZ_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currenttrader.Position[2] = (float)TraderNPCPositionZ.Value;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void TraderNPCOrientationX_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currenttrader.Orientation[0] = (float)TraderNPCOrientationX.Value;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void TraderNPCOrientationY_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currenttrader.Orientation[1] = (float)TraderNPCOrientationY.Value;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void TraderNPCOrientationZ_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currenttrader.Orientation[2] = (float)TraderNPCOrientationZ.Value;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void darkButton35_Click(object sender, EventArgs e)
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
                    currenttrader.Clothes.Add(l);
                    TraderPlusGeneralConfig.isDirty = true;
                }
            }
        }
        private void darkButton34_Click(object sender, EventArgs e)
        {
            currenttrader.Clothes.Remove(TraderNPCClothesLB.GetItemText(TraderNPCClothesLB.SelectedItem));
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void darkButton22_Click(object sender, EventArgs e)
        {
            foreach (var item in addfromCatLB.SelectedItems)
            {
                Tradercategory tcat = item as Tradercategory;
                if (!currenttrader.TraderCategoryList.Categories.Contains(tcat.CategoryName))
                {
                    currenttrader.TraderCategoryList.Categories.Add(tcat.CategoryName);
                    TraderPlusGeneralConfig.isDirty = true;
                }
                else
                {
                    MessageBox.Show(tcat.CategoryName + " is allready in this trader.....");
                }
            }

            TraderInfoGroupBox.Visible = true;
            AddFromCategoryGroupBox.Visible = false;
        }
        private void darkButton48_Click(object sender, EventArgs e)
        {
            foreach (var item in AvailableLicenseLB.SelectedItems)
            {
                string license = item as string;
                if (!currenttrader.TraderCategoryList.LicencesRequired.Contains(license))
                    currenttrader.TraderCategoryList.LicencesRequired.Add(license);
            }
            TraderPlusGeneralConfig.isDirty = true;
            TraderInfoGroupBox.Visible = true;
            AvailalbeLicneseGrouBox.Visible = false;
        }
        private void darkButton47_Click(object sender, EventArgs e)
        {
            TraderInfoGroupBox.Visible = true;
            AvailalbeLicneseGrouBox.Visible = false;
        }
        private void darkButton50_Click(object sender, EventArgs e)
        {
            foreach (var item in AvailabeCurrenciesLB.SelectedItems)
            {
                string currency = item as string;
                if (!currenttrader.TraderCategoryList.CurrenciesAccepted.Contains(currency))
                    currenttrader.TraderCategoryList.CurrenciesAccepted.Add(currency);
            }
            TraderPlusGeneralConfig.isDirty = true;
            TraderInfoGroupBox.Visible = true;
            AvailablecurrenciesGroupBox.Visible = false;
        }
        private void darkButton49_Click(object sender, EventArgs e)
        {
            TraderInfoGroupBox.Visible = true;
            AvailablecurrenciesGroupBox.Visible = false;
        }
        private void IsInsuranceCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currenttrader.isInsurer = IsInsuranceCB.Checked;
            TraderPlusInsuranceConfig.isDirty = true;
        }
        #endregion TarderIDS

        #region traderprice
        public Tradercategory currentTradercategory { get; set; }
        public ItemProducts currentItemProducts { get; set; }
        private void SetupTraderPlusPriceConfig()
        {
            TraderPlusPriceConfig.Sortcategories();
            useraction = false;
            EnableDefaultTraderStockCB.DataSource = Enum.GetValues(typeof(EnableDefaultTraderStock));
            TraderPlusPriceConfig.getproducts();

            EnableAutoCalculationCB.Checked = TraderPlusPriceConfig.EnableAutoCalculation == 1 ? true : false;
            EnableAutoDestockAtRestartCB.Checked = TraderPlusPriceConfig.EnableAutoDestockAtRestart == 1 ? true : false;
            EnableDefaultTraderStockCB.SelectedItem = (EnableDefaultTraderStock)TraderPlusPriceConfig.EnableDefaultTraderStock;

            TraderCategoriesLB.DisplayMember = "Name";
            TraderCategoriesLB.ValueMember = "Value";
            TraderCategoriesLB.DataSource = TraderPlusPriceConfig.TraderCategories;

            useraction = true;
        }
        private void TraderCategoriesLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TraderCategoriesLB.SelectedItems.Count < 1) return;
            currentTradercategory = TraderCategoriesLB.SelectedItem as Tradercategory;
           
            useraction = false;

            CategoryNameTB.Text = currentTradercategory.CategoryName;
            CurrentTraderCatLB.DisplayMember = "Name";
            CurrentTraderCatLB.ValueMember = "Value";
            CurrentTraderCatLB.DataSource = currentTradercategory.itemProducts;

            useraction = true;

        }
        private void CurrentTraderCatLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentTraderCatLB.SelectedItems.Count < 1) return;
            currentItemProducts = CurrentTraderCatLB.SelectedItem as ItemProducts;

            useraction = false;

            ClassnameNUD.Text = currentItemProducts.Classname;
            CoefficientTB.Value = currentItemProducts.Coefficient;
            CoefficientPercentLabel.Text = currentItemProducts.Coefficient.ToString() + "%";

            TradeQuantityNUD.Value = (decimal)currentItemProducts.TradeQuantity;
            if (currentItemProducts.MaxStock == -1)
            {
                MaxStockNUD.Value = -1;
                MaxStockNUD.Enabled = false;
                InfiniteCB.Checked = true;
            }
            else
            {
                MaxStockNUD.Value = (int)currentItemProducts.MaxStock;
                InfiniteCB.Checked = false;
                MaxStockNUD.Enabled = true;
            }
            if (currentItemProducts.BuyPrice == -1)
            {
                BuyPriceNUD.Value = -1;
                BuyPriceNUD.Enabled = false;
                CantBuyCB.Checked = true;
            }
            else
            {
                BuyPriceNUD.Value = (int)currentItemProducts.BuyPrice;
                CantBuyCB.Checked = false;
                BuyPriceNUD.Enabled = true;
            }
            if (currentItemProducts.Sellprice == -1)
            {
                SellpriceNUD.Value = -1;
                SellpriceNUD.Enabled = false;
                CantSellCB.Checked = true;
            }
            else
            {
                SellpriceNUD.Value = (decimal)currentItemProducts.Sellprice;
                SellpriceNUD.Enabled = true;
                CantSellCB.Checked = false;
            }
            if (currentItemProducts.UseDestockCoeff)
            {
                DestockCoefflabel.Visible = true;
                DestockCoeffTB.Visible = true;
                UsedestockCoeffCB.Checked = true;
                DestockCoeffTB.Value = currentItemProducts.destockCoefficent;
                DestockCoefflabel.Text = currentItemProducts.destockCoefficent.ToString() + "%";
            }
            else
            {
                DestockCoefflabel.Visible = false;
                DestockCoeffTB.Visible = false;
                UsedestockCoeffCB.Checked = false;
                DestockCoeffTB.Value = 0;
            }
            useraction = true;
        }
        private void EnableAutoCalculationCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusPriceConfig.EnableAutoCalculation = EnableAutoCalculationCB.Checked == true ? 1 : 0;
            TraderPlusPriceConfig.isDirty = true;
        }
        private void EnableAutoDestockAtRestartCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusPriceConfig.EnableAutoDestockAtRestart = EnableAutoDestockAtRestartCB.Checked == true ? 1 : 0;
            TraderPlusPriceConfig.isDirty = true;
        }
        private void EnableDefaultTraderStockCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            EnableDefaultTraderStock cacl = (EnableDefaultTraderStock)EnableDefaultTraderStockCB.SelectedItem;
            TraderPlusPriceConfig.EnableDefaultTraderStock = (int)cacl;
            TraderPlusPriceConfig.isDirty = true;
        }
        private void darkButton12_Click(object sender, EventArgs e)
        {
            Tradercategory newcat = new Tradercategory()
            {
                CategoryName = "NewCat",
                Products = new BindingList<string>(),
                itemProducts = new BindingList<ItemProducts>()
            };
            TraderPlusPriceConfig.TraderCategories.Add(newcat);
            TraderPlusPriceConfig.isDirty = true;
        }
        private void darkButton11_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This Will Remove The All reference to this category, Are you sure you want to do this?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                    string removeitem = TraderCategoriesLB.GetItemText(TraderCategoriesLB.SelectedItem);

                    string message = removeitem + " Category Removed....\nThe Following Items Were Removed from both Trader and Market Categories\n";
                    foreach (ItemProducts item in currentTradercategory.itemProducts)
                    {
                        message += item.Classname + "\n";
                    }
                    foreach (IDs id in TraderPlusIDsConfig.IDs)
                    {
                        bool remove = false;
                        foreach (string cat in id.Categories)
                        {
                            if (cat == removeitem)
                            {
                                remove = true;
                                break;
                            }
                        }
                        if (remove)
                        {
                            id.Categories.Remove(removeitem);
                            TraderPlusIDsConfig.isDirty = true;
                        }
                    }
                    TraderPlusPriceConfig.TraderCategories.Remove(currentTradercategory);
                    TraderPlusPriceConfig.isDirty = true;
                    MessageBox.Show(message, "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    if (TraderCategoriesLB.Items.Count == 0)
                        TraderCategoriesLB.SelectedIndex = -1;
                    else
                        TraderCategoriesLB.SelectedIndex = 0;
            }
        }
        private void darkButton14_Click(object sender, EventArgs e)
        {
            if (currentTradercategory == null) { return; }
            Dictionary<string, bool> UsedTypes = new Dictionary<string, bool>();
            foreach (Tradercategory mc in TraderPlusPriceConfig.TraderCategories)
            {
                foreach (ItemProducts item in mc.itemProducts)
                {
                    if (!UsedTypes.ContainsKey(item.Classname.ToLower()))
                        UsedTypes.Add(item.Classname.ToLower(), true);
                }
            }
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                usedtypes = UsedTypes
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    ItemProducts NewContainer = new ItemProducts
                    {
                        Classname = l,
                        Coefficient = 0,
                        MaxStock = 0,
                        TradeQuantity = 0,
                        BuyPrice = 0,
                        Sellprice = 0,
                        destockCoefficent = 50,
                        UseDestockCoeff = true
                    };
                    currentTradercategory.AdditemProduct(NewContainer);
                    TraderPlusPriceConfig.isDirty = true;
                }

            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton28_Click(object sender, EventArgs e)
        {
            if (currentTradercategory == null) { return; }
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    ItemProducts NewContainer = new ItemProducts
                    {
                        Classname = l,
                        Coefficient = 0,
                        MaxStock = 0,
                        TradeQuantity = 0,
                        BuyPrice = 0,
                        Sellprice = 0,
                        destockCoefficent = 50,
                        UseDestockCoeff = true
                    };
                    if (!Checkifincat(NewContainer, currentTradercategory))
                    {
                        currentTradercategory.AdditemProduct(NewContainer);
                        TraderPlusPriceConfig.isDirty = true;
                    }
                    else
                    {
                        MessageBox.Show(NewContainer.Classname + " Allready exists.....");
                    }
                }
            }
        }
        private bool Checkifincat(ItemProducts item, Tradercategory tradercat)
        {
            if (tradercat.itemProducts.Any(x => x.Classname == item.Classname))
            {
                return true;
            }
            return false;
        }
        private void darkButton13_Click(object sender, EventArgs e)
        {
            if (currentItemProducts == null) return;
            currentTradercategory.removeItemProduct(currentItemProducts);
            TraderPlusPriceConfig.isDirty = true;
        }
        private void CategoryNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentTradercategory.CategoryName = CategoryNameTB.Text;
            TraderPlusPriceConfig.isDirty = true;
            TraderCategoriesLB.Refresh();
        }
        private void CoefficientTB_MouseUp(object sender, MouseEventArgs e)
        {
            if (!useraction) return;
            currentItemProducts.Coefficient = CoefficientTB.Value;

            if (CurrentTraderCatLB.SelectedItems.Count > 1)
            {
                foreach (var item in CurrentTraderCatLB.SelectedItems)
                {
                    ItemProducts pitem = item as ItemProducts;
                    pitem.Coefficient = CoefficientTB.Value;
                }
            }
            TraderPlusPriceConfig.isDirty = true;
        }
        private void MaxStockNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentItemProducts.MaxStock = (int)MaxStockNUD.Value;
            TraderPlusPriceConfig.isDirty = true;
            if (CurrentTraderCatLB.SelectedItems.Count > 1)
            {
                foreach (var item in CurrentTraderCatLB.SelectedItems)
                {
                    ItemProducts pitem = item as ItemProducts;
                    pitem.MaxStock = (int)MaxStockNUD.Value;
                }
            }
        }
        private void InfiniteCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (InfiniteCB.Checked)
            {
                MaxStockNUD.Value = -1;
                MaxStockNUD.Enabled = false;
            }
            else
            {
                MaxStockNUD.Enabled = true;
                MaxStockNUD.Value = 1;
            }
        }
        private void TradeQuantityNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentItemProducts.TradeQuantity = (decimal)TradeQuantityNUD.Value;
            TraderPlusPriceConfig.isDirty = true;
            if (CurrentTraderCatLB.SelectedItems.Count > 1)
            {
                foreach (var item in CurrentTraderCatLB.SelectedItems)
                {
                    ItemProducts pitem = item as ItemProducts;
                    pitem.TradeQuantity = (decimal)TradeQuantityNUD.Value;
                }
            }
        }
        private void CantBuyCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CantBuyCB.Checked)
            {
                BuyPriceNUD.Value = -1;
                BuyPriceNUD.Enabled = false;
            }
            else
            {
                BuyPriceNUD.Enabled = true;
                BuyPriceNUD.Value = 1;
            }
        }
        private void BuyPriceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            //currentItemProducts.BuyPrice = (int)BuyPriceNUD.Value;
           
            if (CurrentTraderCatLB.SelectedItems.Count > 0)
            {
                foreach (var item in CurrentTraderCatLB.SelectedItems)
                {
                    ItemProducts pitem = item as ItemProducts;
                    pitem.BuyPrice = (int)BuyPriceNUD.Value;
                    TraderPlusPriceConfig.isDirty = true;
                }
            }
        }
        private void CantSellCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CantSellCB.Checked)
            {
                SellpriceNUD.Value = -1;
                SellpriceNUD.Enabled = false;
            }
            else
            {
                SellpriceNUD.Enabled = true;
                SellpriceNUD.Value = 1;
            }
        }
        private void SellpriceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CurrentTraderCatLB.SelectedItems.Count > 0)
            {
                foreach (var item in CurrentTraderCatLB.SelectedItems)
                {
                    ItemProducts pitem = item as ItemProducts;
                    pitem.Sellprice = (float)SellpriceNUD.Value;
                    TraderPlusPriceConfig.isDirty = true;
                }
            }
        }
        private void DestockCoeffTB_MouseUp(object sender, MouseEventArgs e)
        {
            if (!useraction) return;
            currentItemProducts.destockCoefficent = DestockCoeffTB.Value;
            if (CurrentTraderCatLB.SelectedItems.Count > 1)
            {
                foreach (var item in CurrentTraderCatLB.SelectedItems)
                {
                    ItemProducts pitem = item as ItemProducts;
                    pitem.destockCoefficent = DestockCoeffTB.Value;
                    pitem.UseDestockCoeff = true;
                }
            }
            TraderPlusPriceConfig.isDirty = true;
        }
        private void UsedestockCoeffCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (CurrentTraderCatLB.SelectedItems.Count > 0)
            {
                if (UsedestockCoeffCB.Checked)
                {
                    DestockCoefflabel.Visible = true;
                    DestockCoeffTB.Visible = true;
                    foreach (var item in CurrentTraderCatLB.SelectedItems)
                    {

                        ItemProducts pitem = item as ItemProducts;
                        pitem.UseDestockCoeff = true;
                        
                    }
                    TraderPlusPriceConfig.isDirty = true;
                }
                else
                {
                    DestockCoefflabel.Visible = false;
                    DestockCoeffTB.Visible = false;
                    foreach (var item in CurrentTraderCatLB.SelectedItems)
                    {

                        ItemProducts pitem = item as ItemProducts;
                        pitem.UseDestockCoeff = false;
                    }
                    TraderPlusPriceConfig.isDirty = true;
                }
            }
        }
        private void CoefficientTB_ValueChanged(object sender, EventArgs e)
        {
            CoefficientPercentLabel.Text = currentItemProducts.Coefficient.ToString() + "%";
        }
        private void DestockCoeffTB_ValueChanged(object sender, EventArgs e)
        {
            DestockCoefflabel.Text = currentItemProducts.destockCoefficent.ToString() + "%";
        }
        private void CoefficientTB_Move(object sender, EventArgs e)
        {
            CoefficientPercentLabel.Text = CoefficientTB.Value.ToString() + "%";
        }
        private void DestockCoeffTB_Move(object sender, EventArgs e)
        {
            DestockCoefflabel.Text = DestockCoeffTB.Value.ToString() + "%";
        }
        private void CoefficientTB_Scroll(object sender, EventArgs e)
        {
            CoefficientPercentLabel.Text = CoefficientTB.Value.ToString() + "%";
        }
        private void DestockCoeffTB_Scroll(object sender, EventArgs e)
        {
            DestockCoefflabel.Text = DestockCoeffTB.Value.ToString() + "%";
        }
        private void setMaxStockForSelectedCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Input precentage of Buy Price ", "Sell Price", "");
            if (UserAnswer == "") return;
            int value = Convert.ToInt32(UserAnswer);
            foreach (ItemProducts item in currentTradercategory.itemProducts)
            {
                if(item.Sellprice == -1 || item.BuyPrice == -1) { continue; }
                decimal num1 = (decimal)item.BuyPrice / 100;
                decimal num2 = num1 * value;
                item.Sellprice = (int)Math.Round(num2, MidpointRounding.AwayFromZero);
            }
            TraderPlusPriceConfig.isDirty = true;
        }
        private void darkButton55_Click(object sender, EventArgs e)
        {
            if (darkButton55.Tag.ToString() == "Info")
            {
                darkButton55.Text = "Show item info";
                darkButton55.Tag = "Move";
                MoveToCategoryGroupBox.Visible = true;
                ItemInfoGroupBox.Visible = false;
                List<Tradercategory> returnlist = TraderPlusPriceConfig.getallCats();
                MoveToCatLB.Items.Clear();
                MoveToCatLB.Items.AddRange(returnlist.ToArray());
            }
            else if (darkButton55.Tag.ToString() == "Move")
            {
                darkButton55.Text = "Show item Move Category";
                darkButton55.Tag = "Info";
                MoveToCategoryGroupBox.Visible = false;
                ItemInfoGroupBox.Visible = true;
                MoveToCatLB.Items.Clear();
            }
        }
        private void AddItemtoCategory(bool remove)
        {
            if (CurrentTraderCatLB.SelectedItems.Count == 0) return;
            List<ItemProducts> moveitem = new List<ItemProducts>();
            foreach (var item in CurrentTraderCatLB.SelectedItems)
            {
                ItemProducts pitem = item as ItemProducts;
                moveitem.Add(pitem);
            }
            foreach (ItemProducts item in moveitem)
            {
                Tradercategory movetocat = MoveToCatLB.SelectedItem as Tradercategory;
                if (!movetocat.itemProducts.Any(x => x.Classname == item.Classname))
                {
                    movetocat.AdditemProduct(item);
                    if (remove)
                        currentTradercategory.removeItemProduct(item);
                }
                else
                {
                    MessageBox.Show("Item allready exists in destication category, no action taken.");
                    Console.WriteLine("Item allready exists in destication category, no action taken.\n");
                }
            }
        }
        private void darkButton57_Click(object sender, EventArgs e)
        {
            AddItemtoCategory(true);
            TraderPlusPriceConfig.isDirty = true;
        }
        private void darkButton59_Click(object sender, EventArgs e)
        {

            AddItemtoCategory(false);
            TraderPlusPriceConfig.isDirty = true;
        }
        #endregion traderprice

        #region SafeZone
        public Safearealocation currentsafezone;
        public int ZoneScale = 1;
        private Point _mouseLastPosition;
        private Point _newscrollPosition;
        private Rectangle doubleClickRectangle = new Rectangle();
        private Timer doubleClickTimer = new Timer();
        private bool isFirstClick = true;
        private bool isDoubleClick = false;
        private int milliseconds = 0;
        private MouseEventArgs mouseeventargs;

        private void SetupTraderPlusSafeZoneConfig()
        {
            useraction = false;
            EnableAfkDisconnectCB.Checked = TraderPlusSafeZoneConfig.EnableAfkDisconnect == 1 ? true : false;
            KickAfterDelayNUD.Value = (int)TraderPlusSafeZoneConfig.KickAfterDelay;
            MsgEnterZoneTB.Text = TraderPlusSafeZoneConfig.MsgEnterZone;
            MsgExitZoneTB.Text = TraderPlusSafeZoneConfig.MsgExitZone;
            MsgOnLeavingZoneTB.Text = TraderPlusSafeZoneConfig.MsgOnLeavingZone;
            CleanUpTimerNUD.Value = (int)TraderPlusSafeZoneConfig.CleanUpTimer;
            MustRemoveArmbandTB.Text = TraderPlusSafeZoneConfig.MustRemoveArmband;
            IsHideOutActiveCB.Checked = TraderPlusSafeZoneConfig.IsHideOutActive == 1 ? true : false;

            ObjectsToDeleteLB.DisplayMember = "Name";
            ObjectsToDeleteLB.ValueMember = "Value";
            ObjectsToDeleteLB.DataSource = TraderPlusSafeZoneConfig.ObjectsToDelete;

            SZSteamUIDsLB.DisplayMember = "Name";
            SZSteamUIDsLB.ValueMember = "Value";
            SZSteamUIDsLB.DataSource = TraderPlusSafeZoneConfig.SZSteamUIDs;

            SafeAreaLocationLB.DisplayMember = "Name";
            SafeAreaLocationLB.ValueMember = "Value";
            SafeAreaLocationLB.DataSource = TraderPlusSafeZoneConfig.SafeAreaLocation;

            BlackListedItemInStashLB.DisplayMember = "Name";
            BlackListedItemInStashLB.ValueMember = "Value";
            BlackListedItemInStashLB.DataSource = TraderPlusSafeZoneConfig.BlackListedItemInStash;

            useraction = true;
        }
        private void IsHideOutActiveCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusSafeZoneConfig.IsHideOutActive = IsHideOutActiveCB.Checked == true ? 1 : 0;
            TraderPlusSafeZoneConfig.isDirty = true;
        }
        private void EnableAfkDisconnectCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusSafeZoneConfig.EnableAfkDisconnect = EnableAfkDisconnectCB.Checked == true ? 1 : 0;
            TraderPlusSafeZoneConfig.isDirty = true;
        }
        private void KickAfterDelayNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusSafeZoneConfig.KickAfterDelay = (int)KickAfterDelayNUD.Value;
            TraderPlusSafeZoneConfig.isDirty = true;
        }
        private void MsgEnterZoneTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusSafeZoneConfig.MsgEnterZone = MsgEnterZoneTB.Text;
            TraderPlusSafeZoneConfig.isDirty = true;
        }
        private void MsgExitZoneTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusSafeZoneConfig.MsgExitZone = MsgExitZoneTB.Text;
            TraderPlusSafeZoneConfig.isDirty = true;
        }
        private void MsgOnLeavingZoneTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusSafeZoneConfig.MsgOnLeavingZone = MsgOnLeavingZoneTB.Text;
            TraderPlusSafeZoneConfig.isDirty = true;
        }
        private void CleanUpTimerNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusSafeZoneConfig.CleanUpTimer = (int)CleanUpTimerNUD.Value;
            TraderPlusSafeZoneConfig.isDirty = true;
        }
        private void darkButton54_Click(object sender, EventArgs e)
        {
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    TraderPlusSafeZoneConfig.ObjectsToDelete.Add(l);
                    TraderPlusVehiclesConfig.isDirty = true;
                }
            }
        }
        private void darkButton53_Click(object sender, EventArgs e)
        {
            TraderPlusSafeZoneConfig.ObjectsToDelete.Remove(ObjectsToDeleteLB.GetItemText(ObjectsToDeleteLB.SelectedItem));
            TraderPlusSafeZoneConfig.isDirty = true;
        }
        private void darkButton10_Click(object sender, EventArgs e)
        {
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    TraderPlusSafeZoneConfig.SZSteamUIDs.Add(l);
                    TraderPlusSafeZoneConfig.isDirty = true;
                }
            }
        }
        private void darkButton9_Click(object sender, EventArgs e)
        {
            TraderPlusSafeZoneConfig.SZSteamUIDs.Remove(SZSteamUIDsLB.GetItemText(SZSteamUIDsLB.SelectedItem));
            TraderPlusSafeZoneConfig.isDirty = true;
            if (SZSteamUIDsLB.Items.Count == 0)
                SZSteamUIDsLB.SelectedIndex = -1;
            else
                SZSteamUIDsLB.SelectedIndex = 0;
        }
        private void darkButton26_Click(object sender, EventArgs e)
        {
            float[] centre = new float[] { currentproject.MapSize / 2, 0, currentproject.MapSize / 2 };
            Safearealocation newsafezone = new Safearealocation()
            {
                SafeZoneStatut = "NewSafeZone",
                X = centre[0],
                Y = centre[2],
                Radius = 500,
                Countdown = 30
            };
            TraderPlusSafeZoneConfig.SafeAreaLocation.Add(newsafezone);
            SafeAreaLocationLB.SelectedIndex = SafeAreaLocationLB.Items.Count - 1;
            TraderPlusSafeZoneConfig.isDirty = true;
        }
        private void darkButton25_Click(object sender, EventArgs e)
        {
            TraderPlusSafeZoneConfig.SafeAreaLocation.Remove(currentsafezone);
            TraderPlusSafeZoneConfig.isDirty = true;
            pictureBox2.Invalidate();
            if (SafeAreaLocationLB.Items.Count == 0)
                SafeAreaLocationLB.SelectedIndex = -1;
            else
                SafeAreaLocationLB.SelectedIndex = 0;


        }
        private void SafeZoneStatutTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentsafezone.SafeZoneStatut = SafeZoneStatutTB.Text;
            TraderPlusSafeZoneConfig.isDirty = true;
        }
        private void SafeAreaLocationLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SafeAreaLocationLB.SelectedItems.Count < 1) return;
            currentsafezone = SafeAreaLocationLB.SelectedItem as Safearealocation;

            SafeZoneStatutTB.Text = currentsafezone.SafeZoneStatut;
            SafeZoneXNUD.Value = (decimal)currentsafezone.X;
            safeZoneZNUD.Value = (decimal)currentsafezone.Y;
            SafeZoneradiusNUD.Value = (decimal)currentsafezone.Radius;
            SafeZoneCountdownNUD.Value = (int)currentsafezone.Countdown;

            pictureBox2.Invalidate();
        }
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Cursor.Current = Cursors.SizeAll;
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
                    if (currentsafezone == null) return;
                    Cursor.Current = Cursors.WaitCursor;
                    decimal scalevalue = ZoneScale * (decimal)0.05;
                    decimal mapsize = currentproject.MapSize;
                    int newsize = (int)(mapsize * scalevalue);
                    currentsafezone.X = (float)Decimal.Round((decimal)(mouseeventargs.X / scalevalue), 4);
                    currentsafezone.Y = (float)Decimal.Round((decimal)((newsize - mouseeventargs.Y) / scalevalue), 4);
                    Cursor.Current = Cursors.Default;
                    TraderPlusSafeZoneConfig.isDirty = true;
                    pictureBox2.Invalidate();
                }
                else
                {
                    //Console.WriteLine("Perform single click action");
                    if (currentsafezone == null) return;
                    decimal scalevalue = ZoneScale * (decimal)0.05;
                    decimal mapsize = currentproject.MapSize;
                    int newsize = (int)(mapsize * scalevalue);
                    PointF pC = new PointF((float)Decimal.Round((decimal)(mouseeventargs.X / scalevalue), 4), (float)Decimal.Round((decimal)((newsize - mouseeventargs.Y) / scalevalue), 4));
                    foreach (Safearealocation tz in TraderPlusSafeZoneConfig.SafeAreaLocation)
                    {
                        PointF pP = new PointF(tz.X, tz.Y);
                        if (IsWithinCircle(pC, pP, (float)tz.Radius))
                        {
                            SafeAreaLocationLB.SelectedIndex = -1;
                            SafeAreaLocationLB.SelectedItem = tz;
                            SafeAreaLocationLB.Refresh();
                            continue;
                        }
                    }
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
            label43.Text = Decimal.Round((decimal)(e.X / scalevalue), 4) + "," + Decimal.Round((decimal)((newsize - e.Y) / scalevalue), 4);
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
            SetSafeZonescale();
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
            SetSafeZonescale();
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
        private void SetSafeZonescale()
        {
            float scalevalue = ZoneScale * 0.05f;
            float mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            pictureBox2.Size = new Size(newsize, newsize);
        }
        private void trackBar4_MouseUp(object sender, MouseEventArgs e)
        {
            ZoneScale = trackBar4.Value;
            SetSafeZonescale();
        }
        private void DrawAllSafeZones(object sender, PaintEventArgs e)
        {
            foreach (Safearealocation zones in TraderPlusSafeZoneConfig.SafeAreaLocation)
            {
                float scalevalue = ZoneScale * 0.05f;
                int centerX = (int)(Math.Round(zones.X) * scalevalue);
                int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(zones.Y, 0) * scalevalue);
                int radius = (int)(Math.Round(zones.Radius, 0) * scalevalue);
                Point center = new Point(centerX, centerY);
                Pen pen = new Pen(Color.Red, 4);
                if (zones == currentsafezone)
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
        private void SafeZoneXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentsafezone.X = (float)SafeZoneXNUD.Value;
            TraderPlusSafeZoneConfig.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void safeZoneZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentsafezone.Y = (float)safeZoneZNUD.Value;
            TraderPlusSafeZoneConfig.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void SafeZoneradiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentsafezone.Radius = (float)SafeZoneradiusNUD.Value;
            TraderPlusSafeZoneConfig.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void SafeZoneCountdownNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentsafezone.Countdown = (int)SafeZoneCountdownNUD.Value;
            TraderPlusSafeZoneConfig.isDirty = true;
        }
        private void MustRemoveArmbandTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusSafeZoneConfig.MustRemoveArmband = MustRemoveArmbandTB.Text;
            TraderPlusSafeZoneConfig.isDirty = true;
        }
        private void darkButton58_Click(object sender, EventArgs e)
        {
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    TraderPlusSafeZoneConfig.BlackListedItemInStash.Add(l);
                    TraderPlusVehiclesConfig.isDirty = true;
                }
            }
        }
        private void darkButton56_Click(object sender, EventArgs e)
        {
            TraderPlusSafeZoneConfig.BlackListedItemInStash.Remove(BlackListedItemInStashLB.GetItemText(BlackListedItemInStashLB.SelectedItem));
            TraderPlusSafeZoneConfig.isDirty = true;
        }
        #endregion SafeZone

        #region Vehicle Settings
        public Vehiclespart currentVehiclespart { get; set; }

        private void SetupTraderPlusVehiclesConfig()
        {
            useraction = false;

            VehiclePartLB.DisplayMember = "Name";
            VehiclePartLB.ValueMember = "Value";
            VehiclePartLB.DataSource = TraderPlusVehiclesConfig.VehiclesParts;

            useraction = true;
        }
        private void VehiclePartLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (VehiclePartLB.SelectedItems.Count < 1) return;
            currentVehiclespart = VehiclePartLB.SelectedItem as Vehiclespart;

            useraction = false;
            VehicleNameTB.Text = currentVehiclespart.VehicleName;
            vehicleHeightNUD.Value = currentVehiclespart.Height;
            InsurancePriceCoefficientNUD.Value = (decimal)currentVehiclespart.Insurance.InsurancePriceCoefficient;
            CollateralMoneyCoefficientNUD.Value = (decimal)currentVehiclespart.Insurance.CollateralMoneyCoefficient;

            VehiclePartPartsLB.DisplayMember = "Name";
            VehiclePartPartsLB.ValueMember = "Value";
            VehiclePartPartsLB.DataSource = currentVehiclespart.VehicleParts;

            useraction = true;
        }
        private void darkButton20_Click(object sender, EventArgs e)
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
                    Vehiclespart newvehicle = new Vehiclespart()
                    {
                        VehicleName = l,
                        Height = 0,
                        VehicleParts = new BindingList<string>(),
                        Insurance = new Insurance()
                        {
                            VehicleName = l,
                            CollateralMoneyCoefficient = 1.0f,
                            InsurancePriceCoefficient = 0.2f
                        }
                    };
                    TraderPlusVehiclesConfig.VehiclesParts.Add(newvehicle);
                    TraderPlusVehiclesConfig.isDirty = true;
                }
            }
        }
        private void darkButton15_Click(object sender, EventArgs e)
        {
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    Vehiclespart newvehicle = new Vehiclespart()
                    {
                        VehicleName = l,
                        Height = 0,
                        VehicleParts = new BindingList<string>()
                    };
                    TraderPlusVehiclesConfig.VehiclesParts.Add(newvehicle);
                    TraderPlusVehiclesConfig.isDirty = true;
                }
            }
        }
        private void darkButton16_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This Will Remove The All reference to this/these Vehicle/s, Are you sure you want to do this?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (VehiclePartLB.SelectedItems.Count >= 1)
                {
                    List<Vehiclespart> removelist = VehiclePartLB.SelectedItems.Cast<Vehiclespart>().ToList();
                    foreach (Vehiclespart item in removelist)
                    {
                        TraderPlusVehiclesConfig.VehiclesParts.Remove(item);
                        TraderPlusVehiclesConfig.isDirty = true;
                    }
                    if (VehiclePartLB.Items.Count == 0)
                        VehiclePartLB.SelectedIndex = -1;
                    else
                        VehiclePartLB.SelectedIndex = 0;
                }
            }
        }
        private void darkButton19_Click(object sender, EventArgs e)
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
                if (addedtypes.Count > 0)
                {
                    foreach (string l in addedtypes)
                    {
                        if (VehiclePartLB.SelectedItems.Count > 0)
                        {
                            List<Vehiclespart> VehicleList = VehiclePartLB.SelectedItems.Cast<Vehiclespart>().ToList();
                            foreach (Vehiclespart vehicle in VehicleList)
                            {
                                vehicle.VehicleParts.Add(l);
                            }
                        }
                    }
                    TraderPlusVehiclesConfig.isDirty = true;
                }
            }
        }
        private void darkButton17_Click(object sender, EventArgs e)
        {
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    currentVehiclespart.VehicleParts.Add(l);
                }
                TraderPlusVehiclesConfig.isDirty = true;
            }
        }
        private void darkButton18_Click(object sender, EventArgs e)
        {
            currentVehiclespart.VehicleParts.Remove(VehiclePartPartsLB.GetItemText(VehiclePartPartsLB.SelectedItem));
            TraderPlusVehiclesConfig.isDirty = true;
        }
        private void vehicleHeightNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (VehiclePartLB.SelectedItems.Count > 0)
            {
                foreach (Vehiclespart item in VehiclePartLB.SelectedItems)
                {
                    item.Height = (int)vehicleHeightNUD.Value;
                }
                TraderPlusVehiclesConfig.isDirty = true;
            }
        }
        private void InsurancePriceCoefficientNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (VehiclePartLB.SelectedItems.Count > 0)
            {
                foreach (Vehiclespart item in VehiclePartLB.SelectedItems)
                {
                    item.Insurance.InsurancePriceCoefficient = (float)Math.Round(InsurancePriceCoefficientNUD.Value, 2);
                }
                TraderPlusVehiclesConfig.isDirty = true;
            }
        }
        private void CollateralMoneyCoefficientNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (VehiclePartLB.SelectedItems.Count > 0)
            {
                foreach (Vehiclespart item in VehiclePartLB.SelectedItems)
                {
                    item.Insurance.CollateralMoneyCoefficient = (float)CollateralMoneyCoefficientNUD.Value;
                }
                TraderPlusVehiclesConfig.isDirty = true;
            }
        }
        #endregion Vehicle Settings

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            List<string> catlist = new List<string>();
            foreach (Trader t in TraderPlusGeneralConfig.Traders)
            {
                if (t.isBanker) { continue; }
                foreach (string s in t.TraderCategoryList.Categories)
                {
                    if (!catlist.Contains(s))
                        catlist.Add(s);
                }
            }
            List<string> nottrader = new List<string>();
            foreach(Tradercategory cat in TraderPlusPriceConfig.TraderCategories)
            {
                if (!catlist.Contains(cat.CategoryName))
                    nottrader.Add(cat.CategoryName);
            }
            if (nottrader.Count == 0)
                MessageBox.Show("All categorys are assigned to a trader");
            else
            {
                string message = "The following categorys are not assigned to a trader:-";
                foreach(String s in nottrader)
                {
                    message += "\n" + s;
                }
                MessageBox.Show(message);
            }
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\TraderPlus\\TraderPlusConfig");
        }
        private void setCooefToMinPercentageOfBuyPriceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Input min buy precentage of Buy Price ", "Min buy Price", "");
            if (UserAnswer == "") return;
            int value = Convert.ToInt32(UserAnswer);
            foreach (ItemProducts item in currentTradercategory.itemProducts)
            {
                if (item.BuyPrice <= 8) continue;
                decimal num1 = (decimal)item.BuyPrice / 100;
                decimal minBuyPrice = num1 * value;
                item.Coefficient = (int)(Math.Pow((double)(minBuyPrice / item.BuyPrice), (double)(1 / (float)(item.MaxStock - 1))) * 100);
            }
               
            TraderPlusPriceConfig.isDirty = true;
        }
        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Input Percentage to change current category ", "Price change by perecntage", "");
            if (UserAnswer == "") return;
            int value = Convert.ToInt32(UserAnswer);
            foreach (ItemProducts item in currentTradercategory.itemProducts)
            {
                decimal percent = (decimal)value / 100;
                percent += 1;
                item.BuyPrice = (int)((decimal)item.BuyPrice * percent);
                if(item.Sellprice > 1.0)
                {
                    item.Sellprice = (int)((decimal)item.Sellprice * percent);
                }
            }
            TraderPlusPriceConfig.isDirty = true;
        }
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Multiselect = true;
            if(openfile.ShowDialog() == DialogResult.OK)
            {
                foreach (String file in openfile.FileNames)
                {
                    Categories cat = JsonSerializer.Deserialize<Categories>(File.ReadAllText(file));
                    Tradercategory newcat = new Tradercategory()
                    {
                        CategoryName = cat.DisplayName,
                        Products = new BindingList<string>(),
                        itemProducts = new BindingList<ItemProducts>()
                    };
                    foreach (marketItem item in cat.Items)
                    {
                        string propperclassname = currentproject.getcorrectclassamefromtypes(item.ClassName);
                        ItemProducts NewContainer = new ItemProducts
                        {
                            Classname = propperclassname,
                            Coefficient = 100,
                            MaxStock = -1,
                            TradeQuantity = -1,
                            BuyPrice = item.MaxPriceThreshold,
                            Sellprice = 0,
                            destockCoefficent = 100,
                            UseDestockCoeff = false
                        };
                        if (!Checkifincat(NewContainer, newcat))
                        {
                            newcat.AdditemProduct(NewContainer);
                        }
                        if (item.Variants != null && item.Variants.Count > 0)
                        {
                            foreach (String itemv in item.Variants)
                            {
                                string propperclassnamev = currentproject.getcorrectclassamefromtypes(itemv);
                                ItemProducts NewContainerv = new ItemProducts
                                {
                                    Classname = propperclassnamev,
                                    Coefficient = 100,
                                    MaxStock = -1,
                                    TradeQuantity = -1,
                                    BuyPrice = item.MaxPriceThreshold,
                                    Sellprice = 0,
                                    destockCoefficent = 100,
                                    UseDestockCoeff = false
                                };
                                if (!Checkifincat(NewContainerv, newcat))
                                {
                                    newcat.AdditemProduct(NewContainerv);
                                }
                            }
                        }
                    }
                    if (!Checkifincatlist(newcat))
                    {
                        TraderPlusPriceConfig.TraderCategories.Add(newcat);
                        TraderPlusPriceConfig.isDirty = true;
                    }
                    else
                    {
                        MessageBox.Show("You have done this allready, Dummy!!!!\nExpansion json - " + Path.GetFileNameWithoutExtension(file));
                    }
                }
            }
        }
        private bool Checkifincatlist(Tradercategory tradercat)
        {
            if (TraderPlusPriceConfig.TraderCategories.Any(x => x.CategoryName == tradercat.CategoryName))
            {
                return true;
            }
            return false;
        }
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            TraderPlusPriceConfig.TraderCategories = new BindingList<Tradercategory>();
            TraderPlusPriceConfig.isDirty = true;
            CurrentTraderCatLB.DataSource = null;
            SetupTraderPlusPriceConfig();
        }
        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            foreach (Tradercategory tc in TraderPlusPriceConfig.TraderCategories)
            {
                foreach (ItemProducts item in tc.itemProducts)
                {
                    if (item.Classname.StartsWith("*** MISSING ITEM TYPE ("))
                    {
                        string oldclassname = item.Classname.Replace("*** MISSING ITEM TYPE (", "");
                        oldclassname = oldclassname.Replace(")***", "");
                        string propperclassname = currentproject.getcorrectclassamefromtypes(oldclassname);
                        if (propperclassname != item.Classname)
                        {
                            item.Classname = propperclassname;
                            TraderPlusPriceConfig.isDirty = true;
                        }
                    }
                }
            }
        }
        private void toolStripButton9_Click(object sender, EventArgs e)
        {

        }
        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            foreach (Tradercategory tc in TraderPlusPriceConfig.TraderCategories)
            {
                foreach (ItemProducts item in tc.itemProducts)
                {
                    item.Classname = currentproject.getcorrectclassamefromtypes(item.Classname);
                }
            }
        }
        private void TraderPlus_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool needtosave = false;
            if (TraderPlusBankingConfig.isDirty)
            {
                needtosave = true;
            }
            if (TraderPlusGarageConfig.isDirty)
            {
                needtosave = true;
            }
            if (TraderPlusGeneralConfig.isDirty)
            {
                needtosave = true;
            }
            if (TraderPlusVehiclesConfig.isDirty)
            {
                needtosave = true;
            }
            if (TraderPlusInsuranceConfig.isDirty)
            {
                needtosave = true;
            }
            if (TraderPlusSafeZoneConfig.isDirty)
            {
                needtosave = true;
            }
            if (TraderPlusPriceConfig.isDirty)
            {
                needtosave = true;
            }
            if (TraderPlusIDsConfig.isDirty)
            {
                needtosave = true;
            }
            if (needtosave)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SaveTraderfiles();
                }
            }
        }
        private void ExporttoexpansionMarket_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            List<marketItem> checklist = new List<marketItem>();
            MarketCategories MarketCats = new MarketCategories();
            TradersList traders = new TradersList();
            TraderModelMapping maps = new TraderModelMapping();
            TraderZones zone = new TraderZones();

            //Setup currency Market Category
            Categories newccat = new Categories();
            newccat.DisplayName = "#STR_EXPANSION_MARKET_CATEGORY_EXCHANGE";
            newccat.Filename = "EXCHANGE.json";
            newccat.IsExchange = 1;
            foreach (Currency currency in TraderPlusGeneralConfig.Currencies)
            {
                foreach (string c in currency.CurrenciesNames)
                {
                    marketItem newitem = new marketItem() 
                    { 
                        ClassName = c.ToLower(),
                        MaxPriceThreshold = currency.Value,
                        MinPriceThreshold = currency.Value,
                        MaxStockThreshold = 1,
                        MinStockThreshold = 1 
                    };
                    checklist.Add(newitem);
                    newccat.Items.Add(newitem);
                }
            }
            MarketCats.CatList.Add(newccat);

            foreach (Tradercategory cat in TraderPlusPriceConfig.TraderCategories)
            {
                Categories newcat = MarketCats.GetCatFromDisplayName(cat.CategoryName);
                if (newcat == null)
                {
                    newcat = new Categories();
                    newcat.DisplayName = cat.CategoryName;
                    newcat.Filename = ReplaceInvalidChars(newcat.DisplayName) + ".json";
                    newcat.Filename = newcat.Filename.Replace(" ", "_");
                    newcat.Filename = newcat.Filename.ToUpper();
                }
                foreach (ItemProducts item in cat.itemProducts)
                {
                    marketItem newitem = new marketItem();
                    //expansion classname
                    newitem.ClassName = item.Classname.ToLower();
                    if (TraderPlusVehiclesConfig.VehiclesParts.Any(x => x.VehicleName.ToLower() == newitem.ClassName))
                    {
                        Vehiclespart v = TraderPlusVehiclesConfig.VehiclesParts.FirstOrDefault(x => x.VehicleName.ToLower() == newitem.ClassName);
                        foreach (string Part in v.VehicleParts)
                        {
                            newitem.SpawnAttachments.Add(Part.ToLower());
                        }
                    }
                    //expansion max/min price and sell price percent
                    if(item.BuyPrice == -1)
                    {
                        newitem.MaxPriceThreshold = (int)item.Sellprice;
                        newitem.MinPriceThreshold = (int)item.Sellprice;
                        newitem.SellPricePercent = 100;
                    }
                    else
                    {
                        newitem.MaxPriceThreshold = item.BuyPrice;
                        newitem.MinPriceThreshold = (int)(((float)item.BuyPrice / 100) * item.Coefficient);
                        if(item.Sellprice < 1)
                        {
                            newitem.SellPricePercent = (int)(item.Sellprice * 100);
                            if (newitem.SellPricePercent == -100)
                                newitem.SellPricePercent = 100;
                        }
                        else
                        {
                            int newsellprice = (int)((item.Sellprice / (float)newitem.MaxPriceThreshold) * 100);
                            newitem.SellPricePercent = newsellprice;
                        }
                    }
                    //expansion max and min stock
                    if (item.MaxStock == -1)
                    {
                        newitem.MaxStockThreshold = 1;
                    }
                    else
                    {
                        newitem.MaxStockThreshold = item.MaxStock;
                    }
                    newitem.MinStockThreshold = 1;
                    //exansion quantity
                    if (item.TradeQuantity <= 1)
                    {
                        newitem.QuantityPercent = (int)(item.TradeQuantity * 100);
                        if (newitem.QuantityPercent == 100)
                            newitem.QuantityPercent = -1;
                    }
                    else
                    {
                        newitem.QuantityPercent = -1;
                    }

                    if (newitem.MaxPriceThreshold < newitem.MinPriceThreshold)
                        newitem.MinPriceThreshold = newitem.MaxPriceThreshold;
                    
                    if (newitem.MaxStockThreshold < newitem.MinStockThreshold)
                        newitem.MinStockThreshold = newitem.MaxStockThreshold;


                    marketItem checkitem = checklist.FirstOrDefault(x => x.ClassName == newitem.ClassName);
                    //if (!checklist.Any(x => x.ClassName == newitem.ClassName))
                    
                    if (checkitem == null)
                    {
                        checklist.Add(newitem);
                        newcat.Items.Add(newitem);
                    }
                    else
                    {
                        sb.AppendLine(newitem.ClassName + " is allready in " + MarketCats.GetCatNameFromItemName(newitem.ClassName) + "\n will not be added to " + newcat.DisplayName + "\n");
                    }
                }
                MarketCats.CatList.Add(newcat);
                
            }


            if (sb.Length != 0)
                MessageBox.Show(sb.ToString());

            
            Traders exchangetrader = new Traders("Exchange");
            exchangetrader.DisplayName = "#STR_EXPANSION_MARKET_TRADER_EXCHANGE";
            exchangetrader.Filename = "EXCHANGE";
            foreach (Currency currency in TraderPlusGeneralConfig.Currencies)
            {
                foreach (string c in currency.CurrenciesNames)
                {
                    exchangetrader.Currencies.Add(c);
                }
            }
            exchangetrader.Categories.Add("EXCHANGE");
            traders.Traderlist.Add(exchangetrader);

            foreach(Trader trader in TraderPlusGeneralConfig.Traders)
            {
                Traders newtrader = new Traders(trader.Role);
                newtrader.Filename = trader.Role.Replace(" ", "_").ToUpper();
                IDs newid = TraderPlusIDsConfig.getTraderbyID(trader.Id);

                foreach (string CurrenciesAccepted in  newid.CurrenciesAccepted)
                {
                    newtrader.Currencies.Add(CurrenciesAccepted);
                }
                foreach (string cats in newid.Categories)
                {
                    newtrader.Categories.Add(cats.Replace(" ", "_").ToUpper());
                }
                traders.Traderlist.Add(newtrader);

                Tradermap newmap = new Tradermap();
                newmap.NPCName = "ExpansionTrader" + trader.Name.Split('_')[1];
                newmap.NPCTrade = trader.Role.Replace(" ", "_").ToUpper();
                newmap.position = new Vec3(new string[] {trader.Position[0].ToString(), trader.Position[2].ToString(), trader.Position[2].ToString() });
                newmap.roattions = new Vec3(new string[] { trader.Orientation[0].ToString(), trader.Orientation[2].ToString(), trader.Orientation[2].ToString() });
                newmap.Attachments = new BindingList<string>(trader.Clothes.ToList());
                maps.maps.Add(newmap);
            }

            zone.NewTraderZone("WORLD", currentproject.MapSize, false);

            string ExpansionPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod_Market_Convert_from_TraderPlus";
            if (!Directory.Exists(ExpansionPath))
            {
                Directory.CreateDirectory(ExpansionPath);
            }
            else
            {
                Directory.Delete(ExpansionPath);
            }
            foreach (Traders trader in traders.Traderlist)
            {
                Directory.CreateDirectory(ExpansionPath + "\\Traders");
                string traderFilename = ExpansionPath + "\\Traders\\" + trader.Filename + ".json";
                var options = new JsonSerializerOptions();
                options.WriteIndented = true;
                options.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                string jsonString = JsonSerializer.Serialize(trader, options);
                File.WriteAllText(traderFilename, jsonString);
            }
            foreach (Categories cat in MarketCats.CatList)
            {
                Directory.CreateDirectory(ExpansionPath + "\\Market");
                string catFilename = ExpansionPath + "\\Market\\" + cat.Filename;
                var options = new JsonSerializerOptions();
                options.WriteIndented = true;
                options.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                string jsonString = JsonSerializer.Serialize(cat, options);
                File.WriteAllText(catFilename, jsonString);
            }

            foreach (Zones zones in zone.ZoneList)
            {
                Directory.CreateDirectory(ExpansionPath + "\\traderzones");
                string ZoneFilename = ExpansionPath + "\\traderzones\\" + zones.Filename + ".json";
                var options = new JsonSerializerOptions();
                options.WriteIndented = true;
                options.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                string jsonString = JsonSerializer.Serialize(zones, options);
                File.WriteAllText(ZoneFilename, jsonString);
            }

            maps.TradermapsPath = ExpansionPath;
            foreach(Tradermap map in maps.maps)
            {
                map.Filename = ExpansionPath + "\\TraderMaps.map";
            }
            maps.savefiles();
            MessageBox.Show("Convertion Complete...\nfiles stored in folder called\nExpansionMod_Market_Convert_from_TraderPlus\nWithin the project profiles folder.");
        }
        public string ReplaceInvalidChars(string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }


    }
}
