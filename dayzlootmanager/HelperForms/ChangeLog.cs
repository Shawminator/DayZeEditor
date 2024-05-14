using DarkUI.Forms;
using DayZeLib;

namespace DayZeEditor
{
    public partial class ChangeLog : DarkForm
    {
        public string Changelog
        {
            set
            {
                richTextBox1.Text = value;
            }
        }

        public ChangeLog()
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
    }
}
