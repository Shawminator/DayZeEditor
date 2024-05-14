using System;
using System.Windows.Forms;

namespace DayZeEditor
{
    public partial class SplashForm : Form
    {
        public delegate void CloseDelagate();
        public SplashForm()
        {
            InitializeComponent();
        }

        private void SplashForm_Load(object sender, EventArgs e)
        {
            this.Refresh();
        }
        public void closeform()
        {
            this.Close();
        }
    }
}
