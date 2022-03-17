using DarkUI.Forms;
using DayZeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            if (TraderPlusBankingConfig.isDirty)
            {
                TraderPlusBankingConfig.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(TraderPlusBankingConfig, options);
                File.WriteAllText(TraderPlusBankingConfig.FullFilename, jsonString);
                midifiedfiles.Add(Path.GetFileName(TraderPlusBankingConfig.fileName));
            }
            if (TraderPlusGarageConfig.isDirty)
            {
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
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(TraderPlusGeneralConfig, options);
                File.WriteAllText(TraderPlusGeneralConfig.FullFilename, jsonString);
                midifiedfiles.Add(Path.GetFileName(TraderPlusGeneralConfig.fileName));
            }
            if (TraderPlusVehiclesConfig.isDirty)
            {
                TraderPlusVehiclesConfig.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(TraderPlusVehiclesConfig, options);
                File.WriteAllText(TraderPlusVehiclesConfig.FullFilename, jsonString);
                midifiedfiles.Add(Path.GetFileName(TraderPlusVehiclesConfig.fileName));
            }
            if (TraderPlusSafeZoneConfig.isDirty)
            {
                TraderPlusSafeZoneConfig.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(TraderPlusSafeZoneConfig, options);
                File.WriteAllText(TraderPlusSafeZoneConfig.FullFilename, jsonString);
                midifiedfiles.Add(Path.GetFileName(TraderPlusSafeZoneConfig.fileName));
            }
            if (TraderPlusPriceConfig.isDirty)
            {
                TraderPlusPriceConfig.isDirty = false;
                TraderPlusPriceConfig.SetProducts();
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(TraderPlusPriceConfig, options);
                File.WriteAllText(TraderPlusPriceConfig.FullFilename, jsonString);
                midifiedfiles.Add(Path.GetFileName(TraderPlusPriceConfig.fileName));
            }
            if (TraderPlusIDsConfig.isDirty)
            {
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
            }
            TraderPlusGarageConfig.FullFilename = TraderPlusGarageConfigPath;
            SetupTraderPlusGarageConfig();

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
            }
            TraderPlusIDsConfig.FullFilename = TraderPlusIDsConfigPath;
            SetupTraderPlusIDsConfig();

            TraderPlusGeneralConfigPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\TraderPlus\\TraderPlusConfig\\TraderPlusGeneralConfig.json";
            if (!File.Exists(TraderPlusGeneralConfigPath))
            {
                TraderPlusGeneralConfig = new TraderPlusGeneralConfig();
                needtosave = true;
            }
            else
            {
                TraderPlusGeneralConfig = JsonSerializer.Deserialize<TraderPlusGeneralConfig>(File.ReadAllText(TraderPlusGeneralConfigPath));
                TraderPlusGeneralConfig.isDirty = false;
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
                TraderPlusVehiclesConfig.isDirty = false;
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
        }
 
        #region BankingConfig
        private void SetupTraderPlusBankingConfig()
        {
            useraction = false;
            IsCreditCarNeededForTransactionCB.Checked = TraderPlusBankingConfig.IsCreditCarNeededForTransaction == 1 ? true : false;
            TransactionFeesNUD.Value = (decimal)TraderPlusBankingConfig.TransactionFees;
            DefaultStartCurrencyNUD.Value = (int)TraderPlusBankingConfig.DefaultStartCurrency;
            DefaultMaxCurrencyNUD.Value = (int)TraderPlusBankingConfig.DefaultMaxCurrency;

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
        #endregion Bankingconfig

        #region GarageConfig
        public Npc CurrentgarageNPC { get; set; }
        public MapData MapData { get; private set; }

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
        private void VehicleMustHaveLockCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGarageConfig.VehicleMustHaveLock = VehicleMustHaveLockCB.Checked == true ? 1 : 0;
            TraderPlusGarageConfig.isDirty = true;
        }
        private void SaveVehicleCargoCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGarageConfig.VehicleMustHaveLock = VehicleMustHaveLockCB.Checked == true ? 1 : 0;
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
                currentproject = currentproject,
                UseMultiple = false,
                isCategoryitem = true
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

            TraderPlusGeneralConfig.SortTradersByIndex();
            TraderPlusGeneralConfig.getBankers();
            TraderPlusGeneralConfig.SortCurriences();
            TraderPlusGeneralConfig.getallcurenciesclassnames();

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
            EnableShowAllCheckBoxCB.Checked = TraderPlusGeneralConfig.EnableShowAllCheckBox == 1 ? true : false;
            EnableStockAllCategoryCB.Checked = TraderPlusGeneralConfig.EnableStockAllCategory == 1 ? true : false;
            IsReceiptTraderOnlyCB.Checked = TraderPlusGeneralConfig.IsReceiptTraderOnly == 1 ? true : false;
            StoreOnlyToPristineStateCB.Checked = TraderPlusGeneralConfig.StoreOnlyToPristineState == 1 ? true : false;
            LockPickChanceNUD.Value = (decimal)TraderPlusGeneralConfig.LockPickChance;
            AcceptWornCB.Checked = TraderPlusGeneralConfig.AcceptedStates.AcceptWorn == 1 ? true : false;
            AcceptDamagedCB.Checked = TraderPlusGeneralConfig.AcceptedStates.AcceptDamaged == 1 ? true : false;
            AcceptBadlyDamagedCB.Checked = TraderPlusGeneralConfig.AcceptedStates.AcceptBadlyDamaged == 1 ? true : false;

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
            ObjectOrientationXNUD.Value = (decimal)currentTraderObject.Position[0];
            ObjectOrientationYNUD.Value = (decimal)currentTraderObject.Position[1];
            ObjectOrientationZNUD.Value = (decimal)currentTraderObject.Position[2];

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
                UseMultiple = false,
                isCategoryitem = true,
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
                Position = new float[] {0,0,0},
                Orientation = new float[] {0,0,0}
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
        private void EnableStockAllCategoryCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusGeneralConfig.EnableStockAllCategory = EnableStockAllCategoryCB.Checked == true ? 1 : 0;
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
            currentTraderObject.Position[0] = (float)ObjectOrientationXNUD.Value;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void ObjectOrientationYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentTraderObject.Position[1] = (float)ObjectOrientationYNUD.Value;
            TraderPlusGeneralConfig.isDirty = true;
        }
        private void ObjectOrientationZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentTraderObject.Position[2] = (float)ObjectOrientationZNUD.Value;
            TraderPlusGeneralConfig.isDirty = true;
        }
        #endregion general

        #region Traders
        public Id currenttraderID { get; set; }
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
                currenttraderID = TraderPlusIDsConfig.getTraderbyID(currenttrader.Id);
                EnableStockAllCategoryForIDCB.Checked = currenttraderID.EnableStockAllCategoryForID == 1 ? true : false;

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
            if(!currenttrader.isBanker)
            {
                TraderPlusIDsConfig.IDs.RemoveAt(currenttrader.Id);
                TraderPlusIDsConfig.setupIndex();
                TraderPlusIDsConfig.isDirty = true;
            }
            TraderPlusGeneralConfig.Traders.Remove(currenttrader);
            TraderPlusGeneralConfig.UpdateIndexes();
            TraderPlusGeneralConfig.isDirty = true;

            TraderPlusTradersLB.SelectedIndex = -1;
            if (TraderPlusTradersLB.Items.Count > 0)
                TraderPlusTradersLB.SelectedIndex = 0;
        }
        private void IsBankerCB_CheckedChanged(object sender, EventArgs e)
        {
            if(IsBankerCB.Checked)
            {
                TraderInfoGroupBox.Visible = false;
                if (!useraction) return;
                TraderPlusIDsConfig.IDs.Remove(currenttraderID);
                TraderPlusIDsConfig.setupIndex();
                TraderPlusIDsConfig.isDirty = true;
                currenttrader.Id = -2; ;
                currenttrader.isBanker = true;
                TraderPlusGeneralConfig.UpdateIndexes();
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
                TraderPlusGeneralConfig.isDirty = true;
                Id newID = new Id()
                {
                    EnableStockAllCategoryForID = 1,
                    Categories = new BindingList<string>(),
                    LicencesRequired = new BindingList<string>(),
                    CurrenciesAccepted = new BindingList<string>()
                };
                TraderPlusIDsConfig.IDs.Add(newID);
                TraderPlusIDsConfig.setupIndex();
                TraderPlusIDsConfig.isDirty = true;
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
        private void SetupTraderPlusIDsConfig()
        {
            useraction = false;
            TraderPlusIDsConfig.setupIndex();

            useraction = true;
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
            currenttraderID.LicencesRequired.Remove(LicensesRequiredLB.GetItemText(LicensesRequiredLB.SelectedItem));
            TraderPlusIDsConfig.isDirty = true;
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
            currenttraderID.CurrenciesAccepted.Remove(CurrenciesAcceptedLB.GetItemText(CurrenciesAcceptedLB.SelectedItem));
            TraderPlusIDsConfig.isDirty = true;
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
            currenttraderID.Categories.Remove(removeitem);
            TraderPlusIDsConfig.isDirty = true;
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
                currentproject = currentproject,
                UseMultiple = false,
                LowerCase = false,
                isCategoryitem = false
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
                if (!currenttraderID.Categories.Contains(tcat.CategoryName))
                {
                    currenttraderID.Categories.Add(tcat.CategoryName);
                    TraderPlusIDsConfig.isDirty = true;
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
                if (!currenttraderID.LicencesRequired.Contains(license))
                    currenttraderID.LicencesRequired.Add(license);
            }
            TraderPlusIDsConfig.isDirty = true;
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
                if (!currenttraderID.CurrenciesAccepted.Contains(currency))
                    currenttraderID.CurrenciesAccepted.Add(currency);
            }
            TraderPlusIDsConfig.isDirty = true;
            TraderInfoGroupBox.Visible = true;
            AvailablecurrenciesGroupBox.Visible = false;
        }
        private void darkButton49_Click(object sender, EventArgs e)
        {
            TraderInfoGroupBox.Visible = true;
            AvailablecurrenciesGroupBox.Visible = false;
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
            CategoryNameTB.Text = currentTradercategory.CategoryName;

            CurrentTraderCatLB.DisplayMember = "Name";
            CurrentTraderCatLB.ValueMember = "Value";
            CurrentTraderCatLB.DataSource = currentTradercategory.itemProducts;

        }
        private void CurrentTraderCatLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentTraderCatLB.SelectedItems.Count < 1) return;
            currentItemProducts = CurrentTraderCatLB.SelectedItem as ItemProducts;

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
                SellpriceNUD.Value = (int)currentItemProducts.Sellprice;
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
            TraderPlusPriceConfig.EnableAutoDestockAtRestart = EnableAutoDestockAtRestartCB.Checked == true ? 1:0;
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
                foreach (Id id in TraderPlusIDsConfig.IDs)
                {
                    bool remove = false;
                    foreach(string cat in id.Categories)
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
                    if (!UsedTypes.ContainsKey(item.Classname))
                        UsedTypes.Add(item.Classname.ToLower(), true);
                }
            }
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseMultiple = false,
                isCategoryitem = true,
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
                        Classname = l.ToLower(),
                        Coefficient = 0,
                        MaxStock = 0,
                        TradeQuantity = 0,
                        BuyPrice = 0,
                        Sellprice = 0,
                        destockCoefficent = 50,
                        UseDestockCoeff = true
                    };
                    currentTradercategory.itemProducts.Add(NewContainer);
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
                    if (!Checkifincat(NewContainer))
                    {
                        currentTradercategory.itemProducts.Add(NewContainer);
                        TraderPlusPriceConfig.isDirty = true;
                    }
                    else
                    {
                        MessageBox.Show(NewContainer.Classname + " Allready exists.....");
                    }
                }
            }
        }
        private bool Checkifincat(ItemProducts item)
        {
            if(currentTradercategory.itemProducts.Any(x => x.Classname == item.Classname))
            {
                return true;
            }
            return false;
        }
        private void darkButton13_Click(object sender, EventArgs e)
        {
            if (currentItemProducts == null) return;
            currentTradercategory.itemProducts.Remove(currentItemProducts);
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
            currentItemProducts.TradeQuantity = (int)TradeQuantityNUD.Value;
            TraderPlusPriceConfig.isDirty = true;
            if (CurrentTraderCatLB.SelectedItems.Count > 1)
            {
                foreach (var item in CurrentTraderCatLB.SelectedItems)
                {
                    ItemProducts pitem = item as ItemProducts;
                    pitem.TradeQuantity = (int)TradeQuantityNUD.Value;
                }
            }
        }
        private void CantBuyCB_CheckedChanged(object sender, EventArgs e)
        {
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
            currentItemProducts.BuyPrice = (int)BuyPriceNUD.Value;
            TraderPlusPriceConfig.isDirty = true;
            if (CurrentTraderCatLB.SelectedItems.Count > 1)
            {
                foreach (var item in CurrentTraderCatLB.SelectedItems)
                {
                    ItemProducts pitem = item as ItemProducts;
                    pitem.BuyPrice = (int)BuyPriceNUD.Value;
                }
            }
        }
        private void CantSellCB_CheckedChanged(object sender, EventArgs e)
        {
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
            currentItemProducts.Sellprice = (int)SellpriceNUD.Value;
            TraderPlusPriceConfig.isDirty = true;
            if (CurrentTraderCatLB.SelectedItems.Count > 1)
            {
                foreach (var item in CurrentTraderCatLB.SelectedItems)
                {
                    ItemProducts pitem = item as ItemProducts;
                    pitem.Sellprice = (int)SellpriceNUD.Value;
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
            if(UsedestockCoeffCB.Checked)
            {
                DestockCoefflabel.Visible = true;
                DestockCoeffTB.Visible = true;
                currentItemProducts.UseDestockCoeff = true;
                TraderPlusPriceConfig.isDirty = true;
            }
            else
            {
                DestockCoefflabel.Visible = false;
                DestockCoeffTB.Visible = false;
                currentItemProducts.UseDestockCoeff = false;
                TraderPlusPriceConfig.isDirty = true;
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
                decimal num1 = (decimal)item.BuyPrice / 100;
                decimal num2 = num1 * value;
                item.Sellprice = (int)Math.Round(num2, MidpointRounding.AwayFromZero);
            }
            TraderPlusPriceConfig.isDirty = true;
        }
        #endregion traderprice

        #region SafeZone
        public Safearealocation currentsafezone;
        public int ZoneScale = 1;

        private void SetupTraderPlusSafeZoneConfig()
        {
            useraction = false;
            EnableNameTagCB.Checked = TraderPlusSafeZoneConfig.EnableNameTag == 1 ? true : false;
            EnableAfkDisconnectCB.Checked = TraderPlusSafeZoneConfig.EnableAfkDisconnect == 1 ? true : false;
            KickAfterDelayNUD.Value = (int)TraderPlusSafeZoneConfig.KickAfterDelay;
            MsgEnterZoneTB.Text = TraderPlusSafeZoneConfig.MsgEnterZone;
            MsgExitZoneTB.Text = TraderPlusSafeZoneConfig.MsgExitZone;
            MsgOnLeavingZoneTB.Text = TraderPlusSafeZoneConfig.MsgOnLeavingZone;
            CleanUpTimerNUD.Value = (int)TraderPlusSafeZoneConfig.CleanUpTimer;

            ObjectsToDeleteLB.DisplayMember = "Name";
            ObjectsToDeleteLB.ValueMember = "Value";
            ObjectsToDeleteLB.DataSource = TraderPlusSafeZoneConfig.ObjectsToDelete;

            SZSteamUIDsLB.DisplayMember = "Name";
            SZSteamUIDsLB.ValueMember = "Value";
            SZSteamUIDsLB.DataSource = TraderPlusSafeZoneConfig.SZSteamUIDs;

            SafeAreaLocationLB.DisplayMember = "Name";
            SafeAreaLocationLB.ValueMember = "Value";
            SafeAreaLocationLB.DataSource = TraderPlusSafeZoneConfig.SafeAreaLocation;

            useraction = true;
        }
        private void EnableNameTagCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TraderPlusSafeZoneConfig.EnableNameTag = EnableNameTagCB.Checked == true ? 1 : 0;
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
                    TraderPlusVehiclesConfig.isDirty = true;
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
        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {
            if (e is MouseEventArgs mouseEventArgs)
            {
                Cursor.Current = Cursors.WaitCursor;
                float scalevalue = ZoneScale * 0.05f;
                float mapsize = currentproject.MapSize;
                int newsize = (int)(mapsize * scalevalue);
                SafeZoneXNUD.Value = (decimal)(mouseEventArgs.X / scalevalue);
                safeZoneZNUD.Value = (decimal)((newsize - mouseEventArgs.Y) / scalevalue);
                Cursor.Current = Cursors.Default;
                TraderPlusSafeZoneConfig.isDirty = true;
                pictureBox2.Invalidate();
            }
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

            VehicleNameTB.Text = currentVehiclespart.VehicleName;
            vehicleHeightNUD.Value = currentVehiclespart.Height;

            VehiclePartPartsLB.DisplayMember = "Name";
            VehiclePartPartsLB.ValueMember = "Value";
            VehiclePartPartsLB.DataSource = currentVehiclespart.VehicleParts;
        }
        private void darkButton20_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseMultiple = false,
                isCategoryitem = true
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
                        VehicleParts = new BindingList<string>()
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
            if (MessageBox.Show("This Will Remove The All reference to this Vehicle, Are you sure you want to do this?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                TraderPlusVehiclesConfig.VehiclesParts.Remove(currentVehiclespart);
                TraderPlusVehiclesConfig.isDirty = true;
                if (VehiclePartLB.Items.Count == 0)
                    VehiclePartLB.SelectedIndex = -1;
                else
                    VehiclePartLB.SelectedIndex = 0;
            }
        }
        private void darkButton19_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseMultiple = false,
                isCategoryitem = true
            };
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
            currentVehiclespart.Height = (int)vehicleHeightNUD.Value;
            TraderPlusVehiclesConfig.isDirty = true;
        }



        #endregion Vehicle Settings

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            foreach(Tradercategory tradcat in TraderPlusPriceConfig.TraderCategories)
            {

            }
        }
    }
}
