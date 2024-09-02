using DayZeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows.Forms;

namespace DayZeEditor
{
    public partial class ExpansionQuestAISpawnControl : UserControl
    {
        [Browsable(true)]
        public event PropertyChangedEventHandler IsDirtyChanged;
        private void NotifyPropertyChanged(string info)
        {
            IsDirtyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            ListBox lb = sender as ListBox;
            var CurrentItemWidth = (int)this.CreateGraphics().MeasureString(lb.Items[lb.Items.Count - 1].ToString(), lb.Font, TextRenderer.MeasureText(lb.Items[lb.Items.Count - 1].ToString(), new Font("Arial", 20.0F))).Width;
            lb.HorizontalExtent = CurrentItemWidth + 5;
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

        private bool _isdirty;
        public bool isDirty
        {
            get
            {
                return _isdirty;
            }
            set
            {
                if (_isdirty != value)
                {
                    _isdirty = value;
                    NotifyPropertyChanged("isdirty");
                }
            }
        }

        public void setdirty(bool v)
        {
            _isdirty = v;
        }

        public Vec3 CurrentWapypoint { get; private set; }
        private bool useraction { get; set; }

        private ExpansionAIPatrol _currentAISpawn { get; set; }
        public ExpansionAIPatrol currentAISpawn
        {
            get
            {
                return _currentAISpawn;
            }
            set
            {
                _currentAISpawn = value;
                updatevalues();
            }
        }

        public IList<string> Factions { get; set; }
        public IList<AILoadouts> LoadoutList { get; set; }


        public ExpansionQuestAISpawnControl()
        {
            InitializeComponent();

        }
        public void setuplists()
        {
            StaticPatrolFactionCB.DataSource = new BindingList<string>(Factions);
            StaticPatrolLoadoutsCB.DataSource = new BindingList<AILoadouts>(LoadoutList);
        }
        private void updatevalues()
        {
            if (_currentAISpawn == null) return;

            useraction = false;

            StaticPatrolNameTB.Text = _currentAISpawn.Name;
            StaticPatrolPersistCB.Checked = _currentAISpawn.Persist == 1 ? true : false;
            StaticPatrolFactionCB.SelectedIndex = StaticPatrolFactionCB.FindStringExact(_currentAISpawn.Faction);
            StaticPatrolNumberOfAINUD.Value = _currentAISpawn.NumberOfAI;
            StaticPatrolBehaviorCB.SelectedIndex = StaticPatrolBehaviorCB.FindStringExact(_currentAISpawn.Behaviour);
            StaticPatrolSpeedCB.SelectedIndex = StaticPatrolSpeedCB.FindStringExact(_currentAISpawn.Speed);
            StaticPatrolUnderThreatSpeedCB.SelectedIndex = StaticPatrolUnderThreatSpeedCB.FindStringExact(_currentAISpawn.UnderThreatSpeed);
            StaticPatrolRespawnTimeNUD.Value = _currentAISpawn.RespawnTime;
            StaticPatrolDespawnTimeNUD.Value = _currentAISpawn.DespawnTime;
            StaticPatrolMinDistRadiusNUD.Value = _currentAISpawn.MinDistRadius;
            StaticPatrolMaxDistRadiusNUD.Value = _currentAISpawn.MaxDistRadius;
            StaticPatrolDespawnRadiusNUD.Value = _currentAISpawn.DespawnRadius;
            StaticPatrolAccuracyMinNUD.Value = _currentAISpawn.AccuracyMin;
            StaticPatrolAccuracyMaxNUD.Value = _currentAISpawn.AccuracyMax;
            StaticPatrolDamageReceivedMultiplierNUD.Value = _currentAISpawn.DamageReceivedMultiplier;
            StaticPatrolThreatDistanceLimitNUD.Value = _currentAISpawn.ThreatDistanceLimit;
            StaticPatrolSniperProneDistanceThresholdNUD.Value = _currentAISpawn.SniperProneDistanceThreshold;
            StaticPatrolDamageMultiplierNUD.Value = _currentAISpawn.DamageMultiplier;
            StaticPatrolChanceCB.Value = _currentAISpawn.Chance;
            StaticPatrolCanBeLotedCB.Checked = _currentAISpawn.CanBeLooted == 1 ? true : false;
            StaticPatrolLoadoutsCB.SelectedIndex = StaticPatrolLoadoutsCB.FindStringExact(_currentAISpawn.Loadout);
            StaticPatrolMinSpreadRadiusNUD.Value = _currentAISpawn.MinSpreadRadius;
            StaticPatrolMaxSpreadRadiusNUD.Value = _currentAISpawn.MaxSpreadRadius;
            StaticPatrolFormationCB.SelectedIndex = StaticPatrolFormationCB.FindStringExact(_currentAISpawn.Formation);
            StaticPatrolFormationLoosenessNUD.Value = _currentAISpawn.FormationLooseness;
            StaticPatrolWaypointInterpolationCB.SelectedIndex = StaticPatrolWaypointInterpolationCB.FindStringExact(_currentAISpawn.WaypointInterpolation);
            StaticPatrolUseRandomWaypointAsStartPointCB.Checked = _currentAISpawn.UseRandomWaypointAsStartPoint == 1 ? true : false;
            StaticPatrolNoiseInvestigationDistanceLimitNUD.Value = _currentAISpawn.NoiseInvestigationDistanceLimit;
            StaticPatrolCanBeTriggeredByAICB.Checked = _currentAISpawn.CanBeTriggeredByAI == 1 ? true : false;

            int StaticPatrolUnlimitedReloadBitmask = _currentAISpawn.UnlimitedReload;
            if (StaticPatrolUnlimitedReloadBitmask == 1)
                StaticPatrolUnlimitedReloadBitmask = 30;
            StaticPatrolURAnimalsCB.Checked = ((StaticPatrolUnlimitedReloadBitmask & 2) != 0) ? true : false;
            StaticPatrolURInfectedCB.Checked = ((StaticPatrolUnlimitedReloadBitmask & 4) != 0) ? true : false;
            StaticPatrolURPlayersCB.Checked = ((StaticPatrolUnlimitedReloadBitmask & 8) != 0) ? true : false;
            StaticPatrolURVehiclesCB.Checked = ((StaticPatrolUnlimitedReloadBitmask & 16) != 0) ? true : false;

            StaticPatrolUnitsLB.DisplayMember = "DisplayName";
            StaticPatrolUnitsLB.ValueMember = "Value";
            StaticPatrolUnitsLB.DataSource = _currentAISpawn.Units;

            StaticPatrolWayPointsLB.DisplayMember = "DisplayName";
            StaticPatrolWayPointsLB.ValueMember = "Value";
            StaticPatrolWayPointsLB.DataSource = _currentAISpawn._waypoints;

            if (_currentAISpawn._waypoints.Count == 0)
            {
                StaticPatrolWaypointPOSXNUD.Visible = false;
                StaticPatrolWaypointPOSYNUD.Visible = false;
                StaticPatrolWaypointPOSZNUD.Visible = false;
            }
            else
            {
                StaticPatrolWaypointPOSXNUD.Visible = true;
                StaticPatrolWaypointPOSYNUD.Visible = true;
                StaticPatrolWaypointPOSZNUD.Visible = true;
            }

            useraction = true;
        }
        private void StaticPatrolNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.Name = StaticPatrolNameTB.Text;
            isDirty = true;
        }

