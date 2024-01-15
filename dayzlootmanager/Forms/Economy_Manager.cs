using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.Xsl;
using Cyotek.Windows.Forms;
using DarkUI.Forms;
using DayZeLib;
using FastColoredTextBoxNS;

namespace DayZeEditor
{
    public partial class Economy_Manager : DarkForm
    {
        public bool isUserInteraction = true;


        public Project currentproject { get; set; }


        public TypesFile vanillatypes;
        public List<TypesFile> ModTypes;
        public TypesFile currentTypesFile;
        public typesType currentlootpart;
        public string currentcollection;
        public string filename;
        public bool Fileloaded = false;
        public bool FullTypes;
        private int searchnum;
        private List<TreeNode> searchtreeNodes;
        private List<typesType> foundparts;

        public MapData MapData { get; private set; }

        public BindingList<randompresetsAttachments> cargoAttachments = new BindingList<randompresetsAttachments>();
        public BindingList<randompresetsCargo> cargoItems = new BindingList<randompresetsCargo>();
        #region formsstuff
        public Economy_Manager()
        {
            InitializeComponent();
            isUserInteraction = false;
            EconomyTabPage.ItemSize = new Size(0, 1);
            isUserInteraction = true;

        }
        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            ListBox lb = sender as ListBox;
            e.DrawBackground();
            if (lb.Items.Count == 0) return;
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
        private void Economy_Manager_Load(object sender, EventArgs e)
        {
            isUserInteraction = false;
            
            filename = currentproject.ProjectName;
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            doubleClickTimer.Interval = 100;
            doubleClickTimer.Tick += new EventHandler(doubleClickTimer_Tick);

            BindingList<listsCategory> newlist = new BindingList<listsCategory>
            {
                new listsCategory()
                {
                    name = "other"
                }
            };
            foreach (listsCategory cat in currentproject.limitfefinitions.lists.categories)
            {
                newlist.Add(cat);
            }
            comboBox1.DataSource = newlist;
            comboBox2.DataSource = currentproject.limitfefinitions.lists.usageflags;
            if (comboBox2.Items.Count == 0)
                groupBox2.Visible = false;
            else
                groupBox2.Visible = true;
            comboBox4.DataSource = currentproject.limitfefinitions.lists.tags;

            comboBox5.DataSource = currentproject.limitfefinitions.lists.tags;

            comboBox8.DataSource = new List<listsCategory>(currentproject.limitfefinitions.lists.categories.ToList());

            comboBox7.DataSource = currentproject.limitfefinitions.lists.usageflags;
            comboBox6.DataSource = currentproject.limitfefinitions.lists.tags;
            SetuprandomPresetsForSpawnabletypes();

            
            PopulateTreeView();
            Loadevents();
            LoadeventSpawns();
            LoadeventSpawnsGroup();
            LoadRandomPresets();
            LoadSpawnableTypes();
            populateEconmyTreeview();
            LoadPlayerSpawns();
            LoadCFGGamelplay();
            LoadSpawnGear();
            LoadContaminatoedArea();
            LoadGlobals();
            Loadweather();
            Loadignorelist();
            LoadMapgrouProto();
            LoadMapGroupPos();
            LoadTerritories();
            loadinitC();
            SetSummarytiers();
            
            
            NomCountLabel.Text = "Total Nominal Count :- " + currentproject.TotalNomCount.ToString();

            MapData = new MapData(Application.StartupPath + currentproject.MapPath + ".xyz");

            pictureBox2.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Livonia maop size is 12800 x 12800, 0,0 bottom left, center 6400 x 6400
            pictureBox2.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox2.Paint += new PaintEventHandler(DrawAllPlayerSpawns);
            pictureBox2.Invalidate();
            trackBar4.Value = 1;
            SetSpawnScale();

            pictureBox1.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Livonia maop size is 12800 x 12800, 0,0 bottom left, center 6400 x 6400
            pictureBox1.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox1.Paint += new PaintEventHandler(DrawAllEventSpawns);
            pictureBox1.Invalidate();
            trackBar1.Value = 1;
            SetEventSpawnScale();
            EconomyTabPage.SelectedIndex = 3;
            TypesTabButton.Checked = true;
            EconomySearchBoxTB.Visible = true;
            EconomyFindButton.Visible = true;
            isUserInteraction = true;
        }

        private void SetuprandomPresetsForSpawnabletypes()
        {
            foreach (var item in currentproject.cfgrandompresetsconfig.randompresets.Items)
            {
                if (item is randompresetsAttachments)
                {
                    cargoAttachments.Add(item as randompresetsAttachments);
                }
                else if (item is randompresetsCargo)
                {
                    cargoItems.Add(item as randompresetsCargo);
                }
            }

            CargoPresetComboBox.DataSource = cargoItems;
            AttachmentPresetComboBox.DataSource = cargoAttachments;
        }
        private void populateEconmyTreeview()
        {
            var serializer = new XmlSerializer(typeof(economycore));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var sw = new StringWriter();
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true });
            serializer.Serialize(xmlWriter, currentproject.EconomyCore.economycore, ns);


            // Load the xslt used by IE to render the xml
            XslCompiledTransform xTrans = new XslCompiledTransform();
            xTrans.Load(Application.StartupPath + "//lib//defaultss.xsl");

            // Read the xml string.
            StringReader sr = new StringReader(sw.ToString());
            XmlReader xReader = XmlReader.Create(sr);

            // Transform the XML data
            MemoryStream ms = new MemoryStream();
            xTrans.Transform(xReader, null, ms);

            ms.Position = 0;

