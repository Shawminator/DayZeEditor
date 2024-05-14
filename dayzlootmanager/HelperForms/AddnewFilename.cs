using DarkUI.Forms;
using DayZeLib;
using System;

namespace DayZeEditor
{
    public partial class AddNewfileName : DarkForm
    {
        public string NewFileName { get; private set; }

        public AddNewfileName()
        {
            InitializeComponent();
            Form_Controls_AddfromType.InitializeForm_Controls
            (
                this,
                panel1,
                TitleLabel,
                CloseButton
            );
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            NewFileName = textBox3.Text;
        }

        public string SetTitle
        {
            set { TitleLabel.Text = value; }
        }
        public string setdescription
        {
            set { darkLabel7.Text = value; }
        }
        public string Setbutton
        {
            set { darkButton2.Text = value; }
        }
    }
}
