using System;
using System.ComponentModel;
using DarkUI.Config;

namespace DarkUI.Docking
{
	// Token: 0x0200001D RID: 29
	[ToolboxItem(false)]
	public class DarkDocument : DarkDockContent
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00008A14 File Offset: 0x00006C14
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new DarkDockArea DefaultDockArea
		{
			get
			{
				return base.DefaultDockArea;
			}
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00008A2C File Offset: 0x00006C2C
		public DarkDocument()
		{
			this.BackColor = Colors.GreyBackground;
			base.DefaultDockArea = DarkDockArea.Document;
		}
	}
}
