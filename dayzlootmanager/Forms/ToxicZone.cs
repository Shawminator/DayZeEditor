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
    public partial class ToxicZone : DarkForm
    {
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

        private string Projectname;
        private string ToxicZonePath;
        public Project currentproject { get; internal set; }
        public cfgEffectArea ToxicZonesettings { get; private set; }
        public Areas CurrentToxicArea { get; private set; }
        public Position currentsafeposition { get; private set; }

        private bool useraction;

        public ToxicZone()
        {
            InitializeComponent();
        }
        private void ToxicZone_Load(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Projectname = currentproject.ProjectName;
            ToxicZonePath = currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\cfgEffectArea.json";
            ToxicZonesettings = JsonSerializer.Deserialize<cfgEffectArea>(File.ReadAllText(ToxicZonePath));
            ToxicZonesettings.isDirty = false;
            ToxicZonesettings.Filename = ToxicZonePath;
            ToxicZonesettings.convertpositionstolist();


            loadToxicZonesettings();
        }
        private void loadToxicZonesettings()
        {
            useraction = false;

            AreasLB.DisplayMember = "DisplayName";
            AreasLB.ValueMember = "Value";
            AreasLB.DataSource = ToxicZonesettings.Areas;

            SafePositionsLB.DisplayMember = "DisplayName";
            SafePositionsLB.ValueMember = "Value";
            SafePositionsLB.DataSource = ToxicZonesettings._positions;

            useraction = true;
        }

        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            savefiles();
        }
        public void savefiles()
        {
            if (ToxicZonesettings.isDirty)
            {
                ToxicZonesettings.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(ToxicZonesettings, options);
                File.WriteAllText(ToxicZonesettings.Filename, jsonString);
                MessageBox.Show(Path.GetFileName(ToxicZonesettings.Filename) + " Saved....");
            }
        }

        private void SafePositionsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SafePositionsLB.SelectedItems.Count < 1) return;
            currentsafeposition = SafePositionsLB.SelectedItem as Position;
            useraction = false;
            SafePositionXNUD.Value = currentsafeposition.X;
            SafePositionZNUD.Value = currentsafeposition.Z;

            useraction = true;
        }
        private void AreasLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AreasLB.SelectedItems.Count < 1) return;
            CurrentToxicArea = AreasLB.SelectedItem as Areas;
            useraction = false;

            AreaNameTB.Text = CurrentToxicArea.AreaName;
            TypeTB.Text = CurrentToxicArea.Type;
            TriggerTypeTB.Text = CurrentToxicArea.TriggerType;
            PosXNUD.Value = CurrentToxicArea.Data.Pos[0];
            posYNUD.Value = CurrentToxicArea.Data.Pos[1];
            posZNUD.Value = CurrentToxicArea.Data.Pos[2];
            RadiusNUD.Value = CurrentToxicArea.Data.Radius;
            PosHeightNUD.Value = CurrentToxicArea.Data.PosHeight;
            NegHeightNUD.Value = CurrentToxicArea.Data.NegHeight;
            InnerRingCountNUD.Value = CurrentToxicArea.Data.InnerRingCount;
            InnerPartDistNUD.Value = CurrentToxicArea.Data.InnerPartDist;
            OuterRingToggleCB.Checked = CurrentToxicArea.Data.OuterRingToggle == 1? true : false;
            OuterPartDistNUD.Value = CurrentToxicArea.Data.OuterPartDist;
            OuterOffsetNUD.Value = CurrentToxicArea.Data.OuterOffset;
            VerticalLayersNUD.Value = CurrentToxicArea.Data.VerticalLayers;
            VerticalOffsetNUD.Value = CurrentToxicArea.Data.VerticalOffset;
            ParticleNameTB.Text = CurrentToxicArea.Data.ParticleName;
            AroundPartNameTB.Text = CurrentToxicArea.PlayerData.AroundPartName;
            TinyPartNameTB.Text = CurrentToxicArea.PlayerData.TinyPartName;
            PPERequesterTypeTB.Text = CurrentToxicArea.PlayerData.PPERequesterType;

            useraction = true;
        }

        private void darkButton5_Click(object sender, EventArgs e)
        {
            Data newdata = new Data()
            {
                Pos = new int[] {0,0,0},
                Radius = 0,
                PosHeight = 0,
                NegHeight = 0,
                InnerRingCount = 0,
                InnerPartDist = 0,
                OuterRingToggle = 1,
                OuterPartDist = 0,
                OuterOffset = 0,
                VerticalLayers = 0,
                VerticalOffset = 0,
                ParticleName = "graphics/particles/contaminated_area_gas_bigass"
            };
            PlayerData newplayerData = new PlayerData()
            {
                AroundPartName = "graphics/particles/contaminated_area_gas_around",
                TinyPartName = "graphics/particles/contaminated_area_gas_around_tiny",
                PPERequesterType = "PPERequester_ContaminatedAreaTint"
            };
            ToxicZonesettings.Areas.Add(new Areas()
            {
                AreaName = "New-Toxic-Area",
                Type = "ContaminatedArea_Static",
                TriggerType = "ContaminatedTrigger",
                Data = newdata,
                PlayerData = newplayerData
            }
            );
            ToxicZonesettings.isDirty = true;
            AreasLB.SelectedIndex = -1;
            AreasLB.SelectedIndex = AreasLB.Items.Count - 1;
        }
        private void darkButton6_Click(object sender, EventArgs e)
        {
            if (AreasLB.SelectedItems.Count < 1) return;
            int index = AreasLB.SelectedIndex;
            ToxicZonesettings.Areas.Remove(CurrentToxicArea);
            ToxicZonesettings.isDirty = true;
            AreasLB.SelectedIndex = -1;
            if (index - 1 == -1)
            {
                if (AreasLB.Items.Count > 0)
                    AreasLB.SelectedIndex = 0;
            }
            else
            {
                AreasLB.SelectedIndex = index - 1;
            }
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            ToxicZonesettings._positions.Add(new Position()
            {
                Name = "0,0",
                X = 0,
                Z = 0
            }
            );
            ToxicZonesettings.isDirty = true;
            SafePositionsLB.SelectedIndex = -1;
            SafePositionsLB.SelectedIndex = SafePositionsLB.Items.Count - 1;
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
            if (SafePositionsLB.SelectedItems.Count < 1) return;
            int index = SafePositionsLB.SelectedIndex;
            ToxicZonesettings._positions.Remove(currentsafeposition);
            ToxicZonesettings.isDirty = true;
            SafePositionsLB.SelectedIndex = -1;
            if (index - 1 == -1)
            {
                if (SafePositionsLB.Items.Count > 0)
                    SafePositionsLB.SelectedIndex = 0;
            }
            else
            {
                SafePositionsLB.SelectedIndex = index - 1;
            }
        }

        private void AreaNameTB_TextChanged(object sender, EventArgs e)
        {
            if(!useraction) { return; }
            CurrentToxicArea.AreaName = AreaNameTB.Text;
            ToxicZonesettings.isDirty = true;
        }
        private void TypeTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            CurrentToxicArea.Type = TypeTB.Text;
            ToxicZonesettings.isDirty = true;
        }
        private void TriggerTypeTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            CurrentToxicArea.TriggerType = TriggerTypeTB.Text;
            ToxicZonesettings.isDirty = true;
        }
        private void PosNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            CurrentToxicArea.Data.Pos = new int[] { (int)PosXNUD.Value, (int)posYNUD.Value, (int)posZNUD.Value };
            ToxicZonesettings.isDirty = true;
        }
        private void AreaNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            NumericUpDown nud = sender as NumericUpDown;
            CurrentToxicArea.Data.SetIntValue(nud.Name.Substring(0, nud.Name.Length - 3), (int)nud.Value);
            ToxicZonesettings.isDirty = true;
        }
        private void ParticleNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            CurrentToxicArea.Data.ParticleName = ParticleNameTB.Text;
            ToxicZonesettings.isDirty = true;
        }
        private void AroundPartNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            CurrentToxicArea.PlayerData.AroundPartName = AroundPartNameTB.Text;
            ToxicZonesettings.isDirty = true;
        }
        private void TinyPartNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            CurrentToxicArea.PlayerData.TinyPartName = TinyPartNameTB.Text;
            ToxicZonesettings.isDirty = true;
        }
        private void PPERequesterTypeTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            CurrentToxicArea.PlayerData.PPERequesterType = PPERequesterTypeTB.Text;
            ToxicZonesettings.isDirty = true;
        }
        private void SafePositionNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) { return; }
            currentsafeposition.X = (int)SafePositionXNUD.Value;
            currentsafeposition.Z = (int)SafePositionZNUD.Value;
            currentsafeposition.Name = currentsafeposition.X.ToString() + "," + currentsafeposition.Z.ToString();
            SafePositionsLB.Invalidate();
            ToxicZonesettings.isDirty = true;
        }
    }
}
