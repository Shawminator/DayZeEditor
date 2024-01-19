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
        private bool useraction { get; set; }

        private ExpansionQuestAISpawn _currentAISpawn { get; set; }
        public ExpansionQuestAISpawn currentAISpawn 
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
            ObjectivesAICampAISpawnsFactionCB.DataSource = new BindingList<string>(Factions);
            ObjectivesAICampAISpawnsLoadoutCB.DataSource = new BindingList<AILoadouts>(LoadoutList);
        }
        private void updatevalues()
        {
            if (_currentAISpawn == null) return;

            useraction = false;
           
            ObjectivesAICampAISpawnsNumberOfAINUD.Value = _currentAISpawn.NumberOfAI;
            ObjectivesAICampAISpawnsNPCNameTB.Text = _currentAISpawn.NPCName;
            ObjectivesAICampAISpawnsBehaviourCB.SelectedIndex = _currentAISpawn.Behaviour;
            ObjectivesAICampAISpawnsFormationCB.SelectedIndex = ObjectivesAICampAISpawnsFormationCB.FindStringExact(_currentAISpawn.Formation);
            ObjectivesAICampAISpawnsLoadoutCB.SelectedIndex = ObjectivesAICampAISpawnsLoadoutCB.FindStringExact(_currentAISpawn.Loadout);
            ObjectivesAICampAISpawnsFactionCB.SelectedIndex = ObjectivesAICampAISpawnsFactionCB.FindStringExact(_currentAISpawn.Faction);
            ObjectivesAICampAISpawnsSpeedNUD.Value = _currentAISpawn.Speed;
            ObjectivesAICampAISpawnsThreatSpeedNUD.Value = _currentAISpawn.ThreatSpeed;
            ObjectivesAICampAISpawnsMinAccuracyNUD.Value = _currentAISpawn.MinAccuracy;
            ObjectivesAICampAISpawnsMaxAccuracyNUD.Value = _currentAISpawn.MaxAccuracy;
            ObjectivesAICampAISpawnsCanBeLootedCB.Checked = _currentAISpawn.CanBeLooted == 1 ? true : false;
            ObjectivesAICampAISpawnsUnlimitedReloadCB.Checked = _currentAISpawn.UnlimitedReload == 1 ? true : false;
            ObjectivesAICampAiSpawnsThreatDistanceLimitNUD.Value = _currentAISpawn.ThreatDistanceLimit;
            ObjectivesAICampAiSpawnsDamageMultiplierNUD.Value = _currentAISpawn.DamageMultiplier;
            ObjectivesAICampAISpawnsDamageReceivedMultiplierNUD.Value = _currentAISpawn.DamageReceivedMultiplier;
            ObjectivesAICampAISpawnsSniperProneDistanceThresholdNUD.Value = _currentAISpawn.SniperProneDistanceThreshold;
            ObjectivesAICampAiSpawnsRespawnTimeNUD.Value = _currentAISpawn.RespawnTime;
            ObjectivesAICampAiSpawnsDespawnTimeNUD.Value = _currentAISpawn.DespawnTime;
            ObjectivesAICampAiSpawnsMinDistanceRadiusNUD.Value = _currentAISpawn.MinDistanceRadius;
            ObjectivesAICampAiSpawnsMaxDistanceRadiusNUD.Value = _currentAISpawn.MaxDistanceRadius;
            ObjectivesAICampAiSpawnsDespawnRadiusNUD.Value = _currentAISpawn.DespawnRadius;

            ObjectivesAICampAISpawnsWayPointsLB.DisplayMember = "DisplayName";
            ObjectivesAICampAISpawnsWayPointsLB.ValueMember = "Value";
            ObjectivesAICampAISpawnsWayPointsLB.DataSource = _currentAISpawn.Waypoints;

            ObjectivesAiCampAISpawnsClassnamesLB.DisplayMember = "DisplayName";
            ObjectivesAiCampAISpawnsClassnamesLB.ValueMember = "Value";
            ObjectivesAiCampAISpawnsClassnamesLB.DataSource = _currentAISpawn.ClassNames;

            useraction = true;
        }
        private void ObjectivesAICampAISpawnsNumberOfAINUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.NumberOfAI = (int)ObjectivesAICampAISpawnsNumberOfAINUD.Value;
            isDirty = true;
        }
        private void ObjectivesAICampAISpawnsNPCNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.NPCName = ObjectivesAICampAISpawnsNPCNameTB.Text;
            isDirty = true;
        }
        private void ObjectivesAICampAISpawnsBehaviourCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.Behaviour = ObjectivesAICampAISpawnsBehaviourCB.SelectedIndex;
            isDirty = true;
        }
        private void ObjectivesAICampAISpawnsFormationCB_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.Formation = ObjectivesAICampAISpawnsFormationCB.GetItemText(ObjectivesAICampAISpawnsFormationCB.SelectedItem);
            isDirty = true;
        }
        private void ObjectivesAICampAISpawnsLoadoutCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.Loadout = ObjectivesAICampAISpawnsLoadoutCB.GetItemText(ObjectivesAICampAISpawnsLoadoutCB.SelectedItem);
            isDirty = true;
        }
        private void ObjectivesAICampAISpawnsFactionCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.Faction = ObjectivesAICampAISpawnsFactionCB.GetItemText(ObjectivesAICampAISpawnsFactionCB.SelectedItem);
            isDirty = true;
        }
        private void ObjectivesAICampAISpawnsSpeedNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.Speed = ObjectivesAICampAISpawnsSpeedNUD.Value;
            isDirty = true;
        }
        private void ObjectivesAICampAISpawnsThreatSpeedNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.ThreatSpeed = ObjectivesAICampAISpawnsThreatSpeedNUD.Value;
            isDirty = true;
        }
        private void ObjectivesAICampAISpawnsUnlimitedReloadCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.UnlimitedReload = ObjectivesAICampAISpawnsUnlimitedReloadCB.Checked == true ? 1 : 0;
            isDirty = true;
        }
        private void ObjectiovesAICampAISpawnsCanBeLootedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.CanBeLooted = ObjectivesAICampAISpawnsCanBeLootedCB.Checked == true ? 1 : 0;
            isDirty = true;
        }
        private void ObjectivesAICampNPCAccuracyMinNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.MinAccuracy = ObjectivesAICampAISpawnsMinAccuracyNUD.Value;
            isDirty = true;
        }
        private void ObjectivesAICampNPCAccuracyMaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.MaxAccuracy = ObjectivesAICampAISpawnsMaxAccuracyNUD.Value;
            isDirty = true;
        }
        private void ObjectivesAICampAiSpawnsThreatDistanceLimitNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.ThreatDistanceLimit = ObjectivesAICampAiSpawnsThreatDistanceLimitNUD.Value;
            isDirty = true;
        }
        private void ObjectivesAICampAiSpawnsDamageMultiplierNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.DamageMultiplier = ObjectivesAICampAiSpawnsDamageMultiplierNUD.Value;
            isDirty = true;
        }
        private void ObjectivesAICampAISpawnsDamageReceivedMultiplierNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.DamageReceivedMultiplier = ObjectivesAICampAISpawnsDamageReceivedMultiplierNUD.Value;
            isDirty = true;
        }
        private void ObjectivesAICampAISpawnsSniperProneDistanceThresholdNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.SniperProneDistanceThreshold = ObjectivesAICampAISpawnsSniperProneDistanceThresholdNUD.Value;
            isDirty = true;
        }
        private void ObjectivesAICampAiSpawnsDespawnTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.DespawnTime = ObjectivesAICampAiSpawnsDespawnTimeNUD.Value;
            isDirty = true;
        }
        private void ObjectivesAICampAiSpawnsRespawnTimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.RespawnTime = ObjectivesAICampAiSpawnsRespawnTimeNUD.Value;
            isDirty = true;
        }
        private void ObjectivesAICampAiSpawnsMinDistanceRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.MinDistanceRadius = ObjectivesAICampAiSpawnsMinDistanceRadiusNUD.Value;
            isDirty = true;
        }
        private void ObjectivesAICampAiSpawnsMaxDistanceRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.MaxDistanceRadius = ObjectivesAICampAiSpawnsMaxDistanceRadiusNUD.Value;
            isDirty = true;
        }
        private void ObjectivesAICampDespawnRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            _currentAISpawn.DespawnRadius = ObjectivesAICampAiSpawnsDespawnRadiusNUD.Value;
            isDirty = true;
        }

        private void ObjectivesAiCampAiSpawnsClassnamesAddButton_Click(object sender, EventArgs e)
        {
            if (ObjectivesAiCampAISpawnsClassnamesLB.SelectedItems.Count <= 0) return;
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    if (!_currentAISpawn.ClassNames.Contains(l))
                        _currentAISpawn.ClassNames.Add(l);
                }
            }
            isDirty = true;
        }
        private void ObjectivesAiCampAiSpawnsClassnamesRemoveButton_Click(object sender, EventArgs e)
        {
            if (ObjectivesAiCampAISpawnsClassnamesLB.SelectedItems.Count <= 0) return;
            for (int i = 0; i < ObjectivesAiCampAISpawnsClassnamesLB.SelectedItems.Count; i++)
            {
                _currentAISpawn.ClassNames.Remove(ObjectivesAiCampAISpawnsClassnamesLB.GetItemText(ObjectivesAiCampAISpawnsClassnamesLB.SelectedItems[0]));
            }
            isDirty = true;
        }
        public decimal[] CurrentWapypoint;
        private void ObjectivesAICampAISpawnsWayPointsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ObjectivesAICampAISpawnsWayPointsLB.SelectedItems.Count < 1) return;
            CurrentWapypoint = ObjectivesAICampAISpawnsWayPointsLB.SelectedItem as decimal[];
            useraction = false;
            numericUpDown9.Value = (decimal)CurrentWapypoint[0];
            numericUpDown11.Value = (decimal)CurrentWapypoint[1];
            numericUpDown12.Value = (decimal)CurrentWapypoint[2];
            useraction = true;

        }
        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentWapypoint[0] = (decimal)numericUpDown9.Value;
            isDirty = true;
        }
        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentWapypoint[1] = (decimal)numericUpDown11.Value;
            isDirty = true;
        }
        private void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CurrentWapypoint[2] = (decimal)numericUpDown12.Value;
            isDirty = true;
        }
        private void ObjectivesAiCampAISpawnsWaypointAddButton_Click(object sender, EventArgs e)
        {
            if (ObjectivesAiCampAISpawnsClassnamesLB.SelectedItems.Count <= 0) return;
            _currentAISpawn.Waypoints.Add(new decimal[] { 0, 0, 0 });
            isDirty = true;
        }
        private void ObjectivesAiCampAISpawnsWaypointRemoveButton_Click(object sender, EventArgs e)
        {
            _currentAISpawn.Waypoints.Remove(CurrentWapypoint);
            isDirty = true;
            ObjectivesAICampAISpawnsWayPointsLB.Refresh();
        }
        private void darkButton18_Click(object sender, EventArgs e)
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
                        _currentAISpawn.Waypoints.Clear();
                    }
                    for (int i = 0; i < fileContent.Length; i++)
                    {
                        if (fileContent[i] == "") continue;
                        string[] linesplit = fileContent[i].Split('|');
                        string[] XYZ = linesplit[1].Split(' ');
                        decimal[] newfloatarray = new decimal[] { Convert.ToDecimal(XYZ[0]), Convert.ToDecimal(XYZ[1]), Convert.ToDecimal(XYZ[2]) };
                        _currentAISpawn.Waypoints.Add(newfloatarray);

                    }
                    ObjectivesAICampAISpawnsWayPointsLB.SelectedIndex = -1;
                    ObjectivesAICampAISpawnsWayPointsLB.SelectedIndex = ObjectivesAICampAISpawnsWayPointsLB.Items.Count - 1;
                    ObjectivesAICampAISpawnsWayPointsLB.Refresh();
                    isDirty = true;
                }
            }
        }
        private void darkButton17_Click(object sender, EventArgs e)
        {
            StringBuilder SB = new StringBuilder();
            foreach (decimal[] array in _currentAISpawn.Waypoints)
            {
                SB.AppendLine("eAI_SurvivorM_Lewis|" + array[0].ToString() + " " + array[1].ToString() + " " + array[2].ToString() + "|0.0 0.0 0.0");
            }
            SaveFileDialog save = new SaveFileDialog();
            if (save.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(save.FileName + ".map", SB.ToString());
            }
        }
        private void darkButton16_Click(object sender, EventArgs e)
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
                        _currentAISpawn.Waypoints.Clear();
                    }
                    foreach (Editorobject eo in importfile.EditorObjects)
                    {
                        decimal[] newfloatarray = new decimal[] { Convert.ToDecimal(eo.Position[0]), Convert.ToDecimal(eo.Position[1]), Convert.ToDecimal(eo.Position[2]) };
                        _currentAISpawn.Waypoints.Add(newfloatarray);
                    }
                    ObjectivesAICampAISpawnsWayPointsLB.SelectedIndex = -1;
                    ObjectivesAICampAISpawnsWayPointsLB.SelectedIndex = ObjectivesAICampAISpawnsWayPointsLB.Items.Count - 1;
                    ObjectivesAICampAISpawnsWayPointsLB.Refresh();
                    isDirty = true;
                }
            }
        }
        private void darkButton15_Click(object sender, EventArgs e)
        {
            DZE newdze = new DZE()
            {
                MapName = ""
            };
            foreach (decimal[] array in _currentAISpawn.Waypoints)
            {
                Editorobject eo = new Editorobject()
                {
                    Type = "eAI_SurvivorM_Jose",
                    DisplayName = "eAI_SurvivorM_Jose",
                    Position = new float[] { Convert.ToSingle(array[0]), Convert.ToSingle(array[1]), Convert.ToSingle(array[2]) },
                    Orientation = new float[] { 0, 0, 0 },
                    Scale = (float)1.0,
                    Flags = 2147483647
                };
                newdze.EditorObjects.Add(eo);
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
        private void ObjectivesAICampAISpawnsWayPointsLB_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if(ObjectivesAICampAISpawnsWayPointsLB.SelectedItems.Count <=0) { return; }
            useraction = false;
            CurrentWapypoint = ObjectivesAICampAISpawnsWayPointsLB.SelectedItem as decimal[];
            numericUpDown9.Value = CurrentWapypoint[0];
            numericUpDown11.Value = CurrentWapypoint[1];
            numericUpDown12.Value = CurrentWapypoint[2];

            useraction = true;

        }
    }
}
