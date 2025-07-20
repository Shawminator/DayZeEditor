using DarkUI.Forms;
using DayZeLib;
using SevenZipExtractor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace DayZeEditor
{
    public partial class TerjeManager : DarkForm
    {
        public Project currentproject { get; set; }
        public TypesFile vanillatypes;
        public List<TypesFile> ModTypes;
        public string Projectname;
        private bool useraction = false;
        public string TerjeSettingsPath { get; set; }
        public BindingList<TerjeCFGFIle> cfgfiles;
        public TerjeRecipes TerjeCraftingFiles;
        public BindingList<TerjeProtectionIDs> protectionIDsList;
        public TerjeLoadouts TerjeLoadouts;
        public TerjeRespawns TerjeRespawns;
        public TerjeFaces TerjeFaces;
        public TerjeGeneral TerjeGeneral;
        public TerjeScriptableAreas TerjeScriptableAreas;
        public MapData MapData { get; private set; }
        public TerjeCFGFIle currentCFGFIle { get; set; }
        public TerjeCFGLine currentCFGline { get; set; }
        private Dictionary<string, List<ComboBoxItem>> perkMap = new Dictionary<string, List<ComboBoxItem>>();

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        public TerjeManager()
        {
            InitializeComponent();
        }
        private void TerjeManager_Load(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = "";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Projectname = currentproject.ProjectName;
            vanillatypes = currentproject.getvanillatypes();
            ModTypes = currentproject.getModList();

            useraction = false;

            doubleClickTimer.Interval = 100;
            doubleClickTimer.Tick += new EventHandler(doubleClickTimer_Tick);

            LoadSkillsAndPerksFromSingleFile();
            PopulateSkillsCheckBoxes(SAFLP);
            //loading cfg files.
            cfgfiles = new BindingList<TerjeCFGFIle>();
            TerjeSettingsPath = currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\TerjeSettings\\";
            DirectoryInfo d = new DirectoryInfo(TerjeSettingsPath); //Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.cfg"); //Getting Text files
            foreach(FileInfo info in Files)
            {
                Console.Write("serializing " + Path.GetFileName(info.FullName));
                TerjeCFGFIle newcfgfile = new TerjeCFGFIle()
                {
                    Filename = info.FullName
                };
                newcfgfile.Read(info.FullName);
                if (newcfgfile != null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("  OK....");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                cfgfiles.Add(newcfgfile);
            }
            if (File.Exists(TerjeSettingsPath + "CustomCrafting\\Recipes.xml"))
            {
                Console.Write("serializing " + Path.GetFileName(TerjeSettingsPath + "CustomCrafting\\Recipes.xml"));
                string xml = File.ReadAllText(TerjeSettingsPath + "CustomCrafting\\Recipes.xml");
                TerjeCraftingFiles = Helper.DeserializeWithDebug<TerjeRecipes>(xml);
                TerjeCraftingFiles.Filename = TerjeSettingsPath + "CustomCrafting\\Recipes.xml";

                if (TerjeCraftingFiles != null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("  OK....");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            if (Directory.Exists(TerjeSettingsPath + "CustomProtection"))
            {
                DirectoryInfo dir = new DirectoryInfo(TerjeSettingsPath + "CustomProtection"); //Assuming Test is your Folder
                FileInfo[] protectionfiles = dir.GetFiles("*.txt"); //Getting Text files
                protectionIDsList = new BindingList<TerjeProtectionIDs>();
                foreach (FileInfo info in protectionfiles)
                {
                    Console.Write("serializing " + Path.GetFileName(info.FullName));
                    TerjeProtectionIDs newids = new TerjeProtectionIDs(info.FullName);
                    protectionIDsList.Add(newids);
                    if (newids != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("  OK....");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
            if (Directory.Exists(TerjeSettingsPath + "StartScreen"))
            {
                if (File.Exists(TerjeSettingsPath + "StartScreen\\Loadouts.xml"))
                {
                    Console.Write("serializing " + Path.GetFileName(TerjeSettingsPath + "StartScreen\\Loadouts.xml"));
                    string xml = File.ReadAllText(TerjeSettingsPath + "StartScreen\\Loadouts.xml");
                    TerjeLoadouts = Helper.DeserializeWithDebug<TerjeLoadouts>(xml);
                    TerjeLoadouts.Filename = TerjeSettingsPath + "StartScreen\\Loadouts.xml";

                    if (TerjeLoadouts != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("  OK....");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                if (File.Exists(TerjeSettingsPath + "StartScreen\\Respawns.xml"))
                {
                    Console.Write("serializing " + Path.GetFileName(TerjeSettingsPath + "StartScreen\\Respawns.xml"));
                    string xml = File.ReadAllText(TerjeSettingsPath + "StartScreen\\Respawns.xml");
                    TerjeRespawns = Helper.DeserializeWithDebug<TerjeRespawns>(xml);
                    TerjeRespawns.Filename = TerjeSettingsPath + "StartScreen\\Respawns.xml";
                    if (TerjeRespawns != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("  OK....");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                if (File.Exists(TerjeSettingsPath + "StartScreen\\Faces.xml"))
                {
                    Console.Write("serializing " + Path.GetFileName(TerjeSettingsPath + "StartScreen\\Faces.xml"));
                    XmlSerializer mySerializer = new XmlSerializer(typeof(TerjeFaces));
                    using (StreamReader reader = new StreamReader(TerjeSettingsPath + "StartScreen\\Faces.xml"))
                    {
                        TerjeFaces = (TerjeFaces)mySerializer.Deserialize(reader);
                        TerjeFaces.Filename = TerjeSettingsPath + "StartScreen\\Faces.xml";
                    }
                    if (TerjeFaces != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("  OK....");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                if (File.Exists(TerjeSettingsPath + "StartScreen\\General.xml"))
                {
                    Console.Write("serializing " + Path.GetFileName(TerjeSettingsPath + "StartScreen\\General.xml"));
                    XmlSerializer mySerializer = new XmlSerializer(typeof(TerjeGeneral));
                    using (StreamReader reader = new StreamReader(TerjeSettingsPath + "StartScreen\\General.xml"))
                    {
                        TerjeGeneral = (TerjeGeneral)mySerializer.Deserialize(reader);
                        TerjeGeneral.Filename = TerjeSettingsPath + "StartScreen\\General.xml";
                    }
                    if (TerjeGeneral != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("  OK....");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
            if (Directory.Exists(TerjeSettingsPath + "ScriptableAreas"))
            {
                if (File.Exists(TerjeSettingsPath + "ScriptableAreas\\ScriptableAreasSpawner.xml"))
                {
                    Console.Write("serializing " + Path.GetFileName(TerjeSettingsPath + "ScriptableAreas\\ScriptableAreasSpawner.xml"));
                    XmlSerializer mySerializer = new XmlSerializer(typeof(TerjeScriptableAreas));
                    using (StreamReader reader = new StreamReader(TerjeSettingsPath + "ScriptableAreas\\ScriptableAreasSpawner.xml"))
                    {
                        TerjeScriptableAreas = (TerjeScriptableAreas)mySerializer.Deserialize(reader);
                        TerjeScriptableAreas.Filename = TerjeSettingsPath + "ScriptableAreas\\ScriptableAreasSpawner.xml";
                    }
                    if (TerjeScriptableAreas != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("  OK....");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }

            SetToolTips();
            LoadCFGtoTreeview();
            MapData = new MapData(Application.StartupPath + currentproject.MapPath + ".xyz", currentproject.MapSize);
            pictureBox1.BackgroundImage = Image.FromFile(Application.StartupPath + currentproject.MapPath); // Livonia maop size is 12800 x 12800, 0,0 bottom left, center 6400 x 6400
            pictureBox1.Size = new Size(currentproject.MapSize, currentproject.MapSize);
            pictureBox1.Paint += new PaintEventHandler(DrawSpawns);
            trackBar1.Value = 1;
            SetSpawnScale();
            useraction = true;
        }
        
        public int terjeSpawnScale = 1;
        private Point _mouseLastPosition;
        private Point _newscrollPosition;
        private Rectangle doubleClickRectangle = new Rectangle();
        private Timer doubleClickTimer = new Timer();
        private bool isFirstClick = true;
        private bool isDoubleClick = false;
        private int milliseconds = 0;
        private MouseEventArgs mouseeventargs;
        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            terjeSpawnScale = trackBar1.Value;
            SetSpawnScale();
        }
        private void SetSpawnScale()
        {
            float scalevalue = terjeSpawnScale * 0.05f;
            float mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            pictureBox1.Size = new Size(newsize, newsize);
        }
        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }
        private void DrawSpawns(object sender, PaintEventArgs e)
        {
            if (checkBox9.Checked)
            {
                if (currentTreeNode.Tag is TerjeRespawnPoint)
                {
                    foreach (TerjeRespawnPoint point in currentTreeNode.Parent.Tag as BindingList<TerjeRespawnPoint>)
                    {
                        float scalevalue = terjeSpawnScale * 0.05f;
                        int centerX = 0;
                        int centerY = 0;
                        int eventradius = (int)(Math.Round(5f, 0) * scalevalue);
                        if (point.posSpecified)
                        {
                            centerX = (int)(Math.Round(Convert.ToSingle(point.pos.Split(',')[0])) * scalevalue);
                            centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(Convert.ToSingle(point.pos.Split(',')[1]), 0) * scalevalue);

                        }
                        else
                        {

                        }
                        Point center = new Point(centerX, centerY);
                        Pen pen = new Pen(Color.Red, 4);
                        if (currentTreeNode.Tag as TerjeRespawnPoint == point)
                            pen.Color = Color.LimeGreen;
                        else
                            pen.Color = Color.Red;
                        getCircle(e.Graphics, pen, center, eventradius);
                    }
                }
                else if (currentTreeNode.Parent.Tag is TerjeScriptableArea)
                {
                    TerjeScriptableArea area = currentTreeNode.Parent.Tag as TerjeScriptableArea;
                    foreach(TerjeScriptableArea areas in TerjeScriptableAreas.Area)
                    {
                        float scalevalue = terjeSpawnScale * 0.05f;
                        int centerX = 0;
                        int centerY = 0;
                        int eventradius = 0;
                        if (areas.Data.OuterRadiusSpecified)
                            eventradius = (int)(areas.Data.OuterRadius * scalevalue);
                        else
                            eventradius = (int)(areas.Data.Radius * scalevalue);
                        centerX = (int)(Math.Round(areas.PositionVec3.X) * scalevalue);
                        centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(areas.PositionVec3.Z) * scalevalue);
                        Point center = new Point(centerX, centerY);
                        Pen pen = new Pen(Color.Red, 2);
                        if (area == areas)
                            pen.Color = Color.LimeGreen;
                        else
                            pen.Color = Color.Red;
                        getCircle(e.Graphics, pen, center, eventradius);
                    }
                }
            }
            else
            {
                if (currentTreeNode.Tag is TerjeRespawnPoint)
                {
                    TerjeRespawnPoint point = currentTreeNode.Tag as TerjeRespawnPoint;
                    float scalevalue = terjeSpawnScale * 0.05f;
                    int centerX = 0;
                    int centerY = 0;
                    int eventradius = (int)(Math.Round(5f, 0) * scalevalue);
                    if (point.posSpecified)
                    {
                        centerX = (int)(Math.Round(Convert.ToSingle(point.pos.Split(',')[0])) * scalevalue);
                        centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(Convert.ToSingle(point.pos.Split(',')[1]), 0) * scalevalue);

                    }
                    else
                    {

                    }
                    Point center = new Point(centerX, centerY);
                    Pen pen = new Pen(Color.Red, 4);
                    if (currentTreeNode.Tag as TerjeRespawnPoint == point)
                        pen.Color = Color.LimeGreen;
                    else
                        pen.Color = Color.Red;
                    getCircle(e.Graphics, pen, center, eventradius);
                }
                else if (currentTreeNode.Parent.Tag is TerjeScriptableArea)
                {
                    TerjeScriptableArea area = currentTreeNode.Parent.Tag as TerjeScriptableArea;

                    float scalevalue = terjeSpawnScale * 0.05f;
                    int centerX = 0;
                    int centerY = 0;
                    int eventradius = 0;
                    if (area.Data.OuterRadiusSpecified)
                        eventradius = (int)(area.Data.OuterRadius * scalevalue);
                    else
                        eventradius = (int)(area.Data.Radius * scalevalue);
                    centerX = (int)(Math.Round(area.PositionVec3.X) * scalevalue);
                    centerY = (int)(currentproject.MapSize * scalevalue) - (int)(Math.Round(area.PositionVec3.Z) * scalevalue);
                    Point center = new Point(centerX, centerY);
                    Pen pen = new Pen(Color.LimeGreen, 4);
                    getCircle(e.Graphics, pen, center, eventradius);
                }
            }
        }
        private void getCircle(Graphics drawingArea, Pen penToUse, Point center, int radius)
        {
            Rectangle rect = new Rectangle(center.X - 1, center.Y - 1, 2, 2);
            drawingArea.DrawEllipse(penToUse, rect);
            Rectangle rect2 = new Rectangle(center.X - radius, center.Y - radius, radius * 2, radius * 2);
            drawingArea.DrawEllipse(penToUse, rect2);
        }
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            if (pictureBox1.Focused == false)
            {
                pictureBox1.Focus();
                panelEx1.AutoScrollPosition = _newscrollPosition;
                pictureBox1.Invalidate();
            }
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
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
                    if (currentTreeNode.Tag is TerjeRespawnPoint)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        TerjeRespawnPoint spoint = currentTreeNode.Tag as TerjeRespawnPoint;
                        decimal scalevalue = terjeSpawnScale * (decimal)0.05;
                        decimal mapsize = currentproject.MapSize;
                        int newsize = (int)(mapsize * scalevalue);
                        if (spoint.posSpecified)
                        {
                            spoint.pos = ((float)(Decimal.Round((decimal)(mouseeventargs.X / scalevalue), 4))).ToString() + "," + ((float)(Decimal.Round((decimal)((newsize - mouseeventargs.Y) / scalevalue), 4))).ToString();
                            currentTreeNode.Text = spoint.pos;
                        }
                        else
                        {
                            spoint.x = (Decimal.Round((decimal)(mouseeventargs.X / scalevalue), 4));
                            spoint.z = (Decimal.Round((decimal)((newsize - mouseeventargs.Y) / scalevalue), 4));
                            if (spoint.ySpecified && MapData.FileExists)
                            {
                                spoint.y = (decimal)(MapData.gethieght((float)spoint.x, (float)spoint.z));
                            }
                        }

                        Cursor.Current = Cursors.Default;
                        TerjeRespawns.isDirty = true;
                        pictureBox1.Invalidate();
                    }
                    else if (currentTreeNode.Parent.Tag is TerjeScriptableArea)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        TerjeScriptableArea area = currentTreeNode.Parent.Tag as TerjeScriptableArea;
                        decimal scalevalue = terjeSpawnScale * (decimal)0.05;
                        decimal mapsize = currentproject.MapSize;
                        int newsize = (int)(mapsize * scalevalue);
                        area.PositionVec3.X = (float)(Decimal.Round((decimal)(mouseeventargs.X / scalevalue), 4));
                        area.PositionVec3.Z = (float)(Decimal.Round((decimal)((newsize - mouseeventargs.Y) / scalevalue), 4));
                        if (MapData.FileExists)
                        {
                            area.PositionVec3.Y = (float)((decimal)(MapData.gethieght(area.PositionVec3.X, area.PositionVec3.Z)));
                        }

                        Cursor.Current = Cursors.Default;
                        TerjeScriptableAreas.isDirty = true;
                        pictureBox1.Invalidate();
                    }
                }
                else
                {
                    decimal scalevalue = terjeSpawnScale * (decimal)0.05;
                    decimal mapsize = currentproject.MapSize;
                    int newsize = (int)(mapsize * scalevalue);
                    PointF pC = new PointF((float)Decimal.Round((decimal)(mouseeventargs.X / scalevalue), 4), (float)Decimal.Round((decimal)((newsize - mouseeventargs.Y) / scalevalue), 4));
                    if (currentTreeNode.Tag is TerjeRespawnPoint)
                    {
                        TerjeRespawnPoint point = currentTreeNode.Tag as TerjeRespawnPoint;
                        foreach (TerjeRespawnPoint waypoint in currentTreeNode.Parent.Tag as BindingList<TerjeRespawnPoint>)
                        {
                            float centerX = 0;
                            float centerY = 0;
                            if (point.posSpecified)
                            {
                                centerX = Convert.ToSingle(waypoint.pos.Split(',')[0]);
                                centerY = Convert.ToSingle(waypoint.pos.Split(',')[1]);

                            }
                            else
                            {
                                centerX = Convert.ToSingle(waypoint.x);
                                centerY = Convert.ToSingle(waypoint.y);
                            }
                            PointF pP = new PointF(centerX, centerY);
                            if (IsWithinCircle(pC, pP, (float)5))
                            {
                                foreach (TreeNode node in currentTreeNode.Parent.Nodes)
                                {
                                    if (node.Tag as TerjeRespawnPoint == waypoint)
                                    {
                                        TerjeTV.SelectedNode = node;
                                    }
                                }

                            }
                            pictureBox1.Invalidate();
                            continue;
                        }
                    }
                    else if (currentTreeNode.Parent.Tag is TerjeScriptableArea)
                    {
                        foreach (TerjeScriptableArea waypoints in TerjeScriptableAreas.Area)
                        {
                            PointF pP = new PointF((float)waypoints.PositionVec3.X, (float)waypoints.PositionVec3.Z);
                            int eventradius = 0;
                            if (waypoints.Data.OuterRadiusSpecified)
                                eventradius = (int)(waypoints.Data.OuterRadius);
                            else
                                eventradius = (int)(waypoints.Data.Radius);
                            if (IsWithinCircle(pC, pP, (float)eventradius))
                            {
                                foreach (TreeNode node in currentTreeNode.Parent.Parent.Nodes)
                                {
                                    if (node.Tag as TerjeScriptableArea == waypoints)
                                    {
                                        TerjeTV.SelectedNode = node.Nodes[0];
                                    }
                                }
                                pictureBox1.Invalidate();
                                continue;
                            }
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
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
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
                pictureBox1.Invalidate();
            }
            decimal scalevalue = terjeSpawnScale * (decimal)0.05;
            decimal mapsize = currentproject.MapSize;
            int newsize = (int)(mapsize * scalevalue);
            label2.Text = Decimal.Round((decimal)(e.X / scalevalue), 4) + "," + Decimal.Round((decimal)((newsize - e.Y) / scalevalue), 4);
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
            Point oldscrollpos = panelEx1.AutoScrollPosition;
            int tbv = trackBar1.Value;
            int newval = tbv + 1;
            if (newval >= 20)
                newval = 20;
            trackBar1.Value = newval;
            terjeSpawnScale = trackBar1.Value;
            SetSpawnScale();
            if (pictureBox1.Height > panelEx1.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox1.Height * newy);
                _newscrollPosition.Y = y * -1;
                panelEx1.AutoScrollPosition = _newscrollPosition;
            }
            if (pictureBox1.Width > panelEx1.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox1.Width * newy);
                _newscrollPosition.X = x * -1;
                panelEx1.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox1.Invalidate();
        }
        private void pictureBox1_ZoomOut()
        {
            int oldpictureboxhieght = pictureBox1.Height;
            int oldpitureboxwidht = pictureBox1.Width;
            Point oldscrollpos = panelEx1.AutoScrollPosition;
            int tbv = trackBar1.Value;
            int newval = tbv - 1;
            if (newval <= 1)
                newval = 1;
            trackBar1.Value = newval;
            terjeSpawnScale = trackBar1.Value;
            SetSpawnScale();
            if (pictureBox1.Height > panelEx1.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(pictureBox1.Height * newy);
                _newscrollPosition.Y = y * -1;
                panelEx1.AutoScrollPosition = _newscrollPosition;
            }
            if (pictureBox1.Width > panelEx1.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(pictureBox1.Width * newy);
                _newscrollPosition.X = x * -1;
                panelEx1.AutoScrollPosition = _newscrollPosition;
            }
            pictureBox1.Invalidate();
        }

        private void SetToolTips()
        {
            var toolTips = new Dictionary<Control, string>
            {
                { label11, "(required) skill identifier (you can see all skills identifiers in config.cpp CfgTerjeSkills section." },
                { label24, "(required) perk identifier (you can see all perks identifiers in config.cpp CfgTerjeSkills section." },
                { label22, "(required) perk level must be equal to or higher than this value to have access to this loadout/respawn/recipe." },
                { label37, "(required) loadout identifier,\nMust be short and unique for each individual loadout." },
                { label36, "(required) name of loadout that the player will see in the game UI.\nCan be used key from stringtable.csv for localication to all supported languages." },
                { label39, "(required) item classname" },
                { label40, "(optional) overrides the name of the item when used in the selection menu" },
                { label42, "(optional) quantity of item, number from 0 to 1 (where 1 is 100%, 0.5 - 50%, etc). \nCan be used as a range \"0.0:1.0\" for random result between 2 values." },
                { label41, "(optional) items count in the stack.\nCan be used as a range \"10:20\" for random result between 2 values."},
                { label46, "(optional) health of item, number from 0 to 1 (where 1 is 100%, 0.5 - 50%, etc).\nCan be used as a range \"0.0:1.0\" for random result between 2 values."},
                { label45, "(optional) spawning position:\r\n\"@Attachment\" or name of the attachment slot in which the item will be spawned.\r\n\"@Magazine\" to spawn as weapon magazine.\r\n\"@InHands\" to spawn in player hands.\r\n\"@Cargo\" to spawn item in first empty place in cargo.\r\n\"@Cargo:0:3:h\" to spawn in specific place in cargo (col and row position with v(vertical) or h(horizontal) orientation)"},
                { label44 , "(optional) liquid classname from vanilla 'cfgLiquidDefinitions' or from modded 'CfgTerjeCustomLiquids'"},
                { label43 , "(optional) item temperature in degrees Celsius.\nCan be used as a range \"36.6:41.0\" for random result between 2 values."},
                { label53, "(optional) the stage of the food: \nRAW\nBAKED\nBOILED\nDRIED\nBURNEDz\nROTTEN" },
                { label52, "(optional) disinfection (sterility) condition of item: 0 - no, 1 - yes." },
                { label51, "(optional) list of agents separated by comma: \nCHOLERA, INFLUENZA, SALMONELLA, BRAIN, FOOD_POISON, CHEMICAL_POISON, WOUND_AGENT, NERVE_AGENT, HEAVYMETAL" },
                { label50, "(optional) sets the item to quickbar slot (number from 0 to 9)" },
                { label49, "(optional) sets the ammo type to spawn inside a magazine or weapon." },
                { label48, "(optional) sets the amount of ammo to be spawned inside a magazine or weapon.\nCan be used as a range \"10:20\" for random result between 2 values." },
                { label47, "(optional) sets the number of points as a cost for this item,\nIf used inside a selector with a points." }
            };

            ToolTip toolTip = new ToolTip();

            foreach (var pair in toolTips)
            {
                toolTip.SetToolTip(pair.Key, pair.Value);
            }

        }
        private void LoadSkillsAndPerksFromSingleFile()
        {
            var skills = new List<ComboBoxItem>();
            perkMap.Clear();
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TraderNPCs", "skills_and_perks.txt");

            foreach (var line in File.ReadAllLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split('|');
                if (parts.Length < 2) continue;

                string skillDisplay = parts[0].Trim();
                string skillValue = parts[1].Trim();

                // Add to skills list
                var skillItem = new ComboBoxItem { Display = skillDisplay, Value = skillValue };
                skills.Add(skillItem);

                // Parse perks if available
                if (parts.Length >= 3)
                {
                    var perksList = new List<ComboBoxItem>();
                    var perkItems = parts[2].Split(',');

                    foreach (var perk in perkItems)
                    {
                        var perkParts = perk.Split(':');
                        if (perkParts.Length == 2)
                        {
                            perksList.Add(new ComboBoxItem
                            {
                                Display = perkParts[0].Trim(),
                                Value = perkParts[1].Trim()
                            });
                        }
                    }

                    perkMap[skillValue] = perksList;
                }
            }

            // Assign to ComboBoxes
            CRSLskillIdCB.DataSource = new List<ComboBoxItem>(skills);
            CRSPskillIDCB.DataSource = new List<ComboBoxItem>(skills);
        }
        private void PopulateSkillsCheckBoxes(FlowLayoutPanel groupBox)
        {
            // Clear existing controls
            groupBox.Controls.Clear();

            // Path to your external skills file
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TraderNPCs", "skills_and_perks.txt");

            if (!File.Exists(filePath))
            {
                MessageBox.Show("Skills file not found: " + filePath);
                return;
            }

            string[] lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                // Format expected: Display|Value|Perk1|Perk2|...
                var parts = line.Split('|');
                if (parts.Length < 2) continue;

                string display = parts[0];
                string value = parts[1];

                CheckBox checkBox = new CheckBox
                {
                    Text = display,
                    Tag = value, // store the skill ID for later use
                };
                // Attach the event handler
                checkBox.CheckedChanged += SkillsCheckBox_CheckedChanged;
                groupBox.Controls.Add(checkBox);
            }
        }

        private void LoadCFGtoTreeview()
        {
            TerjeTV.Nodes.Clear();
            TreeNode RootNode = new TreeNode("Treje Manager")
            {
                Tag = "Root"
            };
            CreateCFGNodes(RootNode);
            CreateCraftingNodes(RootNode);
            CreateProtectionsNodes(RootNode);
            CrteatescriptableNodes(RootNode);
            CreateStartScreenNodes(RootNode);

            TerjeTV.Nodes.Add(RootNode);
        }
        private void CreateStartScreenNodes(TreeNode RootNode)
        {
            TreeNode StartScreenNode = new TreeNode("Start Screen")
            {
                Tag = "StartScreen"
            };
            CreateFaceNode(StartScreenNode);
            CreateGeneralNodes(StartScreenNode);
            CreateLoadOutNodes(StartScreenNode);
            CreateSpawnNodes(StartScreenNode);
            RootNode.Nodes.Add(StartScreenNode);
        }
        private void CreateCFGNodes(TreeNode RootNode)
        {
            if (cfgfiles.Count > 0)
            {
                TreeNode CFGNodes = new TreeNode("CFG Files")
                {
                    Tag = "CFGFiles"
                };
                foreach (TerjeCFGFIle cfgfile in cfgfiles)
                {
                    CFGNodes.Nodes.Add(GetCfgnodes(cfgfile));
                }
                RootNode.Nodes.Add(CFGNodes);
            }
        }
        private void CreateCraftingNodes(TreeNode RootNode)
        {
            TreeNode TerjeCraftingNodes = new TreeNode("Crafting Recipes")
            {
                Tag = "TerjeCrafting"
            };
            if (TerjeCraftingFiles != null)
            {
                foreach (Object tc in TerjeCraftingFiles.Items)
                {
                    if (tc is TerjeRecipe)
                        TerjeCraftingNodes.Nodes.Add(GetRecipeNodes(tc as TerjeRecipe));
                }
            }
            RootNode.Nodes.Add(TerjeCraftingNodes);
        }
        private void CreateProtectionsNodes(TreeNode RootNode)
        {
            TreeNode Protectionnode = new TreeNode("Protection")
            {
                Tag = "Protectionnode"
            };
            if (protectionIDsList != null && protectionIDsList.Count > 0)
            {
                foreach (TerjeProtectionIDs tc in protectionIDsList)
                {
                    Protectionnode.Nodes.Add(GetProtectionnodes(tc));
                }
            }
            RootNode.Nodes.Add(Protectionnode);
        }
        private void CreateFaceNode(TreeNode rootNode)
        {
            if (TerjeFaces != null)
            {
                TreeNode TerjeFacesNode = new TreeNode("Faces")
                {
                    Tag = TerjeFaces
                };
                foreach(TerjeFace face in TerjeFaces.Face)
                {
                    TreeNode faceNode = new TreeNode(face.classname)
                    {
                        Tag = face
                    };
                    if (face.Conditions != null)
                    {
                        TreeNode SpawnConditionsNode = new TreeNode("Conditions")
                        {
                            Tag = face.Conditions
                        };
                        getConditionNodes(face.Conditions, SpawnConditionsNode);
                        faceNode.Nodes.Add(SpawnConditionsNode);
                    }
                    TerjeFacesNode.Nodes.Add(faceNode);
                }
                rootNode.Nodes.Add(TerjeFacesNode);
            }
        }
        private void CreateGeneralNodes(TreeNode rootNode)
        {
            if (TerjeGeneral != null)
            {
                TreeNode TerjeGeneralNode = new TreeNode("General")
                {
                    Tag = TerjeGeneral
                };
                PropertyInfo[] properties = typeof(TerjeGeneral).GetProperties();
                foreach (var property in properties)
                {
                    if (property.Name == "Filename" || property.Name == "isDirty") continue;
                    var value = property.GetValue(TerjeGeneral);
                    if (value != null)
                    {
                        TreeNode propertyNode = new TreeNode(property.Name)
                        {
                            Tag = property.Name
                        };
                        if (value is TerjeValueString)
                        {
                            TerjeValueString tstring = (TerjeValueString)value;
                             propertyNode.Nodes.Add(new TreeNode(tstring.value)
                            {
                                Tag = tstring
                             });
                        }
                        TerjeGeneralNode.Nodes.Add(propertyNode);
                    }
                    else if (value != null)
                    {
                        Console.WriteLine($"Property {property.Name} has a null value.");
                    }
                }
                rootNode.Nodes.Add(TerjeGeneralNode);
            }
        }
        private void CreateLoadOutNodes(TreeNode RootNode)
        {
            if (TerjeLoadouts != null)
            {
                TreeNode TerjeLoadoutsNode = new TreeNode("Loadouts")
                {
                    Tag = TerjeLoadouts
                };
                foreach (TerjeLoadout ta in TerjeLoadouts.Loadout)
                {
                    TerjeLoadoutsNode.Nodes.Add(GetLoadoutNodes(ta));
                }
                RootNode.Nodes.Add(TerjeLoadoutsNode);
            }
        }
        private void CreateSpawnNodes(TreeNode RootNode)
        {
            if (TerjeRespawns != null)
            {
                TreeNode TerjeSpawnsNode = new TreeNode("Player Spawns")
                {
                    Tag = TerjeRespawns
                };
                foreach (TerjeRespawn ta in TerjeRespawns.Respawn)
                {
                    TerjeSpawnsNode.Nodes.Add(GetSpanNodes(ta));
                }
                RootNode.Nodes.Add(TerjeSpawnsNode);
            }
        }
        private void CrteatescriptableNodes(TreeNode RootNode)
        {
            if (TerjeScriptableAreas != null)
            {
                TreeNode ScriptableNode = new TreeNode("Scriptable Areas")
                {
                    Tag = TerjeScriptableAreas
                };
                foreach (TerjeScriptableArea ta in TerjeScriptableAreas.Area)
                {
                    ScriptableNode.Nodes.Add(getScriptablearea(ta));
                }
                RootNode.Nodes.Add(ScriptableNode);
            }
        }
        private TreeNode GetLoadoutNodes(TerjeLoadout ta)
        {
            TreeNode loadoutnode = new TreeNode(ta.displayName)
            {
                Tag = ta
            };
            if(ta.Items != null)
            {
                loadoutnode.Nodes.Add(GetLoadoutItems(ta));
            }
            if (ta.Conditions != null)
            {
                TreeNode ConditionsNode = new TreeNode("Conditions")
                {
                    Tag = ta.Conditions
                };
                getConditionNodes(ta.Conditions, ConditionsNode);
                loadoutnode.Nodes.Add(ConditionsNode);
            }

            return loadoutnode;
        }
        private TreeNode GetLoadoutItems(TerjeLoadout ta)
        {
            TreeNode itemsnode = new TreeNode("Items")
            {
                Tag = ta.Items
            };
            foreach (object item in ta.Items.Items)
            {
                if (item is TerjeLoadoutItem)
                {
                    itemsnode.Nodes.Add(GetLoadoutitem(item as TerjeLoadoutItem));
                }
                else if (item is TerjeLoadoutSelector)
                {
                    TerjeLoadoutSelector selector = item as TerjeLoadoutSelector;
                    itemsnode.Nodes.Add(GetloadoutSleector(selector));
                }
            }

            return itemsnode;
        }
        private static TreeNode GetloadoutSleector(TerjeLoadoutSelector selector)
        {
            TreeNode selectorNode = new TreeNode($"Selector Type:{selector.type} , Display Name:{selector.displayName}")
            {
                Tag = selector
            };
            if (selector.Item != null && selector.Item.Count > 0)
            {
                foreach (TerjeLoadoutItem litem in selector.Item)
                {
                    TreeNode itemnode = new TreeNode($"Classname:{litem.classname}")
                    {
                        Tag = litem
                    };
                    selectorNode.Nodes.Add(itemnode);
                }
            }
            if (selector.Group != null && selector.Group.Count > 0)
            {
                foreach (TerjeLoadoutGroup litem in selector.Group)
                {
                    selectorNode.Nodes.Add(GetLoadoutGroups(litem));
                }
            }

            return selectorNode;
        }
        private static TreeNode GetLoadoutGroups(TerjeLoadoutGroup litem)
        {
            TreeNode itemnode = new TreeNode($"Group")
            {
                Tag = litem
            };
            if (litem.Item != null && litem.Item.Count > 0)
            {
                foreach (TerjeLoadoutItem Gitem in litem.Item)
                {
                    TreeNode gitemnode = new TreeNode($"Classname:{Gitem.classname}")
                    {
                        Tag = Gitem
                    };
                    itemnode.Nodes.Add(gitemnode);
                }
            }

            return itemnode;
        }
        private TreeNode GetLoadoutitem(TerjeLoadoutItem item)
        {
            TerjeLoadoutItem litem = item as TerjeLoadoutItem;
            TreeNode itemnode = new TreeNode($"Classname:{litem.classname}")
            {
                Tag = item
            };
            if(litem.Item != null && litem.Item.Count > 0)
            {
                foreach(TerjeLoadoutItem iitem in litem.Item)
                {
                    itemnode.Nodes.Add(GetLoadoutitem(iitem));
                }
            }
            return itemnode;
            
        }
        private TreeNode GetSpanNodes(TerjeRespawn ta)
        {
            TreeNode Spawnnode = new TreeNode(ta.displayName)
            {
                Tag = ta
            };
            if(ta.Points.Count > 0)
            {
                TreeNode SpawnPointsNode = new TreeNode("Points")
                {
                    Tag = ta.Points
                };
                foreach(TerjeRespawnPoint point in ta.Points)
                {
                    TreeNode propertyNode = new TreeNode(point.pos)
                    {
                        Tag = point
                    };
                    SpawnPointsNode.Nodes.Add(propertyNode);
                }
                Spawnnode.Nodes.Add(SpawnPointsNode);
            }
            if(ta.Objects.Count > 0)
            {
                TreeNode SpawnObjectNode = new TreeNode("Objects")
                {
                    Tag = ta.Objects
                };
                foreach (TerjeRespawnObject objects in ta.Objects)
                {
                    TreeNode propertyNode = new TreeNode(objects.classname)
                    {
                        Tag = objects
                    };
                    SpawnObjectNode.Nodes.Add(propertyNode);
                }
                Spawnnode.Nodes.Add(SpawnObjectNode);
            }
            if(ta.DeathPoint != null)
            {
                TreeNode SpawnDeathPointNode = new TreeNode("Death Point")
                {
                    Tag = ta.DeathPoint
                };
                Spawnnode.Nodes.Add(SpawnDeathPointNode);
            }
            if (ta.Options != null)
            {
                TreeNode SpawnOptionsNode = new TreeNode("Options")
                {
                    Tag = ta.Options
                };
                PropertyInfo[] properties = typeof(TerjeRespawnOptions).GetProperties();
                foreach (var property in properties)
                {
                    var value = property.GetValue(ta.Options);
                    if (value != null)
                    {
                        TreeNode propertyNode = new TreeNode(property.Name)
                        {
                            Tag = value
                        };

                        SpawnOptionsNode.Nodes.Add(propertyNode);
                    }
                    else if (value != null)
                    {
                        Console.WriteLine($"Property {property.Name} has a null value.");
                    }
                }

                Spawnnode.Nodes.Add(SpawnOptionsNode);
            }
            if (ta.Conditions != null)
            {
                TreeNode SpawnConditionsNode = new TreeNode("Conditions")
                {
                    Tag = ta.Conditions
                };
                getConditionNodes(ta.Conditions, SpawnConditionsNode);
                Spawnnode.Nodes.Add(SpawnConditionsNode);
            }

            return Spawnnode;
        }
        private TreeNode getScriptablearea(TerjeScriptableArea ta)
        {
            TreeNode na = new TreeNode(ta.Classname)
            {
                Tag = ta
            };
            TreeNode pos = new TreeNode("Map View")
            {
                Tag = "MapView"
            };
            na.Nodes.Add(pos);
            na.Nodes.Add(new TreeNode("Data")
            {
                Tag = ta.Data
            });
            return na;
        }
        private TreeNode GetCfgnodes(TerjeCFGFIle cfgfile)
        {
            TreeNode CFGFile = new TreeNode(Path.GetFileName(cfgfile.Filename))
            {
                Tag = cfgfile
            };
            foreach(TerjeCFGLine line in cfgfile.CFGCOntents)
            {
                if (line.isComment)
                {
                    TreeNode node = new TreeNode($"{line.comment}")
                    {
                        Tag = line
                    };
                    CFGFile.Nodes.Add(node);
                }
                else
                {
                    TreeNode node = new TreeNode($"{line.cfgvariablename} = {line.cfgVariable.ToString()}")
                    {
                        Tag = line
                    };
                    CFGFile.Nodes.Add(node);
                }
            }
            return CFGFile;
        }
        private TreeNode GetRecipeNodes(TerjeRecipe recipe)
        {
            TreeNode recipeNode = new TreeNode(Path.GetFileName(recipe.DisplayName))
            {
                Tag = recipe
            };
            recipeNode.Nodes.Add(GetIngredientNodes(recipe.FirstIngredient, 0));
            recipeNode.Nodes.Add(GetIngredientNodes(recipe.SecondIngredient, 1));
            TreeNode resultsnode = new TreeNode("Crafting Results")
            {
                Tag = "CraftingResults"
            };
            foreach(TerjeCraftingResult cr in recipe.CraftingResults.Results)
            {
                resultsnode.Nodes.Add(new TreeNode(cr.ClassName)
                {
                    Tag = cr
                });
            }
            recipeNode.Nodes.Add(resultsnode);
            if(recipe.Conditions != null)
            {
                TreeNode ConditionsNode = new TreeNode("Conditions")
                {
                    Tag = recipe.Conditions
                };
                getConditionNodes(recipe.Conditions, ConditionsNode);
                recipeNode.Nodes.Add(ConditionsNode);
            }
            return recipeNode;
        }
        private static void getConditionNodes(TerjeConditions recipe, TreeNode ConditionsNode)
        {
            foreach(var condition in recipe.items)
            {
                if (condition is TerjeSpecificPlayers)
                {
                    ConditionsNode.Nodes.Add(GetSpecificPlayers(condition as TerjeSpecificPlayers));
                }
                else if (condition is TerjeSpecialConditionsAny || condition is TerjeSpecialConditionsOne || condition is TerjeSpecialConditionsAll)
                {
                    ConditionsNode.Nodes.Add(GetSpecialNodes(condition as TerjeSpecialConditions));
                }
                else
                {
                    ConditionsNode.Nodes.Add(new TreeNode(condition.GetType().Name)
                    {
                        Tag = condition
                    });
                }
            }
        }
        private static TreeNode GetSpecialNodes(TerjeSpecialConditions condition)
        {
            TreeNode AllNode = new TreeNode(condition.GetType().Name)
            {
                Tag = condition
            };
            foreach(var item in condition.Items)
            {
                AllNode.Nodes.Add(new TreeNode(item.GetType().Name)
                {
                    Tag = item
                });
            }

            return AllNode;
        }
        private static TreeNode GetSpecificPlayers(TerjeSpecificPlayers players)
        {
            TreeNode Playernode = new TreeNode("Specific Players")
            {
                Tag = players
            };
            foreach (TerjeSpecificPlayer sp in players.SpecificPlayer)
            {
                Playernode.Nodes.Add(new TreeNode(sp.steamGUID)
                {
                    Tag = sp
                });
            }

            return Playernode;
        }
        private TreeNode GetIngredientNodes(TerjeRecipeIngredient ingredient, int order)
        {
            string name = "";
            if (order == 0)
                name = "First Ingredient";
            else if (order == 1)
                name = "Second Ingredient";

            TreeNode Ingredientnode = new TreeNode(name)
            {
                Tag = ingredient
            };
            TreeNode Itemnode = new TreeNode("Items")
            {
                Tag = "Items"
            };
            foreach(string item in ingredient.Items)
            {
                Itemnode.Nodes.Add(new TreeNode(item)
                {
                    Tag = "IngredientItem"
                });
            }
            Ingredientnode.Nodes.Add(Itemnode);
            return Ingredientnode;
        }
        private TreeNode GetProtectionnodes(TerjeProtectionIDs protection)
        {
            TreeNode filenameNode = new TreeNode(protection.Filetype)
            {
                Tag = protection
            };
            foreach(string id in protection.IDList)
            {
                filenameNode.Nodes.Add(new TreeNode(id)
                {
                    Tag = "IDString"
                });
            }
            return filenameNode;
        }
        private void TerjeManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool savefiles = false;
            foreach (TerjeCFGFIle cfgf in cfgfiles)
            {
                if (cfgf.isDirty)
                {
                    savefiles = true;
                }
            }
            if (TerjeCraftingFiles != null && TerjeCraftingFiles.isDirty)
            {
                savefiles = true;
            }
            foreach (TerjeProtectionIDs prot in protectionIDsList)
            {
                if (prot.isDirty)
                {
                    savefiles = true;
                }
            }
            if (TerjeLoadouts != null && TerjeLoadouts.isDirty)
            {
                savefiles = true;
            }
            if (TerjeRespawns != null && TerjeRespawns.isDirty)
            {
                savefiles = true;
            }
            if (TerjeFaces != null && TerjeFaces.isDirty)
            {
                savefiles = true;
            }
            if (TerjeGeneral != null && TerjeGeneral.isDirty)
            {
                savefiles = true;
            }
            if (TerjeScriptableAreas != null && TerjeScriptableAreas.isDirty)
            {
                savefiles = true;
            }

            if (savefiles == true)
            {
                DialogResult dialogResult = MessageBox.Show("You have Unsaved Changes, do you wish to save", "Unsaved Changes found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Savefiles();
                }
            }
        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            Savefiles();
        }
        private void Savefiles()
        {
            List<string> midifiedfiles = new List<string>();
            string SaveTime = DateTime.Now.ToString("ddMMyy_HHmm");
            foreach(TerjeCFGFIle cfgf in cfgfiles)
            {
                if(cfgf.isDirty)
                {
                    cfgf.isDirty = false;
                    string[] newfile = cfgf.CreateStringArray();
                    File.WriteAllLines(cfgf.Filename, newfile);
                    midifiedfiles.Add(Path.GetFileName(cfgf.Filename));
                }
            }
            if (TerjeCraftingFiles != null && TerjeCraftingFiles.isDirty)
            {
                TerjeCraftingFiles.isDirty = false;
                var serializer = new XmlSerializer(typeof(TerjeRecipes));
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                var sw = new StringWriter();
                var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, });
                serializer.Serialize(xmlWriter, TerjeCraftingFiles, ns);
                File.WriteAllText(TerjeCraftingFiles.Filename, sw.ToString());
                midifiedfiles.Add(Path.GetFileName(TerjeCraftingFiles.Filename));
            }
            if (Directory.Exists(TerjeSettingsPath + "CustomProtection"))
            {
                foreach (TerjeProtectionIDs prot in protectionIDsList)
                {
                    if (prot.isDirty)
                    {
                        prot.isDirty = false;
                        File.WriteAllLines(prot.Filename, prot.IDList.ToArray());
                        midifiedfiles.Add(Path.GetFileName(prot.Filename));
                    }
                }
            }
            if (TerjeLoadouts != null && TerjeLoadouts.isDirty)
            {
                TerjeLoadouts.isDirty = false;
                var serializer = new XmlSerializer(typeof(TerjeLoadouts));
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                var sw = new StringWriter();
                var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, });
                serializer.Serialize(xmlWriter, TerjeLoadouts, ns);
                File.WriteAllText(TerjeLoadouts.Filename, sw.ToString());
                midifiedfiles.Add(Path.GetFileName(TerjeLoadouts.Filename));
            }
            if (TerjeRespawns != null && TerjeRespawns.isDirty)
            {
                TerjeRespawns.isDirty = false;
                var serializer = new XmlSerializer(typeof(TerjeRespawns));
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                var sw = new StringWriter();
                var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, });
                serializer.Serialize(xmlWriter, TerjeRespawns, ns);
                File.WriteAllText(TerjeRespawns.Filename, sw.ToString());
                midifiedfiles.Add(Path.GetFileName(TerjeRespawns.Filename));
            }
            if (TerjeFaces != null && TerjeFaces.isDirty)
            {
                TerjeFaces.isDirty = false;
                var serializer = new XmlSerializer(typeof(TerjeFaces));
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                var sw = new StringWriter();
                var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, });
                serializer.Serialize(xmlWriter, TerjeFaces, ns);
                File.WriteAllText(TerjeFaces.Filename, sw.ToString());
                midifiedfiles.Add(Path.GetFileName(TerjeFaces.Filename));
            }
            if (TerjeGeneral != null && TerjeGeneral.isDirty)
            {
                TerjeGeneral.isDirty = false;
                var serializer = new XmlSerializer(typeof(TerjeGeneral));
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                var sw = new StringWriter();
                var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, });
                serializer.Serialize(xmlWriter, TerjeGeneral, ns);
                File.WriteAllText(TerjeGeneral.Filename, sw.ToString());
                midifiedfiles.Add(Path.GetFileName(TerjeGeneral.Filename));
            }
            if (TerjeScriptableAreas != null && TerjeScriptableAreas.isDirty)
            {
                TerjeScriptableAreas.isDirty = false;
                var serializer = new XmlSerializer(typeof(TerjeScriptableAreas));
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                var sw = new StringWriter();
                var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true, });
                serializer.Serialize(xmlWriter, TerjeScriptableAreas, ns);
                File.WriteAllText(TerjeScriptableAreas.Filename, sw.ToString());
                midifiedfiles.Add(Path.GetFileName(TerjeScriptableAreas.Filename));
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
            Process.Start(currentproject.projectFullName + "\\" + currentproject.ProfilePath + "\\TerjeSettings");
        }
        public TreeNode currentTreeNode { get; set; }
        private void TerjeTV_AfterSelect(object sender, TreeViewEventArgs e)
        {
            currentTreeNode = e.Node;
            MapPanel.Visible = false;
            foreach (Control c in flowLayoutPanel1.Controls)
            {
                c.Visible = false;
            }
            StringTB.Visible = false;
            IntNUD.Visible = false;
            FloatNUD.Visible = false;
            BoolCB.Visible = false;
            SAFGB.Visible = false;
            useraction = false;

            if (e.Node.Tag.ToString() == "Root" || e.Node.Tag.ToString() == "CFGFiles")
            {

            }
            else if (e.Node.Tag is TerjeCFGFIle)
            {
                currentCFGFIle = e.Node.Tag as TerjeCFGFIle;
            }
            else if (e.Node.Tag is TerjeCFGLine)
            {
                currentCFGFIle = e.Node.Parent.Tag as TerjeCFGFIle;
                currentCFGline = e.Node.Tag as TerjeCFGLine;
                if (!currentCFGline.isComment)
                {
                    groupBox1.Visible = true;
                    CommentRTB.Text = currentCFGline.comment;
                    switch (currentCFGline.cfgvariabletype)
                    {
                        case "bool":
                            groupBox1.Text = "Bool";
                            BoolCB.Visible = true;
                            BoolCB.Checked = (bool)currentCFGline.cfgVariable;
                            break;
                        case "float":
                            groupBox1.Text = "Float";
                            FloatNUD.Visible = true;
                            FloatNUD.Value = (decimal)(float)currentCFGline.cfgVariable;
                            break;
                        case "int":
                            groupBox1.Text = "Int";
                            IntNUD.Visible = true;
                            IntNUD.Value = (int)currentCFGline.cfgVariable;
                            break;
                        case "string":
                            groupBox1.Text = "String";
                            StringTB.Visible = true;
                            StringTB.Text = currentCFGline.cfgVariable.ToString();
                            break;
                    }
                }
            }
            else if (e.Node.Tag is TerjeRecipe)
            {
                TerjeRecipe recipe = e.Node.Tag as TerjeRecipe;
                CRecipeGB.Visible = true;
                CRNameTB.Text = recipe.DisplayName;
                CREnabledCB.Checked = recipe.Enabled == 1 ? true : false;
                if (CRtimeSpecifiedCB.Checked = CRAnimationLengthNUD.Visible = recipe.TimeSpecified)
                    CRAnimationLengthNUD.Value = (decimal)recipe.Time;
            }
            else if (e.Node.Tag is TerjeRecipeIngredient)
            {
                TerjeRecipeIngredient ingredient = e.Node.Tag as TerjeRecipeIngredient;
                CIngrdientGB.Visible = true;
                if (CISingleUseSpecifiedCB.Checked = CIDeleteRequiredCB.Visible = ingredient.SingleUseSpecified)
                    CIDeleteRequiredCB.Checked = ingredient.SingleUse == 1 ? true : false;
                if (CIMinQuantitySpecifiedCB.Checked = CIMinQuantityNUD.Visible = ingredient.MinQuantitySpecified)
                    CIMinQuantityNUD.Value = (decimal)ingredient.MinQuantity;
                if (CIMaxQuantitySpecifiedCB.Checked = CIMaxQuantityNUD.Visible = ingredient.MaxQuantitySpecified)
                    CIMaxQuantityNUD.Value = (decimal)ingredient.MaxQuantity;
                if (CIMinDamageSpecifiedCB.Checked = CIMinDamageCB.Visible = ingredient.MinDamageSpecified)
                    CIMinDamageCB.SelectedIndex = (int)ingredient.MinDamage + 1;
                if (CIMaxDamageSpecifiedCB.Checked = CIMaxDamageCB.Visible = ingredient.MaxDamageSpecified)
                    CIMaxDamageCB.SelectedIndex = (int)ingredient.MaxDamage + 1;
                if (CIAddHealthSpecifiedCB.Checked = CIAddHealthNUD.Visible = ingredient.AddHealthSpecified)
                    CIAddHealthNUD.Value = (int)ingredient.AddHealth;
                if (CISetHealthSpecifiedCB.Checked = CISetHealthNUD.Visible = ingredient.SetHealthSpecified)
                    CISetHealthNUD.Value = (int)ingredient.SetHealth;
                if (CIAddQuantitySpecifiedCB.Checked = CIAddQuantityNUD.Visible = ingredient.AddQuantitySpecified)
                    CIAddQuantityNUD.Value = (int)ingredient.AddQuantity;
            }
            else if (e.Node.Tag is TerjeCraftingResult)
            {
                TerjeCraftingResult result = e.Node.Tag as TerjeCraftingResult;
                CResultCB.Visible = true;
                CRClassnameTB.Text = result.ClassName;
                if (CRSetFullQuantitySpecifiedCB.Checked = CRSetFullQuantityCB.Visible = result.SetFullQuantitySpecified)
                    CRSetFullQuantityCB.Checked = result.SetFullQuantity == 1 ? true : false;
                if (CRSetQuantitySpecifiedCB.Checked = CRSetQuantityNUD.Visible = result.SetQuantitySpecified)
                    CRSetQuantityNUD.Value = (decimal)result.SetQuantity;
                if (CRSetHealthSpecifiedCB.Checked = CRSetHealthNUD.Visible = result.SetHealthSpecified)
                    CRSetHealthNUD.Value = (decimal)result.SetHealth;
                if (CRInheritsHealthSpecifiedCB.Checked = CRInheritsHealthCB.Visible = result.InheritsHealthSpecified)
                    CRInheritsHealthCB.SelectedIndex = (int)result.InheritsHealth + 2;
                if (CRInheritsColorSpecifiedCB.Checked = CRInheritsColorCB.Visible = result.InheritsColorSpecified)
                    CRInheritsColorCB.SelectedIndex = (int)result.InheritsColor + 1;
                if (CRSpawnModeSpecifiedCB.Checked = CRToInventoryCB.Visible = result.SpawnModeSpecified)
                    CRToInventoryCB.SelectedIndex = (int)result.SpawnMode + 2;
            }
            else if (e.Node.Tag is TerjeSkillLevel)
            {
                CRSLGB.Visible = true;
                TerjeSkillLevel SL = e.Node.Tag as TerjeSkillLevel;
                CRSLskillIdCB.SelectedItem = SelectComboBoxByValue(CRSLskillIdCB, SL.skillId);
                CRSLrequiredlevelNUD.Value = (int)SL.requiredLevel;
                CONDExtraOptionsGB.Visible = true;
                if (CONDExtraOptionshideOwnerWhenFalseSpecifiedCB.Checked = CONDExtraOptionshideOwnerWhenFalseCB.Visible = SL.hideOwnerWhenFalseSpecified)
                    CONDExtraOptionshideOwnerWhenFalseCB.Checked = SL.hideOwnerWhenFalse == 1 ? true : false;
                if (CONDExtraOptionsdisplayTextSpecifiedCB.Checked = CONDExtraOptionsdisplayTextTB.Visible = SL.displayTextSpecified)
                    CONDExtraOptionsdisplayTextTB.Text = SL.displayText;
                if (CONDExtraOptionssuccessTextSpecifiedCB.Checked = CONDExtraOptionssuccessTextTB.Visible = SL.successTextSpecified)
                    CONDExtraOptionssuccessTextTB.Text = SL.successText;
                if (CONDExtraOptionsfailTextSpecifiedCB.Checked = CONDExtraOptionsfailTextTB.Visible = SL.failTextSpecified)
                    CONDExtraOptionsfailTextTB.Text = SL.failText;

            }
            else if (e.Node.Tag is TerjeSkillPerk)
            {
                CRSPGB.Visible = true;
                TerjeSkillPerk SP = e.Node.Tag as TerjeSkillPerk;
                CRSPskillIDCB.SelectedItem = SelectComboBoxByValue(CRSPskillIDCB, SP.skillId);
                CRSPperkIDCB.SelectedItem = SelectComboBoxByValue(CRSPperkIDCB, SP.perkId);
                CRSPrequiredlevelNUD.Value = (int)SP.requiredLevel;
                CONDExtraOptionsGB.Visible = true;
                if (CONDExtraOptionshideOwnerWhenFalseSpecifiedCB.Checked = CONDExtraOptionshideOwnerWhenFalseCB.Visible = SP.hideOwnerWhenFalseSpecified)
                    CONDExtraOptionshideOwnerWhenFalseCB.Checked = SP.hideOwnerWhenFalse == 1 ? true : false;
                if (CONDExtraOptionsdisplayTextSpecifiedCB.Checked = CONDExtraOptionsdisplayTextTB.Visible = SP.displayTextSpecified)
                    CONDExtraOptionsdisplayTextTB.Text = SP.displayText;
                if (CONDExtraOptionssuccessTextSpecifiedCB.Checked = CONDExtraOptionssuccessTextTB.Visible = SP.successTextSpecified)
                    CONDExtraOptionssuccessTextTB.Text = SP.successText;
                if (CONDExtraOptionsfailTextSpecifiedCB.Checked = CONDExtraOptionsfailTextTB.Visible = SP.failTextSpecified)
                    CONDExtraOptionsfailTextTB.Text = SP.failText;

            }
            else if (e.Node.Tag is TerjeTimeout)
            {
                SCLtimeoutGB.Visible = true;
                TerjeTimeout timeout = e.Node.Tag as TerjeTimeout;
                SCLTimeoutidTB.Text = timeout.id;
                if (SCLTimeouthoursSpecifiedCB.Checked = SCLTimeouthoursNUD.Visible = timeout.hoursSpecified)
                    SCLTimeouthoursNUD.Value = timeout.hours;
                if (SCLTimeoutminutesSpecifiedCB.Checked = SCLTimeoutminutesNUD.Visible = timeout.minutesSpecified)
                    SCLTimeoutminutesNUD.Value = timeout.minutes;
                if (SCLTimeoutsecondsSpecifiedCB.Checked = SCLTimeoutsecondsNUD.Visible = timeout.secondsSpecified)
                    SCLTimeoutsecondsNUD.Value = timeout.hours;
                CONDExtraOptionsGB.Visible = true;
                if (CONDExtraOptionshideOwnerWhenFalseSpecifiedCB.Checked = CONDExtraOptionshideOwnerWhenFalseCB.Visible = timeout.hideOwnerWhenFalseSpecified)
                    CONDExtraOptionshideOwnerWhenFalseCB.Checked = timeout.hideOwnerWhenFalse == 1 ? true : false;
                if (CONDExtraOptionsdisplayTextSpecifiedCB.Checked = CONDExtraOptionsdisplayTextTB.Visible = timeout.displayTextSpecified)
                    CONDExtraOptionsdisplayTextTB.Text = timeout.displayText;
                if (CONDExtraOptionssuccessTextSpecifiedCB.Checked = CONDExtraOptionssuccessTextTB.Visible = timeout.successTextSpecified)
                    CONDExtraOptionssuccessTextTB.Text = timeout.successText;
                if (CONDExtraOptionsfailTextSpecifiedCB.Checked = CONDExtraOptionsfailTextTB.Visible = timeout.failTextSpecified)
                    CONDExtraOptionsfailTextTB.Text = timeout.failText;
            }
            else if (e.Node.Tag is TerjeSpecificPlayers)
            {
                TerjeSpecificPlayers players = e.Node.Tag as TerjeSpecificPlayers;
                CONDExtraOptionsGB.Visible = true;
                if (CONDExtraOptionshideOwnerWhenFalseSpecifiedCB.Checked = CONDExtraOptionshideOwnerWhenFalseCB.Visible = players.hideOwnerWhenFalseSpecified)
                    CONDExtraOptionshideOwnerWhenFalseCB.Checked = players.hideOwnerWhenFalse == 1 ? true : false;
                if (CONDExtraOptionsdisplayTextSpecifiedCB.Checked = CONDExtraOptionsdisplayTextTB.Visible = players.displayTextSpecified)
                    CONDExtraOptionsdisplayTextTB.Text = players.displayText;
                if (CONDExtraOptionssuccessTextSpecifiedCB.Checked = CONDExtraOptionssuccessTextTB.Visible = players.successTextSpecified)
                    CONDExtraOptionssuccessTextTB.Text = players.successText;
                if (CONDExtraOptionsfailTextSpecifiedCB.Checked = CONDExtraOptionsfailTextTB.Visible = players.failTextSpecified)
                    CONDExtraOptionsfailTextTB.Text = players.failText;
            }
            else if (e.Node.Tag is TerjeCustomCondition)
            {
                SCLConditionCustomConditionGB.Visible = true;
                TerjeCustomCondition custcon = e.Node.Tag as TerjeCustomCondition;
                SCLConditionCustomConditionclassnameTB.Text = custcon.classname;
                CONDExtraOptionsGB.Visible = true;
                if (CONDExtraOptionshideOwnerWhenFalseSpecifiedCB.Checked = CONDExtraOptionshideOwnerWhenFalseCB.Visible = custcon.hideOwnerWhenFalseSpecified)
                    CONDExtraOptionshideOwnerWhenFalseCB.Checked = custcon.hideOwnerWhenFalse == 1 ? true : false;
                if (CONDExtraOptionsdisplayTextSpecifiedCB.Checked = CONDExtraOptionsdisplayTextTB.Visible = custcon.displayTextSpecified)
                    CONDExtraOptionsdisplayTextTB.Text = custcon.displayText;
                if (CONDExtraOptionssuccessTextSpecifiedCB.Checked = CONDExtraOptionssuccessTextTB.Visible = custcon.successTextSpecified)
                    CONDExtraOptionssuccessTextTB.Text = custcon.successText;
                if (CONDExtraOptionsfailTextSpecifiedCB.Checked = CONDExtraOptionsfailTextTB.Visible = custcon.failTextSpecified)
                    CONDExtraOptionsfailTextTB.Text = custcon.failText;
            }
            else if (e.Node.Tag is TerjeSetUserVariable)
            {
                CONDExtraVariablesGB.Visible = true;
                TerjeSetUserVariable SetUserCond = e.Node.Tag as TerjeSetUserVariable;
                CONDExtraVariablesnameTB.Text = SetUserCond.name;
                CONDExtraVariablesvalueNUD.Value = SetUserCond.value;
                if (CONDExtraVariablesPersistSpecifiedCB.Checked = CONDExtraVariablesPersistCB.Visible = SetUserCond.persistSpecified)
                    CONDExtraVariablesPersistCB.Checked = SetUserCond.persist == 1 ? true : false;
            }
            else if (e.Node.Tag is TerjeComapreUserVariables)
            {
                CONDExtraVariablesGB.Visible = true;
                TerjeComapreUserVariables custcon = e.Node.Tag as TerjeComapreUserVariables;
                CONDExtraVariablesnameTB.Text = custcon.name;
                CONDExtraVariablesvalueNUD.Value = custcon.value;
                if (CONDExtraVariablesPersistSpecifiedCB.Checked = CONDExtraVariablesPersistCB.Visible = custcon.persistSpecified)
                    CONDExtraVariablesPersistCB.Checked = custcon.persist == 1 ? true : false;
                CONDExtraOptionsGB.Visible = true;
                if (CONDExtraOptionshideOwnerWhenFalseSpecifiedCB.Checked = CONDExtraOptionshideOwnerWhenFalseCB.Visible = custcon.hideOwnerWhenFalseSpecified)
                    CONDExtraOptionshideOwnerWhenFalseCB.Checked = custcon.hideOwnerWhenFalse == 1 ? true : false;
                if (CONDExtraOptionsdisplayTextSpecifiedCB.Checked = CONDExtraOptionsdisplayTextTB.Visible = custcon.displayTextSpecified)
                    CONDExtraOptionsdisplayTextTB.Text = custcon.displayText;
                if (CONDExtraOptionssuccessTextSpecifiedCB.Checked = CONDExtraOptionssuccessTextTB.Visible = custcon.successTextSpecified)
                    CONDExtraOptionssuccessTextTB.Text = custcon.successText;
                if (CONDExtraOptionsfailTextSpecifiedCB.Checked = CONDExtraOptionsfailTextTB.Visible = custcon.failTextSpecified)
                    CONDExtraOptionsfailTextTB.Text = custcon.failText;
            }
            else if (e.Node.Tag is TerjeMathWithUserVariable)
            {
                CONDExtraVariablesMathGB.Visible = true;
                TerjeMathWithUserVariable custcon = e.Node.Tag as TerjeMathWithUserVariable;
                CONDExtraMathnameTB.Text = custcon.name;
                CONDExtraMathvalueNUD.Value = custcon.value;
                if (CONDExtraMathminSpecifiedCB.Checked = CONDExtraMathminNUD.Visible = custcon.minSpecified)
                    CONDExtraMathminNUD.Value = custcon.min;
                if (CONDExtraMathmaxSpecifiedCB.Checked = CONDExtraMathmaxNUD.Visible = custcon.maxSpecified)
                    CONDExtraMathmaxNUD.Value = custcon.min;
                if (CONDExtraMathPersistSpecifiedCB.Checked = CONDExtraMathPersistCB.Visible = custcon.persistSpecified)
                    CONDExtraMathPersistCB.Checked = custcon.persist == 1 ? true : false;
            }
            else if (e.Node.Tag is TerjeSpecialConditions)
            {
                TerjeSpecialConditions custcon = e.Node.Tag as TerjeSpecialConditions;
                CONDExtraOptionsGB.Visible = true;
                if (CONDExtraOptionshideOwnerWhenFalseSpecifiedCB.Checked = CONDExtraOptionshideOwnerWhenFalseCB.Visible = custcon.hideOwnerWhenFalseSpecified)
                    CONDExtraOptionshideOwnerWhenFalseCB.Checked = custcon.hideOwnerWhenFalse == 1 ? true : false;
                if (CONDExtraOptionsdisplayTextSpecifiedCB.Checked = CONDExtraOptionsdisplayTextTB.Visible = custcon.displayTextSpecified)
                    CONDExtraOptionsdisplayTextTB.Text = custcon.displayText;
                if (CONDExtraOptionssuccessTextSpecifiedCB.Checked = CONDExtraOptionssuccessTextTB.Visible = custcon.successTextSpecified)
                    CONDExtraOptionssuccessTextTB.Text = custcon.successText;
                if (CONDExtraOptionsfailTextSpecifiedCB.Checked = CONDExtraOptionsfailTextTB.Visible = custcon.failTextSpecified)
                    CONDExtraOptionsfailTextTB.Text = custcon.failText;
            }
            else if (e.Node.Tag is TerjeScriptableArea)
            {
                SAGB.Visible = true;
                TerjeScriptableArea SA = e.Node.Tag as TerjeScriptableArea;
                SAActiveCB.Checked = SA.Active == 1 ? true : false;
                SAClassnameTB.Text = SA.Classname;
                SAPosXNUD.Value = (decimal)SA.PositionVec3.X;
                SAPosYNUD.Value = (decimal)SA.PositionVec3.Y;
                SAPosZNUD.Value = (decimal)SA.PositionVec3.Z;
                SASpawnChanceNUD.Value = (decimal)SA.SpawnChance;
                if (SA.FilterSpecified)
                {
                    SAFGB.Visible = true;
                    // Split the input string into a list of skill IDs
                    var selectedIds = SA.Filter.Split(',').Select(s => s.Trim()).ToHashSet();

                    // Loop through the CheckBoxes in the GroupBox
                    foreach (var checkBox in SAFLP.Controls.OfType<CheckBox>())
                    {
                        // Tag holds the skill ID
                        string skillId = checkBox.Tag?.ToString();

                        if (!string.IsNullOrEmpty(skillId))
                        {
                            checkBox.Checked = selectedIds.Contains(skillId);
                        }
                    }
                }

            }
            else if (e.Node.Tag is TerjeScriptableAreaData)
            {
                SADGB.Visible = true;
                TerjeScriptableArea SA = e.Node.Parent.Tag as TerjeScriptableArea;
                TerjeScriptableAreaData sad = e.Node.Tag as TerjeScriptableAreaData;
                if (SA.Classname == "TerjePsionicScriptableArea" || SA.Classname == "TerjeRadioactiveScriptableArea")
                {
                    SADOuterRadiusNUD.Visible = true;
                    label34.Visible = true;
                    SADOuterRadiusNUD.Value = (decimal)sad.OuterRadius;
                    label35.Text = "Inner Radius";
                    SADInnerRadiusNUD.Value = (decimal)sad.InnerRadius;
                    SADHeightMInNUD.Value = (decimal)sad.HeightMin;
                    SADHeightMaxNUD.Value = (decimal)sad.HeightMax;
                    SADPowerNUD.Value = (decimal)sad.Power;

                }
                else if (SA.Classname == "TerjeExperienceModScriptableArea")
                {
                    SADOuterRadiusNUD.Visible = false;
                    label34.Visible = false;
                    label35.Text = "Radius";
                    SADInnerRadiusNUD.Value = (decimal)sad.Radius;
                    SADHeightMInNUD.Value = (decimal)sad.HeightMin;
                    SADHeightMaxNUD.Value = (decimal)sad.HeightMax;
                    SADPowerNUD.Value = (decimal)sad.Power;
                }
            }
            else if (e.Node.Tag is TerjeLoadout)
            {
                TerjeLoadout loadout = e.Node.Tag as TerjeLoadout;
                SSLGB.Visible = true;
                SSLdisplayNameTB.Text = loadout.displayName;
                SSLidTB.Text = loadout.id;
            }
            else if (e.Node.Tag is TerjeLoadoutItem)
            {
                TerjeLoadoutItem item = e.Node.Tag as TerjeLoadoutItem;
                SCLItemsGB.Visible = true;
                SCLItemsClassnameTB.Text = item.classname;
                // displayName
                if ((SCLItemsdisplayNameSpecifiedCB.Checked = SCLItemsdisplayNameTB.Visible = item.displayNameSpecified))
                    SCLItemsdisplayNameTB.Text = item.displayName;

                // quantity
                if ((SCLItemsquantitySpecifiedCB.Checked = SCLItemsquantityTB.Visible = item.quantitySpecified))
                    SCLItemsquantityTB.Text = item.quantity;

                // count
                if ((SCLItemscountSpecifiedCB.Checked = SCLItemscountTB.Visible = item.countSpecified))
                    SCLItemscountTB.Text = item.count;

                // health
                if ((SCLItemshealthSpecifiedCB.Checked = SCLItemshealthTB.Visible = item.healthSpecified))
                    SCLItemshealthTB.Text = item.health;

                // position
                if ((SCLItemspositionSpecifiedCB.Checked = SCLItemspositionTB.Visible = item.positionSpecified))
                    SCLItemspositionTB.Text = item.position;

                // liquid
                if ((SCLItemsliquidSpecifiedCB.Checked = SCLItemsliquidTB.Visible = item.liquidSpecified))
                    SCLItemsliquidTB.Text = item.liquid;

                // temperature
                if ((SCLItemstemperatureSpecifiedCB.Checked = SCLItemstemperatureTB.Visible = item.temperatureSpecified))
                    SCLItemstemperatureTB.Text = item.temperature;

                // foodStage
                if ((SCLItemsfoodStageSpecifiedCB.Checked = SCLItemsfoodStageCB.Visible = item.foodStageSpecified))
                    SCLItemsfoodStageCB.SelectedItem = item.foodStage;

                // disinfected
                if (SCLItemsdisinfectedSpecifiedCB.Checked = SCLItemsdisinfectedCB.Visible = item.disinfectedSpecified)
                    SCLItemsdisinfectedCB.Checked = item.disinfected == 1 ? true : false;

                // agents
                if ((SCLItemsagentsSpecifiedCB.Checked = SCLItemsagentsTB.Visible = item.agentsSpecified))
                    SCLItemsagentsTB.Text = item.agents;

                // quickbar
                if ((SCLItemsquickbarSpecifiedCB.Checked = SCLItemsquickbarNUD.Visible = item.quickbarSpecified))
                    SCLItemsquickbarNUD.Value = item.quickbar;

                // ammoType
                if ((SCLItemsammoTypeSpecifiedCB.Checked = SCLItemsammoTypeTB.Visible = item.ammoTypeSpecified))
                    SCLItemsammoTypeTB.Text = item.ammoType;

                // ammoCount
                if ((SCLItemsammoCountSpecifiedCB.Checked = SCLItemsammoCountTB.Visible = item.ammoCountSpecified))
                    SCLItemsammoCountTB.Text = item.ammoCount;

                // cost
                if ((SCLItemscostSpecifiedCB.Checked = SCLItemscostNUD.Visible = item.costSpecified))
                    SCLItemscostNUD.Value = item.cost;


            }
            else if (e.Node.Tag is TerjeLoadoutSelector)
            {
                SCLSelectorGB.Visible = true;
                TerjeLoadoutSelector selector = e.Node.Tag as TerjeLoadoutSelector;
                SCLSelectortypeCB.SelectedItem = selector.type;
                if (SCLSelectorMultipleGB.Visible = selector.type == "MULTIPLE")
                {
                    if (SCLSelectorpointsCountSpecifiedCB.Checked = SCLSelectorpointsCountNUD.Visible = selector.pointsCountSpecified)
                        SCLSelectorpointsCountNUD.Value = selector.pointsCount;
                    if (SCLSelectorpointsHandlerSpecifiedCB.Checked = SCLSelectorpointsHandlerTB.Visible = selector.pointsHandlerSpecified)
                        SCLSelectorpointsHandlerTB.Text = selector.pointsHandler;
                    if (SCLSelectorpointsIconSpecifiedCB.Checked = SCLSelectorpointsIconTB.Visible = selector.pointsIconSpecified)
                        SCLSelectorpointsIconTB.Text = selector.pointsIcon;
                }
                else
                {
                    SCLSelectorpointsCountNUD.Visible = SCLSelectorpointsHandlerTB.Visible = SCLSelectorpointsIconTB.Visible = false;
                }
                if (SCLSelectordisplayNameSpecifiedCB.Checked = SCLSelectordisplayNameTB.Visible = selector.displayNameSpecified)
                    SCLSelectordisplayNameTB.Text = selector.displayName;


            }
            else if (e.Node.Tag is TerjeLoadoutGroup)
            {
                SCLGroupGB.Visible = true;
                TerjeLoadoutGroup group = e.Node.Tag as TerjeLoadoutGroup;
                if (SCLGroupcostSpecifiedCB.Checked = SCLGroupcostNUD.Visible = group.costSpecified)
                    SCLGroupcostNUD.Value = group.cost;
            }
            else if (e.Node.Tag is TerjeRespawnPoint)
            {
                MapPanel.Visible = true;
                pictureBox1.Invalidate();
            }
            else  if (e.Node.Tag.ToString() == "MapView")
            {
                MapPanel.Visible = true;
                pictureBox1.Invalidate();
            }
            else if (e.Node.Tag is BindingList<TerjeRespawnPoint>)
            {
                MapPanel.Visible = true;
            }
            else if (e.Node.Tag is TerjeValueString)
            {
                TerjeValueString vs = e.Node.Tag as TerjeValueString;
                groupBox1.Visible = true;
                CommentRTB.Text = "";
                groupBox1.Text = "String";
                StringTB.Visible = true;
                StringTB.Text = vs.value.ToString();
            }
            else if (e.Node.Tag is TerjeFace)
            {
                SCFaceGB.Visible = true;
                TerjeFace face = currentTreeNode.Tag as TerjeFace;
                SCFaceclassnameTB.Text = face.classname;
                SCFaceiconTB.Text = face.icon;
                if (SCFacebackgroundSpecifiedCB.Checked = SCFacebackgroundTB.Visible = face.backgroundSpecified)
                    SCFacebackgroundTB.Text = face.background;
            }
                useraction = true;
        }
        public ComboBoxItem SelectComboBoxByValue(ComboBox comboBox, string value)
        {
            foreach (ComboBoxItem item in comboBox.Items)
            {
                if (item.Value == value)
                {
                    return item;
                }
            }
            return null;
        }
        private void TerjeTV_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            useraction = false;
            TerjeTV.SelectedNode = e.Node;
            currentTreeNode = e.Node;
            foreach (ToolStripItem item in contextMenuStrip1.Items)
            {
                item.Visible = false;
            }
            if (e.Button == MouseButtons.Right)
            {
                if(e.Node.Tag.ToString() == "TerjeCrafting")
                {
                    addNewCraftingRecipeToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag is TerjeRecipe)
                {
                    removeSelectedRecipeToolStripMenuItem.Visible = true;
                    TerjeRecipe recipe = e.Node.Tag as TerjeRecipe;
                    if (recipe.Conditions == null)
                        addConditionsToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag.ToString() == "CraftingResults")
                {
                    addCraftingResultToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag is TerjeCraftingResult)
                {
                    removeCraftingResultToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag.ToString() == "Items")
                {
                    addIngredientItemToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag.ToString() == "IngredientItem")
                {
                    removeIngredientItemToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag is TerjeProtectionIDs)
                {
                    addNewSteamIDToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag.ToString() == "IDString" || e.Node.Tag is TerjeSpecificPlayer)
                {
                    removeSteamIDToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag is TerjeConditions)
                {
                    addCustomConditionToolStripMenuItem.Visible = true;
                    removeConditionsToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag is TerjeSpecificPlayers)
                {
                    addNewSteamIDToolStripMenuItem.Visible = true;
                    removeConditionToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag is TerjeConditionBase || e.Node.Tag is TerjeSetUserVariable)
                {
                    removeConditionToolStripMenuItem.Visible = true;
                    if(e.Node.Tag is TerjeSpecialConditions)
                    {
                        addCustomConditionToolStripMenuItem.Visible = true;
                    }
                }
                else if(e.Node.Tag is TerjeScriptableAreas)
                {
                    addNewScriptableAreaToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag is TerjeScriptableArea)
                {
                    removeScriptableAreaToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag is TerjeLoadouts)
                {
                    addNewLoadoutToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag is TerjeLoadout)
                {
                    TerjeLoadout loadout = e.Node.Tag as TerjeLoadout;
                    if(loadout.Items == null)
                        addItemsToolStripMenuItem.Visible = true;
                    if(loadout.Conditions == null)
                        addConditionsToolStripMenuItem.Visible = true;
                    removeLoadoutToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag is TerjeLoadoutItems)
                {
                    addItemToolStripMenuItem.Visible = true;
                    addSelectorToolStripMenuItem.Visible = true;
                    removeItemsToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag is TerjeLoadoutItem)
                {
                    addItemToolStripMenuItem.Visible = true;
                    removeItemToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag is TerjeLoadoutGroup)
                {
                    addItemToolStripMenuItem.Visible = true;
                    removeGroupToolStripMenuItem.Visible = true;
                }
                else if(e.Node.Tag is TerjeLoadoutSelector)
                {
                    addItemToolStripMenuItem.Visible = true;
                    addGroupToolStripMenuItem.Visible = true;
                    removeSelectorToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag is TerjeRespawn)
                {
                    TerjeRespawn respawn = e.Node.Tag as TerjeRespawn;
                    if (respawn.Conditions == null)
                        addConditionsToolStripMenuItem.Visible = true;
                    removeLoadoutToolStripMenuItem.Visible = true;
                }
                else if (e.Node.Tag is TerjeFace)
                {
                    TerjeFace face = e.Node.Tag as TerjeFace;
                    if (face.Conditions == null)
                        addConditionsToolStripMenuItem.Visible = true;
                }
                contextMenuStrip1.Show(Cursor.Position);
            }
            useraction = true;
        }
        private void BoolCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentCFGline.cfgVariable = BoolCB.Checked;
            currentTreeNode.Text = $"{currentCFGline.cfgvariablename} = {currentCFGline.cfgVariable.ToString()}";
            currentCFGFIle.isDirty = true;
        }
        private void FloatNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            currentCFGline.cfgVariable = (float)FloatNUD.Value;
            currentTreeNode.Text = $"{currentCFGline.cfgvariablename} = {currentCFGline.cfgVariable.ToString()}";
            currentCFGFIle.isDirty = true;
        }
        private void IntNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (currentTreeNode.Tag is TerjeCFGLine)
            {
                currentCFGline.cfgVariable = (int)IntNUD.Value;
                currentTreeNode.Text = $"{currentCFGline.cfgvariablename} = {currentCFGline.cfgVariable.ToString()}";
                currentCFGFIle.isDirty = true;
            }
            else if (currentTreeNode.Tag is TerjeValueString)
            {

            }
        }
        private void StringTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (currentTreeNode.Tag is TerjeCFGLine)
            {
                currentCFGline.cfgVariable = StringTB.Text;
                currentTreeNode.Text = $"{currentCFGline.cfgvariablename} = {currentCFGline.cfgVariable.ToString()}";
                currentCFGFIle.isDirty = true;
            }
            else if (currentTreeNode.Tag is TerjeValueString)
            {
                TerjeValueString sv = currentTreeNode.Tag as TerjeValueString;
                sv.value = StringTB.Text;
                currentTreeNode.Text = $"{sv.value}";
                TerjeGeneral.isDirty = true;
            }
        }
        private void addNewCraftingRecipeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeRecipe newrecipe = new TerjeRecipe()
            {
                DisplayName = "New Recipe",
                Enabled = 0,
                Time = -1,
                FirstIngredient = new TerjeRecipeIngredient(),
                SecondIngredient = new TerjeRecipeIngredient(),
                CraftingResults = new TerjeCraftingResults()
                {
                    Results = new List<TerjeCraftingResult>()
                }
            };
            TerjeCraftingFiles.Items.Add(newrecipe);
            currentTreeNode.Nodes.Add(GetRecipeNodes(newrecipe));
            TerjeCraftingFiles.isDirty = true;
            TerjeTV.SelectedNode = currentTreeNode.LastNode;
        }
        private void removeSelectedRecipeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeRecipe recipe = currentTreeNode.Tag as TerjeRecipe;
            TerjeCraftingFiles.Items.Remove(recipe);
            currentTreeNode.Parent.Nodes.Remove(currentTreeNode);
            TerjeCraftingFiles.isDirty = true;
        }
        private void addCraftingResultToolStripMenuItem_Click(object sender, EventArgs e)
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
                TerjeRecipe recipe = currentTreeNode.Parent.Tag as TerjeRecipe;
                foreach (string l in addedtypes)
                {
                    TerjeCraftingResult newitem = new TerjeCraftingResult()
                    {
                        ClassName = l,
                        SetFullQuantity = 1,
                        SetQuantity = (float)-1.0,
                        SetHealth = (float)-1.0,
                        InheritsHealth = -1,
                        InheritsColor = -1,
                        SpawnMode = -2
                    };
                    recipe.CraftingResults.Results.Add(newitem);
                    currentTreeNode.Nodes.Add(new TreeNode(newitem.ClassName)
                    {
                        Tag = newitem
                    });
                }
                currentTreeNode.Expand();
                TerjeCraftingFiles.isDirty = true;
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void removeCraftingResultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeRecipe recipe = currentTreeNode.Parent.Parent.Tag as TerjeRecipe;
            recipe.CraftingResults.Results.Remove(currentTreeNode.Tag as TerjeCraftingResult);
            currentTreeNode.Parent.Nodes.Remove(currentTreeNode);
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Tag as TerjeRecipe;
            recipe.DisplayName = CRNameTB.Text;
            currentTreeNode.Text = $"{recipe.DisplayName}";
            TerjeCraftingFiles.isDirty = true;
        }
        private void CREnabledCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Tag as TerjeRecipe;
            recipe.Enabled = CREnabledCB.Checked == true ? 1 : 0;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRIsInstaRecipeCB_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void CRAnimationLengthNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Tag as TerjeRecipe;
            recipe.Time = (float)CRAnimationLengthNUD.Value;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRtimeSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Tag as TerjeRecipe;
            recipe.TimeSpecified = CRAnimationLengthNUD.Visible = CRtimeSpecifiedCB.Checked;
            CRAnimationLengthNUD.Value = (decimal)recipe.Time;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CIDeleteRequiredCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.SingleUse = CIDeleteRequiredCB.Checked == true ? 1 : 0;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CIMinQuantityNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Parent.Tag as TerjeRecipe;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.MinQuantity = (float)CIMinQuantityNUD.Value;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CIMaxQuantityNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Parent.Tag as TerjeRecipe;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.MaxQuantity = (float)CIMaxQuantityNUD.Value;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CIMinDamageCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Parent.Tag as TerjeRecipe;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.MinDamage = CIMinDamageCB.SelectedIndex - 1;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CIMaxDamageCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Parent.Tag as TerjeRecipe;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.MaxDamage = CIMaxDamageCB.SelectedIndex - 1;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CIAddHealthNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Parent.Tag as TerjeRecipe;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.AddHealth = (float)CIAddHealthNUD.Value;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CISetHealthNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Parent.Tag as TerjeRecipe;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.SetHealth = (float)CISetHealthNUD.Value;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CIAddQuantityNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Parent.Tag as TerjeRecipe;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.AddQuantity = (float)CIAddQuantityNUD.Value;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CISingleUseSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.SingleUseSpecified = CIDeleteRequiredCB.Visible = CISingleUseSpecifiedCB.Checked;
            CIDeleteRequiredCB.Checked = ingredient.SingleUse == 1 ? true:false;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CIMinQuantitySpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.MinQuantitySpecified = CIMinQuantityNUD.Visible = CIMinQuantitySpecifiedCB.Checked;
            CIMinQuantityNUD.Value = (decimal)ingredient.MinQuantity;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CIMaxQuantitySpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if(!useraction) return;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.MaxQuantitySpecified = CIMaxQuantityNUD.Visible = CIMaxQuantitySpecifiedCB.Checked;
            CIMaxQuantityNUD.Value = (decimal)ingredient.MaxQuantity;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CIMinDamageSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.MinDamageSpecified = CIMinDamageCB.Visible = CIMinDamageSpecifiedCB.Checked;
            CIMinDamageCB.SelectedIndex = ingredient.MinDamage;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CIMaxDamageSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.MaxDamageSpecified = CIMaxDamageCB.Visible = CIMaxDamageSpecifiedCB.Checked;
            CIMaxDamageCB.SelectedIndex = ingredient.MaxDamage;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CIAddHealthSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.AddHealthSpecified = CIAddHealthNUD.Visible = CIAddHealthSpecifiedCB.Checked;
            CIAddHealthNUD.Value = (int)ingredient.AddHealth;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CISetHealthSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.SetHealthSpecified = CISetHealthNUD.Visible = CISetHealthSpecifiedCB.Checked;
            CISetHealthNUD.Value = (int)ingredient.SetHealth;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CIAddQuantitySpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipeIngredient ingredient = currentTreeNode.Tag as TerjeRecipeIngredient;
            ingredient.AddQuantitySpecified = CIAddQuantityNUD.Visible = CIAddQuantitySpecifiedCB.Checked;
            CIAddQuantityNUD.Value = (int)ingredient.AddQuantity;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRSetFullQuantityCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Parent.Parent.Tag as TerjeRecipe;
            TerjeCraftingResult result = currentTreeNode.Tag as TerjeCraftingResult;
            result.SetFullQuantity = CRSetFullQuantityCB.Checked==true?1:0;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRSetQuantityNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Parent.Parent.Tag as TerjeRecipe;
            TerjeCraftingResult result = currentTreeNode.Tag as TerjeCraftingResult;
            result.SetQuantity = (float)CRSetQuantityNUD.Value;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRSetHealthNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Parent.Parent.Tag as TerjeRecipe;
            TerjeCraftingResult result = currentTreeNode.Tag as TerjeCraftingResult;
            result.SetHealth = (float)CRSetHealthNUD.Value;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRInheritsHealthCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Parent.Parent.Tag as TerjeRecipe;
            TerjeCraftingResult result = currentTreeNode.Tag as TerjeCraftingResult;
            result.InheritsHealth = CRInheritsHealthCB.SelectedIndex - 2;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRInheritsColorCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeRecipe recipe = currentTreeNode.Parent.Parent.Tag as TerjeRecipe;
            TerjeCraftingResult result = currentTreeNode.Tag as TerjeCraftingResult;
            result.InheritsColor = CRInheritsColorCB.SelectedIndex - 1;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRToInventoryCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeCraftingResult result = currentTreeNode.Tag as TerjeCraftingResult;
            result.SpawnMode = (int)CRToInventoryCB.SelectedIndex - 2;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRClassnameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeCraftingResult result = currentTreeNode.Tag as TerjeCraftingResult;
            result.ClassName = CRClassnameTB.Text;
            currentTreeNode.Text = $"{result.ClassName}";
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRSetFullQuantitySpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeCraftingResult result = currentTreeNode.Tag as TerjeCraftingResult;
            result.SetFullQuantitySpecified = CRSetFullQuantityCB.Visible = CRSetFullQuantitySpecifiedCB.Checked;
            CRSetFullQuantityCB.Checked = result.SetFullQuantity == 1 ? true : false;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRSetQuantitySpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeCraftingResult result = currentTreeNode.Tag as TerjeCraftingResult;
            result.SetQuantitySpecified = CRSetQuantityNUD.Visible = CRSetQuantitySpecifiedCB.Checked;
            CRSetQuantityNUD.Value = (decimal)result.SetQuantity;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRSetHealthSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeCraftingResult result = currentTreeNode.Tag as TerjeCraftingResult;
            result.SetHealthSpecified = CRSetHealthNUD.Visible = CRSetHealthSpecifiedCB.Checked;
            CRSetHealthNUD.Value = (decimal)result.SetHealth;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRInheritsHealthSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeCraftingResult result = currentTreeNode.Tag as TerjeCraftingResult;
            result.InheritsHealthSpecified = CRInheritsHealthCB.Visible = CRInheritsHealthSpecifiedCB.Checked;
            CRInheritsHealthCB.SelectedIndex = result.InheritsHealth + 2;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRInheritsColorSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeCraftingResult result = currentTreeNode.Tag as TerjeCraftingResult;
            result.InheritsColorSpecified = CRInheritsColorCB.Visible = CRInheritsColorSpecifiedCB.Checked;
            CRInheritsColorCB.SelectedIndex = result.InheritsColor + 1;
            TerjeCraftingFiles.isDirty = true;
        }
        private void CRSpawnModeSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeCraftingResult result = currentTreeNode.Tag as TerjeCraftingResult;
            result.SpawnModeSpecified = CRToInventoryCB.Visible = CRSpawnModeSpecifiedCB.Checked;
            CRToInventoryCB.SelectedIndex = result.SpawnMode + 2;
            TerjeCraftingFiles.isDirty = true;
        }
        private void addIngredientItemToolStripMenuItem_Click(object sender, EventArgs e)
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
                TerjeRecipeIngredient ingredient = currentTreeNode.Parent.Tag as TerjeRecipeIngredient;
                foreach (string l in addedtypes)
                {
                    ingredient.Items.Add(l);
                    currentTreeNode.Nodes.Add(new TreeNode(l)
                    {
                        Tag = "IngredientItem"
                    });
                }
                TerjeCraftingFiles.isDirty = true;
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }

        }
        private void removeIngredientItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeRecipeIngredient ingredient = currentTreeNode.Parent.Parent.Tag as TerjeRecipeIngredient;
            ingredient.Items.Remove(currentTreeNode.Text);
            currentTreeNode.Parent.Nodes.Remove(currentTreeNode);
            TerjeCraftingFiles.isDirty = true;
        }
        private void addNewSteamIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewfileName form = new AddNewfileName()
            {
                setdescription = "Please enter the steam id you want Protection for",
                SetTitle = "Add Steam ID",
                Setbutton = "Add"
            };
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (currentTreeNode.Tag is TerjeProtectionIDs)
                {
                    TerjeProtectionIDs prot = currentTreeNode.Tag as TerjeProtectionIDs;
                    prot.IDList.Add(form.NewFileName);
                    currentTreeNode.Nodes.Add(new TreeNode(form.NewFileName)
                    {
                        Tag = "IDString"
                    });
                    currentTreeNode.ExpandAll();
                    prot.isDirty = true;
                }
                else if (currentTreeNode.Tag is TerjeSpecificPlayers)
                {
                    TerjeSpecificPlayers SP = currentTreeNode.Tag as TerjeSpecificPlayers;
                    TerjeSpecificPlayer terjeCraftingSpecificPlayer = new TerjeSpecificPlayer()
                    {
                       steamGUID = form.NewFileName
                    };
                    SP.SpecificPlayer.Add(terjeCraftingSpecificPlayer);
                   TreeNode newTN = new TreeNode(form.NewFileName)
                    {
                        Tag = terjeCraftingSpecificPlayer
                    };
                    currentTreeNode.Nodes.Add(newTN);
                    SetCorrectFileasDirty();
                    currentTreeNode.Expand();
                    TerjeTV.SelectedNode = newTN;
                    

                }
            }
        }
        private void removeSteamIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentTreeNode.Parent.Tag is TerjeProtectionIDs)
            {
                TerjeProtectionIDs prot = currentTreeNode.Parent.Tag as TerjeProtectionIDs;
                prot.IDList.Remove(currentTreeNode.Text);
                currentTreeNode.Parent.Nodes.Remove(currentTreeNode);
                prot.isDirty = true;
            }
            else if (currentTreeNode.Tag is TerjeSpecificPlayer)
            {
                TerjeSpecificPlayers SP = currentTreeNode.Parent.Tag as TerjeSpecificPlayers;
                SP.SpecificPlayer.Remove(currentTreeNode.Tag as TerjeSpecificPlayer);

                SetCorrectFileasDirty();

                currentTreeNode.Parent.Nodes.Remove(currentTreeNode);
            }
        }
        private void darkButton1_Click(object sender, EventArgs e)
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
                    CRClassnameTB.Text = l;
                }
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void CRSLSkillIDCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeSkillLevel SL = currentTreeNode.Tag as TerjeSkillLevel;
            var selectedSkill = CRSLskillIdCB.SelectedItem as ComboBoxItem;
            SL.skillId = selectedSkill.Value;
            SetCorrectFileasDirty();
        }
        private void CRSLRequiredlevelNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeSkillLevel SL = currentTreeNode.Tag as TerjeSkillLevel;
            SL.requiredLevel = (int)CRSLrequiredlevelNUD.Value;
            SetCorrectFileasDirty();
        }
        private void addCustomConditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddnewTerjeCondition form = new AddnewTerjeCondition();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (currentTreeNode.Tag is TerjeConditions)
                {
                    TerjeConditions condition = currentTreeNode.Tag as TerjeConditions;
                    switch (form.Selectedcondition)
                    {
                        case "Timeout":
                            TerjeTimeout newtimeout = new TerjeTimeout()
                            {
                                id = "Test",
                            };
                            condition.items.Add(newtimeout);
                            currentTreeNode.Nodes.Add(new TreeNode(newtimeout.GetType().Name)
                            {
                                Tag = newtimeout
                            });
                            break;
                        case "Skill Level":
                            TerjeSkillLevel terjeSkillLevel = new TerjeSkillLevel()
                            {
                                skillId = "hunt",
                                requiredLevel = 25,
                            };
                            condition.items.Add(terjeSkillLevel);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeSkillLevel.GetType().Name)
                            {
                                Tag = terjeSkillLevel
                            });
                            break;

                        case "Perk Level":
                            TerjeSkillPerk terjeSkillPerk = new TerjeSkillPerk()
                            {
                                skillId = "hunt",
                                perkId = "exphunter",
                                requiredLevel = 25,
                            };
                            condition.items.Add(terjeSkillPerk);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeSkillPerk.GetType().Name)
                            {
                                Tag = terjeSkillPerk
                            });
                            break;

                        case "Specific Players":
                            TerjeSpecificPlayers terjeSpecificPlayers = new TerjeSpecificPlayers()
                            {
                                SpecificPlayer = new BindingList<TerjeSpecificPlayer>()
                            };
                            condition.items.Add(terjeSpecificPlayers);
                            currentTreeNode.Nodes.Add(GetSpecificPlayers(terjeSpecificPlayers));
                            break;

                        case "Custom Condition":
                            TerjeCustomCondition terjeCustomCondition = new TerjeCustomCondition()
                            {
                                classname = "Change Me",
                            };
                            condition.items.Add(terjeCustomCondition);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeCustomCondition.GetType().Name)
                            {
                                Tag = terjeCustomCondition
                            });
                            break;

                        case "Set user variable":
                            TerjeSetUserVariable terjeSetUserVariable = new TerjeSetUserVariable()
                            {
                                name = "myVar1",
                                value = 1
                            };
                            condition.items.Add(terjeSetUserVariable);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeSetUserVariable.GetType().Name)
                            {
                                Tag = terjeSetUserVariable
                            });
                            break;

                        case "Compare user variables : Equal":
                            TerjeComapreUserVariablesEqual terjeComapreUserVariablesEqual = new TerjeComapreUserVariablesEqual()
                            {
                                name = "myVar1",
                                value = 1
                            };
                            condition.items.Add(terjeComapreUserVariablesEqual);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeComapreUserVariablesEqual.GetType().Name)
                            {
                                Tag = terjeComapreUserVariablesEqual
                            });
                            break;

                        case "Compare user variables : NotEqual":
                            TerjeComapreUserVariablesNotEqual terjeComapreUserVariablesNotEqual = new TerjeComapreUserVariablesNotEqual()
                            {
                                name = "myVar1",
                                value = 1
                            };
                            condition.items.Add(terjeComapreUserVariablesNotEqual);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeComapreUserVariablesNotEqual.GetType().Name)
                            {
                                Tag = terjeComapreUserVariablesNotEqual
                            });
                            break;

                        case "Compare user variables : LessThen":
                            TerjeComapreUserVariablesLessThen terjeComapreUserVariablesLessThen = new TerjeComapreUserVariablesLessThen()
                            {
                                name = "myVar1",
                                value = 1
                            };
                            condition.items.Add(terjeComapreUserVariablesLessThen);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeComapreUserVariablesLessThen.GetType().Name)
                            {
                                Tag = terjeComapreUserVariablesLessThen
                            });
                            break;

                        case "Compare user variables : GreaterThen":
                            TerjeComapreUserVariablesGreaterThen terjeComapreUserVariablesGreaterThen = new TerjeComapreUserVariablesGreaterThen()
                            {
                                name = "myVar1",
                                value = 1
                            };
                            condition.items.Add(terjeComapreUserVariablesGreaterThen);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeComapreUserVariablesGreaterThen.GetType().Name)
                            {
                                Tag = terjeComapreUserVariablesGreaterThen
                            });
                            break;

                        case "Compare user variables : LessOrEqual":
                            TerjeComapreUserVariablesLessOrEqual terjeComapreUserVariablesLessOrEqual = new TerjeComapreUserVariablesLessOrEqual()
                            {
                                name = "myVar1",
                                value = 1
                            };
                            condition.items.Add(terjeComapreUserVariablesLessOrEqual);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeComapreUserVariablesLessOrEqual.GetType().Name)
                            {
                                Tag = terjeComapreUserVariablesLessOrEqual
                            });
                            break;

                        case "Compare user variables : GreaterOrEqual":
                            TerjeComapreUserVariablesGreaterOrEqual terjeComapreUserVariablesGreaterOrEqual = new TerjeComapreUserVariablesGreaterOrEqual()
                            {
                                name = "myVar1",
                                value = 1
                            };
                            condition.items.Add(terjeComapreUserVariablesGreaterOrEqual);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeComapreUserVariablesGreaterOrEqual.GetType().Name)
                            {
                                Tag = terjeComapreUserVariablesGreaterOrEqual
                            });
                            break;

                        case "Math with user variables : Sum":
                            TerjeMathWithUserVariableSum terjeMathWithUserVariableSum = new TerjeMathWithUserVariableSum()
                            {
                                name = "myVar1",
                                value = 1
                            };
                            condition.items.Add(terjeMathWithUserVariableSum);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeMathWithUserVariableSum.GetType().Name)
                            {
                                Tag = terjeMathWithUserVariableSum
                            });
                            break;

                        case "Math with user variables : Subtract":
                            TerjeMathWithUserVariableSubtract terjeMathWithUserVariableSubtract = new TerjeMathWithUserVariableSubtract()
                            {
                                name = "myVar1",
                                value = 1
                            };
                            condition.items.Add(terjeMathWithUserVariableSubtract);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeMathWithUserVariableSubtract.GetType().Name)
                            {
                                Tag = terjeMathWithUserVariableSubtract
                            });
                            break;

                        case "Math with user variables : Multiply":
                            TerjeMathWithUserVariableMultiply terjeMathWithUserVariableMultiply = new TerjeMathWithUserVariableMultiply()
                            {
                                name = "myVar1",
                                value = 1
                            };
                            condition.items.Add(terjeMathWithUserVariableMultiply);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeMathWithUserVariableMultiply.GetType().Name)
                            {
                                Tag = terjeMathWithUserVariableMultiply
                            });
                            break;

                        case "Math with user variables : Divide":
                            TerjeMathWithUserVariableDivide terjeMathWithUserVariableDivide = new TerjeMathWithUserVariableDivide()
                            {
                                name = "myVar1",
                                value = 1
                            };
                            condition.items.Add(terjeMathWithUserVariableDivide);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeMathWithUserVariableDivide.GetType().Name)
                            {
                                Tag = terjeMathWithUserVariableDivide
                            });
                            break;

                        case "Special conditions: All":
                            TerjeSpecialConditionsAll terjeSpecialConditionsAll = new TerjeSpecialConditionsAll()
                            {
                                Items = new BindingList<object>()
                            };
                            condition.items.Add(terjeSpecialConditionsAll);
                            currentTreeNode.Nodes.Add(GetSpecialNodes(terjeSpecialConditionsAll as TerjeSpecialConditions));
                            break;

                        case "Special conditions: Any":
                            TerjeSpecialConditionsAny terjeSpecialConditionsAny = new TerjeSpecialConditionsAny()
                            {
                                Items = new BindingList<object>()
                            };
                            condition.items.Add(terjeSpecialConditionsAny);
                            currentTreeNode.Nodes.Add(GetSpecialNodes(terjeSpecialConditionsAny as TerjeSpecialConditions));
                            break;

                        case "Special conditions: One":
                            TerjeSpecialConditionsOne terjeSpecialConditionsOne = new TerjeSpecialConditionsOne()
                            {
                                Items = new BindingList<object>()
                            };
                            condition.items.Add(terjeSpecialConditionsOne);
                            currentTreeNode.Nodes.Add(GetSpecialNodes(terjeSpecialConditionsOne as TerjeSpecialConditions));
                            break;

                        default:

                            break;
                    }
                }
                else if (currentTreeNode.Tag is TerjeSpecialConditions)
                {
                    TerjeSpecialConditions condition = currentTreeNode.Tag as TerjeSpecialConditions;
                    switch (form.Selectedcondition)
                    {
                        case "Timeout":
                            TerjeTimeout newtimeout = new TerjeTimeout()
                            {
                                id = "Test",
                            };
                            condition.Items.Add(newtimeout);
                            currentTreeNode.Nodes.Add(new TreeNode(newtimeout.GetType().Name)
                            {
                                Tag = newtimeout
                            });
                            break;
                        case "Skill Level":
                            TerjeSkillLevel terjeSkillLevel = new TerjeSkillLevel()
                            {
                                skillId = "hunt",
                                requiredLevel = 25,
                            };
                            condition.Items.Add(terjeSkillLevel);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeSkillLevel.GetType().Name)
                            {
                                Tag = terjeSkillLevel
                            });
                            break;

                        case "Perk Level":
                            TerjeSkillPerk terjeSkillPerk = new TerjeSkillPerk()
                            {
                                skillId = "hunt",
                                perkId = "exphunter",
                                requiredLevel = 25,
                            };
                            condition.Items.Add(terjeSkillPerk);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeSkillPerk.GetType().Name)
                            {
                                Tag = terjeSkillPerk
                            });
                            break;

                        case "Specific Players":
                            TerjeSpecificPlayers terjeSpecificPlayers = new TerjeSpecificPlayers()
                            {
                                SpecificPlayer = new BindingList<TerjeSpecificPlayer>()
                            };
                            condition.Items.Add(terjeSpecificPlayers);
                            currentTreeNode.Nodes.Add(GetSpecificPlayers(terjeSpecificPlayers));
                            break;

                        case "Custom Condition":
                            TerjeCustomCondition terjeCustomCondition = new TerjeCustomCondition()
                            {
                                classname = "Change Me",
                            };
                            condition.Items.Add(terjeCustomCondition);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeCustomCondition.GetType().Name)
                            {
                                Tag = terjeCustomCondition
                            });
                            break;

                        case "Set user variable":
                            TerjeSetUserVariable terjeSetUserVariable = new TerjeSetUserVariable()
                            {
                                name = "myVar1",
                                value = 1
                            };
                            condition.Items.Add(terjeSetUserVariable);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeSetUserVariable.GetType().Name)
                            {
                                Tag = terjeSetUserVariable
                            });
                            break;

                        case "Compare user variables : Equal":
                            TerjeComapreUserVariablesEqual terjeComapreUserVariablesEqual = new TerjeComapreUserVariablesEqual()
                            {
                                name = "myVar1",
                                value = 1
                            };
                            condition.Items.Add(terjeComapreUserVariablesEqual);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeComapreUserVariablesEqual.GetType().Name)
                            {
                                Tag = terjeComapreUserVariablesEqual
                            });
                            break;

                        case "Compare user variables : NotEqual":
                            TerjeComapreUserVariablesNotEqual terjeComapreUserVariablesNotEqual = new TerjeComapreUserVariablesNotEqual()
                            {
                                name = "myVar1",
                                value = 1
                            };
                            condition.Items.Add(terjeComapreUserVariablesNotEqual);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeComapreUserVariablesNotEqual.GetType().Name)
                            {
                                Tag = terjeComapreUserVariablesNotEqual
                            });
                            break;

                        case "Compare user variables : LessThen":
                            TerjeComapreUserVariablesLessThen terjeComapreUserVariablesLessThen = new TerjeComapreUserVariablesLessThen()
                            {
                                name = "myVar1",
                                value = 1
                            };
                            condition.Items.Add(terjeComapreUserVariablesLessThen);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeComapreUserVariablesLessThen.GetType().Name)
                            {
                                Tag = terjeComapreUserVariablesLessThen
                            });
                            break;

                        case "Compare user variables : GreaterThen":
                            TerjeComapreUserVariablesGreaterThen terjeComapreUserVariablesGreaterThen = new TerjeComapreUserVariablesGreaterThen()
                            {
                                name = "myVar1",
                                value = 1
                            };
                            condition.Items.Add(terjeComapreUserVariablesGreaterThen);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeComapreUserVariablesGreaterThen.GetType().Name)
                            {
                                Tag = terjeComapreUserVariablesGreaterThen
                            });
                            break;

                        case "Compare user variables : LessOrEqual":
                            TerjeComapreUserVariablesLessOrEqual terjeComapreUserVariablesLessOrEqual = new TerjeComapreUserVariablesLessOrEqual()
                            {
                                name = "myVar1",
                                value = 1
                            };
                            condition.Items.Add(terjeComapreUserVariablesLessOrEqual);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeComapreUserVariablesLessOrEqual.GetType().Name)
                            {
                                Tag = terjeComapreUserVariablesLessOrEqual
                            });
                            break;

                        case "Compare user variables : GreaterOrEqual":
                            TerjeComapreUserVariablesGreaterOrEqual terjeComapreUserVariablesGreaterOrEqual = new TerjeComapreUserVariablesGreaterOrEqual()
                            {
                                name = "myVar1",
                                value = 1
                            };
                            condition.Items.Add(terjeComapreUserVariablesGreaterOrEqual);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeComapreUserVariablesGreaterOrEqual.GetType().Name)
                            {
                                Tag = terjeComapreUserVariablesGreaterOrEqual
                            });
                            break;

                        case "Math with user variables : Sum":
                            TerjeMathWithUserVariableSum terjeMathWithUserVariableSum = new TerjeMathWithUserVariableSum()
                            {
                                name = "myVar1",
                                value = 1
                            };
                            condition.Items.Add(terjeMathWithUserVariableSum);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeMathWithUserVariableSum.GetType().Name)
                            {
                                Tag = terjeMathWithUserVariableSum
                            });
                            break;

                        case "Math with user variables : Subtract":
                            TerjeMathWithUserVariableSubtract terjeMathWithUserVariableSubtract = new TerjeMathWithUserVariableSubtract()
                            {
                                name = "myVar1",
                                value = 1
                            };
                            condition.Items.Add(terjeMathWithUserVariableSubtract);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeMathWithUserVariableSubtract.GetType().Name)
                            {
                                Tag = terjeMathWithUserVariableSubtract
                            });
                            break;

                        case "Math with user variables : Multiply":
                            TerjeMathWithUserVariableMultiply terjeMathWithUserVariableMultiply = new TerjeMathWithUserVariableMultiply()
                            {
                                name = "myVar1",
                                value = 1
                            };
                            condition.Items.Add(terjeMathWithUserVariableMultiply);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeMathWithUserVariableMultiply.GetType().Name)
                            {
                                Tag = terjeMathWithUserVariableMultiply
                            });
                            break;

                        case "Math with user variables : Divide":
                            TerjeMathWithUserVariableDivide terjeMathWithUserVariableDivide = new TerjeMathWithUserVariableDivide()
                            {
                                name = "myVar1",
                                value = 1
                            };
                            condition.Items.Add(terjeMathWithUserVariableDivide);
                            currentTreeNode.Nodes.Add(new TreeNode(terjeMathWithUserVariableDivide.GetType().Name)
                            {
                                Tag = terjeMathWithUserVariableDivide
                            });
                            break;

                        case "Special conditions: All":
                            TerjeSpecialConditionsAll terjeSpecialConditionsAll = new TerjeSpecialConditionsAll()
                            {
                                Items = new BindingList<object>()
                            };
                            condition.Items.Add(terjeSpecialConditionsAll);
                            currentTreeNode.Nodes.Add(GetSpecialNodes(terjeSpecialConditionsAll as TerjeSpecialConditions));
                            break;

                        case "Special conditions: Any":
                            TerjeSpecialConditionsAny terjeSpecialConditionsAny = new TerjeSpecialConditionsAny()
                            {
                                Items = new BindingList<object>()
                            };
                            condition.Items.Add(terjeSpecialConditionsAny);
                            currentTreeNode.Nodes.Add(GetSpecialNodes(terjeSpecialConditionsAny as TerjeSpecialConditions));
                            break;

                        case "Special conditions: One":
                            TerjeSpecialConditionsOne terjeSpecialConditionsOne = new TerjeSpecialConditionsOne()
                            {
                                Items = new BindingList<object>()
                            };
                            condition.Items.Add(terjeSpecialConditionsOne);
                            currentTreeNode.Nodes.Add(GetSpecialNodes(terjeSpecialConditionsOne as TerjeSpecialConditions));
                            break;

                        default:

                            break;
                    }
                }
                SetCorrectFileasDirty();
            }
        }
        private void removeConditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentTreeNode.Parent.Tag is TerjeConditions)
            {
                TerjeConditions condition = currentTreeNode.Parent.Tag as TerjeConditions;
                condition.items.Remove(currentTreeNode.Tag);
            }
            else if (currentTreeNode.Parent.Tag is TerjeSpecialConditions)
            {
                TerjeSpecialConditions sconditions = currentTreeNode.Parent.Tag as TerjeSpecialConditions;
                sconditions.Items.Remove(currentTreeNode.Tag);
            }
            SetCorrectFileasDirty();
            currentTreeNode.Parent.Nodes.Remove(currentTreeNode);
        }
        private void CRSPSkillIDCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedSkill = CRSPskillIDCB.SelectedItem as ComboBoxItem;
            if (selectedSkill != null && perkMap.TryGetValue(selectedSkill.Value, out var perks))
            {
                CRSPperkIDCB.DataSource = perks;
            }
            else
            {
                CRSPperkIDCB.DataSource = null;
            }
            if (!useraction) return;
            TerjeSkillPerk SP = currentTreeNode.Tag as TerjeSkillPerk;
            SP.skillId = selectedSkill.Value;
            if (currentTreeNode.Parent.Parent.Tag is TerjeRecipe)
                TerjeCraftingFiles.isDirty = true;
            else if (currentTreeNode.Parent.Parent.Tag is TerjeLoadout)
                TerjeLoadouts.isDirty = true;
            else if (currentTreeNode.Parent.Parent.Tag is TerjeRespawn)
                TerjeRespawns.isDirty = true;
            else if (currentTreeNode.Parent.Parent.Tag is TerjeFace)
                TerjeFaces.isDirty = true;
        }
        private void CRSPPerkIDCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeSkillPerk SP = currentTreeNode.Tag as TerjeSkillPerk;
            var selectedPerk = CRSPperkIDCB.SelectedItem as ComboBoxItem;
            SP.perkId = selectedPerk.Value;
            if (currentTreeNode.Parent.Parent.Tag is TerjeRecipe)
                TerjeCraftingFiles.isDirty = true;
            else if (currentTreeNode.Parent.Parent.Tag is TerjeLoadout)
                TerjeLoadouts.isDirty = true;
            else if (currentTreeNode.Parent.Parent.Tag is TerjeRespawn)
                TerjeRespawns.isDirty = true;
            else if (currentTreeNode.Parent.Parent.Tag is TerjeFace)
                TerjeFaces.isDirty = true;
        }
        private void CRSPRequiredlevelNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeSkillPerk SP = currentTreeNode.Tag as TerjeSkillPerk;
            SP.requiredLevel = (int)CRSPrequiredlevelNUD.Value;
            if (currentTreeNode.Parent.Parent.Tag is TerjeRecipe)
                TerjeCraftingFiles.isDirty = true;
            else if (currentTreeNode.Parent.Parent.Tag is TerjeLoadout)
                TerjeLoadouts.isDirty = true;
            else if (currentTreeNode.Parent.Parent.Tag is TerjeRespawn)
                TerjeRespawns.isDirty = true;
            else if (currentTreeNode.Parent.Parent.Tag is TerjeFace)
                TerjeFaces.isDirty = true;
        }
        private void addConditionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeConditions tc = new TerjeConditions()
            {
                items = new BindingList<object>()
            };
            if (currentTreeNode.Tag is TerjeLoadout)
            {
                TerjeLoadout loadout = currentTreeNode.Tag as TerjeLoadout;
                loadout.Conditions = tc;
                TerjeLoadouts.isDirty = true;
            }
            else if (currentTreeNode.Tag is TerjeRecipe)
            {
                TerjeRecipe tr = currentTreeNode.Tag as TerjeRecipe;
                tr.Conditions = tc;
                TerjeCraftingFiles.isDirty = true;
            }
            else if (currentTreeNode.Tag is TerjeFace)
            {
                TerjeFace face = currentTreeNode.Tag as TerjeFace;
                face.Conditions = tc;
                TerjeFaces.isDirty = true;
            }
            TreeNode ConditionsNode = new TreeNode("Conditions")
            {
                Tag = tc
            };
            getConditionNodes(tc, ConditionsNode);
            currentTreeNode.Nodes.Add(ConditionsNode);
            currentTreeNode.Expand();
            TerjeTV.SelectedNode = currentTreeNode.LastNode;
        }
        private void removeConditionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeConditions conditions = currentTreeNode.Tag as TerjeConditions;
            if (currentTreeNode.Parent.Tag is TerjeRecipe)
            {
                TerjeRecipe recipe = currentTreeNode.Parent.Tag as TerjeRecipe;
                recipe.Conditions = null;
                TerjeCraftingFiles.isDirty = true;
            }
            else if (currentTreeNode.Parent.Tag is TerjeLoadout)
            {
                TerjeLoadout loadout = currentTreeNode.Parent.Tag as TerjeLoadout;
                loadout.Conditions = null;
                TerjeLoadouts.isDirty = true;
            }
            else if (currentTreeNode.Parent.Tag is TerjeRespawn)
            {
                TerjeRespawn respawn = currentTreeNode.Parent.Tag as TerjeRespawn;
                respawn.Conditions = null;
                TerjeRespawns.isDirty = true;
            }
            else if (currentTreeNode.Parent.Tag is TerjeFace)
            {
                TerjeFace face = currentTreeNode.Parent.Tag as TerjeFace;
                face.Conditions = null;
                TerjeFaces.isDirty = true;
            }
            currentTreeNode.Parent.Nodes.Remove(currentTreeNode);
        }
        private void addNewScriptableAreaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddnewTerjeScriptableArea form = new AddnewTerjeScriptableArea();
            if (form.ShowDialog() == DialogResult.OK)
            {
                TerjeScriptableArea newarea = new TerjeScriptableArea()
                {
                    Active = 0,
                    Classname = form.SelectedAreaType,
                    Position = "0 0 0",
                    SpawnChance = 1,
                };
                if (newarea.Classname == "TerjePsionicScriptableArea" || newarea.Classname == "TerjeRadioactiveScriptableArea")
                {
                    newarea.Data = new TerjeScriptableAreaData()
                    {
                        OuterRadiusSpecified = true,
                        OuterRadius = 100,
                        InnerRadiusSpecified = true,
                        InnerRadius = 50,
                        HeightMin = -100,
                        HeightMax = 100,
                        Power = 5
                    };
                }
                else if (newarea.Classname == "TerjeExperienceModScriptableArea")
                {
                    newarea.FilterSpecified = true;
                    newarea.Filter = "";
                    newarea.Data = new TerjeScriptableAreaData()
                    {
                        RadiusSpecified = true,
                        Radius = 100,
                        HeightMin = -100,
                        HeightMax = 100,
                        Power = 5
                    };
                }
                TerjeScriptableAreas.Area.Add(newarea);
                currentTreeNode.Nodes.Add(getScriptablearea(newarea));
                TerjeScriptableAreas.isDirty = true;
            }
        }
        private void removeScriptableAreaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeScriptableArea area = currentTreeNode.Tag as TerjeScriptableArea;
            TerjeScriptableAreas.Area.Remove(area);
            currentTreeNode.Parent.Nodes.Remove(currentTreeNode);
            TerjeScriptableAreas.isDirty = true;
        }
        private void SAActiveCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeScriptableArea area = currentTreeNode.Tag as TerjeScriptableArea;
            area.Active = SAActiveCB.Checked == true ? 1 : 0;
            TerjeScriptableAreas.isDirty = true;
        }
        private void SAClassnameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
        }
        private void SAPosXNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeScriptableArea area = currentTreeNode.Tag as TerjeScriptableArea;
            area.PositionVec3.X = (float)SAPosXNUD.Value;
            TerjeScriptableAreas.isDirty = true;
        }
        private void SAPosYNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeScriptableArea area = currentTreeNode.Tag as TerjeScriptableArea;
            area.PositionVec3.Y = (float)SAPosYNUD.Value;
            TerjeScriptableAreas.isDirty = true;
        }
        private void SAPosZNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeScriptableArea area = currentTreeNode.Tag as TerjeScriptableArea;
            area.PositionVec3.Z = (float)SAPosZNUD.Value;
            TerjeScriptableAreas.isDirty = true;
        }
        private void SASpawnChanceNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeScriptableArea area = currentTreeNode.Tag as TerjeScriptableArea;
            area.SpawnChance = (int)SASpawnChanceNUD.Value;
            TerjeScriptableAreas.isDirty = true;
        }
        private void SkillsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            string currentSelectedSkills = GetCheckedSkillsCsv(SAFLP);
            TerjeScriptableArea area = currentTreeNode.Tag as TerjeScriptableArea;
            area.Filter = currentSelectedSkills;
            TerjeScriptableAreas.isDirty = true;
        }
        private string GetCheckedSkillsCsv(FlowLayoutPanel groupBox)
        {
            return string.Join(",",
                groupBox.Controls.OfType<CheckBox>()
                    .Where(cb => cb.Checked)
                    .Select(cb => cb.Tag.ToString()));
        }
        private void SADOuterRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeScriptableAreaData data = currentTreeNode.Tag as TerjeScriptableAreaData;
            data.OuterRadius = (int)SADOuterRadiusNUD.Value;
            TerjeScriptableAreas.isDirty = true;
        }
        private void SADInnerRadiusNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeScriptableAreaData data = currentTreeNode.Tag as TerjeScriptableAreaData;
            if (data.InnerRadiusSpecified)
                data.InnerRadius = (int)SADInnerRadiusNUD.Value;
            else if (data.RadiusSpecified)
                data.Radius = (int)SADInnerRadiusNUD.Value;
            TerjeScriptableAreas.isDirty = true;
        }
        private void SADHeightMInNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeScriptableAreaData data = currentTreeNode.Tag as TerjeScriptableAreaData;
            data.HeightMin = (int)SADHeightMInNUD.Value;
            TerjeScriptableAreas.isDirty = true;
        }
        private void SADHeightMaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeScriptableAreaData data = currentTreeNode.Tag as TerjeScriptableAreaData;
            data.HeightMax = (int)SADHeightMaxNUD.Value;
            TerjeScriptableAreas.isDirty = true;
        }
        private void SADPowerNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeScriptableAreaData data = currentTreeNode.Tag as TerjeScriptableAreaData;
            data.Power = (int)SADPowerNUD.Value;            
            TerjeScriptableAreas.isDirty = true;
        }
        private void SSLdisplayNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadout loadout = currentTreeNode.Tag as TerjeLoadout;
            loadout.displayName = SSLdisplayNameTB.Text;
            currentTreeNode.Text = loadout.displayName;
            TerjeLoadouts.isDirty = true;
        }
        private void SSLidTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadout loadout = currentTreeNode.Tag as TerjeLoadout;
            loadout.id = SSLidTB.Text;
            TerjeLoadouts.isDirty = true;
        }
        private void darkButton2_Click(object sender, EventArgs e)
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
                    SCLItemsClassnameTB.Text = l;
                }
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        private void SCLItemsClassnameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.classname = SCLItemsClassnameTB.Text;
            currentTreeNode.Text = $"Classname:{litem.classname}";
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsDisplayNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.displayName = SCLItemsdisplayNameTB.Text;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsQuantityTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.quantity = SCLItemsquantityTB.Text;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsCountTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.count = SCLItemscountTB.Text;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsHealthTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.health = SCLItemshealthTB.Text;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsPositionTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.position = SCLItemspositionTB.Text;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsLiquidTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.liquid = SCLItemsliquidTB.Text;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsTemperatureTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.temperature = SCLItemstemperatureTB.Text;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsFoodStageCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.foodStage = SCLItemsfoodStageCB.GetItemText(SCLItemsfoodStageCB.SelectedItem);
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsDisinfectedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.disinfected = SCLItemsdisinfectedCB.Checked == true ? 1 : 0;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsAgentsTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.liquid = SCLItemsliquidTB.Text;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsQuickbarNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.quickbar = (int)SCLItemsquickbarNUD.Value;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsAmmoTypeTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.ammoType = SCLItemsammoTypeTB.Text;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsammoCountTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.ammoCount = SCLItemsammoCountTB.Text;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsCostNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
            litem.cost = (int)SCLItemscostNUD.Value;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLItemsSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CheckBox check = sender as CheckBox;
            string namespecified = check.Name;
            string name = check.Tag.ToString();
            foreach (Control control in SCLItemsGB.Controls)
            {
                if (control.Name == name)
                {
                    TerjeLoadoutItem litem = currentTreeNode.Tag as TerjeLoadoutItem;
                    SetPropertyValue(litem, namespecified.Substring(8, namespecified.Length - 8 - 2), control.Visible = check.Checked);
                    if (control is TextBox)
                    {
                        TextBox tb = control as TextBox;
                        string n = name.Substring(8, name.Length - 8 - 2);
                        object value = GetPropertyValue(litem, n);
                        if (value == null)
                            tb.Text = "Change Me";
                        else
                            tb.Text = value.ToString();
                    }
                    else if(control is NumericUpDown)
                    {
                        NumericUpDown nud = control as NumericUpDown;
                        string n = name.Substring(8, name.Length - 8 - 3);
                        object value = GetPropertyValue(litem, n);
                        nud.Value = (int)value;
                    }
                    else if(control is CheckBox)
                    {
                        CheckBox cb = control as CheckBox;
                        string n = name.Substring(8, name.Length - 8 - 2);
                        object value = GetPropertyValue(litem, n);
                        if (value == null)
                            cb.Checked = true;
                        else
                            cb.Checked = (int)value ==1 ? true:false;
                    }
                    else if (control is ComboBox)
                    {
                        ComboBox cb = control as ComboBox;
                        string n = name.Substring(8, name.Length - 8 - 2);
                        object value = GetPropertyValue(litem, n);
                        if (value == null)
                            cb.SelectedIndex = 0;
                        else
                            cb.SelectedItem = value.ToString();
                    }
                }
            }
            TerjeLoadouts.isDirty = true;
        }
        private void SCLSelectorSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CheckBox check = sender as CheckBox;
            string namespecified = check.Name;
            string name = check.Tag.ToString();
            GetControls(SCLSelectorGB, check, namespecified, name);
            TerjeLoadouts.isDirty = true;
        }
        private void GetControls(GroupBox groupbox, CheckBox check, string namespecified, string name)
        {
            foreach (Control control in groupbox.Controls)
            {
                if (control is GroupBox)
                {
                    GroupBox box = control as GroupBox;
                    GetControls(box, check, namespecified, name);
                }
                else if (control.Name == name)
                {
                    TerjeLoadoutSelector selector = currentTreeNode.Tag as TerjeLoadoutSelector;
                    SetPropertyValue(selector, namespecified.Substring(11, namespecified.Length - 11 - 2), control.Visible = check.Checked);
                    if (control is TextBox)
                    {
                        TextBox tb = control as TextBox;
                        string n = name.Substring(11, name.Length - 11 - 2);
                        object value = GetPropertyValue(selector, n);
                        if (value == null)
                            tb.Text = "Change Me";
                        else
                            tb.Text = value.ToString();

                    }
                    else if (control is NumericUpDown)
                    {
                        NumericUpDown nud = control as NumericUpDown;
                        string n = name.Substring(11, name.Length - 11 - 3);
                        object value = GetPropertyValue(selector, n);
                        nud.Value = (int)value;
                    }
                    else if (control is CheckBox)
                    {
                        CheckBox cb = control as CheckBox;
                        string n = name.Substring(11, name.Length - 11 - 2);
                        object value = GetPropertyValue(selector, n);
                        if (value == null)
                            cb.Checked = true;
                        else
                            cb.Checked = (int)value == 1 ? true : false;
                    }
                    else if (control is ComboBox)
                    {
                        ComboBox cb = control as ComboBox;
                        string n = name.Substring(11, name.Length - 11 - 2);
                        object value = GetPropertyValue(selector, n);
                        if (value == null)
                            cb.SelectedIndex = 0;
                        else
                            cb.SelectedItem = value.ToString();
                    }
                }
            }
        }
        static object GetPropertyValue(object obj, string propertyName)
        {
            if (obj == null || string.IsNullOrEmpty(propertyName))
                return null;

            PropertyInfo prop = obj.GetType().GetProperty(propertyName);
            if (prop == null)
                return null;

            return prop.GetValue(obj, null);
        }
        static void SetPropertyValue(object obj, string propertyName, object value)
        {
            if (obj == null || string.IsNullOrEmpty(propertyName))
                return;

            PropertyInfo prop = obj.GetType().GetProperty(propertyName);
            if (prop == null || !prop.CanWrite)
                return;

            prop.SetValue(obj, value);
        }
        private void SCLSelectortypeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutSelector selector = currentTreeNode.Tag as TerjeLoadoutSelector;
            selector.type = SCLSelectortypeCB.GetItemText(SCLSelectortypeCB.SelectedItem);
            if (SCLSelectorMultipleGB.Visible = selector.type == "MULTIPLE")
            {
                if (SCLSelectorpointsCountSpecifiedCB.Checked = SCLSelectorpointsCountNUD.Visible = selector.pointsCountSpecified)
                    SCLSelectorpointsCountNUD.Value = selector.pointsCount;
                if (SCLSelectorpointsHandlerSpecifiedCB.Checked = SCLSelectorpointsHandlerTB.Visible = selector.pointsHandlerSpecified)
                    SCLSelectorpointsHandlerTB.Text = selector.pointsHandler;
                if (SCLSelectorpointsIconSpecifiedCB.Checked = SCLSelectorpointsIconTB.Visible = selector.pointsIconSpecified)
                    SCLSelectorpointsIconTB.Text = selector.pointsIcon;
            }
            else
            {
                SCLSelectorpointsCountSpecifiedCB.Checked = SCLSelectorpointsCountNUD.Visible = SCLSelectorpointsHandlerTB.Visible = SCLSelectorpointsIconTB.Visible = false;
            }
            currentTreeNode.Text = $"Selector Type:{selector.type} , Display Name:{selector.displayName}";
            TerjeLoadouts.isDirty = true;
        }
        private void SCLSelectordisplayNameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutSelector selector = currentTreeNode.Tag as TerjeLoadoutSelector;
            selector.displayName = SCLSelectordisplayNameTB.Text;
            currentTreeNode.Text = $"Selector Type:{selector.type} , Display Name:{selector.displayName}";
            TerjeLoadouts.isDirty = true;
        }
        private void SCLSelectorpointsCountNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutSelector selector = currentTreeNode.Tag as TerjeLoadoutSelector;
            selector.pointsCount = (int)SCLSelectorpointsCountNUD.Value;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLSelectorpointsHandlerTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutSelector selector = currentTreeNode.Tag as TerjeLoadoutSelector;
            selector.pointsHandler = SCLSelectorpointsHandlerTB.Text;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLSelectorpointsIconTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutSelector selector = currentTreeNode.Tag as TerjeLoadoutSelector;
            selector.pointsIcon = SCLSelectorpointsIconTB.Text;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLGroupcostSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutGroup group = currentTreeNode.Tag as TerjeLoadoutGroup;
            group.costSpecified = SCLGroupcostNUD.Visible = SCLGroupcostSpecifiedCB.Checked;
            SCLGroupcostNUD.Value = group.cost;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLGroupcostNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeLoadoutGroup group = currentTreeNode.Tag as TerjeLoadoutGroup;
            group.cost = (int)SCLGroupcostNUD.Value;
            TerjeLoadouts.isDirty = true;
        }
        private void SCLTimeoutidTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeTimeout timeout = currentTreeNode.Tag as TerjeTimeout;
            timeout.id = SCLTimeoutidTB.Text;
            SetCorrectFileasDirty();
        }
        private void SCLTimeouthoursNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeTimeout timeout = currentTreeNode.Tag as TerjeTimeout;
            timeout.hours = (int)SCLTimeouthoursNUD.Value;
            SetCorrectFileasDirty();
        }
        private void SCLTimeoutminutesNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeTimeout timeout = currentTreeNode.Tag as TerjeTimeout;
            timeout.minutes = (int)SCLTimeoutminutesNUD.Value;
            SetCorrectFileasDirty();
        }
        private void SCLTimeoutsecondsNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeTimeout timeout = currentTreeNode.Tag as TerjeTimeout;
            timeout.seconds = (int)SCLTimeoutsecondsNUD.Value;
            SetCorrectFileasDirty();
        }
        private void SCLTimeoutSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            CheckBox check = sender as CheckBox;
            string namespecified = check.Name;
            string name = check.Tag.ToString();
            foreach (Control control in SCLtimeoutGB.Controls)
            {
                if (control.Name == name)
                {
                    TerjeTimeout timeout = currentTreeNode.Tag as TerjeTimeout;
                    SetPropertyValue(timeout, namespecified.Substring(10, namespecified.Length - 10 - 2), control.Visible = check.Checked);
                    if (control is TextBox)
                    {
                        TextBox tb = control as TextBox;
                        string n = name.Substring(10, name.Length - 10 - 2);
                        object value = GetPropertyValue(timeout, n);
                        if (value == null)
                            tb.Text = "Change Me";
                        else
                            tb.Text = value.ToString();
                    }
                    else if (control is NumericUpDown)
                    {
                        NumericUpDown nud = control as NumericUpDown;
                        string n = name.Substring(10, name.Length - 10 - 3);
                        object value = GetPropertyValue(timeout, n);
                        nud.Value = (int)value;
                    }
                    else if (control is CheckBox)
                    {
                        CheckBox cb = control as CheckBox;
                        string n = name.Substring(10, name.Length - 10 - 2);
                        object value = GetPropertyValue(timeout, n);
                        if (value == null)
                            cb.Checked = true;
                        else
                            cb.Checked = (int)value == 1 ? true : false;
                    }
                    else if (control is ComboBox)
                    {
                        ComboBox cb = control as ComboBox;
                        string n = name.Substring(10, name.Length - 10 - 2);
                        object value = GetPropertyValue(timeout, n);
                        if (value == null)
                            cb.SelectedIndex = 0;
                        else
                            cb.SelectedItem = value.ToString();
                    }
                }
            }
            SetCorrectFileasDirty();
        }
        private void SCLConditionCustomConditionclassnameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeCustomCondition custcon = currentTreeNode.Tag as TerjeCustomCondition;
            custcon.classname = SCLConditionCustomConditionclassnameTB.Text;
            SetCorrectFileasDirty();

        }
        private void addNewLoadoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeLoadout loadout = new TerjeLoadout()
            {
                id = "New Loadout, Change me to something short and unique",
                displayName = "New Loadout, Change Me"
            };
            TerjeLoadouts.Loadout.Add(loadout);
            TerjeLoadouts.isDirty = true;
            currentTreeNode.Nodes.Add(GetLoadoutNodes(loadout));
        }
        private void removeLoadoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeLoadouts.Loadout.Remove(currentTreeNode.Tag as TerjeLoadout);
            currentTreeNode.Parent.Nodes.Remove(currentTreeNode);
            TerjeLoadouts.isDirty = true;
        }
        private void CONDExtraOptionshideOwnerWhenFalseCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeConditionBase condbase = currentTreeNode.Tag as TerjeConditionBase;
            condbase.hideOwnerWhenFalse = CONDExtraOptionshideOwnerWhenFalseCB.Checked == true?1:0;
            SetCorrectFileasDirty();
        }
        private void CONDExtraOptionsdisplayTextTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeConditionBase condbase = currentTreeNode.Tag as TerjeConditionBase;
            condbase.displayText = CONDExtraOptionsdisplayTextTB.Text;
            SetCorrectFileasDirty();
        }
        private void CONDExtraOptionssuccessTextTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeConditionBase condbase = currentTreeNode.Tag as TerjeConditionBase;
            condbase.successText = CONDExtraOptionssuccessTextTB.Text;
            SetCorrectFileasDirty();
        }
        private void CONDExtraOptionsfailTextTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeConditionBase condbase = currentTreeNode.Tag as TerjeConditionBase;
            condbase.failText = CONDExtraOptionsfailTextTB.Text;
            SetCorrectFileasDirty();
        }
        private void CONDExtraOptionshideOwnerWhenFalseSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeConditionBase condition = currentTreeNode.Tag as TerjeConditionBase;
            condition.hideOwnerWhenFalseSpecified = CONDExtraOptionshideOwnerWhenFalseCB.Visible = CONDExtraOptionshideOwnerWhenFalseSpecifiedCB.Checked;
            CONDExtraOptionshideOwnerWhenFalseCB.Checked = condition.hideOwnerWhenFalse == 1 ? true : false;
            SetCorrectFileasDirty();
        }
        private void CONDExtraOptionsdisplayTextSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeConditionBase condition = currentTreeNode.Tag as TerjeConditionBase;
            condition.displayTextSpecified = CONDExtraOptionsdisplayTextTB.Visible = CONDExtraOptionsdisplayTextSpecifiedCB.Checked;
            if (condition.displayText == null)
                condition.displayText = CONDExtraOptionsdisplayTextTB.Text = "Change Me";
            else
                CONDExtraOptionsdisplayTextTB.Text = condition.displayText;
            SetCorrectFileasDirty();
        }
        private void CONDExtraOptionssuccessTextSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeConditionBase condition = currentTreeNode.Tag as TerjeConditionBase;
            condition.successTextSpecified = CONDExtraOptionssuccessTextTB.Visible = CONDExtraOptionssuccessTextSpecifiedCB.Checked;
            if (condition.successText == null)
                condition.successText = CONDExtraOptionssuccessTextTB.Text = "Change Me";
            else
                CONDExtraOptionssuccessTextTB.Text = condition.successText;
            SetCorrectFileasDirty();
        }
        private void CONDExtraOptionsfailTextSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeConditionBase condition = currentTreeNode.Tag as TerjeConditionBase;
            condition.failTextSpecified = CONDExtraOptionsfailTextTB.Visible = CONDExtraOptionsfailTextSpecifiedCB.Checked;
            if (condition.failText == null)
                condition.failText = CONDExtraOptionsfailTextTB.Text = "Change Me";
            else
                CONDExtraOptionsfailTextTB.Text = condition.failText;
            SetCorrectFileasDirty();
        }
        private void CONDExtraVariablesGB_Enter(object sender, EventArgs e)
        {
           
        }
        private void CONDExtraVariablesnameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (currentTreeNode.Tag is TerjeSetUserVariable)
            {
                TerjeSetUserVariable condition = currentTreeNode.Tag as TerjeSetUserVariable;
                condition.name = CONDExtraVariablesnameTB.Text;
            }
            else if (currentTreeNode.Tag is TerjeComapreUserVariables)
            {
                TerjeComapreUserVariables condition = currentTreeNode.Tag as TerjeComapreUserVariables;
                condition.name = CONDExtraVariablesnameTB.Text;
            }
            SetCorrectFileasDirty();
        }
        private void CONDExtraVariablesvalueNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (currentTreeNode.Tag is TerjeSetUserVariable)
            {
                TerjeSetUserVariable condition = currentTreeNode.Tag as TerjeSetUserVariable;
                condition.value = (int)CONDExtraVariablesvalueNUD.Value;
            }
            else if (currentTreeNode.Tag is TerjeComapreUserVariables)
            {
                TerjeComapreUserVariables condition = currentTreeNode.Tag as TerjeComapreUserVariables;
                condition.value = (int)CONDExtraVariablesvalueNUD.Value;
            }
            SetCorrectFileasDirty();
        }
        private void CONDExtraVariablesPersistCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (currentTreeNode.Tag is TerjeSetUserVariable)
            {
                TerjeSetUserVariable condition = currentTreeNode.Tag as TerjeSetUserVariable;
                condition.persist = CONDExtraVariablesPersistCB.Checked == true ? 1 : 0;
            }
            else if (currentTreeNode.Tag is TerjeComapreUserVariables)
            {
                TerjeComapreUserVariables condition = currentTreeNode.Tag as TerjeComapreUserVariables;
                condition.persist = CONDExtraVariablesPersistCB.Checked == true ? 1 : 0;
            }
            SetCorrectFileasDirty();
        }
        private void CONDExtraVariablesPersistSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            if (currentTreeNode.Tag is TerjeSetUserVariable)
            {
                TerjeSetUserVariable condition = currentTreeNode.Tag as TerjeSetUserVariable;
                condition.persistSpecified = CONDExtraVariablesPersistCB.Visible = CONDExtraVariablesPersistSpecifiedCB.Checked;
                CONDExtraVariablesPersistCB.Checked = condition.persist == 1 ? true : false;
            }
            else if(currentTreeNode.Tag is TerjeComapreUserVariables)
            {
                TerjeComapreUserVariables condition = currentTreeNode.Tag as TerjeComapreUserVariables;
                condition.persistSpecified = CONDExtraVariablesPersistCB.Visible = CONDExtraVariablesPersistSpecifiedCB.Checked;
                CONDExtraVariablesPersistCB.Checked = condition.persist == 1 ? true : false;
            }
            SetCorrectFileasDirty();
        }
        private void CONDExtraMathnameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;

            TerjeMathWithUserVariable condition = currentTreeNode.Tag as TerjeMathWithUserVariable;
            condition.name = CONDExtraVariablesnameTB.Text;
            SetCorrectFileasDirty();

        }
        private void CONDExtraMathvalueNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;

            TerjeMathWithUserVariable condition = currentTreeNode.Tag as TerjeMathWithUserVariable;
            condition.value = (int)CONDExtraVariablesvalueNUD.Value;
            SetCorrectFileasDirty();
        }
        private void CONDExtraMathminSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeMathWithUserVariable condition = currentTreeNode.Tag as TerjeMathWithUserVariable;
            condition.minSpecified = CONDExtraMathminNUD.Visible = CONDExtraMathminSpecifiedCB.Checked;
            CONDExtraMathminNUD.Value = condition.min;
            SetCorrectFileasDirty();
        }
        private void CONDExtraMathminNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;

            TerjeMathWithUserVariable condition = currentTreeNode.Tag as TerjeMathWithUserVariable;
            condition.min = (int)CONDExtraMathminNUD.Value;
            SetCorrectFileasDirty();
        }
        private void CONDExtraMathmaxSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeMathWithUserVariable condition = currentTreeNode.Tag as TerjeMathWithUserVariable;
            condition.maxSpecified = CONDExtraMathmaxNUD.Visible = CONDExtraMathmaxSpecifiedCB.Checked;
            CONDExtraMathmaxNUD.Value = condition.max;
            SetCorrectFileasDirty();
        }
        private void CONDExtraMathmaxNUD_ValueChanged(object sender, EventArgs e)
        {
            if (!useraction) return;

            TerjeMathWithUserVariable condition = currentTreeNode.Tag as TerjeMathWithUserVariable;
            condition.max = (int)CONDExtraMathmaxNUD.Value;
            SetCorrectFileasDirty();
        }
        private void CONDExtraMathPersistCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;

            TerjeMathWithUserVariable condition = currentTreeNode.Tag as TerjeMathWithUserVariable;
            condition.persist = CONDExtraMathPersistCB.Checked == true ? 1 : 0;
            SetCorrectFileasDirty();
        }
        private void CONDExtraMathPersistSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeMathWithUserVariable condition = currentTreeNode.Tag as TerjeMathWithUserVariable;
            condition.persistSpecified = CONDExtraMathPersistCB.Visible = CONDExtraMathPersistSpecifiedCB.Checked;
            CONDExtraMathPersistCB.Checked = condition.persist == 1 ? true : false;
            SetCorrectFileasDirty();
        }
        private void addItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeLoadout loadout = currentTreeNode.Tag as TerjeLoadout;
            loadout.Items = new TerjeLoadoutItems()
            {
                Items = new BindingList<object>()
            };
            currentTreeNode.Nodes.Add(GetLoadoutItems(loadout));
            TerjeLoadouts.isDirty = true;

        }
        private void removeItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeLoadout loadout = currentTreeNode.Parent.Tag as TerjeLoadout;
            loadout.Items = null;
            currentTreeNode.Parent.Nodes.Remove(currentTreeNode);
            TerjeLoadouts.isDirty = true;
        }
        private void addItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddItemfromTypes form = new AddItemfromTypes
            {
                vanillatypes = vanillatypes,
                ModTypes = ModTypes,
                currentproject = currentproject,
                UseOnlySingleitem = false
            };
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<string> addedtypes = form.addedtypes.ToList();
                if (currentTreeNode.Tag is TerjeLoadoutItems)
                {
                    TerjeLoadoutItems itemlist = currentTreeNode.Tag as TerjeLoadoutItems;
                    foreach (string l in addedtypes)
                    {
                        TerjeLoadoutItem newitem = new TerjeLoadoutItem()
                        {
                            classname = l,
                            classnameSpecified = true

                        };
                        itemlist.Items.Add(newitem);
                        currentTreeNode.Nodes.Add(GetLoadoutitem(newitem));
                    }
                }
                else if (currentTreeNode.Tag is TerjeLoadoutItem)
                {
                    TerjeLoadoutItem currentitem = currentTreeNode.Tag as TerjeLoadoutItem;
                    if (currentitem.Item == null)
                        currentitem.Item = new BindingList<TerjeLoadoutItem>();
                    foreach (string l in addedtypes)
                    {
                        TerjeLoadoutItem newitem = new TerjeLoadoutItem()
                        {
                            classname = l,
                            classnameSpecified = true

                        };
                        currentitem.Item.Add(newitem);
                        currentTreeNode.Nodes.Add(GetLoadoutitem(newitem));
                    }
                }
                else if (currentTreeNode.Tag is TerjeLoadoutGroup)
                {
                    TerjeLoadoutGroup group = currentTreeNode.Tag as TerjeLoadoutGroup;
                    if (group.Item == null)
                        group.Item = new BindingList<TerjeLoadoutItem>();
                    foreach (string l in addedtypes)
                    {
                        TerjeLoadoutItem newitem = new TerjeLoadoutItem()
                        {
                            classname = l,
                            classnameSpecified = true

                        };
                        group.Item.Add(newitem);
                        currentTreeNode.Nodes.Add(GetLoadoutitem(newitem));
                    }
                }
                else if (currentTreeNode.Tag is TerjeLoadoutSelector)
                {
                    TerjeLoadoutSelector selector = currentTreeNode.Tag as TerjeLoadoutSelector;
                    if (selector.Item == null)
                        selector.Item = new BindingList<TerjeLoadoutItem>();
                    foreach (string l in addedtypes)
                    {
                        TerjeLoadoutItem newitem = new TerjeLoadoutItem()
                        {
                            classname = l,
                            classnameSpecified = true

                        };
                        selector.Item.Add(newitem);
                        currentTreeNode.Nodes.Add(GetLoadoutitem(newitem));
                    }
                }
                TerjeLoadouts.isDirty = true;
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }

        }
        private void removeItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeLoadoutItem currentitem = currentTreeNode.Tag as TerjeLoadoutItem;
            if(currentTreeNode.Parent.Tag is TerjeLoadoutItems)
            {
                TerjeLoadoutItems items = currentTreeNode.Parent.Tag as TerjeLoadoutItems;
                items.Items.Remove(currentitem);
            }
            else if(currentTreeNode.Parent.Tag is TerjeLoadoutItem)
            {
                TerjeLoadoutItem items = currentTreeNode.Parent.Tag as TerjeLoadoutItem;
                items.Item.Remove(currentitem);
            }
            else if(currentTreeNode.Parent.Tag is TerjeLoadoutSelector)
            {
                TerjeLoadoutSelector items = currentTreeNode.Parent.Tag as TerjeLoadoutSelector;
                items.Item.Remove(currentitem);
            }
            else if (currentTreeNode.Parent.Tag is TerjeLoadoutGroup)
            {
                TerjeLoadoutGroup items = currentTreeNode.Parent.Tag as TerjeLoadoutGroup;
                items.Item.Remove(currentitem);
            }
            currentTreeNode.Parent.Nodes.Remove(currentTreeNode);
            TerjeLoadouts.isDirty = true;
        }
        private void addSelectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeLoadoutItems items = currentTreeNode.Tag as TerjeLoadoutItems;
            TerjeLoadoutSelector selector = new TerjeLoadoutSelector()
            {
                type = "SINGLE"
            };
            if (items.Items == null)
                items.Items = new BindingList<object>();
            items.Items.Add(selector);
            currentTreeNode.Nodes.Add(GetloadoutSleector(selector));
            TerjeLoadouts.isDirty = true;
        }
        private void removeSelectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeLoadoutSelector selector = currentTreeNode.Tag as TerjeLoadoutSelector;
            TerjeLoadoutItems item = currentTreeNode.Parent.Tag as TerjeLoadoutItems;
            item.Items.Remove(selector);
            currentTreeNode.Parent.Nodes.Remove(currentTreeNode);
            TerjeLoadouts.isDirty = true;
        }
        private void addGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeLoadoutSelector selector = currentTreeNode.Tag as TerjeLoadoutSelector;
            if (selector.Group == null)
                selector.Group = new BindingList<TerjeLoadoutGroup>();
            TerjeLoadoutGroup newgroup = new TerjeLoadoutGroup();
            selector.Group.Add(newgroup);
            currentTreeNode.Nodes.Add(GetLoadoutGroups(newgroup));
            TerjeLoadouts.isDirty = true;
        }
        private void removeGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerjeLoadoutGroup group = currentTreeNode.Tag as TerjeLoadoutGroup;
            TerjeLoadoutSelector selector = currentTreeNode.Parent.Tag as TerjeLoadoutSelector;
            selector.Group.Remove(group);
            if (selector.Group.Count < 1)
                selector.Group = null;
            currentTreeNode.Parent.Nodes.Remove(currentTreeNode);
            TerjeLoadouts.isDirty = true;
        }
        private void SCFaceclassnameTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeFace face = currentTreeNode.Tag as TerjeFace;
            face.classname = SCFaceclassnameTB.Text;
            TerjeFaces.isDirty = true;
        }
        private void SCFaceiconTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeFace face = currentTreeNode.Tag as TerjeFace;
            face.icon = SCFaceiconTB.Text;
            TerjeFaces.isDirty = true;
        }
        private void SCFacebackgroundTB_TextChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeFace face = currentTreeNode.Tag as TerjeFace;
            face.background = SCFacebackgroundTB.Text;
            TerjeFaces.isDirty = true;
        }
        private void SCFacebackgroundSpecifiedCB_CheckedChanged(object sender, EventArgs e)
        {
            if (!useraction) return;
            TerjeFace face = currentTreeNode.Tag as TerjeFace;
            face.backgroundSpecified = SCFacebackgroundTB.Visible = SCFacebackgroundSpecifiedCB.Checked;
            if (face.background == null)
                face.background = SCFacebackgroundTB.Text = "Change Me";
            else
                SCFacebackgroundTB.Text = face.background;
            TerjeFaces.isDirty = true;
        }
        private void SetCorrectFileasDirty()
        {
            TreeNode node = currentTreeNode;
            while (node != null)
            {
                if (node.Tag is string tagStr && tagStr == "Root")
                {
                    break;
                }

                if (node.Tag is TerjeRecipe)
                {
                    TerjeCraftingFiles.isDirty = true;
                    return;
                }

                if (node.Tag is TerjeLoadout)
                {
                    TerjeLoadouts.isDirty = true;
                    return;
                }

                if (node.Tag is TerjeRespawn)
                {
                    TerjeRespawns.isDirty = true;
                    return;
                }

                if (node.Tag is TerjeFace)
                {
                    TerjeFaces.isDirty = true;
                    return;
                }

                node = node.Parent;
            }
        }
    }
    public class ComboBoxItem
    {
        public string Display { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return Display;
        }
    }
}
