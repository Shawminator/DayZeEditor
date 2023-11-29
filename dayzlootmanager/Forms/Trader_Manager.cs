using DarkUI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Windows.Forms;
using DayZeLib;

namespace DayZeEditor
{

    public partial class DRJonesTrader_Manager : DarkForm
    {
        #region some shit


        private const int TVIF_STATE = 0x8;
        private const int TVIS_STATEIMAGEMASK = 0xF000;
        private const int TV_FIRST = 0x1100;
        private const int TVM_SETITEM = TV_FIRST + 63;

        [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Auto)]
        private struct TVITEM
        {
            public int mask;
            public IntPtr hItem;
            public int state;
            public int stateMask;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszText;
            public int cchTextMax;
            public int iImage;
            public int iSelectedImage;
            public int cChildren;
            public IntPtr lParam;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam,
                                                 ref TVITEM lParam);

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
        #endregion some shit


        public Project currentproject { get; internal set; }
        public TypesFile vanillatypes;
        public TypesFile Expansiontypes;
        public List<TypesFile> ModTypes;
        public string filename;

        public string DrJonesPath { get; set; }

        public DrJonesAdmins drJonesAdmisList { get; set; }
        public DRJonesVariables drJonesVariables { get; set; }
        public DrJonesvehicleParts drjonesvehicleparts { get; set; }
        public VehicleParts CurrentVehicleParts { get; set; }
        public DRJonesTraderConfig drjonestraderconfig { get; set; }
        public DrJonesObjects drjonesobjects { get; set; }

        public DRJonesCurrency CurrentCurrency { get; set; }
        public DrjonesFullTraderconfig CurrentTrader { get; set; }
        public TraderCats currentCat { get; set; }
        public TraderItems currentItem { get; set; }

        public bool action { get; private set; }
        private ContextMenuStrip contexMenu;
        public TraderCats treeviewcat;
        public TraderItems treeviewitem;

        public DRJonesTrader_Manager()
        {
            InitializeComponent();
            tabControl3.ItemSize = new Size(0, 1);
        }

        #region Form Load And Other shit
        private void Trader_Manager_Load(object sender, EventArgs e)
        {
            DrJonesPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\Trader";
            filename = currentproject.ProjectName;
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            drJonesAdmisList = new DrJonesAdmins(DrJonesPath + "\\TraderAdmins.txt");
            listBox5.DisplayMember = "Name";
            listBox5.ValueMember = "Value";
            listBox5.DataSource = drJonesAdmisList.Admins;

            drJonesVariables = new DRJonesVariables(DrJonesPath + "\\TraderVariables.txt");
            populateVariables();

            drjonesvehicleparts = new DrJonesvehicleParts(DrJonesPath + "\\TraderVehicleParts.txt");
            listBox6.DisplayMember = "Name";
            listBox6.ValueMember = "Value";
            listBox6.DataSource = drjonesvehicleparts.Vehicles;

            drjonesobjects = new DrJonesObjects(DrJonesPath + "\\TraderObjects.txt");

            drjonestraderconfig = new DRJonesTraderConfig(DrJonesPath + "\\TraderConfig.txt");
            drjonestraderconfig.SetupFullTraderList();

            listBox8.DisplayMember = "Name";
            listBox8.ValueMember = "Value";
            listBox8.DataSource = drjonestraderconfig.drjonesfullList;

            textBox2.Text = drjonestraderconfig.CurrencyConfig.m_Trader_CurrencyName;
            listBox10.DisplayMember = "Name";
            listBox10.ValueMember = "Value";
            listBox10.DataSource = drjonestraderconfig.CurrencyConfig.currencyList;

        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Process.Start(DrJonesPath);
        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            savefiles();

        }

