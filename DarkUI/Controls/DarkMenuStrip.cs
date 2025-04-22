using System;
using System.Windows.Forms;
using DarkUI.Renderers;

namespace DarkUI.Controls
{
	// Token: 0x02000042 RID: 66
	public class DarkMenuStrip : MenuStrip
	{
		// Token: 0x060002EF RID: 751 RVA: 0x0001231E File Offset: 0x0001051E
		public DarkMenuStrip()
		{
			base.Renderer = new DarkMenuRenderer();
			base.Padding = new Padding(3, 2, 0, 2);
		}
	}
}
