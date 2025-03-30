using DarkUI.Forms;
using DayZeLib;
using DayZeLib.DynamicWeatherPlugin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows.Forms;

namespace DayZeEditor
{
    public partial class DynamicWeatherPluginManager : DarkForm
    {
        public Project currentproject { get; set; }
        public string DynamicWeatherPluginPath { get; private set; }
        public string Projectname { get; private set; }

        private bool useraction;

        public WeatherDynamic CurrentDynamicWeather { get; private set; }

        public DynamicWeatherPlugin DynamicWeatherPlugin;

        public DynamicWeatherPluginManager()
        {
            InitializeComponent();
        }
        private void DynamicWeatherPluginManagerClosing(object sender, FormClosingEventArgs e)
        {
            if (DynamicWeatherPlugin.isDirty)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SaveFile();
                }
            }
        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void SaveFile()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (DynamicWeatherPlugin.isDirty)
            {
                if (currentproject.Createbackups && File.Exists(DynamicWeatherPlugin.Filename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(DynamicWeatherPlugin.Filename) + "\\Backup\\" + SaveTime);
                    File.Copy(DynamicWeatherPlugin.Filename, Path.GetDirectoryName(DynamicWeatherPlugin.Filename) + "\\Backup\\" + SaveTime + "\\" + Path.GetFileNameWithoutExtension(DynamicWeatherPlugin.Filename) + ".bak", true);
                }
                DynamicWeatherPlugin.isDirty = false;
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(DynamicWeatherPlugin.m_Dynamics, options);
                File.WriteAllText(DynamicWeatherPlugin.Filename, jsonString);
                midifiedfiles.Add(Path.GetFileName(DynamicWeatherPlugin.Filename));
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
            Process.Start(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath);
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
        private void DynamicWeatherPluginManagerLoad(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Projectname = currentproject.ProjectName;

            useraction = false;
            DynamicWeatherPluginPath = currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\weather.json";
            DynamicWeatherPlugin = new DynamicWeatherPlugin();
            DynamicWeatherPlugin.m_Dynamics = new BindingList<WeatherDynamic>(JsonSerializer.Deserialize<WeatherDynamic[]>(File.ReadAllText(DynamicWeatherPluginPath), new JsonSerializerOptions { Converters = { new BoolConverter() } }).ToList());
            DynamicWeatherPlugin.isDirty = false;
            DynamicWeatherPlugin.Filename = DynamicWeatherPluginPath;

            WeatherLB.DisplayMember = "DisplayName";
            WeatherLB.ValueMember = "Value";
            WeatherLB.DataSource = DynamicWeatherPlugin.m_Dynamics;

            useraction = true;

        }

        private void WeatherLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (WeatherLB.SelectedItems.Count <= 0) return;
            useraction = false;
            CurrentDynamicWeather = WeatherLB.SelectedItem as WeatherDynamic;

            chat_messageCB.Checked = CurrentDynamicWeather.chat_message == 1 ? true : false;
            notify_messageCB.Checked = CurrentDynamicWeather.notify_message == 1 ? true : false;
            nameTB.Text = CurrentDynamicWeather.name;
            transition_minNUD.Value = CurrentDynamicWeather.transition_min;
            transition_maxNUD.Value = CurrentDynamicWeather.transition_max;
            duration_minNUD.Value = CurrentDynamicWeather.duration_min;
            duration_maxNUD.Value = CurrentDynamicWeather.duration_max;
            overcast_minNUD.Value = CurrentDynamicWeather.overcast_min;
            overcast_maxNUD.Value = CurrentDynamicWeather.overcast_max;

            use_dyn_vol_fogCB.Checked = CurrentDynamicWeather.use_dyn_vol_fog;
            dyn_vol_fog_dist_minNUD.Value = CurrentDynamicWeather.dyn_vol_fog_dist_min;
            dyn_vol_fog_dist_maxNUD.Value = CurrentDynamicWeather.dyn_vol_fog_dist_max;
            dyn_vol_fog_height_minNUD.Value = CurrentDynamicWeather.dyn_vol_fog_height_min;
            dyn_vol_fog_height_maxNUD.Value = CurrentDynamicWeather.dyn_vol_fog_height_max;
            dyn_vol_fog_biasNUD.Value = CurrentDynamicWeather.dyn_vol_fog_bias;
            fog_transition_timeNUD.Value = CurrentDynamicWeather.fog_transition_time;
            fog_minNUD.Value = CurrentDynamicWeather.fog_min;
            fog_maxNUD.Value = CurrentDynamicWeather.fog_max;
            rain_minNUD.Value = CurrentDynamicWeather.rain_min;
            rain_maxNUD.Value = CurrentDynamicWeather.rain_max;
            wind_minNUD.Value = CurrentDynamicWeather.wind_min;
            wind_maxNUD.Value = CurrentDynamicWeather.wind_max;
            snowfall_minNUD.Value = CurrentDynamicWeather.snowfall_min;
            snowfall_maxNUD.Value= CurrentDynamicWeather.snowfall_max;
            use_snowflake_scaleCB.Checked = CurrentDynamicWeather.use_snowflake_scale;
            snowflake_scale_minNUD.Value = CurrentDynamicWeather.snowflake_scale_min;
            snowflake_scale_maxNUD.Value = CurrentDynamicWeather.snowflake_scale_max;

            stormCB.Checked = CurrentDynamicWeather.storm == 1 ? true : false;
            thunder_timeoutNUD.Value = CurrentDynamicWeather.thunder_timeout;

            useraction = true;
        }