        private void savefiles()
        {
            string message = "The folliwng files Were saved." + Environment.NewLine;
            int savedfiles = 0;
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (drJonesAdmisList.savefile(SaveTime))
            {
                message += Path.GetFileName(drJonesAdmisList.Filename) + Environment.NewLine;
            }
            if (drJonesVariables.SaveVariables(SaveTime))
            {
                message += Path.GetFileName(drJonesVariables.Filename) + Environment.NewLine;
                savedfiles++;
            }
            if (drjonesvehicleparts.saveVehicleParts(SaveTime))
            {
                message += Path.GetFileName(drjonesvehicleparts.Filename) + Environment.NewLine;
                savedfiles++;
            }
            if (drjonestraderconfig.saveTraderConfig(SaveTime))
            {
                message += Path.GetFileName(drjonestraderconfig.Filename) + " plus any Additions Files linked to from the main Config." + Environment.NewLine;
                savedfiles++;
            }
            if (savedfiles > 0)
                MessageBox.Show(message);
            else
                MessageBox.Show("no files were saved");
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            tabControl3.SelectedIndex = 0;
            if (tabControl3.SelectedIndex == 0)
                toolStripButton1.Checked = true;
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            tabControl3.SelectedIndex = 1;
            if (tabControl3.SelectedIndex == 1)
                toolStripButton3.Checked = true;
        }
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            tabControl3.SelectedIndex = 2;
            if (tabControl3.SelectedIndex == 2)
                toolStripButton4.Checked = true;
        }
        private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl3.SelectedIndex)
            {
                case 0:
                    toolStripButton3.Checked = false;
                    toolStripButton4.Checked = false;
                    break;
                case 1:
                    toolStripButton1.Checked = false;
                    toolStripButton4.Checked = false;
                    break;
                case 2:
                    toolStripButton1.Checked = false;
                    toolStripButton3.Checked = false;
                    break;
                default:
                    break;
            }
        }

        #endregion region Form Load And Other shit

        #region Admins
        private void darkButton15_Click(object sender, EventArgs e)
        {
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Enter ID of New Admin ", "Admins", "");
            if (UserAnswer == "") return;
            if(!drJonesAdmisList.AddnewAdmin(UserAnswer))
                MessageBox.Show(UserAnswer + " is already in the admin list....");
        }
        private void darkButton14_Click(object sender, EventArgs e)
        {
            string removeitem = listBox5.GetItemText(listBox5.SelectedItem);
            drJonesAdmisList.removeAdmin(removeitem);

        }
        #endregion Admins

        #region Variables
        public bool IsUser = true;
        private void populateVariables()
        {
            IsUser = false;
            BuySellTimerUpDown.Value = (decimal)drJonesVariables.BuySellTimer;
            StatUpdateTimerUpDown.Value = (decimal)drJonesVariables.StatUpdateTimer;
            FireBarrelUpdateTimerUpDown.Value = (decimal)drJonesVariables.FireBarrelUpdateTimer;
            ZombieCleanupTimerUpDown.Value = (decimal)drJonesVariables.ZombieCleanupTimer;
            SafezoneTimeoutUpDown.Value = (decimal)drJonesVariables.SafezoneTimeout;
            VehicleCleanupTimerUpDown.Value = (decimal)drJonesVariables.VehicleCleanupTimer;
            IsUser = true;
        }
        private void BuySellTimerUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (IsUser)
            {
                drJonesVariables.BuySellTimer = (float)BuySellTimerUpDown.Value;
                drJonesVariables.isDirty = true;
            }
        }
        private void StatUpdateTimerUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (IsUser)
            {
                drJonesVariables.StatUpdateTimer = (float)StatUpdateTimerUpDown.Value;
                drJonesVariables.isDirty = true;
            }
        }
        private void FireBarrelUpdateTimerUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (IsUser)
            {
                drJonesVariables.FireBarrelUpdateTimer = (float)FireBarrelUpdateTimerUpDown.Value;
                drJonesVariables.isDirty = true;
            }
        }
        private void ZombieCleanupTimerUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (IsUser)
            {
                drJonesVariables.ZombieCleanupTimer = (float)ZombieCleanupTimerUpDown.Value;
                drJonesVariables.isDirty = true;
            }
        }
        private void SafezoneTimeoutUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (IsUser)
            {
                drJonesVariables.SafezoneTimeout = (float)SafezoneTimeoutUpDown.Value;
                drJonesVariables.isDirty = true;
            }
        }
        private void VehicleCleanupTimerUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (IsUser)
            {
                drJonesVariables.StatUpdateTimer = (float)VehicleCleanupTimerUpDown.Value;
                drJonesVariables.isDirty = true;
            }
        }
        #endregion Variables

        #region vehicle parts
        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox6.SelectedItems.Count < 1) return;
            CurrentVehicleParts = listBox6.SelectedItem as VehicleParts;
            listBox7.DisplayMember = "Name";
            listBox7.ValueMember = "Value";
            listBox7.DataSource = CurrentVehicleParts.Parts;
        }
        private void darkButton17_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes();
            form.vanillatypes = vanillatypes;
            form.ModTypes = ModTypes;
            form.currentproject = currentproject;
             DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    VehicleParts vp = new VehicleParts();
                    vp.ClasssName = l;
                    vp.Parts = new BindingList<string>();
                    if (!drjonesvehicleparts.Vehicles.Any(x => x.ClasssName == l))
                    {
                        drjonesvehicleparts.Vehicles.Add(vp);
                        drjonesvehicleparts.isDirty = true;
                    }
                }
            }
        }
        private void darkButton16_Click(object sender, EventArgs e)
        {
            if (listBox6.SelectedItems.Count < 1) return;
            drjonesvehicleparts.Vehicles.Remove(CurrentVehicleParts);
            drjonesvehicleparts.isDirty = true;
            listBox6.SelectedIndex = 0;
        }
        private void darkButton19_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes();
            form.vanillatypes = vanillatypes;
            form.ModTypes = ModTypes;
            form.currentproject = currentproject;
            form.UseMultipleofSameItem = true;
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    CurrentVehicleParts.AddPart(l.ToLower());
                    drjonesvehicleparts.isDirty = true;
                }
            }
        }
        private void darkButton18_Click(object sender, EventArgs e)
        {
            if (listBox7.SelectedItems.Count < 1) return;
            string removeitem = listBox7.GetItemText(listBox7.SelectedItem);
            CurrentVehicleParts.Parts.Remove(removeitem);
            drjonesvehicleparts.isDirty = true;
        }
        #endregion vehicle parts

        #region trader currency
        private void darkButton13_Click(object sender, EventArgs e)
        {
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Enter ClassName of Currency ", "currency", "");
            drjonestraderconfig.addnewCurrency(UserAnswer);
            drjonestraderconfig.isDirty = true;
        }
        private void darkButton12_Click(object sender, EventArgs e)
        {
            string removeitem = listBox10.GetItemText(listBox10.SelectedItem);
            drjonestraderconfig.removecurrency(removeitem);
            drjonestraderconfig.isDirty = true;
            if (listBox10.Items.Count > 0)
                listBox10.SelectedIndex = 0;
            else
                numericUpDown1.Value = 0;
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            drjonestraderconfig.CurrencyConfig.m_Trader_CurrencyName = textBox2.Text;
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (CurrentCurrency != null)
            {
                CurrentCurrency.m_Trader_CurrencyValues = (int)numericUpDown1.Value;
                drjonestraderconfig.isDirty = true;
                Console.WriteLine(CurrentCurrency.m_Trader_CurrencyClassnames + " Value has been changed to " + CurrentCurrency.m_Trader_CurrencyValues.ToString());
            }
        }
        #endregion trader currency

        #region Trader
        private void listBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentCurrency = listBox10.SelectedItem as DRJonesCurrency;
            if(CurrentCurrency != null)
                numericUpDown1.Value = CurrentCurrency.m_Trader_CurrencyValues;

        }
        private void listBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox8.SelectedItems.Count < 1) return;
            groupBox13.Visible = false;
            CurrentTrader = listBox8.SelectedItem as DrjonesFullTraderconfig;
            textBox4.Text = CurrentTrader.m_traderpath;
            Poppulatetreeview();
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            CurrentTrader.m_traderpath = textBox4.Text;
        }
        private void Poppulatetreeview()
        {
            treeView1.Nodes.Clear();
            //Get items from Category 
            TreeNode tn = new TreeNode(CurrentTrader.Tradername);
            tn.Tag = "Parent";
            foreach (TraderCats cat in CurrentTrader.cats)
            {
                TreeNode ccn = new TreeNode(cat.CatName);
                ccn.Tag = cat;
                foreach (TraderItems item in cat.ItemList)
                {
                    TreeNode itemnode = new TreeNode(item.m_Trader_ItemsClassnames);
                    itemnode.Tag = item;
                    ccn.Nodes.Add(itemnode);
                }
                tn.Nodes.Add(ccn);
            }
            treeView1.Nodes.Add(tn);
            foreach (TreeNode tn1 in treeView1.Nodes)
            {
                tn1.Expand();
            }
        }
        public TreeNode currentnode;
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView1.SelectedNode = e.Node;
            currentnode = e.Node as TreeNode;
            if (e.Button == MouseButtons.Right)
            {
                contexMenu = new ContextMenuStrip();
                contexMenu.BackColor = Color.FromArgb(60, 63, 65);
                contexMenu.ForeColor = SystemColors.Control;
                contexMenu.ShowCheckMargin = false;
                contexMenu.ShowImageMargin = false;
                

                treeView1.SelectedNode = treeView1.GetNodeAt(e.X, e.Y);
                if (e.Node.Tag is TraderItems)
                {
                    contexMenu.Items.Add("Remove item from this trader");
                }
                else if (e.Node.Tag is TraderCats)
                {
                    treeviewcat = e.Node.Tag as TraderCats;
                    contexMenu.Items.Add("Add Item From Types");
                    contexMenu.Items.Add("Rename Category");
                    contexMenu.Items.Add("Remove Category");
                }
                else if (e.Node.Tag.ToString() == "Parent")
                {
                    contexMenu.Items.Add("Add new Category");
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
                if (currentnode.Tag is TraderItems)
                {
                    currentItem = currentnode.Tag as TraderItems;
                    groupBox13.Visible = true;
                    textBox3.Text = currentItem.m_Trader_ItemsClassnames;
                    numericUpDown2.Value = currentItem.m_Trader_ItemsSellValue;
                    numericUpDown3.Value = currentItem.m_Trader_ItemsBuyValue;
                    textBox1.Text = currentItem.m_Trader_ItemsQuantity;

                }
                else if (currentnode.Tag is TraderCats)
                {
                    groupBox13.Visible = false;
                    currentItem = null;
                }
                else if (currentnode.Tag is String)
                {
                    groupBox13.Visible = false;
                }
            }
            if(currentnode.Tag is TraderItems)
            {
                currentCat = currentnode.Parent.Tag as TraderCats;
            }
            if (currentnode.Tag is TraderCats)
                currentCat = currentnode.Tag as TraderCats;
        }
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem clickedItem = e.ClickedItem;
            contexMenu.Close();
            string text = clickedItem.Text;
            switch (text)
            {
                case "Remove item from this trader":
                    currentItem = currentnode.Tag as TraderItems;
                    Console.WriteLine(currentItem.m_Trader_ItemsClassnames + " removed from " + currentCat.CatName);
                    currentCat.ItemList.Remove(currentItem);
                    var savedExpansionState = treeView1.Nodes.GetExpansionState();
                    treeView1.BeginUpdate();
                    Poppulatetreeview();
                    treeView1.Nodes.SetExpansionState(savedExpansionState);
                    treeView1.EndUpdate();
                    drjonestraderconfig.isDirty = true;
                    break;
                case "Add Item From Types":
                    AddItemfromTypes form = new AddItemfromTypes();
                    form.vanillatypes = vanillatypes;
                    form.ModTypes = ModTypes;
                    form.currentproject = currentproject;
                    DialogResult result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        List<string> addedtypes = form.addedtypes.ToList();
                        foreach (string l in addedtypes)
                        {
                            TraderItems NewContainer = new TraderItems();
                            NewContainer.m_Trader_ItemsClassnames = l;
                            NewContainer.m_Trader_ItemsQuantity = "*";
                            NewContainer.m_Trader_ItemsSellValue = 0;
                            NewContainer.m_Trader_ItemsBuyValue = 0;
                            if (!currentCat.ItemList.Any(x => x.m_Trader_ItemsClassnames == NewContainer.m_Trader_ItemsClassnames))
                            {
                                currentCat.ItemList.Add(NewContainer);
                                Console.WriteLine(NewContainer.m_Trader_ItemsClassnames + " Added to " + currentCat.CatName);
                            }
                        }
                        drjonestraderconfig.isDirty = true;
                    }
                    savedExpansionState = treeView1.Nodes.GetExpansionState();
                    treeView1.BeginUpdate();
                    Poppulatetreeview();
                    treeView1.Nodes.SetExpansionState(savedExpansionState);
                    currentnode.Expand();
                    treeView1.EndUpdate();
                    break;
                case "Add new Category":
                    string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Enter Name of New Category ", "Categories", "");
                    CurrentTrader.AddNewCat(UserAnswer);
                    drjonestraderconfig.isDirty = true;
                    savedExpansionState = treeView1.Nodes.GetExpansionState();
                    treeView1.BeginUpdate();
                    Poppulatetreeview();
                    treeView1.Nodes.SetExpansionState(savedExpansionState);
                    treeView1.EndUpdate();
                    Console.WriteLine("new Category added...." + UserAnswer);
                    break;
                case "Rename Category":
                    UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Enter New Name of Category ", "Categories", "");
                    Console.WriteLine(currentCat.CatName + " has been changed to " + UserAnswer);
                    currentCat.CatName = UserAnswer;
                    drjonestraderconfig.isDirty = true;
                    savedExpansionState = treeView1.Nodes.GetExpansionState();
                    treeView1.BeginUpdate();
                    Poppulatetreeview();
                    treeView1.Nodes.SetExpansionState(savedExpansionState);
                    treeView1.EndUpdate();
                    break;
                case "Remove Category":
                    CurrentTrader.removecatfromtrader(currentCat);
                    drjonestraderconfig.isDirty = true;
                    savedExpansionState = treeView1.Nodes.GetExpansionState();
                    treeView1.BeginUpdate();
                    Poppulatetreeview();
                    treeView1.Nodes.SetExpansionState(savedExpansionState);
                    currentnode.Expand();
                    treeView1.EndUpdate();
                    break;
            }
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            currentItem.m_Trader_ItemsSellValue = (int)numericUpDown2.Value;
            drjonestraderconfig.isDirty = true;
        }
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            currentItem.m_Trader_ItemsBuyValue = (int)numericUpDown3.Value;
            drjonestraderconfig.isDirty = true;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            currentItem.m_Trader_ItemsQuantity = textBox1.Text;
            drjonestraderconfig.isDirty = true;
        }
        private void darkButton21_Click(object sender, EventArgs e)
        {
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Enter Name of New Trader ", "Traders", "");
            drjonestraderconfig.AddNewTrader(UserAnswer);
            listBox8.SelectedIndex = listBox8.Items.Count - 1;
            Console.WriteLine("New Trader Added.... " + UserAnswer);
            drjonestraderconfig.isDirty = true;
        }
        private void darkButton20_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This Will Remove The All reference to this Trader including all categories and items, Are you sure you want to do this?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string removeitem = listBox8.GetItemText(listBox8.SelectedItem);

               Console.WriteLine(removeitem + " Trader Removed....\n\nThe Following Categories and Items Were Removed:-");
                foreach (TraderCats item in CurrentTrader.cats)
                {
                    Console.WriteLine("\t" + item.CatName + " Category Removed....");
                    foreach (TraderItems l in item.ItemList)
                    {
                        Console.WriteLine("\t\t" + l.m_Trader_ItemsClassnames);
                    }
                }
                drjonestraderconfig.removetrader(removeitem);
                MessageBox.Show(removeitem + " has now been removed along wtih all categories and items", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                listBox8.SelectedIndex = 0;
                drjonestraderconfig.isDirty = true;
            }
        }

        #endregion Trader
        public string ReplaceInvalidChars(string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }
        private void convertToExpansionMarketToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            //set up market items list first.
            List<marketItem> checklist = new List<marketItem>();
            MarketCategories MarketCats = new MarketCategories();
            TradersList traders = new TradersList();
            TraderModelMapping maps = new TraderModelMapping();
            TraderZones zone = new TraderZones();

            //set up world zone
            zone.NewTraderZone("WORLD", currentproject.MapSize, false);

            //Setup currency Market Category
            Categories newccat = new Categories();
            newccat.DisplayName = "#STR_EXPANSION_MARKET_CATEGORY_EXCHANGE";
            newccat.Filename = "EXCHANGE.json";
            newccat.IsExchange = 1;
            foreach (DRJonesCurrency drjc in drjonestraderconfig.CurrencyConfig.currencyList)
            {
                marketItem newitem = new marketItem()
                { 
                    ClassName = drjc.m_Trader_CurrencyClassnames,
                    MaxPriceThreshold = drjc.m_Trader_CurrencyValues,
                    MinPriceThreshold = drjc.m_Trader_CurrencyValues,
                    MaxStockThreshold = 1, MinStockThreshold = 1 
                };
                checklist.Add(newitem);
                newccat.Items.Add(newitem);
            }
            MarketCats.CatList.Add(newccat);

            foreach (DRJonesCategories cat in drjonestraderconfig.m_categories)
            {
                if(cat.m_Trader_Categorys == "Keys&Supplies")
                {
                    string stop = "";
                }
                //check if cat has been created allready
                string Displayname = ReplaceInvalidChars(cat.m_Trader_Categorys);
                Displayname = Displayname.Replace(" ", "_");
                Displayname = Displayname.ToUpper();
                Categories newcat = MarketCats.GetCatFromDisplayName(Displayname);
                if (newcat == null)
                {
                    newcat = new Categories();
                    newcat.DisplayName = Displayname;
                    newcat.Filename = ReplaceInvalidChars(newcat.DisplayName);
                    newcat.Filename = newcat.Filename.Replace(" ", "_");
                    newcat.Filename = newcat.Filename.ToUpper();
                }
                List<DrjonesItems> dritems = drjonestraderconfig.getItems(cat.m_Trader_CategoryID);
                foreach (DrjonesItems item in dritems)
                {
                    if (item.m_Trader_ItemsClassnames.ToLower() == "carbattery")
                    {
                        string stop = "";
                    }
                    //create new item with default stock values of 1/1
                    marketItem newitem = new marketItem();
                    newitem.ClassName = item.m_Trader_ItemsClassnames.ToLower();
                    if(drjonesvehicleparts.Vehicles.Any(x => x.ClasssName.ToLower() == newitem.ClassName))
                    {
                        VehicleParts v = drjonesvehicleparts.Vehicles.FirstOrDefault(x => x.ClasssName.ToLower() == newitem.ClassName);
                        foreach (string Part in v.Parts)
                        {
                            newitem.SpawnAttachments.Add(Part.ToLower());
                        }
                    }
                    if (item.m_Trader_ItemsBuyValue == -1)
                        newitem.MaxPriceThreshold = (int)((float)item.m_Trader_ItemsSellValue);
                    else
                        newitem.MaxPriceThreshold = item.m_Trader_ItemsBuyValue;
                    int n;
                    if(int.TryParse(item.m_Trader_ItemsQuantity, out n))
                    {
                        newitem.MaxPriceThreshold = newitem.MaxPriceThreshold / n;
                    }
                    newitem.MinPriceThreshold = newitem.MaxPriceThreshold;
                    newitem.MaxStockThreshold = 1;
                    newitem.MinStockThreshold = 1;
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
                
                //if (newcat.Items.Count > 0)
                MarketCats.CatList.Add(newcat);
            }
            if (sb.Length != 0)
                Console.Write(sb.ToString());

            


            Traders exchangetrader = new Traders("Exchange");
            exchangetrader.DisplayName = "#STR_EXPANSION_MARKET_TRADER_EXCHANGE";
            exchangetrader.Filename = "EXCHANGE";
            foreach (DRJonesCurrency drjc in drjonestraderconfig.CurrencyConfig.currencyList)
            {
                exchangetrader.Currencies.Add(drjc.m_Trader_CurrencyClassnames);
            }
            Categories mcat = MarketCats.GetCatFromDisplayName("EXCHANGE");
            foreach(marketItem mitem in mcat.Items)
            {
                TradersItem ti = new TradersItem();
                ti.ClassName = mitem.ClassName.ToLower();
                ti.buysell = canBuyCansell.CanBuyAndsell;
                ti.CatName = "EXCHANGE";
                exchangetrader.ListItems.Add(ti);
            }
            traders.Traderlist.Add(exchangetrader);



            for (int i = 0; i < drjonestraderconfig.drjonesfullList.Count; i++ )
            {
                DrjonesFullTraderconfig trader = drjonestraderconfig.drjonesfullList[i];
                Traders newtrader = new Traders(trader.Tradername.Replace(" ", "_").ToUpper());
                newtrader.Filename = trader.Tradername.Replace(" ", "_").ToUpper();
                foreach (DRJonesCurrency drjc in drjonestraderconfig.CurrencyConfig.currencyList)
                {
                    newtrader.Currencies.Add(drjc.m_Trader_CurrencyClassnames);
                }
                foreach (TraderCats tcats in trader.cats)
                {
                    foreach(TraderItems item in tcats.ItemList)
                    {
                        marketItem mitem = MarketCats.getitemfromcategory(item.m_Trader_ItemsClassnames.ToLower());
                        TradersItem ti = new TradersItem();
                        ti.ClassName = item.m_Trader_ItemsClassnames.ToLower();
                        if (item.m_Trader_ItemsSellValue == -1)
                            ti.buysell = canBuyCansell.CanOnlyBuy;
                        else if (item.m_Trader_ItemsBuyValue == -1)
                            ti.buysell = canBuyCansell.CanOnlySell;
                        else
                            ti.buysell = canBuyCansell.CanBuyAndsell;

                        if(mitem.SpawnAttachments.Count != 0)
                        {
                            ti.HasAttachemnts = true;
                        }

                        ti.CatName = ReplaceInvalidChars(tcats.CatName);
                        ti.CatName = ti.CatName.Replace(" ", "_");
                        ti.CatName = ti.CatName.ToUpper();
                        if (!newtrader.ListItems.Any(x => x.ClassName.ToLower() == ti.ClassName.ToLower()))
                        {
                            newtrader.ListItems.Add(ti);
                            newtrader.isDirty = true;
                        }
                        else
                        {
                            TradersItem eti = newtrader.ListItems.First(x => x.ClassName.ToLower() == ti.ClassName.ToLower());
                            eti.buysell = ti.buysell;

                        }
                    }
                }
                traders.Traderlist.Add(newtrader);

                DrJonesObject drjonesobject = drjonesobjects.DrJonesobjectslist[i];
                Tradermap newmap = new Tradermap();
                if(drjonesobject.DrJonesNPCClassname.Contains("Survivor"))
                {
                    newmap.NPCName = "ExpansionTrader" + drjonesobject.DrJonesNPCClassname.Split('_')[1];
                }
                else
                {
                    newmap.NPCName = drjonesobject.DrJonesNPCClassname;
                }
                newmap.NPCTrade = newtrader.Filename;
                newmap.position = new Vec3(drjonesobject.DrJonesNPCPosition);
                newmap.roattions = new Vec3(drjonesobject.DrJonesNPCOrientaion);
                newmap.Attachments = new BindingList<string>(drjonesobject.DrJonesAttchments);
                maps.maps.Add(newmap);
            }

            string ExpansionPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\drJonestoExpansionMarket";
            if(!Directory.Exists(ExpansionPath))
            {
                Directory.CreateDirectory(ExpansionPath);
            }
            foreach (Traders trader in traders.Traderlist)
            {
                Directory.CreateDirectory(ExpansionPath + "\\Traders");
                string traderFilename = ExpansionPath + "\\Traders\\" + trader.Filename + ".json";
                var options = new JsonSerializerOptions();
                options.WriteIndented = true;
                options.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                trader.isDirty = false;
                trader.ConvertToDict(MarketCats);
                string jsonString = JsonSerializer.Serialize(trader, options);
                File.WriteAllText(traderFilename, jsonString);
            }
            foreach (Categories cat in MarketCats.CatList)
            {
                Directory.CreateDirectory(ExpansionPath + "\\Market");
                string catFilename = ExpansionPath + "\\Market\\" + cat.Filename + ".json";
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
                zones.isDirty = false;
                zones.ConvertlisttoDict();
                string jsonString = JsonSerializer.Serialize(zones, options);
                File.WriteAllText(ZoneFilename, jsonString);
            }

            maps.TradermapsPath = ExpansionPath;
            foreach (Tradermap map in maps.maps)
            {
                map.Filename = ExpansionPath + "\\TraderMaps.map";
            }
            maps.savefiles();
            MessageBox.Show("Convertion Complete...\nfiles stored in folder called\nExpansionMod_Market_Convert_from_TraderPlus\nWithin the project profiles folder.");
        }

        private bool CheckIfInTypes(DrjonesItems item)
        {
            if (vanillatypes.types.type.Any(x => x.name.ToLower() == item.m_Trader_ItemsClassnames.ToLower()))
                return true;
            if (Expansiontypes != null && Expansiontypes.types.type.Any(x => x.name.ToLower() == item.m_Trader_ItemsClassnames.ToLower()))
                return true;
            foreach (TypesFile tf in ModTypes)
            {
                if (tf.types.type.Any(x => x.name.ToLower() == item.m_Trader_ItemsClassnames.ToLower()))
                    return true;
            }

            return false;
        }
        private void exportClassnameAndBuyPriceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            foreach (DRJonesCategories cat in drjonestraderconfig.m_categories)
            {
                List<DrjonesItems> dritems = drjonestraderconfig.getItems(cat.m_Trader_CategoryID);
                foreach (DrjonesItems item in dritems)
                {
                    if (list.Any(x => x.Split(',')[0] == item.m_Trader_ItemsClassnames.ToLower())) { continue; }
                    if(item.m_Trader_ItemsBuyValue == -1)
                        list.Add(item.m_Trader_ItemsClassnames.ToLower() + "," + item.m_Trader_ItemsSellValue.ToString());
                    else
                        list.Add(item.m_Trader_ItemsClassnames.ToLower() + "," + item.m_Trader_ItemsBuyValue.ToString());
                }
            }
            File.WriteAllText("PriceExport.txt", String.Join("\n", list.ToArray()));
        }
        private void DRJonesTrader_Manager_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
