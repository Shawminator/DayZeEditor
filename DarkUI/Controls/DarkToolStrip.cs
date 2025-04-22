using System;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Renderers;

namespace DarkUI.Controls
{
	// Token: 0x02000049 RID: 73
	public class DarkToolStrip : ToolStrip
	{
		// Token: 0x0600031F RID: 799 RVA: 0x000137D2 File Offset: 0x000119D2
		public DarkToolStrip()
		{
			base.Renderer = new DarkToolStripRenderer();
			base.Padding = new Padding(5, 0, 1, 0);
			this.AutoSize = false;
			base.Size = new Size(1, 28);
		}
	}
}