        private void StaticPatrolFactionCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.Faction = StaticPatrolFactionCB.GetItemText(StaticPatrolFactionCB.SelectedItem);
            isDirty = true;
        }

        private void StaticPatrolFormationCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.Formation = StaticPatrolFormationCB.GetItemText(StaticPatrolFormationCB.SelectedItem);
            isDirty = true;
        }

        private void StaticPatrolFormationLoosenessNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.FormationLooseness = (int)StaticPatrolFormationLoosenessNUD.Value;
            isDirty = true;
        }

        private void StaticPatrolPersistCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.Persist = StaticPatrolPersistCB.Checked == true ? 1 : 0;
            isDirty = true;
        }

        private void StaticPatrolLoadoutsCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.Loadout = StaticPatrolLoadoutsCB.GetItemText(StaticPatrolLoadoutsCB.SelectedItem);
            isDirty = true;
        }

        private void StaticPatrolNumberOfAINUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.NumberOfAI = (int)StaticPatrolNumberOfAINUD.Value;
            isDirty = true;
        }

        private void StaticPatrolBehaviorCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.Behaviour = StaticPatrolBehaviorCB.GetItemText(StaticPatrolBehaviorCB.SelectedItem);
            isDirty = true;
        }

        private void StaticPatrolSpeedCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.Speed = StaticPatrolSpeedCB.GetItemText(StaticPatrolSpeedCB.SelectedItem);
            isDirty = true;
        }

        private void StaticPatrolUnderThreatSpeedCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.UnderThreatSpeed = StaticPatrolUnderThreatSpeedCB.GetItemText(StaticPatrolUnderThreatSpeedCB.SelectedItem);
            isDirty = true;
        }

        private void StaticPatrolAccuracyMinNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.AccuracyMin = StaticPatrolAccuracyMinNUD.Value;
            isDirty = true;
        }

        private void StaticPatrolAccuracyMaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.AccuracyMax = StaticPatrolAccuracyMaxNUD.Value;
            isDirty = true;
        }

        private void StaticPatrolThreatDistanceLimitNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.ThreatDistanceLimit = StaticPatrolThreatDistanceLimitNUD.Value;
            isDirty = true;
        }

        private void StaticPatrolDamageMultiplierNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.DamageMultiplier = StaticPatrolDamageMultiplierNUD.Value;
            isDirty = true;
        }

        private void StaticPatrolMinSpreadRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.MinSpreadRadius = (int)StaticPatrolMinSpreadRadiusNUD.Value;
            isDirty = true;
        }

        private void StaticPatrolRespawnTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.RespawnTime = StaticPatrolRespawnTimeNUD.Value;
            isDirty = true;
        }

        private void StaticPatrolDespawnRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.DespawnRadius = StaticPatrolDespawnRadiusNUD.Value;
            isDirty = true;
        }

        private void StaticPatrolMinDistRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.MinDistRadius = StaticPatrolMinDistRadiusNUD.Value;
            isDirty = true;
        }

        private void StaticPatrolCanBeLotedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.CanBeLooted = StaticPatrolCanBeLotedCB.Checked == true ? 1 : 0;
            isDirty = true;
        }

        private void StaticPatrolURBitmaskCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            int StaticPatrolUnlimitedReloadBitmask = 0;
            StaticPatrolUnlimitedReloadBitmask |= StaticPatrolURAnimalsCB.Checked ? 2 : 0;
            StaticPatrolUnlimitedReloadBitmask |= StaticPatrolURInfectedCB.Checked ? 4 : 0;
            StaticPatrolUnlimitedReloadBitmask |= StaticPatrolURPlayersCB.Checked ? 8 : 0;
            StaticPatrolUnlimitedReloadBitmask |= StaticPatrolURVehiclesCB.Checked ? 16 : 0;
            if (StaticPatrolUnlimitedReloadBitmask == 30)
                StaticPatrolUnlimitedReloadBitmask = 1;
            _currentAISpawn.UnlimitedReload = StaticPatrolUnlimitedReloadBitmask;
            isDirty = true;
        }
        
        private void StaticPatrolCanBeTriggeredByAICB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.CanBeTriggeredByAI = StaticPatrolCanBeTriggeredByAICB.Checked == true ? 1 : 0;
            isDirty = true;
        }

        private void StaticPatrolSniperProneDistanceThresholdNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.SniperProneDistanceThreshold = StaticPatrolSniperProneDistanceThresholdNUD.Value;
            isDirty = true;
        }

        private void StaticPatrolDamageReceivedMultiplierNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.DamageReceivedMultiplier = StaticPatrolDamageReceivedMultiplierNUD.Value;
            isDirty = true;
        }

        private void StaticPatrolNoiseInvestigationDistanceLimitNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.NoiseInvestigationDistanceLimit = StaticPatrolNoiseInvestigationDistanceLimitNUD.Value;
            isDirty = true;
        }

        private void StaticPatrolMaxSpreadRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.MaxSpreadRadius = (int)StaticPatrolMaxSpreadRadiusNUD.Value;
            isDirty = true;
        }

        private void StaticPatrolDespawnTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.DespawnTime = StaticPatrolDespawnTimeNUD.Value;
            isDirty = true;
        }

        private void StaticPatrolChanceCB_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.Chance = StaticPatrolChanceCB.Value;
            isDirty = true;
        }

        private void StaticPatrolMaxDistRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.MaxDistRadius = StaticPatrolMaxDistRadiusNUD.Value;
            isDirty = true;
        }

        private void StaticPatrolWaypointInterpolationCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.WaypointInterpolation = StaticPatrolWaypointInterpolationCB.GetItemText(StaticPatrolWaypointInterpolationCB.SelectedItem);
            isDirty = true;
        }

        private void darkButton11_Click(object sender, EventArgs e)
        {
            AddNewfileName form = new AddNewfileName()
            {
                setdescription = "Please enter the Unit you wish to add",
                SetTitle = "Add Unit",
                Setbutton = "Add"
            };
            if (form.ShowDialog() == DialogResult.OK)
            {
                _currentAISpawn.Units.Add(form.NewFileName);
                StaticPatrolUnitsLB.Invalidate();
                isDirty = true;
            }
        }

        private void darkButton10_Click(object sender, EventArgs e)
        {
            _currentAISpawn.Units.Remove(StaticPatrolUnitsLB.GetItemText(StaticPatrolUnitsLB.SelectedItem));
            StaticPatrolUnitsLB.Invalidate();
            isDirty = true;
        }

        private void StaticPatrolWayPointsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StaticPatrolWayPointsLB.SelectedItems.Count < 1) return;
            CurrentWapypoint = StaticPatrolWayPointsLB.SelectedItem as Vec3;
            useraction = false;
            StaticPatrolWaypointPOSXNUD.Value = (decimal)CurrentWapypoint.X;
            StaticPatrolWaypointPOSYNUD.Value = (decimal)CurrentWapypoint.Y;
            StaticPatrolWaypointPOSZNUD.Value = (decimal)CurrentWapypoint.Z;
            useraction = true;
        }

        private void StaticPatrolWaypointPOSXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentWapypoint.X = (float)StaticPatrolWaypointPOSXNUD.Value;
            isDirty = true;
        }

        private void StaticPatrolWaypointPOSYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentWapypoint.Y = (float)StaticPatrolWaypointPOSYNUD.Value;
            isDirty = true;
        }

        private void StaticPatrolWaypointPOSZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentWapypoint.Z = (float)StaticPatrolWaypointPOSZNUD.Value;
            isDirty = true;
        }

        private void StaticPatrolUseRandomWaypointAsStartPointCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.UseRandomWaypointAsStartPoint = StaticPatrolUseRandomWaypointAsStartPointCB.Checked == true ? 1 : 0;
            isDirty = true;
        }

        private void darkButton17_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    DZE importfile = DZEHelpers.LoadFile(filePath);
                    DialogResult dialogResult = MessageBox.Show("Clear Exisitng Position?", "Clear position", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        _currentAISpawn._waypoints.Clear();
                    }
                    foreach (Editorobject eo in importfile.EditorObjects)
                    {
                        _currentAISpawn._waypoints.Add(new Vec3(eo.Position));
                    }
                    StaticPatrolWayPointsLB.SelectedIndex = -1;
                    StaticPatrolWayPointsLB.SelectedIndex = StaticPatrolWayPointsLB.Items.Count - 1;
                    StaticPatrolWayPointsLB.Refresh();
                    StaticPatrolWaypointPOSXNUD.Visible = true;
                    StaticPatrolWaypointPOSYNUD.Visible = true;
                    StaticPatrolWaypointPOSZNUD.Visible = true;
                    isDirty = true;
                }
            }
        }

        private void darkButton5_Click(object sender, EventArgs e)
        {
            string[] fileContent = new string[] { };
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    fileContent = File.ReadAllLines(filePath);
                    DialogResult dialogResult = MessageBox.Show("Clear Exisitng Position?", "Clear position", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        _currentAISpawn._waypoints.Clear();
                    }
                    for (int i = 0; i < fileContent.Length; i++)
                    {
                        if (fileContent[i] == "") continue;
                        string[] linesplit = fileContent[i].Split('|');
                        string[] XYZ = linesplit[1].Split(' ');
                        _currentAISpawn._waypoints.Add(new Vec3(XYZ));

                    }
                    StaticPatrolWayPointsLB.SelectedIndex = -1;
                    StaticPatrolWayPointsLB.SelectedIndex = StaticPatrolWayPointsLB.Items.Count - 1;
                    StaticPatrolWayPointsLB.Refresh();
                    StaticPatrolWaypointPOSXNUD.Visible = true;
                    StaticPatrolWaypointPOSYNUD.Visible = true;
                    StaticPatrolWaypointPOSZNUD.Visible = true;
                    isDirty = true;
                }
            }
        }

        private void darkButton14_Click(object sender, EventArgs e)
        {
            DZE newdze = new DZE()
            {
                MapName = ""
            };
            int m_Id = 0;
            foreach (Vec3 array in _currentAISpawn._waypoints)
            {
                Editorobject eo = new Editorobject()
                {
                    Type = "eAI_SurvivorM_Jose",
                    DisplayName = "eAI_SurvivorM_Jose",
                    Position = array.getfloatarray(),
                    Orientation = new float[] { 0, 0, 0 },
                    Scale = 1.0f,
                    Model = "",
                    Flags = 2147483647,
                    m_Id = m_Id
                };
                newdze.EditorObjects.Add(eo);
                m_Id++;
            }
            newdze.CameraPosition = newdze.EditorObjects[0].Position;
            SaveFileDialog save = new SaveFileDialog();
            if (save.ShowDialog() == DialogResult.OK)
            {
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(newdze, options);
                File.WriteAllText(save.FileName + ".dze", jsonString);
            }
        }

        private void StaticPatrolExporttoMapButton_Click(object sender, EventArgs e)
        {
            StringBuilder SB = new StringBuilder();
            foreach (Vec3 array in _currentAISpawn._waypoints)
            {
                SB.AppendLine("eAI_SurvivorM_Lewis|" + array.GetString() + "|0.0 0.0 0.0");
            }
            SaveFileDialog save = new SaveFileDialog();
            if (save.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(save.FileName + ".map", SB.ToString());
            }
        }

        public void setAInumASReadOnly(bool v)
        {
            StaticPatrolNumberOfAINUD.ReadOnly = v;
            if (v == true)
                StaticPatrolNumberOfAINUD.Increment = 0;
            else
                StaticPatrolNumberOfAINUD.Increment = 1;
        }

        private void darkButton22_Click(object sender, EventArgs e)
        {
            if (_currentAISpawn._waypoints == null)
                _currentAISpawn._waypoints = new BindingList<Vec3>();
            _currentAISpawn._waypoints.Add(new Vec3(0, 0, 0));
            isDirty = true;
            StaticPatrolWayPointsLB.Refresh();
            StaticPatrolWaypointPOSXNUD.Visible = true;
            StaticPatrolWaypointPOSYNUD.Visible = true;
            StaticPatrolWaypointPOSZNUD.Visible = true;
            StaticPatrolWayPointsLB.SelectedIndex = -1;
            StaticPatrolWayPointsLB.SelectedIndex = StaticPatrolWayPointsLB.Items.Count - 1;
        }

        private void darkButton21_Click(object sender, EventArgs e)
        {
            int index = StaticPatrolWayPointsLB.SelectedIndex;
            _currentAISpawn._waypoints.Remove(CurrentWapypoint);
            isDirty = true;
            StaticPatrolWayPointsLB.Refresh();
            StaticPatrolWayPointsLB.SelectedIndex = -1;
            if (StaticPatrolWayPointsLB.Items.Count > 0)
            {
                if (StaticPatrolWayPointsLB.Items.Count == index)
                {
                    StaticPatrolWayPointsLB.SelectedIndex = index - 1;
                }
                else
                {
                    StaticPatrolWayPointsLB.SelectedIndex = index;
                }
            }
            else
            {
                StaticPatrolWayPointsLB.SelectedIndex = -1;
                StaticPatrolWaypointPOSXNUD.Visible = false;
                StaticPatrolWaypointPOSYNUD.Visible = false;
                StaticPatrolWaypointPOSZNUD.Visible = false;
            }
        }
    }
}
