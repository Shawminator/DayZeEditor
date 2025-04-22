using System;
using System.ComponentModel;
using DarkUI.Config;

namespace DarkUI.Docking
{
	// Token: 0x0200001F RID: 31
	[ToolboxItem(false)]
	public class DarkTemplate : DarkDockContent
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00008A4C File Offset: 0x00006C4C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new DarkDockArea DefaultDockArea
		{
			get
			{
				return base.DefaultDockArea;
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00008A2C File Offset: 0x00006C2C
		public DarkTemplate()
		{
			this.BackColor = Colors.GreyBackground;
			base.DefaultDockArea = DarkDockArea.Document;
		}
	}
}
