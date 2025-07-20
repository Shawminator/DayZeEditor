using System;
using System.Drawing;
using System.Windows.Forms;

namespace DarkUI.Forms
{
	// Token: 0x02000015 RID: 21
	internal partial class DarkTranslucentForm : Form
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00005568 File Offset: 0x00003768
		protected override bool ShowWithoutActivation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000557C File Offset: 0x0000377C
		public DarkTranslucentForm(Color backColor, double opacity = 0.6)
		{
			base.StartPosition = FormStartPosition.Manual;
			base.FormBorderStyle = FormBorderStyle.None;
			base.Size = new Size(1, 1);
			base.ShowInTaskbar = false;
			base.AllowTransparency = true;
			base.Opacity = opacity;
			this.BackColor = backColor;
		}
	}
}
