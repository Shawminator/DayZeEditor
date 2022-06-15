using Cyotek.Windows.Forms;
using DarkUI.Forms;
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
using DayZeLib;

namespace DayZeEditor
{
    public partial class ExpansionMarket : DarkForm
    {
        public Project currentproject { get; internal set; }
        public List<int> catids;
        public TraderZones Zones;
        public TradersList Traders;
        public MarketCategories MarketCats;
        public MarketSettings marketsettings;
        public TraderModelMapping tradermaps;
        public BindingList<Tradermap> NoZoneTraders;
        public string ZonesPath;
        public string TradersPath;
        public string CatPath;
        public string MarketSettingsPath;
        public string tradermapsPath;

        public TypesFile vanillatypes;
        public TypesFile Expansiontypes;
        public List<TypesFile> ModTypes;
        public string filename;

        public Categories treeviewcat;
        public marketItem treeviewitem;
        public MytreeNode currenttreenodeparent;
        public MytreeNode currentnode;
        public bool pcontroll = false;

        public marketItem currentitem;
        private ContextMenuStrip contexMenu;

        public Categories currentCat;
        public bool action = false;

        public string Typeofcat;
        public Zones currentZone;

        public Traders currentTrader;

        public MapData data;

        #region Form Load and populate plus other general Functions
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
        public ExpansionMarket()
        {
            InitializeComponent();
            tabControl1.ItemSize = new Size(0, 1);
            tabControl3.ItemSize = new Size(0, 1);
        }
        private void ExpansionMarket_Load(object sender, EventArgs e)
        {
            bool needtosave = false;
            filename = currentproject.ProjectName;
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            catids = new List<int>();
            ZonesPath = currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\traderzones";
            TradersPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Traders";
            CatPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\Market";
            MarketSettingsPath = currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\settings\\MarketSettings.json";
            tradermapsPath = currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\traders";
            numericUpDown1.Maximum = currentproject.MapSize;
            numericUpDown3.Maximum = currentproject.MapSize;
            if (!Directory.Exists(Path.GetDirectoryName(MarketSettingsPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(MarketSettingsPath));
            if (!File.Exists(MarketSettingsPath))
            {
                marketsettings = new MarketSettings(MarketSettingsPath);
                needtosave = true;
            }
            else
            {
                marketsettings = JsonSerializer.Deserialize<MarketSettings>(File.ReadAllText(MarketSettingsPath));
                marketsettings.isDirty = false;
                if (marketsettings.m_Version != 9)
                {
                    MessageBox.Show("MarketSettings Version number not up to date, updating to latest version....");
                    marketsettings.m_Version = 9;
                    marketsettings.isDirty = true;
                    needtosave = true;
                }
                Console.WriteLine("Serializing " + MarketSettingsPath);
            }
            marketsettings.Filename = MarketSettingsPath;
            marketsettings.setspawnnames();

            MarketCats = new MarketCategories(CatPath);
            Traders = new TradersList(TradersPath, MarketCats);
            Zones = new TraderZones(ZonesPath);
            tradermaps = new TraderModelMapping(tradermapsPath);
            foreach (Zones zone in Zones.ZoneList)
            {
                foreach (Tradermap tm in tradermaps.maps)
                {
                    PointF pC = new PointF(zone.Position[0], zone.Position[2]);
                    PointF pP = new PointF(tm.position.X, tm.position.Z);
                    if (IsWithinCircle(pC, pP, zone.Radius))
                    {
                        tm.Filename = Path.GetDirectoryName(tm.Filename) + "\\" + zone.m_ZoneName + ".map";
                        tm.IsInAZone = true;
                    }

                }
            }
            NoZoneTraders = new BindingList<Tradermap>();
            foreach(Tradermap tm in tradermaps.maps)
            {
                if (!tm.IsInAZone)
                    NoZoneTraders.Add(tm);
            }



            Console.WriteLine("Setting Market Setting Variables");
            Loadsettings();

            SetupListBoxes();
            dataGridView1.Invalidate();

            data = new MapData(Application.StartupPath + currentproject.MapPath + ".xyz");

            pictureBox1.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Chernarus Map Size is 15360 x 15360, 0,0 bottom left, center 7680 x 7680
            pictureBox1.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox1.Paint += new PaintEventHandler(DrawAll);
            trackBar2.Value = 1;
            SetScale();

            pictureBox2.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Livonia maop size is 12800 x 12800, 0,0 bottom left, center 6400 x 6400
            pictureBox2.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox2.Paint += new PaintEventHandler(DrawAllSpawns);
            trackBar4.Value = 1;
            SetSpawnscale();

            if(needtosave)
            {
                saveMarketfiles();
            }
        }
        private void SetupListBoxes()
        {
            listBox1.DisplayMember = "Name";
            listBox1.ValueMember = "Value";
            listBox1.DataSource = Zones.ZoneList;

            listBox13.DisplayMember = "Name";
            listBox13.ValueMember = "Value";
            listBox13.DataSource = Zones.ZoneList;

            listBox2.DisplayMember = "Name";
            listBox2.ValueMember = "Value";
            listBox2.DataSource = Traders.Traderlist;

            listBox16.DisplayMember = "Name";
            listBox16.ValueMember = "Value";
            listBox16.DataSource = Traders.Traderlist;

            listBox5.DisplayMember = "Name";
            listBox5.ValueMember = "Value";
            listBox5.DataSource = MarketCats.CatList;

            listBox18.DisplayMember = "Name";
            listBox18.ValueMember = "Value";
            listBox18.DataSource = NoZoneTraders;
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddFromCategoryListBox.Visible = false;
            dataGridView1.Update();
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    toolStripButton3.Checked = false;
                    toolStripButton4.Checked = false;
                    toolStripButton5.Checked = false;
                    toolStripButton6.Checked = false;
                    break;
                case 1:
                    toolStripButton1.Checked = false;
                    toolStripButton4.Checked = false;
                    toolStripButton5.Checked = false;
                    toolStripButton6.Checked = false;
                    break;
                case 2:
                    toolStripButton1.Checked = false;
                    toolStripButton3.Checked = false;
                    toolStripButton5.Checked = false;
                    toolStripButton6.Checked = false;
                    break;
                case 3:
                    toolStripButton1.Checked = false;
                    toolStripButton3.Checked = false;
                    toolStripButton4.Checked = false;
                    toolStripButton6.Checked = false;
                    break;
                case 4:
                    toolStripButton1.Checked = false;
                    toolStripButton3.Checked = false;
                    toolStripButton4.Checked = false;
                    toolStripButton5.Checked = false;
                    break;
                default:
                    break;
            }
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    Process.Start(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\settings");
                    break;
                case 1:
                    Process.Start(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\traders");
                    break;
                case 2:
                    Process.Start(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\expansion\\traderzones");
                    break;
                case 3:
                    Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\expansionMod\\Traders");
                    break;
                case 4:
                    Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\expansionMod\\Market");
                    break;
            }
        }
        private void syncMinToMaxPricesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Categories cats in MarketCats.CatList)
            {
                foreach (marketItem item in cats.Items)
                {
                    item.MinPriceThreshold = item.MaxPriceThreshold;

                }
                cats.isDirty = true;
            }
            //setzoneprices();
        }
        private void setMinMaxStockForItemsWith0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Categories cats in MarketCats.CatList)
            {
                foreach (marketItem item in cats.Items)
                {
                    if (item.MaxStockThreshold == 0 || item.MinStockThreshold == 0)
                    {
                        item.MaxStockThreshold = 100;
                        item.MinStockThreshold = 1;
                    }

                }
                cats.isDirty = true;
            }
            //setzoneprices();
        }
        private void syncMaxToMinPricesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Categories cats in MarketCats.CatList)
            {
                foreach (marketItem item in cats.Items)
                {
                    item.MaxPriceThreshold = item.MinPriceThreshold;

                }
                cats.isDirty = true;
            }
           // setzoneprices();
        }
        private void setPricesForItemWithZeroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Set your Max Price... ", "max Price", "");
            if (UserAnswer == "") return;
            int value = Convert.ToInt32(UserAnswer);
            foreach (Categories cats in MarketCats.CatList)
            {
                foreach (marketItem item in cats.Items)
                {
                    if (item.MaxPriceThreshold == 0)
                        item.MaxPriceThreshold = value;
                    if (item.MinPriceThreshold == 0)
                        item.MinPriceThreshold = (int)((float)value * 0.5f);

                }
                cats.isDirty = true;
            }
            //setzoneprices();
        }
        private void setMAxStockForAllItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Set your max Stock... ", "Max Stock", "");
            if (UserAnswer == "") return;
            int value = Convert.ToInt32(UserAnswer);
            foreach (Categories cats in MarketCats.CatList)
            {
                foreach (marketItem item in cats.Items)
                {
                    item.MaxStockThreshold = value; ;

                }
                cats.isDirty = true;
            }
            //setzoneprices();
            dataGridView1.Invalidate();
        }
        private void setMinStockForAllItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Set your Min Stock... ", "Min Stock", "");
            if (UserAnswer == "") return;
            int value = Convert.ToInt32(UserAnswer);
            foreach (Categories cats in MarketCats.CatList)
            {
                foreach (marketItem item in cats.Items)
                {
                    item.MinStockThreshold = value;
                }
                cats.isDirty = true;
            }
            //setzoneprices();
        }
        private void setMaxStockForSelectedCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Set your max Stock... ", "Max Stock", "");
            if (UserAnswer == "") return;
            int value = Convert.ToInt32(UserAnswer);
            foreach (marketItem item in currentCat.Items)
            {
                item.MaxStockThreshold = value; ;

            }
            currentCat.isDirty = true;
            //setzoneprices();
        }
        private void setMinStockForSelectedCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Set your Min Stock... ", "Min Stock", "");
            if (UserAnswer == "") return;
            int value = Convert.ToInt32(UserAnswer);
            foreach (marketItem item in currentCat.Items)
            {
                item.MinStockThreshold = value;

            }
            currentCat.isDirty = true;
            //setzoneprices();
        }
        private void setMaxPriceForSelectedCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Set your Max Price... ", "max Price", "");
            if (UserAnswer == "") return;
            int value = Convert.ToInt32(UserAnswer);
            foreach (marketItem item in currentCat.Items)
            {
                item.MaxPriceThreshold = value;

            }
            currentCat.isDirty = true;
            //setzoneprices();
        }
        private void exportItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox5.SelectedItems.Count > 0)
            {
                foreach (var item in listBox5.SelectedItems)
                {
                    StringBuilder sb = new StringBuilder();
                    Categories pitem = item as Categories;
                    foreach (marketItem mitem in pitem.Items)
                    {
                        sb.Append(mitem.ClassName + "," + mitem.MaxPriceThreshold.ToString() + Environment.NewLine);
                        //if(item.Variants != null && item.Variants.Count > 0)
                        //{
                        //    foreach(string itemv in item.Variants)
                        //    {
                        //        sb.Append(itemv + "," + item.MaxPriceThreshold.ToString() + Environment.NewLine);
                        //    }
                        //}
                    }
                    File.WriteAllText(pitem.DisplayName + ".txt", sb.ToString());
                }
            }
        }
        private void setPriceForItemsWithZeroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Set your Max Price... ", "max Price", "");
            if (UserAnswer == "") return;
            int value = Convert.ToInt32(UserAnswer);
            foreach (marketItem item in currentCat.Items)
            {
                if (item.MaxPriceThreshold == 0)
                    item.MaxPriceThreshold = value;
                if (item.MinPriceThreshold == 0)
                    item.MinPriceThreshold = (int)((float)value * 0.5f);

            }
            currentCat.isDirty = true;
            //setzoneprices();

        }
        private void setMinPriceForSelectedCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Set your Min Price... ", "Min Price", "");
            if (UserAnswer == "") return;
            int value = Convert.ToInt32(UserAnswer);
            foreach (marketItem item in currentCat.Items)
            {
                item.MinPriceThreshold = value;

            }
            currentCat.isDirty = true;
            //setzoneprices();
        }
        private void setMinPricePercentageOfMaxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Input precentage of Max Price ", "Min Price", "");
            if (UserAnswer == "") return;
            int value = Convert.ToInt32(UserAnswer);
            foreach (marketItem item in currentCat.Items)
            {
                decimal num1 = (decimal)item.MaxPriceThreshold / 100;
                decimal num2 = num1 * value;
                item.MinPriceThreshold = (int)Math.Round(num2, MidpointRounding.AwayFromZero);
            }
            currentCat.isDirty = true;
            //setzoneprices();
        }
        private void setMaxPriceToPercentageOfMinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("input perecentage min shoudl be to max ", "Min Price", "");
            if (UserAnswer == "") return;
            int value = Convert.ToInt32(UserAnswer);
            foreach (marketItem item in currentCat.Items)
            {
                decimal num1 = (decimal)item.MinPriceThreshold / value;
                decimal num2 = num1 * 100; ;
                item.MaxPriceThreshold = (int)Math.Round(num2, MidpointRounding.AwayFromZero);
            }
            currentCat.isDirty = true;
            //setzoneprices();
        }
        private void getPriceFromListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.InitialDirectory = Application.StartupPath;
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                List<string> list = File.ReadAllLines(openfile.FileName).ToList();
                foreach (marketItem item in currentCat.Items)
                {
                    if (list.Any(x => x.Split(',')[0] == item.ClassName))
                    {
                        string price = list.First(x => x.Split(',')[0] == item.ClassName);
                        item.MaxPriceThreshold = Convert.ToInt32(price.Split(',')[1]);
                    }
                }
                currentCat.isDirty = true;
            }
        }
        private void createNewMarketToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //backup existing folders if there and existing market settings.
            BackupMarketFiles();
            marketsettings = new MarketSettings(MarketSettingsPath);
            MarketCats = new MarketCategories(CatPath);
            Traders = new TradersList(TradersPath, MarketCats);
            Zones = new TraderZones(ZonesPath);
            tradermaps = new TraderModelMapping(tradermapsPath);
            Loadsettings();
            SetupListBoxes();
            this.Controls.ClearControls();
        }
        private void BackupMarketFiles()
        {
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            string backupdir = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\ExpansionMod\\MarketBackup\\" + SaveTime;
            Directory.CreateDirectory(backupdir);
            if (File.Exists(MarketSettingsPath))
                File.Move(MarketSettingsPath, backupdir + "\\" + Path.GetFileName(MarketSettingsPath));
            if (Directory.Exists(ZonesPath))
                Directory.Move(ZonesPath, backupdir + "\\TraderZones");
            if (Directory.Exists(TradersPath))
                Directory.Move(TradersPath, backupdir + "\\Traders");
            if (Directory.Exists(CatPath))
                Directory.Move(CatPath, backupdir + "\\Market");
            if (Directory.Exists(tradermapsPath))
                Directory.Move(tradermapsPath, backupdir + "\\TraderMaps");
        }
        #endregion Form Load and populate plus other general Functions

        #region Form Save function for all documents
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            saveMarketfiles();
        }

        private void saveMarketfiles()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (marketsettings.isDirty)
            {
                marketsettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(marketsettings, options);
                if (currentproject.Createbackups && File.Exists(marketsettings.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(MarketSettingsPath) + "\\Backup\\" + SaveTime);
                    File.Copy(marketsettings.Filename, Path.GetDirectoryName(MarketSettingsPath) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(marketsettings.Filename) + ".bak", true);
                }
                File.WriteAllText(marketsettings.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(marketsettings.Filename));
            }
            if (tradermaps.isDirty)
            {
                tradermaps.isDirty = false;
                tradermaps.savefiles(SaveTime);
                midifiedfiles.Add("Al trader maps Saved.....");
            }
            foreach (Zones zones in Zones.ZoneList)
            {
                if (!zones.isDirty) continue;
                zones.isDirty = false;
                zones.ConvertlisttoDict();
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(zones, options);
                if (currentproject.Createbackups && File.Exists(zones.Filename))
                {
                    Directory.CreateDirectory(ZonesPath + "\\Backup\\" + SaveTime);
                    File.Copy(zones.Filename, ZonesPath + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(zones.Filename) + ".bak", true);
                }
                File.WriteAllText(zones.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(zones.Filename));

            }
            foreach (Traders trader in Traders.Traderlist)
            {
                if (!trader.isDirty) continue;
                trader.isDirty = false;
                trader.ConvertToDict(MarketCats);
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(trader, options);
                if (currentproject.Createbackups && File.Exists(trader.Filename))
                {
                    Directory.CreateDirectory(TradersPath + "\\Backup\\" + SaveTime);
                    File.Copy(trader.Filename, TradersPath + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(trader.Filename) + ".bak", true);
                }
                File.WriteAllText(trader.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(trader.Filename));
            }
            foreach (Categories cat in MarketCats.CatList)
            {
                if (!cat.isDirty) continue;
                cat.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(cat, options);
                if (currentproject.Createbackups && File.Exists(cat.Filename))
                {
                    Directory.CreateDirectory(CatPath + "\\Backup\\" + SaveTime);
                    File.Copy(cat.Filename, CatPath + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(cat.Filename) + ".bak", true);
                }
                File.WriteAllText(cat.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(cat.Filename));
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
        #endregion Form Save function for all documents

        #region MarketSettings
        public int scale2 = 1;
        private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl3.SelectedIndex)
            {
                case 0:
                    toolStripButton7.Checked = false;
                    break;
                case 1:
                    toolStripButton8.Checked = false;
                    break;
                default:
                    break;
            }
        }
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            tabControl3.SelectedIndex = 0;
            if (tabControl3.SelectedIndex == 0)
                toolStripButton8.Checked = true;
        }
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            tabControl3.SelectedIndex = 1;
            if (tabControl3.SelectedIndex == 1)
                toolStripButton7.Checked = true;
        }
        private void Loadsettings()
        {
            action = true;
            MarketSytemeEnabedcheckBox.Checked = marketsettings.MarketSystemEnabled == 1 ? true : false;
            ATMSytemeEnabledcheckBox.Checked = marketsettings.ATMSystemEnabled == 1 ? true : false;
            MaxdepositUpDown.Value = marketsettings.MaxDepositMoney;
            DefaultDepositUpDown.Value = marketsettings.DefaultDepositMoney;
            ATMPlayerTransferEnabledcheckbox.Checked = marketsettings.ATMPlayerTransferEnabled == 1 ? true : false;
            ATMPartyLockerEnabledCheckBox.Checked = marketsettings.ATMPartyLockerEnabled == 1 ? true : false;
            MaxPartyDepositMoneyUpDown.Value = marketsettings.MaxPartyDepositMoney;
            trackBar1.Value = (int)marketsettings.SellPricePercent;
            UseWholeMapForATMPlayerListCheckBox.Checked = marketsettings.UseWholeMapForATMPlayerList == 1 ? true : false;
            NetworkBatchSizeNUD.Value = marketsettings.NetworkBatchSize;
            MaxVehicleDistanceToTraderNUD.Value = (decimal)marketsettings.MaxVehicleDistanceToTrader;
            MaxLargeVehicleDistanceToTraderNUD.Value = (decimal)marketsettings.MaxLargeVehicleDistanceToTrader;

            listBox7.DisplayMember = "Name";
            listBox7.ValueMember = "Value";
            listBox7.DataSource = marketsettings.LandSpawnPositions;

            listBox8.DisplayMember = "Name";
            listBox8.ValueMember = "Value";
            listBox8.DataSource = marketsettings.AirSpawnPositions;

            listBox9.DisplayMember = "Name";
            listBox9.ValueMember = "Value";
            listBox9.DataSource = marketsettings.WaterSpawnPositions;

           
            listBox17.DisplayMember = "DisplayName";
            listBox17.ValueMember = "Value";
            listBox17.DataSource = marketsettings.LargeVehicles;

            listBox12.DisplayMember = "DisplayName";
            listBox12.ValueMember = "Value";
            listBox12.DataSource = marketsettings.Currencies;

            Categories cat = MarketCats.GetCatFromFileName("EXCHANGE");
            if (cat != null)
            {
                listBox19.Items.Clear();
                List<string> items = new List<string>();
                foreach (marketItem item in cat.Items)
                {
                    foreach (string vitem in item.Variants)
                    {
                        if (!items.Contains(vitem))
                            items.Add(vitem);
                    }
                    if (!items.Contains(item.ClassName))
                        items.Add(item.ClassName);

                }
                items.Sort();
                listBox19.Items.AddRange(items.ToArray());
            }
            action = false;
        }
        private void BackGround_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            Rectangle region;
            region = pb.ClientRectangle;
            string col = marketsettings.getcolourfromcontrol(pb.Name);
            string col1 = "#" + col.Substring(6) + col.Remove(6, 2);
            Color colour = ColorTranslator.FromHtml(col1);
            using (Brush brush = new SolidBrush(colour))
            {
                e.Graphics.FillRectangle(brush, region);
            }
            e.Graphics.DrawRectangle(SystemPens.ControlText, region.Left, region.Top, region.Width - 1, region.Height - 1);
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pcontroll = true;
            if (listBox1.SelectedItems.Count < 1) return;
            currentZone = listBox1.SelectedItem as Zones;
            textBox1.Text = currentZone.m_Version.ToString();
            textBox3.Text = currentZone.m_ZoneName;
            textBox4.Text = currentZone.m_DisplayName;
            numericUpDown1.Value = (int)currentZone.Position[0];
            numericUpDown2.Value = (int)currentZone.Position[1];
            numericUpDown3.Value = (int)currentZone.Position[2];
            numericUpDown4.Value = (int)currentZone.Radius;
            numericUpDown5.Value = (decimal)currentZone.BuyPricePercent;
            numericUpDown13.Value = (decimal)currentZone.SellPricePercent;
            BindingSource source = new BindingSource(currentZone.StockItems, null);
            dataGridView1.DataSource = source;
            dataGridView1.Columns[0].ReadOnly = true;
            //dataGridView1.Columns[2].ReadOnly = true;
           // dataGridView1.Columns[3].ReadOnly = true;
            //dataGridView1.Columns[4].ReadOnly = true;

            darkLabel56.Text = "Zone Stock Count:- " + currentZone.StockItems.Count().ToString();
            pictureBox1.Invalidate();
            pcontroll = false;
        }
        private void BackgroundButton_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            ColorPickerDialog cpick = new ColorPickerDialog
            {
                StartPosition = FormStartPosition.CenterParent
            };
            string col = marketsettings.getcolourfromcontrol(pb.Name);
            string col1 = "#" + col.Substring(6) + col.Remove(6, 2);
            cpick.Color = ColorTranslator.FromHtml(col1);
            if (cpick.ShowDialog() == DialogResult.OK)
            {
                
                marketsettings.setcolour(pb.Name, cpick.Color.Name.ToUpper());
                pb.Invalidate();
                marketsettings.isDirty = true;
            }
        }
        private void BackgroundColour_MouseHover(object sender, EventArgs e)
        {
            if(sender is PictureBox)
            {
                PictureBox pb = sender as PictureBox;
                ttpShow = new ToolTip
                {
                    AutoPopDelay = 2000,
                    InitialDelay = 1000,
                    ReshowDelay = 500,
                    IsBalloon = true
                };
                ttpShow.SetToolTip(pb, marketsettings.getcolourfromcontrol(pb.Name));
                ttpShow.Show("0x" + marketsettings.getcolourfromcontrol(pb.Name), pb, pb.Width, pb.Height / 10, 5000);
            }
        }
        private void ColorIncreaseQuantityIconColour_MouseLeave(object sender, EventArgs e)
        {
            ttpShow.Dispose();
        }
        private void SpawnPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            action = true;
            SetPositions();
            pictureBox2.Invalidate();
            action = false;
        }
        public void Setvalues(SpawnPositions pos)
        {
            if (pos == null) return;
            action = true;
            numericUpDown10.Value = (decimal)pos.Position[0];
            numericUpDown11.Value = (decimal)pos.Position[1];
            numericUpDown12.Value = (decimal)pos.Position[2];
            numericUpDown20.Value = (decimal)pos.Orientation[0];
            numericUpDown21.Value = (decimal)pos.Orientation[1];
            numericUpDown22.Value = (decimal)pos.Orientation[2];
            action = false;
        }
        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            action = true;
            SetPositions();
            pictureBox2.Invalidate();
            action = false;
        }
        private void SetPositions()
        {
            switch (tabControl2.SelectedIndex)
            {
                case 0:
                    int index = listBox7.SelectedIndex;
                    SpawnPositions sp = marketsettings.getSpawnbyindex(0, index);
                    Setvalues(sp);
                    break;
                case 1:
                    index = listBox8.SelectedIndex;
                    sp = marketsettings.getSpawnbyindex(1, index);
                    Setvalues(sp);
                    break;
                case 2:
                    index = listBox9.SelectedIndex;
                    sp = marketsettings.getSpawnbyindex(2, index);
                    Setvalues(sp);
                    break;
            }
        }
        private void UseWholeMapForATMPlayerListCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (action) return;
            marketsettings.UseWholeMapForATMPlayerList = UseWholeMapForATMPlayerListCheckBox.Checked == true ? 1 : 0;
            marketsettings.isDirty = true;
        }
        private void darkButton31_Click(object sender, EventArgs e)
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
                    if (!marketsettings.LargeVehicles.Contains(l.ToLower()))
                    {
                        marketsettings.LargeVehicles.Add(l.ToLower());
                        marketsettings.isDirty = true;
                    }
                }
            }
        }
        private void darkButton30_Click(object sender, EventArgs e)
        {
            marketsettings.LargeVehicles.Remove(listBox17.GetItemText(listBox17.SelectedItem));
            marketsettings.isDirty = true;
        }
        private void MaxLargeVehicleDistanceToTraderNUD_ValueChanged(object sender, EventArgs e)
        {
            if (action) return;
            marketsettings.MaxLargeVehicleDistanceToTrader = (int)MaxLargeVehicleDistanceToTraderNUD.Value;
            marketsettings.isDirty = true;
        }
        private void MaxVehicleDistanceToTraderNUD_ValueChanged(object sender, EventArgs e)
        {
            if (action) return;
            marketsettings.MaxLargeVehicleDistanceToTrader = (int)MaxVehicleDistanceToTraderNUD.Value;
            marketsettings.isDirty = true;
        }
        private void NetworkBatchSizeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (action) return;
            marketsettings.NetworkBatchSize = (int)NetworkBatchSizeNUD.Value;
            marketsettings.isDirty = true;
        }
        private void MarketSytemeEnabedcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (action) return;
            marketsettings.MarketSystemEnabled = MarketSytemeEnabedcheckBox.Checked == true ? 1 : 0;
            marketsettings.isDirty = true;
        }
        private void ATMSytemeEnabledcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (action) return;
            marketsettings.ATMSystemEnabled = ATMSytemeEnabledcheckBox.Checked == true ? 1 : 0;
            marketsettings.isDirty = true;
        }
        private void MaxdepositUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (action) return;
            marketsettings.MaxDepositMoney = (int)MaxdepositUpDown.Value;
            marketsettings.isDirty = true;
        }
        private void DefaultDepositUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (action) return;
            marketsettings.DefaultDepositMoney = (int)DefaultDepositUpDown.Value;
            marketsettings.isDirty = true;
        }
        private void ATMPlayerTransferEnabledcheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (action) return;
            marketsettings.ATMPlayerTransferEnabled = ATMPlayerTransferEnabledcheckbox.Checked == true ? 1 : 0;
            marketsettings.isDirty = true;
        }
        private void ATMPartyLockerEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (action) return;
            marketsettings.ATMPartyLockerEnabled = ATMPartyLockerEnabledCheckBox.Checked == true ? 1 : 0;
            marketsettings.isDirty = true;
        }
        private void MaxPartyDepositMoneyUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (action) return;
            marketsettings.MaxPartyDepositMoney = (int)MaxPartyDepositMoneyUpDown.Value;
            marketsettings.isDirty = true;
        }
        private void darkButton19_Click(object sender, EventArgs e)
        {
            switch (tabControl2.SelectedIndex)
            {
                case 0:
                    Cursor.Current = Cursors.WaitCursor;
                    marketsettings.AddnewSpawn(0);
                    listBox7.SelectedIndex = listBox7.Items.Count - 1;
                    SpawnPositions sp = marketsettings.getSpawnbyindex(0, listBox7.Items.Count - 1);
                    sp.Position[0] = currentproject.MapSize / 2;
                    sp.Position[2] = currentproject.MapSize / 2;
                    sp.Position[1] = data.gethieght(sp.Position[0], sp.Position[2]);
                    marketsettings.isDirty = true;
                    Cursor.Current = Cursors.Default;
                    break;
                case 1:
                    marketsettings.AddnewSpawn(1);
                    marketsettings.isDirty = true;
                    listBox8.SelectedIndex = listBox8.Items.Count - 1;

                    break;
                case 2:
                    marketsettings.AddnewSpawn(2);
                    marketsettings.isDirty = true;
                    listBox9.SelectedIndex = listBox9.Items.Count - 1;
                    break;
            }
            SetPositions();
        }
        private void darkButton18_Click(object sender, EventArgs e)
        {
            switch (tabControl2.SelectedIndex)
            {
                case 0:
                    action = true;
                    int index = listBox7.SelectedIndex;
                    marketsettings.RemoveSpawn(0, index);
                    marketsettings.isDirty = true;
                    listBox7.SelectedIndex = -1;
                    if (listBox7.Items.Count > 0)
                        listBox7.SelectedIndex = 0;
                    action = false;
                    break;
                case 1:
                    action = true;
                    index = listBox8.SelectedIndex;
                    marketsettings.RemoveSpawn(1, index);
                    marketsettings.isDirty = true;
                    listBox8.SelectedIndex = -1;
                    if (listBox8.Items.Count > 0)
                        listBox8.SelectedIndex = 0;
                    action = false;
                    break;
                case 2:
                    action = true;
                    index = listBox9.SelectedIndex;
                    marketsettings.RemoveSpawn(2, index);
                    marketsettings.isDirty = true;
                    listBox9.SelectedIndex = -1;
                    if (listBox9.Items.Count > 0)
                        listBox9.SelectedIndex = 0;
                    action = false;
                    break;
            }
        }
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            darkLabel13.Text = trackBar1.Value.ToString() + "%";
            if (action) return;
            marketsettings.SellPricePercent = (int)trackBar1.Value;
            marketsettings.isDirty = true;
            //setzoneprices();
        }
        private void DrawAllSpawns(object sender, PaintEventArgs e)
        {
            float scalevalue = scale2 * 0.05f;
            switch (tabControl2.SelectedIndex)
            {
                case 0:
                    int Selectedindex = listBox7.SelectedIndex;
                    if (Selectedindex == -1) break;
                    for (int i = 0; i < listBox7.Items.Count; i++)
                    {
                        if (i == Selectedindex) continue;
                        SpawnPositions sp = marketsettings.getSpawnbyindex(0, i);

                        
                        int centerX = (int)(Math.Round(sp.Position[0], 0) * scalevalue);
                        int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(sp.Position[2], 0) * scalevalue);
                        Point center = new Point(centerX, centerY);
                        Pen pen = new Pen(Color.Red)
                        {
                            Width = 2
                        };
                        getCircle(e.Graphics, pen, center, 10);
                    }
                    SpawnPositions sp1 = marketsettings.getSpawnbyindex(0, Selectedindex);
                    int centerX1 = (int)(Math.Round(sp1.Position[0], 0) * scalevalue);
                    int centerY1 = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(sp1.Position[2], 0) * scalevalue);

                    Point center1 = new Point(centerX1, centerY1);
                    Pen pen1 = new Pen(Color.LimeGreen)
                    {
                        Width = 2
                    };
                    getCircle(e.Graphics, pen1, center1, 10);
                    break;
                case 1:
                    Selectedindex = listBox8.SelectedIndex;
                    if (Selectedindex == -1) break;
                    for (int i = 0; i < listBox8.Items.Count; i++)
                    {
                        SpawnPositions sp = marketsettings.getSpawnbyindex(1, i);
                        int centerX = (int)(Math.Round(sp.Position[0], 0) * scalevalue);
                        int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(sp.Position[2], 0) * scalevalue);
                        Point center = new Point(centerX, centerY);
                        Pen pen = new Pen(Color.Red)
                        {
                            Width = 1
                        };
                        getCircle(e.Graphics, pen, center, 10);
                    }
                    sp1 = marketsettings.getSpawnbyindex(1, Selectedindex);
                    centerX1 = (int)(Math.Round(sp1.Position[0], 0) * scalevalue);
                    centerY1 = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(sp1.Position[2], 0) * scalevalue);

                    center1 = new Point(centerX1, centerY1);
                    pen1 = new Pen(Color.LimeGreen)
                    {
                        Width = 1
                    };
                    getCircle(e.Graphics, pen1, center1, 10);
                    break;
                case 2:
                    Selectedindex = listBox9.SelectedIndex;
                    if (Selectedindex == -1) break;
                    for (int i = 0; i < listBox9.Items.Count; i++)
                    {
                        SpawnPositions sp = marketsettings.getSpawnbyindex(2, i);
                        int centerX = (int)(Math.Round(sp.Position[0], 0) * scalevalue);
                        int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(sp.Position[2], 0) * scalevalue);

                        Point center = new Point(centerX, centerY);
                        Pen pen = new Pen(Color.Red)
                        {
                            Width = 1
                        };
                        getCircle(e.Graphics, pen, center, 10);
                    }
                    sp1 = marketsettings.getSpawnbyindex(2, Selectedindex);
                    centerX1 = (int)(Math.Round(sp1.Position[0], 0) * scalevalue);
                    centerY1 = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(sp1.Position[2], 0) * scalevalue);

                    center1 = new Point(centerX1, centerY1);
                    pen1 = new Pen(Color.LimeGreen)
                    {
                        Width = 1
                    };
                    getCircle(e.Graphics, pen1, center1, 10);
                    break;
            }
        }
        private void SpawnLocation_ValueChanged(object sender, EventArgs e)
        {
            if (action) return;
            switch (tabControl2.SelectedIndex)
            {
                case 0:
                    int index = listBox7.SelectedIndex;
                    SpawnPositions sp = marketsettings.getSpawnbyindex(0, index);
                    SetPositions(sp);
                    break;
                case 1:
                    index = listBox8.SelectedIndex;
                    sp = marketsettings.getSpawnbyindex(1, index);
                    SetPositions(sp);
                    break;
                case 2:
                    index = listBox9.SelectedIndex;
                    sp = marketsettings.getSpawnbyindex(2, index);
                    SetPositions(sp);
                    break;
            }
            pictureBox2.Invalidate();
        }
        private void SetPositions(SpawnPositions pos)
        {
            if (pos == null) return;
            action = true;
            pos.Position[0] = (float)numericUpDown10.Value;
            pos.Position[1] = (float)numericUpDown11.Value;
            pos.Position[2] = (float)numericUpDown12.Value;
            pos.Orientation[0] = (float)numericUpDown20.Value;
            pos.Orientation[1] = (float)numericUpDown21.Value;
            pos.Orientation[2] = (float)numericUpDown22.Value;
            marketsettings.isDirty = true;
            action = false;
        }
        private void trackBar4_MouseUp(object sender, MouseEventArgs e)
        {
            scale2 = trackBar4.Value;
            SetSpawnscale();
        }
        private void SetSpawnscale()
        {
            float scalevalue = scale2 * 0.05f;
            float mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            pictureBox2.Size = new Size(newsize, newsize);
        }
        private void pictureBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e is MouseEventArgs mouseEventArgs)
            {
                float scalevalue = scale2 * 0.05f;
                float mapsize = currentproject.MapSize;
                int newsize = (int)(mapsize * scalevalue);
                Cursor.Current = Cursors.WaitCursor;
                numericUpDown10.Value = (decimal)(mouseEventArgs.X / scalevalue);
                numericUpDown12.Value = (decimal)((newsize - mouseEventArgs.Y) / scalevalue);
                if (data.FileExists)
                {
                    numericUpDown11.Value = (decimal)(data.gethieght((float)numericUpDown10.Value, (float)numericUpDown12.Value));
                }
                Cursor.Current = Cursors.Default;
                marketsettings.isDirty = true;
                pictureBox2.Invalidate();
            }
        }

        #endregion MarketSettings

        #region trader
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            CanOnlyBuyRB.Visible = false;
            CanBuySellRB.Visible = false;
            CanOnlySellRB.Visible = false;
            AddFromCategoryListBox.Visible = false;
            if (listBox2.SelectedItems.Count < 1) return;
            action = true;
            currentTrader = listBox2.SelectedItem as Traders;
            textBox2.Text = currentTrader.Filename;
            textBox8.Text = currentTrader.m_Version.ToString();
            textBox7.Text = currentTrader.TraderName;
            textBox5.Text = currentTrader.DisplayName;
            textBox17.Text = currentTrader.TraderIcon;
            listBox10.DisplayMember = "Name";
            listBox10.ValueMember = "Value";
            listBox10.DataSource = currentTrader.Currencies;

            Poppulatetreeview();
            action = false;
        }
        private void Poppulatetreeview()
        {
            treeView1.Nodes.Clear();
            MytreeNode tn = new MytreeNode(currentTrader.TraderName)
            {
                Tag = "Parent"
            };
            foreach (TradersItem name in currentTrader.ListItems)
            {
                if (name.buysell != canBuyCansell.Attchment)
                {
                    marketItem mitem = MarketCats.getitemfromcategory(name.ClassName.ToLower().Replace("#STR_EXPANSION_MARKET_CATEGORY_", ""));

                    Categories cat = MarketCats.GetCat(mitem);
                    if (cat == null)
                    {
                        Console.WriteLine(currentTrader.DisplayName + " has an Item that could not be found int the market files:- " + name.ClassName);
                        MessageBox.Show("Item could not be found :- " + name.ClassName);
                        continue;
                    }
                    String CatName = cat.DisplayName.Replace("#STR_EXPANSION_MARKET_CATEGORY_", "");
                    MytreeNode cn = NodeExists(tn, CatName);
                    if (cn == null)
                    {
                        MytreeNode ccn = new MytreeNode(CatName)
                        {
                            Tag = cat
                        };
                        MytreeNode itemnode = new MytreeNode(name.ClassName)
                        {
                            BuySell = (int)name.buysell,
                            Tag = name
                        };
                        ccn.Nodes.Add(itemnode);
                        tn.Nodes.Add(ccn);
                    }
                    else
                    {
                        MytreeNode itemnode = new MytreeNode(name.ClassName)
                        {
                            BuySell = (int)name.buysell,
                            Tag = name
                        };
                        cn.Nodes.Add(itemnode);
                    }
                }

            }
            //foreach (TradersItem ti in titemstoremove)
            //{
            //    currentTrader.ListItems.Remove(ti);
            //    currentTrader.isDirty = true;
            //}
            treeView1.Nodes.Add(tn);
            treeView1.Sort();
            foreach (MytreeNode tn1 in treeView1.Nodes)
            {
                tn1.Expand();
            }

        }
        private MytreeNode NodeExists(MytreeNode node, string key)
        {
            foreach (MytreeNode subNode in node.Nodes)
            {
                if (subNode.Text == key)
                {
                    return subNode;
                }
            }
            return null;
        }
        public bool buysellFullCat = false;
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            pcontroll = true;
            AddFromCategoryListBox.Visible = false;
            treeView1.SelectedNode = e.Node;
            currentnode = e.Node as MytreeNode;
            if (e.Button == MouseButtons.Right)
            {
                contexMenu = new ContextMenuStrip();
                treeView1.SelectedNode = treeView1.GetNodeAt(e.X, e.Y);
                currenttreenodeparent = e.Node.Parent as MytreeNode;
                if (e.Node.Tag is Categories)
                {
                    CanOnlyBuyRB.Visible = false;
                    CanBuySellRB.Visible = false;
                    CanOnlySellRB.Visible = false;
                    treeviewcat = e.Node.Tag as Categories;
                    contexMenu.Items.Add("Remove Category and all items");
                    contexMenu.Items.Add("Add item from category list");
                }
                else if (e.Node.Tag is TradersItem)
                {
                    buysellFullCat = false;
                    CanOnlyBuyRB.Visible = true;
                    CanBuySellRB.Visible = true;
                    CanOnlySellRB.Visible = true;
                    switch (currentnode.BuySell)
                    {
                        case 0:
                            CanOnlyBuyRB.Checked = true;
                            break;

                        case 1:
                            CanBuySellRB.Checked = true;
                            break;

                        case 2:
                            CanOnlySellRB.Checked = true;
                            break;
                    }
                    treeviewitem = e.Node.Tag as marketItem;
                    contexMenu.Items.Add("Remove item from this trader");

                }
                else if (e.Node.Tag is string && e.Node.Tag as string == "Parent")
                {
                    CanOnlyBuyRB.Visible = false;
                    CanBuySellRB.Visible = false;
                    CanOnlySellRB.Visible = false;
                    contexMenu.Items.Add("Add new Category and all items");

                }
                contexMenu.Show(treeView1, e.Location);
                contexMenu.ItemClicked += new ToolStripItemClickedEventHandler(contextMenuStrip1_ItemClicked);
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (!currentnode.IsExpanded)
                    currentnode.Expand();
                else
                    currentnode.Collapse();
                if (e.Node.Tag is TradersItem)
                {
                    buysellFullCat = false;
                    CanOnlyBuyRB.Visible = true;
                    CanBuySellRB.Visible = true;
                    CanOnlySellRB.Visible = true;
                    switch (currentnode.BuySell)
                    {
                        case 0:
                            CanOnlyBuyRB.Checked = true;
                            break;

                        case 1:
                            CanBuySellRB.Checked = true;
                            break;

                        case 2:
                            CanOnlySellRB.Checked = true;
                            break;
                    }
                }
                else if (e.Node.Tag is Categories)
                { 
                    buysellFullCat = true;
                    CanOnlyBuyRB.Visible = true;
                    CanBuySellRB.Visible = true;
                    CanOnlySellRB.Visible = true;
                    CanOnlyBuyRB.Checked = false;
                    CanBuySellRB.Checked = false;
                    CanOnlySellRB.Checked = false;
                }
                else
                {
                    CanOnlyBuyRB.Visible = false;
                    CanBuySellRB.Visible = false;
                    CanOnlySellRB.Visible = false;
                }
            }
            pcontroll = false;
        }
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem clickedItem = e.ClickedItem;
            contexMenu.Close();
            string text = clickedItem.Text;
            switch (text)
            {
                case "Remove Category and all items":
                     foreach(TreeNode tn in currentnode.Nodes)
                     {
                        currentTrader.removetraderitem(tn.Text);
                     }
                    var savedExpansionState = treeView1.Nodes.GetExpansionState();
                    treeView1.BeginUpdate();
                    Poppulatetreeview();
                    treeView1.Nodes.SetExpansionState(savedExpansionState);
                    treeView1.EndUpdate();
                    break;
                case "Add new Category and all items":
                    AddFromCategoryListBox.Visible = true;
                    AddFromCategoryListBox.Tag = "AddCat";
                    AddFromCategoryListBox.Text = "CategoryList";
                    List<TraderListItem> returnlist = MarketCats.getallCats();
                    listBox3.Items.Clear();
                    listBox3.Items.AddRange(returnlist.ToArray());
                    break;
                case "Remove item from this trader":
                    currentTrader.removetraderitem(currentnode.Text);
                    savedExpansionState = treeView1.Nodes.GetExpansionState();
                    treeView1.BeginUpdate();
                    Poppulatetreeview();
                    treeView1.Nodes.SetExpansionState(savedExpansionState);
                    treeView1.EndUpdate();
                    break;
                case "Add item from category list":
                    AddFromCategoryListBox.Visible = true;
                    AddFromCategoryListBox.Text = "Add from Category List";
                    AddFromCategoryListBox.Tag = "AddFromCat";
                    Categories cat = MarketCats.GetCatFromDisplayName(currentnode.Text);
                    if(cat == null)
                    {
                        MessageBox.Show("there is no cartegory called " + currentnode.Text);
                        return;
                    }
                    listBox3.Items.Clear();
                    List<string> items = new List<string>();
                    foreach(marketItem item in cat.Items)
                    {
                        foreach (string vitem in item.Variants)
                        {
                            if (!items.Contains(vitem))
                                items.Add(vitem);
                        }
                        if (!items.Contains(item.ClassName))
                            items.Add(item.ClassName);

                    }
                    items.Sort();
                    listBox3.Items.AddRange(items.ToArray());
                    break;
            }
        }
        private void darkButton5_Click(object sender, EventArgs e)
        {
            if (AddFromCategoryListBox.Tag.ToString() == "AddExchange")
            {
                foreach (var item in listBox3.SelectedItems)
                {
                    string name = item.ToString();
                    currentTrader.Currencies.Add(name);
                    currentTrader.isDirty = true;
                }
                return;
            }
            if (AddFromCategoryListBox.Tag.ToString() == "AddCat")
            {
                foreach (var item in listBox3.SelectedItems)
                {
                    string name = item.ToString();
                    Categories Cat = MarketCats.GetCatFromDisplayName(name);
                    string catname = Path.GetFileNameWithoutExtension(Cat.Filename);
                    foreach (marketItem mi in Cat.Items)
                    {
                        currentTrader.AdditemtoTrader(mi.ClassName, catname);
                        currentTrader.isDirty = true;
                    }
                }
            }
            else if (AddFromCategoryListBox.Tag.ToString() == "AddFromCat")
            {
                foreach (var item in listBox3.SelectedItems)
                {
                    string name = item.ToString();
                    string catname = MarketCats.GetCatNameFromItemName(name);
                    currentTrader.AdditemtoTrader(name, catname);
                    currentTrader.isDirty = true;
                }
            }
            var savedExpansionState = treeView1.Nodes.GetExpansionState();
            treeView1.BeginUpdate();
            Poppulatetreeview();
            treeView1.Nodes.SetExpansionState(savedExpansionState);
            treeView1.EndUpdate();
        }
        private void darkButton10_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This Will Remove The All reference to this Trader, Are you sure you want to do this?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string currentname = currentTrader.TraderName;
                Traders.removeTrader(currentTrader);
                MessageBox.Show(currentname + " has been removed from the traders list", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                if (listBox2.Items.Count == 0)
                    listBox2.SelectedIndex = -1;
                else
                    listBox2.SelectedIndex = 0;
            }
        }
        private void darkButton11_Click(object sender, EventArgs e)
        {
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Enter Name of New Trader ", "Traders", "");
            Traders.AddNewTrader(UserAnswer);
            List<string> currencies = MarketCats.GetCatFromFileName("EXCHANGE").getallItemsasString();
            listBox2.SelectedIndex = -1;
            if (listBox2.Items.Count == 0)
                listBox2.SelectedIndex = listBox2.Items.Count - 1;
            else
                listBox2.SelectedIndex = 0;
            foreach(string s in currencies)
            {
                currentTrader.Currencies.Add(s);
            }
        }
        private void CanBuyCanSell_CheckChanged(object sender, EventArgs e)
        {
            if (pcontroll) return;
            TradersItem TI = currentnode.Tag as TradersItem;
            RadioButton rb = sender as RadioButton;
            if (!rb.Checked) { return; }
            switch (rb.Name)
            {
                case "CanOnlyBuyRB":
                    if (!buysellFullCat)
                    {
                        currentnode.BuySell = (int)canBuyCansell.CanOnlyBuy;
                        TI.buysell = canBuyCansell.CanOnlyBuy;
                    }
                    else if (buysellFullCat)
                    {
                        foreach(MytreeNode n in currentnode.Nodes)
                        {
                            n.BuySell =(int)canBuyCansell.CanOnlyBuy;
                            TradersItem t = n.Tag as TradersItem;
                            t.buysell = canBuyCansell.CanOnlyBuy;
                        }

                    }
                    break;
                case "CanBuySellRB":
                    if (!buysellFullCat)
                    {
                        currentnode.BuySell = (int)canBuyCansell.CanBuyAndsell;
                        TI.buysell = canBuyCansell.CanBuyAndsell;
                    }
                    else if (buysellFullCat)
                    {
                        foreach (MytreeNode n in currentnode.Nodes)
                        {
                            n.BuySell = (int)canBuyCansell.CanBuyAndsell;
                            TradersItem t = n.Tag as TradersItem;
                            t.buysell = canBuyCansell.CanBuyAndsell;
                        }
                    }
                    break;
                case "CanOnlySellRB":
                    if (!buysellFullCat)
                    {
                        currentnode.BuySell = (int)canBuyCansell.CanOnlySell;
                        TI.buysell = canBuyCansell.CanOnlySell;
                    }
                    else if (buysellFullCat)
                    {
                        foreach (MytreeNode n in currentnode.Nodes)
                        {
                            n.BuySell = (int)canBuyCansell.CanOnlySell;
                            TradersItem t = n.Tag as TradersItem;
                            t.buysell = canBuyCansell.CanOnlySell;
                        }
                    }
                    break;
                default:
                    break;
            }

            currentTrader.isDirty = true;
        }
        private bool CheckiffullCat(TradersItem TI)
        {
            List<marketItem> itemslist = MarketCats.GetCat(MarketCats.getitemfromcategory(TI.ClassName)).Items.ToList();
            List<string> a = new List<string>();
            foreach (marketItem i in itemslist)
            {
                a.Add(i.ClassName);
            }
            List<string> b = new List<string>();
            foreach (TradersItem ti in currentTrader.ListItems)
            {
                b.Add(ti.ClassName);
            }

            return Helper.ContainsAllItems(b, a);
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (action) return;
            currentTrader.isDirty = true;
            currentTrader.DisplayName = textBox5.Text;

        }
        private void darkButton13_Click(object sender, EventArgs e)
        {
            AddFromCategoryListBox.Visible = true;
            AddFromCategoryListBox.Text = "Add Currency from Exchange";
            AddFromCategoryListBox.Tag = "AddExchange";
            Categories cat = MarketCats.GetCatFromFileName("EXCHANGE");
            listBox3.Items.Clear();
            List<string> items = new List<string>();
            foreach (marketItem item in cat.Items)
            {
                foreach (string vitem in item.Variants)
                {
                    if (!items.Contains(vitem))
                        items.Add(vitem);
                }
                if (!items.Contains(item.ClassName))
                    items.Add(item.ClassName);

            }
            items.Sort();
            listBox3.Items.AddRange(items.ToArray());
        }
        private void darkButton12_Click(object sender, EventArgs e)
        {
            string removeitem = listBox10.GetItemText(listBox10.SelectedItem);
            currentTrader.Currencies.Remove(removeitem);
            currentTrader.isDirty = true;
        }
        private void textBox17_TextChanged(object sender, EventArgs e)
        {
            if (action) return;
            currentTrader.isDirty = true;
            currentTrader.TraderIcon = textBox17.Text;
        }
        #endregion trader

        #region Market Categories
        // Form Actions
        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox5.SelectedItems.Count < 1) return;
            currentCat = listBox5.SelectedItem as Categories;
            action = true;
            textBox12.Text = currentCat.m_Version.ToString();
            textBox11.Text = currentCat.Filename;
            textBox9.Text = currentCat.DisplayName;
            IconTB.Text = currentCat.Icon;
            InitStockPercentNUD.Value = (decimal)currentCat.InitStockPercent;
            CategorycolourPB.Invalidate();

            listBox4.DisplayMember = "Name";
            listBox4.ValueMember = "Value";
            listBox4.DataSource = currentCat.Items;

            action = false;
        }
        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox4.SelectedItems.Count < 1) return;
            action = true;
            currentitem = listBox4.SelectedItem as marketItem;
            textBox10.Text = currentitem.ClassName;
            if (currentitem.MaxPriceThreshold == -1)
                currentitem.MaxPriceThreshold = 1;
            numericUpDown6.Value = currentitem.MaxPriceThreshold;
            numericUpDown7.Value = currentitem.MinPriceThreshold;
            numericUpDown23.Value = currentitem.SellPricePercent;
            numericUpDown8.Value = currentitem.MaxStockThreshold;
            numericUpDown9.Value = currentitem.MinStockThreshold;
            numericUpDown24.Value = currentitem.QuantityPercent;

            listBox6.DisplayMember = "Name";
            listBox6.ValueMember = "Value";
            listBox6.DataSource = currentitem.SpawnAttachments;


            listBox11.DisplayMember = "Name";
            listBox11.ValueMember = "Value";
            listBox11.DataSource = currentitem.Variants;
            action = false;
        }
        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            if (!action)
            {
                currentCat.DisplayName = textBox9.Text;
                currentCat.isDirty = true;
                listBox5.Refresh();
            }
        }
        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            if (action) return;
            currentitem.ClassName = textBox10.Text;
            currentCat.isDirty = true;
        }
        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            if (action) return;
            if (listBox4.SelectedItems.Count > 0)
            {
                foreach (var item in listBox4.SelectedItems)
                {
                    marketItem pitem = item as marketItem;
                    pitem.MinStockThreshold = (int)numericUpDown9.Value;
                }
                currentCat.isDirty = true;
            }
        }
        private void numericUpDown24_ValueChanged(object sender, EventArgs e)
        {
            if (action) return;
            if (listBox4.SelectedItems.Count > 0)
            {
                foreach (var item in listBox4.SelectedItems)
                {
                    marketItem pitem = item as marketItem;
                    pitem.QuantityPercent = (int)numericUpDown24.Value;
                }
                currentCat.isDirty = true;
            }
        }
        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            if (action) return;
            if (listBox4.SelectedItems.Count > 0)
            {
                foreach (var item in listBox4.SelectedItems)
                {
                    marketItem pitem = item as marketItem;
                    pitem.MaxStockThreshold = (int)numericUpDown8.Value;
                }
                currentCat.isDirty = true;
            }
        }
        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            if (action) return;
            if (listBox4.SelectedItems.Count > 0)
            {
                foreach (var item in listBox4.SelectedItems)
                {
                    marketItem pitem = item as marketItem;
                    pitem.MinPriceThreshold = (int)numericUpDown7.Value;
                }
                currentCat.isDirty = true;
            }
        }
        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            if (action) return;
            if (listBox4.SelectedItems.Count > 0)
            {
                foreach (var item in listBox4.SelectedItems)
                {
                    marketItem pitem = item as marketItem;
                    pitem.MaxPriceThreshold = (int)numericUpDown6.Value;
                }
                currentCat.isDirty = true;
            }
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
            if (currentitem == null) { return; }
            currentitem.SpawnAttachments.Remove(listBox6.GetItemText(listBox6.SelectedItem));
            currentCat.isDirty = true;
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            if (currentitem == null) { return; }
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseMultiple = true,
                isCategoryitem = false
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (form.UseMultiple)
                    {
                        currentitem.SpawnAttachments.Add(l.ToLower());
                        currentCat.isDirty = true;
                    }
                    else if (!currentitem.SpawnAttachments.Contains(l.ToLower()))
                    {
                        currentitem.SpawnAttachments.Add(l.ToLower());
                        currentCat.isDirty = true;
                    }
                }
            }
        }


        private void darkButton27_Click(object sender, EventArgs e)
        {
            if(currentitem == null) { return; }
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if(result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    currentitem.SpawnAttachments.Add(l.ToLower());
                    currentCat.isDirty = true;
                }
            }
        }
        private void darkButton8_Click(object sender, EventArgs e)
        {
            if (currentitem == null) { return; }
            currentitem.Variants.Remove(listBox11.GetItemText(listBox11.SelectedItem));
            currentCat.isDirty = true;
        }
        private void darkButton9_Click(object sender, EventArgs e)
        {
            if (currentitem == null) { return; }
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
                    if (!currentitem.Variants.Contains(l.ToLower()))
                    {
                        currentitem.Variants.Add(l.ToLower());
                        currentCat.isDirty = true;
                    }
                }
            }
        }
        private void darkButton29_Click(object sender, EventArgs e)
        {
            if (currentitem == null) { return; }
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (!currentitem.Variants.Contains(l.ToLower()))
                    {
                        currentitem.Variants.Add(l.ToLower());
                        currentCat.isDirty = true;
                    }
                }
            }
        }
        private void darkButton3_Click(object sender, EventArgs e)
        {
            if(currentCat == null) { return; }
            List<string> removeitems = new List<string>();
            foreach (var item in listBox4.SelectedItems)
            {
                removeitems.Add(item.ToString());
            }
            StringBuilder sb = new StringBuilder();
            foreach (string removeitem in removeitems)
            {
                sb.Append(removeitem + Environment.NewLine);
            }
            if (MessageBox.Show("This Will Remove The folowing Items\n" + sb.ToString() + "Are you really sure you want me to do this?, if it goes Pete Tong its not my fault. Understood?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                foreach (string removeitem in removeitems)
                {
                    List<string> removedvarients = RemoveItem(removeitem);
                    currentCat.Items.Remove(currentCat.getitem(removeitem));
                    currentCat.isDirty = true;
                }
                if (listBox4.Items.Count == 0)
                    listBox4.SelectedIndex = -1;
                else
                   listBox4.SelectedIndex = 0;
                MessageBox.Show("items removed", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
        private List<string> RemoveItem(string removeitem)
        {
            marketItem item = currentCat.getitem(removeitem);
            List<string> Varients = item.Variants.ToList();
            List<string> toremovefromtraders = new List<string>();
            //check if varients has ther own item if so do not remove
            foreach (string vitem in Varients)
            {
                marketItem vi = currentCat.getitem(vitem);
                if (vi.ClassName != vitem)
                {
                    toremovefromtraders.Add(vitem);
                }
            }
            Traders.removelistfromtrader(toremovefromtraders);
            Traders.RemoveItemFromTrader(removeitem);
            return toremovefromtraders;
        }
        private void darkButton4_Click(object sender, EventArgs e)
        {
            if(currentCat == null) { return;  }
            Dictionary<string, bool> UsedTypes = new Dictionary<string, bool>();
            foreach(Categories mc in MarketCats.CatList)
            {
                foreach(marketItem item in mc.Items)
                {
                    if (!UsedTypes.ContainsKey(item.ClassName))
                        UsedTypes.Add(item.ClassName, true);
                    if(item.Variants.Count > 0)
                    {
                        foreach(string v in item.Variants)
                        {
                            if(!UsedTypes.ContainsKey(v))
                                UsedTypes.Add(v, true);
                        }
                    }
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
                    marketItem NewContainer = new marketItem
                    {
                        ClassName = l.ToLower(),
                        MaxPriceThreshold = 0,
                        MinPriceThreshold = 0,
                        MaxStockThreshold = 0,
                        MinStockThreshold = 0,
                        SellPricePercent = -1,
                        QuantityPercent = -1,
                        SpawnAttachments = new BindingList<string>(),
                        Variants = new BindingList<string>()

                    };
                    if (!Checkifincat(NewContainer))
                    {
                        currentCat.Items.Add(NewContainer);
                        currentCat.isDirty = true;
                    }
                    else
                    {
                        MessageBox.Show(NewContainer.ClassName + " Allready exists.....");
                    }
                }

            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton28_Click(object sender, EventArgs e)
        {
            if (currentCat == null) { return; }
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    marketItem NewContainer = new marketItem
                    {
                        ClassName = l.ToLower(),
                        MaxPriceThreshold = 0,
                        MinPriceThreshold = 0,
                        MaxStockThreshold = 0,
                        MinStockThreshold = 0
                    };
                    if (!Checkifincat(NewContainer))
                    {
                        currentCat.Items.Add(NewContainer);
                        currentCat.isDirty = true;
                    }
                    else
                    {
                        MessageBox.Show(NewContainer.ClassName + " Allready exists.....");
                    }
                }
            }
        }
        private bool Checkifincat(marketItem item)
        {
            foreach(Categories cat in  MarketCats.CatList)
            {
                if(cat.Items.Any(x => x.ClassName == item.ClassName))
                {
                    return true;
                }
            }
            return false;
        }
        private void darkButton6_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This Will Remove The All reference to this category, Are you sure you want to do this?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string removeitem = listBox5.GetItemText(listBox5.SelectedItem);

                string message = removeitem + " Category Removed....\nThe Following Items Were Removed from both Trader and Market Categories\n";
                foreach (marketItem item in currentCat.Items)
                {
                    message += item.ClassName + "\n";
                    List<string> removedvarients = RemoveItem(item.ClassName);
                    foreach (string l in removedvarients)
                    {
                        message += l + "\n";
                    }
                }
                MarketCats.RemoveCat(currentCat);
                MessageBox.Show(message, "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                if (listBox5.Items.Count == 0)
                    listBox5.SelectedIndex = -1;
                else
                    listBox5.SelectedIndex = 0;
            }

        }
        private void darkButton7_Click(object sender, EventArgs e)
        {
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Enter Name of New Category\nPlease not that this is the filename, do not use spaces or special characters\nYou can change the display name once its been created ", "Categories", "");
            if(UserAnswer == "") return;
            MarketCats.CreateNewCat(UserAnswer);
            listBox5.SelectedIndex = -1;
            if (listBox5.Items.Count == 0)
                listBox5.SelectedIndex = listBox5.Items.Count - 1;
            else
                listBox5.SelectedIndex = 0;
        }
        private void darkButton16_Click(object sender, EventArgs e)
        {
            if (currentitem == null) { return; }
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("your search term ", "Variants", "");
            if (UserAnswer == "") return;
            List<marketItem> varients = MarketCats.searchforitems(UserAnswer);
            bool searchsplit = false;
            if(UserAnswer.Contains("&"))
            {
                searchsplit = true;
            }
            List<typesType> vtypesvariaNTS = new List<typesType>();
            //check types files for any variants depending on search term
            if (!searchsplit)
                vtypesvariaNTS = vanillatypes.SerachTypes(UserAnswer);
            else if(searchsplit)
                vtypesvariaNTS = vanillatypes.SerachTypes(UserAnswer.Split('&'));
            foreach (TypesFile tfile in ModTypes)
            {
                if (!searchsplit)
                    vtypesvariaNTS.AddRange(tfile.SerachTypes(UserAnswer));
                else if (searchsplit)
                    vtypesvariaNTS.AddRange(tfile.SerachTypes(UserAnswer.Split('&')));
            }
            if (searchsplit)
            {
                for (int i = 0; i < vtypesvariaNTS.Count; i++)
                {
                    bool remove = false;
                    foreach (string s in UserAnswer.Split('&'))
                    {
                        if (!vtypesvariaNTS[i].name.ToLower().Contains(s))
                        {
                            remove = true;
                        }
                    }
                    if (remove)
                    {
                        vtypesvariaNTS.Remove(vtypesvariaNTS[i]);
                        i--;
                    }
                }
            }
            List<typesType> newlist = new List<typesType>();
            foreach (typesType type in vtypesvariaNTS)
            {
                if (!newlist.Any(x => x.name == type.name) && type.name.ToLower() != currentitem.ClassName)
                    newlist.Add(type);
            }

            foreach (typesType type in newlist)
            {
                marketItem newitem = new marketItem
                {
                    ClassName = type.name.ToLower(),
                    MaxPriceThreshold = currentitem.MaxPriceThreshold,
                    MinPriceThreshold = currentitem.MinPriceThreshold,
                    MaxStockThreshold = currentitem.MaxStockThreshold,
                    MinStockThreshold = currentitem.MinStockThreshold
                };
                if (!varients.Any(x => x.ClassName == newitem.ClassName))
                {
                    varients.Add(newitem);
                }
            }

            varients.Remove(currentitem);
            Variants vform = new Variants
            {
                ListOfpossibleVariants = varients,
                CurrentItem = currentitem
            };
            if (vform.ShowDialog() == DialogResult.OK)
            {
                List<marketItem> varientlist = vform.returneditems;
                foreach (marketItem v in varientlist)
                {
                    Categories cat = MarketCats.GetCat(v);
                    if (!currentitem.Variants.Contains(v.ClassName))
                    {
                        currentitem.Variants.Add(v.ClassName);
                        currentCat.isDirty = true;
                    }
                    if (cat != null)
                    {
                        if (currentCat.DisplayName != cat.DisplayName)
                        {
                            MarketCats.removeitemfromcat(v);
                            currentCat.Items.Add(v);
                            currentCat.isDirty = true;
                            if (cat.Items.Count == 0)
                            {
                                MarketCats.RemoveCat(cat);
                            }
                        }
                        if (currentitem.MaxPriceThreshold == v.MaxPriceThreshold && currentitem.MinPriceThreshold == v.MinPriceThreshold)
                        {
                            MarketCats.removeitemfromcat(v);
                        }

                    }
                }
            }
        }
        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                case 1:
                    categoriesToolStripMenuItem.Visible = false;
                    selectedItemsToolStripMenuItem.Visible = false;
                    itemsToolStripMenuItem.Visible = false;
                    allZonesToolStripMenuItem.Visible = false;
                    selectedZoneToolStripMenuItem.Visible = false;
                    break;
                case 2:
                    categoriesToolStripMenuItem.Visible = false;
                    selectedItemsToolStripMenuItem.Visible = false;
                    itemsToolStripMenuItem.Visible = false;
                    allZonesToolStripMenuItem.Visible = true;
                    selectedZoneToolStripMenuItem.Visible = true;
                    break;
                case 3:
                    categoriesToolStripMenuItem.Visible = false;
                    selectedItemsToolStripMenuItem.Visible = false;
                    itemsToolStripMenuItem.Visible = false;
                    allZonesToolStripMenuItem.Visible = false;
                    selectedZoneToolStripMenuItem.Visible = false;
                    break;
                case 4:
                    categoriesToolStripMenuItem.Visible = true;
                    selectedItemsToolStripMenuItem.Visible = true;
                    itemsToolStripMenuItem.Visible = true;
                    allZonesToolStripMenuItem.Visible = false;
                    selectedZoneToolStripMenuItem.Visible = false;
                    break;
            }
        }
        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<marketItem> items = listBox4.SelectedItems.Cast<marketItem>().ToList();
            CategoryList cl = new CategoryList
            {
                MarketCats = MarketCats,
                marketitems = items
            };
            cl.ShowDialog();
        }
        private void darkButton25_Click(object sender, EventArgs e)
        {
            if (currentitem == null) { return; }
            if(listBox11.SelectedItem == null) { return; }
            string newitem = listBox11.GetItemText(listBox11.SelectedItem);
            marketItem ni = new marketItem()
            {
                ClassName = newitem,
                MaxPriceThreshold = currentitem.MaxPriceThreshold,
                MinPriceThreshold = currentitem.MinPriceThreshold,
                MaxStockThreshold = currentitem.MaxStockThreshold,
                MinStockThreshold = currentitem.MinStockThreshold,
                SellPricePercent = currentitem.SellPricePercent,
                QuantityPercent = currentitem.QuantityPercent
            };
            currentCat.Items.Add(ni);
            currentCat.isDirty = true;
        }

        /// <summary>
        /// Added New Fuctions for Version 6
        /// Icon and Colour for Market Cat
        /// Purchase Type removed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CategorycolourPB_Paint(object sender, PaintEventArgs e)
        {
            if (currentCat == null) { return; }
            PictureBox pb = sender as PictureBox;
            Rectangle region;
            region = pb.ClientRectangle;
            string col = currentCat.Color;
            string col1 = "#" + col.Substring(6) + col.Remove(6, 2);
            Color colour = ColorTranslator.FromHtml(col1);
            using (Brush brush = new SolidBrush(colour))
            {
                e.Graphics.FillRectangle(brush, region);
            }
            e.Graphics.DrawRectangle(SystemPens.ControlText, region.Left, region.Top, region.Width - 1, region.Height - 1);
        }
        private void CategorycolourPB_Click(object sender, EventArgs e)
        {
            if(currentCat == null) { return; }
            PictureBox pb = sender as PictureBox;
            ColorPickerDialog cpick = new ColorPickerDialog
            {
                StartPosition = FormStartPosition.CenterParent
            };
            string col = "";
            if (currentCat == null)
                col = "FBFCFEFF";
            else
                col = currentCat.Color;
            string col1 = "#" + col.Substring(6) + col.Remove(6, 2);
            cpick.Color = ColorTranslator.FromHtml(col1);
            if (cpick.ShowDialog() == DialogResult.OK)
            {
                currentCat.Color = (cpick.Color.Name.Substring(2) + cpick.Color.Name.Substring(0, 2)).ToUpper();
                pb.Invalidate();
                currentCat.isDirty = true;
            }
        }
        private void IconTB_TextChanged(object sender, EventArgs e)
        {
            if (!action)
            {
                currentCat.Icon = IconTB.Text;
                currentCat.isDirty = true;
            }
        }
        
        /// <summary>
        /// Added in version 7 item sell price percentage of buy value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numericUpDown23_ValueChanged(object sender, EventArgs e)
        {
            if (action) return;
            if (listBox4.SelectedItems.Count > 0)
            {
                foreach (var item in listBox4.SelectedItems)
                {
                    marketItem pitem = item as marketItem;
                    pitem.SellPricePercent = (int)numericUpDown23.Value;
                }
                currentCat.isDirty = true;
            }
        }

        /// <summary>
        /// Added in version 9 for cat sell stock percent
        /// </summary>
        private void InitStockPercentNUD_ValueChanged(object sender, EventArgs e)
        {
            if (action) return;
            currentCat.InitStockPercent = (float)InitStockPercentNUD.Value;
            currentCat.isDirty = true;
        }

        #endregion Market Categories

        #region TraderZones
        public int TraderZoneMapScale = 1;
        private void trackBar2_MouseUp(object sender, MouseEventArgs e)
        {
            TraderZoneMapScale = trackBar2.Value;
            SetScale();
        }
        private void SetScale()
        {
            float scalevalue = TraderZoneMapScale * 0.05f;
            float mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            pictureBox1.Size = new Size(newsize, newsize);
        }
        private void DrawAll(object sender, PaintEventArgs e)
        {
            float scalevalue = TraderZoneMapScale * 0.05f;
            foreach (Zones zones in Zones.ZoneList)
            {
                int centerX = (int)(Math.Round(zones.Position[0], 0) * scalevalue);
                int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(zones.Position[2], 0) * scalevalue);
                int radius = ((int)zones.Radius / (currentproject.MapSize / 512)) * TraderZoneMapScale;
                Point center = new Point(centerX, centerY);
                Pen pen = new Pen(Color.Red);
                if (currentZone.m_ZoneName == zones.m_ZoneName)
                    pen.Color = Color.LimeGreen;
                pen.Width = 2;
                getCircle(e.Graphics, pen, center, radius);
            }
        }
        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            if (e is MouseEventArgs mouseEventArgs)
            {
                Cursor.Current = Cursors.WaitCursor;
                float scalevalue = TraderZoneMapScale * 0.05f;
                float mapsize = currentproject.MapSize;
                int newsize = (int)(mapsize * scalevalue);
                numericUpDown1.Value = (decimal)(mouseEventArgs.X / scalevalue);
                numericUpDown3.Value = (decimal)((newsize - mouseEventArgs.Y) / scalevalue);
                if (data.FileExists)
                {
                    numericUpDown2.Value = (decimal)(data.gethieght((float)numericUpDown1.Value, (float)numericUpDown3.Value));
                }
                Cursor.Current = Cursors.Default;
                currentZone.isDirty = true;
                pictureBox2.Invalidate();
            }
        }
        private void getCircle(Graphics drawingArea, Pen penToUse, Point center, int radius)
        {
            Rectangle rect = new Rectangle(center.X - 1, center.Y - 1, 2, 2);
            drawingArea.DrawEllipse(penToUse, rect);
            Rectangle rect2 = new Rectangle(center.X - radius, center.Y - radius, radius * 2, radius * 2);
            drawingArea.DrawEllipse(penToUse, rect2);
        }
        private void darkButton14_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This Will Remove The All reference to this Trader Zone, Are you sure you want to do this?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
                if (File.Exists(currentZone.Filename))
                {
                    Directory.CreateDirectory(ZonesPath + "\\Backup\\" + SaveTime);
                    File.Copy(currentZone.Filename, ZonesPath + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(currentZone.Filename) + ".bak");
                    File.Delete(currentZone.Filename);
                }
                MessageBox.Show(currentZone.m_ZoneName + " has been removed from the Zone list", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                Zones.removeZone(currentZone);
                if (Zones.ZoneList.Count == 0)
                    listBox1.SelectedIndex = -1;
                else
                    listBox1.SelectedIndex = 0;
            }
        }
        private void darkButton15_Click(object sender, EventArgs e)
        {
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Enter Name of New Zone ", "Zones", "");
            Zones.NewTraderZone(UserAnswer, currentproject.MapSize);
            listBox1.SelectedIndex = -1;
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
            pictureBox1.Invalidate();
        }
        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (pcontroll) return;
            currentZone.Radius = (float)numericUpDown4.Value;
            currentZone.isDirty = true;
            pictureBox1.Invalidate();
        }
        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (pcontroll) return;
            currentZone.BuyPricePercent = (float)numericUpDown5.Value;
            currentZone.isDirty = true;
            //setzoneprices();
            //pictureBox1.Invalidate();
        }
        private void numericUpDown13_ValueChanged(object sender, EventArgs e)
        {
            if (pcontroll) return;
            currentZone.SellPricePercent = (float)numericUpDown13.Value;
            currentZone.isDirty = true;
            //setzoneprices();
            //pictureBox1.Invalidate();
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (pcontroll) return;
            currentZone.Position[0] = (float)numericUpDown1.Value;
            currentZone.isDirty = true;
            pictureBox1.Invalidate();
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (pcontroll) return;
            currentZone.Position[1] = (float)numericUpDown2.Value;
            currentZone.isDirty = true;
            pictureBox1.Invalidate();
        }
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (pcontroll) return;
            currentZone.Position[2] = (float)numericUpDown3.Value;
            currentZone.isDirty = true;
            pictureBox1.Invalidate();
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (pcontroll) return;
            currentZone.m_DisplayName = textBox4.Text;
            currentZone.isDirty = true;
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (pcontroll) return;
            currentZone.m_ZoneName = textBox3.Text;
            currentZone.isDirty = true;
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (pcontroll) return;
            DataGridViewColumn column = dataGridView1.Columns[e.ColumnIndex];
            if (column.Name == "StockCheker")
            {
                //setzoneprices();
            }
            currentZone.isDirty = true;
        }
        private void setStockValueForAllItemsInAllZonesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Eneter your stock value % ", "Stock Value", "");
            if (UserAnswer == "") return;
            int num = Convert.ToInt32(UserAnswer);
            //get all items in current zone area
            foreach (Zones zone in Zones.ZoneList)
            {
                List<Traders> ZoneTraders = new List<Traders>();
                foreach (Tradermap tm in tradermaps.maps)
                {
                    PointF pC = new PointF(zone.Position[0], zone.Position[2]);
                    PointF pP = new PointF(tm.position.X, tm.position.Z);
                    if (IsWithinCircle(pC, pP, zone.Radius))
                    {
                        ZoneTraders.Add(Traders.GetTraderFromName(tm.NPCTrade));
                    }
                }
                zone.StockItems = new BindingList<StockItem>();
                foreach (Traders t in ZoneTraders)
                {
                    foreach (TradersItem ti in t.ListItems)
                    {
                        Categories cat = MarketCats.GetCatFromDisplayName(ti.ClassName);
                        marketItem i = MarketCats.getitemfromcategory(ti.ClassName);
                        int Stocknum = (int)((float)i.MaxStockThreshold / 100 * num);
                        StockItem si = new StockItem
                        {
                            Classname = ti.ClassName,
                            StockValue = Stocknum,
                            //StockCheker = Stocknum
                        };
                        //float initialvalue = Helper.PowCurveCalc((float)i.MinStockThreshold, (float)i.MaxStockThreshold, i.MaxStockThreshold - si.StockCheker, i.MinPriceThreshold, i.MaxPriceThreshold, 6.0f);
                        //si.ZoneBuyPrice = (int)(initialvalue * (zone.BuyPricePercent / 100));

                        //if (currentZone.SellPricePercent == -1)
                        //    si.ZoneSellPrice = (int)(initialvalue * (marketsettings.SellPricePercent / 100));
                        //else
                        //{
                        //    si.ZoneSellPrice = (int)(initialvalue * (zone.SellPricePercent / 100));
                        //}
                        if (!zone.StockItems.Any(x => x.Classname == si.Classname))
                            zone.StockItems.Add(si);
                    }
                }
                zone.StockItems = new BindingList<StockItem>(new BindingList<StockItem>(zone.StockItems.OrderBy(x => x.Classname).ToList()));
                zone.isDirty = true;
            }
            pcontroll = true;

            pcontroll = false;
        }
        private void deleteAllStockForAllZonesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Zones zone in Zones.ZoneList)
            {
                zone.StockItems = new BindingList<StockItem>();
                zone.isDirty = true;
            }
        }
        private void deleteStockForSelectedZoneToolStripMenuItem_Click(object sender, EventArgs e)
        {

            currentZone.StockItems = new BindingList<StockItem>();
            currentZone.isDirty = true;
        }
        private void resetStockItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Eneter your stock value %", "Stock Value", "");
            if (UserAnswer == "") return;
            int num = Convert.ToInt32(UserAnswer);
            //get all items in current zone area
            List<Traders> ZoneTraders = new List<Traders>();
            foreach (Tradermap tm in tradermaps.maps)
            {
                PointF pC = new PointF(currentZone.Position[0], currentZone.Position[2]);
                PointF pP = new PointF(tm.position.X, tm.position.Z);
                if(IsWithinCircle(pC, pP, currentZone.Radius))
                {
                    ZoneTraders.Add(Traders.GetTraderFromName(tm.NPCTrade));
                }
            }
            currentZone.StockItems = new BindingList<StockItem>();
            foreach(Traders t in ZoneTraders)
            {
                foreach(TradersItem ti in t.ListItems)
                {
                    Categories cat = MarketCats.GetCatFromDisplayName(ti.ClassName);
                    marketItem i = MarketCats.getitemfromcategory(ti.ClassName);
                    SetStock(num, ti.ClassName, i);
                    if (i.Variants.Count > 0)
                    {
                        foreach (string v in i.Variants)
                        {
                            marketItem vi = MarketCats.getitemfromcategory(v);
                            if (!currentZone.StockItems.Any(x => x.Classname == v))
                            {
                                if (vi == null)
                                    SetStock(num, v, i);
                                else
                                    SetStock(num, v, vi);
                            }
                        }
                    }
                    if (i.SpawnAttachments.Count > 0)
                    {
                        foreach (string a in i.SpawnAttachments)
                        {
                            marketItem ai = MarketCats.getitemfromcategory(a);
                            if (!currentZone.StockItems.Any(x => x.Classname == a))
                            {
                                if (ai == null)
                                    SetStock(num, a, i);
                                else
                                    SetStock(num, a, ai);
                            }
                        }
                    }
                }
            }
            currentZone.StockItems = new BindingList<StockItem>(new BindingList<StockItem>(currentZone.StockItems.OrderBy(x => x.Classname).ToList()));
            currentZone.isDirty = true;
            dataGridView1.Invalidate();
            pcontroll = true;

            pcontroll = false;
        }
        private void SetStock(int num, string classname, marketItem i)
        {
            int Stocknum = (int)((float)i.MaxStockThreshold / 100 * num);
            if (Stocknum == 0) Stocknum = 1;
            StockItem si = new StockItem
            {
                Classname = classname,
                StockValue = Stocknum,
               // StockCheker = Stocknum
            };
            //float initialvalue = Helper.PowCurveCalc((float)i.MinStockThreshold, (float)i.MaxStockThreshold, i.MaxStockThreshold - si.StockCheker, i.MinPriceThreshold, i.MaxPriceThreshold, 6.0f);
            //si.ZoneBuyPrice = (int)(initialvalue * (currentZone.BuyPricePercent / 100));

            //if (currentZone.SellPricePercent == -1)
            //    si.ZoneSellPrice = (int)(initialvalue * (marketsettings.SellPricePercent / 100));
            //else
            //{
            //    si.ZoneSellPrice = (int)(initialvalue * (currentZone.SellPricePercent / 100));
            //}
            if (!currentZone.StockItems.Any(x => x.Classname == si.Classname))
                currentZone.StockItems.Add(si);
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
        public void setzoneprices()
        {
            foreach (Zones zone in Zones.ZoneList)
            {
                float zonebuypercentage = zone.BuyPricePercent;
                
                foreach(StockItem item in zone.StockItems)
                {
                    marketItem i = MarketCats.getitemfromcategory(item.Classname);
                    if(i == null) { continue; }
                    //float initialvalue = Helper.PowCurveCalc((float)i.MinStockThreshold, (float)i.MaxStockThreshold,  item.StockCheker, i.MaxPriceThreshold, i.MinPriceThreshold, 6.0f);
                    //item.ZoneBuyPrice = (int)(initialvalue * (zone.BuyPricePercent / 100));
                    //if (zone.SellPricePercent == -1)
                    //    item.ZoneSellPrice = (int)(initialvalue * (marketsettings.SellPricePercent / 100));
                    //else
                    //{
                    //   item.ZoneSellPrice = (int)(initialvalue * (zone.SellPricePercent / 100));
                    //}
                }
            }
            dataGridView1.Invalidate();
        }

        private void findMissingItemsAndSetStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> missingitems = new List<string>();
            List<Traders> ZoneTraders = new List<Traders>();
            foreach (Tradermap tm in tradermaps.maps)
            {
                PointF pC = new PointF(currentZone.Position[0], currentZone.Position[2]);
                PointF pP = new PointF(tm.position.X, tm.position.Z);
                if (IsWithinCircle(pC, pP, currentZone.Radius))
                {
                    ZoneTraders.Add(Traders.GetTraderFromName(tm.NPCTrade));
                }
            }
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Eneter your stock value %", "Stock Value", "");
            if (UserAnswer == "") return;
            int num = Convert.ToInt32(UserAnswer);
            foreach (Traders t in ZoneTraders)
            {
                foreach (TradersItem ti in t.ListItems)
                {
                    Categories cat = MarketCats.GetCatFromDisplayName(ti.ClassName);
                    marketItem i = MarketCats.getitemfromcategory(ti.ClassName);
                    if (!currentZone.StockItems.Any(x => x.Classname == ti.ClassName))
                    {
                        SetStock(num, ti.ClassName, i);
                        missingitems.Add(i.ClassName);
                    }
                    if (i.Variants.Count > 0)
                    {
                        foreach (string v in i.Variants)
                        {
                            marketItem vi = MarketCats.getitemfromcategory(v);
                            if (!currentZone.StockItems.Any(x => x.Classname == v))
                            {
                                if (vi == null)
                                    SetStock(num, v, i);
                                else
                                    SetStock(num, v, vi);
                            }
                        }
                    }
                    if (i.SpawnAttachments.Count > 0)
                    {
                        foreach (string a in i.SpawnAttachments)
                        {
                            marketItem ai = MarketCats.getitemfromcategory(a);
                            if (!currentZone.StockItems.Any(x => x.Classname == a))
                            {
                                if (ai == null)
                                    SetStock(num, a, i);
                                else
                                    SetStock(num, a, ai);
                            }
                        }
                    }
                }
            }
            currentZone.isDirty = true;
            dataGridView1.Invalidate();
            pcontroll = true;

            pcontroll = false;
            if(missingitems.Count> 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("The following items were added to the stock zone:-" + Environment.NewLine);
                foreach(string s in missingitems)
                {
                    sb.Append(s + Environment.NewLine);
                }
                MessageBox.Show(sb.ToString());
            }
        }
        #endregion TraderZones

        #region tradermaps
        public Zones currenttradermapzone;
        public Tradermap currentctradermap;
        public BindingList<Tradermap> ZoneTraders;
        private void listBox13_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox13.SelectedItems.Count < 1) { return; }
            currenttradermapzone = listBox13.SelectedItem as Zones;
            setTraderzonelist();
        }
        private void setTraderzonelist()
        {
            ZoneTraders = GetTradersforzone();
            listBox14.DisplayMember = "Name";
            listBox14.ValueMember = "Value";
            listBox14.DataSource = ZoneTraders;
        }
        private BindingList<Tradermap> GetTradersforzone()
        {
            if(currentZone == null) { return null; }
            BindingList<Tradermap> ZoneTraders = new BindingList<Tradermap>();
            foreach (Tradermap tm in tradermaps.maps)
            {
                PointF pC = new PointF(currentZone.Position[0], currentZone.Position[2]);
                PointF pP = new PointF(tm.position.X, tm.position.Z);
                if (IsWithinCircle(pC, pP, currentZone.Radius))
                {
                    tm.Filename = Path.GetDirectoryName(tm.Filename) + "\\" + currentZone.m_ZoneName + ".map";
                    ZoneTraders.Add(tm);
                }
            }
            return ZoneTraders;
        }
        private void listBox14_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox14.SelectedItems.Count < 1) return;
            currentctradermap = listBox14.SelectedItem as Tradermap;
            pcontroll = true;
            textBox14.Text = currentctradermap.Filename;
            textBox15.Text = currentctradermap.NPCName;

            SetNPCTrade();

            
            numericUpDown14.Value = (decimal)currentctradermap.position.X;
            numericUpDown15.Value = (decimal)currentctradermap.position.Y;
            numericUpDown16.Value = (decimal)currentctradermap.position.Z;
            numericUpDown17.Value = (decimal)currentctradermap.roattions.X;
            numericUpDown18.Value = (decimal)currentctradermap.roattions.Y;
            numericUpDown19.Value = (decimal)currentctradermap.roattions.Z;
            listBox15.DisplayMember = "Name";
            listBox15.ValueMember = "Value";
            listBox15.DataSource = currentctradermap.Attachments;
            pcontroll = false;
        }
        private void listBox18_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox14.SelectedItems.Count < 1) return;
            currentctradermap = listBox18.SelectedItem as Tradermap;
            if(currentctradermap == null) { return; }
            pcontroll = true;
            textBox14.Text = currentctradermap.Filename;
            textBox15.Text = currentctradermap.NPCName;

            SetNPCTrade();

            numericUpDown14.Value = (decimal)currentctradermap.position.X;
            numericUpDown15.Value = (decimal)currentctradermap.position.Y;
            numericUpDown16.Value = (decimal)currentctradermap.position.Z;
            numericUpDown17.Value = (decimal)currentctradermap.roattions.X;
            numericUpDown18.Value = (decimal)currentctradermap.roattions.Y;
            numericUpDown19.Value = (decimal)currentctradermap.roattions.Z;
            listBox15.DisplayMember = "Name";
            listBox15.ValueMember = "Value";
            listBox15.DataSource = currentctradermap.Attachments;
            pcontroll = false;
        }
        public bool hastrader;
        private void SetNPCTrade()
        {
            textBox16.Text = currentctradermap.NPCTrade;
            // check if tader exists
            Traders t = Traders.GetTraderFromName(currentctradermap.NPCTrade);
            if (t == null)
            {
                TradercheckPanel.BackgroundImage = imageList1.Images[1];
                TradercheckPanel.BackgroundImageLayout = ImageLayout.None;
                hastrader = false;
            }
            else
            {
                TradercheckPanel.BackgroundImage = imageList1.Images[0];
                hastrader = true;
            }
        }
        private void TradercheckPanel_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Panel)
            {
                Panel p = sender as Panel;
                ttpShow = new ToolTip
                {
                    AutoPopDelay = 2000,
                    InitialDelay = 1000,
                    ReshowDelay = 500,
                    IsBalloon = true
                };
                if (hastrader)
                    ttpShow.Show("NPC attched to existing trader", p, p.Width, p.Height / 10, 5000);
                else
                    ttpShow.Show("No Trader of this Name,\nClick cross to assign to exiting trader", p, p.Width, p.Height / 10, 5000);

            }   
        }
        private void TradercheckPanel_MouseLeave(object sender, EventArgs e)
        {
            ttpShow.Dispose();
        }
        private void TradercheckPanel_MouseClick(object sender, MouseEventArgs e)
        {
            groupBox12.Visible = true;
        }
        private void darkButton22_Click(object sender, EventArgs e)
        {
            Traders newNPCTrader = listBox16.SelectedItem as Traders;
            currentctradermap.NPCTrade = newNPCTrader.TraderName;
            tradermaps.isDirty = true;
            SetNPCTrade();
            groupBox12.Visible = false;
        }
        private void darkButton21_Click(object sender, EventArgs e)
        {
            if(listBox13.Items.Count == 0)
            {
                MessageBox.Show("Please create some trader zones..");
                return;
            }
            NewTraderMapForm NTM = new NewTraderMapForm
            {
                Zones = Zones,
                TraderMaps = tradermaps,
                Traders = Traders
            };
            if (NTM.ShowDialog() == DialogResult.OK)
            {
                if (NTM.NPC == "")
                {
                    MessageBox.Show(" No NPC was selected...");
                    return;
                }
                Tradermap newtradermap = new Tradermap
                {
                    Filename = tradermapsPath + "\\" + NTM.SelectedZone.m_ZoneName + ".map",
                    NPCName = NTM.NPC,
                    NPCTrade = NTM.selectedTrader.TraderName,
                    position = new Vec3() { X = NTM.SelectedZone.Position[0], Y = NTM.SelectedZone.Position[1], Z = NTM.SelectedZone.Position[2] },
                    roattions = new Vec3() { X = 0, Y = 0, Z = 0 },
                    Attachments = new BindingList<string>()
                };
                tradermaps.maps.Add(newtradermap);
                tradermaps.isDirty = true;
                setTraderzonelist();
            }
        }
        private void darkButton20_Click(object sender, EventArgs e)
        {
            Tradermap newNPCTrader = listBox14.SelectedItem as Tradermap;
            tradermaps.maps.Remove(newNPCTrader);
            tradermaps.isDirty = true;
            setTraderzonelist();
        }
        private void darkButton33_Click(object sender, EventArgs e)
        {
            Tradermap newNPCTrader = listBox18.SelectedItem as Tradermap;
            tradermaps.maps.Remove(newNPCTrader);
            tradermaps.isDirty = true;
            NoZoneTraders.Remove(newNPCTrader);
            setTraderzonelist();
        }
        private void numericUpDown14_ValueChanged(object sender, EventArgs e)
        {
            if (pcontroll) return;
            currentctradermap.position.X = (float)numericUpDown14.Value;
            tradermaps.isDirty = true;
        }
        private void numericUpDown15_ValueChanged(object sender, EventArgs e)
        {
            if (pcontroll) return;
            currentctradermap.position.Y = (float)numericUpDown15.Value;
            tradermaps.isDirty = true;
        }
        private void numericUpDown16_ValueChanged(object sender, EventArgs e)
        {
            if (pcontroll) return;
            currentctradermap.position.Z = (float)numericUpDown16.Value;
            tradermaps.isDirty = true;
        }
        private void numericUpDown17_ValueChanged(object sender, EventArgs e)
        {
            if (pcontroll) return;
            currentctradermap.roattions.X = (float)numericUpDown17.Value;
            tradermaps.isDirty = true;
        }
        private void numericUpDown18_ValueChanged(object sender, EventArgs e)
        {
            if (pcontroll) return;
            currentctradermap.roattions.Y = (float)numericUpDown18.Value;
            tradermaps.isDirty = true;

        }
        private void numericUpDown19_ValueChanged(object sender, EventArgs e)
        {
            if (pcontroll) return;
            currentctradermap.roattions.Z = (float)numericUpDown19.Value;
            tradermaps.isDirty = true;
        }
        private void darkButton24_Click(object sender, EventArgs e)
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
                    currentctradermap.Attachments.Add(l);
                    tradermaps.isDirty = true;
                }
            }
        }
        private void darkButton23_Click(object sender, EventArgs e)
        {
            currentctradermap.Attachments.Remove(listBox15.GetItemText(listBox15.SelectedItem));
            tradermaps.isDirty = true;
        }
        private void darkButton26_Click(object sender, EventArgs e)
        {
            TraderNPCs newnpc = new TraderNPCs();
            if(newnpc.ShowDialog() == DialogResult.OK)
            {
                textBox15.Text = newnpc.selectedNPC;
            }
        }
        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            if (pcontroll) return;
            currentctradermap.NPCName = textBox15.Text;
            listBox14.Refresh();
            tradermaps.isDirty = true;
        }
        private void darkButton32_Click(object sender, EventArgs e)
        {
            foreach (Zones zone in Zones.ZoneList)
            {
                foreach (Tradermap tm in tradermaps.maps)
                {
                    PointF pC = new PointF(zone.Position[0], zone.Position[2]);
                    PointF pP = new PointF(tm.position.X, tm.position.Z);
                    if (IsWithinCircle(pC, pP, zone.Radius))
                    {
                        tm.Filename = Path.GetDirectoryName(tm.Filename) + "\\" + zone.m_ZoneName + ".map";
                        tm.IsInAZone = true;
                    }
                }
            }
            setTraderzonelist();
        }
        #endregion tradermaps

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            string searchterm = toolStripTextBox1.Text;
            switch (tabControl1.SelectedIndex)
            {
                case 4:
                    string found = "";
                    foreach(Categories cat in MarketCats.CatList)
                    {
                        if (cat.Items.Any(x => x.ClassName.Contains(searchterm)))
                        {
                            found += "The Category \"" + cat.DisplayName + "\" has an item copntaining the serchterm." + Environment.NewLine;
                        }
                    }
                    if (found == "")
                        MessageBox.Show("No items were found containing the serch term");
                    else
                    {
                        MessageBox.Show(found);
                    }

                    break;
                default:
                    break;
            }
        }
        private void darkButton34_Click(object sender, EventArgs e)
        {
            Tradermap newNPCTrader = listBox18.SelectedItem as Tradermap;
            NoZoneTraders.Remove(newNPCTrader);
            newNPCTrader.position = new Vec3()
            {
                X = currentZone.Position[0],
                Y = currentZone.Position[1],
                Z = currentZone.Position[2]
            };
            tradermaps.isDirty = true;
            setTraderzonelist();
        }

        private void trackBar3_MouseUp(object sender, MouseEventArgs e)
        {

        }



        private void darkButton17_Click(object sender, EventArgs e)
        {
            string removeitem = listBox12.GetItemText(listBox12.SelectedItem);
            marketsettings.Currencies.Remove(removeitem);
            marketsettings.isDirty = true;
        }

        private void darkButton36_Click(object sender, EventArgs e)
        {
            foreach (var item in listBox19.SelectedItems)
            {
                string name = item.ToString();
                marketsettings.Currencies.Add(name);
                marketsettings.isDirty = true;
            }
            return;
        }

        private void chnageAttchmentsToLowerCaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (marketItem item in currentCat.Items)
            {
                for(int i = 0; i < item.SpawnAttachments.Count(); i++)
                {
                    item.SpawnAttachments[i] = item.SpawnAttachments[i].ToLower();
                }
                for(int J = 0; J < item.Variants.Count(); J++)
                {
                    item.Variants[J] = item.Variants[J].ToLower();
                }
            }
            currentCat.isDirty = true;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Set your Starting Percentage... ", "Starting Stock", "");
            if (UserAnswer == "") return;
            int value = Convert.ToInt32(UserAnswer);
            foreach (Categories cats in MarketCats.CatList)
            {
                cats.InitStockPercent = value;
                cats.isDirty = true;
            }
        }


    }
}
