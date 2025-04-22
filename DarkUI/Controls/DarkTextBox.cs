using System;
using System.Windows.Forms;
using DarkUI.Config;

namespace DarkUI.Controls
{
	// Token: 0x02000048 RID: 72
	public class DarkTextBox : TextBox
	{
		// Token: 0x0600031E RID: 798 RVA: 0x00013798 File Offset: 0x00011998
		public DarkTextBox()
		{
			this.BackColor = Colors.LightBackground;
			this.ForeColor = Colors.LightText;
			base.Padding = new Padding(2, 2, 2, 2);
			base.BorderStyle = BorderStyle.FixedSingle;
		}
	}
}
