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
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZeEditor
{
    public partial class HelicrashMissionsManager : DarkForm
    {
        public Project currentproject { get; set; }
        public TypesFile vanillatypes;
        public List<TypesFile> ModTypes;

        public string HelicrashMissionsPath { get; private set; }
        public Helicrash Helicrash { get; private set; }

        public string Projectname;
        private bool _useraction = false;
        private Weaponloottable currentWeaponloottable;
        private Crashpoint currentCrashpoint;

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
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    toolStripButton3.Checked = false;
                    toolStripButton7.Checked = false;
                    break;
                case 1:
                    toolStripButton8.Checked = false;
                    toolStripButton7.Checked = false;
                    break;
                case 2:
                    toolStripButton8.Checked = false;
                    toolStripButton3.Checked = false;
                    break;
                default:
                    break;
            }
        }
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            if (tabControl1.SelectedIndex == 0)
                toolStripButton8.Checked = true;
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            if (tabControl1.SelectedIndex == 1)
                toolStripButton3.Checked = true;
        }
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
            if (tabControl1.SelectedIndex == 2)
                toolStripButton7.Checked = true;
        }
 
        public HelicrashMissionsManager()
        {
            InitializeComponent();
        }
        private void HelicrashMissionsManager_Load(object sender, EventArgs e)
        {
            tabControl1.ItemSize = new Size(0, 1);
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;


            Projectname = currentproject.ProjectName;
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            HelicrashMissionsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\HeliCrashMissions\\Helicrash.json";
            Helicrash = JsonSerializer.Deserialize<Helicrash>(File.ReadAllText(HelicrashMissionsPath));
            Helicrash.isDirty = false;
            Helicrash.FullFilename = HelicrashMissionsPath;

            LoadHeliCrash();

        }

        private void LoadHeliCrash()
        {
            var sortedListInstance = new BindingList<Weaponloottable>(Helicrash.WeaponLootTables.OrderBy(x => x.WeaponName).ToList());
            Helicrash.WeaponLootTables = sortedListInstance;

            CrashpointLB.DisplayMember = "DisplayName";
            CrashpointLB.ValueMember = "Value";
            CrashpointLB.DataSource = Helicrash.CrashPoints;

            Loot_HelicrashLB.DisplayMember = "DisplayName";
            Loot_HelicrashLB.ValueMember = "Value";
            Loot_HelicrashLB.DataSource = Helicrash.Loot_Helicrash;

            WeaponLootTablesLB.DisplayMember = "DisplayName";
            WeaponLootTablesLB.ValueMember = "Value";
            WeaponLootTablesLB.DataSource = Helicrash.WeaponLootTables;
        }

        private void WeaponLootTablesLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (WeaponLootTablesLB.SelectedItems.Count < 1) return;
            currentWeaponloottable = WeaponLootTablesLB.SelectedItem as Weaponloottable;
            useraction = false;
            SetweaponInfo();
            useraction = true;
        }

        private void SetweaponInfo()
        {
            weaponTB.Text = currentWeaponloottable.WeaponName;
            opticTB.Text = currentWeaponloottable.Sight;

            attachmentsLB.DisplayMember = "DisplayName";
            attachmentsLB.ValueMember = "Value";
            attachmentsLB.DataSource = currentWeaponloottable.Attachments;
        }

        private void CrashpointLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(CrashpointLB.SelectedItems.Count == 0) { return; }
            currentCrashpoint = CrashpointLB.SelectedItem as Crashpoint;
            useraction = false;
            SetupcrashPoint();
            useraction = true;
        }
        private void SetupcrashPoint()
        {
            
        }
    }
}