        private void darkButton5_Click(object sender, EventArgs e)
        {
            WeatherDynamic newweather = new WeatherDynamic()
            {
                chat_message = 1,
                notify_message = 1,
                name = "TEMPLATE",
                transition_min = 1.0m,
                transition_max = 2.0m,
                duration_min = 30.0m,
                duration_max = 60.0m,
                overcast_min = 0.0m,
                overcast_max = 1.0m,
                fog_min = 0.0m,
                fog_max = 1.0m,
                use_dyn_vol_fog = true,
                dyn_vol_fog_dist_min = 0.2m,
                dyn_vol_fog_dist_max = 0.5m,
                dyn_vol_fog_height_min = 0.1m,
                dyn_vol_fog_height_max = 0.3m,
                dyn_vol_fog_bias = 10.0m,
                fog_transition_time = 60.0m,
                wind_min = 0.0m,
                wind_max = 1.0m,
                rain_min = 0.0m,
                rain_max = 1.0m,
                snowfall_min = 0.0m,
                snowfall_max = 1.0m,
                snowflake_scale_min = 0.5m,
                snowflake_scale_max = 2.0m,
                use_snowflake_scale = true,
                storm = 0,
                thunder_timeout = 0.0m
            };
            DynamicWeatherPlugin.m_Dynamics.Add(newweather);
            DynamicWeatherPlugin.isDirty = true;
            WeatherLB.SelectedIndex = WeatherLB.Items.Count - 1;
        }

        private void darkButton6_Click(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (WeatherLB.SelectedItems.Count > 0)
            {
                DynamicWeatherPlugin.m_Dynamics.Remove(WeatherLB.SelectedItem as WeatherDynamic);
                DynamicWeatherPlugin.isDirty = true;
                if (WeatherLB.Items.Count == 0)
                    WeatherLB.SelectedIndex = -1;
                else
                    WeatherLB.SelectedIndex = 0;
            }
        }
        private void DWPstringTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TextBox textbox = sender as TextBox;
            Helper.SetStringValue(CurrentDynamicWeather, textbox.Name.Substring(0, textbox.Name.Length - 2), textbox.Text);
            DynamicWeatherPlugin.isDirty = true;
        }
        private void DWPBoolsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CheckBox chk = sender as CheckBox;
            Helper.SetBoolValue(CurrentDynamicWeather, chk.Name.Substring(0, chk.Name.Length - 2), chk.Checked);
            DynamicWeatherPlugin.isDirty = true;
        }
        private void DWPFaketBoolsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CheckBox chk = sender as CheckBox;
            Helper.SetFakeBoolValue(CurrentDynamicWeather, chk.Name.Substring(0, chk.Name.Length - 2), chk.Checked);
            DynamicWeatherPlugin.isDirty = true;
        }
        private void DWPDecimalsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            NumericUpDown nud = sender as NumericUpDown;
            Helper.SetDecimalValue(CurrentDynamicWeather, nud.Name.Substring(0, nud.Name.Length - 3), nud.Value);
            DynamicWeatherPlugin.isDirty = true;
        }
    }
}
