using DarkUI.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DayZeEditor
{
    public partial class P2PMarketInfo : DarkForm
    {
        public P2PMarketInfo()
        {
            InitializeComponent();
            InitializeForm_Controls
            (
                this,
                panel1,
                TitleLabel
            );
        }
        public static Size Formsize;
        private static Button B;
        private static Panel P;
        private static Label L;
        private static Form F;
        private static bool mouseDown;
        private static Point lastLocation;
        public static void InitializeForm_Controls(Form _F, Panel _P, Label _L, Button _B = null)
        {
            F = _F;
            P = _P;

            B = _B;
            L = _L;
            P.MouseDoubleClick += new MouseEventHandler(FormMax_MouseDoubleClick);
            P.MouseDown += new MouseEventHandler(FormMove_MouseDown);
            P.MouseMove += new MouseEventHandler(FormMove_MouseMove);
            P.MouseUp += new MouseEventHandler(FormMove_MouseUp);
            L.MouseDoubleClick += new MouseEventHandler(FormMax_MouseDoubleClick);
            L.MouseDown += new MouseEventHandler(FormMove_MouseDown);
            L.MouseMove += new MouseEventHandler(FormMove_MouseMove);
            L.MouseUp += new MouseEventHandler(FormMove_MouseUp);
            if (_B != null)
                B.Click += new System.EventHandler(FormClose_Click);
            Formsize = F.Size;
        }
        private static void FormMove_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }
        private static void FormMove_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                if (F.WindowState == System.Windows.Forms.FormWindowState.Maximized)
                    F.WindowState = System.Windows.Forms.FormWindowState.Normal;
                F.Location = new Point(
                    (F.Location.X - lastLocation.X) + e.X, (F.Location.Y - lastLocation.Y) + e.Y);

                F.Update();
            }
        }
        private static void FormMove_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
        private static void FormMax_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                if (F.WindowState == System.Windows.Forms.FormWindowState.Maximized)
                {
                    F.WindowState = System.Windows.Forms.FormWindowState.Normal;
                    Formsize = F.Size;
                }
                else if (F.WindowState == System.Windows.Forms.FormWindowState.Normal)
                {
                    Rectangle rect = Screen.FromHandle(F.Handle).WorkingArea;
                    rect.Location = new Point(0, 0);
                    F.MaximumSize = rect.Size;
                    F.WindowState = FormWindowState.Maximized; ;
                    Formsize = F.Size;
                }
                F.Refresh();
            }
        }
        private static void FormClose_Click(object sender, EventArgs e)
        {
            F.Close();
        }
    }
}
