using DarkUI.Forms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DayZeLib
{
    public partial class AddItemfromString : DarkForm
    {
        public string TitleLable
        {
            set
            {
                TitleLabel.Text = value;
            }
        }
        public List<string> addedtypes { get; set; }
        public AddItemfromString()
        {
            InitializeComponent();
            Form_Controls_AddfromType.InitializeForm_Controls
            (
                this,
                panel1,
                TitleLabel,
                CloseButton
            );
            addedtypes = new List<string>();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            addedtypes = richTextBox1.Lines.ToList();
        }

        private void darkButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