            // Set to the document stream
            webBrowser1.DocumentStream = ms;
        }
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Process.Start(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath);
        }
        private void SaveFileButton_Click(object sender, EventArgs e) //empty
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            if (EconomyTabPage.SelectedIndex == 16)
            {
                File.WriteAllText(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\init.c", fastColoredTextBox1.Text);
                midifiedfiles.Add("init.c");
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
            else
            {
                SaveEconomyFiles(midifiedfiles, SaveTime);
            }
        }
        private void SaveEconomyFiles(List<string> midifiedfiles, string SaveTime)
        {
            if (vanillatypes.isDirty)
            {
                if (currentproject.Createbackups)
                    vanillatypes.SaveTyes(SaveTime);
                else
                    vanillatypes.SaveTyes();
                vanillatypes.isDirty = false;
                midifiedfiles.Add(Path.GetFileName(vanillatypes.Filename));
            }
            foreach (TypesFile tf in ModTypes)
            {
                if (tf.isDirty)
                {
                    if (currentproject.Createbackups)
                        tf.SaveTyes(SaveTime);
                    else
                        tf.SaveTyes();
                    tf.isDirty = false;
                    midifiedfiles.Add(Path.GetFileName(tf.Filename));
                }
            }

            foreach (eventscofig eventconfig in currentproject.ModEventsList)
            {
                if (eventconfig.isDirty)
                {
                    if (currentproject.Createbackups)
                        eventconfig.SaveEvent(SaveTime);
                    else
                        eventconfig.SaveEvent();
                    eventconfig.isDirty = false;
                    midifiedfiles.Add(Path.GetFileName(eventconfig.Filename));
                }
            }

            if (currentproject.cfgeventspawns != null)
            {
                if (currentproject.cfgeventspawns.isDirty)
                {
                    if (currentproject.Createbackups)
                        currentproject.cfgeventspawns.SaveEventSpawns(SaveTime);
                    else
                        currentproject.cfgeventspawns.SaveEventSpawns();
                    currentproject.cfgeventspawns.isDirty = false;
                    midifiedfiles.Add(Path.GetFileName(currentproject.cfgeventspawns.Filename));
                }
            }
            if (currentproject.cfgeventgroups != null)
            {
                if (currentproject.cfgeventgroups.isDirty)
                {
                    if (currentproject.Createbackups)
                        currentproject.cfgeventgroups.SaveEventGroups(SaveTime);
                    else
                        currentproject.cfgeventgroups.SaveEventGroups();
                    currentproject.cfgeventgroups.isDirty = false;
                    midifiedfiles.Add(Path.GetFileName(currentproject.cfgeventgroups.Filename));
                }
            }

            if (currentproject.spawnabletypesList != null)
            {
                foreach (Spawnabletypesconfig Spawnabletypesconfig in currentproject.spawnabletypesList)
                {
                    if (Spawnabletypesconfig.isDirty)
                    {
                        if (currentproject.Createbackups)
                            Spawnabletypesconfig.Savespawnabletypes(SaveTime);
                        else
                            Spawnabletypesconfig.Savespawnabletypes();
                        Spawnabletypesconfig.isDirty = false;
                        midifiedfiles.Add(Path.GetFileName(Spawnabletypesconfig.Filename));
                    }
                }
            }
            if (currentproject.cfgrandompresetsconfig != null)
            {
                if (currentproject.cfgrandompresetsconfig.isDirty)
                {
                    if (currentproject.Createbackups)
                        currentproject.cfgrandompresetsconfig.SaveRandomPresets(SaveTime);
                    else
                        currentproject.cfgrandompresetsconfig.SaveRandomPresets();
                    currentproject.cfgrandompresetsconfig.isDirty = false;
                    midifiedfiles.Add(Path.GetFileName(currentproject.cfgrandompresetsconfig.Filename));
                }
            }

            if (currentproject.cfgplayerspawnpoints != null)
            {
                if (currentproject.cfgplayerspawnpoints.isDirty)
                {
                    if (currentproject.Createbackups)
                        currentproject.cfgplayerspawnpoints.Savecfgplayerspawnpoints(SaveTime);
                    else
                        currentproject.cfgplayerspawnpoints.Savecfgplayerspawnpoints();
                    currentproject.cfgplayerspawnpoints.isDirty = false;
                    midifiedfiles.Add(Path.GetFileName(currentproject.cfgplayerspawnpoints.Filename));
                }
            }
            if (currentproject.CFGGameplayConfig != null)
            {
                if (currentproject.CFGGameplayConfig.isDirty)
                {
                    currentproject.CFGGameplayConfig.SaveCFGGameplay();
                    
                    currentproject.CFGGameplayConfig.isDirty = false;
                    midifiedfiles.Add(Path.GetFileName(currentproject.CFGGameplayConfig.Filename) + " Saved....");
                }
                foreach(SpawnGearPresetFiles SGPF in currentproject.CFGGameplayConfig.cfggameplay.SpawnGearPresetFiles)
                {
                    if(SGPF.isDirty)
                    {
                        midifiedfiles.Add(SGPF.Filename + " Saved....");
                    }
                }
                currentproject.CFGGameplayConfig.SaveSpawnGearPresetFiles(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\");
            }
            
            if (currentproject.cfgEffectAreaConfig != null)
            {
                if (currentproject.cfgEffectAreaConfig.isDirty)
                {
                    currentproject.cfgEffectAreaConfig.SavecfgEffectArea();
                    currentproject.cfgEffectAreaConfig.isDirty = false;
                    midifiedfiles.Add(Path.GetFileName(currentproject.cfgEffectAreaConfig.Filename) + " Saved....");
                }
            }

            if (currentproject.gloabsconfig != null)
            {
                if (currentproject.gloabsconfig.isDirty)
                {
                    if (currentproject.Createbackups)
                        currentproject.gloabsconfig.SaveGlobals(SaveTime);
                    else
                        currentproject.gloabsconfig.SaveGlobals();
                    currentproject.gloabsconfig.isDirty = false;
                    midifiedfiles.Add(Path.GetFileName(currentproject.gloabsconfig.Filename));
                }
            }
            if (currentproject.weatherconfig != null)
            {
                if (currentproject.weatherconfig.isDirty)
                {
                    if (currentproject.Createbackups)
                        currentproject.weatherconfig.SaveWeather(SaveTime);
                    else
                        currentproject.weatherconfig.SaveWeather();
                    currentproject.weatherconfig.isDirty = false;
                    midifiedfiles.Add(Path.GetFileName(currentproject.weatherconfig.Filename));
                }
            }
            if (currentproject.cfgignorelist != null)
            {
                if (currentproject.cfgignorelist.isDirty)
                {
                    if (currentproject.Createbackups)
                        currentproject.cfgignorelist.Saveignorelist(SaveTime);
                    else
                        currentproject.cfgignorelist.Saveignorelist();
                    currentproject.cfgignorelist.isDirty = false;
                    midifiedfiles.Add(Path.GetFileName(currentproject.cfgignorelist.Filename));
                }
            }
            if (currentproject.mapgroupproto != null)
            {
                if (currentproject.mapgroupproto.isDirty)
                {
                    if (currentproject.Createbackups)
                        currentproject.mapgroupproto.Savemapgroupproto(SaveTime);
                    else
                        currentproject.mapgroupproto.Savemapgroupproto();
                    currentproject.mapgroupproto.isDirty = false;
                    midifiedfiles.Add(Path.GetFileName(currentproject.mapgroupproto.Filename));
                }
            }
            if(currentproject.mapgrouppos != null)
            {
                if(currentproject.mapgrouppos.isDirty)
                {
                    if (currentproject.Createbackups)
                        currentproject.mapgrouppos.Savemapgrouppos(SaveTime);
                    else
                        currentproject.mapgrouppos.Savemapgrouppos();
                    currentproject.mapgrouppos.isDirty = false;
                    midifiedfiles.Add(Path.GetFileName(currentproject.mapgrouppos.Filename));
                }
            }
            if (currentproject.territoriesList != null)
            {
                foreach (territoriesConfig territoriesConfig in currentproject.territoriesList)
                {
                    if (territoriesConfig.isDirty)
                    {
                        if (currentproject.Createbackups)
                            territoriesConfig.SaveTerritories(SaveTime);
                        else
                            territoriesConfig.SaveTerritories();
                        territoriesConfig.isDirty = false;
                        midifiedfiles.Add(Path.GetFileName(territoriesConfig.Filename));
                    }
                }
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
        private void Economy_Manager_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool needtosave = false;
            //events
            foreach (eventscofig eventconfig in currentproject.ModEventsList)
            {
                if (eventconfig.isDirty)
                {
                    needtosave = true;
                }
            }
            //eventspawns
            if (currentproject.cfgeventspawns != null)
            {
                if (currentproject.cfgeventspawns.isDirty)
                {
                    needtosave = true;
                }
            }
            //eventgroups
            if (currentproject.cfgeventgroups != null)
            {
                if (currentproject.cfgeventgroups.isDirty)
                {
                    needtosave = true;
                }
            }
            //types
            if (vanillatypes.isDirty)
            {
                needtosave = true;
            }
            foreach (TypesFile tf in ModTypes)
            {
                if (tf.isDirty)
                {
                    needtosave = true;
                }
            }
            //spawnabletypes
            if (currentproject.spawnabletypesList != null)
            {
                foreach (Spawnabletypesconfig Spawnabletypesconfig in currentproject.spawnabletypesList)
                {
                    if (Spawnabletypesconfig.isDirty)
                    {
                        needtosave = true;
                    }
                }
            }
            //randompresets
            if (currentproject.cfgrandompresetsconfig != null)
            {
                if (currentproject.cfgrandompresetsconfig.isDirty)
                {
                    needtosave = true;
                }
            }
            //globals
            if (currentproject.gloabsconfig != null)
            {
                if (currentproject.gloabsconfig.isDirty)
                {
                    needtosave = true;
                }
            }
            //playerspawns
            if (currentproject.cfgplayerspawnpoints != null)
            {
                if (currentproject.cfgplayerspawnpoints.isDirty)
                {
                    needtosave = true;
                }
            }
            //cfggameplay
            if (currentproject.CFGGameplayConfig != null)
            {
                if (currentproject.CFGGameplayConfig.isDirty)
                {
                    needtosave = true;
                }
                foreach (SpawnGearPresetFiles SGPF in currentproject.CFGGameplayConfig.cfggameplay.SpawnGearPresetFiles)
                {
                    if (SGPF.isDirty)
                    {
                        needtosave = true;
                    }
                }
            }
            //contaminatioedarea
            if (currentproject.cfgEffectAreaConfig != null)
            {
                if (currentproject.cfgEffectAreaConfig.isDirty)
                {
                    needtosave = true;
                }
            }
            //weather
            if (currentproject.weatherconfig != null)
            {
                if (currentproject.weatherconfig.isDirty)
                {
                    needtosave = true;
                }
            }
            //cfgignorelist
            if (currentproject.cfgignorelist != null)
            {
                if (currentproject.cfgignorelist.isDirty)
                {
                    needtosave = true;
                }
            }
            //mapgroupproto
            if (currentproject.mapgroupproto != null)
            {
                if (currentproject.mapgroupproto.isDirty)
                {
                    needtosave = true;
                }
            }
            //mapgrouppos
            if (currentproject.mapgrouppos != null)
            {
                if (currentproject.mapgrouppos.isDirty)
                {
                    needtosave = true;
                }
            }
            if (needtosave)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    List<string> midifiedfiles = new List<string>();
                    string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
                    SaveEconomyFiles(midifiedfiles, SaveTime);
                }
            }
        }

        private void EventsTabButton_Click(object sender, EventArgs e)
        {
            EconomyTabPage.SelectedIndex = 0;
            if (EconomyTabPage.SelectedIndex == 0)
                EventsTabButton.Checked = true;
        }
        private void EventSpawnTabButton_Click(object sender, EventArgs e)
        {
            EconomyTabPage.SelectedIndex = 1;
            if (EconomyTabPage.SelectedIndex == 1)
                EventSpawnTabButton.Checked = true;
        }
        private void EventGroupsTabButton_Click(object sender, EventArgs e)
        {
            EconomyTabPage.SelectedIndex = 2;
            if (EconomyTabPage.SelectedIndex == 2)
                EventGroupsTabButton.Checked = true;
        }
        private void TypesTabButton_Click(object sender, EventArgs e)
        {
            EconomyTabPage.SelectedIndex = 3;
            if (EconomyTabPage.SelectedIndex == 3)
            {
                EconomySearchBoxTB.Visible = true;
                EconomyFindButton.Visible = true;
                TypesTabButton.Checked = true;
            }
        }
        private void TypesSummaryTabButton_Click(object sender, EventArgs e)
        {
            EconomyTabPage.SelectedIndex = 4;
            if (EconomyTabPage.SelectedIndex == 4)
                TypesSummaryTabButton.Checked = true;
        }
        private void EconomyCorePreviewTabButton_Click(object sender, EventArgs e)
        {
            EconomyTabPage.SelectedIndex = 5;
            if (EconomyTabPage.SelectedIndex == 5)
                EconomyCorePreviewTabButton.Checked = true;
        }
        private void SpawnabletypesTabButton_Click(object sender, EventArgs e)
        {
            EconomyTabPage.SelectedIndex = 6;
            if (EconomyTabPage.SelectedIndex == 6)
                SpawnabletypesTabButton.Checked = true;
        }
        private void RandomPresetsTabButton_Click(object sender, EventArgs e)
        {
            EconomyTabPage.SelectedIndex = 7;
            if (EconomyTabPage.SelectedIndex == 7)
                RandomPresetsTabButton.Checked = true;
        }
        private void GlobalsTabButton_Click(object sender, EventArgs e)
        {
            EconomyTabPage.SelectedIndex = 8;
            if (EconomyTabPage.SelectedIndex == 8)
                GlobalsTabButton.Checked = true;
        }
        private void PlayerSpawnPointsTabButton_Click(object sender, EventArgs e)
        {
            EconomyTabPage.SelectedIndex = 9;
            PlayerSpawnFreshButton.AutoSize = true;
            PlayerSpawnHopButton.AutoSize = true;
            PlayerSpawnTravelButton.AutoSize = true;
            if (EconomyTabPage.SelectedIndex == 9)
                PlayerSpawnPointsTabButton.Checked = true;
        }
        private void CFGGameplayTabButton_Click(object sender, EventArgs e)
        {
            EconomyTabPage.SelectedIndex = 10;
            if (EconomyTabPage.SelectedIndex == 10)
                CFGGameplayTabButton.Checked = true;
        }
        private void ContaminatoedAreaTabButton_Click(object sender, EventArgs e)
        {
            EconomyTabPage.SelectedIndex = 11;
            if (EconomyTabPage.SelectedIndex == 11)
                ContaminatoedAreaTabButton.Checked = true;
        }
        private void WeatherTabButton_Click(object sender, EventArgs e)
        {
            EconomyTabPage.SelectedIndex = 12;
            if (EconomyTabPage.SelectedIndex == 12)
                WeatherTabButton.Checked = true;
        }
        private void CFGignoreListTabButton_Click(object sender, EventArgs e)
        {
            EconomyTabPage.SelectedIndex = 13;
            if (EconomyTabPage.SelectedIndex == 13)
                CFGignoreListTabButton.Checked = true;
        }
        private void MapGroupProtoTabButton_Click(object sender, EventArgs e)
        {
            EconomyTabPage.SelectedIndex = 14;
            if (EconomyTabPage.SelectedIndex == 14)
                MapGroupProtoTabButton.Checked = true;

        }
        private void MapGroupPosTabButton_Click(object sender, EventArgs e)
        {
            EconomyTabPage.SelectedIndex = 18;
            if (EconomyTabPage.SelectedIndex == 18)
                MapGroupPosTabButton.Checked = true;
        }
        private void TerritoriesTabButton_Click(object sender, EventArgs e)
        {
            EconomyTabPage.SelectedIndex = 15;
            if (EconomyTabPage.SelectedIndex == 15)
                TerritoriesTabButton.Checked = true;
        }
        private void InitCTabButton_Click(object sender, EventArgs e)
        {
            EconomyTabPage.SelectedIndex = 16;
            if (EconomyTabPage.SelectedIndex == 16)
                InitCTabButton.Checked = true;
        }
        private void SpawnGearTabButton_Click(object sender, EventArgs e)
        {
            EconomyTabPage.SelectedIndex = 17;
            if (EconomyTabPage.SelectedIndex == 17)
                SpawnGearTabButton.Checked = true;
        }
        private void EconomyTabPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            EventsTabButton.Checked = false;
            EventSpawnTabButton.Checked = false;
            EventGroupsTabButton.Checked = false;
            TypesTabButton.Checked = false;
            TypesSummaryTabButton.Checked = false;
            EconomyCorePreviewTabButton.Checked = false;
            SpawnabletypesTabButton.Checked = false;
            RandomPresetsTabButton.Checked = false;
            GlobalsTabButton.Checked = false;
            PlayerSpawnPointsTabButton.Checked = false;
            CFGGameplayTabButton.Checked = false;
            ContaminatoedAreaTabButton.Checked = false;
            WeatherTabButton.Checked = false;
            CFGignoreListTabButton.Checked = false;
            MapGroupProtoTabButton.Checked = false;
            TerritoriesTabButton.Checked = false;
            EconomySearchBoxTB.Visible = false;
            EconomyFindButton.Visible = false;
            economySearchNextButton.Visible = false;
            InitCTabButton.Checked = false;
            SpawnGearTabButton.Checked = false;
            MapGroupPosTabButton.Checked = false;
        }
        #endregion formsstuff
        #region Types
        public void SetSummarytiers()
        {
            Console.WriteLine("Loading SummaryPage");
            List<CheckBox> checkboxesSummary = SetdefinitionsTPSummary.Controls.OfType<CheckBox>().ToList();
            foreach (CheckBox cb in checkboxesSummary)
            {
                cb.Visible = false;
            }
            int index = currentproject.limitfefinitions.lists.valueflags.Count;
            for (int i = 0; i < currentproject.limitfefinitions.lists.valueflags.Count; i++)
            {
                listsValue value = currentproject.limitfefinitions.lists.valueflags[i];
                CheckBox cb = checkboxesSummary[i];
                cb.Tag = value.name;
                cb.Checked = false;
                cb.Visible = true;
                cb.Text = value.name;
                index--;
            }

            checkboxesSummary = UserdefinitionsTPSummary.Controls.OfType<CheckBox>().ToList();
            checkboxesSummary = checkboxesSummary.OrderBy(x => x.Tag.ToString()).ToList();
            foreach (CheckBox cb in checkboxesSummary)
            {
                cb.Visible = false;
            }
            index = 0;
            foreach (user_listsUser1 user in currentproject.limitfefinitionsuser.user_lists.valueflags)
            {
                CheckBox cb = checkboxesSummary[index];
                cb.Tag = user.name;
                cb.Visible = true;
                cb.Checked = false;
                cb.Text = user.name;
                index++;
            }
        }
        public void setNumberofTiers()
        {
            List<CheckBox> checkboxes = flowLayoutPanel1.Controls.OfType<CheckBox>().ToList();

            foreach (CheckBox cb in checkboxes)
            {
                cb.Visible = false;
            }

            int index = currentproject.limitfefinitions.lists.valueflags.Count;
            for (int i = 0; i < currentproject.limitfefinitions.lists.valueflags.Count; i++)
            {
                listsValue value = currentproject.limitfefinitions.lists.valueflags[i];
                CheckBox cb = checkboxes[i];
                cb.Tag = value.name;
                cb.Checked = false;
                cb.Visible = true;
                cb.Text = value.name;
                index--;
            }

            checkboxes = UserdefinitionsTP.Controls.OfType<CheckBox>().ToList();
            checkboxes = checkboxes.OrderBy(x => x.Name).ToList();
            foreach (CheckBox cb in checkboxes)
            {
                cb.Visible = false;
            }


            index = 0;
            foreach (user_listsUser1 user in currentproject.limitfefinitionsuser.user_lists.valueflags)
            {
                CheckBox cb = checkboxes[index];
                cb.Tag = user.name;
                cb.Visible = true;
                cb.Checked = false;
                cb.Text = user.name;
                index++;
            }

            //if (currentlootpart.Usinguserdifinitions)
            //{
            //    tabControl3.SelectedIndex = 1;
            //}
            //else
            //    tabControl3.SelectedIndex = 0;
        }
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            Loot_Info f2 = new Loot_Info();
            f2.ShowDialog();
        }
        private void PopulateTreeView()
        {
            Console.WriteLine("populating Types treeView");
            treeViewMS1.Nodes.Clear();
            TreeNode root = new TreeNode(Path.GetFileName(filename))
            {
                Tag = "Parent"
            };
            //Set Vanilla Treenode types
            TreeNode vanilla = new TreeNode("Vanilla Types")
            {
                Tag = "VanillaTypes"
            };
            Console.WriteLine("populating Vanilla types to treeview");
            foreach (typesType type in vanillatypes.types.type)
            {
                string cat = "other";
                if (type.category != null)
                    cat = type.category.name;
                TreeNode typenode = new TreeNode(type.name)
                {
                    Tag = type
                };
                if (!vanilla.Nodes.ContainsKey(cat))
                {
                    TreeNode newcatnode = new TreeNode(cat)
                    {
                        Name = cat,
                        Tag = cat
                    };
                    vanilla.Nodes.Add(newcatnode);
                }
                vanilla.Nodes[cat].Nodes.Add(typenode);
            }
            root.Nodes.Add(vanilla);
            if (ModTypes.Count > 0)
            {
                foreach (TypesFile tf in ModTypes)
                {
                    Console.WriteLine("populating " + tf.modname + " to treeview");
                    TreeNode ModTypes = new TreeNode(tf.modname)
                    {
                        Tag = tf.modname
                    };
                    foreach (typesType type in tf.types.type)
                    {
                        string cat = "other";
                        if (type.category != null)
                            cat = type.category.name;
                        
                        TreeNode typenode = new TreeNode(type.name)
                        {
                            Tag = type
                        };
                        if (!ModTypes.Nodes.ContainsKey(cat))
                        {
                            TreeNode newcatnode = new TreeNode(cat)
                            {
                                Name = cat,
                                Tag = cat
                            };
                            ModTypes.Nodes.Add(newcatnode);
                        }
                        ModTypes.Nodes[cat].Nodes.Add(typenode);
                    }


                    root.Nodes.Add(ModTypes);
                }
            }
            Console.WriteLine("All Types files Populated......");
            //treeView1.Nodes.Add(root);
            treeViewMS1.Nodes.Add(root);
        }
        private void treeViewMS1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //TreeNode usingtreenode = e.Node;
            //if (ModifierKeys.HasFlag(Keys.Control) || ModifierKeys.HasFlag(Keys.Shift))
            //{
            //    return;
            //}

            //if (usingtreenode.Tag != null && usingtreenode.Tag is typesType)
            //{
            //    TreeNode parent = usingtreenode.Parent;
            //    TreeNode mainparent = parent.Parent;
            //    currentcollection = parent.Text;
            //    String typesfile = mainparent.Text;
            //    if (typesfile == "Vanilla Types")
            //        currentTypesFile = vanillatypes;
            //    else
            //        currentTypesFile = ModTypes.FirstOrDefault(x => x.modname == typesfile);
            //    isUserInteraction = false;
            //    tabControl1.SelectedIndex = 1;
            //    currentlootpart = usingtreenode.Tag as typesType;
            //    PopulateLootPartInfo();
            //    isUserInteraction = true;
            //}
            //else if (usingtreenode.Tag != null && usingtreenode.Tag is string)
            //{
            //    tabControl1.SelectedIndex = 0;
            //    currentcollection = usingtreenode.Tag.ToString();
            //    darkLabel2.Text = currentcollection;
            //    TreeNode parent = usingtreenode.Parent;

            //    if (parent != null && parent.Tag.ToString() == "Parent")
            //    {
            //        FullTypes = true;
            //        if (currentcollection == "VanillaTypes")
            //        {
            //            currentTypesFile = vanillatypes;
            //        }
            //        else
            //        {
            //            currentTypesFile = ModTypes.FirstOrDefault(x => x.modname == currentcollection);
            //        }
            //    }
            //    else if (parent != null)
            //    {
            //        FullTypes = false;
            //        if (parent.Text == "Vanilla Types")
            //            currentTypesFile = vanillatypes;
            //        else
            //        {
            //            currentTypesFile = ModTypes.FirstOrDefault(x => x.modname == parent.Text);
            //        }
            //    }
            //}
            //this.treeViewMS1.SelectedNode = usingtreenode;
        }
        private void treeViewMS1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode usingtreenode = e.Node;
            if (ModifierKeys.HasFlag(Keys.Control) || ModifierKeys.HasFlag(Keys.Shift))
            {
                return;
            }
            if (usingtreenode.Tag != null && usingtreenode.Tag is typesType)
            {
                TreeNode parent = usingtreenode.Parent;
                TreeNode mainparent = parent.Parent;
                currentcollection = parent.Text;
                String typesfile = mainparent.Text;
                if (typesfile == "Vanilla Types")
                    currentTypesFile = vanillatypes;
                else
                    currentTypesFile = ModTypes.FirstOrDefault(x => x.modname == typesfile);
                isUserInteraction = false;
                tabControl1.SelectedIndex = 0;
                currentlootpart = usingtreenode.Tag as typesType;
                PopulateLootPartInfo();
                isUserInteraction = true;
                if (e.Button == MouseButtons.Right)
                {
                    // Display context menu for eg:
                    DeleteTypesTSMI.Visible = false;
                    AddTypesTSMI.Visible = false;
                    DeleteSpecificTypeTSMI.Visible = true;
                    exportAllToExcelToolStripMenuItem.Visible = false;
                    DeleteSpecificTypeTSMI.Text = "Delete " + currentlootpart.name + " from " + currentcollection + " in " + typesfile;
                    checkForDuplicateTypesTSMI.Visible = false;
                    TypesContextMenu.Show(Cursor.Position);
                }
            }
            else if (usingtreenode.Tag != null && usingtreenode.Tag is string)
            {
                tabControl1.SelectedIndex = 1;
                currentcollection = usingtreenode.Tag.ToString();
                darkLabel2.Text = currentcollection;
                TreeNode parent = usingtreenode.Parent;

                if (parent != null && parent.Tag.ToString() == "Parent")
                {
                    FullTypes = true;
                    if (currentcollection == "VanillaTypes")
                    {
                        currentTypesFile = vanillatypes;
                        if (e.Button == MouseButtons.Right)
                        {
                            // Display context menu for eg:
                            DeleteSpecificTypeTSMI.Visible = false;
                            DeleteTypesTSMI.Visible = false;
                            AddTypesTSMI.Visible = true;
                            exportAllToExcelToolStripMenuItem.Visible = false;
                            AddTypesTSMI.Text = "Add new Types to Vanilla Types";
                            checkForDuplicateTypesTSMI.Visible = false;
                            TypesContextMenu.Show(Cursor.Position);

                        }
                    }
                    else
                    {
                        currentTypesFile = ModTypes.FirstOrDefault(x => x.modname == currentcollection);
                        if (e.Button == MouseButtons.Right)
                        {
                            DeleteSpecificTypeTSMI.Visible = false;
                            // Display context menu for eg:
                            if (currentTypesFile.modname != "expansion_types")
                                DeleteTypesTSMI.Visible = true;
                            else
                                DeleteTypesTSMI.Visible = false;
                            DeleteTypesTSMI.Text = "Delete " + currentTypesFile.modname;
                            AddTypesTSMI.Visible = true;
                            AddTypesTSMI.Text = "Add new Types to " + currentTypesFile.modname;
                            checkForDuplicateTypesTSMI.Visible = false;
                            exportAllToExcelToolStripMenuItem.Visible = false;
                            TypesContextMenu.Show(Cursor.Position);
                        }
                    }
                }
                else if (parent != null)
                {
                    FullTypes = false;
                    if (parent.Text == "Vanilla Types")
                        currentTypesFile = vanillatypes;
                    else
                    {
                        currentTypesFile = ModTypes.FirstOrDefault(x => x.modname == parent.Text);
                    }
                    if (e.Button == MouseButtons.Right)
                    {
                        DeleteSpecificTypeTSMI.Visible = false;
                        DeleteTypesTSMI.Visible = true;
                        AddTypesTSMI.Visible = false;
                        checkForDuplicateTypesTSMI.Visible = false;
                        exportAllToExcelToolStripMenuItem.Visible = false;
                        DeleteTypesTSMI.Text = "Delete " + currentcollection + " from " + currentTypesFile.modname;
                        TypesContextMenu.Show(Cursor.Position);
                    }
                }
                else if (currentcollection == "Parent")
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        DeleteSpecificTypeTSMI.Visible = false;
                        AddTypesTSMI.Visible = true;
                        DeleteTypesTSMI.Visible = false;
                        checkForDuplicateTypesTSMI.Visible = true;
                        exportAllToExcelToolStripMenuItem.Visible = true;
                        AddTypesTSMI.Text = "Add new Types to Custom Folder";
                        TypesContextMenu.Show(Cursor.Position);
                    }
                }
            }
            this.treeViewMS1.SelectedNode = usingtreenode;
        }
        private void checkForDuplicateTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Tuple<string, string>> typecheck = new List<Tuple<string, string>>();
            List<Tuple<string, string, string>> duplicatelist = new List<Tuple<string, string, string>>();
            foreach (typesType type in vanillatypes.types.type)
            {
                if (typecheck.Any(x => x.Item1 == type.name))
                {

                    Tuple<string, string> first = typecheck.FirstOrDefault(x => x.Item1 == type.name);
                    duplicatelist.Add(new Tuple<string, string, string>(type.name, first.Item2, Path.GetFileNameWithoutExtension(vanillatypes.Filename)));
                }
                else
                {
                    typecheck.Add(new Tuple<string, string>(type.name, Path.GetFileNameWithoutExtension(vanillatypes.Filename)));
                }
            }
            if (ModTypes.Count > 0)
            {
                foreach (TypesFile tf in ModTypes)
                {
                    foreach (typesType type in tf.types.type)
                    {
                        if (typecheck.Any(x => x.Item1 == type.name))
                        {
                            Tuple<string, string> first = typecheck.FirstOrDefault(x => x.Item1 == type.name);
                            duplicatelist.Add(new Tuple<string, string, string>(type.name, first.Item2, Path.GetFileNameWithoutExtension(tf.Filename)));
                        }
                        else
                        {
                            typecheck.Add(new Tuple<string, string>(type.name, Path.GetFileNameWithoutExtension(tf.Filename)));
                        }
                    }
                }
            }
            if (duplicatelist.Count > 0)
            {
                Console.WriteLine("");
                Console.WriteLine("Duplicate types found");
                foreach (Tuple<string, string, string> tup in duplicatelist)
                {
                    Console.WriteLine(tup.Item1 + " is in both " + tup.Item2 + " and " + tup.Item3);
                }
                MessageBox.Show("Duplicates found, Please see console");
            }
        }
        private void DeleteSpecificTypeTSMI_Click(object sender, EventArgs e)
        {
            foreach (TreeNode tn in treeViewMS1.SelectedNodes)
            {
                typesType looptype = tn.Tag as typesType;
                currentTypesFile.types.type.Remove(looptype);
            }
            currentTypesFile.SaveTyes(DateTime.Now.ToString("ddMMyy_HHmm"));
            var savedExpansionState = treeViewMS1.Nodes.GetExpansionState();
            treeViewMS1.BeginUpdate();
            PopulateTreeView();
            treeViewMS1.Nodes.SetExpansionState(savedExpansionState);
            treeViewMS1.EndUpdate();
            populateEconmyTreeview();
            currentproject.SetTotNomCount();
            NomCountLabel.Text = "Total Nominal Count :- " + currentproject.TotalNomCount.ToString();
        }
        private void DeleteTypesTSMI_Click(object sender, EventArgs e)
        {
            if (currentcollection == currentTypesFile.modname)
            {
                if (MessageBox.Show("This Will Remove The types file from the Project, Are you sure you want to do this?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    string Modname = currentTypesFile.modname;
                    currentproject.EconomyCore.RemoveCe(Modname, out string foflderpath, out string filename, out bool deletedirectory);
                    if (currentproject.EconomyCore.checkiftodelete(Modname))
                    {
                        File.Delete(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\" + foflderpath + "\\" + filename);
                        if (deletedirectory)
                            Directory.Delete(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\" + foflderpath, true);
                    }
                    currentproject.EconomyCore.SaveEconomycore();
                    currentproject.removeMod(currentTypesFile.modname);
                    ModTypes = currentproject.getModList();

                    var savedExpansionState = treeViewMS1.Nodes.GetExpansionState();
                    treeViewMS1.BeginUpdate();
                    PopulateTreeView();
                    treeViewMS1.Nodes.SetExpansionState(savedExpansionState);
                    treeViewMS1.EndUpdate();
                    MessageBox.Show("Mod Removed from Project....", "Done", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            else
            {
                List<typesType> typetoremove = new List<typesType>();
                foreach (typesType type in currentTypesFile.types.type)
                {
                    if (type.category == null && currentcollection == "other")
                    {
                        typetoremove.Add(type);
                    }
                    else if (type.category != null && type.category.name == currentcollection)
                    {
                        typetoremove.Add(type);
                    }
                }
                foreach (typesType t in typetoremove)
                {
                    currentTypesFile.types.type.Remove(t);
                }
                currentTypesFile.SaveTyes(DateTime.Now.ToString("ddMMyy_HHmm"));
                var savedExpansionState = treeViewMS1.Nodes.GetExpansionState();
                treeViewMS1.BeginUpdate();
                PopulateTreeView();
                treeViewMS1.Nodes.SetExpansionState(savedExpansionState);
                treeViewMS1.EndUpdate();
            }
            populateEconmyTreeview();
            currentproject.SetTotNomCount();
            NomCountLabel.Text = "Total Nominal Count :- " + currentproject.TotalNomCount.ToString();
        }
        private static string ReplaceCaseInsensitive(string input, string search, string replacement)
        {
            string result = Regex.Replace(
                input,
                Regex.Escape(search),
                replacement.Replace("$", "$$"),
                RegexOptions.IgnoreCase
            );
            return result;
        }
        private void AddtypesTSMI_Click(object sender, EventArgs e)
        {
            List<typesType> Addedtypes = new List<typesType>();
            AddNewTypes form = new AddNewTypes
            {
                currentproject = currentproject
            };
            if (currentcollection == "Parent")
            {
                form.newlocation = true;
            }
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (currentcollection == "Parent")
                {
                    string path = form.CustomLocation;
                    string modname = form.TypesName;
                    Directory.CreateDirectory(path);
                    string line1 = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>";
                    string line2 = "<types>";
                    List<string> modtypes = form.modtypes;
                    string last = "</types>";
                    modtypes.Insert(0, line2);
                    modtypes.Insert(0, line1);
                    modtypes.Add(last);
                    File.WriteAllLines(path + "\\" + modname + "_types.xml", modtypes);
                    TypesFile test = new TypesFile(path + "\\" + modname + "_types.xml");
                    test.SaveTyes();
                    string pathreplacer = currentproject.projectFullName + @"\mpmissions\" + currentproject.mpmissionpath + @"\";
                    string pathrep = ReplaceCaseInsensitive(path, pathreplacer, "");
                    currentproject.EconomyCore.AddCe(pathrep, modname + "_types.xml", "types");
                    currentproject.EconomyCore.SaveEconomycore();
                    currentproject.SetModListtypes();
                    ModTypes = currentproject.getModList();
                    var savedExpansionState = treeViewMS1.Nodes.GetExpansionState();
                    treeViewMS1.BeginUpdate();
                    PopulateTreeView();
                    treeViewMS1.Nodes.SetExpansionState(savedExpansionState);
                    treeViewMS1.EndUpdate();
                }
                else if (currentcollection == "VanillaTypes")
                {
                    string line1 = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>";
                    string line2 = "<types>";
                    List<string> modtypes = form.modtypes;
                    string last = "</types>";
                    modtypes.Insert(0, line2);
                    modtypes.Insert(0, line1);
                    modtypes.Add(last);
                    File.WriteAllLines("Temp_types.xml", modtypes);
                    TypesFile test = new TypesFile("Temp_types.xml");
                    File.Delete("Temp_types.xml");
                    Console.WriteLine("The following Types have been added....");
                    foreach (typesType type in test.types.type)
                    {
                        if (!vanillatypes.types.type.Any(x => x.name.ToLower() == type.name.ToLower()))
                        {
                            Addedtypes.Add(type);
                            Console.WriteLine(type.name);
                            vanillatypes.types.type.Add(type);
                        }
                    }
                    vanillatypes.SaveTyes(DateTime.Now.ToString("ddMMyy_HHmm"));
                    var savedExpansionState = treeViewMS1.Nodes.GetExpansionState();
                    treeViewMS1.BeginUpdate();
                    PopulateTreeView();
                    treeViewMS1.Nodes.SetExpansionState(savedExpansionState);
                    treeViewMS1.EndUpdate();
                }
                else
                {
                    currentTypesFile = ModTypes.FirstOrDefault(x => x.modname == currentcollection);
                    string line1 = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>";
                    string line2 = "<types>";
                    List<string> modtypes = form.modtypes;
                    string last = "</types>";
                    modtypes.Insert(0, line2);
                    modtypes.Insert(0, line1);
                    modtypes.Add(last);
                    File.WriteAllLines("Temp_types.xml", modtypes);
                    TypesFile test = new TypesFile("Temp_types.xml");
                    File.Delete("Temp_types.xml");
                    foreach (typesType type in test.types.type)
                    {
                        currentTypesFile.types.type.Add(type);
                    }
                    currentTypesFile.SaveTyes(DateTime.Now.ToString("ddMMyy_HHmm"));
                    var savedExpansionState = treeViewMS1.Nodes.GetExpansionState();
                    treeViewMS1.BeginUpdate();
                    PopulateTreeView();
                    treeViewMS1.Nodes.SetExpansionState(savedExpansionState);
                    treeViewMS1.EndUpdate();
                }
            }
            populateEconmyTreeview();
            currentproject.SetTotNomCount();
            NomCountLabel.Text = "Total Nominal Count :- " + currentproject.TotalNomCount.ToString();
        }
        private void exportAllToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Filename,Classname,Nomainal,min,Lifetime,Restock,QuantMin,QuantMaxm,Cost,CountInCargo,CountInhoarder,CountInMap,CountInPlayer,Crafter,Deloot,Category,Usage,Tag" + Environment.NewLine);
            string filename = Path.GetFileName(vanillatypes.Filename);
            foreach(typesType type in  vanillatypes.types.type)
            {
                sb.Append(GetStringline(type, filename));
            }
            foreach (TypesFile tf in ModTypes)
            {
                filename = Path.GetFileName(tf.Filename);
                foreach (typesType type in tf.types.type)
                {
                    sb.Append(GetStringline(type, filename));
                }
            }
            SaveFileDialog save = new SaveFileDialog();
            if(save.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(save.FileName, sb.ToString());
            }

        }
        private string GetStringline(typesType type, string filename)
        {
            string line = "";
            line += filename + ",";
            line += type.name;
            line += ",";
            if(type.nominalSpecified)
                line += type.nominal;
            line += ",";
            if(type.minSpecified)
                line += type.min;
            line += ",";
            if(type.lifetimeSpecified)
                line += type.lifetime;
            line += ",";
            if (type.restockSpecified)
                line += type.restock;
            line += ",";
            if (type.quantminSpecified)
                line += type.quantmin;
            line += ",";
            if(type.quantmaxSpecified)
                line += type.quantmax;
            line += ",";
            if(type.costSpecified)
                line += type.cost;
            line += ",";
            if (type.flags != null)
            {
                line += type.flags.count_in_cargo == 1 ? true : false;
                line += ",";
                line += type.flags.count_in_hoarder == 1 ? true : false;
                line += ",";
                line += type.flags.count_in_map == 1 ? true : false;
                line += ",";
                line += type.flags.count_in_player == 1 ? true : false;
                line += ",";
                line += type.flags.crafted == 1 ? true : false;
                line += ",";
                line += type.flags.deloot == 1 ? true : false;
                line += ",";
            }
            else
            {
                line += ",,,,,,";
            }
            if(type.category != null)
                line += type.category.name;
            line += ",";
            if(type.usage != null || type.usage.Count != 0)
            {
                foreach(typesTypeUsage u in type.usage)
                {
                    line += u.name + " ";
                }
            }
            line += ",";
            if (type.tag != null || type.tag.Count != 0)
            {
                foreach (typesTypeTag t in type.tag)
                {
                    line += t.name + " ";
                }
            }
            line += Environment.NewLine;
            return line;
        }
        private void PopulateLootPartInfo()
        {
            if (currentlootpart == null) return;
            isUserInteraction = false;
            textBox1.Text = currentlootpart.name;
            if (currentlootpart.category == null)
                comboBox1.SelectedIndex = 0;
            else
                comboBox1.SelectedIndex = comboBox1.FindStringExact(currentlootpart.category.name);
            populateUsage();
            PopulateCounts();
            PopulateFlags();
            PopulateTiers();
            PopulateTags();
            isUserInteraction = true;
        }
        private void PopulateTiers()
        {
            setNumberofTiers();
            if (currentlootpart.value != null)
            {
                for (int i = 0; i < currentlootpart.value.Count; i++)
                {
                    if (currentlootpart.value[i].user != null && currentlootpart.value[i].user.Count() > 0 && currentlootpart.value[i].name == null)
                    {
                        tabControl3.SelectedIndex = 1;
                        try
                        {
                            UserdefinitionsTP.Controls.OfType<CheckBox>().First(x => x.Tag.ToString() == currentlootpart.value[i].user).Checked = true;
                        }
                        catch
                        {
                            currentlootpart.value.RemoveAt(i);
                            i--;
                        }
                    }
                    else
                    {
                        tabControl3.SelectedIndex = 0;

                        try
                        {
                            flowLayoutPanel1.Controls.OfType<CheckBox>().First(x => x.Tag.ToString() == currentlootpart.value[i].name).Checked = true;
                        }
                        catch
                        {
                            currentlootpart.value.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
        }
        private void PopulateCounts()
        {
            if (typeNomCountNUD.Visible = NomCountCB.Checked = currentlootpart.nominalSpecified)
                typeNomCountNUD.Value = (decimal)currentlootpart.nominal;
            if (typeMinCountNUD.Visible = MinCountCB.Checked = currentlootpart.minSpecified)
                typeMinCountNUD.Value = (decimal)currentlootpart.min;
            typeLifetimeNUD.Visible = true;
            typeLifetimeNUD.Value = (decimal)currentlootpart.lifetime;
            if (typeRestockNUD.Visible = RestockCB.Checked = currentlootpart.restockSpecified)
                typeRestockNUD.Value = (decimal)currentlootpart.restock;
            if (typeQuantMINNUD.Visible = QuanMinCB.Checked = currentlootpart.quantminSpecified)
                typeQuantMINNUD.Value = (decimal)currentlootpart.quantmin;
            if (typeQuantMAXNUD.Visible = QuanMaxCB.Checked = currentlootpart.quantmaxSpecified)
                typeQuantMAXNUD.Value = (decimal)currentlootpart.quantmax;
            if (typeCostNUD.Visible = costCB.Checked = currentlootpart.costSpecified)
                typeCostNUD.Value = (decimal)currentlootpart.cost;
        }
        private void NomCountCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            foreach (TreeNode tn in treeViewMS1.SelectedNodes)
            {
                typesType looptype = tn.Tag as typesType;
                if (typeNomCountNUD.Visible = looptype.nominalSpecified = NomCountCB.Checked)
                {
                    typeNomCountNUD.Value = 0;
                    looptype.nominal = (int)typeNomCountNUD.Value;
                    currentTypesFile.isDirty = true;
                    currentproject.SetTotNomCount();
                }
            }
            NomCountLabel.Text = "Total Nominal Count :- " + currentproject.TotalNomCount.ToString();
        }
        private void MinCountCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            foreach (TreeNode tn in treeViewMS1.SelectedNodes)
            {
                typesType looptype = tn.Tag as typesType;
                if (typeMinCountNUD.Visible = looptype.minSpecified = MinCountCB.Checked)
                {
                    typeMinCountNUD.Value = 0;
                    looptype.min = (int)typeMinCountNUD.Value;
                    currentTypesFile.isDirty = true;
                }
            }
        }
        private void RestockCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            foreach (TreeNode tn in treeViewMS1.SelectedNodes)
            {
                typesType looptype = tn.Tag as typesType;
                if (typeRestockNUD.Visible = looptype.restockSpecified = RestockCB.Checked)
                {
                    typeRestockNUD.Value = 900;
                    looptype.restock = (int)typeRestockNUD.Value;
                    currentTypesFile.isDirty = true;
                }
            }
        }
        private void QuanMinCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            foreach (TreeNode tn in treeViewMS1.SelectedNodes)
            {
                typesType looptype = tn.Tag as typesType;
                if (typeQuantMINNUD.Visible = looptype.minSpecified = QuanMinCB.Checked)
                {
                    typeQuantMINNUD.Value = -1;
                    looptype.quantmin = (int)typeQuantMINNUD.Value;
                    currentTypesFile.isDirty = true;
                }
            }
        }
        private void QuanMaxCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            foreach (TreeNode tn in treeViewMS1.SelectedNodes)
            {
                typesType looptype = tn.Tag as typesType;
                if (typeQuantMAXNUD.Visible = looptype.quantmaxSpecified = QuanMaxCB.Checked)
                {
                    typeQuantMAXNUD.Value = -1;
                    looptype.quantmax = (int)typeQuantMAXNUD.Value;
                    currentTypesFile.isDirty = true;
                }
            }
        }
        private void costCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            foreach (TreeNode tn in treeViewMS1.SelectedNodes)
            {
                typesType looptype = tn.Tag as typesType;
                if (typeCostNUD.Visible = looptype.costSpecified = costCB.Checked)
                {
                    typeCostNUD.Value = 100;
                    looptype.cost = (int)typeCostNUD.Value; ;
                    currentTypesFile.isDirty = true;
                }
            }
        }
        private void populateUsage()
        {
            listBox1.DisplayMember = "Name";
            listBox1.ValueMember = "Value";
            listBox1.DataSource = currentlootpart.usage;
        }
        private void PopulateFlags()
        {
            if (currentlootpart.flags != null)
            {
                checkBox1.Checked = currentlootpart.flags.count_in_cargo == 1 ? true : false;
                checkBox2.Checked = currentlootpart.flags.count_in_hoarder == 1 ? true : false;
                checkBox3.Checked = currentlootpart.flags.count_in_map == 1 ? true : false;
                checkBox4.Checked = currentlootpart.flags.count_in_player == 1 ? true : false;
                checkBox5.Checked = currentlootpart.flags.crafted == 1 ? true : false;
                checkBox6.Checked = currentlootpart.flags.deloot == 1 ? true : false;
            }
            else
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;
            }
        }
        private void PopulateTags()
        {
            listBox2.DisplayMember = "Name";
            listBox2.ValueMember = "Value";
            listBox2.DataSource = currentlootpart.tag;
        }
        private void darkButton2_Click(object sender, EventArgs e)
        {
            listsUsage u = comboBox2.SelectedItem as listsUsage;
            foreach (TreeNode tn in treeViewMS1.SelectedNodes)
            {
                typesType looptype = tn.Tag as typesType;
                looptype.AddnewUsage(u);
                populateUsage();
                currentTypesFile.isDirty = true;
            }
        }
        private void darkButton1_Click(object sender, EventArgs e)
        {
            typesTypeUsage u = listBox1.SelectedItem as typesTypeUsage;
            foreach (TreeNode tn in treeViewMS1.SelectedNodes)
            {
                typesType looptype = tn.Tag as typesType;
                looptype.removeusage(u);
                currentTypesFile.isDirty = true;
            }
        }
        private void darkButton8_Click(object sender, EventArgs e)
        {
            listsTag t = comboBox4.SelectedItem as listsTag;
            foreach (TreeNode tn in treeViewMS1.SelectedNodes)
            {
                typesType looptype = tn.Tag as typesType;
                looptype.Addnewtag(t);
                currentTypesFile.isDirty = true;
                PopulateTags();
            }
        }
        private void darkButton7_Click(object sender, EventArgs e)
        {
            typesTypeTag t = listBox2.SelectedItem as typesTypeTag;
            foreach (TreeNode tn in treeViewMS1.SelectedNodes)
            {
                typesType looptype = tn.Tag as typesType;
                looptype.removetag(t);
                currentTypesFile.isDirty = true;
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                listsCategory c = comboBox1.SelectedItem as listsCategory;
                foreach (TreeNode tn in treeViewMS1.SelectedNodes)
                {
                    typesType looptype = tn.Tag as typesType;
                    looptype.changecategory(c);
                }
                currentTypesFile.isDirty = true;
                PopulateTreeView();
                isUserInteraction = false;
            }
        }
        private void typeNomCountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                foreach (TreeNode tn in treeViewMS1.SelectedNodes)
                {
                    typesType looptype = tn.Tag as typesType;
                    if (looptype.nominalSpecified)
                    {
                        looptype.nominal = (int)typeNomCountNUD.Value;
                        currentTypesFile.isDirty = true;
                        currentproject.SetTotNomCount();
                    }
                }
                NomCountLabel.Text = "Total Nominal Count :- " + currentproject.TotalNomCount.ToString();
            }
        }
        private void typeMinCountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                foreach (TreeNode tn in treeViewMS1.SelectedNodes)
                {
                    typesType looptype = tn.Tag as typesType;
                    if (looptype.minSpecified)
                    {
                        looptype.min = (int)typeMinCountNUD.Value;
                        currentTypesFile.isDirty = true;
                    }
                }
            }
        }
        private void typeLifetimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                foreach (TreeNode tn in treeViewMS1.SelectedNodes)
                {
                    typesType looptype = tn.Tag as typesType;
                    if (looptype.lifetimeSpecified)
                    {
                        looptype.lifetime = (int)typeLifetimeNUD.Value;
                        currentTypesFile.isDirty = true;
                    }
                }
            }
        }
        private void typeRestockNUD_ValueChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                foreach (TreeNode tn in treeViewMS1.SelectedNodes)
                {
                    typesType looptype = tn.Tag as typesType;
                    if (looptype.restockSpecified)
                    {
                        looptype.restock = (int)typeRestockNUD.Value;
                        currentTypesFile.isDirty = true;
                    }
                }
            }

        }
        private void typeQuantMINNUD_ValueChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                foreach (TreeNode tn in treeViewMS1.SelectedNodes)
                {
                    typesType looptype = tn.Tag as typesType;
                    if (looptype.quantminSpecified)
                    {
                        looptype.quantmin = (int)typeQuantMINNUD.Value;
                        currentTypesFile.isDirty = true;
                    }
                }
            }
        }
        private void typeQuantMAXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                foreach (TreeNode tn in treeViewMS1.SelectedNodes)
                {
                    typesType looptype = tn.Tag as typesType;
                    if (looptype.quantmaxSpecified)
                    {
                        looptype.quantmax = (int)typeQuantMAXNUD.Value;
                        currentTypesFile.isDirty = true;
                    }
                }
            }
        }
        private void typeCostNUD_ValueChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                foreach (TreeNode tn in treeViewMS1.SelectedNodes)
                {
                    typesType looptype = tn.Tag as typesType;
                    if (looptype.costSpecified)
                    {
                        looptype.cost = (int)typeCostNUD.Value; ;
                        currentTypesFile.isDirty = true;
                    }
                }
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                foreach (TreeNode tn in treeViewMS1.SelectedNodes)
                {
                    typesType looptype = tn.Tag as typesType;
                    CheckBox cb = sender as CheckBox;
                    switch (cb.Name)
                    {
                        case "checkBox1":
                            looptype.flags.count_in_cargo = checkBox1.Checked == true ? 1 : 0;
                            break;
                        case "checkBox2":
                            looptype.flags.count_in_hoarder = checkBox2.Checked == true ? 1 : 0;
                            break;
                        case "checkBox3":
                            looptype.flags.count_in_map = checkBox3.Checked == true ? 1 : 0;
                            break;
                        case "checkBox4":
                            looptype.flags.count_in_player = checkBox4.Checked == true ? 1 : 0;
                            break;
                        case "checkBox5":
                            looptype.flags.crafted = checkBox5.Checked == true ? 1 : 0;
                            break;
                        case "checkBox6":
                            looptype.flags.deloot = checkBox6.Checked == true ? 1 : 0;
                            break;
                    }
                    currentTypesFile.isDirty = true;
                }
            }
        }
        private void TierCheckBoxchanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                CheckBox cb = sender as CheckBox;
                string tier = cb.Tag.ToString();
                foreach (TreeNode tn in treeViewMS1.SelectedNodes)
                {
                    typesType looptype = tn.Tag as typesType;
                    if (cb.Checked)
                        looptype.AddTier(tier);
                    else
                        looptype.removetier(tier);
                    currentTypesFile.isDirty = true;
                }
                PopulateLootPartInfo();
            }
        }
        private void UserdefiniedTiersChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                CheckBox cb = sender as CheckBox;
                string tier = cb.Tag.ToString();
                foreach (TreeNode tn in treeViewMS1.SelectedNodes)
                {
                    typesType looptype = tn.Tag as typesType;
                    if (cb.Checked)
                    {
                        if (looptype.value != null)
                        {
                            looptype.removetiers();
                        }
                        looptype.AdduserTier(tier);
                    }
                    else
                        looptype.removeusertier(tier);
                }
                currentTypesFile.isDirty = true;
                PopulateLootPartInfo();
            }
        }
        private void darkButton28_Click(object sender, EventArgs e)
        {
            foreach (TreeNode tn in treeViewMS1.SelectedNodes)
            {
                typesType looptype = tn.Tag as typesType;
                looptype.removetiers();
                currentTypesFile.isDirty = true;
            }
            PopulateLootPartInfo();
        }
        private void darkButton3_Click(object sender, EventArgs e)
        {
            List<typesType> loot = new List<typesType>();

            if (currentcollection == "Parent")
            {
                if (vanillatypes.types.type != null)
                {
                    loot = vanillatypes.types.type.ToList();
                    editnommax(loot);
                }
                foreach (TypesFile tf in ModTypes)
                {
                    if (tf.types.type != null)
                    {
                        loot = tf.types.type.ToList();
                        editnommax(loot);
                    }
                }
            }
            else if (FullTypes)
            {
                loot = currentTypesFile.types.type.ToList();
                editnommax(loot);
            }
            else
            {

                if (currentcollection == "other")
                {
                    loot = currentTypesFile.types.type.Where(x => x.category == null).ToList();
                    editnommax(loot);
                }
                else
                {
                    loot = currentTypesFile.types.type.Where(x => x.category != null && x.category.name == currentcollection).ToList();
                    editnommax(loot);
                }
            }
            currentproject.SetTotNomCount();
            NomCountLabel.Text = "Total Nominal Count :- " + currentproject.TotalNomCount.ToString();
        }
        public void editnommax(List<typesType> loot)
        {
            currentTypesFile.isDirty = true;
            Domultiplier(loot);
        }
        private void Domultiplier(List<typesType> list)
        {
            foreach (typesType item in list)
            {
                if (!item.nominalSpecified) { continue; }
                if (item.nominal != 0)
                {
                    item.nominal = getmultiplier((int)item.nominal);
                    if (item.nominal == 0 && if0setto1CB.Checked)
                        item.nominal = 1;
                }
                if (ChangeMinCheckBox.Checked)
                {
                    if (item.min != 0)
                    {
                        item.min = getmultiplier((int)item.min);
                        if (item.min == 0 && if0setto1CB.Checked)
                            item.min = 1;
                    }
                }
            }
        }
        private int getmultiplier(int nominal)
        {
            switch (comboBox3.SelectedIndex)
            {
                case 0:
                    return nominal * 10;
                case 1:
                    return nominal * 9;
                case 2:
                    return nominal * 8;
                case 3:
                    return nominal * 7;
                case 4:
                    return nominal * 6;
                case 5:
                    return nominal * 5;
                case 6:
                    return nominal * 4;
                case 7:
                    return nominal * 3;
                case 8:
                    return nominal * 2;
                case 9:
                    return (int)((float)(nominal * 1.5));
                case 10:
                    return (int)((float)(nominal / 1.5));
                case 11:
                    return nominal / 2;
                case 12:
                    return nominal / 3;
                case 13:
                    return nominal / 4;
                case 14:
                    return nominal / 5;
                case 15:
                    return nominal / 6;
                case 16:
                    return nominal / 7;
                case 17:
                    return nominal / 8;
                case 18:
                    return nominal / 9;
                case 19:
                    return nominal / 10;
                default:
                    return 0;
            }
        }
        private void darkButton6_Click(object sender, EventArgs e)
        {
            Updatflag(sender);
        }
        private void Updatflag(object sender)
        {
            Button SB = (Button)sender;
            List<typesType> loot = new List<typesType>();
            if (FullTypes)
            {
                loot = currentTypesFile.types.type.ToList();
            }
            else
            {

                if (currentcollection == "other")
                {
                    loot = currentTypesFile.types.type.Where(x => x.category == null).ToList();
                }
                else
                {
                    loot = currentTypesFile.types.type.Where(x => x.category != null && x.category.name == currentcollection).ToList();
                }
            }
            currentTypesFile.isDirty = true;
            SetFlags(SB, loot);
        }
        private void SetFlags(Button SB, List<typesType> list)
        {
            switch (SB.Name)
            {
                case "CargoButton":
                    foreach (typesType item in list)
                    {
                        item.flags.count_in_cargo = checkBox17.Checked == true ? 1 : 0;
                    }
                    break;
                case "HoarderButton":
                    foreach (typesType item in list)
                    {
                        item.flags.count_in_hoarder = checkBox16.Checked == true ? 1 : 0;
                    }
                    break;
                case "MapButton":
                    foreach (typesType item in list)
                    {
                        item.flags.count_in_map = checkBox15.Checked == true ? 1 : 0;
                    }
                    break;
                case "PlayerButton":
                    foreach (typesType item in list)
                    {
                        item.flags.count_in_player = checkBox14.Checked == true ? 1 : 0;
                    }
                    break;
                case "CraftedButton":
                    foreach (typesType item in list)
                    {
                        item.flags.crafted = checkBox9.Checked == true ? 1 : 0;
                    }
                    break;
                case "DelootButton":
                    foreach (typesType item in list)
                    {
                        item.flags.deloot = checkBox8.Checked == true ? 1 : 0;
                    }
                    break;
                default:
                    break;
            }
        }
        private void darkButton5_Click(object sender, EventArgs e)
        {
            List<typesType> loot = new List<typesType>();
            if (FullTypes)
            {
                loot = currentTypesFile.types.type.ToList();
            }
            else
            {

                if (currentcollection == "other")
                {
                    loot = currentTypesFile.types.type.Where(x => x.category == null).ToList();
                }
                else
                {
                    loot = currentTypesFile.types.type.Where(x => x.category != null && x.category.name == currentcollection).ToList();
                }
            }
            currentTypesFile.isDirty = true;
            DoSyncMintoNom(loot);
        }
        private void DoSyncMintoNom(List<typesType> list)
        {
            foreach (typesType item in list)
            {
                item.min = item.nominal;
            }
        }
        private void darkButton6_Click_1(object sender, EventArgs e)
        {
            List<typesType> loot = new List<typesType>();
            if (FullTypes)
            {
                loot = currentTypesFile.types.type.ToList();
            }
            else
            {
                if (currentcollection == "other")
                {
                    loot = currentTypesFile.types.type.Where(x => x.category == null).ToList();
                }
                else
                {
                    loot = currentTypesFile.types.type.Where(x => x.category != null && x.category.name == currentcollection).ToList();
                }
            }
            currentTypesFile.isDirty = true;
            DoSyncNomtoMin(loot);
        }
        private void DoSyncNomtoMin(List<typesType> list)
        {
            foreach (typesType item in list)
            {
                item.nominal = item.min;
            }
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            switch (EconomyTabPage.SelectedIndex)
            {
                case 3:
                    FindTypes();
                    break;
            }
        }

        private void FindTypes()
        {
            isUserInteraction = false;
            if (treeViewMS1.Nodes.Count < 1)
                return;
            string text = EconomySearchBoxTB.Text;
            if (text == "") return;
            searchnum = 0;
            searchtreeNodes = new List<TreeNode>();
            foundparts = new List<typesType>();
            foreach (typesType type in vanillatypes.types.type)
            {
                if (type.name.ToLower().Contains(text.ToLower()))
                {
                    foundparts.Add(type);
                }
            }
            foreach (TypesFile tf in ModTypes)
            {
                foreach (typesType type in tf.types.type)
                {
                    if (type.name.ToLower().Contains(text.ToLower()))
                    {
                        foundparts.Add(type);
                    }
                }
            }
            if (foundparts.Count == 0)
            {
                MessageBox.Show("No Items found");
                return;
            }
            foreach (typesType obj in foundparts)
            {
                foreach (TreeNode node in treeViewMS1.Nodes)
                {
                    searchtree(obj.name.ToLower(), node, searchtreeNodes);
                }
            }
            treeViewMS1.SelectedNode = searchtreeNodes[searchnum];
            treeViewMS1.Focus();
            if (treeViewMS1.SelectedNode.Tag != null && treeViewMS1.SelectedNode.Tag is typesType)
            {
                isUserInteraction = false;
                tabControl1.SelectedIndex = 1;
                currentlootpart = treeViewMS1.SelectedNode.Tag as typesType;
                PopulateLootPartInfo();
                isUserInteraction = true;
            }
            if (searchtreeNodes.Count > 1)
            {
                economySearchNextButton.Visible = true;
                economySearchNextButton.AutoSize = false;
                economySearchNextButton.AutoSize = true;
            }
            isUserInteraction = true;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            searchnum++;
            if (searchnum == searchtreeNodes.Count)
            {
                economySearchNextButton.Visible = false;
                MessageBox.Show("No More Items found");
                return;
            }
            treeViewMS1.SelectedNode = searchtreeNodes[searchnum];
            treeViewMS1.Focus();
            if (treeViewMS1.SelectedNode.Tag != null && treeViewMS1.SelectedNode.Tag is typesType)
            {
                isUserInteraction = false;
                tabControl1.SelectedIndex = 1;
                currentlootpart = treeViewMS1.SelectedNode.Tag as typesType;
                PopulateLootPartInfo();
                isUserInteraction = true;
            }
        }
        private void searchtree(string str, TreeNode tree, List<TreeNode> List)
        {
            if (tree.Text.ToString().ToLower().Contains(str))
                if (!List.Contains(tree))
                    List.Add(tree);
            foreach (TreeNode tn in tree.Nodes)
            {
                if (tn.Text.ToString().ToLower().Contains(str))
                    if (!List.Contains(tn))
                        List.Add(tn);
                if (tn.Nodes.Count > 0)
                {
                    foreach (TreeNode ctn in tn.Nodes)
                        searchtree(str, ctn, List);
                }
            }
        }
        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
            economySearchNextButton.Visible = false;
        }
        #endregion types
        #region events
        public eventscofig currenteventsfile;
        private eventsEvent CurrentEvent;
        private eventsEventChild CurrentChild;
        public BindingList<string> AllEvents;
        private void Loadevents()
        {
            isUserInteraction = false;

            Console.WriteLine("Loading Events");
            AllEvents = new BindingList<string>();
            foreach (eventscofig eventsconfig in currentproject.ModEventsList)
            {
                foreach (eventsEvent cevents in eventsconfig.events.@event)
                {
                    AllEvents.Add(cevents.name);
                }
            }
            var sortedListInstance = new BindingList<string>(AllEvents.OrderBy(x => x).ToList());
            sortedListInstance.Insert(0, "None");
            SecondaryCB.DisplayMember = "DisplayName";
            SecondaryCB.ValueMember = "Value";
            SecondaryCB.DataSource = sortedListInstance;

            EventsLIstLB.DisplayMember = "DisplayName";
            EventsLIstLB.ValueMember = "Value";
            EventsLIstLB.DataSource = currentproject.ModEventsList;
            isUserInteraction = true;

        }
        private void darkButton60_Click(object sender, EventArgs e)
        {
            AllEvents = new BindingList<string>();
            foreach (eventscofig eventsconfig in currentproject.ModEventsList)
            {
                foreach (eventsEvent cevents in eventsconfig.events.@event)
                {
                    AllEvents.Add(cevents.name);
                }
            }
            var sortedListInstance = new BindingList<string>(AllEvents.OrderBy(x => x).ToList());
            sortedListInstance.Insert(0, "None");
            SecondaryCB.DisplayMember = "DisplayName";
            SecondaryCB.ValueMember = "Value";
            SecondaryCB.DataSource = sortedListInstance;
        }
        private void EventsLIstLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EventsLIstLB.SelectedItem as eventscofig == currenteventsfile) { return; }
            if (EventsLIstLB.SelectedIndex == -1) { return; }
            isUserInteraction = false;
            currenteventsfile = EventsLIstLB.SelectedItem as eventscofig;
            positionComboBox.DataSource = Enum.GetValues(typeof(position));
            limitComboBox.DataSource = Enum.GetValues(typeof(limit));

            var sortedListInstance = new BindingList<eventsEvent>(currenteventsfile.events.@event.OrderBy(x => x.name).ToList());
            currenteventsfile.events.@event = sortedListInstance;

            EventsLB.DisplayMember = "DisplayName";
            EventsLB.ValueMember = "Value";
            EventsLB.DataSource = currenteventsfile.events.@event;
            isUserInteraction = true;
        }
        private void EventsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EventsLB.SelectedItem as eventsEvent == CurrentEvent) { return; }
            if (EventsLB.SelectedIndex == -1) { return; }
            CurrentEvent = EventsLB.SelectedItem as eventsEvent;

            isUserInteraction = false;
            ClearChildren();
            positionComboBox.SelectedItem = (position)CurrentEvent.position;
            limitComboBox.SelectedItem = (limit)CurrentEvent.limit;

            nameTB.Text = CurrentEvent.name;
            nominalNUD.Value = (int)CurrentEvent.nominal;
            minNUD.Value = (int)CurrentEvent.min;
            maxNUD.Value = (int)CurrentEvent.max;
            lifetimeNUD.Value = (int)CurrentEvent.lifetime;
            restockNUD.Value = (int)CurrentEvent.restock;
            saferadiusNUD.Value = (int)CurrentEvent.saferadius;
            distanceradiusNUD.Value = (int)CurrentEvent.distanceradius;
            cleanupradiusNUD.Value = (int)CurrentEvent.cleanupradius;
            deletableCB.Checked = CurrentEvent.flags.deletable == 1 ? true : false;
            init_randomCB.Checked = CurrentEvent.flags.init_random == 1 ? true : false;
            remove_damagedCB.Checked = CurrentEvent.flags.remove_damaged == 1 ? true : false;
            activeCB.Checked = CurrentEvent.active == 1 ? true : false;
            if (CurrentEvent.secondary != null)
                SecondaryCB.SelectedIndex = SecondaryCB.FindStringExact(CurrentEvent.secondary);
            else
                SecondaryCB.SelectedIndex = SecondaryCB.FindStringExact("None");

            ChildrenLB.DisplayMember = "DisplayName";
            ChildrenLB.ValueMember = "Value";
            ChildrenLB.DataSource = CurrentEvent.children;

            isUserInteraction = true;
        }
        private void ClearChildren()
        {
            ChildrenLB.DataSource = null;
            ChildGB.Visible = false;
        }
        private void ChildrenLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ChildrenLB.SelectedItem as eventsEventChild == CurrentChild) { return; }
            if (ChildrenLB.SelectedIndex == -1) { return; }
            CurrentChild = ChildrenLB.SelectedItem as eventsEventChild;
            isUserInteraction = false;
            ChildGB.Visible = true;
            ClootmaxNUD.Value = (int)CurrentChild.lootmax;
            ClootminNUD.Value = (int)CurrentChild.lootmin;
            CmaxNUD.Value = (int)CurrentChild.max;
            CminNUD.Value = (int)CurrentChild.min;
            CtypeTB.Text = CurrentChild.type;
            isUserInteraction = true;
        }
        private void EventsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            NumericUpDown nud = sender as NumericUpDown;
            CurrentEvent.SetIntValue(nud.Name.Substring(0, nud.Name.Length - 3), (int)nud.Value);
            currenteventsfile.isDirty = true;
        }
        private void EventsChildNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            NumericUpDown nud = sender as NumericUpDown;
            CurrentChild.SetIntValue(nud.Name.Substring(1, nud.Name.Length - 4), (int)nud.Value);
            currenteventsfile.isDirty = true;
        }
        private void activeCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentEvent.active = activeCB.Checked == true ? 1 : 0;
            currenteventsfile.isDirty = true;
        }
        private void deletableCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentEvent.flags.deletable = deletableCB.Checked == true ? 1 : 0;
            currenteventsfile.isDirty = true;
        }
        private void init_randomCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentEvent.flags.init_random = init_randomCB.Checked == true ? 1 : 0;
            currenteventsfile.isDirty = true;
        }
        private void remove_damagedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentEvent.flags.remove_damaged = remove_damagedCB.Checked == true ? 1 : 0;
            currenteventsfile.isDirty = true;
        }
        private void positionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            position pos = (position)positionComboBox.SelectedItem;
            CurrentEvent.position = pos;
            currenteventsfile.isDirty = true;
        }
        private void limitComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            limit lim = (limit)limitComboBox.SelectedItem;
            CurrentEvent.limit = lim;
            currenteventsfile.isDirty = true;
        }
        private void SecondaryCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            if (SecondaryCB.GetItemText(SecondaryCB.SelectedItem) == "None")
                CurrentEvent.secondary = null;
            else
                CurrentEvent.secondary = SecondaryCB.GetItemText(SecondaryCB.SelectedItem);
            currenteventsfile.isDirty = true;
        }
        private void nameTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentEvent.name = nameTB.Text;
            currenteventsfile.isDirty = true;
            EventsLB.Refresh();
        }
        private void CtypeTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentChild.type = CtypeTB.Text;
            currenteventsfile.isDirty = true;
            EventsLB.Refresh();
        }
        private void darkButton16_Click(object sender, EventArgs e)
        {
            eventsEvent neweventEvent = new eventsEvent()
            {
                name = "NewEvent",
                nominal = 0,
                min = 0,
                max = 0,
                lifetime = 0,
                restock = 0,
                saferadius = 0,
                distanceradius = 0,
                cleanupradius = 0,
                position = position.@fixed,
                limit = limit.child,
                active = 0,
                flags = new eventsEventFlags(),
                children = new BindingList<eventsEventChild>()
            };
            currenteventsfile.events.AddnewEvent(neweventEvent);
            currenteventsfile.isDirty = true;
        }
        private void darkButton17_Click(object sender, EventArgs e)
        {
            currenteventsfile.events.RemoveEvent(CurrentEvent);
            currenteventsfile.isDirty = true;
        }
        private void darkButton19_Click(object sender, EventArgs e)
        {
            eventsEventChild newventeventschild = new eventsEventChild()
            {
                type = "newChild",
                lootmax = 0,
                lootmin = 0,
                max = 0,
                min = 0
            };
            CurrentEvent.Addnechild(newventeventschild);
            currenteventsfile.isDirty = true;
        }
        private void darkButton18_Click(object sender, EventArgs e)
        {
            CurrentEvent.Removechild(CurrentChild);
            currenteventsfile.isDirty = true;
        }
        private void darkButton21_Click(object sender, EventArgs e)
        {
            AddNeweventFile form = new AddNeweventFile
            {
                currentproject = currentproject,
                newlocation = true
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                string path = form.CustomLocation;
                string modname = form.TypesName;
                Directory.CreateDirectory(path);
                List<string> Eventfile = new List<string>
                {
                    "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>",
                    "<events>",
                    "</events>"
                };
                File.WriteAllLines(path + "\\" + modname + "_events.xml", Eventfile);
                eventscofig test = new eventscofig(path + "\\" + modname + "_events.xml");
                test.SaveEvent();
                currentproject.EconomyCore.AddCe(path.Replace(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\", ""), modname + "_events.xml", "events");
                currentproject.EconomyCore.SaveEconomycore();
                currentproject.SetEvents();
                populateEconmyTreeview();
                Loadevents();
            }
        }
        private void darkButton20_Click(object sender, EventArgs e)
        {
            string Modname = Path.GetFileNameWithoutExtension(currenteventsfile.Filename);
            currentproject.EconomyCore.RemoveCe(Modname, out string foflderpath, out string filename, out bool deletedirectory);
            File.Delete(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\" + foflderpath + "\\" + filename);
            if (deletedirectory)
                Directory.Delete(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\" + foflderpath, true);
            currentproject.EconomyCore.SaveEconomycore();
            currentproject.removeevent(currenteventsfile.Filename);
            currentproject.SetEvents();
            Loadevents();
            populateEconmyTreeview();
        }
        private void CreateeventSpawnButton_Click(object sender, EventArgs e)
        {
            eventposdefEvent newvenspawn = new eventposdefEvent()
            {
                name = CurrentEvent.name,
                zone = null,
                pos = null
            };
            eventposdef.@event.Add(newvenspawn);
            TreeNode neweventspawn = new TreeNode(CurrentEvent.name);
            neweventspawn.Tag = newvenspawn;
            EventSpawnTV.Nodes[0].Nodes.Add(neweventspawn);
            currentproject.cfgeventspawns.isDirty = true;
            MessageBox.Show(CurrentEvent.name + " Event spawn Added");
        }
        #endregion events
        #region eventspawn
        public eventposdef eventposdef;
        public eventposdefEvent eventposdefEvent;
        public eventposdefEventPos eventposdefEventPos;
        public eventposdefEventZone eventposdefEventZone;
        public int ZoneEventScale = 1;
        private void LoadeventSpawns()
        {
            Console.WriteLine("Loading EventSpawns");
            isUserInteraction = false;
            var sortedListInstance = new BindingList<eventposdefEvent>(currentproject.cfgeventspawns.eventposdef.@event.OrderBy(x => x.name).ToList());
            currentproject.cfgeventspawns.eventposdef.@event = sortedListInstance;
            eventposdef = currentproject.cfgeventspawns.eventposdef;
            isUserInteraction = true;

            SetUpeventspawnTreeview();

            isUserInteraction = true;
        }
        private void SetUpeventspawnTreeview()
        {
            EventSpawnTV.Nodes.Clear();
            TreeNode rootnoot = new TreeNode(Path.GetFileName(currentproject.cfgeventspawns.Filename));
            rootnoot.Tag = "EventSpawnParent";
            foreach(eventposdefEvent eventspawn in eventposdef.@event)
            {
                TreeNode newevent = new TreeNode(eventspawn.ToString());
                newevent.Tag = eventspawn;
                if(eventspawn.zone!= null)
                {
                    eventposdefEventZone zone = eventspawn.zone;
                    TreeNode zonenode = new TreeNode("zone");
                    zonenode.Name = "ZONE";
                    zonenode.Tag = zone;
                    newevent.Nodes.Add(zonenode);
                }
                if(eventspawn.pos != null && eventspawn.pos.Count > 0)
                {
                    TreeNode eventposnodes = new TreeNode("pos");
                    eventposnodes.Name = "POS";
                    eventposnodes.Tag = "PosParent";
                    foreach(eventposdefEventPos pos in eventspawn.pos)
                    {
                        TreeNode posnodes = new TreeNode(pos.ToString());
                        posnodes.Tag = pos;
                        eventposnodes.Nodes.Add(posnodes);
                    }
                    newevent.Nodes.Add(eventposnodes);
                }
                rootnoot.Nodes.Add(newevent);
            }
            EventSpawnTV.Nodes.Add(rootnoot);
        }
        private void EventSpawnTV_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            foreach (ToolStripMenuItem TSMI in EventSpawnContextMenu.Items)
            {
                TSMI.Visible = false;
            }
            EventSpawnTV.SelectedNode = e.Node;
            EventspawnZoneGB.Visible = false;
            EventspawnPositionGB.Visible = false;
            EventSpawnInfoGB.Visible = false;
            isUserInteraction = false;
            if (e.Node.Tag != null && e.Node.Tag is string)
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (e.Node.Tag.ToString() == "EventSpawnParent")
                    {
                        addNewEventSpawnToolStripMenuItem.Visible = true;
                        EventSpawnContextMenu.Show(Cursor.Position);
                    }
                }
            }
            else if (e.Node.Tag != null && e.Node.Tag is eventposdefEvent)
            {
                eventposdefEvent = e.Node.Tag as eventposdefEvent;
                EventSpawnInfoGB.Visible = true;
                eventSpawnNameTB.Text = eventposdefEvent.name;
                if (eventposdefEvent.zone == null)
                    EventspawnUseZoneCB.Checked = false;
                else
                    EventspawnUseZoneCB.Checked = true;
                if (e.Button == MouseButtons.Right)
                {
                    addNewPosirtionToolStripMenuItem.Visible = true;
                    importPositionFromdzeToolStripMenuItem.Visible = true;
                    importPositionAndCreateEventgroupFormdzeToolStripMenuItem.Visible = true;
                    deleteSelectedEventSpawnToolStripMenuItem.Visible = true;
                    if (eventposdefEvent.pos != null && eventposdefEvent.pos.Count > 0)
                    {
                        removeAllPositionToolStripMenuItem.Visible = true;
                        exportPositionTodzeToolStripMenuItem.Visible = true;
                    }
                    EventSpawnContextMenu.Show(Cursor.Position);
                }
            }
            else if (e.Node.Tag != null && e.Node.Tag is eventposdefEventZone)
            {
                EventspawnZoneGB.Visible = true;
                eventposdefEventZone = e.Node.Tag as eventposdefEventZone;
                eventzonesminNUD.Value = eventposdefEventZone.smin;
                eventzonesmaxNUD.Value = eventposdefEventZone.smax;
                eventzonedminNUD.Value = eventposdefEventZone.dmin;
                eventzonedmaxNUD.Value = eventposdefEventZone.dmax;
                eventzonedNUD.Value = eventposdefEventZone.r;
            }
            else if (e.Node.Tag != null && e.Node.Tag is eventposdefEventPos)
            {
                eventposdefEvent = e.Node.Parent.Parent.Tag as eventposdefEvent;
                eventposdefEventPos = e.Node.Tag as eventposdefEventPos;
                EventspawnPositionGB.Visible = true;
                EventSpawnPosXNUD.Value = eventposdefEventPos.x;
                if (EventSpawnPosYNUD.Visible = checkBox50.Checked = eventposdefEventPos.ySpecified)
                {
                    EventSpawnPosYNUD.Value = eventposdefEventPos.y;
                }
                EventSpawnPosZNUD.Value = eventposdefEventPos.z;
                if (EventSpawnPosANUD.Visible = checkBox51.Checked = eventposdefEventPos.aSpecified)
                    EventSpawnPosANUD.Value = eventposdefEventPos.a;
                if (eventposdefEventPos.@group != null)
                {
                    EventSpawnGroupTB.Visible = true;
                    checkBox64.Checked = true;
                    EventSpawnGroupTB.Text = eventposdefEventPos.group;
                }
                else
                {
                    EventSpawnGroupTB.Visible = false;
                    checkBox64.Checked = false;
                }
                pictureBox1.Invalidate();
                if (e.Button == MouseButtons.Right)
                {
                    removeSelectedPositionToolStripMenuItem.Visible = true;
                    if(eventposdefEventPos.group != null)
                    {
                        exportGroupSpawnTodzeToolStripMenuItem.Visible = true;
                    }
                    EventSpawnContextMenu.Show(Cursor.Position);
                }
            }
            isUserInteraction = true;
        }
        private void checkBox64_CheckedChanged(object sender, EventArgs e)
        {
            EventSpawnGroupTB.Visible = checkBox64.Checked;
            if (!isUserInteraction) return;
            if (checkBox64.Checked)
            {
                eventposdefEventPos.group = "New Group Name, Please change";
                EventSpawnGroupTB.Text = eventposdefEventPos.group;
            }
            else
                eventposdefEventPos.group = null;
            currentproject.cfgeventspawns.isDirty = true;
        }
        private void eventSpawnNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            if (EventSpawnTV.SelectedNode.Tag is eventposdefEvent)
            {
                eventposdefEvent.name = EventSpawnTV.SelectedNode.Text = eventSpawnNameTB.Text;
            }
            currentproject.cfgeventspawns.isDirty = true;
        }
        private void EventspawnUseZoneCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            if (EventspawnUseZoneCB.Checked)
            {
                eventposdefEvent.zone = new eventposdefEventZone()
                {
                    smax = 0,
                    smin = 0,
                    dmin = 0,
                    dmax = 0,
                    r = 0
                };
                TreeNode zonenode = new TreeNode("zone");
                zonenode.Name = "ZONE";
                zonenode.Tag = eventposdefEvent.zone;
                EventSpawnTV.SelectedNode.Nodes.Add(zonenode);
            }
            else
            {
                TreeNode zonennode = EventSpawnTV.SelectedNode.Nodes.Find("ZONE", false)[0];
                zonennode.Remove();
                eventposdefEvent.zone = null;
            }
            currentproject.cfgeventspawns.isDirty = true;
        }
        private void checkBox50_CheckedChanged(object sender, EventArgs e)
        {
            EventSpawnPosYNUD.Visible = checkBox50.Checked;
            if (!isUserInteraction) return;
            foreach (TreeNode tn in EventSpawnTV.SelectedNodes)
            {
                eventposdefEventPos eventpos = tn.Tag as eventposdefEventPos;
                if (checkBox50.Checked)
                {
                    eventpos.y = 0;
                    eventpos.ySpecified = true;
                    EventSpawnPosYNUD.Value = eventpos.y;
                }
                else
                {
                    eventpos.ySpecified = false;
                }
                currentproject.cfgeventspawns.isDirty = true;
            }
        }
        private void checkBox51_CheckedChanged(object sender, EventArgs e)
        {
            EventSpawnPosANUD.Visible = checkBox51.Checked;
            if (!isUserInteraction) return;
            if (checkBox51.Checked)
            {
                eventposdefEventPos.a = 0;
                eventposdefEventPos.aSpecified = true;
                EventSpawnPosANUD.Value = eventposdefEventPos.a;
            }
            else
            {
                eventposdefEventPos.aSpecified = false;
            }
            currentproject.cfgeventspawns.isDirty = true;
        }
        private void eventzonesminNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            eventposdefEventZone.smin = (int)eventzonesminNUD.Value;
            currentproject.cfgeventspawns.isDirty = true;
        }
        private void eventzonesmaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            eventposdefEventZone.smax = (int)eventzonesmaxNUD.Value;
            currentproject.cfgeventspawns.isDirty = true;
        }
        private void eventzonedminNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            eventposdefEventZone.dmin = (int)eventzonedminNUD.Value;
            currentproject.cfgeventspawns.isDirty = true;
        }
        private void eventzonedmaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            eventposdefEventZone.dmax = (int)eventzonedmaxNUD.Value;
            currentproject.cfgeventspawns.isDirty = true;
        }
        private void eventzonedNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            eventposdefEventZone.r = (int)eventzonedNUD.Value;
            currentproject.cfgeventspawns.isDirty = true;
        }
        private void DrawAllEventSpawns(object sender, PaintEventArgs e)
        {
            if (eventposdefEvent == null) return;
            eventposdefEventPos currentpos = EventSpawnTV.SelectedNode.Tag as eventposdefEventPos;
            if (currentpos == null) return;
            foreach (eventposdefEventPos newpos in eventposdefEvent.pos)
            {
                float scalevalue = ZoneEventScale * 0.05f;
                int centerX = (int)(Math.Round((float)newpos.x) * scalevalue);
                int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round((float)newpos.z, 0) * scalevalue);
                int radius = (int)(Math.Round(1f, 0) * scalevalue);
                Point center = new Point(centerX, centerY);
                Pen pen = new Pen(Color.Red, 4);
                if (newpos == currentpos)
                    pen.Color = Color.LimeGreen;
                getEventCircle(e.Graphics, pen, center, radius);
            }
        }
        private void SetEventSpawnScale()
        {
            float scalevalue = ZoneEventScale * 0.05f;
            float mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            pictureBox1.Size = new Size(newsize, newsize);
        }
        private void getEventCircle(Graphics drawingArea, Pen penToUse, Point center, int radius)
        {
            Rectangle rect = new Rectangle(center.X - 1, center.Y - 1, 2, 2);
            drawingArea.DrawEllipse(penToUse, rect);
            Rectangle rect2 = new Rectangle(center.X - radius, center.Y - radius, radius * 2, radius * 2);
            drawingArea.DrawEllipse(penToUse, rect2);
        }
        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            ZoneEventScale = trackBar1.Value;
            SetEventSpawnScale();
        }
        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            if (e is MouseEventArgs mouseEventArgs)
            {
                Cursor.Current = Cursors.WaitCursor;
                float scalevalue = ZoneEventScale * 0.05f;
                float mapsize = currentproject.MapSize;
                int newsize = (int)(mapsize * scalevalue);
                EventSpawnPosXNUD.Value = Decimal.Round((decimal)(mouseEventArgs.X / scalevalue), 4);
                EventSpawnPosZNUD.Value = Decimal.Round((decimal)((newsize - mouseEventArgs.Y) / scalevalue), 4);
                if (eventposdefEventPos.ySpecified)
                {
                    if (MapData.FileExists)
                    {
                        EventSpawnPosYNUD.Value = Decimal.Round((decimal)(MapData.gethieght((float)eventposdefEventPos.x, (float)eventposdefEventPos.z)), 4);
                    }
                }
                Cursor.Current = Cursors.Default;
                currentproject.cfgeventspawns.isDirty = true;
                pictureBox1.Invalidate();
            }
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _mouseLastPosition = e.Location;
            }
        }
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            if (pictureBox1.Focused == false)
            {
                pictureBox1.Focus();
                panel1.AutoScrollPosition = _newscrollPosition;
                pictureBox1.Invalidate();
            }
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point changePoint = new Point(e.Location.X - _mouseLastPosition.X, e.Location.Y - _mouseLastPosition.Y);
                _newscrollPosition = new Point(-panel1.AutoScrollPosition.X - changePoint.X, -panel1.AutoScrollPosition.Y - changePoint.Y);
                if (_newscrollPosition.X <= 0)
                    _newscrollPosition.X = 0;
                if (_newscrollPosition.Y <= 0)
                    _newscrollPosition.Y = 0;
                panel1.AutoScrollPosition = _newscrollPosition;
                pictureBox1.Invalidate();
            }
            decimal scalevalue = ZoneEventScale * (decimal)0.05;
            decimal mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            decimal x = Decimal.Round((decimal)(e.X / scalevalue), 4);
            decimal y = Decimal.Round((decimal)((newsize - e.Y) / scalevalue), 4);
            label157.Text = x + "," + y;
        }
        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                pictureBox1_ZoomOut();
            }
            else
            {
                pictureBox1_ZoomIn();
            }

        }
        private void pictureBox1_ZoomIn()
        {
            int oldpictureboxhieght = pictureBox1.Height;
            int oldpitureboxwidht = pictureBox1.Width;
            Point oldscrollpos = panel1.AutoScrollPosition;
            int tbv = trackBar1.Value;
            int newval = tbv + 1;
            if (newval >= 20)
                newval = 20;
            trackBar1.Value = newval;
            ZoneEventScale = trackBar1.Value;
            SetEventSpawnScale();
            if (pictureBox1.Height > panel1.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox1.Height * newy);
                _newscrollPosition.Y = y * -1;
                panel1.AutoScrollPosition = _newscrollPosition;
            }
            if (pictureBox1.Width > panel1.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox1.Width * newy);
                _newscrollPosition.X = x * -1;
                panel1.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox1.Invalidate();
        }
        private void pictureBox1_ZoomOut()
        {
            int oldpictureboxhieght = pictureBox1.Height;
            int oldpitureboxwidht = pictureBox1.Width;
            Point oldscrollpos = panel1.AutoScrollPosition;
            int tbv = trackBar1.Value;
            int newval = tbv - 1;
            if (newval <= 1)
                newval = 1;
            trackBar1.Value = newval;
            ZoneEventScale = trackBar1.Value;
            SetEventSpawnScale();
            if (pictureBox1.Height > panel1.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox1.Height * newy);
                _newscrollPosition.Y = y * -1;
                panel1.AutoScrollPosition = _newscrollPosition;
            }
            if (pictureBox1.Width > panel1.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox1.Width * newy);
                _newscrollPosition.X = x * -1;
                panel1.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox1.Invalidate();
        }
        private void EventSpawnPosXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            eventposdefEventPos.x = EventSpawnPosXNUD.Value;
            currentproject.cfgeventspawns.isDirty = true;
            EventSpawnTV.SelectedNode.Text = eventposdefEventPos.ToString();
            pictureBox1.Invalidate();
            if(eventposdefEventPos.ySpecified)
            {
                if (MapData.FileExists)
                {
                    EventSpawnPosYNUD.Value = (decimal)(MapData.gethieght((float)eventposdefEventPos.x, (float)eventposdefEventPos.z));
                }
            }
        }
        private void EventSpawnPosYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            eventposdefEventPos.y = EventSpawnPosYNUD.Value;
            currentproject.cfgeventspawns.isDirty = true;
            pictureBox1.Invalidate();
        }
        private void EventSpawnPosZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            eventposdefEventPos.z = EventSpawnPosZNUD.Value;
            currentproject.cfgeventspawns.isDirty = true;
            EventSpawnTV.SelectedNode.Text = eventposdefEventPos.ToString();
            pictureBox1.Invalidate();
            if (eventposdefEventPos.ySpecified)
            {
                if (MapData.FileExists)
                {
                    EventSpawnPosYNUD.Value = (decimal)(MapData.gethieght((float)eventposdefEventPos.x, (float)eventposdefEventPos.z));
                }
            }
        }
        private void EventSpawnPosANUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            eventposdefEventPos.group = EventSpawnGroupTB.Text;
            currentproject.cfgeventspawns.isDirty = true;
        }
        private void EventSpawnGroupTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            eventposdefEventPos.a = EventSpawnPosANUD.Value;
            currentproject.cfgeventspawns.isDirty = true;
        }
        private void darkButton59_Click(object sender, EventArgs e)
        {
            eventposdefEventPos newpos = new eventposdefEventPos()
            {
                x = currentproject.MapSize / 2,
                z = currentproject.MapSize / 2
            };
            if (eventposdefEvent.pos == null)
            {
                eventposdefEvent.pos = new BindingList<eventposdefEventPos>();
            }
            eventposdefEvent.pos.Add(newpos);
            currentproject.cfgeventspawns.isDirty = true;
            pictureBox1.Invalidate();
        }
        private void darkButton58_Click(object sender, EventArgs e)
        {
            eventposdefEvent.pos.Remove(eventposdefEventPos);
            currentproject.cfgeventspawns.isDirty = true;
            pictureBox1.Invalidate();
        }
        private void darkButton62_Click(object sender, EventArgs e)
        {
            if (eventposdefEvent == null) return;
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.DefaultExt = ".map";
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                string[] fileContent = File.ReadAllLines(openfile.FileName);
                eventposdefEvent.pos = new BindingList<eventposdefEventPos>();
                for (int i = 0; i < fileContent.Length; i++)
                {
                    if (fileContent[i] == "") continue;
                    string[] linesplit = fileContent[i].Split('|');
                    string[] XYZ = linesplit[1].Split(' ');
                    string a = linesplit[2].Split(' ')[0];
                    eventposdefEventPos newpos = new eventposdefEventPos()
                    {
                        x = Convert.ToDecimal(XYZ[0]),
                        ySpecified = true,
                        y = Convert.ToDecimal(XYZ[1]),
                        z = Convert.ToDecimal(XYZ[2]),
                        aSpecified = true,
                        a = Convert.ToDecimal(a)
                    };
                    eventposdefEvent.pos.Add(newpos);

                }
                currentproject.cfgeventspawns.isDirty = true;
            }
        }
        private void darkButton61_Click(object sender, EventArgs e)
        {
            if (eventposdefEvent == null || eventposdefEvent.pos == null || eventposdefEvent.pos.Count == 0) return;
            eventsEvent selectedeevent = geteventfromspawn(eventposdefEvent.name);
            if (selectedeevent == null) return;
            string classname = selectedeevent.children[0].type;
            StringBuilder sb = new StringBuilder();
            foreach (eventposdefEventPos pos in eventposdefEvent.pos)
            {
                sb.Append(classname + "|" + pos.x.ToString("F6") + " " + pos.y.ToString("F6") + " " + pos.z.ToString("F6") + "|" + pos.a.ToString("F6") + " 0.000000 0.000000" + Environment.NewLine);
            }
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.DefaultExt = ".map";
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(savefile.FileName, sb.ToString());
            }

        }
        public eventsEvent geteventfromspawn(string name)
        {
            foreach (eventscofig eventsconfig in currentproject.ModEventsList)
            {
                eventsEvent ee = eventsconfig.events.@event.FirstOrDefault(x => x.name == name);
                if (ee == null)
                    continue;
                else
                    return ee;
            }
            return null;
        }
        private void addNewEventSpawnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eventposdefEvent newvenspawn = new eventposdefEvent()
            {
                name = "NewEventSpawn",
                zone = null,
                pos = null
            };
            eventposdef.@event.Add(newvenspawn);
            TreeNode neweventspawn = new TreeNode("NewEventSpawn");
            neweventspawn.Tag = newvenspawn;
            EventSpawnTV.SelectedNode.Nodes.Add(neweventspawn);
            currentproject.cfgeventspawns.isDirty = true;
        }
        private void deleteSelectedEventSpawnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eventposdef.@event.Remove(eventposdefEvent);
            eventposdefEvent = null;
            EventSpawnTV.SelectedNode.Remove();
            currentproject.cfgeventspawns.isDirty = true;
            pictureBox1.Invalidate();
        }
        private void importPositionsFromMapToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void importPositionFromdzeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    DZE importfile = DZEHelpers.LoadFile(filePath);
                    TreeNode eventposnodes = null;
                    if (eventposdefEvent.pos == null || eventposdefEvent.pos.Count == 0)
                    {
                        eventposdefEvent.pos = new BindingList<eventposdefEventPos>();
                        if (!EventSpawnTV.SelectedNode.Nodes.ContainsKey("POS"))
                        {
                            eventposnodes = new TreeNode("pos");
                            eventposnodes.Name = "POS";
                            eventposnodes.Tag = "PosParent";
                        }
                        else
                        {
                            eventposnodes = EventSpawnTV.SelectedNode.Nodes.Find("POS", false)[0];
                        }
                    }
                    else
                    {
                        eventposnodes = EventSpawnTV.SelectedNode.Nodes.Find("POS", false)[0];
                        DialogResult dialogResult = MessageBox.Show("Clear Exisitng Position?", "Clear position", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            eventposdefEvent.pos = new BindingList<eventposdefEventPos>();
                            eventposnodes.Nodes.Clear();
                            
                        }
                        eventposnodes.Remove();
                    }
                    
                    foreach (Editorobject eo in importfile.EditorObjects)
                    {
                        eventposdefEventPos newpos = new eventposdefEventPos()
                        {
                            x = Convert.ToDecimal(eo.Position[0]),
                            ySpecified = true,
                            y = Convert.ToDecimal(eo.Position[1]),
                            z = Convert.ToDecimal(eo.Position[2]),
                            aSpecified = true,
                            a = Convert.ToDecimal(eo.Orientation[0]),
                            group = null

                        };
                        eventposdefEvent.pos.Add(newpos);
                    }
                    foreach (eventposdefEventPos pos in eventposdefEvent.pos)
                    {
                        TreeNode posnodes = new TreeNode(pos.ToString());
                        posnodes.Tag = pos;
                        eventposnodes.Nodes.Add(posnodes);
                    }
                    EventSpawnTV.SelectedNode.Nodes.Add(eventposnodes);
                    currentproject.cfgeventspawns.isDirty = true;
                }
            }
        }
        private void importPositionAndCreateEventgroupFormdzeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Multiselect = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    TreeNode eventposnodes = null;
                    if (eventposdefEvent.pos == null || eventposdefEvent.pos.Count == 0)
                    {
                        eventposdefEvent.pos = new BindingList<eventposdefEventPos>();
                        if (!EventSpawnTV.SelectedNode.Nodes.ContainsKey("POS"))
                        {
                            eventposnodes = new TreeNode("pos");
                            eventposnodes.Name = "POS";
                            eventposnodes.Tag = "PosParent";
                        }
                        else
                        {
                            eventposnodes = EventSpawnTV.SelectedNode.Nodes.Find("POS", false)[0];
                        }
                    }
                    else
                    {
                        eventposnodes = EventSpawnTV.SelectedNode.Nodes.Find("POS", false)[0];
                        DialogResult dialogResult = MessageBox.Show("Clear Exisitng Position?", "Clear position", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            eventposdefEvent.pos = new BindingList<eventposdefEventPos>();
                            eventposnodes.Nodes.Clear();

                        }
                        eventposnodes.Remove();
                    }
                    foreach (string file in openFileDialog.FileNames)
                    {
                        string filePath = file;
                        DZE importfile = DZEHelpers.LoadFile(filePath);
                        //AddItemfromString form = new AddItemfromString();
                        //form.TitleLable = "Enter Name of Event Group";
                        //DialogResult result = form.ShowDialog();
                        //if (result == DialogResult.OK)
                        //{
                            string Groupname = Path.GetFileNameWithoutExtension(file);
                            //List<string> addedtypes = form.addedtypes.ToList();
                            //foreach (string l in addedtypes)
                            //{
                            //    Groupname = l;
                            //}

                            eventposdefEventPos newpos = new eventposdefEventPos()
                            {
                                x = Convert.ToDecimal(importfile.EditorObjects[0].Position[0]),
                                ySpecified = true,
                                y = Convert.ToDecimal(importfile.EditorObjects[0].Position[1]),
                                z = Convert.ToDecimal(importfile.EditorObjects[0].Position[2]),
                                aSpecified = true,
                                a = 0,
                                group = Groupname

                            };
                            eventposdefEvent.pos.Add(newpos);
                            TreeNode posnodes = new TreeNode(newpos.ToString());
                            posnodes.Tag = newpos;
                            eventposnodes.Nodes.Add(posnodes);
                            eventgroupdefGroup newvengroup = new eventgroupdefGroup()
                            {
                                name = Groupname,
                                child = new BindingList<eventgroupdefGroupChild>()
                            };

                            TreeNode neweventspawn = new TreeNode(Groupname);
                            neweventspawn.Tag = newvengroup;
                            foreach (Editorobject eo in importfile.EditorObjects)
                            {
                                if (eo.Orientation[0] < 0)
                                    eo.Orientation[0] = 360 + eo.Orientation[0];
                                eventgroupdefGroupChild eventgroupdefGroupChild = new eventgroupdefGroupChild()
                                {
                                    type = eo.Type,
                                    x = (decimal)(eo.Position[0]) - newpos.x,
                                    ySpecified = true,
                                    y = (decimal)(eo.Position[1]) - newpos.y,
                                    z = (decimal)(eo.Position[2]) - newpos.z,
                                    a = (decimal)(eo.Orientation[0]),
                                    delootSpecified = true,
                                    deloot = 0,
                                    lootminSpecified = true,
                                    lootmin = 1,
                                    lootmaxSpecified = true,
                                    lootmax = 3
                                };
                                TreeNode eventgroupchile = new TreeNode(eventgroupdefGroupChild.type);
                                eventgroupchile.Tag = eventgroupdefGroupChild;
                                neweventspawn.Nodes.Add(eventgroupchile);
                                newvengroup.child.Add(eventgroupdefGroupChild);
                            }
                            eventgroupdef.group.Add(newvengroup);
                            eventspawngroupTV.Nodes[0].Nodes.Add(neweventspawn);
                            currentproject.cfgeventgroups.isDirty = true;
                        //}
                    }
                    EventSpawnTV.SelectedNode.Nodes.Add(eventposnodes);
                    currentproject.cfgeventspawns.isDirty = true;
                }
            }
        }
        private void removeAllPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EventSpawnTV.SelectedNode.Nodes.ContainsKey("POS"))
            {
                EventSpawnTV.SelectedNode.Nodes.Find("POS", false)[0].Remove();
            }
            eventposdefEvent.pos = new BindingList<eventposdefEventPos>();
            currentproject.cfgeventspawns.isDirty = true;
        }
        private void exportPositionsTomapToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void exportPositionTodzeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DZE newdze = new DZE()
            {
                EditorObjects = new BindingList<Editorobject>(),
                EditorDeletedObjects = new BindingList<Editordeletedobject>(),
                MapName = Path.GetFileNameWithoutExtension(currentproject.MapPath).Split('_')[0]
            };
            string Classname = "";
            foreach (eventscofig eventconfig in currentproject.ModEventsList)
            {
                if (Classname != "")
                    break;
                foreach(eventsEvent eve in eventconfig.events.@event)
                {
                    if(eve.name == eventposdefEvent.name)
                    {
                        Classname = eve.children[0].type;
                        break;
                    }
                }
            }
            foreach (eventposdefEventPos array in eventposdefEvent.pos)
            {
                Editorobject eo = new Editorobject()
                {
                    Type = Classname,
                    DisplayName = Classname,
                    Position = new float[] { (float)array.x, 0f, (float)array.z },
                    Orientation = new float[] { 0, 0, 0 },
                    Scale = 1.0f,
                    Flags = 2147483647
                };
                if (array.ySpecified)
                    eo.Position[1] = (float)array.y;
                if (array.aSpecified)
                    eo.Orientation[0] = (float)array.a;
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
        private void addNewPosirtionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eventposdefEventPos newpos = new eventposdefEventPos()
            {
                x = currentproject.MapSize / 2,
                z = currentproject.MapSize / 2
            };
            if (eventposdefEvent.pos == null)
            {
                eventposdefEvent.pos = new BindingList<eventposdefEventPos>();
            }
            eventposdefEvent.pos.Add(newpos);
            currentproject.cfgeventspawns.isDirty = true;
            pictureBox1.Invalidate();
            TreeNode eventposnodes = null;
            if (!EventSpawnTV.SelectedNode.Nodes.ContainsKey("POS"))
            {
                eventposnodes = new TreeNode("pos");
                eventposnodes.Name = "POS";
                eventposnodes.Tag = "PosParent";
                EventSpawnTV.SelectedNode.Nodes.Add(eventposnodes);
            }
            else
            {
                eventposnodes = EventSpawnTV.SelectedNode.Nodes.Find("POS", false)[0];
            }
            TreeNode posnodes = new TreeNode(newpos.ToString());
            posnodes.Tag = newpos;
            eventposnodes.Nodes.Add(posnodes);
            
            currentproject.cfgeventspawns.isDirty = true;
        }
        private void removeSelectedPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eventposdefEvent.pos.Remove(eventposdefEventPos);
            currentproject.cfgeventspawns.isDirty = true;
            EventSpawnTV.SelectedNode.Remove();
            pictureBox1.Invalidate();
        }
        private void exportGroupSpawnTodzeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eventgroupdefGroup eventgroupdefGroup = eventgroupdef.getassociatedgroup(eventposdefEventPos.group);
            if (eventgroupdefGroup == null) return;
            DZE newdze = new DZE()
            {
                CameraPosition = new float[] { (float)eventposdefEventPos.x, (float)(eventposdefEventPos.y + 8), (float)eventposdefEventPos.z },
                MapName = Path.GetFileNameWithoutExtension(currentproject.MapPath.Split('_')[0]),
                EditorObjects = new BindingList<Editorobject>(),
                EditorDeletedObjects = new BindingList<Editordeletedobject>()
            };
            foreach(eventgroupdefGroupChild eventgroupdefGroupChild in eventgroupdefGroup.child)
            {
                Editorobject newobject = new Editorobject()
                {
                    Position = new float[] 
                    {
                       (float)(eventposdefEventPos.x + eventgroupdefGroupChild.x),
                       (float)(eventposdefEventPos.y + eventgroupdefGroupChild.y),
                       (float)(eventposdefEventPos.z + eventgroupdefGroupChild.z)
                    },
                    Orientation = new float[]
                    {
                        (float)(eventposdefEventPos.a + eventgroupdefGroupChild.a),
                        0,
                        0
                    },
                    DisplayName = eventgroupdefGroupChild.type,
                    Type = eventgroupdefGroupChild.type,
                    Scale = 1.0f,
                    Flags = 2147483647
                };
                newdze.EditorObjects.Add(newobject);
            }
            SaveFileDialog save = new SaveFileDialog();
            save.FileName = eventgroupdefGroup.name;
            if (save.ShowDialog() == DialogResult.OK)
            {
                var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                string jsonString = JsonSerializer.Serialize(newdze, options);
                File.WriteAllText(save.FileName + ".dze", jsonString);
            }
        }
        #endregion eventspawns
        #region evetngroups
        public eventgroupdef eventgroupdef;
        public eventgroupdefGroup eventgroupdefGroup;
        public eventgroupdefGroupChild eventgroupdefGroupChild;
        public void LoadeventSpawnsGroup()
        {
            Console.WriteLine("Loading Eventgroups");
            isUserInteraction = false;
            var sortedListInstance = new BindingList<eventgroupdefGroup>(currentproject.cfgeventgroups.eventgroupdef.group.OrderBy(x => x.name).ToList());
            currentproject.cfgeventgroups.eventgroupdef.group = sortedListInstance;
            eventgroupdef = currentproject.cfgeventgroups.eventgroupdef;
            SetupEventSpawnGroupTreeView();
            isUserInteraction = true;
        }
        private void SetupEventSpawnGroupTreeView()
        {
            eventspawngroupTV.Nodes.Clear();
            TreeNode rootnoot = new TreeNode(Path.GetFileName(currentproject.cfgeventgroups.Filename));
            rootnoot.Tag = "EventGroupParent";
            foreach (eventgroupdefGroup eventspawngroup in eventgroupdef.group)
            {
                TreeNode neweventgroup = new TreeNode(eventspawngroup.ToString());
                neweventgroup.Tag = eventspawngroup;
                if (eventspawngroup.child != null && eventspawngroup.child.Count > 0)
                {
                    foreach (eventgroupdefGroupChild child in eventspawngroup.child)
                    {
                        TreeNode childnode = new TreeNode(child.ToString());
                        childnode.Tag = child;
                        neweventgroup.Nodes.Add(childnode);
                    }
                }
                rootnoot.Nodes.Add(neweventgroup);
            }
            eventspawngroupTV.Nodes.Add(rootnoot);
        }
        private void eventspawngroupTV_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            foreach (ToolStripMenuItem TSMI in EventgroupContextMenu.Items)
            {
                TSMI.Visible = false;
            }
            eventspawngroupTV.SelectedNode = e.Node;
            isUserInteraction = false;
            if (e.Node.Tag != null && e.Node.Tag is string)
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (e.Node.Tag.ToString() == "EventGroupParent")
                    {
                        addNewGroupToolStripMenuItem.Visible = true;
                        EventgroupContextMenu.Show(Cursor.Position);
                    }
                }
            }
            else if (e.Node.Tag != null && e.Node.Tag is eventgroupdefGroup)
            {
                eventgroupdefGroup = e.Node.Tag as eventgroupdefGroup;
                if (e.Button == MouseButtons.Right)
                {
                    addChildToolStripMenuItem.Visible = true;
                    removeGroupToolStripMenuItem.Visible = true;
                    //importChildrenFromdzeToolStripMenuItem.Visible = true;
                    EventgroupContextMenu.Show(Cursor.Position);
                }
            }
            else if (e.Node.Tag != null && e.Node.Tag is eventgroupdefGroupChild)
            {
                eventgroupdefGroupChild = e.Node.Tag as eventgroupdefGroupChild;
                eventgroupnameTB.Text = eventgroupdefGroupChild.type;
                eventgroupXNUD.Value = eventgroupdefGroupChild.x;
                eventgroupYNUD.Value = eventgroupdefGroupChild.y;
                eventgroupZNUD.Value = eventgroupdefGroupChild.z;
                eventgroupANUD.Value = eventgroupdefGroupChild.a;
                eventgroupdelootNUD.Value = eventgroupdefGroupChild.deloot;
                eventgroupLootminNUD.Value = eventgroupdefGroupChild.lootmin;
                eventgrouplootmaxNUD.Value = eventgroupdefGroupChild.lootmax;
                if (e.Button == MouseButtons.Right)
                {
                    removeChildToolStripMenuItem.Visible = true;
                    EventgroupContextMenu.Show(Cursor.Position);
                }
            }
            isUserInteraction = true;
        }
        private void addNewGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eventgroupdefGroup newvengroup = new eventgroupdefGroup()
            {
                name = "NewEventGroup",
                child = new BindingList<eventgroupdefGroupChild>()
            };
            eventgroupdef.group.Add(newvengroup);
            TreeNode neweventspawn = new TreeNode("NewEventGroup");
            neweventspawn.Tag = newvengroup;
            eventspawngroupTV.SelectedNode.Nodes.Add(neweventspawn);
            currentproject.cfgeventgroups.isDirty = true;
        }
        private void addChildToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eventgroupdefGroupChild eventgroupdefGroupChild = new eventgroupdefGroupChild()
            {
                type = "ClassName, Replace me please....",
                x = 0,
                ySpecified = true,
                y = 0,
                z = 0,
                a = 0,
                delootSpecified = true,
                deloot = 0,
                lootminSpecified = true,
                lootmin = 1,
                lootmaxSpecified = true,
                lootmax = 3
            };
            TreeNode eventgroupchile = new TreeNode(eventgroupdefGroupChild.type);
            eventgroupchile.Tag = eventgroupdefGroupChild;
            eventspawngroupTV.SelectedNode.Nodes.Add(eventgroupchile);
            eventgroupdefGroup.child.Add(eventgroupdefGroupChild);
            currentproject.cfgeventgroups.isDirty = true;
        }
        private void removeGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eventgroupdef.group.Remove(eventgroupdefGroup);
            eventspawngroupTV.SelectedNode.Remove();
            currentproject.cfgeventgroups.isDirty = true;
        }
        private void removeChildToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eventgroupdefGroup eventgroupdefGroup = eventspawngroupTV.SelectedNode.Parent.Tag as eventgroupdefGroup;
            eventspawngroupTV.SelectedNode.Remove();
            eventgroupdefGroup.child.Remove(eventgroupdefGroupChild);
            currentproject.cfgeventgroups.isDirty = true;
        }
        private void importChildrenFromdzeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Eventgroupname = eventgroupdefGroup.name;
            eventposdefEventPos eventspawn = eventposdef.findeventgroup(Eventgroupname);
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    DZE importfile = DZEHelpers.LoadFile(filePath);
                }
            }
        }
        private void eventgroupnameTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            eventgroupdefGroupChild.type = eventgroupnameTB.Text;
            currentproject.cfgeventgroups.isDirty = true;
        }
        private void eventgroupXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            eventgroupdefGroupChild.x = eventgroupXNUD.Value;
            currentproject.cfgeventgroups.isDirty = true;
        }
        private void eventgroupYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            eventgroupdefGroupChild.y = eventgroupYNUD.Value;
            currentproject.cfgeventgroups.isDirty = true;
        }
        private void eventgroupZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            eventgroupdefGroupChild.z = eventgroupZNUD.Value;
            currentproject.cfgeventgroups.isDirty = true;
        }
        private void eventgroupANUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            eventgroupdefGroupChild.a = eventgroupANUD.Value;
            currentproject.cfgeventgroups.isDirty = true;
        }
        private void eventgroupdelootNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            eventgroupdefGroupChild.deloot = (int)eventgroupdelootNUD.Value;
            currentproject.cfgeventgroups.isDirty = true;
        }
        private void eventgroupLootminNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            eventgroupdefGroupChild.lootmin = (int)eventgroupLootminNUD.Value;
            currentproject.cfgeventgroups.isDirty = true;
        }
        private void eventgrouplootmaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            eventgroupdefGroupChild.lootmax = (int)eventgrouplootmaxNUD.Value;
            currentproject.cfgeventgroups.isDirty = true;
        }
        #endregion eventgroups
        #region spawnabletypes
        public Spawnabletypesconfig currentspawnabletypesfile;
        public spawnabletypesType CurrentspawnabletypesType;
        public object CurrentspawnabletypesTypetype;
        private void LoadSpawnableTypes()
        {
            Console.WriteLine("Loading SpawnableTypes");
            isUserInteraction = false;
            SpawnabletypeslistLB.DisplayMember = "DisplayName";
            SpawnabletypeslistLB.ValueMember = "Value";
            SpawnabletypeslistLB.DataSource = currentproject.spawnabletypesList;
            isUserInteraction = true;
        }
        private void SpawnabletypeslistLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SpawnabletypeslistLB.SelectedItem as Spawnabletypesconfig == currentspawnabletypesfile) { return; }
            if (SpawnabletypeslistLB.SelectedIndex == -1) { return; }
            currentspawnabletypesfile = SpawnabletypeslistLB.SelectedItem as Spawnabletypesconfig;
            isUserInteraction = false;
            SetDamange();

            SpawnabletpesLB.DisplayMember = "DisplayName";
            SpawnabletpesLB.ValueMember = "Value";
            SpawnabletpesLB.DataSource = currentspawnabletypesfile.spawnabletypes.type;
            isUserInteraction = true;
        }
        private void SetDamange()
        {
            if (currentspawnabletypesfile.spawnabletypes.damage != null)
            {
                label24.Visible = true;
                label25.Visible = true;
                HasDamageCB.Checked = true;
                DamageMinNUD.Value = currentspawnabletypesfile.spawnabletypes.damage.min;
                DamageMaxNUD.Value = currentspawnabletypesfile.spawnabletypes.damage.max;
                DamageMinNUD.Visible = true;
                DamageMaxNUD.Visible = true;
            }
            else
            {
                label25.Visible = false;
                label24.Visible = false;
                HasDamageCB.Checked = false;
                DamageMinNUD.Visible = false;
                DamageMaxNUD.Visible = false;
            }
        }
        private void SpawnabletpesLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearInfo();

            //if (SpawnabletpesLB.SelectedItem as spawnabletypesType == CurrentspawnabletypesType) { return; }
            if (SpawnabletpesLB.SelectedIndex == -1) { return; }
            CurrentspawnabletypesType = SpawnabletpesLB.SelectedItem as spawnabletypesType;
            isUserInteraction = false;
            listBox6.DisplayMember = "DisplayName";
            listBox6.ValueMember = "Value";
            listBox6.DataSource = CurrentspawnabletypesType.Items;
            listBox6.SelectedIndex = -1;
            if (listBox6.Items.Count > 0)
            {
                listBox6.SelectedIndex = 0;
            }
            isUserInteraction = true;
        }
        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (listBox6.SelectedItem as object == CurrentspawnabletypesTypetype) { return; }
            if (listBox6.SelectedIndex == -1) { return; }
            isUserInteraction = false;
            CurrentspawnabletypesTypetype = listBox6.SelectedItem as object;
            ClearInfo();
            if (CurrentspawnabletypesTypetype is spawnabletypesTypeHoarder)
            {

            }
            else if (CurrentspawnabletypesTypetype is spawnabletypesTypeTag)
            {
                spawnabletypesTypeTag currenttag = CurrentspawnabletypesTypetype as spawnabletypesTypeTag;
                Spaenabletypestagbox.Visible = true;
                textBox2.Text = currenttag.name;
            }
            else if (CurrentspawnabletypesTypetype is spawnabletypesTypeCargo)
            {
                CargoGB.Visible = true;
                spawnabletypesTypeCargo currentcargo = CurrentspawnabletypesTypetype as spawnabletypesTypeCargo;
                CargochanceCB.Checked = !currentcargo.chanceSpecified;
                SetCargo();
            }
            else if (CurrentspawnabletypesTypetype is spawnabletypesTypeAttachments)
            {

                AttachmentGB.Visible = true;
                spawnabletypesTypeAttachments currentAttchment = CurrentspawnabletypesTypetype as spawnabletypesTypeAttachments;
                AttchmentIsPresetCB.Checked = !currentAttchment.chanceSpecified;
                setattchments();
            }
            isUserInteraction = true;
        }
        private void CargochanceCB_CheckedChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                spawnabletypesTypeCargo currentcargo = CurrentspawnabletypesTypetype as spawnabletypesTypeCargo;
                currentcargo.chanceSpecified = !CargochanceCB.Checked;
                if (currentcargo.chanceSpecified)
                {
                    currentcargo.preset = null;
                    currentcargo.chance = 1;
                    
                }
                else
                {
                    currentcargo.item = new BindingList<spawnabletypesTypeCargoItem>();
                    currentcargo.chance = 0;
                    
                }
            }
            SetCargo();
        }
        private void AttchmentIsPresetCB_CheckedChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                spawnabletypesTypeAttachments currentAttchment = CurrentspawnabletypesTypetype as spawnabletypesTypeAttachments;
                currentAttchment.chanceSpecified = !AttchmentIsPresetCB.Checked;
                if (currentAttchment.chanceSpecified)
                {
                    currentAttchment.preset = null;
                    currentAttchment.chance = 1;
                    AttachemntTB.Text = "";
                    currentAttchment.item = new BindingList<spawnabletypesTypeAttachmentsItem>();
                }
                else
                {
                    currentAttchment.item = new BindingList<spawnabletypesTypeAttachmentsItem>();
                    currentAttchment.chance = 0;
                    AttachemntTB.Text = "";
                }
            }
            setattchments();
        }
        private void setattchments()
        {
            spawnabletypesTypeAttachments currentAttchment = CurrentspawnabletypesTypetype as spawnabletypesTypeAttachments;
            if (currentAttchment.chanceSpecified)
            {
                AttachmentItemLB.Visible = true;
                CargoAttachmentRemoveButton.Visible = true;
                cargoattachemntAddButton.Visible = true;
                chancAttachmentselabel.Visible = true;
                AttachmentChangeItemButton.Visible = true;
                AttachmentPresetGB.Visible = false;
                ItemAttachmentchanceNUD.Visible = true;
                AttachmemtItemChanceLabel.Visible = true;
                AttachmentchanceNUD.Visible = true;
                AttachemntTB.Visible = false;
                AttachmentchanceNUD.Value = currentAttchment.chance;
                AttachmentItemLB.DisplayMember = "DisplayName";
                AttachmentItemLB.ValueMember = "Value";
                AttachmentItemLB.DataSource = currentAttchment.item;
            }
            else
            {
                AttachmentItemLB.Visible = false;
                CargoAttachmentRemoveButton.Visible = false;
                cargoattachemntAddButton.Visible = false;
                chancAttachmentselabel.Visible = false;
                AttachmentchanceNUD.Visible = false;
                ItemAttachmentchanceNUD.Visible = false;
                AttachemntTB.Visible = true;
                AttachmemtItemChanceLabel.Visible = false;
                AttachmentChangeItemButton.Visible = false;
                AttachmentPresetGB.Visible = true;
                if (currentAttchment.preset != null)
                    AttachemntTB.Text = currentAttchment.preset;
                else
                    AttachemntTB.Text = "";
            }
        }
        private void SetCargo()
        {
            spawnabletypesTypeCargo currentcargo = CurrentspawnabletypesTypetype as spawnabletypesTypeCargo;
            if (currentcargo.chanceSpecified)
            {
                cargoItemRemoveButton.Visible = true;
                CargoItemAddButton.Visible = true;
                CargoPresetTB.Visible = false;
                CargoItemchanceNUD.Visible = true;
                CargoItemLB.Visible = true;
                cargochanceLabel.Visible = true;
                CargoChangeItemButton.Visible = true;
                CargoPresetGB.Visible = false;
                CarcgoChanceNUD.Visible = true;
                CarcgoChanceNUD.Value = currentcargo.chance;
                CargoItemLB.DisplayMember = "DisplayName";
                CargoItemLB.ValueMember = "Value";
                CargoItemLB.DataSource = currentcargo.item;
                checkBox49.Visible = true;
            }
            else
            {
                cargoItemRemoveButton.Visible = false;
                CargoItemAddButton.Visible = false;
                CargoPresetTB.Visible = true;
                CargoItemchanceNUD.Visible = false;
                CargoItemLB.Visible = false;
                CarcgoChanceNUD.Visible = false;
                cargochanceLabel.Visible = false;
                CargoChangeItemButton.Visible = false;
                CargoPresetGB.Visible = true;
                checkBox49.Visible = false;
                if (currentcargo.preset != null)
                    CargoPresetTB.Text = currentcargo.preset;
                else
                    CargoPresetTB.Text = "";
            }
        }
        private void darkButton36_Click(object sender, EventArgs e)
        {
            randompresetsCargo newcargopreset = CargoPresetComboBox.SelectedItem as randompresetsCargo;
            spawnabletypesTypeCargo currentcargo = CurrentspawnabletypesTypetype as spawnabletypesTypeCargo;
            currentcargo.preset = newcargopreset.name;
            SetCargo();
            currentspawnabletypesfile.isDirty = true;
        }
        private void darkButton37_Click(object sender, EventArgs e)
        {
            randompresetsAttachments newattachmentpreset = AttachmentPresetComboBox.SelectedItem as randompresetsAttachments;
            spawnabletypesTypeAttachments currentattchemnt = CurrentspawnabletypesTypetype as spawnabletypesTypeAttachments;
            currentattchemnt.preset = newattachmentpreset.name;
            setattchments();
            currentspawnabletypesfile.isDirty = true;
        }
        private void ClearInfo()
        {
            textBox2.Text = null;
            Spaenabletypestagbox.Visible = false;
            AttachmentGB.Visible = false;
            CargoGB.Visible = false;
        }
        private void darkButton34_Click(object sender, EventArgs e)
        {
            if (listBox6.SelectedItems == null) { return; }
            int index = listBox6.SelectedIndex;
            CurrentspawnabletypesType.Items.Remove(CurrentspawnabletypesTypetype);
            currentspawnabletypesfile.isDirty = true;
            listBox6.SelectedIndex = -1;
            if (index == 0 && listBox6.Items.Count > 0)
                listBox6.SelectedIndex = index;
            else
                listBox6.SelectedIndex = index - 1;
        }
        private void darkButton30_Click(object sender, EventArgs e)
        {
            if (SpawnabletpesLB.SelectedItem == null) { return; }
            if (CurrentspawnabletypesType.Items == null)
            {
                CurrentspawnabletypesType.Items = new BindingList<object>
                {
                    new spawnabletypesTypeHoarder()
                };
            }
            else
                CurrentspawnabletypesType.Items.Insert(0, new spawnabletypesTypeHoarder());
            listBox6.SelectedIndex = -1;
            listBox6.SelectedIndex = 0;
            currentspawnabletypesfile.isDirty = true;

        }
        private void darkButton31_Click(object sender, EventArgs e)
        {
            if (SpawnabletpesLB.SelectedItem == null) { return; }
            if (CurrentspawnabletypesType.Items == null)
            {
                CurrentspawnabletypesType.Items = new BindingList<object>
                {
                     new spawnabletypesTypeTag()
                };
            }
            else
                CurrentspawnabletypesType.Items.Insert(0, new spawnabletypesTypeTag());
            listBox6.SelectedIndex = -1;
            listBox6.SelectedIndex = 0;
            currentspawnabletypesfile.isDirty = true;
        }
        private void darkButton32_Click(object sender, EventArgs e)
        {
            if (SpawnabletpesLB.SelectedItem == null) { return; }
            spawnabletypesTypeCargo newcargo = new spawnabletypesTypeCargo()
            {
                item = new BindingList<spawnabletypesTypeCargoItem>()
            };
            if (CurrentspawnabletypesType.Items == null)
                CurrentspawnabletypesType.Items = new BindingList<object>();
            CurrentspawnabletypesType.Items.Add(newcargo);
            listBox6.SelectedIndex = -1;
            listBox6.SelectedIndex = 0;
            currentspawnabletypesfile.isDirty = true;
        }
        private void darkButton33_Click(object sender, EventArgs e)
        {
            if (SpawnabletpesLB.SelectedItem == null) { return; }
            spawnabletypesTypeAttachments newspawnabletypesTypeAttachments = new spawnabletypesTypeAttachments();
            if (CurrentspawnabletypesType.Items == null)
                CurrentspawnabletypesType.Items = new BindingList<object>();
            CurrentspawnabletypesType.Items.Add(newspawnabletypesTypeAttachments);
            listBox6.SelectedIndex = -1;
            listBox6.SelectedIndex = 0;
            currentspawnabletypesfile.isDirty = true;
        }
        private void darkButton29_Click(object sender, EventArgs e)
        {
            typesTypeTag t = comboBox5.SelectedItem as typesTypeTag;
            spawnabletypesTypeTag currenttag = CurrentspawnabletypesTypetype as spawnabletypesTypeTag;
            currenttag.name = t.name;
            textBox2.Text = currenttag.name;
            currentspawnabletypesfile.isDirty = true;
        }
        private void AttachmentChangeItemButton_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseOnlySingleitem = true
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    spawnabletypesTypeAttachmentsItem currentcargoitem = AttachmentItemLB.SelectedItem as spawnabletypesTypeAttachmentsItem;
                    currentcargoitem.name = l;
                    AttachmentItemLB.Refresh();
                    currentspawnabletypesfile.isDirty = true;
                    setattchments();
                }

            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void CargoChangeItemButton_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseOnlySingleitem = true
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    spawnabletypesTypeCargoItem currentcargoitem = CargoItemLB.SelectedItem as spawnabletypesTypeCargoItem;
                    currentcargoitem.name = l;
                    CargoItemLB.Refresh();
                    currentspawnabletypesfile.isDirty = true;
                    SetCargo();
                }

            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void AttachmentchanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            spawnabletypesTypeAttachments currentattachment = CurrentspawnabletypesTypetype as spawnabletypesTypeAttachments;
            currentattachment.chance = AttachmentchanceNUD.Value;
            currentspawnabletypesfile.isDirty = true;
        }
        private void CarcgoChanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            spawnabletypesTypeCargo currentcargo = CurrentspawnabletypesTypetype as spawnabletypesTypeCargo;
            currentcargo.chance = CarcgoChanceNUD.Value;
            currentspawnabletypesfile.isDirty = true;
        }
        private void listBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CargoItemLB.SelectedItems.Count == 0) return;
            spawnabletypesTypeCargoItem currentcargoitem = CargoItemLB.SelectedItem as spawnabletypesTypeCargoItem;
            isUserInteraction = false;
            if (currentcargoitem.chanceSpecified)
            {
                checkBox49.Checked = currentcargoitem.chanceSpecified;
                CargoItemchanceNUD.Value = currentcargoitem.chance;
            }
            isUserInteraction = true;
        }
        private void checkBox49_CheckedChanged(object sender, EventArgs e)
        {
            CargoItemchanceNUD.Visible = checkBox49.Checked;
            if (!isUserInteraction) return;
            spawnabletypesTypeCargoItem currentcargoitem = CargoItemLB.SelectedItem as spawnabletypesTypeCargoItem;
            currentcargoitem.chanceSpecified = checkBox49.Checked;
            if(currentcargoitem.chanceSpecified)
            {
                CargoItemchanceNUD.Value = currentcargoitem.chance = 1;
            }
            else
            {

            }
            currentspawnabletypesfile.isDirty = true;
        }
        private void AttachmentItemLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AttachmentItemLB.SelectedItems.Count == 0) return;
            spawnabletypesTypeAttachmentsItem currentAttachmentitem = AttachmentItemLB.SelectedItem as spawnabletypesTypeAttachmentsItem;
            isUserInteraction = false;
            ItemAttachmentchanceNUD.Value = currentAttachmentitem.chance;
            isUserInteraction = true;
        }
        private void darkButton35_Click_1(object sender, EventArgs e)
        {
            spawnabletypesTypeCargoItem newitem = new spawnabletypesTypeCargoItem()
            {
                name = "New Item, Please change me...",
                chance = 1
            };
            spawnabletypesTypeCargo currentcargo = CurrentspawnabletypesTypetype as spawnabletypesTypeCargo;
            currentcargo.item.Add(newitem);
            currentspawnabletypesfile.isDirty = true;
        }
        private void darkButton39_Click(object sender, EventArgs e)
        {
            if (CargoItemLB.SelectedItems == null) { return; }
            int index = CargoItemLB.SelectedIndex;
            spawnabletypesTypeCargoItem currentcargoitem = CargoItemLB.SelectedItem as spawnabletypesTypeCargoItem;
            spawnabletypesTypeCargo currentcargo = CurrentspawnabletypesTypetype as spawnabletypesTypeCargo;
            currentcargo.item.Remove(currentcargoitem);
            currentspawnabletypesfile.isDirty = true;
            CargoItemLB.SelectedIndex = -1;
            if (index == 0 && CargoItemLB.Items.Count > 0)
                CargoItemLB.SelectedIndex = index;
            else
                CargoItemLB.SelectedIndex = index - 1;
        }
        private void CargoItemchanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            spawnabletypesTypeCargoItem currentcargoitem = CargoItemLB.SelectedItem as spawnabletypesTypeCargoItem;
            currentcargoitem.chance = CargoItemchanceNUD.Value;
            currentspawnabletypesfile.isDirty = true;
        }
        private void ItemAttachmentchanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            spawnabletypesTypeAttachmentsItem currentattachmentitem = AttachmentItemLB.SelectedItem as spawnabletypesTypeAttachmentsItem;
            currentattachmentitem.chance = ItemAttachmentchanceNUD.Value;
            currentspawnabletypesfile.isDirty = true;
        }
        private void darkButton38_Click_1(object sender, EventArgs e)
        {
            spawnabletypesTypeAttachmentsItem newitem = new spawnabletypesTypeAttachmentsItem()
            {
                name = "New Item, Please change me...",
                chance = 1
            };
            spawnabletypesTypeAttachments currentattachment = CurrentspawnabletypesTypetype as spawnabletypesTypeAttachments;
            currentattachment.item.Add(newitem);
            currentspawnabletypesfile.isDirty = true;
        }
        private void darkButton35_Click_2(object sender, EventArgs e)
        {
            if (AttachmentItemLB.SelectedItems == null) { return; }
            int index = AttachmentItemLB.SelectedIndex;
            spawnabletypesTypeAttachmentsItem currentAttachmentitem = AttachmentItemLB.SelectedItem as spawnabletypesTypeAttachmentsItem;
            spawnabletypesTypeAttachments currentAttachment = CurrentspawnabletypesTypetype as spawnabletypesTypeAttachments;
            currentAttachment.item.Remove(currentAttachmentitem);
            currentspawnabletypesfile.isDirty = true;
            AttachmentItemLB.SelectedIndex = -1;
            if (index == 0 && AttachmentItemLB.Items.Count > 0)
                AttachmentItemLB.SelectedIndex = index;
            else
                AttachmentItemLB.SelectedIndex = index - 1;
        }
        private void darkButton35_Click_3(object sender, EventArgs e)
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
                    spawnabletypesType newspawnabletypesType = new spawnabletypesType()
                    {
                        name = l,
                        Items = new BindingList<object>()

                    };
                    currentspawnabletypesfile.spawnabletypes.type.Add(newspawnabletypesType);
                    currentspawnabletypesfile.isDirty = true;
                }

            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }

        }
        private void darkButton38_Click_2(object sender, EventArgs e)
        {
            currentspawnabletypesfile.spawnabletypes.type.Remove(SpawnabletpesLB.SelectedItem as spawnabletypesType);
            currentspawnabletypesfile.isDirty = true;
        }
        private void darkButton39_Click_1(object sender, EventArgs e)
        {
            AddNeweventFile form = new AddNeweventFile
            {
                currentproject = currentproject,
                newlocation = true,
                setbuttontest = "Add Spawnabletype File",
                SetTitle = "Add New Spawnabletypes File",
                settype = "Spawnabletype file Name (Use ModName.. eg. DorTags)"
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                string path = form.CustomLocation;
                string modname = form.TypesName;
                Directory.CreateDirectory(path);
                List<string> Spawnabletypesfile = new List<string>
                {
                    "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>",
                    "<spawnabletypes>",
                    "</spawnabletypes>"
                };
                File.WriteAllLines(path + "\\" + modname + "_spawnabletypes.xml", Spawnabletypesfile);
                Spawnabletypesconfig test = new Spawnabletypesconfig(path + "\\" + modname + "_spawnabletypes.xml");
                test.Savespawnabletypes();
                currentproject.EconomyCore.AddCe(path.Replace(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\", ""), modname + "_spawnabletypes.xml", "spawnabletypes");
                currentproject.EconomyCore.SaveEconomycore();
                currentproject.SetSpawnabletypes();
                LoadSpawnableTypes();
                populateEconmyTreeview();
            }
        }
        private void darkButton40_Click(object sender, EventArgs e)
        {
            string Modname = Path.GetFileNameWithoutExtension(currentspawnabletypesfile.Filename);
            currentproject.EconomyCore.RemoveCe(Modname, out string foflderpath, out string filename, out bool deletedirectory);
            File.Delete(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\" + foflderpath + "\\" + filename);
            if (deletedirectory)
                Directory.Delete(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\" + foflderpath, true);
            currentproject.EconomyCore.SaveEconomycore();
            currentproject.removeSpawnabletype(currentspawnabletypesfile.Filename);
            currentproject.SetSpawnabletypes();
            populateEconmyTreeview();
        }
        private void HasDamageCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            if (HasDamageCB.Checked)
            {
                if (currentspawnabletypesfile.spawnabletypes.damage == null)
                {
                    currentspawnabletypesfile.spawnabletypes.damage = new spawnabletypesDamage()
                    {
                        min = 0,
                        max = 0
                    };
                }
            }
            else
            {
                if (currentspawnabletypesfile.spawnabletypes.damage != null)
                {
                    currentspawnabletypesfile.spawnabletypes.damage = null;
                }
            }
            currentspawnabletypesfile.isDirty = true;
            SetDamange();
        }
        #endregion spawnabletypes
        #region typesquery
        public bool FilterTiers = false;
        public bool FilterCategories = false;
        public bool FilterLocations = false;
        public bool FilterTags = false;
        public bool FilterFlags = false;
        private void darkButton15_Click(object sender, EventArgs e)
        {
            treeView2.Nodes.Clear();
        }
        public bool typeinfilterlist(typesType type, List<string> Queryitems)
        {
            bool istrue = true;
            if (Queryitems.Count == 0) { return true; }
            foreach (string filter in Queryitems)
            {
                string[] fsplit = filter.Split(',');
                switch (fsplit[1])
                {
                    case "definintions":
                        if (type.value == null)
                        {
                            istrue = false;
                            break;
                        }
                        switch (fsplit[2])
                        {
                            case "0":
                                string def = fsplit[0];
                                if (!type.value.Any(x => x.name == def))
                                    istrue = false;
                                break;
                            case "1":
                                def = fsplit[0];
                                if (!type.value.Any(x => x.name == def))
                                    istrue = false;
                                break;
                        }
                        break;
                    case "categories":
                        string cat = fsplit[0];
                        string typecat = "";
                        if (type.category == null)
                            typecat = "Other";
                        else
                            typecat = type.category.name;
                        if (typecat != cat)
                            istrue = false;
                        break;
                    case "usage":
                        string usage = fsplit[0];
                        if (type.usage == null)
                        {
                            istrue = false;
                        }
                        else
                        {
                            if (!type.usage.Any(x => x.name == usage))
                                istrue = false;
                        }
                        break;
                    case "tags":
                        string tag = fsplit[0];
                        if (type.tag == null)
                        {
                            if (tag != "NULL")
                                istrue = false;
                        }
                        else
                        {
                            if (!type.tag.Any(x => x.name == tag))
                                istrue = false;
                        }
                        break;
                    case "flags":
                        {
                            switch (fsplit[0])
                            {
                                case "deloot":
                                    if (type.flags.deloot != 1)
                                        istrue = false;
                                    break;
                                case "crafted":
                                    if (type.flags.crafted != 1)
                                        istrue = false;
                                    break;
                                case "count_in_player":
                                    if (type.flags.count_in_player != 1)
                                        istrue = false;
                                    break;
                                case "count_in_map":
                                    if (type.flags.count_in_map != 1)
                                        istrue = false;
                                    break;
                                case "count_in_hoarder":
                                    if (type.flags.count_in_hoarder != 1)
                                        istrue = false;
                                    break;
                                case "count_in_cargo":
                                    if (type.flags.count_in_cargo != 1)
                                        istrue = false;
                                    break;
                            }
                        }

                        break;

                }
            }
            if (istrue)
                return true;
            return false;
        }
        private void darkButton14_Click(object sender, EventArgs e)
        {
            List<string> Queryitems = new List<string>();
            int totalnominals = 0;
            //check Tiers
            //need to check if the fil;ter is for vanilla or user definitions
            if (FilterTiers)
            {
                switch (tabControl8.SelectedIndex)
                {
                    case 0:
                        List<CheckBox> checkboxesSummary = SetdefinitionsTPSummary.Controls.OfType<CheckBox>().ToList();
                        foreach (CheckBox cb in checkboxesSummary)
                        {
                            if (cb.Visible == true && cb.Checked)
                                Queryitems.Add(cb.Tag.ToString() + ",definintions,0");
                        }
                        break;
                    case 1:
                        checkboxesSummary = UserdefinitionsTPSummary.Controls.OfType<CheckBox>().ToList();
                        foreach (CheckBox cb in checkboxesSummary)
                        {
                            if (cb.Visible == true && cb.Checked)
                                Queryitems.Add(cb.Tag.ToString() + ",definintions,1");
                        }
                        break;
                }
            }
            //Check Categorys
            if (FilterCategories)
            {
                if (listBox5.Items.Count > 0)
                {
                    foreach (var item in listBox5.Items)
                    {
                        listsCategory c = item as listsCategory;
                        Queryitems.Add(c.name + ",categories");
                    }
                }
                else
                {
                    Queryitems.Add("Other,categories");
                }
            }
            // check locations (usage)
            if (FilterLocations)
            {
                if (listBox4.Items.Count > 0)
                {
                    foreach (var item in listBox4.Items)
                    {
                        listsUsage u = item as listsUsage;
                        Queryitems.Add(u.name + ",usage");
                    }
                }
            }
            //Check tag
            if (FilterTags)
            {
                if (listBox3.Items.Count > 0)
                {
                    foreach (var item in listBox3.Items)
                    {
                        listsTag c = item as listsTag;
                        Queryitems.Add(c.name + ",tags");
                    }
                }
                else
                {
                    Queryitems.Add("NULL,tags");
                }
            }
            //check flags
            if (FilterFlags)
            {
                if (checkBox43.Checked)
                    Queryitems.Add("deloot,flags");
                if (checkBox44.Checked)
                    Queryitems.Add("crafted,flags");
                if (checkBox45.Checked)
                    Queryitems.Add("count_in_player,flags");
                if (checkBox46.Checked)
                    Queryitems.Add("count_in_map,flags");
                if (checkBox47.Checked)
                    Queryitems.Add("count_in_hoarder,flags");
                if (checkBox48.Checked)
                    Queryitems.Add("count_in_cargo,flags");
            }

            int count = 0;
            treeView2.Nodes.Clear();
            TreeNode root = new TreeNode(Path.GetFileName(filename))
            {
                Tag = "Parent"
            };
            //Set Vanilla Treenode types
            TreeNode vanilla = new TreeNode("Vanilla Types")
            {
                Tag = "VanillaTypes"
            };
            foreach (typesType type in vanillatypes.types.type)
            {
                if (!typeinfilterlist(type, Queryitems)) { continue; }
                if (ZeroNomCB.Checked)
                {
                    if (type.nominal == 0)
                        continue;
                }
                string cat = "other";
                if (type.category != null)
                    cat = type.category.name;
                TreeNode typenode = new TreeNode(type.name)
                {
                    Tag = type
                };
                if (!vanilla.Nodes.ContainsKey(cat))
                {
                    TreeNode newcatnode = new TreeNode(cat)
                    {
                        Name = cat,
                        Tag = cat
                    };

                    vanilla.Nodes.Add(newcatnode);
                }
                totalnominals += type.nominal;
                count++;
                vanilla.Nodes[cat].Nodes.Add(typenode);
            }
            root.Nodes.Add(vanilla);

            if (ModTypes.Count > 0)
            {
                foreach (TypesFile tf in ModTypes)
                {
                    TreeNode ModTypes = new TreeNode(tf.modname)
                    {
                        Tag = tf.modname
                    };
                    foreach (typesType type in tf.types.type)
                    {
                        if (!typeinfilterlist(type, Queryitems)) { continue; }
                        if (ZeroNomCB.Checked)
                        {
                            if (type.nominal == 0)
                                continue;
                        }
                        string cat = "other";
                        if (type.category != null)
                            cat = type.category.name;
                        TreeNode typenode = new TreeNode(type.name)
                        {
                            Tag = type
                        };
                        if (!ModTypes.Nodes.ContainsKey(cat))
                        {
                            TreeNode newcatnode = new TreeNode(cat)
                            {
                                Name = cat,
                                Tag = cat
                            };
                            ModTypes.Nodes.Add(newcatnode);
                        }
                        totalnominals += type.nominal;
                        count++;
                        ModTypes.Nodes[cat].Nodes.Add(typenode);
                    }


                    root.Nodes.Add(ModTypes);
                }
            }
            darkLabel6.Text = "Found Items :- " + count.ToString();
            darkLabel27.Text = "Total Number of nominals :- " + totalnominals.ToString();
            treeView2.Nodes.Add(root);
        }
        private void darkButton12_Click(object sender, EventArgs e)
        {
            listsCategory c = comboBox8.SelectedItem as listsCategory;
            if (!listBox5.Items.Contains(c))
                listBox5.Items.Add(c);
        }
        private void darkButton13_Click(object sender, EventArgs e)
        {
            if (listBox5.SelectedItems.Count > 0)
            {
                listsCategory c = listBox5.SelectedItem as listsCategory;
                listBox5.Items.Remove(c);
            }
        }
        private void darkButton10_Click(object sender, EventArgs e)
        {
            listsUsage u = comboBox7.SelectedItem as listsUsage;
            if (!listBox4.Items.Contains(u))
                listBox4.Items.Add(u);
        }
        private void darkButton11_Click(object sender, EventArgs e)
        {
            if (listBox4.SelectedItems.Count > 0)
            {
                listsUsage u = listBox4.SelectedItem as listsUsage;
                listBox4.Items.Remove(u);
            }
        }
        private void darkButton4_Click(object sender, EventArgs e)
        {
            listsTag t = comboBox6.SelectedItem as listsTag;
            if (!listBox3.Items.Contains(t))
                listBox3.Items.Add(t);
        }
        private void darkButton9_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedItems.Count > 0)
            {
                listsTag t = listBox3.SelectedItem as listsTag;
                listBox3.Items.Remove(t);
            }
        }
        private void checkBox69_CheckedChanged(object sender, EventArgs e)
        {
            FilterTiers = groupBox18.Enabled = checkBox69.Checked;
        }
        private void checkBox68_CheckedChanged(object sender, EventArgs e)
        {
            FilterCategories = groupBox13.Enabled = checkBox68.Checked;
        }
        private void checkBox67_CheckedChanged(object sender, EventArgs e)
        {
            FilterLocations = groupBox16.Enabled = checkBox67.Checked;
        }
        private void checkBox66_CheckedChanged(object sender, EventArgs e)
        {
            FilterTags = groupBox14.Enabled = checkBox66.Checked;
        }
        private void checkBox65_CheckedChanged(object sender, EventArgs e)
        {
            FilterFlags = groupBox19.Enabled = checkBox65.Checked;
        }
        private void darkButton54_Click(object sender, EventArgs e)
        {

        }
        private void darkButton55_Click(object sender, EventArgs e)
        {
            List<string> nopositions = new List<string>();
            int Total = 0;
            foreach (mapGroup lootposition in currentproject.mapgrouppos.map.group)
            {
                prototypeGroup group = currentproject.mapgroupproto.prototypeGroup.group.FirstOrDefault(x => x.name.ToLower() == lootposition.name.ToLower());
                if (group == null)
                {
                    nopositions.Add(lootposition.name);
                    continue;
                }
                if (group.lootmax == 0)
                {
                    Total += currentproject.mapgroupproto.prototypeGroup.defaults.FirstOrDefault(x => x.name == "group").lootmax;
                }
                else
                {
                    Total += currentproject.mapgroupproto.prototypeGroup.group.First(x => x.name.ToLower() == lootposition.name.ToLower()).lootmax;
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("Total Maximum Loot Positions : " + Total.ToString() + Environment.NewLine);
            if (nopositions.Count > 0)
            {
                sb.Append("Please see LootOutput.txt in project folder for Items with no loot positions");
            }
            MessageBox.Show(sb.ToString());
            if (nopositions.Count > 0)
            {
                foreach (string s in nopositions)
                {
                    sb.Append(s + Environment.NewLine);
                }
                File.WriteAllText(currentproject.projectFullName + "//LootOutput.txt", sb.ToString());
            }
        }
        #endregion typesquery
        #region PlayerSpawnPoints
        public int PlayerSpawnScale = 1;
        public playerspawnpoints playerspawnpoints;
        public playerspawnpointssection Currentplayerspawnpointssection;
        public playerspawnpointsGroup currentplayerspawnpointsGroup;
        public playerspawnpointsGroupPos currentpos;
        private void PlayerSpawnFreshButton_Click(object sender, EventArgs e)
        {
            PlayerSpawnFreshButton.Checked = true;
            PlayerSpawnHopButton.Checked = false;
            PlayerSpawnTravelButton.Checked = false;
            Currentplayerspawnpointssection = playerspawnpoints.fresh;
            SetPlayerSpawnLists();
        }
        private void PlayerSpawnHopButton_Click(object sender, EventArgs e)
        {
            generatorposbubblesPosLB.DataSource = null;
            currentplayerspawnpointsGroup = null;
            pictureBox2.Invalidate();
            PlayerSpawnFreshButton.Checked = false;
            PlayerSpawnHopButton.Checked = true;
            PlayerSpawnTravelButton.Checked = false;
            Currentplayerspawnpointssection = playerspawnpoints.hop;
            SetPlayerSpawnLists();
        }
        private void PlayerSpawnTravelButton_Click(object sender, EventArgs e)
        {
            PlayerSpawnFreshButton.Checked = false;
            PlayerSpawnHopButton.Checked = false;
            PlayerSpawnTravelButton.Checked = true;
            Currentplayerspawnpointssection = playerspawnpoints.travel;
            SetPlayerSpawnLists();
        }
        public void LoadPlayerSpawns()
        {
            Console.WriteLine("Loading PlayerSpawn");
            isUserInteraction = false;
            playerspawnpoints = currentproject.cfgplayerspawnpoints.playerspawnpoints;
            Currentplayerspawnpointssection = playerspawnpoints.fresh;
            PlayerSpawnFreshButton.Checked = true;
            SetPlayerSpawnLists();
            isUserInteraction = true;
        }
        private void SetPlayerSpawnLists()
        {
            isUserInteraction = false;
            SpawnParamsmin_dist_infectedNUD.Value = (decimal)Currentplayerspawnpointssection.spawn_params.min_dist_infected;
            SpawnParamsmax_dist_infectedNUD.Value = (decimal)Currentplayerspawnpointssection.spawn_params.max_dist_infected;
            SpawnParamsmin_dist_playerNUD.Value = (decimal)Currentplayerspawnpointssection.spawn_params.min_dist_player;
            SpawnParamsmax_dist_playerNUD.Value = (decimal)Currentplayerspawnpointssection.spawn_params.max_dist_player;
            SpawnParamsmin_dist_staticNUD.Value = (decimal)Currentplayerspawnpointssection.spawn_params.min_dist_static;
            SpawnParamsmax_dist_staticNUD.Value = (decimal)Currentplayerspawnpointssection.spawn_params.max_dist_static;

            generatorparamsgrid_densityNUD.Value = (decimal)Currentplayerspawnpointssection.generator_params.grid_density;
            generatorparamsgrid_widthNUD.Value = (decimal)Currentplayerspawnpointssection.generator_params.grid_width;
            generatorparamsgrid_heightNUD.Value = (decimal)Currentplayerspawnpointssection.generator_params.grid_height;
            generatorparamsmin_dist_staticNUD.Value = (decimal)Currentplayerspawnpointssection.generator_params.min_dist_static;
            generatorparamsmax_dist_staticNUD.Value = (decimal)Currentplayerspawnpointssection.generator_params.max_dist_static;
            generatorparamsmin_steepnessNUD.Value = (decimal)Currentplayerspawnpointssection.generator_params.min_steepness;
            generatorparamsmax_steepnessNUD.Value = (decimal)Currentplayerspawnpointssection.generator_params.max_steepness;

            GroupParamsenablegroupsCB.Checked = Currentplayerspawnpointssection.group_params.enablegroups;
            GroupParamslifetimeNUD.Value = Currentplayerspawnpointssection.group_params.lifetime;
            GroupParamscounterNUD.Value = Currentplayerspawnpointssection.group_params.counter;
            generatorposbubblesLB.DisplayMember = "DisplayName";
            generatorposbubblesLB.ValueMember = "Value";
            generatorposbubblesLB.DataSource = Currentplayerspawnpointssection.generator_posbubbles;
            isUserInteraction = true;

        }
        private void generatorposbubblesLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (generatorposbubblesLB.SelectedItems.Count == 0) return;
            currentplayerspawnpointsGroup = generatorposbubblesLB.SelectedItem as playerspawnpointsGroup;
            isUserInteraction = false;
            generatorposbubblesGroupnameTB.Text = currentplayerspawnpointsGroup.name;
            if (generatorposbubblesUseLifetimeCB.Checked = currentplayerspawnpointsGroup.lifetimeSpecified == true)
            {
                generatorposbubbleslifetiemNUD.Value = currentplayerspawnpointsGroup.lifetime;
            }
            if(generatorposbubblesusecounterCB.Checked = currentplayerspawnpointsGroup.counterSpecified == true)
            {
                generatorposbubblescounterNUD.Value = currentplayerspawnpointsGroup.counter;
            }
            generatorposbubblesPosLB.DisplayMember = "DisplayName";
            generatorposbubblesPosLB.ValueMember = "Value";
            generatorposbubblesPosLB.DataSource = currentplayerspawnpointsGroup.pos;

            isUserInteraction = true;
            pictureBox2.Invalidate();
        }
        private void generatorposbubblesGroupnameTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            currentplayerspawnpointsGroup.name = generatorposbubblesGroupnameTB.Text;
            generatorposbubblesLB.Refresh();
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void generatorposbubbleslifetiemNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            currentplayerspawnpointsGroup.lifetime = (int)generatorposbubbleslifetiemNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void generatorposbubblescounterNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            currentplayerspawnpointsGroup.counter = (int)generatorposbubblescounterNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void generatorposbubblesUseLifetimeCB_CheckedChanged(object sender, EventArgs e)
        {
            if (generatorposbubblesUseLifetimeCB.Checked == true)
            {
                generatorposbubbleslifetiemNUD.Visible = true;
                label42.Visible = true;
                if (isUserInteraction)
                {
                    currentplayerspawnpointsGroup.lifetimeSpecified = true;
                    currentplayerspawnpointsGroup.lifetime = 0;
                }
            }
            else
            {
                generatorposbubbleslifetiemNUD.Visible = false;
                label42.Visible = false;
                if (isUserInteraction)
                {
                    currentplayerspawnpointsGroup.lifetimeSpecified = false;
                }
            }
            
        }
        private void generatorposbubblesusecounterCB_CheckedChanged(object sender, EventArgs e)
        {
            if (generatorposbubblesusecounterCB.Checked == true)
            {
                generatorposbubblescounterNUD.Visible = true;
                label43.Visible = true;
                if (isUserInteraction)
                {
                    currentplayerspawnpointsGroup.counterSpecified = true;
                    currentplayerspawnpointsGroup.counter = 0;
                }
            }
            else
            {
                generatorposbubblescounterNUD.Visible = false;
                label43.Visible = false;
                if (isUserInteraction)
                {
                    currentplayerspawnpointsGroup.counterSpecified = false;
                }
            }
        }
        private void darkButton24_Click(object sender, EventArgs e)
        {
            playerspawnpointsGroup newgroup = new playerspawnpointsGroup()
            {
                name = "Change Me",
                lifetimeSpecified = false,
                counterSpecified = false,
                pos = new BindingList<playerspawnpointsGroupPos>()
            };
            Currentplayerspawnpointssection.generator_posbubbles.Add(newgroup);
            generatorposbubblesLB.SelectedIndex = generatorposbubblesLB.Items.Count - 1;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void darkButton25_Click(object sender, EventArgs e)
        {
            if (generatorposbubblesLB.SelectedItems.Count < 0) return;
            Currentplayerspawnpointssection.generator_posbubbles.Remove(currentplayerspawnpointsGroup);
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void generatorposbubblesPosLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (generatorposbubblesPosLB.SelectedItems.Count < 1) return;
            currentpos = generatorposbubblesPosLB.SelectedItem as playerspawnpointsGroupPos;
            pictureBox2.Invalidate();
            isUserInteraction = false;
            FreshPosXNUD.Value = currentpos.x;
            FreshPosZNUD.Value = currentpos.z;
            isUserInteraction = true;
        }
        private void FreshPosXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            currentpos.x = (decimal)FreshPosXNUD.Value;
            generatorposbubblesPosLB.Invalidate();
            currentproject.cfgplayerspawnpoints.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void FreshPosZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            currentpos.z = (decimal)FreshPosZNUD.Value;
            generatorposbubblesPosLB.Invalidate();
            currentproject.cfgplayerspawnpoints.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void darkButton23_Click(object sender, EventArgs e)
        {
            playerspawnpointsGroupPos newpos = new playerspawnpointsGroupPos()
            {
               x = currentproject.MapSize / 2,
                z = currentproject.MapSize / 2
            };
            currentplayerspawnpointsGroup.pos.Add(newpos);
            generatorposbubblesPosLB.Invalidate();
            generatorposbubblesPosLB.SelectedIndex = generatorposbubblesPosLB.Items.Count - 1;
            currentproject.cfgplayerspawnpoints.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void darkButton22_Click(object sender, EventArgs e)
        {
            if (generatorposbubblesPosLB.SelectedItems.Count == 0) return;
            currentplayerspawnpointsGroup.pos.Remove(currentpos);
            generatorposbubblesPosLB.Invalidate();
            currentproject.cfgplayerspawnpoints.isDirty = true;
            pictureBox2.Invalidate();
        }
        private void trackBar4_MouseUp(object sender, MouseEventArgs e)
        {
            PlayerSpawnScale = trackBar4.Value;
            SetSpawnScale();
        }
        private void min_dist_infectedNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            Currentplayerspawnpointssection.spawn_params.min_dist_infected = SpawnParamsmin_dist_infectedNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void min_dist_playerNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            Currentplayerspawnpointssection.spawn_params.min_dist_player = SpawnParamsmin_dist_playerNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void min_dist_staticNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            Currentplayerspawnpointssection.spawn_params.min_dist_static = SpawnParamsmin_dist_staticNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void max_dist_infectedNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            Currentplayerspawnpointssection.spawn_params.max_dist_infected = SpawnParamsmax_dist_infectedNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void max_dist_playerNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            Currentplayerspawnpointssection.spawn_params.max_dist_player = SpawnParamsmax_dist_playerNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void max_dist_staticNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            Currentplayerspawnpointssection.spawn_params.max_dist_static = SpawnParamsmax_dist_staticNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void grid_densityNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            Currentplayerspawnpointssection.generator_params.grid_density = (int)generatorparamsgrid_densityNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void grid_widthNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            Currentplayerspawnpointssection.generator_params.grid_width = (int)generatorparamsgrid_widthNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void GPmin_dist_staticNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            Currentplayerspawnpointssection.generator_params.min_dist_static = (int)generatorparamsmin_dist_staticNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void min_steepnessNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            Currentplayerspawnpointssection.generator_params.min_steepness = (int)generatorparamsmin_steepnessNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void grid_heightNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            Currentplayerspawnpointssection.generator_params.grid_height = (int)generatorparamsgrid_heightNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void GPmax_dist_staticNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            Currentplayerspawnpointssection.generator_params.max_dist_static = (int)generatorparamsmax_dist_staticNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void max_steepnessNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            Currentplayerspawnpointssection.generator_params.max_steepness = (int)generatorparamsmax_steepnessNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void GroupParamsenablegroupsCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            Currentplayerspawnpointssection.group_params.enablegroups = GroupParamsenablegroupsCB.Checked;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void GroupParamslifetimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            Currentplayerspawnpointssection.group_params.lifetime = (int)GroupParamslifetimeNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void GroupParamscounterNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            Currentplayerspawnpointssection.group_params.counter = (int)GroupParamscounterNUD.Value;
            currentproject.cfgplayerspawnpoints.isDirty = true;
        }
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _mouseLastPosition = e.Location;
            }
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
            decimal scalevalue = PlayerSpawnScale * (decimal)0.05;
            decimal mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            label156.Text = Decimal.Round((decimal)(e.X / scalevalue),4) + "," + Decimal.Round((decimal)((newsize - e.Y) / scalevalue),4);
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
            PlayerSpawnScale = trackBar4.Value;
            SetSpawnScale();
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
            PlayerSpawnScale = trackBar4.Value;
            SetSpawnScale();
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
        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {
            if (e is MouseEventArgs mouseEventArgs)
            {
                Cursor.Current = Cursors.WaitCursor;
                float scalevalue = PlayerSpawnScale * 0.05f;
                float mapsize = currentproject.MapSize;
                int newsize = (int)(mapsize * scalevalue);
                FreshPosXNUD.Value = Decimal.Round((decimal)(mouseEventArgs.X / scalevalue),4);
                FreshPosZNUD.Value = Decimal.Round((decimal)((newsize - mouseEventArgs.Y) / scalevalue),4);
                Cursor.Current = Cursors.Default;
                currentproject.cfgplayerspawnpoints.isDirty = true;
                pictureBox2.Invalidate();
            }
        }
        private void DrawAllPlayerSpawns(object sender, PaintEventArgs e)
        {
            if (PlayerSpawnAllGroupsCB.Checked == true)
            {
                foreach (playerspawnpointsGroup pointgroup in Currentplayerspawnpointssection.generator_posbubbles)
                {
                    foreach (playerspawnpointsGroupPos newpos in pointgroup.pos)
                    {
                        float scalevalue = PlayerSpawnScale * 0.05f;
                        int centerX = (int)(Math.Round(newpos.x) * (decimal)scalevalue);
                        int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(newpos.z, 0) * (decimal)scalevalue);
                        int radius = (int)(Math.Round(1f, 0) * scalevalue);
                        Point center = new Point(centerX, centerY);
                        Pen pen = new Pen(Color.Red, 4);
                        if (newpos == currentpos)
                            pen.Color = Color.LimeGreen;
                        getCircle(e.Graphics, pen, center, radius);
                    }
                }
            }
            else
            {
                if (currentplayerspawnpointsGroup != null)
                {
                    if (currentplayerspawnpointsGroup.pos != null)
                    {
                        foreach (playerspawnpointsGroupPos newpos in currentplayerspawnpointsGroup.pos)
                        {
                            float scalevalue = PlayerSpawnScale * 0.05f;
                            int centerX = (int)(Math.Round(newpos.x) * (decimal)scalevalue);
                            int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(newpos.z, 0) * (decimal)scalevalue);
                            int radius = (int)(Math.Round(1f, 0) * scalevalue);
                            Point center = new Point(centerX, centerY);
                            Pen pen = new Pen(Color.Red, 4);
                            if (newpos == currentpos)
                                pen.Color = Color.LimeGreen;
                            getCircle(e.Graphics, pen, center, radius);
                        }
                    }
                }
            }
        }
        private void PlayerSpawnAllGroupsCB_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox2.Invalidate();
        }
        private void SetSpawnScale()
        {
            float scalevalue = PlayerSpawnScale * 0.05f;
            float mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            pictureBox2.Size = new Size(newsize, newsize);
        }
        private void getCircle(Graphics drawingArea, Pen penToUse, Point center, int radius)
        {
            Rectangle rect = new Rectangle(center.X - 1, center.Y - 1, 2, 2);
            drawingArea.DrawEllipse(penToUse, rect);
            Rectangle rect2 = new Rectangle(center.X - radius, center.Y - radius, radius * 2, radius * 2);
            drawingArea.DrawEllipse(penToUse, rect2);
        }
        #endregion PlayerSpawnPoints
        #region cfggameplayconfig
        private cfggameplay cfggameplay;
        private void LoadCFGGamelplay()
        {
            Console.WriteLine("Loading CFGamePlay");
            isUserInteraction = false;
            cfggameplay = currentproject.CFGGameplayConfig.cfggameplay;
            CFGGameplayTB.Text = cfggameplay.version.ToString();
            disableBaseDamageCB.Checked = cfggameplay.GeneralData.disableBaseDamage;
            disableContainerDamageCB.Checked = cfggameplay.GeneralData.disableContainerDamage;
            disableRespawnDialogCB.Checked = cfggameplay.GeneralData.disableRespawnDialog;
            disableRespawnInUnconsciousnessCB.Checked = cfggameplay.GeneralData.disableRespawnInUnconsciousness;

            disablePersonalLightCB.Checked = cfggameplay.PlayerData.disablePersonalLight;
            sprintStaminaModifierErcNUD.Value = cfggameplay.PlayerData.StaminaData.sprintStaminaModifierErc;
            sprintStaminaModifierCroNUD.Value = cfggameplay.PlayerData.StaminaData.sprintStaminaModifierCro;
            staminaWeightLimitThresholdNUD.Value = cfggameplay.PlayerData.StaminaData.staminaWeightLimitThreshold;
            staminaMaxNUD.Value = cfggameplay.PlayerData.StaminaData.staminaMax;
            staminaKgToStaminaPercentPenaltyNUD.Value = cfggameplay.PlayerData.StaminaData.staminaKgToStaminaPercentPenalty;
            staminaMinCapNUD.Value = cfggameplay.PlayerData.StaminaData.staminaMinCap;
            sprintSwimmingStaminaModifierNUD.Value = cfggameplay.PlayerData.StaminaData.sprintSwimmingStaminaModifier;
            sprintLadderStaminaModifierNUD.Value = cfggameplay.PlayerData.StaminaData.sprintLadderStaminaModifier;
            meleeStaminaModifierNUD.Value = cfggameplay.PlayerData.StaminaData.meleeStaminaModifier;
            obstacleTraversalStaminaModifierNUD.Value = cfggameplay.PlayerData.StaminaData.obstacleTraversalStaminaModifier;
            holdBreathStaminaModifierNUD.Value = cfggameplay.PlayerData.StaminaData.holdBreathStaminaModifier;

            shockRefillSpeedConsciousNUD.Value = cfggameplay.PlayerData.ShockHandlingData.shockRefillSpeedConscious;
            shockRefillSpeedUnconsciousNUD.Value = cfggameplay.PlayerData.ShockHandlingData.shockRefillSpeedUnconscious;
            allowRefillSpeedModifierCB.Checked = cfggameplay.PlayerData.ShockHandlingData.allowRefillSpeedModifier;

            timeToStrafeJogNUD.Value = cfggameplay.PlayerData.MovementData.timeToStrafeJog;
            rotationSpeedJogNUD.Value = cfggameplay.PlayerData.MovementData.rotationSpeedJog;
            timeToSprintNUD.Value = cfggameplay.PlayerData.MovementData.timeToSprint;
            timeToStrafeSprintNUD.Value = cfggameplay.PlayerData.MovementData.timeToStrafeSprint;
            rotationSpeedSprintNUD.Value = cfggameplay.PlayerData.MovementData.rotationSpeedSprint;
            allowStaminaAffectInertiaCB.Checked = cfggameplay.PlayerData.MovementData.allowStaminaAffectInertia;

            staminaDepletionSpeedNUD.Value = cfggameplay.PlayerData.DrowningData.staminaDepletionSpeed;
            healthDepletionSpeedNUD.Value = cfggameplay.PlayerData.DrowningData.healthDepletionSpeed;
            shockDepletionSpeedNUD.Value = cfggameplay.PlayerData.DrowningData.shockDepletionSpeed;

            lightingConfigNUD.Value = cfggameplay.WorldsData.lightingConfig;
            JanMinNUD.Value = cfggameplay.WorldsData.environmentMinTemps[0];
            FebMinNUD.Value = cfggameplay.WorldsData.environmentMinTemps[1];
            MarMinNUD.Value = cfggameplay.WorldsData.environmentMinTemps[2];
            AprMinNUD.Value = cfggameplay.WorldsData.environmentMinTemps[3];
            MayMinNUD.Value = cfggameplay.WorldsData.environmentMinTemps[4];
            JunMinNUD.Value = cfggameplay.WorldsData.environmentMinTemps[5];
            JulMinNUD.Value = cfggameplay.WorldsData.environmentMinTemps[6];
            AugMinNUD.Value = cfggameplay.WorldsData.environmentMinTemps[7];
            SepMinNUD.Value = cfggameplay.WorldsData.environmentMinTemps[8];
            OctMinNUD.Value = cfggameplay.WorldsData.environmentMinTemps[9];
            NovMinNUD.Value = cfggameplay.WorldsData.environmentMinTemps[10];
            DecMinNUD.Value = cfggameplay.WorldsData.environmentMinTemps[11];

            JanMaxNUD.Value = cfggameplay.WorldsData.environmentMaxTemps[0];
            FebMaxNUD.Value = cfggameplay.WorldsData.environmentMaxTemps[1];
            MarMaxNUD.Value = cfggameplay.WorldsData.environmentMaxTemps[2];
            AprMaxNUD.Value = cfggameplay.WorldsData.environmentMaxTemps[3];
            MayMaxNUD.Value = cfggameplay.WorldsData.environmentMaxTemps[4];
            JunMaxNUD.Value = cfggameplay.WorldsData.environmentMaxTemps[5];
            JulMaxNUD.Value = cfggameplay.WorldsData.environmentMaxTemps[6];
            AugMaxNUD.Value = cfggameplay.WorldsData.environmentMaxTemps[7];
            SepMaxNUD.Value = cfggameplay.WorldsData.environmentMaxTemps[8];
            OctMaxNUD.Value = cfggameplay.WorldsData.environmentMaxTemps[9];
            NovMaxNUD.Value = cfggameplay.WorldsData.environmentMaxTemps[10];
            DecMaxNUD.Value = cfggameplay.WorldsData.environmentMaxTemps[11];

            wetnessWeightModifiers1NUD.Value = cfggameplay.WorldsData.wetnessWeightModifiers[0];
            wetnessWeightModifiers2NUD.Value = cfggameplay.WorldsData.wetnessWeightModifiers[1];
            wetnessWeightModifiers3NUD.Value = cfggameplay.WorldsData.wetnessWeightModifiers[2];
            wetnessWeightModifiers4NUD.Value = cfggameplay.WorldsData.wetnessWeightModifiers[3];
            wetnessWeightModifiers5NUD.Value = cfggameplay.WorldsData.wetnessWeightModifiers[4];


            disableIsCollidingBBoxCheckCB.Checked = cfggameplay.BaseBuildingData.HologramData.disableIsCollidingBBoxCheck;
            disableIsCollidingPlayerCheckCB.Checked = cfggameplay.BaseBuildingData.HologramData.disableIsCollidingPlayerCheck;
            disableIsClippingRoofCheckCB.Checked = cfggameplay.BaseBuildingData.HologramData.disableIsClippingRoofCheck;
            disableIsBaseViableCheckCB.Checked = cfggameplay.BaseBuildingData.HologramData.disableIsBaseViableCheck;
            disableIsCollidingGPlotCheckCB.Checked = cfggameplay.BaseBuildingData.HologramData.disableIsCollidingGPlotCheck;
            disableIsCollidingAngleCheckCB.Checked = cfggameplay.BaseBuildingData.HologramData.disableIsCollidingAngleCheck;
            disableIsPlacementPermittedCheckCB.Checked = cfggameplay.BaseBuildingData.HologramData.disableIsPlacementPermittedCheck;
            disableHeightPlacementCheckCB.Checked = cfggameplay.BaseBuildingData.HologramData.disableHeightPlacementCheck;
            disableIsUnderwaterCheckCB.Checked = cfggameplay.BaseBuildingData.HologramData.disableIsUnderwaterCheck;
            disableIsInTerrainCheckCB.Checked = cfggameplay.BaseBuildingData.HologramData.disableIsInTerrainCheck;

            disablePerformRoofCheckCB.Checked = cfggameplay.BaseBuildingData.ConstructionData.disablePerformRoofCheck;
            disableIsCollidingCheckCB.Checked = cfggameplay.BaseBuildingData.ConstructionData.disableIsCollidingCheck;
            disableDistanceCheckCB.Checked = cfggameplay.BaseBuildingData.ConstructionData.disableDistanceCheck;

            use3DMapCB.Checked = cfggameplay.UIData.use3DMap;
            hitDirectionOverrideEnabledCB.Checked = cfggameplay.UIData.HitIndicationData.hitDirectionOverrideEnabled;
            hitDirectionBehaviourCB.Checked = cfggameplay.UIData.HitIndicationData.hitDirectionBehaviour == 1 ? true : false;
            hitDirectionStyleCB.Checked = cfggameplay.UIData.HitIndicationData.hitDirectionStyle == 1 ? true : false;
            hitDirectionMaxDurationNUD.Value = (decimal)cfggameplay.UIData.HitIndicationData.hitDirectionMaxDuration;
            hitDirectionBreakPointRelativeNUD.Value = (decimal)cfggameplay.UIData.HitIndicationData.hitDirectionBreakPointRelative;
            hitDirectionScatterNUD.Value = (decimal)cfggameplay.UIData.HitIndicationData.hitDirectionScatter;
            hitIndicationPostProcessEnabledCB.Checked = cfggameplay.UIData.HitIndicationData.hitIndicationPostProcessEnabled;

            ignoreMapOwnershipCB.Checked = cfggameplay.MapData.ignoreMapOwnership;
            ignoreNavItemsOwnershipCB.Checked = cfggameplay.MapData.ignoreNavItemsOwnership;
            displayPlayerPositionCB.Checked = cfggameplay.MapData.displayPlayerPosition;
            displayNavInfoCB.Checked = cfggameplay.MapData.displayNavInfo;

            CFGGameplayDisallowedtypesLB.DisplayMember = "DisplayName";
            CFGGameplayDisallowedtypesLB.ValueMember = "Value";
            CFGGameplayDisallowedtypesLB.DataSource = cfggameplay.BaseBuildingData.HologramData.disallowedTypesInUnderground;

            isUserInteraction = true;
        }
        private void m_Color_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            ColorPickerDialog cpick = new ColorPickerDialog();
            cpick.StartPosition = FormStartPosition.CenterParent;
            string col = cfggameplay.UIData.HitIndicationData.hitDirectionIndicatorColorStr;
            cpick.Color = ColorTranslator.FromHtml(col);
            if (cpick.ShowDialog() == DialogResult.OK)
            {
                string Colourname = "0x" + cpick.Color.Name.ToLower();
                cfggameplay.UIData.HitIndicationData.hitDirectionIndicatorColorStr = Colourname;
                pb.Invalidate();
                currentproject.CFGGameplayConfig.isDirty = true;
            }
        }
        private void m_Color_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            Rectangle region;
            region = pb.ClientRectangle;
            Color colour = ColorTranslator.FromHtml(cfggameplay.UIData.HitIndicationData.hitDirectionIndicatorColorStr);
            using (Brush brush = new SolidBrush(colour))
            {
                e.Graphics.FillRectangle(brush, region);
            }
            e.Graphics.DrawRectangle(SystemPens.ControlText, region.Left, region.Top, region.Width - 1, region.Height - 1);
        }
        private void disableBaseDamageCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.GeneralData.disableBaseDamage = disableBaseDamageCB.Checked;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableContainerDamageCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.GeneralData.disableContainerDamage = disableContainerDamageCB.Checked;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableRespawnDialogCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.GeneralData.disableRespawnDialog = disableRespawnDialogCB.Checked;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableRespawnInUnconsciousnessCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.GeneralData.disableRespawnInUnconsciousness = disableRespawnInUnconsciousnessCB.Checked;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disablePersonalLightCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.disablePersonalLight = disablePersonalLightCB.Checked;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void sprintStaminaModifierErcNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.StaminaData.sprintStaminaModifierErc = Math.Round(sprintStaminaModifierErcNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void sprintStaminaModifierCroNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.StaminaData.sprintStaminaModifierCro = Math.Round(sprintStaminaModifierCroNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void staminaWeightLimitThresholdNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.StaminaData.staminaWeightLimitThreshold = Math.Round(staminaWeightLimitThresholdNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void staminaMaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.StaminaData.staminaMax = Math.Round(staminaMaxNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void staminaKgToStaminaPercentPenaltyNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.StaminaData.staminaKgToStaminaPercentPenalty = Math.Round(staminaKgToStaminaPercentPenaltyNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void staminaMinCapNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.StaminaData.staminaMinCap = Math.Round(staminaMinCapNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void shockRefillSpeedConsciousNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.ShockHandlingData.shockRefillSpeedConscious = Math.Round(shockRefillSpeedConsciousNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void shockRefillSpeedUnconsciousNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.ShockHandlingData.shockRefillSpeedUnconscious = Math.Round(shockRefillSpeedUnconsciousNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void allowRefillSpeedModifierCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.ShockHandlingData.allowRefillSpeedModifier = allowRefillSpeedModifierCB.Checked;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableIsCollidingBBoxCheckCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.BaseBuildingData.HologramData.disableIsCollidingBBoxCheck = disableIsCollidingBBoxCheckCB.Checked;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableIsCollidingPlayerCheckCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.BaseBuildingData.HologramData.disableIsCollidingPlayerCheck = disableIsCollidingPlayerCheckCB.Checked;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableIsClippingRoofCheckCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.BaseBuildingData.HologramData.disableIsClippingRoofCheck = disableIsClippingRoofCheckCB.Checked;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableIsBaseViableCheckCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.BaseBuildingData.HologramData.disableIsBaseViableCheck = disableIsBaseViableCheckCB.Checked;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableIsCollidingGPlotCheckCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.BaseBuildingData.HologramData.disableIsCollidingGPlotCheck = disableIsCollidingGPlotCheckCB.Checked;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableIsCollidingAngleCheckCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.BaseBuildingData.HologramData.disableIsCollidingAngleCheck = disableIsCollidingAngleCheckCB.Checked;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableIsPlacementPermittedCheckCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.BaseBuildingData.HologramData.disableIsPlacementPermittedCheck = disableIsPlacementPermittedCheckCB.Checked;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableHeightPlacementCheckCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.BaseBuildingData.HologramData.disableHeightPlacementCheck = disableHeightPlacementCheckCB.Checked;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableIsUnderwaterCheckCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.BaseBuildingData.HologramData.disableIsUnderwaterCheck = disableIsUnderwaterCheckCB.Checked;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableIsInTerrainCheckCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.BaseBuildingData.HologramData.disableIsInTerrainCheck = disableIsInTerrainCheckCB.Checked;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disablePerformRoofCheckCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.BaseBuildingData.ConstructionData.disablePerformRoofCheck = disablePerformRoofCheckCB.Checked;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableIsCollidingCheckCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.BaseBuildingData.ConstructionData.disableIsCollidingCheck = disableIsCollidingCheckCB.Checked;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void disableDistanceCheckCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.BaseBuildingData.ConstructionData.disableDistanceCheck = disableDistanceCheckCB.Checked;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void use3DMapCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.UIData.use3DMap = use3DMapCB.Checked;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void hitDirectionOverrideEnabledCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.UIData.HitIndicationData.hitDirectionOverrideEnabled = hitDirectionOverrideEnabledCB.Checked;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void hitDirectionBehaviourCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.UIData.HitIndicationData.hitDirectionBehaviour = hitDirectionBehaviourCB.Checked == true ? 1 : 0;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void hitDirectionStyleCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.UIData.HitIndicationData.hitDirectionStyle = hitDirectionStyleCB.Checked == true ? 1 : 0;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void hitDirectionMaxDurationNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.UIData.HitIndicationData.hitDirectionMaxDuration = Math.Round(hitDirectionMaxDurationNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void hitDirectionBreakPointRelativeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.UIData.HitIndicationData.hitDirectionBreakPointRelative = Math.Round(hitDirectionBreakPointRelativeNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void hitDirectionScatterNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.UIData.HitIndicationData.hitDirectionScatter = Math.Round(hitDirectionScatterNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void hitIndicationPostProcessEnabledCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.UIData.HitIndicationData.hitIndicationPostProcessEnabled = hitIndicationPostProcessEnabledCB.Checked;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void lightingConfigNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.WorldsData.lightingConfig = (int)lightingConfigNUD.Value;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void MinTemp_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.WorldsData.environmentMinTemps[0] = JanMinNUD.Value;
            cfggameplay.WorldsData.environmentMinTemps[1] = FebMinNUD.Value;
            cfggameplay.WorldsData.environmentMinTemps[2] = MarMinNUD.Value;
            cfggameplay.WorldsData.environmentMinTemps[3] = AprMinNUD.Value;
            cfggameplay.WorldsData.environmentMinTemps[4] = MayMinNUD.Value;
            cfggameplay.WorldsData.environmentMinTemps[5] = JunMinNUD.Value;
            cfggameplay.WorldsData.environmentMinTemps[6] = JulMinNUD.Value;
            cfggameplay.WorldsData.environmentMinTemps[7] = AugMinNUD.Value;
            cfggameplay.WorldsData.environmentMinTemps[8] = SepMinNUD.Value;
            cfggameplay.WorldsData.environmentMinTemps[9] = OctMinNUD.Value;
            cfggameplay.WorldsData.environmentMinTemps[10] = NovMinNUD.Value;
            cfggameplay.WorldsData.environmentMinTemps[11] = DecMinNUD.Value;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void MaxTemp_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.WorldsData.environmentMaxTemps[0] = JanMaxNUD.Value;
            cfggameplay.WorldsData.environmentMaxTemps[1] = FebMaxNUD.Value;
            cfggameplay.WorldsData.environmentMaxTemps[2] = MarMaxNUD.Value;
            cfggameplay.WorldsData.environmentMaxTemps[3] = AprMaxNUD.Value;
            cfggameplay.WorldsData.environmentMaxTemps[4] = MayMaxNUD.Value;
            cfggameplay.WorldsData.environmentMaxTemps[5] = JunMaxNUD.Value;
            cfggameplay.WorldsData.environmentMaxTemps[6] = JulMaxNUD.Value;
            cfggameplay.WorldsData.environmentMaxTemps[7] = AugMaxNUD.Value;
            cfggameplay.WorldsData.environmentMaxTemps[8] = SepMaxNUD.Value;
            cfggameplay.WorldsData.environmentMaxTemps[9] = OctMaxNUD.Value;
            cfggameplay.WorldsData.environmentMaxTemps[10] = NovMaxNUD.Value;
            cfggameplay.WorldsData.environmentMaxTemps[11] = DecMaxNUD.Value;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void wetnessWeightModifiers_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.WorldsData.wetnessWeightModifiers[0] = wetnessWeightModifiers1NUD.Value;
            cfggameplay.WorldsData.wetnessWeightModifiers[1] = wetnessWeightModifiers2NUD.Value;
            cfggameplay.WorldsData.wetnessWeightModifiers[2] = wetnessWeightModifiers3NUD.Value;
            cfggameplay.WorldsData.wetnessWeightModifiers[3] = wetnessWeightModifiers4NUD.Value;
            cfggameplay.WorldsData.wetnessWeightModifiers[4] = wetnessWeightModifiers5NUD.Value;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void allowStaminaAffectInertiaCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.MovementData.allowStaminaAffectInertia = allowStaminaAffectInertiaCB.Checked;
            currentproject.CFGGameplayConfig.isDirty = true;

        }
        private void staminaDepletionSpeedNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.DrowningData.staminaDepletionSpeed = Math.Round(staminaDepletionSpeedNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void healthDepletionSpeedNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.DrowningData.healthDepletionSpeed = Math.Round(healthDepletionSpeedNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void shockDepletionSpeedNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.DrowningData.shockDepletionSpeed = Math.Round(shockDepletionSpeedNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void sprintSwimmingStaminaModifierNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.StaminaData.sprintSwimmingStaminaModifier = Math.Round(sprintSwimmingStaminaModifierNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void sprintLadderStaminaModifierNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.StaminaData.sprintLadderStaminaModifier = Math.Round(sprintLadderStaminaModifierNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void meleeStaminaModifierNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.StaminaData.meleeStaminaModifier = Math.Round(meleeStaminaModifierNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void obstacleTraversalStaminaModifierNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.StaminaData.obstacleTraversalStaminaModifier = Math.Round(obstacleTraversalStaminaModifierNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void holdBreathStaminaModifierNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.StaminaData.holdBreathStaminaModifier = Math.Round(holdBreathStaminaModifierNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void timeToStrafeJogNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.MovementData.timeToStrafeJog = Math.Round(timeToStrafeJogNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void rotationSpeedJogNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.MovementData.rotationSpeedJog = Math.Round(rotationSpeedJogNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void timeToSprintNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.MovementData.timeToSprint = Math.Round(timeToSprintNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void timeToStrafeSprintNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.MovementData.timeToSprint = Math.Round(timeToSprintNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void rotationSpeedSprintNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.PlayerData.MovementData.timeToSprint = Math.Round(timeToSprintNUD.Value, 2);
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void ignoreMapOwnershipCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.MapData.ignoreMapOwnership = ignoreMapOwnershipCB.Checked;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void ignoreNavItemsOwnershipCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.MapData.ignoreNavItemsOwnership = ignoreNavItemsOwnershipCB.Checked;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void displayPlayerPositionCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.MapData.displayPlayerPosition = displayPlayerPositionCB.Checked;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void displayNavInfoCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            cfggameplay.MapData.displayNavInfo = displayNavInfoCB.Checked;
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        private void darkButton66_Click(object sender, EventArgs e)
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
                    if(!cfggameplay.BaseBuildingData.HologramData.disallowedTypesInUnderground.Contains(l))
                        cfggameplay.BaseBuildingData.HologramData.disallowedTypesInUnderground.Add(l);
                }
                currentproject.CFGGameplayConfig.isDirty = true;

            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton67_Click(object sender, EventArgs e)
        {
            cfggameplay.BaseBuildingData.HologramData.disallowedTypesInUnderground.Remove(CFGGameplayDisallowedtypesLB.GetItemText(CFGGameplayDisallowedtypesLB.SelectedItem));
            currentproject.CFGGameplayConfig.isDirty = true;
        }
        #endregion cfggameplayconfig
        #region cfgrandompresets
        public object currentRandomPreset;
        private void LoadRandomPresets()
        {
            Console.WriteLine("Loading Random presets");
            isUserInteraction = false;
            PresetItemListLB.DisplayMember = "DisplayName";
            PresetItemListLB.ValueMember = "Value";
            PresetItemListLB.DataSource = currentproject.cfgrandompresetsconfig.randompresets.Items;
            isUserInteraction = true;
        }
        private void PresetItemListLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PresetItemListLB.SelectedIndex == -1) { return; }
            isUserInteraction = false;
            currentRandomPreset = PresetItemListLB.SelectedItem as object;
            ClearInfo();
            if (currentRandomPreset is randompresetsAttachments)
            {
                randompresetsAttachments currentAttachments = currentRandomPreset as randompresetsAttachments;
                RandompresetCargoGB.Visible = false;
                RandompreseAttachmentGB.Visible = true;
                setRandompresetAttachemnt();
            }
            else if (currentRandomPreset is randompresetsCargo)
            {
                randompresetsCargo currentCargo = currentRandomPreset as randompresetsCargo;
                RandompresetCargoGB.Visible = true;
                RandompreseAttachmentGB.Visible = false;
                SetRandomPresetCargo();
            }
            isUserInteraction = true;
        }
        private void setRandompresetAttachemnt()
        {
            randompresetsAttachments currentAttachments = currentRandomPreset as randompresetsAttachments;
            RandomPresetAttachmentChanceNUD.Value = currentAttachments.chance;
            RandomPresetAttchemntNameTB.Text = currentAttachments.name;
            PresetAttachmentsItemsLB.DisplayMember = "DisplayName";
            PresetAttachmentsItemsLB.ValueMember = "Value";
            PresetAttachmentsItemsLB.DataSource = currentAttachments.item;
        }
        private void SetRandomPresetCargo()
        {
            randompresetsCargo currentcargo = currentRandomPreset as randompresetsCargo;
            RandomPresetCargoChanceNUD.Value = currentcargo.chance;
            RandomPresetNameTB.Text = currentcargo.name;
            PresetCargoItemsLB.DisplayMember = "DisplayName";
            PresetCargoItemsLB.ValueMember = "Value";
            PresetCargoItemsLB.DataSource = currentcargo.item;
        }
        private void PresetCArgoItemsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PresetCargoItemsLB.SelectedItems.Count == 0) return;
            randompresetsCargoItem currentcargoitem = PresetCargoItemsLB.SelectedItem as randompresetsCargoItem;
            isUserInteraction = false;
            RandomPresetCargoItemchanceNUD.Value = currentcargoitem.chance;
            isUserInteraction = true;
        }
        private void PresetAttachmentsItemsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PresetAttachmentsItemsLB.SelectedItems.Count == 0) return;
            randompresetsAttachmentsItem currentattachmentitem = PresetAttachmentsItemsLB.SelectedItem as randompresetsAttachmentsItem;
            isUserInteraction = false;
            RandomPresetAttachmentItemchanceNUD.Value = currentattachmentitem.chance;
            isUserInteraction = true;
        }
        private void RandomPresetCargoChanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            randompresetsCargo currentCargo = currentRandomPreset as randompresetsCargo;
            currentCargo.chance = RandomPresetCargoChanceNUD.Value;
            currentproject.cfgrandompresetsconfig.isDirty = true;
        }
        private void RandomPresetCargoItemchanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            randompresetsCargoItem currentcargoitem = PresetCargoItemsLB.SelectedItem as randompresetsCargoItem;
            currentcargoitem.chance = RandomPresetCargoItemchanceNUD.Value;
            currentproject.cfgrandompresetsconfig.isDirty = true;
        }
        private void RandomPresetAttachmentChanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            randompresetsAttachments currentAttachments = currentRandomPreset as randompresetsAttachments;
            currentAttachments.chance = RandomPresetAttachmentChanceNUD.Value;
            currentproject.cfgrandompresetsconfig.isDirty = true;
        }
        private void RandomPresetAttachmentItemchanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            randompresetsAttachmentsItem currentAttachmentitem = PresetAttachmentsItemsLB.SelectedItem as randompresetsAttachmentsItem;
            currentAttachmentitem.chance = RandomPresetAttachmentItemchanceNUD.Value;
            currentproject.cfgrandompresetsconfig.isDirty = true;
        }
        private void darkButton48_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseOnlySingleitem = true
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    randompresetsCargoItem currentcargoitem = PresetCargoItemsLB.SelectedItem as randompresetsCargoItem;
                    currentcargoitem.name = l;
                    PresetCargoItemsLB.Refresh();
                    currentproject.cfgrandompresetsconfig.isDirty = true;
                    SetRandomPresetCargo();
                }

            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton47_Click(object sender, EventArgs e)
        {
            randompresetsCargoItem newitem = new randompresetsCargoItem()
            {
                name = "New Item, Please change me...",
                chance = 1
            };
            randompresetsCargo currentcargo = currentRandomPreset as randompresetsCargo;
            currentcargo.item.Add(newitem);
            currentproject.cfgrandompresetsconfig.isDirty = true;
        }
        private void darkButton46_Click(object sender, EventArgs e)
        {
            if (PresetCargoItemsLB.SelectedItems == null) { return; }
            int index = PresetCargoItemsLB.SelectedIndex;
            randompresetsCargoItem currentcargoitem = PresetCargoItemsLB.SelectedItem as randompresetsCargoItem;
            randompresetsCargo currentcargo = currentRandomPreset as randompresetsCargo;
            currentcargo.item.Remove(currentcargoitem);
            currentproject.cfgrandompresetsconfig.isDirty = true;
            PresetCargoItemsLB.SelectedIndex = -1;
            if (index == 0 && PresetCargoItemsLB.Items.Count > 0)
                PresetCargoItemsLB.SelectedIndex = index;
            else
                PresetCargoItemsLB.SelectedIndex = index - 1;
        }
        private void darkButton49_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseMultipleofSameItem = false
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    randompresetsAttachmentsItem currentAttachemntitem = PresetAttachmentsItemsLB.SelectedItem as randompresetsAttachmentsItem;
                    currentAttachemntitem.name = l;
                    PresetAttachmentsItemsLB.Refresh();
                    currentproject.cfgrandompresetsconfig.isDirty = true;
                    setRandompresetAttachemnt();
                }

            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton42_Click(object sender, EventArgs e)
        {
            randompresetsAttachmentsItem newitem = new randompresetsAttachmentsItem()
            {
                name = "New Item, Please change me...",
                chance = 1
            };
            randompresetsAttachments currentAttachemnt = currentRandomPreset as randompresetsAttachments;
            currentAttachemnt.item.Add(newitem);
            currentproject.cfgrandompresetsconfig.isDirty = true;
        }
        private void darkButton41_Click(object sender, EventArgs e)
        {
            if (PresetAttachmentsItemsLB.SelectedItems == null) { return; }
            int index = PresetAttachmentsItemsLB.SelectedIndex;
            randompresetsAttachmentsItem currentAttachemntitem = PresetAttachmentsItemsLB.SelectedItem as randompresetsAttachmentsItem;
            randompresetsAttachments currentAttachent = currentRandomPreset as randompresetsAttachments;
            currentAttachent.item.Remove(currentAttachemntitem);
            currentproject.cfgrandompresetsconfig.isDirty = true;
            PresetAttachmentsItemsLB.SelectedIndex = -1;
            if (index == 0 && PresetAttachmentsItemsLB.Items.Count > 0)
                PresetAttachmentsItemsLB.SelectedIndex = index;
            else
                PresetAttachmentsItemsLB.SelectedIndex = index - 1;
        }
        private void darkButton43_Click(object sender, EventArgs e)
        {
            if (PresetItemListLB.SelectedItems == null) { return; }
            int index = PresetItemListLB.SelectedIndex;
            currentproject.cfgrandompresetsconfig.randompresets.Items.Remove(currentRandomPreset);
            currentproject.cfgrandompresetsconfig.isDirty = true;
            PresetItemListLB.SelectedIndex = -1;
            if (index == 0 && PresetItemListLB.Items.Count > 0)
                PresetItemListLB.SelectedIndex = index;
            else
                PresetItemListLB.SelectedIndex = index - 1;
            SetuprandomPresetsForSpawnabletypes();
        }
        private void darkButton45_Click(object sender, EventArgs e)
        {
            if (PresetItemListLB.SelectedItem == null) { return; }
            randompresetsCargo newcargo = new randompresetsCargo()
            {
                name = "NewCargoList",
                chance = 1,
                item = new BindingList<randompresetsCargoItem>()
            };
            if (currentproject.cfgrandompresetsconfig.randompresets.Items == null)
                currentproject.cfgrandompresetsconfig.randompresets.Items = new BindingList<object>();
            currentproject.cfgrandompresetsconfig.randompresets.Items.Add(newcargo);
            currentproject.cfgrandompresetsconfig.isDirty = true;
            SetuprandomPresetsForSpawnabletypes();
            PresetItemListLB.SelectedIndex = PresetItemListLB.Items.Count - 1;
        }
        private void darkButton44_Click(object sender, EventArgs e)
        {
            if (PresetItemListLB.SelectedItem == null) { return; }
            randompresetsAttachments newspawnabletypesTypeAttachments = new randompresetsAttachments()
            {
                name = "NewAttachmentList",
                chance = 1,
                item = new BindingList<randompresetsAttachmentsItem>()
               
            };
            if (currentproject.cfgrandompresetsconfig.randompresets.Items == null)
                currentproject.cfgrandompresetsconfig.randompresets.Items = new BindingList<object>();
            currentproject.cfgrandompresetsconfig.randompresets.Items.Add(newspawnabletypesTypeAttachments);
            currentproject.cfgrandompresetsconfig.isDirty = true;
            SetuprandomPresetsForSpawnabletypes();
            PresetItemListLB.SelectedIndex = PresetItemListLB.Items.Count - 1;
        }
        private void RandomPresetNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            randompresetsCargo currentCargo = currentRandomPreset as randompresetsCargo;
            currentCargo.name = RandomPresetNameTB.Text;
            PresetItemListLB.Refresh();
            currentproject.cfgrandompresetsconfig.isDirty = true;
            SetuprandomPresetsForSpawnabletypes();
        }
        private void RandomPresetAttchemntNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            randompresetsAttachments currentAttachment = currentRandomPreset as randompresetsAttachments;
            currentAttachment.name = RandomPresetAttchemntNameTB.Text;
            PresetItemListLB.Refresh();
            currentproject.cfgrandompresetsconfig.isDirty = true;
            SetuprandomPresetsForSpawnabletypes();
        }
        #endregion cfgrandompresets
        #region CFGAreaEffects
        public cfgEffectArea cfgEffectArea;
        public Position currentsafeposition;
        public Areas CurrentToxicArea;
        private void LoadContaminatoedArea()
        {
            Console.WriteLine("Loading ContaminatedAreas");
            isUserInteraction = false;
            cfgEffectArea = currentproject.cfgEffectAreaConfig.cfgEffectArea;
            AreasLB.DisplayMember = "DisplayName";
            AreasLB.ValueMember = "Value";
            AreasLB.DataSource = cfgEffectArea.Areas;

            SafePositionsLB.DisplayMember = "DisplayName";
            SafePositionsLB.ValueMember = "Value";
            SafePositionsLB.DataSource = cfgEffectArea._positions;

            isUserInteraction = true;
        }
        private void SafePositionsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SafePositionsLB.SelectedItems.Count < 1) return;
            currentsafeposition = SafePositionsLB.SelectedItem as Position;
            isUserInteraction = false;
            SafePositionXNUD.Value = (decimal)currentsafeposition.X;
            SafePositionZNUD.Value = (decimal)currentsafeposition.Z;

            isUserInteraction = true;
        }
        private void AreasLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AreasLB.SelectedItems.Count < 1) return;
            CurrentToxicArea = AreasLB.SelectedItem as Areas;
            isUserInteraction = false;

            AreaNameTB.Text = CurrentToxicArea.AreaName;
            TypeTB.Text = CurrentToxicArea.Type;
            TriggerTypeTB.Text = CurrentToxicArea.TriggerType;
            PosXNUD.Value = (decimal)CurrentToxicArea.Data.Pos[0];
            posYNUD.Value = (decimal)CurrentToxicArea.Data.Pos[1];
            posZNUD.Value = (decimal)CurrentToxicArea.Data.Pos[2];
            RadiusNUD.Value = (decimal)CurrentToxicArea.Data.Radius;
            PosHeightNUD.Value = (decimal)CurrentToxicArea.Data.PosHeight;
            NegHeightNUD.Value = (decimal)CurrentToxicArea.Data.NegHeight;
            InnerRingCountNUD.Value = CurrentToxicArea.Data.InnerRingCount;
            InnerPartDistNUD.Value = CurrentToxicArea.Data.InnerPartDist;
            OuterRingToggleCB.Checked = CurrentToxicArea.Data.OuterRingToggle == 1 ? true : false;
            OuterPartDistNUD.Value = CurrentToxicArea.Data.OuterPartDist;
            OuterOffsetNUD.Value = CurrentToxicArea.Data.OuterOffset;
            VerticalLayersNUD.Value = CurrentToxicArea.Data.VerticalLayers;
            VerticalOffsetNUD.Value = CurrentToxicArea.Data.VerticalOffset;
            ParticleNameTB.Text = CurrentToxicArea.Data.ParticleName;
            AroundPartNameTB.Text = CurrentToxicArea.PlayerData.AroundPartName;
            TinyPartNameTB.Text = CurrentToxicArea.PlayerData.TinyPartName;
            PPERequesterTypeTB.Text = CurrentToxicArea.PlayerData.PPERequesterType;

            isUserInteraction = true;
        }
        private void AreaNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentToxicArea.AreaName = AreaNameTB.Text;
            currentproject.cfgEffectAreaConfig.isDirty = true;
        }
        private void TypeTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentToxicArea.Type = TypeTB.Text;
            currentproject.cfgEffectAreaConfig.isDirty = true;
        }
        private void TriggerTypeTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentToxicArea.TriggerType = TriggerTypeTB.Text;
            currentproject.cfgEffectAreaConfig.isDirty = true;
        }
        private void PosNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentToxicArea.Data.Pos = new float[] { (float)PosXNUD.Value, (float)posYNUD.Value, (float)posZNUD.Value };
            currentproject.cfgEffectAreaConfig.isDirty = true;
        }
        private void AreaNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            NumericUpDown nud = sender as NumericUpDown;
            CurrentToxicArea.Data.SetIntValue(nud.Name.Substring(0, nud.Name.Length - 3), (int)nud.Value);
            currentproject.cfgEffectAreaConfig.isDirty = true;
        }
        private void OuterRingToggleCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentToxicArea.Data.OuterRingToggle = OuterRingToggleCB.Checked == true ? 1 : 0;
            currentproject.cfgEffectAreaConfig.isDirty = true;
        }
        private void ParticleNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentToxicArea.Data.ParticleName = ParticleNameTB.Text;
            currentproject.cfgEffectAreaConfig.isDirty = true;
        }
        private void AroundPartNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentToxicArea.PlayerData.AroundPartName = AroundPartNameTB.Text;
            currentproject.cfgEffectAreaConfig.isDirty = true;
        }
        private void TinyPartNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentToxicArea.PlayerData.TinyPartName = TinyPartNameTB.Text;
            currentproject.cfgEffectAreaConfig.isDirty = true;
        }
        private void PPERequesterTypeTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            CurrentToxicArea.PlayerData.PPERequesterType = PPERequesterTypeTB.Text;
            currentproject.cfgEffectAreaConfig.isDirty = true;
        }
        private void SafePositionNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            currentsafeposition.X = (int)SafePositionXNUD.Value;
            currentsafeposition.Z = (int)SafePositionZNUD.Value;
            currentsafeposition.Name = currentsafeposition.X.ToString() + "," + currentsafeposition.Z.ToString();
            SafePositionsLB.Invalidate();
            currentproject.cfgEffectAreaConfig.isDirty = true;
        }
        private void darkButton52_Click(object sender, EventArgs e)
        {
            Data newdata = new Data()
            {
                Pos = new float[] { 0, 0, 0 },
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
            cfgEffectArea.Areas.Add(new Areas()
            {
                AreaName = "New-Toxic-Area",
                Type = "ContaminatedArea_Static",
                TriggerType = "ContaminatedTrigger",
                Data = newdata,
                PlayerData = newplayerData
            }
            );
            currentproject.cfgEffectAreaConfig.isDirty = true;
            AreasLB.SelectedIndex = -1;
            AreasLB.SelectedIndex = AreasLB.Items.Count - 1;
        }
        private void darkButton53_Click(object sender, EventArgs e)
        {
            if (AreasLB.SelectedItems.Count < 1) return;
            int index = AreasLB.SelectedIndex;
            cfgEffectArea.Areas.Remove(CurrentToxicArea);
            currentproject.cfgEffectAreaConfig.isDirty = true;
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
        private void darkButton50_Click(object sender, EventArgs e)
        {
            cfgEffectArea._positions.Add(new Position()
            {
                Name = "0,0",
                X = 0,
                Z = 0
            }
);
            currentproject.cfgEffectAreaConfig.isDirty = true;
            SafePositionsLB.SelectedIndex = -1;
            SafePositionsLB.SelectedIndex = SafePositionsLB.Items.Count - 1;
        }
        private void darkButton51_Click(object sender, EventArgs e)
        {
            if (SafePositionsLB.SelectedItems.Count < 1) return;
            int index = SafePositionsLB.SelectedIndex;
            cfgEffectArea._positions.Remove(currentsafeposition);
            currentproject.cfgEffectAreaConfig.isDirty = true;
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
        #endregion CFGAreaEffects
        #region gloabls
        private variables variables;
        public void LoadGlobals()
        {
            Console.WriteLine("Loading Globals");
            isUserInteraction = false;
            variablesvarvalueNUD.Maximum = int.MaxValue;
            variables = currentproject.gloabsconfig.variables;
            VariablesLB.DisplayMember = "DisplayName";
            VariablesLB.ValueMember = "Value";
            VariablesLB.DataSource = variables.var;
            isUserInteraction = true;
        }
        private void listBox7_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (VariablesLB.SelectedItems.Count < 1) return;
            variablesVar m_var = VariablesLB.SelectedItem as variablesVar;
            isUserInteraction = false;
            variablesvarnameTB.Text = m_var.name;
            variablesvartypeNUD.Value = m_var.type;
            switch (m_var.type)
            {
                case 0:
                    variablesvarvalueNUD.DecimalPlaces = 0;
                    variablesvarvalueNUD.Increment = 1;
                    variablesvarvalueNUD.Maximum = 999999999999;
                    variablesvarvalueNUD.Minimum = 0;
                    break;
                case 1:
                    variablesvarvalueNUD.DecimalPlaces = 2;
                    variablesvarvalueNUD.Increment = (decimal)0.05;
                    variablesvarvalueNUD.Maximum = 1;
                    variablesvarvalueNUD.Minimum = 0;
                    break;

            }
            variablesvarvalueNUD.Value = m_var.value;
            isUserInteraction = true;
        }
        private void variablesvartypeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            variablesVar m_var = VariablesLB.SelectedItem as variablesVar;
            m_var.type = (byte)variablesvartypeNUD.Value;
            currentproject.gloabsconfig.isDirty = true;
        }
        private void variablesvarvalueNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) { return; }
            variablesVar m_var = VariablesLB.SelectedItem as variablesVar;
            switch (m_var.type)
            {
                case 0:
                    m_var.value = (int)variablesvarvalueNUD.Value;
                    break;
                case 1:
                    m_var.value = variablesvarvalueNUD.Value;
                    break;

            }
            currentproject.gloabsconfig.isDirty = true;
        }
        #endregion globals
        #region weather
        public weather weather;
        public void Loadweather()
        {
            Console.WriteLine("Loading Weather");
            isUserInteraction = false;
            weather = currentproject.weatherconfig.weather;

            weatherenabledCB.Checked = weather.enable == 1 ? true : false;
            weatherrestartCB.Checked = weather.reset == 1 ? true : false;
            //Overcast
            OCactualNUD.Value = weather.overcast.current.actual;
            OCtimeNUD.Value = weather.overcast.current.time;
            OCdurationNUD.Value = weather.overcast.current.duration;
            OLminNUD.Value = weather.overcast.limits.min;
            OLmaxNUD.Value = weather.overcast.limits.max;
            OTLminNUD.Value = weather.overcast.timelimits.min;
            OTLmaxNUD.Value = weather.overcast.timelimits.max;
            OCLminNUD.Value = weather.overcast.changelimits.min;
            OCLmaxNUD.Value = weather.overcast.changelimits.max;
            //fog
            FCactualNUD.Value = weather.fog.current.actual;
            FCtimeNUD.Value = weather.fog.current.time;
            FCdurationNUD.Value = weather.fog.current.duration;
            FLminNUD.Value = weather.fog.limits.min;
            FLmaxNUD.Value = weather.fog.limits.max;
            FTLminNUD.Value = weather.fog.timelimits.min;
            FTLmaxNUD.Value = weather.fog.timelimits.max;
            FCLminNUD.Value = weather.fog.changelimits.min;
            FCLmaxNUD.Value = weather.fog.changelimits.max;
            //rain
            RCactualNUD.Value = weather.rain.current.actual;
            RCtimeNUD.Value = weather.rain.current.time;
            RCdurationNUD.Value = weather.rain.current.duration;
            RLminNUD.Value = weather.rain.limits.min;
            RLmaxNUD.Value = weather.rain.limits.max;
            RTLminNUD.Value = weather.rain.timelimits.min;
            RTLmaxNUD.Value = weather.rain.timelimits.max;
            RCLminNUD.Value = weather.rain.changelimits.min;
            RCLmaxNUD.Value = weather.rain.changelimits.max;
            RTmaxNUD.Value = weather.rain.thresholds.max;
            RTminNUD.Value = weather.rain.thresholds.min;
            RTendNUD.Value = weather.rain.thresholds.end;
            //wind
            WMaxSpeedNUD.Value = weather.wind.maxspeed;
            WPminNUD.Value = weather.wind.@params.min;
            WPmaxNUD.Value = weather.wind.@params.max;
            WPfrequencyNUD.Value = weather.wind.@params.frequency;
            //storm
            SdensityNUD.Value = weather.storm.density;
            SthresholdNUD.Value = weather.storm.threshold;
            StimeoutNUD.Value = weather.storm.timeout;

            isUserInteraction = true;
        }
        private void weatherenabledCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.enable = weatherenabledCB.Checked == true ? 1 : 0;
            currentproject.weatherconfig.isDirty = true;
        }
        private void weatherrestartCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.reset = weatherrestartCB.Checked == true ? 1 : 0;
            currentproject.weatherconfig.isDirty = true;
        }
        private void RCactualNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.rain.current.actual = RCactualNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void RCtimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.rain.current.time = (int)RCtimeNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void RCdurationNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.rain.current.duration = (int)RCdurationNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void RLminNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.rain.limits.min = RLminNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void RLmaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.rain.limits.min = RLmaxNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void RTLminNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.rain.timelimits.min = (int)RTLminNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void RTLmaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.rain.timelimits.max = (int)RTLmaxNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void RCLminNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.rain.changelimits.min = RCLminNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void RCLmaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.rain.changelimits.max = RCLmaxNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void RTminNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.rain.thresholds.min = RTminNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void RTmaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.rain.thresholds.max = RTmaxNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void RTendNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.rain.thresholds.end = (int)RTendNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void OCactualNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.overcast.current.actual = OCactualNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void OCtimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.overcast.current.time = (int)OCtimeNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void OCdurationNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.overcast.current.duration = (int)OCdurationNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void OLminNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.overcast.limits.min = OLminNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void OLmaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.overcast.limits.max = OLmaxNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void OTLminNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.overcast.timelimits.min = (int)OTLminNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void OTLmaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.overcast.timelimits.max = (int)OTLmaxNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void OCLminNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.overcast.changelimits.min = OCLminNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void OCLmaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.overcast.changelimits.max = OCLmaxNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void WMaxSpeedNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.wind.maxspeed = (int)WMaxSpeedNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void WPminNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.wind.@params.min = WPminNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void WPmaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.wind.@params.max = WPmaxNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void WPfrequencyNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.wind.@params.frequency = (int)WPfrequencyNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void FCactualNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.fog.current.actual = FCactualNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void FCtimeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.fog.current.time = (int)FCtimeNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void FCdurationNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.fog.current.duration = (int)FCdurationNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void FLminNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.fog.limits.min = FLminNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void FLmaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.fog.limits.max = FLmaxNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void FTLminNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.fog.timelimits.min = (int)FTLminNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void FTLmaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.fog.timelimits.max = (int)FTLmaxNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void FCLminNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.fog.changelimits.min = FCLminNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void FCLmaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.fog.changelimits.max = FCLmaxNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void SdensityNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.storm.density = SdensityNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void SthresholdNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.storm.threshold = SthresholdNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        private void StimeoutNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            weather.storm.timeout = (int)StimeoutNUD.Value;
            currentproject.weatherconfig.isDirty = true;
        }
        #endregion weather
        #region ignorelist
        public ignore ignore;
        public ignoreType currentignoretype;
        private void Loadignorelist()
        {
            Console.WriteLine("Loading cfgIgnorelist");
            isUserInteraction = false;
            ignore = currentproject.cfgignorelist.ignore;
            populateignoretreeview();
            isUserInteraction = true;
        }
        void populateignoretreeview()
        {
            Console.WriteLine("populating ignore treeView");
            IgnoreTreeView.Nodes.Clear();
            TreeNode root = new TreeNode(Path.GetFileName(filename))
            {
                Tag = "Parent"
            };
            //Set Vanilla Treenode types
            TreeNode Ignorelist = new TreeNode("CFG Ignore list")
            {
                Tag = "CFGIgnoreList"
            };
            foreach (ignoreType ignoreType in ignore.type)
            {
                TreeNode typenode = new TreeNode(ignoreType.name)
                {
                    Tag = ignoreType
                };
                Ignorelist.Nodes.Add(typenode);
            }
            Console.WriteLine("All Types files Populated......");
            //treeView1.Nodes.Add(root);
            root.Nodes.Add(Ignorelist);
            IgnoreTreeView.Nodes.Add(root);
        }
        private void IgnoreTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode usingtreenode = e.Node;
            if (ModifierKeys.HasFlag(Keys.Control) || ModifierKeys.HasFlag(Keys.Shift))
            {
                return;
            }
            if (usingtreenode.Tag != null && usingtreenode.Tag is ignoreType)
            {
                currentignoretype = usingtreenode.Tag as ignoreType;
                if (e.Button == MouseButtons.Right)
                {
                    addClassnameFromTypesToolStripMenuItem.Visible = false;
                    addClassnameFromStringToolStripMenuItem.Visible = false;
                    removeClassnameToolStripMenuItem.Visible = true;
                    CFGIgnoreContextMenu.Show(Cursor.Position);
                }
            }
            else if (usingtreenode.Tag != null && usingtreenode.Tag is string)
            {
                currentignoretype = null;
                if (usingtreenode.Tag.ToString() == "Parent")
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        removeClassnameToolStripMenuItem.Visible = false;
                        addClassnameFromTypesToolStripMenuItem.Visible = true;
                        addClassnameFromStringToolStripMenuItem.Visible = true;
                        CFGIgnoreContextMenu.Show(Cursor.Position);
                    }
                }
            }
            IgnoreTreeView.SelectedNode = usingtreenode;
        }
        private void removeClassnameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (TreeNode tn in IgnoreTreeView.SelectedNodes)
            {
                ignoreType ignotrtype = tn.Tag as ignoreType;
                ignore.type.Remove(ignotrtype);
            }
            currentproject.cfgignorelist.isDirty = true;
            var savedExpansionState = IgnoreTreeView.Nodes.GetExpansionState();
            IgnoreTreeView.BeginUpdate();
            populateignoretreeview();
            IgnoreTreeView.Nodes.SetExpansionState(savedExpansionState);
            IgnoreTreeView.EndUpdate();
        }
        private void addClassnameFromTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseMultipleofSameItem = false,
                LowerCase = true
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    ignoreType newignoretype = new ignoreType()
                    {
                        name = l
                    };
                    if(!ignore.type.Any(x => x.name == newignoretype.name))
                    {
                        ignore.type.Add(newignoretype);
                    }
                    else
                    {
                        MessageBox.Show(newignoretype.name + " Allready exists.....");
                    }
                }
                currentproject.cfgignorelist.isDirty = true;
                var savedExpansionState = IgnoreTreeView.Nodes.GetExpansionState();
                IgnoreTreeView.BeginUpdate();
                populateignoretreeview();
                IgnoreTreeView.Nodes.SetExpansionState(savedExpansionState);
                IgnoreTreeView.EndUpdate();
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void addClassnameFromStringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddItemfromString form = new AddItemfromString();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    ignoreType newignoretype = new ignoreType()
                    {
                        name = l
                    };
                    if (!ignore.type.Any(x => x.name == newignoretype.name))
                    {
                        ignore.type.Add(newignoretype);
                    }
                    else
                    {
                        MessageBox.Show(newignoretype.name + " Allready exists.....");
                    }
                }
                currentproject.cfgignorelist.isDirty = true;
                var savedExpansionState = IgnoreTreeView.Nodes.GetExpansionState();
                IgnoreTreeView.BeginUpdate();
                populateignoretreeview();
                IgnoreTreeView.Nodes.SetExpansionState(savedExpansionState);
                IgnoreTreeView.EndUpdate();
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        #endregion ignorelist
        #region mapgroupproto
        public prototype prototype;
        public prototypeGroup currentmapgroupprotoGroup;
        public prototypeGroupContainer currentmapgroupprotogroupcontainer;
        private void LoadMapgrouProto()
        {
            Console.WriteLine("Loading MapgroupProto");
            isUserInteraction = false;

            prototype = currentproject.mapgroupproto.prototypeGroup;

            MapGroupProtoUsageCB.DataSource = currentproject.limitfefinitions.lists.usageflags;

            MapGroupProtoGroupContainerCategroyCB.DataSource = new BindingList<listsCategory>(currentproject.limitfefinitions.lists.categories);
            MapGroupProtoGroupContainerTagCB.DataSource = currentproject.limitfefinitions.lists.tags;

            MapGroupProtoGroupsLB.DisplayMember = "DisplayName";
            MapGroupProtoGroupsLB.ValueMember = "Value";
            MapGroupProtoGroupsLB.DataSource = currentproject.mapgroupproto.prototypeGroup.group;

            isUserInteraction = true;
        }
        public void MapGroupProtoSetTiers()
        {
            List<CheckBox> checkboxes = flowLayoutPanel2.Controls.OfType<CheckBox>().ToList();
            foreach (CheckBox cb in checkboxes)
            {
                cb.Visible = false;
            }

            int index = currentproject.limitfefinitions.lists.valueflags.Count;
            for (int i = 0; i < currentproject.limitfefinitions.lists.valueflags.Count; i++)
            {
                listsValue value = currentproject.limitfefinitions.lists.valueflags[i];
                CheckBox cb = checkboxes[i];
                cb.Tag = value.name;
                cb.Checked = false;
                cb.Visible = true;
                cb.Text = value.name;
                index--;
            }

            checkboxes = flowLayoutPanel3.Controls.OfType<CheckBox>().ToList();
            foreach (CheckBox cb in checkboxes)
            {
                cb.Visible = false;
            }

            index = 0;
            foreach (user_listsUser1 user in currentproject.limitfefinitionsuser.user_lists.valueflags)
            {
                CheckBox cb = checkboxes[index];
                cb.Tag = user.name;
                cb.Visible = true;
                cb.Checked = false;
                cb.Text = user.name;
                index++;
            }
        }
        public void MapgroupProtoPopulateTiers()
        {
            MapGroupProtoSetTiers();
            if (currentmapgroupprotoGroup.value != null)
            {
                for (int i = 0; i < currentmapgroupprotoGroup.value.Count; i++)
                {
                    if (currentmapgroupprotoGroup.value[i].user != null && currentmapgroupprotoGroup.value[i].user.Count() > 0 && currentmapgroupprotoGroup.value[i].name == null)
                    {
                        tabControl24.SelectedIndex = 1;
                        try
                        {
                            flowLayoutPanel3.Controls.OfType<CheckBox>().First(x => x.Tag.ToString() == currentmapgroupprotoGroup.value[i].user).Checked = true;
                        }
                        catch
                        {
                            currentmapgroupprotoGroup.value.RemoveAt(i);
                            i--;
                        }
                    }
                    else
                    {
                        tabControl24.SelectedIndex = 0;

                        try
                        {
                            flowLayoutPanel2.Controls.OfType<CheckBox>().First(x => x.Tag.ToString() == currentmapgroupprotoGroup.value[i].name).Checked = true;
                        }
                        catch
                        {
                            currentmapgroupprotoGroup.value.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
        }
        private void MapgroupProtopopulateUsage()
        {
            MapGroupProtoGroupUsageLB.DisplayMember = "DisplayName";
            MapGroupProtoGroupUsageLB.ValueMember = "Value";
            MapGroupProtoGroupUsageLB.DataSource = currentmapgroupprotoGroup.usage;
        }
        private void MapGroupProtoGroupsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MapGroupProtoGroupsLB.SelectedItem as prototypeGroup == currentmapgroupprotoGroup) { return; }
            if (MapGroupProtoGroupsLB.SelectedIndex == -1) { return; }
            currentmapgroupprotoGroup = MapGroupProtoGroupsLB.SelectedItem as prototypeGroup;
            isUserInteraction = false;

            MapgroupprotoGroupNameTB.Text = currentmapgroupprotoGroup.name;
            MapGroupProtoGroupUseLootMaxNUD.Visible = MapGroupprotoGroupUseLootmaxCB.Checked = currentmapgroupprotoGroup.lootmaxSpecified;
            MapGroupProtoGroupUseLootMaxNUD.Value = currentmapgroupprotoGroup.lootmax;

            MapgroupProtoPopulateTiers();
            MapgroupProtopopulateUsage();
            MapgroupProtopopulatecontainer();

            isUserInteraction = true;
        }
        private void MapgroupprotoGroupNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            currentmapgroupprotoGroup.name = MapgroupprotoGroupNameTB.Text;
            currentproject.mapgroupproto.isDirty = true;
        }
        private void MapGroupprotoGroupUseLootmaxCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            MapGroupProtoGroupUseLootMaxNUD.Visible = currentmapgroupprotoGroup.lootmaxSpecified = MapGroupprotoGroupUseLootmaxCB.Checked;
            currentproject.mapgroupproto.isDirty = true;
        }
        private void MapGroupProtoGroupUseLootMaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            currentmapgroupprotoGroup.lootmax = (int)MapgroupProtoGroupcontainerUseLootMaxNUD.Value;
            currentproject.mapgroupproto.isDirty = true;
        }
        private void mapgroupprotoTierCheckBoxchanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            CheckBox cb = sender as CheckBox;
            string tier = cb.Tag.ToString();
            if (cb.Checked)
                currentmapgroupprotoGroup.AddTier(tier);
            else
                currentmapgroupprotoGroup.removetier(tier);
            currentproject.mapgroupproto.isDirty = true;
            isUserInteraction = false;
            MapgroupProtoPopulateTiers();
            isUserInteraction = true;
        }
        private void MapgroupProtoUserdefiniedTiersChanged(object sender, EventArgs e)
        {
            if (isUserInteraction)
            {
                CheckBox cb = sender as CheckBox;
                string tier = cb.Tag.ToString();
                if (cb.Checked)
                {
                    if (currentmapgroupprotoGroup.value != null)
                    {
                        currentmapgroupprotoGroup.removetiers();
                    }
                    currentmapgroupprotoGroup.AdduserTier(tier);
                }
                else
                    currentmapgroupprotoGroup.removeusertier(tier);
                currentproject.mapgroupproto.isDirty = true;
                isUserInteraction = false;
                MapgroupProtoPopulateTiers();
                isUserInteraction = true;
            }
        }
        private void darkButton63_Click(object sender, EventArgs e)
        {
            currentmapgroupprotoGroup.removetiers();
            currentproject.mapgroupproto.isDirty = true;
            isUserInteraction = false;
            MapgroupProtoPopulateTiers();
            isUserInteraction = true;
        }
        private void darkButton58_Click_1(object sender, EventArgs e)
        {
            prototypeGroupUsage u = MapGroupProtoGroupUsageLB.SelectedItem as prototypeGroupUsage;
            currentmapgroupprotoGroup.removeusage(u);
            currentproject.mapgroupproto.isDirty = true;
        }
        private void darkButton56_Click(object sender, EventArgs e)
        {
            listsUsage u = MapGroupProtoUsageCB.SelectedItem as listsUsage;
            currentmapgroupprotoGroup.AddnewUsage(u);
            currentproject.mapgroupproto.isDirty = true;
            isUserInteraction = false;
            MapgroupProtopopulateUsage();
            isUserInteraction = true;
        }
        private void MapgroupProtoGroupPopulatecategory()
        {
            MapGroupProtoGroupContainerCategroyLB.DisplayMember = "DisplayName";
            MapGroupProtoGroupContainerCategroyLB.ValueMember = "Value";
            MapGroupProtoGroupContainerCategroyLB.DataSource = currentmapgroupprotogroupcontainer.category;
        }
        private void MapgroupProtoGroupPopulateTags()
        {
            MapGroupprotoGroupContainerTagLB.DisplayMember = "DisplayName";
            MapGroupprotoGroupContainerTagLB.ValueMember = "Value";
            MapGroupprotoGroupContainerTagLB.DataSource = currentmapgroupprotogroupcontainer.tag;
        }
        private void MapgroupProtopopulatecontainer()
        {
            MapGroupProtoGroupContainersLB.DisplayMember = "DisplayName";
            MapGroupProtoGroupContainersLB.ValueMember = "Value";
            MapGroupProtoGroupContainersLB.DataSource = currentmapgroupprotoGroup.container;
        }
        private void MapGroupProtoGroupContainersLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MapGroupProtoGroupContainersLB.SelectedItem as prototypeGroupContainer == currentmapgroupprotogroupcontainer) { return; }
            if (MapGroupProtoGroupContainersLB.SelectedIndex == -1) { return; }
            currentmapgroupprotogroupcontainer = MapGroupProtoGroupContainersLB.SelectedItem as prototypeGroupContainer;
            isUserInteraction = false;

            MapgroupProtoGroupcontainerNameTB.Text = currentmapgroupprotogroupcontainer.name;
            MapgroupProtoGroupcontainerUseLootMaxNUD.Visible = MapgroupProtoGroupcontainerUseLootMaxCB.Checked = currentmapgroupprotogroupcontainer.lootmaxSpecified;
            MapgroupProtoGroupcontainerUseLootMaxNUD.Value = currentmapgroupprotogroupcontainer.lootmax;

            MapgroupProtoGroupPopulatecategory();
            MapgroupProtoGroupPopulateTags();

            isUserInteraction = true;
        }
        private void MapgroupProtoGroupcontainerNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            currentmapgroupprotogroupcontainer.name = MapgroupProtoGroupcontainerNameTB.Text;
            currentproject.mapgroupproto.isDirty = true;
        }
        private void MapgroupProtoGroupcontainerUseLootMaxCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            MapgroupProtoGroupcontainerUseLootMaxNUD.Visible = currentmapgroupprotogroupcontainer.lootmaxSpecified = MapgroupProtoGroupcontainerUseLootMaxCB.Checked;
            currentproject.mapgroupproto.isDirty = true;
        }
        private void darkButton61_Click_1(object sender, EventArgs e)
        {
            listsCategory u = MapGroupProtoGroupContainerCategroyCB.SelectedItem as listsCategory;
            currentmapgroupprotogroupcontainer.AddnewCategory(u);
            currentproject.mapgroupproto.isDirty = true;
            isUserInteraction = false;
            MapgroupProtoGroupPopulatecategory();
            isUserInteraction = true;
        }
        private void darkButton62_Click_1(object sender, EventArgs e)
        {
            prototypeGroupContainerCategory c = MapGroupProtoGroupContainerCategroyLB.SelectedItem as prototypeGroupContainerCategory;
            currentmapgroupprotogroupcontainer.removecategory(c);
            currentproject.mapgroupproto.isDirty = true;
        }
        private void darkButton57_Click(object sender, EventArgs e)
        {
            listsTag t = MapGroupProtoGroupContainerTagCB.SelectedItem as listsTag;
            currentmapgroupprotogroupcontainer.Addnewtag(t);
            currentproject.mapgroupproto.isDirty = true;
            isUserInteraction = false;
            MapgroupProtoGroupPopulateTags();
            isUserInteraction = true;
        }
        private void darkButton59_Click_1(object sender, EventArgs e)
        {
            prototypeGroupContainerTag t = MapGroupprotoGroupContainerTagLB.SelectedItem as prototypeGroupContainerTag;
            currentmapgroupprotogroupcontainer.removetag(t);
            currentproject.mapgroupproto.isDirty = true;
        }
        #endregion
        #region territories
        public decimal MissionMapscale = 1;
        public territoriesConfig currentterritoriesConfig;
        public territorytypeTerritory currentterritorytypeTerritory;
        public territorytypeTerritoryZone currentterritorytypeTerritoryZone;

        private void LoadTerritories()
        {
            Console.WriteLine("Loading Territoires");
            isUserInteraction = false;

            if (currentproject.territoriesList.Count > 0)
                populateterritorytreeview();


            pictureBox6.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Map Size is 15360 x 15360, 0,0 bottom left, middle 7680 x 7680
            pictureBox6.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox6.Paint += new PaintEventHandler(DrawTerritories);

            trackBar6.Value = 1;
            SetsMissionScale();
            panel5.AutoScrollPosition = new Point(0, 0);

            isUserInteraction = true;
        }
        private Point _mouseLastPosition;
        private Point _newscrollPosition;
        private Rectangle doubleClickRectangle = new Rectangle();
        private Timer doubleClickTimer = new Timer();
        private bool isFirstClick = true;
        private bool isDoubleClick = false;
        private int milliseconds = 0;
        private MouseEventArgs mouseeventargs;
        private void pictureBox6_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
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
                    if (currentterritorytypeTerritoryZone == null) return;
                    //if (e is MouseEventArgs mouseEventArgs)
                    //{
                        Cursor.Current = Cursors.WaitCursor;
                        decimal scalevalue = MissionMapscale * (decimal)0.05;
                        decimal mapsize = currentproject.MapSize;
                        int newsize = (int)(mapsize * scalevalue);
                        currentterritorytypeTerritoryZone.x = Decimal.Round((decimal)(mouseeventargs.X / scalevalue), 4);
                        currentterritorytypeTerritoryZone.z = Decimal.Round((decimal)((newsize - mouseeventargs.Y) / scalevalue), 4);
                        Cursor.Current = Cursors.Default;
                        currentterritoriesConfig.isDirty = true;
                        pictureBox6.Invalidate();
                    //}
                }
                else
                {
                    //Console.WriteLine("Perform single click action");
                    if (currentterritorytypeTerritory == null) return;
                    if (currentterritorytypeTerritoryZone == null) return;
                    //if (e is MouseEventArgs mouseEventArgs)
                    //{
                        decimal scalevalue = MissionMapscale * (decimal)0.05;
                        decimal mapsize = currentproject.MapSize;
                        int newsize = (int)(mapsize * scalevalue);
                        PointF pC = new PointF((float)Decimal.Round((decimal)(mouseeventargs.X / scalevalue), 4), (float)Decimal.Round((decimal)((newsize - mouseeventargs.Y) / scalevalue), 4));
                        foreach (territorytypeTerritoryZone tz in currentterritorytypeTerritory.zone)
                        {
                            PointF pP = new PointF((float)tz.x, (float)tz.z);
                            if (IsWithinCircle(pC, pP, (float)tz.r))
                            {
                                TerritoriesZonesLB.SelectedItem = tz;
                                TerritoriesZonesLB.Refresh();
                                continue;
                            }
                        }
                    //}
                }

                // Allow the MouseDown event handler to process clicks again.
                isFirstClick = true;
                isDoubleClick = false;
                milliseconds = 0;
            }
        }
        private void pictureBox6_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point changePoint = new Point(e.Location.X - _mouseLastPosition.X, e.Location.Y - _mouseLastPosition.Y);
                _newscrollPosition = new Point(-panel5.AutoScrollPosition.X - changePoint.X, -panel5.AutoScrollPosition.Y - changePoint.Y);
                if (_newscrollPosition.X <= 0)
                    _newscrollPosition.X = 0;
                if (_newscrollPosition.Y <= 0)
                    _newscrollPosition.Y = 0;
                panel5.AutoScrollPosition = _newscrollPosition;
                pictureBox6.Invalidate();
             }
            decimal scalevalue = MissionMapscale * (decimal)0.05;
            decimal mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            label155.Text = Decimal.Round((decimal)(e.X / scalevalue),4) + "," + Decimal.Round((decimal)((newsize - e.Y) / scalevalue),4);
        }
        private void pictureBox6_MouseEnter(object sender, EventArgs e)
        {
            if (pictureBox6.Focused == false)
            {
                pictureBox6.Focus();
                panel5.AutoScrollPosition = _newscrollPosition;
                pictureBox6.Invalidate();
            }
        }
        private void pictureBox6_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                pictureBox6_ZoomOut();
            }
            else
            {
                pictureBox6_ZoomIn();
            }

        }
        private void pictureBox6_ZoomIn()
        {
            int oldpictureboxhieght = pictureBox6.Height;
            int oldpitureboxwidht = pictureBox6.Width;
            Point oldscrollpos = panel5.AutoScrollPosition;
            int tbv = trackBar6.Value;
            int newval = tbv + 1;
            if (newval >= 20)
                newval = 20;
            trackBar6.Value = newval;
            MissionMapscale = trackBar6.Value;
            SetsMissionScale();
            if (pictureBox6.Height > panel5.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox6.Height * newy);
                _newscrollPosition.Y = y * -1;
                panel5.AutoScrollPosition = _newscrollPosition;
            }
            if (pictureBox6.Width > panel5.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox6.Width * newy);
                _newscrollPosition.X = x * -1;
                panel5.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox6.Invalidate();
        }
        private void pictureBox6_ZoomOut()
        {
            int oldpictureboxhieght = pictureBox6.Height;
            int oldpitureboxwidht = pictureBox6.Width;
            Point oldscrollpos = panel5.AutoScrollPosition;
            int tbv = trackBar6.Value;
            int newval = tbv - 1;
            if (newval <= 1)
                newval = 1;
            trackBar6.Value = newval;
            MissionMapscale = trackBar6.Value;
            SetsMissionScale();
            if (pictureBox6.Height > panel5.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox6.Height * newy);
                _newscrollPosition.Y = y * -1;
                panel5.AutoScrollPosition = _newscrollPosition;
            }
            if (pictureBox6.Width > panel5.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox6.Width * newy);
                _newscrollPosition.X = x * -1;
                panel5.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox6.Invalidate();
        }
        private void SetsMissionScale()
        {

            decimal scalevalue = MissionMapscale * (decimal)0.05;
            decimal mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            pictureBox6.Size = new Size(newsize, newsize);
        }
        private void getSquare(Graphics drawingArea, Pen penToUse, Point center, int radius)
        {
            Rectangle rect2 = new Rectangle(center.X - radius, center.Y - radius, radius * 2, radius * 2);
            Pen pen = new Pen(Color.LimeGreen)
            {
                Width = 6,
                DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
            };
            drawingArea.DrawRectangle(pen, rect2);
        }
        private void DrawTerritories(object sender, PaintEventArgs e)
        {
            if (currentterritorytypeTerritory == null) return;
            decimal scalevalue = MissionMapscale * (decimal)0.05;
            if(TerritoryPaintAllCB.Checked)
            {
                foreach (territoriesConfig territoriesConfig in currentproject.territoriesList)
                {
                    foreach (territorytypeTerritory t in territoriesConfig.territorytype.territory)
                    {
                        if (!t.Equals(currentterritorytypeTerritory))
                        {
                            foreach (territorytypeTerritoryZone newpos in t.zone)
                            {
                                int centerX = (int)(Math.Round(newpos.x, 0) * scalevalue);
                                int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(newpos.z, 0) * scalevalue);

                                int radius = (int)(newpos.r * scalevalue);
                                Point center = new Point(centerX, centerY);
                                Pen pen = new Pen(Color.Red)
                                {
                                    Width = 4
                                };
                                string col = string.Format("{0:X}", t.color);
                                pen.Color = ColorTranslator.FromHtml("#" + col.Substring(2));
                                getCircle(e.Graphics, pen, center, radius);
                            }
                        }
                    }
                }
            }
            else if(TerritorieszonesCB.Checked)
            {
                foreach (territorytypeTerritory t in currentterritoriesConfig.territorytype.territory)
                {
                    if (!t.Equals(currentterritorytypeTerritory))
                    {
                        foreach (territorytypeTerritoryZone newpos in t.zone)
                        {
                            int centerX = (int)(Math.Round(newpos.x, 0) * scalevalue);
                            int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(newpos.z, 0) * scalevalue);

                            int radius = (int)(newpos.r * scalevalue);
                            Point center = new Point(centerX, centerY);
                            Pen pen = new Pen(Color.Red)
                            {
                                Width = 4
                            };
                            string col = string.Format("{0:X}", t.color);
                            pen.Color = ColorTranslator.FromHtml("#" + col.Substring(2));
                            getCircle(e.Graphics, pen, center, radius);
                        }
                    }
                }
            }
            foreach (territorytypeTerritoryZone newpos in currentterritorytypeTerritory.zone)
            {
                int centerX = (int)(Math.Round(newpos.x, 0) * scalevalue);
                int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(newpos.z, 0) * scalevalue);

                int radius = (int)(newpos.r * scalevalue);
                Point center = new Point(centerX, centerY);
                Pen pen = new Pen(Color.Red)
                {
                    Width = 4
                };
                string col = string.Format("{0:X}", currentterritorytypeTerritory.color);
                pen.Color = ColorTranslator.FromHtml("#" + col.Substring(2));
                getCircle(e.Graphics, pen, center, radius);
                if (newpos.Equals(currentterritorytypeTerritoryZone))
                {
                    getSquare(e.Graphics, pen, center, radius);
                }
            }
        }
        private void pictureBox6_DoubleClick(object sender, EventArgs e)
        {
            
        }
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            
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
        private void trackBar6_MouseUp(object sender, MouseEventArgs e)
        {
            MissionMapscale = trackBar6.Value;
            SetsMissionScale();
        }
        private void populateterritorytreeview()
        {
            Console.WriteLine("populating Territories treeView");
            TerritoriesTreeview.Nodes.Clear();
            TreeNode root = new TreeNode(Path.GetFileName(filename))
            {
                Tag = "Parent"
            };
            //Set Vanilla Treenode types
            TreeNode Ignorelist = new TreeNode("Territories")
            {
                Tag = "Territories"
            };
            foreach (territoriesConfig territoriesConfig in currentproject.territoriesList)
            {
                TreeNode typenode = new TreeNode(Path.GetFileNameWithoutExtension(territoriesConfig.Filename))
                {
                    Tag = territoriesConfig
                };
                int name = 1;
                foreach (territorytypeTerritory territorytypeTerritory in territoriesConfig.territorytype.territory)
                {
                    TreeNode typeterritorynode = new TreeNode("Territory" + name.ToString())
                    {
                        Tag = territorytypeTerritory
                    };
                    name++;
                    typenode.Nodes.Add(typeterritorynode);
                }
                Ignorelist.Nodes.Add(typenode);
            }
            Console.WriteLine("All Territory files Populated......");
            //treeView1.Nodes.Add(root);
            root.Nodes.Add(Ignorelist);
            TerritoriesTreeview.Nodes.Add(root);
        }
        private void TerritoriesTreeview_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode usingtreenode = e.Node;
            if (e.Node.Tag is string)
            {

            }
            else if (e.Node.Tag is territoriesConfig)
            {
                currentterritoriesConfig = e.Node.Tag as territoriesConfig;
                currentterritorytypeTerritory = null;
                if (e.Button == MouseButtons.Right)
                {
                    TerritoryContextMenu.Show(Cursor.Position);
                    addNewTerritoryToolStripMenuItem.Visible = true;
                    removeTerritoryToolStripMenuItem.Visible = false;
                }
            }
            else if (e.Node.Tag is territorytypeTerritory)
            {
                currentterritorytypeTerritory = e.Node.Tag as territorytypeTerritory;
                currentterritoriesConfig = e.Node.Parent.Tag as territoriesConfig;

                TerritoriesZonesLB.DisplayMember = "DisplayName";
                TerritoriesZonesLB.ValueMember = "Value";
                TerritoriesZonesLB.DataSource = currentterritorytypeTerritory.zone;

                pictureBox6.Invalidate();
                pictureBox3.Invalidate();

                if (e.Button == MouseButtons.Right)
                {
                    TerritoryContextMenu.Show(Cursor.Position);
                    addNewTerritoryToolStripMenuItem.Visible = false;
                    removeTerritoryToolStripMenuItem.Visible = true;
                }
            }
            TerritoriesTreeview.SelectedNode = usingtreenode;
        }
        private void TerritorieszonesCB_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox6.Invalidate();
            pictureBox3.Invalidate();
        }
        private void TerritoryPaintAllCB_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox6.Invalidate();
            pictureBox3.Invalidate();
        }
        private void TerritoriesZonesLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentterritorytypeTerritoryZone = TerritoriesZonesLB.SelectedItem as territorytypeTerritoryZone;
            if (currentterritorytypeTerritoryZone == null) return;
            pictureBox6.Invalidate();
            isUserInteraction = false;
            TerritoriesZonesRadiusNUD.Value = currentterritorytypeTerritoryZone.r;
            TerritoriesZonesStaticMInNUD.Value = currentterritorytypeTerritoryZone.smin;
            TerritoriesZonesStaticMaxNUD.Value = currentterritorytypeTerritoryZone.smax;
            TerritoriesZonesDynamicMinNUD.Value = currentterritorytypeTerritoryZone.dmin;
            TerritoriesZonesDynamicMaxNUD.Value = currentterritorytypeTerritoryZone.dmax;
            RadioButton rb = groupBox74.Controls
                              .OfType<RadioButton>()
                              .FirstOrDefault(x => x.Text == currentterritorytypeTerritoryZone.name);
            if (rb != null)
                rb.Checked = true;
            else
            {
                TerritoriesZonesDynamicRB.Checked = true;
                TerritoriesZonesDynamicTB.Text = currentterritorytypeTerritoryZone.name;
            }
            pictureBox3.Invalidate();
            isUserInteraction = true;
        }
        private void TerritoriesZonesAIUSage_CheckedChanged(object sender, EventArgs e)
        {
            if (TerritoriesZonesDynamicRB.Checked)
            {
                TerritoriesZonesDynamicTB.Visible = true;
            }
            else
            {
                TerritoriesZonesDynamicTB.Visible = false;
            }
            if (!isUserInteraction) return;
            RadioButton rb = groupBox74.Controls
                              .OfType<RadioButton>()
                              .FirstOrDefault(x => x.Checked == true);
            if (rb.Text == "Dynamic")
                currentterritorytypeTerritoryZone.name = TerritoriesZonesDynamicTB.Text;
            else
                currentterritorytypeTerritoryZone.name = rb.Text;
            TerritoriesZonesLB.Refresh();
            currentterritoriesConfig.isDirty = true;

        }
        private void TerritoriesZonesDynamicTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            currentterritorytypeTerritoryZone.name = TerritoriesZonesDynamicTB.Text;
            currentterritoriesConfig.isDirty = true;
            TerritoriesZonesLB.Refresh();
        }
        private void TerritoriesZonesRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            currentterritorytypeTerritoryZone.r = (int)TerritoriesZonesRadiusNUD.Value;
            currentterritoriesConfig.isDirty = true;
            pictureBox6.Invalidate();
        }
        private void TerritoriesZonesStaticMInNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            currentterritorytypeTerritoryZone.smin = (int)TerritoriesZonesStaticMInNUD.Value;
            currentterritoriesConfig.isDirty = true;
        }
        private void TerritoriesZonesStaticMaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
           currentterritorytypeTerritoryZone.smax = (int)TerritoriesZonesStaticMaxNUD.Value;
            currentterritoriesConfig.isDirty = true;
        }
        private void TerritoriesZonesDynamicMinNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            currentterritorytypeTerritoryZone.dmin = (int)TerritoriesZonesDynamicMinNUD.Value;
            currentterritoriesConfig.isDirty = true;
        }
        private void TerritoriesZonesDynamicMaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            currentterritorytypeTerritoryZone.dmax = (int)TerritoriesZonesDynamicMaxNUD.Value;
            currentterritoriesConfig.isDirty = true;
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (currentterritorytypeTerritory == null) return;
            PictureBox pb = sender as PictureBox;
            ColorPickerDialog cpick = new ColorPickerDialog();
            cpick.StartPosition = FormStartPosition.CenterParent;
            string col = string.Format("{0:X}", currentterritorytypeTerritory.color);
            cpick.Color = ColorTranslator.FromHtml("#" + col.Substring(2));
            if (cpick.ShowDialog() == DialogResult.OK)
            {
                long answer = Convert.ToInt64(cpick.Color.Name.ToLower(), 16);
                currentterritorytypeTerritory.color = answer;
                pb.Invalidate();
                pictureBox6.Invalidate();
                currentterritoriesConfig.isDirty = true;
            }
        }
        private void pictureBox3_Paint(object sender, PaintEventArgs e)
        {
            if (currentterritorytypeTerritory == null) return;
            PictureBox pb = sender as PictureBox;
            Rectangle region;
            region = pb.ClientRectangle;
            string col = string.Format("{0:X}", currentterritorytypeTerritory.color);
            Color colour = ColorTranslator.FromHtml("#" + col.Substring(2));
            using (Brush brush = new SolidBrush(colour))
            {
                e.Graphics.FillRectangle(brush, region);
            }
            e.Graphics.DrawRectangle(SystemPens.ControlText, region.Left, region.Top, region.Width - 1, region.Height - 1);
           

        }
        private void darkButton65_Click(object sender, EventArgs e)
        {
            if (currentterritoriesConfig == null) { return; }
            territorytypeTerritoryZone newzone = new territorytypeTerritoryZone()
            {
                name = "Graze",
                smin = 0,
                smax = 0,
                dmin = 0,
                dmax = 0,
                x = currentproject.MapSize / 2,
                z = currentproject.MapSize / 2,
                r = 50
            };
            currentterritorytypeTerritory.zone.Add(newzone);
            pictureBox3.Invalidate();
            pictureBox6.Invalidate();
            TerritoriesZonesLB.Refresh();
            currentterritoriesConfig.isDirty = true;
        }
        private void darkButton64_Click(object sender, EventArgs e)
        {

            if (TerritoriesZonesLB.SelectedItems.Count == 0) return;
            territorytypeTerritoryZone currentpos = TerritoriesZonesLB.SelectedItem as territorytypeTerritoryZone;
            currentterritorytypeTerritory.zone.Remove(currentpos);
            pictureBox3.Invalidate();
            pictureBox6.Invalidate();
            TerritoriesZonesLB.Refresh();
            currentterritoriesConfig.isDirty = true;
        }
        private void addNewTerritoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            territorytypeTerritory newterritory = new territorytypeTerritory()
            {
                zone = new BindingList<territorytypeTerritoryZone>(),
                color = 4294967295
            };
            currentterritoriesConfig.territorytype.territory.Add(newterritory);

            var savedExpansionState = TerritoriesTreeview.Nodes.GetExpansionState();
            TerritoriesTreeview.BeginUpdate();
            populateterritorytreeview();
            TerritoriesTreeview.Nodes.SetExpansionState(savedExpansionState);
            TerritoriesTreeview.EndUpdate();

            currentterritoriesConfig.isDirty = true;
        }
        private void removeTerritoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode usingtreenode = TerritoriesTreeview.SelectedNode.Parent.Nodes[0];
            currentterritoriesConfig.territorytype.territory.Remove(currentterritorytypeTerritory);
            currentterritorytypeTerritory = null;
            currentterritorytypeTerritoryZone = null;
            TreeNode parentnode = TerritoriesTreeview.SelectedNode.Parent;
            var savedExpansionState = TerritoriesTreeview.Nodes.GetExpansionState();
            TerritoriesTreeview.BeginUpdate();
            populateterritorytreeview();
            TerritoriesTreeview.Nodes.SetExpansionState(savedExpansionState);
            TerritoriesTreeview.EndUpdate();
            TerritoriesZonesLB.Refresh();
            currentterritoriesConfig.isDirty = true;
        }
        #endregion territories
        #region initc
        //styles
        TextStyle BlueStyle = new TextStyle(Brushes.Blue, null, FontStyle.Regular);
        TextStyle BoldStyle = new TextStyle(null, null, FontStyle.Bold | FontStyle.Underline);
        TextStyle GrayStyle = new TextStyle(Brushes.Gray, null, FontStyle.Regular);
        TextStyle MagentaStyle = new TextStyle(Brushes.Magenta, null, FontStyle.Regular);
        TextStyle GreenStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);
        TextStyle BrownStyle = new TextStyle(Brushes.Brown, null, FontStyle.Italic);
        TextStyle MaroonStyle = new TextStyle(Brushes.Maroon, null, FontStyle.Regular);
        MarkerStyle SameWordsStyle = new MarkerStyle(new SolidBrush(Color.FromArgb(40, Color.Gray)));
        private void loadinitC()
        {
            isUserInteraction = false;
            fastColoredTextBox1.Text = File.ReadAllText(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\init.c");
            isUserInteraction = true;
        }
        private void InitStylesPriority()
        {
            //add this style explicitly for drawing under other styles
            fastColoredTextBox1.AddStyle(SameWordsStyle);
        }
        private void fastColoredTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            CSharpSyntaxHighlight(e);//custom highlighting
        }
        private void CSharpSyntaxHighlight(TextChangedEventArgs e)
        {
            fastColoredTextBox1.LeftBracket = '(';
            fastColoredTextBox1.RightBracket = ')';
            fastColoredTextBox1.LeftBracket2 = '\x0';
            fastColoredTextBox1.RightBracket2 = '\x0';
            //clear style of changed range
            e.ChangedRange.ClearStyle(BlueStyle, BoldStyle, GrayStyle, MagentaStyle, GreenStyle, BrownStyle);

            //string highlighting
            e.ChangedRange.SetStyle(BrownStyle, @"""""|@""""|''|@"".*?""|(?<!@)(?<range>"".*?[^\\]"")|'.*?[^\\]'");
            //comment highlighting
            e.ChangedRange.SetStyle(GreenStyle, @"//.*$", RegexOptions.Multiline);
            e.ChangedRange.SetStyle(GreenStyle, @"(/\*.*?\*/)|(/\*.*)", RegexOptions.Singleline);
            e.ChangedRange.SetStyle(GreenStyle, @"(/\*.*?\*/)|(.*\*/)", RegexOptions.Singleline | RegexOptions.RightToLeft);
            //number highlighting
            e.ChangedRange.SetStyle(MagentaStyle, @"\b\d+[\.]?\d*([eE]\-?\d+)?[lLdDfF]?\b|\b0x[a-fA-F\d]+\b");
            //attribute highlighting
            e.ChangedRange.SetStyle(GrayStyle, @"^\s*(?<range>\[.+?\])\s*$", RegexOptions.Multiline);
            //class name highlighting
            e.ChangedRange.SetStyle(BoldStyle, @"\b(class|struct|enum|interface)\s+(?<range>\w+?)\b");
            //keyword highlighting
            e.ChangedRange.SetStyle(BlueStyle, @"\b(abstract|as|base|bool|break|byte|case|catch|char|checked|class|const|continue|decimal|default|delegate|do|double|else|enum|event|explicit|extern|false|finally|fixed|float|for|foreach|goto|if|implicit|in|int|interface|internal|is|lock|long|namespace|new|null|object|operator|out|override|params|private|protected|public|readonly|ref|return|sbyte|sealed|short|sizeof|stackalloc|static|string|struct|switch|this|throw|true|try|typeof|uint|ulong|unchecked|unsafe|ushort|using|virtual|void|volatile|while|add|alias|ascending|descending|dynamic|from|get|global|group|into|join|let|orderby|partial|remove|select|set|value|var|where|yield)\b|#region\b|#endregion\b");

            //clear folding markers
            e.ChangedRange.ClearFoldingMarkers();

            //set folding markers
            e.ChangedRange.SetFoldingMarkers("{", "}");//allow to collapse brackets block
            e.ChangedRange.SetFoldingMarkers(@"#region\b", @"#endregion\b");//allow to collapse #region blocks
            e.ChangedRange.SetFoldingMarkers(@"/\*", @"\*/");//allow to collapse comment block
        }
        private void fastColoredTextBox1_AutoIndentNeeded(object sender, AutoIndentEventArgs args)
        {
            //block {}
            if (Regex.IsMatch(args.LineText, @"^[^""']*\{.*\}[^""']*$"))
                return;
            //start of block {}
            if (Regex.IsMatch(args.LineText, @"^[^""']*\{"))
            {
                args.ShiftNextLines = args.TabLength;
                return;
            }
            //end of block {}
            if (Regex.IsMatch(args.LineText, @"}[^""']*$"))
            {
                args.Shift = -args.TabLength;
                args.ShiftNextLines = -args.TabLength;
                return;
            }
            //label
            if (Regex.IsMatch(args.LineText, @"^\s*\w+\s*:\s*($|//)") &&
                !Regex.IsMatch(args.LineText, @"^\s*default\s*:"))
            {
                args.Shift = -args.TabLength;
                return;
            }
            //some statements: case, default
            if (Regex.IsMatch(args.LineText, @"^\s*(case|default)\b.*:\s*($|//)"))
            {
                args.Shift = -args.TabLength / 2;
                return;
            }
            //is unclosed operator in previous line ?
            if (Regex.IsMatch(args.PrevLineText, @"^\s*(if|for|foreach|while|[\}\s]*else)\b[^{]*$"))
                if (!Regex.IsMatch(args.PrevLineText, @"(;\s*$)|(;\s*//)"))//operator is unclosed
                {
                    args.Shift = args.TabLength;
                    return;
                }
        }
        private void fastColoredTextBox1_SelectionChangedDelayed(object sender, EventArgs e)
        {
            fastColoredTextBox1.VisibleRange.ClearStyle(SameWordsStyle);
            if (!fastColoredTextBox1.Selection.IsEmpty)
                return;//user selected diapason

            //get fragment around caret
            var fragment = fastColoredTextBox1.Selection.GetFragment(@"\w");
            string text = fragment.Text;
            if (text.Length == 0)
                return;
            //highlight same words
            var ranges = fastColoredTextBox1.VisibleRange.GetRanges("\\b" + text + "\\b").ToArray();
            if (ranges.Length > 1)
                foreach (var r in ranges)
                    r.SetStyle(SameWordsStyle);
        }
        private void fastColoredTextBox1_CustomAction(object sender, CustomActionEventArgs e)
        {
            MessageBox.Show(e.Action.ToString());
        }
        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.ShowFindDialog();
        }
        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.ShowReplaceDialog();
        }
        private void commentSelectedLinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.InsertLinePrefix(fastColoredTextBox1.CommentPrefix);
        }
        private void uncommentSelectedLinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.RemoveLinePrefix(fastColoredTextBox1.CommentPrefix);
        }
        private void autoIndentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.DoAutoIndent();
        }
        #endregion initc
        #region spawngear
        public SpawnGearPresetFiles currentspawnGearPresetFiles;
        public Attachmentslotitemset CurrentAttachmentslotitemset;
        public Discreteitemset CurrentDiscreteitemset;
        public Complexchildrentype CurrentComplexchildrentype;
        public Discreteunsorteditemset CurrentDiscreteunsorteditemset;
        public TreeNode CurrentTreeNode { get; private set; }
        private void LoadSpawnGear()
        {
            isUserInteraction = false;
            SpawnGearPresetFilesLB.DisplayMember = "DisplayName";
            SpawnGearPresetFilesLB.ValueMember = "Value";
            SpawnGearPresetFilesLB.DataSource = cfggameplay.SpawnGearPresetFiles;
            isUserInteraction = true;
        }
        private void listBox7_SelectedIndexChanged_2(object sender, EventArgs e)
        {
            if (SpawnGearPresetFilesLB.SelectedItems.Count < 1) return;
            currentspawnGearPresetFiles = SpawnGearPresetFilesLB.SelectedItem as SpawnGearPresetFiles;
            isUserInteraction = false;
            LoadSpawnGearTreeView();
            isUserInteraction = true;
        }
        private void LoadSpawnGearTreeView()
        {
            treeViewMS3.Nodes.Clear();
            TreeNode rootNode = new TreeNode(currentspawnGearPresetFiles.Filename)
            {
                Tag = "SpawnGearPresetFilesParent"
            };
            rootNode.Nodes.Add(new TreeNode("Name")
            {
                Tag = "name"
            });
            rootNode.Nodes.Add(new TreeNode("Spawn Weight")
            {
                Tag = "spawnWeight"
            });
            rootNode.Nodes.Add(new TreeNode("Character Types")
            {
                Tag = "characterTypes"
            });
            TreeNode AttachmentslotitemsetNode = new TreeNode("Attachment slot item set")
            {
                Tag = "attachmentSlotItemSetsParent"
            };
            foreach (Attachmentslotitemset Slot in currentspawnGearPresetFiles.attachmentSlotItemSets)
            {
                AttachmentslotitemsetNode.Nodes.Add(AttachmentslotitemsetNodeTN(Slot));
            }
            rootNode.Nodes.Add(AttachmentslotitemsetNode);
            TreeNode discreteUnsortedItemSets = new TreeNode("Discrete Unsorted Item Sets")
            {
                Tag = "discreteUnsortedItemSetsParent"
            };
            foreach (Discreteunsorteditemset DUIS in currentspawnGearPresetFiles.discreteUnsortedItemSets)
            {
                discreteUnsortedItemSets.Nodes.Add(DiscreteunsorteditemsetTN(DUIS));
            }
            rootNode.Nodes.Add(discreteUnsortedItemSets);
            treeViewMS3.Nodes.Add(rootNode);
        }
        private TreeNode DiscreteunsorteditemsetTN(Discreteunsorteditemset dUIS)
        {
            TreeNode DUIS = new TreeNode(dUIS.name)
            {
                Tag = dUIS
            };
            DUIS.Nodes.Add(new TreeNode("Spawn Weight")
            {
                Tag = "spawnWeight"
            });
            DUIS.Nodes.Add(new TreeNode("Attributes")
            {
                Tag = "attributes"
            });
            TreeNode ComplexChildrenTypes = new TreeNode("Complex Children Types")
            {
                Tag = "complexChildrenTypes"
            };
            foreach (Complexchildrentype CCT in dUIS.complexChildrenTypes)
            {
                ComplexChildrenTypes.Nodes.Add(ComplexChildrenTypesNodeTN(CCT));
            }
            DUIS.Nodes.Add(ComplexChildrenTypes);
            DUIS.Nodes.Add(new TreeNode("Simple Children Use Default Attributes")
            {
                Tag = "simpleChildrenUseDefaultAttributes"
            });
            DUIS.Nodes.Add(new TreeNode("Simple Children Types")
            {
                Tag = "simpleChildrenTypes"
            });

            return DUIS;
        }
        private TreeNode AttachmentslotitemsetNodeTN(Attachmentslotitemset slot)
        {
            string slotname = slot.slotName;
            TreeNode ASISnode = new TreeNode(slotname)
            {
                Tag = slot
            };
            foreach (Discreteitemset DIS in slot.discreteItemSets)
            {
                ASISnode.Nodes.Add(DiscreetItemSetsTN(DIS));
            }
            return ASISnode;
        }
        private TreeNode DiscreetItemSetsTN( Discreteitemset DIS)
        {
            TreeNode DISNode = new TreeNode(DIS.itemType)
            {
                Tag = DIS
            };
            DISNode.Nodes.Add(new TreeNode("Spawn Weight")
            {
                Tag = "spawnWeight"
            });
            DISNode.Nodes.Add(new TreeNode("Attributes")
            {
                Tag = "attributes"
            });
            DISNode.Nodes.Add(new TreeNode("Quick Bar Slot")
            {
                Tag = "quickBarSlot"
            });
            TreeNode ComplexChildrenTypes = new TreeNode("Complex Children Types")
            {
                Tag = "complexChildrenTypes"
            };
            foreach (Complexchildrentype CCT in DIS.complexChildrenTypes)
            {
                ComplexChildrenTypes.Nodes.Add(ComplexChildrenTypesNodeTN(CCT));
            }
            DISNode.Nodes.Add(ComplexChildrenTypes);
            DISNode.Nodes.Add(new TreeNode("Simple Children Use Default Attributes")
            {
                Tag = "simpleChildrenUseDefaultAttributes"
            });
            DISNode.Nodes.Add(new TreeNode("Simple Children Types")
            {
                Tag = "simpleChildrenTypes"
            });

            return DISNode;
        }
        private TreeNode ComplexChildrenTypesNodeTN(Complexchildrentype cCT)
        {
            string slotname = cCT.itemType;
            TreeNode CCTNode = new TreeNode(slotname)
            {
                Tag = cCT
            };
            CCTNode.Nodes.Add(new TreeNode("Attributes")
            {
                Tag = "attributes"
            });
            CCTNode.Nodes.Add(new TreeNode("Quick Bar Slot")
            {
                Tag = "quickBarSlot"
            });
            CCTNode.Nodes.Add(new TreeNode("Simple Children Use Default Attributes")
            {
                Tag = "simpleChildrenUseDefaultAttributes"
            });
            CCTNode.Nodes.Add(new TreeNode("Simple Children Types")
            {
                Tag = "simpleChildrenTypes"
            });
            return CCTNode;
        }
        private void darkButton68_Click(object sender, EventArgs e)
        {
            AddNeweventFile form = new AddNeweventFile
            {
                currentproject = currentproject,
                newlocation = true,
                SetTitle = "Add New Spawn Gear File",
                settype = "Spawn Gear Preset File Name",
                SetFolderName = "Select folder where File will created, must be in mpmission folder....",
                setbuttontest = "Add Spawn Gear Preset"
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                string path = form.CustomLocation;
                string modname = form.TypesName;
                Directory.CreateDirectory(path);
                cfggameplay.Addnewspawngearfile(path.Replace(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\", "") + "/" + modname + ".json");
                currentproject.CFGGameplayConfig.isDirty = true;
            }
        }
        private void darkButton69_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This Will Remove The All reference to this SpawnGearPreset, Are you sure you want to do this?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (File.Exists(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\" + currentspawnGearPresetFiles.Filename))
                {
                    File.Delete(currentproject.projectFullName + "\\mpmissions\\" + currentproject.mpmissionpath + "\\" + currentspawnGearPresetFiles.Filename);
                }
                cfggameplay.SpawnGearPresetFiles.Remove(currentspawnGearPresetFiles);
                currentproject.CFGGameplayConfig.isDirty = true;
            }
        }
        private void treeViewMS3_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            isUserInteraction = false;
            treeViewMS3.SelectedNode = e.Node;
            CurrentTreeNode = e.Node;

            SpawnGearNameGB.Visible = false;
            characterTypesGB.Visible = false;
            attachmentSlotItemSetsGB.Visible = false;
            itemTypeGB.Visible = false;
            spawnWeightGB.Visible = false;
            SpawnGearAttributesGB.Visible = false;
            quickBarSlotGB.Visible = false;
            simpleChildrenUseDefaultAttributesGB.Visible = false;
            simpleChildrenTypesGB.Visible = false;

            addNewAttachmentSlotItemSet.Visible = false;
            deleteAttachmentSlotItemSetToolStripMenuItem.Visible = false;
            addNewDisctreetItemSetToolStripMenuItem.Visible = false;
            deleteDiscreetItemSetToolStripMenuItem.Visible = false;
            addNewComplexChildSetToolStripMenuItem.Visible = false;
            deleteComplexChildSetToolStripMenuItem.Visible = false;
            addNewDiscreetUnsortedSetToolStripMenuItem.Visible = false;
            deleteDiscreetUnsortedItemSetToolStripMenuItem.Visible = false;

            if (e.Node.Tag is string)
            {
                if (e.Node.Tag.ToString() == "SpawnGearPresetFilesParent")
                {
                    isUserInteraction = true;
                    return;
                }
                if (e.Node.Tag.ToString() is "attachmentSlotItemSetsParent")
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        addNewAttachmentSlotItemSet.Visible = true;
                        SpawnGearCMS.Show(Cursor.Position);
                    }
                }
                else if (e.Node.Tag.ToString() == "discreteUnsortedItemSetsParent")
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        addNewDiscreetUnsortedSetToolStripMenuItem.Visible = true;
                        SpawnGearCMS.Show(Cursor.Position);
                    }
                }
                else if (e.Node.Parent.Tag.ToString() == "SpawnGearPresetFilesParent")
                {
                    if (e.Node.Tag.ToString() == "spawnWeight")
                    {
                        spawnWeightGB.Visible = true;
                        spawnWeightNUD.Value = currentspawnGearPresetFiles.spawnWeight;
                    }
                    else if (e.Node.Tag.ToString() == "name")
                    {
                        SpawnGearNameGB.Visible = true;
                        SpawnGearNameTB.Text = currentspawnGearPresetFiles.name;
                    }
                    else if (e.Node.Tag.ToString() == "characterTypes")
                    {
                        characterTypesGB.Visible = true;
                        characterTypesLB.DisplayMember = "DisplayName";
                        characterTypesLB.ValueMember = "Value";
                        characterTypesLB.DataSource = currentspawnGearPresetFiles.characterTypes;
                    }
                }
                else if (e.Node.Parent.Tag is Discreteitemset)
                {
                    CurrentDiscreteitemset = e.Node.Parent.Tag as Discreteitemset;
                    if (e.Node.Tag.ToString() == "spawnWeight")
                    {
                        spawnWeightGB.Visible = true;
                        spawnWeightNUD.Value = CurrentDiscreteitemset.spawnWeight;
                    }
                    else if (e.Node.Tag.ToString() == "attributes")
                    {
                        SpawnGearAttributesGB.Visible = true;
                        SpawnGearhealthMinNUD.Value = CurrentDiscreteitemset.attributes.healthMin;
                        SpawnGearhealthMaxNUD.Value = CurrentDiscreteitemset.attributes.healthMax;
                        SpawnGearQuanityMinNUD.Value = CurrentDiscreteitemset.attributes.quantityMin;
                        SpawnGearQuanityMaxNUD.Value = CurrentDiscreteitemset.attributes.quantityMax;
                    }
                    else if (e.Node.Tag.ToString() == "quickBarSlot")
                    {
                        quickBarSlotGB.Visible = true;
                        quickBarSlotNUD.Value = CurrentDiscreteitemset.quickBarSlot;
                    }
                    else if (e.Node.Tag.ToString() == "simpleChildrenUseDefaultAttributes")
                    {
                        simpleChildrenUseDefaultAttributesGB.Visible = true;
                        simpleChildrenUseDefaultAttributesCB.Checked = CurrentDiscreteitemset.simpleChildrenUseDefaultAttributes;
                    }
                    else if (e.Node.Tag.ToString() == "simpleChildrenTypes")
                    {
                        simpleChildrenTypesGB.Visible = true;
                        simpleChildrenTypesLB.DisplayMember = "DisplayName";
                        simpleChildrenTypesLB.ValueMember = "Value";
                        simpleChildrenTypesLB.DataSource = CurrentDiscreteitemset.simpleChildrenTypes;
                    }
                    else if (e.Node.Tag.ToString() == "complexChildrenTypes")
                    {
                        if (e.Button == MouseButtons.Right)
                        {
                            addNewComplexChildSetToolStripMenuItem.Visible = true;
                            SpawnGearCMS.Show(Cursor.Position);
                        }
                    }
                }
                else if (e.Node.Parent.Tag is Complexchildrentype)
                {
                    CurrentComplexchildrentype = e.Node.Parent.Tag as Complexchildrentype;
                    if (e.Node.Tag.ToString() == "attributes")
                    {
                        SpawnGearAttributesGB.Visible = true;
                        SpawnGearhealthMinNUD.Value = CurrentComplexchildrentype.attributes.healthMin;
                        SpawnGearhealthMaxNUD.Value = CurrentComplexchildrentype.attributes.healthMax;
                        SpawnGearQuanityMinNUD.Value = CurrentComplexchildrentype.attributes.quantityMin;
                        SpawnGearQuanityMaxNUD.Value = CurrentComplexchildrentype.attributes.quantityMax;
                    }
                    else if (e.Node.Tag.ToString() == "quickBarSlot")
                    {
                        quickBarSlotGB.Visible = true;
                        quickBarSlotNUD.Value = CurrentComplexchildrentype.quickBarSlot;
                    }
                    else if (e.Node.Tag.ToString() == "simpleChildrenUseDefaultAttributes")
                    {
                        simpleChildrenUseDefaultAttributesGB.Visible = true;
                        simpleChildrenUseDefaultAttributesCB.Checked = CurrentComplexchildrentype.simpleChildrenUseDefaultAttributes;
                    }
                    else if (e.Node.Tag.ToString() == "simpleChildrenTypes")
                    {
                        simpleChildrenTypesGB.Visible = true;
                        simpleChildrenTypesLB.DisplayMember = "DisplayName";
                        simpleChildrenTypesLB.ValueMember = "Value";
                        simpleChildrenTypesLB.DataSource = CurrentComplexchildrentype.simpleChildrenTypes;
                    }
                }
                else if (e.Node.Parent.Tag is Discreteunsorteditemset)
                {
                    CurrentDiscreteunsorteditemset = e.Node.Parent.Tag as Discreteunsorteditemset;
                    if (e.Node.Tag.ToString() == "spawnWeight")
                    {
                        spawnWeightGB.Visible = true;
                        spawnWeightNUD.Value = CurrentDiscreteunsorteditemset.spawnWeight;
                    }
                    else if (e.Node.Tag.ToString() == "attributes")
                    {
                        SpawnGearAttributesGB.Visible = true;
                        SpawnGearhealthMinNUD.Value = CurrentDiscreteunsorteditemset.attributes.healthMin;
                        SpawnGearhealthMaxNUD.Value = CurrentDiscreteunsorteditemset.attributes.healthMax;
                        SpawnGearQuanityMinNUD.Value = CurrentDiscreteunsorteditemset.attributes.quantityMin;
                        SpawnGearQuanityMaxNUD.Value = CurrentDiscreteunsorteditemset.attributes.quantityMax;
                    }
                    else if (e.Node.Tag.ToString() == "simpleChildrenUseDefaultAttributes")
                    {
                        simpleChildrenUseDefaultAttributesGB.Visible = true;
                        simpleChildrenUseDefaultAttributesCB.Checked = CurrentDiscreteunsorteditemset.simpleChildrenUseDefaultAttributes;
                    }
                    else if (e.Node.Tag.ToString() == "simpleChildrenTypes")
                    {
                        simpleChildrenTypesGB.Visible = true;
                        simpleChildrenTypesLB.DisplayMember = "DisplayName";
                        simpleChildrenTypesLB.ValueMember = "Value";
                        simpleChildrenTypesLB.DataSource = CurrentDiscreteunsorteditemset.simpleChildrenTypes;
                    }
                    else if (e.Node.Tag.ToString() == "complexChildrenTypes")
                    {
                        if (e.Button == MouseButtons.Right)
                        {
                            addNewComplexChildSetToolStripMenuItem.Visible = true;
                            SpawnGearCMS.Show(Cursor.Position);
                        }
                    }
                }
            }
            else if (e.Node.Tag is Attachmentslotitemset)
            {
                attachmentSlotItemSetsGB.Visible = true;
                CurrentAttachmentslotitemset = e.Node.Tag as Attachmentslotitemset;
                string slotname = CurrentAttachmentslotitemset.slotName;
                ItemAttachmentSlotNameCB.SelectedIndex = ItemAttachmentSlotNameCB.FindStringExact(slotname);
                if (e.Button == MouseButtons.Right)
                {
                    deleteAttachmentSlotItemSetToolStripMenuItem.Visible = true;
                    addNewDisctreetItemSetToolStripMenuItem.Visible = true;
                    SpawnGearCMS.Show(Cursor.Position);
                }
            }
            else if (e.Node.Tag is Discreteitemset)
            {
                CurrentDiscreteitemset = e.Node.Tag as Discreteitemset;
                itemTypeGB.Visible = true;
                SpawnGearItemTypeTB.Text = CurrentDiscreteitemset.itemType;
                if (e.Button == MouseButtons.Right)
                {
                    deleteDiscreetItemSetToolStripMenuItem.Visible = true;
                    SpawnGearCMS.Show(Cursor.Position);
                }
            }
            else if (e.Node.Tag is Complexchildrentype)
            {
                CurrentComplexchildrentype = e.Node.Tag as Complexchildrentype;
                itemTypeGB.Visible = true;
                SpawnGearItemTypeTB.Text = CurrentComplexchildrentype.itemType;
                if (e.Button == MouseButtons.Right)
                {
                    deleteComplexChildSetToolStripMenuItem.Visible = true;
                    SpawnGearCMS.Show(Cursor.Position);
                }
            }
            else if (e.Node.Tag is Discreteunsorteditemset)
            {
                CurrentDiscreteunsorteditemset = e.Node.Tag as Discreteunsorteditemset;
                SpawnGearNameGB.Visible = true;
                SpawnGearNameTB.Text = CurrentDiscreteunsorteditemset.name;
                if (e.Button == MouseButtons.Right)
                {
                    deleteDiscreetUnsortedItemSetToolStripMenuItem.Visible = true;
                    SpawnGearCMS.Show(Cursor.Position);
                }
            }
            isUserInteraction = true;
        }
        private void darkButton71_Click(object sender, EventArgs e)
        {
            string NPCClassname = characterTypesCB.GetItemText(characterTypesCB.SelectedItem);
            if (!currentspawnGearPresetFiles.characterTypes.Contains(NPCClassname))
            {
                currentspawnGearPresetFiles.characterTypes.Add(NPCClassname);
                currentspawnGearPresetFiles.isDirty = true;
            }
        }
        private void darkButton75_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < characterTypesCB.Items.Count; i++)
            {
                string NPCClassname = characterTypesCB.GetItemText(characterTypesCB.Items[i]);
                if (!currentspawnGearPresetFiles.characterTypes.Contains(NPCClassname))
                {
                    currentspawnGearPresetFiles.characterTypes.Add(NPCClassname);
                    currentspawnGearPresetFiles.isDirty = true;
                }
            }
        }
        private void darkButton72_Click(object sender, EventArgs e)
        {
            if (characterTypesLB.SelectedItems.Count < 1) return;
            List<String> lstitems = new List<String>();
            foreach (int i in characterTypesLB.SelectedIndices)
            {
                lstitems.Add(characterTypesLB.GetItemText(characterTypesLB.Items[i]));
            }
            foreach (var item in lstitems)
            {
                currentspawnGearPresetFiles.characterTypes.Remove(item.ToString());
                currentspawnGearPresetFiles.isDirty = true;
            }
            
        }
        private void darkButton73_Click(object sender, EventArgs e)
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
                if (CurrentTreeNode.Parent.Tag is Discreteitemset)
                {
                    foreach (string l in addedtypes)
                    {
                        if (!CurrentDiscreteitemset.simpleChildrenTypes.Contains(l))
                        {
                            CurrentDiscreteitemset.simpleChildrenTypes.Add(l);
                            simpleChildrenTypesLB.Refresh();
                            currentspawnGearPresetFiles.isDirty = true;
                        }
                    }
                }
                else if (CurrentTreeNode.Parent.Tag is Complexchildrentype)
                {
                    foreach (string l in addedtypes)
                    {
                        if (!CurrentComplexchildrentype.simpleChildrenTypes.Contains(l))
                        {
                            CurrentComplexchildrentype.simpleChildrenTypes.Add(l);
                            simpleChildrenTypesLB.Refresh();
                            currentspawnGearPresetFiles.isDirty = true;
                        }
                    }
                }
                else if (CurrentTreeNode.Parent.Tag is Discreteunsorteditemset)
                {
                    foreach (string l in addedtypes)
                    {
                        if (!CurrentDiscreteunsorteditemset.simpleChildrenTypes.Contains(l))
                        {
                            CurrentDiscreteunsorteditemset.simpleChildrenTypes.Add(l);
                            simpleChildrenTypesLB.Refresh();
                            currentspawnGearPresetFiles.isDirty = true;
                        }
                    }
                }
               
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void darkButton74_Click(object sender, EventArgs e)
        {
            if (CurrentTreeNode.Parent.Tag is Discreteitemset)
            {
                CurrentDiscreteitemset.simpleChildrenTypes.Remove(simpleChildrenTypesLB.GetItemText(simpleChildrenTypesLB.SelectedItem));
            }
            else if (CurrentTreeNode.Parent.Tag is Complexchildrentype)
            {
                CurrentComplexchildrentype.simpleChildrenTypes.Remove(simpleChildrenTypesLB.GetItemText(simpleChildrenTypesLB.SelectedItem));
            }
            else if (CurrentTreeNode.Parent.Tag is Discreteunsorteditemset)
            {
                CurrentDiscreteunsorteditemset.simpleChildrenTypes.Remove(simpleChildrenTypesLB.GetItemText(simpleChildrenTypesLB.SelectedItem));
            }
            currentspawnGearPresetFiles.isDirty = true;
            simpleChildrenTypesLB.Refresh();
        }
        private void quickBarSlotNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            if(CurrentTreeNode.Parent.Tag is Discreteitemset)
            {
                CurrentDiscreteitemset.quickBarSlot = (int)quickBarSlotNUD.Value;
                currentspawnGearPresetFiles.isDirty = true;
            }
            else if (CurrentTreeNode.Parent.Tag is Complexchildrentype)
            {
                CurrentComplexchildrentype.quickBarSlot = (int)quickBarSlotNUD.Value;
                currentspawnGearPresetFiles.isDirty = true;
            }
        }
        private void spawnWeightNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            if(CurrentTreeNode.Parent.Tag is string && CurrentTreeNode.Parent.Tag.ToString() == "SpawnGearPresetFilesParent")
            {
                currentspawnGearPresetFiles.spawnWeight = (int)spawnWeightNUD.Value;
                currentspawnGearPresetFiles.isDirty = true;
            }
            else if (CurrentTreeNode.Parent.Tag is Discreteitemset)
            {
                CurrentDiscreteitemset.spawnWeight = (int)spawnWeightNUD.Value;
                currentspawnGearPresetFiles.isDirty = true;
            }
            else if (CurrentTreeNode.Parent.Tag is Discreteunsorteditemset)
            {
                CurrentDiscreteunsorteditemset.spawnWeight = (int)spawnWeightNUD.Value;
                currentspawnGearPresetFiles.isDirty = true;
            }
        }
        private void darkButton70_Click(object sender, EventArgs e)
        {
            string Classname = "";
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseOnlySingleitem = true
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                foreach (string l in addedtypes)
                {
                    Classname = l;
                }
                if (CurrentTreeNode.Tag is Discreteitemset)
                {
                    CurrentDiscreteitemset.itemType = SpawnGearItemTypeTB.Text = CurrentTreeNode.Text = Classname;
                    currentspawnGearPresetFiles.isDirty = true;
                }
                else if (CurrentTreeNode.Tag is Complexchildrentype)
                {
                    CurrentComplexchildrentype.itemType = SpawnGearItemTypeTB.Text = CurrentTreeNode.Text = Classname;
                    currentspawnGearPresetFiles.isDirty = true;
                }
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void ItemAttachmentSlotNameCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            if(CurrentTreeNode.Parent.Tag.ToString() == "attachmentSlotItemSetsParent")
            {
                string Slotname = ItemAttachmentSlotNameCB.GetItemText(ItemAttachmentSlotNameCB.SelectedItem);
                if(!currentspawnGearPresetFiles.attachmentSlotItemSets.Any(x => x.slotName == Slotname))
                {
                    CurrentAttachmentslotitemset.slotName = CurrentTreeNode.Text = ItemAttachmentSlotNameCB.GetItemText(ItemAttachmentSlotNameCB.SelectedItem);
                    currentspawnGearPresetFiles.isDirty = true;
                }
                else
                {
                    MessageBox.Show("Slot Name allready in Use.....");
                }
            }
        }
        private void SpawnGearhealthMinNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            if (CurrentTreeNode.Parent.Tag is Discreteitemset)
            {
                CurrentDiscreteitemset.attributes.healthMin = SpawnGearhealthMinNUD.Value;
                currentspawnGearPresetFiles.isDirty = true;
            }
            else if (CurrentTreeNode.Parent.Tag is Complexchildrentype)
            {
                CurrentComplexchildrentype.attributes.healthMin = SpawnGearhealthMinNUD.Value;
                currentspawnGearPresetFiles.isDirty = true;
            }
            else if (CurrentTreeNode.Parent.Tag is Discreteunsorteditemset)
            {
                CurrentDiscreteunsorteditemset.attributes.healthMin = SpawnGearhealthMinNUD.Value;
                currentspawnGearPresetFiles.isDirty = true;
            }
        }
        private void SpawnGearhealthMaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            if (CurrentTreeNode.Parent.Tag is Discreteitemset)
            {
                CurrentDiscreteitemset.attributes.healthMax = SpawnGearhealthMaxNUD.Value;
                currentspawnGearPresetFiles.isDirty = true;
            }
            else if (CurrentTreeNode.Parent.Tag is Complexchildrentype)
            {
                CurrentComplexchildrentype.attributes.healthMax = SpawnGearhealthMaxNUD.Value;
                currentspawnGearPresetFiles.isDirty = true;
            }
            else if (CurrentTreeNode.Parent.Tag is Discreteunsorteditemset)
            {
                CurrentDiscreteunsorteditemset.attributes.healthMax = SpawnGearhealthMaxNUD.Value;
                currentspawnGearPresetFiles.isDirty = true;
            }
        }
        private void SpawnGearQuanityMinNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            if (CurrentTreeNode.Parent.Tag is Discreteitemset)
            {
                CurrentDiscreteitemset.attributes.quantityMin = SpawnGearQuanityMinNUD.Value;
                currentspawnGearPresetFiles.isDirty = true;
            }
            else if (CurrentTreeNode.Parent.Tag is Complexchildrentype)
            {
                CurrentComplexchildrentype.attributes.quantityMin = SpawnGearQuanityMinNUD.Value;
                currentspawnGearPresetFiles.isDirty = true;
            }
            else if (CurrentTreeNode.Parent.Tag is Discreteunsorteditemset)
            {
                CurrentDiscreteunsorteditemset.attributes.quantityMin = SpawnGearQuanityMinNUD.Value;
                currentspawnGearPresetFiles.isDirty = true;
            }
        }
        private void SpawnGearQuanityMaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            if (CurrentTreeNode.Parent.Tag is Discreteitemset)
            {
                CurrentDiscreteitemset.attributes.quantityMax = SpawnGearQuanityMaxNUD.Value;
                currentspawnGearPresetFiles.isDirty = true;
            }
            else if (CurrentTreeNode.Parent.Tag is Complexchildrentype)
            {
                CurrentComplexchildrentype.attributes.quantityMax = SpawnGearQuanityMaxNUD.Value;
                currentspawnGearPresetFiles.isDirty = true;
            }
            else if (CurrentTreeNode.Parent.Tag is Discreteunsorteditemset)
            {
                CurrentDiscreteunsorteditemset.attributes.quantityMax = SpawnGearQuanityMaxNUD.Value;
                currentspawnGearPresetFiles.isDirty = true;
            }
        }
        private void SpawnGearNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            if (CurrentTreeNode.Parent.Tag is string && CurrentTreeNode.Parent.Tag.ToString() == "SpawnGearPresetFilesParent")
            {
                currentspawnGearPresetFiles.name = CurrentTreeNode.Text = SpawnGearNameTB.Text;
                currentspawnGearPresetFiles.isDirty = true;
            }
            else if (CurrentTreeNode.Tag is Discreteunsorteditemset)
            {
                CurrentDiscreteunsorteditemset.name = CurrentTreeNode.Text = SpawnGearNameTB.Text;
                currentspawnGearPresetFiles.isDirty = true;
            }
        }
        private void simpleChildrenUseDefaultAttributesCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUserInteraction) return;
            if (CurrentTreeNode.Parent.Tag is Discreteitemset)
            {
                CurrentDiscreteitemset.simpleChildrenUseDefaultAttributes = simpleChildrenUseDefaultAttributesCB.Checked;
                currentspawnGearPresetFiles.isDirty = true;
            }
            else if (CurrentTreeNode.Parent.Tag is Complexchildrentype)
            {
                CurrentComplexchildrentype.simpleChildrenUseDefaultAttributes = simpleChildrenUseDefaultAttributesCB.Checked;
                currentspawnGearPresetFiles.isDirty = true;
            }
            else if (CurrentTreeNode.Parent.Tag is Discreteunsorteditemset)
            {
                CurrentDiscreteunsorteditemset.simpleChildrenUseDefaultAttributes = simpleChildrenUseDefaultAttributesCB.Checked;
                currentspawnGearPresetFiles.isDirty = true;
            }
        }
        private void addNewAttachmentSlotItemSet_Click(object sender, EventArgs e)
        {
            Attachmentslotitemset newASIS = new Attachmentslotitemset()
            {
                slotName = "CHANGE ME",
                discreteItemSets = new BindingList<Discreteitemset>()
            };
            currentspawnGearPresetFiles.attachmentSlotItemSets.Add(newASIS);
            CurrentTreeNode.Nodes.Add(AttachmentslotitemsetNodeTN(newASIS));
            currentspawnGearPresetFiles.isDirty = true;
        }
        private void deleteAttachmentSlotItemSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentspawnGearPresetFiles.attachmentSlotItemSets.Remove(CurrentAttachmentslotitemset);
            treeViewMS3.SelectedNode.Remove();
            currentspawnGearPresetFiles.isDirty = true;
        }
        private void addNewDisctreetItemSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Discreteitemset newDIS = new Discreteitemset()
            {
                itemType = "CHANGE ME",
                spawnWeight = 1,
                attributes = new Attributes()
                {
                    healthMin = 1,
                    healthMax = 1,
                    quantityMin = 1,
                    quantityMax = 1
                },
                quickBarSlot = -1,
                complexChildrenTypes = new BindingList<Complexchildrentype>(),
                simpleChildrenUseDefaultAttributes = false,
                simpleChildrenTypes = new BindingList<string>()
            };
            CurrentAttachmentslotitemset.discreteItemSets.Add(newDIS);
            CurrentTreeNode.Nodes.Add(DiscreetItemSetsTN(newDIS));
            currentspawnGearPresetFiles.isDirty = true;
        }
        private void deleteDiscreetItemSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentAttachmentslotitemset.discreteItemSets.Remove(CurrentDiscreteitemset);
            treeViewMS3.SelectedNode.Remove();
            currentspawnGearPresetFiles.isDirty = true;
        }
        private void addNewComplexChildSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Complexchildrentype newCCIS = new Complexchildrentype()
            {
                itemType = "CHANGE ME",
                attributes = new Attributes()
                {
                    healthMin = 1,
                    healthMax = 1,
                    quantityMin = 1,
                    quantityMax = 1
                },
                quickBarSlot = -1,
                simpleChildrenUseDefaultAttributes = false,
                simpleChildrenTypes = new BindingList<string>()
            };
            if (CurrentTreeNode.Parent.Tag is Discreteunsorteditemset)
            {
                CurrentDiscreteunsorteditemset.complexChildrenTypes.Add(newCCIS);
                CurrentTreeNode.Nodes.Add(ComplexChildrenTypesNodeTN(newCCIS));
                currentspawnGearPresetFiles.isDirty = true;

            }
            else if (CurrentTreeNode.Parent.Tag is Discreteitemset)
            {
                CurrentDiscreteitemset.complexChildrenTypes.Add(newCCIS);
                CurrentTreeNode.Nodes.Add(ComplexChildrenTypesNodeTN(newCCIS));
                currentspawnGearPresetFiles.isDirty = true;
            }
        }
        private void deleteComplexChildSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentTreeNode.Parent.Parent.Tag is Discreteunsorteditemset)
            {
                CurrentDiscreteunsorteditemset.complexChildrenTypes.Remove(CurrentComplexchildrentype);
                treeViewMS3.SelectedNode.Remove();
                currentspawnGearPresetFiles.isDirty = true;
            }
            else if (CurrentTreeNode.Parent.Parent.Tag is Discreteitemset)
            {
                CurrentDiscreteitemset.complexChildrenTypes.Remove(CurrentComplexchildrentype);
                treeViewMS3.SelectedNode.Remove();
                currentspawnGearPresetFiles.isDirty = true;
            }
        }
        private void addNewDiscreetUnsortedSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Discreteunsorteditemset newDUIS = new Discreteunsorteditemset()
            {
                name = "New Cargo - Change me",
                spawnWeight = 1,
                attributes = new Attributes()
                {
                    healthMin = 1,
                    healthMax = 1,
                    quantityMin = 1,
                    quantityMax = 1
                },
                complexChildrenTypes = new BindingList<Complexchildrentype>(),
                simpleChildrenUseDefaultAttributes = false,
                simpleChildrenTypes = new BindingList<string>()
            };
            currentspawnGearPresetFiles.discreteUnsortedItemSets.Add(newDUIS);
            CurrentTreeNode.Nodes.Add(DiscreteunsorteditemsetTN(newDUIS));
            currentspawnGearPresetFiles.isDirty = true;
        }
        private void deleteDiscreetUnsortedItemSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentspawnGearPresetFiles.discreteUnsortedItemSets.Remove(CurrentDiscreteunsorteditemset);
            treeViewMS3.SelectedNode.Remove();
            currentspawnGearPresetFiles.isDirty = true;
        }
        #endregion spawngear
        #region mapgrouppos
        public decimal mapgroupposMapscale = 1;
        private map mapgroupposmap;
        private mapGroup currentmapgroup;
        public TreeNode currentmapgrouptreenode;
        private void LoadMapGroupPos()
        {
            Console.WriteLine("Loading MapgroupPos");
            isUserInteraction = false;

            mapgroupposmap = currentproject.mapgrouppos.map;
            LoadeMapGroupPosTreeview();

            pictureBox4.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Map Size is 15360 x 15360, 0,0 bottom left, middle 7680 x 7680
            pictureBox4.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox4.Paint += new PaintEventHandler(DrawMapGrouPro);

            trackBar2.Value = 1;
            SetsMapGroupposScale();
            panel1.AutoScrollPosition = new Point(0, 0);

            isUserInteraction = true;
        }
        private void DrawMapGrouPro(object sender, PaintEventArgs e)
        {
            decimal scalevalue = mapgroupposMapscale * (decimal)0.05;
            foreach (mapGroup MGPMG in mapgroupposmap.group)
            {
                decimal xx = Convert.ToDecimal(MGPMG.pos.Split(' ')[0]);
                decimal yy = Convert.ToDecimal(MGPMG.pos.Split(' ')[2]);
                int centerX = (int)(Math.Round(xx, 0) * scalevalue);
                int centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(yy, 0) * scalevalue);

                int radius = (int)(5 * scalevalue);
                Point center = new Point(centerX, centerY);
                Pen pen = new Pen(Color.Red)
                {
                    Width = 4
                };
                if (currentmapgroup == MGPMG)
                {
                    pen.Color = Color.LimeGreen;
                }
                getCircle(e.Graphics, pen, center, radius);
                
            }
        }
        private void LoadeMapGroupPosTreeview()
        {
            treeViewMS2.Nodes.Clear();
            TreeNode rootNode = new TreeNode(Path.GetFileNameWithoutExtension(currentproject.mapgrouppos.Filename))
            {
                Tag = "MapGrouPosParent"
            };
            foreach(mapGroup MGPM in mapgroupposmap.group)
            {
                if (!rootNode.Nodes.ContainsKey(MGPM.name))
                {
                    rootNode.Nodes.Add(new TreeNode(MGPM.name)
                    {
                        Name = MGPM.name
                    });
                }
                rootNode.Nodes[MGPM.name].Nodes.Add(new TreeNode(MGPM.name) { Tag = MGPM });
            }
            treeViewMS2.Nodes.Add(rootNode);
            treeViewMS2.Sort();
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (e is MouseEventArgs mouseEventArgs)
            {
                decimal scalevalue = mapgroupposMapscale * (decimal)0.05;
                decimal mapsize = currentproject.MapSize;
                int newsize = (int)(mapsize * scalevalue);
                PointF pC = new PointF((float)Decimal.Round((decimal)(mouseEventArgs.X / scalevalue), 4), (float)Decimal.Round((decimal)((newsize - mouseEventArgs.Y) / scalevalue), 4));
                foreach (mapGroup MGPMG in mapgroupposmap.group)
                {
                    PointF pP = new PointF(Convert.ToSingle(MGPMG.pos.Split(' ')[0]), Convert.ToSingle(MGPMG.pos.Split(' ')[2]));
                    if (IsWithinCircle(pC, pP, (float)5))
                    {
                        foreach (TreeNode node in treeViewMS2.Nodes)
                        {
                            foreach (TreeNode nnode in node.Nodes)
                            {
                                foreach (TreeNode nnnodes in nnode.Nodes)
                                {
                                    if (nnnodes.Tag.Equals(MGPMG))
                                    {
                                        currentmapgroup = MGPMG;
                                        treeViewMS2.SelectedNode = nnnodes;
                                        currentmapgrouptreenode = nnnodes;
                                        treeViewMS2.Focus();
                                        pictureBox4.Invalidate();
                                        return;
                                    }
                                }
                            }
                        }

                    }
                }
            }
        }
        private void pictureBox4_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _mouseLastPosition = e.Location;
            }
        }
        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {
            if (pictureBox4.Focused == false)
            {
                pictureBox4.Focus();
                panelEx1.AutoScrollPosition = _newscrollPosition;
                pictureBox4.Invalidate();
            }
        }
        private void pictureBox4_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point changePoint = new Point(e.Location.X - _mouseLastPosition.X, e.Location.Y - _mouseLastPosition.Y);
                _newscrollPosition = new Point(-panelEx1.AutoScrollPosition.X - changePoint.X, -panelEx1.AutoScrollPosition.Y - changePoint.Y);
                if (_newscrollPosition.X <= 0)
                    _newscrollPosition.X = 0;
                if (_newscrollPosition.Y <= 0)
                    _newscrollPosition.Y = 0;
                panelEx1.AutoScrollPosition = _newscrollPosition;
                pictureBox4.Invalidate();
            }
            decimal scalevalue = mapgroupposMapscale * (decimal)0.05;
            decimal mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            label44.Text = Decimal.Round((decimal)(e.X / scalevalue), 4) + "," + Decimal.Round((decimal)((newsize - e.Y) / scalevalue), 4);
        }
        private void pictureBox4_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                pictureBox4_ZoomOut();
            }
            else
            {
                pictureBox4_ZoomIn();
            }

        }
        private void pictureBox4_ZoomIn()
        {
            int oldpictureboxhieght = pictureBox4.Height;
            int oldpitureboxwidht = pictureBox4.Width;
            Point oldscrollpos = panelEx1.AutoScrollPosition;
            int tbv = trackBar2.Value;
            int newval = tbv + 1;
            if (newval >= 20)
                newval = 20;
            trackBar2.Value = newval;
            mapgroupposMapscale = trackBar2.Value;
            SetsMapGroupposScale();
            if (pictureBox4.Height > panelEx1.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox4.Height * newy);
                _newscrollPosition.Y = y * -1;
                panelEx1.AutoScrollPosition = _newscrollPosition;
            }
            if (pictureBox4.Width > panelEx1.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox4.Width * newy);
                _newscrollPosition.X = x * -1;
                panelEx1.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox4.Invalidate();
        }
        private void pictureBox4_ZoomOut()
        {
            int oldpictureboxhieght = pictureBox4.Height;
            int oldpitureboxwidht = pictureBox4.Width;
            Point oldscrollpos = panelEx1.AutoScrollPosition;
            int tbv = trackBar2.Value;
            int newval = tbv - 1;
            if (newval <= 1)
                newval = 1;
            trackBar2.Value = newval;
            mapgroupposMapscale = trackBar2.Value;
            SetsMapGroupposScale();
            if (pictureBox4.Height > panelEx1.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox4.Height * newy);
                _newscrollPosition.Y = y * -1;
                panelEx1.AutoScrollPosition = _newscrollPosition;
            }
            if (pictureBox4.Width > panelEx1.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox4.Width * newy);
                _newscrollPosition.X = x * -1;
                panelEx1.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox4.Invalidate();
        }
        private void SetsMapGroupposScale()
        {

            decimal scalevalue = mapgroupposMapscale * (decimal)0.05;
            decimal mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            pictureBox4.Size = new Size(newsize, newsize);
        }
        private void treeViewMS2_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            currentmapgrouptreenode = e.Node;
            currentmapgroup = e.Node.Tag as mapGroup;
            pictureBox4.Invalidate();
            if (e.Button == MouseButtons.Right)
            {
                if(e.Node.Tag is mapGroup)
                    mapgroupposcontextMenu.Show(Cursor.Position);
            }
        }
        private void trackBar2_MouseUp(object sender, MouseEventArgs e)
        {
            mapgroupposMapscale = trackBar2.Value;
            SetsMapGroupposScale();
        }
        private void removeMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mapgroupposmap.group.Remove(currentmapgroup);
            TreeNode Parent = currentmapgrouptreenode.Parent;
            treeViewMS2.Nodes.Remove(currentmapgrouptreenode);
            if (Parent.Nodes.Count == 0)
                treeViewMS2.Nodes.Remove(Parent);
            currentproject.mapgrouppos.isDirty = true;
            pictureBox4.Invalidate();
        }
        #endregion mapgrouppos
    }
}
