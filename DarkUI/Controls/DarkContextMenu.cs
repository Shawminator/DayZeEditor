using System;
using System.Windows.Forms;
using DarkUI.Renderers;

namespace DarkUI.Controls
{
	// Token: 0x02000040 RID: 64
	public class DarkContextMenu : ContextMenuStrip
	{
		// Token: 0x060002E5 RID: 741 RVA: 0x000121A0 File Offset: 0x000103A0
		public DarkContextMenu()
		{
			base.Renderer = new DarkMenuRenderer();
		}
	}
}
