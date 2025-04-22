using System;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Renderers;

namespace DarkUI.Controls
{
	// Token: 0x0200003A RID: 58
	public class DarkToolStrip2 : ToolStrip
	{
		// Token: 0x06000223 RID: 547 RVA: 0x0000D0C0 File Offset: 0x0000B2C0
		public DarkToolStrip2()
		{
			base.Renderer = new DarkToolStripRenderer2();
			base.Padding = new Padding(5, 0, 1, 0);
			this.AutoSize = false;
			base.Size = new Size(1, 28);
		}
	}
}
